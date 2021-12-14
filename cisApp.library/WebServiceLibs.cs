using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Net.Http;
using System.IO;
using Newtonsoft.Json;

namespace DIGITAL_ID.library
{
    public class WebServiceLibs
    {
        public WebServiceLibs()
        {
            decompressionMethods = DecompressionMethods.GZip;
            Querystring = new Dictionary<string, object>();
            ItemsHeader = new Dictionary<string, string>();
            ItemsBody = new Dictionary<string, object>();
        }

        /// <summary>
        /// ตั้งค่า Call Web service
        /// </summary>
        /// <returns></returns>
        /// 
        public DecompressionMethods decompressionMethods { get; set; }
        // ค่า Query String
        public Dictionary<string, object> Querystring { get; set; }
        //ค่า Header
        public Dictionary<string, string> ItemsHeader { get; set; }
        // ค่า JsonBody
        public Dictionary<string, Object> ItemsBody { get; set; }

        public string UrlSetting { get; set; }
        public class HeaderSetting
        {
            public string HeaderName { get; set; }
            public string Value { get; set; }
        }
        //private WebRequestMethods TypeMethod(string TypeAction)
        //{
        //    WebRequestMethods _result = new WebRequestMethods();
        //    switch (TypeAction)
        //    {
        //        case "Get":
        //            return WebRequestMethods.Http.Get; 
        //    }
        //}
        public enum Http
        {
            CONNECT,
            GET,
            HEAD,
            MKCOL,
            POST,
            PUT
        }
        public enum AcceptType
        {
            Json = 1,
            urlencoded = 2
        }
        private static string ChkTypeAccept(string value)
        {
            string _type = "application/json;charset=utf-8";
            switch (value)
            {
                case "Json":
                    _type = "application/json;charset=utf-8";
                    break;
                case "urlencoded": _type = "application/x-www-form-urlencoded"; break;
                default: _type = "application/json;charset=utf-8"; break;
            }
            return _type;
        }
        public static string QueryString(IDictionary<string, object> dict)
        {
            var list = new List<string>();
            foreach (var item in dict)
            {
                list.Add(item.Key + "=" + item.Value);
            }
            return string.Join("&", list);
        }
        public T CallWebservice<T>(Http Method, AcceptType acceptType)
        {
            return JsonConvert.DeserializeObject<T>(CallWebservice(Method, acceptType));
        }
        public string CallWebservice(Http Method, AcceptType acceptType)
        {
            if (string.IsNullOrEmpty(UrlSetting))
            {
                return "กรุณาระบุค่า UrlSetting ที่ใช้ดึงข้อมูล";

            }
            string html = string.Empty;

            // if QueryString not empty add to UrlSetting
            if (Querystring.Count > 0)
            {
                UrlSetting = UrlSetting + "?" + QueryString(Querystring);
            }

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(UrlSetting);
            request.ContentType = "application/json;charset=utf-8";// ****
            foreach (var Items in ItemsHeader)
            {
                request.Headers.Add(Items.Key, Items.Value);
            }
            request.Method = Method.ToString();

            if (ItemsBody.Count > 0)
            {
                List<string> postString = new List<string>();
                foreach (var Items in ItemsBody)
                {
                    postString.Add("\"" + Items.Key + "\":" + "\"" + Items.Value.ToString() + "\"");

                }
                string postData = String.Join(",", postString);
                var data = Encoding.UTF8.GetBytes("{" + postData + "}");
                request.ContentLength = data.Length;

                using (var stream = request.GetRequestStream())
                {
                    stream.Write(data, 0, data.Length);
                }
            }

            request.Accept = ChkTypeAccept(acceptType.ToString());
            request.AutomaticDecompression = decompressionMethods;
            string responseFromServer = "";
            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
            using (Stream stream = response.GetResponseStream())
            using (StreamReader reader = new StreamReader(stream))
            {
                responseFromServer = reader.ReadToEnd();
            }
            return responseFromServer;
        }
    }
}
