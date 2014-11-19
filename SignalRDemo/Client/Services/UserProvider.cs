using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client.Services
{


    internal class UserProvider : IUserProvider
    {
        public string Username
        {
            get { return "WPF-" + new Random().Next(1000); }
        }
    }
}
