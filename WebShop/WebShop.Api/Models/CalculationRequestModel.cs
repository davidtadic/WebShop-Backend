using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebShop.Api.Enums;

namespace WebShop.Api.Models
{
    public class CalculationRequestModel
    {
        public Tariff Tariff { get; set; }
    }

    public class Tariff
    {
        public DateTime InsuranceBeginDate { get; set; }
        public Nullable<DateTime> InsuranceEndDate { get; set; }
        public bool FullYear { get; set; }
        public InsuranceCoverage InsuranceCoverage { get; set; }
        public ProductVariant ProductVariant { get; set; }
        public int AmountInsured { get; set; }
        public int TravelReason { get; set; }
        public bool CancellationInsurance { get; set; }
        public Nullable<DateTime> BookingDate { get; set; }

    }
}
