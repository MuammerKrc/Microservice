using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Course.Client1.Services.Abstract
{
    public interface IClientCredentialsTokenService
    {
        Task<string> GetToken();
    }
}
