using Microsoft.AspNetCore.Identity;

namespace Store.Entities
{
    public class User : IdentityUser
    {
        public string FirstName { get; set; }

        //Este id solo se usara para las migraciones futuras, no sirve para nada mas
        public int IdUserMiration { get; set; }
        public string SecondName { get; set; }
        public string LastName { get; set; }
        public string SecondLastName { get; set; }
        public string Address { get; set; }
        public string FullName => $"{FirstName} {LastName}";
        public Rol Rol { get; set; }
        public bool IsActive { get; set; }
        public bool IsDarkMode { get; set; }
        public bool IsActiveSession { get; set; }
        public bool IsDefaultPass { get; set; }
        public UserSession UserSession { get; set; }    
        public ICollection<Almacen> StoreAccess { get; set; }
    }
}
