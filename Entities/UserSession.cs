namespace Store.Entities
{
    public class UserSession
    {
        public int Id { get; set; }
        public string UserToken { get; set; }
        public DateTime ExpirationDateToken { get; set; }
        public string UserBrowser { get; set; }
        public string UserSO { get; set; }
    }
}
