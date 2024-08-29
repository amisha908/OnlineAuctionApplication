using OnlineAuction.DAL.Model.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineAuction.BLL.Services.Interface
{
    public interface IAuthService
    {
        Task<string> GenerateJwtTokenAsync(ApplicationUser user);
        Task<ApplicationUser> AuthenticateUserAsync(string email, string password);
        Task<string> GetUsernameByIdAsync(string userId);
    }
}
