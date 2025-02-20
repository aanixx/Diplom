using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
namespace Diplom.Data.IdentityContext
{
    public class IdentityContext : IdentityDbContext<SingleUser>
    {
        public IdentityContext(DbContextOptions<IdentityContext> ops) : base(ops) { }
    }
}
