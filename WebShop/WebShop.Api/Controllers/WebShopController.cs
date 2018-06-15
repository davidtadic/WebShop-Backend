using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using WebShop.Api.Helpers;
using WebShop.Api.Models;

namespace WebShop.Api.Controllers
{
    [Route("api/[controller]")]
    public class WebShopController : Controller
    {
        private readonly ApiConfiguration _apiConfiguration;
        private readonly IHostingEnvironment _environment;

        public WebShopController(IOptions<ApiConfiguration> apiConfiguration, IHostingEnvironment environment)
        {
            _apiConfiguration = apiConfiguration.Value;
            _environment = environment;
        }

        [HttpPost("CalculatePremiums")]
        [Produces("application/json")]
        public List<CalculationResponseModel> CalculatePremiums([FromBody]CalculationRequestModel model)
        {
            model.Tariff.InsuranceEndDate = model.Tariff.FullYear ? null : model.Tariff.InsuranceEndDate;
            List<CalculationRequestModel> requestModels = PrepareDataForCalculation(model);
            List<CalculationResponseModel> result = new List<CalculationResponseModel>();

            foreach (CalculationRequestModel requestModel in requestModels)
            {
                string requestString = JsonConvert.SerializeObject(requestModel);
                string responseString = ApiHelper.MakeRequest(_apiConfiguration.Url + "travel/calculation", _apiConfiguration.Username, _apiConfiguration.Password, "POST", requestString);
                CalculationResponseModel response = JsonConvert.DeserializeObject<CalculationResponseModel>(responseString);
                response.ProductVariant = requestModel.Tariff.ProductVariant;
                response.AmountInsured = requestModel.Tariff.AmountInsured;

                result.Add(response);
            }


            return result;
        }

        [HttpGet("InfoUgovaraca")]
        public string Get()
        {
            string OfferId = "50093972-c42d-4f64-bfcd-f0ef58779bcc";
            string responseString = ApiHelper.SavePDF(_apiConfiguration.Url + OfferId + "/files/2", _apiConfiguration.Username, _apiConfiguration.Password,  _environment, OfferId);
            return responseString;
        }

        [HttpPost("CalculateTravelStarPremium")]
        [Produces("application/json")]
        public CalculationResponseModel CalculateTravelStarPremium([FromBody]CalculationRequestModel model)
        {
            CalculationResponseModel result = new CalculationResponseModel();
            model.Tariff.InsuranceEndDate = model.Tariff.FullYear ? null : model.Tariff.InsuranceEndDate;

            string requestString = JsonConvert.SerializeObject(model);
            string responseString = ApiHelper.MakeRequest(_apiConfiguration.Url + "travel/calculation", _apiConfiguration.Username, _apiConfiguration.Password, "POST", requestString);
            result = JsonConvert.DeserializeObject<CalculationResponseModel>(responseString);
            result.ProductVariant = model.Tariff.ProductVariant;
            result.AmountInsured = model.Tariff.AmountInsured;

            return result;
        }

        [HttpPost("OfferRequest")]
        [Produces("application/json")]
        public OfferResponseModel OfferRequest([FromBody]OfferRequestModel model)
        {
            model.Tariff.InsuranceEndDate = model.Tariff.FullYear ? null : model.Tariff.InsuranceEndDate;
            OfferResponseModel result = new OfferResponseModel();

            string requestString = JsonConvert.SerializeObject(model);
            string responseString = ApiHelper.MakeRequest(_apiConfiguration.Url + "travel/offer", _apiConfiguration.Username, _apiConfiguration.Password, "POST", requestString);
            result = JsonConvert.DeserializeObject<OfferResponseModel>(responseString);

            return result;
        }

        [NonAction]
        private List<CalculationRequestModel> PrepareDataForCalculation(CalculationRequestModel model)
        {
            List<CalculationRequestModel> result = new List<CalculationRequestModel>();

            if (model.Tariff.InsuranceCoverage == Enums.InsuranceCoverage.Individual)
            {
                CalculationRequestModel travelRequestFirst = new CalculationRequestModel()
                {
                    Tariff = new Tariff()
                    {
                        InsuranceBeginDate = model.Tariff.InsuranceBeginDate,
                        InsuranceEndDate = model.Tariff.InsuranceEndDate,
                        AmountInsured = 12000,
                        FullYear = model.Tariff.FullYear,
                        InsuranceCoverage = Enums.InsuranceCoverage.Individual,
                        TravelReason = model.Tariff.TravelReason,
                        ProductVariant = Enums.ProductVariant.Travel,
                        CancellationInsurance = false,
                        BookingDate = null
                    }
                };

                CalculationRequestModel travelRequestSecond = new CalculationRequestModel()
                {
                    Tariff = new Tariff()
                    {
                        InsuranceBeginDate = model.Tariff.InsuranceBeginDate,
                        InsuranceEndDate = model.Tariff.InsuranceEndDate,
                        AmountInsured = 32000,
                        FullYear = model.Tariff.FullYear,
                        InsuranceCoverage = Enums.InsuranceCoverage.Individual,
                        TravelReason = model.Tariff.TravelReason,
                        ProductVariant = Enums.ProductVariant.Travel,
                        CancellationInsurance = false,
                        BookingDate = null
                    }
                };

                CalculationRequestModel travelStarRequest = new CalculationRequestModel()
                {
                    Tariff = new Tariff()
                    {
                        InsuranceBeginDate = model.Tariff.InsuranceBeginDate,
                        InsuranceEndDate = model.Tariff.InsuranceEndDate,
                        AmountInsured = 120000,
                        FullYear = model.Tariff.FullYear,
                        InsuranceCoverage = Enums.InsuranceCoverage.Individual,
                        TravelReason = model.Tariff.TravelReason,
                        ProductVariant = Enums.ProductVariant.TravelStar,
                        CancellationInsurance = false,
                        BookingDate = null
                    }
                };

                result.Add(travelRequestFirst);
                result.Add(travelRequestSecond);
                result.Add(travelStarRequest);
            }
            else
            {
                CalculationRequestModel travelRequestFirst = new CalculationRequestModel()
                {
                    Tariff = new Tariff()
                    {
                        InsuranceBeginDate = model.Tariff.InsuranceBeginDate,
                        InsuranceEndDate = model.Tariff.InsuranceEndDate,
                        AmountInsured = 24000,
                        FullYear = model.Tariff.FullYear,
                        InsuranceCoverage = Enums.InsuranceCoverage.Family,
                        TravelReason = model.Tariff.TravelReason,
                        ProductVariant = Enums.ProductVariant.Travel,
                        CancellationInsurance = false,
                        BookingDate = null
                    }
                };

                CalculationRequestModel travelRequestSecond = new CalculationRequestModel()
                {
                    Tariff = new Tariff()
                    {
                        InsuranceBeginDate = model.Tariff.InsuranceBeginDate,
                        InsuranceEndDate = model.Tariff.InsuranceEndDate,
                        AmountInsured = 62000,
                        FullYear = model.Tariff.FullYear,
                        InsuranceCoverage = Enums.InsuranceCoverage.Family,
                        TravelReason = model.Tariff.TravelReason,
                        ProductVariant = Enums.ProductVariant.Travel,
                        CancellationInsurance = false,
                        BookingDate = null
                    }
                };

                CalculationRequestModel travelStarRequest = new CalculationRequestModel()
                {
                    Tariff = new Tariff()
                    {
                        InsuranceBeginDate = model.Tariff.InsuranceBeginDate,
                        InsuranceEndDate = model.Tariff.InsuranceEndDate,
                        AmountInsured = 120000,
                        FullYear = model.Tariff.FullYear,
                        InsuranceCoverage = Enums.InsuranceCoverage.Family,
                        TravelReason = model.Tariff.TravelReason,
                        ProductVariant = Enums.ProductVariant.TravelStar,
                        CancellationInsurance = false,
                        BookingDate = null
                    }
                };

                result.Add(travelRequestFirst);
                result.Add(travelRequestSecond);
                result.Add(travelStarRequest);
            }


            return result;
        }

    }
}
