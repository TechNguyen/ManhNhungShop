using ManhNhungShop_Account_Services.DataContext;
using ManhNhungShop_Account_Services.Interface;
using ManhNhungShop_Account_Services.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using Microsoft.Extensions.Options;
using Microsoft.Identity.Client;
using Microsoft.IdentityModel.Tokens;
using OtpNet;
using System.Data;
using System.Diagnostics.SymbolStore;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Net.Mail;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace ManhNhungShop_Account_Services.Repository
{
    public class AccountRepository : IAccounts
    {
        private readonly DataAccountContext _dataAccountContext;

        private readonly SMTP _smtp;

        private const string templatePath = @"Template/sendmail.html";
      
        private readonly IConfiguration _config;
        public AccountRepository(DataAccountContext dataAccountContext, IConfiguration config, IOptions<SMTP> options) {
            _dataAccountContext = dataAccountContext;
            _config = config;
            _smtp = options.Value;
        }
        public async Task<Accounts> Login(LoginModel account)
        {
            var hashpass = HashPassword(account.Password);
            var user = await _dataAccountContext.Accounts.FirstOrDefaultAsync(p => p.UserName == account.UserName && p.Password == hashpass);
            if(user != null)
            {
                return user;
            } else
            {
                return null;
            }
        }
        public async Task<string> Generate(Accounts account)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var generate = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, account.LastName),
                new Claim(ClaimTypes.Email, account.Email),
                new Claim(ClaimTypes.Role, account.Role),
            };
            var token = new JwtSecurityToken(_config["Jwt:Issuer"], _config["Jwt:Audience"], claims, expires: DateTime.Now.AddMinutes(30), signingCredentials: generate);
            return new JwtSecurityTokenHandler().WriteToken(token);
        }


        public async Task<bool> CreateAccount(Accounts account)
        {
            var checkaccount = _dataAccountContext.Accounts.FirstOrDefault(p => p.UserName == account.UserName);
            if(checkaccount != null)
            {
                return false;
            } else
            {
                string hasspass = HashPassword(account.Password);
                account.Password = hasspass;
                _dataAccountContext.Accounts.Add(account);
                _dataAccountContext.SaveChangesAsync();
                return true;
            }
        }

        public async Task<bool> UpdateDetails(Accounts account)
        {
            var checkAccount = _dataAccountContext.Accounts.FirstOrDefault(p => p.UserName == account.UserName);
            if (checkAccount != null)
            {
                _dataAccountContext.Entry(checkAccount).CurrentValues.SetValues(account);
                await _dataAccountContext.SaveChangesAsync();
                return true;
            }
            return false;
        }


        public async Task<bool> ResetPassword(string email)
        {
            
            return true;
        }


        private async Task SendMail(UserOptionMail options)
        {
            MailMessage mail = new MailMessage {
                Subject = options.subject,
                Body = options.body,
                From = new MailAddress(_smtp.SendAddress, _smtp.SendDisplayname),
                IsBodyHtml = _smtp.IsBodyHTML
            };


            mail.To.Add(options.toEmail);


            NetworkCredential networkCredential = new NetworkCredential(_smtp.Username,_smtp.Password);
            // create smtp client
            SmtpClient smtpClient = new SmtpClient()
            {
                Host = _smtp.Host,
                Port = _smtp.Port,
                EnableSsl = _smtp.EnableSSL,
                UseDefaultCredentials = _smtp.UseDefaultCredentials,
                Credentials = networkCredential
            };
            await smtpClient.SendMailAsync(mail);
        }


        private OtpCode generateOtp(string secreket)
        {
            var totp = new Totp(Base32Encoding.ToBytes(secreket), mode: OtpHashMode.Sha256, totpSize: 6);
            DateTime expiration = DateTime.UtcNow.AddMinutes(2);
            string otp = totp.ComputeTotp(expiration);
            OtpCode otpCode = new OtpCode
            {
                Otpcode = Int32.Parse(otp),
                Timestamp = DateTime.UtcNow
            };

            return otpCode;
        }

        public string GetEmailBody(string tempalte)
        {
            var bodyEmail = File.ReadAllText(string.Format(templatePath, tempalte));
            return bodyEmail;
        }


        public string UpdatePlaceHolder(string text, List<KeyValuePair<string, int>> keyValuePairs)
        {
            if (!string.IsNullOrEmpty(text) && keyValuePairs != null)
            {
                foreach (var placeholder in keyValuePairs)
                {
                    if (text.Contains(placeholder.Key))
                    {
                        text = text.Replace(placeholder.Key, placeholder.Value.ToString());
                    }
                }
            }
            return text;
        }


        public async Task SendToTest(UserOptionMail userEmailOption, IConfiguration builder)
        {
            var otpCode = generateOtp(builder["Secretkey"]);
            List<KeyValuePair<string, int>> keyValuePairCode = new List<KeyValuePair<string, int>>(){
                new KeyValuePair<string, int>("{{forget_otp}}",  otpCode.Otpcode)
            };
            userEmailOption.keyValuePairs = keyValuePairCode;
            userEmailOption.body = UpdatePlaceHolder(GetEmailBody("TestEmail"), userEmailOption.keyValuePairs);
            updateOtp(builder, userEmailOption, otpCode);
            await SendMail(userEmailOption);
        }

  

        //add otp to db
        public async Task updateOtp(IConfiguration builder, UserOptionMail userEmailOption, OtpCode otpCode)
        {
            SqlConnection connection = new SqlConnection();
            connection.ConnectionString = builder.GetConnectionString("DefaultConnection");
            connection.Open();
            string procedurename = "dbo.SP_GenerateOtp";
            var emailParams = new SqlParameter("@email", SqlDbType.NVarChar)
            {
                Direction = ParameterDirection.Input,
                Value = userEmailOption.toEmail
            };
            var forGetOtp = new SqlParameter("@forgetOtp", SqlDbType.Int)
            {
                Direction = ParameterDirection.Input,
                Value = otpCode.Otpcode
            };
            var createatOtp = new SqlParameter("@createtime", SqlDbType.DateTime)
            {
                Direction = ParameterDirection.Input,
                Value = otpCode.Timestamp
            };
            using (SqlCommand command = new SqlCommand(procedurename, connection))
            {
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.Add(emailParams);
                command.Parameters.Add(forGetOtp);
                command.Parameters.Add(createatOtp);

                command.ExecuteNonQuery();
            }
        }


        //get MaTv,Password new
        //public OtpCode CheckOtp(checkOtp check, IConfiguration builder)
        //{
        //    SqlConnection connection = new SqlConnection();
        //    connection.ConnectionString = builder.GetConnectionString("DefaultConnection");
        //    connection.Open();
        //    string procedurename = "dbo.SP_checkOtp";
        //    var emailParams = new SqlParameter("@email", SqlDbType.NVarChar, 255)
        //    {
        //        Direction = ParameterDirection.Input,
        //        Value = check.email
        //    };
        //    var forGetOtp = new SqlParameter("@forgetOtp", SqlDbType.Int)
        //    {
        //        Direction = ParameterDirection.Input,
        //        Value = check.Otp
        //    };
        //    var MatvParams = new SqlParameter("@MaTV", SqlDbType.Char, 10)
        //    {
        //        Direction = ParameterDirection.Output,
        //    };
        //    var StatusCode = new SqlParameter("@statusCode", SqlDbType.Int)
        //    {
        //        Direction = ParameterDirection.Output,
        //    };
        //    var createatOtp = new SqlParameter("@createat", SqlDbType.DateTime)
        //    {
        //        Direction = ParameterDirection.Output,
        //    };
        //    OtpSend resetotp = new OtpSend();
        //    using (SqlCommand command = new SqlCommand(procedurename, connection))
        //    {
        //        command.CommandType = CommandType.StoredProcedure;

        //        command.Parameters.Add(emailParams);
        //        command.Parameters.Add(forGetOtp);
        //        command.Parameters.Add(MatvParams);
        //        command.Parameters.Add(StatusCode);
        //        command.Parameters.Add(createatOtp);


        //        command.ExecuteNonQuery();
        //        // check timeline
        //        string MaTv = command.Parameters["@MaTV"].Value.ToString();
        //        int status = (int)command.Parameters["@statusCode"].Value;
        //        resetotp.email = check.email;
        //        resetotp.MaTV = MaTv;
        //        resetotp.Otp = check.Otp;
        //        if (status == 200)
        //        {
        //            DateTime time = (DateTime)command.Parameters["@createat"].Value;
        //            if (DateTime.Compare(time.AddMinutes(2), DateTime.UtcNow) > 0)
        //            {
        //                resetotp.statusCode = status;
        //            }
        //            else
        //            {
        //                resetotp.statusCode = 405;
        //            }
        //        }
        //        resetotp.statusCode = status;
        //    }
        //    return resetotp;


        //}
        private string HashPassword(string password)
        {
            SHA256 hash = SHA256.Create();
            var asByteArr = Encoding.UTF8.GetBytes(password);
            var hashPassord = hash.ComputeHash(asByteArr);
            return Convert.ToBase64String(hashPassord); 
        }

     
    }
}
