namespace StyleVaulAPI.Dto.Collections.Response
{
    public class CollectionsExpensiveBudgetsResponse
    {
        public CollectionsResponse collection { get; set; }
        public string ResponsibleName { get; set; }
        public int QtyOfModels { get; set; }

        public CollectionsExpensiveBudgetsResponse(CollectionsResponse collection, string responsibleName, int qtyOfModels)
        {
            this.collection = collection;
            ResponsibleName = responsibleName;
            QtyOfModels = qtyOfModels;
        }
    }
}