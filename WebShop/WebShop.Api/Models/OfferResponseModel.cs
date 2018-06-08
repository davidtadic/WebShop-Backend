using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebShop.Api.Models
{
    public class OfferResponseModel
    {
        public string OfferId { get; set; }
        public decimal PremiumEur { get; set; }
        public decimal PremiumRsd { get; set; }
        public DateTime InsuranceBeginDate { get; set; }
        public DateTime InsuranceEndDate { get; set; }
        public int AmountInsured { get; set; }
        public bool CancellationInsurance { get; set; }
        public DateTime BookingDate { get; set; }
    }
}
