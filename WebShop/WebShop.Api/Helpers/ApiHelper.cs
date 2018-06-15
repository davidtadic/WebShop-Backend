using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using WebShop.Api.Models;

namespace WebShop.Api.Helpers
{
    public class ApiHelper
    {
        public static string MakeRequest(string url, string username, string password, string method, string body)
        {
            try
            {
                var uri = new Uri(url);

                // Create the request
                HttpWebRequest request = (HttpWebRequest)WebRequest.CreateDefault(uri);
                request.Method = method;
                request.KeepAlive = false;
                request.ContentType = "application/json";
                string encodedUsernamePass = System.Convert.ToBase64String(System.Text.Encoding.GetEncoding("ISO-8859-1").GetBytes(username + ":" + password));
                request.Headers.Add(HttpRequestHeader.Authorization, $"Basic {encodedUsernamePass}");

                if(!method.Equals("GET"))
                { 
                // Send the data
                StreamWriter reqStream = new StreamWriter(request.GetRequestStream());
                reqStream.Write(body);
                reqStream.Close();
                }
                // Get the response
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                StreamReader responseStream = new StreamReader(response.GetResponseStream());

                //    var filePath = Microsoft.AspNetCore.Hosting.Server.MapPath("~");
             
                using (var writeStream = File.OpenWrite("~"))
                {
                    response.GetResponseStream().CopyToAsync(writeStream);
                    writeStream.Close();
                }

                string line = string.Empty;
                string responseString = string.Empty;

                while ((line = responseStream.ReadLine()) != null)
                {
                    responseString += line;
                }

                

                responseStream.Close();

                return responseString;
            }
            catch(WebException webEx)
            {
                using (Stream stream = webEx.Response.GetResponseStream())
                using (StreamReader reader = new StreamReader(stream))
                {
                    string exception = reader.ReadToEnd();
                    ExceptionModel exceptionObject = JsonConvert.DeserializeObject<ExceptionModel>(exception);
                    throw new ValidationException(exceptionObject.ErrorMessage);
                }
            }
            catch (Exception ex)
            {
                throw new ValidationException(ex.Message);
            }
        }


        public static string SavePDF(string url, string username, string password, IHostingEnvironment _environment, string OfferId)
        {
            try
            {
                var uri = new Uri(url);

                // Create the request
                HttpWebRequest request = (HttpWebRequest)WebRequest.CreateDefault(uri);
                request.Method = "GET";
                request.KeepAlive = false;
                request.ContentType = "application/pdf";
                string encodedUsernamePass = System.Convert.ToBase64String(System.Text.Encoding.GetEncoding("ISO-8859-1").GetBytes(username + ":" + password));
                request.Headers.Add(HttpRequestHeader.Authorization, $"Basic {encodedUsernamePass}");

             
                // Get the response
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                StreamReader responseStream = new StreamReader(response.GetResponseStream());
                var path = Path.Combine(_environment.ContentRootPath, @"Files\" + OfferId);
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }

                var filePath = Path.Combine(path, "Inforamcija_za_ugovaraca.pdf");
              
                using (var writeStream = new FileStream(filePath, FileMode.Create))//File.OpenWrite("~"))
                {
                    response.GetResponseStream().CopyTo(writeStream);
                    writeStream.Close();
                }


                return "jbggg";
            }
            catch (WebException webEx)
            {
                using (Stream stream = webEx.Response.GetResponseStream())
                using (StreamReader reader = new StreamReader(stream))
                {
                    string exception = reader.ReadToEnd();
                    ExceptionModel exceptionObject = JsonConvert.DeserializeObject<ExceptionModel>(exception);
                    throw new ValidationException(exceptionObject.ErrorMessage);
                }
            }
            catch (Exception ex)
            {
                throw new ValidationException(ex.Message);
            }
        }

    }
}
