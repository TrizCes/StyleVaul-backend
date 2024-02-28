using AutoMapper;
using StyleVaulAPI.Dto.Models.Request;
using StyleVaulAPI.Dto.Models.Response;
using StyleVaulAPI.Exceptions;
using StyleVaulAPI.Interfaces.Repositories;
using StyleVaulAPI.Interfaces.Services;
using StyleVaulAPI.Models;

namespace StyleVaulAPI.Services
{
    public class ModelsService : IModelsService
    {
        private readonly IModelsRepository _repository;
        private readonly IMapper _mapper;

        public ModelsService(
            IModelsRepository repository,
            IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<int> CreateAsync(PostModels dto, int companyId)
        {
            if (string.IsNullOrWhiteSpace(dto.Name))
                throw new BadRequestException("Nome do modelo não pode estar vazio");

            if (await _repository.CheckModelNameAsync(companyId, dto.Name))
                throw new BadRequestException("Nome do modelo já existe, ele deve ser único!");

            if (!await _repository.CheckCollectionAsync(companyId, dto.CollectionId))
                throw new BadRequestException("Coleção inválida para criar este modelo");

            if (!await _repository.CheckResponsibleAsync(companyId, dto.ResponsibleId))
                throw new BadRequestException("Responsável inválido para criar este modelo!");

            var model = _mapper.Map<Model>(dto);
            model.CompanyId = companyId;
            await _repository.CreateAsync(model);
            return model.Id;
        }

        public async Task<bool> DeleteAsync(int id, int companyId)
        {
            var model = await _repository.GetByIdAsync(id, companyId);
            if (model == null)
                throw new BadRequestException("Modelo inválido para deletar");

            return await _repository.DeleteAsync(id, companyId);
        }

        public async Task<List<ModelResponse>> GetAllAsync(int companyId)
        {
            return _mapper.Map<List<ModelResponse>>(await _repository.GetAllAsync(companyId));
        }

        public async Task<Model> GetByIdAsync(int id, int companyId)
        {
            var model = await _repository.GetByIdAsync(id, companyId);

            if (model == null)
                throw new NotFoundException("Modelo inválido para esta empresa");

            return model;
        }

        public async Task<bool> UpdateAsync(PutModels modelDto, int id, int companyId)
        {
            if (string.IsNullOrWhiteSpace(modelDto.Name))
                throw new BadRequestException("Nome do modelo não pode estar vazio");

            var model = await _repository.GetByIdAsync(id, companyId);

            if (model == null)
                throw new BadRequestException("Modelo não encontrado.");

            if (model.Name != modelDto.Name
                && await _repository.CheckModelNameAsync(companyId, modelDto.Name))
                throw new BadRequestException("Nome de modelo já cadastrado.");

            if (!await _repository.CheckCollectionAsync(companyId, modelDto.CollectionId))
                throw new BadRequestException("Coleção inválida para criar este modelo");

            if (await _repository.CheckModelCompanyAsync(id, companyId))
                throw new BadRequestException("Modelo inválido para esta empresa");

            if (modelDto.ResponsibleId > 0
               && modelDto.ResponsibleId != model.ResponsibleId)
                model.ResponsibleId = modelDto.ResponsibleId;

            if (modelDto.CollectionId > 0
                && modelDto.CollectionId != model.CollectionId)
                model.CollectionId = modelDto.CollectionId;

            if (!string.IsNullOrWhiteSpace(modelDto.Name)
               && modelDto.Name != model.Name)
                model.Name = modelDto.Name;

            if (model.RealCost > 0
               && Math.Abs(modelDto.RealCost - model.RealCost) > 0.0)
                model.RealCost = modelDto.RealCost;

            if (modelDto.Type != model.Type)
                model.Type = modelDto.Type;

            if (modelDto.Embroidery != model.Embroidery)
                model.Embroidery = modelDto.Embroidery;

            if (modelDto.Print != model.Print)
                model.Print = modelDto.Print;

            return await _repository.UpdateAsync(model);
        }
    }
}
