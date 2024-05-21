using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace WorkZen.Helpers
{
    public class JwtHelper
    {
        //secret key
        private static readonly SymmetricSecurityKey SecretKey;

        //assigning a key to the generated rsa key
        static JwtHelper()
        {
            SecretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Convert.ToString(ConfigurationManager.AppSettings["config:JwtKey"])));
        }


        //this method will create encrypted jwt token
        public static string CreateEncryptedToken(string employeecode, string role)
        {
            //the actual payload key value object for your token
            var Claims = new List<Claim> {
                new Claim(JwtRegisteredClaimNames.Sub, employeecode),
                new Claim(ClaimTypes.Role, role)
                };

            //credentials for signing the jwt token
            var SigningCredentials = new SigningCredentials(SecretKey, SecurityAlgorithms.HmacSha256);

            var Expires = DateTime.Now.AddDays(Convert.ToDouble(Convert.ToString(ConfigurationManager.AppSettings["config:JwtExpireDays"])));

            var JwtToken = new JwtSecurityToken(
                issuer: ConfigurationManager.AppSettings["config:JwtIssuer"],
                audience: ConfigurationManager.AppSettings["config:JwtAudience"],
                claims: Claims,
                expires: Expires,
                signingCredentials: SigningCredentials);

            return new JwtSecurityTokenHandler().WriteToken(JwtToken);
        }

        public static string ValidateToken(string Token)
        {
            if(Token is null)
            {
                return null;
            }
            var TokenHandler = new JwtSecurityTokenHandler();

            try
            {
                TokenHandler.ValidateToken(Token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = SecretKey,
                    ValidateIssuer = true,
                    ValidateAudience = true,
                }, out SecurityToken ValidatedToken);

                var JwtToken = (JwtSecurityToken)ValidatedToken;

                var EmployeeCode = JwtToken.Claims.First(claim => claim.Type == "sub");

            }
            catch
            {

            }
        }



    }
}