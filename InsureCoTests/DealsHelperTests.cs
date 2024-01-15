using InsureCo.Domain;
using InsureCo.Helpers;

namespace InsureCoTests
{
    public class DealsHelperTests
    {
        [Fact]
        public void GetCoveredDeals_WhenDealsIsNull_ThrowsArgumentNullException()
        {
            // Arrange
            List<Deal> deals = null;
            Contract contract = new Contract();

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => DealsHelper.GetCoveredDeals(deals, contract));
        }

        [Fact]
        public void GetCoveredDeals_WhenContractIsNull_ThrowsArgumentNullException()
        {
            // Arrange
            List<Deal> deals = new List<Deal>();
            Contract contract = null;

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => DealsHelper.GetCoveredDeals(deals, contract));
        }


        [Fact]
        public void GetCoveredDeals_WhenCoverageRulesMatch_ReturnsMatchingDeals()
        {
            // Arrange
            List<Deal> deals = new List<Deal>
            {
            new Deal { Location = "USA", Peril = "Fire" },
            new Deal { Location = "Brazil", Peril = "Flood" },
            new Deal { Location = "France", Peril = "Cold" },
        };

            Contract contract = new Contract {
                Coverage = new List<CoverageRule>
                {
                new CoverageRule { Attribute = "Location", Include = new List<string> { "USA", "Brazil" } },
                new CoverageRule { Attribute = "Peril", Exclude = new List<string> { "Cold" } }
            }
            };

            // Act
            var result = DealsHelper.GetCoveredDeals(deals, contract).ToList();

            // Assert
            Assert.Equal(2, result.Count);
            Assert.Contains(result, d => d.Location == "USA");
            Assert.Contains(result, d => d.Location == "Brazil");
        }
    }
}