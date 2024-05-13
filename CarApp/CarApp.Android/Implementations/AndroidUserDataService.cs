using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Android.Gms.Extensions;
using Android.Runtime;
using CarApp.Abstractions;
using CarApp.Droid.Implementations;
using CarApp.Models;
using Firebase.Firestore;
using Java.Util;
using IEventListener = Firebase.Firestore.IEventListener;
using Observable = System.Reactive.Linq.Observable;

[assembly: Xamarin.Forms.Dependency(typeof(AndroidUserDataService))]

namespace CarApp.Droid.Implementations
{
    public class AndroidUserDataService : IUserDataService
    {
        private readonly CollectionReference _usersCollection = FirebaseFirestore.Instance.Collection("users");

        private string? _uid;

        public string Uid
        {
            get
            {
                if (_uid is null)
                {
                    throw new InvalidOperationException();
                }

                return _uid;
            }
            set => _uid = value;
        }

        private UserData UserDataFromSnapshot(DocumentSnapshot snapshot)
        {
            var data = snapshot.Data;
            if (data == null) throw new Exception("Snapshot data is null");

            var favCars =
                (data["favouriteCars"] as JavaList)?.ToArray().OfType<Java.Lang.String>().Select(s => s.ToString())
                .ToList() ?? new List<string>();

            return new UserData
            {
                Uid = Uid,
                Firstname = data["firstname"].ToString(),
                Lastname = data["lastname"].ToString(),
                Birthday = Convert.ToDateTime(data["birthday"]),
                Gender = data["gender"].ToString(),
                Address = data["address"].ToString(),
                Phone = data["phone"].ToString(),
                CarCountry = data["carCountry"].ToString(),
                CarBody = data["carBody"].ToString(),
                CarDrive = data["carDrive"].ToString(),
                Transmission = data["transmission"].ToString(),
                FavCars = favCars
            };
        }

        IObservable<UserData> IUserDataService.GetUserData()
        {
            return Observable.Create<UserData>(observer =>
            {
                var listenerRegistration = _usersCollection.Document(Uid).AddSnapshotListener(new EventListener(
                    (snapshot, error) =>
                    {
                        if (error != null)
                        {
                            observer.OnError(new Exception("Firestore error: " + error.Message));
                            return;
                        }

                        if (snapshot != null && snapshot.Exists())
                        {
                            try
                            {
                                UserData userData = UserDataFromSnapshot(snapshot);
                                observer.OnNext(userData);
                            }
                            catch (Exception e)
                            {
                                observer.OnError(e);
                            }
                        }
                        else
                        {
                            observer.OnError(new Exception("No user data found"));
                        }
                    }));

                return System.Reactive.Disposables.Disposable.Create(() => listenerRegistration.Remove());
            });
        }

        private class EventListener : Java.Lang.Object, IEventListener
        {
            private readonly Action<DocumentSnapshot, FirebaseFirestoreException> _onEvent;

            public EventListener(Action<DocumentSnapshot, FirebaseFirestoreException> onEvent)
            {
                _onEvent = onEvent;
            }

            public void OnEvent(Java.Lang.Object value, FirebaseFirestoreException error)
            {
                var snapshot = value as DocumentSnapshot;
                _onEvent(snapshot, error);
            }
        }

        public async Task CreateUserDataAsync()
        {
            var user = new HashMap();
            user.Put("firstname", "");
            user.Put("lastname", "");
            user.Put("birthday", new Date(DateTime.Now.Millisecond));
            user.Put("gender", "Мужчина");
            user.Put("address", "");
            user.Put("phone", "");
            user.Put("carCountry", "DE");
            user.Put("carBody", "");
            user.Put("carDrive", "");
            user.Put("transmission", "");
            user.Put("favouriteCars", new ArrayList());

            await _usersCollection.Document(Uid).Set(user);
        }

        public async Task UpdateUserDataAsync(string firstname, string lastname, DateTime birthday, string gender,
            string address, string phone, string carCountry, string carBody, string carDrive, string transmission)
        {
            var userUpdate = new Dictionary<string, Java.Lang.Object>
            {
                { "firstname", firstname },
                { "lastname", lastname },
                { "birthday", new Date(birthday.Millisecond) },
                { "gender", gender },
                { "address", address },
                { "phone", phone },
                { "carCountry", carCountry },
                { "carBody", carBody },
                { "carDrive", carDrive },
                { "transmission", transmission }
            };

            await _usersCollection.Document(Uid).Update(userUpdate);
        }

        public async Task UpdateUserFavCarsAsync(List<string> favCars)
        {
            var favCarsList = new ArrayList();
            foreach (var car in favCars)
            {
                favCarsList.Add(car);
            }

            IDictionary<string, Java.Lang.Object> update = new Dictionary<string, Java.Lang.Object>
            {
                { "favouriteCars", favCarsList }
            };
            await _usersCollection.Document(Uid).Update(update);
        }

        public async Task DeleteUserDataAsync()
        {
            await _usersCollection.Document(Uid).Delete();
        }
    }
}