using HotelShare.Domain.Models.SqlModels.HotelModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HotelShare.DAL.EntityConfigurations.Game
{
    public class HotelConfiguration : IEntityTypeConfiguration<Hotel>
    {
        public void Configure(EntityTypeBuilder<Hotel> builder)
        {
            builder.ToTable("Hotel");

            builder.Property(a => a.Id).ValueGeneratedNever();

            builder.Property(p => p.AvailableRooms)
                .HasColumnType("smallint");
        }
    }
}