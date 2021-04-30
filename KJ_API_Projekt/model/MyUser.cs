using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KJ_API_Projekt.model
{
    public class MyUser : IdentityUser
    {
        public string FirstName { get;set; }
        public string LastName { get; set; }

    }
}
