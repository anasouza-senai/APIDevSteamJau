using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using APIDevSteamJau.Data;
using APIDevSteamJau.Models;

namespace APIDevSteamJau.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class JogosController : ControllerBase
    {
        private readonly APIContext _context;

        public JogosController(APIContext context)
        {
            _context = context;
        }

        // GET: api/Jogos
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Jogo>>> GetJogos()
        {
            return await _context.Jogos.ToListAsync();
        }

        // GET: api/Jogos/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Jogo>> GetJogo(Guid id)
        {
            var jogo = await _context.Jogos.FindAsync(id);

            if (jogo == null)
            {
                return NotFound();
            }

            return jogo;
        }

        // PUT: api/Jogos/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutJogo(Guid id, Jogo jogo)
        {
            if (id != jogo.JogoId)
            {
                return BadRequest();
            }

            _context.Entry(jogo).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!JogoExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Jogos
        [HttpPost]
        public async Task<ActionResult<Jogo>> PostJogo(Jogo jogo)
        {
            _context.Jogos.Add(jogo);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetJogo", new { id = jogo.JogoId }, jogo);
        }

        // DELETE: api/Jogos/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteJogo(Guid id)
        {
            var jogo = await _context.Jogos.FindAsync(id);
            if (jogo == null)
            {
                return NotFound();
            }

            _context.Jogos.Remove(jogo);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // POST: Upload Foto de Perfil
        [HttpPost("UploadFotoPerfil")]
        public async Task<IActionResult> UploadFotoPerfil(Guid usuarioId, IFormFile foto)
        {
            if (foto == null || foto.Length == 0)
                return BadRequest("Arquivo inválido.");

            var caminhoArquivo = Path.Combine("CaminhoParaSalvarImagens", $"{usuarioId}.jpg");

            using (var stream = new FileStream(caminhoArquivo, FileMode.Create))
            {
                await foto.CopyToAsync(stream);
            }

            return Ok("Foto enviada com sucesso.");
        }

        // GET: Buscar Foto de Perfil
        [HttpGet("BuscarFotoPerfil")]
        public IActionResult BuscarFotoPerfil(Guid usuarioId)
        {
            var caminhoArquivo = Path.Combine("CaminhoParaSalvarImagens", $"{usuarioId}.jpg");

            if (!System.IO.File.Exists(caminhoArquivo))
                return NotFound("Foto não encontrada.");

            var imagemBytes = System.IO.File.ReadAllBytes(caminhoArquivo);
            var imagemBase64 = Convert.ToBase64String(imagemBytes);

            return Ok(imagemBase64);
        }

        private bool JogoExists(Guid id)
        {
            return _context.Jogos.Any(e => e.JogoId == id);
        }
    }
}