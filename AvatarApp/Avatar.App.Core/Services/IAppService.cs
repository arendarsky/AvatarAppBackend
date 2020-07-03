using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Avatar.App.Core.Services
{
    public interface IAppService
    {
        Task SetSemifinalistAsync(long? userId);
    }
}
