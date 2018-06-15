using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.OpenSsl;
using Org.BouncyCastle.Security;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace WebShop.Api.Helpers
{
    public class PaymentHelper
    {
        private static readonly string _merchantId = "1755074";
        private static readonly string _terminalId = "E7883084";
        private static readonly string _currencyId = "941";
        private static readonly string _gatewayAddress = "https://ecg.test.upc.ua/rbrs/enter";
        //private static readonly string _pemFilePath = $@"C:\Users\david\Documents\{_merchantId}.pem";
        private static readonly string _locale = "rs";

        public static string GenerateForm(int orderId, int amount)
        {
            string paymentForm = string.Empty;
            string pemFilePath = System.IO.Path.GetFullPath($@"AppData\{_merchantId}.pem");
            string purchaseTime = DateTime.UtcNow.ToString("yyMMddHHmmss");
            string signature = string.Empty;

            string data = $"{_merchantId};{_terminalId};{purchaseTime};{orderId};{_currencyId};{amount};;";

            AsymmetricCipherKeyPair keyPair;
            using (StreamReader sr = new StreamReader(pemFilePath))
            {
                PemReader pr = new PemReader(sr);
                keyPair = (AsymmetricCipherKeyPair)pr.ReadObject();
            }

            RSAParameters rsaParams = DotNetUtilities.ToRSAParameters((RsaPrivateCrtKeyParameters)keyPair.Private);

            //byte[] originalData = Encoding.UTF8.GetBytes(data);
            //byte[] signedData;
            //signedData = HashAndSignBytes(originalData, rsaParams);
            //signature = Convert.ToBase64String(signedData);

            signature = SignData(data, rsaParams);

            //using (RSACryptoServiceProvider rsa = new RSACryptoServiceProvider())
            //{
            //    rsa.ImportParameters(rsaParams);
            //    byte[] signatureBytes = rsa.SignData(Encoding.UTF8.GetBytes(data), CryptoConfig.MapNameToOID("SHA256"));
            //    signature = Convert.ToBase64String(signatureBytes);
            //}

            paymentForm += "<!DOCTYPE html PUBLIC '-//W3C//DTD XHTML 1.0 Transitional//EN' 'http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd'>\n";
            paymentForm += "<html xmlns='http://www.w3.org/1999/xhtml'>\n";
            paymentForm += "<head>\n";
            paymentForm += "<meta http-equiv='Content-Type' content='text/html; charset=utf-8'/>\n";
            paymentForm += "</head>\n";
            paymentForm += "<body>\n";
            paymentForm += $"<form action='{_gatewayAddress}' method='post'>\n";
            paymentForm += "<input name='Version' type='hidden' value='1'/>\n";
            paymentForm += $"<input name='MerchantID' type='hidden' value='{_merchantId}'/>\n";
            paymentForm += $"<input name='TerminalID' type='hidden' value='{_terminalId}'/>\n";
            paymentForm += $"<input name='TotalAmount' type='hidden' value='{amount}'/>\n";
            paymentForm += $"<input name='Currency' type='hidden' value='{_currencyId}'/>\n";
            paymentForm += $"<input name='locale' type='hidden' value='{_locale}'/>\n";
            paymentForm += $"<input name='PurchaseTime' type='hidden' value='{purchaseTime}'/>\n";
            paymentForm += $"<input name='OrderID' type='hidden' value='{orderId}'/>\n";
            paymentForm += $"<input name='Signature' type='hidden' value='{signature}'/>\n";
            paymentForm += $"<input type='submit'>\n";
            paymentForm += "</form>\n";
            paymentForm += "</body>\n";
            paymentForm += "</html>\n";

            return paymentForm;
        }

        private static string SignData(string message, RSAParameters privateKey)
        {
            // The array to store the signed message in bytes
            byte[] signedBytes;
            using (var rsa = new RSACryptoServiceProvider())
            {
                // Write the message to a byte array using UTF8 as the encoding.
                var encoder = new UTF8Encoding();
                byte[] originalData = encoder.GetBytes(message);

                try
                {
                    // Import the private key used for signing the message
                    rsa.ImportParameters(privateKey);

                    // Sign the data, using SHA512 as the hashing algorithm 
                    signedBytes = rsa.SignData(originalData, new SHA1CryptoServiceProvider());
                }
                catch (CryptographicException e)
                {
                    Console.WriteLine(e.Message);
                    return null;
                }
                finally
                {
                    // Set the keycontainer to be cleared when rsa is garbage collected.
                    rsa.PersistKeyInCsp = false;
                }
            }
            // Convert the a base64 string before returning
            return Convert.ToBase64String(signedBytes);
        }

        public static byte[] HashAndSignBytes(byte[] DataToSign, RSAParameters Key)
        {
            try
            {
                // Create a new instance of RSACryptoServiceProvider using the 
                // key from RSAParameters.  
                RSACryptoServiceProvider RSAalg = new RSACryptoServiceProvider();

                RSAalg.ImportParameters(Key);

                // Hash and sign the data. Pass a new instance of SHA1CryptoServiceProvider
                // to specify the use of SHA1 for hashing.
                return RSAalg.SignData(DataToSign, new SHA1CryptoServiceProvider());
            }
            catch (CryptographicException e)
            {
                Console.WriteLine(e.Message);

                return null;
            }
        }
    }
}
