﻿// Copyright Information
// ==================================
// EFCoreExamples - 13_JsonColumns - BlogsContext.cs
// All samples copyright Philip Japikse
// http://www.skimedic.com 2022/11/11
// ==================================

using System.Net;
using JsonColumns.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Context;

public class BlogsContext : DbContext
{
    public BlogsContext()
    {
    }

    public BlogsContext(DbContextOptions<BlogsContext> options): base(options)
    {
            
    }


    public bool LoggingEnabled { get; set; }
    public virtual MappingStrategy MappingStrategy => MappingStrategy.Tph;

    public DbSet<Blog> Blogs => Set<Blog>();
    public DbSet<Tag> Tags => Set<Tag>();
    public DbSet<Post> Posts => Set<Post>();
    public DbSet<Author> Authors => Set<Author>();

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer(@$"Server=(localdb)\mssqllocaldb;Database={GetType().Name}");
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<FeaturedPost>();
        modelBuilder.Entity<Author>().OwnsOne(
            author => author.Contact, ownedNavigationBuilder =>
            {
                ownedNavigationBuilder.ToJson();
                ownedNavigationBuilder.OwnsOne(contactDetails => contactDetails.Address);
            });
    }

    protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
    {
        configurationBuilder.Properties<List<string>>().HaveConversion<StringListConverter>();

        base.ConfigureConventions(configurationBuilder);
    }

    private class StringListConverter : ValueConverter<List<string>, string>
    {
        public StringListConverter()
            : base(v => string.Join(", ", v!), v => v.Split(',', StringSplitOptions.TrimEntries).ToList())
        {
        }
    }

    public async Task Seed()
    {
        var tagEntityFramework = new Tag("TagEF", "Entity Framework");
        var tagDotNet = new Tag("TagNet", ".NET");
        var tagDotNetMaui = new Tag("TagMaui", ".NET MAUI");
        var tagAspDotNet = new Tag("TagAsp", "ASP.NET");
        var tagAspDotNetCore = new Tag("TagAspC", "ASP.NET Core");
        var tagDotNetCore = new Tag("TagC", ".NET Core");
        var tagHacking = new Tag("TagHx", "Hacking");
        var tagLinux = new Tag("TagLin", "Linux");
        var tagSqlite = new Tag("TagLite", "SQLite");
        var tagVisualStudio = new Tag("TagVS", "Visual Studio");
        var tagGraphQl = new Tag("TagQL", "GraphQL");
        var tagCosmosDb = new Tag("TagCos", "CosmosDB");
        var tagBlazor = new Tag("TagBl", "Blazor");

        var maddy = new Author("Maddy Montaquila")
        {
            Contact = new() { Address = new("1 Main St", "Camberwick Green", "CW1 5ZH", "UK"), Phone = "01632 12345" }
        };
        var jeremy = new Author("Jeremy Likness")
        {
            Contact = new() { Address = new("2 Main St", "Chigley", "CW1 5ZH", "UK"), Phone = "01632 12346" }
        };
        var dan = new Author("Daniel Roth")
        {
            Contact = new() { Address = new("3 Main St", "Camberwick Green", "CW1 5ZH", "UK"), Phone = "01632 12347" }
        };
        var arthur = new Author("Arthur Vickers")
        {
            Contact = new() { Address = new("15a Main St", "Chigley", "CW1 5ZH", "UK"), Phone = "01632 12348" }
        };
        var brice = new Author("Brice Lambson")
        {
            Contact = new() { Address = new("4 Main St", "Chigley", "CW1 5ZH", "UK"), Phone = "01632 12349" }
        };

        var blogs = new List<Blog>
        {
            new(".NET Blog")
            {
                Posts =
                {
                    new Post(
                        "Productivity comes to .NET MAUI in Visual Studio 2022",
                        "Visual Studio 2022 17.3 is now available and...",
                        new DateTime(2022, 8, 9)) { Tags = { tagDotNetMaui, tagDotNet }, Author = maddy, Metadata = BuildPostMetadata() },
                    new Post(
                        "Announcing .NET 7 Preview 7", ".NET 7 Preview 7 is now available with improvements to System.LINQ, Unix...",
                        new DateTime(2022, 8, 9)) { Tags = { tagDotNet }, Author = jeremy, Metadata = BuildPostMetadata() },
                    new Post(
                        "ASP.NET Core updates in .NET 7 Preview 7", ".NET 7 Preview 7 is now available! Check out what's new in...",
                        new DateTime(2022, 8, 9))
                    {
                        Tags = { tagDotNet, tagAspDotNet, tagAspDotNetCore }, Author = dan, Metadata = BuildPostMetadata()
                    },
                    new FeaturedPost(
                        "Announcing Entity Framework 7 Preview 7: Interceptors!",
                        "Announcing EF7 Preview 7 with new and improved interceptors, and...",
                        new DateTime(2022, 8, 9),
                        "Loads of runnable code!")
                    {
                        Tags = { tagEntityFramework, tagDotNet, tagDotNetCore }, Author = arthur, Metadata = BuildPostMetadata()
                    }
                },
            },
            new("1unicorn2")
            {
                Posts =
                {
                    new Post(
                        "Hacking my Sixth Form College network in 1991",
                        "Back in 1991 I was a student at Franklin Sixth Form College...",
                        new DateTime(2020, 4, 10)) { Tags = { tagHacking }, Author = arthur, Metadata = BuildPostMetadata() },
                    new FeaturedPost(
                        "All your versions are belong to us",
                        "Totally made up conversations about choosing Entity Framework version numbers...",
                        new DateTime(2020, 3, 26),
                        "Way funny!") { Tags = { tagEntityFramework }, Author = arthur, Metadata = BuildPostMetadata() },
                    new Post(
                        "Moving to Linux", "A few weeks ago, I decided to move from Windows to Linux as...",
                        new DateTime(2020, 3, 7)) { Tags = { tagLinux }, Author = arthur, Metadata = BuildPostMetadata() },
                    new Post(
                        "Welcome to One Unicorn 2.0!", "I created my first blog back in 2011..",
                        new DateTime(2020, 2, 29)) { Tags = { tagEntityFramework }, Author = arthur, Metadata = BuildPostMetadata() }
                }
            },
            new("Brice's Blog")
            {
                Posts =
                {
                    new FeaturedPost(
                        "SQLite in Visual Studio 2022", "A couple of years ago, I was thinking of ways...",
                        new DateTime(2022, 7, 26), "Love for VS!")
                    {
                        Tags = { tagSqlite, tagVisualStudio }, Author = brice, Metadata = BuildPostMetadata()
                    },
                    new Post(
                        "On .NET - Entity Framework Migrations Explained",
                        "This week, @JamesMontemagno invited me onto the On .NET show...",
                        new DateTime(2022, 5, 4))
                    {
                        Tags = { tagEntityFramework, tagDotNet }, Author = brice, Metadata = BuildPostMetadata()
                    },
                    new Post(
                        "Dear DBA: A silly idea", "We have fun on the Entity Framework team...",
                        new DateTime(2022, 3, 31)) { Tags = { tagEntityFramework }, Author = brice, Metadata = BuildPostMetadata() },
                    new Post(
                        "Microsoft.Data.Sqlite 6", "It’s that time of year again. Microsoft.Data.Sqlite version...",
                        new DateTime(2021, 11, 8)) { Tags = { tagSqlite, tagDotNet }, Author = brice, Metadata = BuildPostMetadata() }
                }
            },
            new("Developer for Life")
            {
                Posts =
                {
                    new Post(
                        "GraphQL for .NET Developers", "A comprehensive overview of GraphQL as...",
                        new DateTime(2021, 7, 1))
                    {
                        Tags = { tagDotNet, tagGraphQl, tagAspDotNetCore }, Author = jeremy, Metadata = BuildPostMetadata()
                    },
                    new FeaturedPost(
                        "Azure Cosmos DB With EF Core on Blazor Server",
                        "Learn how to build Azure Cosmos DB apps using Entity Framework Core...",
                        new DateTime(2021, 5, 16),
                        "Blazor FTW!")
                    {
                        Tags =
                        {
                            tagDotNet,
                            tagEntityFramework,
                            tagAspDotNetCore,
                            tagCosmosDb,
                            tagBlazor
                        },
                        Author = jeremy,
                        Metadata = BuildPostMetadata()
                    },
                    new Post(
                        "Multi-tenancy with EF Core in Blazor Server Apps",
                        "Learn several ways to implement multi-tenant databases in Blazor Server apps...",
                        new DateTime(2021, 4, 29))
                    {
                        Tags = { tagDotNet, tagEntityFramework, tagAspDotNetCore, tagBlazor },
                        Author = jeremy,
                        Metadata = BuildPostMetadata()
                    },
                    new Post(
                        "An Easier Blazor Debounce", "Where I propose a simple method to debounce input without...",
                        new DateTime(2021, 4, 12))
                    {
                        Tags = { tagDotNet, tagAspDotNetCore, tagBlazor }, Author = jeremy, Metadata = BuildPostMetadata()
                    }
                }
            }
        };

        await AddRangeAsync(blogs);
        await SaveChangesAsync();

        PostMetadata BuildPostMetadata()
        {
            var random = new Random(Guid.NewGuid().GetHashCode());

            var metadata = new PostMetadata(random.Next(10000));

            for (var i = 0; i < random.Next(5); i++)
            {
                var update = new PostUpdate(IPAddress.Loopback, DateTime.UtcNow - TimeSpan.FromDays(random.Next(1, 10000)))
                {
                    UpdatedBy = "Admin"
                };

                for (var j = 0; j < random.Next(3); j++)
                {
                    update.Commits.Add(new(DateTime.Today, $"Commit #{j + 1}"));
                }

                metadata.Updates.Add(update);
            }

            for (var i = 0; i < random.Next(5); i++)
            {
                metadata.TopSearches.Add(new($"Search #{i + 1}", 10000 - random.Next(i * 1000, i * 1000 + 900)));
            }

            for (var i = 0; i < random.Next(5); i++)
            {
                metadata.TopGeographies.Add(
                    new(
                        // Issue https://github.com/dotnet/efcore/issues/28811 (Support spatial types in JSON columns)
                        // new Point(115.7930 + 20 - random.Next(40), 37.2431 + 10 - random.Next(20)) { SRID = 4326 },
                        115.7930 + 20 - random.Next(40),
                        37.2431 + 10 - random.Next(20),
                        1000 - random.Next(i * 100, i * 100 + 90))
                    { Browsers = new() { "Firefox", "Netscape" } });
            }

            return metadata;
        }
    }
}