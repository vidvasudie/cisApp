﻿using cisApp.Common;
using cisApp.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks; 
using Newtonsoft.Json;
using Microsoft.AspNetCore.Http;

namespace cisApp.Function
{
    public class GetLogActivity
    {
        public class Get
        {
            public static List<LogActivity> GetByuserId(Guid id)
            {
                try
                {
                    using (var context = new CAppContext())
                    {
                        var data = context.LogActivity.Where(o => o.RefUserId == id).ToList();

                        return data;
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
            public static async Task<LogActivity> AddAsync(HttpRequest request, Guid? userId, string fullname, LogCommon.LogMode mode, string msg = "", string exception = "")
            {
                try
                {
                    using (var context = new CAppContext())
                    {
                        using (var dbContextTransaction = context.Database.BeginTransaction())
                        {
                            LogActivity data = new LogActivity();
                            LogCommon log = new LogCommon(request.RouteValues["Controller"].ToString(), request.RouteValues["Action"].ToString(), msg);
                            data.Controller = request.RouteValues["Controller"].ToString();
                            data.Action = request.RouteValues["Action"].ToString();
                            data.Ip = request.HttpContext.Connection.RemoteIpAddress.ToString();
                            data.RefUserId = userId;
                            data.RefFullname = fullname;
                            data.Note = log.LogMessage(mode);
                            data.Url = $"{request.Scheme}://{request.Host}{request.PathBase}{request.Path}";
                            data.ExceptionNote = exception;
                            data.RequestData = await GetRequestData(request);
                            data.CreatedDate = DateTime.Now;

                            context.LogActivity.Add(data);
                            context.SaveChanges();

                            dbContextTransaction.Commit();

                            return data;
                        }
                    }

                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }

            private static async Task<string> GetRequestData(HttpRequest request)
            {
                string result = "";
                /// query string
                result = request.QueryString.ToString();
                /// body
                try
                {
                    request.EnableBuffering();
                    request.Body.Position = 0;
                    result += await new System.IO.StreamReader(request.Body).ReadToEndAsync();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                }
                ///form
                try
                {
                    if (request.Form.Count > 0)
                    {

                        var dictionary = RequestFormToDictionary(request.Form);
                        result += JsonConvert.SerializeObject(dictionary, Formatting.Indented);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                }

                return result;
            }

            public static IDictionary<string, string> RequestFormToDictionary(IFormCollection col)
            {
                var dict = new Dictionary<string, string>();

                foreach (var key in col.Keys)
                {
                    dict.Add(key, col[key]);
                }

                return dict;
            }

        }
    }
}
