using cisApp.Core;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;

namespace cisApp.Function
{
    public static class GetUserResetPassword
    {
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
            public static UsersResetPassword Add(Guid userId)
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
                            RefCode = "RefCode"
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

                        return obj;
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
