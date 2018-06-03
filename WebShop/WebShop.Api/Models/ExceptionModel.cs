using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebShop.Api.Models
{
    public class ExceptionModel
    {
        public int ErrorCode { get; set; }
        public string ErrorMessage { get; set; }
    }
}
