using DogHouse.Api.DTOs;
using DogHouse.Api.Models;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace DogHouse.Api.Services
{
    public interface IDogService
    {
        Task<(List<Dog>, int total)> GetDogsAsync(string? attribute, string? order, int pageNumber, int pageSize);
        Task CreateDogAsync(CreateDogDto dto);
    }

    public class DogService : IDogService
    {
        private readonly Repositories.IDogRepository _repo;
        private readonly IMapper _mapper;

        public DogService(Repositories.IDogRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        public async Task<(List<Dog>, int total)> GetDogsAsync(string? attribute, string? order, int pageNumber, int pageSize)
        {
            var query = _repo.Query();
            bool desc = string.Equals(order, "desc", StringComparison.OrdinalIgnoreCase);
            query = attribute?.ToLower() switch
            {
                "weight" => desc ? query.OrderByDescending(d => d.Weight) : query.OrderBy(d => d.Weight),
                "tail_length" => desc ? query.OrderByDescending(d => d.TailLength) : query.OrderBy(d => d.TailLength),
                "name" => desc ? query.OrderByDescending(d => d.Name) : query.OrderBy(d => d.Name),
                "color" => desc ? query.OrderByDescending(d => d.Color) : query.OrderBy(d => d.Color),
                null or "" => query.OrderBy(d => d.Name),
                _ => throw new ArgumentException($"Unknown attribute '{attribute}'")
            };

            var total = await query.CountAsync();

            if (pageNumber < 1) pageNumber = 1;
            if (pageSize <= 0) pageSize = 10;

            var items = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (items, total);
        }

        public async Task CreateDogAsync(CreateDogDto dto)
        {
            if (dto == null)
            {
                throw new ArgumentNullException(nameof(dto));
            }
            if (string.IsNullOrWhiteSpace(dto.Name))
            {
                throw new ArgumentException("Name is required");
            }
            if (!dto.TailLength.HasValue)
            {
                throw new ArgumentException("tail_length is required and must be a number");
            }
            if (!dto.Weight.HasValue)
            {
                throw new ArgumentException("weight is required and must be a number");
            }
            if (dto.TailLength < 0)
            {
                throw new ArgumentException("tail_length cannot be negative");
            }
            if (dto.Weight < 0)
            {
                throw new ArgumentException("weight cannot be negative");
            }
            var exists = await _repo.GetByNameAsync(dto.Name);
            if (exists != null)
                throw new InvalidOperationException("Dog with the same name already exists");

            var dog = _mapper.Map<Dog>(dto);
            await _repo.AddAsync(dog);
            await _repo.SaveChangesAsync();
        }
    }
}