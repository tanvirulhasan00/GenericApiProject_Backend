using GenericApiProject.Database.Data;
using GenericApiProject.Models.GenericModels;
using GenericApiProject.Services.IService;
using Microsoft.EntityFrameworkCore;

namespace GenericApiProject.Services.Service;

public class Service<T>(GenericApiDbContext db) : IService<T> where T : class
{
    private readonly DbSet<T> _dbSet = db.Set<T>();
    public async Task<IEnumerable<T>> GetAllAsync(GenericServiceRequest<T> request)
    {
        var query = request.NoTracking ? _dbSet.AsNoTracking() : _dbSet;
        if(request.Expression != null)
        {
            query = query.Where(request.Expression);
        }
        // Include navigation properties using LINQ Aggregate
        if (!string.IsNullOrWhiteSpace(request.IncludeProperties))
        {
            query = request.IncludeProperties
                .Split(',', StringSplitOptions.RemoveEmptyEntries)
                .Select(p => p.Trim())
                .Aggregate(query, (current, include) => current.Include(include));
        }
        return await query.ToListAsync(request.CancellationToken);
    }

    public async Task<T> GetAsync(GenericServiceRequest<T> request)
    {
        var query = request.NoTracking ? _dbSet.AsNoTracking() : _dbSet;
        if(request.Expression != null)
        {
            query = query.Where(request.Expression);
        }
        // Include navigation properties using LINQ Aggregate
        if (!string.IsNullOrWhiteSpace(request.IncludeProperties))
        {
            query = request.IncludeProperties
                .Split(',', StringSplitOptions.RemoveEmptyEntries)
                .Select(p => p.Trim())
                .Aggregate(query, (current, include) => current.Include(include));
        }
        return await query.FirstOrDefaultAsync(request.CancellationToken) ?? null!;
    }

    public async Task AddAsync(T entity)
    {
        await _dbSet.AddAsync(entity);
    }

    public async Task<bool> AnyAsync(GenericServiceRequest<T> request)
    {
        var query = request.NoTracking ? _dbSet.AsNoTracking() : _dbSet;
        if (request.Expression != null)
        {
            return await query.AnyAsync(request.Expression,request.CancellationToken);
        }
        return await query.AnyAsync(request.CancellationToken);
    }

    public void Remove(T entity)
    {
        _dbSet.Remove(entity);
    }

    public void RemoveRange(IEnumerable<T> entities)
    {
        _dbSet.RemoveRange(entities);
    }
}