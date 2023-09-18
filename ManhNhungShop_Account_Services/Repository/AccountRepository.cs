﻿using ManhNhungShop_Account_Services.DataContext;
using ManhNhungShop_Account_Services.Interface;
using ManhNhungShop_Account_Services.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using Microsoft.IdentityModel.Tokens;
using System.Diagnostics.SymbolStore;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace ManhNhungShop_Account_Services.Repository
{
    public class AccountRepository : IAccounts
    {
        private readonly DataAccountContext _dataAccountContext;


        private readonly IConfiguration _config;
        public AccountRepository(DataAccountContext dataAccountContext) {
            _dataAccountContext = dataAccountContext;
        }
        public async Task<Accounts> Authentication(Accounts account)
        {
            var user = _dataAccountContext.Accounts.FirstOrDefault(p => p.UserName.ToLower() == account.UserName.ToLower() && p.Password.ToLower() == account.Password.ToLower() );
            if(user != null)
            {
                return user;
            }
            return null;
        }
        public Task<bool> Generate(AccountsDetails account)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var generate = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);


            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, account.FullName),
                new Claim(ClaimTypes.Email, account.Email),
                new Claim(ClaimTypes.Role, account.Role),
            };
            var token = new JwtSecurityToken(_config["Jwt:Issuer"], _config["Jwt:Audience"], claims, expires: DateTime.Now.AddMinutes(30), signingCredentials: generate);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public Task<bool> Verify(string username)
        {
            throw new NotImplementedException();
        }
    }
}