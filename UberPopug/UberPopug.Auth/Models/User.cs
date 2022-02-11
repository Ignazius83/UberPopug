using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UberPopug.Auth.Models
{
    public class User : IdentityUser
    {
        public string public_id { get; set; }
    }
}
