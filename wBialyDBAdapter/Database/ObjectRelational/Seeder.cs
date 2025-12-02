using Microsoft.EntityFrameworkCore;
using wBialyDBAdapter.Database.ObjectRelational.Entities;

namespace wBialyDBAdapter.Database.ObjectRelational
{
    public static class ModelBuilderExtensions
    {
        public static void Seed(this ModelBuilder modelBuilder)
        {

            var tagEventMusic = new Tag_Event
            {
                TagID = 1,
                Name = "Music",
                EventID = 0
            };

            var tagEventSport = new Tag_Event
            {
                TagID = 2,
                Name = "Sport",
                EventID = 0
            };

            var tagGastroPizza = new Tag_Gastro
            {
                TagID = 3,
                Name = "Pizza",
                GastroID = 0
            };

            var tagGastroVegan = new Tag_Gastro
            {
                TagID = 4,
                Name = "Vegan",
                GastroID = 0
            };

            modelBuilder.Entity<Tag_Event>().HasData(tagEventMusic, tagEventSport);
            modelBuilder.Entity<Tag_Gastro>().HasData(tagGastroPizza, tagGastroVegan);

            var event1 = new Event
            {
                PostId = 1,
                Title = "Rock Festival",
                Description = "Największy festiwal rockowy w mieście.",
                AddDate = new DateTime(2025, 1, 1),
                Place = "Białystok Arena",
                Author = "Admin",
                EventDate = new DateTime(2025, 2, 1),
                Link = "https://event.com/rock"
            };

            var event2 = new Event
            {
                PostId = 2,
                Title = "Mecz siatkówki",
                Description = "Turniej siatkówki amatorskiej.",
                AddDate = new DateTime(2025, 1, 2),
                Place = "Hala Sportowa",
                Author = "Admin",
                EventDate = new DateTime(2025, 2, 5),
                Link = "https://event.com/sport"
            };

            modelBuilder.Entity<Event>().HasData(event1, event2);

            var gastro1 = new Gastro
            {
                PostId = 3,
                Title = "Pizza Day",
                Description = "Promocje na pizzę w całym mieście.",
                AddDate = new DateTime(2025, 1, 3),
                Place = "PizzaHouse",
                Author = "Admin",
                Day = new DateTime(2025, 1, 10),
                Link = "https://gastro.com/pizza"
            };

            var gastro2 = new Gastro
            {
                PostId = 4,
                Title = "Vegan Fest",
                Description = "Święto kuchni wegańskiej.",
                AddDate = new DateTime(2025, 1, 4),
                Place = "GreenFood",
                Author = "Admin",
                Day = new DateTime(2025, 1, 12),
                Link = "https://gastro.com/vegan"
            };

            modelBuilder.Entity<Gastro>().HasData(gastro1, gastro2);

            modelBuilder.Entity<Event>()
                .HasMany(e => e.EventTags)
                .WithMany(t => t.Events)
                .UsingEntity(j => j.HasData(
                    new { EventsPostId = 1, EventTagsTagID = 1 }, 
                    new { EventsPostId = 2, EventTagsTagID = 2 }  
                ));

            modelBuilder.Entity<Gastro>()
                .HasMany(g => g.GastroTags)
                .WithMany(t => t.Gastros)
                .UsingEntity(j => j.HasData(
                    new { GastrosPostId = 3, GastroTagsTagID = 3 },
                    new { GastrosPostId = 4, GastroTagsTagID = 4 }
                ));
        }
    }
}
