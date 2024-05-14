using System;
using System.Threading.Tasks;
using CarApp.Abstractions;
using CarApp.Droid.Implementations;
using CarApp.Models;
using Firebase.Auth;

[assembly: Xamarin.Forms.Dependency(typeof(AndroidAuthService))]

namespace CarApp.Droid.Implementations
{
    public class AndroidAuthService : AuthService
    {
        private readonly FirebaseAuth _firebaseAuth = FirebaseAuth.Instance;

        public AndroidAuthService()
        {
            _firebaseAuth.AuthState += (sender, args) =>
            {
                AuthState?.Invoke(sender, new AuthStateEventArgs(new User{Uid = _firebaseAuth.Uid}));
            };
        }
        
        public override User? CurrentUser => _firebaseAuth.CurrentUser?.Uid == null
            ? null
            : new User { Uid = _firebaseAuth.CurrentUser.Uid };
        

        public override async Task SignInWithEmailAndPassword(string email, string password)
        {
            try
            {
                await _firebaseAuth.SignInWithEmailAndPasswordAsync(email, password);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        public override async Task RegisterWithEmailAndPassword(string email, string password)
        {
            try
            {
                await _firebaseAuth.CreateUserWithEmailAndPasswordAsync(email, password);
                if (CurrentUser != null)
                {
                    var userDataService = new AndroidUserDataService { Uid = CurrentUser.Uid };
                    await userDataService.CreateUserDataAsync();
                }
                
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        public override Task<bool> SignOut()
        {
            try
            {
                _firebaseAuth.SignOut();
                return Task.FromResult(true);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return Task.FromResult(false);
            }
        }
    }
}