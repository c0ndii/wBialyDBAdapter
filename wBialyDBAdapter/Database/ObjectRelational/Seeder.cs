using Microsoft.EntityFrameworkCore;
using wBialyDBAdapter.Database.ObjectRelational.Entities;

namespace wBialyDBAdapter.Database.ObjectRelational
{
    public static class ModelBuilderExtensions
    {
        public static void Seed(this ModelBuilder modelBuilder)
        {
            var tagEventMusic = new Tag_Event { TagID = 1, Name = "Music", EventID = 0 };
            var tagEventSport = new Tag_Event { TagID = 2, Name = "Sport", EventID = 0 };
            var tagEventCulture = new Tag_Event { TagID = 3, Name = "Culture", EventID = 0 };
            var tagEventFamily = new Tag_Event { TagID = 4, Name = "Family", EventID = 0 };
            var tagEventOutdoor = new Tag_Event { TagID = 5, Name = "Outdoor", EventID = 0 };
            var tagEventEducation = new Tag_Event { TagID = 6, Name = "Education", EventID = 0 };
            var tagEventArt = new Tag_Event { TagID = 7, Name = "Art", EventID = 0 };
            var tagEventTechnology = new Tag_Event { TagID = 8, Name = "Technology", EventID = 0 };

            var tagGastroPizza = new Tag_Gastro { TagID = 9, Name = "Pizza", GastroID = 0 };
            var tagGastroVegan = new Tag_Gastro { TagID = 10, Name = "Vegan", GastroID = 0 };
            var tagGastroAsian = new Tag_Gastro { TagID = 11, Name = "Asian", GastroID = 0 };
            var tagGastroItalian = new Tag_Gastro { TagID = 12, Name = "Italian", GastroID = 0 };
            var tagGastroDessert = new Tag_Gastro { TagID = 13, Name = "Dessert", GastroID = 0 };
            var tagGastroHealthy = new Tag_Gastro { TagID = 14, Name = "Healthy", GastroID = 0 };
            var tagGastroFastFood = new Tag_Gastro { TagID = 15, Name = "FastFood", GastroID = 0 };
            var tagGastroSeafood = new Tag_Gastro { TagID = 16, Name = "Seafood", GastroID = 0 };

            modelBuilder.Entity<Tag_Event>().HasData(
                tagEventMusic, tagEventSport, tagEventCulture, tagEventFamily,
                tagEventOutdoor, tagEventEducation, tagEventArt, tagEventTechnology
            );

            modelBuilder.Entity<Tag_Gastro>().HasData(
                tagGastroPizza, tagGastroVegan, tagGastroAsian, tagGastroItalian,
                tagGastroDessert, tagGastroHealthy, tagGastroFastFood, tagGastroSeafood
            );

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

            var event3 = new Event
            {
                PostId = 3,
                Title = "Wystawa sztuki współczesnej",
                Description = "Prezentacja dzieł lokalnych artystów.",
                AddDate = new DateTime(2025, 1, 3),
                Place = "Galeria Arsenał",
                Author = "Curator",
                EventDate = new DateTime(2025, 2, 10),
                Link = "https://event.com/art"
            };

            var event4 = new Event
            {
                PostId = 4,
                Title = "Piknik rodzinny w parku",
                Description = "Dzień pełen zabaw dla całej rodziny.",
                AddDate = new DateTime(2025, 1, 4),
                Place = "Park Planty",
                Author = "Admin",
                EventDate = new DateTime(2025, 2, 15),
                Link = "https://event.com/family"
            };

            var event5 = new Event
            {
                PostId = 5,
                Title = "Tech Conference 2025",
                Description = "Konferencja dla programistów i entuzjastów technologii.",
                AddDate = new DateTime(2025, 1, 5),
                Place = "Centrum Konferencyjne",
                Author = "TechOrg",
                EventDate = new DateTime(2025, 2, 20),
                Link = "https://event.com/tech"
            };

            modelBuilder.Entity<Event>().HasData(event1, event2, event3, event4, event5);

            var gastro1 = new Gastro
            {
                PostId = 6,
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
                PostId = 7,
                Title = "Vegan Fest",
                Description = "Święto kuchni wegańskiej.",
                AddDate = new DateTime(2025, 1, 4),
                Place = "GreenFood",
                Author = "Admin",
                Day = new DateTime(2025, 1, 12),
                Link = "https://gastro.com/vegan"
            };

            var gastro3 = new Gastro
            {
                PostId = 8,
                Title = "Asian Food Week",
                Description = "Tydzień kuchni azjatyckiej z degustacjami.",
                AddDate = new DateTime(2025, 1, 5),
                Place = "Asia Restaurant",
                Author = "Chef Wang",
                Day = new DateTime(2025, 1, 15),
                Link = "https://gastro.com/asian"
            };

            var gastro4 = new Gastro
            {
                PostId = 9,
                Title = "Seafood Fiesta",
                Description = "Świeże owoce morza prosto z wybrzeża.",
                AddDate = new DateTime(2025, 1, 6),
                Place = "Ocean Bistro",
                Author = "Admin",
                Day = new DateTime(2025, 1, 18),
                Link = "https://gastro.com/seafood"
            };

            var gastro5 = new Gastro
            {
                PostId = 10,
                Title = "Dessert Paradise",
                Description = "Najlepsze desery w mieście w jednym miejscu.",
                AddDate = new DateTime(2025, 1, 7),
                Place = "Sweet Corner",
                Author = "Pastry Chef",
                Day = new DateTime(2025, 1, 20),
                Link = "https://gastro.com/dessert"
            };

            modelBuilder.Entity<Gastro>().HasData(gastro1, gastro2, gastro3, gastro4, gastro5);

            modelBuilder.Entity<Event>()
                .HasMany(e => e.EventTags)
                .WithMany(t => t.Events)
                .UsingEntity(j => j.HasData(
                    new { EventsPostId = 1, EventTagsTagID = 1 },
                    new { EventsPostId = 2, EventTagsTagID = 2 },
                    new { EventsPostId = 3, EventTagsTagID = 7 },
                    new { EventsPostId = 4, EventTagsTagID = 4 },
                    new { EventsPostId = 5, EventTagsTagID = 8 }
                ));

            modelBuilder.Entity<Gastro>()
                .HasMany(g => g.GastroTags)
                .WithMany(t => t.Gastros)
                .UsingEntity(j => j.HasData(
                    new { GastrosPostId = 6, GastroTagsTagID = 9 },
                    new { GastrosPostId = 7, GastroTagsTagID = 10 },
                    new { GastrosPostId = 8, GastroTagsTagID = 11 },
                    new { GastrosPostId = 9, GastroTagsTagID = 16 },
                    new { GastrosPostId = 10, GastroTagsTagID = 13 }
                ));
        }
    }
}
