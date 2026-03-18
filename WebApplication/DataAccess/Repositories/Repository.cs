// WebApplication/DataAccess/Repositories/Repository.cs

using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using WebApplication.DataAccess.Context;

namespace WebApplication.DataAccess.Repositories;

/// <summary>
/// Generic EF Core repository implementation providing default CRUD operations
/// for all entity types. All specific repositories inherit from this class and
/// add domain-specific query methods on top.
/// <para>
/// <b>Tracking policy:</b> <see cref="GetAllAsync"/> and <see cref="FindAsync"/>
/// use <c>AsNoTracking()</c> — they are read-only operations and tracking would
/// waste memory and CPU. <see cref="GetByIdAsync"/> uses <c>FindAsync</c> which
/// checks the EF Core identity map first, making it safe for both read and
/// subsequent update scenarios.
/// </para>
/// <para>
/// <b>SaveChanges scope:</b> Each method calls <c>SaveChangesAsync</c>
/// immediately after its operation. For multi-table transactions (e.g.
/// CreateOrder, ReceiveStock), specific repositories use
/// <see cref="Context"/> directly inside a <c>BeginTransactionAsync</c> block
/// rather than relying on the base methods.
/// </para>
/// </summary>
/// <typeparam name="T">The EF Core entity type. Must be a reference type.</typeparam>
public class Repository<T> : IRepository<T> where T : class
{
    /// <summary>
    /// The EF Core database context. Exposed as protected so specific
    /// repositories can access it directly for complex queries and
    /// multi-table transactions without breaking encapsulation.
    /// </summary>
    protected readonly AppDbContext Context;

    /// <summary>
    /// The EF Core DbSet for entity type <typeparamref name="T"/>.
    /// Exposed as protected so specific repositories can build queries
    /// on top of it without duplicating the DbSet reference.
    /// </summary>
    protected readonly DbSet<T> DbSet;

    /// <summary>
    /// Initialises the repository with the injected EF Core context.
    /// </summary>
    /// <param name="context">
    /// The application's EF Core database context.
    /// Injected via constructor DI — registered as scoped in Program.cs.
    /// </param>
    public Repository(AppDbContext context)
    {
        Context = context ?? throw new ArgumentNullException(nameof(context));
        DbSet = context.Set<T>();
    }

    /// <inheritdoc/>
    public async Task<IReadOnlyList<T>> GetAllAsync(
        CancellationToken cancellationToken = default)
    {
        return await DbSet
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }

    /// <inheritdoc/>
    public async Task<T?> GetByIdAsync(
        int id,
        CancellationToken cancellationToken = default)
    {
        return await DbSet.FindAsync(new object[] { id }, cancellationToken);
    }

    /// <inheritdoc/>
    public async Task AddAsync(
        T entity,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(entity);

        await DbSet.AddAsync(entity, cancellationToken);
        await Context.SaveChangesAsync(cancellationToken);
    }

    /// <inheritdoc/>
    public async Task UpdateAsync(
        T entity,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(entity);

        DbSet.Update(entity);
        await Context.SaveChangesAsync(cancellationToken);
    }

    /// <inheritdoc/>
    /// <exception cref="InvalidOperationException">
    /// Thrown when no entity with <paramref name="id"/> exists in the database.
    /// </exception>
    public async Task DeleteAsync(
        int id,
        CancellationToken cancellationToken = default)
    {
        T? entity = await DbSet.FindAsync(new object[] { id }, cancellationToken);

        if (entity is null)
            throw new InvalidOperationException(
                $"Cannot delete {typeof(T).Name} with id {id}: entity not found.");

        DbSet.Remove(entity);
        await Context.SaveChangesAsync(cancellationToken);
    }

    /// <inheritdoc/>
    public async Task<IReadOnlyList<T>> FindAsync(
        Expression<Func<T, bool>> predicate,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(predicate);

        return await DbSet
            .AsNoTracking()
            .Where(predicate)
            .ToListAsync(cancellationToken);
    }
}