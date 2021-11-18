using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace cisApp
{
    public class ResponseModel
    {
        public bool success { get; set; }
        public string message { get; set; }
        public bool isRedirect { get; set; }
        public string redirectUrl { get; set; }
        public object data { get; set; }

        public ResponseModel(bool success, string message, bool isRedirect, string redirectUrl, object data = null)
        {
            this.success = success;
            this.message = message;
            this.isRedirect = isRedirect;
            this.redirectUrl = redirectUrl;
            this.data = data;
        }

        public ResponseModel()
        {

        }

        public ResponseModel ResponseSuccess()
        {
            return new ResponseModel(true
                , MessageCommon.SaveSuccess
                , false
                , "");
        }

        public ResponseModel ResponseSuccess(string message)
        {
            return new ResponseModel(true
                , message
                , false
                , "");
        }

        public ResponseModel ResponseSuccess(string message, object data)
        {
            return new ResponseModel(true
                , message
                , true
                , ""
                , data);
        }

        public ResponseModel ResponseSuccess(string message, string redirect)
        {
            return new ResponseModel(true
                , message
                , true
                , redirect);
        }

        public ResponseModel ResponseSuccess(string message, string redirect, object data)
        {
            return new ResponseModel(true
                , message
                , true
                , redirect
                , data);
        }
        public ResponseModel ResponseSuccess(string message, string redirect, string id)
        {
            return new ResponseModel(true
                , message
                , true
                , redirect + "/" + id
                );
        }

        public ResponseModel ResponseError()
        {
            return new ResponseModel(false
                , MessageCommon.SaveFail
                , false
                , "");
        }



        public ResponseModel ResponseError(string message)
        {
            return new ResponseModel(false
                , message
                , false
                , "");
        }

        public ResponseModel ResponseError(string message, object data)
        {
            return new ResponseModel(false
                , message
                , false
                , ""
                , data);
        }
    }
}
