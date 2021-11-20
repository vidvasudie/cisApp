using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Text;

namespace cisApp.library
{
	public static class SendMail
	{
		readonly static IConfigurationRoot config = new ConfigurationBuilder()
			.SetBasePath(Directory.GetCurrentDirectory())
					  .AddJsonFile("appsettings.json")
					  .Build();

		public static bool SendMailStatus(string subject, string fullname, string userType, string status, string toMail, string webRootPath, string lang)
		{
			string host = config.GetSection("ConfigMail:Host").Value;
			string fromEmail = config.GetSection("ConfigMail:FromEmail").Value;
			string passwordEmail = config.GetSection("ConfigMail:Password").Value;
			int port = Int32.Parse(config.GetSection("ConfigMail:Port").Value);
			bool enableSsl = Boolean.Parse(config.GetSection("ConfigMail:EnableSsl").Value);
			string domain = config.GetSection("Domain").Value;
			try
			{
				string _filePath = Path.Combine(webRootPath, "Templates/SendUpdateStatusTH.htm");
				string html = System.IO.File.ReadAllText(_filePath);

				//ใช้ทดสอบ
				MailMessage mail = new MailMessage();
				SmtpClient SmtpServer = new SmtpClient();
				SmtpServer.UseDefaultCredentials = false;
				mail.To.Add(toMail);
				mail.From = new MailAddress(fromEmail);
				mail.Subject = subject;
				mail.IsBodyHtml = true;
				mail.Body = string.Format(html, fullname, userType, status, domain + "Login");
				SmtpServer.EnableSsl = enableSsl;
				ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls;
				SmtpServer.Host = host;
				SmtpServer.Credentials = new System.Net.NetworkCredential(fromEmail, passwordEmail);
				SmtpServer.Port = port;
				SmtpServer.DeliveryMethod = System.Net.Mail.SmtpDeliveryMethod.Network;
				SmtpServer.Send(mail);



				//MailMessage mailMessage = new MailMessage();
				//SmtpClient smtpClient = new SmtpClient();
				//smtpClient.Host = host;
				//smtpClient.Port = port;
				//smtpClient.EnableSsl = true;
				//if (smtpClient.EnableSsl)
				//{
				//	ServicePointManager.ServerCertificateValidationCallback =
				//		delegate (object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
				//		{ return true; };
				//}
				//ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls;
				//smtpClient.UseDefaultCredentials = false;
				//smtpClient.DeliveryMethod = System.Net.Mail.SmtpDeliveryMethod.Network;
				//NetworkCredential MyCredentials = new NetworkCredential(fromEmail, passwordEmail);
				//smtpClient.Credentials = MyCredentials;
				//smtpClient.Timeout = 100000;
				//mailMessage.From = new MailAddress(fromEmail);
				//mailMessage.Subject = "รหัสผ่านเข้าใช้งานของท่าน";
				//mailMessage.Body = "ดดด";
				//mailMessage.IsBodyHtml = true;
				//if (SetMailAddressCollection(mailMessage.To, toMail))
				//{
				//	smtpClient.Send(mailMessage);
				//}
				//return true;


				return true;
			}
			catch (Exception ex)
			{
				return false;
			}
		}
		public static bool SendMailPassword(string fullname, string username, string password, string toMail, string webRootPath, string lang)
		{
			string host = config.GetSection("ConfigMail:Host").Value;
			string fromEmail = config.GetSection("ConfigMail:FromEmail").Value;
			string passwordEmail = config.GetSection("ConfigMail:Password").Value;
			int port = Int32.Parse(config.GetSection("ConfigMail:Port").Value);
			bool enableSsl = Boolean.Parse(config.GetSection("ConfigMail:EnableSsl").Value);
			string domain = config.GetSection("Domain").Value;
			try
			{
				string _filePath = Path.Combine(webRootPath, "Templates/SendPasswordTH.htm");
				string html = System.IO.File.ReadAllText(_filePath);

				//ใช้ทดสอบ
				MailMessage mail = new MailMessage();
				SmtpClient SmtpServer = new SmtpClient();
				SmtpServer.UseDefaultCredentials = false;
				mail.To.Add(toMail);
				mail.From = new MailAddress(fromEmail);
				mail.Subject = "รหัสผ่านของท่าน";
				mail.IsBodyHtml = true;
				mail.Body = string.Format(html, fullname, username, password, domain + "Login");
				SmtpServer.EnableSsl = enableSsl;
				ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls;
				SmtpServer.Host = host;
				SmtpServer.Credentials = new System.Net.NetworkCredential(fromEmail, passwordEmail);
				SmtpServer.Port = port;
				SmtpServer.DeliveryMethod = System.Net.Mail.SmtpDeliveryMethod.Network;
				SmtpServer.Send(mail);



				//MailMessage mailMessage = new MailMessage();
				//SmtpClient smtpClient = new SmtpClient();
				//smtpClient.Host = host;
				//smtpClient.Port = port;
				//smtpClient.EnableSsl = true;
				//if (smtpClient.EnableSsl)
				//{
				//	ServicePointManager.ServerCertificateValidationCallback =
				//		delegate (object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
				//		{ return true; };
				//}
				//ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls;
				//smtpClient.UseDefaultCredentials = false;
				//smtpClient.DeliveryMethod = System.Net.Mail.SmtpDeliveryMethod.Network;
				//NetworkCredential MyCredentials = new NetworkCredential(fromEmail, passwordEmail);
				//smtpClient.Credentials = MyCredentials;
				//smtpClient.Timeout = 100000;
				//mailMessage.From = new MailAddress(fromEmail);
				//mailMessage.Subject = "รหัสผ่านเข้าใช้งานของท่าน";
				//mailMessage.Body = "ดดด";
				//mailMessage.IsBodyHtml = true;
				//if (SetMailAddressCollection(mailMessage.To, toMail))
				//{
				//	smtpClient.Send(mailMessage);
				//}
				//return true;


				return true;
			}
			catch (Exception ex)
			{
				return false;
			}
		}

		public static bool SendMailResetPassword(string fullname, string username, string password, string toMail, string webRootPath, string lang)
		{
			string host = config.GetSection("ConfigMail:Host").Value;
			string fromEmail = config.GetSection("ConfigMail:FromEmail").Value;
			string passwordEmail = config.GetSection("ConfigMail:Password").Value;
			int port = Int32.Parse(config.GetSection("ConfigMail:Port").Value);
			bool enableSsl = Boolean.Parse(config.GetSection("ConfigMail:EnableSsl").Value);
			string domain = config.GetSection("Domain").Value;
			try
			{
				string _filePath = Path.Combine(webRootPath, "Templates/ChangePasswordTH.htm");
				string html = System.IO.File.ReadAllText(_filePath);

				//ใช้ทดสอบ
				MailMessage mail = new MailMessage();
				SmtpClient SmtpServer = new SmtpClient();
				SmtpServer.UseDefaultCredentials = false;
				mail.To.Add(toMail);
				mail.From = new MailAddress(fromEmail);
				mail.Subject = "รหัสผ่านใหม่ของท่าน";
				mail.IsBodyHtml = true;
				mail.Body = string.Format(html, fullname, username, password, domain + "Login");
				SmtpServer.EnableSsl = enableSsl;
				//ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls;
				SmtpServer.Host = host;
				SmtpServer.Credentials = new System.Net.NetworkCredential(fromEmail, passwordEmail);
				SmtpServer.Port = port;
				SmtpServer.DeliveryMethod = System.Net.Mail.SmtpDeliveryMethod.Network;
				SmtpServer.Send(mail);
				return true;
			}
			catch (Exception ex)
			{
				return false;
			}
		}

		//public static bool SendMailRequestReportStatus(string fullName, string requestCode, string requestDate, string toMail, string webRootPath, string lang)
		//{
		//	string host = config.GetSection("ConfigMail:Host").Value;
		//	string fromEmail = config.GetSection("ConfigMail:FromEmail").Value;
		//	string passwordEmail = config.GetSection("ConfigMail:Password").Value;
		//	int port = Int32.Parse(config.GetSection("ConfigMail:Port").Value);
		//	bool enableSsl = Boolean.Parse(config.GetSection("ConfigMail:EnableSsl").Value);
		//	string domain = config.GetSection("Domain").Value;
		//	try
		//	{
		//		string _filePath = Path.Combine(webRootPath, "Templates/SendRequestReportStatusTH.htm");
		//		string html = System.IO.File.ReadAllText(_filePath);

		//		//ใช้ทดสอบ
		//		MailMessage mail = new MailMessage();
		//		SmtpClient SmtpServer = new SmtpClient();
		//		SmtpServer.UseDefaultCredentials = false;
		//		mail.To.Add(toMail);
		//		mail.From = new MailAddress(fromEmail);
		//		mail.Subject = "อัพเดตสถานะการขอใช้รายงาน";
		//		mail.IsBodyHtml = true;
		//		mail.Body = string.Format(html, fullName, requestCode, fullName, requestDate, GetReportRows(requestCode), domain + "Login");
		//		SmtpServer.EnableSsl = enableSsl;
		//		//ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls;
		//		SmtpServer.Host = host;
		//		//SmtpServer.UseDefaultCredentials = false;
		//		SmtpServer.Credentials = new System.Net.NetworkCredential(fromEmail, passwordEmail);
		//		SmtpServer.Port = port;
		//		SmtpServer.DeliveryMethod = System.Net.Mail.SmtpDeliveryMethod.Network;
		//		SmtpServer.Send(mail);

		//		return true;
		//	}
		//	catch (Exception ex)
		//	{
		//		Console.WriteLine(ex.ToString());
		//		return false;
		//	}
		//}

		//public static bool SendMailAppointment(SysAppointment sysAppointment, string webRootPath)
		//{
		//	string host = config.GetSection("ConfigMail:Host").Value;
		//	string fromEmail = config.GetSection("ConfigMail:FromEmail").Value;
		//	string passwordEmail = config.GetSection("ConfigMail:Password").Value;
		//	int port = Int32.Parse(config.GetSection("ConfigMail:Port").Value);
		//	bool enableSsl = Boolean.Parse(config.GetSection("ConfigMail:EnableSsl").Value);
		//	string domain = config.GetSection("Domain").Value;
		//	try
		//	{
		//		SysUser sysUser = GetSysUser.Get.GetByGuid(sysAppointment.Usercode);
		//		string _filePath = "";
		//		string html = "";
		//		string body = "";
		//		string appointmentDate = sysAppointment.Appmdate.Value.ToString("dd MMM yy", new CultureInfo("th-TH"))
		//			+ " " + TimeFormat.ToTimeFormat(sysAppointment.Appmtime) + "น.";
		//		string detailUrl = domain + "AppointmentRequest/Report/" + sysAppointment.Appmcode.Value.ToString();
		//		string appointmentStatus = "";
		//		if (sysAppointment.Appmstatus == 1)
		//		{
		//			appointmentStatus = "รับนัดหมาย";
		//		}
		//		else if (sysAppointment.Appmstatus == 2)
		//		{
		//			appointmentStatus = "เลื่อนนัดหมาย";
		//		}
		//		else if (sysAppointment.Appmstatus == 3)
		//		{
		//			appointmentStatus = "ยกเลิกนัดหมาย";
		//		}

		//		_filePath = Path.Combine(webRootPath, "Templates/SendAppointmentApprove.htm");
		//		html = System.IO.File.ReadAllText(_filePath);

		//		body = string.Format(html, sysUser.Firstname + " " + sysUser.Surname, appointmentStatus
		//			, appointmentDate, sysAppointment.DoctorName, sysAppointment.RespDoctorName
		//			, sysAppointment.Appmremark, QrGenerator.GetUrl(sysAppointment.Appmcode.Value), detailUrl
		//			, BarcodeGenerator.GetUrl(sysAppointment.Apcode));


		//		string toMail = sysUser.Email;
		//		string subject = "อัพเดทสถานะการนัดหมาย " + sysAppointment.Apcode;

		//		//ใช้ทดสอบ
		//		MailMessage mail = new MailMessage();
		//		SmtpClient SmtpServer = new SmtpClient();
		//		SmtpServer.UseDefaultCredentials = false;
		//		mail.To.Add(toMail);
		//		mail.From = new MailAddress(fromEmail);
		//		mail.Subject = subject;
		//		mail.IsBodyHtml = true;


		//		mail.Body = body;
		//		SmtpServer.EnableSsl = enableSsl;
		//		ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls;
		//		SmtpServer.Host = host;
		//		SmtpServer.Credentials = new System.Net.NetworkCredential(fromEmail, passwordEmail);
		//		SmtpServer.Port = port;
		//		SmtpServer.DeliveryMethod = System.Net.Mail.SmtpDeliveryMethod.Network;
		//		SmtpServer.Send(mail);

		//		return true;
		//	}
		//	catch (Exception ex)
		//	{
		//		return false;
		//	}
		//}

		//private static string GetReportRows(string requestCode)
		//{
		//	var maps = GetSysReportReqMap.Get.GetByRequestCode(requestCode);
		//	string template = "<tr> " +
		//	 "   <td style=\"text-align: left;padding-left: 10px; \">{0}</td>" +
		//	 "   <td style=\"text-align: center; \">{1}</td>" +
		//	 "</tr>";
		//	string tmp = "";
		//	foreach (var item in maps)
		//	{
		//		tmp += String.Format(template, GetSysReport.Get.GetByReportId(item.Reportcode).ReportName, item.Status == null ? "รอดำเนินการ" : item.Status.Value ? "อนุมัติ" : "ไม่อนุมัติ");
		//	}

		//	return tmp;
		//}

		private static bool SetMailAddressCollection(MailAddressCollection toAddresses, string mailId)
		{
			bool successfulAddressCreation = true;
			toAddresses.Add(new MailAddress(mailId));
			return successfulAddressCreation;
		}



		public static bool TestMail(string fullname, string username, string password, string toMail, string webRootPath, string lang)
		{
			string host = config.GetSection("ConfigMail:Host").Value;
			string fromEmail = config.GetSection("ConfigMail:FromEmail").Value;
			string passwordEmail = config.GetSection("ConfigMail:Password").Value;
			int port = Int32.Parse(config.GetSection("ConfigMail:Port").Value);
			bool enableSsl = Boolean.Parse(config.GetSection("ConfigMail:EnableSsl").Value);
			string domain = config.GetSection("Domain").Value;
			string _filePath = Path.Combine(webRootPath, "Templates/ChangePasswordTH.htm");
			string html = System.IO.File.ReadAllText(_filePath);

			//ใช้ทดสอบ
			MailMessage mail = new MailMessage();
			SmtpClient SmtpServer = new SmtpClient();
			SmtpServer.UseDefaultCredentials = false;
			mail.To.Add(toMail);
			mail.From = new MailAddress(fromEmail);
			mail.Subject = "รหัสผ่านใหม่ของท่าน";
			mail.IsBodyHtml = true;
			mail.Body = string.Format(html, fullname, username, password, domain + "Login");
			SmtpServer.EnableSsl = enableSsl;
			ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls;
			SmtpServer.Host = host;
			SmtpServer.Credentials = new System.Net.NetworkCredential(fromEmail, passwordEmail);
			SmtpServer.Port = port;
			SmtpServer.DeliveryMethod = System.Net.Mail.SmtpDeliveryMethod.Network;
			SmtpServer.Send(mail);
			return true;
		}


		public static bool TestMailNotPass(string fullname, string username, string password, string toMail, string webRootPath, string lang)
		{
			string host = config.GetSection("ConfigMail:Host").Value;
			string fromEmail = config.GetSection("ConfigMail:FromEmail").Value;
			string passwordEmail = config.GetSection("ConfigMail:Password").Value;
			int port = Int32.Parse(config.GetSection("ConfigMail:Port").Value);
			bool enableSsl = Boolean.Parse(config.GetSection("ConfigMail:EnableSsl").Value);
			string domain = config.GetSection("Domain").Value;
			string _filePath = Path.Combine(webRootPath, "Templates/ChangePasswordTH.htm");
			string html = System.IO.File.ReadAllText(_filePath);

			MailMessage mail = new MailMessage();
			SmtpClient SmtpServer = new SmtpClient();
			mail.To.Add(toMail);
			mail.From = new MailAddress(fromEmail);
			mail.Subject = "รหัสผ่านใหม่ของท่าน";
			mail.IsBodyHtml = true;
			mail.Body = string.Format(html, fullname, username, password, domain + "Login");
			SmtpServer.Host = host;
			SmtpServer.Port = port;
			SmtpServer.EnableSsl = enableSsl;
			SmtpServer.DeliveryMethod = System.Net.Mail.SmtpDeliveryMethod.Network;
			SmtpServer.Send(mail);
			return true;
		}




	}
}
