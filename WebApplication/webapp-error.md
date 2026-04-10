[WARNING] Google Cloud Storage unavailable: Your default credentials were not found. To set up Application Default Credentials, see https://cloud.google.com/docs/authentication/external/set-up-adc.
[WARNING] File upload features will be disabled. Set GOOGLE_APPLICATION_CREDENTIALS to enable.
info: Microsoft.AspNetCore.DataProtection.KeyManagement.XmlKeyManager[63]
      User profile is available. Using 'C:\Users\user\AppData\Local\ASP.NET\DataProtection-Keys' as key repository and Windows DPAPI to encrypt keys at rest.
info: WebApplication.BackgroundJobs.InventorySyncJob[0]
      InventorySyncJob started.
fail: WebApplication.BackgroundJobs.InventorySyncJob[0]
      InventorySyncJob cycle failed.
      System.ArgumentException: Keyword not supported: 'userid'.
         at Microsoft.Data.SqlClient.SqlConnectionStringBuilder.GetIndex(String keyword)
         at Microsoft.Data.SqlClient.SqlConnectionStringBuilder.set_Item(String keyword, Object value)
         at System.Data.Common.DbConnectionStringBuilder.set_ConnectionString(String value)
         at Microsoft.Data.SqlClient.SqlConnectionStringBuilder..ctor(String connectionString)
         at Microsoft.EntityFrameworkCore.SqlServer.Storage.Internal.SqlServerConnection.<>c.<get_IsMultipleActiveResultSetsEnabled>b__7_0(String cs)
         at System.Collections.Concurrent.ConcurrentDictionary`2.GetOrAdd(TKey key, Func`2 valueFactory)
         at Microsoft.EntityFrameworkCore.SqlServer.Storage.Internal.SqlServerConnection.get_IsMultipleActiveResultSetsEnabled()
         at Microsoft.EntityFrameworkCore.SqlServer.Query.Internal.SqlServerCompiledQueryCacheKeyGenerator.GenerateCacheKey(Expression query, Boolean async)
         at Microsoft.EntityFrameworkCore.Query.Internal.QueryCompiler.ExecuteAsync[TResult](Expression query, CancellationToken cancellationToken)
         at Microsoft.EntityFrameworkCore.Query.Internal.EntityQueryProvider.ExecuteAsync[TResult](Expression expression, CancellationToken cancellationToken)
         at Microsoft.EntityFrameworkCore.EntityFrameworkQueryableExtensions.ExecuteAsync[TSource,TResult](MethodInfo operatorMethodInfo, IQueryable`1 source, Expression expression, CancellationToken cancellationToken)
         at Microsoft.EntityFrameworkCore.EntityFrameworkQueryableExtensions.ExecuteAsync[TSource,TResult](MethodInfo operatorMethodInfo, IQueryable`1 source, CancellationToken cancellationToken)
         at Microsoft.EntityFrameworkCore.EntityFrameworkQueryableExtensions.CountAsync[TSource](IQueryable`1 source, CancellationToken cancellationToken)
         at WebApplication.BackgroundJobs.InventorySyncJob.RunCycleAsync(CancellationToken cancellationToken) in C:\Users\user\Documents\ANDREI_FILES\PDM_FILES\2ND YEAR\2ND SEM\OOP\OOP-TaurusBikeShop\WebApplication\BackgroundJobs\InventorySyncJob.cs:line 68
fail: Microsoft.EntityFrameworkCore.Update[10000]
      An exception occurred in the database while saving changes for context type 'WebApplication.DataAccess.Context.AppDbContext'.
      System.ArgumentException: Keyword not supported: 'userid'.
         at Microsoft.Data.Common.DbConnectionOptions.ParseInternal(Dictionary`2 parsetable, String connectionString, Boolean buildChain, Dictionary`2 synonyms, Boolean firstKey)
         at Microsoft.Data.Common.DbConnectionOptions..ctor(String connectionString, Dictionary`2 synonyms)
         at Microsoft.Data.SqlClient.SqlConnectionString..ctor(String connectionString)
         at Microsoft.Data.SqlClient.SqlConnectionFactory.CreateConnectionOptions(String connectionString, DbConnectionOptions previous)
         at Microsoft.Data.ProviderBase.DbConnectionFactory.GetConnectionPoolGroup(DbConnectionPoolKey key, DbConnectionPoolGroupOptions poolOptions, DbConnectionOptions& userConnectionOptions)
         at Microsoft.Data.SqlClient.SqlConnection.ConnectionString_Set(DbConnectionPoolKey key)
         at Microsoft.Data.SqlClient.SqlConnection.set_ConnectionString(String value)
         at Microsoft.Data.SqlClient.SqlConnection..ctor(String connectionString)
         at Microsoft.EntityFrameworkCore.SqlServer.Storage.Internal.SqlServerConnection.CreateDbConnection()
         at Microsoft.EntityFrameworkCore.Storage.RelationalConnection.get_DbConnection()
         at Microsoft.EntityFrameworkCore.Storage.RelationalConnection.OpenAsync(CancellationToken cancellationToken, Boolean errorsExpected)
         at Microsoft.EntityFrameworkCore.Update.Internal.BatchExecutor.ExecuteAsync(IEnumerable`1 commandBatches, IRelationalConnection connection, CancellationToken cancellationToken)
         at Microsoft.EntityFrameworkCore.Update.Internal.BatchExecutor.ExecuteAsync(IEnumerable`1 commandBatches, IRelationalConnection connection, CancellationToken cancellationToken)
         at Microsoft.EntityFrameworkCore.Update.Internal.BatchExecutor.ExecuteAsync(IEnumerable`1 commandBatches, IRelationalConnection connection, CancellationToken cancellationToken)
         at Microsoft.EntityFrameworkCore.ChangeTracking.Internal.StateManager.SaveChangesAsync(IList`1 entriesToSave, CancellationToken cancellationToken)
         at Microsoft.EntityFrameworkCore.ChangeTracking.Internal.StateManager.SaveChangesAsync(StateManager stateManager, Boolean acceptAllChangesOnSuccess, CancellationToken cancellationToken)
         at Microsoft.EntityFrameworkCore.Storage.ExecutionStrategy.<>c__DisplayClass30_0`2.<<ExecuteAsync>b__0>d.MoveNext()
      --- End of stack trace from previous location ---
         at Microsoft.EntityFrameworkCore.Storage.ExecutionStrategy.ExecuteImplementationAsync[TState,TResult](Func`4 operation, Func`4 verifySucceeded, TState state, CancellationToken cancellationToken)
         at Microsoft.EntityFrameworkCore.Storage.ExecutionStrategy.ExecuteImplementationAsync[TState,TResult](Func`4 operation, Func`4 verifySucceeded, TState state, CancellationToken cancellationToken)
         at Microsoft.EntityFrameworkCore.Storage.ExecutionStrategy.ExecuteAsync[TState,TResult](TState state, Func`4 operation, Func`4 verifySucceeded, CancellationToken cancellationToken)
         at Microsoft.EntityFrameworkCore.DbContext.SaveChangesAsync(Boolean acceptAllChangesOnSuccess, CancellationToken cancellationToken)
      System.ArgumentException: Keyword not supported: 'userid'.
         at Microsoft.Data.Common.DbConnectionOptions.ParseInternal(Dictionary`2 parsetable, String connectionString, Boolean buildChain, Dictionary`2 synonyms, Boolean firstKey)
         at Microsoft.Data.Common.DbConnectionOptions..ctor(String connectionString, Dictionary`2 synonyms)
         at Microsoft.Data.SqlClient.SqlConnectionString..ctor(String connectionString)
         at Microsoft.Data.SqlClient.SqlConnectionFactory.CreateConnectionOptions(String connectionString, DbConnectionOptions previous)
         at Microsoft.Data.ProviderBase.DbConnectionFactory.GetConnectionPoolGroup(DbConnectionPoolKey key, DbConnectionPoolGroupOptions poolOptions, DbConnectionOptions& userConnectionOptions)
         at Microsoft.Data.SqlClient.SqlConnection.ConnectionString_Set(DbConnectionPoolKey key)
         at Microsoft.Data.SqlClient.SqlConnection.set_ConnectionString(String value)
         at Microsoft.Data.SqlClient.SqlConnection..ctor(String connectionString)
         at Microsoft.EntityFrameworkCore.SqlServer.Storage.Internal.SqlServerConnection.CreateDbConnection()
         at Microsoft.EntityFrameworkCore.Storage.RelationalConnection.get_DbConnection()
         at Microsoft.EntityFrameworkCore.Storage.RelationalConnection.OpenAsync(CancellationToken cancellationToken, Boolean errorsExpected)
         at Microsoft.EntityFrameworkCore.Update.Internal.BatchExecutor.ExecuteAsync(IEnumerable`1 commandBatches, IRelationalConnection connection, CancellationToken cancellationToken)
         at Microsoft.EntityFrameworkCore.Update.Internal.BatchExecutor.ExecuteAsync(IEnumerable`1 commandBatches, IRelationalConnection connection, CancellationToken cancellationToken)
         at Microsoft.EntityFrameworkCore.Update.Internal.BatchExecutor.ExecuteAsync(IEnumerable`1 commandBatches, IRelationalConnection connection, CancellationToken cancellationToken)
         at Microsoft.EntityFrameworkCore.ChangeTracking.Internal.StateManager.SaveChangesAsync(IList`1 entriesToSave, CancellationToken cancellationToken)
         at Microsoft.EntityFrameworkCore.ChangeTracking.Internal.StateManager.SaveChangesAsync(StateManager stateManager, Boolean acceptAllChangesOnSuccess, CancellationToken cancellationToken)
         at Microsoft.EntityFrameworkCore.Storage.ExecutionStrategy.<>c__DisplayClass30_0`2.<<ExecuteAsync>b__0>d.MoveNext()
      --- End of stack trace from previous location ---
         at Microsoft.EntityFrameworkCore.Storage.ExecutionStrategy.ExecuteImplementationAsync[TState,TResult](Func`4 operation, Func`4 verifySucceeded, TState state, CancellationToken cancellationToken)
         at Microsoft.EntityFrameworkCore.Storage.ExecutionStrategy.ExecuteImplementationAsync[TState,TResult](Func`4 operation, Func`4 verifySucceeded, TState state, CancellationToken cancellationToken)
         at Microsoft.EntityFrameworkCore.Storage.ExecutionStrategy.ExecuteAsync[TState,TResult](TState state, Func`4 operation, Func`4 verifySucceeded, CancellationToken cancellationToken)
         at Microsoft.EntityFrameworkCore.DbContext.SaveChangesAsync(Boolean acceptAllChangesOnSuccess, CancellationToken cancellationToken)
fail: WebApplication.BackgroundJobs.InventorySyncJob[0]
      Failed to write error log for InventorySyncJob.
      System.ArgumentException: Keyword not supported: 'userid'.
         at Microsoft.Data.Common.DbConnectionOptions.ParseInternal(Dictionary`2 parsetable, String connectionString, Boolean buildChain, Dictionary`2 synonyms, Boolean firstKey)
         at Microsoft.Data.Common.DbConnectionOptions..ctor(String connectionString, Dictionary`2 synonyms)
         at Microsoft.Data.SqlClient.SqlConnectionString..ctor(String connectionString)
         at Microsoft.Data.SqlClient.SqlConnectionFactory.CreateConnectionOptions(String connectionString, DbConnectionOptions previous)
         at Microsoft.Data.ProviderBase.DbConnectionFactory.GetConnectionPoolGroup(DbConnectionPoolKey key, DbConnectionPoolGroupOptions poolOptions, DbConnectionOptions& userConnectionOptions)
         at Microsoft.Data.SqlClient.SqlConnection.ConnectionString_Set(DbConnectionPoolKey key)
         at Microsoft.Data.SqlClient.SqlConnection.set_ConnectionString(String value)
         at Microsoft.Data.SqlClient.SqlConnection..ctor(String connectionString)
         at Microsoft.EntityFrameworkCore.SqlServer.Storage.Internal.SqlServerConnection.CreateDbConnection()
         at Microsoft.EntityFrameworkCore.Storage.RelationalConnection.get_DbConnection()
         at Microsoft.EntityFrameworkCore.Storage.RelationalConnection.OpenAsync(CancellationToken cancellationToken, Boolean errorsExpected)
         at Microsoft.EntityFrameworkCore.Update.Internal.BatchExecutor.ExecuteAsync(IEnumerable`1 commandBatches, IRelationalConnection connection, CancellationToken cancellationToken)
         at Microsoft.EntityFrameworkCore.Update.Internal.BatchExecutor.ExecuteAsync(IEnumerable`1 commandBatches, IRelationalConnection connection, CancellationToken cancellationToken)
         at Microsoft.EntityFrameworkCore.Update.Internal.BatchExecutor.ExecuteAsync(IEnumerable`1 commandBatches, IRelationalConnection connection, CancellationToken cancellationToken)
         at Microsoft.EntityFrameworkCore.ChangeTracking.Internal.StateManager.SaveChangesAsync(IList`1 entriesToSave, CancellationToken cancellationToken)
         at Microsoft.EntityFrameworkCore.ChangeTracking.Internal.StateManager.SaveChangesAsync(StateManager stateManager, Boolean acceptAllChangesOnSuccess, CancellationToken cancellationToken)
         at Microsoft.EntityFrameworkCore.Storage.ExecutionStrategy.<>c__DisplayClass30_0`2.<<ExecuteAsync>b__0>d.MoveNext()
      --- End of stack trace from previous location ---
         at Microsoft.EntityFrameworkCore.Storage.ExecutionStrategy.ExecuteImplementationAsync[TState,TResult](Func`4 operation, Func`4 verifySucceeded, TState state, CancellationToken cancellationToken)
         at Microsoft.EntityFrameworkCore.Storage.ExecutionStrategy.ExecuteImplementationAsync[TState,TResult](Func`4 operation, Func`4 verifySucceeded, TState state, CancellationToken cancellationToken)
         at Microsoft.EntityFrameworkCore.Storage.ExecutionStrategy.ExecuteAsync[TState,TResult](TState state, Func`4 operation, Func`4 verifySucceeded, CancellationToken cancellationToken)
         at Microsoft.EntityFrameworkCore.DbContext.SaveChangesAsync(Boolean acceptAllChangesOnSuccess, CancellationToken cancellationToken)
         at Microsoft.EntityFrameworkCore.DbContext.SaveChangesAsync(Boolean acceptAllChangesOnSuccess, CancellationToken cancellationToken)
         at WebApplication.BackgroundJobs.InventorySyncJob.RunCycleAsync(CancellationToken cancellationToken) in C:\Users\user\Documents\ANDREI_FILES\PDM_FILES\2ND YEAR\2ND SEM\OOP\OOP-TaurusBikeShop\WebApplication\BackgroundJobs\InventorySyncJob.cs:line 112
         at WebApplication.BackgroundJobs.InventorySyncJob.RunCycleAsync(CancellationToken cancellationToken) in C:\Users\user\Documents\ANDREI_FILES\PDM_FILES\2ND YEAR\2ND SEM\OOP\OOP-TaurusBikeShop\WebApplication\BackgroundJobs\InventorySyncJob.cs:line 112
info: WebApplication.BackgroundJobs.PendingOrderMonitorJob[0]
      PendingOrderMonitorJob started.
fail: Microsoft.EntityFrameworkCore.Update[10000]
      An exception occurred in the database while saving changes for context type 'WebApplication.DataAccess.Context.AppDbContext'.
      System.ArgumentException: Keyword not supported: 'userid'.
         at Microsoft.Data.Common.DbConnectionOptions.ParseInternal(Dictionary`2 parsetable, String connectionString, Boolean buildChain, Dictionary`2 synonyms, Boolean firstKey)
         at Microsoft.Data.Common.DbConnectionOptions..ctor(String connectionString, Dictionary`2 synonyms)
         at Microsoft.Data.SqlClient.SqlConnectionString..ctor(String connectionString)
         at Microsoft.Data.SqlClient.SqlConnectionFactory.CreateConnectionOptions(String connectionString, DbConnectionOptions previous)
         at Microsoft.Data.ProviderBase.DbConnectionFactory.GetConnectionPoolGroup(DbConnectionPoolKey key, DbConnectionPoolGroupOptions poolOptions, DbConnectionOptions& userConnectionOptions)
         at Microsoft.Data.SqlClient.SqlConnection.ConnectionString_Set(DbConnectionPoolKey key)
         at Microsoft.Data.SqlClient.SqlConnection.set_ConnectionString(String value)
         at Microsoft.Data.SqlClient.SqlConnection..ctor(String connectionString)
         at Microsoft.EntityFrameworkCore.SqlServer.Storage.Internal.SqlServerConnection.CreateDbConnection()
         at Microsoft.EntityFrameworkCore.Storage.RelationalConnection.get_DbConnection()
         at Microsoft.EntityFrameworkCore.Storage.RelationalConnection.OpenAsync(CancellationToken cancellationToken, Boolean errorsExpected)
         at Microsoft.EntityFrameworkCore.Update.Internal.BatchExecutor.ExecuteAsync(IEnumerable`1 commandBatches, IRelationalConnection connection, CancellationToken cancellationToken)
         at Microsoft.EntityFrameworkCore.Update.Internal.BatchExecutor.ExecuteAsync(IEnumerable`1 commandBatches, IRelationalConnection connection, CancellationToken cancellationToken)
         at Microsoft.EntityFrameworkCore.Update.Internal.BatchExecutor.ExecuteAsync(IEnumerable`1 commandBatches, IRelationalConnection connection, CancellationToken cancellationToken)
         at Microsoft.EntityFrameworkCore.ChangeTracking.Internal.StateManager.SaveChangesAsync(IList`1 entriesToSave, CancellationToken cancellationToken)
         at Microsoft.EntityFrameworkCore.ChangeTracking.Internal.StateManager.SaveChangesAsync(StateManager stateManager, Boolean acceptAllChangesOnSuccess, CancellationToken cancellationToken)
         at Microsoft.EntityFrameworkCore.Storage.ExecutionStrategy.<>c__DisplayClass30_0`2.<<ExecuteAsync>b__0>d.MoveNext()
      --- End of stack trace from previous location ---
         at Microsoft.EntityFrameworkCore.Storage.ExecutionStrategy.ExecuteImplementationAsync[TState,TResult](Func`4 operation, Func`4 verifySucceeded, TState state, CancellationToken cancellationToken)
         at Microsoft.EntityFrameworkCore.Storage.ExecutionStrategy.ExecuteImplementationAsync[TState,TResult](Func`4 operation, Func`4 verifySucceeded, TState state, CancellationToken cancellationToken)
         at Microsoft.EntityFrameworkCore.Storage.ExecutionStrategy.ExecuteAsync[TState,TResult](TState state, Func`4 operation, Func`4 verifySucceeded, CancellationToken cancellationToken)
         at Microsoft.EntityFrameworkCore.DbContext.SaveChangesAsync(Boolean acceptAllChangesOnSuccess, CancellationToken cancellationToken)
      System.ArgumentException: Keyword not supported: 'userid'.
         at Microsoft.Data.Common.DbConnectionOptions.ParseInternal(Dictionary`2 parsetable, String connectionString, Boolean buildChain, Dictionary`2 synonyms, Boolean firstKey)
         at Microsoft.Data.Common.DbConnectionOptions..ctor(String connectionString, Dictionary`2 synonyms)
         at Microsoft.Data.SqlClient.SqlConnectionString..ctor(String connectionString)
         at Microsoft.Data.SqlClient.SqlConnectionFactory.CreateConnectionOptions(String connectionString, DbConnectionOptions previous)
         at Microsoft.Data.ProviderBase.DbConnectionFactory.GetConnectionPoolGroup(DbConnectionPoolKey key, DbConnectionPoolGroupOptions poolOptions, DbConnectionOptions& userConnectionOptions)
         at Microsoft.Data.SqlClient.SqlConnection.ConnectionString_Set(DbConnectionPoolKey key)
         at Microsoft.Data.SqlClient.SqlConnection.set_ConnectionString(String value)
         at Microsoft.Data.SqlClient.SqlConnection..ctor(String connectionString)
         at Microsoft.EntityFrameworkCore.SqlServer.Storage.Internal.SqlServerConnection.CreateDbConnection()
         at Microsoft.EntityFrameworkCore.Storage.RelationalConnection.get_DbConnection()
         at Microsoft.EntityFrameworkCore.Storage.RelationalConnection.OpenAsync(CancellationToken cancellationToken, Boolean errorsExpected)
         at Microsoft.EntityFrameworkCore.Update.Internal.BatchExecutor.ExecuteAsync(IEnumerable`1 commandBatches, IRelationalConnection connection, CancellationToken cancellationToken)
         at Microsoft.EntityFrameworkCore.Update.Internal.BatchExecutor.ExecuteAsync(IEnumerable`1 commandBatches, IRelationalConnection connection, CancellationToken cancellationToken)
         at Microsoft.EntityFrameworkCore.Update.Internal.BatchExecutor.ExecuteAsync(IEnumerable`1 commandBatches, IRelationalConnection connection, CancellationToken cancellationToken)
         at Microsoft.EntityFrameworkCore.ChangeTracking.Internal.StateManager.SaveChangesAsync(IList`1 entriesToSave, CancellationToken cancellationToken)
         at Microsoft.EntityFrameworkCore.ChangeTracking.Internal.StateManager.SaveChangesAsync(StateManager stateManager, Boolean acceptAllChangesOnSuccess, CancellationToken cancellationToken)
         at Microsoft.EntityFrameworkCore.Storage.ExecutionStrategy.<>c__DisplayClass30_0`2.<<ExecuteAsync>b__0>d.MoveNext()
      --- End of stack trace from previous location ---
         at Microsoft.EntityFrameworkCore.Storage.ExecutionStrategy.ExecuteImplementationAsync[TState,TResult](Func`4 operation, Func`4 verifySucceeded, TState state, CancellationToken cancellationToken)
         at Microsoft.EntityFrameworkCore.Storage.ExecutionStrategy.ExecuteImplementationAsync[TState,TResult](Func`4 operation, Func`4 verifySucceeded, TState state, CancellationToken cancellationToken)
         at Microsoft.EntityFrameworkCore.Storage.ExecutionStrategy.ExecuteAsync[TState,TResult](TState state, Func`4 operation, Func`4 verifySucceeded, CancellationToken cancellationToken)
         at Microsoft.EntityFrameworkCore.DbContext.SaveChangesAsync(Boolean acceptAllChangesOnSuccess, CancellationToken cancellationToken)
warn: WebApplication.BackgroundJobs.PendingOrderMonitorJob[0]
      PendingOrderMonitorJob could not write start log - DB may be unavailable.
      System.ArgumentException: Keyword not supported: 'userid'.
         at Microsoft.Data.Common.DbConnectionOptions.ParseInternal(Dictionary`2 parsetable, String connectionString, Boolean buildChain, Dictionary`2 synonyms, Boolean firstKey)
         at Microsoft.Data.Common.DbConnectionOptions..ctor(String connectionString, Dictionary`2 synonyms)
         at Microsoft.Data.SqlClient.SqlConnectionString..ctor(String connectionString)
         at Microsoft.Data.SqlClient.SqlConnectionFactory.CreateConnectionOptions(String connectionString, DbConnectionOptions previous)
         at Microsoft.Data.ProviderBase.DbConnectionFactory.GetConnectionPoolGroup(DbConnectionPoolKey key, DbConnectionPoolGroupOptions poolOptions, DbConnectionOptions& userConnectionOptions)
         at Microsoft.Data.SqlClient.SqlConnection.ConnectionString_Set(DbConnectionPoolKey key)
         at Microsoft.Data.SqlClient.SqlConnection.set_ConnectionString(String value)
         at Microsoft.Data.SqlClient.SqlConnection..ctor(String connectionString)
         at Microsoft.EntityFrameworkCore.SqlServer.Storage.Internal.SqlServerConnection.CreateDbConnection()
         at Microsoft.EntityFrameworkCore.Storage.RelationalConnection.get_DbConnection()
         at Microsoft.EntityFrameworkCore.Storage.RelationalConnection.OpenAsync(CancellationToken cancellationToken, Boolean errorsExpected)
         at Microsoft.EntityFrameworkCore.Update.Internal.BatchExecutor.ExecuteAsync(IEnumerable`1 commandBatches, IRelationalConnection connection, CancellationToken cancellationToken)
         at Microsoft.EntityFrameworkCore.Update.Internal.BatchExecutor.ExecuteAsync(IEnumerable`1 commandBatches, IRelationalConnection connection, CancellationToken cancellationToken)
         at Microsoft.EntityFrameworkCore.Update.Internal.BatchExecutor.ExecuteAsync(IEnumerable`1 commandBatches, IRelationalConnection connection, CancellationToken cancellationToken)
         at Microsoft.EntityFrameworkCore.ChangeTracking.Internal.StateManager.SaveChangesAsync(IList`1 entriesToSave, CancellationToken cancellationToken)
         at Microsoft.EntityFrameworkCore.ChangeTracking.Internal.StateManager.SaveChangesAsync(StateManager stateManager, Boolean acceptAllChangesOnSuccess, CancellationToken cancellationToken)
         at Microsoft.EntityFrameworkCore.Storage.ExecutionStrategy.<>c__DisplayClass30_0`2.<<ExecuteAsync>b__0>d.MoveNext()
      --- End of stack trace from previous location ---
         at Microsoft.EntityFrameworkCore.Storage.ExecutionStrategy.ExecuteImplementationAsync[TState,TResult](Func`4 operation, Func`4 verifySucceeded, TState state, CancellationToken cancellationToken)
         at Microsoft.EntityFrameworkCore.Storage.ExecutionStrategy.ExecuteImplementationAsync[TState,TResult](Func`4 operation, Func`4 verifySucceeded, TState state, CancellationToken cancellationToken)
         at Microsoft.EntityFrameworkCore.Storage.ExecutionStrategy.ExecuteAsync[TState,TResult](TState state, Func`4 operation, Func`4 verifySucceeded, CancellationToken cancellationToken)
         at Microsoft.EntityFrameworkCore.DbContext.SaveChangesAsync(Boolean acceptAllChangesOnSuccess, CancellationToken cancellationToken)
         at Microsoft.EntityFrameworkCore.DbContext.SaveChangesAsync(Boolean acceptAllChangesOnSuccess, CancellationToken cancellationToken)
         at WebApplication.BackgroundJobs.PendingOrderMonitorJob.RunCycleAsync(CancellationToken cancellationToken) in C:\Users\user\Documents\ANDREI_FILES\PDM_FILES\2ND YEAR\2ND SEM\OOP\OOP-TaurusBikeShop\WebApplication\BackgroundJobs\PendingOrderMonitorJob.cs:line 75
fail: WebApplication.BackgroundJobs.PendingOrderMonitorJob[0]
      PendingOrderMonitorJob cycle failed.
      System.ArgumentException: Keyword not supported: 'userid'.
         at Microsoft.Data.SqlClient.SqlConnectionStringBuilder.GetIndex(String keyword)
         at Microsoft.Data.SqlClient.SqlConnectionStringBuilder.set_Item(String keyword, Object value)
         at System.Data.Common.DbConnectionStringBuilder.set_ConnectionString(String value)
         at Microsoft.Data.SqlClient.SqlConnectionStringBuilder..ctor(String connectionString)
         at Microsoft.EntityFrameworkCore.SqlServer.Storage.Internal.SqlServerConnection.<>c.<get_IsMultipleActiveResultSetsEnabled>b__7_0(String cs)
         at System.Collections.Concurrent.ConcurrentDictionary`2.GetOrAdd(TKey key, Func`2 valueFactory)
         at Microsoft.EntityFrameworkCore.SqlServer.Storage.Internal.SqlServerConnection.get_IsMultipleActiveResultSetsEnabled()
         at Microsoft.EntityFrameworkCore.SqlServer.Query.Internal.SqlServerCompiledQueryCacheKeyGenerator.GenerateCacheKey(Expression query, Boolean async)
         at Microsoft.EntityFrameworkCore.Query.Internal.QueryCompiler.ExecuteAsync[TResult](Expression query, CancellationToken cancellationToken)
         at Microsoft.EntityFrameworkCore.Query.Internal.EntityQueryProvider.ExecuteAsync[TResult](Expression expression, CancellationToken cancellationToken)
         at Microsoft.EntityFrameworkCore.Query.Internal.EntityQueryable`1.GetAsyncEnumerator(CancellationToken cancellationToken)
         at System.Runtime.CompilerServices.ConfiguredCancelableAsyncEnumerable`1.GetAsyncEnumerator()
         at Microsoft.EntityFrameworkCore.EntityFrameworkQueryableExtensions.ToListAsync[TSource](IQueryable`1 source, CancellationToken cancellationToken)
         at WebApplication.BackgroundJobs.PendingOrderMonitorJob.RunCycleAsync(CancellationToken cancellationToken) in C:\Users\user\Documents\ANDREI_FILES\PDM_FILES\2ND YEAR\2ND SEM\OOP\OOP-TaurusBikeShop\WebApplication\BackgroundJobs\PendingOrderMonitorJob.cs:line 90
fail: Microsoft.EntityFrameworkCore.Update[10000]
      An exception occurred in the database while saving changes for context type 'WebApplication.DataAccess.Context.AppDbContext'.
      System.ArgumentException: Keyword not supported: 'userid'.
         at Microsoft.Data.Common.DbConnectionOptions.ParseInternal(Dictionary`2 parsetable, String connectionString, Boolean buildChain, Dictionary`2 synonyms, Boolean firstKey)
         at Microsoft.Data.Common.DbConnectionOptions..ctor(String connectionString, Dictionary`2 synonyms)
         at Microsoft.Data.SqlClient.SqlConnectionString..ctor(String connectionString)
         at Microsoft.Data.SqlClient.SqlConnectionFactory.CreateConnectionOptions(String connectionString, DbConnectionOptions previous)
         at Microsoft.Data.ProviderBase.DbConnectionFactory.GetConnectionPoolGroup(DbConnectionPoolKey key, DbConnectionPoolGroupOptions poolOptions, DbConnectionOptions& userConnectionOptions)
         at Microsoft.Data.SqlClient.SqlConnection.ConnectionString_Set(DbConnectionPoolKey key)
         at Microsoft.Data.SqlClient.SqlConnection.set_ConnectionString(String value)
         at Microsoft.Data.SqlClient.SqlConnection..ctor(String connectionString)
         at Microsoft.EntityFrameworkCore.SqlServer.Storage.Internal.SqlServerConnection.CreateDbConnection()
         at Microsoft.EntityFrameworkCore.Storage.RelationalConnection.get_DbConnection()
         at Microsoft.EntityFrameworkCore.Storage.RelationalConnection.OpenAsync(CancellationToken cancellationToken, Boolean errorsExpected)
         at Microsoft.EntityFrameworkCore.Update.Internal.BatchExecutor.ExecuteAsync(IEnumerable`1 commandBatches, IRelationalConnection connection, CancellationToken cancellationToken)
         at Microsoft.EntityFrameworkCore.Update.Internal.BatchExecutor.ExecuteAsync(IEnumerable`1 commandBatches, IRelationalConnection connection, CancellationToken cancellationToken)
         at Microsoft.EntityFrameworkCore.Update.Internal.BatchExecutor.ExecuteAsync(IEnumerable`1 commandBatches, IRelationalConnection connection, CancellationToken cancellationToken)
         at Microsoft.EntityFrameworkCore.ChangeTracking.Internal.StateManager.SaveChangesAsync(IList`1 entriesToSave, CancellationToken cancellationToken)
         at Microsoft.EntityFrameworkCore.ChangeTracking.Internal.StateManager.SaveChangesAsync(StateManager stateManager, Boolean acceptAllChangesOnSuccess, CancellationToken cancellationToken)
         at Microsoft.EntityFrameworkCore.Storage.ExecutionStrategy.<>c__DisplayClass30_0`2.<<ExecuteAsync>b__0>d.MoveNext()
      --- End of stack trace from previous location ---
         at Microsoft.EntityFrameworkCore.Storage.ExecutionStrategy.ExecuteImplementationAsync[TState,TResult](Func`4 operation, Func`4 verifySucceeded, TState state, CancellationToken cancellationToken)
         at Microsoft.EntityFrameworkCore.Storage.ExecutionStrategy.ExecuteImplementationAsync[TState,TResult](Func`4 operation, Func`4 verifySucceeded, TState state, CancellationToken cancellationToken)
         at Microsoft.EntityFrameworkCore.Storage.ExecutionStrategy.ExecuteAsync[TState,TResult](TState state, Func`4 operation, Func`4 verifySucceeded, CancellationToken cancellationToken)
         at Microsoft.EntityFrameworkCore.DbContext.SaveChangesAsync(Boolean acceptAllChangesOnSuccess, CancellationToken cancellationToken)
      System.ArgumentException: Keyword not supported: 'userid'.
         at Microsoft.Data.Common.DbConnectionOptions.ParseInternal(Dictionary`2 parsetable, String connectionString, Boolean buildChain, Dictionary`2 synonyms, Boolean firstKey)
         at Microsoft.Data.Common.DbConnectionOptions..ctor(String connectionString, Dictionary`2 synonyms)
         at Microsoft.Data.SqlClient.SqlConnectionString..ctor(String connectionString)
         at Microsoft.Data.SqlClient.SqlConnectionFactory.CreateConnectionOptions(String connectionString, DbConnectionOptions previous)
         at Microsoft.Data.ProviderBase.DbConnectionFactory.GetConnectionPoolGroup(DbConnectionPoolKey key, DbConnectionPoolGroupOptions poolOptions, DbConnectionOptions& userConnectionOptions)
         at Microsoft.Data.SqlClient.SqlConnection.ConnectionString_Set(DbConnectionPoolKey key)
         at Microsoft.Data.SqlClient.SqlConnection.set_ConnectionString(String value)
         at Microsoft.Data.SqlClient.SqlConnection..ctor(String connectionString)
         at Microsoft.EntityFrameworkCore.SqlServer.Storage.Internal.SqlServerConnection.CreateDbConnection()
         at Microsoft.EntityFrameworkCore.Storage.RelationalConnection.get_DbConnection()
         at Microsoft.EntityFrameworkCore.Storage.RelationalConnection.OpenAsync(CancellationToken cancellationToken, Boolean errorsExpected)
         at Microsoft.EntityFrameworkCore.Update.Internal.BatchExecutor.ExecuteAsync(IEnumerable`1 commandBatches, IRelationalConnection connection, CancellationToken cancellationToken)
         at Microsoft.EntityFrameworkCore.Update.Internal.BatchExecutor.ExecuteAsync(IEnumerable`1 commandBatches, IRelationalConnection connection, CancellationToken cancellationToken)
         at Microsoft.EntityFrameworkCore.Update.Internal.BatchExecutor.ExecuteAsync(IEnumerable`1 commandBatches, IRelationalConnection connection, CancellationToken cancellationToken)
         at Microsoft.EntityFrameworkCore.ChangeTracking.Internal.StateManager.SaveChangesAsync(IList`1 entriesToSave, CancellationToken cancellationToken)
         at Microsoft.EntityFrameworkCore.ChangeTracking.Internal.StateManager.SaveChangesAsync(StateManager stateManager, Boolean acceptAllChangesOnSuccess, CancellationToken cancellationToken)
         at Microsoft.EntityFrameworkCore.Storage.ExecutionStrategy.<>c__DisplayClass30_0`2.<<ExecuteAsync>b__0>d.MoveNext()
      --- End of stack trace from previous location ---
         at Microsoft.EntityFrameworkCore.Storage.ExecutionStrategy.ExecuteImplementationAsync[TState,TResult](Func`4 operation, Func`4 verifySucceeded, TState state, CancellationToken cancellationToken)
         at Microsoft.EntityFrameworkCore.Storage.ExecutionStrategy.ExecuteImplementationAsync[TState,TResult](Func`4 operation, Func`4 verifySucceeded, TState state, CancellationToken cancellationToken)
         at Microsoft.EntityFrameworkCore.Storage.ExecutionStrategy.ExecuteAsync[TState,TResult](TState state, Func`4 operation, Func`4 verifySucceeded, CancellationToken cancellationToken)
         at Microsoft.EntityFrameworkCore.DbContext.SaveChangesAsync(Boolean acceptAllChangesOnSuccess, CancellationToken cancellationToken)
fail: WebApplication.BackgroundJobs.PendingOrderMonitorJob[0]
      Failed to write error log for PendingOrderMonitorJob.
      System.ArgumentException: Keyword not supported: 'userid'.
         at Microsoft.Data.Common.DbConnectionOptions.ParseInternal(Dictionary`2 parsetable, String connectionString, Boolean buildChain, Dictionary`2 synonyms, Boolean firstKey)
         at Microsoft.Data.Common.DbConnectionOptions..ctor(String connectionString, Dictionary`2 synonyms)
         at Microsoft.Data.SqlClient.SqlConnectionString..ctor(String connectionString)
         at Microsoft.Data.SqlClient.SqlConnectionFactory.CreateConnectionOptions(String connectionString, DbConnectionOptions previous)
         at Microsoft.Data.ProviderBase.DbConnectionFactory.GetConnectionPoolGroup(DbConnectionPoolKey key, DbConnectionPoolGroupOptions poolOptions, DbConnectionOptions& userConnectionOptions)
         at Microsoft.Data.SqlClient.SqlConnection.ConnectionString_Set(DbConnectionPoolKey key)
         at Microsoft.Data.SqlClient.SqlConnection.set_ConnectionString(String value)
         at Microsoft.Data.SqlClient.SqlConnection..ctor(String connectionString)
         at Microsoft.EntityFrameworkCore.SqlServer.Storage.Internal.SqlServerConnection.CreateDbConnection()
         at Microsoft.EntityFrameworkCore.Storage.RelationalConnection.get_DbConnection()
         at Microsoft.EntityFrameworkCore.Storage.RelationalConnection.OpenAsync(CancellationToken cancellationToken, Boolean errorsExpected)
         at Microsoft.EntityFrameworkCore.Update.Internal.BatchExecutor.ExecuteAsync(IEnumerable`1 commandBatches, IRelationalConnection connection, CancellationToken cancellationToken)
         at Microsoft.EntityFrameworkCore.Update.Internal.BatchExecutor.ExecuteAsync(IEnumerable`1 commandBatches, IRelationalConnection connection, CancellationToken cancellationToken)
         at Microsoft.EntityFrameworkCore.Update.Internal.BatchExecutor.ExecuteAsync(IEnumerable`1 commandBatches, IRelationalConnection connection, CancellationToken cancellationToken)
         at Microsoft.EntityFrameworkCore.ChangeTracking.Internal.StateManager.SaveChangesAsync(IList`1 entriesToSave, CancellationToken cancellationToken)
         at Microsoft.EntityFrameworkCore.ChangeTracking.Internal.StateManager.SaveChangesAsync(StateManager stateManager, Boolean acceptAllChangesOnSuccess, CancellationToken cancellationToken)
         at Microsoft.EntityFrameworkCore.Storage.ExecutionStrategy.<>c__DisplayClass30_0`2.<<ExecuteAsync>b__0>d.MoveNext()
      --- End of stack trace from previous location ---
         at Microsoft.EntityFrameworkCore.Storage.ExecutionStrategy.ExecuteImplementationAsync[TState,TResult](Func`4 operation, Func`4 verifySucceeded, TState state, CancellationToken cancellationToken)
         at Microsoft.EntityFrameworkCore.Storage.ExecutionStrategy.ExecuteImplementationAsync[TState,TResult](Func`4 operation, Func`4 verifySucceeded, TState state, CancellationToken cancellationToken)
         at Microsoft.EntityFrameworkCore.Storage.ExecutionStrategy.ExecuteAsync[TState,TResult](TState state, Func`4 operation, Func`4 verifySucceeded, CancellationToken cancellationToken)
         at Microsoft.EntityFrameworkCore.DbContext.SaveChangesAsync(Boolean acceptAllChangesOnSuccess, CancellationToken cancellationToken)
         at Microsoft.EntityFrameworkCore.DbContext.SaveChangesAsync(Boolean acceptAllChangesOnSuccess, CancellationToken cancellationToken)
         at WebApplication.BackgroundJobs.PendingOrderMonitorJob.RunCycleAsync(CancellationToken cancellationToken) in C:\Users\user\Documents\ANDREI_FILES\PDM_FILES\2ND YEAR\2ND SEM\OOP\OOP-TaurusBikeShop\WebApplication\BackgroundJobs\PendingOrderMonitorJob.cs:line 200
         at WebApplication.BackgroundJobs.PendingOrderMonitorJob.RunCycleAsync(CancellationToken cancellationToken) in C:\Users\user\Documents\ANDREI_FILES\PDM_FILES\2ND YEAR\2ND SEM\OOP\OOP-TaurusBikeShop\WebApplication\BackgroundJobs\PendingOrderMonitorJob.cs:line 200
info: WebApplication.BackgroundJobs.PaymentTimeoutJob[0]
      PaymentTimeoutJob started.
fail: Microsoft.EntityFrameworkCore.Update[10000]
      An exception occurred in the database while saving changes for context type 'WebApplication.DataAccess.Context.AppDbContext'.
      System.ArgumentException: Keyword not supported: 'userid'.
         at Microsoft.Data.Common.DbConnectionOptions.ParseInternal(Dictionary`2 parsetable, String connectionString, Boolean buildChain, Dictionary`2 synonyms, Boolean firstKey)
         at Microsoft.Data.Common.DbConnectionOptions..ctor(String connectionString, Dictionary`2 synonyms)
         at Microsoft.Data.SqlClient.SqlConnectionString..ctor(String connectionString)
         at Microsoft.Data.SqlClient.SqlConnectionFactory.CreateConnectionOptions(String connectionString, DbConnectionOptions previous)
         at Microsoft.Data.ProviderBase.DbConnectionFactory.GetConnectionPoolGroup(DbConnectionPoolKey key, DbConnectionPoolGroupOptions poolOptions, DbConnectionOptions& userConnectionOptions)
         at Microsoft.Data.SqlClient.SqlConnection.ConnectionString_Set(DbConnectionPoolKey key)
         at Microsoft.Data.SqlClient.SqlConnection.set_ConnectionString(String value)
         at Microsoft.Data.SqlClient.SqlConnection..ctor(String connectionString)
         at Microsoft.EntityFrameworkCore.SqlServer.Storage.Internal.SqlServerConnection.CreateDbConnection()
         at Microsoft.EntityFrameworkCore.Storage.RelationalConnection.get_DbConnection()
         at Microsoft.EntityFrameworkCore.Storage.RelationalConnection.OpenAsync(CancellationToken cancellationToken, Boolean errorsExpected)
         at Microsoft.EntityFrameworkCore.Update.Internal.BatchExecutor.ExecuteAsync(IEnumerable`1 commandBatches, IRelationalConnection connection, CancellationToken cancellationToken)
         at Microsoft.EntityFrameworkCore.Update.Internal.BatchExecutor.ExecuteAsync(IEnumerable`1 commandBatches, IRelationalConnection connection, CancellationToken cancellationToken)
         at Microsoft.EntityFrameworkCore.Update.Internal.BatchExecutor.ExecuteAsync(IEnumerable`1 commandBatches, IRelationalConnection connection, CancellationToken cancellationToken)
         at Microsoft.EntityFrameworkCore.ChangeTracking.Internal.StateManager.SaveChangesAsync(IList`1 entriesToSave, CancellationToken cancellationToken)
         at Microsoft.EntityFrameworkCore.ChangeTracking.Internal.StateManager.SaveChangesAsync(StateManager stateManager, Boolean acceptAllChangesOnSuccess, CancellationToken cancellationToken)
         at Microsoft.EntityFrameworkCore.Storage.ExecutionStrategy.<>c__DisplayClass30_0`2.<<ExecuteAsync>b__0>d.MoveNext()
      --- End of stack trace from previous location ---
         at Microsoft.EntityFrameworkCore.Storage.ExecutionStrategy.ExecuteImplementationAsync[TState,TResult](Func`4 operation, Func`4 verifySucceeded, TState state, CancellationToken cancellationToken)
         at Microsoft.EntityFrameworkCore.Storage.ExecutionStrategy.ExecuteImplementationAsync[TState,TResult](Func`4 operation, Func`4 verifySucceeded, TState state, CancellationToken cancellationToken)
         at Microsoft.EntityFrameworkCore.Storage.ExecutionStrategy.ExecuteAsync[TState,TResult](TState state, Func`4 operation, Func`4 verifySucceeded, CancellationToken cancellationToken)
         at Microsoft.EntityFrameworkCore.DbContext.SaveChangesAsync(Boolean acceptAllChangesOnSuccess, CancellationToken cancellationToken)
      System.ArgumentException: Keyword not supported: 'userid'.
         at Microsoft.Data.Common.DbConnectionOptions.ParseInternal(Dictionary`2 parsetable, String connectionString, Boolean buildChain, Dictionary`2 synonyms, Boolean firstKey)
         at Microsoft.Data.Common.DbConnectionOptions..ctor(String connectionString, Dictionary`2 synonyms)
         at Microsoft.Data.SqlClient.SqlConnectionString..ctor(String connectionString)
         at Microsoft.Data.SqlClient.SqlConnectionFactory.CreateConnectionOptions(String connectionString, DbConnectionOptions previous)
         at Microsoft.Data.ProviderBase.DbConnectionFactory.GetConnectionPoolGroup(DbConnectionPoolKey key, DbConnectionPoolGroupOptions poolOptions, DbConnectionOptions& userConnectionOptions)
         at Microsoft.Data.SqlClient.SqlConnection.ConnectionString_Set(DbConnectionPoolKey key)
         at Microsoft.Data.SqlClient.SqlConnection.set_ConnectionString(String value)
         at Microsoft.Data.SqlClient.SqlConnection..ctor(String connectionString)
         at Microsoft.EntityFrameworkCore.SqlServer.Storage.Internal.SqlServerConnection.CreateDbConnection()
         at Microsoft.EntityFrameworkCore.Storage.RelationalConnection.get_DbConnection()
         at Microsoft.EntityFrameworkCore.Storage.RelationalConnection.OpenAsync(CancellationToken cancellationToken, Boolean errorsExpected)
         at Microsoft.EntityFrameworkCore.Update.Internal.BatchExecutor.ExecuteAsync(IEnumerable`1 commandBatches, IRelationalConnection connection, CancellationToken cancellationToken)
         at Microsoft.EntityFrameworkCore.Update.Internal.BatchExecutor.ExecuteAsync(IEnumerable`1 commandBatches, IRelationalConnection connection, CancellationToken cancellationToken)
         at Microsoft.EntityFrameworkCore.Update.Internal.BatchExecutor.ExecuteAsync(IEnumerable`1 commandBatches, IRelationalConnection connection, CancellationToken cancellationToken)
         at Microsoft.EntityFrameworkCore.ChangeTracking.Internal.StateManager.SaveChangesAsync(IList`1 entriesToSave, CancellationToken cancellationToken)
         at Microsoft.EntityFrameworkCore.ChangeTracking.Internal.StateManager.SaveChangesAsync(StateManager stateManager, Boolean acceptAllChangesOnSuccess, CancellationToken cancellationToken)
         at Microsoft.EntityFrameworkCore.Storage.ExecutionStrategy.<>c__DisplayClass30_0`2.<<ExecuteAsync>b__0>d.MoveNext()
      --- End of stack trace from previous location ---
         at Microsoft.EntityFrameworkCore.Storage.ExecutionStrategy.ExecuteImplementationAsync[TState,TResult](Func`4 operation, Func`4 verifySucceeded, TState state, CancellationToken cancellationToken)
         at Microsoft.EntityFrameworkCore.Storage.ExecutionStrategy.ExecuteImplementationAsync[TState,TResult](Func`4 operation, Func`4 verifySucceeded, TState state, CancellationToken cancellationToken)
         at Microsoft.EntityFrameworkCore.Storage.ExecutionStrategy.ExecuteAsync[TState,TResult](TState state, Func`4 operation, Func`4 verifySucceeded, CancellationToken cancellationToken)
         at Microsoft.EntityFrameworkCore.DbContext.SaveChangesAsync(Boolean acceptAllChangesOnSuccess, CancellationToken cancellationToken)
warn: WebApplication.BackgroundJobs.PaymentTimeoutJob[0]
      PaymentTimeoutJob could not write start log - DB may be unavailable.
      System.ArgumentException: Keyword not supported: 'userid'.
         at Microsoft.Data.Common.DbConnectionOptions.ParseInternal(Dictionary`2 parsetable, String connectionString, Boolean buildChain, Dictionary`2 synonyms, Boolean firstKey)
         at Microsoft.Data.Common.DbConnectionOptions..ctor(String connectionString, Dictionary`2 synonyms)
         at Microsoft.Data.SqlClient.SqlConnectionString..ctor(String connectionString)
         at Microsoft.Data.SqlClient.SqlConnectionFactory.CreateConnectionOptions(String connectionString, DbConnectionOptions previous)
         at Microsoft.Data.ProviderBase.DbConnectionFactory.GetConnectionPoolGroup(DbConnectionPoolKey key, DbConnectionPoolGroupOptions poolOptions, DbConnectionOptions& userConnectionOptions)
         at Microsoft.Data.SqlClient.SqlConnection.ConnectionString_Set(DbConnectionPoolKey key)
         at Microsoft.Data.SqlClient.SqlConnection.set_ConnectionString(String value)
         at Microsoft.Data.SqlClient.SqlConnection..ctor(String connectionString)
         at Microsoft.EntityFrameworkCore.SqlServer.Storage.Internal.SqlServerConnection.CreateDbConnection()
         at Microsoft.EntityFrameworkCore.Storage.RelationalConnection.get_DbConnection()
         at Microsoft.EntityFrameworkCore.Storage.RelationalConnection.OpenAsync(CancellationToken cancellationToken, Boolean errorsExpected)
         at Microsoft.EntityFrameworkCore.Update.Internal.BatchExecutor.ExecuteAsync(IEnumerable`1 commandBatches, IRelationalConnection connection, CancellationToken cancellationToken)
         at Microsoft.EntityFrameworkCore.Update.Internal.BatchExecutor.ExecuteAsync(IEnumerable`1 commandBatches, IRelationalConnection connection, CancellationToken cancellationToken)
         at Microsoft.EntityFrameworkCore.Update.Internal.BatchExecutor.ExecuteAsync(IEnumerable`1 commandBatches, IRelationalConnection connection, CancellationToken cancellationToken)
         at Microsoft.EntityFrameworkCore.ChangeTracking.Internal.StateManager.SaveChangesAsync(IList`1 entriesToSave, CancellationToken cancellationToken)
         at Microsoft.EntityFrameworkCore.ChangeTracking.Internal.StateManager.SaveChangesAsync(StateManager stateManager, Boolean acceptAllChangesOnSuccess, CancellationToken cancellationToken)
         at Microsoft.EntityFrameworkCore.Storage.ExecutionStrategy.<>c__DisplayClass30_0`2.<<ExecuteAsync>b__0>d.MoveNext()
      --- End of stack trace from previous location ---
         at Microsoft.EntityFrameworkCore.Storage.ExecutionStrategy.ExecuteImplementationAsync[TState,TResult](Func`4 operation, Func`4 verifySucceeded, TState state, CancellationToken cancellationToken)
         at Microsoft.EntityFrameworkCore.Storage.ExecutionStrategy.ExecuteImplementationAsync[TState,TResult](Func`4 operation, Func`4 verifySucceeded, TState state, CancellationToken cancellationToken)
         at Microsoft.EntityFrameworkCore.Storage.ExecutionStrategy.ExecuteAsync[TState,TResult](TState state, Func`4 operation, Func`4 verifySucceeded, CancellationToken cancellationToken)
         at Microsoft.EntityFrameworkCore.DbContext.SaveChangesAsync(Boolean acceptAllChangesOnSuccess, CancellationToken cancellationToken)
         at Microsoft.EntityFrameworkCore.DbContext.SaveChangesAsync(Boolean acceptAllChangesOnSuccess, CancellationToken cancellationToken)
         at WebApplication.BackgroundJobs.PaymentTimeoutJob.RunCycleAsync(CancellationToken cancellationToken) in C:\Users\user\Documents\ANDREI_FILES\PDM_FILES\2ND YEAR\2ND SEM\OOP\OOP-TaurusBikeShop\WebApplication\BackgroundJobs\PaymentTimeoutJob.cs:line 86
fail: WebApplication.BackgroundJobs.PaymentTimeoutJob[0]
      PaymentTimeoutJob cycle failed.
      System.ArgumentException: Keyword not supported: 'userid'.
         at Microsoft.Data.SqlClient.SqlConnectionStringBuilder.GetIndex(String keyword)
         at Microsoft.Data.SqlClient.SqlConnectionStringBuilder.set_Item(String keyword, Object value)
         at System.Data.Common.DbConnectionStringBuilder.set_ConnectionString(String value)
         at Microsoft.Data.SqlClient.SqlConnectionStringBuilder..ctor(String connectionString)
         at Microsoft.EntityFrameworkCore.SqlServer.Storage.Internal.SqlServerConnection.<>c.<get_IsMultipleActiveResultSetsEnabled>b__7_0(String cs)
         at System.Collections.Concurrent.ConcurrentDictionary`2.GetOrAdd(TKey key, Func`2 valueFactory)
         at Microsoft.EntityFrameworkCore.SqlServer.Storage.Internal.SqlServerConnection.get_IsMultipleActiveResultSetsEnabled()
         at Microsoft.EntityFrameworkCore.SqlServer.Query.Internal.SqlServerCompiledQueryCacheKeyGenerator.GenerateCacheKey(Expression query, Boolean async)
         at Microsoft.EntityFrameworkCore.Query.Internal.QueryCompiler.ExecuteAsync[TResult](Expression query, CancellationToken cancellationToken)
         at Microsoft.EntityFrameworkCore.Query.Internal.EntityQueryProvider.ExecuteAsync[TResult](Expression expression, CancellationToken cancellationToken)
         at Microsoft.EntityFrameworkCore.Query.Internal.EntityQueryable`1.GetAsyncEnumerator(CancellationToken cancellationToken)
         at System.Runtime.CompilerServices.ConfiguredCancelableAsyncEnumerable`1.GetAsyncEnumerator()
         at Microsoft.EntityFrameworkCore.EntityFrameworkQueryableExtensions.ToListAsync[TSource](IQueryable`1 source, CancellationToken cancellationToken)
         at WebApplication.BackgroundJobs.PaymentTimeoutJob.RunCycleAsync(CancellationToken cancellationToken) in C:\Users\user\Documents\ANDREI_FILES\PDM_FILES\2ND YEAR\2ND SEM\OOP\OOP-TaurusBikeShop\WebApplication\BackgroundJobs\PaymentTimeoutJob.cs:line 105
fail: Microsoft.EntityFrameworkCore.Update[10000]
      An exception occurred in the database while saving changes for context type 'WebApplication.DataAccess.Context.AppDbContext'.
      System.ArgumentException: Keyword not supported: 'userid'.
         at Microsoft.Data.Common.DbConnectionOptions.ParseInternal(Dictionary`2 parsetable, String connectionString, Boolean buildChain, Dictionary`2 synonyms, Boolean firstKey)
         at Microsoft.Data.Common.DbConnectionOptions..ctor(String connectionString, Dictionary`2 synonyms)
         at Microsoft.Data.SqlClient.SqlConnectionString..ctor(String connectionString)
         at Microsoft.Data.SqlClient.SqlConnectionFactory.CreateConnectionOptions(String connectionString, DbConnectionOptions previous)
         at Microsoft.Data.ProviderBase.DbConnectionFactory.GetConnectionPoolGroup(DbConnectionPoolKey key, DbConnectionPoolGroupOptions poolOptions, DbConnectionOptions& userConnectionOptions)
         at Microsoft.Data.SqlClient.SqlConnection.ConnectionString_Set(DbConnectionPoolKey key)
         at Microsoft.Data.SqlClient.SqlConnection.set_ConnectionString(String value)
         at Microsoft.Data.SqlClient.SqlConnection..ctor(String connectionString)
         at Microsoft.EntityFrameworkCore.SqlServer.Storage.Internal.SqlServerConnection.CreateDbConnection()
         at Microsoft.EntityFrameworkCore.Storage.RelationalConnection.get_DbConnection()
         at Microsoft.EntityFrameworkCore.Storage.RelationalConnection.OpenAsync(CancellationToken cancellationToken, Boolean errorsExpected)
         at Microsoft.EntityFrameworkCore.Update.Internal.BatchExecutor.ExecuteAsync(IEnumerable`1 commandBatches, IRelationalConnection connection, CancellationToken cancellationToken)
         at Microsoft.EntityFrameworkCore.Update.Internal.BatchExecutor.ExecuteAsync(IEnumerable`1 commandBatches, IRelationalConnection connection, CancellationToken cancellationToken)
         at Microsoft.EntityFrameworkCore.Update.Internal.BatchExecutor.ExecuteAsync(IEnumerable`1 commandBatches, IRelationalConnection connection, CancellationToken cancellationToken)
         at Microsoft.EntityFrameworkCore.ChangeTracking.Internal.StateManager.SaveChangesAsync(IList`1 entriesToSave, CancellationToken cancellationToken)
         at Microsoft.EntityFrameworkCore.ChangeTracking.Internal.StateManager.SaveChangesAsync(StateManager stateManager, Boolean acceptAllChangesOnSuccess, CancellationToken cancellationToken)
         at Microsoft.EntityFrameworkCore.Storage.ExecutionStrategy.<>c__DisplayClass30_0`2.<<ExecuteAsync>b__0>d.MoveNext()
      --- End of stack trace from previous location ---
         at Microsoft.EntityFrameworkCore.Storage.ExecutionStrategy.ExecuteImplementationAsync[TState,TResult](Func`4 operation, Func`4 verifySucceeded, TState state, CancellationToken cancellationToken)
         at Microsoft.EntityFrameworkCore.Storage.ExecutionStrategy.ExecuteImplementationAsync[TState,TResult](Func`4 operation, Func`4 verifySucceeded, TState state, CancellationToken cancellationToken)
         at Microsoft.EntityFrameworkCore.Storage.ExecutionStrategy.ExecuteAsync[TState,TResult](TState state, Func`4 operation, Func`4 verifySucceeded, CancellationToken cancellationToken)
         at Microsoft.EntityFrameworkCore.DbContext.SaveChangesAsync(Boolean acceptAllChangesOnSuccess, CancellationToken cancellationToken)
      System.ArgumentException: Keyword not supported: 'userid'.
         at Microsoft.Data.Common.DbConnectionOptions.ParseInternal(Dictionary`2 parsetable, String connectionString, Boolean buildChain, Dictionary`2 synonyms, Boolean firstKey)
         at Microsoft.Data.Common.DbConnectionOptions..ctor(String connectionString, Dictionary`2 synonyms)
         at Microsoft.Data.SqlClient.SqlConnectionString..ctor(String connectionString)
         at Microsoft.Data.SqlClient.SqlConnectionFactory.CreateConnectionOptions(String connectionString, DbConnectionOptions previous)
         at Microsoft.Data.ProviderBase.DbConnectionFactory.GetConnectionPoolGroup(DbConnectionPoolKey key, DbConnectionPoolGroupOptions poolOptions, DbConnectionOptions& userConnectionOptions)
         at Microsoft.Data.SqlClient.SqlConnection.ConnectionString_Set(DbConnectionPoolKey key)
         at Microsoft.Data.SqlClient.SqlConnection.set_ConnectionString(String value)
         at Microsoft.Data.SqlClient.SqlConnection..ctor(String connectionString)
         at Microsoft.EntityFrameworkCore.SqlServer.Storage.Internal.SqlServerConnection.CreateDbConnection()
         at Microsoft.EntityFrameworkCore.Storage.RelationalConnection.get_DbConnection()
         at Microsoft.EntityFrameworkCore.Storage.RelationalConnection.OpenAsync(CancellationToken cancellationToken, Boolean errorsExpected)
         at Microsoft.EntityFrameworkCore.Update.Internal.BatchExecutor.ExecuteAsync(IEnumerable`1 commandBatches, IRelationalConnection connection, CancellationToken cancellationToken)
         at Microsoft.EntityFrameworkCore.Update.Internal.BatchExecutor.ExecuteAsync(IEnumerable`1 commandBatches, IRelationalConnection connection, CancellationToken cancellationToken)
         at Microsoft.EntityFrameworkCore.Update.Internal.BatchExecutor.ExecuteAsync(IEnumerable`1 commandBatches, IRelationalConnection connection, CancellationToken cancellationToken)
         at Microsoft.EntityFrameworkCore.ChangeTracking.Internal.StateManager.SaveChangesAsync(IList`1 entriesToSave, CancellationToken cancellationToken)
         at Microsoft.EntityFrameworkCore.ChangeTracking.Internal.StateManager.SaveChangesAsync(StateManager stateManager, Boolean acceptAllChangesOnSuccess, CancellationToken cancellationToken)
         at Microsoft.EntityFrameworkCore.Storage.ExecutionStrategy.<>c__DisplayClass30_0`2.<<ExecuteAsync>b__0>d.MoveNext()
      --- End of stack trace from previous location ---
         at Microsoft.EntityFrameworkCore.Storage.ExecutionStrategy.ExecuteImplementationAsync[TState,TResult](Func`4 operation, Func`4 verifySucceeded, TState state, CancellationToken cancellationToken)
         at Microsoft.EntityFrameworkCore.Storage.ExecutionStrategy.ExecuteImplementationAsync[TState,TResult](Func`4 operation, Func`4 verifySucceeded, TState state, CancellationToken cancellationToken)
         at Microsoft.EntityFrameworkCore.Storage.ExecutionStrategy.ExecuteAsync[TState,TResult](TState state, Func`4 operation, Func`4 verifySucceeded, CancellationToken cancellationToken)
         at Microsoft.EntityFrameworkCore.DbContext.SaveChangesAsync(Boolean acceptAllChangesOnSuccess, CancellationToken cancellationToken)
fail: WebApplication.BackgroundJobs.PaymentTimeoutJob[0]
      Failed to write error log for PaymentTimeoutJob.
      System.ArgumentException: Keyword not supported: 'userid'.
         at Microsoft.Data.Common.DbConnectionOptions.ParseInternal(Dictionary`2 parsetable, String connectionString, Boolean buildChain, Dictionary`2 synonyms, Boolean firstKey)
         at Microsoft.Data.Common.DbConnectionOptions..ctor(String connectionString, Dictionary`2 synonyms)
         at Microsoft.Data.SqlClient.SqlConnectionString..ctor(String connectionString)
         at Microsoft.Data.SqlClient.SqlConnectionFactory.CreateConnectionOptions(String connectionString, DbConnectionOptions previous)
         at Microsoft.Data.ProviderBase.DbConnectionFactory.GetConnectionPoolGroup(DbConnectionPoolKey key, DbConnectionPoolGroupOptions poolOptions, DbConnectionOptions& userConnectionOptions)
         at Microsoft.Data.SqlClient.SqlConnection.ConnectionString_Set(DbConnectionPoolKey key)
         at Microsoft.Data.SqlClient.SqlConnection.set_ConnectionString(String value)
         at Microsoft.Data.SqlClient.SqlConnection..ctor(String connectionString)
         at Microsoft.EntityFrameworkCore.SqlServer.Storage.Internal.SqlServerConnection.CreateDbConnection()
         at Microsoft.EntityFrameworkCore.Storage.RelationalConnection.get_DbConnection()
         at Microsoft.EntityFrameworkCore.Storage.RelationalConnection.OpenAsync(CancellationToken cancellationToken, Boolean errorsExpected)
         at Microsoft.EntityFrameworkCore.Update.Internal.BatchExecutor.ExecuteAsync(IEnumerable`1 commandBatches, IRelationalConnection connection, CancellationToken cancellationToken)
         at Microsoft.EntityFrameworkCore.Update.Internal.BatchExecutor.ExecuteAsync(IEnumerable`1 commandBatches, IRelationalConnection connection, CancellationToken cancellationToken)
         at Microsoft.EntityFrameworkCore.Update.Internal.BatchExecutor.ExecuteAsync(IEnumerable`1 commandBatches, IRelationalConnection connection, CancellationToken cancellationToken)
         at Microsoft.EntityFrameworkCore.ChangeTracking.Internal.StateManager.SaveChangesAsync(IList`1 entriesToSave, CancellationToken cancellationToken)
         at Microsoft.EntityFrameworkCore.ChangeTracking.Internal.StateManager.SaveChangesAsync(StateManager stateManager, Boolean acceptAllChangesOnSuccess, CancellationToken cancellationToken)
         at Microsoft.EntityFrameworkCore.Storage.ExecutionStrategy.<>c__DisplayClass30_0`2.<<ExecuteAsync>b__0>d.MoveNext()
      --- End of stack trace from previous location ---
         at Microsoft.EntityFrameworkCore.Storage.ExecutionStrategy.ExecuteImplementationAsync[TState,TResult](Func`4 operation, Func`4 verifySucceeded, TState state, CancellationToken cancellationToken)
         at Microsoft.EntityFrameworkCore.Storage.ExecutionStrategy.ExecuteImplementationAsync[TState,TResult](Func`4 operation, Func`4 verifySucceeded, TState state, CancellationToken cancellationToken)
         at Microsoft.EntityFrameworkCore.Storage.ExecutionStrategy.ExecuteAsync[TState,TResult](TState state, Func`4 operation, Func`4 verifySucceeded, CancellationToken cancellationToken)
         at Microsoft.EntityFrameworkCore.DbContext.SaveChangesAsync(Boolean acceptAllChangesOnSuccess, CancellationToken cancellationToken)
         at Microsoft.EntityFrameworkCore.DbContext.SaveChangesAsync(Boolean acceptAllChangesOnSuccess, CancellationToken cancellationToken)
         at WebApplication.BackgroundJobs.PaymentTimeoutJob.RunCycleAsync(CancellationToken cancellationToken) in C:\Users\user\Documents\ANDREI_FILES\PDM_FILES\2ND YEAR\2ND SEM\OOP\OOP-TaurusBikeShop\WebApplication\BackgroundJobs\PaymentTimeoutJob.cs:line 195
         at WebApplication.BackgroundJobs.PaymentTimeoutJob.RunCycleAsync(CancellationToken cancellationToken) in C:\Users\user\Documents\ANDREI_FILES\PDM_FILES\2ND YEAR\2ND SEM\OOP\OOP-TaurusBikeShop\WebApplication\BackgroundJobs\PaymentTimeoutJob.cs:line 195
info: WebApplication.BackgroundJobs.StockMonitorJob[0]
      StockMonitorJob started.
fail: Microsoft.EntityFrameworkCore.Update[10000]
      An exception occurred in the database while saving changes for context type 'WebApplication.DataAccess.Context.AppDbContext'.
      System.ArgumentException: Keyword not supported: 'userid'.
         at Microsoft.Data.Common.DbConnectionOptions.ParseInternal(Dictionary`2 parsetable, String connectionString, Boolean buildChain, Dictionary`2 synonyms, Boolean firstKey)
         at Microsoft.Data.Common.DbConnectionOptions..ctor(String connectionString, Dictionary`2 synonyms)
         at Microsoft.Data.SqlClient.SqlConnectionString..ctor(String connectionString)
         at Microsoft.Data.SqlClient.SqlConnectionFactory.CreateConnectionOptions(String connectionString, DbConnectionOptions previous)
         at Microsoft.Data.ProviderBase.DbConnectionFactory.GetConnectionPoolGroup(DbConnectionPoolKey key, DbConnectionPoolGroupOptions poolOptions, DbConnectionOptions& userConnectionOptions)
         at Microsoft.Data.SqlClient.SqlConnection.ConnectionString_Set(DbConnectionPoolKey key)
         at Microsoft.Data.SqlClient.SqlConnection.set_ConnectionString(String value)
         at Microsoft.Data.SqlClient.SqlConnection..ctor(String connectionString)
         at Microsoft.EntityFrameworkCore.SqlServer.Storage.Internal.SqlServerConnection.CreateDbConnection()
         at Microsoft.EntityFrameworkCore.Storage.RelationalConnection.get_DbConnection()
         at Microsoft.EntityFrameworkCore.Storage.RelationalConnection.OpenAsync(CancellationToken cancellationToken, Boolean errorsExpected)
         at Microsoft.EntityFrameworkCore.Update.Internal.BatchExecutor.ExecuteAsync(IEnumerable`1 commandBatches, IRelationalConnection connection, CancellationToken cancellationToken)
         at Microsoft.EntityFrameworkCore.Update.Internal.BatchExecutor.ExecuteAsync(IEnumerable`1 commandBatches, IRelationalConnection connection, CancellationToken cancellationToken)
         at Microsoft.EntityFrameworkCore.Update.Internal.BatchExecutor.ExecuteAsync(IEnumerable`1 commandBatches, IRelationalConnection connection, CancellationToken cancellationToken)
         at Microsoft.EntityFrameworkCore.ChangeTracking.Internal.StateManager.SaveChangesAsync(IList`1 entriesToSave, CancellationToken cancellationToken)
         at Microsoft.EntityFrameworkCore.ChangeTracking.Internal.StateManager.SaveChangesAsync(StateManager stateManager, Boolean acceptAllChangesOnSuccess, CancellationToken cancellationToken)
         at Microsoft.EntityFrameworkCore.Storage.ExecutionStrategy.<>c__DisplayClass30_0`2.<<ExecuteAsync>b__0>d.MoveNext()
      --- End of stack trace from previous location ---
         at Microsoft.EntityFrameworkCore.Storage.ExecutionStrategy.ExecuteImplementationAsync[TState,TResult](Func`4 operation, Func`4 verifySucceeded, TState state, CancellationToken cancellationToken)
         at Microsoft.EntityFrameworkCore.Storage.ExecutionStrategy.ExecuteImplementationAsync[TState,TResult](Func`4 operation, Func`4 verifySucceeded, TState state, CancellationToken cancellationToken)
         at Microsoft.EntityFrameworkCore.Storage.ExecutionStrategy.ExecuteAsync[TState,TResult](TState state, Func`4 operation, Func`4 verifySucceeded, CancellationToken cancellationToken)
         at Microsoft.EntityFrameworkCore.DbContext.SaveChangesAsync(Boolean acceptAllChangesOnSuccess, CancellationToken cancellationToken)
      System.ArgumentException: Keyword not supported: 'userid'.
         at Microsoft.Data.Common.DbConnectionOptions.ParseInternal(Dictionary`2 parsetable, String connectionString, Boolean buildChain, Dictionary`2 synonyms, Boolean firstKey)
         at Microsoft.Data.Common.DbConnectionOptions..ctor(String connectionString, Dictionary`2 synonyms)
         at Microsoft.Data.SqlClient.SqlConnectionString..ctor(String connectionString)
         at Microsoft.Data.SqlClient.SqlConnectionFactory.CreateConnectionOptions(String connectionString, DbConnectionOptions previous)
         at Microsoft.Data.ProviderBase.DbConnectionFactory.GetConnectionPoolGroup(DbConnectionPoolKey key, DbConnectionPoolGroupOptions poolOptions, DbConnectionOptions& userConnectionOptions)
         at Microsoft.Data.SqlClient.SqlConnection.ConnectionString_Set(DbConnectionPoolKey key)
         at Microsoft.Data.SqlClient.SqlConnection.set_ConnectionString(String value)
         at Microsoft.Data.SqlClient.SqlConnection..ctor(String connectionString)
         at Microsoft.EntityFrameworkCore.SqlServer.Storage.Internal.SqlServerConnection.CreateDbConnection()
         at Microsoft.EntityFrameworkCore.Storage.RelationalConnection.get_DbConnection()
         at Microsoft.EntityFrameworkCore.Storage.RelationalConnection.OpenAsync(CancellationToken cancellationToken, Boolean errorsExpected)
         at Microsoft.EntityFrameworkCore.Update.Internal.BatchExecutor.ExecuteAsync(IEnumerable`1 commandBatches, IRelationalConnection connection, CancellationToken cancellationToken)
         at Microsoft.EntityFrameworkCore.Update.Internal.BatchExecutor.ExecuteAsync(IEnumerable`1 commandBatches, IRelationalConnection connection, CancellationToken cancellationToken)
         at Microsoft.EntityFrameworkCore.Update.Internal.BatchExecutor.ExecuteAsync(IEnumerable`1 commandBatches, IRelationalConnection connection, CancellationToken cancellationToken)
         at Microsoft.EntityFrameworkCore.ChangeTracking.Internal.StateManager.SaveChangesAsync(IList`1 entriesToSave, CancellationToken cancellationToken)
         at Microsoft.EntityFrameworkCore.ChangeTracking.Internal.StateManager.SaveChangesAsync(StateManager stateManager, Boolean acceptAllChangesOnSuccess, CancellationToken cancellationToken)
         at Microsoft.EntityFrameworkCore.Storage.ExecutionStrategy.<>c__DisplayClass30_0`2.<<ExecuteAsync>b__0>d.MoveNext()
      --- End of stack trace from previous location ---
         at Microsoft.EntityFrameworkCore.Storage.ExecutionStrategy.ExecuteImplementationAsync[TState,TResult](Func`4 operation, Func`4 verifySucceeded, TState state, CancellationToken cancellationToken)
         at Microsoft.EntityFrameworkCore.Storage.ExecutionStrategy.ExecuteImplementationAsync[TState,TResult](Func`4 operation, Func`4 verifySucceeded, TState state, CancellationToken cancellationToken)
         at Microsoft.EntityFrameworkCore.Storage.ExecutionStrategy.ExecuteAsync[TState,TResult](TState state, Func`4 operation, Func`4 verifySucceeded, CancellationToken cancellationToken)
         at Microsoft.EntityFrameworkCore.DbContext.SaveChangesAsync(Boolean acceptAllChangesOnSuccess, CancellationToken cancellationToken)
warn: WebApplication.BackgroundJobs.StockMonitorJob[0]
      StockMonitorJob could not write start log - DB may be unavailable.
      System.ArgumentException: Keyword not supported: 'userid'.
         at Microsoft.Data.Common.DbConnectionOptions.ParseInternal(Dictionary`2 parsetable, String connectionString, Boolean buildChain, Dictionary`2 synonyms, Boolean firstKey)
         at Microsoft.Data.Common.DbConnectionOptions..ctor(String connectionString, Dictionary`2 synonyms)
         at Microsoft.Data.SqlClient.SqlConnectionString..ctor(String connectionString)
         at Microsoft.Data.SqlClient.SqlConnectionFactory.CreateConnectionOptions(String connectionString, DbConnectionOptions previous)
         at Microsoft.Data.ProviderBase.DbConnectionFactory.GetConnectionPoolGroup(DbConnectionPoolKey key, DbConnectionPoolGroupOptions poolOptions, DbConnectionOptions& userConnectionOptions)
         at Microsoft.Data.SqlClient.SqlConnection.ConnectionString_Set(DbConnectionPoolKey key)
         at Microsoft.Data.SqlClient.SqlConnection.set_ConnectionString(String value)
         at Microsoft.Data.SqlClient.SqlConnection..ctor(String connectionString)
         at Microsoft.EntityFrameworkCore.SqlServer.Storage.Internal.SqlServerConnection.CreateDbConnection()
         at Microsoft.EntityFrameworkCore.Storage.RelationalConnection.get_DbConnection()
         at Microsoft.EntityFrameworkCore.Storage.RelationalConnection.OpenAsync(CancellationToken cancellationToken, Boolean errorsExpected)
         at Microsoft.EntityFrameworkCore.Update.Internal.BatchExecutor.ExecuteAsync(IEnumerable`1 commandBatches, IRelationalConnection connection, CancellationToken cancellationToken)
         at Microsoft.EntityFrameworkCore.Update.Internal.BatchExecutor.ExecuteAsync(IEnumerable`1 commandBatches, IRelationalConnection connection, CancellationToken cancellationToken)
         at Microsoft.EntityFrameworkCore.Update.Internal.BatchExecutor.ExecuteAsync(IEnumerable`1 commandBatches, IRelationalConnection connection, CancellationToken cancellationToken)
         at Microsoft.EntityFrameworkCore.ChangeTracking.Internal.StateManager.SaveChangesAsync(IList`1 entriesToSave, CancellationToken cancellationToken)
         at Microsoft.EntityFrameworkCore.ChangeTracking.Internal.StateManager.SaveChangesAsync(StateManager stateManager, Boolean acceptAllChangesOnSuccess, CancellationToken cancellationToken)
         at Microsoft.EntityFrameworkCore.Storage.ExecutionStrategy.<>c__DisplayClass30_0`2.<<ExecuteAsync>b__0>d.MoveNext()
      --- End of stack trace from previous location ---
         at Microsoft.EntityFrameworkCore.Storage.ExecutionStrategy.ExecuteImplementationAsync[TState,TResult](Func`4 operation, Func`4 verifySucceeded, TState state, CancellationToken cancellationToken)
         at Microsoft.EntityFrameworkCore.Storage.ExecutionStrategy.ExecuteImplementationAsync[TState,TResult](Func`4 operation, Func`4 verifySucceeded, TState state, CancellationToken cancellationToken)
         at Microsoft.EntityFrameworkCore.Storage.ExecutionStrategy.ExecuteAsync[TState,TResult](TState state, Func`4 operation, Func`4 verifySucceeded, CancellationToken cancellationToken)
         at Microsoft.EntityFrameworkCore.DbContext.SaveChangesAsync(Boolean acceptAllChangesOnSuccess, CancellationToken cancellationToken)
         at Microsoft.EntityFrameworkCore.DbContext.SaveChangesAsync(Boolean acceptAllChangesOnSuccess, CancellationToken cancellationToken)
         at WebApplication.BackgroundJobs.StockMonitorJob.RunCycleAsync(CancellationToken cancellationToken) in C:\Users\user\Documents\ANDREI_FILES\PDM_FILES\2ND YEAR\2ND SEM\OOP\OOP-TaurusBikeShop\WebApplication\BackgroundJobs\StockMonitorJob.cs:line 107
fail: WebApplication.BackgroundJobs.StockMonitorJob[0]
      StockMonitorJob cycle failed.
      System.ArgumentException: Keyword not supported: 'userid'.
         at Microsoft.Data.SqlClient.SqlConnectionStringBuilder.GetIndex(String keyword)
         at Microsoft.Data.SqlClient.SqlConnectionStringBuilder.set_Item(String keyword, Object value)
         at System.Data.Common.DbConnectionStringBuilder.set_ConnectionString(String value)
         at Microsoft.Data.SqlClient.SqlConnectionStringBuilder..ctor(String connectionString)
         at Microsoft.EntityFrameworkCore.SqlServer.Storage.Internal.SqlServerConnection.<>c.<get_IsMultipleActiveResultSetsEnabled>b__7_0(String cs)
         at System.Collections.Concurrent.ConcurrentDictionary`2.GetOrAdd(TKey key, Func`2 valueFactory)
         at Microsoft.EntityFrameworkCore.SqlServer.Storage.Internal.SqlServerConnection.get_IsMultipleActiveResultSetsEnabled()
         at Microsoft.EntityFrameworkCore.SqlServer.Query.Internal.SqlServerCompiledQueryCacheKeyGenerator.GenerateCacheKey(Expression query, Boolean async)
         at Microsoft.EntityFrameworkCore.Query.Internal.QueryCompiler.ExecuteAsync[TResult](Expression query, CancellationToken cancellationToken)
         at Microsoft.EntityFrameworkCore.Query.Internal.EntityQueryProvider.ExecuteAsync[TResult](Expression expression, CancellationToken cancellationToken)
         at Microsoft.EntityFrameworkCore.Query.Internal.EntityQueryable`1.GetAsyncEnumerator(CancellationToken cancellationToken)
         at System.Runtime.CompilerServices.ConfiguredCancelableAsyncEnumerable`1.GetAsyncEnumerator()
         at Microsoft.EntityFrameworkCore.EntityFrameworkQueryableExtensions.ToListAsync[TSource](IQueryable`1 source, CancellationToken cancellationToken)
         at WebApplication.BackgroundJobs.StockMonitorJob.RunCycleAsync(CancellationToken cancellationToken) in C:\Users\user\Documents\ANDREI_FILES\PDM_FILES\2ND YEAR\2ND SEM\OOP\OOP-TaurusBikeShop\WebApplication\BackgroundJobs\StockMonitorJob.cs:line 120
fail: Microsoft.EntityFrameworkCore.Update[10000]
      An exception occurred in the database while saving changes for context type 'WebApplication.DataAccess.Context.AppDbContext'.
      System.ArgumentException: Keyword not supported: 'userid'.
         at Microsoft.Data.Common.DbConnectionOptions.ParseInternal(Dictionary`2 parsetable, String connectionString, Boolean buildChain, Dictionary`2 synonyms, Boolean firstKey)
         at Microsoft.Data.Common.DbConnectionOptions..ctor(String connectionString, Dictionary`2 synonyms)
         at Microsoft.Data.SqlClient.SqlConnectionString..ctor(String connectionString)
         at Microsoft.Data.SqlClient.SqlConnectionFactory.CreateConnectionOptions(String connectionString, DbConnectionOptions previous)
         at Microsoft.Data.ProviderBase.DbConnectionFactory.GetConnectionPoolGroup(DbConnectionPoolKey key, DbConnectionPoolGroupOptions poolOptions, DbConnectionOptions& userConnectionOptions)
         at Microsoft.Data.SqlClient.SqlConnection.ConnectionString_Set(DbConnectionPoolKey key)
         at Microsoft.Data.SqlClient.SqlConnection.set_ConnectionString(String value)
         at Microsoft.Data.SqlClient.SqlConnection..ctor(String connectionString)
         at Microsoft.EntityFrameworkCore.SqlServer.Storage.Internal.SqlServerConnection.CreateDbConnection()
         at Microsoft.EntityFrameworkCore.Storage.RelationalConnection.get_DbConnection()
         at Microsoft.EntityFrameworkCore.Storage.RelationalConnection.OpenAsync(CancellationToken cancellationToken, Boolean errorsExpected)
         at Microsoft.EntityFrameworkCore.Update.Internal.BatchExecutor.ExecuteAsync(IEnumerable`1 commandBatches, IRelationalConnection connection, CancellationToken cancellationToken)
         at Microsoft.EntityFrameworkCore.Update.Internal.BatchExecutor.ExecuteAsync(IEnumerable`1 commandBatches, IRelationalConnection connection, CancellationToken cancellationToken)
         at Microsoft.EntityFrameworkCore.Update.Internal.BatchExecutor.ExecuteAsync(IEnumerable`1 commandBatches, IRelationalConnection connection, CancellationToken cancellationToken)
         at Microsoft.EntityFrameworkCore.ChangeTracking.Internal.StateManager.SaveChangesAsync(IList`1 entriesToSave, CancellationToken cancellationToken)
         at Microsoft.EntityFrameworkCore.ChangeTracking.Internal.StateManager.SaveChangesAsync(StateManager stateManager, Boolean acceptAllChangesOnSuccess, CancellationToken cancellationToken)
         at Microsoft.EntityFrameworkCore.Storage.ExecutionStrategy.<>c__DisplayClass30_0`2.<<ExecuteAsync>b__0>d.MoveNext()
      --- End of stack trace from previous location ---
         at Microsoft.EntityFrameworkCore.Storage.ExecutionStrategy.ExecuteImplementationAsync[TState,TResult](Func`4 operation, Func`4 verifySucceeded, TState state, CancellationToken cancellationToken)
         at Microsoft.EntityFrameworkCore.Storage.ExecutionStrategy.ExecuteImplementationAsync[TState,TResult](Func`4 operation, Func`4 verifySucceeded, TState state, CancellationToken cancellationToken)
         at Microsoft.EntityFrameworkCore.Storage.ExecutionStrategy.ExecuteAsync[TState,TResult](TState state, Func`4 operation, Func`4 verifySucceeded, CancellationToken cancellationToken)
         at Microsoft.EntityFrameworkCore.DbContext.SaveChangesAsync(Boolean acceptAllChangesOnSuccess, CancellationToken cancellationToken)
      System.ArgumentException: Keyword not supported: 'userid'.
         at Microsoft.Data.Common.DbConnectionOptions.ParseInternal(Dictionary`2 parsetable, String connectionString, Boolean buildChain, Dictionary`2 synonyms, Boolean firstKey)
         at Microsoft.Data.Common.DbConnectionOptions..ctor(String connectionString, Dictionary`2 synonyms)
         at Microsoft.Data.SqlClient.SqlConnectionString..ctor(String connectionString)
         at Microsoft.Data.SqlClient.SqlConnectionFactory.CreateConnectionOptions(String connectionString, DbConnectionOptions previous)
         at Microsoft.Data.ProviderBase.DbConnectionFactory.GetConnectionPoolGroup(DbConnectionPoolKey key, DbConnectionPoolGroupOptions poolOptions, DbConnectionOptions& userConnectionOptions)
         at Microsoft.Data.SqlClient.SqlConnection.ConnectionString_Set(DbConnectionPoolKey key)
         at Microsoft.Data.SqlClient.SqlConnection.set_ConnectionString(String value)
         at Microsoft.Data.SqlClient.SqlConnection..ctor(String connectionString)
         at Microsoft.EntityFrameworkCore.SqlServer.Storage.Internal.SqlServerConnection.CreateDbConnection()
         at Microsoft.EntityFrameworkCore.Storage.RelationalConnection.get_DbConnection()
         at Microsoft.EntityFrameworkCore.Storage.RelationalConnection.OpenAsync(CancellationToken cancellationToken, Boolean errorsExpected)
         at Microsoft.EntityFrameworkCore.Update.Internal.BatchExecutor.ExecuteAsync(IEnumerable`1 commandBatches, IRelationalConnection connection, CancellationToken cancellationToken)
         at Microsoft.EntityFrameworkCore.Update.Internal.BatchExecutor.ExecuteAsync(IEnumerable`1 commandBatches, IRelationalConnection connection, CancellationToken cancellationToken)
         at Microsoft.EntityFrameworkCore.Update.Internal.BatchExecutor.ExecuteAsync(IEnumerable`1 commandBatches, IRelationalConnection connection, CancellationToken cancellationToken)
         at Microsoft.EntityFrameworkCore.ChangeTracking.Internal.StateManager.SaveChangesAsync(IList`1 entriesToSave, CancellationToken cancellationToken)
         at Microsoft.EntityFrameworkCore.ChangeTracking.Internal.StateManager.SaveChangesAsync(StateManager stateManager, Boolean acceptAllChangesOnSuccess, CancellationToken cancellationToken)
         at Microsoft.EntityFrameworkCore.Storage.ExecutionStrategy.<>c__DisplayClass30_0`2.<<ExecuteAsync>b__0>d.MoveNext()
      --- End of stack trace from previous location ---
         at Microsoft.EntityFrameworkCore.Storage.ExecutionStrategy.ExecuteImplementationAsync[TState,TResult](Func`4 operation, Func`4 verifySucceeded, TState state, CancellationToken cancellationToken)
         at Microsoft.EntityFrameworkCore.Storage.ExecutionStrategy.ExecuteImplementationAsync[TState,TResult](Func`4 operation, Func`4 verifySucceeded, TState state, CancellationToken cancellationToken)
         at Microsoft.EntityFrameworkCore.Storage.ExecutionStrategy.ExecuteAsync[TState,TResult](TState state, Func`4 operation, Func`4 verifySucceeded, CancellationToken cancellationToken)
         at Microsoft.EntityFrameworkCore.DbContext.SaveChangesAsync(Boolean acceptAllChangesOnSuccess, CancellationToken cancellationToken)
fail: WebApplication.BackgroundJobs.StockMonitorJob[0]
      Failed to write error log for StockMonitorJob.
      System.ArgumentException: Keyword not supported: 'userid'.
         at Microsoft.Data.Common.DbConnectionOptions.ParseInternal(Dictionary`2 parsetable, String connectionString, Boolean buildChain, Dictionary`2 synonyms, Boolean firstKey)
         at Microsoft.Data.Common.DbConnectionOptions..ctor(String connectionString, Dictionary`2 synonyms)
         at Microsoft.Data.SqlClient.SqlConnectionString..ctor(String connectionString)
         at Microsoft.Data.SqlClient.SqlConnectionFactory.CreateConnectionOptions(String connectionString, DbConnectionOptions previous)
         at Microsoft.Data.ProviderBase.DbConnectionFactory.GetConnectionPoolGroup(DbConnectionPoolKey key, DbConnectionPoolGroupOptions poolOptions, DbConnectionOptions& userConnectionOptions)
         at Microsoft.Data.SqlClient.SqlConnection.ConnectionString_Set(DbConnectionPoolKey key)
         at Microsoft.Data.SqlClient.SqlConnection.set_ConnectionString(String value)
         at Microsoft.Data.SqlClient.SqlConnection..ctor(String connectionString)
         at Microsoft.EntityFrameworkCore.SqlServer.Storage.Internal.SqlServerConnection.CreateDbConnection()
         at Microsoft.EntityFrameworkCore.Storage.RelationalConnection.get_DbConnection()
         at Microsoft.EntityFrameworkCore.Storage.RelationalConnection.OpenAsync(CancellationToken cancellationToken, Boolean errorsExpected)
         at Microsoft.EntityFrameworkCore.Update.Internal.BatchExecutor.ExecuteAsync(IEnumerable`1 commandBatches, IRelationalConnection connection, CancellationToken cancellationToken)
         at Microsoft.EntityFrameworkCore.Update.Internal.BatchExecutor.ExecuteAsync(IEnumerable`1 commandBatches, IRelationalConnection connection, CancellationToken cancellationToken)
         at Microsoft.EntityFrameworkCore.Update.Internal.BatchExecutor.ExecuteAsync(IEnumerable`1 commandBatches, IRelationalConnection connection, CancellationToken cancellationToken)
         at Microsoft.EntityFrameworkCore.ChangeTracking.Internal.StateManager.SaveChangesAsync(IList`1 entriesToSave, CancellationToken cancellationToken)
         at Microsoft.EntityFrameworkCore.ChangeTracking.Internal.StateManager.SaveChangesAsync(StateManager stateManager, Boolean acceptAllChangesOnSuccess, CancellationToken cancellationToken)
         at Microsoft.EntityFrameworkCore.Storage.ExecutionStrategy.<>c__DisplayClass30_0`2.<<ExecuteAsync>b__0>d.MoveNext()
      --- End of stack trace from previous location ---
         at Microsoft.EntityFrameworkCore.Storage.ExecutionStrategy.ExecuteImplementationAsync[TState,TResult](Func`4 operation, Func`4 verifySucceeded, TState state, CancellationToken cancellationToken)
         at Microsoft.EntityFrameworkCore.Storage.ExecutionStrategy.ExecuteImplementationAsync[TState,TResult](Func`4 operation, Func`4 verifySucceeded, TState state, CancellationToken cancellationToken)
         at Microsoft.EntityFrameworkCore.Storage.ExecutionStrategy.ExecuteAsync[TState,TResult](TState state, Func`4 operation, Func`4 verifySucceeded, CancellationToken cancellationToken)
         at Microsoft.EntityFrameworkCore.DbContext.SaveChangesAsync(Boolean acceptAllChangesOnSuccess, CancellationToken cancellationToken)
         at Microsoft.EntityFrameworkCore.DbContext.SaveChangesAsync(Boolean acceptAllChangesOnSuccess, CancellationToken cancellationToken)
         at WebApplication.BackgroundJobs.StockMonitorJob.RunCycleAsync(CancellationToken cancellationToken) in C:\Users\user\Documents\ANDREI_FILES\PDM_FILES\2ND YEAR\2ND SEM\OOP\OOP-TaurusBikeShop\WebApplication\BackgroundJobs\StockMonitorJob.cs:line 314
         at WebApplication.BackgroundJobs.StockMonitorJob.RunCycleAsync(CancellationToken cancellationToken) in C:\Users\user\Documents\ANDREI_FILES\PDM_FILES\2ND YEAR\2ND SEM\OOP\OOP-TaurusBikeShop\WebApplication\BackgroundJobs\StockMonitorJob.cs:line 314
info: WebApplication.BackgroundJobs.DeliveryStatusPollJob[0]
      DeliveryStatusPollJob started.
fail: Microsoft.EntityFrameworkCore.Update[10000]
      An exception occurred in the database while saving changes for context type 'WebApplication.DataAccess.Context.AppDbContext'.
      System.ArgumentException: Keyword not supported: 'userid'.
         at Microsoft.Data.Common.DbConnectionOptions.ParseInternal(Dictionary`2 parsetable, String connectionString, Boolean buildChain, Dictionary`2 synonyms, Boolean firstKey)
         at Microsoft.Data.Common.DbConnectionOptions..ctor(String connectionString, Dictionary`2 synonyms)
         at Microsoft.Data.SqlClient.SqlConnectionString..ctor(String connectionString)
         at Microsoft.Data.SqlClient.SqlConnectionFactory.CreateConnectionOptions(String connectionString, DbConnectionOptions previous)
         at Microsoft.Data.ProviderBase.DbConnectionFactory.GetConnectionPoolGroup(DbConnectionPoolKey key, DbConnectionPoolGroupOptions poolOptions, DbConnectionOptions& userConnectionOptions)
         at Microsoft.Data.SqlClient.SqlConnection.ConnectionString_Set(DbConnectionPoolKey key)
         at Microsoft.Data.SqlClient.SqlConnection.set_ConnectionString(String value)
         at Microsoft.Data.SqlClient.SqlConnection..ctor(String connectionString)
         at Microsoft.EntityFrameworkCore.SqlServer.Storage.Internal.SqlServerConnection.CreateDbConnection()
         at Microsoft.EntityFrameworkCore.Storage.RelationalConnection.get_DbConnection()
         at Microsoft.EntityFrameworkCore.Storage.RelationalConnection.OpenAsync(CancellationToken cancellationToken, Boolean errorsExpected)
         at Microsoft.EntityFrameworkCore.Update.Internal.BatchExecutor.ExecuteAsync(IEnumerable`1 commandBatches, IRelationalConnection connection, CancellationToken cancellationToken)
         at Microsoft.EntityFrameworkCore.Update.Internal.BatchExecutor.ExecuteAsync(IEnumerable`1 commandBatches, IRelationalConnection connection, CancellationToken cancellationToken)
         at Microsoft.EntityFrameworkCore.Update.Internal.BatchExecutor.ExecuteAsync(IEnumerable`1 commandBatches, IRelationalConnection connection, CancellationToken cancellationToken)
         at Microsoft.EntityFrameworkCore.ChangeTracking.Internal.StateManager.SaveChangesAsync(IList`1 entriesToSave, CancellationToken cancellationToken)
         at Microsoft.EntityFrameworkCore.ChangeTracking.Internal.StateManager.SaveChangesAsync(StateManager stateManager, Boolean acceptAllChangesOnSuccess, CancellationToken cancellationToken)
         at Microsoft.EntityFrameworkCore.Storage.ExecutionStrategy.<>c__DisplayClass30_0`2.<<ExecuteAsync>b__0>d.MoveNext()
      --- End of stack trace from previous location ---
         at Microsoft.EntityFrameworkCore.Storage.ExecutionStrategy.ExecuteImplementationAsync[TState,TResult](Func`4 operation, Func`4 verifySucceeded, TState state, CancellationToken cancellationToken)
         at Microsoft.EntityFrameworkCore.Storage.ExecutionStrategy.ExecuteImplementationAsync[TState,TResult](Func`4 operation, Func`4 verifySucceeded, TState state, CancellationToken cancellationToken)
         at Microsoft.EntityFrameworkCore.Storage.ExecutionStrategy.ExecuteAsync[TState,TResult](TState state, Func`4 operation, Func`4 verifySucceeded, CancellationToken cancellationToken)
         at Microsoft.EntityFrameworkCore.DbContext.SaveChangesAsync(Boolean acceptAllChangesOnSuccess, CancellationToken cancellationToken)
      System.ArgumentException: Keyword not supported: 'userid'.
         at Microsoft.Data.Common.DbConnectionOptions.ParseInternal(Dictionary`2 parsetable, String connectionString, Boolean buildChain, Dictionary`2 synonyms, Boolean firstKey)
         at Microsoft.Data.Common.DbConnectionOptions..ctor(String connectionString, Dictionary`2 synonyms)
         at Microsoft.Data.SqlClient.SqlConnectionString..ctor(String connectionString)
         at Microsoft.Data.SqlClient.SqlConnectionFactory.CreateConnectionOptions(String connectionString, DbConnectionOptions previous)
         at Microsoft.Data.ProviderBase.DbConnectionFactory.GetConnectionPoolGroup(DbConnectionPoolKey key, DbConnectionPoolGroupOptions poolOptions, DbConnectionOptions& userConnectionOptions)
         at Microsoft.Data.SqlClient.SqlConnection.ConnectionString_Set(DbConnectionPoolKey key)
         at Microsoft.Data.SqlClient.SqlConnection.set_ConnectionString(String value)
         at Microsoft.Data.SqlClient.SqlConnection..ctor(String connectionString)
         at Microsoft.EntityFrameworkCore.SqlServer.Storage.Internal.SqlServerConnection.CreateDbConnection()
         at Microsoft.EntityFrameworkCore.Storage.RelationalConnection.get_DbConnection()
         at Microsoft.EntityFrameworkCore.Storage.RelationalConnection.OpenAsync(CancellationToken cancellationToken, Boolean errorsExpected)
         at Microsoft.EntityFrameworkCore.Update.Internal.BatchExecutor.ExecuteAsync(IEnumerable`1 commandBatches, IRelationalConnection connection, CancellationToken cancellationToken)
         at Microsoft.EntityFrameworkCore.Update.Internal.BatchExecutor.ExecuteAsync(IEnumerable`1 commandBatches, IRelationalConnection connection, CancellationToken cancellationToken)
         at Microsoft.EntityFrameworkCore.Update.Internal.BatchExecutor.ExecuteAsync(IEnumerable`1 commandBatches, IRelationalConnection connection, CancellationToken cancellationToken)
         at Microsoft.EntityFrameworkCore.ChangeTracking.Internal.StateManager.SaveChangesAsync(IList`1 entriesToSave, CancellationToken cancellationToken)
         at Microsoft.EntityFrameworkCore.ChangeTracking.Internal.StateManager.SaveChangesAsync(StateManager stateManager, Boolean acceptAllChangesOnSuccess, CancellationToken cancellationToken)
         at Microsoft.EntityFrameworkCore.Storage.ExecutionStrategy.<>c__DisplayClass30_0`2.<<ExecuteAsync>b__0>d.MoveNext()
      --- End of stack trace from previous location ---
         at Microsoft.EntityFrameworkCore.Storage.ExecutionStrategy.ExecuteImplementationAsync[TState,TResult](Func`4 operation, Func`4 verifySucceeded, TState state, CancellationToken cancellationToken)
         at Microsoft.EntityFrameworkCore.Storage.ExecutionStrategy.ExecuteImplementationAsync[TState,TResult](Func`4 operation, Func`4 verifySucceeded, TState state, CancellationToken cancellationToken)
         at Microsoft.EntityFrameworkCore.Storage.ExecutionStrategy.ExecuteAsync[TState,TResult](TState state, Func`4 operation, Func`4 verifySucceeded, CancellationToken cancellationToken)
         at Microsoft.EntityFrameworkCore.DbContext.SaveChangesAsync(Boolean acceptAllChangesOnSuccess, CancellationToken cancellationToken)
warn: WebApplication.BackgroundJobs.DeliveryStatusPollJob[0]
      DeliveryStatusPollJob could not write start log - DB may be unavailable.
      System.ArgumentException: Keyword not supported: 'userid'.
         at Microsoft.Data.Common.DbConnectionOptions.ParseInternal(Dictionary`2 parsetable, String connectionString, Boolean buildChain, Dictionary`2 synonyms, Boolean firstKey)
         at Microsoft.Data.Common.DbConnectionOptions..ctor(String connectionString, Dictionary`2 synonyms)
         at Microsoft.Data.SqlClient.SqlConnectionString..ctor(String connectionString)
         at Microsoft.Data.SqlClient.SqlConnectionFactory.CreateConnectionOptions(String connectionString, DbConnectionOptions previous)
         at Microsoft.Data.ProviderBase.DbConnectionFactory.GetConnectionPoolGroup(DbConnectionPoolKey key, DbConnectionPoolGroupOptions poolOptions, DbConnectionOptions& userConnectionOptions)
         at Microsoft.Data.SqlClient.SqlConnection.ConnectionString_Set(DbConnectionPoolKey key)
         at Microsoft.Data.SqlClient.SqlConnection.set_ConnectionString(String value)
         at Microsoft.Data.SqlClient.SqlConnection..ctor(String connectionString)
         at Microsoft.EntityFrameworkCore.SqlServer.Storage.Internal.SqlServerConnection.CreateDbConnection()
         at Microsoft.EntityFrameworkCore.Storage.RelationalConnection.get_DbConnection()
         at Microsoft.EntityFrameworkCore.Storage.RelationalConnection.OpenAsync(CancellationToken cancellationToken, Boolean errorsExpected)
         at Microsoft.EntityFrameworkCore.Update.Internal.BatchExecutor.ExecuteAsync(IEnumerable`1 commandBatches, IRelationalConnection connection, CancellationToken cancellationToken)
         at Microsoft.EntityFrameworkCore.Update.Internal.BatchExecutor.ExecuteAsync(IEnumerable`1 commandBatches, IRelationalConnection connection, CancellationToken cancellationToken)
         at Microsoft.EntityFrameworkCore.Update.Internal.BatchExecutor.ExecuteAsync(IEnumerable`1 commandBatches, IRelationalConnection connection, CancellationToken cancellationToken)
         at Microsoft.EntityFrameworkCore.ChangeTracking.Internal.StateManager.SaveChangesAsync(IList`1 entriesToSave, CancellationToken cancellationToken)
         at Microsoft.EntityFrameworkCore.ChangeTracking.Internal.StateManager.SaveChangesAsync(StateManager stateManager, Boolean acceptAllChangesOnSuccess, CancellationToken cancellationToken)
         at Microsoft.EntityFrameworkCore.Storage.ExecutionStrategy.<>c__DisplayClass30_0`2.<<ExecuteAsync>b__0>d.MoveNext()
      --- End of stack trace from previous location ---
         at Microsoft.EntityFrameworkCore.Storage.ExecutionStrategy.ExecuteImplementationAsync[TState,TResult](Func`4 operation, Func`4 verifySucceeded, TState state, CancellationToken cancellationToken)
         at Microsoft.EntityFrameworkCore.Storage.ExecutionStrategy.ExecuteImplementationAsync[TState,TResult](Func`4 operation, Func`4 verifySucceeded, TState state, CancellationToken cancellationToken)
         at Microsoft.EntityFrameworkCore.Storage.ExecutionStrategy.ExecuteAsync[TState,TResult](TState state, Func`4 operation, Func`4 verifySucceeded, CancellationToken cancellationToken)
         at Microsoft.EntityFrameworkCore.DbContext.SaveChangesAsync(Boolean acceptAllChangesOnSuccess, CancellationToken cancellationToken)
         at Microsoft.EntityFrameworkCore.DbContext.SaveChangesAsync(Boolean acceptAllChangesOnSuccess, CancellationToken cancellationToken)
         at WebApplication.BackgroundJobs.DeliveryStatusPollJob.RunCycleAsync(CancellationToken cancellationToken) in C:\Users\user\Documents\ANDREI_FILES\PDM_FILES\2ND YEAR\2ND SEM\OOP\OOP-TaurusBikeShop\WebApplication\BackgroundJobs\DeliveryStatusPollJob.cs:line 87
fail: WebApplication.BackgroundJobs.DeliveryStatusPollJob[0]
      DeliveryStatusPollJob cycle failed.
      System.ArgumentException: Keyword not supported: 'userid'.
         at Microsoft.Data.SqlClient.SqlConnectionStringBuilder.GetIndex(String keyword)
         at Microsoft.Data.SqlClient.SqlConnectionStringBuilder.set_Item(String keyword, Object value)
         at System.Data.Common.DbConnectionStringBuilder.set_ConnectionString(String value)
         at Microsoft.Data.SqlClient.SqlConnectionStringBuilder..ctor(String connectionString)
         at Microsoft.EntityFrameworkCore.SqlServer.Storage.Internal.SqlServerConnection.<>c.<get_IsMultipleActiveResultSetsEnabled>b__7_0(String cs)
         at System.Collections.Concurrent.ConcurrentDictionary`2.GetOrAdd(TKey key, Func`2 valueFactory)
         at Microsoft.EntityFrameworkCore.SqlServer.Storage.Internal.SqlServerConnection.get_IsMultipleActiveResultSetsEnabled()
         at Microsoft.EntityFrameworkCore.SqlServer.Query.Internal.SqlServerCompiledQueryCacheKeyGenerator.GenerateCacheKey(Expression query, Boolean async)
         at Microsoft.EntityFrameworkCore.Query.Internal.QueryCompiler.ExecuteAsync[TResult](Expression query, CancellationToken cancellationToken)
         at Microsoft.EntityFrameworkCore.Query.Internal.EntityQueryProvider.ExecuteAsync[TResult](Expression expression, CancellationToken cancellationToken)
         at Microsoft.EntityFrameworkCore.Query.Internal.EntityQueryable`1.GetAsyncEnumerator(CancellationToken cancellationToken)
         at System.Runtime.CompilerServices.ConfiguredCancelableAsyncEnumerable`1.GetAsyncEnumerator()
         at Microsoft.EntityFrameworkCore.EntityFrameworkQueryableExtensions.ToListAsync[TSource](IQueryable`1 source, CancellationToken cancellationToken)
         at WebApplication.BackgroundJobs.DeliveryStatusPollJob.RunCycleAsync(CancellationToken cancellationToken) in C:\Users\user\Documents\ANDREI_FILES\PDM_FILES\2ND YEAR\2ND SEM\OOP\OOP-TaurusBikeShop\WebApplication\BackgroundJobs\DeliveryStatusPollJob.cs:line 100
fail: Microsoft.EntityFrameworkCore.Update[10000]
      An exception occurred in the database while saving changes for context type 'WebApplication.DataAccess.Context.AppDbContext'.
      System.ArgumentException: Keyword not supported: 'userid'.
         at Microsoft.Data.Common.DbConnectionOptions.ParseInternal(Dictionary`2 parsetable, String connectionString, Boolean buildChain, Dictionary`2 synonyms, Boolean firstKey)
         at Microsoft.Data.Common.DbConnectionOptions..ctor(String connectionString, Dictionary`2 synonyms)
         at Microsoft.Data.SqlClient.SqlConnectionString..ctor(String connectionString)
         at Microsoft.Data.SqlClient.SqlConnectionFactory.CreateConnectionOptions(String connectionString, DbConnectionOptions previous)
         at Microsoft.Data.ProviderBase.DbConnectionFactory.GetConnectionPoolGroup(DbConnectionPoolKey key, DbConnectionPoolGroupOptions poolOptions, DbConnectionOptions& userConnectionOptions)
         at Microsoft.Data.SqlClient.SqlConnection.ConnectionString_Set(DbConnectionPoolKey key)
         at Microsoft.Data.SqlClient.SqlConnection.set_ConnectionString(String value)
         at Microsoft.Data.SqlClient.SqlConnection..ctor(String connectionString)
         at Microsoft.EntityFrameworkCore.SqlServer.Storage.Internal.SqlServerConnection.CreateDbConnection()
         at Microsoft.EntityFrameworkCore.Storage.RelationalConnection.get_DbConnection()
         at Microsoft.EntityFrameworkCore.Storage.RelationalConnection.OpenAsync(CancellationToken cancellationToken, Boolean errorsExpected)
         at Microsoft.EntityFrameworkCore.Update.Internal.BatchExecutor.ExecuteAsync(IEnumerable`1 commandBatches, IRelationalConnection connection, CancellationToken cancellationToken)
         at Microsoft.EntityFrameworkCore.Update.Internal.BatchExecutor.ExecuteAsync(IEnumerable`1 commandBatches, IRelationalConnection connection, CancellationToken cancellationToken)
         at Microsoft.EntityFrameworkCore.Update.Internal.BatchExecutor.ExecuteAsync(IEnumerable`1 commandBatches, IRelationalConnection connection, CancellationToken cancellationToken)
         at Microsoft.EntityFrameworkCore.ChangeTracking.Internal.StateManager.SaveChangesAsync(IList`1 entriesToSave, CancellationToken cancellationToken)
         at Microsoft.EntityFrameworkCore.ChangeTracking.Internal.StateManager.SaveChangesAsync(StateManager stateManager, Boolean acceptAllChangesOnSuccess, CancellationToken cancellationToken)
         at Microsoft.EntityFrameworkCore.Storage.ExecutionStrategy.<>c__DisplayClass30_0`2.<<ExecuteAsync>b__0>d.MoveNext()
      --- End of stack trace from previous location ---
         at Microsoft.EntityFrameworkCore.Storage.ExecutionStrategy.ExecuteImplementationAsync[TState,TResult](Func`4 operation, Func`4 verifySucceeded, TState state, CancellationToken cancellationToken)
         at Microsoft.EntityFrameworkCore.Storage.ExecutionStrategy.ExecuteImplementationAsync[TState,TResult](Func`4 operation, Func`4 verifySucceeded, TState state, CancellationToken cancellationToken)
         at Microsoft.EntityFrameworkCore.Storage.ExecutionStrategy.ExecuteAsync[TState,TResult](TState state, Func`4 operation, Func`4 verifySucceeded, CancellationToken cancellationToken)
         at Microsoft.EntityFrameworkCore.DbContext.SaveChangesAsync(Boolean acceptAllChangesOnSuccess, CancellationToken cancellationToken)
      System.ArgumentException: Keyword not supported: 'userid'.
         at Microsoft.Data.Common.DbConnectionOptions.ParseInternal(Dictionary`2 parsetable, String connectionString, Boolean buildChain, Dictionary`2 synonyms, Boolean firstKey)
         at Microsoft.Data.Common.DbConnectionOptions..ctor(String connectionString, Dictionary`2 synonyms)
         at Microsoft.Data.SqlClient.SqlConnectionString..ctor(String connectionString)
         at Microsoft.Data.SqlClient.SqlConnectionFactory.CreateConnectionOptions(String connectionString, DbConnectionOptions previous)
         at Microsoft.Data.ProviderBase.DbConnectionFactory.GetConnectionPoolGroup(DbConnectionPoolKey key, DbConnectionPoolGroupOptions poolOptions, DbConnectionOptions& userConnectionOptions)
         at Microsoft.Data.SqlClient.SqlConnection.ConnectionString_Set(DbConnectionPoolKey key)
         at Microsoft.Data.SqlClient.SqlConnection.set_ConnectionString(String value)
         at Microsoft.Data.SqlClient.SqlConnection..ctor(String connectionString)
         at Microsoft.EntityFrameworkCore.SqlServer.Storage.Internal.SqlServerConnection.CreateDbConnection()
         at Microsoft.EntityFrameworkCore.Storage.RelationalConnection.get_DbConnection()
         at Microsoft.EntityFrameworkCore.Storage.RelationalConnection.OpenAsync(CancellationToken cancellationToken, Boolean errorsExpected)
         at Microsoft.EntityFrameworkCore.Update.Internal.BatchExecutor.ExecuteAsync(IEnumerable`1 commandBatches, IRelationalConnection connection, CancellationToken cancellationToken)
         at Microsoft.EntityFrameworkCore.Update.Internal.BatchExecutor.ExecuteAsync(IEnumerable`1 commandBatches, IRelationalConnection connection, CancellationToken cancellationToken)
         at Microsoft.EntityFrameworkCore.Update.Internal.BatchExecutor.ExecuteAsync(IEnumerable`1 commandBatches, IRelationalConnection connection, CancellationToken cancellationToken)
         at Microsoft.EntityFrameworkCore.ChangeTracking.Internal.StateManager.SaveChangesAsync(IList`1 entriesToSave, CancellationToken cancellationToken)
         at Microsoft.EntityFrameworkCore.ChangeTracking.Internal.StateManager.SaveChangesAsync(StateManager stateManager, Boolean acceptAllChangesOnSuccess, CancellationToken cancellationToken)
         at Microsoft.EntityFrameworkCore.Storage.ExecutionStrategy.<>c__DisplayClass30_0`2.<<ExecuteAsync>b__0>d.MoveNext()
      --- End of stack trace from previous location ---
         at Microsoft.EntityFrameworkCore.Storage.ExecutionStrategy.ExecuteImplementationAsync[TState,TResult](Func`4 operation, Func`4 verifySucceeded, TState state, CancellationToken cancellationToken)
         at Microsoft.EntityFrameworkCore.Storage.ExecutionStrategy.ExecuteImplementationAsync[TState,TResult](Func`4 operation, Func`4 verifySucceeded, TState state, CancellationToken cancellationToken)
         at Microsoft.EntityFrameworkCore.Storage.ExecutionStrategy.ExecuteAsync[TState,TResult](TState state, Func`4 operation, Func`4 verifySucceeded, CancellationToken cancellationToken)
         at Microsoft.EntityFrameworkCore.DbContext.SaveChangesAsync(Boolean acceptAllChangesOnSuccess, CancellationToken cancellationToken)
fail: WebApplication.BackgroundJobs.DeliveryStatusPollJob[0]
      Failed to write error log for DeliveryStatusPollJob.
      System.ArgumentException: Keyword not supported: 'userid'.
         at Microsoft.Data.Common.DbConnectionOptions.ParseInternal(Dictionary`2 parsetable, String connectionString, Boolean buildChain, Dictionary`2 synonyms, Boolean firstKey)
         at Microsoft.Data.Common.DbConnectionOptions..ctor(String connectionString, Dictionary`2 synonyms)
         at Microsoft.Data.SqlClient.SqlConnectionString..ctor(String connectionString)
         at Microsoft.Data.SqlClient.SqlConnectionFactory.CreateConnectionOptions(String connectionString, DbConnectionOptions previous)
         at Microsoft.Data.ProviderBase.DbConnectionFactory.GetConnectionPoolGroup(DbConnectionPoolKey key, DbConnectionPoolGroupOptions poolOptions, DbConnectionOptions& userConnectionOptions)
         at Microsoft.Data.SqlClient.SqlConnection.ConnectionString_Set(DbConnectionPoolKey key)
         at Microsoft.Data.SqlClient.SqlConnection.set_ConnectionString(String value)
         at Microsoft.Data.SqlClient.SqlConnection..ctor(String connectionString)
         at Microsoft.EntityFrameworkCore.SqlServer.Storage.Internal.SqlServerConnection.CreateDbConnection()
         at Microsoft.EntityFrameworkCore.Storage.RelationalConnection.get_DbConnection()
         at Microsoft.EntityFrameworkCore.Storage.RelationalConnection.OpenAsync(CancellationToken cancellationToken, Boolean errorsExpected)
         at Microsoft.EntityFrameworkCore.Update.Internal.BatchExecutor.ExecuteAsync(IEnumerable`1 commandBatches, IRelationalConnection connection, CancellationToken cancellationToken)
         at Microsoft.EntityFrameworkCore.Update.Internal.BatchExecutor.ExecuteAsync(IEnumerable`1 commandBatches, IRelationalConnection connection, CancellationToken cancellationToken)
         at Microsoft.EntityFrameworkCore.Update.Internal.BatchExecutor.ExecuteAsync(IEnumerable`1 commandBatches, IRelationalConnection connection, CancellationToken cancellationToken)
         at Microsoft.EntityFrameworkCore.ChangeTracking.Internal.StateManager.SaveChangesAsync(IList`1 entriesToSave, CancellationToken cancellationToken)
         at Microsoft.EntityFrameworkCore.ChangeTracking.Internal.StateManager.SaveChangesAsync(StateManager stateManager, Boolean acceptAllChangesOnSuccess, CancellationToken cancellationToken)
         at Microsoft.EntityFrameworkCore.Storage.ExecutionStrategy.<>c__DisplayClass30_0`2.<<ExecuteAsync>b__0>d.MoveNext()
      --- End of stack trace from previous location ---
         at Microsoft.EntityFrameworkCore.Storage.ExecutionStrategy.ExecuteImplementationAsync[TState,TResult](Func`4 operation, Func`4 verifySucceeded, TState state, CancellationToken cancellationToken)
         at Microsoft.EntityFrameworkCore.Storage.ExecutionStrategy.ExecuteImplementationAsync[TState,TResult](Func`4 operation, Func`4 verifySucceeded, TState state, CancellationToken cancellationToken)
         at Microsoft.EntityFrameworkCore.Storage.ExecutionStrategy.ExecuteAsync[TState,TResult](TState state, Func`4 operation, Func`4 verifySucceeded, CancellationToken cancellationToken)
         at Microsoft.EntityFrameworkCore.DbContext.SaveChangesAsync(Boolean acceptAllChangesOnSuccess, CancellationToken cancellationToken)
         at Microsoft.EntityFrameworkCore.DbContext.SaveChangesAsync(Boolean acceptAllChangesOnSuccess, CancellationToken cancellationToken)
         at WebApplication.BackgroundJobs.DeliveryStatusPollJob.RunCycleAsync(CancellationToken cancellationToken) in C:\Users\user\Documents\ANDREI_FILES\PDM_FILES\2ND YEAR\2ND SEM\OOP\OOP-TaurusBikeShop\WebApplication\BackgroundJobs\DeliveryStatusPollJob.cs:line 168
         at WebApplication.BackgroundJobs.DeliveryStatusPollJob.RunCycleAsync(CancellationToken cancellationToken) in C:\Users\user\Documents\ANDREI_FILES\PDM_FILES\2ND YEAR\2ND SEM\OOP\OOP-TaurusBikeShop\WebApplication\BackgroundJobs\DeliveryStatusPollJob.cs:line 168
info: Microsoft.Hosting.Lifetime[14]
      Now listening on: https://localhost:7177
info: Microsoft.Hosting.Lifetime[14]
      Now listening on: http://localhost:5064
info: Microsoft.Hosting.Lifetime[0]
      Application started. Press Ctrl+C to shut down.
info: Microsoft.Hosting.Lifetime[0]
      Hosting environment: Development
info: Microsoft.Hosting.Lifetime[0]
      Content root path: C:\Users\user\Documents\ANDREI_FILES\PDM_FILES\2ND YEAR\2ND SEM\OOP\OOP-TaurusBikeShop\WebApplication
info: Microsoft.AspNetCore.Hosting.Diagnostics[1]
      Request starting HTTP/2 GET https://localhost:7177/ - - -
info: Microsoft.AspNetCore.Routing.EndpointMiddleware[0]
      Executing endpoint 'WebApplication.Controllers.HomeController.Index (WebApplication)'
info: Microsoft.AspNetCore.Mvc.Infrastructure.ControllerActionInvoker[102]
      Route matched with {action = "Index", controller = "Home"}. Executing controller action with signature System.Threading.Tasks.Task`1[Microsoft.AspNetCore.Mvc.IActionResult] Index(System.Threading.CancellationToken) on controller WebApplication.Controllers.HomeController (WebApplication).
fail: WebApplication.Controllers.HomeController[0]
      Failed to load featured products for homepage.
      System.ArgumentException: Keyword not supported: 'userid'.
         at Microsoft.Data.SqlClient.SqlConnectionStringBuilder.GetIndex(String keyword)
         at Microsoft.Data.SqlClient.SqlConnectionStringBuilder.set_Item(String keyword, Object value)
         at System.Data.Common.DbConnectionStringBuilder.set_ConnectionString(String value)
         at Microsoft.Data.SqlClient.SqlConnectionStringBuilder..ctor(String connectionString)
         at Microsoft.EntityFrameworkCore.SqlServer.Storage.Internal.SqlServerConnection.<>c.<get_IsMultipleActiveResultSetsEnabled>b__7_0(String cs)
         at System.Collections.Concurrent.ConcurrentDictionary`2.GetOrAdd(TKey key, Func`2 valueFactory)
         at Microsoft.EntityFrameworkCore.SqlServer.Storage.Internal.SqlServerConnection.get_IsMultipleActiveResultSetsEnabled()
         at Microsoft.EntityFrameworkCore.SqlServer.Query.Internal.SqlServerCompiledQueryCacheKeyGenerator.GenerateCacheKey(Expression query, Boolean async)
         at Microsoft.EntityFrameworkCore.Query.Internal.QueryCompiler.ExecuteAsync[TResult](Expression query, CancellationToken cancellationToken)
         at Microsoft.EntityFrameworkCore.Query.Internal.EntityQueryProvider.ExecuteAsync[TResult](Expression expression, CancellationToken cancellationToken)
         at Microsoft.EntityFrameworkCore.Query.Internal.EntityQueryable`1.GetAsyncEnumerator(CancellationToken cancellationToken)
         at System.Runtime.CompilerServices.ConfiguredCancelableAsyncEnumerable`1.GetAsyncEnumerator()
         at Microsoft.EntityFrameworkCore.EntityFrameworkQueryableExtensions.ToListAsync[TSource](IQueryable`1 source, CancellationToken cancellationToken)
         at WebApplication.DataAccess.Repositories.ProductRepository.GetFeaturedAsync(Int32 count, CancellationToken cancellationToken) in C:\Users\user\Documents\ANDREI_FILES\PDM_FILES\2ND YEAR\2ND SEM\OOP\OOP-TaurusBikeShop\WebApplication\DataAccess\Repositories\ProductRepository.cs:line 194
         at WebApplication.BusinessLogic.Services.ProductService.GetFeaturedAsync(Int32 count, CancellationToken cancellationToken) in C:\Users\user\Documents\ANDREI_FILES\PDM_FILES\2ND YEAR\2ND SEM\OOP\OOP-TaurusBikeShop\WebApplication\BusinessLogic\Services\ProductService.cs:line 131
         at WebApplication.Controllers.HomeController.Index(CancellationToken cancellationToken) in C:\Users\user\Documents\ANDREI_FILES\PDM_FILES\2ND YEAR\2ND SEM\OOP\OOP-TaurusBikeShop\WebApplication\Controllers\HomeController.cs:line 39
info: Microsoft.AspNetCore.Mvc.ViewFeatures.ViewResultExecutor[1]
      Executing ViewResult, running view Index.
info: Microsoft.AspNetCore.Mvc.ViewFeatures.ViewResultExecutor[4]
      Executed ViewResult - view Index executed in 98.8835ms.
info: Microsoft.AspNetCore.Hosting.Diagnostics[1]
      Request starting HTTP/2 GET https://localhost:7177/js/mesh-gradient-bg.js - - -
info: Microsoft.AspNetCore.Hosting.Diagnostics[1]
      Request starting HTTP/2 GET https://localhost:7177/_vs/browserLink - - -
info: Microsoft.AspNetCore.Hosting.Diagnostics[1]
      Request starting HTTP/2 GET https://localhost:7177/_framework/aspnetcore-browser-refresh.js - - -
info: Microsoft.AspNetCore.Mvc.Infrastructure.ControllerActionInvoker[105]
      Executed action WebApplication.Controllers.HomeController.Index (WebApplication) in 208.6726ms
info: Microsoft.AspNetCore.Routing.EndpointMiddleware[1]
      Executed endpoint 'WebApplication.Controllers.HomeController.Index (WebApplication)'
info: Microsoft.AspNetCore.Hosting.Diagnostics[2]
      Request finished HTTP/2 GET https://localhost:7177/_framework/aspnetcore-browser-refresh.js - 200 14933 application/javascript;+charset=utf-8 26.2242ms
info: Microsoft.AspNetCore.Hosting.Diagnostics[2]
      Request finished HTTP/2 GET https://localhost:7177/js/mesh-gradient-bg.js - 404 0 - 33.3644ms
info: Microsoft.AspNetCore.Hosting.Diagnostics[16]
      Request reached the end of the middleware pipeline without being handled by application code. Request path: GET https://localhost:7177/js/mesh-gradient-bg.js, Response status code: 404
info: Microsoft.AspNetCore.Hosting.Diagnostics[2]
      Request finished HTTP/2 GET https://localhost:7177/ - 200 - text/html;+charset=utf-8 347.0384ms
info: Microsoft.AspNetCore.Hosting.Diagnostics[2]
      Request finished HTTP/2 GET https://localhost:7177/_vs/browserLink - 200 - text/javascript;+charset=UTF-8 65.7347ms
info: Microsoft.AspNetCore.Hosting.Diagnostics[1]
      Request starting HTTP/2 GET https://localhost:7177/Cart/GetCartCount - - -
info: Microsoft.AspNetCore.Hosting.Diagnostics[1]
      Request starting HTTP/2 GET https://localhost:7177/antiforgery/token - - -
info: Microsoft.AspNetCore.Routing.EndpointMiddleware[0]
      Executing endpoint 'WebApplication.Controllers.CartController.GetCartCount (WebApplication)'
info: Microsoft.AspNetCore.Hosting.Diagnostics[2]
      Request finished HTTP/2 GET https://localhost:7177/antiforgery/token - 404 0 - 6.1901ms
info: Microsoft.AspNetCore.Hosting.Diagnostics[16]
      Request reached the end of the middleware pipeline without being handled by application code. Request path: GET https://localhost:7177/antiforgery/token, Response status code: 404
info: Microsoft.AspNetCore.Mvc.Infrastructure.ControllerActionInvoker[102]
      Route matched with {action = "GetCartCount", controller = "Cart"}. Executing controller action with signature System.Threading.Tasks.Task`1[Microsoft.AspNetCore.Mvc.IActionResult] GetCartCount(System.Threading.CancellationToken) on controller WebApplication.Controllers.CartController (WebApplication).
info: Microsoft.AspNetCore.Mvc.Infrastructure.SystemTextJsonResultExecutor[1]
      Executing JsonResult, writing value of type 'WebApplication.Models.ApiResponse'.
info: Microsoft.AspNetCore.Mvc.Infrastructure.ControllerActionInvoker[105]
      Executed action WebApplication.Controllers.CartController.GetCartCount (WebApplication) in 22.1483ms
info: Microsoft.AspNetCore.Routing.EndpointMiddleware[1]
      Executed endpoint 'WebApplication.Controllers.CartController.GetCartCount (WebApplication)'
info: Microsoft.AspNetCore.Hosting.Diagnostics[2]
      Request finished HTTP/2 GET https://localhost:7177/Cart/GetCartCount - 200 - application/json;+charset=utf-8 42.6463ms
info: Microsoft.AspNetCore.Hosting.Diagnostics[1]
      Request starting HTTP/2 GET https://localhost:7177/Customer/Login - - -
info: Microsoft.AspNetCore.Routing.EndpointMiddleware[0]
      Executing endpoint 'WebApplication.Controllers.CustomerController.Login (WebApplication)'
info: Microsoft.AspNetCore.Mvc.Infrastructure.ControllerActionInvoker[102]
      Route matched with {action = "Login", controller = "Customer"}. Executing controller action with signature Microsoft.AspNetCore.Mvc.IActionResult Login(System.String) on controller WebApplication.Controllers.CustomerController (WebApplication).
info: Microsoft.AspNetCore.Mvc.ViewFeatures.ViewResultExecutor[1]
      Executing ViewResult, running view Login.
info: Microsoft.AspNetCore.Mvc.ViewFeatures.ViewResultExecutor[4]
      Executed ViewResult - view Login executed in 33.552ms.
info: Microsoft.AspNetCore.Mvc.Infrastructure.ControllerActionInvoker[105]
      Executed action WebApplication.Controllers.CustomerController.Login (WebApplication) in 39.7005ms
info: Microsoft.AspNetCore.Routing.EndpointMiddleware[1]
      Executed endpoint 'WebApplication.Controllers.CustomerController.Login (WebApplication)'
info: Microsoft.AspNetCore.Hosting.Diagnostics[2]
      Request finished HTTP/2 GET https://localhost:7177/Customer/Login - 200 - text/html;+charset=utf-8 53.7849ms
info: Microsoft.AspNetCore.Hosting.Diagnostics[1]
      Request starting HTTP/2 GET https://localhost:7177/_framework/aspnetcore-browser-refresh.js - - -
info: Microsoft.AspNetCore.Hosting.Diagnostics[1]
      Request starting HTTP/2 GET https://localhost:7177/_vs/browserLink - - -
info: Microsoft.AspNetCore.Hosting.Diagnostics[2]
      Request finished HTTP/2 GET https://localhost:7177/_framework/aspnetcore-browser-refresh.js - 200 14933 application/javascript;+charset=utf-8 3.7866ms
info: Microsoft.AspNetCore.Hosting.Diagnostics[1]
      Request starting HTTP/2 GET https://localhost:7177/Cart/GetCartCount - - -
info: Microsoft.AspNetCore.Hosting.Diagnostics[2]
      Request finished HTTP/2 GET https://localhost:7177/_vs/browserLink - 200 - text/javascript;+charset=UTF-8 20.3028ms
info: Microsoft.AspNetCore.Routing.EndpointMiddleware[0]
      Executing endpoint 'WebApplication.Controllers.CartController.GetCartCount (WebApplication)'
info: Microsoft.AspNetCore.Mvc.Infrastructure.ControllerActionInvoker[102]
      Route matched with {action = "GetCartCount", controller = "Cart"}. Executing controller action with signature System.Threading.Tasks.Task`1[Microsoft.AspNetCore.Mvc.IActionResult] GetCartCount(System.Threading.CancellationToken) on controller WebApplication.Controllers.CartController (WebApplication).
info: Microsoft.AspNetCore.Mvc.Infrastructure.SystemTextJsonResultExecutor[1]
      Executing JsonResult, writing value of type 'WebApplication.Models.ApiResponse'.
info: Microsoft.AspNetCore.Mvc.Infrastructure.ControllerActionInvoker[105]
      Executed action WebApplication.Controllers.CartController.GetCartCount (WebApplication) in 5.4227ms
info: Microsoft.AspNetCore.Routing.EndpointMiddleware[1]
      Executed endpoint 'WebApplication.Controllers.CartController.GetCartCount (WebApplication)'
info: Microsoft.AspNetCore.Hosting.Diagnostics[2]
      Request finished HTTP/2 GET https://localhost:7177/Cart/GetCartCount - 200 - application/json;+charset=utf-8 17.0301ms
info: Microsoft.AspNetCore.Hosting.Diagnostics[1]
      Request starting HTTP/2 POST https://localhost:7177/Customer/Login - application/x-www-form-urlencoded 240
info: Microsoft.AspNetCore.Routing.EndpointMiddleware[0]
      Executing endpoint 'WebApplication.Controllers.CustomerController.Login (WebApplication)'
info: Microsoft.AspNetCore.Mvc.Infrastructure.ControllerActionInvoker[102]
      Route matched with {action = "Login", controller = "Customer"}. Executing controller action with signature System.Threading.Tasks.Task`1[Microsoft.AspNetCore.Mvc.IActionResult] Login(WebApplication.Models.ViewModels.LoginViewModel, System.Threading.CancellationToken) on controller WebApplication.Controllers.CustomerController (WebApplication).
fail: WebApplication.Controllers.CustomerController[0]
      Login failed for juan.dc@email.com.
      System.ArgumentException: Keyword not supported: 'userid'.
         at Microsoft.Data.SqlClient.SqlConnectionStringBuilder.GetIndex(String keyword)
         at Microsoft.Data.SqlClient.SqlConnectionStringBuilder.set_Item(String keyword, Object value)
         at System.Data.Common.DbConnectionStringBuilder.set_ConnectionString(String value)
         at Microsoft.Data.SqlClient.SqlConnectionStringBuilder..ctor(String connectionString)
         at Microsoft.EntityFrameworkCore.SqlServer.Storage.Internal.SqlServerConnection.<>c.<get_IsMultipleActiveResultSetsEnabled>b__7_0(String cs)
         at System.Collections.Concurrent.ConcurrentDictionary`2.GetOrAdd(TKey key, Func`2 valueFactory)
         at Microsoft.EntityFrameworkCore.SqlServer.Storage.Internal.SqlServerConnection.get_IsMultipleActiveResultSetsEnabled()
         at Microsoft.EntityFrameworkCore.SqlServer.Query.Internal.SqlServerCompiledQueryCacheKeyGenerator.GenerateCacheKey(Expression query, Boolean async)
         at Microsoft.EntityFrameworkCore.Query.Internal.QueryCompiler.ExecuteAsync[TResult](Expression query, CancellationToken cancellationToken)
         at Microsoft.EntityFrameworkCore.Query.Internal.EntityQueryProvider.ExecuteAsync[TResult](Expression expression, CancellationToken cancellationToken)
         at Microsoft.EntityFrameworkCore.EntityFrameworkQueryableExtensions.ExecuteAsync[TSource,TResult](MethodInfo operatorMethodInfo, IQueryable`1 source, Expression expression, CancellationToken cancellationToken)
         at Microsoft.EntityFrameworkCore.EntityFrameworkQueryableExtensions.ExecuteAsync[TSource,TResult](MethodInfo operatorMethodInfo, IQueryable`1 source, LambdaExpression expression, CancellationToken cancellationToken)
         at Microsoft.EntityFrameworkCore.EntityFrameworkQueryableExtensions.FirstOrDefaultAsync[TSource](IQueryable`1 source, Expression`1 predicate, CancellationToken cancellationToken)
         at WebApplication.DataAccess.Repositories.UserRepository.FindByEmailAsync(String email, CancellationToken cancellationToken) in C:\Users\user\Documents\ANDREI_FILES\PDM_FILES\2ND YEAR\2ND SEM\OOP\OOP-TaurusBikeShop\WebApplication\DataAccess\Repositories\UserRepository.cs:line 33
         at WebApplication.BusinessLogic.Services.UserService.LoginAsync(String email, String password, CancellationToken cancellationToken) in C:\Users\user\Documents\ANDREI_FILES\PDM_FILES\2ND YEAR\2ND SEM\OOP\OOP-TaurusBikeShop\WebApplication\BusinessLogic\Services\UserService.cs:line 195
         at WebApplication.Controllers.CustomerController.Login(LoginViewModel vm, CancellationToken cancellationToken) in C:\Users\user\Documents\ANDREI_FILES\PDM_FILES\2ND YEAR\2ND SEM\OOP\OOP-TaurusBikeShop\WebApplication\Controllers\CustomerController.cs:line 228
info: Microsoft.AspNetCore.Mvc.ViewFeatures.ViewResultExecutor[1]
      Executing ViewResult, running view Login.
info: Microsoft.AspNetCore.Mvc.ViewFeatures.ViewResultExecutor[4]
      Executed ViewResult - view Login executed in 7.5739ms.
info: Microsoft.AspNetCore.Mvc.Infrastructure.ControllerActionInvoker[105]
      Executed action WebApplication.Controllers.CustomerController.Login (WebApplication) in 85.7049ms
info: Microsoft.AspNetCore.Routing.EndpointMiddleware[1]
      Executed endpoint 'WebApplication.Controllers.CustomerController.Login (WebApplication)'
info: Microsoft.AspNetCore.Hosting.Diagnostics[2]
      Request finished HTTP/2 POST https://localhost:7177/Customer/Login - 200 - text/html;+charset=utf-8 101.8639ms
info: Microsoft.AspNetCore.Hosting.Diagnostics[1]
      Request starting HTTP/2 GET https://localhost:7177/_framework/aspnetcore-browser-refresh.js - - -
info: Microsoft.AspNetCore.Hosting.Diagnostics[1]
      Request starting HTTP/2 GET https://localhost:7177/_vs/browserLink - - -
info: Microsoft.AspNetCore.Hosting.Diagnostics[2]
      Request finished HTTP/2 GET https://localhost:7177/_framework/aspnetcore-browser-refresh.js - 200 14933 application/javascript;+charset=utf-8 4.1435ms
info: Microsoft.AspNetCore.Hosting.Diagnostics[2]
      Request finished HTTP/2 GET https://localhost:7177/_vs/browserLink - 200 - text/javascript;+charset=UTF-8 13.8833ms
info: Microsoft.AspNetCore.Hosting.Diagnostics[1]
      Request starting HTTP/2 GET https://localhost:7177/Cart/GetCartCount - - -
info: Microsoft.AspNetCore.Routing.EndpointMiddleware[0]
      Executing endpoint 'WebApplication.Controllers.CartController.GetCartCount (WebApplication)'
info: Microsoft.AspNetCore.Mvc.Infrastructure.ControllerActionInvoker[102]
      Route matched with {action = "GetCartCount", controller = "Cart"}. Executing controller action with signature System.Threading.Tasks.Task`1[Microsoft.AspNetCore.Mvc.IActionResult] GetCartCount(System.Threading.CancellationToken) on controller WebApplication.Controllers.CartController (WebApplication).
info: Microsoft.AspNetCore.Mvc.Infrastructure.SystemTextJsonResultExecutor[1]
      Executing JsonResult, writing value of type 'WebApplication.Models.ApiResponse'.
info: Microsoft.AspNetCore.Mvc.Infrastructure.ControllerActionInvoker[105]
      Executed action WebApplication.Controllers.CartController.GetCartCount (WebApplication) in 8.3007ms
info: Microsoft.AspNetCore.Routing.EndpointMiddleware[1]
      Executed endpoint 'WebApplication.Controllers.CartController.GetCartCount (WebApplication)'
info: Microsoft.AspNetCore.Hosting.Diagnostics[2]
      Request finished HTTP/2 GET https://localhost:7177/Cart/GetCartCount - 200 - application/json;+charset=utf-8 17.8712ms
fail: WebApplication.BackgroundJobs.InventorySyncJob[0]
      InventorySyncJob cycle failed.
      System.ArgumentException: Keyword not supported: 'userid'.
         at Microsoft.Data.SqlClient.SqlConnectionStringBuilder.GetIndex(String keyword)
         at Microsoft.Data.SqlClient.SqlConnectionStringBuilder.set_Item(String keyword, Object value)
         at System.Data.Common.DbConnectionStringBuilder.set_ConnectionString(String value)
         at Microsoft.Data.SqlClient.SqlConnectionStringBuilder..ctor(String connectionString)
         at Microsoft.EntityFrameworkCore.SqlServer.Storage.Internal.SqlServerConnection.<>c.<get_IsMultipleActiveResultSetsEnabled>b__7_0(String cs)
         at System.Collections.Concurrent.ConcurrentDictionary`2.GetOrAdd(TKey key, Func`2 valueFactory)
         at Microsoft.EntityFrameworkCore.SqlServer.Storage.Internal.SqlServerConnection.get_IsMultipleActiveResultSetsEnabled()
         at Microsoft.EntityFrameworkCore.SqlServer.Query.Internal.SqlServerCompiledQueryCacheKeyGenerator.GenerateCacheKey(Expression query, Boolean async)
         at Microsoft.EntityFrameworkCore.Query.Internal.QueryCompiler.ExecuteAsync[TResult](Expression query, CancellationToken cancellationToken)
         at Microsoft.EntityFrameworkCore.Query.Internal.EntityQueryProvider.ExecuteAsync[TResult](Expression expression, CancellationToken cancellationToken)
         at Microsoft.EntityFrameworkCore.EntityFrameworkQueryableExtensions.ExecuteAsync[TSource,TResult](MethodInfo operatorMethodInfo, IQueryable`1 source, Expression expression, CancellationToken cancellationToken)
         at Microsoft.EntityFrameworkCore.EntityFrameworkQueryableExtensions.ExecuteAsync[TSource,TResult](MethodInfo operatorMethodInfo, IQueryable`1 source, CancellationToken cancellationToken)
         at Microsoft.EntityFrameworkCore.EntityFrameworkQueryableExtensions.CountAsync[TSource](IQueryable`1 source, CancellationToken cancellationToken)
         at WebApplication.BackgroundJobs.InventorySyncJob.RunCycleAsync(CancellationToken cancellationToken) in C:\Users\user\Documents\ANDREI_FILES\PDM_FILES\2ND YEAR\2ND SEM\OOP\OOP-TaurusBikeShop\WebApplication\BackgroundJobs\InventorySyncJob.cs:line 68
fail: Microsoft.EntityFrameworkCore.Update[10000]
      An exception occurred in the database while saving changes for context type 'WebApplication.DataAccess.Context.AppDbContext'.
      System.ArgumentException: Keyword not supported: 'userid'.
         at Microsoft.Data.Common.DbConnectionOptions.ParseInternal(Dictionary`2 parsetable, String connectionString, Boolean buildChain, Dictionary`2 synonyms, Boolean firstKey)
         at Microsoft.Data.Common.DbConnectionOptions..ctor(String connectionString, Dictionary`2 synonyms)
         at Microsoft.Data.SqlClient.SqlConnectionString..ctor(String connectionString)
         at Microsoft.Data.SqlClient.SqlConnectionFactory.CreateConnectionOptions(String connectionString, DbConnectionOptions previous)
         at Microsoft.Data.ProviderBase.DbConnectionFactory.GetConnectionPoolGroup(DbConnectionPoolKey key, DbConnectionPoolGroupOptions poolOptions, DbConnectionOptions& userConnectionOptions)
         at Microsoft.Data.SqlClient.SqlConnection.ConnectionString_Set(DbConnectionPoolKey key)
         at Microsoft.Data.SqlClient.SqlConnection.set_ConnectionString(String value)
         at Microsoft.Data.SqlClient.SqlConnection..ctor(String connectionString)
         at Microsoft.EntityFrameworkCore.SqlServer.Storage.Internal.SqlServerConnection.CreateDbConnection()
         at Microsoft.EntityFrameworkCore.Storage.RelationalConnection.get_DbConnection()
         at Microsoft.EntityFrameworkCore.Storage.RelationalConnection.OpenAsync(CancellationToken cancellationToken, Boolean errorsExpected)
         at Microsoft.EntityFrameworkCore.Update.Internal.BatchExecutor.ExecuteAsync(IEnumerable`1 commandBatches, IRelationalConnection connection, CancellationToken cancellationToken)
         at Microsoft.EntityFrameworkCore.Update.Internal.BatchExecutor.ExecuteAsync(IEnumerable`1 commandBatches, IRelationalConnection connection, CancellationToken cancellationToken)
         at Microsoft.EntityFrameworkCore.Update.Internal.BatchExecutor.ExecuteAsync(IEnumerable`1 commandBatches, IRelationalConnection connection, CancellationToken cancellationToken)
         at Microsoft.EntityFrameworkCore.ChangeTracking.Internal.StateManager.SaveChangesAsync(IList`1 entriesToSave, CancellationToken cancellationToken)
         at Microsoft.EntityFrameworkCore.ChangeTracking.Internal.StateManager.SaveChangesAsync(StateManager stateManager, Boolean acceptAllChangesOnSuccess, CancellationToken cancellationToken)
         at Microsoft.EntityFrameworkCore.Storage.ExecutionStrategy.<>c__DisplayClass30_0`2.<<ExecuteAsync>b__0>d.MoveNext()
      --- End of stack trace from previous location ---
         at Microsoft.EntityFrameworkCore.Storage.ExecutionStrategy.ExecuteImplementationAsync[TState,TResult](Func`4 operation, Func`4 verifySucceeded, TState state, CancellationToken cancellationToken)
         at Microsoft.EntityFrameworkCore.Storage.ExecutionStrategy.ExecuteImplementationAsync[TState,TResult](Func`4 operation, Func`4 verifySucceeded, TState state, CancellationToken cancellationToken)
         at Microsoft.EntityFrameworkCore.Storage.ExecutionStrategy.ExecuteAsync[TState,TResult](TState state, Func`4 operation, Func`4 verifySucceeded, CancellationToken cancellationToken)
         at Microsoft.EntityFrameworkCore.DbContext.SaveChangesAsync(Boolean acceptAllChangesOnSuccess, CancellationToken cancellationToken)
      System.ArgumentException: Keyword not supported: 'userid'.
         at Microsoft.Data.Common.DbConnectionOptions.ParseInternal(Dictionary`2 parsetable, String connectionString, Boolean buildChain, Dictionary`2 synonyms, Boolean firstKey)
         at Microsoft.Data.Common.DbConnectionOptions..ctor(String connectionString, Dictionary`2 synonyms)
         at Microsoft.Data.SqlClient.SqlConnectionString..ctor(String connectionString)
         at Microsoft.Data.SqlClient.SqlConnectionFactory.CreateConnectionOptions(String connectionString, DbConnectionOptions previous)
         at Microsoft.Data.ProviderBase.DbConnectionFactory.GetConnectionPoolGroup(DbConnectionPoolKey key, DbConnectionPoolGroupOptions poolOptions, DbConnectionOptions& userConnectionOptions)
         at Microsoft.Data.SqlClient.SqlConnection.ConnectionString_Set(DbConnectionPoolKey key)
         at Microsoft.Data.SqlClient.SqlConnection.set_ConnectionString(String value)
         at Microsoft.Data.SqlClient.SqlConnection..ctor(String connectionString)
         at Microsoft.EntityFrameworkCore.SqlServer.Storage.Internal.SqlServerConnection.CreateDbConnection()
         at Microsoft.EntityFrameworkCore.Storage.RelationalConnection.get_DbConnection()
         at Microsoft.EntityFrameworkCore.Storage.RelationalConnection.OpenAsync(CancellationToken cancellationToken, Boolean errorsExpected)
         at Microsoft.EntityFrameworkCore.Update.Internal.BatchExecutor.ExecuteAsync(IEnumerable`1 commandBatches, IRelationalConnection connection, CancellationToken cancellationToken)
         at Microsoft.EntityFrameworkCore.Update.Internal.BatchExecutor.ExecuteAsync(IEnumerable`1 commandBatches, IRelationalConnection connection, CancellationToken cancellationToken)
         at Microsoft.EntityFrameworkCore.Update.Internal.BatchExecutor.ExecuteAsync(IEnumerable`1 commandBatches, IRelationalConnection connection, CancellationToken cancellationToken)
         at Microsoft.EntityFrameworkCore.ChangeTracking.Internal.StateManager.SaveChangesAsync(IList`1 entriesToSave, CancellationToken cancellationToken)
         at Microsoft.EntityFrameworkCore.ChangeTracking.Internal.StateManager.SaveChangesAsync(StateManager stateManager, Boolean acceptAllChangesOnSuccess, CancellationToken cancellationToken)
         at Microsoft.EntityFrameworkCore.Storage.ExecutionStrategy.<>c__DisplayClass30_0`2.<<ExecuteAsync>b__0>d.MoveNext()
      --- End of stack trace from previous location ---
         at Microsoft.EntityFrameworkCore.Storage.ExecutionStrategy.ExecuteImplementationAsync[TState,TResult](Func`4 operation, Func`4 verifySucceeded, TState state, CancellationToken cancellationToken)
         at Microsoft.EntityFrameworkCore.Storage.ExecutionStrategy.ExecuteImplementationAsync[TState,TResult](Func`4 operation, Func`4 verifySucceeded, TState state, CancellationToken cancellationToken)
         at Microsoft.EntityFrameworkCore.Storage.ExecutionStrategy.ExecuteAsync[TState,TResult](TState state, Func`4 operation, Func`4 verifySucceeded, CancellationToken cancellationToken)
         at Microsoft.EntityFrameworkCore.DbContext.SaveChangesAsync(Boolean acceptAllChangesOnSuccess, CancellationToken cancellationToken)
fail: WebApplication.BackgroundJobs.InventorySyncJob[0]
      Failed to write error log for InventorySyncJob.
      System.ArgumentException: Keyword not supported: 'userid'.
         at Microsoft.Data.Common.DbConnectionOptions.ParseInternal(Dictionary`2 parsetable, String connectionString, Boolean buildChain, Dictionary`2 synonyms, Boolean firstKey)
         at Microsoft.Data.Common.DbConnectionOptions..ctor(String connectionString, Dictionary`2 synonyms)
         at Microsoft.Data.SqlClient.SqlConnectionString..ctor(String connectionString)
         at Microsoft.Data.SqlClient.SqlConnectionFactory.CreateConnectionOptions(String connectionString, DbConnectionOptions previous)
         at Microsoft.Data.ProviderBase.DbConnectionFactory.GetConnectionPoolGroup(DbConnectionPoolKey key, DbConnectionPoolGroupOptions poolOptions, DbConnectionOptions& userConnectionOptions)
         at Microsoft.Data.SqlClient.SqlConnection.ConnectionString_Set(DbConnectionPoolKey key)
         at Microsoft.Data.SqlClient.SqlConnection.set_ConnectionString(String value)
         at Microsoft.Data.SqlClient.SqlConnection..ctor(String connectionString)
         at Microsoft.EntityFrameworkCore.SqlServer.Storage.Internal.SqlServerConnection.CreateDbConnection()
         at Microsoft.EntityFrameworkCore.Storage.RelationalConnection.get_DbConnection()
         at Microsoft.EntityFrameworkCore.Storage.RelationalConnection.OpenAsync(CancellationToken cancellationToken, Boolean errorsExpected)
         at Microsoft.EntityFrameworkCore.Update.Internal.BatchExecutor.ExecuteAsync(IEnumerable`1 commandBatches, IRelationalConnection connection, CancellationToken cancellationToken)
         at Microsoft.EntityFrameworkCore.Update.Internal.BatchExecutor.ExecuteAsync(IEnumerable`1 commandBatches, IRelationalConnection connection, CancellationToken cancellationToken)
         at Microsoft.EntityFrameworkCore.Update.Internal.BatchExecutor.ExecuteAsync(IEnumerable`1 commandBatches, IRelationalConnection connection, CancellationToken cancellationToken)
         at Microsoft.EntityFrameworkCore.ChangeTracking.Internal.StateManager.SaveChangesAsync(IList`1 entriesToSave, CancellationToken cancellationToken)
         at Microsoft.EntityFrameworkCore.ChangeTracking.Internal.StateManager.SaveChangesAsync(StateManager stateManager, Boolean acceptAllChangesOnSuccess, CancellationToken cancellationToken)
         at Microsoft.EntityFrameworkCore.Storage.ExecutionStrategy.<>c__DisplayClass30_0`2.<<ExecuteAsync>b__0>d.MoveNext()
      --- End of stack trace from previous location ---
         at Microsoft.EntityFrameworkCore.Storage.ExecutionStrategy.ExecuteImplementationAsync[TState,TResult](Func`4 operation, Func`4 verifySucceeded, TState state, CancellationToken cancellationToken)
         at Microsoft.EntityFrameworkCore.Storage.ExecutionStrategy.ExecuteImplementationAsync[TState,TResult](Func`4 operation, Func`4 verifySucceeded, TState state, CancellationToken cancellationToken)
         at Microsoft.EntityFrameworkCore.Storage.ExecutionStrategy.ExecuteAsync[TState,TResult](TState state, Func`4 operation, Func`4 verifySucceeded, CancellationToken cancellationToken)
         at Microsoft.EntityFrameworkCore.DbContext.SaveChangesAsync(Boolean acceptAllChangesOnSuccess, CancellationToken cancellationToken)
         at Microsoft.EntityFrameworkCore.DbContext.SaveChangesAsync(Boolean acceptAllChangesOnSuccess, CancellationToken cancellationToken)
         at WebApplication.BackgroundJobs.InventorySyncJob.RunCycleAsync(CancellationToken cancellationToken) in C:\Users\user\Documents\ANDREI_FILES\PDM_FILES\2ND YEAR\2ND SEM\OOP\OOP-TaurusBikeShop\WebApplication\BackgroundJobs\InventorySyncJob.cs:line 112
         at WebApplication.BackgroundJobs.InventorySyncJob.RunCycleAsync(CancellationToken cancellationToken) in C:\Users\user\Documents\ANDREI_FILES\PDM_FILES\2ND YEAR\2ND SEM\OOP\OOP-TaurusBikeShop\WebApplication\BackgroundJobs\InventorySyncJob.cs:line 112
