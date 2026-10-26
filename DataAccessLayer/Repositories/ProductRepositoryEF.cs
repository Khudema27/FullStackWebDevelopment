using ClassLibraryModel;
using DataAccessLayer.Data;
using DataAccessLayer.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DataAccessLayer.Repositories;

public class ProductRepositoryEF : IProductRepository
{
    private readonly IDbContextFactory<AppDbContext> _factory;

    public ProductRepositoryEF(IDbContextFactory<AppDbContext> factory)
    {
        _factory = factory;
    }

    public async Task<List<Products>> GetAllAsync()
    {
        await using var db = await _factory.CreateDbContextAsync();
        return await db.Products
            .AsNoTracking()
            .OrderBy(p => p.Name)
            .ToListAsync();
    }

    public async Task<Products?> GetByIdAsync(Guid id)
    {
        await using var db = await _factory.CreateDbContextAsync();
        return await db.Products.FindAsync(id);
    }

    public async Task AddAsync(Products product)
    {
        await using var db = await _factory.CreateDbContextAsync();
        db.Products.Add(product);
        await db.SaveChangesAsync();
    }

    public async Task UpdateAsync(Products product)
    {
        await using var db = await _factory.CreateDbContextAsync();
        db.Products.Update(product);
        await db.SaveChangesAsync();
    }

    public async Task DeleteAsync(Guid id)
    {
        await using var db = await _factory.CreateDbContextAsync();
        var entity = await db.Products.FindAsync(id);
        if (entity != null)
        {
            db.Products.Remove(entity);
            await db.SaveChangesAsync();
        }
    }

    public async Task<List<Products>> SearchAsync(string keyword)
    {
        await using var db = await _factory.CreateDbContextAsync();
        if (string.IsNullOrWhiteSpace(keyword))
            return await GetAllAsync();

        return await db.Products
            .AsNoTracking()
            .Where(p => p.Name.Contains(keyword) || (p.Description != null && p.Description.Contains(keyword)))
            .OrderBy(p => p.Name)
            .ToListAsync();
    }
}