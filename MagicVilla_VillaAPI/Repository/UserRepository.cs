﻿using AutoMapper;
using MagicVilla_VillaAPI.DB;
using MagicVilla_VillaAPI.Models;
using MagicVilla_VillaAPI.Models.Dto;
using MagicVilla_VillaAPI.Repository.IRepository;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace MagicVilla_VillaAPI.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDBContext _db;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IMapper _mapper;
        private readonly UserManager<ApplicationUser> _userManager;
        private string secretKey;

        public UserRepository(IMapper mapper, ApplicationDBContext db, IConfiguration configuration, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _db = db;
            _mapper = mapper;
            _userManager = userManager;
            secretKey = configuration.GetValue<string>("ApiSettings:Secret");
            _roleManager = roleManager;
        }
        public bool IsUniqueUser(string username)
        {
            var user = _db.ApplicationUsers.FirstOrDefault(x => x.UserName == username.ToLower());
            if (user == null)
            {
                return true;
            }
            return false;
        }

        public async Task<TokenDTO> Login(LoginRequestDTO loginRequestDTO)
        {
            var user = _db.ApplicationUsers
                .FirstOrDefault(u => u.UserName.ToLower() == loginRequestDTO.UserName.ToLower());

            bool isvalid = await _userManager.CheckPasswordAsync(user, loginRequestDTO.Password);

            if (user == null || isvalid == false)
            {
                return new TokenDTO()
                {
                    AccessToken = ""
                };
            }
            var jwtTokenId = $"JTI{Guid.NewGuid()}";
            var accessToken=await GetAccessToken(user,jwtTokenId);
            var refreshToken = await CreateNewRefreshToken(user.Id, jwtTokenId);
            TokenDTO Tokendto = new TokenDTO()
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken
               // Role = roles.FirstOrDefault()
            };
            return Tokendto;
        }

        

        public async Task<UserDTO> Register(RegistrationRequestDTO registrationRequestDTO)
        {
            ApplicationUser user = new()
            {
                UserName = registrationRequestDTO.UserName,
                Name = registrationRequestDTO.Name,
                Email = registrationRequestDTO.UserName,
                NormalizedEmail = registrationRequestDTO.UserName.ToUpper()
            };

            try
            {
                var result = await _userManager.CreateAsync(user, registrationRequestDTO.Password);
                if (result.Succeeded)
                {
                    if (!_roleManager.RoleExistsAsync(registrationRequestDTO.Role).GetAwaiter().GetResult())
                    {
                        await _roleManager.CreateAsync(new IdentityRole(registrationRequestDTO.Role));
                    }
                    await _userManager.AddToRoleAsync(user,registrationRequestDTO.Role);
                    var usertoReturn = _db.ApplicationUsers.
                        FirstOrDefault(u => u.UserName == registrationRequestDTO.UserName);
                    return _mapper.Map<UserDTO>(usertoReturn);
                }
            }
            catch (Exception ex) { }
            return new UserDTO();
        }

        private async Task<string> GetAccessToken(ApplicationUser user,string jwtTokenId)
        {

            var roles = await _userManager.GetRolesAsync(user);

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(secretKey);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name,user.UserName.ToString()),
                    new Claim(ClaimTypes.Role,roles.FirstOrDefault()),
                    new Claim(Microsoft.IdentityModel.JsonWebTokens.JwtRegisteredClaimNames.Jti,jwtTokenId),
                    new Claim(System.IdentityModel.Tokens.Jwt.JwtRegisteredClaimNames.Sub,user.Id)
                }),
                Expires = DateTime.UtcNow.AddMinutes(1),
                SigningCredentials = new(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenStr=tokenHandler.WriteToken(token);
            return tokenStr;

        }

        public async Task<TokenDTO> RefreshAccessToken(TokenDTO tokenDTO)
        {
            //find an existing refresh token
            var existingRefreshToken = await _db.RefreshTokens.FirstOrDefaultAsync(u => u.Refresh_Token == tokenDTO.RefreshToken);
            if (existingRefreshToken == null) {
                return new TokenDTO();
            }
            // compare data from existing refresh and access token providd and if there is any mismatch consider it as a fraud
            var isTokenValid = GetAccessTokenData(tokenDTO.AccessToken,existingRefreshToken.UserId,existingRefreshToken.JwtTokenId);
            if (!isTokenValid)
            {
                await MarkTokenAsInvalid(existingRefreshToken);
                return new TokenDTO();
            }
            // when someone tries to use not valid refresh token, fraud possible
            if (!existingRefreshToken.IsValid) {
                await MarkAllTokenInchainInvalid(existingRefreshToken.UserId, existingRefreshToken.JwtTokenId);

                return new TokenDTO();
            
            }


            // if just expired then mark ad invalid and return empty

            if (existingRefreshToken.ExpiresAt < DateTime.UtcNow)
            {
                await MarkTokenAsInvalid(existingRefreshToken);
                return new TokenDTO();
            }

            // replace old refresh with a new one with updated expired date
            var newRefreshToken = await CreateNewRefreshToken(existingRefreshToken.UserId, existingRefreshToken.JwtTokenId);

            // revoke existing refresh token
            await MarkTokenAsInvalid(existingRefreshToken);
            // generate new token

            var applicationUser=_db.ApplicationUsers.FirstOrDefault(u=>u.Id==existingRefreshToken.UserId);
            if (applicationUser == null) { return new TokenDTO(); }
            var newAccessToken = await GetAccessToken(applicationUser, existingRefreshToken.JwtTokenId);

            return new TokenDTO()
            {
                AccessToken = newAccessToken,
                RefreshToken = newRefreshToken
            };
        }

        public async Task RevokeRefreshToken(TokenDTO tokenDTO)
        {
            var existingRefreshToken =
                await _db.RefreshTokens.FirstOrDefaultAsync(_ => _.Refresh_Token == tokenDTO.RefreshToken);
            if (existingRefreshToken == null) { return; }


            // compare data from existing refresh and access token provided and
            // if there is any mismatch then we should do nothing with refresh token
            var isTokenValid = GetAccessTokenData(tokenDTO.AccessToken, existingRefreshToken.UserId, existingRefreshToken.JwtTokenId);
            if (!isTokenValid)
            {
                return;
            }
            await MarkAllTokenInchainInvalid(existingRefreshToken.UserId, existingRefreshToken.JwtTokenId);
        }


        private async Task<string> CreateNewRefreshToken(string userId,string tokenId)
        {
            RefreshToken refreshToken = new()
            {
                IsValid = true,
                UserId = userId,
                JwtTokenId = tokenId,
                ExpiresAt = DateTime.UtcNow.AddMinutes(3),
                Refresh_Token=Guid.NewGuid()+"-"+Guid.NewGuid()
            };
            await _db.RefreshTokens.AddAsync(refreshToken);
            await _db.SaveChangesAsync();
            return refreshToken.Refresh_Token;
        }
        private bool GetAccessTokenData(string accessToken,string expectedUserId,string expectedTokenId)
        {
            try
            {
                var tokenHandler=new JwtSecurityTokenHandler();
                var jwt=tokenHandler.ReadJwtToken(accessToken);
                var jwtTokenId = jwt.Claims.FirstOrDefault(u => u.Type == System.IdentityModel.Tokens.Jwt.JwtRegisteredClaimNames.Jti).Value;
                var userId = jwt.Claims.FirstOrDefault(u => u.Type == System.IdentityModel.Tokens.Jwt.JwtRegisteredClaimNames.Sub).Value;
                return userId==expectedUserId && jwtTokenId==expectedTokenId;


            }
            catch (Exception ex)
            {

                return false;
            }
        }

        private async Task MarkAllTokenInchainInvalid(string userId,string tokenId)
        {
            await _db.RefreshTokens.Where(u => u.UserId == userId
            && u.JwtTokenId == tokenId).
            ExecuteUpdateAsync(u => u.SetProperty(refreshtoken => refreshtoken.IsValid, false));
        }

        private Task MarkTokenAsInvalid(RefreshToken refreshToken)
        {
            refreshToken.IsValid = false;
            return _db.SaveChangesAsync();
        }


    }
}
