namespace ChatWebAPI.Hubs
{
    public class UserManager
    {
        private readonly Dictionary<string, string> _users = new Dictionary<string, string>();

        public void AddUser(string userId, string connectionId)
        {
            _users.Add(userId, connectionId);
        }

        public void RemoveUser(string userId)
        {
            _users.Remove(userId);
        }

        public string GetConnectionId(string userId)
        {
            return _users[userId];
        }
    }
}
