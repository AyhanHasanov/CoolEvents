using Microsoft.EntityFrameworkCore;

namespace CoolEvents.Data
{
    public class ApplicationDbContext : DbContext
    {
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<Event> Events { get; set; }
        public virtual DbSet<Role> Roles { get; set; }
        public virtual DbSet<Ticket> Tickets { get; set; }
        public virtual DbSet<UserTickets> UserTickets { get; set; }
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Role>().HasData(
                new Role()
                {
                    Id = 1,
                    Name = "Administrator"
                },
                new Role()
                {
                    Id = 2,
                    Name = "User"
                }
                );

            modelBuilder.Entity<User>().HasData(
                new User()
                {
                    Id = 1,
                    Username = "admin",
                    Password = "adminpass",
                    FirstName = "Admin",
                    LastName = "Adminov",
                    RoleId = 1
                });

            modelBuilder.Entity<UserTickets>()
                .HasOne(ut => ut.User)
                .WithMany(u => u.UserTickets)
                .HasForeignKey(ut => ut.UserId);

            modelBuilder.Entity<UserTickets>()
               .HasOne(ut => ut.Ticket)
               .WithMany(u => u.UserTickets)
               .HasForeignKey(ut => ut.TicketId);

            modelBuilder.Entity<Event>().HasData(
                new Event()
                {
                    Id = 1,
                    Name = "Lion Heart Utopia",
                    Description = "Състезанията по крос-триатлон на Лъвско сърце са многокомпонентни. Масов старт, навигация в открити води, смяна на спортовете, техника за преминаване на вариращи терени, стръмно спускане по нестабилен терен, коловози, бодили, слънце и насекоми. ",
                    Date = DateTime.Parse("28/05/2023"),
                    imageUrl = @"https://images.pexels.com/photos/2774556/pexels-photo-2774556.jpeg?auto=compress&cs=tinysrgb&w=1260&h=750&dpr=1"
                }, 
                new Event()
                {
                    Id = 2,
                    Name = "Bulgarian Rose Event",
                    Description = "See the most beautiful bulgarian roses! aaaaaaaaaaaaa",
                    Date = DateTime.Parse("21/06/2023")
                }
                );

        }
    }
}
