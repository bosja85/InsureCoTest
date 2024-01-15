using CsvHelper.Configuration;
using InsureCo.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InsureCo.DataLoader
{
    public sealed class LossMap : ClassMap<Loss>
    {
        public LossMap()
        {
            Map(l => l.EventId);
            Map(l => l.DealId);
            Map(l => l.LossAmmount).Name("Loss");
        }
    }
}
