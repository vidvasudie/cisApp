using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using DIGITAL_ID.library;
using Newtonsoft.Json; 
namespace cisApp.Function
{
    public   class MobileNotfication
    {

        #region สำหรับนักออกแบบ


        public enum ModeDesigner
        {
            favorite, //เมื่อมีลูกค้าที่ กด ชอบไว้ สร้างใบงาน 
            contest, // เมื่อได้รับเลือกในการเข้าประกวด
            winner, // เมื่อได้รับการเลือกจากลูกค้า
            submit, //เมื่อต้อง ถึงเวล่ใกล้ส่งงาน
            alert  // เมื่อลูกค้าแจ้งขอแก้ไขงาน 
        }

        public async void Fordesigner(ModeDesigner modeDesigner,   Guid userId)
        {
            NotiModel obj = new NotiModel();
      
            obj.notification = new notification();

            switch (modeDesigner.ToString())
            {
                case "favorite":

                    obj.notification.body = "มีใบงานที่กดถูกใจคุณ ถูกสร้างขึ้นคลิ๊กเลย";
                    obj.notification.title = "มีลูกค้าสนใจคุณ!";
                    //obj.notification.icon = "";

                    break;
                case "contest":

                    obj.notification.body = "มีใบงานที่คุณได้รับคัดเลือกเข้าประกวดงาน อย่าลืมสอบถามรายละเอียดความต้องการละ คลิ๊กเลยเพื่อดูรายละเอียด ";
                    obj.notification.title = "คุณได้รับคัดเลือกในกวดเข้าร่วมประกวดงาน";
                    //obj.notification.icon = ""; 
                    break;
                case "winner":

                    obj.notification.body = "เราขอแสดงความยินดีด้วยงานของคุณได้รับเลือกให้เป็นผู้ชนะในครั้งนี้ อย่าลืมส่งรายละเอียดการออกแบบให้ลูกค้า คลิ๊กเลยเพื่อดูรายละเอียด ";
                    obj.notification.title = "ขอแสดงความยินดี";
                    //obj.notification.icon = ""; 
                    break;
                case "submit":

                    obj.notification.body = "เราแจ้งเตือนการส่งงานเนื่องจากงานที่ท่านได้ประกวดไว้ใกล้ถึงกำหนดส่ง คลิ๊กเลยเพื่อดูรายละเอียด";
                    obj.notification.title = "ใกล้ถึงเวลาส่งงานแล้วนะ!";
                    //obj.notification.icon = ""; 
                    break;
                case "alert":

                    obj.notification.body = "เราขอแจ้งให้ท่านทราบว่า ใบงานของท่านได้ถูกขอแก้ไขโดยลูกค้า อย่าลืมสอบถามรายละเอียดความต้องการละ คลิ๊กเลยเพื่อดูรายละเอียด";
                    obj.notification.title = "มีใบงานถูกขอแก้ไข!";
                    //obj.notification.icon = ""; 
                    break; 
            } 
             
            GetNotification.Manage.add(userId, "", obj.notification.title, obj.notification.body); 
            var _c = GetUserClientId.Get.GetbyUserid(userId);
            if (_c != null)
            {
                obj.to = _c.ClientId; 
                await NotifyAsync(obj);
            }
        }
         
        public class NotiModel
        { 
         public string to { get; set; } 
        public notification notification { get; set; }
        }
        public class notification {
            public string body { get; set; }
            public string title { get; set; }
            public string click_action { get; set; } 
        } 
        #endregion
        #region สำหรับลูกค้า
        public enum Modecustomer
        {
            regist,// เมื่อ มี ดีไซต์เนอร์สมัคร
            regist3,// เมื่อมีดีไซต์เนอร์ครบ 3 คนแจ้งให้จ่ายเงิน
            submit,// เมื่อ นักออกแบบส่งงาน
        }
        #endregion

        public async void Forcustomer(Modecustomer modeDesigner,   Guid userId)
        {



            NotiModel obj = new NotiModel();
       
            obj.notification = new notification();

            switch (modeDesigner.ToString())
            {
                case "regist":

                    obj.notification.body = "กรุณาเลือกและคอนเฟิร์มฟรีแลนซ์เพื่อเริ่มงาน";
                    obj.notification.title = "ฟรีแลนซ์ส่งงานให้คุณแล้ว";
                    //obj.notification.icon = "";

                    break;
                case "regist3":

                    obj.notification.body = "กรุณาคอนเฟิร์มฟรีแลนซ์เพื่อเริ่มงาน";
                    obj.notification.title = "เราหาฟรีแลนซ์ให้คุณครบแล้ว";
                    //obj.notification.icon = ""; 
                    break;
                case "submit":

                    obj.notification.body = "เข้าดูงานที่ได้รับของคุณ";
                    obj.notification.title = "ฟรีแลนซ์ส่งงานให้คุณแล้ว";
                    //obj.notification.icon = ""; 
                    break;
               
            }
 
            GetNotification.Manage.add(userId, "", obj.notification.title, obj.notification.body);
            var _c = GetUserClientId.Get.GetbyUserid(userId);
            if (_c != null)
            {
                obj.to = _c.ClientId;
                await NotifyAsync(obj);
            }
        }





        public async Task<bool> NotifyAsync(NotiModel obj )
        {
            try
            {
                // Get the server key from FCM console
                var serverKey = string.Format("key={0}", "AAAAqkbgFso:APA91bGczQPjYBPkej-7mhArSDDgeGGCK1pylhuSIvipTcIpt-BjRh4_7md4omhfK7g7tB7yJ6KdgtmEINpOniQ4DpCSmHy_zsaNRGZHDfkLb6QHGQMUgsX5_HPN4L3y7n0pMyu3X3wU");

                // Get the sender id from FCM console
               // var senderId = string.Format("id={0}", "477030036496");

             

                // Using Newtonsoft.Json
             

                using (var httpRequest = new HttpRequestMessage(HttpMethod.Post, "https://fcm.googleapis.com/fcm/send"))
                {
                    httpRequest.Headers.TryAddWithoutValidation("Authorization", serverKey);
                 //   httpRequest.Headers.TryAddWithoutValidation("Sender", senderId);
                    httpRequest.Content = new StringContent(JsonConvert.SerializeObject(obj) , Encoding.UTF8, "application/json");

                    using (var httpClient = new HttpClient())
                    {
                        var result = await httpClient.SendAsync(httpRequest);

                        if (result.IsSuccessStatusCode)
                        {
                            return true;
                        }
                        else
                        {
                            // Use result.StatusCode to handle failure
                            // Your custom error handler here
                            //_logger.LogError($"Error sending notification. Status Code: {result.StatusCode}");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                //_logger.LogError($"Exception thrown in Notify Service: {ex}");
            }

            return false;
        }


    }
}
