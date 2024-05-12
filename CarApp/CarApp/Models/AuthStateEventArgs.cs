using System;

namespace CarApp.Models
{
    public class AuthStateEventArgs :EventArgs
    {
        public AuthStateEventArgs(User user) => User = user;

        public User User { get; }
    }
}