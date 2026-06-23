using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using LogisticaApp.Data;
using LogisticaApp.DTOs;
using LogisticaApp.Models;
using LogisticaApp.Interfaces;

namespace LogisticaApp.Services;

public class AuthService : IAuthService
{
    private readonly AppDbContext _context;
    private readonly IConfiguration _configuration;

    public AuthService(AppDbContext context, IConfiguration configuration)
    {
        _context = context;
        _configuration = configuration;
    }

    public async Task<AuthResponseDto> Register(RegisterDto dto)
    {
        var emailExists = await _context.Usuarios.AnyAsync(u => u.Email == dto.Email);
        if (emailExists)
            throw new Exception("El email ya está registrado.");

        var usuario = new Usuario
        {
            Nombre = dto.Nombre,
            Email = dto.Email,
            Password = BCrypt.Net.BCrypt.HashPassword(dto.Password),
            Rol = dto.Rol
        };

        _context.Usuarios.Add(usuario);
        await _context.SaveChangesAsync();

        var token = GenerateToken(usuario);

        return new AuthResponseDto
        {
            Id = usuario.Id,
            Nombre = usuario.Nombre,
            Email = usuario.Email,
            Rol = usuario.Rol,
            Token = token
        };
    }

    public async Task<AuthResponseDto> Login(LoginDto dto)
    {
        var usuario = await _context.Usuarios.FirstOrDefaultAsync(u => u.Email == dto.Email);
        if (usuario == null)
            throw new UnauthorizedAccessException("Credenciales incorrectas.");

        if (!BCrypt.Net.BCrypt.Verify(dto.Password, usuario.Password))
            throw new UnauthorizedAccessException("Credenciales incorrectas.");

        var token = GenerateToken(usuario);

        return new AuthResponseDto
        {
            Id = usuario.Id,
            Nombre = usuario.Nombre,
            Email = usuario.Email,
            Rol = usuario.Rol,
            Token = token
        };
    }

    private string GenerateToken(Usuario usuario)
    {
        var key = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
            new Claim("id", usuario.Id.ToString()),
            new Claim(ClaimTypes.Email, usuario.Email),
            new Claim(ClaimTypes.Role, usuario.Rol)
        };

        var token = new JwtSecurityToken(
            issuer: _configuration["Jwt:Issuer"],
            audience: _configuration["Jwt:Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddHours(8),
            signingCredentials: credentials
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}