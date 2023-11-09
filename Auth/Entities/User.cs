namespace Auth.Entities
{
    public class User
    {
        public User(string username, string password)
        {
            this.username = username;
            this.password = password;
        }

        public string username { get; }
        public string password { get; }
    }
}