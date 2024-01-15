using InsureCo.Domain;
using InsureCo.Helpers;

namespace InsureCoTests
{
    public class ClaimsHelperTests
    {
        [Fact]
        public void GetClaims_WhenDealsIsNull_ThrowsArgumentNullException()
        {
            // Arrange
            List<Deal> deals = null;
            Contract contract = new Contract();
            List<Loss> losses = new List<Loss>();

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => ClaimsHelper.GetClaims(deals, contract, losses));
        }

        [Fact]
        public void GetClaims_WhenContractIsNull_ThrowsArgumentNullException()
        {
            // Arrange
            List<Deal> deals = new List<Deal>();
            Contract contract = null;
            List<Loss> losses = new List<Loss>();

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => ClaimsHelper.GetClaims(deals, contract, losses));
        }

        [Fact]
        public void GetClaims_WhenLossesIsNull_ThrowsArgumentNullException()
        {
            // Arrange
            List<Deal> deals = new List<Deal>();
            Contract contract = new Contract();
            List<Loss> losses = null;

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => ClaimsHelper.GetClaims(deals, contract, losses));
        }


        [Fact]
        public void GetClaims_WhenCoverageRulesMatch_ReturnsExpectedClaims()
        {
            // Arrange
            List<Deal> deals = new List<Deal>
            {
            new Deal { DealId = 1, Location="USA", Peril = "Wind" },
            new Deal { DealId = 2, Location="Brazil", Peril = "Flood" },
            new Deal { DealId = 3, Location="France", Peril = "Dry" },
        };

            Contract contract = new Contract {
                MaxAmount = 200,
                Coverage = new List<CoverageRule>
                {
                new CoverageRule { Attribute = "Location", Include = new List<string> { "USA", "France" } },
                new CoverageRule { Attribute = "Peril", Exclude = new List<string> { "Flood" } }
            }
            };

            List<Loss> losses = new List<Loss>
            {
            new Loss { DealId = 1, LossAmmount = 100 },
            new Loss { DealId = 2, LossAmmount = 150 },
            new Loss { DealId = 3, LossAmmount = 50 },
        };

            // Act
            var result = ClaimsHelper.GetClaims(deals, contract, losses).ToList();

            // Assert
            Assert.Equal(2, result.Count);
            Assert.Contains(result, claim => claim.Peril == "Wind" && claim.Loss == 100);
            Assert.Contains(result, claim => claim.Peril == "Dry" && claim.Loss == 50);
        }
    }
}
