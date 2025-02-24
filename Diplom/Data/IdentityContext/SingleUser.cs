using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Diplom.Data.IdentityContext
{
    public class SingleUser : IdentityUser
    {
        public string? Login { get; set; }

        public override string?  PhoneNumber { get; set; }
        public override string? Email { get; set; }
    }
}
