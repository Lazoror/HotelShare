using HotelShare.Domain.Models.SqlModels.AccountModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HotelShare.DAL.EntityConfigurations.Account
{
    public class UserRoleConfiguration : IEntityTypeConfiguration<UserRole>
    {
        public void Configure(EntityTypeBuilder<UserRole> builder)
        {
            builder.ToTable("UserRole");

            builder.HasKey(ur => new { ur.UserId, ur.RoleId });
        }
    }
}