using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Linq;

namespace cisApp.library
{
    public static class Encryption
    {
        public static string AppSettings_SecretKey = "P@ssw0rd@aitDOPOne";
        public static string AppSettings_SecretKey2 = "P@ssw0rd@aitDOPTwo";

        public static string UrlEncrypt(string message)
        {
            return Convert.ToBase64String(Encoding.UTF8.GetBytes(message)); ;
        }
        public static string UrlDecrypt(string message)
        {
            return Encoding.UTF8.GetString(Convert.FromBase64String(message));
        }

        public static string Encrypt(string Message)
        {
            if (!string.IsNullOrEmpty(Message))
            {
                byte[] Results;
                System.Text.UTF8Encoding UTF8 = new System.Text.UTF8Encoding();

                MD5CryptoServiceProvider HashProvider = new MD5CryptoServiceProvider();
                byte[] TDESKey = HashProvider.ComputeHash(UTF8.GetBytes("LicenseByAIT@@#"));

                TripleDESCryptoServiceProvider TDESAlgorithm = new TripleDESCryptoServiceProvider();

                TDESAlgorithm.Key = TDESKey;
                TDESAlgorithm.Mode = CipherMode.ECB;
                TDESAlgorithm.Padding = PaddingMode.PKCS7;

                byte[] DataToEncrypt = UTF8.GetBytes(Message);
                try
                {
                    ICryptoTransform Encryptor = TDESAlgorithm.CreateEncryptor();
                    Results = Encryptor.TransformFinalBlock(DataToEncrypt, 0, DataToEncrypt.Length);
                }
                finally
                {
                    TDESAlgorithm.Clear();
                    HashProvider.Clear();
                }

                return Convert.ToBase64String(Results);
            }
            else
            {
                return null;
            }
        }
        public static string Decrypt(string Message)
        {
            if (!string.IsNullOrEmpty(Message))
            {
                byte[] Results;
                System.Text.UTF8Encoding UTF8 = new System.Text.UTF8Encoding();

                MD5CryptoServiceProvider HashProvider = new MD5CryptoServiceProvider();
                byte[] TDESKey = HashProvider.ComputeHash(UTF8.GetBytes("LicenseByAIT@@#"));

                TripleDESCryptoServiceProvider TDESAlgorithm = new TripleDESCryptoServiceProvider();
                TDESAlgorithm.Key = TDESKey;
                TDESAlgorithm.Mode = CipherMode.ECB;
                TDESAlgorithm.Padding = PaddingMode.PKCS7;

                byte[] DataToDecrypt = Convert.FromBase64String(Message);
                try
                {
                    ICryptoTransform Decryptor = TDESAlgorithm.CreateDecryptor();
                    Results = Decryptor.TransformFinalBlock(DataToDecrypt, 0, DataToDecrypt.Length);
                }
                finally
                {
                    TDESAlgorithm.Clear();
                    HashProvider.Clear();
                }

                return UTF8.GetString(Results);
            }
            else
            {
                return null;
            }
        }


        public static string EncryptJWTTokenToString(string Id)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.Default.GetBytes(AppSettings_SecretKey));
            var securityKey2 = new SymmetricSecurityKey(Encoding.Default.GetBytes(AppSettings_SecretKey2));

            var signingCredentials = new SigningCredentials(
                                    securityKey,
                                    SecurityAlgorithms.HmacSha512);

            List<Claim> claims = new List<Claim>()
            {
                new Claim("Id", Id),
            };

            var ep = new EncryptingCredentials(
                securityKey2,
                SecurityAlgorithms.Aes128KW,
                SecurityAlgorithms.Aes128CbcHmacSha256);

            var handler = new JwtSecurityTokenHandler();

            var jwtSecurityToken = handler.CreateJwtSecurityToken(
                "issuer",
                "Audience",
                new ClaimsIdentity(claims),
                DateTime.Now,
                DateTime.Now.AddHours(10),
                DateTime.Now,
                signingCredentials,
                ep);

            string tokenString = handler.WriteToken(jwtSecurityToken);

            return tokenString;
        }

        public static string DecryptJWTTokenToString(string tokenString)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.Default.GetBytes(AppSettings_SecretKey));
            var securityKey2 = new SymmetricSecurityKey(Encoding.Default.GetBytes(AppSettings_SecretKey2));

            var jwt = new JwtSecurityToken(tokenString);

            var tokenValidationParameters = new TokenValidationParameters()
            {
                ValidIssuers = new string[]
                {
                    "issuer"
                },
                ValidAudiences = new string[]
                {
                    "Audience"
                },
                IssuerSigningKey = securityKey,
                // This is the decryption key
                TokenDecryptionKey = securityKey2
            };

            SecurityToken validatedToken;
            var handler = new JwtSecurityTokenHandler();

            ClaimsPrincipal claimsP = handler.ValidateToken(tokenString, tokenValidationParameters, out validatedToken);

            return claimsP.Claims.ToList()[0].Value;
        }


        public static string MD5(string Message)
        {
            var textBytes = Encoding.Default.GetBytes(Message);
            var cryptHandler = new MD5CryptoServiceProvider();
            var hash = cryptHandler.ComputeHash(textBytes);
            var ret = "";
            foreach (var i in hash)
            {
                ret += i.ToString("x2");
            }
            return ret;
        }
    }
}
