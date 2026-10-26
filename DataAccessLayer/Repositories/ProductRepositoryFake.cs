using ClassLibraryModel;
using DataAccessLayer.Interfaces;

namespace DataAccessLayer.Repositories;

public class ProductRepositoryFake : IProductRepository
{
    private readonly List<Products> _products = new()
    {
        new Products { ProductId = Guid.NewGuid(), Name = "Fake Laptop", Price = 100000, StockQuantity = 10, Description = "Fake data for testing", CreatedAt = DateTimeOffset.UtcNow },
        new Products { ProductId = Guid.NewGuid(), Name = "Fake Mouse", Price = 2000, StockQuantity = 25, Description = "Fake data for testing", CreatedAt = DateTimeOffset.UtcNow }
    };

    public Task<List<Products>> GetAllAsync() => Task.FromResult(_products);

    public Task<Products?> GetByIdAsync(Guid id) =>
        Task.FromResult(_products.FirstOrDefault(p => p.ProductId == id));

    public Task AddAsync(Products product)
    {
        _products.Add(product);
        return Task.CompletedTask;
    }

    public Task UpdateAsync(Products product)
    {
        var existing = _products.FirstOrDefault(p => p.ProductId == product.ProductId);
        if (existing != null)
        {
            existing.Name = product.Name;
            existing.Price = product.Price;
            existing.StockQuantity = product.StockQuantity;
            existing.Description = product.Description;
        }
        return Task.CompletedTask;
    }

    public Task DeleteAsync(Guid id)
    {
        var existing = _products.FirstOrDefault(p => p.ProductId == id);
        if (existing != null)
        {
            _products.Remove(existing);
        }
        return Task.CompletedTask;
    }

    public Task<List<Products>> SearchAsync(string keyword)
    {
        if (string.IsNullOrWhiteSpace(keyword))
            return Task.FromResult(_products);

        var result = _products.Where(p =>
            p.Name.Contains(keyword, StringComparison.OrdinalIgnoreCase) ||
            (p.Description != null && p.Description.Contains(keyword, StringComparison.OrdinalIgnoreCase))
        ).ToList();

        return Task.FromResult(result);
    }
}