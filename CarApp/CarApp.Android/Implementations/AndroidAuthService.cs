using System;
using System.Linq;
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
                var result = await _firebaseAuth.CreateUserWithEmailAndPasswordAsync(email, password);

                if (result.User.Uid is null)
                {
                    throw new ArgumentNullException();
                }
                
                var userDataService = new AndroidUserDataService
                {
                    Uid = result.User.Uid
                };
                await userDataService.CreateUserDataAsync();
                
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

        public override async Task DeleteAccount()
        {
            var user = FirebaseAuth.Instance.CurrentUser;
            var userDataService = new AndroidUserDataService { Uid = user.Uid };
            if (user != null)
            {
                try
                {
                    await user.DeleteAsync();
                }
                catch (FirebaseAuthRecentLoginRequiredException e)
                {
                    Console.WriteLine(e.Message);
                    await ReauthenticateAndDelete();
                }
                catch (FirebaseAuthException e)
                {
                    Console.WriteLine(e.Message);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }

            await userDataService.DeleteUserDataAsync();

        }
        private async Task ReauthenticateAndDelete()
        {
            try
            {
                var providerData = FirebaseAuth.Instance.CurrentUser.ProviderData.FirstOrDefault();

                if (providerData != null)
                {
                    if (providerData.ProviderId == GoogleAuthProvider.GetCredential(null, null).Provider)
                    {
                        await FirebaseAuth.Instance.CurrentUser.ReauthenticateAsync(GoogleAuthProvider.GetCredential(null, null));
                    }
                }

                await FirebaseAuth.Instance.CurrentUser.DeleteAsync();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }
    
}