using MongoDB.Driver;
using wBialyDBAdapter.Database.NoSQL.Entities;

namespace wBialyDBAdapter.Database.NoSQL
{
    public static class NoSQLSeeder
    {
        public static async Task SeedAsync(NoSQLDB db)
        {
            await SeedTagsAsync(db);
            await SeedEventsAsync(db);
            await SeedGastrosAsync(db);
        }

        private static async Task SeedTagsAsync(NoSQLDB db)
        {
            var existingTagsCount = await db.Tags.CountDocumentsAsync(FilterDefinition<Entities.Tag>.Empty);
            
            if (existingTagsCount > 0)
            {
                return;
            }

            var tags = new List<Entities.Tag>
            {
                new Entities.Tag { Name = "Music" },
                new Entities.Tag { Name = "Sport" },
                new Entities.Tag { Name = "Culture" },
                new Entities.Tag { Name = "Family" },
                new Entities.Tag { Name = "Outdoor" },
                new Entities.Tag { Name = "Education" },
                new Entities.Tag { Name = "Art" },
                new Entities.Tag { Name = "Technology" },
                new Entities.Tag { Name = "Pizza" },
                new Entities.Tag { Name = "Vegan" },
                new Entities.Tag { Name = "Asian" },
                new Entities.Tag { Name = "Italian" },
                new Entities.Tag { Name = "Dessert" },
                new Entities.Tag { Name = "Healthy" },
                new Entities.Tag { Name = "FastFood" },
                new Entities.Tag { Name = "Seafood" }
            };

            await db.Tags.InsertManyAsync(tags);
        }

        private static async Task SeedEventsAsync(NoSQLDB db)
        {
            var existingEventsCount = await db.Events.CountDocumentsAsync(FilterDefinition<Event>.Empty);
            
            if (existingEventsCount > 0)
            {
                return;
            }

            var musicTag = await GetTagByNameAsync(db, "Music");
            var sportTag = await GetTagByNameAsync(db, "Sport");
            var artTag = await GetTagByNameAsync(db, "Art");
            var familyTag = await GetTagByNameAsync(db, "Family");
            var techTag = await GetTagByNameAsync(db, "Technology");

            var events = new List<Event>
            {
                new Event
                {
                    Title = "Rock Festival",
                    Description = "Najwiêkszy festiwal rockowy w mieœcie.",
                    AddDate = new DateTime(2025, 1, 1),
                    Place = "Bia³ystok Arena",
                    Author = "Admin",
                    EventDate = new DateTime(2025, 2, 1),
                    Link = "https://event.com/rock",
                    Tags = musicTag != null ? new List<Entities.Tag> { musicTag } : new List<Entities.Tag>()
                },
                new Event
                {
                    Title = "Mecz siatkówki",
                    Description = "Turniej siatkówki amatorskiej.",
                    AddDate = new DateTime(2025, 1, 2),
                    Place = "Hala Sportowa",
                    Author = "Admin",
                    EventDate = new DateTime(2025, 2, 5),
                    Link = "https://event.com/sport",
                    Tags = sportTag != null ? new List<Entities.Tag> { sportTag } : new List<Entities.Tag>()
                },
                new Event
                {
                    Title = "Wystawa sztuki wspó³czesnej",
                    Description = "Prezentacja dzie³ lokalnych artystów.",
                    AddDate = new DateTime(2025, 1, 3),
                    Place = "Galeria Arsena³",
                    Author = "Curator",
                    EventDate = new DateTime(2025, 2, 10),
                    Link = "https://event.com/art",
                    Tags = artTag != null ? new List<Entities.Tag> { artTag } : new List<Entities.Tag>()
                },
                new Event
                {
                    Title = "Piknik rodzinny w parku",
                    Description = "Dzieñ pe³en zabaw dla ca³ej rodziny.",
                    AddDate = new DateTime(2025, 1, 4),
                    Place = "Park Planty",
                    Author = "Admin",
                    EventDate = new DateTime(2025, 2, 15),
                    Link = "https://event.com/family",
                    Tags = familyTag != null ? new List<Entities.Tag> { familyTag } : new List<Entities.Tag>()
                },
                new Event
                {
                    Title = "Tech Conference 2025",
                    Description = "Konferencja dla programistów i entuzjastów technologii.",
                    AddDate = new DateTime(2025, 1, 5),
                    Place = "Centrum Konferencyjne",
                    Author = "TechOrg",
                    EventDate = new DateTime(2025, 2, 20),
                    Link = "https://event.com/tech",
                    Tags = techTag != null ? new List<Entities.Tag> { techTag } : new List<Entities.Tag>()
                }
            };

            await db.Events.InsertManyAsync(events);
        }

        private static async Task SeedGastrosAsync(NoSQLDB db)
        {
            var existingGastrosCount = await db.Gastros.CountDocumentsAsync(FilterDefinition<Gastro>.Empty);
            
            if (existingGastrosCount > 0)
            {
                return;
            }

            var pizzaTag = await GetTagByNameAsync(db, "Pizza");
            var veganTag = await GetTagByNameAsync(db, "Vegan");
            var asianTag = await GetTagByNameAsync(db, "Asian");
            var seafoodTag = await GetTagByNameAsync(db, "Seafood");
            var dessertTag = await GetTagByNameAsync(db, "Dessert");

            var gastros = new List<Gastro>
            {
                new Gastro
                {
                    Title = "Pizza Day",
                    Description = "Promocje na pizzê w ca³ym mieœcie.",
                    AddDate = new DateTime(2025, 1, 3),
                    Place = "PizzaHouse",
                    Author = "Admin",
                    Day = new DateTime(2025, 1, 10),
                    Link = "https://gastro.com/pizza",
                    Tags = pizzaTag != null ? new List<Entities.Tag> { pizzaTag } : new List<Entities.Tag>()
                },
                new Gastro
                {
                    Title = "Vegan Fest",
                    Description = "Œwiêto kuchni wegañskiej.",
                    AddDate = new DateTime(2025, 1, 4),
                    Place = "GreenFood",
                    Author = "Admin",
                    Day = new DateTime(2025, 1, 12),
                    Link = "https://gastro.com/vegan",
                    Tags = veganTag != null ? new List<Entities.Tag> { veganTag } : new List<Entities.Tag>()
                },
                new Gastro
                {
                    Title = "Asian Food Week",
                    Description = "Tydzieñ kuchni azjatyckiej z degustacjami.",
                    AddDate = new DateTime(2025, 1, 5),
                    Place = "Asia Restaurant",
                    Author = "Chef Wang",
                    Day = new DateTime(2025, 1, 15),
                    Link = "https://gastro.com/asian",
                    Tags = asianTag != null ? new List<Entities.Tag> { asianTag } : new List<Entities.Tag>()
                },
                new Gastro
                {
                    Title = "Seafood Fiesta",
                    Description = "Œwie¿e owoce morza prosto z wybrze¿a.",
                    AddDate = new DateTime(2025, 1, 6),
                    Place = "Ocean Bistro",
                    Author = "Admin",
                    Day = new DateTime(2025, 1, 18),
                    Link = "https://gastro.com/seafood",
                    Tags = seafoodTag != null ? new List<Entities.Tag> { seafoodTag } : new List<Entities.Tag>()
                },
                new Gastro
                {
                    Title = "Dessert Paradise",
                    Description = "Najlepsze desery w mieœcie w jednym miejscu.",
                    AddDate = new DateTime(2025, 1, 7),
                    Place = "Sweet Corner",
                    Author = "Pastry Chef",
                    Day = new DateTime(2025, 1, 20),
                    Link = "https://gastro.com/dessert",
                    Tags = dessertTag != null ? new List<Entities.Tag> { dessertTag } : new List<Entities.Tag>()
                }
            };

            await db.Gastros.InsertManyAsync(gastros);
        }

        private static async Task<Entities.Tag?> GetTagByNameAsync(NoSQLDB db, string tagName)
        {
            var filter = Builders<Entities.Tag>.Filter.Eq(t => t.Name, tagName);
            return await db.Tags.Find(filter).FirstOrDefaultAsync();
        }
    }
}
