using InsureCo.Domain;

namespace InsureCo.Helpers
{
    public static class DealsHelper
    {
        public static IEnumerable<Deal> GetCoveredDeals(List<Deal> deals, Contract contract)
        {
            if (deals == null || !deals.Any()) throw new ArgumentNullException(nameof(deals), "Deals not provided");
            if (contract == null || !contract.Coverage.Any()) throw new ArgumentNullException(nameof(contract), "Contract coverage not provided");

            IQueryable<Deal> query = deals.AsQueryable();
            foreach(var rule in contract.Coverage) {

                switch (rule.Attribute) {
                    case "Location":
                        query = ApplyLocationRule(query, rule);
                        break;

                    case "Peril":
                        query = ApplyPerilRule(query, rule);
                        break;

                    default:
                        break;
                }
            }
            return query.AsEnumerable();
        }

        private static IQueryable<Deal> ApplyLocationRule(IQueryable<Deal> query, CoverageRule rule)
        {
            return query.Where(x => rule.Include != null && rule.Include.Contains(x.Location));
        }

        private static IQueryable<Deal> ApplyPerilRule(IQueryable<Deal> query, CoverageRule rule)
        {
            return query.Where(x => rule.Exclude != null && !rule.Exclude.Contains(x.Peril));
        }
    }
}
