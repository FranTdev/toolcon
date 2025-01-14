﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ToolConWebAPI; // Replace with the namespace of your DbContext and models
using System.Threading.Tasks;
using System.Collections.Generic;
using ToolConWebAPI.Models;

[Route("api/[controller]")]
[ApiController]
public class HerramientasController : ControllerBase
{
	private readonly ToolConDbContext _context;

	public HerramientasController(ToolConDbContext context)
	{
		_context = context;
	}

	// GET: api/Herramientas
	[HttpGet]
	public async Task<ActionResult<IEnumerable<Herramienta>>> GetHerramientas()
	{
		return await _context.Herramientas.ToListAsync();
	}

	// POST: api/Herramientas/AgregarHerramientas
	[HttpPost("AgregarHerramientas")]
	public async Task<ActionResult<Herramienta>> AgregarHerramientas([FromBody] Herramienta herramienta)
	{
		if (herramienta == null)
		{
			return BadRequest("Invalid tool data");
		}

		try
		{
			_context.Herramientas.Add(herramienta);
			await _context.SaveChangesAsync();

			return CreatedAtAction(nameof(GetHerramientas), new { id = herramienta.HerramientaId}, herramienta);
		}
		catch (Exception ex)
		{
			return StatusCode(500, "Internal server error: " + ex.Message);
		}
	}

	// POST: api/Herramientas/asignarHerramienta
	[HttpPost("asignarHerramienta")]
	public async Task<IActionResult> AsignarHerramienta([FromBody] Prestamo asignacion)
	{
		if (asignacion == null)
		{
			return BadRequest("Invalid assignment data");
		}

		// Create a new Prestamo record
		var prestamo = new Prestamo
		{
			HerramientaID = asignacion.HerramientaID,
			UsuarioID = asignacion.UsuarioID,
			FechaPrestamo = DateTime.Now, // Assuming the loan starts now
			FechaDevolucion = DateTime.MinValue // Assuming the return date is not set at the time of assignment
		};

		try
		{
			_context.Prestamos.Add(prestamo);
			await _context.SaveChangesAsync();
			return Ok(new { Message = "Tool assigned successfully", PrestamoID = prestamo.PrestamoID });
		}
		catch (Exception ex)
		{
			// Log the exception details for debugging
			return StatusCode(500, "Internal server error: " + ex.Message);
		}
	}


}
