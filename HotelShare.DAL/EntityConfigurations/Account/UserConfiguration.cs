using HotelShare.Domain.Models.SqlModels.AccountModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HotelShare.DAL.EntityConfigurations.Account
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("User");

            builder.HasIndex(u => u.Email).IsUnique();
        }
    }
}