using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Diplom.Data
{
    public class ShafaStoreDBContext : DbContext
    {
        public ShafaStoreDBContext(DbContextOptions<ShafaStoreDBContext> options) : base(options) { }
    }
}
