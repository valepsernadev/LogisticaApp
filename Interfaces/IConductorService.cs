using LogisticaApp.DTOs;

namespace LogisticaApp.Interfaces;

public interface IConductorService
{
    Task<ConductorDto> Create(CreateConductorDto dto);
    Task<List<ConductorDto>> GetAll();
    Task<ConductorDto> GetById(Guid id);
    Task<ConductorDto> Update(Guid id, UpdateConductorDto dto);
    Task<ConductorDto> ChangeStatus(Guid id, bool activo);
}