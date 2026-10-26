using ClassLibraryModel;

namespace DataAccessLayer.Interfaces;

public interface IProductRepository
{
    Task<List<Products>> GetAllAsync();
    Task<Products?> GetByIdAsync(Guid id);
    Task AddAsync(Products product);
    Task UpdateAsync(Products product);
    Task DeleteAsync(Guid id);
    Task<List<Products>> SearchAsync(string keyword);
}