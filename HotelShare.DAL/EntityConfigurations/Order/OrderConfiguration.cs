using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OrderModel = HotelShare.Domain.Models.SqlModels.OrderModels.Order;

namespace HotelShare.DAL.EntityConfigurations.Order
{
    public class OrderConfiguration : IEntityTypeConfiguration<OrderModel>
    {
        public void Configure(EntityTypeBuilder<OrderModel> builder)
        {
            builder.ToTable("Order");

            builder.Property(a => a.Id).ValueGeneratedNever();
        }
    }
}