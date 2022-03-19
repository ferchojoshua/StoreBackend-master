namespace Store.Entities
{
    public class RolPermissions
    {
        public int Id { get; set; }
        public Rol Roles { get; set; }
        public Permission Permisos { get; set; }
    }
}
