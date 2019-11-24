using HotelShare.Domain.Models.SqlModels.AccountModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;

namespace HotelShare.DAL.EntityConfigurations.Account
{
    public class RoleConfiguration : IEntityTypeConfiguration<Role>
    {
        public void Configure(EntityTypeBuilder<Role> builder)
        {
            builder.HasIndex(r => r.Name).IsUnique();

            var roles = new Role[]
            {
                new Role {Id = Guid.NewGuid(), Name = "User"},
                new Role {Id = Guid.NewGuid(), Name = "Moderator"},
                new Role {Id = Guid.NewGuid(), Name = "Manager"},
                new Role {Id = Guid.NewGuid(), Name = "Administrator"},
            };

            //builder.HasData(roles);

            builder.ToTable("Role");
        }
    }
}