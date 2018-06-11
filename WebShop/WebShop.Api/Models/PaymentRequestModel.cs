using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebShop.Api.Models
{
    public class PaymentRequestModel
    {
        public PolicyRequestModel PolicyRequest { get; set; }
        public OfferResponseModel OfferResponse { get; set; }
    }

    public class PolicyRequestModel
    {
        public string OfferId { get; set; }
        public int Vkto { get; set; }
        public bool ConditionsAccepted { get; set; }
        public bool PreContractInfoAccepted { get; set; }
        public bool Newsletter { get; set; }
    }
}
