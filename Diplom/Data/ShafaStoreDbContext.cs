using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShafaStoreDbTables;
namespace Diplom.Data
{
    public class ShafaStoreDBContext : DbContext
    {
        internal DbSet<User> users {  get; set; }
        public ShafaStoreDBContext(DbContextOptions<ShafaStoreDBContext> options) : base(options) { }
    }
}
