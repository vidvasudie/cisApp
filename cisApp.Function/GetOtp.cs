using cisApp.Core;
using Microsoft.Extensions.Configuration;
using RestSharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using cisApp.library;

namespace cisApp.Function
{
    public static class GetOtp
    {
        static int OtpExpiredMinute = 10;

        static string _SuccessNode = "<Status>1</Status>";

        public static IConfigurationRoot _config = new ConfigurationBuilder()
                                    .SetBasePath(Directory.GetCurrentDirectory())
                                    .AddJsonFile("appsettings.json")
                                    .Build();

        public class Get
        {
            public static bool ValidateOtp(string sentTo, string otpMessage)
            {
                try
                {
                    using (var context = new CAppContext())
                    {
                        var otp = context.Otp.Where(o => o.SendTo == sentTo && o.OtpMsg == otpMessage).FirstOrDefault();

                        if (otp != null)
                        {
                            if (otp.Expire > DateTime.Now)
                            {
                                return true;
                            }
                        }
                        return false;
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        public class Manage
        {
            public static Otp Add(Guid? userId, string otpType, string sendTo, int? otpRef)
            {
                try
                {
                    using (var context = new CAppContext())
                    {
                        using (var dbContextTransaction = context.Database.BeginTransaction())
                        {
                            Otp otp = new Otp()
                            {
                                UserId = userId,
                                CreateDate = DateTime.Now,
                                Expire = DateTime.Now.AddMinutes(OtpExpiredMinute),
                                OtpMsg = Utility.RandomOtpNumber(5),
                                IsReferesh = otpRef == null ? false : true,
                                OtpRef = otpRef,
                                OtpType = otpType,
                                SendTo = sendTo
                            };

                            context.Otp.Add(otp);
                            context.SaveChanges();

                            if (otp.OtpType == "sms")
                            {
                                SendSMSOtp(otp);
                            }

                            dbContextTransaction.Commit();

                            return otp;
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }

            public static bool SendSMSOtp(Otp otp)
            {
                try
                {
                    string username = _config.GetSection("SMSAPI:Username").Value;
                    string password = _config.GetSection("SMSAPI:Password").Value;
                    string sender = _config.GetSection("SMSAPI:Sender").Value;
                    string scheduleDelivery = _config.GetSection("SMSAPI:ScheduledDelivery").Value;
                    string force = _config.GetSection("SMSAPI:Force").Value;
                    //otp.SendTo = "0974677906";

                    string message = "รหัส OTP ของท่านคือ " + otp.OtpMsg;

                    var client = new RestClient("http://www.thaibulksms.com/sms_api.php"
                        + "?username=" + username
                        + "&password=" + password
                        + "&sender=" + sender
                        + "&ScheduledDelivery=" + scheduleDelivery
                        + "&force=" + force
                        + "&msisdn=" + otp.SendTo
                        + "&message=" + message);
                    client.Timeout = -1;
                    var request = new RestRequest(Method.GET);
                    IRestResponse response = client.Execute(request);
                    Console.WriteLine(response.Content);

                    bool isSuccess = false;
                    if (response.Content.Contains(_SuccessNode))
                    {
                        isSuccess = true;
                        return isSuccess;
                    }
                    else
                    {
                        isSuccess = false;
                        throw new Exception("เกิดข้อผิดพลาดในการส่ง OTP");
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }

        }

    }
}
