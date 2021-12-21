using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using BuildRestApiNetCore.Models;

namespace BuildRestApiNetCore.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("[controller]")]
    [Produces("application/json")]
    public class ProductsController : ControllerBase
    {

        private readonly ILogger<ProductsController> _logger;

        private readonly ProductContext _context;

        public ProductsController(ProductContext context, ILogger<ProductsController> logger)
        {
            _context = context;
            _logger = logger;

            _logger.LogDebug("NLog injected into ProductsController");

            if(_context.Products.Any())
                return;
            
            ProductSeed.InitData(context);
        }

        [HttpGet]
        [Route("")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<IQueryable<Product>> GetProducts([FromQuery] string? department, [FromQuery] ProductRequest request)
        {
            var result = _context.Products as IQueryable<Product>;

            Response.Headers["x-total-count"] = result.Count().ToString();

            if(!string.IsNullOrEmpty(department))
            {
                _logger.LogInformation($"Filtering on department = {department}");
                result = result.Where(p => p.Department.StartsWith(department, System.StringComparison.InvariantCultureIgnoreCase));
            }

            if(request.Limit >= 100)
                _logger.LogInformation($"Requesting more then 100 products");

            return Ok(result
                .OrderBy(p => p.ProductNumber)
                .Skip(request.Offset)
                .Take(request.Limit)
                );
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<Product> PostProduct([FromBody] Product product)
        {
            try
            {
                _context.Products.Add(product);
                _context.SaveChanges();

                return new CreatedResult($"/prodcuts/{product.ProductNumber.ToLower()}", product);
            }
            catch(Exception e)
            {
                _logger.LogWarning(e, "Unable to POST product");

                return ValidationProblem(e.Message);
            }
        }

        [HttpGet]
        [Route("{productNumber}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<Product> GetProductByProductNumber([FromRoute] string productNumber)
        {
            var productDb = _context.Products.FirstOrDefault(p => p.ProductNumber.Equals(productNumber, StringComparison.InvariantCultureIgnoreCase));

            if(productDb == null)
                return NotFound();

            return Ok(productDb);
        }        

        [HttpPut]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<Product> PutProduct([FromBody] Product product)
        {
            try
            {
                var productDb = _context.Products.FirstOrDefault(p => p.ProductNumber.Equals(product.ProductNumber, StringComparison.InvariantCultureIgnoreCase));

                if(productDb == null)
                    return NotFound();

                productDb.Name = product.Name;
                product.Price = product.Price;
                product.Department = product.Department;

                _context.SaveChanges();

                return Ok(product);
            }
            catch(Exception ex)
            {
                _logger.LogWarning(ex, "Unable to PUT product");
                return ValidationProblem(ex.Message);
            }
        }

        [HttpPatch]
        [Route("{productNumber}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<Product> PatchProduct([FromRoute] string productNumber, [FromBody] JsonPatchDocument<Product> patch)
        {
            try
            {
                var productDb = _context.Products.FirstOrDefault(p => p.ProductNumber.Equals(productNumber, StringComparison.InvariantCultureIgnoreCase));

                if(productDb == null)
                    return NotFound();

                patch.ApplyTo(productDb, ModelState);

                if(!ModelState.IsValid || !TryValidateModel(productDb))
                    return ValidationProblem(ModelState);

                _context.SaveChanges();

                return Ok(productDb);

            }
            catch(Exception e)
            {
                _logger.LogWarning(e, "Unable to PATCH product");

                return ValidationProblem(e.Message);
            }
        }

        [HttpDelete]
        [Route("{productNumber}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<Product> DeleteProduct([FromRoute] string productNumber)
        {
            var productDb = _context.Products.FirstOrDefault(p => p.ProductNumber.Equals(productNumber, StringComparison.InvariantCultureIgnoreCase));

            if(productDb == null)
                return NotFound();

            _context.Products.Remove(productDb);
            _context.SaveChanges();

            return NoContent();   
        }
    }
}