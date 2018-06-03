using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebShop.Api.Enums;

namespace WebShop.Api.Models
{
    public class CalculationResponseModel
    {
        public ProductVariant ProductVariant { get; set; }
        public int AmountInsured { get; set; }
        public decimal PremiumEur { get; set; }
        public decimal PremiumRsd { get; set; }
    }
}
