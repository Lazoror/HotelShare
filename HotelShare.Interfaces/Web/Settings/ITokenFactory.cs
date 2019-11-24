using System.Collections.Generic;
using System.Security.Claims;

namespace HotelShare.Interfaces.Web.Settings
{
    public interface ITokenFactory
    {
        string Create(IList<Claim> claims);
    }
}