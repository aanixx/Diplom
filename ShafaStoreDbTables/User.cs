using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
 
namespace ShafaStoreDbTables
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Patronymic { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public string Gender { get; set; }
        public string City { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public ICollection<string> Subscriber { get; set; }
        public ICollection<string> Sales { get; set; }
        public ICollection<string> Reviews { get; set; }
        public SqlBinary Photo { get; set; }
    }
}
