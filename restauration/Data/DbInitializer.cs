using Microsoft.EntityFrameworkCore;
using restauration.Models;

namespace restauration.Data
{
    public static class DbInitializer
    {
        public static void Initialize(RestaurantDbContext context)
        {
            // Assure que la base de données existe
            context.Database.EnsureCreated();

            // Vérifie si des données existent déjà
            if (context.Categories.Any())
            {
                return; // Base déjà initialisée
            }

            // 1. Catégories
            var catEntrees = new Category { Name = "Entrées", Description = "Pour bien commencer votre repas", DisplayOrder = 1 };
            var catPlats = new Category { Name = "Plats Principaux", Description = "Spécialités du Chef et viandes/poissons raffinés", DisplayOrder = 2 };
            var catDesserts = new Category { Name = "Desserts", Description = "Douceurs sucrées et créations maison", DisplayOrder = 3 };
            var catBoissons = new Category { Name = "Boissons & Vins", Description = "Sélection de vins, cocktails et rafraîchissements", DisplayOrder = 4 };

            context.Categories.AddRange(catEntrees, catPlats, catDesserts, catBoissons);
            context.SaveChanges();

            // 2. Plats & Menus
            var items = new List<MenuItem>
            {
                // Entrées
                new MenuItem
                {
                    Name = "Salade César Gourmande",
                    Description = "Filet de poulet croustillant, copeaux de parmesan, croûtons dorés et sauce césar maison.",
                    Price = 12.50m,
                    CategoryId = catEntrees.Id,
                    IsAvailable = true,
                    PreparationTimeMinutes = 10,
                    ImageUrl = "https://images.unsplash.com/photo-1546793665-c74683f339c1?w=600"
                },
                new MenuItem
                {
                    Name = "Carpaccio de Bœuf Charolais",
                    Description = "Fines tranches de bœuf, roquette, câpres, huile d'olive vierge et copeaux de parmesan.",
                    Price = 14.00m,
                    CategoryId = catEntrees.Id,
                    IsAvailable = true,
                    PreparationTimeMinutes = 10,
                    ImageUrl = "https://images.unsplash.com/photo-1544025162-d76694265947?w=600"
                },
                new MenuItem
                {
                    Name = "Soupe à l'Oignon Traditionnelle",
                    Description = "Gratinée à l'emmental affiné et tranches de pain de campagne aillées.",
                    Price = 9.50m,
                    CategoryId = catEntrees.Id,
                    IsAvailable = true,
                    PreparationTimeMinutes = 15,
                    ImageUrl = "https://images.unsplash.com/photo-1547592166-23ac45744acd?w=600"
                },

                // Plats
                new MenuItem
                {
                    Name = "Entrecôte Grillée (300g)",
                    Description = "Viande maturée servie avec frites croustillantes maison et sauce au poivre vert de Madagascar.",
                    Price = 24.50m,
                    CategoryId = catPlats.Id,
                    IsAvailable = true,
                    PreparationTimeMinutes = 20,
                    ImageUrl = "https://images.unsplash.com/photo-1558030006-450675393462?w=600"
                },
                new MenuItem
                {
                    Name = "Pavé de Saumon Rôti",
                    Description = "Saumon label rouge, émulsion au citron confit, purée de patates douces et légumes glacés.",
                    Price = 21.00m,
                    CategoryId = catPlats.Id,
                    IsAvailable = true,
                    PreparationTimeMinutes = 18,
                    ImageUrl = "https://images.unsplash.com/photo-1467003909585-2f8a72700288?w=600"
                },
                new MenuItem
                {
                    Name = "Burger Gourmet du Chef",
                    Description = "Steak haché façon bouchère, cheddar affiné, compotée d'oignons rouges, bacon croustillant.",
                    Price = 18.50m,
                    CategoryId = catPlats.Id,
                    IsAvailable = true,
                    PreparationTimeMinutes = 15,
                    ImageUrl = "https://images.unsplash.com/photo-1568901346375-23c9450c58cd?w=600"
                },
                new MenuItem
                {
                    Name = "Risotto Crémeux aux Cèpes",
                    Description = "Riz Arborio fondant, poêlée de champignons des sous-bois, parmesan 24 mois et truffe d'été.",
                    Price = 19.00m,
                    CategoryId = catPlats.Id,
                    IsAvailable = true,
                    PreparationTimeMinutes = 20,
                    ImageUrl = "https://images.unsplash.com/photo-1633964913295-ceb43826e7c9?w=600"
                },

                // Desserts
                new MenuItem
                {
                    Name = "Fondant au Chocolat Grand Cru",
                    Description = "Cœur coulant au chocolat noir 70%, boule de glace vanille Bourbon de Madagascar.",
                    Price = 8.50m,
                    CategoryId = catDesserts.Id,
                    IsAvailable = true,
                    PreparationTimeMinutes = 10,
                    ImageUrl = "https://images.unsplash.com/photo-1606313564200-e75d5e30476c?w=600"
                },
                new MenuItem
                {
                    Name = "Tiramisu Traditionnel Italien",
                    Description = "Biscuits cuillère imbibés d'expresso corsé et amaretto, mascarpone onctueux et cacao amer.",
                    Price = 8.00m,
                    CategoryId = catDesserts.Id,
                    IsAvailable = true,
                    PreparationTimeMinutes = 5,
                    ImageUrl = "https://images.unsplash.com/photo-1571877227200-a0d98ea607e9?w=600"
                },
                new MenuItem
                {
                    Name = "Tarte Tatin Caramélisée",
                    Description = "Pommes rôties au beurre demi-sel et caramel doré, crème fraîche d'Isigny.",
                    Price = 8.50m,
                    CategoryId = catDesserts.Id,
                    IsAvailable = true,
                    PreparationTimeMinutes = 8,
                    ImageUrl = "https://images.unsplash.com/photo-1519915028121-7d3463d20b13?w=600"
                },

                // Boissons
                new MenuItem
                {
                    Name = "Cocktail Signature Le Mirage",
                    Description = "Gin artisanal, liqueur de sureau, jus de citron vert frais, purée de framboise et prosecco.",
                    Price = 11.00m,
                    CategoryId = catBoissons.Id,
                    IsAvailable = true,
                    PreparationTimeMinutes = 5,
                    ImageUrl = "https://images.unsplash.com/photo-1514362545857-3bc16c4c7d1b?w=600"
                },
                new MenuItem
                {
                    Name = "Château Saint-Émilion Grand Cru",
                    Description = "Bouteille 75cl - Vin rouge équilibré, arômes de fruits noirs et tanins soyeux.",
                    Price = 38.00m,
                    CategoryId = catBoissons.Id,
                    IsAvailable = true,
                    PreparationTimeMinutes = 3,
                    ImageUrl = "https://images.unsplash.com/photo-1510812431401-41d2bd2722f3?w=600"
                },
                new MenuItem
                {
                    Name = "Limonade Artisanale Menthe & Citron",
                    Description = "Boisson rafraîchissante pressée minute aux agrumes bios et feuilles de menthe fraîche.",
                    Price = 5.00m,
                    CategoryId = catBoissons.Id,
                    IsAvailable = true,
                    PreparationTimeMinutes = 4,
                    ImageUrl = "https://images.unsplash.com/photo-1513558161293-cdaf765ed2fd?w=600"
                }
            };

            context.MenuItems.AddRange(items);
            context.SaveChanges();

            // 3. Tables
            var tables = new List<RestaurantTable>
            {
                new RestaurantTable { TableNumber = 1, Capacity = 2, Status = TableStatus.Disponible, Location = "Salle principale" },
                new RestaurantTable { TableNumber = 2, Capacity = 2, Status = TableStatus.Occupee, Location = "Salle principale" },
                new RestaurantTable { TableNumber = 3, Capacity = 4, Status = TableStatus.Occupee, Location = "Salle principale" },
                new RestaurantTable { TableNumber = 4, Capacity = 4, Status = TableStatus.Reservee, Location = "Salle principale" },
                new RestaurantTable { TableNumber = 5, Capacity = 6, Status = TableStatus.Disponible, Location = "Salle principale" },
                new RestaurantTable { TableNumber = 6, Capacity = 2, Status = TableStatus.Disponible, Location = "Terrasse" },
                new RestaurantTable { TableNumber = 7, Capacity = 4, Status = TableStatus.Disponible, Location = "Terrasse" },
                new RestaurantTable { TableNumber = 8, Capacity = 8, Status = TableStatus.Disponible, Location = "Salon VIP" }
            };

            context.RestaurantTables.AddRange(tables);
            context.SaveChanges();

            // 4. Réservations d'exemple
            var table4 = tables.First(t => t.TableNumber == 4);
            var reservations = new List<Reservation>
            {
                new Reservation
                {
                    CustomerName = "Claire Dubois",
                    CustomerPhone = "06 12 34 56 78",
                    CustomerEmail = "claire.dubois@example.com",
                    ReservationDate = DateTime.Today.AddHours(20),
                    NumberOfGuests = 4,
                    RestaurantTableId = table4.Id,
                    Status = ReservationStatus.Confirmee,
                    Notes = "Anniversaire - table calme souhaitée",
                    CreatedAt = DateTime.Now.AddDays(-1)
                },
                new Reservation
                {
                    CustomerName = "Marc Lefebvre",
                    CustomerPhone = "07 98 76 54 32",
                    CustomerEmail = "marc.l@example.com",
                    ReservationDate = DateTime.Today.AddDays(1).AddHours(12).AddMinutes(30),
                    NumberOfGuests = 2,
                    RestaurantTableId = null,
                    Status = ReservationStatus.EnAttente,
                    Notes = "Près de la baie vitrée si possible",
                    CreatedAt = DateTime.Now.AddHours(-3)
                }
            };

            context.Reservations.AddRange(reservations);
            context.SaveChanges();

            // 5. Commandes d'exemple pour le Dashboard et la Cuisine
            var table2 = tables.First(t => t.TableNumber == 2);
            var table3 = tables.First(t => t.TableNumber == 3);

            var itemEntrecote = items.First(i => i.Name.Contains("Entrecôte"));
            var itemSaumon = items.First(i => i.Name.Contains("Saumon"));
            var itemSalade = items.First(i => i.Name.Contains("Salade"));
            var itemBurger = items.First(i => i.Name.Contains("Burger"));
            var itemVin = items.First(i => i.Name.Contains("Saint-Émilion"));
            var itemTiramisu = items.First(i => i.Name.Contains("Tiramisu"));

            var order1 = new Order
            {
                OrderNumber = "CMD-001",
                RestaurantTableId = table2.Id,
                CustomerName = "Table 2",
                OrderType = OrderType.SurPlace,
                Status = OrderStatus.EnPreparation,
                CreatedAt = DateTime.Now.AddMinutes(-25),
                PaymentMethod = null,
                Notes = "Entrecôte saignante s'il vous plaît",
                OrderItems = new List<OrderItem>
                {
                    new OrderItem { MenuItemId = itemEntrecote.Id, Quantity = 1, UnitPrice = itemEntrecote.Price, Notes = "Cuisson saignante" },
                    new OrderItem { MenuItemId = itemSalade.Id, Quantity = 1, UnitPrice = itemSalade.Price },
                    new OrderItem { MenuItemId = itemVin.Id, Quantity = 1, UnitPrice = itemVin.Price }
                }
            };
            order1.TotalAmount = order1.OrderItems.Sum(i => i.Quantity * i.UnitPrice);

            var order2 = new Order
            {
                OrderNumber = "CMD-002",
                RestaurantTableId = table3.Id,
                CustomerName = "Table 3",
                OrderType = OrderType.SurPlace,
                Status = OrderStatus.Servie,
                CreatedAt = DateTime.Now.AddMinutes(-50),
                PaymentMethod = null,
                OrderItems = new List<OrderItem>
                {
                    new OrderItem { MenuItemId = itemBurger.Id, Quantity = 2, UnitPrice = itemBurger.Price },
                    new OrderItem { MenuItemId = itemSaumon.Id, Quantity = 1, UnitPrice = itemSaumon.Price },
                    new OrderItem { MenuItemId = itemTiramisu.Id, Quantity = 2, UnitPrice = itemTiramisu.Price }
                }
            };
            order2.TotalAmount = order2.OrderItems.Sum(i => i.Quantity * i.UnitPrice);

            var order3 = new Order
            {
                OrderNumber = "CMD-003",
                RestaurantTableId = null,
                CustomerName = "Julien Dupuis",
                OrderType = OrderType.AEmporter,
                Status = OrderStatus.Payee,
                CreatedAt = DateTime.Now.AddHours(-2),
                PaymentMethod = "Carte Bancaire",
                OrderItems = new List<OrderItem>
                {
                    new OrderItem { MenuItemId = itemBurger.Id, Quantity = 1, UnitPrice = itemBurger.Price },
                    new OrderItem { MenuItemId = itemTiramisu.Id, Quantity = 1, UnitPrice = itemTiramisu.Price }
                }
            };
            order3.TotalAmount = order3.OrderItems.Sum(i => i.Quantity * i.UnitPrice);

            context.Orders.AddRange(order1, order2, order3);
            context.SaveChanges();

            // 6. Inventaire / Stocks
            var stocks = new List<InventoryItem>
            {
                new InventoryItem { Name = "Filet de bœuf Charolais", Quantity = 14.5m, Unit = "kg", MinAlertQuantity = 5.0m, LastRestocked = DateTime.Now.AddDays(-2) },
                new InventoryItem { Name = "Pavés de Saumon frais", Quantity = 3.2m, Unit = "kg", MinAlertQuantity = 4.0m, LastRestocked = DateTime.Now.AddDays(-3) }, // En alerte!
                new InventoryItem { Name = "Fromage Parmesan AOP", Quantity = 8.0m, Unit = "kg", MinAlertQuantity = 2.0m, LastRestocked = DateTime.Now.AddDays(-5) },
                new InventoryItem { Name = "Café en grains Torréfié", Quantity = 2.0m, Unit = "kg", MinAlertQuantity = 3.0m, LastRestocked = DateTime.Now.AddDays(-7) }, // En alerte!
                new InventoryItem { Name = "Vin Saint-Émilion Grand Cru", Quantity = 24.0m, Unit = "bouteilles", MinAlertQuantity = 6.0m, LastRestocked = DateTime.Now.AddDays(-4) },
                new InventoryItem { Name = "Crème fraîche liquide 35%", Quantity = 12.0m, Unit = "litres", MinAlertQuantity = 5.0m, LastRestocked = DateTime.Now.AddDays(-1) }
            };

            context.InventoryItems.AddRange(stocks);
            context.SaveChanges();
        }
    }
}

