using Microsoft.Extensions.Options;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.Extensions.Configuration;
using SecurityService_Core.Security.Tokens;
using SecurityService_Core.Interfaces;
using SecurityService_Core.Models.DB;
using System.Security.Cryptography;

namespace SecurityService_Core.Security
{
    public class TokenHandler : ITokenHandler
    {
        private readonly ISet<RefreshToken> _refreshTokens = new HashSet<RefreshToken>();

        private readonly TokenOptions _tokenOptions;
        private readonly SigningConfigurations _signingConfigurations;
        private readonly IConfiguration _configuration;

        public TokenHandler(IConfiguration configuration, IOptions<TokenOptions> tokenOptionsSnapshot, SigningConfigurations signingConfigurations)
        {
            _tokenOptions = tokenOptionsSnapshot.Value;
            _signingConfigurations = signingConfigurations;
            _configuration = configuration;
        }

        public AccessToken CreateAccessToken(User user)
        {
            var refreshToken = BuildRefreshToken();
            var accessToken = BuildAccessToken(user.Id, refreshToken, user.UserName!);
            _refreshTokens.Add(refreshToken);

            return accessToken;
        }

        public RefreshToken TakeRefreshToken(string token)
        {
            if (string.IsNullOrWhiteSpace(token))
                return null;

            var refreshToken = _refreshTokens.SingleOrDefault(t => t.Token == token);
            if (refreshToken != null)
                _refreshTokens.Remove(refreshToken);

            return refreshToken;
        }

        public void RevokeRefreshToken(string token)
        {
            TakeRefreshToken(token);
        }

        private RefreshToken BuildRefreshToken()
        {
            var tokenOptions = new TokenOptions();
            _configuration.GetSection("TokenOptions").Bind(tokenOptions);
            var refreshToken = new RefreshToken
            (
                token: HashPassword(Guid.NewGuid().ToString()),
                expiration: DateTime.UtcNow.AddSeconds(tokenOptions.RefreshTokenExpiration).Ticks
            );

            return refreshToken;
        }

        private AccessToken BuildAccessToken(Guid id, RefreshToken refreshToken, string login)
        {
            var tokenOptions = new TokenOptions();
            _configuration.GetSection("TokenOptions").Bind(tokenOptions);
            var accessTokenExpiration = DateTime.UtcNow.AddSeconds(tokenOptions.AccessTokenExpiration);

            //create a identity and add claims to the user which we want to log in
            ClaimsIdentity claimsIdentity = new ClaimsIdentity(new[]
            {
                new Claim("Id", id.ToString(), ClaimValueTypes.String),
                new Claim("Login", login, ClaimValueTypes.String),
                new Claim("isAuthenticated", "true")
            }, "Custom");
            //create the jwt
            var handler = new JwtSecurityTokenHandler();
            var token = handler.CreateJwtSecurityToken(issuer: tokenOptions.Issuer, audience: tokenOptions.AllowedAudiences.FirstOrDefault(),
            subject: claimsIdentity, notBefore: DateTime.UtcNow, expires: accessTokenExpiration, signingCredentials: _signingConfigurations.SigningCredentials);

            var accessToken = handler.WriteToken(token);

            return new AccessToken(accessToken, accessTokenExpiration.Ticks, refreshToken);
        }

        /// <summary>
        /// Хеширование пароля
        /// </summary>
        /// <param name="password"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public string HashPassword(string password)
        {
            byte[] salt;
            byte[] buffer2;
            if (string.IsNullOrEmpty(password))
            {
                throw new ArgumentNullException("Пароль пуст или отсутствует");
            }
            using (var bytes = new Rfc2898DeriveBytes(password, 0x10, 0x3e8))
            {
                salt = bytes.Salt;
                buffer2 = bytes.GetBytes(0x20);
            }
            byte[] dst = new byte[0x31];
            Buffer.BlockCopy(salt, 0, dst, 1, 0x10);
            Buffer.BlockCopy(buffer2, 0, dst, 0x11, 0x20);
            return Convert.ToBase64String(dst);
        }
    }
}
