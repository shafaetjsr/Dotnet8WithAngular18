﻿using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interfaces
{
	public interface IProductRepository
	{
		Task<IReadOnlyList<Product>> GetProductsAsync( string ? brand, string? type,string? sort);
		Task<Product?> GetProductByIdAsync(int id);
		Task<IReadOnlyList<string>> GetBrandsAsynce();
		Task<IReadOnlyList<string>> GetTypesAsynce();
		void AddProduct(Product product);
		void UpdateProduct(Product product);
		void DeleteProduct(Product product);
		bool ProductExist(int id);
		Task<bool> SaveChangeAsync();
	}
}
 