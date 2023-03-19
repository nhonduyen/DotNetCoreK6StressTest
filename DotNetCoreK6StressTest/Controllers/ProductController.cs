using DotNetCoreK6StressTest.Infrastructure.Data;
using DotNetCoreK6StressTest.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NuGet.Protocol.Plugins;
using System.ComponentModel.DataAnnotations;

namespace DotNetCoreK6StressTest.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly ProductDBContext _context;

        public ProductController(ProductDBContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<ActionResult<List<Product>>> Generate([FromBody] int numberOfProduct)
        {
            if (_context.Products == null)
            {
                return Problem("Entity set 'ProductDBContext.Products'  is null.");
            }
            var products = new List<Product>();
            for (int i = 0; i < numberOfProduct; i++)
            {
                var product = new Product();
                product.Name = RandomString(80);
                product.Description = RandomString(80);
                product.Category = RandomString(80);
                product.Active = true;
                product.Price = i;
                product.Id = Guid.NewGuid();
                products.Add(product);

            }
            _context.Products.AddRange(products);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetProduct", products);
        }

        private static string RandomString(int length)
        {
            Random random = new Random();
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        // GET: ProductController
        [HttpGet]
        public async Task<ActionResult> GetAll()
        {
            try
            {
                var products = await _context.Products.ToListAsync();
                return products is not null ? Ok(products) : NotFound(products);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // GET: ProductController/Details/5
        [HttpGet("{id}")]
        public async Task<ActionResult> Details([Required] Guid id)
        {

            try
            {
                var product = await _context.Products.FirstOrDefaultAsync(x => x.Id == id);
                return product is not null ? Ok(product) : NotFound(product);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // GET: ProductController/Create
        [HttpPost]
        public async Task<ActionResult> Create([FromBody] Product product)
        {
            try
            {
                _context.Products.Add(product);
                var result = await _context.SaveChangesAsync();
                return result > 0 ? Ok(product) : NotFound(product);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        // GET: ProductController/Edit/5
        [HttpPost]
        public async Task<ActionResult> Edit([FromBody] Product product)
        {
            try
            {
                _context.Entry(product).State = EntityState.Modified;
                var result = await _context.SaveChangesAsync();

                return result > 0 ? Ok(product) : NotFound(product);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // GET: ProductController/Delete/5
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete([Required] Guid id)
        {
            try
            {
                var product = await _context.Products.FindAsync(id);
                if (product == null)
                {
                    return NotFound();
                }

                _context.Products.Remove(product);
                await _context.SaveChangesAsync();

                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
