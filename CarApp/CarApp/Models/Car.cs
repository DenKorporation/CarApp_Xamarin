namespace CarApp.Models
{
    public class Car
    {
        public string Uid { get; set; }
        public string Name { get; set; }
        public string Years { get; set; }
        public string Description { get; set; }
        public string Country { get; set; }
        public string Body { get; set; }
        public string Drive { get; set; }
        public string Transmission { get; set; }

        public bool IsFav { get; set; } = false;
    }
}