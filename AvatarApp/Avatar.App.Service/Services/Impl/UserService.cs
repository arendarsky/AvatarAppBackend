using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Avatar.App.Service.Helpers;
using System.Threading.Tasks;

namespace Avatar.App.Service.Services.Impl
{
    public class UserService : IUserService
    {
        

        public async Task<bool> SetName(string userName)
        {

            //here must be interactions with data base
            
        //delete next line
        await Task.Run(()=>{});
        

            return true;
        }

        
    }
}