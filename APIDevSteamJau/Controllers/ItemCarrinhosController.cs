using System;
using System.Collections.Generic;
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
    public class ItemCarrinhosController : ControllerBase
    {
        private readonly APIContext _context;

        public ItemCarrinhosController(APIContext context)
        {
            _context = context;
        }

        // GET: api/ItemCarrinhos
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ItemCarrinho>>> GetItemCarrinho()
        {
            return await _context.ItemCarrinhos.ToListAsync();
        }

        // GET: api/ItemCarrinhos/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ItemCarrinho>> GetItemCarrinho(Guid id)
        {
            var itemCarrinho = await _context.ItemCarrinhos.FindAsync(id);

            if (itemCarrinho == null)
            {
                return NotFound();
            }

            return itemCarrinho;
        }

        // PUT: api/ItemCarrinhos/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutItemCarrinho(Guid id, ItemCarrinho itemCarrinho)
        {
            if (id != itemCarrinho.ItemCarrinhoId)
            {
                return BadRequest();
            }

            _context.Entry(itemCarrinho).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ItemCarrinhoExists(id))
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

        // POST: api/ItemCarrinhos
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<ItemCarrinho>> PostItemCarrinho(ItemCarrinho itemCarrinho)
        {
            //Verifica se o carrinho existe
            var carrinho = await _context.Carrinhos.FindAsync(itemCarrinho.CarrinhoId);
            if (carrinho == null)
            {
                return NotFound("Carrinho não encontrado.");
            }

            //Verifica se o jogo existe
            var jogo = await _context.Jogos.FindAsync(itemCarrinho.JogoId);
            if (jogo == null)
            {
                return NotFound("Jogo não encontrado.");
            }

            //calcula valor total
            itemCarrinho.ValorTotal = itemCarrinho.Quantidade * jogo.Preco;

            //adiconar valor total no carrinho 
            carrinho.ValorTotal += itemCarrinho.ValorTotal;


            _context.ItemCarrinhos.Add(itemCarrinho);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetItemCarrinho", new { id = itemCarrinho.ItemCarrinhoId }, itemCarrinho);
        }

        // DELETE: api/ItemCarrinhos/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteItemCarrinho(Guid id)
        {
            var itemCarrinho = await _context.ItemCarrinhos.FindAsync(id);
            if (itemCarrinho == null)
            {
                return NotFound();
            }

            //Verificar se o carrinho existe 
            var carrinho = await _context.Carrinhos.FindAsync(itemCarrinho.CarrinhoId);
            if (carrinho == null)
            {
                return NotFound("Carrinho não encontrado.");
            }


            //subtrair o valor total do carrinho
            carrinho.ValorTotal -= itemCarrinho.ValorTotal;

            //verifica se o valor total do carrinho é menor que 0
            if (carrinho.ValorTotal < 0)
            {
                carrinho.ValorTotal = 0;
            }

            _context.ItemCarrinhos.Remove(itemCarrinho);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ItemCarrinhoExists(Guid id)
        {
            return _context.ItemCarrinhos.Any(e => e.ItemCarrinhoId == id);
        }
    }
}
