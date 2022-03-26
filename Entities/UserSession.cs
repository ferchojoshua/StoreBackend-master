namespace Store.Entities
{
    public class UserSession
    {
        public int Id { get; set; }
        public string UserToken { get; set; }
        public string UserDevice { get; set; }
    }
}
