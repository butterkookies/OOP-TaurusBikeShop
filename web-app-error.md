An unhandled exception occurred while processing the request.
InvalidOperationException: The view 'Index' was not found. The following locations were searched:
/Views/Home/Index.cshtml
/Views/Shared/Index.cshtml
Microsoft.AspNetCore.Mvc.ViewEngines.ViewEngineResult.EnsureSuccessful(IEnumerable<string> originalLocations)

Stack Query Cookies Headers Routing
InvalidOperationException: The view 'Index' was not found. The following locations were searched: /Views/Home/Index.cshtml /Views/Shared/Index.cshtml
Microsoft.AspNetCore.Mvc.ViewEngines.ViewEngineResult.EnsureSuccessful(IEnumerable<string> originalLocations)
Microsoft.AspNetCore.Mvc.ViewFeatures.ViewResultExecutor.ExecuteAsync(ActionContext context, ViewResult result)
Microsoft.AspNetCore.Mvc.ViewResult.ExecuteResultAsync(ActionContext context)
Microsoft.AspNetCore.Mvc.Infrastructure.ResourceInvoker.<InvokeNextResultFilterAsync>g__Awaited|30_0<TFilter, TFilterAsync>(ResourceInvoker invoker, Task lastTask, State next, Scope scope, object state, bool isCompleted)
Microsoft.AspNetCore.Mvc.Infrastructure.ResourceInvoker.Rethrow(ResultExecutedContextSealed context)
Microsoft.AspNetCore.Mvc.Infrastructure.ResourceInvoker.ResultNext<TFilter, TFilterAsync>(ref State next, ref Scope scope, ref object state, ref bool isCompleted)
Microsoft.AspNetCore.Mvc.Infrastructure.ResourceInvoker.InvokeResultFilters()
Microsoft.AspNetCore.Mvc.Infrastructure.ResourceInvoker.<InvokeNextResourceFilter>g__Awaited|25_0(ResourceInvoker invoker, Task lastTask, State next, Scope scope, object state, bool isCompleted)
Microsoft.AspNetCore.Mvc.Infrastructure.ResourceInvoker.Rethrow(ResourceExecutedContextSealed context)
Microsoft.AspNetCore.Mvc.Infrastructure.ResourceInvoker.Next(ref State next, ref Scope scope, ref object state, ref bool isCompleted)
Microsoft.AspNetCore.Mvc.Infrastructure.ResourceInvoker.<InvokeFilterPipelineAsync>g__Awaited|20_0(ResourceInvoker invoker, Task lastTask, State next, Scope scope, object state, bool isCompleted)
Microsoft.AspNetCore.Mvc.Infrastructure.ResourceInvoker.<InvokeAsync>g__Logged|17_1(ResourceInvoker invoker)
Microsoft.AspNetCore.Mvc.Infrastructure.ResourceInvoker.<InvokeAsync>g__Logged|17_1(ResourceInvoker invoker)
Microsoft.AspNetCore.Routing.EndpointMiddleware.<Invoke>g__AwaitRequestTask|7_0(Endpoint endpoint, Task requestTask, ILogger logger)
Microsoft.AspNetCore.Session.SessionMiddleware.Invoke(HttpContext context)
Microsoft.AspNetCore.Session.SessionMiddleware.Invoke(HttpContext context)
Microsoft.AspNetCore.Authorization.AuthorizationMiddleware.Invoke(HttpContext context)
Microsoft.AspNetCore.Authentication.AuthenticationMiddleware.Invoke(HttpContext context)
Microsoft.AspNetCore.Diagnostics.DeveloperExceptionPageMiddlewareImpl.Invoke(HttpContext context)

Show raw exception details
System.InvalidOperationException: The view 'Index' was not found. The following locations were searched:
/Views/Home/Index.cshtml
/Views/Shared/Index.cshtml
   at Microsoft.AspNetCore.Mvc.ViewEngines.ViewEngineResult.EnsureSuccessful(IEnumerable`1 originalLocations)
   at Microsoft.AspNetCore.Mvc.ViewFeatures.ViewResultExecutor.ExecuteAsync(ActionContext context, ViewResult result)
   at Microsoft.AspNetCore.Mvc.ViewResult.ExecuteResultAsync(ActionContext context)
   at Microsoft.AspNetCore.Mvc.Infrastructure.ResourceInvoker.<InvokeNextResultFilterAsync>g__Awaited|30_0[TFilter,TFilterAsync](ResourceInvoker invoker, Task lastTask, State next, Scope scope, Object state, Boolean isCompleted)
   at Microsoft.AspNetCore.Mvc.Infrastructure.ResourceInvoker.Rethrow(ResultExecutedContextSealed context)
   at Microsoft.AspNetCore.Mvc.Infrastructure.ResourceInvoker.ResultNext[TFilter,TFilterAsync](State& next, Scope& scope, Object& state, Boolean& isCompleted)
   at Microsoft.AspNetCore.Mvc.Infrastructure.ResourceInvoker.InvokeResultFilters()
--- End of stack trace from previous location ---
   at Microsoft.AspNetCore.Mvc.Infrastructure.ResourceInvoker.<InvokeNextResourceFilter>g__Awaited|25_0(ResourceInvoker invoker, Task lastTask, State next, Scope scope, Object state, Boolean isCompleted)
   at Microsoft.AspNetCore.Mvc.Infrastructure.ResourceInvoker.Rethrow(ResourceExecutedContextSealed context)
   at Microsoft.AspNetCore.Mvc.Infrastructure.ResourceInvoker.Next(State& next, Scope& scope, Object& state, Boolean& isCompleted)
   at Microsoft.AspNetCore.Mvc.Infrastructure.ResourceInvoker.<InvokeFilterPipelineAsync>g__Awaited|20_0(ResourceInvoker invoker, Task lastTask, State next, Scope scope, Object state, Boolean isCompleted)
   at Microsoft.AspNetCore.Mvc.Infrastructure.ResourceInvoker.<InvokeAsync>g__Logged|17_1(ResourceInvoker invoker)
   at Microsoft.AspNetCore.Mvc.Infrastructure.ResourceInvoker.<InvokeAsync>g__Logged|17_1(ResourceInvoker invoker)
   at Microsoft.AspNetCore.Routing.EndpointMiddleware.<Invoke>g__AwaitRequestTask|7_0(Endpoint endpoint, Task requestTask, ILogger logger)
   at Microsoft.AspNetCore.Session.SessionMiddleware.Invoke(HttpContext context)
   at Microsoft.AspNetCore.Session.SessionMiddleware.Invoke(HttpContext context)
   at Microsoft.AspNetCore.Authorization.AuthorizationMiddleware.Invoke(HttpContext context)
   at Microsoft.AspNetCore.Authentication.AuthenticationMiddleware.Invoke(HttpContext context)
   at Microsoft.AspNetCore.Diagnostics.DeveloperExceptionPageMiddlewareImpl.Invoke(HttpContext context)

   [WARNING] Google Cloud Storage unavailable: Your default credentials were not found. To set up Application Default Credentials, see https://cloud.google.com/docs/authentication/external/set-up-adc.
[WARNING] Support-ticket attachments will be disabled. Set GOOGLE_APPLICATION_CREDENTIALS to enable.
info: Microsoft.AspNetCore.DataProtection.KeyManagement.XmlKeyManager[63]
      User profile is available. Using 'C:\Users\user\AppData\Local\ASP.NET\DataProtection-Keys' as key repository and Windows DPAPI to encrypt keys at rest.
info: WebApplication.BackgroundJobs.InventorySyncJob[0]
      InventorySyncJob started.
info: WebApplication.BackgroundJobs.PendingOrderMonitorJob[0]
      PendingOrderMonitorJob started.
info: WebApplication.BackgroundJobs.PaymentTimeoutJob[0]
      PaymentTimeoutJob started.
info: WebApplication.BackgroundJobs.StockMonitorJob[0]
      StockMonitorJob started.
info: WebApplication.BackgroundJobs.DeliveryStatusPollJob[0]
      DeliveryStatusPollJob started.
info: WebApplication.BackgroundJobs.NotificationDispatchJob[0]
      NotificationDispatchJob started.
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
warn: Microsoft.EntityFrameworkCore.Model.Validation[10622]
      Entity 'User' has a global query filter defined and is the required end of a relationship with the entity 'Address'. This may lead to unexpected results when the required entity is filtered out. Either configure the navigation as optional, or define matching query filters for both entities in the navigation. See https://go.microsoft.com/fwlink/?linkid=2131316 for more information.
warn: Microsoft.EntityFrameworkCore.Model.Validation[10622]
      Entity 'Order' has a global query filter defined and is the required end of a relationship with the entity 'Delivery'. This may lead to unexpected results when the required entity is filtered out. Either configure the navigation as optional, or define matching query filters for both entities in the navigation. See https://go.microsoft.com/fwlink/?linkid=2131316 for more information.
warn: Microsoft.EntityFrameworkCore.Model.Validation[10622]
      Entity 'Order' has a global query filter defined and is the required end of a relationship with the entity 'OrderItem'. This may lead to unexpected results when the required entity is filtered out. Either configure the navigation as optional, or define matching query filters for both entities in the navigation. See https://go.microsoft.com/fwlink/?linkid=2131316 for more information.
warn: Microsoft.EntityFrameworkCore.Model.Validation[10622]
      Entity 'User' has a global query filter defined and is the required end of a relationship with the entity 'POS_Session'. This may lead to unexpected results when the required entity is filtered out. Either configure the navigation as optional, or define matching query filters for both entities in the navigation. See https://go.microsoft.com/fwlink/?linkid=2131316 for more information.
warn: Microsoft.EntityFrameworkCore.Model.Validation[10622]
      Entity 'Order' has a global query filter defined and is the required end of a relationship with the entity 'Payment'. This may lead to unexpected results when the required entity is filtered out. Either configure the navigation as optional, or define matching query filters for both entities in the navigation. See https://go.microsoft.com/fwlink/?linkid=2131316 for more information.
warn: Microsoft.EntityFrameworkCore.Model.Validation[10622]
      Entity 'Order' has a global query filter defined and is the required end of a relationship with the entity 'PickupOrder'. This may lead to unexpected results when the required entity is filtered out. Either configure the navigation as optional, or define matching query filters for both entities in the navigation. See https://go.microsoft.com/fwlink/?linkid=2131316 for more information.
warn: Microsoft.EntityFrameworkCore.Model.Validation[10622]
      Entity 'Order' has a global query filter defined and is the required end of a relationship with the entity 'Review'. This may lead to unexpected results when the required entity is filtered out. Either configure the navigation as optional, or define matching query filters for both entities in the navigation. See https://go.microsoft.com/fwlink/?linkid=2131316 for more information.
warn: Microsoft.EntityFrameworkCore.Model.Validation[10622]
      Entity 'User' has a global query filter defined and is the required end of a relationship with the entity 'SupportTicket'. This may lead to unexpected results when the required entity is filtered out. Either configure the navigation as optional, or define matching query filters for both entities in the navigation. See https://go.microsoft.com/fwlink/?linkid=2131316 for more information.
warn: Microsoft.EntityFrameworkCore.Model.Validation[10622]
      Entity 'User' has a global query filter defined and is the required end of a relationship with the entity 'SupportTicketReply'. This may lead to unexpected results when the required entity is filtered out. Either configure the navigation as optional, or define matching query filters for both entities in the navigation. See https://go.microsoft.com/fwlink/?linkid=2131316 for more information.
warn: Microsoft.EntityFrameworkCore.Model.Validation[10622]
      Entity 'User' has a global query filter defined and is the required end of a relationship with the entity 'UserRole'. This may lead to unexpected results when the required entity is filtered out. Either configure the navigation as optional, or define matching query filters for both entities in the navigation. See https://go.microsoft.com/fwlink/?linkid=2131316 for more information.
warn: Microsoft.EntityFrameworkCore.Model.Validation[10622]
      Entity 'User' has a global query filter defined and is the required end of a relationship with the entity 'UserVoucher'. This may lead to unexpected results when the required entity is filtered out. Either configure the navigation as optional, or define matching query filters for both entities in the navigation. See https://go.microsoft.com/fwlink/?linkid=2131316 for more information.
warn: Microsoft.EntityFrameworkCore.Model.Validation[10622]
      Entity 'Order' has a global query filter defined and is the required end of a relationship with the entity 'VoucherUsage'. This may lead to unexpected results when the required entity is filtered out. Either configure the navigation as optional, or define matching query filters for both entities in the navigation. See https://go.microsoft.com/fwlink/?linkid=2131316 for more information.
warn: Microsoft.EntityFrameworkCore.Model.Validation[10622]
      Entity 'User' has a global query filter defined and is the required end of a relationship with the entity 'Wishlist'. This may lead to unexpected results when the required entity is filtered out. Either configure the navigation as optional, or define matching query filters for both entities in the navigation. See https://go.microsoft.com/fwlink/?linkid=2131316 for more information.
info: Microsoft.EntityFrameworkCore.Database.Command[20101]
      Executed DbCommand (92ms) [Parameters=[@__p_0='?' (DbType = Int32)], CommandType='Text', CommandTimeout='30']
      SELECT [t].[ProductId], [t].[AdditionalSpecs], [t].[AxleStandard], [t].[BoostCompatible], [t].[BrakeType], [t].[BrandId], [t].[CategoryId], [t].[Color], [t].[CreatedAt], [t].[Currency], [t].[Description], [t].[IsActive], [t].[IsFeatured], [t].[Material], [t].[Name], [t].[Price], [t].[SKU], [t].[ShortDescription], [t].[SpeedCompatibility], [t].[SuspensionTravel], [t].[TubelessReady], [t].[UpdatedAt], [t].[WheelSize], [b].[BrandId], [b].[BrandName], [b].[Country], [b].[CreatedAt], [b].[Description], [b].[IsActive], [b].[Website], [t].[CategoryId0], [t].[CategoryCode], [t].[Description0], [t].[DisplayOrder], [t].[IsActive0], [t].[Name0], [t].[ParentCategoryId]
      FROM (
          SELECT TOP(@__p_0) [p].[ProductId], [p].[AdditionalSpecs], [p].[AxleStandard], [p].[BoostCompatible], [p].[BrakeType], [p].[BrandId], [p].[CategoryId], [p].[Color], [p].[CreatedAt], [p].[Currency], [p].[Description], [p].[IsActive], [p].[IsFeatured], [p].[Material], [p].[Name], [p].[Price], [p].[SKU], [p].[ShortDescription], [p].[SpeedCompatibility], [p].[SuspensionTravel], [p].[TubelessReady], [p].[UpdatedAt], [p].[WheelSize], [c].[CategoryId] AS [CategoryId0], [c].[CategoryCode], [c].[Description] AS [Description0], [c].[DisplayOrder], [c].[IsActive] AS [IsActive0], [c].[Name] AS [Name0], [c].[ParentCategoryId]
          FROM [Product] AS [p]
          INNER JOIN [Category] AS [c] ON [p].[CategoryId] = [c].[CategoryId]
          WHERE [p].[IsActive] = CAST(1 AS bit) AND [p].[IsFeatured] = CAST(1 AS bit) AND [c].[IsActive] = CAST(1 AS bit)
          ORDER BY [p].[Name]
      ) AS [t]
      LEFT JOIN [Brand] AS [b] ON [t].[BrandId] = [b].[BrandId]
      ORDER BY [t].[Name], [t].[ProductId], [t].[CategoryId0], [b].[BrandId]
info: Microsoft.EntityFrameworkCore.Database.Command[20101]
      Executed DbCommand (59ms) [Parameters=[@__p_0='?' (DbType = Int32)], CommandType='Text', CommandTimeout='30']
      SELECT [t0].[ProductImageId], [t0].[AltText], [t0].[CreatedAt], [t0].[DisplayOrder], [t0].[FileSizeBytes], [t0].[Height], [t0].[ImageType], [t0].[ImageUrl], [t0].[IsPrimary], [t0].[MimeType], [t0].[ProductId], [t0].[StorageBucket], [t0].[StoragePath], [t0].[UploadedByUserId], [t0].[Width], [t].[ProductId], [t].[CategoryId0], [b].[BrandId]
      FROM (
          SELECT TOP(@__p_0) [p].[ProductId], [p].[BrandId], [p].[Name], [c].[CategoryId] AS [CategoryId0]
          FROM [Product] AS [p]
          INNER JOIN [Category] AS [c] ON [p].[CategoryId] = [c].[CategoryId]
          WHERE [p].[IsActive] = CAST(1 AS bit) AND [p].[IsFeatured] = CAST(1 AS bit) AND [c].[IsActive] = CAST(1 AS bit)
          ORDER BY [p].[Name]
      ) AS [t]
      LEFT JOIN [Brand] AS [b] ON [t].[BrandId] = [b].[BrandId]
      INNER JOIN (
          SELECT [p0].[ProductImageId], [p0].[AltText], [p0].[CreatedAt], [p0].[DisplayOrder], [p0].[FileSizeBytes], [p0].[Height], [p0].[ImageType], [p0].[ImageUrl], [p0].[IsPrimary], [p0].[MimeType], [p0].[ProductId], [p0].[StorageBucket], [p0].[StoragePath], [p0].[UploadedByUserId], [p0].[Width]
          FROM [ProductImage] AS [p0]
          WHERE [p0].[IsPrimary] = CAST(1 AS bit)
      ) AS [t0] ON [t].[ProductId] = [t0].[ProductId]
      ORDER BY [t].[Name], [t].[ProductId], [t].[CategoryId0], [b].[BrandId]
info: Microsoft.EntityFrameworkCore.Database.Command[20101]
      Executed DbCommand (60ms) [Parameters=[@__p_0='?' (DbType = Int32)], CommandType='Text', CommandTimeout='30']
      SELECT [t0].[ProductVariantId], [t0].[AdditionalPrice], [t0].[CreatedAt], [t0].[IsActive], [t0].[ProductId], [t0].[ReorderThreshold], [t0].[SKU], [t0].[StockQuantity], [t0].[UpdatedAt], [t0].[VariantName], [t].[ProductId], [t].[CategoryId0], [b].[BrandId]
      FROM (
          SELECT TOP(@__p_0) [p].[ProductId], [p].[BrandId], [p].[Name], [c].[CategoryId] AS [CategoryId0]
          FROM [Product] AS [p]
          INNER JOIN [Category] AS [c] ON [p].[CategoryId] = [c].[CategoryId]
          WHERE [p].[IsActive] = CAST(1 AS bit) AND [p].[IsFeatured] = CAST(1 AS bit) AND [c].[IsActive] = CAST(1 AS bit)
          ORDER BY [p].[Name]
      ) AS [t]
      LEFT JOIN [Brand] AS [b] ON [t].[BrandId] = [b].[BrandId]
      INNER JOIN (
          SELECT [p0].[ProductVariantId], [p0].[AdditionalPrice], [p0].[CreatedAt], [p0].[IsActive], [p0].[ProductId], [p0].[ReorderThreshold], [p0].[SKU], [p0].[StockQuantity], [p0].[UpdatedAt], [p0].[VariantName]
          FROM [ProductVariant] AS [p0]
          WHERE [p0].[IsActive] = CAST(1 AS bit)
      ) AS [t0] ON [t].[ProductId] = [t0].[ProductId]
      ORDER BY [t].[Name], [t].[ProductId], [t].[CategoryId0], [b].[BrandId]
info: Microsoft.AspNetCore.Mvc.ViewFeatures.ViewResultExecutor[1]
      Executing ViewResult, running view Index.
fail: Microsoft.AspNetCore.Mvc.ViewFeatures.ViewResultExecutor[3]
      The view 'Index' was not found. Searched locations: /Views/Home/Index.cshtml, /Views/Shared/Index.cshtml
info: Microsoft.AspNetCore.Mvc.Infrastructure.ControllerActionInvoker[105]
      Executed action WebApplication.Controllers.HomeController.Index (WebApplication) in 1944.0799ms
info: Microsoft.AspNetCore.Routing.EndpointMiddleware[1]
      Executed endpoint 'WebApplication.Controllers.HomeController.Index (WebApplication)'
fail: Microsoft.AspNetCore.Diagnostics.DeveloperExceptionPageMiddleware[1]
      An unhandled exception has occurred while executing the request.
      System.InvalidOperationException: The view 'Index' was not found. The following locations were searched:
      /Views/Home/Index.cshtml
      /Views/Shared/Index.cshtml
         at Microsoft.AspNetCore.Mvc.ViewEngines.ViewEngineResult.EnsureSuccessful(IEnumerable`1 originalLocations)
         at Microsoft.AspNetCore.Mvc.ViewFeatures.ViewResultExecutor.ExecuteAsync(ActionContext context, ViewResult result)
         at Microsoft.AspNetCore.Mvc.ViewResult.ExecuteResultAsync(ActionContext context)
         at Microsoft.AspNetCore.Mvc.Infrastructure.ResourceInvoker.<InvokeNextResultFilterAsync>g__Awaited|30_0[TFilter,TFilterAsync](ResourceInvoker invoker, Task lastTask, State next, Scope scope, Object state, Boolean isCompleted)
         at Microsoft.AspNetCore.Mvc.Infrastructure.ResourceInvoker.Rethrow(ResultExecutedContextSealed context)
         at Microsoft.AspNetCore.Mvc.Infrastructure.ResourceInvoker.ResultNext[TFilter,TFilterAsync](State& next, Scope& scope, Object& state, Boolean& isCompleted)
         at Microsoft.AspNetCore.Mvc.Infrastructure.ResourceInvoker.InvokeResultFilters()
      --- End of stack trace from previous location ---
         at Microsoft.AspNetCore.Mvc.Infrastructure.ResourceInvoker.<InvokeNextResourceFilter>g__Awaited|25_0(ResourceInvoker invoker, Task lastTask, State next, Scope scope, Object state, Boolean isCompleted)
         at Microsoft.AspNetCore.Mvc.Infrastructure.ResourceInvoker.Rethrow(ResourceExecutedContextSealed context)
         at Microsoft.AspNetCore.Mvc.Infrastructure.ResourceInvoker.Next(State& next, Scope& scope, Object& state, Boolean& isCompleted)
         at Microsoft.AspNetCore.Mvc.Infrastructure.ResourceInvoker.<InvokeFilterPipelineAsync>g__Awaited|20_0(ResourceInvoker invoker, Task lastTask, State next, Scope scope, Object state, Boolean isCompleted)
         at Microsoft.AspNetCore.Mvc.Infrastructure.ResourceInvoker.<InvokeAsync>g__Logged|17_1(ResourceInvoker invoker)
         at Microsoft.AspNetCore.Mvc.Infrastructure.ResourceInvoker.<InvokeAsync>g__Logged|17_1(ResourceInvoker invoker)
         at Microsoft.AspNetCore.Routing.EndpointMiddleware.<Invoke>g__AwaitRequestTask|7_0(Endpoint endpoint, Task requestTask, ILogger logger)
         at Microsoft.AspNetCore.Session.SessionMiddleware.Invoke(HttpContext context)
         at Microsoft.AspNetCore.Session.SessionMiddleware.Invoke(HttpContext context)
         at Microsoft.AspNetCore.Authorization.AuthorizationMiddleware.Invoke(HttpContext context)
         at Microsoft.AspNetCore.Authentication.AuthenticationMiddleware.Invoke(HttpContext context)
         at Microsoft.AspNetCore.Diagnostics.DeveloperExceptionPageMiddlewareImpl.Invoke(HttpContext context)
info: Microsoft.AspNetCore.Hosting.Diagnostics[1]
      Request starting HTTP/2 GET https://localhost:7177/_framework/aspnetcore-browser-refresh.js - - -
info: Microsoft.AspNetCore.Hosting.Diagnostics[1]
      Request starting HTTP/2 GET https://localhost:7177/_vs/browserLink - - -
info: Microsoft.EntityFrameworkCore.Database.Command[20101]
      Executed DbCommand (59ms) [Parameters=[], CommandType='Text', CommandTimeout='30']
      SELECT COUNT(*)
      FROM [ProductVariant] AS [p]
info: Microsoft.AspNetCore.Hosting.Diagnostics[2]
      Request finished HTTP/2 GET https://localhost:7177/ - 500 - text/html;+charset=utf-8 2115.2702ms
info: Microsoft.AspNetCore.Hosting.Diagnostics[2]
      Request finished HTTP/2 GET https://localhost:7177/_framework/aspnetcore-browser-refresh.js - 200 14933 application/javascript;+charset=utf-8 19.4771ms
info: Microsoft.AspNetCore.Hosting.Diagnostics[2]
      Request finished HTTP/2 GET https://localhost:7177/_vs/browserLink - 200 - text/javascript;+charset=UTF-8 53.9218ms
info: Microsoft.EntityFrameworkCore.Database.Command[20101]
      Executed DbCommand (60ms) [Parameters=[@p0='?' (DbType = DateTime2), @p1='?' (Size = 1000), @p2='?' (Size = 100), @p3='?' (DbType = Int32), @p4='?' (DbType = DateTime2), @p5='?' (Size = 1000), @p6='?' (Size = 100), @p7='?' (DbType = Int32)], CommandType='Text', CommandTimeout='30']
      SET IMPLICIT_TRANSACTIONS OFF;
      SET NOCOUNT ON;
      MERGE [SystemLog] USING (
      VALUES (@p0, @p1, @p2, @p3, 0),
      (@p4, @p5, @p6, @p7, 1)) AS i ([CreatedAt], [EventDescription], [EventType], [UserId], _Position) ON 1=0
      WHEN NOT MATCHED THEN
      INSERT ([CreatedAt], [EventDescription], [EventType], [UserId])
      VALUES (i.[CreatedAt], i.[EventDescription], i.[EventType], i.[UserId])
      OUTPUT INSERTED.[SystemLogId], i._Position;
info: Microsoft.EntityFrameworkCore.Database.Command[20101]
      Executed DbCommand (59ms) [Parameters=[@p0='?' (DbType = DateTime2), @p1='?' (Size = 1000), @p2='?' (Size = 100), @p3='?' (DbType = Int32)], CommandType='Text', CommandTimeout='30']
      SET IMPLICIT_TRANSACTIONS OFF;
      SET NOCOUNT ON;
      INSERT INTO [SystemLog] ([CreatedAt], [EventDescription], [EventType], [UserId])
      OUTPUT INSERTED.[SystemLogId]
      VALUES (@p0, @p1, @p2, @p3);
info: Microsoft.EntityFrameworkCore.Database.Command[20101]
      Executed DbCommand (58ms) [Parameters=[], CommandType='Text', CommandTimeout='30']
      SELECT [p].[ProductVariantId], [p].[VariantName], [p].[StockQuantity], [p].[ReorderThreshold], [p].[ProductId], [p0].[Name] AS [ProductName]
      FROM [ProductVariant] AS [p]
      INNER JOIN [Product] AS [p0] ON [p].[ProductId] = [p0].[ProductId]
      WHERE [p].[IsActive] = CAST(1 AS bit) AND [p0].[IsActive] = CAST(1 AS bit) AND [p].[StockQuantity] < [p].[ReorderThreshold]
info: Microsoft.EntityFrameworkCore.Database.Command[20101]
      Executed DbCommand (57ms) [Parameters=[], CommandType='Text', CommandTimeout='30']
      SELECT [u].[UserId], [u].[Email], [u].[FirstName]
      FROM [User] AS [u]
      WHERE [u].[IsDeleted] = CAST(0 AS bit) AND [u].[IsActive] = CAST(1 AS bit) AND [u].[IsWalkIn] = CAST(0 AS bit) AND [u].[Email] IS NOT NULL AND EXISTS (
          SELECT 1
          FROM [UserRole] AS [u0]
          INNER JOIN [Role] AS [r] ON [u0].[RoleId] = [r].[RoleId]
          WHERE [u].[UserId] = [u0].[UserId] AND [r].[RoleName] IN (N'Admin', N'Manager'))
warn: WebApplication.BackgroundJobs.StockMonitorJob[0]
      StockMonitorJob: low stock - variant 1 (2021 Pinewood Climber CARBON 27.5 / Default): 0 remaining (threshold 5).
info: Microsoft.EntityFrameworkCore.Database.Command[20101]
      Executed DbCommand (58ms) [Parameters=[@__8__locals1_cooldownStart_0='?' (DbType = DateTime2), @__Format_2='?' (Size = 1000)], CommandType='Text', CommandTimeout='30']
      SELECT CASE
          WHEN EXISTS (
              SELECT 1
              FROM [SystemLog] AS [s]
              WHERE [s].[EventType] = N'LowStockTriggered' AND [s].[CreatedAt] > @__8__locals1_cooldownStart_0 AND [s].[EventDescription] LIKE @__Format_2) THEN CAST(1 AS bit)
          ELSE CAST(0 AS bit)
      END
warn: WebApplication.BackgroundJobs.StockMonitorJob[0]
      StockMonitorJob: low stock - variant 2 (Cult Odyssey Hydro Brakes 27.5 / Default): 0 remaining (threshold 5).
info: Microsoft.EntityFrameworkCore.Database.Command[20101]
      Executed DbCommand (57ms) [Parameters=[@__8__locals1_cooldownStart_0='?' (DbType = DateTime2), @__Format_2='?' (Size = 1000)], CommandType='Text', CommandTimeout='30']
      SELECT CASE
          WHEN EXISTS (
              SELECT 1
              FROM [SystemLog] AS [s]
              WHERE [s].[EventType] = N'LowStockTriggered' AND [s].[CreatedAt] > @__8__locals1_cooldownStart_0 AND [s].[EventDescription] LIKE @__Format_2) THEN CAST(1 AS bit)
          ELSE CAST(0 AS bit)
      END
warn: WebApplication.BackgroundJobs.StockMonitorJob[0]
      StockMonitorJob: low stock - variant 3 (Toseek Chester 700c Disc Brake ALLOY (2x9) / Default): 3 remaining (threshold 5).
info: Microsoft.EntityFrameworkCore.Database.Command[20101]
      Executed DbCommand (57ms) [Parameters=[@__8__locals1_cooldownStart_0='?' (DbType = DateTime2), @__Format_2='?' (Size = 1000)], CommandType='Text', CommandTimeout='30']
      SELECT CASE
          WHEN EXISTS (
              SELECT 1
              FROM [SystemLog] AS [s]
              WHERE [s].[EventType] = N'LowStockTriggered' AND [s].[CreatedAt] > @__8__locals1_cooldownStart_0 AND [s].[EventDescription] LIKE @__Format_2) THEN CAST(1 AS bit)
          ELSE CAST(0 AS bit)
      END
warn: WebApplication.BackgroundJobs.StockMonitorJob[0]
      StockMonitorJob: low stock - variant 4 (Ryder X3 27.5 2021 MT200 Hydro Brakes (3x8) / Default): 2 remaining (threshold 5).
info: Microsoft.EntityFrameworkCore.Database.Command[20101]
      Executed DbCommand (57ms) [Parameters=[@__8__locals1_cooldownStart_0='?' (DbType = DateTime2), @__Format_2='?' (Size = 1000)], CommandType='Text', CommandTimeout='30']
      SELECT CASE
          WHEN EXISTS (
              SELECT 1
              FROM [SystemLog] AS [s]
              WHERE [s].[EventType] = N'LowStockTriggered' AND [s].[CreatedAt] > @__8__locals1_cooldownStart_0 AND [s].[EventDescription] LIKE @__Format_2) THEN CAST(1 AS bit)
          ELSE CAST(0 AS bit)
      END
warn: WebApplication.BackgroundJobs.StockMonitorJob[0]
      StockMonitorJob: low stock - variant 5 (Pinewood Trident Flux / Default): 2 remaining (threshold 5).
info: Microsoft.EntityFrameworkCore.Database.Command[20101]
      Executed DbCommand (57ms) [Parameters=[@__8__locals1_cooldownStart_0='?' (DbType = DateTime2), @__Format_2='?' (Size = 1000)], CommandType='Text', CommandTimeout='30']
      SELECT CASE
          WHEN EXISTS (
              SELECT 1
              FROM [SystemLog] AS [s]
              WHERE [s].[EventType] = N'LowStockTriggered' AND [s].[CreatedAt] > @__8__locals1_cooldownStart_0 AND [s].[EventDescription] LIKE @__Format_2) THEN CAST(1 AS bit)
          ELSE CAST(0 AS bit)
      END
warn: WebApplication.BackgroundJobs.StockMonitorJob[0]
      StockMonitorJob: low stock - variant 6 (Garuda Rampage / Default): 0 remaining (threshold 5).
info: Microsoft.EntityFrameworkCore.Database.Command[20101]
      Executed DbCommand (57ms) [Parameters=[@__8__locals1_cooldownStart_0='?' (DbType = DateTime2), @__Format_2='?' (Size = 1000)], CommandType='Text', CommandTimeout='30']
      SELECT CASE
          WHEN EXISTS (
              SELECT 1
              FROM [SystemLog] AS [s]
              WHERE [s].[EventType] = N'LowStockTriggered' AND [s].[CreatedAt] > @__8__locals1_cooldownStart_0 AND [s].[EventDescription] LIKE @__Format_2) THEN CAST(1 AS bit)
          ELSE CAST(0 AS bit)
      END
warn: WebApplication.BackgroundJobs.StockMonitorJob[0]
      StockMonitorJob: low stock - variant 7 (Pinewood Challenger / Default): 2 remaining (threshold 5).
info: Microsoft.EntityFrameworkCore.Database.Command[20101]
      Executed DbCommand (57ms) [Parameters=[@__8__locals1_cooldownStart_0='?' (DbType = DateTime2), @__Format_2='?' (Size = 1000)], CommandType='Text', CommandTimeout='30']
      SELECT CASE
          WHEN EXISTS (
              SELECT 1
              FROM [SystemLog] AS [s]
              WHERE [s].[EventType] = N'LowStockTriggered' AND [s].[CreatedAt] > @__8__locals1_cooldownStart_0 AND [s].[EventDescription] LIKE @__Format_2) THEN CAST(1 AS bit)
          ELSE CAST(0 AS bit)
      END
warn: WebApplication.BackgroundJobs.StockMonitorJob[0]
      StockMonitorJob: low stock - variant 8 (Kespor Stork Feather CX 1.0 2022 / Default): 2 remaining (threshold 5).
info: Microsoft.EntityFrameworkCore.Database.Command[20101]
      Executed DbCommand (58ms) [Parameters=[@__8__locals1_cooldownStart_0='?' (DbType = DateTime2), @__Format_2='?' (Size = 1000)], CommandType='Text', CommandTimeout='30']
      SELECT CASE
          WHEN EXISTS (
              SELECT 1
              FROM [SystemLog] AS [s]
              WHERE [s].[EventType] = N'LowStockTriggered' AND [s].[CreatedAt] > @__8__locals1_cooldownStart_0 AND [s].[EventDescription] LIKE @__Format_2) THEN CAST(1 AS bit)
          ELSE CAST(0 AS bit)
      END
warn: WebApplication.BackgroundJobs.StockMonitorJob[0]
      StockMonitorJob: low stock - variant 9 (Pinewood Lancer 1.0 2022 Gravel RX (2x9) / Default): 3 remaining (threshold 5).
info: Microsoft.EntityFrameworkCore.Database.Command[20101]
      Executed DbCommand (59ms) [Parameters=[@__8__locals1_cooldownStart_0='?' (DbType = DateTime2), @__Format_2='?' (Size = 1000)], CommandType='Text', CommandTimeout='30']
      SELECT CASE
          WHEN EXISTS (
              SELECT 1
              FROM [SystemLog] AS [s]
              WHERE [s].[EventType] = N'LowStockTriggered' AND [s].[CreatedAt] > @__8__locals1_cooldownStart_0 AND [s].[EventDescription] LIKE @__Format_2) THEN CAST(1 AS bit)
          ELSE CAST(0 AS bit)
      END
info: Microsoft.EntityFrameworkCore.Database.Command[20101]
      Executed DbCommand (59ms) [Parameters=[@p0='?' (DbType = DateTime2), @p1='?' (Size = 1000), @p2='?' (Size = 100), @p3='?' (DbType = Int32)], CommandType='Text', CommandTimeout='30']
      SET IMPLICIT_TRANSACTIONS OFF;
      SET NOCOUNT ON;
      INSERT INTO [SystemLog] ([CreatedAt], [EventDescription], [EventType], [UserId])
      OUTPUT INSERTED.[SystemLogId]
      VALUES (@p0, @p1, @p2, @p3);
info: Microsoft.EntityFrameworkCore.Database.Command[20101]
      Executed DbCommand (59ms) [Parameters=[@p0='?' (DbType = DateTime2), @p1='?' (Size = 1000), @p2='?' (Size = 100), @p3='?' (DbType = Int32)], CommandType='Text', CommandTimeout='30']
      SET IMPLICIT_TRANSACTIONS OFF;
      SET NOCOUNT ON;
      INSERT INTO [SystemLog] ([CreatedAt], [EventDescription], [EventType], [UserId])
      OUTPUT INSERTED.[SystemLogId]
      VALUES (@p0, @p1, @p2, @p3);
info: Microsoft.EntityFrameworkCore.Database.Command[20101]
      Executed DbCommand (58ms) [Parameters=[@__now_0='?' (DbType = DateTime2)], CommandType='Text', CommandTimeout='30']
      SELECT [o].[OrderId], [o].[CartId], [o].[ContactPhone], [o].[CreatedAt], [o].[DeliveryInstructions], [o].[DiscountAmount], [o].[FulfillmentType], [o].[GuestSessionId], [o].[IsDeleted], [o].[IsWalkIn], [o].[OrderDate], [o].[OrderNumber], [o].[OrderStatus], [o].[POSSessionId], [o].[PaymentMethod], [o].[ShippingAddressId], [o].[ShippingFee], [o].[SubTotal], [o].[TotalAmount], [o].[UpdatedAt], [o].[UserId], [t].[UserId], [t].[CreatedAt], [t].[DefaultAddressId], [t].[Email], [t].[FailedLoginAttempts], [t].[FirstName], [t].[IsActive], [t].[IsDeleted], [t].[IsWalkIn], [t].[LastLoginAt], [t].[LastName], [t].[LockoutUntil], [t].[PasswordHash], [t].[PhoneNumber]
      FROM [Order] AS [o]
      INNER JOIN (
          SELECT [u].[UserId], [u].[CreatedAt], [u].[DefaultAddressId], [u].[Email], [u].[FailedLoginAttempts], [u].[FirstName], [u].[IsActive], [u].[IsDeleted], [u].[IsWalkIn], [u].[LastLoginAt], [u].[LastName], [u].[LockoutUntil], [u].[PasswordHash], [u].[PhoneNumber]
          FROM [User] AS [u]
          WHERE [u].[IsDeleted] = CAST(0 AS bit)
      ) AS [t] ON [o].[UserId] = [t].[UserId]
      WHERE [o].[IsDeleted] = CAST(0 AS bit) AND [o].[OrderStatus] = N'PendingVerification' AND EXISTS (
          SELECT 1
          FROM [Payment] AS [p]
          LEFT JOIN [BankTransferPayment] AS [b] ON [p].[PaymentId] = [b].[PaymentId]
          WHERE [o].[OrderId] = [p].[OrderId] AND [p].[PaymentStatus] = N'VerificationPending' AND [p].[PaymentMethod] = N'BankTransfer' AND [b].[PaymentId] IS NOT NULL AND [b].[VerificationDeadline] IS NOT NULL AND [b].[VerificationDeadline] < @__now_0)
      ORDER BY [o].[OrderId], [t].[UserId]
info: Microsoft.EntityFrameworkCore.Database.Command[20101]
      Executed DbCommand (60ms) [Parameters=[@p0='?' (DbType = DateTime2), @p1='?' (Size = 1000), @p2='?' (Size = 100), @p3='?' (DbType = Int32)], CommandType='Text', CommandTimeout='30']
      SET IMPLICIT_TRANSACTIONS OFF;
      SET NOCOUNT ON;
      INSERT INTO [SystemLog] ([CreatedAt], [EventDescription], [EventType], [UserId])
      OUTPUT INSERTED.[SystemLogId]
      VALUES (@p0, @p1, @p2, @p3);
info: Microsoft.EntityFrameworkCore.Database.Command[20101]
      Executed DbCommand (59ms) [Parameters=[@p0='?' (DbType = DateTime2), @p1='?' (Size = 1000), @p2='?' (Size = 100), @p3='?' (DbType = Int32)], CommandType='Text', CommandTimeout='30']
      SET IMPLICIT_TRANSACTIONS OFF;
      SET NOCOUNT ON;
      INSERT INTO [SystemLog] ([CreatedAt], [EventDescription], [EventType], [UserId])
      OUTPUT INSERTED.[SystemLogId]
      VALUES (@p0, @p1, @p2, @p3);
info: Microsoft.EntityFrameworkCore.Database.Command[20101]
      Executed DbCommand (57ms) [Parameters=[@__cancelThreshold_0='?' (DbType = DateTime2)], CommandType='Text', CommandTimeout='30']
      SELECT [o].[OrderId], [o].[CartId], [o].[ContactPhone], [o].[CreatedAt], [o].[DeliveryInstructions], [o].[DiscountAmount], [o].[FulfillmentType], [o].[GuestSessionId], [o].[IsDeleted], [o].[IsWalkIn], [o].[OrderDate], [o].[OrderNumber], [o].[OrderStatus], [o].[POSSessionId], [o].[PaymentMethod], [o].[ShippingAddressId], [o].[ShippingFee], [o].[SubTotal], [o].[TotalAmount], [o].[UpdatedAt], [o].[UserId], [t].[UserId], [t].[CreatedAt], [t].[DefaultAddressId], [t].[Email], [t].[FailedLoginAttempts], [t].[FirstName], [t].[IsActive], [t].[IsDeleted], [t].[IsWalkIn], [t].[LastLoginAt], [t].[LastName], [t].[LockoutUntil], [t].[PasswordHash], [t].[PhoneNumber]
      FROM [Order] AS [o]
      INNER JOIN (
          SELECT [u].[UserId], [u].[CreatedAt], [u].[DefaultAddressId], [u].[Email], [u].[FailedLoginAttempts], [u].[FirstName], [u].[IsActive], [u].[IsDeleted], [u].[IsWalkIn], [u].[LastLoginAt], [u].[LastName], [u].[LockoutUntil], [u].[PasswordHash], [u].[PhoneNumber]
          FROM [User] AS [u]
          WHERE [u].[IsDeleted] = CAST(0 AS bit)
      ) AS [t] ON [o].[UserId] = [t].[UserId]
      WHERE [o].[IsDeleted] = CAST(0 AS bit) AND [o].[OrderStatus] = N'Pending' AND [o].[OrderDate] < @__cancelThreshold_0
info: Microsoft.EntityFrameworkCore.Database.Command[20101]
      Executed DbCommand (57ms) [Parameters=[], CommandType='Text', CommandTimeout='30']
      SELECT DISTINCT [n].[OrderId]
      FROM [Notification] AS [n]
      WHERE [n].[NotifType] = N'PendingOrderReminder' AND [n].[OrderId] IS NOT NULL
info: Microsoft.EntityFrameworkCore.Database.Command[20101]
      Executed DbCommand (57ms) [Parameters=[@__reminderThreshold_0='?' (DbType = DateTime2), @__cancelThreshold_1='?' (DbType = DateTime2), @__alreadyReminded_2='?' (Size = 4000)], CommandType='Text', CommandTimeout='30']
      SELECT [o].[OrderId], [o].[CartId], [o].[ContactPhone], [o].[CreatedAt], [o].[DeliveryInstructions], [o].[DiscountAmount], [o].[FulfillmentType], [o].[GuestSessionId], [o].[IsDeleted], [o].[IsWalkIn], [o].[OrderDate], [o].[OrderNumber], [o].[OrderStatus], [o].[POSSessionId], [o].[PaymentMethod], [o].[ShippingAddressId], [o].[ShippingFee], [o].[SubTotal], [o].[TotalAmount], [o].[UpdatedAt], [o].[UserId], [t].[UserId], [t].[CreatedAt], [t].[DefaultAddressId], [t].[Email], [t].[FailedLoginAttempts], [t].[FirstName], [t].[IsActive], [t].[IsDeleted], [t].[IsWalkIn], [t].[LastLoginAt], [t].[LastName], [t].[LockoutUntil], [t].[PasswordHash], [t].[PhoneNumber]
      FROM [Order] AS [o]
      INNER JOIN (
          SELECT [u].[UserId], [u].[CreatedAt], [u].[DefaultAddressId], [u].[Email], [u].[FailedLoginAttempts], [u].[FirstName], [u].[IsActive], [u].[IsDeleted], [u].[IsWalkIn], [u].[LastLoginAt], [u].[LastName], [u].[LockoutUntil], [u].[PasswordHash], [u].[PhoneNumber]
          FROM [User] AS [u]
          WHERE [u].[IsDeleted] = CAST(0 AS bit)
      ) AS [t] ON [o].[UserId] = [t].[UserId]
      WHERE [o].[IsDeleted] = CAST(0 AS bit) AND [o].[OrderStatus] = N'Pending' AND [o].[OrderDate] < @__reminderThreshold_0 AND [o].[OrderDate] >= @__cancelThreshold_1 AND [o].[OrderId] NOT IN (
          SELECT [a].[value]
          FROM OPENJSON(@__alreadyReminded_2) WITH ([value] int '$') AS [a]
      )
info: Microsoft.EntityFrameworkCore.Database.Command[20101]
      Executed DbCommand (58ms) [Parameters=[@p0='?' (DbType = DateTime2), @p1='?' (Size = 1000), @p2='?' (Size = 100), @p3='?' (DbType = Int32)], CommandType='Text', CommandTimeout='30']
      SET IMPLICIT_TRANSACTIONS OFF;
      SET NOCOUNT ON;
      INSERT INTO [SystemLog] ([CreatedAt], [EventDescription], [EventType], [UserId])
      OUTPUT INSERTED.[SystemLogId]
      VALUES (@p0, @p1, @p2, @p3);

C:\Users\user\Documents\ANDREI_FILES\PDM_FILES\2ND YEAR\2ND SEM\OOP\OOP-TaurusBikeShop\WebApplication\bin\Debug\net8.0\WebApplication.exe (process 29744) exited with code -1 (0xffffffff).
To automatically close the console when debugging stops, enable Tools->Options->Debugging->Automatically close the console when debugging stops.
Press any key to close this window . . .