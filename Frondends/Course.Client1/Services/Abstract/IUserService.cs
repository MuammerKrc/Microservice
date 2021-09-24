using Course.Client1.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Course.Client1.Services.Abstract
{
    public interface IUserService
    {
        Task<UserViewModel> GetUser();
    }
}
