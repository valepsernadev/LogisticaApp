using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using LogisticaApp.DTOs;
using LogisticaApp.Interfaces;

namespace LogisticaApp.Controllers;

[ApiController]
[Route("api/conductores")]
[Authorize(Policy = "OperadorOAdmin")]
public class ConductorController : ControllerBase
{
    private readonly IConductorService _conductorService;

    public ConductorController(IConductorService conductorService)
    {
        _conductorService = conductorService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var data = await _conductorService.GetAll();
        return Ok(new ApiResponseDto<List<ConductorDto>>
        {
            Success = true,
            Message = "Conductores obtenidos exitosamente",
            Data = data,
            StatusCode = 200
        });
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var data = await _conductorService.GetById(id);
        return Ok(new ApiResponseDto<ConductorDto>
        {
            Success = true,
            Message = "Conductor obtenido exitosamente",
            Data = data,
            StatusCode = 200
        });
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateConductorDto dto)
    {
        var data = await _conductorService.Create(dto);
        return Created(string.Empty, new ApiResponseDto<ConductorDto>
        {
            Success = true,
            Message = "Conductor creado exitosamente",
            Data = data,
            StatusCode = 201
        });
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateConductorDto dto)
    {
        var data = await _conductorService.Update(id, dto);
        return Ok(new ApiResponseDto<ConductorDto>
        {
            Success = true,
            Message = "Conductor actualizado exitosamente",
            Data = data,
            StatusCode = 200
        });
    }

    [HttpPatch("{id}/estado")]
    public async Task<IActionResult> ChangeStatus(Guid id, [FromBody] bool activo)
    {
        var data = await _conductorService.ChangeStatus(id, activo);
        return Ok(new ApiResponseDto<ConductorDto>
        {
            Success = true,
            Message = activo ? "Conductor activado" : "Conductor desactivado",
            Data = data,
            StatusCode = 200
        });
    }
}