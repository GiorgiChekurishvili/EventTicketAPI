using Microsoft.EntityFrameworkCore;

namespace EventTicketAPI.Entities
{
    public class EventTicketContext : DbContext
    {
        public EventTicketContext(DbContextOptions<EventTicketContext> options) : base(options)
        {
            
        }
        
        public DbSet<Category> Categories { get; set; }
        public DbSet<Event> Events { get; set; }
        public DbSet<Favorite> Favorites { get; set;}
        public DbSet<TicketSale> TicketSales { get; set; }
        public DbSet<TicketType> TicketTypes { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Category>().HasKey(x => x.Id);
            modelBuilder.Entity<Category>().Property(x => x.Name).IsRequired().HasColumnType("nvarchar(50)");

            modelBuilder.Entity<Event>().HasKey(x => x.Id);
            modelBuilder.Entity<Event>().Property(x => x.DateAdded).HasDefaultValueSql("SYSDATETIME()");
            modelBuilder.Entity<Event>().HasOne(x => x.Category).WithMany(x => x.Event).HasForeignKey(x => x.CategoryId);
            modelBuilder.Entity<Event>().Property(x => x.EventName).IsRequired().HasColumnType("nvarchar(60)");
            modelBuilder.Entity<Event>().Property(x => x.EventDescription).IsRequired().HasColumnType("nvarchar(200)");
            modelBuilder.Entity<Event>().Property(x => x.EventLocation).IsRequired().HasColumnType("nvarchar(150)");
            modelBuilder.Entity<Event>().HasQueryFilter(x => !x.IsDeleted);

            modelBuilder.Entity<Role>().HasKey(x => x.Id);
            modelBuilder.Entity<Role>().Property(x => x.Name).IsRequired().HasColumnType("nvarchar(50)");


            modelBuilder.Entity<TicketSale>().HasKey(x => x.Id);
            modelBuilder.Entity<TicketSale>().Property(x => x.PurchaseDate).HasDefaultValueSql("SYSDATETIME()");
            modelBuilder.Entity<TicketSale>().HasOne(x => x.User).WithMany(x => x.TicketSale).HasForeignKey(x => x.UserId).OnDelete(DeleteBehavior.NoAction);
            modelBuilder.Entity<TicketSale>().HasOne(x => x.Event).WithMany(x => x.TicketSale).HasForeignKey(x => x.EventId).OnDelete(DeleteBehavior.NoAction);
            modelBuilder.Entity<TicketSale>().HasOne(x => x.TicketType).WithMany(x => x.TicketSale).HasForeignKey(x => x.TicketTypeId).OnDelete(DeleteBehavior.NoAction);
            modelBuilder.Entity<TicketSale>().HasQueryFilter(x => !x.IsDeleted);

            modelBuilder.Entity<TicketType>().HasKey(x => x.Id);
            modelBuilder.Entity<TicketType>().Property(x => x.DateAdded).HasDefaultValueSql("SYSDATETIME()");
            modelBuilder.Entity<TicketType>().HasOne(x => x.Event).WithMany(x => x.TicketType).HasForeignKey(x => x.EventId).OnDelete(DeleteBehavior.NoAction);
            modelBuilder.Entity<TicketType>().Property(x => x.TicketTypeName).HasColumnType("nvarchar(20)");
            modelBuilder.Entity<TicketType>().HasQueryFilter(x=>!x.IsDeleted);

            modelBuilder.Entity<User>().HasKey(x => x.Id);
            modelBuilder.Entity<User>().Property(x => x.DateCreated).HasDefaultValueSql("SYSDATETIME()");
            modelBuilder.Entity<User>().HasOne(x => x.Role).WithMany(x => x.User).HasForeignKey(x => x.RoleId);
            modelBuilder.Entity<User>().Property(x => x.Name).IsRequired().HasColumnType("nvarchar(30)");
            modelBuilder.Entity<User>().Property(x => x.LastName).IsRequired().HasColumnType("nvarchar(50)");
            modelBuilder.Entity<User>().Property(x => x.Email).IsRequired().HasColumnType("nvarchar(100)");


            modelBuilder.Entity<Favorite>().HasKey(x => x.Id);
            modelBuilder.Entity<Favorite>().Property(x=> x.FavoriteAdded).HasDefaultValueSql("SYSDATETIME()");
            modelBuilder.Entity<Favorite>().HasOne(x=>x.User).WithMany(x=>x.Favorite).HasForeignKey(x => x.UserId);
            modelBuilder.Entity<Favorite>().HasOne(x => x.Event).WithMany(x => x.Favorite).HasForeignKey(x => x.EventId);

        }
        public override int SaveChanges()
        {
            foreach (var entry in ChangeTracker.Entries())
            {
                
                if (entry.Entity is ISoftDeletable softDeletable && entry.State == EntityState.Deleted)
                {
                    
                    entry.State = EntityState.Modified;
                    entry.CurrentValues["IsDeleted"] = true;
                }
            }
            return base.SaveChanges();
        }
    }
}
