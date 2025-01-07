using System;
using System.Linq;
using LostAndFound.API.Models;
using Microsoft.EntityFrameworkCore;
using BCrypt.Net;
using System.Collections.Generic;

namespace LostAndFound.API.Data
{
    public static class DbInitializer
    {
        private static readonly Dictionary<string, string[]> CategoryImages = new()
        {
            ["electronics"] = new[]
            {
                "https://images.unsplash.com/photo-1505740420928-5e560c06d30e",  // Casque
                "https://images.unsplash.com/photo-1585060544812-6b45742d762f",  // iPhone
                "https://images.unsplash.com/photo-1517336714731-489689fd1ca8",  // MacBook
                "https://images.unsplash.com/photo-1546868871-7041f2a55e12",  // Apple Watch
                "https://images.unsplash.com/photo-1592899677977-9c10ca588bbd",  // AirPods
            },
            ["documents"] = new[]
            {
                "https://images.unsplash.com/photo-1618005198919-d3d4b5a92ead",  // Passeport
                "https://images.unsplash.com/photo-1573739022854-abceaeb585dc",  // Cartes
                "https://images.unsplash.com/photo-1586281380349-632531db7ed4",  // Documents
                "https://images.unsplash.com/photo-1554907984-15263bfd63bd",     // Permis
                "https://images.unsplash.com/photo-1506784365847-bbad939e9335",  // Cahiers
            },
            ["accessories"] = new[]
            {
                "https://images.unsplash.com/photo-1523275335684-37898b6baf30",  // Montre
                "https://images.unsplash.com/photo-1572635196237-14b3f281503f",  // Lunettes
                "https://images.unsplash.com/photo-1553062407-98eeb64c6a62",     // Sac
                "https://images.unsplash.com/photo-1627123424574-724758594e93",  // Portefeuille
                "https://images.unsplash.com/photo-1622434641406-a158123450f9",  // Clés
            },
            ["clothing"] = new[]
            {
                "https://images.unsplash.com/photo-1591047139829-d91aecb6caea",  // Veste
                "https://images.unsplash.com/photo-1520006403909-838d6b92c22e",  // Écharpe
                "https://images.unsplash.com/photo-1588850561407-ed78c282e89b",  // Casquette
                "https://images.unsplash.com/photo-1578587018452-892bacefd3f2",  // Pull
                "https://images.unsplash.com/photo-1556306535-0f09a537f0a3",     // Gants
            }
        };

        private static readonly Random Random = new Random();

        private static string GetRandomImageForCategory(string category)
        {
            var images = CategoryImages[category.ToLower()];
            return images[Random.Next(images.Length)];
        }

        public static void Initialize(ApplicationDbContext context, bool force = false)
        {
            // Ensure database is created with all tables
            context.Database.EnsureCreated();

            try
            {
                // Clear existing data if force is true
                if (force)
                {
                    if (context.Messages.Any())
                        context.Messages.RemoveRange(context.Messages);
                    if (context.Demandes.Any())
                        context.Demandes.RemoveRange(context.Demandes);
                    if (context.Items.Any())
                        context.Items.RemoveRange(context.Items);
                    if (context.Users.Any())
                        context.Users.RemoveRange(context.Users);
                    context.SaveChanges();
                }

                // Add Users (1 Admin + 15 Users)
                var users = new List<User>
                {
                    // Admin
                    new User
                    {
                        Name = "Admin EMSI",
                        Email = "admin@emsi.ma",
                        Password = BCrypt.Net.BCrypt.HashPassword("Admin123!"),
                        Role = "Admin",
                        Status = "active",
                        LastLogin = DateTime.UtcNow,
                        CreatedAt = DateTime.UtcNow
                    },
                    // Regular Users
                    new User
                    {
                        Name = "Mohammed Alami",
                        Email = "m.alami@emsi.ma",
                        Password = BCrypt.Net.BCrypt.HashPassword("User123!"),
                        Role = "User",
                        Status = "active",
                        LastLogin = DateTime.UtcNow,
                        CreatedAt = DateTime.UtcNow
                    },
                    new User
                    {
                        Name = "Sara Bennis",
                        Email = "s.bennis@emsi.ma",
                        Password = BCrypt.Net.BCrypt.HashPassword("User123!"),
                        Role = "User",
                        Status = "active",
                        LastLogin = DateTime.UtcNow,
                        CreatedAt = DateTime.UtcNow
                    },
                    new User
                    {
                        Name = "Karim Idrissi",
                        Email = "k.idrissi@emsi.ma",
                        Password = BCrypt.Net.BCrypt.HashPassword("User123!"),
                        Role = "User",
                        Status = "inactive",
                        LastLogin = DateTime.UtcNow,
                        CreatedAt = DateTime.UtcNow
                    },
                    new User
                    {
                        Name = "Fatima Zahra",
                        Email = "f.zahra@emsi.ma",
                        Password = BCrypt.Net.BCrypt.HashPassword("User123!"),
                        Role = "User",
                        Status = "active",
                        LastLogin = DateTime.UtcNow,
                        CreatedAt = DateTime.UtcNow
                    },
                    new User
                    {
                        Name = "Youssef Mansouri",
                        Email = "y.mansouri@emsi.ma",
                        Password = BCrypt.Net.BCrypt.HashPassword("User123!"),
                        Role = "User",
                        Status = "active",
                        LastLogin = DateTime.UtcNow,
                        CreatedAt = DateTime.UtcNow
                    },
                    new User
                    {
                        Name = "Amina Tazi",
                        Email = "a.tazi@emsi.ma",
                        Password = BCrypt.Net.BCrypt.HashPassword("User123!"),
                        Role = "User",
                        Status = "active",
                        LastLogin = DateTime.UtcNow,
                        CreatedAt = DateTime.UtcNow
                    },
                    new User
                    {
                        Name = "Hassan Benjelloun",
                        Email = "h.benjelloun@emsi.ma",
                        Password = BCrypt.Net.BCrypt.HashPassword("User123!"),
                        Role = "User",
                        Status = "inactive",
                        LastLogin = DateTime.UtcNow,
                        CreatedAt = DateTime.UtcNow
                    },
                    new User
                    {
                        Name = "Leila Chakiri",
                        Email = "l.chakiri@emsi.ma",
                        Password = BCrypt.Net.BCrypt.HashPassword("User123!"),
                        Role = "User",
                        Status = "active",
                        LastLogin = DateTime.UtcNow,
                        CreatedAt = DateTime.UtcNow
                    },
                    new User
                    {
                        Name = "Omar Bennani",
                        Email = "o.bennani@emsi.ma",
                        Password = BCrypt.Net.BCrypt.HashPassword("User123!"),
                        Role = "User",
                        Status = "active",
                        LastLogin = DateTime.UtcNow,
                        CreatedAt = DateTime.UtcNow
                    },
                    new User
                    {
                        Name = "Salma Kabbaj",
                        Email = "s.kabbaj@emsi.ma",
                        Password = BCrypt.Net.BCrypt.HashPassword("User123!"),
                        Role = "User",
                        Status = "active",
                        LastLogin = DateTime.UtcNow,
                        CreatedAt = DateTime.UtcNow
                    },
                    new User
                    {
                        Name = "Rachid Moussaoui",
                        Email = "r.moussaoui@emsi.ma",
                        Password = BCrypt.Net.BCrypt.HashPassword("User123!"),
                        Role = "User",
                        Status = "inactive",
                        LastLogin = DateTime.UtcNow,
                        CreatedAt = DateTime.UtcNow
                    },
                    new User
                    {
                        Name = "Nadia Berrada",
                        Email = "n.berrada@emsi.ma",
                        Password = BCrypt.Net.BCrypt.HashPassword("User123!"),
                        Role = "User",
                        Status = "active",
                        LastLogin = DateTime.UtcNow,
                        CreatedAt = DateTime.UtcNow
                    },
                    new User
                    {
                        Name = "Mehdi Alaoui",
                        Email = "m.alaoui@emsi.ma",
                        Password = BCrypt.Net.BCrypt.HashPassword("User123!"),
                        Role = "User",
                        Status = "active",
                        LastLogin = DateTime.UtcNow,
                        CreatedAt = DateTime.UtcNow
                    },
                    new User
                    {
                        Name = "Houda Fassi",
                        Email = "h.fassi@emsi.ma",
                        Password = BCrypt.Net.BCrypt.HashPassword("User123!"),
                        Role = "User",
                        Status = "active",
                        LastLogin = DateTime.UtcNow,
                        CreatedAt = DateTime.UtcNow
                    },
                    new User
                    {
                        Name = "Amine Chraibi",
                        Email = "a.chraibi@emsi.ma",
                        Password = BCrypt.Net.BCrypt.HashPassword("User123!"),
                        Role = "User",
                        Status = "active",
                        LastLogin = DateTime.UtcNow,
                        CreatedAt = DateTime.UtcNow
                    }
                };

                context.Users.AddRange(users);
                context.SaveChanges();

                // Add Items (20 items)
                var items = new Item[]
                {
                    // Electronics
                    CreateItem("iPhone 14 Pro", "Bibliothèque", "lost", "electronics", users[1].Id),
                    CreateItem("MacBook Air M2", "Salle 205", "found", "electronics", users[0].Id),
                    CreateItem("AirPods Pro", "Cafétéria", "pending", "electronics", users[1].Id),
                    CreateItem("Samsung Galaxy Watch", "Salle de sport", "lost", "electronics", users[0].Id),
                    CreateItem("iPad Air", "Laboratoire info", "found", "electronics", users[1].Id),

                    // Documents
                    CreateItem("Carte d'étudiant EMSI", "Accueil", "lost", "documents", users[0].Id),
                    CreateItem("Passeport marocain", "Salle 304", "found", "documents", users[1].Id),
                    CreateItem("Carte CIN", "Parking", "pending", "documents", users[0].Id),
                    CreateItem("Permis de conduire", "Administration", "lost", "documents", users[1].Id),
                    CreateItem("Carnet de notes", "Amphithéâtre", "found", "documents", users[0].Id),

                    // Accessories
                    CreateItem("Montre Casio", "Vestiaire", "lost", "accessories", users[1].Id),
                    CreateItem("Lunettes Ray-Ban", "Bibliothèque", "found", "accessories", users[0].Id),
                    CreateItem("Sac à dos Nike", "Cafétéria", "pending", "accessories", users[1].Id),
                    CreateItem("Portefeuille cuir", "Salle 102", "lost", "accessories", users[0].Id),
                    CreateItem("Clés de voiture", "Parking", "found", "accessories", users[1].Id),

                    // Clothing
                    CreateItem("Veste EMSI", "Vestiaire", "lost", "clothing", users[0].Id),
                    CreateItem("Écharpe rouge", "Hall d'entrée", "found", "clothing", users[1].Id),
                    CreateItem("Casquette Nike", "Terrain sport", "pending", "clothing", users[0].Id),
                    CreateItem("Pull EMSI", "Salle 201", "lost", "clothing", users[1].Id),
                    CreateItem("Gants cuir", "Accueil", "found", "clothing", users[0].Id)
                };

                context.Items.AddRange(items);
                context.SaveChanges();

                // Add Demandes (3 demandes)
                var demandes = new Demande[]
                {
                    new Demande
                    {
                        ItemId = items[0].Id, // iPhone
                        RequestedByUserId = users[1].Id,
                        Status = "pending",
                        Message = "J'ai perdu mon iPhone dans la bibliothèque",
                        CreatedAt = DateTime.UtcNow.AddDays(-2),
                        UpdatedAt = DateTime.UtcNow.AddDays(-2)
                    },
                    new Demande
                    {
                        ItemId = items[1].Id, // MacBook
                        RequestedByUserId = users[0].Id,
                        Status = "approved",
                        Message = "J'ai trouvé ce MacBook dans la salle 205",
                        CreatedAt = DateTime.UtcNow.AddDays(-1),
                        UpdatedAt = DateTime.UtcNow.AddDays(-1)
                    },
                    new Demande
                    {
                        ItemId = items[2].Id, // AirPods
                        RequestedByUserId = users[1].Id,
                        Status = "rejected",
                        Message = "Je pense avoir trouvé vos AirPods",
                        CreatedAt = DateTime.UtcNow.AddHours(-12),
                        UpdatedAt = DateTime.UtcNow.AddHours(-12)
                    }
                };

                context.Demandes.AddRange(demandes);
                context.SaveChanges();

                // Add Messages (4 messages)
                var messages = new Message[]
                {
                    new Message
                    {
                        DemandeId = demandes[0].Id,
                        SenderId = users[1].Id,
                        ReceiverId = users[0].Id,
                        Content = "Bonjour, j'ai perdu mon iPhone hier à la bibliothèque",
                        CreatedAt = DateTime.UtcNow.AddHours(-24),
                        IsRead = true
                    },
                    new Message
                    {
                        DemandeId = demandes[0].Id,
                        SenderId = users[0].Id,
                        ReceiverId = users[1].Id,
                        Content = "Je vais vérifier les objets trouvés",
                        CreatedAt = DateTime.UtcNow.AddHours(-23),
                        IsRead = true
                    },
                    new Message
                    {
                        DemandeId = demandes[1].Id,
                        SenderId = users[0].Id,
                        ReceiverId = users[1].Id,
                        Content = "J'ai trouvé un MacBook, pouvez-vous le décrire ?",
                        CreatedAt = DateTime.UtcNow.AddHours(-12),
                        IsRead = false
                    },
                    new Message
                    {
                        DemandeId = demandes[2].Id,
                        SenderId = users[1].Id,
                        ReceiverId = users[0].Id,
                        Content = "Les AirPods ont-ils un étui de protection bleu ?",
                        CreatedAt = DateTime.UtcNow.AddHours(-6),
                        IsRead = false
                    }
                };

                context.Messages.AddRange(messages);
                context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new Exception("Error seeding the database", ex);
            }
        }

        private static Item CreateItem(string description, string location, string status, string category, int userId)
        {
            return new Item
            {
                Description = description,
                Location = location,
                ReportedDate = DateTime.UtcNow.AddDays(-Random.Next(1, 10)),
                Status = status,
                Type = status == "found" ? "Found" : "Lost",
                Category = category,
                ImageUrl = GetRandomImageForCategory(category),
                ReportedByUserId = userId,
                UpdatedAt = DateTime.UtcNow
            };
        }
    }
}