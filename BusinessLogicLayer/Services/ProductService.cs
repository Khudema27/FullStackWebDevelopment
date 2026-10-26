using ClassLibraryModel;
using DataAccessLayer.Interfaces;
using BusinessLogicLayer.Interfaces;

namespace BusinessLogicLayer.Services;

public class ProductService : IProductService
{
    private readonly IProductRepository _repository;

    public ProductService(IProductRepository repository)
    {
        _repository = repository;
    }

    public async Task<List<Products>> GetAllAsync()
    {
        return await _repository.GetAllAsync();
    }

    public async Task<Products?> GetByIdAsync(Guid id)
    {
        return await _repository.GetByIdAsync(id);
    }

    public async Task CreateAsync(Products product)
    {
        if (product.Price < 0)
            throw new ArgumentException("Price cannot be negative.");

        if (product.StockQuantity < 0)
            throw new ArgumentException("Stock quantity cannot be negative.");

        await _repository.AddAsync(product);
    }

    public async Task UpdateAsync(Products product)
    {
        if (product.Price < 0)
            throw new ArgumentException("Price cannot be negative.");

        if (product.StockQuantity < 0)
            throw new ArgumentException("Stock quantity cannot be negative.");

        await _repository.UpdateAsync(product);
    }

    public async Task DeleteAsync(Guid id)
    {
        await _repository.DeleteAsync(id);
    }

    public async Task<List<Products>> SearchAsync(string keyword)
    {
        return await _repository.SearchAsync(keyword);
    }
}