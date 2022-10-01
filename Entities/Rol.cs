namespace Store.Entities
{
    public class Rol
    {
        public int Id { get; set; }
        public string RoleName { get; set; }
        public DateTime StartOperations { get; set; }
        public DateTime EndOperations { get; set; }
        public bool IsServerAccess { get; set; }
        public ICollection<Permission> Permissions { get; set; }
    }
}
