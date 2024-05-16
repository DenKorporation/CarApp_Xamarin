using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using CarApp.Abstractions;
using CarApp.Droid.Implementations;
using CarApp.Models;
using Firebase.Firestore;

[assembly: Xamarin.Forms.Dependency(typeof(AndroidCarsService))]

namespace CarApp.Droid.Implementations
{
    public class AndroidCarsService : ICarsService
    {
        private readonly CollectionReference _carsCollection = FirebaseFirestore.Instance.Collection("cars");
        
        public ObservableCollection<Car> Cars { get; } = new ObservableCollection<Car>();

        public AndroidCarsService()
        {
            _carsCollection.AddSnapshotListener(new EventListener((snapshot, error) =>
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
                        var cars = CarsListFromSnapshot(snapshot);

                        foreach (var car in cars)
                        {
                            var foundCar = Cars.FirstOrDefault(c => car.Uid == c.Uid);
                            if (foundCar != default)
                            {
                                if (!car.Equals(foundCar))
                                {
                                    Cars[Cars.IndexOf(foundCar)] = car;
                                }
                            }
                            else
                            {
                                Cars.Add(car);
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        System.Diagnostics.Debug.WriteLine(e.Message);
                    }
                }
            }));
        }
        
        private List<Car> CarsListFromSnapshot(QuerySnapshot snapshot)
        {
            var data = snapshot.Documents;
            
            if (data == null) throw new Exception("Snapshot data is null");
            
            return data.Select(CarFromSnapshot).ToList();
        }
        
        private Car CarFromSnapshot(DocumentSnapshot snapshot)
        {
            var data = snapshot.Data;
            if (data == null) throw new Exception("Snapshot data is null");
            
            return new Car
            {
                Uid = snapshot.Id,
                Name = data["name"].ToString(),
                Years = data["years"].ToString(),
                Description = data["description"].ToString(),
                Country = data["country"].ToString(),
                Body = data["body"].ToString(),
                Drive = data["drive"].ToString(),
                Transmission = data["transmission"].ToString()
            };
        }

        private class EventListener : Java.Lang.Object, IEventListener
        {
            private readonly Action<QuerySnapshot, FirebaseFirestoreException> _onEvent;

            public EventListener(Action<QuerySnapshot, FirebaseFirestoreException> onEvent)
            {
                _onEvent = onEvent;
            }

            public void OnEvent(Java.Lang.Object value, FirebaseFirestoreException error)
            {
                if (value is QuerySnapshot snapshot)
                {
                    _onEvent(snapshot, error);
                }
            }
        }
    }
}