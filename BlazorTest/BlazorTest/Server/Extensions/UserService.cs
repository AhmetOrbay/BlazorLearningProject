using AutoMapper;
using AutoMapper.QueryableExtensions;
using BlazorTest.Server.Extensions.Insfrastruce;
using BlazorTest.Shared.Dto;
using BlazorTest.Shared.Extensions;
using BlazorTest;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using MealOrdering.Server.Data.Context;
using MealOrdering.Server.Data.Models;

namespace BlazorTest.Server.Extensions
{
    public class UserService : IUserService
    {
        private readonly IMapper _mapper;
        private readonly MealOrderinDbContext _dbContext;
        private readonly IConfiguration _Configuration;
        public UserService(IMapper mapper,MealOrderinDbContext dbContext,IConfiguration configuration)
        {
            _mapper = mapper;
            _dbContext = dbContext;
            _Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public async Task<UserDto> CreateUser(UserDto User)
        {
            var userDb = await _dbContext.User.FirstOrDefaultAsync(x => x.Id == User.Id);
            if (userDb == null)
            {
                User.Password = PasswordEncrypter.Encrypt(User.Password);
                userDb = _mapper.Map<Users>(User);
                await _dbContext.User.AddAsync(userDb);
                var result = await _dbContext.SaveChangesAsync();
                return _mapper.Map<UserDto>(userDb);
            }
            throw new Exception($"User Found By Name ");
        }

        public async Task<bool> DeleteUserById(Guid Id)
        {
            var user = await _dbContext.User.FirstOrDefaultAsync(x => x.Id == Id);
            if (user != null)
            {
                user.IsActive = false;
                _dbContext.Update(user);
                var result = await _dbContext.SaveChangesAsync();
                return result > 0;
            }
            throw new ArgumentNullException("User Not Found");

        }

        public async Task<UserDto> GetById(Guid Id)
        {
            return  await _dbContext.User
                .ProjectTo<UserDto>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync(x => x.IsActive && x.Id == Id);
        }

        public async Task<List<UserDto>> GetUsers()
        {
            return await _dbContext.User
                .Where(x => x.IsActive)
                .ProjectTo<UserDto>(_mapper.ConfigurationProvider)
                .ToListAsync();
        }

        public async Task<UserLoginResponseDTO> Login(UserLoginRequestDTO userLogin)
        {
            //user dogrulama alani yapilablir
            var passwordEncrypt = PasswordEncrypter.Encrypt(userLogin.Password);
            var dbUser = await _dbContext.User
                        .FirstOrDefaultAsync(x => x.EmailAdress == userLogin.Email
                                    && x.Password == passwordEncrypt);
            if(dbUser == null) throw new Exception("User Not found");
            
            if (!dbUser.IsActive) throw new Exception("User is Passive");
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_Configuration["JwtSecurityKey"]));
            var creantials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expriDate = DateTime.Now.AddDays(double.Parse(_Configuration["JwtExpiryInDays"]));
            var claims = new[]
            {
                new Claim(ClaimTypes.Email,userLogin.Email),
                new Claim(ClaimTypes.Name,$"{dbUser.FirstName} {dbUser.LastName}"),
                new Claim(ClaimTypes.UserData,dbUser.Id.ToString())
            };
            var token = new JwtSecurityToken(_Configuration["JwtIssuer"], _Configuration["JwtAudience"], claims, null, expriDate, creantials);
            var tokenStr = new JwtSecurityTokenHandler().WriteToken(token);
            return new UserLoginResponseDTO()
            {
                ApiToken = tokenStr,
                User = _mapper.Map<UserDto>(dbUser)
            };

        }

        public async Task<UserDto> UpdateUser(UserDto User)
        {
            var userDb = await _dbContext.User.FirstOrDefaultAsync(x => x.Id == User.Id);
            if (userDb != null)
            {
                _mapper.Map(User, userDb);
                var result = await _dbContext.SaveChangesAsync();
                return _mapper.Map<UserDto>(userDb);
            }
            throw new ArgumentNullException($"User Not Found ");
        }
    }
}
