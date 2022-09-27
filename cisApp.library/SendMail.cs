using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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

		public static bool Send(EmailModel emailModel, EmailSettingModel setting)
		{
			string host = setting.Host;
			string fromEmail = setting.FromEmail;
			string passwordEmail = setting.Password;
			int port = Int32.Parse(setting.Port);
			bool enableSsl = Boolean.Parse(setting.EnableSsl);

			try
			{
				string _filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Templates", emailModel.TemplateFileName);
				string html = System.IO.File.ReadAllText(_filePath);

				MailMessage mail = new MailMessage();
				SmtpClient SmtpServer = new SmtpClient();
				SmtpServer.UseDefaultCredentials = false;
				mail.To.Add(emailModel.ToMail);
				mail.From = new MailAddress(fromEmail);
				mail.Subject = emailModel.Subject;
				mail.IsBodyHtml = true;
				mail.Body = string.Format(html, emailModel.Body.Select(x => x.ToString()).ToArray());
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
				throw ex;
			}
		}

		public static bool SendMailResetPassword(string toMail, string username, string password, string webRootPath, EmailSettingModel setting)
		{
			string host = setting.Host;
			string fromEmail = setting.FromEmail;
			string passwordEmail = setting.Password;
			int port = Int32.Parse(setting.Port);
			bool enableSsl = Boolean.Parse(setting.EnableSsl);
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

		public static bool SendMailProblemReply(string toMail, string username, string problem, string reply, string webRootPath, EmailSettingModel setting)
		{
			string host = setting.Host;
			string fromEmail = setting.FromEmail;
			string passwordEmail = setting.Password;
			int port = Int32.Parse(setting.Port);
			bool enableSsl = Boolean.Parse(setting.EnableSsl);
			string domain = config.GetSection("Domain").Value;
			try
			{
				string _filePath = Path.Combine(webRootPath, "Templates/ProblemReply.htm");
				string html = System.IO.File.ReadAllText(_filePath);

				MailMessage mail = new MailMessage();
				SmtpClient SmtpServer = new SmtpClient();
				SmtpServer.UseDefaultCredentials = false;
				mail.To.Add(toMail);
				mail.From = new MailAddress(fromEmail);
				mail.Subject = "ตอบกลับ แจ้งปัญหาระบบ";
				mail.IsBodyHtml = true;
				mail.Body = string.Format(html, username, problem, reply);
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
