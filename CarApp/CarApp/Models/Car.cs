using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace CarApp.Models
{
    public class Car : INotifyPropertyChanged
    {
        private bool _isFav = false;
        public string Uid { get; set; }
        public string Name { get; set; }
        public string Years { get; set; }
        public string Description { get; set; }
        public string Country { get; set; }
        public string Body { get; set; }
        public string Drive { get; set; }
        public string Transmission { get; set; }

        public bool IsFav
        {
            get => _isFav;
            set
            {
                if (value == _isFav) return;
                _isFav = value;
                OnPropertyChanged();
            }
        }

        public override bool Equals(object obj)
        {
            if (obj is Car car)
            {
                return Equals(car);
            }

            return false;
        }

        private bool Equals(Car other)
        {
            return Uid == other.Uid && Name == other.Name && Years == other.Years && Description == other.Description &&
                   Country == other.Country && Body == other.Body && Drive == other.Drive &&
                   Transmission == other.Transmission;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}