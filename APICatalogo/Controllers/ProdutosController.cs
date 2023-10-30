using APICatalogo.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace APICatalogo.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ProdutosController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ProdutosController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public ActionResult<IEnumerable<Produto>> Get() //Retorna um produto
        {
            var produtos = _context.Produtos.ToList();
            if(produtos is null)
            {
                return NotFound("Produtos não encontrados...");
            }
            return produtos;
        }

        [HttpGet("{id:int}", Name = "ObterProduto")]

        public ActionResult<Produto> Get(int id) //Get = retorna produto, nesse caso por id
        {
            var produto = _context.Produtos.FirstOrDefault(p => p.ProdutoId == id);
            if(produto is null) 
            {
                return NotFound("Produto não encontrado");            
            }
            return produto;
        }

        [HttpPost]
        public ActionResult Post(Produto produto) //Post = Cria um novo produto
        {
            if (produto is null)
            {
                return BadRequest();
            }

            _context.Produtos.Add(produto);
            _context.SaveChanges(); //Para salvar no BD

            return new CreatedAtRouteResult("ObterProduto", //Rota ObterProduto
                new { id = produto.ProdutoId }, produto);
        }

        [HttpPut("{id:int}")] // /produtos/{id}
        public ActionResult Put(int id, Produto produto) //Put = Atualizar produto
        {
            if(id != produto.ProdutoId)
            {
                return BadRequest();
            }
            _context.Entry(produto).State = EntityState.Modified;
            _context.SaveChanges();

            return Ok(produto);
        }

        [HttpDelete("{id:int}")]
        public ActionResult Delete(int id) //Delete = Deletar produto
        {
            var produto = _context.Produtos.FirstOrDefault(p => p.ProdutoId == id);
            //var produto = _context.Produtos.Find(id);  -> Tem que ser chave primária

            if (produto is null)
            {
                return NotFound("Produto não localizado...");
            }
            _context.Produtos.Remove(produto);
            _context.SaveChanges();

            return Ok(produto);
        }
    }
}
