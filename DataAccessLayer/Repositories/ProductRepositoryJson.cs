using System.Text.Json;
using ClassLibraryModel;
using DataAccessLayer.Interfaces;

namespace DataAccessLayer.Repositories;

public class ProductRepositoryJson : IProductRepository
{
    private readonly string _filePath;

    public ProductRepositoryJson()
    {
        _filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data", "products.json");

        var directoryPath = Path.GetDirectoryName(_filePath);
        if (!string.IsNullOrEmpty(directoryPath) && !Directory.Exists(directoryPath))
        {
            Directory.CreateDirectory(directoryPath);
        }

        if (!File.Exists(_filePath))
        {
            File.WriteAllText(_filePath, "[]");
        }
    }

    private async Task<List<Products>> ReadFromFileAsync()
    {
        if (!File.Exists(_filePath))
            return new List<Products>();

        var json = await File.ReadAllTextAsync(_filePath);
        if (string.IsNullOrWhiteSpace(json))
            return new List<Products>();

        return JsonSerializer.Deserialize<List<Products>>(json) ?? new List<Products>();
    }

    private async Task WriteToFileAsync(List<Products> products)
    {
        var json = JsonSerializer.Serialize(products, new JsonSerializerOptions { WriteIndented = true });
        await File.WriteAllTextAsync(_filePath, json);
    }

    public async Task<List<Products>> GetAllAsync() => await ReadFromFileAsync();

    public async Task<Products?> GetByIdAsync(Guid id)
    {
        var products = await ReadFromFileAsync();
        return products.FirstOrDefault(p => p.ProductId == id);
    }

    public async Task AddAsync(Products product)
    {
        var products = await ReadFromFileAsync();
        products.Add(product);
        await WriteToFileAsync(products);
    }

    public async Task UpdateAsync(Products product)
    {
        var products = await ReadFromFileAsync();
        var index = products.FindIndex(p => p.ProductId == product.ProductId);
        if (index != -1)
        {
            products[index] = product;
            await WriteToFileAsync(products);
        }
    }

    public async Task DeleteAsync(Guid id)
    {
        var products = await ReadFromFileAsync();
        var product = products.FirstOrDefault(p => p.ProductId == id);
        if (product != null)
        {
            products.Remove(product);
            await WriteToFileAsync(products);
        }
    }

    public async Task<List<Products>> SearchAsync(string keyword)
    {
        var products = await ReadFromFileAsync();
        if (string.IsNullOrWhiteSpace(keyword))
            return products;

        return products.Where(p =>
            p.Name.Contains(keyword, StringComparison.OrdinalIgnoreCase) ||
            (p.Description != null && p.Description.Contains(keyword, StringComparison.OrdinalIgnoreCase))
        ).ToList();
    }
}