﻿using Core.Entities;
using Core.Interfaces;
using Infrastructure.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class ProductsController(IProductRepository repo) : ControllerBase
	{
		

        [HttpGet]
		public async Task<ActionResult<IReadOnlyList<Product>>>GetProducts(string ? brand, string? type, string? sort)
		{
			return Ok(await repo.GetProductsAsync(brand,type, sort));
		}

		[HttpGet("{id:int}")]
		public async Task<ActionResult<Product>> GetProduct(int id)
		{
			var product = await repo.GetProductByIdAsync(id);
			if (product == null)
			{
				return NotFound();
			}

			return product;
		}

		[HttpPost]
		public async Task<ActionResult<Product>>CreateProduct(Product product)
		{
			repo.AddProduct(product);

			if(await repo.SaveChangeAsync())
			{
				return CreatedAtAction("GetProduct", new { id = product.Id }, product);
			}

			return BadRequest("Problem Create Product");
		}

		[HttpPut("{id:int}")]
		public async Task<ActionResult>UpdateProduct(int id,Product product)
		{
			if (product.Id == null || !ProductExist(id)) 
				return BadRequest("Can not update Product");

			repo.UpdateProduct(product);
			if (await repo.SaveChangeAsync())
			{
				return NoContent();
			}

			return BadRequest("Problem Updating Product.");
		}

		[HttpDelete("{id:int}")]
		public async Task<ActionResult>DeleteProduct(int id)
		{
			var product = await repo.GetProductByIdAsync(id);

			if(product == null) return NotFound();

			repo.DeleteProduct(product);
			if (await repo.SaveChangeAsync())
			{
				return NoContent();
			}

			return BadRequest("Problem Deleting Product.");
		}


		[HttpGet("brands")]
		public async Task<ActionResult<IReadOnlyList<string>>>GetBrands()
		{
			return Ok(await repo.GetBrandsAsynce());
		}

		[HttpGet("types")]
		public async Task<ActionResult<IReadOnlyList<string>>> GetTypes()
		{
			return Ok(await repo.GetTypesAsynce());
		}
		private bool ProductExist(int id)
		{
			return repo.ProductExist(id);
		}
	}
}
