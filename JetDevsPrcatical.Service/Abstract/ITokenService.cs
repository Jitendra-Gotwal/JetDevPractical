using JetDevsPrcatical.Data.Entity;
using JetDevsPrcatical.Data.Response.Account;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace JetDevsPrcatical.Service.Abstract
{
    public interface ITokenService
    {
        Task<LoginResponse> GenerateAccessToken(Users users);       
    }
}
