using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebShop.Api.Models
{
    public class OfferRequestModel
    {
        public Tariff Tariff { get; set; }
        public Customer Customer { get; set; }
        public List<InsuredPerson> InsuredPersons { get; set; }
    }

    public class Customer
    {
        public int SalutatoryAddress { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string JMBG { get; set; }
        public string EMailAddress { get; set; }
        public string PhoneNumber { get; set; }
        public Address Address { get; set; }
    }

    public class InsuredPerson
    {
        public int SalutatoryAddress { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string JMBG { get; set; }
        public string PassportNumber { get; set; }
    }

    public class Address
    {
        public string Street { get; set; }
        public string HouseNumber { get; set; }
        public string HouseNumberExtension { get; set; }
        public string ZipCode { get; set; }
        public string Town { get; set; }
        public string Nation { get; set; }
    }

}
