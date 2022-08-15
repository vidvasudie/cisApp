using System;
using System.Collections.Generic;
using System.Text;

namespace cisApp.Common
{
    public class LogCommon
    {
        public LogCommon(string controller, string action, string message)
        {
            Controller = controller;
            Action = action;
            Message = message;
        }
        public enum LogMode
        {
            INSERT = 0,
            UPDATE = 1,
            DELETE = 2,
            SEARCH = 4,
            UPLOAD = 5,
            LOGIN = 6,
            LOGOUT = 7,
            DASHBOARD = 8,
            JOB = 9,
            WAITPAYMENT = 10,
            JOB_RPT = 11,
            CUSTOMER = 12,
            CUSTOME_RPT = 13,
            DESIGNER_REQ = 14,
            DESIGNER_REQ_HIST = 15,
            DESIGNER_PERFORM = 16,
            DESIGNER = 17,
            DESIGNER_PAY_HIST = 18,
            USER_MGNT = 19,
            OFFICER_MGNT = 20,
            PERMISSION = 21,
            CHAT = 22,
            GROUP_CHAT = 23,
            CHAT_HIST = 24,
            GROUP_CHAT_HIST = 25,
            SETTING_SYS = 26,
            SETTING_DATA = 27,
            PLOBLEM = 28,
            FAQ = 29,
            LOG = 30,
            PAYMENT = 31,
            PAYMENT_RPT = 32,
            FORGETPASSWD = 33,
            DETAIL = 34,
            MANAGE = 35,
            PAYMENT_HIST = 36,
            RESETPASSWD = 37
        }
        private string Message { get; set; }
        private string Controller { get; set; }
        private string Action { get; set; }
        public String LogMessage(LogMode mode)
        {
            string message = "";
            switch (mode)
            {
                case LogMode.INSERT:
                    message = $"[{this.Controller}][{this.Action}] เพิ่มข้อมูล {this.Message} "; break;
                case LogMode.UPDATE:
                    message = $"[{this.Controller}][{this.Action}] แก้ไขข้อมูล {this.Message} "; break;
                case LogMode.DELETE:
                    message = $"[{this.Controller}][{this.Action}] ลบข้อมูล {this.Message} "; break;
                case LogMode.SEARCH:
                    message = $"[{this.Controller}][{this.Action}] ค้นหาข้อมูล {this.Message} "; break;
                case LogMode.UPLOAD:
                    message = $"[{this.Controller}][{this.Action}] อัพโหลดข้อมูล {this.Message} "; break; 
                case LogMode.LOGIN:
                    message = $"[{this.Controller}][{this.Action}] เข้าสู่ระบบ {this.Message} "; break;
                case LogMode.LOGOUT:
                    message = $"[{this.Controller}][{this.Action}] ออกจากระบบ {this.Message} "; break;

                case LogMode.DASHBOARD:
                    message = $"[{this.Controller}][{this.Action}] ภาพรวมระบบ {this.Message} "; break;
                case LogMode.JOB:
                    message = $"[{this.Controller}][{this.Action}] ใบงาน {this.Message} "; break;
                case LogMode.WAITPAYMENT:
                    message = $"[{this.Controller}][{this.Action}] รอชำระเงิน {this.Message} "; break;
                case LogMode.JOB_RPT:
                    message = $"[{this.Controller}][{this.Action}] รายงาน(ใบงาน) {this.Message} "; break;
                case LogMode.CUSTOMER:
                    message = $"[{this.Controller}][{this.Action}] รายชื่อลูกค้า {this.Message} "; break;
                case LogMode.CUSTOME_RPT:
                    message = $"[{this.Controller}][{this.Action}] รายงาน(ลูกค้า) {this.Message} "; break;
                case LogMode.DESIGNER_REQ:
                    message = $"[{this.Controller}][{this.Action}] คำขอนักออกแบบ {this.Message} "; break;
                case LogMode.DESIGNER_REQ_HIST:
                    message = $"[{this.Controller}][{this.Action}] ประวัติคำขอ {this.Message} "; break;
                case LogMode.DESIGNER_PERFORM:
                    message = $"[{this.Controller}][{this.Action}] ผลงาน {this.Message} "; break;
                case LogMode.DESIGNER:
                    message = $"[{this.Controller}][{this.Action}] รายชื่อนักออกแบบ {this.Message} "; break;
                case LogMode.DESIGNER_PAY_HIST:
                    message = $"[{this.Controller}][{this.Action}] ประวัติการจ่ายเงิน {this.Message} "; break;

                case LogMode.USER_MGNT:
                    message = $"[{this.Controller}][{this.Action}] จัดการผู้ใช้งาน {this.Message} "; break;
                case LogMode.OFFICER_MGNT:
                    message = $"[{this.Controller}][{this.Action}] ข้อมูลเจ้าหน้าที่ {this.Message} "; break;
                case LogMode.PERMISSION:
                    message = $"[{this.Controller}][{this.Action}] สิทธิ์การใช้งาน {this.Message} "; break;
                case LogMode.CHAT:
                    message = $"[{this.Controller}][{this.Action}] แชทบุคคล {this.Message} "; break;
                case LogMode.GROUP_CHAT:
                    message = $"[{this.Controller}][{this.Action}] แชทกลุ่ม {this.Message} "; break;
                case LogMode.CHAT_HIST:
                    message = $"[{this.Controller}][{this.Action}] ประวัติแชทบุคคล {this.Message} "; break;
                case LogMode.GROUP_CHAT_HIST:
                    message = $"[{this.Controller}][{this.Action}] ประวัติแชทกลุ่ม {this.Message} "; break;
                case LogMode.SETTING_SYS:
                    message = $"[{this.Controller}][{this.Action}] ตั้งค่าระบบ {this.Message} "; break;
                case LogMode.SETTING_DATA:
                    message = $"[{this.Controller}][{this.Action}] ตั้งค่าข้อมูล {this.Message} "; break;
                case LogMode.PLOBLEM:
                    message = $"[{this.Controller}][{this.Action}] แจ้งปัญหาระบบ {this.Message} "; break;

                case LogMode.PAYMENT:
                    message = $"[{this.Controller}][{this.Action}] รายการรอตรวจสอบการชำระเงิน {this.Message} "; break;
                case LogMode.PAYMENT_RPT:
                    message = $"[{this.Controller}][{this.Action}] รายงาน(การชำระเงิน) {this.Message} "; break;
                case LogMode.PAYMENT_HIST:
                    message = $"[{this.Controller}][{this.Action}] ประวัติการชำระเงิน {this.Message} "; break;

                case LogMode.FORGETPASSWD:
                    message = $"[{this.Controller}][{this.Action}] ลืมรหัสผ่าน {this.Message} "; break;
                case LogMode.RESETPASSWD:
                    message = $"[{this.Controller}][{this.Action}] แก้ไขรหัสผ่าน {this.Message} "; break;
                case LogMode.DETAIL:
                    message = $"[{this.Controller}][{this.Action}] รายละเอียด {this.Message} "; break;
                case LogMode.MANAGE:
                    message = $"[{this.Controller}][{this.Action}] จัดการข้อมูล {this.Message} "; break;


                case LogMode.FAQ:
                    message = $"[{this.Controller}][{this.Action}] FAQ {this.Message} "; break;
                case LogMode.LOG:
                    message = $"[{this.Controller}][{this.Action}] ประวัติการใช้งาน {this.Message} "; break; 
                default:
                    message = $"[{this.Controller}][{this.Action}] NO Action {this.Message} "; break;
            }
            return message;
        }


    }
}
