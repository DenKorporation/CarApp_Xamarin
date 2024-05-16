using System;
using System.Threading.Tasks;
using CarApp.Models;

namespace CarApp.Abstractions
{
    public abstract class  AuthService
    {
        public abstract User CurrentUser { get; }
        public EventHandler<AuthStateEventArgs> AuthState;
        public abstract Task SignInWithEmailAndPassword(string email, string password);
        public abstract Task RegisterWithEmailAndPassword(string email, string password);
        public abstract Task<bool> SignOut();
        public abstract Task DeleteAccount();
    }
}