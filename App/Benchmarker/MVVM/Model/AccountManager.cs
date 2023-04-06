namespace Benchmarker.MVVM.Model
{
    public static class AccountManager
    {
        private static User User;

        public static User GetUser()
        {
            return User;
        }

        public static void SetUser(User user)
        {
            User = user;
        }
        
        public static bool IsUserLoggedIn()
        {
            return User != null;
        }
    }
}
