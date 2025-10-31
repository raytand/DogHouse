using DogHouse.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace DogHouse.Api.Repositories
{
    public interface IDogRepository
    {
        Task<List<Dog>> GetAllAsync();
        Task<Dog> GetByNameAsync(string name);
        Task AddAsync(Dog dog);
        Task<int> CountAsync();
        Task SaveChangesAsync();
        IQueryable<Dog> Query();
    }

    public class DogRepository : IDogRepository
    {
        private readonly Data.DogHouseContext _ctx;
        public DogRepository(Data.DogHouseContext ctx) => _ctx = ctx;

        public async Task<List<Dog>> GetAllAsync() => await _ctx.Dogs.ToListAsync();

        public async Task<Dog> GetByNameAsync(string name) => await _ctx.Dogs.FindAsync(name);

        public async Task AddAsync(Dog dog) => await _ctx.Dogs.AddAsync(dog);

        public async Task<int> CountAsync() => await _ctx.Dogs.CountAsync();

        public async Task SaveChangesAsync() => await _ctx.SaveChangesAsync();

        public IQueryable<Dog> Query() => _ctx.Dogs.AsQueryable();
    }
}