using AutoMapper;
using JetDevsPrcatical.Data;
using JetDevsPrcatical.Data.Abstract;
using JetDevsPrcatical.Data.Entity;
using JetDevsPrcatical.Data.Request;
using Microsoft.EntityFrameworkCore;

namespace JetDevsPrcatical.Data.Concrete
{
    public class UserRepository : IUserRepository
    {

        private readonly JetDevsPrcaticalContext _context;
        private readonly IMapper _mapper;
        public UserRepository(JetDevsPrcaticalContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;

        }

        public async Task<bool> UserRegister(UserRegisterRequest userRegisterModel)
        {
            Users userData = _mapper.Map<Users>(userRegisterModel);
            await _context.Users.AddAsync(userData);

            int result = await _context.SaveChangesAsync();
            if (result == 1)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public async Task<Users> AuthenticateAsync(LoginRequest loginRequest)
        {
            return await _context.Users.Where(x => x.Email == loginRequest.Email && x.Password == loginRequest.Password).FirstOrDefaultAsync();
        }

        public async Task<Users> GetUserByEmailAsync(string email)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task<bool> IsEmailExist(string email)
        {
            return await _context.Users.AnyAsync(u => u.Email == email);
        }

        public async Task UpdatePasswordAsync(Users user, string newPassword)
        {
            user.Password = newPassword;
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateTokenAsync(Users user, string token)
        {
            user.Token = token;
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
        }

        public async Task ResetToken(Users user)
        {
            user.Token = null;
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> ValidateToken(string userId, string token)
        {
            return await _context.Users.AnyAsync(u => Convert.ToString(u.UserId) == userId && u.Token == token);
        }
    }
}
