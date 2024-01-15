using InsureCo.DataLoader;
using InsureCo.Domain;
using InsureCo.Helpers;
using Microsoft.Extensions.Configuration;
using System.Diagnostics;

try 
{
    var configuration = new ConfigurationBuilder()
        .AddJsonFile($"appsettings.json").Build();

    var dealsFilePath = configuration["DataFilePaths:Deals"];
    var lossesFilePath = configuration["DataFilePaths:Losses"];
    var contractsFilePath = configuration["DataFilePaths:Contracts"];

    var deals = DataLoader.LoadDeals(dealsFilePath);
    var losses = DataLoader.LoadLosses(lossesFilePath);
    var contract = DataLoader.LoadContract(contractsFilePath);

    await_input:

    Console.WriteLine("\n");
    Console.WriteLine("Enter d for covered deals, r for reinsurrance and e for exit program");
    string action  = Console.ReadLine();

    switch (action) {
        case "d": {
                var coveredDeals = DealsHelper.GetCoveredDeals(deals, contract);
                Console.WriteLine("Deal Id \t Company \t\t Peril \t\t Location");
                Console.WriteLine("-------------------------------------------------------------");

                foreach (var deal in coveredDeals) {
                    Console.WriteLine($"{deal.DealId} \t\t{deal.Company} \t\t{deal.Peril} \t\t{deal.Location}");
                }

                goto await_input;
            }
        case "r": {
                IEnumerable<Claim> claims = ClaimsHelper.GetClaims(deals, contract, losses);
                Console.WriteLine("Peril \t Loss");
                Console.WriteLine("-------------------------------------------------------------");
                foreach (var claim in claims) {
                    Console.WriteLine($"{claim.Peril} \t {claim.Loss}");
                }
                goto await_input;
            }
        case "e": {
                break;
            }
        default:
            goto await_input;
    }  
} catch (Exception e) {
    var st = new StackTrace(e, true);
    var frame = st.GetFrame(0);
    var line = frame.GetFileLineNumber();
    Console.WriteLine($"ERROR occured on line : {line}\n");
    Console.WriteLine(e.Message);
}