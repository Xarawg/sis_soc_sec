using SecurityService_Core.Security.Tokens;

namespace SecurityService_AspNetCore.Services.Communication
{
    public class TokenResponse : BaseResponse
    {
        public AccessToken Token { get; set; }
        public int? Role { get; set; }

        public TokenResponse(bool success, string message, AccessToken token, int? role) : base(success, message)
        {
            Token = token;
            Role = role;
        }
    }
}
