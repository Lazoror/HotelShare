using Autofac;
using HotelShare.Interfaces.Web.Settings;
using HotelShare.Web.Payment;
using HotelShare.Web.Settings.API;

namespace HotelShare.Web.AutofacModules
{
    public class WebModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<BankPayment>().Keyed<IPayment>(PaymentType.Bank);
            builder.RegisterType<IBoxPayment>().Keyed<IPayment>(PaymentType.IBox);
            builder.RegisterType<CardPayment>().Keyed<IPayment>(PaymentType.Visa);
            builder.RegisterType<JwtTokenFactory>().As<ITokenFactory>();
        }
    }
}