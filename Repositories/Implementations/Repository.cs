using Microsoft.EntityFrameworkCore;
using SmartBookmarkApi.Data;
using SmartBookmarkApi.Models.Base;
using SmartBookmarkApi.Repositories.Interfaces;
using SmartBookmarkApi.Utilities;

namespace SmartBookmarkApi.Repositories.Implementations
{
    // The generic Repository class provides basic CRUD operations for any entity.
    // It isolates all Entity Framework Core database logic, so the service layer
    // doesn’t need to know how the data is actually stored or retrieved.
    // This makes the application easier to maintain, test, and extend.
    public class Repository<T> : IRepository<T> where T : class, IEntity
    {
        protected readonly AppDbContext _context;
        protected readonly ILogger _logger;
        protected readonly DbSet<T> _dbSet;

        public Repository(AppDbContext context, ILogger logger)
        {
            _context = context;
            _logger = logger;
            _dbSet = _context.Set<T>();
        }

        public virtual async Task<OperationResultOfT<T>> AddAsync(T entity, CancellationToken cancellationToken)
        {
            try
            {
                await _dbSet.AddAsync(entity, cancellationToken);
                await _context.SaveChangesAsync(cancellationToken);
                return new OperationResultOfT<T> { Success = true, Data = entity };
            }
            catch (Exception ex) when (ex is DbUpdateConcurrencyException or DbUpdateException)
            {
                _logger.LogError(ex, "Failed to add entity");
                return new OperationResultOfT<T> { Success = false, ErrorMessage = ex.Message };
            }
            catch (Exception ex) // general fallback for unexpected exceptions
            {
                _logger.LogError(ex, "Unexpected error occurred while adding {entity}.", typeof(T).Name);
                return new OperationResultOfT<T> { Success = false, ErrorMessage = ex.Message };
            }
        }

        public virtual async Task<OperationResultOfT<List<T>>> GetAllAsync(CancellationToken cancellationToken)
        {
            try
            {
                var allEntities = await _dbSet.AsNoTracking().ToListAsync(cancellationToken);

                return new OperationResultOfT<List<T>>
                {
                    Success = true,
                    Data = allEntities
                };
            }
            catch (Exception ex) when (ex is ArgumentNullException)
            {
                _logger.LogError(ex, "Failed to get {entity}s", typeof(T).Name);
                return new OperationResultOfT<List<T>>
                {
                    Success = false,
                    Data = Enumerable.Empty<T>().ToList()
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error occurred while getting {entity}s.", typeof(T).Name);
                return new OperationResultOfT<List<T>>
                {
                    Success = false,
                    Data = Enumerable.Empty<T>().ToList()
                };
            }
        }

        public virtual async Task<OperationResultOfT<T>> GetByIdAsync(int id, CancellationToken cancellationToken)
        {
            try
            {
                var entity = await _dbSet.AsNoTracking().FirstOrDefaultAsync(e => e.Id == id, cancellationToken);
                return new OperationResultOfT<T>
                {
                    Success = false,
                    Data = entity
                };
            }
            catch (Exception ex) when (ex is ArgumentNullException)
            {
                _logger.LogError(ex, "Failed to get {entity} with ID {EntityId}", typeof(T).Name, id);
                return new OperationResultOfT<T>
                {
                    Success = false,
                    Data = null
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error occurred while getting {entity} with ID {EntityId}", typeof(T).Name, id);
                return new OperationResultOfT<T>
                {
                    Success = false,
                    Data = null
                };
            }
        }

        public virtual async Task<OperationResult> UpdateAsync(int id, T updatedEntity, CancellationToken cancellationToken)
        {
            try
            {
                var existing = await _dbSet.FindAsync(id, cancellationToken);

                if (existing != null)
                {
                    _context.Entry(existing).CurrentValues.SetValues(updatedEntity);

                    await _context.SaveChangesAsync(cancellationToken);

                    return new OperationResult { Success = true };
                }

                return new OperationResult { Success = false, ErrorMessage = $"{typeof(T).Name} with ID {id} not found" };
            }
            catch (Exception ex) when (ex is DbUpdateConcurrencyException or DbUpdateException)
            {
                _logger.LogError(ex, "Failed to update {entity} with ID {EntityId}", typeof(T).Name, id);
                return new OperationResult { Success = false, ErrorMessage = ex.Message };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error occurred while adding {entity} with ID {EntityId}", typeof(T).Name, id);
                return new OperationResult { Success = false, ErrorMessage = ex.Message };
            }
        }

        /// <summary>
        /// Updates an already-loaded entity instance.
        /// This overload is used when the entity is already retrieved and modified
        /// within the current unit of work to avoid an additional database fetch.
        /// </summary>
        public virtual async Task<OperationResult> UpdateAsync(T entity, CancellationToken cancellationToken)
        {
            try
            {
                _dbSet.Update(entity);
                await _context.SaveChangesAsync(cancellationToken);

                return new OperationResult { Success = true };
            }
            catch (DbUpdateConcurrencyException ex)
            {
                _logger.LogError(ex, "Concurrency error while updating {entity}", typeof(T).Name);
                return new OperationResult { Success = false, ErrorMessage = ex.Message };
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "Database error while updating {entity}", typeof(T).Name);
                return new OperationResult { Success = false, ErrorMessage = ex.Message };
            }
        }

        public virtual async Task<OperationResult> RemoveAsync(int id, CancellationToken cancellationToken)
        {
            try
            {
                var existing = await _dbSet.FindAsync(id, cancellationToken);

                if (existing != null)
                {
                    _dbSet.Remove(existing);

                    await _context.SaveChangesAsync(cancellationToken);

                    return new OperationResult { Success = true };
                }

                return new OperationResult { Success = false, ErrorMessage = $"{typeof(T).Name} with ID {id} not found" };
            }
            catch (Exception ex) when (ex is DbUpdateConcurrencyException or DbUpdateException)
            {
                _logger.LogError(ex, "Failed to remove {entity} with ID {EntityId}", typeof(T).Name, id);
                return new OperationResult { Success = false, ErrorMessage = ex.Message };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error occurred while removing {entity} with ID {EntityId}", typeof(T).Name, id);
                return new OperationResult { Success = false, ErrorMessage = ex.Message };
            }
        }
    }
}