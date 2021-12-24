using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using BuildRestApiNetCore.Models;
using BuildRestApiNetCore.Services;

namespace BuildRestApiNetCore.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("[controller]")]
    [Produces("application/json")]
    public class ProductsController : ControllerBase
    {

        private readonly ILogger<ProductsController> _logger;

        private readonly ShopbridgeContext _context;

        public ProductsController(ShopbridgeContext context, ILogger<ProductsController> logger)
        {
            _context = context;
            _logger = logger;

            _logger.LogDebug("NLog injected into ProductsController");
        }

        [HttpGet]
        [Route("")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<Product>>> GetAllProducts()
        {
            var products = await _context.Products.ToListAsync();
            
            return Ok(products);
        }

        // [HttpPost]
        // [ProducesResponseType(StatusCodes.Status201Created)]
        // [ProducesResponseType(StatusCodes.Status400BadRequest)]
        // public ActionResult<Product> PostProduct([FromBody] Product product)
        // {
        //     try
        //     {
        //         _context.Products.Add(product);
        //         _context.SaveChanges();

        //         return new CreatedResult($"/prodcuts/{product.ProductNumber.ToLower()}", product);
        //     }
        //     catch(Exception e)
        //     {
        //         _logger.LogWarning(e, "Unable to POST product");

        //         return ValidationProblem(e.Message);
        //     }
        // }

        // [HttpGet]
        // [Route("{productNumber}")]
        // [ProducesResponseType(StatusCodes.Status200OK)]
        // [ProducesResponseType(StatusCodes.Status404NotFound)]
        // public ActionResult<Product> GetProductByProductNumber([FromRoute] string productNumber)
        // {
        //     var productDb = _context.Products.FirstOrDefault(p => p.ProductNumber.Equals(productNumber, StringComparison.InvariantCultureIgnoreCase));

        //     if(productDb == null)
        //         return NotFound();

        //     return Ok(productDb);
        // }        

        // [HttpPut]
        // [ProducesResponseType(StatusCodes.Status200OK)]
        // [ProducesResponseType(StatusCodes.Status400BadRequest)]
        // [ProducesResponseType(StatusCodes.Status404NotFound)]
        // public ActionResult<Product> PutProduct([FromBody] Product product)
        // {
        //     try
        //     {
        //         var productDb = _context.Products.FirstOrDefault(p => p.ProductNumber.Equals(product.ProductNumber, StringComparison.InvariantCultureIgnoreCase));

        //         if(productDb == null)
        //             return NotFound();

        //         productDb.Name = product.Name;
        //         product.Price = product.Price;
        //         product.Department = product.Department;

        //         _context.SaveChanges();

        //         return Ok(product);
        //     }
        //     catch(Exception ex)
        //     {
        //         _logger.LogWarning(ex, "Unable to PUT product");
        //         return ValidationProblem(ex.Message);
        //     }
        // }

        // [HttpPatch]
        // [Route("{productNumber}")]
        // [ProducesResponseType(StatusCodes.Status200OK)]
        // [ProducesResponseType(StatusCodes.Status400BadRequest)]
        // [ProducesResponseType(StatusCodes.Status404NotFound)]
        // public ActionResult<Product> PatchProduct([FromRoute] string productNumber, [FromBody] JsonPatchDocument<Product> patch)
        // {
        //     try
        //     {
        //         var productDb = _context.Products.FirstOrDefault(p => p.ProductNumber.Equals(productNumber, StringComparison.InvariantCultureIgnoreCase));

        //         if(productDb == null)
        //             return NotFound();

        //         patch.ApplyTo(productDb, ModelState);

        //         if(!ModelState.IsValid || !TryValidateModel(productDb))
        //             return ValidationProblem(ModelState);

        //         _context.SaveChanges();

        //         return Ok(productDb);

        //     }
        //     catch(Exception e)
        //     {
        //         _logger.LogWarning(e, "Unable to PATCH product");

        //         return ValidationProblem(e.Message);
        //     }
        // }

        // [HttpDelete]
        // [Route("{productNumber}")]
        // [ProducesResponseType(StatusCodes.Status200OK)]
        // [ProducesResponseType(StatusCodes.Status404NotFound)]
        // public ActionResult<Product> DeleteProduct([FromRoute] string productNumber)
        // {
        //     var productDb = _context.Products.FirstOrDefault(p => p.ProductNumber.Equals(productNumber, StringComparison.InvariantCultureIgnoreCase));

        //     if(productDb == null)
        //         return NotFound();

        //     _context.Products.Remove(productDb);
        //     _context.SaveChanges();

        //     return NoContent();   
        // }
    }
}