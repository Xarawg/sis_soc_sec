using SecurityService_Core.Models.DB;
using SecurityService_Core.Security.Tokens;

namespace SecurityService_Core.Interfaces
{
    public interface ITokenHandler
    {
        AccessToken CreateAccessToken(User user);
        RefreshToken TakeRefreshToken(string token);
        void RevokeRefreshToken(string token);
    }
}

