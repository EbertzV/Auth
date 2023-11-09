namespace Auth.Entities
{
    public sealed class SessionResult
    {
        private SessionResult(bool valid)
        {
            Valid = valid;
        }

        public static SessionResult NewValid()
            => new SessionResult(true);

        public static SessionResult NewInvalid()
            => new SessionResult(false);

        public bool Valid { get; }
    }
}
