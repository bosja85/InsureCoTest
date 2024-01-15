using CsvHelper;
using InsureCo.Domain;
using Newtonsoft.Json;
using System.Globalization;

namespace InsureCo.DataLoader
{
    public static class DataLoader
    {
        public static List<Deal> LoadDeals(string filePath)
        {
            try 
            {
                using (var reader = new StreamReader(filePath))
                using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture)) {
                    return csv.GetRecords<Deal>().ToList();
            }
            } catch (Exception ex) {
                throw new Exception($"Error loading deals from CSV: {ex.Message}", ex);
            }
        }

        public static List<Loss> LoadLosses(string filePath)
        {
            try {
                using (var reader = new StreamReader(filePath))
                using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture)) {

                    csv.Context.RegisterClassMap<LossMap>();
                    return csv.GetRecords<Loss>().ToList();
                }
            } catch (Exception ex) {
                throw new Exception($"Error loading losses from CSV: {ex.Message}", ex);
            }
        }

        public static Contract LoadContract(string filePath)
        {
            try {
                return JsonConvert.DeserializeObject<Contract>(File.ReadAllText(filePath));
            }
            catch (Exception ex) {
                throw new Exception($"Error loading losses from CSV: {ex.Message}", ex);
    }
}

    }
}
