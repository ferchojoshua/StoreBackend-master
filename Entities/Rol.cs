using System.Collections;

namespace Store.Entities
{
    public class Rol
    {
        public int Id { get; set; }
        public string RoleName { get; set; }
        public ICollection<Permission> Permissions { get; set; }
    }
}
