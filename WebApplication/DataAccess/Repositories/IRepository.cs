// WebApplication/DataAccess/Repositories/IRepository.cs

using System.Linq.Expressions;

namespace WebApplication.DataAccess.Repositories;

/// <summary>
/// Generic repository interface defining the base data access contract
/// for all entity repositories in the WebApplication.
/// <para>
/// Specific repositories extend this interface with domain-specific query
/// methods. Services depend on specific repository interfaces — never on
/// this base interface directly, except in generic infrastructure code.
/// </para>
/// <para>
/// <b>IQueryable is never exposed</b> outside the repository boundary.
/// All methods return materialised collections or single entities only.
/// </para>
/// </summary>
/// <typeparam name="T">The EF Core entity type. Must be a reference type.</typeparam>
public interface IRepository<T> where T : class
{
    /// <summary>
    /// Returns all rows for this entity type as a read-only list.
    /// Results are untracked — use for read-only scenarios only.
    /// </summary>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>All rows, or an empty list if the table is empty.</returns>
    Task<IReadOnlyList<T>> GetAllAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Returns a single entity by its integer primary key, or <c>null</c>
    /// if no row with that key exists.
    /// </summary>
    /// <param name="id">The primary key value to look up.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The entity, or <c>null</c> if not found.</returns>
    Task<T?> GetByIdAsync(int id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Inserts a new entity row and persists it to the database.
    /// </summary>
    /// <param name="entity">The entity to insert. Must not be null.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    Task AddAsync(T entity, CancellationToken cancellationToken = default);

    /// <summary>
    /// Marks an existing entity as modified and persists the changes.
    /// The entity must already be tracked or must have a valid PK value.
    /// </summary>
    /// <param name="entity">The entity with updated values. Must not be null.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    Task UpdateAsync(T entity, CancellationToken cancellationToken = default);

    /// <summary>
    /// Deletes the entity with the specified primary key.
    /// </summary>
    /// <param name="id">The primary key of the entity to delete.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <exception cref="InvalidOperationException">
    /// Thrown when no entity with <paramref name="id"/> exists.
    /// </exception>
    Task DeleteAsync(int id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Returns all entities matching the given predicate as a read-only list.
    /// Results are untracked — use for read-only scenarios only.
    /// </summary>
    /// <param name="predicate">A LINQ expression to filter entities.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>All matching entities, or an empty list if none match.</returns>
    Task<IReadOnlyList<T>> FindAsync(
        Expression<Func<T, bool>> predicate,
        CancellationToken cancellationToken = default);
}