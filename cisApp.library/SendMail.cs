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

		public static bool SendMailResetPassword(string toMail, string username, string password, string webRootPath)
		{
			string host = config.GetSection("ConfigMail:Host").Value;
			string fromEmail = config.GetSection("ConfigMail:FromEmail").Value;
			string passwordEmail = config.GetSection("ConfigMail:Password").Value;
			int port = Int32.Parse(config.GetSection("ConfigMail:Port").Value);
			bool enableSsl = Boolean.Parse(config.GetSection("ConfigMail:EnableSsl").Value);
			string domain = config.GetSection("Domain").Value;
			try
			{
				string _filePath = Path.Combine(webRootPath, "Templates/UserForgetPassword.htm");
				string html = System.IO.File.ReadAllText(_filePath);

				MailMessage mail = new MailMessage();
				SmtpClient SmtpServer = new SmtpClient();
				SmtpServer.UseDefaultCredentials = false;
				mail.To.Add(toMail);
				mail.From = new MailAddress(fromEmail);
				mail.Subject = "คำขอรีเซ็ตรหัสผ่าน";
				mail.IsBodyHtml = true;
				mail.Body = string.Format(html, username, password);
				SmtpServer.EnableSsl = enableSsl;
				ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
				SmtpServer.Host = host;
				SmtpServer.Credentials = new System.Net.NetworkCredential(fromEmail, passwordEmail);
				SmtpServer.Port = port;
				SmtpServer.DeliveryMethod = System.Net.Mail.SmtpDeliveryMethod.Network;
				SmtpServer.Send(mail);

				return true;
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}
	}
}
