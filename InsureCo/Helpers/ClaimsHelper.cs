using InsureCo.Domain;

namespace InsureCo.Helpers
{
    public class ClaimsHelper
    {
        public static IEnumerable<Claim> GetClaims(List<Deal> deals, Contract contract, List<Loss> losses)
        {
            if (deals == null || !deals.Any()) throw new ArgumentNullException(nameof(deals), "Deals not provided");
            if (contract == null || !contract.Coverage.Any()) throw new ArgumentNullException(nameof(contract), "Contract coverage not provided");
            if (losses == null || !losses.Any()) throw new ArgumentNullException(nameof(losses), "Losses not provided");

            var coveredDealIds = DealsHelper.GetCoveredDeals(deals, contract)
                                    .Select(coveredDeal => coveredDeal.DealId)
                                    .ToHashSet();

            var claims = losses
                .Join(coveredDealIds, loss => loss.DealId, coveredDealId => coveredDealId, (loss, _) => new {
                    Loss = loss,
                    CoveredDeal = deals.Single(deal => deal.DealId == loss.DealId)
                })
                .Select(result => new Claim(
                    Peril: result.CoveredDeal.Peril,
                    Loss: Math.Min(result.Loss.LossAmmount, contract.MaxAmount)
                ))
                .GroupBy(claim => claim.Peril)
                .Select(group => new Claim(group.Key, group.Sum(claim => claim.Loss)));

            return claims;
        }
    }
}
