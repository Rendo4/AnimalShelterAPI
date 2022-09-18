namespace Shelter.Models
{
    public class Animal
    {
        public int AnimalId { get; set; }
        public string Name { get; set; }
        public string Species { get; set; }
        public string Breed { get; set; }
        public int Age { get; set; }
        public string Gender { get; set; }
        public string About { get; set; }
        public string Aggression { get; set; }
        public bool Vaccinated { get; set; }
        public bool Spayed { get; set; }
        public bool HouseTrained { get; set; }
        public string Image { get; set; }
        public int Fee { get; set; }
    }
}