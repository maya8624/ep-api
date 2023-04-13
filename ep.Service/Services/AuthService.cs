﻿using ep.Domain.Models;
using Org.BouncyCastle.Asn1.Ocsp;
using Org.BouncyCastle.Ocsp;

namespace ep.Service.Services
{
    public class AuthService : IAuthService
    {
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;
        private readonly IRepositoryWrapper _repository;
        private readonly IUnitOfWork _unitOfWork;
        
        public AuthService(
            IConfiguration configuration, 
            IMapper mapper, IRepositoryWrapper repository, 
            IUnitOfWork unitOfWork)
        {
            _configuration = configuration;
            _mapper = mapper;
            _repository = repository;
            _unitOfWork = unitOfWork;
        }

        public string CreateToken(List<Claim> claims)
        {
            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_configuration.GetSection("AppSettings:Secret").Value));

            var creditionals = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature);
            var securityToken = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddMinutes(Constants.DefaultTokenExpireMins), 
                signingCredentials: creditionals);

            var token = new JwtSecurityTokenHandler().WriteToken(securityToken);
            return token;
        }

        private static void ThrowAuthorizedException(string errorMessage = ErrorMessageConstants.UserNotCorrect)
        {
            throw new AuthorizedException(ErrorCodeConstants.CredentialError, errorMessage);
        }

        public async Task<UserTokenView> GetTokenAsync(UserRequest request)
        {
            // TODO: move ThrowAuthorizedException method in a base class
            var user = await _repository.User.GetUserByEmailAsync(request.Email!);
            if (user == null) ThrowAuthorizedException();
            user!.Role = request.Role;

            var isVerified = VerifyPasswordHash(request.Password!, user);
            if (!isVerified) ThrowAuthorizedException();

            var claims = CreateClaims(request.Email, request.Role);
            var accessToken = CreateToken(claims);
            var refreshToken = await CreateRefreshTokenAsync(id: user.Id);

            var response = new UserTokenView
            {
                User = _mapper.Map<UserView>(user),
                AccessToken = accessToken,
                RefreshToken = refreshToken,
            };
            return response;
        }      
     
        private async Task<string> CreateRefreshTokenAsync(int id, int days = Constants.DefaultTokenExpireDays)
        {
            var userToken = new UserToken
            {
                RefreshToken = CryptoService.GenerateRandomKey(),
                TokenExpires = DateTime.UtcNow.AddDays(days),
                UserId = id
            };

            await _repository.UserToken.CreateAsync(userToken);
            await _unitOfWork.CompleteAsync();

            return userToken.RefreshToken;
        }

        private bool VerifyPasswordHash(string password, User user)
        {
            var hash = CryptoService.HashPassword(password, user.Salt!);
            return hash.SequenceEqual(user.Password!);
        }
               
        public async Task<UserTokenView> SilentLoginAsync(SilentLoginRequest request)
        {
            // Create a new token, refresh token and return it 
            var user = await _repository.User.GetUserByEmailAsync(request.Email);
            if (user == null) ThrowAuthorizedException();

            // create a new method to show different error messages.
            var userToken = await _repository.UserToken.GetUserTokenByRefreshToken(request.RefreshToken, user!.Id);
            if (userToken == null) ThrowAuthorizedException(ErrorMessageConstants.InvalidRefreshToken);

            await VerifyRefreshTokenAsync(user!.Id, request.RefreshToken);

            // TODO: refactor - duplicate code
            var claims = CreateClaims(request.Email, request.Role);
            var accessToken = CreateToken(claims);

            var response = new UserTokenView
            {
                User = _mapper.Map<UserView>(user),
                AccessToken = accessToken,
                RefreshToken = request.RefreshToken,
            };
            return response;
        }

        private async Task<bool> VerifyRefreshTokenAsync(int userId, string refreshToken)
        {
            var userToken = await _repository.UserToken.GetLatestUserTokenByUserId(userId);
            if (userToken == null) ThrowAuthorizedException(ErrorMessageConstants.InvalidUser);

            if (!userToken!.RefreshToken!.Equals(refreshToken))
            {
                ThrowAuthorizedException(ErrorMessageConstants.InvalidRefreshToken);
            }
            else if (userToken.TokenExpires < DateTimeOffset.UtcNow)
            {
                throw new AuthorizedException(ErrorCodeConstants.TokenExpiredError, ErrorMessageConstants.TokenExpired);
            }
            return true;
        }

        private List<Claim> CreateClaims(string email, string role)
        {
            // TODO: if a user is authenticated
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Email, email),
                new Claim(ClaimTypes.Role, role),
            };
            return claims;
        }
    }
}
