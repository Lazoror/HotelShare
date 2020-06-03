using HotelShare.Domain.Models.SqlModels.HotelModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HotelShare.DAL.EntityConfigurations.Game
{
    public class HotelImageConfiguration : IEntityTypeConfiguration<HotelImage>
    {
        public void Configure(EntityTypeBuilder<HotelImage> builder)
        {
            builder.ToTable("HotelImage");

            builder.HasKey(sc => new { GameId = sc.HotelId, sc.ImageId });
        }
    }
}