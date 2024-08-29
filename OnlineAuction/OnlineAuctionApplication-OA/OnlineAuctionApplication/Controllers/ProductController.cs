// OnlineAuctionApplication/Controllers/ProductController.cs
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using OnlineAuction.BLL.Services.Interface;
using OnlineAuction.DAL.Model.Domain;
using OnlineAuction.DAL.Model.Dto;

[ApiController]
[Route("api/[controller]")]
public class ProductController : ControllerBase
{
    private readonly IProductService _productService;
    private readonly IMapper _mapper;

    public ProductController(IProductService productService, IMapper mapper)
    {
        _productService = productService;
        _mapper = mapper;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<ProductDto>>> GetAllProducts()
    {
        var products = await _productService.GetAllAsync();
        var productDtos = _mapper.Map<IEnumerable<ProductDto>>(products);
        return Ok(productDtos);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ProductDto>> GetProduct(string id)
    {
        var product = await _productService.GetByIdAsync(id);
        if (product == null)
        {
            return NotFound();
        }
        var productDto = _mapper.Map<ProductDto>(product);
        return Ok(productDto);
    }

    [HttpPost]
    public async Task<ActionResult<ProductDto>> CreateProduct(ProductDto productDto)
    {
        var product = _mapper.Map<Product>(productDto);
        product.ProductId = Guid.NewGuid().ToString(); // Automatically generate Guid for ProductId
        await _productService.AddAsync(product);
        var createdProductDto = _mapper.Map<ProductDto>(product);
        return CreatedAtAction(nameof(GetProduct), new { id = createdProductDto.ProductId }, createdProductDto);
    }


    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateProduct(string id, ProductDto productDto)
    {
        if (id != productDto.ProductId)
        {
            return BadRequest("ID mismatch");
        }

        // Fetch existing product to ensure it exists
        var existingProduct = await _productService.GetByIdAsync(id);
        if (existingProduct == null)
        {
            return NotFound();
        }

        // Update properties
        _mapper.Map(productDto, existingProduct);

        // Update the product in the database
        await _productService.UpdateAsync(existingProduct);

        return NoContent();
    }


    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteProduct(string id)
    {
        await _productService.DeleteAsync(id);
        return NoContent();
    }
}
