using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;


namespace Shelter.Models
{
    public class ShelterContext : IdentityDbContext
    {
        public ShelterContext(DbContextOptions<ShelterContext> options)
            : base(options)
        {
        }

        public DbSet<Animal> Animals { get; set; }


        protected override void OnModelCreating(ModelBuilder builder)
        {
          base.OnModelCreating(builder);
          builder.Entity<Animal>()
            .HasData(new Animal {
              AnimalId = 3,
              Name = "Frank",
              Species = "Dog",
              Breed = "Pitbull Terrier",
              Age = 28,
              Gender = "Male",
              About = "Frank (Frankey) – is our resident clown and a favorite of many of the staff; there isn’t a photo that completely captures all of his personality. He is a happy, gregarious 2ish little pitty ready to be someone’s best friend and partner.",
              Aggression = "friendly",
              Vaccinated = true,
              Spayed = true,
              HouseTrained = true,
              Image = "https://4152bg3zv88l2xvzsx4fs0md-wpengine.netdna-ssl.com/wp-content/uploads/2022/03/Frank-2-700x700.jpg",
              Fee = 300
            },
            new Animal {
              AnimalId = 4,
              Name = "Cherry",
              Species = "Dog",
              Breed = "Shar Pei Mix",
              Age = 4,
              Gender = "Female",
              About = "If you are interested in meeting me, please email Newberg Animal Shelter at frontdesk@newberganimals.com",
              Aggression = "good with dogs",
              Vaccinated = true,
              Spayed = true,
              HouseTrained = false,
              Image = "https://4152bg3zv88l2xvzsx4fs0md-wpengine.netdna-ssl.com/wp-content/uploads/2022/06/cherry3-700x933.jpeg",
              Fee = 600
            }
            );
        }
    }
}