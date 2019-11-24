using HotelShare.Domain.Models.SqlModels.OrderModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HotelShare.DAL.EntityConfigurations.Order
{
    public class OrderDetailConfiguration : IEntityTypeConfiguration<OrderDetail>
    {
        public void Configure(EntityTypeBuilder<OrderDetail> builder)
        {
            builder.ToTable("OrderDetail");

            builder.Property(a => a.Id).ValueGeneratedNever();

            builder.Property(p => p.Price)
                .HasColumnType("Money");

            builder.Property(p => p.Quantity)
                .HasColumnType("smallint");

            builder.Property(p => p.Discount)
                .HasColumnType("Real");
        }
    }
}