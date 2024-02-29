using AutoMapper;
using StyleVaulAPI.Dto.Collections.Request;
using StyleVaulAPI.Dto.Collections.Response;
using StyleVaulAPI.Exceptions;
using StyleVaulAPI.Interfaces.Repositories;
using StyleVaulAPI.Interfaces.Services;
using StyleVaulAPI.Models;
using StyleVaulAPI.Models.Enums;

namespace StyleVaulAPI.Services
{
    public class CollectionsService : ICollectionsService
    {
        private readonly ICollectionsRepository _repository;
        private readonly IUsersRepository _userRepository;
        private readonly IMapper _mapper;
        private readonly IModelsRepository _modelRepository;

        public CollectionsService(
            ICollectionsRepository repository,
            IUsersRepository userRepository,
            IMapper mapper,
            IModelsRepository modelRepository)
        {
            _repository = repository;
            _userRepository = userRepository;
            _mapper = mapper;
            _modelRepository = modelRepository;
        }

        public async Task<int> CreateAsync(PostCollectionsDto dto, int companyId)
        {
            if (string.IsNullOrWhiteSpace(dto.Name))
                throw new BadRequestException("Nome da coleção não pode estar vazio");

            if (!await _userRepository.CheckUserExistsAsync(dto.ResponsibleId, companyId))
                throw new BadRequestException("Responsável inválido.");

            if (await _repository.CheckNameAsync(0, dto.Name, companyId))
                throw new BadRequestException("Nome da coleção já existe, ela deve ser única por empresa!");

            if (string.IsNullOrWhiteSpace(dto.Brand))
                throw new BadRequestException("Marca da coleção não pode estar vazio");

            if (dto.Budget <= 0)
                throw new BadRequestException("Orçamento da coleção não pode ser menor ou igual a zero");

            if (dto.ReleaseYear.Year < DateTime.UtcNow.Year)
                throw new BadRequestException("Ano de lançamento da coleção não pode ser menor que o ano atual");

            if (string.IsNullOrWhiteSpace(dto.Collors))
                throw new BadRequestException("Cores da coleção não pode estar vazio");

            if (!Enum.IsDefined(typeof(SeasonEnum), dto.Season))
                throw new BadRequestException("Estação da coleção não pode estar vazio");

            if (!Enum.IsDefined(typeof(StatusEnum), dto.Status))
                throw new BadRequestException("Status da coleção não pode estar vazio");

            var collection = _mapper.Map<Collection>(dto);
            collection.CompanyId = companyId;
            collection.InclusionAt = DateTime.Now;
            await _repository.CreateAsync(collection);
            return collection.Id;
        }

        public async Task<bool> DeleteAsync(int id, int companyId)
        {
            if (!await _repository.CheckCollectionExistsAsync(id, companyId))
                throw new NotFoundException("Coleção não encontrada.");

            if (await _modelRepository.CheckModelExistsByCollectionAsync(id))
                throw new BadRequestException("Não é possível excluir uma coleção que possui modelos cadastrados.");
            return await _repository.DeleteAsync(id);
        }

        public async Task<List<CollectionsResponse>> GetAllAsync(int companyId)
        {
            return _mapper.Map<List<CollectionsResponse>>(await _repository.GetAllAsync(companyId));
        }

        public async Task<List<CollectionsResponse>> GetAllValidAsync(int companyId)
        {
            return _mapper.Map<List<CollectionsResponse>>(await _repository.GetAllValidAsync(companyId));
        }

        public async Task<CollectionsResponse> GetByIdAsync(int id, int companyId)
        {
            var collection = await _repository.GetByIdAsync(id, companyId);

            if (collection == null)
                throw new NotFoundException("Coleção não encontrada.");

            var response = _mapper.Map<CollectionsResponse>(collection);
            return response;
        }

        public async Task<PutCollectionsDto> UpdateAsync(PutCollectionsDto dto, int companyId)
        {
            if (string.IsNullOrWhiteSpace(dto.Name))
                throw new BadRequestException("Nome da coleção não pode estar vazio");

            var collection = await _repository.GetByIdAsync(dto.Id, companyId);
            if (collection == null)
                throw new BadRequestException("Erro em obter coleção");

            if (collection.CompanyId != companyId)
                throw new BadRequestException("Coleção não pertence a empresa informada.");

            if (await _repository.CheckNameAsync(dto.Id, dto.Name, collection.CompanyId))
                throw new BadRequestException("Nome da coleção já existe, ela deve ser única por empresa!");

            if (string.IsNullOrWhiteSpace(dto.Brand))
                throw new BadRequestException("Marca da coleção não pode estar vazio");

            if (dto.Budget <= 0)
                throw new BadRequestException("Orçamento da coleção não pode ser menor ou igual a zero");

            if (dto.ReleaseYear.Year < DateTime.UtcNow.Year)
                throw new BadRequestException("Ano de lançamento da coleção não pode ser menor que o ano atual");

            if (string.IsNullOrWhiteSpace(dto.Collors))
                throw new BadRequestException("Cores da coleção não pode estar vazio");

            if (!Enum.IsDefined(typeof(SeasonEnum), dto.Season))
                throw new BadRequestException("Estação da coleção não pode estar vazio");

            if (!Enum.IsDefined(typeof(StatusEnum), dto.Status))
                throw new BadRequestException("Status da coleção não pode estar vazio");


            collection.ResponsibleId = dto.ResponsibleId;
            collection.Name = dto.Name;
            collection.Brand = dto.Brand;
            collection.Budget = dto.Budget;
            collection.ReleaseYear = dto.ReleaseYear;
            collection.Collors = dto.Collors;
            collection.Status = dto.Status;
            collection.Season = dto.Season;

            await _repository.UpdateAsync(collection);
            return dto;
        }

        public async Task<List<CollectionsExpensiveBudgetsResponse>> GetExpensiveBudgetsAsync(int companyId, int qty)
        {
            var expensiveCollections = await _repository.GetExpensiveBudgetsAsync(companyId, qty);
            var response = new List<CollectionsExpensiveBudgetsResponse>();

            foreach (var c in expensiveCollections)
            {
                var collection = _mapper.Map<CollectionsResponse>(c);
                var modelsQty = await _modelRepository.GetAllByCollectionIdAsync(c.Id);
                var responsible = await _userRepository.GetByIdAsync(c.ResponsibleId);
                var res = new CollectionsExpensiveBudgetsResponse(collection, responsible.Name, modelsQty.Count());

                response.Add(res);
            }

            if (response.Count == 0)
                throw new BadRequestException("Não foi possível encontrar a lista de maiores orçamentos.");

            return response;
        }

        public async Task<List<CollectionsBudgetsVsCosts>> GetBudgetsVsCosts(int companyId)
        {
            var collections = await _repository.GetAllAsync(companyId);
            var models = await _modelRepository.GetAllForCostsAsync(companyId);
            List<CollectionsBudgetsVsCosts> response = new List<CollectionsBudgetsVsCosts>();

            for (int monthValue = 1; monthValue <= 12; monthValue++)
            {
                MonthEnum currentMonth = (MonthEnum)monthValue;
                double cost = CostsByMonth(models, monthValue);
                decimal budget = BudgetsByMonth(collections, monthValue);
                CollectionsBudgetsVsCosts budgetVsCosts = new CollectionsBudgetsVsCosts(currentMonth, cost, budget);
                response.Add(budgetVsCosts);
            }

            return response;

        }

        private decimal BudgetsByMonth(List<Collection> collections, int month)
        {
            decimal total = 0;
            foreach (var collection in collections)
            {
                if (collection.InclusionAt.Month == month)
                {
                    total += collection.Budget;
                }
            }

            return total;
        }

        private double CostsByMonth(List<Model> models, int month)
        {
            double total = 0;
            foreach (var model in models)
            {
                if (model.InclusionAt.Month == month)
                {
                    total += model.RealCost;
                }
            }

            return total;
        }
    }
}
