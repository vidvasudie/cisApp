using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using DIGITAL_ID.library;
using Newtonsoft.Json; 
namespace cisApp.Function
{
    public class MobileNotfication
    {

        #region สำหรับนักออกแบบ


        public enum ModeDesigner
        {
            favorite, //เมื่อมีลูกค้าที่ กด ชอบไว้ สร้างใบงาน 
            contest, // เมื่อได้รับเลือกในการเข้าประกวด
            winner, // เมื่อได้รับการเลือกจากลูกค้า
            submit, //เมื่อต้อง ถึงเวล่ใกล้ส่งงาน
            alert,  // เมื่อลูกค้าแจ้งขอแก้ไขงาน 
            alertreject,  // เมื่อลูกค้าปฎิเสธนักออกแบบ

            approve, // อนุมัติ คำขอเป็นนักออกแบบ
            notApprove,  // ไม่อนุมัติ คำขอเป็นนักออกแบบ

            complete, //เมื่อลูกค้ายืนยันผลงาน
            installfile // เมื่อลูกค้าขอไฟล์แบบติดตั้ง

        }

        public async void Fordesigner(ModeDesigner modeDesigner, Guid userId, Guid? JobsID)
        {
            NotiModel obj = new NotiModel();
            obj.data = new Data();
            obj.data.isDesigner = true;
            obj.data.jobId = JobsID;
            obj.notification = new notification();
            string page = "";
            switch (modeDesigner.ToString())
            {
                case "favorite":
                    obj.notification.body = "มีลูกค้าที่กดถูกใจคุณ สร้างใบงานใหม่ คลิ๊กเลย!!";
                    obj.notification.title = "มีลูกค้าสนใจคุณ!";
                    //obj.notification.icon = "";
                    page = "Contrat";
                    obj.data.prevPage = "Home";
                    break;
                case "contest":
                    obj.notification.body = "มีใบงานที่คุณได้รับคัดเลือกเข้าประกวดงาน อย่าลืมสอบถามรายละเอียดความต้องการ คลิ๊กเลยเพื่อดูรายละเอียด";
                    obj.notification.title = "คุณได้รับคัดเลือกในกวดเข้าร่วมประกวดงาน";
                    //obj.notification.icon = ""; 
                    page = "StatusDesigner";
                    break;
                case "winner":
                    obj.notification.body = "เราขอแสดงความยินดีด้วยงานของคุณได้รับเลือกให้เป็นผู้ชนะในครั้งนี้ อย่าลืมส่งรายละเอียดการออกแบบให้ลูกค้า คลิ๊กเลยเพื่อดูรายละเอียด";
                    obj.notification.title = "ขอแสดงความยินดี";
                    page = "StatusDesigner";
                    //obj.notification.icon = ""; 
                    break;
                case "submit": 
                    obj.notification.body = "เราแจ้งเตือนการส่งงานเนื่องจากงานที่ท่านได้ประกวดไว้ใกล้ถึงกำหนดส่ง คลิ๊กเลยเพื่อดูรายละเอียด";
                    obj.notification.title = "ใกล้ถึงเวลาส่งงานแล้วนะ!";
                    page = "StatusDesigner";

                    //obj.notification.icon = ""; 
                    break;
                case "alert":

                    obj.notification.body = "เราขอแจ้งให้ท่านทราบว่า ใบงานของท่านได้ถูกขอแก้ไขโดยลูกค้า อย่าลืมสอบถามรายละเอียดความต้องการ คลิ๊กเลยเพื่อดูรายละเอียด";
                    obj.notification.title = "มีใบงานถูกขอแก้ไข!";
                    page = "StatusDesigner";

                    //obj.notification.icon = ""; 
                    break;
               case "alertreject":

                    obj.notification.body = "เราขอแจ้งให้ท่านทราบว่า ท่านไม่ผ่านการคัดเลือกสมัครใบงาน";
                    obj.notification.title = "ท่านไม่ผ่านการคัดเลือก สมัครใบงาน";
                    page = "StatusDesigner";

                    //obj.notification.icon = ""; 
                    break;
                case "approve":

                    obj.notification.body = "ขอแสดงความยินดี ท่านได้รับการอนุมัติคำขอเป็นนักออกแบบแล้ว";
                    obj.notification.title = "ยินดีด้วย";
                    page = "Home";

                    //obj.notification.icon = ""; 
                    break;

                case "notApprove":

                    obj.notification.body = "คำขอของท่าน ไม่ผ่านการอนุมติคำขอเป็นนักออกแบบ";
                    obj.notification.title = "แจ้งผลดำเนินการ";
                    page = "Home";

                    //obj.notification.icon = ""; 
                    break;
                case "complete":
                    obj.notification.body = "ขอแสดงความยินดี ลูกค้ายืนยันผลงานเรียบร้อย";
                    obj.notification.title = "แจ้งผลดำเนินการ";
                    page = "StatusDesigner";
                    break;
                case "installfile":
                    obj.notification.body = "ลูกค้าแจ้งขอไฟล์แบบติดตั้ง";
                    obj.notification.title = "แจ้งผลดำเนินการ";
                    page = "StatusDesigner";
                    break;
            }
            var NotiID = new Core.Notification();

            if (JobsID != null)
            {
                 NotiID = GetNotification.Manage.add(userId, "", obj.notification.title, obj.notification.body, page, JobsID.Value);
            }
            var _c = GetUserClientId.Get.GetbyUserid(userId);
            if (_c != null)
            {
                obj.to = _c.ClientId;
                //if (NotiID != null)
                //{
                //    obj.notification.click_action = page;//string.Format(page+"/{0}", NotiID.Id);
                //}
                //obj.notification.click_action = page;
                obj.data.action = page;

                await NotifyAsync(obj);
            }
        }
        
        public class NotiModel
        { 
            public string to { get; set; } 
            public notification notification { get; set; }
            public Android android { get; set; }
            public Ios apns { get; set; }
            public Data data { get; set; }
        }
        public class notification {
            public string body { get; set; }
            public string title { get; set; }
            //public string click_action { get; set; } 
        }
        #region android
        public class Android 
        { 
            public Android()
            {
                notification = new AndroidNotification();
            }
            public AndroidNotification notification { get; set; }
        }
        public class AndroidNotification
        {
            public string sound { get; set; } = "default";
        }
        #endregion
        #region ios
        public class Ios
        {
            public Ios()
            {
                payload = new IosPayload();
            }
            public IosPayload payload { get; set; }
        }
        public class IosPayload
        {
            public IosPayload()
            {
                aps = new PayloadData();
            }
            public PayloadData aps { get; set; }
        }
        public class PayloadData
        {
            public string sound { get; set; } = "default";
        }
        #endregion
        public class Data
        {
            /// <summary>
            /// บอกว่าดูมุมมองนักออกแบบหรือลูกค้า
            /// </summary>
            public bool isDesigner { get; set; }
            public Guid? jobId { get; set; }
            /// <summary>
            /// หน้าก่อนหน้า สำหรับ check ว่าจะแสดงปุ่มรับงานหรือไม่
            /// </summary>
            public string prevPage { get; set; } = "Home";
            public string action { get; set; }

        }
        #endregion

        #region สำหรับลูกค้า
        public enum Modecustomer
        {
            regist,// เมื่อ มี ดีไซต์เนอร์สมัคร
            regist3,// เมื่อมีดีไซต์เนอร์ครบ 3 คนแจ้งให้จ่ายเงิน
            submit,// เมื่อ นักออกแบบส่งงาน
            payment//เมื่อ ลูกค้าโอนเงินแล้ว แจ้งเตือนว่ากำลังตรวจสอบ
        }
        #endregion

        public async void Forcustomer(Modecustomer modeDesigner,   Guid userId,Guid JobsID)
        {

            string page = "";

            NotiModel obj = new NotiModel();
            obj.data = new Data();
            obj.data.isDesigner = false;
            obj.data.jobId = JobsID;
            obj.notification = new notification();

            switch (modeDesigner.ToString())
            {
                case "regist":

                    obj.notification.body = "กรุณาเลือกและยืนยัน นักออกแบบเพื่อเริ่มงาน";
                    obj.notification.title = "นักออกแบบสมัครใบงานแล้ว";
                    //obj.notification.icon = "";
                    page = "StatusUser";
                    break;
                case "regist3":

                    obj.notification.body = "กรุณาเลือกและยืนยัน นักออกแบบเพื่อเริ่มงาน";
                    obj.notification.title = "เราหานักออกแบบให้คุณครบแล้ว";
                    //obj.notification.icon = ""; 
                    page = "StatusUser";
                    break;
                case "submit":

                    obj.notification.body = "เข้าดูผลงานนักออกแบบที่ได้รับ";
                    obj.notification.title = "นักออกแบบส่งงานให้คุณแล้ว";
                    //obj.notification.icon = ""; 
                    page = "StatusUser";

                    break;
                case "payment": 
                    obj.notification.body = "ขณะนี้ระบบได้รับข้อมูลการชำระเงินเรียบร้อย เจ้าหน้าที่กำลังเร่งตรวจสอบยอดเงินต่อไป";
                    obj.notification.title = "ระบบกำลังเร่งตรวจสอบยอดเงิน";
                    //obj.notification.icon = ""; 
                    page = "StatusUser";

                    break;

            }
 
            //var NotiID = GetNotification.Manage.add(userId, "", obj.notification.title, obj.notification.body, page, JobsID);
            var _c = GetUserClientId.Get.GetbyUserid(userId);
            if (_c != null)
            {
                obj.to = _c.ClientId;
                //obj.notification.click_action = page;// string.Format(page+"/{0}", NotiID.Id) ;
                obj.data.action = page;

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

                obj.android = new Android(); 
                obj.apns = new Ios(); 

                // Using Newtonsoft.Json


                using (var httpRequest = new HttpRequestMessage(HttpMethod.Post, "https://fcm.googleapis.com/fcm/send"))
                {
                    httpRequest.Headers.TryAddWithoutValidation("Authorization", serverKey);
                    //   httpRequest.Headers.TryAddWithoutValidation("Sender", senderId);

                    string _data = JsonConvert.SerializeObject(obj);
                    httpRequest.Content = new StringContent(_data, Encoding.UTF8, "application/json");

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
                //throw ex;
            }

            return false;
        }


    }
}
