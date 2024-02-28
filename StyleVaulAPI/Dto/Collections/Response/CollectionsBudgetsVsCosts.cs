using StyleVaulAPI.Models.Enums;

namespace StyleVaulAPI.Dto.Collections.Response
{
    public class CollectionsBudgetsVsCosts
    {
        public MonthEnum Month { get; set; }
        public decimal Costs { get; set; }
        public decimal Budgets { get; set; }

        public CollectionsBudgetsVsCosts(MonthEnum month, double costs, decimal budget)
        {
            Month = month;
            Costs = (decimal)costs;
            Budgets = budget;
        }
    }
}
