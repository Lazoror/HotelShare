using HotelShare.DAL.EntityConfigurations;
using HotelShare.DAL.EntityConfigurations.Account;
using HotelShare.DAL.EntityConfigurations.Game;
using HotelShare.DAL.EntityConfigurations.Order;
using HotelShare.Domain.Models.SqlModels;
using HotelShare.Domain.Models.SqlModels.AccountModels;
using HotelShare.Domain.Models.SqlModels.CommentModels;
using HotelShare.Domain.Models.SqlModels.HotelModels;
using HotelShare.Domain.Models.SqlModels.OrderModels;
using Microsoft.EntityFrameworkCore;

namespace HotelShare.DAL.Data
{
    public class HotelContext : DbContext
    {
        public HotelContext(DbContextOptions<HotelContext> options) : base(options)
        {
            //Database.Migrate();
        }

        public DbSet<Hotel> Hotels { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }
        public DbSet<Room> Rooms { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new HotelConfiguration());
            modelBuilder.ApplyConfiguration(new CommentConfiguration());
            modelBuilder.ApplyConfiguration(new OrderConfiguration());
            modelBuilder.ApplyConfiguration(new OrderDetailConfiguration());
            modelBuilder.ApplyConfiguration(new RoomConfiguration());
            modelBuilder.ApplyConfiguration(new UserConfiguration());
            modelBuilder.ApplyConfiguration(new RoleConfiguration());
            modelBuilder.ApplyConfiguration(new UserRoleConfiguration());
            modelBuilder.ApplyConfiguration(new HotelImageConfiguration());
            modelBuilder.ApplyConfiguration(new ImageConfiguration());
        }
    }
}