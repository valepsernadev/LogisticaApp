using Microsoft.EntityFrameworkCore;
using LogisticaApp.Data;
using LogisticaApp.DTOs;
using LogisticaApp.Models;
using LogisticaApp.Interfaces;

namespace LogisticaApp.Services;

public class ConductorService : IConductorService
{
    private readonly AppDbContext _context;

    public ConductorService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<ConductorDto> Create(CreateConductorDto dto)
    {
        var emailExists = await _context.Usuarios.AnyAsync(u => u.Email == dto.Email);
        if (emailExists)
            throw new Exception("El email ya está registrado.");

        var usuario = new Usuario
        {
            Nombre = dto.Nombre,
            Email = dto.Email,
            Password = BCrypt.Net.BCrypt.HashPassword(dto.Password),
            Rol = "conductor"
        };

        _context.Usuarios.Add(usuario);
        await _context.SaveChangesAsync();

        var conductor = new Conductor
        {
            UsuarioId = usuario.Id,
            Licencia = dto.Licencia
        };

        _context.Conductores.Add(conductor);
        await _context.SaveChangesAsync();

        return MapToDto(conductor, usuario);
    }

    public async Task<List<ConductorDto>> GetAll()
    {
        var conductores = await _context.Conductores
            .Include(c => c.Usuario)
            .ToListAsync();

        return conductores.Select(c => MapToDto(c, c.Usuario)).ToList();
    }

    public async Task<ConductorDto> GetById(Guid id)
    {
        var conductor = await _context.Conductores
            .Include(c => c.Usuario)
            .FirstOrDefaultAsync(c => c.Id == id);

        if (conductor == null)
            throw new KeyNotFoundException("Conductor no encontrado.");

        return MapToDto(conductor, conductor.Usuario);
    }

    public async Task<ConductorDto> Update(Guid id, UpdateConductorDto dto)
    {
        var conductor = await _context.Conductores
            .Include(c => c.Usuario)
            .FirstOrDefaultAsync(c => c.Id == id);

        if (conductor == null)
            throw new KeyNotFoundException("Conductor no encontrado.");

        conductor.Usuario.Nombre = dto.Nombre;
        conductor.Licencia = dto.Licencia;
        conductor.Activo = dto.Activo;

        await _context.SaveChangesAsync();

        return MapToDto(conductor, conductor.Usuario);
    }

    public async Task<ConductorDto> ChangeStatus(Guid id, bool activo)
    {
        var conductor = await _context.Conductores
            .Include(c => c.Usuario)
            .FirstOrDefaultAsync(c => c.Id == id);

        if (conductor == null)
            throw new KeyNotFoundException("Conductor no encontrado.");

        conductor.Activo = activo;
        await _context.SaveChangesAsync();

        return MapToDto(conductor, conductor.Usuario);
    }

    private static ConductorDto MapToDto(Conductor conductor, Usuario usuario)
    {
        return new ConductorDto
        {
            Id = conductor.Id,
            Nombre = usuario.Nombre,
            Email = usuario.Email,
            Licencia = conductor.Licencia,
            Activo = conductor.Activo,
            CreatedAt = conductor.CreatedAt
        };
    }
}