using cisApp.Core;
using cisApp.library;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Text;

namespace cisApp.Function
{
    public static class GetUserResetPassword
    {
        readonly static IConfigurationRoot config = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
                      .AddJsonFile("appsettings.json")
                      .Build();

        public class Get
        {
            public static UsersResetPassword GetByToken(string jwt)
            {
                try
                {
                    using (var context = new CAppContext())
                    {
                        var handler = new JwtSecurityTokenHandler();
                        var token = handler.ReadJwtToken(jwt);
                        var expDate = token.ValidTo;
                        if (DateTime.UtcNow > expDate)
                        {
                            return null;
                        }

                        var data = context.UsersResetPassword.Where(o => o.Token == jwt && o.IsDeleted == false).FirstOrDefault();

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
            public static UsersResetPassword Add(Guid userId, bool mobileLink = false)
            {
                try
                {
                    using (var context = new CAppContext())
                    {
                        // only update content
                        UsersResetPassword obj = new UsersResetPassword()
                        {
                            UserId = userId,
                            CreateDate = DateTime.Now,
                            IsDeleted = false,
                            RefCode = cisApp.library.Utility.RandomPassword(6)
                        };

                        var tokenHandler = new JwtSecurityTokenHandler();
                        var key = Encoding.ASCII.GetBytes("SecretKey0123456789Capp");
                        var tokenDescriptor = new SecurityTokenDescriptor
                        {
                            Subject = new ClaimsIdentity(new Claim[]
                                      {
                                                new Claim("UserId", userId.ToString()),
                                                new Claim("Dateget", DateTime.Now.ToString())

                                      }),
                            Expires = DateTime.UtcNow.AddHours(24),
                            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
                        };
                        var token = tokenHandler.CreateToken(tokenDescriptor);

                        obj.Token = tokenHandler.WriteToken(token);

                        context.UsersResetPassword.Update(obj);

                        context.SaveChanges();

                        // SendEmail

                        var user = context.Users.Find(userId);
                        if (mobileLink)
                        {
                            string webAdmin = config.GetSection("WebConfig:MobileLink").Value;

                            EmailModel emailModel = new EmailModel()
                            {
                                ToMail = user.Email,
                                Subject = "คำขอรีเซ็ตรหัสผ่าน",
                                TemplateFileName = "UserResetPassword.htm",
                                Body = new List<string>()
                            {
                                user.Fname + " " + user.Lname, webAdmin + "ResetPassword/ChangPassword/" + obj.Token
                            }
                            };

                            SendMail.Send(emailModel);
                        }
                        else
                        {
                            string webAdmin = config.GetSection("WebConfig:AdminWebStie").Value;

                            EmailModel emailModel = new EmailModel()
                            {
                                ToMail = user.Email,
                                Subject = "คำขอรีเซ็ตรหัสผ่าน",
                                TemplateFileName = "UserResetPassword.htm",
                                Body = new List<string>()
                            {
                                user.Fname + " " + user.Lname, webAdmin + "ResetPassword/Index?token=" + obj.Token
                            }
                            };

                            SendMail.Send(emailModel);
                        }
                        

                        

                        return obj;
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }

            public static Users ResetPassword(string token, string password)
            {
                try
                {
                    using (var context = new CAppContext())
                    {
                        var obj = GetUserResetPassword.Get.GetByToken(token);

                        if (obj == null)
                        {
                            throw new Exception("Invalid token");
                        }

                        UsersPassword usersPassword = new UsersPassword()
                        {
                            UserId = obj.UserId,
                            Password = Encryption.Encrypt(password)
                        };

                        context.UsersPassword.Update(usersPassword);

                        context.SaveChanges();

                        var user = context.Users.Find(obj.UserId);

                        user.PasswordId = usersPassword.PasswordId;

                        context.Users.Update(user);

                        context.SaveChanges();

                        // delete token
                        var userResetPassword = context.UsersResetPassword.Where(o => o.Token == token).FirstOrDefault();

                        userResetPassword.IsDeleted = true;
                        userResetPassword.UpdateDate = DateTime.Now;

                        context.UsersResetPassword.Update(userResetPassword);

                        context.SaveChanges();

                        return user;
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }

        }
    }
}
