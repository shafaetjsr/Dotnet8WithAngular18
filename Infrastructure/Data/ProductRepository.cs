﻿using Core.Entities;
using Core.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Data
{
	public class ProductRepository(StoreContext context) : IProductRepository
	{
		public void AddProduct(Product product)
		{
			context.Products.Add(product);
		}

		public void DeleteProduct(Product product)
		{
			context.Products.Remove(product);
		}

		public async Task<IReadOnlyList<string>> GetBrandsAsynce()
		{
			return await context.Products.Select(x=>x.Brand)
				.Distinct()
				.ToListAsync();
		}

		public async Task<Product?> GetProductByIdAsync(int id)
		{
			return await context.Products.FindAsync(id);
		}

		public async Task<IReadOnlyList<Product>> GetProductsAsync(string? brand, string? type, string? sort)
		{
			var query = context.Products.AsQueryable();
			if(!string.IsNullOrWhiteSpace(brand) )
				query = query.Where(x=> x.Brand == brand);

			if(!string.IsNullOrWhiteSpace(type) )
				query = query.Where(x=>x.type == type);

			
				query=sort switch
				{
					"priceAsc" => query.OrderBy(x=>x.Price),
					"priceDsce" => query.OrderByDescending(x=>x.Price),
					_=>query.OrderBy(x=>x.Name)
				};

			return await query.ToListAsync();
		}

		public async Task<IReadOnlyList<string>> GetTypesAsynce()
		{
			return await context.Products.Select(x => x.type)
				.Distinct()
				.ToListAsync();
		}

		public bool ProductExist(int id)
		{
			return context.Products.Any(x => x.Id == id);
		}

		public async Task<bool> SaveChangeAsync()
		{
			return await context.SaveChangesAsync()>0;
		}

		public void UpdateProduct(Product product)
		{
			context.Entry(product).State = EntityState.Modified;
		}
	}
}