using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using PD411_Books.BLL.Dtos.User;
using PD411_Books.DAL.Entities;
using PD411_Books.DAL.Repositories;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace PD411_Books.BLL.Services
{
    public class UserService
    {
        private readonly UserRepository _userRepository;
        private readonly EmailService _emailService;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;

        public UserService(UserRepository userRepository, EmailService emailService, IMapper mapper, IConfiguration configuration)
        {
            _userRepository = userRepository;
            _emailService = emailService;
            _mapper = mapper;
            _configuration = configuration;
        }

        public async Task<ServiceResponse> RegisterAsync(RegisterDto dto, string confirmationUrl)
        {
            var existingUser = await _userRepository.GetAll()
                .FirstOrDefaultAsync(u => u.Email == dto.Email);

            if (existingUser != null)
            {
                return new ServiceResponse
                {
                    Success = false,
                    Message = "Користувач з таким email вже існує"
                };
            }

            var user = _mapper.Map<UserEntity>(dto);
            user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password);
            user.EmailConfirmationToken = Guid.NewGuid().ToString();
            user.EmailConfirmationSent = DateTime.UtcNow;

            bool result = await _userRepository.CreateAsync(user);

            if (!result)
            {
                return new ServiceResponse
                {
                    Success = false,
                    Message = "Не вдалося зареєструвати користувача"
                };
            }

            var confirmationLink = $"{confirmationUrl}?token={user.EmailConfirmationToken}";
            var emailResponse = await _emailService.SendConfirmationEmailAsync(user.Email, confirmationLink);

            if (!emailResponse.Success)
            {
                return new ServiceResponse
                {
                    Success = false,
                    Message = $"Користувача зареєстровано, але не вдалося відправити лист: {emailResponse.Message}"
                };
            }

            return new ServiceResponse
            {
                Message = "Користувача успішно зареєстровано. Перевірте email для підтвердження.",
                Payload = _mapper.Map<UserDto>(user)
            };
        }

        public async Task<ServiceResponse> LoginAsync(LoginDto dto)
        {
            var user = await _userRepository.GetAll()
                .FirstOrDefaultAsync(u => u.Email == dto.Email);

            if (user == null)
            {
                return new ServiceResponse
                {
                    Success = false,
                    Message = "Невірний email або пароль"
                };
            }

            if (!BCrypt.Net.BCrypt.Verify(dto.Password, user.PasswordHash))
            {
                return new ServiceResponse
                {
                    Success = false,
                    Message = "Невірний email або пароль"
                };
            }

            if (!user.IsEmailConfirmed)
            {
                return new ServiceResponse
                {
                    Success = false,
                    Message = "Підтвердіть вашу email адресу перед входом"
                };
            }

            var token = GenerateJwtToken(user);

            var authResponse = new AuthResponseDto
            {
                Token = token,
                User = _mapper.Map<UserDto>(user)
            };

            return new ServiceResponse
            {
                Message = "Вхід успішний",
                Payload = authResponse
            };
        }

        public async Task<ServiceResponse> ConfirmEmailAsync(string token)
        {
            var user = await _userRepository.GetAll()
                .FirstOrDefaultAsync(u => u.EmailConfirmationToken == token);

            if (user == null)
            {
                return new ServiceResponse
                {
                    Success = false,
                    Message = "Невірний токен підтвердження"
                };
            }

            if (user.IsEmailConfirmed)
            {
                return new ServiceResponse
                {
                    Success = false,
                    Message = "Email вже підтверджено"
                };
            }

            user.IsEmailConfirmed = true;
            user.EmailConfirmationToken = null;

            bool result = await _userRepository.UpdateAsync(user);

            if (!result)
            {
                return new ServiceResponse
                {
                    Success = false,
                    Message = "Не вдалося підтвердити email"
                };
            }

            return new ServiceResponse
            {
                Message = "Email успішно підтверджено",
                Payload = _mapper.Map<UserDto>(user)
            };
        }

        private string GenerateJwtToken(UserEntity user)
        {
            var jwtSettings = _configuration.GetSection("JwtSettings");
            var secretKey = jwtSettings["SecretKey"] ?? throw new InvalidOperationException("JWT SecretKey not configured");
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Name, $"{user.FirstName} {user.LastName}")
            };

            var token = new JwtSecurityToken(
                issuer: jwtSettings["Issuer"],
                audience: jwtSettings["Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddHours(24),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
