using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Android.Gms.Extensions;
using Android.Runtime;
using CarApp.Abstractions;
using CarApp.Droid.Implementations;
using CarApp.Models;
using Firebase;
using Firebase.Firestore;
using Java.Util;
using IEventListener = Firebase.Firestore.IEventListener;

[assembly: Xamarin.Forms.Dependency(typeof(AndroidUserDataService))]

namespace CarApp.Droid.Implementations
{
    public class AndroidUserDataService : IUserDataService
    {
        private readonly CollectionReference _usersCollection = FirebaseFirestore.Instance.Collection("users");
        private IListenerRegistration? _listenerRegistration;

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
            set
            {
                if (value != _uid)
                {
                    _listenerRegistration?.Remove();
                    _listenerRegistration = _usersCollection.Document(value).AddSnapshotListener(new EventListener(
                        (snapshot, error) =>
                        {
                            if (error != null)
                            {
                                System.Diagnostics.Debug.WriteLine(error.Message);
                                return;
                            }

                            if (snapshot != null)
                            {
                                try
                                {
                                    Console.WriteLine("abra");
                                    var userData = UserDataFromSnapshot(snapshot);
                                    var userDataType = userData.GetType();
                                    var properties = userDataType.GetProperties();
                                    foreach (var property in properties)
                                    {
                                        if (property.GetValue(userData) != property.GetValue(UserData))
                                        {
                                            property.SetValue(UserData, property.GetValue(userData));
                                        }
                                    }

                                    Console.WriteLine("cadabra");
                                }
                                catch (Exception e)
                                {
                                    Console.WriteLine(e.Message);
                                }
                            }
                        }));
                    _uid = value;
                }
            }
        }

        public UserData UserData { get; } = new UserData();

        private UserData UserDataFromSnapshot(DocumentSnapshot snapshot)
        {
            var data = snapshot.Data;
            if (data == null) throw new Exception("Snapshot data is null");
            
            var favCars =
                (data["favouriteCars"] as JavaList)?.ToArray().Select(s => s.ToString())
                .ToList() ?? throw new ArgumentNullException();


            Console.WriteLine(data["gender"].ToString());

            long javaDateAsMilliseconds = (data["birthday"] as Timestamp).Seconds * 1000;
            var dateTime =
                new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc).Add(
                    TimeSpan.FromMilliseconds(javaDateAsMilliseconds));

            return new UserData
            {
                Uid = Uid,
                Firstname = data["firstname"].ToString(),
                Lastname = data["lastname"].ToString(),
                Birthday = dateTime,
                Gender = data["gender"].ToString(),
                Address = data["address"].ToString(),
                Phone = data["phone"].ToString(),
                CarCountry = data["carCountry"].ToString(),
                CarBody = data["carBody"].ToString() == "" ? "Нет предпочтений" : data["carBody"].ToString(),
                CarDrive = data["carDrive"].ToString() == "" ? "Нет предпочтений" : data["carDrive"].ToString(),
                Transmission = data["transmission"].ToString() == ""
                    ? "Нет предпочтений"
                    : data["transmission"].ToString(),
                FavCars = favCars
            };
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

            user.Put(new Java.Lang.String("firstname"), new Java.Lang.String(""));
            user.Put(new Java.Lang.String("lastname"), new Java.Lang.String(""));
            user.Put(new Java.Lang.String("birthday"), new Date());
            user.Put(new Java.Lang.String("gender"), new Java.Lang.String("Мужчина"));
            user.Put(new Java.Lang.String("address"), new Java.Lang.String(""));
            user.Put(new Java.Lang.String("phone"), new Java.Lang.String(""));
            user.Put(new Java.Lang.String("carCountry"), new Java.Lang.String("DE"));
            user.Put(new Java.Lang.String("carBody"), new Java.Lang.String(""));
            user.Put(new Java.Lang.String("carDrive"), new Java.Lang.String(""));
            user.Put(new Java.Lang.String("transmission"), new Java.Lang.String(""));
            user.Put(new Java.Lang.String("favouriteCars"), new ArrayList());

            await _usersCollection.Document(Uid).Set(user);
        }

        public async Task UpdateUserDataAsync(string firstname, string lastname, DateTime birthday, string gender,
            string address, string phone, string carCountry, string carBody, string carDrive, string transmission)
        {
            long dateTimeUtcAsMilliseconds = (long)birthday.ToUniversalTime().Subtract(
                new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)
            ).TotalMilliseconds;
            var userUpdate = new Dictionary<string, Java.Lang.Object>
            {
                { "firstname", firstname },
                { "lastname", lastname },
                { "birthday", new Date(dateTimeUtcAsMilliseconds) },
                { "gender", gender },
                { "address", address },
                { "phone", phone },
                { "carCountry", carCountry },
                { "carBody", carBody == "Нет предпочтений" ? "" : carBody },
                { "carDrive", carDrive == "Нет предпочтений" ? "" : carDrive },
                { "transmission", transmission == "Нет предпочтений" ? "" : transmission }
            };

            await _usersCollection.Document(Uid).Update(userUpdate);
        }

        public async Task UpdateUserFavCarsAsync(List<string> favCars)
        {
            Console.WriteLine("Update fav cars start");
            var favCarsList = new ArrayList();
            foreach (var car in favCars)
            {
                Console.WriteLine(car);
                favCarsList.Add(car);
            }

            Console.WriteLine("Update fav cars middle");

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