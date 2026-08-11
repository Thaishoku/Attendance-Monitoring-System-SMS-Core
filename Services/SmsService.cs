using Microsoft.Extensions.Configuration;
using System;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;

namespace AttendanceMonitoringSystem.Services 
{
    public class SmsService 
    {
        private readonly IConfiguration _config;
        
        public SmsService(IConfiguration config) {
            _config = config; 
        }

        public bool SendSms(string toPhoneNumber, string message)
        {
            try
            {
                string formattedNumber = FormatPhoneNumber(toPhoneNumber);
                string apiToken = _config["iProgSms:ApiToken"]!; 
                
                var payload = new
                {
                    api_token = apiToken,
                    phone_number = formattedNumber,
                    message = message
                };

                string jsonPayload = JsonSerializer.Serialize(payload);
                var content = new StringContent(jsonPayload, Encoding.UTF8, "application/json");

                using (var client = new HttpClient())
                {
                    HttpResponseMessage response = client.PostAsync("https://www.iprogsms.com/api/v1/sms_messages", content).Result;
                    string result = response.Content.ReadAsStringAsync().Result;

                    if (response.IsSuccessStatusCode)
                    {
                        Console.WriteLine("iProgSMS Sent Successfully!");
                        return true;
                    }
                    else
                    {
                        Console.WriteLine($"iProgSMS Delivery Failed. Error - {result}");
                        return false;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"General SMS Error - {ex.Message}");
                return false;
            }
        }

        public string FormatPhoneNumber(string phone)
        {
            if (string.IsNullOrEmpty(phone)) return "";
            string cleanPhone = new string(phone.Where(char.IsDigit).ToArray());
 
            if (cleanPhone.StartsWith("63") && cleanPhone.Length == 12)
            {
                return "0" + cleanPhone.Substring(2);
            }
            else if (cleanPhone.StartsWith("09") && cleanPhone.Length == 11)
            {
                return cleanPhone;
            }
            return cleanPhone;
        }
    }
}
