using ClassLibraryModel;

namespace BusinessLogicLayer.Interfaces;

public interface IProductService
{
    Task<List<Products>> GetAllAsync();
    Task<Products?> GetByIdAsync(Guid id);
    Task CreateAsync(Products product);
    Task UpdateAsync(Products product);
    Task DeleteAsync(Guid id);
    Task<List<Products>> SearchAsync(string keyword);
}