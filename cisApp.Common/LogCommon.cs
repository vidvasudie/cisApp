﻿using System;
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
        [Flags]
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
            RESETPASSWD = 37,
            /*API*/
            JOB_CREATE = 38,
            JOB_CANCEL = 39,
            JOB_REJECT_CA = 40,
            SUBMIT_JOB = 41,
            FINISH_JOB = 42,
            JOB_CA_SELECT = 43,
            JOB_REVIEW_SUBMIT = 44,
            JOB_REQ_FILE = 45,
            JOB_REQ_EDIT = 46,

            BANK_EDIT = 47,
            PAYMENT_SLIP_ADD = 48,
            REGISTER = 49,
            DELETE_ACC = 50,

            DESIGNER_CANCEL = 51,
            DESIGNER_REGIST = 52,
            DESIGNER_FAV = 53,
            DESIGNER_BOOKMARK = 54,

            HOME=55,
            DESIGNER_JOB_HISTORY = 56,
            DESIGNER_JOB_CONTEST = 57,
            DESIGNER_PROFILE=58,
            NOTIFICATION=59,
            WINNER_LIST = 60,
            REVIEW_LIST = 61,
            CONTEST_SUMMARY=62,
            DESIGNER_FAV_LIST=63,
            BANK_PROFILE=64,
            CUSTOMER_PROFILE=65,
            CHAT_LIST = 66,
            CUSTOMER_JOB_HISTORY = 67,
            CUSTOMER_JOB_LIST = 68
            /*API*/
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

                case LogMode.JOB_CREATE:
                    message = $"[{this.Controller}][{this.Action}] สร้างใบงาน {this.Message} "; break;
                case LogMode.JOB_CANCEL:
                    message = $"[{this.Controller}][{this.Action}] ยกเลิกใบงาน {this.Message} "; break;
                case LogMode.JOB_REJECT_CA:
                    message = $"[{this.Controller}][{this.Action}] ปฏิเสธนักออกแบบ {this.Message} "; break;
                case LogMode.SUBMIT_JOB:
                    message = $"[{this.Controller}][{this.Action}] ส่งงาน {this.Message} "; break;
                case LogMode.FINISH_JOB:
                    message = $"[{this.Controller}][{this.Action}] ยืนยันรับงาน {this.Message} "; break;
                case LogMode.JOB_CA_SELECT:
                    message = $"[{this.Controller}][{this.Action}] เลือกนักออกแบบ {this.Message} "; break;
                case LogMode.JOB_REVIEW_SUBMIT:
                    message = $"[{this.Controller}][{this.Action}] แสดงความคิดเห็นนักออกแบบ {this.Message} "; break;
                case LogMode.JOB_REQ_FILE:
                    message = $"[{this.Controller}][{this.Action}] ขอไฟล์แบบติดตั้ง {this.Message} "; break;
                case LogMode.JOB_REQ_EDIT:
                    message = $"[{this.Controller}][{this.Action}] ขอแก้ไขผลงาน {this.Message} "; break;

                case LogMode.BANK_EDIT:
                    message = $"[{this.Controller}][{this.Action}] ขอแก้ไขบัญชี {this.Message} "; break;
                case LogMode.PAYMENT_SLIP_ADD:
                    message = $"[{this.Controller}][{this.Action}] แนบข้อมูลการชำระเงิน {this.Message} "; break;
                case LogMode.REGISTER:
                    message = $"[{this.Controller}][{this.Action}] ลงทะเบียน {this.Message} "; break;
                case LogMode.DELETE_ACC:
                    message = $"[{this.Controller}][{this.Action}] ลบผู้ใช้งาน {this.Message} "; break;

                case LogMode.DESIGNER_CANCEL:
                    message = $"[{this.Controller}][{this.Action}] ยกเลิกสมัครใบงาน {this.Message} "; break;
                case LogMode.DESIGNER_REGIST:
                    message = $"[{this.Controller}][{this.Action}] สมัครเข้าร่วมใบงาน {this.Message} "; break;
                case LogMode.DESIGNER_FAV:
                    message = $"[{this.Controller}][{this.Action}] ถูกใจนักออกแบบ {this.Message} "; break;
                case LogMode.DESIGNER_BOOKMARK:
                    message = $"[{this.Controller}][{this.Action}] Bookmark นักออกแบบ {this.Message} "; break;

                case LogMode.HOME:
                    message = $"[{this.Controller}][{this.Action}] หน้าหลัก {this.Message} "; break;
                case LogMode.DESIGNER_JOB_HISTORY:
                    message = $"[{this.Controller}][{this.Action}] รายการใบงานรอสถานะของนักออกแบบ {this.Message} "; break;
                case LogMode.DESIGNER_JOB_CONTEST:
                    message = $"[{this.Controller}][{this.Action}] รายการใบงานที่รอประกวดของนักออกแบบ {this.Message} "; break;
                case LogMode.DESIGNER_PROFILE:
                    message = $"[{this.Controller}][{this.Action}] ประวัตินักออกแบบ {this.Message} "; break;
                case LogMode.NOTIFICATION:
                    message = $"[{this.Controller}][{this.Action}] รายการแจ้งเตือน {this.Message} "; break;
                case LogMode.WINNER_LIST:
                    message = $"[{this.Controller}][{this.Action}] รายการผู้ชนะ 50 อันดับ {this.Message} "; break;
                case LogMode.REVIEW_LIST:
                    message = $"[{this.Controller}][{this.Action}] รายการรีวีวนักออกแบบ {this.Message} "; break;
                case LogMode.CONTEST_SUMMARY:
                    message = $"[{this.Controller}][{this.Action}] รายการชนะการประกวด {this.Message} "; break;
                case LogMode.DESIGNER_FAV_LIST:
                    message = $"[{this.Controller}][{this.Action}] รายการคนที่สนใจนักออกแบบ {this.Message} "; break;
                case LogMode.BANK_PROFILE:
                    message = $"[{this.Controller}][{this.Action}] ข้อมูลบัญชีธนาคาร {this.Message} "; break;
                case LogMode.CUSTOMER_PROFILE:
                    message = $"[{this.Controller}][{this.Action}] ประวัติผู้ใช้งาน {this.Message} "; break;
                case LogMode.CHAT_LIST:
                    message = $"[{this.Controller}][{this.Action}] รายการแชท {this.Message} "; break;
                case LogMode.CUSTOMER_JOB_HISTORY:
                    message = $"[{this.Controller}][{this.Action}] ประวัติสถานะใบงานลูกค้า {this.Message} "; break;
                case LogMode.CUSTOMER_JOB_LIST:
                    message = $"[{this.Controller}][{this.Action}] รายการใบงานลูกค้าที่กำลังดำเนินการ {this.Message} "; break;

                default:
                    message = $"[{this.Controller}][{this.Action}] NO Action {this.Message} "; break;
            }
            return message;
        }


    }
}
