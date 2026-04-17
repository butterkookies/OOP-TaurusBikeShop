[WARNING] Google Cloud Storage unavailable: Your default credentials were not found. To set up Application Default Credentials, see https://cloud.google.com/docs/authentication/external/set-up-adc.
[WARNING] File upload features will be disabled. Set GOOGLE_APPLICATION_CREDENTIALS to enable.
info: Microsoft.AspNetCore.DataProtection.KeyManagement.XmlKeyManager[63]
      User profile is available. Using 'C:\Users\Brian\AppData\Local\ASP.NET\DataProtection-Keys' as key repository and Windows DPAPI to encrypt keys at rest.
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
dbug: Microsoft.AspNetCore.Watch.BrowserRefresh.BlazorWasmHotReloadMiddleware[0]
      Middleware loaded
dbug: Microsoft.AspNetCore.Watch.BrowserRefresh.BrowserScriptMiddleware[0]
      Middleware loaded. Script /_framework/aspnetcore-browser-refresh.js (16475 B).
dbug: Microsoft.AspNetCore.Watch.BrowserRefresh.BrowserScriptMiddleware[0]
      Middleware loaded. Script /_framework/blazor-hotreload.js (799 B).
dbug: Microsoft.AspNetCore.Watch.BrowserRefresh.BrowserRefreshMiddleware[0]
      Middleware loaded: DOTNET_MODIFIABLE_ASSEMBLIES=debug, __ASPNETCORE_BROWSER_TOOLS=true
info: Microsoft.Hosting.Lifetime[14]
      Now listening on: https://localhost:7177
info: Microsoft.Hosting.Lifetime[14]
      Now listening on: http://localhost:5064
info: Microsoft.Hosting.Lifetime[0]
      Application started. Press Ctrl+C to shut down.
info: Microsoft.Hosting.Lifetime[0]
      Hosting environment: Development
info: Microsoft.Hosting.Lifetime[0]
      Content root path: C:\Users\Brian\OOP-TaurusBikeShop\WebApplication
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
      Executed DbCommand (183ms) [Parameters=[@__p_0='?' (DbType = Int32)], CommandType='Text', CommandTimeout='30']
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
      Executed DbCommand (125ms) [Parameters=[@__p_0='?' (DbType = Int32)], CommandType='Text', CommandTimeout='30']
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
      Executed DbCommand (117ms) [Parameters=[@__p_0='?' (DbType = Int32)], CommandType='Text', CommandTimeout='30']
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
info: Microsoft.EntityFrameworkCore.Database.Command[20101]
      Executed DbCommand (93ms) [Parameters=[], CommandType='Text', CommandTimeout='30']
      SELECT COUNT(*)
      FROM [ProductVariant] AS [p]
dbug: Microsoft.AspNetCore.Watch.BrowserRefresh.BrowserRefreshMiddleware[1]
      Response markup is scheduled to include browser refresh script injection.
info: Microsoft.AspNetCore.Hosting.Diagnostics[1]
      Request starting HTTP/2 GET https://localhost:7177/FULL-COLOR.png - - -
info: Microsoft.AspNetCore.Hosting.Diagnostics[1]
      Request starting HTTP/2 GET https://localhost:7177/css/output.css?v=QkuBhV2JwMBNDh3sDlaKZGIS9wyv9pXnFrVZe6jAtmc - - -
info: Microsoft.AspNetCore.Hosting.Diagnostics[1]
      Request starting HTTP/2 GET https://localhost:7177/lib/jquery/dist/jquery.min.js - - -
info: Microsoft.AspNetCore.StaticFiles.StaticFileMiddleware[6]
      The file /FULL-COLOR.png was not modified
info: Microsoft.AspNetCore.StaticFiles.StaticFileMiddleware[6]
      The file /lib/jquery/dist/jquery.min.js was not modified
info: Microsoft.AspNetCore.StaticFiles.StaticFileMiddleware[6]
      The file /css/output.css was not modified
info: Microsoft.AspNetCore.Hosting.Diagnostics[2]
      Request finished HTTP/2 GET https://localhost:7177/FULL-COLOR.png - 304 - image/png 21.0931ms
info: Microsoft.AspNetCore.Hosting.Diagnostics[2]
      Request finished HTTP/2 GET https://localhost:7177/lib/jquery/dist/jquery.min.js - 304 - text/javascript 10.5889ms
info: Microsoft.AspNetCore.Hosting.Diagnostics[2]
      Request finished HTTP/2 GET https://localhost:7177/css/output.css?v=QkuBhV2JwMBNDh3sDlaKZGIS9wyv9pXnFrVZe6jAtmc - 304 - text/css 21.0881ms
info: Microsoft.AspNetCore.Mvc.ViewFeatures.ViewResultExecutor[4]
      Executed ViewResult - view Index executed in 199.5112ms.
info: Microsoft.AspNetCore.Hosting.Diagnostics[1]
      Request starting HTTP/2 GET https://localhost:7177/js/utils.js?v=FmKd-Gz4ukVHy90T9zxoGNgcaXIdIxgYnAmXep_Oz9M - - -
info: Microsoft.AspNetCore.Hosting.Diagnostics[1]
      Request starting HTTP/2 GET https://localhost:7177/js/app.js?v=DTfZNZxcdNfiaVKlm1nlUmSgEw_wxzST3UrJJqnCqW0 - - -
info: Microsoft.AspNetCore.StaticFiles.StaticFileMiddleware[6]
      The file /js/utils.js was not modified
info: Microsoft.AspNetCore.StaticFiles.StaticFileMiddleware[6]
      The file /js/app.js was not modified
info: Microsoft.AspNetCore.Hosting.Diagnostics[2]
      Request finished HTTP/2 GET https://localhost:7177/js/utils.js?v=FmKd-Gz4ukVHy90T9zxoGNgcaXIdIxgYnAmXep_Oz9M - 304 - text/javascript 6.3536ms
info: Microsoft.AspNetCore.Hosting.Diagnostics[2]
      Request finished HTTP/2 GET https://localhost:7177/js/app.js?v=DTfZNZxcdNfiaVKlm1nlUmSgEw_wxzST3UrJJqnCqW0 - 304 - text/javascript 7.9098ms
info: Microsoft.AspNetCore.Hosting.Diagnostics[1]
      Request starting HTTP/2 GET https://localhost:7177/js/site.js?v=h3a0Utrh7E1bD8mlNvz6owDBe9CR7mm4koHNGtwJHd8 - - -
info: Microsoft.AspNetCore.Hosting.Diagnostics[1]
      Request starting HTTP/2 GET https://localhost:7177/_framework/aspnetcore-browser-refresh.js - - -
info: Microsoft.AspNetCore.StaticFiles.StaticFileMiddleware[6]
      The file /js/site.js was not modified
info: Microsoft.AspNetCore.Hosting.Diagnostics[2]
      Request finished HTTP/2 GET https://localhost:7177/js/site.js?v=h3a0Utrh7E1bD8mlNvz6owDBe9CR7mm4koHNGtwJHd8 - 304 - text/javascript 5.9413ms
info: Microsoft.AspNetCore.Mvc.Infrastructure.ControllerActionInvoker[105]
      Executed action WebApplication.Controllers.HomeController.Index (WebApplication) in 3200.1532ms
info: Microsoft.AspNetCore.Routing.EndpointMiddleware[1]
      Executed endpoint 'WebApplication.Controllers.HomeController.Index (WebApplication)'
dbug: Microsoft.AspNetCore.Watch.BrowserRefresh.BrowserScriptMiddleware[0]
      Script injected: /_framework/aspnetcore-browser-refresh.js
info: Microsoft.AspNetCore.Hosting.Diagnostics[1]
      Request starting HTTP/2 GET https://localhost:7177/_vs/browserLink - - -
info: Microsoft.AspNetCore.Hosting.Diagnostics[2]
      Request finished HTTP/2 GET https://localhost:7177/_framework/aspnetcore-browser-refresh.js - 200 16475 application/javascript;+charset=utf-8 17.6214ms
dbug: Microsoft.AspNetCore.Watch.BrowserRefresh.BrowserRefreshMiddleware[2]
      Response markup was updated to include browser refresh script injection.
info: Microsoft.AspNetCore.Hosting.Diagnostics[1]
      Request starting HTTP/2 GET https://localhost:7177/Cart/GetCartCount - - -
info: Microsoft.AspNetCore.Hosting.Diagnostics[1]
      Request starting HTTP/2 GET https://localhost:7177/Customer/NotificationCount - - -
info: Microsoft.AspNetCore.Hosting.Diagnostics[1]
      Request starting HTTP/2 GET https://localhost:7177/antiforgery/token - - -
info: Microsoft.AspNetCore.Routing.EndpointMiddleware[0]
      Executing endpoint 'WebApplication.Controllers.CartController.GetCartCount (WebApplication)'
info: Microsoft.AspNetCore.Routing.EndpointMiddleware[0]
      Executing endpoint 'HTTP: GET /antiforgery/token'
info: Microsoft.AspNetCore.Mvc.Infrastructure.ControllerActionInvoker[102]
      Route matched with {action = "GetCartCount", controller = "Cart"}. Executing controller action with signature System.Threading.Tasks.Task`1[Microsoft.AspNetCore.Mvc.IActionResult] GetCartCount(System.Threading.CancellationToken) on controller WebApplication.Controllers.CartController (WebApplication).
info: Microsoft.AspNetCore.Routing.EndpointMiddleware[0]
      Executing endpoint 'WebApplication.Controllers.CustomerController.NotificationCount (WebApplication)'
info: Microsoft.AspNetCore.Hosting.Diagnostics[2]
      Request finished HTTP/2 GET https://localhost:7177/ - 200 - text/html;+charset=utf-8 3406.0799ms
info: Microsoft.AspNetCore.Mvc.Infrastructure.ControllerActionInvoker[102]
      Route matched with {action = "NotificationCount", controller = "Customer"}. Executing controller action with signature System.Threading.Tasks.Task`1[Microsoft.AspNetCore.Mvc.IActionResult] NotificationCount(System.Threading.CancellationToken) on controller WebApplication.Controllers.CustomerController (WebApplication).
info: Microsoft.AspNetCore.Http.Result.OkObjectResult[1]
      Setting HTTP status code 200.
info: Microsoft.AspNetCore.Http.Result.OkObjectResult[3]
      Writing value of type '<>f__AnonymousType21`1' as Json.
info: Microsoft.AspNetCore.Routing.EndpointMiddleware[1]
      Executed endpoint 'HTTP: GET /antiforgery/token'
info: Microsoft.AspNetCore.Hosting.Diagnostics[2]
      Request finished HTTP/2 GET https://localhost:7177/antiforgery/token - 200 - application/json;+charset=utf-8 92.1272ms
info: Microsoft.AspNetCore.Hosting.Diagnostics[2]
      Request finished HTTP/2 GET https://localhost:7177/_vs/browserLink - 200 - text/javascript;+charset=UTF-8 117.4137ms
info: Microsoft.EntityFrameworkCore.Database.Command[20101]
      Executed DbCommand (110ms) [Parameters=[@__userId_0='?' (DbType = Int32)], CommandType='Text', CommandTimeout='30']
      SELECT COUNT(*)
      FROM [Notification] AS [n]
      WHERE [n].[UserId] = @__userId_0 AND [n].[IsRead] = CAST(0 AS bit) AND [n].[Channel] = N'InApp'
info: Microsoft.AspNetCore.Mvc.Infrastructure.SystemTextJsonResultExecutor[1]
      Executing JsonResult, writing value of type '<>f__AnonymousType6`1[[System.Int32, System.Private.CoreLib, Version=8.0.0.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e]]'.
info: Microsoft.AspNetCore.Mvc.Infrastructure.ControllerActionInvoker[105]
      Executed action WebApplication.Controllers.CustomerController.NotificationCount (WebApplication) in 162.6789ms
info: Microsoft.AspNetCore.Routing.EndpointMiddleware[1]
      Executed endpoint 'WebApplication.Controllers.CustomerController.NotificationCount (WebApplication)'
info: Microsoft.AspNetCore.Hosting.Diagnostics[2]
      Request finished HTTP/2 GET https://localhost:7177/Customer/NotificationCount - 200 - application/json;+charset=utf-8 236.8753ms
info: Microsoft.EntityFrameworkCore.Database.Command[20101]
      Executed DbCommand (128ms) [Parameters=[@p0='?' (DbType = DateTime2), @p1='?' (Size = 1000), @p2='?' (Size = 100), @p3='?' (DbType = Int32), @p4='?' (DbType = DateTime2), @p5='?' (Size = 1000), @p6='?' (Size = 100), @p7='?' (DbType = Int32)], CommandType='Text', CommandTimeout='30']
      SET IMPLICIT_TRANSACTIONS OFF;
      SET NOCOUNT ON;
      MERGE [SystemLog] USING (
      VALUES (@p0, @p1, @p2, @p3, 0),
      (@p4, @p5, @p6, @p7, 1)) AS i ([CreatedAt], [EventDescription], [EventType], [UserId], _Position) ON 1=0
      WHEN NOT MATCHED THEN
      INSERT ([CreatedAt], [EventDescription], [EventType], [UserId])
      VALUES (i.[CreatedAt], i.[EventDescription], i.[EventType], i.[UserId])
      OUTPUT INSERTED.[SystemLogId], i._Position;
info: Microsoft.AspNetCore.Hosting.Diagnostics[1]
      Request starting HTTP/2 GET https://localhost:7177/Customer/NotificationCount - - -
info: Microsoft.AspNetCore.Routing.EndpointMiddleware[0]
      Executing endpoint 'WebApplication.Controllers.CustomerController.NotificationCount (WebApplication)'
info: Microsoft.AspNetCore.Mvc.Infrastructure.ControllerActionInvoker[102]
      Route matched with {action = "NotificationCount", controller = "Customer"}. Executing controller action with signature System.Threading.Tasks.Task`1[Microsoft.AspNetCore.Mvc.IActionResult] NotificationCount(System.Threading.CancellationToken) on controller WebApplication.Controllers.CustomerController (WebApplication).
info: Microsoft.EntityFrameworkCore.Database.Command[20101]
      Executed DbCommand (117ms) [Parameters=[@__userId_Value_0='?' (DbType = Int32)], CommandType='Text', CommandTimeout='30']
      SELECT TOP(1) [c].[CartId], [c].[CreatedAt], [c].[GuestSessionId], [c].[IsCheckedOut], [c].[LastUpdatedAt], [c].[UserId]
      FROM [Cart] AS [c]
      WHERE [c].[IsCheckedOut] = CAST(0 AS bit) AND [c].[UserId] = @__userId_Value_0
      ORDER BY [c].[CartId]
info: Microsoft.AspNetCore.Mvc.Infrastructure.SystemTextJsonResultExecutor[1]
      Executing JsonResult, writing value of type 'WebApplication.Models.ApiResponse'.
info: Microsoft.AspNetCore.Mvc.Infrastructure.ControllerActionInvoker[105]
      Executed action WebApplication.Controllers.CartController.GetCartCount (WebApplication) in 763.4993ms
info: Microsoft.AspNetCore.Routing.EndpointMiddleware[1]
      Executed endpoint 'WebApplication.Controllers.CartController.GetCartCount (WebApplication)'
info: Microsoft.AspNetCore.Hosting.Diagnostics[2]
      Request finished HTTP/2 GET https://localhost:7177/Cart/GetCartCount - 200 - application/json;+charset=utf-8 801.2493ms
info: Microsoft.EntityFrameworkCore.Database.Command[20101]
      Executed DbCommand (84ms) [Parameters=[@__userId_0='?' (DbType = Int32)], CommandType='Text', CommandTimeout='30']
      SELECT COUNT(*)
      FROM [Notification] AS [n]
      WHERE [n].[UserId] = @__userId_0 AND [n].[IsRead] = CAST(0 AS bit) AND [n].[Channel] = N'InApp'
info: Microsoft.AspNetCore.Mvc.Infrastructure.SystemTextJsonResultExecutor[1]
      Executing JsonResult, writing value of type '<>f__AnonymousType6`1[[System.Int32, System.Private.CoreLib, Version=8.0.0.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e]]'.
info: Microsoft.AspNetCore.Mvc.Infrastructure.ControllerActionInvoker[105]
      Executed action WebApplication.Controllers.CustomerController.NotificationCount (WebApplication) in 93.1442ms
info: Microsoft.AspNetCore.Routing.EndpointMiddleware[1]
      Executed endpoint 'WebApplication.Controllers.CustomerController.NotificationCount (WebApplication)'
info: Microsoft.AspNetCore.Hosting.Diagnostics[2]
      Request finished HTTP/2 GET https://localhost:7177/Customer/NotificationCount - 200 - application/json;+charset=utf-8 111.3092ms
info: Microsoft.EntityFrameworkCore.Database.Command[20101]
      Executed DbCommand (94ms) [Parameters=[@p0='?' (DbType = DateTime2), @p1='?' (Size = 1000), @p2='?' (Size = 100), @p3='?' (DbType = Int32)], CommandType='Text', CommandTimeout='30']
      SET IMPLICIT_TRANSACTIONS OFF;
      SET NOCOUNT ON;
      INSERT INTO [SystemLog] ([CreatedAt], [EventDescription], [EventType], [UserId])
      OUTPUT INSERTED.[SystemLogId]
      VALUES (@p0, @p1, @p2, @p3);
info: Microsoft.EntityFrameworkCore.Database.Command[20101]
      Executed DbCommand (120ms) [Parameters=[], CommandType='Text', CommandTimeout='30']
      SELECT [p].[ProductVariantId], [p].[VariantName], [p].[StockQuantity], [p].[ReorderThreshold], [p].[ProductId], [p0].[Name] AS [ProductName]
      FROM [ProductVariant] AS [p]
      INNER JOIN [Product] AS [p0] ON [p].[ProductId] = [p0].[ProductId]
      WHERE [p].[IsActive] = CAST(1 AS bit) AND [p0].[IsActive] = CAST(1 AS bit) AND [p].[StockQuantity] < [p].[ReorderThreshold]
info: Microsoft.EntityFrameworkCore.Database.Command[20101]
      Executed DbCommand (120ms) [Parameters=[], CommandType='Text', CommandTimeout='30']
      SELECT [u].[UserId], [u].[Email], [u].[FirstName]
      FROM [User] AS [u]
      WHERE [u].[IsDeleted] = CAST(0 AS bit) AND [u].[IsActive] = CAST(1 AS bit) AND [u].[IsWalkIn] = CAST(0 AS bit) AND [u].[Email] IS NOT NULL AND EXISTS (
          SELECT 1
          FROM [UserRole] AS [u0]
          INNER JOIN [Role] AS [r] ON [u0].[RoleId] = [r].[RoleId]
          WHERE [u].[UserId] = [u0].[UserId] AND [r].[RoleName] IN (N'Admin', N'Manager'))
warn: WebApplication.BackgroundJobs.StockMonitorJob[0]
      StockMonitorJob: low stock - variant 1 (2021 Pinewood Climber CARBON 27.5 / Default): 1 remaining (threshold 5).
info: Microsoft.EntityFrameworkCore.Database.Command[20101]
      Executed DbCommand (108ms) [Parameters=[@__8__locals1_cooldownStart_0='?' (DbType = DateTime2), @__Format_2='?' (Size = 1000)], CommandType='Text', CommandTimeout='30']
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
      Executed DbCommand (90ms) [Parameters=[@__8__locals1_cooldownStart_0='?' (DbType = DateTime2), @__Format_2='?' (Size = 1000)], CommandType='Text', CommandTimeout='30']
      SELECT CASE
          WHEN EXISTS (
              SELECT 1
              FROM [SystemLog] AS [s]
              WHERE [s].[EventType] = N'LowStockTriggered' AND [s].[CreatedAt] > @__8__locals1_cooldownStart_0 AND [s].[EventDescription] LIKE @__Format_2) THEN CAST(1 AS bit)
          ELSE CAST(0 AS bit)
      END
warn: WebApplication.BackgroundJobs.StockMonitorJob[0]
      StockMonitorJob: low stock - variant 3 (Toseek Chester 700c Disc Brake ALLOY (2x9) / Default): 4 remaining (threshold 5).
info: Microsoft.EntityFrameworkCore.Database.Command[20101]
      Executed DbCommand (112ms) [Parameters=[@__8__locals1_cooldownStart_0='?' (DbType = DateTime2), @__Format_2='?' (Size = 1000)], CommandType='Text', CommandTimeout='30']
      SELECT CASE
          WHEN EXISTS (
              SELECT 1
              FROM [SystemLog] AS [s]
              WHERE [s].[EventType] = N'LowStockTriggered' AND [s].[CreatedAt] > @__8__locals1_cooldownStart_0 AND [s].[EventDescription] LIKE @__Format_2) THEN CAST(1 AS bit)
          ELSE CAST(0 AS bit)
      END
warn: WebApplication.BackgroundJobs.StockMonitorJob[0]
      StockMonitorJob: low stock - variant 6 (Garuda Rampage / Default): 2 remaining (threshold 5).
info: Microsoft.EntityFrameworkCore.Database.Command[20101]
      Executed DbCommand (86ms) [Parameters=[@__8__locals1_cooldownStart_0='?' (DbType = DateTime2), @__Format_2='?' (Size = 1000)], CommandType='Text', CommandTimeout='30']
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
      Executed DbCommand (96ms) [Parameters=[@__8__locals1_cooldownStart_0='?' (DbType = DateTime2), @__Format_2='?' (Size = 1000)], CommandType='Text', CommandTimeout='30']
      SELECT CASE
          WHEN EXISTS (
              SELECT 1
              FROM [SystemLog] AS [s]
              WHERE [s].[EventType] = N'LowStockTriggered' AND [s].[CreatedAt] > @__8__locals1_cooldownStart_0 AND [s].[EventDescription] LIKE @__Format_2) THEN CAST(1 AS bit)
          ELSE CAST(0 AS bit)
      END
info: Microsoft.EntityFrameworkCore.Database.Command[20101]
      Executed DbCommand (101ms) [Parameters=[@p0='?' (DbType = DateTime2), @p1='?' (Size = 1000), @p2='?' (Size = 100), @p3='?' (DbType = Int32)], CommandType='Text', CommandTimeout='30']
      SET IMPLICIT_TRANSACTIONS OFF;
      SET NOCOUNT ON;
      INSERT INTO [SystemLog] ([CreatedAt], [EventDescription], [EventType], [UserId])
      OUTPUT INSERTED.[SystemLogId]
      VALUES (@p0, @p1, @p2, @p3);
info: Microsoft.EntityFrameworkCore.Database.Command[20101]
      Executed DbCommand (125ms) [Parameters=[@p0='?' (DbType = DateTime2), @p1='?' (Size = 1000), @p2='?' (Size = 100), @p3='?' (DbType = Int32)], CommandType='Text', CommandTimeout='30']
      SET IMPLICIT_TRANSACTIONS OFF;
      SET NOCOUNT ON;
      INSERT INTO [SystemLog] ([CreatedAt], [EventDescription], [EventType], [UserId])
      OUTPUT INSERTED.[SystemLogId]
      VALUES (@p0, @p1, @p2, @p3);
info: Microsoft.EntityFrameworkCore.Database.Command[20101]
      Executed DbCommand (173ms) [Parameters=[@__now_0='?' (DbType = DateTime2)], CommandType='Text', CommandTimeout='30']
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
      Executed DbCommand (95ms) [Parameters=[@p0='?' (DbType = DateTime2), @p1='?' (Size = 1000), @p2='?' (Size = 100), @p3='?' (DbType = Int32)], CommandType='Text', CommandTimeout='30']
      SET IMPLICIT_TRANSACTIONS OFF;
      SET NOCOUNT ON;
      INSERT INTO [SystemLog] ([CreatedAt], [EventDescription], [EventType], [UserId])
      OUTPUT INSERTED.[SystemLogId]
      VALUES (@p0, @p1, @p2, @p3);
info: Microsoft.AspNetCore.Hosting.Diagnostics[1]
      Request starting HTTP/2 GET https://localhost:7177/Order/History - - -
info: Microsoft.AspNetCore.Routing.EndpointMiddleware[0]
      Executing endpoint 'WebApplication.Controllers.OrderController.History (WebApplication)'
info: Microsoft.AspNetCore.Mvc.Infrastructure.ControllerActionInvoker[102]
      Route matched with {action = "History", controller = "Order"}. Executing controller action with signature System.Threading.Tasks.Task`1[Microsoft.AspNetCore.Mvc.IActionResult] History(Int32, System.Threading.CancellationToken) on controller WebApplication.Controllers.OrderController (WebApplication).
info: Microsoft.EntityFrameworkCore.Database.Command[20101]
      Executed DbCommand (126ms) [Parameters=[@__userId_0='?' (DbType = Int32), @__p_1='?' (DbType = Int32), @__p_2='?' (DbType = Int32)], CommandType='Text', CommandTimeout='30']
      SELECT [t].[OrderId], [t].[CartId], [t].[ContactPhone], [t].[CreatedAt], [t].[DeliveryInstructions], [t].[DiscountAmount], [t].[FulfillmentType], [t].[GuestSessionId], [t].[IsDeleted], [t].[IsWalkIn], [t].[OrderDate], [t].[OrderNumber], [t].[OrderStatus], [t].[POSSessionId], [t].[PaymentMethod], [t].[ShippingAddressId], [t].[ShippingFee], [t].[SubTotal], [t].[TotalAmount], [t].[UpdatedAt], [t].[UserId], [p].[PickupOrderId], [p].[OrderId], [p].[PickupConfirmedAt], [p].[PickupExpiresAt], [p].[PickupReadyAt]
      FROM (
          SELECT [o].[OrderId], [o].[CartId], [o].[ContactPhone], [o].[CreatedAt], [o].[DeliveryInstructions], [o].[DiscountAmount], [o].[FulfillmentType], [o].[GuestSessionId], [o].[IsDeleted], [o].[IsWalkIn], [o].[OrderDate], [o].[OrderNumber], [o].[OrderStatus], [o].[POSSessionId], [o].[PaymentMethod], [o].[ShippingAddressId], [o].[ShippingFee], [o].[SubTotal], [o].[TotalAmount], [o].[UpdatedAt], [o].[UserId]
          FROM [Order] AS [o]
          WHERE [o].[IsDeleted] = CAST(0 AS bit) AND [o].[UserId] = @__userId_0
          ORDER BY [o].[OrderDate] DESC
          OFFSET @__p_1 ROWS FETCH NEXT @__p_2 ROWS ONLY
      ) AS [t]
      LEFT JOIN [PickupOrder] AS [p] ON [t].[OrderId] = [p].[OrderId]
      ORDER BY [t].[OrderDate] DESC, [t].[OrderId], [p].[PickupOrderId]
info: Microsoft.EntityFrameworkCore.Database.Command[20101]
      Executed DbCommand (106ms) [Parameters=[@__userId_0='?' (DbType = Int32), @__p_1='?' (DbType = Int32), @__p_2='?' (DbType = Int32)], CommandType='Text', CommandTimeout='30']
      SELECT [o0].[OrderItemId], [o0].[OrderId], [o0].[ProductId], [o0].[ProductVariantId], [o0].[Quantity], [o0].[UnitPrice], [t].[OrderId], [p].[PickupOrderId]
      FROM (
          SELECT [o].[OrderId], [o].[OrderDate]
          FROM [Order] AS [o]
          WHERE [o].[IsDeleted] = CAST(0 AS bit) AND [o].[UserId] = @__userId_0
          ORDER BY [o].[OrderDate] DESC
          OFFSET @__p_1 ROWS FETCH NEXT @__p_2 ROWS ONLY
      ) AS [t]
      LEFT JOIN [PickupOrder] AS [p] ON [t].[OrderId] = [p].[OrderId]
      INNER JOIN [OrderItem] AS [o0] ON [t].[OrderId] = [o0].[OrderId]
      ORDER BY [t].[OrderDate] DESC, [t].[OrderId], [p].[PickupOrderId]
info: Microsoft.EntityFrameworkCore.Database.Command[20101]
      Executed DbCommand (99ms) [Parameters=[@__userId_0='?' (DbType = Int32)], CommandType='Text', CommandTimeout='30']
      SELECT COUNT(*)
      FROM [Order] AS [o]
      WHERE [o].[IsDeleted] = CAST(0 AS bit) AND [o].[UserId] = @__userId_0
info: Microsoft.AspNetCore.Mvc.ViewFeatures.ViewResultExecutor[1]
      Executing ViewResult, running view ~/Views/Customer/OrderHistory.cshtml.
dbug: Microsoft.AspNetCore.Watch.BrowserRefresh.BrowserRefreshMiddleware[1]
      Response markup is scheduled to include browser refresh script injection.
info: Microsoft.AspNetCore.Mvc.ViewFeatures.ViewResultExecutor[4]
      Executed ViewResult - view ~/Views/Customer/OrderHistory.cshtml executed in 28.6213ms.
info: Microsoft.AspNetCore.Mvc.Infrastructure.ControllerActionInvoker[105]
      Executed action WebApplication.Controllers.OrderController.History (WebApplication) in 449.2776ms
info: Microsoft.AspNetCore.Routing.EndpointMiddleware[1]
      Executed endpoint 'WebApplication.Controllers.OrderController.History (WebApplication)'
dbug: Microsoft.AspNetCore.Watch.BrowserRefresh.BrowserRefreshMiddleware[2]
      Response markup was updated to include browser refresh script injection.
info: Microsoft.AspNetCore.Hosting.Diagnostics[2]
      Request finished HTTP/2 GET https://localhost:7177/Order/History - 200 - text/html;+charset=utf-8 473.5772ms
info: Microsoft.AspNetCore.Hosting.Diagnostics[1]
      Request starting HTTP/2 GET https://localhost:7177/_framework/aspnetcore-browser-refresh.js - - -
info: Microsoft.AspNetCore.Hosting.Diagnostics[1]
      Request starting HTTP/2 GET https://localhost:7177/_vs/browserLink - - -
dbug: Microsoft.AspNetCore.Watch.BrowserRefresh.BrowserScriptMiddleware[0]
      Script injected: /_framework/aspnetcore-browser-refresh.js
info: Microsoft.AspNetCore.Hosting.Diagnostics[2]
      Request finished HTTP/2 GET https://localhost:7177/_framework/aspnetcore-browser-refresh.js - 200 16475 application/javascript;+charset=utf-8 7.8167ms
info: Microsoft.AspNetCore.Hosting.Diagnostics[1]
      Request starting HTTP/2 GET https://localhost:7177/Customer/NotificationCount - - -
info: Microsoft.AspNetCore.Hosting.Diagnostics[1]
      Request starting HTTP/2 GET https://localhost:7177/Cart/GetCartCount - - -
info: Microsoft.AspNetCore.Hosting.Diagnostics[1]
      Request starting HTTP/2 GET https://localhost:7177/antiforgery/token - - -
info: Microsoft.AspNetCore.Routing.EndpointMiddleware[0]
      Executing endpoint 'WebApplication.Controllers.CustomerController.NotificationCount (WebApplication)'
info: Microsoft.AspNetCore.Routing.EndpointMiddleware[0]
      Executing endpoint 'WebApplication.Controllers.CartController.GetCartCount (WebApplication)'
info: Microsoft.AspNetCore.Mvc.Infrastructure.ControllerActionInvoker[102]
      Route matched with {action = "NotificationCount", controller = "Customer"}. Executing controller action with signature System.Threading.Tasks.Task`1[Microsoft.AspNetCore.Mvc.IActionResult] NotificationCount(System.Threading.CancellationToken) on controller WebApplication.Controllers.CustomerController (WebApplication).
info: Microsoft.AspNetCore.Routing.EndpointMiddleware[0]
      Executing endpoint 'HTTP: GET /antiforgery/token'
info: Microsoft.AspNetCore.Mvc.Infrastructure.ControllerActionInvoker[102]
      Route matched with {action = "GetCartCount", controller = "Cart"}. Executing controller action with signature System.Threading.Tasks.Task`1[Microsoft.AspNetCore.Mvc.IActionResult] GetCartCount(System.Threading.CancellationToken) on controller WebApplication.Controllers.CartController (WebApplication).
info: Microsoft.AspNetCore.Http.Result.OkObjectResult[1]
      Setting HTTP status code 200.
info: Microsoft.AspNetCore.Http.Result.OkObjectResult[3]
      Writing value of type '<>f__AnonymousType21`1' as Json.
info: Microsoft.AspNetCore.Routing.EndpointMiddleware[1]
      Executed endpoint 'HTTP: GET /antiforgery/token'
info: Microsoft.AspNetCore.Hosting.Diagnostics[2]
      Request finished HTTP/2 GET https://localhost:7177/antiforgery/token - 200 - application/json;+charset=utf-8 23.2325ms
info: Microsoft.AspNetCore.Hosting.Diagnostics[2]
      Request finished HTTP/2 GET https://localhost:7177/_vs/browserLink - 200 - text/javascript;+charset=UTF-8 44.0775ms
info: Microsoft.EntityFrameworkCore.Database.Command[20101]
      Executed DbCommand (86ms) [Parameters=[@__userId_Value_0='?' (DbType = Int32)], CommandType='Text', CommandTimeout='30']
      SELECT TOP(1) [c].[CartId], [c].[CreatedAt], [c].[GuestSessionId], [c].[IsCheckedOut], [c].[LastUpdatedAt], [c].[UserId]
      FROM [Cart] AS [c]
      WHERE [c].[IsCheckedOut] = CAST(0 AS bit) AND [c].[UserId] = @__userId_Value_0
      ORDER BY [c].[CartId]
info: Microsoft.AspNetCore.Mvc.Infrastructure.SystemTextJsonResultExecutor[1]
      Executing JsonResult, writing value of type 'WebApplication.Models.ApiResponse'.
info: Microsoft.AspNetCore.Mvc.Infrastructure.ControllerActionInvoker[105]
      Executed action WebApplication.Controllers.CartController.GetCartCount (WebApplication) in 95.0958ms
info: Microsoft.AspNetCore.Routing.EndpointMiddleware[1]
      Executed endpoint 'WebApplication.Controllers.CartController.GetCartCount (WebApplication)'
info: Microsoft.AspNetCore.Hosting.Diagnostics[2]
      Request finished HTTP/2 GET https://localhost:7177/Cart/GetCartCount - 200 - application/json;+charset=utf-8 116.5226ms
info: Microsoft.EntityFrameworkCore.Database.Command[20101]
      Executed DbCommand (82ms) [Parameters=[@__userId_0='?' (DbType = Int32)], CommandType='Text', CommandTimeout='30']
      SELECT COUNT(*)
      FROM [Notification] AS [n]
      WHERE [n].[UserId] = @__userId_0 AND [n].[IsRead] = CAST(0 AS bit) AND [n].[Channel] = N'InApp'
info: Microsoft.AspNetCore.Mvc.Infrastructure.SystemTextJsonResultExecutor[1]
      Executing JsonResult, writing value of type '<>f__AnonymousType6`1[[System.Int32, System.Private.CoreLib, Version=8.0.0.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e]]'.
info: Microsoft.AspNetCore.Mvc.Infrastructure.ControllerActionInvoker[105]
      Executed action WebApplication.Controllers.CustomerController.NotificationCount (WebApplication) in 109.4287ms
info: Microsoft.AspNetCore.Routing.EndpointMiddleware[1]
      Executed endpoint 'WebApplication.Controllers.CustomerController.NotificationCount (WebApplication)'
info: Microsoft.AspNetCore.Hosting.Diagnostics[2]
      Request finished HTTP/2 GET https://localhost:7177/Customer/NotificationCount - 200 - application/json;+charset=utf-8 127.2518ms
info: Microsoft.AspNetCore.Hosting.Diagnostics[1]
      Request starting HTTP/2 GET https://localhost:7177/Order/Detail?orderId=16 - - -
info: Microsoft.AspNetCore.Routing.EndpointMiddleware[0]
      Executing endpoint 'WebApplication.Controllers.OrderController.Detail (WebApplication)'
info: Microsoft.AspNetCore.Mvc.Infrastructure.ControllerActionInvoker[102]
      Route matched with {action = "Detail", controller = "Order"}. Executing controller action with signature System.Threading.Tasks.Task`1[Microsoft.AspNetCore.Mvc.IActionResult] Detail(Int32, System.Threading.CancellationToken) on controller WebApplication.Controllers.OrderController (WebApplication).
info: Microsoft.EntityFrameworkCore.Database.Command[20101]
      Executed DbCommand (134ms) [Parameters=[@__orderId_0='?' (DbType = Int32)], CommandType='Text', CommandTimeout='30']
      SELECT TOP(1) [o].[OrderId], [o].[CartId], [o].[ContactPhone], [o].[CreatedAt], [o].[DeliveryInstructions], [o].[DiscountAmount], [o].[FulfillmentType], [o].[GuestSessionId], [o].[IsDeleted], [o].[IsWalkIn], [o].[OrderDate], [o].[OrderNumber], [o].[OrderStatus], [o].[POSSessionId], [o].[PaymentMethod], [o].[ShippingAddressId], [o].[ShippingFee], [o].[SubTotal], [o].[TotalAmount], [o].[UpdatedAt], [o].[UserId], [p].[PickupOrderId], [a].[AddressId], [p].[OrderId], [p].[PickupConfirmedAt], [p].[PickupExpiresAt], [p].[PickupReadyAt], [a].[City], [a].[Country], [a].[CreatedAt], [a].[IsDefault], [a].[IsSnapshot], [a].[Label], [a].[PostalCode], [a].[Province], [a].[Street], [a].[UserId]
      FROM [Order] AS [o]
      LEFT JOIN [PickupOrder] AS [p] ON [o].[OrderId] = [p].[OrderId]
      LEFT JOIN [Address] AS [a] ON [o].[ShippingAddressId] = [a].[AddressId]
      WHERE [o].[IsDeleted] = CAST(0 AS bit) AND [o].[OrderId] = @__orderId_0
      ORDER BY [o].[OrderId], [p].[PickupOrderId], [a].[AddressId]
info: Microsoft.EntityFrameworkCore.Database.Command[20101]
      Executed DbCommand (113ms) [Parameters=[@p0='?' (DbType = DateTime2), @p1='?' (Size = 1000), @p2='?' (Size = 100), @p3='?' (DbType = Int32)], CommandType='Text', CommandTimeout='30']
      SET IMPLICIT_TRANSACTIONS OFF;
      SET NOCOUNT ON;
      INSERT INTO [SystemLog] ([CreatedAt], [EventDescription], [EventType], [UserId])
      OUTPUT INSERTED.[SystemLogId]
      VALUES (@p0, @p1, @p2, @p3);
info: Microsoft.EntityFrameworkCore.Database.Command[20101]
      Executed DbCommand (79ms) [Parameters=[@__staleThreshold_0='?' (DbType = DateTime2)], CommandType='Text', CommandTimeout='30']
      SELECT [o].[OrderId], [o].[OrderNumber]
      FROM [Order] AS [o]
      WHERE [o].[IsDeleted] = CAST(0 AS bit) AND [o].[OrderStatus] = N'Pending' AND [o].[OrderDate] < @__staleThreshold_0
info: Microsoft.EntityFrameworkCore.Database.Command[20101]
      Executed DbCommand (147ms) [Parameters=[@__orderId_0='?' (DbType = Int32)], CommandType='Text', CommandTimeout='30']
      SELECT [t0].[OrderItemId], [t0].[OrderId], [t0].[ProductId], [t0].[ProductVariantId], [t0].[Quantity], [t0].[UnitPrice], [t0].[ProductId0], [t0].[AdditionalSpecs], [t0].[AxleStandard], [t0].[BoostCompatible], [t0].[BrakeType], [t0].[BrandId], [t0].[CategoryId], [t0].[Color], [t0].[CreatedAt], [t0].[Currency], [t0].[Description], [t0].[IsActive], [t0].[IsFeatured], [t0].[Material], [t0].[Name], [t0].[Price], [t0].[SKU], [t0].[ShortDescription], [t0].[SpeedCompatibility], [t0].[SuspensionTravel], [t0].[TubelessReady], [t0].[UpdatedAt], [t0].[WheelSize], [t].[OrderId], [t].[PickupOrderId], [t].[AddressId], [t0].[ProductVariantId0], [t0].[AdditionalPrice], [t0].[CreatedAt0], [t0].[IsActive0], [t0].[ProductId1], [t0].[ReorderThreshold], [t0].[SKU0], [t0].[StockQuantity], [t0].[UpdatedAt0], [t0].[VariantName]
      FROM (
          SELECT TOP(1) [o].[OrderId], [p].[PickupOrderId], [a].[AddressId]
          FROM [Order] AS [o]
          LEFT JOIN [PickupOrder] AS [p] ON [o].[OrderId] = [p].[OrderId]
          LEFT JOIN [Address] AS [a] ON [o].[ShippingAddressId] = [a].[AddressId]
          WHERE [o].[IsDeleted] = CAST(0 AS bit) AND [o].[OrderId] = @__orderId_0
      ) AS [t]
      INNER JOIN (
          SELECT [o0].[OrderItemId], [o0].[OrderId], [o0].[ProductId], [o0].[ProductVariantId], [o0].[Quantity], [o0].[UnitPrice], [p0].[ProductId] AS [ProductId0], [p0].[AdditionalSpecs], [p0].[AxleStandard], [p0].[BoostCompatible], [p0].[BrakeType], [p0].[BrandId], [p0].[CategoryId], [p0].[Color], [p0].[CreatedAt], [p0].[Currency], [p0].[Description], [p0].[IsActive], [p0].[IsFeatured], [p0].[Material], [p0].[Name], [p0].[Price], [p0].[SKU], [p0].[ShortDescription], [p0].[SpeedCompatibility], [p0].[SuspensionTravel], [p0].[TubelessReady], [p0].[UpdatedAt], [p0].[WheelSize], [p1].[ProductVariantId] AS [ProductVariantId0], [p1].[AdditionalPrice], [p1].[CreatedAt] AS [CreatedAt0], [p1].[IsActive] AS [IsActive0], [p1].[ProductId] AS [ProductId1], [p1].[ReorderThreshold], [p1].[SKU] AS [SKU0], [p1].[StockQuantity], [p1].[UpdatedAt] AS [UpdatedAt0], [p1].[VariantName]
          FROM [OrderItem] AS [o0]
          INNER JOIN [Product] AS [p0] ON [o0].[ProductId] = [p0].[ProductId]
          LEFT JOIN [ProductVariant] AS [p1] ON [o0].[ProductVariantId] = [p1].[ProductVariantId]
      ) AS [t0] ON [t].[OrderId] = [t0].[OrderId]
      ORDER BY [t].[OrderId], [t].[PickupOrderId], [t].[AddressId], [t0].[OrderItemId], [t0].[ProductId0], [t0].[ProductVariantId0]
info: Microsoft.EntityFrameworkCore.Database.Command[20101]
      Executed DbCommand (95ms) [Parameters=[@p0='?' (DbType = DateTime2), @p1='?' (Size = 1000), @p2='?' (Size = 100), @p3='?' (DbType = Int32)], CommandType='Text', CommandTimeout='30']
      SET IMPLICIT_TRANSACTIONS OFF;
      SET NOCOUNT ON;
      INSERT INTO [SystemLog] ([CreatedAt], [EventDescription], [EventType], [UserId])
      OUTPUT INSERTED.[SystemLogId]
      VALUES (@p0, @p1, @p2, @p3);
info: Microsoft.EntityFrameworkCore.Database.Command[20101]
      Executed DbCommand (118ms) [Parameters=[@__orderId_0='?' (DbType = Int32)], CommandType='Text', CommandTimeout='30']
      SELECT [p2].[ProductImageId], [p2].[AltText], [p2].[CreatedAt], [p2].[DisplayOrder], [p2].[FileSizeBytes], [p2].[Height], [p2].[ImageType], [p2].[ImageUrl], [p2].[IsPrimary], [p2].[MimeType], [p2].[ProductId], [p2].[StorageBucket], [p2].[StoragePath], [p2].[UploadedByUserId], [p2].[Width], [t].[OrderId], [t].[PickupOrderId], [t].[AddressId], [t0].[OrderItemId], [t0].[ProductId0], [t0].[ProductVariantId0]
      FROM (
          SELECT TOP(1) [o].[OrderId], [p].[PickupOrderId], [a].[AddressId]
          FROM [Order] AS [o]
          LEFT JOIN [PickupOrder] AS [p] ON [o].[OrderId] = [p].[OrderId]
          LEFT JOIN [Address] AS [a] ON [o].[ShippingAddressId] = [a].[AddressId]
          WHERE [o].[IsDeleted] = CAST(0 AS bit) AND [o].[OrderId] = @__orderId_0
      ) AS [t]
      INNER JOIN (
          SELECT [o0].[OrderItemId], [o0].[OrderId], [p0].[ProductId] AS [ProductId0], [p1].[ProductVariantId] AS [ProductVariantId0]
          FROM [OrderItem] AS [o0]
          INNER JOIN [Product] AS [p0] ON [o0].[ProductId] = [p0].[ProductId]
          LEFT JOIN [ProductVariant] AS [p1] ON [o0].[ProductVariantId] = [p1].[ProductVariantId]
      ) AS [t0] ON [t].[OrderId] = [t0].[OrderId]
      INNER JOIN [ProductImage] AS [p2] ON [t0].[ProductId0] = [p2].[ProductId]
      ORDER BY [t].[OrderId], [t].[PickupOrderId], [t].[AddressId], [t0].[OrderItemId], [t0].[ProductId0], [t0].[ProductVariantId0]
info: Microsoft.EntityFrameworkCore.Database.Command[20101]
      Executed DbCommand (114ms) [Parameters=[@__orderId_0='?' (DbType = Int32)], CommandType='Text', CommandTimeout='30']
      SELECT [t0].[PaymentId], [t0].[Amount], [t0].[CreatedAt], [t0].[OrderId], [t0].[PaidToAccountName], [t0].[PaidToAccountNumber], [t0].[PaidToBankName], [t0].[PaymentDate], [t0].[PaymentMethod], [t0].[PaymentStage], [t0].[PaymentStatus], [t0].[PaymentId0], [t0].[GcashTransactionId], [t0].[ScreenshotUrl], [t0].[StorageBucket], [t0].[StoragePath], [t0].[SubmittedAt], [t0].[PaymentId1], [t0].[BpiReferenceNumber], [t0].[ProofStorageBucket], [t0].[ProofStoragePath], [t0].[ProofUrl], [t0].[SubmittedAt0], [t0].[VerificationDeadline], [t0].[VerificationNotes], [t0].[VerifiedAt], [t0].[VerifiedByUserId], [t].[OrderId], [t].[PickupOrderId], [t].[AddressId]
      FROM (
          SELECT TOP(1) [o].[OrderId], [p].[PickupOrderId], [a].[AddressId]
          FROM [Order] AS [o]
          LEFT JOIN [PickupOrder] AS [p] ON [o].[OrderId] = [p].[OrderId]
          LEFT JOIN [Address] AS [a] ON [o].[ShippingAddressId] = [a].[AddressId]
          WHERE [o].[IsDeleted] = CAST(0 AS bit) AND [o].[OrderId] = @__orderId_0
      ) AS [t]
      INNER JOIN (
          SELECT [p0].[PaymentId], [p0].[Amount], [p0].[CreatedAt], [p0].[OrderId], [p0].[PaidToAccountName], [p0].[PaidToAccountNumber], [p0].[PaidToBankName], [p0].[PaymentDate], [p0].[PaymentMethod], [p0].[PaymentStage], [p0].[PaymentStatus], [g].[PaymentId] AS [PaymentId0], [g].[GcashTransactionId], [g].[ScreenshotUrl], [g].[StorageBucket], [g].[StoragePath], [g].[SubmittedAt], [b].[PaymentId] AS [PaymentId1], [b].[BpiReferenceNumber], [b].[ProofStorageBucket], [b].[ProofStoragePath], [b].[ProofUrl], [b].[SubmittedAt] AS [SubmittedAt0], [b].[VerificationDeadline], [b].[VerificationNotes], [b].[VerifiedAt], [b].[VerifiedByUserId]
          FROM [Payment] AS [p0]
          LEFT JOIN [GCashPayment] AS [g] ON [p0].[PaymentId] = [g].[PaymentId]
          LEFT JOIN [BankTransferPayment] AS [b] ON [p0].[PaymentId] = [b].[PaymentId]
      ) AS [t0] ON [t].[OrderId] = [t0].[OrderId]
      ORDER BY [t].[OrderId], [t].[PickupOrderId], [t].[AddressId]
info: Microsoft.EntityFrameworkCore.Database.Command[20101]
      Executed DbCommand (103ms) [Parameters=[@__orderId_0='?' (DbType = Int32)], CommandType='Text', CommandTimeout='30']
      SELECT [t0].[DeliveryId], [t0].[ActualDeliveryTime], [t0].[Courier], [t0].[CreatedAt], [t0].[DelayedUntil], [t0].[DeliveryStatus], [t0].[EstimatedDeliveryTime], [t0].[IsDelayed], [t0].[OrderId], [t0].[DeliveryId0], [t0].[BookingRef], [t0].[DriverName], [t0].[DriverPhone], [t0].[DeliveryId1], [t0].[TrackingNumber], [t].[OrderId], [t].[PickupOrderId], [t].[AddressId]
      FROM (
          SELECT TOP(1) [o].[OrderId], [p].[PickupOrderId], [a].[AddressId]
          FROM [Order] AS [o]
          LEFT JOIN [PickupOrder] AS [p] ON [o].[OrderId] = [p].[OrderId]
          LEFT JOIN [Address] AS [a] ON [o].[ShippingAddressId] = [a].[AddressId]
          WHERE [o].[IsDeleted] = CAST(0 AS bit) AND [o].[OrderId] = @__orderId_0
      ) AS [t]
      INNER JOIN (
          SELECT [d].[DeliveryId], [d].[ActualDeliveryTime], [d].[Courier], [d].[CreatedAt], [d].[DelayedUntil], [d].[DeliveryStatus], [d].[EstimatedDeliveryTime], [d].[IsDelayed], [d].[OrderId], [l].[DeliveryId] AS [DeliveryId0], [l].[BookingRef], [l].[DriverName], [l].[DriverPhone], [l0].[DeliveryId] AS [DeliveryId1], [l0].[TrackingNumber]
          FROM [Delivery] AS [d]
          LEFT JOIN [LalamoveDelivery] AS [l] ON [d].[DeliveryId] = [l].[DeliveryId]
          LEFT JOIN [LBCDelivery] AS [l0] ON [d].[DeliveryId] = [l0].[DeliveryId]
      ) AS [t0] ON [t].[OrderId] = [t0].[OrderId]
      ORDER BY [t].[OrderId], [t].[PickupOrderId], [t].[AddressId]
info: Microsoft.AspNetCore.Mvc.ViewFeatures.ViewResultExecutor[1]
      Executing ViewResult, running view ~/Views/Customer/OrderDetail.cshtml.
dbug: Microsoft.AspNetCore.Watch.BrowserRefresh.BrowserRefreshMiddleware[1]
      Response markup is scheduled to include browser refresh script injection.
info: Microsoft.AspNetCore.Mvc.ViewFeatures.ViewResultExecutor[4]
      Executed ViewResult - view ~/Views/Customer/OrderDetail.cshtml executed in 29.6126ms.
info: Microsoft.AspNetCore.Mvc.Infrastructure.ControllerActionInvoker[105]
      Executed action WebApplication.Controllers.OrderController.Detail (WebApplication) in 751.3964ms
info: Microsoft.AspNetCore.Routing.EndpointMiddleware[1]
      Executed endpoint 'WebApplication.Controllers.OrderController.Detail (WebApplication)'
dbug: Microsoft.AspNetCore.Watch.BrowserRefresh.BrowserRefreshMiddleware[2]
      Response markup was updated to include browser refresh script injection.
info: Microsoft.AspNetCore.Hosting.Diagnostics[2]
      Request finished HTTP/2 GET https://localhost:7177/Order/Detail?orderId=16 - 200 - text/html;+charset=utf-8 770.1219ms
info: Microsoft.AspNetCore.Hosting.Diagnostics[1]
      Request starting HTTP/2 GET https://localhost:7177/_vs/browserLink - - -
info: Microsoft.AspNetCore.Hosting.Diagnostics[1]
      Request starting HTTP/2 GET https://localhost:7177/_framework/aspnetcore-browser-refresh.js - - -
dbug: Microsoft.AspNetCore.Watch.BrowserRefresh.BrowserScriptMiddleware[0]
      Script injected: /_framework/aspnetcore-browser-refresh.js
info: Microsoft.AspNetCore.Hosting.Diagnostics[2]
      Request finished HTTP/2 GET https://localhost:7177/_framework/aspnetcore-browser-refresh.js - 200 16475 application/javascript;+charset=utf-8 7.4652ms
info: Microsoft.AspNetCore.Hosting.Diagnostics[1]
      Request starting HTTP/2 GET https://localhost:7177/Cart/GetCartCount - - -
info: Microsoft.AspNetCore.Hosting.Diagnostics[1]
      Request starting HTTP/2 GET https://localhost:7177/Customer/NotificationCount - - -
info: Microsoft.AspNetCore.Routing.EndpointMiddleware[0]
      Executing endpoint 'WebApplication.Controllers.CartController.GetCartCount (WebApplication)'
info: Microsoft.AspNetCore.Routing.EndpointMiddleware[0]
      Executing endpoint 'WebApplication.Controllers.CustomerController.NotificationCount (WebApplication)'
info: Microsoft.AspNetCore.Mvc.Infrastructure.ControllerActionInvoker[102]
      Route matched with {action = "GetCartCount", controller = "Cart"}. Executing controller action with signature System.Threading.Tasks.Task`1[Microsoft.AspNetCore.Mvc.IActionResult] GetCartCount(System.Threading.CancellationToken) on controller WebApplication.Controllers.CartController (WebApplication).
info: Microsoft.AspNetCore.Mvc.Infrastructure.ControllerActionInvoker[102]
      Route matched with {action = "NotificationCount", controller = "Customer"}. Executing controller action with signature System.Threading.Tasks.Task`1[Microsoft.AspNetCore.Mvc.IActionResult] NotificationCount(System.Threading.CancellationToken) on controller WebApplication.Controllers.CustomerController (WebApplication).
info: Microsoft.AspNetCore.Hosting.Diagnostics[2]
      Request finished HTTP/2 GET https://localhost:7177/_vs/browserLink - 200 - text/javascript;+charset=UTF-8 31.5725ms
info: Microsoft.EntityFrameworkCore.Database.Command[20101]
      Executed DbCommand (97ms) [Parameters=[@__userId_0='?' (DbType = Int32)], CommandType='Text', CommandTimeout='30']
      SELECT COUNT(*)
      FROM [Notification] AS [n]
      WHERE [n].[UserId] = @__userId_0 AND [n].[IsRead] = CAST(0 AS bit) AND [n].[Channel] = N'InApp'
info: Microsoft.EntityFrameworkCore.Database.Command[20101]
      Executed DbCommand (88ms) [Parameters=[@__userId_Value_0='?' (DbType = Int32)], CommandType='Text', CommandTimeout='30']
      SELECT TOP(1) [c].[CartId], [c].[CreatedAt], [c].[GuestSessionId], [c].[IsCheckedOut], [c].[LastUpdatedAt], [c].[UserId]
      FROM [Cart] AS [c]
      WHERE [c].[IsCheckedOut] = CAST(0 AS bit) AND [c].[UserId] = @__userId_Value_0
      ORDER BY [c].[CartId]
info: Microsoft.AspNetCore.Mvc.Infrastructure.SystemTextJsonResultExecutor[1]
      Executing JsonResult, writing value of type '<>f__AnonymousType6`1[[System.Int32, System.Private.CoreLib, Version=8.0.0.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e]]'.
info: Microsoft.AspNetCore.Mvc.Infrastructure.SystemTextJsonResultExecutor[1]
      Executing JsonResult, writing value of type 'WebApplication.Models.ApiResponse'.
info: Microsoft.AspNetCore.Mvc.Infrastructure.ControllerActionInvoker[105]
      Executed action WebApplication.Controllers.CustomerController.NotificationCount (WebApplication) in 105.1451ms
info: Microsoft.AspNetCore.Mvc.Infrastructure.ControllerActionInvoker[105]
      Executed action WebApplication.Controllers.CartController.GetCartCount (WebApplication) in 108.5995ms
info: Microsoft.AspNetCore.Routing.EndpointMiddleware[1]
      Executed endpoint 'WebApplication.Controllers.CustomerController.NotificationCount (WebApplication)'
info: Microsoft.AspNetCore.Routing.EndpointMiddleware[1]
      Executed endpoint 'WebApplication.Controllers.CartController.GetCartCount (WebApplication)'
info: Microsoft.AspNetCore.Hosting.Diagnostics[2]
      Request finished HTTP/2 GET https://localhost:7177/Customer/NotificationCount - 200 - application/json;+charset=utf-8 123.0301ms
info: Microsoft.AspNetCore.Hosting.Diagnostics[2]
      Request finished HTTP/2 GET https://localhost:7177/Cart/GetCartCount - 200 - application/json;+charset=utf-8 126.0487ms
info: Microsoft.AspNetCore.Hosting.Diagnostics[1]
      Request starting HTTP/2 GET https://localhost:7177/Payment/Submit?orderId=16 - - -
info: Microsoft.AspNetCore.Routing.EndpointMiddleware[0]
      Executing endpoint 'WebApplication.Controllers.PaymentController.Submit (WebApplication)'
info: Microsoft.AspNetCore.Mvc.Infrastructure.ControllerActionInvoker[102]
      Route matched with {action = "Submit", controller = "Payment"}. Executing controller action with signature System.Threading.Tasks.Task`1[Microsoft.AspNetCore.Mvc.IActionResult] Submit(Int32, System.Threading.CancellationToken) on controller WebApplication.Controllers.PaymentController (WebApplication).
info: Microsoft.EntityFrameworkCore.Database.Command[20101]
      Executed DbCommand (106ms) [Parameters=[@__orderId_0='?' (DbType = Int32)], CommandType='Text', CommandTimeout='30']
      SELECT TOP(1) [o].[OrderId], [o].[CartId], [o].[ContactPhone], [o].[CreatedAt], [o].[DeliveryInstructions], [o].[DiscountAmount], [o].[FulfillmentType], [o].[GuestSessionId], [o].[IsDeleted], [o].[IsWalkIn], [o].[OrderDate], [o].[OrderNumber], [o].[OrderStatus], [o].[POSSessionId], [o].[PaymentMethod], [o].[ShippingAddressId], [o].[ShippingFee], [o].[SubTotal], [o].[TotalAmount], [o].[UpdatedAt], [o].[UserId], [p].[PickupOrderId], [a].[AddressId], [p].[OrderId], [p].[PickupConfirmedAt], [p].[PickupExpiresAt], [p].[PickupReadyAt], [a].[City], [a].[Country], [a].[CreatedAt], [a].[IsDefault], [a].[IsSnapshot], [a].[Label], [a].[PostalCode], [a].[Province], [a].[Street], [a].[UserId]
      FROM [Order] AS [o]
      LEFT JOIN [PickupOrder] AS [p] ON [o].[OrderId] = [p].[OrderId]
      LEFT JOIN [Address] AS [a] ON [o].[ShippingAddressId] = [a].[AddressId]
      WHERE [o].[IsDeleted] = CAST(0 AS bit) AND [o].[OrderId] = @__orderId_0
      ORDER BY [o].[OrderId], [p].[PickupOrderId], [a].[AddressId]
info: Microsoft.EntityFrameworkCore.Database.Command[20101]
      Executed DbCommand (103ms) [Parameters=[@__orderId_0='?' (DbType = Int32)], CommandType='Text', CommandTimeout='30']
      SELECT [t0].[OrderItemId], [t0].[OrderId], [t0].[ProductId], [t0].[ProductVariantId], [t0].[Quantity], [t0].[UnitPrice], [t0].[ProductId0], [t0].[AdditionalSpecs], [t0].[AxleStandard], [t0].[BoostCompatible], [t0].[BrakeType], [t0].[BrandId], [t0].[CategoryId], [t0].[Color], [t0].[CreatedAt], [t0].[Currency], [t0].[Description], [t0].[IsActive], [t0].[IsFeatured], [t0].[Material], [t0].[Name], [t0].[Price], [t0].[SKU], [t0].[ShortDescription], [t0].[SpeedCompatibility], [t0].[SuspensionTravel], [t0].[TubelessReady], [t0].[UpdatedAt], [t0].[WheelSize], [t].[OrderId], [t].[PickupOrderId], [t].[AddressId], [t0].[ProductVariantId0], [t0].[AdditionalPrice], [t0].[CreatedAt0], [t0].[IsActive0], [t0].[ProductId1], [t0].[ReorderThreshold], [t0].[SKU0], [t0].[StockQuantity], [t0].[UpdatedAt0], [t0].[VariantName]
      FROM (
          SELECT TOP(1) [o].[OrderId], [p].[PickupOrderId], [a].[AddressId]
          FROM [Order] AS [o]
          LEFT JOIN [PickupOrder] AS [p] ON [o].[OrderId] = [p].[OrderId]
          LEFT JOIN [Address] AS [a] ON [o].[ShippingAddressId] = [a].[AddressId]
          WHERE [o].[IsDeleted] = CAST(0 AS bit) AND [o].[OrderId] = @__orderId_0
      ) AS [t]
      INNER JOIN (
          SELECT [o0].[OrderItemId], [o0].[OrderId], [o0].[ProductId], [o0].[ProductVariantId], [o0].[Quantity], [o0].[UnitPrice], [p0].[ProductId] AS [ProductId0], [p0].[AdditionalSpecs], [p0].[AxleStandard], [p0].[BoostCompatible], [p0].[BrakeType], [p0].[BrandId], [p0].[CategoryId], [p0].[Color], [p0].[CreatedAt], [p0].[Currency], [p0].[Description], [p0].[IsActive], [p0].[IsFeatured], [p0].[Material], [p0].[Name], [p0].[Price], [p0].[SKU], [p0].[ShortDescription], [p0].[SpeedCompatibility], [p0].[SuspensionTravel], [p0].[TubelessReady], [p0].[UpdatedAt], [p0].[WheelSize], [p1].[ProductVariantId] AS [ProductVariantId0], [p1].[AdditionalPrice], [p1].[CreatedAt] AS [CreatedAt0], [p1].[IsActive] AS [IsActive0], [p1].[ProductId] AS [ProductId1], [p1].[ReorderThreshold], [p1].[SKU] AS [SKU0], [p1].[StockQuantity], [p1].[UpdatedAt] AS [UpdatedAt0], [p1].[VariantName]
          FROM [OrderItem] AS [o0]
          INNER JOIN [Product] AS [p0] ON [o0].[ProductId] = [p0].[ProductId]
          LEFT JOIN [ProductVariant] AS [p1] ON [o0].[ProductVariantId] = [p1].[ProductVariantId]
      ) AS [t0] ON [t].[OrderId] = [t0].[OrderId]
      ORDER BY [t].[OrderId], [t].[PickupOrderId], [t].[AddressId], [t0].[OrderItemId], [t0].[ProductId0], [t0].[ProductVariantId0]
info: Microsoft.EntityFrameworkCore.Database.Command[20101]
      Executed DbCommand (94ms) [Parameters=[@__orderId_0='?' (DbType = Int32)], CommandType='Text', CommandTimeout='30']
      SELECT [p2].[ProductImageId], [p2].[AltText], [p2].[CreatedAt], [p2].[DisplayOrder], [p2].[FileSizeBytes], [p2].[Height], [p2].[ImageType], [p2].[ImageUrl], [p2].[IsPrimary], [p2].[MimeType], [p2].[ProductId], [p2].[StorageBucket], [p2].[StoragePath], [p2].[UploadedByUserId], [p2].[Width], [t].[OrderId], [t].[PickupOrderId], [t].[AddressId], [t0].[OrderItemId], [t0].[ProductId0], [t0].[ProductVariantId0]
      FROM (
          SELECT TOP(1) [o].[OrderId], [p].[PickupOrderId], [a].[AddressId]
          FROM [Order] AS [o]
          LEFT JOIN [PickupOrder] AS [p] ON [o].[OrderId] = [p].[OrderId]
          LEFT JOIN [Address] AS [a] ON [o].[ShippingAddressId] = [a].[AddressId]
          WHERE [o].[IsDeleted] = CAST(0 AS bit) AND [o].[OrderId] = @__orderId_0
      ) AS [t]
      INNER JOIN (
          SELECT [o0].[OrderItemId], [o0].[OrderId], [p0].[ProductId] AS [ProductId0], [p1].[ProductVariantId] AS [ProductVariantId0]
          FROM [OrderItem] AS [o0]
          INNER JOIN [Product] AS [p0] ON [o0].[ProductId] = [p0].[ProductId]
          LEFT JOIN [ProductVariant] AS [p1] ON [o0].[ProductVariantId] = [p1].[ProductVariantId]
      ) AS [t0] ON [t].[OrderId] = [t0].[OrderId]
      INNER JOIN [ProductImage] AS [p2] ON [t0].[ProductId0] = [p2].[ProductId]
      ORDER BY [t].[OrderId], [t].[PickupOrderId], [t].[AddressId], [t0].[OrderItemId], [t0].[ProductId0], [t0].[ProductVariantId0]
info: Microsoft.EntityFrameworkCore.Database.Command[20101]
      Executed DbCommand (75ms) [Parameters=[@p0='?' (DbType = DateTime2), @p1='?' (Size = 1000), @p2='?' (Size = 100), @p3='?' (DbType = Int32)], CommandType='Text', CommandTimeout='30']
      SET IMPLICIT_TRANSACTIONS OFF;
      SET NOCOUNT ON;
      INSERT INTO [SystemLog] ([CreatedAt], [EventDescription], [EventType], [UserId])
      OUTPUT INSERTED.[SystemLogId]
      VALUES (@p0, @p1, @p2, @p3);
info: Microsoft.EntityFrameworkCore.Database.Command[20101]
      Executed DbCommand (125ms) [Parameters=[@__orderId_0='?' (DbType = Int32)], CommandType='Text', CommandTimeout='30']
      SELECT [t0].[PaymentId], [t0].[Amount], [t0].[CreatedAt], [t0].[OrderId], [t0].[PaidToAccountName], [t0].[PaidToAccountNumber], [t0].[PaidToBankName], [t0].[PaymentDate], [t0].[PaymentMethod], [t0].[PaymentStage], [t0].[PaymentStatus], [t0].[PaymentId0], [t0].[GcashTransactionId], [t0].[ScreenshotUrl], [t0].[StorageBucket], [t0].[StoragePath], [t0].[SubmittedAt], [t0].[PaymentId1], [t0].[BpiReferenceNumber], [t0].[ProofStorageBucket], [t0].[ProofStoragePath], [t0].[ProofUrl], [t0].[SubmittedAt0], [t0].[VerificationDeadline], [t0].[VerificationNotes], [t0].[VerifiedAt], [t0].[VerifiedByUserId], [t].[OrderId], [t].[PickupOrderId], [t].[AddressId]
      FROM (
          SELECT TOP(1) [o].[OrderId], [p].[PickupOrderId], [a].[AddressId]
          FROM [Order] AS [o]
          LEFT JOIN [PickupOrder] AS [p] ON [o].[OrderId] = [p].[OrderId]
          LEFT JOIN [Address] AS [a] ON [o].[ShippingAddressId] = [a].[AddressId]
          WHERE [o].[IsDeleted] = CAST(0 AS bit) AND [o].[OrderId] = @__orderId_0
      ) AS [t]
      INNER JOIN (
          SELECT [p0].[PaymentId], [p0].[Amount], [p0].[CreatedAt], [p0].[OrderId], [p0].[PaidToAccountName], [p0].[PaidToAccountNumber], [p0].[PaidToBankName], [p0].[PaymentDate], [p0].[PaymentMethod], [p0].[PaymentStage], [p0].[PaymentStatus], [g].[PaymentId] AS [PaymentId0], [g].[GcashTransactionId], [g].[ScreenshotUrl], [g].[StorageBucket], [g].[StoragePath], [g].[SubmittedAt], [b].[PaymentId] AS [PaymentId1], [b].[BpiReferenceNumber], [b].[ProofStorageBucket], [b].[ProofStoragePath], [b].[ProofUrl], [b].[SubmittedAt] AS [SubmittedAt0], [b].[VerificationDeadline], [b].[VerificationNotes], [b].[VerifiedAt], [b].[VerifiedByUserId]
          FROM [Payment] AS [p0]
          LEFT JOIN [GCashPayment] AS [g] ON [p0].[PaymentId] = [g].[PaymentId]
          LEFT JOIN [BankTransferPayment] AS [b] ON [p0].[PaymentId] = [b].[PaymentId]
      ) AS [t0] ON [t].[OrderId] = [t0].[OrderId]
      ORDER BY [t].[OrderId], [t].[PickupOrderId], [t].[AddressId]
info: Microsoft.EntityFrameworkCore.Database.Command[20101]
      Executed DbCommand (130ms) [Parameters=[], CommandType='Text', CommandTimeout='30']
      SELECT [d].[DeliveryId], [d].[ActualDeliveryTime], [d].[Courier], [d].[CreatedAt], [d].[DelayedUntil], [d].[DeliveryStatus], [d].[EstimatedDeliveryTime], [d].[IsDelayed], [d].[OrderId], [l].[DeliveryId], [l].[BookingRef], [l].[DriverName], [l].[DriverPhone], [l0].[DeliveryId], [l0].[TrackingNumber], [t].[OrderId], [t].[CartId], [t].[ContactPhone], [t].[CreatedAt], [t].[DeliveryInstructions], [t].[DiscountAmount], [t].[FulfillmentType], [t].[GuestSessionId], [t].[IsDeleted], [t].[IsWalkIn], [t].[OrderDate], [t].[OrderNumber], [t].[OrderStatus], [t].[POSSessionId], [t].[PaymentMethod], [t].[ShippingAddressId], [t].[ShippingFee], [t].[SubTotal], [t].[TotalAmount], [t].[UpdatedAt], [t].[UserId], [t0].[UserId], [t0].[CreatedAt], [t0].[DefaultAddressId], [t0].[Email], [t0].[FailedLoginAttempts], [t0].[FirstName], [t0].[IsActive], [t0].[IsDeleted], [t0].[IsWalkIn], [t0].[LastLoginAt], [t0].[LastName], [t0].[LockoutUntil], [t0].[PasswordHash], [t0].[PhoneNumber]
      FROM [Delivery] AS [d]
      LEFT JOIN [LalamoveDelivery] AS [l] ON [d].[DeliveryId] = [l].[DeliveryId]
      LEFT JOIN [LBCDelivery] AS [l0] ON [d].[DeliveryId] = [l0].[DeliveryId]
      INNER JOIN (
          SELECT [o].[OrderId], [o].[CartId], [o].[ContactPhone], [o].[CreatedAt], [o].[DeliveryInstructions], [o].[DiscountAmount], [o].[FulfillmentType], [o].[GuestSessionId], [o].[IsDeleted], [o].[IsWalkIn], [o].[OrderDate], [o].[OrderNumber], [o].[OrderStatus], [o].[POSSessionId], [o].[PaymentMethod], [o].[ShippingAddressId], [o].[ShippingFee], [o].[SubTotal], [o].[TotalAmount], [o].[UpdatedAt], [o].[UserId]
          FROM [Order] AS [o]
          WHERE [o].[IsDeleted] = CAST(0 AS bit)
      ) AS [t] ON [d].[OrderId] = [t].[OrderId]
      INNER JOIN (
          SELECT [u].[UserId], [u].[CreatedAt], [u].[DefaultAddressId], [u].[Email], [u].[FailedLoginAttempts], [u].[FirstName], [u].[IsActive], [u].[IsDeleted], [u].[IsWalkIn], [u].[LastLoginAt], [u].[LastName], [u].[LockoutUntil], [u].[PasswordHash], [u].[PhoneNumber]
          FROM [User] AS [u]
          WHERE [u].[IsDeleted] = CAST(0 AS bit)
      ) AS [t0] ON [t].[UserId] = [t0].[UserId]
      WHERE [d].[DeliveryStatus] IN (N'PickedUp', N'InTransit')
info: Microsoft.EntityFrameworkCore.Database.Command[20101]
      Executed DbCommand (121ms) [Parameters=[@__orderId_0='?' (DbType = Int32)], CommandType='Text', CommandTimeout='30']
      SELECT [t0].[DeliveryId], [t0].[ActualDeliveryTime], [t0].[Courier], [t0].[CreatedAt], [t0].[DelayedUntil], [t0].[DeliveryStatus], [t0].[EstimatedDeliveryTime], [t0].[IsDelayed], [t0].[OrderId], [t0].[DeliveryId0], [t0].[BookingRef], [t0].[DriverName], [t0].[DriverPhone], [t0].[DeliveryId1], [t0].[TrackingNumber], [t].[OrderId], [t].[PickupOrderId], [t].[AddressId]
      FROM (
          SELECT TOP(1) [o].[OrderId], [p].[PickupOrderId], [a].[AddressId]
          FROM [Order] AS [o]
          LEFT JOIN [PickupOrder] AS [p] ON [o].[OrderId] = [p].[OrderId]
          LEFT JOIN [Address] AS [a] ON [o].[ShippingAddressId] = [a].[AddressId]
          WHERE [o].[IsDeleted] = CAST(0 AS bit) AND [o].[OrderId] = @__orderId_0
      ) AS [t]
      INNER JOIN (
          SELECT [d].[DeliveryId], [d].[ActualDeliveryTime], [d].[Courier], [d].[CreatedAt], [d].[DelayedUntil], [d].[DeliveryStatus], [d].[EstimatedDeliveryTime], [d].[IsDelayed], [d].[OrderId], [l].[DeliveryId] AS [DeliveryId0], [l].[BookingRef], [l].[DriverName], [l].[DriverPhone], [l0].[DeliveryId] AS [DeliveryId1], [l0].[TrackingNumber]
          FROM [Delivery] AS [d]
          LEFT JOIN [LalamoveDelivery] AS [l] ON [d].[DeliveryId] = [l].[DeliveryId]
          LEFT JOIN [LBCDelivery] AS [l0] ON [d].[DeliveryId] = [l0].[DeliveryId]
      ) AS [t0] ON [t].[OrderId] = [t0].[OrderId]
      ORDER BY [t].[OrderId], [t].[PickupOrderId], [t].[AddressId]
info: Microsoft.EntityFrameworkCore.Database.Command[20101]
      Executed DbCommand (97ms) [Parameters=[@p0='?' (DbType = DateTime2), @p1='?' (Size = 1000), @p2='?' (Size = 100), @p3='?' (DbType = Int32)], CommandType='Text', CommandTimeout='30']
      SET IMPLICIT_TRANSACTIONS OFF;
      SET NOCOUNT ON;
      INSERT INTO [SystemLog] ([CreatedAt], [EventDescription], [EventType], [UserId])
      OUTPUT INSERTED.[SystemLogId]
      VALUES (@p0, @p1, @p2, @p3);
info: Microsoft.EntityFrameworkCore.Database.Command[20101]
      Executed DbCommand (107ms) [Parameters=[@__paymentMethod_0='?' (Size = 50)], CommandType='Text', CommandTimeout='30']
      SELECT TOP(1) [s].[StorePaymentAccountId], [s].[AccountName], [s].[AccountNumber], [s].[BankName], [s].[CreatedAt], [s].[DisplayOrder], [s].[Instructions], [s].[IsActive], [s].[PaymentMethod], [s].[QrImageUrl], [s].[UpdatedAt]
      FROM [StorePaymentAccount] AS [s]
      WHERE [s].[PaymentMethod] = @__paymentMethod_0 AND [s].[IsActive] = CAST(1 AS bit)
info: Microsoft.AspNetCore.Mvc.ViewFeatures.ViewResultExecutor[1]
      Executing ViewResult, running view ~/Views/Customer/Payment.cshtml.
dbug: Microsoft.AspNetCore.Watch.BrowserRefresh.BrowserRefreshMiddleware[1]
      Response markup is scheduled to include browser refresh script injection.
info: Microsoft.AspNetCore.Mvc.ViewFeatures.ViewResultExecutor[4]
      Executed ViewResult - view ~/Views/Customer/Payment.cshtml executed in 25.0184ms.
info: Microsoft.AspNetCore.Mvc.Infrastructure.ControllerActionInvoker[105]
      Executed action WebApplication.Controllers.PaymentController.Submit (WebApplication) in 721.1467ms
info: Microsoft.AspNetCore.Routing.EndpointMiddleware[1]
      Executed endpoint 'WebApplication.Controllers.PaymentController.Submit (WebApplication)'
dbug: Microsoft.AspNetCore.Watch.BrowserRefresh.BrowserRefreshMiddleware[2]
      Response markup was updated to include browser refresh script injection.
info: Microsoft.AspNetCore.Hosting.Diagnostics[2]
      Request finished HTTP/2 GET https://localhost:7177/Payment/Submit?orderId=16 - 200 - text/html;+charset=utf-8 739.0538ms
info: Microsoft.AspNetCore.Hosting.Diagnostics[1]
      Request starting HTTP/2 GET https://localhost:7177/lib/jquery-validation/dist/jquery.validate.min.js - - -
info: Microsoft.AspNetCore.Hosting.Diagnostics[1]
      Request starting HTTP/2 GET https://localhost:7177/lib/jquery-validation-unobtrusive/dist/jquery.validate.unobtrusive.min.js - - -
info: Microsoft.AspNetCore.Hosting.Diagnostics[1]
      Request starting HTTP/2 GET https://localhost:7177/_framework/aspnetcore-browser-refresh.js - - -
info: Microsoft.AspNetCore.Hosting.Diagnostics[1]
      Request starting HTTP/2 GET https://localhost:7177/_vs/browserLink - - -
info: Microsoft.AspNetCore.StaticFiles.StaticFileMiddleware[6]
      The file /lib/jquery-validation/dist/jquery.validate.min.js was not modified
info: Microsoft.AspNetCore.StaticFiles.StaticFileMiddleware[6]
      The file /lib/jquery-validation-unobtrusive/dist/jquery.validate.unobtrusive.min.js was not modified
info: Microsoft.AspNetCore.Hosting.Diagnostics[2]
      Request finished HTTP/2 GET https://localhost:7177/lib/jquery-validation-unobtrusive/dist/jquery.validate.unobtrusive.min.js - 304 - text/javascript 9.0792ms
dbug: Microsoft.AspNetCore.Watch.BrowserRefresh.BrowserScriptMiddleware[0]
      Script injected: /_framework/aspnetcore-browser-refresh.js
info: Microsoft.AspNetCore.Hosting.Diagnostics[2]
      Request finished HTTP/2 GET https://localhost:7177/lib/jquery-validation/dist/jquery.validate.min.js - 304 - text/javascript 11.7155ms
info: Microsoft.AspNetCore.Hosting.Diagnostics[2]
      Request finished HTTP/2 GET https://localhost:7177/_framework/aspnetcore-browser-refresh.js - 200 16475 application/javascript;+charset=utf-8 15.0273ms
info: Microsoft.AspNetCore.Hosting.Diagnostics[1]
      Request starting HTTP/2 GET https://localhost:7177/Customer/NotificationCount - - -
info: Microsoft.AspNetCore.Hosting.Diagnostics[1]
      Request starting HTTP/2 GET https://localhost:7177/Cart/GetCartCount - - -
info: Microsoft.AspNetCore.Routing.EndpointMiddleware[0]
      Executing endpoint 'WebApplication.Controllers.CustomerController.NotificationCount (WebApplication)'
info: Microsoft.AspNetCore.Routing.EndpointMiddleware[0]
      Executing endpoint 'WebApplication.Controllers.CartController.GetCartCount (WebApplication)'
info: Microsoft.AspNetCore.Mvc.Infrastructure.ControllerActionInvoker[102]
      Route matched with {action = "NotificationCount", controller = "Customer"}. Executing controller action with signature System.Threading.Tasks.Task`1[Microsoft.AspNetCore.Mvc.IActionResult] NotificationCount(System.Threading.CancellationToken) on controller WebApplication.Controllers.CustomerController (WebApplication).
info: Microsoft.AspNetCore.Mvc.Infrastructure.ControllerActionInvoker[102]
      Route matched with {action = "GetCartCount", controller = "Cart"}. Executing controller action with signature System.Threading.Tasks.Task`1[Microsoft.AspNetCore.Mvc.IActionResult] GetCartCount(System.Threading.CancellationToken) on controller WebApplication.Controllers.CartController (WebApplication).
info: Microsoft.AspNetCore.Hosting.Diagnostics[2]
      Request finished HTTP/2 GET https://localhost:7177/_vs/browserLink - 200 - text/javascript;+charset=UTF-8 54.5110ms
info: Microsoft.EntityFrameworkCore.Database.Command[20101]
      Executed DbCommand (94ms) [Parameters=[@__userId_Value_0='?' (DbType = Int32)], CommandType='Text', CommandTimeout='30']
      SELECT TOP(1) [c].[CartId], [c].[CreatedAt], [c].[GuestSessionId], [c].[IsCheckedOut], [c].[LastUpdatedAt], [c].[UserId]
      FROM [Cart] AS [c]
      WHERE [c].[IsCheckedOut] = CAST(0 AS bit) AND [c].[UserId] = @__userId_Value_0
      ORDER BY [c].[CartId]
info: Microsoft.AspNetCore.Mvc.Infrastructure.SystemTextJsonResultExecutor[1]
      Executing JsonResult, writing value of type 'WebApplication.Models.ApiResponse'.
info: Microsoft.AspNetCore.Mvc.Infrastructure.ControllerActionInvoker[105]
      Executed action WebApplication.Controllers.CartController.GetCartCount (WebApplication) in 98.9049ms
info: Microsoft.AspNetCore.Routing.EndpointMiddleware[1]
      Executed endpoint 'WebApplication.Controllers.CartController.GetCartCount (WebApplication)'
info: Microsoft.AspNetCore.Hosting.Diagnostics[2]
      Request finished HTTP/2 GET https://localhost:7177/Cart/GetCartCount - 200 - application/json;+charset=utf-8 114.5170ms
info: Microsoft.EntityFrameworkCore.Database.Command[20101]
      Executed DbCommand (127ms) [Parameters=[@__userId_0='?' (DbType = Int32)], CommandType='Text', CommandTimeout='30']
      SELECT COUNT(*)
      FROM [Notification] AS [n]
      WHERE [n].[UserId] = @__userId_0 AND [n].[IsRead] = CAST(0 AS bit) AND [n].[Channel] = N'InApp'
info: Microsoft.AspNetCore.Mvc.Infrastructure.SystemTextJsonResultExecutor[1]
      Executing JsonResult, writing value of type '<>f__AnonymousType6`1[[System.Int32, System.Private.CoreLib, Version=8.0.0.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e]]'.
info: Microsoft.AspNetCore.Mvc.Infrastructure.ControllerActionInvoker[105]
      Executed action WebApplication.Controllers.CustomerController.NotificationCount (WebApplication) in 133.3089ms
info: Microsoft.AspNetCore.Routing.EndpointMiddleware[1]
      Executed endpoint 'WebApplication.Controllers.CustomerController.NotificationCount (WebApplication)'
info: Microsoft.AspNetCore.Hosting.Diagnostics[2]
      Request finished HTTP/2 GET https://localhost:7177/Customer/NotificationCount - 200 - application/json;+charset=utf-8 146.8684ms
info: Microsoft.EntityFrameworkCore.Database.Command[20101]
      Executed DbCommand (96ms) [Parameters=[@__p_0='?' (DbType = Int32)], CommandType='Text', CommandTimeout='30']
      SELECT TOP(@__p_0) [n].[NotificationId], [n].[Body], [n].[Channel], [n].[CreatedAt], [n].[FailureReason], [n].[IsRead], [n].[NotifType], [n].[OrderId], [n].[ReadAt], [n].[Recipient], [n].[RetryCount], [n].[SentAt], [n].[Status], [n].[Subject], [n].[TicketId], [n].[UserId]
      FROM [Notification] AS [n]
      WHERE [n].[Status] = N'Pending' AND [n].[RetryCount] < 3 AND [n].[Channel] = N'Email'
      ORDER BY [n].[CreatedAt]

C:\Users\Brian\OOP-TaurusBikeShop\WebApplication\bin\Debug\net8.0\WebApplication.exe (process 28192) exited with code -1 (0xffffffff).
To automatically close the console when debugging stops, enable Tools->Options->Debugging->Automatically close the console when debugging stops.
Press any key to close this window . . .