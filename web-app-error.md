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
info: Microsoft.AspNetCore.Hosting.Diagnostics[1]
      Request starting HTTP/2 GET https://localhost:7177/_framework/aspnetcore-browser-refresh.js - - -
info: Microsoft.AspNetCore.Hosting.Diagnostics[2]
      Request finished HTTP/2 GET https://localhost:7177/_framework/aspnetcore-browser-refresh.js - 200 14933 application/javascript;+charset=utf-8 9.0608ms
info: Microsoft.AspNetCore.Hosting.Diagnostics[1]
      Request starting HTTP/2 GET https://localhost:7177/antiforgery/token - - -
info: Microsoft.AspNetCore.Routing.EndpointMiddleware[0]
      Executing endpoint 'HTTP: GET /antiforgery/token'
info: Microsoft.AspNetCore.Http.Result.OkObjectResult[1]
      Setting HTTP status code 200.
info: Microsoft.AspNetCore.Http.Result.OkObjectResult[3]
      Writing value of type '<>f__AnonymousType21`1' as Json.
info: Microsoft.AspNetCore.Routing.EndpointMiddleware[1]
      Executed endpoint 'HTTP: GET /antiforgery/token'
info: Microsoft.AspNetCore.Hosting.Diagnostics[2]
      Request finished HTTP/2 GET https://localhost:7177/antiforgery/token - 200 - application/json;+charset=utf-8 32.7995ms
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
      Executed DbCommand (82ms) [Parameters=[], CommandType='Text', CommandTimeout='30']
      SELECT COUNT(*)
      FROM [ProductVariant] AS [p]
info: Microsoft.EntityFrameworkCore.Database.Command[20101]
      Executed DbCommand (75ms) [Parameters=[@p0='?' (DbType = DateTime2), @p1='?' (Size = 1000), @p2='?' (Size = 100), @p3='?' (DbType = Int32), @p4='?' (DbType = DateTime2), @p5='?' (Size = 1000), @p6='?' (Size = 100), @p7='?' (DbType = Int32)], CommandType='Text', CommandTimeout='30']
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
      Executed DbCommand (59ms) [Parameters=[@__p_0='?' (DbType = Int32)], CommandType='Text', CommandTimeout='30']
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
      Executed DbCommand (58ms) [Parameters=[@__p_0='?' (DbType = Int32)], CommandType='Text', CommandTimeout='30']
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
      Executed DbCommand (58ms) [Parameters=[@__p_0='?' (DbType = Int32)], CommandType='Text', CommandTimeout='30']
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
info: Microsoft.AspNetCore.Mvc.ViewFeatures.ViewResultExecutor[4]
      Executed ViewResult - view Index executed in 95.2647ms.
info: Microsoft.AspNetCore.Mvc.Infrastructure.ControllerActionInvoker[105]
      Executed action WebApplication.Controllers.HomeController.Index (WebApplication) in 2126.1324ms
info: Microsoft.AspNetCore.Routing.EndpointMiddleware[1]
      Executed endpoint 'WebApplication.Controllers.HomeController.Index (WebApplication)'
info: Microsoft.AspNetCore.Hosting.Diagnostics[2]
      Request finished HTTP/2 GET https://localhost:7177/ - 200 - text/html;+charset=utf-8 2245.1167ms
info: Microsoft.AspNetCore.Hosting.Diagnostics[1]
      Request starting HTTP/2 GET https://localhost:7177/_framework/aspnetcore-browser-refresh.js - - -
info: Microsoft.AspNetCore.Hosting.Diagnostics[1]
      Request starting HTTP/2 GET https://localhost:7177/_vs/browserLink - - -
info: Microsoft.AspNetCore.Hosting.Diagnostics[1]
      Request starting HTTP/2 GET https://localhost:7177/_framework/aspnetcore-browser-refresh.js - - -
info: Microsoft.AspNetCore.Hosting.Diagnostics[2]
      Request finished HTTP/2 GET https://localhost:7177/_framework/aspnetcore-browser-refresh.js - 200 14933 application/javascript;+charset=utf-8 6.0657ms
info: Microsoft.AspNetCore.Hosting.Diagnostics[2]
      Request finished HTTP/2 GET https://localhost:7177/_framework/aspnetcore-browser-refresh.js - 200 14933 application/javascript;+charset=utf-8 8.8486ms
info: Microsoft.AspNetCore.Hosting.Diagnostics[1]
      Request starting HTTP/2 GET https://localhost:7177/_framework/aspnetcore-browser-refresh.js - - -
info: Microsoft.AspNetCore.Hosting.Diagnostics[1]
      Request starting HTTP/2 GET https://localhost:7177/antiforgery/token - - -
info: Microsoft.AspNetCore.Hosting.Diagnostics[1]
      Request starting HTTP/2 GET https://localhost:7177/Cart/GetCartCount - - -
info: Microsoft.AspNetCore.Hosting.Diagnostics[2]
      Request finished HTTP/2 GET https://localhost:7177/_framework/aspnetcore-browser-refresh.js - 200 14933 application/javascript;+charset=utf-8 3.0213ms
info: Microsoft.AspNetCore.Hosting.Diagnostics[1]
      Request starting HTTP/2 GET https://localhost:7177/_framework/aspnetcore-browser-refresh.js - - -
info: Microsoft.AspNetCore.Hosting.Diagnostics[2]
      Request finished HTTP/2 GET https://localhost:7177/_framework/aspnetcore-browser-refresh.js - 200 14933 application/javascript;+charset=utf-8 16.8865ms
info: Microsoft.AspNetCore.Routing.EndpointMiddleware[0]
      Executing endpoint 'WebApplication.Controllers.CartController.GetCartCount (WebApplication)'
info: Microsoft.AspNetCore.Routing.EndpointMiddleware[0]
      Executing endpoint 'HTTP: GET /antiforgery/token'
info: Microsoft.AspNetCore.Http.Result.OkObjectResult[1]
      Setting HTTP status code 200.
info: Microsoft.AspNetCore.Http.Result.OkObjectResult[3]
      Writing value of type '<>f__AnonymousType21`1' as Json.
info: Microsoft.AspNetCore.Routing.EndpointMiddleware[1]
      Executed endpoint 'HTTP: GET /antiforgery/token'
info: Microsoft.AspNetCore.Hosting.Diagnostics[2]
      Request finished HTTP/2 GET https://localhost:7177/antiforgery/token - 200 - application/json;+charset=utf-8 35.7939ms
info: Microsoft.AspNetCore.Mvc.Infrastructure.ControllerActionInvoker[102]
      Route matched with {action = "GetCartCount", controller = "Cart"}. Executing controller action with signature System.Threading.Tasks.Task`1[Microsoft.AspNetCore.Mvc.IActionResult] GetCartCount(System.Threading.CancellationToken) on controller WebApplication.Controllers.CartController (WebApplication).
info: Microsoft.AspNetCore.Hosting.Diagnostics[1]
      Request starting HTTP/2 GET https://localhost:7177/antiforgery/token - - -
info: Microsoft.AspNetCore.Routing.EndpointMiddleware[0]
      Executing endpoint 'HTTP: GET /antiforgery/token'
info: Microsoft.AspNetCore.Http.Result.OkObjectResult[1]
      Setting HTTP status code 200.
info: Microsoft.AspNetCore.Http.Result.OkObjectResult[3]
      Writing value of type '<>f__AnonymousType21`1' as Json.
info: Microsoft.AspNetCore.Routing.EndpointMiddleware[1]
      Executed endpoint 'HTTP: GET /antiforgery/token'
info: Microsoft.AspNetCore.Hosting.Diagnostics[2]
      Request finished HTTP/2 GET https://localhost:7177/antiforgery/token - 200 - application/json;+charset=utf-8 10.6158ms
info: Microsoft.AspNetCore.Hosting.Diagnostics[1]
      Request starting HTTP/2 GET https://localhost:7177/antiforgery/token - - -
info: Microsoft.AspNetCore.Routing.EndpointMiddleware[0]
      Executing endpoint 'HTTP: GET /antiforgery/token'
info: Microsoft.AspNetCore.Http.Result.OkObjectResult[1]
      Setting HTTP status code 200.
info: Microsoft.AspNetCore.Http.Result.OkObjectResult[3]
      Writing value of type '<>f__AnonymousType21`1' as Json.
info: Microsoft.AspNetCore.Routing.EndpointMiddleware[1]
      Executed endpoint 'HTTP: GET /antiforgery/token'
info: Microsoft.AspNetCore.Hosting.Diagnostics[2]
      Request finished HTTP/2 GET https://localhost:7177/antiforgery/token - 200 - application/json;+charset=utf-8 10.6000ms
info: Microsoft.AspNetCore.Mvc.Infrastructure.SystemTextJsonResultExecutor[1]
      Executing JsonResult, writing value of type 'WebApplication.Models.ApiResponse'.
info: Microsoft.AspNetCore.Mvc.Infrastructure.ControllerActionInvoker[105]
      Executed action WebApplication.Controllers.CartController.GetCartCount (WebApplication) in 44.2903ms
info: Microsoft.AspNetCore.Routing.EndpointMiddleware[1]
      Executed endpoint 'WebApplication.Controllers.CartController.GetCartCount (WebApplication)'
info: Microsoft.AspNetCore.Hosting.Diagnostics[2]
      Request finished HTTP/2 GET https://localhost:7177/_vs/browserLink - 200 - text/javascript;+charset=UTF-8 105.9554ms
info: Microsoft.AspNetCore.Hosting.Diagnostics[2]
      Request finished HTTP/2 GET https://localhost:7177/Cart/GetCartCount - 200 - application/json;+charset=utf-8 90.6923ms
info: Microsoft.AspNetCore.Hosting.Diagnostics[1]
      Request starting HTTP/2 GET https://localhost:7177/antiforgery/token - - -
info: Microsoft.AspNetCore.Routing.EndpointMiddleware[0]
      Executing endpoint 'HTTP: GET /antiforgery/token'
info: Microsoft.AspNetCore.Http.Result.OkObjectResult[1]
      Setting HTTP status code 200.
info: Microsoft.AspNetCore.Http.Result.OkObjectResult[3]
      Writing value of type '<>f__AnonymousType21`1' as Json.
info: Microsoft.AspNetCore.Routing.EndpointMiddleware[1]
      Executed endpoint 'HTTP: GET /antiforgery/token'
info: Microsoft.AspNetCore.Hosting.Diagnostics[2]
      Request finished HTTP/2 GET https://localhost:7177/antiforgery/token - 200 - application/json;+charset=utf-8 10.2645ms
info: Microsoft.AspNetCore.Hosting.Diagnostics[1]
      Request starting HTTP/2 GET https://localhost:7177/_framework/aspnetcore-browser-refresh.js - - -
info: Microsoft.AspNetCore.Hosting.Diagnostics[2]
      Request finished HTTP/2 GET https://localhost:7177/_framework/aspnetcore-browser-refresh.js - 200 14933 application/javascript;+charset=utf-8 2.7835ms
info: Microsoft.AspNetCore.Hosting.Diagnostics[1]
      Request starting HTTP/2 GET https://localhost:7177/antiforgery/token - - -
info: Microsoft.AspNetCore.Routing.EndpointMiddleware[0]
      Executing endpoint 'HTTP: GET /antiforgery/token'
info: Microsoft.AspNetCore.Http.Result.OkObjectResult[1]
      Setting HTTP status code 200.
info: Microsoft.AspNetCore.Http.Result.OkObjectResult[3]
      Writing value of type '<>f__AnonymousType21`1' as Json.
info: Microsoft.AspNetCore.Routing.EndpointMiddleware[1]
      Executed endpoint 'HTTP: GET /antiforgery/token'
info: Microsoft.AspNetCore.Hosting.Diagnostics[2]
      Request finished HTTP/2 GET https://localhost:7177/antiforgery/token - 200 - application/json;+charset=utf-8 10.4536ms
info: Microsoft.EntityFrameworkCore.Database.Command[20101]
      Executed DbCommand (59ms) [Parameters=[@p0='?' (DbType = DateTime2), @p1='?' (Size = 1000), @p2='?' (Size = 100), @p3='?' (DbType = Int32)], CommandType='Text', CommandTimeout='30']
      SET IMPLICIT_TRANSACTIONS OFF;
      SET NOCOUNT ON;
      INSERT INTO [SystemLog] ([CreatedAt], [EventDescription], [EventType], [UserId])
      OUTPUT INSERTED.[SystemLogId]
      VALUES (@p0, @p1, @p2, @p3);
info: Microsoft.EntityFrameworkCore.Database.Command[20101]
      Executed DbCommand (57ms) [Parameters=[], CommandType='Text', CommandTimeout='30']
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
      Executed DbCommand (57ms) [Parameters=[@__8__locals1_cooldownStart_0='?' (DbType = DateTime2), @__Format_2='?' (Size = 1000)], CommandType='Text', CommandTimeout='30']
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
      StockMonitorJob: low stock - variant 4 (Ryder X3 27.5 2021 MT200 Hydro Brakes (3x8) / Default): 4 remaining (threshold 5).
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
      StockMonitorJob: low stock - variant 8 (Kespor Stork Feather CX 1.0 2022 / Default): 2 remaining (threshold 5).
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
      StockMonitorJob: low stock - variant 9 (Pinewood Lancer 1.0 2022 Gravel RX (2x9) / Default): 4 remaining (threshold 5).
info: Microsoft.EntityFrameworkCore.Database.Command[20101]
      Executed DbCommand (57ms) [Parameters=[@__8__locals1_cooldownStart_0='?' (DbType = DateTime2), @__Format_2='?' (Size = 1000)], CommandType='Text', CommandTimeout='30']
      SELECT CASE
          WHEN EXISTS (
              SELECT 1
              FROM [SystemLog] AS [s]
              WHERE [s].[EventType] = N'LowStockTriggered' AND [s].[CreatedAt] > @__8__locals1_cooldownStart_0 AND [s].[EventDescription] LIKE @__Format_2) THEN CAST(1 AS bit)
          ELSE CAST(0 AS bit)
      END
info: Microsoft.EntityFrameworkCore.Database.Command[20101]
      Executed DbCommand (58ms) [Parameters=[@p0='?' (DbType = DateTime2), @p1='?' (Size = 1000), @p2='?' (Size = 100), @p3='?' (DbType = Int32)], CommandType='Text', CommandTimeout='30']
      SET IMPLICIT_TRANSACTIONS OFF;
      SET NOCOUNT ON;
      INSERT INTO [SystemLog] ([CreatedAt], [EventDescription], [EventType], [UserId])
      OUTPUT INSERTED.[SystemLogId]
      VALUES (@p0, @p1, @p2, @p3);
info: Microsoft.EntityFrameworkCore.Database.Command[20101]
      Executed DbCommand (58ms) [Parameters=[@p0='?' (DbType = DateTime2), @p1='?' (Size = 1000), @p2='?' (Size = 100), @p3='?' (DbType = Int32)], CommandType='Text', CommandTimeout='30']
      SET IMPLICIT_TRANSACTIONS OFF;
      SET NOCOUNT ON;
      INSERT INTO [SystemLog] ([CreatedAt], [EventDescription], [EventType], [UserId])
      OUTPUT INSERTED.[SystemLogId]
      VALUES (@p0, @p1, @p2, @p3);
info: Microsoft.EntityFrameworkCore.Database.Command[20101]
      Executed DbCommand (57ms) [Parameters=[@__now_0='?' (DbType = DateTime2)], CommandType='Text', CommandTimeout='30']
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
      Executed DbCommand (58ms) [Parameters=[@p0='?' (DbType = DateTime2), @p1='?' (Size = 1000), @p2='?' (Size = 100), @p3='?' (DbType = Int32)], CommandType='Text', CommandTimeout='30']
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
      Executed DbCommand (57ms) [Parameters=[@__staleThreshold_0='?' (DbType = DateTime2)], CommandType='Text', CommandTimeout='30']
      SELECT [o].[OrderId], [o].[OrderNumber]
      FROM [Order] AS [o]
      WHERE [o].[IsDeleted] = CAST(0 AS bit) AND [o].[OrderStatus] = N'Pending' AND [o].[OrderDate] < @__staleThreshold_0
info: Microsoft.EntityFrameworkCore.Database.Command[20101]
      Executed DbCommand (59ms) [Parameters=[@p0='?' (DbType = DateTime2), @p1='?' (Size = 1000), @p2='?' (Size = 100), @p3='?' (DbType = Int32)], CommandType='Text', CommandTimeout='30']
      SET IMPLICIT_TRANSACTIONS OFF;
      SET NOCOUNT ON;
      INSERT INTO [SystemLog] ([CreatedAt], [EventDescription], [EventType], [UserId])
      OUTPUT INSERTED.[SystemLogId]
      VALUES (@p0, @p1, @p2, @p3);
info: Microsoft.AspNetCore.Hosting.Diagnostics[1]
      Request starting HTTP/2 GET https://localhost:7177/ - - -
info: Microsoft.AspNetCore.Authentication.Cookies.CookieAuthenticationHandler[7]
      Cookies was not authenticated. Failure message: Ticket expired
info: Microsoft.AspNetCore.Routing.EndpointMiddleware[0]
      Executing endpoint 'WebApplication.Controllers.HomeController.Index (WebApplication)'
info: Microsoft.AspNetCore.Mvc.Infrastructure.ControllerActionInvoker[102]
      Route matched with {action = "Index", controller = "Home"}. Executing controller action with signature System.Threading.Tasks.Task`1[Microsoft.AspNetCore.Mvc.IActionResult] Index(System.Threading.CancellationToken) on controller WebApplication.Controllers.HomeController (WebApplication).
info: Microsoft.EntityFrameworkCore.Database.Command[20101]
      Executed DbCommand (58ms) [Parameters=[@__p_0='?' (DbType = Int32)], CommandType='Text', CommandTimeout='30']
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
      Executed DbCommand (58ms) [Parameters=[@__p_0='?' (DbType = Int32)], CommandType='Text', CommandTimeout='30']
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
      Executed DbCommand (58ms) [Parameters=[@__p_0='?' (DbType = Int32)], CommandType='Text', CommandTimeout='30']
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
info: Microsoft.AspNetCore.Mvc.ViewFeatures.ViewResultExecutor[4]
      Executed ViewResult - view Index executed in 11.5515ms.
info: Microsoft.AspNetCore.Mvc.Infrastructure.ControllerActionInvoker[105]
      Executed action WebApplication.Controllers.HomeController.Index (WebApplication) in 195.4349ms
info: Microsoft.AspNetCore.Routing.EndpointMiddleware[1]
      Executed endpoint 'WebApplication.Controllers.HomeController.Index (WebApplication)'
info: Microsoft.AspNetCore.Hosting.Diagnostics[1]
      Request starting HTTP/2 GET https://localhost:7177/_framework/aspnetcore-browser-refresh.js - - -
info: Microsoft.AspNetCore.Hosting.Diagnostics[1]
      Request starting HTTP/2 GET https://localhost:7177/_vs/browserLink - - -
info: Microsoft.AspNetCore.Hosting.Diagnostics[2]
      Request finished HTTP/2 GET https://localhost:7177/ - 200 - text/html;+charset=utf-8 212.2239ms
info: Microsoft.AspNetCore.Hosting.Diagnostics[2]
      Request finished HTTP/2 GET https://localhost:7177/_framework/aspnetcore-browser-refresh.js - 200 14933 application/javascript;+charset=utf-8 5.2621ms
info: Microsoft.AspNetCore.Hosting.Diagnostics[2]
      Request finished HTTP/2 GET https://localhost:7177/_vs/browserLink - 200 - text/javascript;+charset=UTF-8 20.9209ms
info: Microsoft.AspNetCore.Hosting.Diagnostics[1]
      Request starting HTTP/2 GET https://localhost:7177/antiforgery/token - - -
info: Microsoft.AspNetCore.Hosting.Diagnostics[1]
      Request starting HTTP/2 GET https://localhost:7177/Cart/GetCartCount - - -
info: Microsoft.AspNetCore.Authentication.Cookies.CookieAuthenticationHandler[7]
      Cookies was not authenticated. Failure message: Ticket expired
info: Microsoft.AspNetCore.Authentication.Cookies.CookieAuthenticationHandler[7]
      Cookies was not authenticated. Failure message: Ticket expired
info: Microsoft.AspNetCore.Routing.EndpointMiddleware[0]
      Executing endpoint 'HTTP: GET /antiforgery/token'
info: Microsoft.AspNetCore.Routing.EndpointMiddleware[0]
      Executing endpoint 'WebApplication.Controllers.CartController.GetCartCount (WebApplication)'
info: Microsoft.AspNetCore.Http.Result.OkObjectResult[1]
      Setting HTTP status code 200.
info: Microsoft.AspNetCore.Mvc.Infrastructure.ControllerActionInvoker[102]
      Route matched with {action = "GetCartCount", controller = "Cart"}. Executing controller action with signature System.Threading.Tasks.Task`1[Microsoft.AspNetCore.Mvc.IActionResult] GetCartCount(System.Threading.CancellationToken) on controller WebApplication.Controllers.CartController (WebApplication).
info: Microsoft.AspNetCore.Http.Result.OkObjectResult[3]
      Writing value of type '<>f__AnonymousType21`1' as Json.
info: Microsoft.AspNetCore.Routing.EndpointMiddleware[1]
      Executed endpoint 'HTTP: GET /antiforgery/token'
info: Microsoft.AspNetCore.Hosting.Diagnostics[2]
      Request finished HTTP/2 GET https://localhost:7177/antiforgery/token - 200 - application/json;+charset=utf-8 16.5386ms
info: Microsoft.EntityFrameworkCore.Database.Command[20101]
      Executed DbCommand (56ms) [Parameters=[@__guestSessionId_Value_0='?' (DbType = Int32)], CommandType='Text', CommandTimeout='30']
      SELECT TOP(1) [c].[CartId], [c].[CreatedAt], [c].[GuestSessionId], [c].[IsCheckedOut], [c].[LastUpdatedAt], [c].[UserId]
      FROM [Cart] AS [c]
      WHERE [c].[IsCheckedOut] = CAST(0 AS bit) AND [c].[GuestSessionId] = @__guestSessionId_Value_0
      ORDER BY [c].[CartId]
info: Microsoft.AspNetCore.Mvc.Infrastructure.SystemTextJsonResultExecutor[1]
      Executing JsonResult, writing value of type 'WebApplication.Models.ApiResponse'.
info: Microsoft.AspNetCore.Mvc.Infrastructure.ControllerActionInvoker[105]
      Executed action WebApplication.Controllers.CartController.GetCartCount (WebApplication) in 93.2661ms
info: Microsoft.AspNetCore.Routing.EndpointMiddleware[1]
      Executed endpoint 'WebApplication.Controllers.CartController.GetCartCount (WebApplication)'
info: Microsoft.AspNetCore.Hosting.Diagnostics[2]
      Request finished HTTP/2 GET https://localhost:7177/Cart/GetCartCount - 200 - application/json;+charset=utf-8 109.6857ms
info: Microsoft.AspNetCore.Hosting.Diagnostics[1]
      Request starting HTTP/2 GET https://localhost:7177/Customer/Login - - -
info: Microsoft.AspNetCore.Authentication.Cookies.CookieAuthenticationHandler[7]
      Cookies was not authenticated. Failure message: Ticket expired
info: Microsoft.AspNetCore.Routing.EndpointMiddleware[0]
      Executing endpoint 'WebApplication.Controllers.CustomerController.Login (WebApplication)'
info: Microsoft.AspNetCore.Mvc.Infrastructure.ControllerActionInvoker[102]
      Route matched with {action = "Login", controller = "Customer"}. Executing controller action with signature Microsoft.AspNetCore.Mvc.IActionResult Login(System.String) on controller WebApplication.Controllers.CustomerController (WebApplication).
info: Microsoft.AspNetCore.Mvc.ViewFeatures.ViewResultExecutor[1]
      Executing ViewResult, running view Login.
info: Microsoft.AspNetCore.Mvc.ViewFeatures.ViewResultExecutor[4]
      Executed ViewResult - view Login executed in 25.4183ms.
info: Microsoft.AspNetCore.Mvc.Infrastructure.ControllerActionInvoker[105]
      Executed action WebApplication.Controllers.CustomerController.Login (WebApplication) in 32.9216ms
info: Microsoft.AspNetCore.Routing.EndpointMiddleware[1]
      Executed endpoint 'WebApplication.Controllers.CustomerController.Login (WebApplication)'
info: Microsoft.AspNetCore.Hosting.Diagnostics[2]
      Request finished HTTP/2 GET https://localhost:7177/Customer/Login - 200 - text/html;+charset=utf-8 46.8364ms
info: Microsoft.AspNetCore.Hosting.Diagnostics[1]
      Request starting HTTP/2 GET https://localhost:7177/_vs/browserLink - - -
info: Microsoft.AspNetCore.Hosting.Diagnostics[1]
      Request starting HTTP/2 GET https://localhost:7177/_framework/aspnetcore-browser-refresh.js - - -
info: Microsoft.AspNetCore.Hosting.Diagnostics[2]
      Request finished HTTP/2 GET https://localhost:7177/_framework/aspnetcore-browser-refresh.js - 200 14933 application/javascript;+charset=utf-8 4.4612ms
info: Microsoft.AspNetCore.Hosting.Diagnostics[2]
      Request finished HTTP/2 GET https://localhost:7177/_vs/browserLink - 200 - text/javascript;+charset=UTF-8 13.3993ms
info: Microsoft.AspNetCore.Hosting.Diagnostics[1]
      Request starting HTTP/2 GET https://localhost:7177/Cart/GetCartCount - - -
info: Microsoft.AspNetCore.Authentication.Cookies.CookieAuthenticationHandler[7]
      Cookies was not authenticated. Failure message: Ticket expired
info: Microsoft.AspNetCore.Routing.EndpointMiddleware[0]
      Executing endpoint 'WebApplication.Controllers.CartController.GetCartCount (WebApplication)'
info: Microsoft.AspNetCore.Mvc.Infrastructure.ControllerActionInvoker[102]
      Route matched with {action = "GetCartCount", controller = "Cart"}. Executing controller action with signature System.Threading.Tasks.Task`1[Microsoft.AspNetCore.Mvc.IActionResult] GetCartCount(System.Threading.CancellationToken) on controller WebApplication.Controllers.CartController (WebApplication).
info: Microsoft.EntityFrameworkCore.Database.Command[20101]
      Executed DbCommand (57ms) [Parameters=[@__guestSessionId_Value_0='?' (DbType = Int32)], CommandType='Text', CommandTimeout='30']
      SELECT TOP(1) [c].[CartId], [c].[CreatedAt], [c].[GuestSessionId], [c].[IsCheckedOut], [c].[LastUpdatedAt], [c].[UserId]
      FROM [Cart] AS [c]
      WHERE [c].[IsCheckedOut] = CAST(0 AS bit) AND [c].[GuestSessionId] = @__guestSessionId_Value_0
      ORDER BY [c].[CartId]
info: Microsoft.AspNetCore.Mvc.Infrastructure.SystemTextJsonResultExecutor[1]
      Executing JsonResult, writing value of type 'WebApplication.Models.ApiResponse'.
info: Microsoft.AspNetCore.Mvc.Infrastructure.ControllerActionInvoker[105]
      Executed action WebApplication.Controllers.CartController.GetCartCount (WebApplication) in 64.5866ms
info: Microsoft.AspNetCore.Routing.EndpointMiddleware[1]
      Executed endpoint 'WebApplication.Controllers.CartController.GetCartCount (WebApplication)'
info: Microsoft.AspNetCore.Hosting.Diagnostics[2]
      Request finished HTTP/2 GET https://localhost:7177/Cart/GetCartCount - 200 - application/json;+charset=utf-8 76.5871ms
info: Microsoft.EntityFrameworkCore.Database.Command[20101]
      Executed DbCommand (58ms) [Parameters=[@p0='?' (DbType = DateTime2), @p1='?' (Size = 1000), @p2='?' (Size = 100), @p3='?' (DbType = Int32)], CommandType='Text', CommandTimeout='30']
      SET IMPLICIT_TRANSACTIONS OFF;
      SET NOCOUNT ON;
      INSERT INTO [SystemLog] ([CreatedAt], [EventDescription], [EventType], [UserId])
      OUTPUT INSERTED.[SystemLogId]
      VALUES (@p0, @p1, @p2, @p3);
info: Microsoft.EntityFrameworkCore.Database.Command[20101]
      Executed DbCommand (57ms) [Parameters=[], CommandType='Text', CommandTimeout='30']
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
      Executed DbCommand (58ms) [Parameters=[@p0='?' (DbType = DateTime2), @p1='?' (Size = 1000), @p2='?' (Size = 100), @p3='?' (DbType = Int32)], CommandType='Text', CommandTimeout='30']
      SET IMPLICIT_TRANSACTIONS OFF;
      SET NOCOUNT ON;
      INSERT INTO [SystemLog] ([CreatedAt], [EventDescription], [EventType], [UserId])
      OUTPUT INSERTED.[SystemLogId]
      VALUES (@p0, @p1, @p2, @p3);
info: Microsoft.AspNetCore.Hosting.Diagnostics[1]
      Request starting HTTP/2 POST https://localhost:7177/Customer/Login - application/x-www-form-urlencoded 250
info: Microsoft.AspNetCore.Cors.Infrastructure.CorsService[4]
      CORS policy execution successful.
info: Microsoft.AspNetCore.Authentication.Cookies.CookieAuthenticationHandler[7]
      Cookies was not authenticated. Failure message: Ticket expired
info: Microsoft.AspNetCore.Routing.EndpointMiddleware[0]
      Executing endpoint 'WebApplication.Controllers.CustomerController.Login (WebApplication)'
info: Microsoft.AspNetCore.Mvc.Infrastructure.ControllerActionInvoker[102]
      Route matched with {action = "Login", controller = "Customer"}. Executing controller action with signature System.Threading.Tasks.Task`1[Microsoft.AspNetCore.Mvc.IActionResult] Login(WebApplication.Models.ViewModels.LoginViewModel, System.Threading.CancellationToken) on controller WebApplication.Controllers.CustomerController (WebApplication).
info: Microsoft.EntityFrameworkCore.Database.Command[20101]
      Executed DbCommand (57ms) [Parameters=[@__email_0='?' (Size = 255)], CommandType='Text', CommandTimeout='30']
      SELECT TOP(1) [u].[UserId], [u].[CreatedAt], [u].[DefaultAddressId], [u].[Email], [u].[FailedLoginAttempts], [u].[FirstName], [u].[IsActive], [u].[IsDeleted], [u].[IsWalkIn], [u].[LastLoginAt], [u].[LastName], [u].[LockoutUntil], [u].[PasswordHash], [u].[PhoneNumber]
      FROM [User] AS [u]
      WHERE [u].[IsDeleted] = CAST(0 AS bit) AND [u].[Email] = @__email_0 AND [u].[IsActive] = CAST(1 AS bit)
info: Microsoft.EntityFrameworkCore.Database.Command[20101]
      Executed DbCommand (57ms) [Parameters=[@__p_0='?' (DbType = Int32)], CommandType='Text', CommandTimeout='30']
      SELECT TOP(1) [u].[UserId], [u].[CreatedAt], [u].[DefaultAddressId], [u].[Email], [u].[FailedLoginAttempts], [u].[FirstName], [u].[IsActive], [u].[IsDeleted], [u].[IsWalkIn], [u].[LastLoginAt], [u].[LastName], [u].[LockoutUntil], [u].[PasswordHash], [u].[PhoneNumber]
      FROM [User] AS [u]
      WHERE [u].[IsDeleted] = CAST(0 AS bit) AND [u].[UserId] = @__p_0
info: Microsoft.AspNetCore.Authentication.Cookies.CookieAuthenticationHandler[10]
      AuthenticationScheme: Cookies signed in.
info: Microsoft.AspNetCore.Mvc.Infrastructure.RedirectResultExecutor[1]
      Executing RedirectResult, redirecting to /Customer/Dashboard.
info: Microsoft.AspNetCore.Mvc.Infrastructure.ControllerActionInvoker[105]
      Executed action WebApplication.Controllers.CustomerController.Login (WebApplication) in 719.2001ms
info: Microsoft.AspNetCore.Routing.EndpointMiddleware[1]
      Executed endpoint 'WebApplication.Controllers.CustomerController.Login (WebApplication)'
info: Microsoft.AspNetCore.Hosting.Diagnostics[2]
      Request finished HTTP/2 POST https://localhost:7177/Customer/Login - 302 - - 741.2754ms
info: Microsoft.AspNetCore.Hosting.Diagnostics[1]
      Request starting HTTP/2 GET https://localhost:7177/Customer/Dashboard - - -
info: Microsoft.AspNetCore.Routing.EndpointMiddleware[0]
      Executing endpoint 'WebApplication.Controllers.CustomerController.Dashboard (WebApplication)'
info: Microsoft.AspNetCore.Mvc.Infrastructure.ControllerActionInvoker[102]
      Route matched with {action = "Dashboard", controller = "Customer"}. Executing controller action with signature System.Threading.Tasks.Task`1[Microsoft.AspNetCore.Mvc.IActionResult] Dashboard(System.Threading.CancellationToken) on controller WebApplication.Controllers.CustomerController (WebApplication).
info: Microsoft.EntityFrameworkCore.Database.Command[20101]
      Executed DbCommand (58ms) [Parameters=[@__userId_0='?' (DbType = Int32), @__p_1='?' (DbType = Int32), @__p_2='?' (DbType = Int32)], CommandType='Text', CommandTimeout='30']
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
      Executed DbCommand (58ms) [Parameters=[@__userId_0='?' (DbType = Int32), @__p_1='?' (DbType = Int32), @__p_2='?' (DbType = Int32)], CommandType='Text', CommandTimeout='30']
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
      Executed DbCommand (57ms) [Parameters=[@__userId_0='?' (DbType = Int32)], CommandType='Text', CommandTimeout='30']
      SELECT COUNT(*)
      FROM [Order] AS [o]
      WHERE [o].[IsDeleted] = CAST(0 AS bit) AND [o].[UserId] = @__userId_0
info: Microsoft.EntityFrameworkCore.Database.Command[20101]
      Executed DbCommand (57ms) [Parameters=[@__userId_0='?' (DbType = Int32)], CommandType='Text', CommandTimeout='30']
      SELECT COUNT(*)
      FROM [Wishlist] AS [w]
      WHERE [w].[UserId] = @__userId_0
info: Microsoft.EntityFrameworkCore.Database.Command[20101]
      Executed DbCommand (57ms) [Parameters=[@__userId_0='?' (DbType = Int32)], CommandType='Text', CommandTimeout='30']
      SELECT [r].[ProductId]
      FROM [Review] AS [r]
      WHERE [r].[UserId] = @__userId_0
info: Microsoft.EntityFrameworkCore.Database.Command[20101]
      Executed DbCommand (57ms) [Parameters=[@__userId_0='?' (DbType = Int32), @__reviewedProductIds_1='?' (Size = 4000)], CommandType='Text', CommandTimeout='30']
      SELECT DISTINCT [o].[ProductId], [o].[OrderId], [p].[Name] AS [ProductName]
      FROM [OrderItem] AS [o]
      INNER JOIN (
          SELECT [o0].[OrderId], [o0].[OrderStatus], [o0].[UserId]
          FROM [Order] AS [o0]
          WHERE [o0].[IsDeleted] = CAST(0 AS bit)
      ) AS [t] ON [o].[OrderId] = [t].[OrderId]
      INNER JOIN [Product] AS [p] ON [o].[ProductId] = [p].[ProductId]
      WHERE [t].[UserId] = @__userId_0 AND [t].[OrderStatus] = N'Delivered' AND [o].[ProductId] NOT IN (
          SELECT [r].[value]
          FROM OPENJSON(@__reviewedProductIds_1) WITH ([value] int '$') AS [r]
      )
info: Microsoft.EntityFrameworkCore.Database.Command[20101]
      Executed DbCommand (57ms) [Parameters=[@__p_0='?' (DbType = Int32)], CommandType='Text', CommandTimeout='30']
      SELECT TOP(@__p_0) [n].[NotificationId], [n].[Body], [n].[Channel], [n].[CreatedAt], [n].[FailureReason], [n].[IsRead], [n].[NotifType], [n].[OrderId], [n].[ReadAt], [n].[Recipient], [n].[RetryCount], [n].[SentAt], [n].[Status], [n].[Subject], [n].[TicketId], [n].[UserId]
      FROM [Notification] AS [n]
      WHERE [n].[Status] = N'Pending' AND [n].[RetryCount] < 3 AND [n].[Channel] = N'Email'
      ORDER BY [n].[CreatedAt]
info: Microsoft.EntityFrameworkCore.Database.Command[20101]
      Executed DbCommand (58ms) [Parameters=[@__userId_0='?' (DbType = Int32)], CommandType='Text', CommandTimeout='30']
      SELECT [s].[TicketId], [s].[AssignedToUserId], [s].[AttachmentBucket], [s].[AttachmentPath], [s].[AttachmentUrl], [s].[CreatedAt], [s].[Description], [s].[OrderId], [s].[ResolvedAt], [s].[Subject], [s].[TicketCategory], [s].[TicketSource], [s].[TicketStatus], [s].[UpdatedAt], [s].[UserId]
      FROM [SupportTicket] AS [s]
      WHERE [s].[UserId] = @__userId_0
      ORDER BY [s].[CreatedAt] DESC
info: Microsoft.AspNetCore.Mvc.ViewFeatures.ViewResultExecutor[1]
      Executing ViewResult, running view Dashboard.
info: Microsoft.AspNetCore.Mvc.ViewFeatures.ViewResultExecutor[4]
      Executed ViewResult - view Dashboard executed in 23.0028ms.
info: Microsoft.AspNetCore.Mvc.Infrastructure.ControllerActionInvoker[105]
      Executed action WebApplication.Controllers.CustomerController.Dashboard (WebApplication) in 501.078ms
info: Microsoft.AspNetCore.Routing.EndpointMiddleware[1]
      Executed endpoint 'WebApplication.Controllers.CustomerController.Dashboard (WebApplication)'
info: Microsoft.AspNetCore.Hosting.Diagnostics[2]
      Request finished HTTP/2 GET https://localhost:7177/Customer/Dashboard - 200 - text/html;+charset=utf-8 517.7762ms
info: Microsoft.AspNetCore.Hosting.Diagnostics[1]
      Request starting HTTP/2 GET https://localhost:7177/_framework/aspnetcore-browser-refresh.js - - -
info: Microsoft.AspNetCore.Hosting.Diagnostics[1]
      Request starting HTTP/2 GET https://localhost:7177/_vs/browserLink - - -
info: Microsoft.AspNetCore.Hosting.Diagnostics[2]
      Request finished HTTP/2 GET https://localhost:7177/_framework/aspnetcore-browser-refresh.js - 200 14933 application/javascript;+charset=utf-8 4.1203ms
info: Microsoft.AspNetCore.Hosting.Diagnostics[2]
      Request finished HTTP/2 GET https://localhost:7177/_vs/browserLink - 200 - text/javascript;+charset=UTF-8 14.5604ms
info: Microsoft.AspNetCore.Hosting.Diagnostics[1]
      Request starting HTTP/2 GET https://localhost:7177/Customer/NotificationCount - - -
info: Microsoft.AspNetCore.Hosting.Diagnostics[1]
      Request starting HTTP/2 GET https://localhost:7177/Cart/GetCartCount - - -
info: Microsoft.AspNetCore.Hosting.Diagnostics[1]
      Request starting HTTP/2 GET https://localhost:7177/antiforgery/token - - -
info: Microsoft.AspNetCore.Routing.EndpointMiddleware[0]
      Executing endpoint 'HTTP: GET /antiforgery/token'
info: Microsoft.AspNetCore.Routing.EndpointMiddleware[0]
      Executing endpoint 'WebApplication.Controllers.CartController.GetCartCount (WebApplication)'
info: Microsoft.AspNetCore.Mvc.Infrastructure.ControllerActionInvoker[102]
      Route matched with {action = "GetCartCount", controller = "Cart"}. Executing controller action with signature System.Threading.Tasks.Task`1[Microsoft.AspNetCore.Mvc.IActionResult] GetCartCount(System.Threading.CancellationToken) on controller WebApplication.Controllers.CartController (WebApplication).
info: Microsoft.AspNetCore.Http.Result.OkObjectResult[1]
      Setting HTTP status code 200.
info: Microsoft.AspNetCore.Routing.EndpointMiddleware[0]
      Executing endpoint 'WebApplication.Controllers.CustomerController.NotificationCount (WebApplication)'
info: Microsoft.AspNetCore.Http.Result.OkObjectResult[3]
      Writing value of type '<>f__AnonymousType21`1' as Json.
info: Microsoft.AspNetCore.Routing.EndpointMiddleware[1]
      Executed endpoint 'HTTP: GET /antiforgery/token'
info: Microsoft.AspNetCore.Hosting.Diagnostics[2]
      Request finished HTTP/2 GET https://localhost:7177/antiforgery/token - 200 - application/json;+charset=utf-8 42.8755ms
info: Microsoft.AspNetCore.Mvc.Infrastructure.ControllerActionInvoker[102]
      Route matched with {action = "NotificationCount", controller = "Customer"}. Executing controller action with signature System.Threading.Tasks.Task`1[Microsoft.AspNetCore.Mvc.IActionResult] NotificationCount(System.Threading.CancellationToken) on controller WebApplication.Controllers.CustomerController (WebApplication).
info: Microsoft.EntityFrameworkCore.Database.Command[20101]
      Executed DbCommand (58ms) [Parameters=[@__userId_0='?' (DbType = Int32)], CommandType='Text', CommandTimeout='30']
      SELECT COUNT(*)
      FROM [Notification] AS [n]
      WHERE [n].[UserId] = @__userId_0 AND [n].[IsRead] = CAST(0 AS bit) AND [n].[Channel] = N'InApp'
info: Microsoft.AspNetCore.Mvc.Infrastructure.SystemTextJsonResultExecutor[1]
      Executing JsonResult, writing value of type '<>f__AnonymousType6`1[[System.Int32, System.Private.CoreLib, Version=8.0.0.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e]]'.
info: Microsoft.AspNetCore.Mvc.Infrastructure.ControllerActionInvoker[105]
      Executed action WebApplication.Controllers.CustomerController.NotificationCount (WebApplication) in 66.0788ms
info: Microsoft.AspNetCore.Routing.EndpointMiddleware[1]
      Executed endpoint 'WebApplication.Controllers.CustomerController.NotificationCount (WebApplication)'
info: Microsoft.AspNetCore.Hosting.Diagnostics[2]
      Request finished HTTP/2 GET https://localhost:7177/Customer/NotificationCount - 200 - application/json;+charset=utf-8 147.7438ms
info: Microsoft.EntityFrameworkCore.Database.Command[20101]
      Executed DbCommand (57ms) [Parameters=[@__userId_Value_0='?' (DbType = Int32)], CommandType='Text', CommandTimeout='30']
      SELECT TOP(1) [c].[CartId], [c].[CreatedAt], [c].[GuestSessionId], [c].[IsCheckedOut], [c].[LastUpdatedAt], [c].[UserId]
      FROM [Cart] AS [c]
      WHERE [c].[IsCheckedOut] = CAST(0 AS bit) AND [c].[UserId] = @__userId_Value_0
      ORDER BY [c].[CartId]
info: Microsoft.AspNetCore.Mvc.Infrastructure.SystemTextJsonResultExecutor[1]
      Executing JsonResult, writing value of type 'WebApplication.Models.ApiResponse'.
info: Microsoft.AspNetCore.Mvc.Infrastructure.ControllerActionInvoker[105]
      Executed action WebApplication.Controllers.CartController.GetCartCount (WebApplication) in 124.0953ms
info: Microsoft.AspNetCore.Routing.EndpointMiddleware[1]
      Executed endpoint 'WebApplication.Controllers.CartController.GetCartCount (WebApplication)'
info: Microsoft.AspNetCore.Hosting.Diagnostics[2]
      Request finished HTTP/2 GET https://localhost:7177/Cart/GetCartCount - 200 - application/json;+charset=utf-8 161.4956ms
info: Microsoft.AspNetCore.Hosting.Diagnostics[1]
      Request starting HTTP/2 GET https://localhost:7177/Support - - -
info: Microsoft.AspNetCore.Routing.EndpointMiddleware[0]
      Executing endpoint 'WebApplication.Controllers.SupportController.Index (WebApplication)'
info: Microsoft.AspNetCore.Mvc.Infrastructure.ControllerActionInvoker[102]
      Route matched with {action = "Index", controller = "Support"}. Executing controller action with signature System.Threading.Tasks.Task`1[Microsoft.AspNetCore.Mvc.IActionResult] Index(System.Threading.CancellationToken) on controller WebApplication.Controllers.SupportController (WebApplication).
info: Microsoft.EntityFrameworkCore.Database.Command[20101]
      Executed DbCommand (57ms) [Parameters=[@__userId_0='?' (DbType = Int32)], CommandType='Text', CommandTimeout='30']
      SELECT [s].[TicketId], [s].[AssignedToUserId], [s].[AttachmentBucket], [s].[AttachmentPath], [s].[AttachmentUrl], [s].[CreatedAt], [s].[Description], [s].[OrderId], [s].[ResolvedAt], [s].[Subject], [s].[TicketCategory], [s].[TicketSource], [s].[TicketStatus], [s].[UpdatedAt], [s].[UserId]
      FROM [SupportTicket] AS [s]
      WHERE [s].[UserId] = @__userId_0
      ORDER BY [s].[CreatedAt] DESC
info: Microsoft.AspNetCore.Mvc.ViewFeatures.ViewResultExecutor[1]
      Executing ViewResult, running view ~/Views/Customer/SupportList.cshtml.
info: Microsoft.AspNetCore.Mvc.ViewFeatures.ViewResultExecutor[4]
      Executed ViewResult - view ~/Views/Customer/SupportList.cshtml executed in 8.0513ms.
info: Microsoft.AspNetCore.Mvc.Infrastructure.ControllerActionInvoker[105]
      Executed action WebApplication.Controllers.SupportController.Index (WebApplication) in 70.5977ms
info: Microsoft.AspNetCore.Routing.EndpointMiddleware[1]
      Executed endpoint 'WebApplication.Controllers.SupportController.Index (WebApplication)'
info: Microsoft.AspNetCore.Hosting.Diagnostics[2]
      Request finished HTTP/2 GET https://localhost:7177/Support - 200 - text/html;+charset=utf-8 83.4850ms
info: Microsoft.AspNetCore.Hosting.Diagnostics[1]
      Request starting HTTP/2 GET https://localhost:7177/_vs/browserLink - - -
info: Microsoft.AspNetCore.Hosting.Diagnostics[1]
      Request starting HTTP/2 GET https://localhost:7177/_framework/aspnetcore-browser-refresh.js - - -
info: Microsoft.AspNetCore.Hosting.Diagnostics[2]
      Request finished HTTP/2 GET https://localhost:7177/_framework/aspnetcore-browser-refresh.js - 200 14933 application/javascript;+charset=utf-8 4.7811ms
info: Microsoft.AspNetCore.Hosting.Diagnostics[2]
      Request finished HTTP/2 GET https://localhost:7177/_vs/browserLink - 200 - text/javascript;+charset=UTF-8 16.5589ms
info: Microsoft.AspNetCore.Hosting.Diagnostics[1]
      Request starting HTTP/2 GET https://localhost:7177/antiforgery/token - - -
info: Microsoft.AspNetCore.Hosting.Diagnostics[1]
      Request starting HTTP/2 GET https://localhost:7177/Customer/NotificationCount - - -
info: Microsoft.AspNetCore.Hosting.Diagnostics[1]
      Request starting HTTP/2 GET https://localhost:7177/Cart/GetCartCount - - -
info: Microsoft.AspNetCore.Routing.EndpointMiddleware[0]
      Executing endpoint 'HTTP: GET /antiforgery/token'
info: Microsoft.AspNetCore.Routing.EndpointMiddleware[0]
      Executing endpoint 'WebApplication.Controllers.CustomerController.NotificationCount (WebApplication)'
info: Microsoft.AspNetCore.Routing.EndpointMiddleware[0]
      Executing endpoint 'WebApplication.Controllers.CartController.GetCartCount (WebApplication)'
info: Microsoft.AspNetCore.Http.Result.OkObjectResult[1]
      Setting HTTP status code 200.
info: Microsoft.AspNetCore.Mvc.Infrastructure.ControllerActionInvoker[102]
      Route matched with {action = "NotificationCount", controller = "Customer"}. Executing controller action with signature System.Threading.Tasks.Task`1[Microsoft.AspNetCore.Mvc.IActionResult] NotificationCount(System.Threading.CancellationToken) on controller WebApplication.Controllers.CustomerController (WebApplication).
info: Microsoft.AspNetCore.Http.Result.OkObjectResult[3]
      Writing value of type '<>f__AnonymousType21`1' as Json.
info: Microsoft.AspNetCore.Routing.EndpointMiddleware[1]
      Executed endpoint 'HTTP: GET /antiforgery/token'
info: Microsoft.AspNetCore.Mvc.Infrastructure.ControllerActionInvoker[102]
      Route matched with {action = "GetCartCount", controller = "Cart"}. Executing controller action with signature System.Threading.Tasks.Task`1[Microsoft.AspNetCore.Mvc.IActionResult] GetCartCount(System.Threading.CancellationToken) on controller WebApplication.Controllers.CartController (WebApplication).
info: Microsoft.AspNetCore.Hosting.Diagnostics[2]
      Request finished HTTP/2 GET https://localhost:7177/antiforgery/token - 200 - application/json;+charset=utf-8 22.1329ms
info: Microsoft.EntityFrameworkCore.Database.Command[20101]
      Executed DbCommand (58ms) [Parameters=[@__userId_0='?' (DbType = Int32)], CommandType='Text', CommandTimeout='30']
      SELECT COUNT(*)
      FROM [Notification] AS [n]
      WHERE [n].[UserId] = @__userId_0 AND [n].[IsRead] = CAST(0 AS bit) AND [n].[Channel] = N'InApp'
info: Microsoft.AspNetCore.Mvc.Infrastructure.SystemTextJsonResultExecutor[1]
      Executing JsonResult, writing value of type '<>f__AnonymousType6`1[[System.Int32, System.Private.CoreLib, Version=8.0.0.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e]]'.
info: Microsoft.AspNetCore.Mvc.Infrastructure.ControllerActionInvoker[105]
      Executed action WebApplication.Controllers.CustomerController.NotificationCount (WebApplication) in 65.2144ms
info: Microsoft.AspNetCore.Routing.EndpointMiddleware[1]
      Executed endpoint 'WebApplication.Controllers.CustomerController.NotificationCount (WebApplication)'
info: Microsoft.AspNetCore.Hosting.Diagnostics[2]
      Request finished HTTP/2 GET https://localhost:7177/Customer/NotificationCount - 200 - application/json;+charset=utf-8 83.8346ms
info: Microsoft.EntityFrameworkCore.Database.Command[20101]
      Executed DbCommand (58ms) [Parameters=[@__userId_Value_0='?' (DbType = Int32)], CommandType='Text', CommandTimeout='30']
      SELECT TOP(1) [c].[CartId], [c].[CreatedAt], [c].[GuestSessionId], [c].[IsCheckedOut], [c].[LastUpdatedAt], [c].[UserId]
      FROM [Cart] AS [c]
      WHERE [c].[IsCheckedOut] = CAST(0 AS bit) AND [c].[UserId] = @__userId_Value_0
      ORDER BY [c].[CartId]
info: Microsoft.AspNetCore.Mvc.Infrastructure.SystemTextJsonResultExecutor[1]
      Executing JsonResult, writing value of type 'WebApplication.Models.ApiResponse'.
info: Microsoft.AspNetCore.Mvc.Infrastructure.ControllerActionInvoker[105]
      Executed action WebApplication.Controllers.CartController.GetCartCount (WebApplication) in 65.5497ms
info: Microsoft.AspNetCore.Routing.EndpointMiddleware[1]
      Executed endpoint 'WebApplication.Controllers.CartController.GetCartCount (WebApplication)'
info: Microsoft.AspNetCore.Hosting.Diagnostics[2]
      Request finished HTTP/2 GET https://localhost:7177/Cart/GetCartCount - 200 - application/json;+charset=utf-8 91.9947ms
info: Microsoft.AspNetCore.Hosting.Diagnostics[1]
      Request starting HTTP/2 GET https://localhost:7177/Support/Create - - -
info: Microsoft.AspNetCore.Routing.EndpointMiddleware[0]
      Executing endpoint 'WebApplication.Controllers.SupportController.Create (WebApplication)'
info: Microsoft.AspNetCore.Mvc.Infrastructure.ControllerActionInvoker[102]
      Route matched with {action = "Create", controller = "Support"}. Executing controller action with signature Microsoft.AspNetCore.Mvc.IActionResult Create(System.Nullable`1[System.Int32]) on controller WebApplication.Controllers.SupportController (WebApplication).
info: Microsoft.AspNetCore.Mvc.ViewFeatures.ViewResultExecutor[1]
      Executing ViewResult, running view ~/Views/Customer/SupportCreate.cshtml.
info: Microsoft.AspNetCore.Mvc.ViewFeatures.ViewResultExecutor[4]
      Executed ViewResult - view ~/Views/Customer/SupportCreate.cshtml executed in 25.6161ms.
info: Microsoft.AspNetCore.Mvc.Infrastructure.ControllerActionInvoker[105]
      Executed action WebApplication.Controllers.SupportController.Create (WebApplication) in 29.0103ms
info: Microsoft.AspNetCore.Routing.EndpointMiddleware[1]
      Executed endpoint 'WebApplication.Controllers.SupportController.Create (WebApplication)'
info: Microsoft.AspNetCore.Hosting.Diagnostics[2]
      Request finished HTTP/2 GET https://localhost:7177/Support/Create - 200 - text/html;+charset=utf-8 41.6988ms
info: Microsoft.AspNetCore.Hosting.Diagnostics[1]
      Request starting HTTP/2 GET https://localhost:7177/_framework/aspnetcore-browser-refresh.js - - -
info: Microsoft.AspNetCore.Hosting.Diagnostics[1]
      Request starting HTTP/2 GET https://localhost:7177/_vs/browserLink - - -
info: Microsoft.AspNetCore.Hosting.Diagnostics[1]
      Request starting HTTP/2 GET https://localhost:7177/js/support.js?v=vN1WgMBr9NyIwFzi8wCv_0oCi6Mov4yIsPV2liXW5O0 - - -
info: Microsoft.AspNetCore.Hosting.Diagnostics[2]
      Request finished HTTP/2 GET https://localhost:7177/_framework/aspnetcore-browser-refresh.js - 200 14933 application/javascript;+charset=utf-8 8.0017ms
info: Microsoft.AspNetCore.Hosting.Diagnostics[2]
      Request finished HTTP/2 GET https://localhost:7177/_vs/browserLink - 200 - text/javascript;+charset=UTF-8 15.9573ms
info: Microsoft.AspNetCore.StaticFiles.StaticFileMiddleware[2]
      Sending file. Request path: '/js/support.js'. Physical path: 'C:\Users\user\Documents\ANDREI_FILES\PDM_FILES\2ND YEAR\2ND SEM\OOP\OOP-TaurusBikeShop\WebApplication\wwwroot\js\support.js'
info: Microsoft.AspNetCore.Hosting.Diagnostics[2]
      Request finished HTTP/2 GET https://localhost:7177/js/support.js?v=vN1WgMBr9NyIwFzi8wCv_0oCi6Mov4yIsPV2liXW5O0 - 200 2741 text/javascript 25.2268ms
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
info: Microsoft.EntityFrameworkCore.Database.Command[20101]
      Executed DbCommand (57ms) [Parameters=[@__userId_Value_0='?' (DbType = Int32)], CommandType='Text', CommandTimeout='30']
      SELECT TOP(1) [c].[CartId], [c].[CreatedAt], [c].[GuestSessionId], [c].[IsCheckedOut], [c].[LastUpdatedAt], [c].[UserId]
      FROM [Cart] AS [c]
      WHERE [c].[IsCheckedOut] = CAST(0 AS bit) AND [c].[UserId] = @__userId_Value_0
      ORDER BY [c].[CartId]
info: Microsoft.EntityFrameworkCore.Database.Command[20101]
      Executed DbCommand (58ms) [Parameters=[@__userId_0='?' (DbType = Int32)], CommandType='Text', CommandTimeout='30']
      SELECT COUNT(*)
      FROM [Notification] AS [n]
      WHERE [n].[UserId] = @__userId_0 AND [n].[IsRead] = CAST(0 AS bit) AND [n].[Channel] = N'InApp'
info: Microsoft.AspNetCore.Mvc.Infrastructure.SystemTextJsonResultExecutor[1]
      Executing JsonResult, writing value of type 'WebApplication.Models.ApiResponse'.
info: Microsoft.AspNetCore.Mvc.Infrastructure.SystemTextJsonResultExecutor[1]
      Executing JsonResult, writing value of type '<>f__AnonymousType6`1[[System.Int32, System.Private.CoreLib, Version=8.0.0.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e]]'.
info: Microsoft.AspNetCore.Mvc.Infrastructure.ControllerActionInvoker[105]
      Executed action WebApplication.Controllers.CartController.GetCartCount (WebApplication) in 63.4837ms
info: Microsoft.AspNetCore.Mvc.Infrastructure.ControllerActionInvoker[105]
      Executed action WebApplication.Controllers.CustomerController.NotificationCount (WebApplication) in 67.0854ms
info: Microsoft.AspNetCore.Routing.EndpointMiddleware[1]
      Executed endpoint 'WebApplication.Controllers.CartController.GetCartCount (WebApplication)'
info: Microsoft.AspNetCore.Routing.EndpointMiddleware[1]
      Executed endpoint 'WebApplication.Controllers.CustomerController.NotificationCount (WebApplication)'
info: Microsoft.AspNetCore.Hosting.Diagnostics[2]
      Request finished HTTP/2 GET https://localhost:7177/Cart/GetCartCount - 200 - application/json;+charset=utf-8 81.3838ms
info: Microsoft.AspNetCore.Hosting.Diagnostics[2]
      Request finished HTTP/2 GET https://localhost:7177/Customer/NotificationCount - 200 - application/json;+charset=utf-8 82.8748ms
info: Microsoft.AspNetCore.Hosting.Diagnostics[1]
      Request starting HTTP/2 POST https://localhost:7177/Support/Create - application/x-www-form-urlencoded 318
info: Microsoft.AspNetCore.Cors.Infrastructure.CorsService[4]
      CORS policy execution successful.
info: Microsoft.AspNetCore.Routing.EndpointMiddleware[0]
      Executing endpoint 'WebApplication.Controllers.SupportController.Create (WebApplication)'
info: Microsoft.AspNetCore.Mvc.Infrastructure.ControllerActionInvoker[102]
      Route matched with {action = "Create", controller = "Support"}. Executing controller action with signature System.Threading.Tasks.Task`1[Microsoft.AspNetCore.Mvc.IActionResult] Create(WebApplication.Models.ViewModels.SupportCreateViewModel, System.Threading.CancellationToken) on controller WebApplication.Controllers.SupportController (WebApplication).
info: Microsoft.EntityFrameworkCore.Database.Command[20101]
      Executed DbCommand (66ms) [Parameters=[@p0='?' (DbType = Int32), @p1='?' (Size = 200), @p2='?' (Size = 1000), @p3='?' (Size = 1000), @p4='?' (DbType = DateTime2), @p5='?' (Size = 4000), @p6='?' (DbType = Int32), @p7='?' (DbType = DateTime2), @p8='?' (Size = 200), @p9='?' (Size = 100), @p10='?' (Size = 50), @p11='?' (Size = 50), @p12='?' (DbType = DateTime2), @p13='?' (DbType = Int32)], CommandType='Text', CommandTimeout='30']
      SET IMPLICIT_TRANSACTIONS OFF;
      SET NOCOUNT ON;
      INSERT INTO [SupportTicket] ([AssignedToUserId], [AttachmentBucket], [AttachmentPath], [AttachmentUrl], [CreatedAt], [Description], [OrderId], [ResolvedAt], [Subject], [TicketCategory], [TicketSource], [TicketStatus], [UpdatedAt], [UserId])
      OUTPUT INSERTED.[TicketId]
      VALUES (@p0, @p1, @p2, @p3, @p4, @p5, @p6, @p7, @p8, @p9, @p10, @p11, @p12, @p13);
info: Microsoft.EntityFrameworkCore.Database.Command[20101]
      Executed DbCommand (58ms) [Parameters=[@__userId_0='?' (DbType = Int32)], CommandType='Text', CommandTimeout='30']
      SELECT TOP(1) [u].[UserId], [u].[CreatedAt], [u].[DefaultAddressId], [u].[Email], [u].[FailedLoginAttempts], [u].[FirstName], [u].[IsActive], [u].[IsDeleted], [u].[IsWalkIn], [u].[LastLoginAt], [u].[LastName], [u].[LockoutUntil], [u].[PasswordHash], [u].[PhoneNumber]
      FROM [User] AS [u]
      WHERE [u].[IsDeleted] = CAST(0 AS bit) AND [u].[UserId] = @__userId_0
info: Microsoft.EntityFrameworkCore.Database.Command[20101]
      Executed DbCommand (60ms) [Parameters=[@p0='?' (Size = 4000), @p1='?' (Size = 20), @p2='?' (DbType = DateTime2), @p3='?' (Size = 500), @p4='?' (DbType = Boolean), @p5='?' (Size = 100), @p6='?' (DbType = Int32), @p7='?' (DbType = DateTime2), @p8='?' (Size = 255), @p9='?' (DbType = Int32), @p10='?' (DbType = DateTime2), @p11='?' (Size = 20), @p12='?' (Size = 255), @p13='?' (DbType = Int32), @p14='?' (DbType = Int32)], CommandType='Text', CommandTimeout='30']
      SET IMPLICIT_TRANSACTIONS OFF;
      SET NOCOUNT ON;
      INSERT INTO [Notification] ([Body], [Channel], [CreatedAt], [FailureReason], [IsRead], [NotifType], [OrderId], [ReadAt], [Recipient], [RetryCount], [SentAt], [Status], [Subject], [TicketId], [UserId])
      OUTPUT INSERTED.[NotificationId]
      VALUES (@p0, @p1, @p2, @p3, @p4, @p5, @p6, @p7, @p8, @p9, @p10, @p11, @p12, @p13, @p14);
info: Microsoft.AspNetCore.Mvc.RedirectToActionResult[1]
      Executing RedirectResult, redirecting to /Support/Detail?ticketId=1.
info: Microsoft.AspNetCore.Mvc.Infrastructure.ControllerActionInvoker[105]
      Executed action WebApplication.Controllers.SupportController.Create (WebApplication) in 276.4893ms
info: Microsoft.AspNetCore.Routing.EndpointMiddleware[1]
      Executed endpoint 'WebApplication.Controllers.SupportController.Create (WebApplication)'
info: Microsoft.AspNetCore.Hosting.Diagnostics[2]
      Request finished HTTP/2 POST https://localhost:7177/Support/Create - 302 - - 292.5607ms
info: Microsoft.AspNetCore.Hosting.Diagnostics[1]
      Request starting HTTP/2 GET https://localhost:7177/Support/Detail?ticketId=1 - - -
info: Microsoft.AspNetCore.Routing.EndpointMiddleware[0]
      Executing endpoint 'WebApplication.Controllers.SupportController.Detail (WebApplication)'
info: Microsoft.AspNetCore.Mvc.Infrastructure.ControllerActionInvoker[102]
      Route matched with {action = "Detail", controller = "Support"}. Executing controller action with signature System.Threading.Tasks.Task`1[Microsoft.AspNetCore.Mvc.IActionResult] Detail(Int32, System.Threading.CancellationToken) on controller WebApplication.Controllers.SupportController (WebApplication).
info: Microsoft.EntityFrameworkCore.Database.Command[20101]
      Executed DbCommand (96ms) [Parameters=[@__ticketId_0='?' (DbType = Int32)], CommandType='Text', CommandTimeout='30']
      SELECT TOP(1) [s].[TicketId], [s].[AssignedToUserId], [s].[AttachmentBucket], [s].[AttachmentPath], [s].[AttachmentUrl], [s].[CreatedAt], [s].[Description], [s].[OrderId], [s].[ResolvedAt], [s].[Subject], [s].[TicketCategory], [s].[TicketSource], [s].[TicketStatus], [s].[UpdatedAt], [s].[UserId], [t].[UserId], [t].[CreatedAt], [t].[DefaultAddressId], [t].[Email], [t].[FailedLoginAttempts], [t].[FirstName], [t].[IsActive], [t].[IsDeleted], [t].[IsWalkIn], [t].[LastLoginAt], [t].[LastName], [t].[LockoutUntil], [t].[PasswordHash], [t].[PhoneNumber], [t0].[OrderId], [t0].[CartId], [t0].[ContactPhone], [t0].[CreatedAt], [t0].[DeliveryInstructions], [t0].[DiscountAmount], [t0].[FulfillmentType], [t0].[GuestSessionId], [t0].[IsDeleted], [t0].[IsWalkIn], [t0].[OrderDate], [t0].[OrderNumber], [t0].[OrderStatus], [t0].[POSSessionId], [t0].[PaymentMethod], [t0].[ShippingAddressId], [t0].[ShippingFee], [t0].[SubTotal], [t0].[TotalAmount], [t0].[UpdatedAt], [t0].[UserId], [t1].[UserId], [t1].[CreatedAt], [t1].[DefaultAddressId], [t1].[Email], [t1].[FailedLoginAttempts], [t1].[FirstName], [t1].[IsActive], [t1].[IsDeleted], [t1].[IsWalkIn], [t1].[LastLoginAt], [t1].[LastName], [t1].[LockoutUntil], [t1].[PasswordHash], [t1].[PhoneNumber]
      FROM [SupportTicket] AS [s]
      INNER JOIN (
          SELECT [u].[UserId], [u].[CreatedAt], [u].[DefaultAddressId], [u].[Email], [u].[FailedLoginAttempts], [u].[FirstName], [u].[IsActive], [u].[IsDeleted], [u].[IsWalkIn], [u].[LastLoginAt], [u].[LastName], [u].[LockoutUntil], [u].[PasswordHash], [u].[PhoneNumber]
          FROM [User] AS [u]
          WHERE [u].[IsDeleted] = CAST(0 AS bit)
      ) AS [t] ON [s].[UserId] = [t].[UserId]
      LEFT JOIN (
          SELECT [o].[OrderId], [o].[CartId], [o].[ContactPhone], [o].[CreatedAt], [o].[DeliveryInstructions], [o].[DiscountAmount], [o].[FulfillmentType], [o].[GuestSessionId], [o].[IsDeleted], [o].[IsWalkIn], [o].[OrderDate], [o].[OrderNumber], [o].[OrderStatus], [o].[POSSessionId], [o].[PaymentMethod], [o].[ShippingAddressId], [o].[ShippingFee], [o].[SubTotal], [o].[TotalAmount], [o].[UpdatedAt], [o].[UserId]
          FROM [Order] AS [o]
          WHERE [o].[IsDeleted] = CAST(0 AS bit)
      ) AS [t0] ON [s].[OrderId] = [t0].[OrderId]
      LEFT JOIN (
          SELECT [u0].[UserId], [u0].[CreatedAt], [u0].[DefaultAddressId], [u0].[Email], [u0].[FailedLoginAttempts], [u0].[FirstName], [u0].[IsActive], [u0].[IsDeleted], [u0].[IsWalkIn], [u0].[LastLoginAt], [u0].[LastName], [u0].[LockoutUntil], [u0].[PasswordHash], [u0].[PhoneNumber]
          FROM [User] AS [u0]
          WHERE [u0].[IsDeleted] = CAST(0 AS bit)
      ) AS [t1] ON [s].[AssignedToUserId] = [t1].[UserId]
      WHERE [s].[TicketId] = @__ticketId_0
      ORDER BY [s].[TicketId], [t].[UserId], [t0].[OrderId], [t1].[UserId]
info: Microsoft.EntityFrameworkCore.Database.Command[20101]
      Executed DbCommand (78ms) [Parameters=[@__ticketId_0='?' (DbType = Int32)], CommandType='Text', CommandTimeout='30']
      SELECT [t3].[ReplyId], [t3].[AttachmentBucket], [t3].[AttachmentPath], [t3].[AttachmentUrl], [t3].[CreatedAt], [t3].[IsAdminReply], [t3].[Message], [t3].[TicketId], [t3].[UserId], [t3].[UserId0], [t3].[CreatedAt0], [t3].[DefaultAddressId], [t3].[Email], [t3].[FailedLoginAttempts], [t3].[FirstName], [t3].[IsActive], [t3].[IsDeleted], [t3].[IsWalkIn], [t3].[LastLoginAt], [t3].[LastName], [t3].[LockoutUntil], [t3].[PasswordHash], [t3].[PhoneNumber], [t2].[TicketId], [t2].[UserId], [t2].[OrderId], [t2].[UserId0]
      FROM (
          SELECT TOP(1) [s].[TicketId], [t].[UserId], [t0].[OrderId], [t1].[UserId] AS [UserId0]
          FROM [SupportTicket] AS [s]
          INNER JOIN (
              SELECT [u].[UserId]
              FROM [User] AS [u]
              WHERE [u].[IsDeleted] = CAST(0 AS bit)
          ) AS [t] ON [s].[UserId] = [t].[UserId]
          LEFT JOIN (
              SELECT [o].[OrderId]
              FROM [Order] AS [o]
              WHERE [o].[IsDeleted] = CAST(0 AS bit)
          ) AS [t0] ON [s].[OrderId] = [t0].[OrderId]
          LEFT JOIN (
              SELECT [u0].[UserId]
              FROM [User] AS [u0]
              WHERE [u0].[IsDeleted] = CAST(0 AS bit)
          ) AS [t1] ON [s].[AssignedToUserId] = [t1].[UserId]
          WHERE [s].[TicketId] = @__ticketId_0
      ) AS [t2]
      INNER JOIN (
          SELECT [s0].[ReplyId], [s0].[AttachmentBucket], [s0].[AttachmentPath], [s0].[AttachmentUrl], [s0].[CreatedAt], [s0].[IsAdminReply], [s0].[Message], [s0].[TicketId], [s0].[UserId], [t4].[UserId] AS [UserId0], [t4].[CreatedAt] AS [CreatedAt0], [t4].[DefaultAddressId], [t4].[Email], [t4].[FailedLoginAttempts], [t4].[FirstName], [t4].[IsActive], [t4].[IsDeleted], [t4].[IsWalkIn], [t4].[LastLoginAt], [t4].[LastName], [t4].[LockoutUntil], [t4].[PasswordHash], [t4].[PhoneNumber]
          FROM [SupportTicketReply] AS [s0]
          INNER JOIN (
              SELECT [u1].[UserId], [u1].[CreatedAt], [u1].[DefaultAddressId], [u1].[Email], [u1].[FailedLoginAttempts], [u1].[FirstName], [u1].[IsActive], [u1].[IsDeleted], [u1].[IsWalkIn], [u1].[LastLoginAt], [u1].[LastName], [u1].[LockoutUntil], [u1].[PasswordHash], [u1].[PhoneNumber]
              FROM [User] AS [u1]
              WHERE [u1].[IsDeleted] = CAST(0 AS bit)
          ) AS [t4] ON [s0].[UserId] = [t4].[UserId]
      ) AS [t3] ON [t2].[TicketId] = [t3].[TicketId]
      ORDER BY [t2].[TicketId], [t2].[UserId], [t2].[OrderId], [t2].[UserId0], [t3].[CreatedAt]
info: Microsoft.EntityFrameworkCore.Database.Command[20101]
      Executed DbCommand (79ms) [Parameters=[@__ticketId_0='?' (DbType = Int32)], CommandType='Text', CommandTimeout='30']
      SELECT [t3].[TaskId], [t3].[AssignedToUserId], [t3].[CompletedAt], [t3].[CreatedAt], [t3].[DueDate], [t3].[Notes], [t3].[TaskStatus], [t3].[TaskType], [t3].[TicketId], [t3].[UserId], [t3].[CreatedAt0], [t3].[DefaultAddressId], [t3].[Email], [t3].[FailedLoginAttempts], [t3].[FirstName], [t3].[IsActive], [t3].[IsDeleted], [t3].[IsWalkIn], [t3].[LastLoginAt], [t3].[LastName], [t3].[LockoutUntil], [t3].[PasswordHash], [t3].[PhoneNumber], [t2].[TicketId], [t2].[UserId], [t2].[OrderId], [t2].[UserId0]
      FROM (
          SELECT TOP(1) [s].[TicketId], [t].[UserId], [t0].[OrderId], [t1].[UserId] AS [UserId0]
          FROM [SupportTicket] AS [s]
          INNER JOIN (
              SELECT [u].[UserId]
              FROM [User] AS [u]
              WHERE [u].[IsDeleted] = CAST(0 AS bit)
          ) AS [t] ON [s].[UserId] = [t].[UserId]
          LEFT JOIN (
              SELECT [o].[OrderId]
              FROM [Order] AS [o]
              WHERE [o].[IsDeleted] = CAST(0 AS bit)
          ) AS [t0] ON [s].[OrderId] = [t0].[OrderId]
          LEFT JOIN (
              SELECT [u0].[UserId]
              FROM [User] AS [u0]
              WHERE [u0].[IsDeleted] = CAST(0 AS bit)
          ) AS [t1] ON [s].[AssignedToUserId] = [t1].[UserId]
          WHERE [s].[TicketId] = @__ticketId_0
      ) AS [t2]
      INNER JOIN (
          SELECT [s0].[TaskId], [s0].[AssignedToUserId], [s0].[CompletedAt], [s0].[CreatedAt], [s0].[DueDate], [s0].[Notes], [s0].[TaskStatus], [s0].[TaskType], [s0].[TicketId], [t4].[UserId], [t4].[CreatedAt] AS [CreatedAt0], [t4].[DefaultAddressId], [t4].[Email], [t4].[FailedLoginAttempts], [t4].[FirstName], [t4].[IsActive], [t4].[IsDeleted], [t4].[IsWalkIn], [t4].[LastLoginAt], [t4].[LastName], [t4].[LockoutUntil], [t4].[PasswordHash], [t4].[PhoneNumber]
          FROM [SupportTask] AS [s0]
          LEFT JOIN (
              SELECT [u1].[UserId], [u1].[CreatedAt], [u1].[DefaultAddressId], [u1].[Email], [u1].[FailedLoginAttempts], [u1].[FirstName], [u1].[IsActive], [u1].[IsDeleted], [u1].[IsWalkIn], [u1].[LastLoginAt], [u1].[LastName], [u1].[LockoutUntil], [u1].[PasswordHash], [u1].[PhoneNumber]
              FROM [User] AS [u1]
              WHERE [u1].[IsDeleted] = CAST(0 AS bit)
          ) AS [t4] ON [s0].[AssignedToUserId] = [t4].[UserId]
      ) AS [t3] ON [t2].[TicketId] = [t3].[TicketId]
      ORDER BY [t2].[TicketId], [t2].[UserId], [t2].[OrderId], [t2].[UserId0]
info: Microsoft.AspNetCore.Mvc.ViewFeatures.ViewResultExecutor[1]
      Executing ViewResult, running view ~/Views/Customer/SupportDetail.cshtml.
info: Microsoft.AspNetCore.Mvc.ViewFeatures.ViewResultExecutor[4]
      Executed ViewResult - view ~/Views/Customer/SupportDetail.cshtml executed in 13.9325ms.
info: Microsoft.AspNetCore.Mvc.Infrastructure.ControllerActionInvoker[105]
      Executed action WebApplication.Controllers.SupportController.Detail (WebApplication) in 313.5314ms
info: Microsoft.AspNetCore.Routing.EndpointMiddleware[1]
      Executed endpoint 'WebApplication.Controllers.SupportController.Detail (WebApplication)'
info: Microsoft.AspNetCore.Hosting.Diagnostics[2]
      Request finished HTTP/2 GET https://localhost:7177/Support/Detail?ticketId=1 - 200 - text/html;+charset=utf-8 326.2811ms
info: Microsoft.AspNetCore.Hosting.Diagnostics[1]
      Request starting HTTP/2 GET https://localhost:7177/_vs/browserLink - - -
info: Microsoft.AspNetCore.Hosting.Diagnostics[1]
      Request starting HTTP/2 GET https://localhost:7177/_framework/aspnetcore-browser-refresh.js - - -
info: Microsoft.AspNetCore.Hosting.Diagnostics[2]
      Request finished HTTP/2 GET https://localhost:7177/_framework/aspnetcore-browser-refresh.js - 200 14933 application/javascript;+charset=utf-8 5.8359ms
info: Microsoft.AspNetCore.Hosting.Diagnostics[1]
      Request starting HTTP/2 GET https://localhost:7177/Cart/GetCartCount - - -
info: Microsoft.AspNetCore.Hosting.Diagnostics[1]
      Request starting HTTP/2 GET https://localhost:7177/Customer/NotificationCount - - -
info: Microsoft.AspNetCore.Hosting.Diagnostics[2]
      Request finished HTTP/2 GET https://localhost:7177/_vs/browserLink - 200 - text/javascript;+charset=UTF-8 16.7577ms
info: Microsoft.AspNetCore.Routing.EndpointMiddleware[0]
      Executing endpoint 'WebApplication.Controllers.CustomerController.NotificationCount (WebApplication)'
info: Microsoft.AspNetCore.Routing.EndpointMiddleware[0]
      Executing endpoint 'WebApplication.Controllers.CartController.GetCartCount (WebApplication)'
info: Microsoft.AspNetCore.Mvc.Infrastructure.ControllerActionInvoker[102]
      Route matched with {action = "NotificationCount", controller = "Customer"}. Executing controller action with signature System.Threading.Tasks.Task`1[Microsoft.AspNetCore.Mvc.IActionResult] NotificationCount(System.Threading.CancellationToken) on controller WebApplication.Controllers.CustomerController (WebApplication).
info: Microsoft.AspNetCore.Mvc.Infrastructure.ControllerActionInvoker[102]
      Route matched with {action = "GetCartCount", controller = "Cart"}. Executing controller action with signature System.Threading.Tasks.Task`1[Microsoft.AspNetCore.Mvc.IActionResult] GetCartCount(System.Threading.CancellationToken) on controller WebApplication.Controllers.CartController (WebApplication).
info: Microsoft.EntityFrameworkCore.Database.Command[20101]
      Executed DbCommand (57ms) [Parameters=[@__userId_Value_0='?' (DbType = Int32)], CommandType='Text', CommandTimeout='30']
      SELECT TOP(1) [c].[CartId], [c].[CreatedAt], [c].[GuestSessionId], [c].[IsCheckedOut], [c].[LastUpdatedAt], [c].[UserId]
      FROM [Cart] AS [c]
      WHERE [c].[IsCheckedOut] = CAST(0 AS bit) AND [c].[UserId] = @__userId_Value_0
      ORDER BY [c].[CartId]
info: Microsoft.EntityFrameworkCore.Database.Command[20101]
      Executed DbCommand (58ms) [Parameters=[@__userId_0='?' (DbType = Int32)], CommandType='Text', CommandTimeout='30']
      SELECT COUNT(*)
      FROM [Notification] AS [n]
      WHERE [n].[UserId] = @__userId_0 AND [n].[IsRead] = CAST(0 AS bit) AND [n].[Channel] = N'InApp'
info: Microsoft.AspNetCore.Mvc.Infrastructure.SystemTextJsonResultExecutor[1]
      Executing JsonResult, writing value of type 'WebApplication.Models.ApiResponse'.
info: Microsoft.AspNetCore.Mvc.Infrastructure.SystemTextJsonResultExecutor[1]
      Executing JsonResult, writing value of type '<>f__AnonymousType6`1[[System.Int32, System.Private.CoreLib, Version=8.0.0.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e]]'.
info: Microsoft.AspNetCore.Mvc.Infrastructure.ControllerActionInvoker[105]
      Executed action WebApplication.Controllers.CartController.GetCartCount (WebApplication) in 62.8397ms
info: Microsoft.AspNetCore.Mvc.Infrastructure.ControllerActionInvoker[105]
      Executed action WebApplication.Controllers.CustomerController.NotificationCount (WebApplication) in 66.3707ms
info: Microsoft.AspNetCore.Routing.EndpointMiddleware[1]
      Executed endpoint 'WebApplication.Controllers.CartController.GetCartCount (WebApplication)'
info: Microsoft.AspNetCore.Routing.EndpointMiddleware[1]
      Executed endpoint 'WebApplication.Controllers.CustomerController.NotificationCount (WebApplication)'
info: Microsoft.AspNetCore.Hosting.Diagnostics[2]
      Request finished HTTP/2 GET https://localhost:7177/Cart/GetCartCount - 200 - application/json;+charset=utf-8 83.9926ms
info: Microsoft.AspNetCore.Hosting.Diagnostics[2]
      Request finished HTTP/2 GET https://localhost:7177/Customer/NotificationCount - 200 - application/json;+charset=utf-8 85.7528ms
info: Microsoft.AspNetCore.Hosting.Diagnostics[1]
      Request starting HTTP/2 GET https://localhost:7177/Support/Detail?ticketId=1 - - -
info: Microsoft.AspNetCore.Routing.EndpointMiddleware[0]
      Executing endpoint 'WebApplication.Controllers.SupportController.Detail (WebApplication)'
info: Microsoft.AspNetCore.Mvc.Infrastructure.ControllerActionInvoker[102]
      Route matched with {action = "Detail", controller = "Support"}. Executing controller action with signature System.Threading.Tasks.Task`1[Microsoft.AspNetCore.Mvc.IActionResult] Detail(Int32, System.Threading.CancellationToken) on controller WebApplication.Controllers.SupportController (WebApplication).
info: Microsoft.EntityFrameworkCore.Database.Command[20101]
      Executed DbCommand (59ms) [Parameters=[@__ticketId_0='?' (DbType = Int32)], CommandType='Text', CommandTimeout='30']
      SELECT TOP(1) [s].[TicketId], [s].[AssignedToUserId], [s].[AttachmentBucket], [s].[AttachmentPath], [s].[AttachmentUrl], [s].[CreatedAt], [s].[Description], [s].[OrderId], [s].[ResolvedAt], [s].[Subject], [s].[TicketCategory], [s].[TicketSource], [s].[TicketStatus], [s].[UpdatedAt], [s].[UserId], [t].[UserId], [t].[CreatedAt], [t].[DefaultAddressId], [t].[Email], [t].[FailedLoginAttempts], [t].[FirstName], [t].[IsActive], [t].[IsDeleted], [t].[IsWalkIn], [t].[LastLoginAt], [t].[LastName], [t].[LockoutUntil], [t].[PasswordHash], [t].[PhoneNumber], [t0].[OrderId], [t0].[CartId], [t0].[ContactPhone], [t0].[CreatedAt], [t0].[DeliveryInstructions], [t0].[DiscountAmount], [t0].[FulfillmentType], [t0].[GuestSessionId], [t0].[IsDeleted], [t0].[IsWalkIn], [t0].[OrderDate], [t0].[OrderNumber], [t0].[OrderStatus], [t0].[POSSessionId], [t0].[PaymentMethod], [t0].[ShippingAddressId], [t0].[ShippingFee], [t0].[SubTotal], [t0].[TotalAmount], [t0].[UpdatedAt], [t0].[UserId], [t1].[UserId], [t1].[CreatedAt], [t1].[DefaultAddressId], [t1].[Email], [t1].[FailedLoginAttempts], [t1].[FirstName], [t1].[IsActive], [t1].[IsDeleted], [t1].[IsWalkIn], [t1].[LastLoginAt], [t1].[LastName], [t1].[LockoutUntil], [t1].[PasswordHash], [t1].[PhoneNumber]
      FROM [SupportTicket] AS [s]
      INNER JOIN (
          SELECT [u].[UserId], [u].[CreatedAt], [u].[DefaultAddressId], [u].[Email], [u].[FailedLoginAttempts], [u].[FirstName], [u].[IsActive], [u].[IsDeleted], [u].[IsWalkIn], [u].[LastLoginAt], [u].[LastName], [u].[LockoutUntil], [u].[PasswordHash], [u].[PhoneNumber]
          FROM [User] AS [u]
          WHERE [u].[IsDeleted] = CAST(0 AS bit)
      ) AS [t] ON [s].[UserId] = [t].[UserId]
      LEFT JOIN (
          SELECT [o].[OrderId], [o].[CartId], [o].[ContactPhone], [o].[CreatedAt], [o].[DeliveryInstructions], [o].[DiscountAmount], [o].[FulfillmentType], [o].[GuestSessionId], [o].[IsDeleted], [o].[IsWalkIn], [o].[OrderDate], [o].[OrderNumber], [o].[OrderStatus], [o].[POSSessionId], [o].[PaymentMethod], [o].[ShippingAddressId], [o].[ShippingFee], [o].[SubTotal], [o].[TotalAmount], [o].[UpdatedAt], [o].[UserId]
          FROM [Order] AS [o]
          WHERE [o].[IsDeleted] = CAST(0 AS bit)
      ) AS [t0] ON [s].[OrderId] = [t0].[OrderId]
      LEFT JOIN (
          SELECT [u0].[UserId], [u0].[CreatedAt], [u0].[DefaultAddressId], [u0].[Email], [u0].[FailedLoginAttempts], [u0].[FirstName], [u0].[IsActive], [u0].[IsDeleted], [u0].[IsWalkIn], [u0].[LastLoginAt], [u0].[LastName], [u0].[LockoutUntil], [u0].[PasswordHash], [u0].[PhoneNumber]
          FROM [User] AS [u0]
          WHERE [u0].[IsDeleted] = CAST(0 AS bit)
      ) AS [t1] ON [s].[AssignedToUserId] = [t1].[UserId]
      WHERE [s].[TicketId] = @__ticketId_0
      ORDER BY [s].[TicketId], [t].[UserId], [t0].[OrderId], [t1].[UserId]
info: Microsoft.EntityFrameworkCore.Database.Command[20101]
      Executed DbCommand (78ms) [Parameters=[@__ticketId_0='?' (DbType = Int32)], CommandType='Text', CommandTimeout='30']
      SELECT [t3].[ReplyId], [t3].[AttachmentBucket], [t3].[AttachmentPath], [t3].[AttachmentUrl], [t3].[CreatedAt], [t3].[IsAdminReply], [t3].[Message], [t3].[TicketId], [t3].[UserId], [t3].[UserId0], [t3].[CreatedAt0], [t3].[DefaultAddressId], [t3].[Email], [t3].[FailedLoginAttempts], [t3].[FirstName], [t3].[IsActive], [t3].[IsDeleted], [t3].[IsWalkIn], [t3].[LastLoginAt], [t3].[LastName], [t3].[LockoutUntil], [t3].[PasswordHash], [t3].[PhoneNumber], [t2].[TicketId], [t2].[UserId], [t2].[OrderId], [t2].[UserId0]
      FROM (
          SELECT TOP(1) [s].[TicketId], [t].[UserId], [t0].[OrderId], [t1].[UserId] AS [UserId0]
          FROM [SupportTicket] AS [s]
          INNER JOIN (
              SELECT [u].[UserId]
              FROM [User] AS [u]
              WHERE [u].[IsDeleted] = CAST(0 AS bit)
          ) AS [t] ON [s].[UserId] = [t].[UserId]
          LEFT JOIN (
              SELECT [o].[OrderId]
              FROM [Order] AS [o]
              WHERE [o].[IsDeleted] = CAST(0 AS bit)
          ) AS [t0] ON [s].[OrderId] = [t0].[OrderId]
          LEFT JOIN (
              SELECT [u0].[UserId]
              FROM [User] AS [u0]
              WHERE [u0].[IsDeleted] = CAST(0 AS bit)
          ) AS [t1] ON [s].[AssignedToUserId] = [t1].[UserId]
          WHERE [s].[TicketId] = @__ticketId_0
      ) AS [t2]
      INNER JOIN (
          SELECT [s0].[ReplyId], [s0].[AttachmentBucket], [s0].[AttachmentPath], [s0].[AttachmentUrl], [s0].[CreatedAt], [s0].[IsAdminReply], [s0].[Message], [s0].[TicketId], [s0].[UserId], [t4].[UserId] AS [UserId0], [t4].[CreatedAt] AS [CreatedAt0], [t4].[DefaultAddressId], [t4].[Email], [t4].[FailedLoginAttempts], [t4].[FirstName], [t4].[IsActive], [t4].[IsDeleted], [t4].[IsWalkIn], [t4].[LastLoginAt], [t4].[LastName], [t4].[LockoutUntil], [t4].[PasswordHash], [t4].[PhoneNumber]
          FROM [SupportTicketReply] AS [s0]
          INNER JOIN (
              SELECT [u1].[UserId], [u1].[CreatedAt], [u1].[DefaultAddressId], [u1].[Email], [u1].[FailedLoginAttempts], [u1].[FirstName], [u1].[IsActive], [u1].[IsDeleted], [u1].[IsWalkIn], [u1].[LastLoginAt], [u1].[LastName], [u1].[LockoutUntil], [u1].[PasswordHash], [u1].[PhoneNumber]
              FROM [User] AS [u1]
              WHERE [u1].[IsDeleted] = CAST(0 AS bit)
          ) AS [t4] ON [s0].[UserId] = [t4].[UserId]
      ) AS [t3] ON [t2].[TicketId] = [t3].[TicketId]
      ORDER BY [t2].[TicketId], [t2].[UserId], [t2].[OrderId], [t2].[UserId0], [t3].[CreatedAt]
fail: Microsoft.EntityFrameworkCore.Query[10100]
      An exception occurred while iterating over the results of a query for context type 'WebApplication.DataAccess.Context.AppDbContext'.
      System.InvalidCastException: Unable to cast object of type 'System.Int64' to type 'System.Int32'.
         at Microsoft.Data.SqlClient.SqlBuffer.get_Int32()
         at Microsoft.EntityFrameworkCore.Query.Internal.BufferedDataReader.BufferedDataRecord.ReadInt(DbDataReader reader, Int32 ordinal, ReaderColumn column)
         at Microsoft.EntityFrameworkCore.Query.Internal.BufferedDataReader.BufferedDataRecord.ReadRow()
         at Microsoft.EntityFrameworkCore.Query.Internal.BufferedDataReader.BufferedDataRecord.InitializeAsync(DbDataReader reader, IReadOnlyList`1 columns, CancellationToken cancellationToken)
         at Microsoft.EntityFrameworkCore.Query.Internal.BufferedDataReader.InitializeAsync(IReadOnlyList`1 columns, CancellationToken cancellationToken)
         at Microsoft.EntityFrameworkCore.Query.Internal.BufferedDataReader.InitializeAsync(IReadOnlyList`1 columns, CancellationToken cancellationToken)
         at Microsoft.EntityFrameworkCore.Storage.RelationalCommand.ExecuteReaderAsync(RelationalCommandParameterObject parameterObject, CancellationToken cancellationToken)
         at Microsoft.EntityFrameworkCore.Storage.RelationalCommand.ExecuteReaderAsync(RelationalCommandParameterObject parameterObject, CancellationToken cancellationToken)
         at Microsoft.EntityFrameworkCore.Query.RelationalShapedQueryCompilingExpressionVisitor.ShaperProcessingExpressionVisitor.<PopulateSplitIncludeCollectionAsync>g__InitializeReaderAsync|26_0[TIncludingEntity,TIncludedEntity](RelationalQueryContext queryContext, RelationalCommandCache relationalCommandCache, IReadOnlyList`1 readerColumns, Boolean detailedErrorsEnabled, CancellationToken cancellationToken)
         at Microsoft.EntityFrameworkCore.Storage.ExecutionStrategy.<>c__DisplayClass30_0`2.<<ExecuteAsync>b__0>d.MoveNext()
      --- End of stack trace from previous location ---
         at Microsoft.EntityFrameworkCore.Storage.ExecutionStrategy.ExecuteImplementationAsync[TState,TResult](Func`4 operation, Func`4 verifySucceeded, TState state, CancellationToken cancellationToken)
         at Microsoft.EntityFrameworkCore.Storage.ExecutionStrategy.ExecuteImplementationAsync[TState,TResult](Func`4 operation, Func`4 verifySucceeded, TState state, CancellationToken cancellationToken)
         at Microsoft.EntityFrameworkCore.Storage.ExecutionStrategy.ExecuteAsync[TState,TResult](TState state, Func`4 operation, Func`4 verifySucceeded, CancellationToken cancellationToken)
         at Microsoft.EntityFrameworkCore.Query.RelationalShapedQueryCompilingExpressionVisitor.ShaperProcessingExpressionVisitor.PopulateSplitIncludeCollectionAsync[TIncludingEntity,TIncludedEntity](Int32 collectionId, RelationalQueryContext queryContext, IExecutionStrategy executionStrategy, RelationalCommandCache relationalCommandCache, IReadOnlyList`1 readerColumns, Boolean detailedErrorsEnabled, SplitQueryResultCoordinator resultCoordinator, Func`3 childIdentifier, IReadOnlyList`1 identifierValueComparers, Func`5 innerShaper, Func`4 relatedDataLoaders, INavigationBase inverseNavigation, Action`2 fixup, Boolean trackingQuery)
         at Microsoft.EntityFrameworkCore.Query.RelationalShapedQueryCompilingExpressionVisitor.ShaperProcessingExpressionVisitor.TaskAwaiter(Func`1[] taskFactories)
         at Microsoft.EntityFrameworkCore.Query.Internal.SplitQueryingEnumerable`1.AsyncEnumerator.MoveNextAsync()
      System.InvalidCastException: Unable to cast object of type 'System.Int64' to type 'System.Int32'.
         at Microsoft.Data.SqlClient.SqlBuffer.get_Int32()
         at Microsoft.EntityFrameworkCore.Query.Internal.BufferedDataReader.BufferedDataRecord.ReadInt(DbDataReader reader, Int32 ordinal, ReaderColumn column)
         at Microsoft.EntityFrameworkCore.Query.Internal.BufferedDataReader.BufferedDataRecord.ReadRow()
         at Microsoft.EntityFrameworkCore.Query.Internal.BufferedDataReader.BufferedDataRecord.InitializeAsync(DbDataReader reader, IReadOnlyList`1 columns, CancellationToken cancellationToken)
         at Microsoft.EntityFrameworkCore.Query.Internal.BufferedDataReader.InitializeAsync(IReadOnlyList`1 columns, CancellationToken cancellationToken)
         at Microsoft.EntityFrameworkCore.Query.Internal.BufferedDataReader.InitializeAsync(IReadOnlyList`1 columns, CancellationToken cancellationToken)
         at Microsoft.EntityFrameworkCore.Storage.RelationalCommand.ExecuteReaderAsync(RelationalCommandParameterObject parameterObject, CancellationToken cancellationToken)
         at Microsoft.EntityFrameworkCore.Storage.RelationalCommand.ExecuteReaderAsync(RelationalCommandParameterObject parameterObject, CancellationToken cancellationToken)
         at Microsoft.EntityFrameworkCore.Query.RelationalShapedQueryCompilingExpressionVisitor.ShaperProcessingExpressionVisitor.<PopulateSplitIncludeCollectionAsync>g__InitializeReaderAsync|26_0[TIncludingEntity,TIncludedEntity](RelationalQueryContext queryContext, RelationalCommandCache relationalCommandCache, IReadOnlyList`1 readerColumns, Boolean detailedErrorsEnabled, CancellationToken cancellationToken)
         at Microsoft.EntityFrameworkCore.Storage.ExecutionStrategy.<>c__DisplayClass30_0`2.<<ExecuteAsync>b__0>d.MoveNext()
      --- End of stack trace from previous location ---
         at Microsoft.EntityFrameworkCore.Storage.ExecutionStrategy.ExecuteImplementationAsync[TState,TResult](Func`4 operation, Func`4 verifySucceeded, TState state, CancellationToken cancellationToken)
         at Microsoft.EntityFrameworkCore.Storage.ExecutionStrategy.ExecuteImplementationAsync[TState,TResult](Func`4 operation, Func`4 verifySucceeded, TState state, CancellationToken cancellationToken)
         at Microsoft.EntityFrameworkCore.Storage.ExecutionStrategy.ExecuteAsync[TState,TResult](TState state, Func`4 operation, Func`4 verifySucceeded, CancellationToken cancellationToken)
         at Microsoft.EntityFrameworkCore.Query.RelationalShapedQueryCompilingExpressionVisitor.ShaperProcessingExpressionVisitor.PopulateSplitIncludeCollectionAsync[TIncludingEntity,TIncludedEntity](Int32 collectionId, RelationalQueryContext queryContext, IExecutionStrategy executionStrategy, RelationalCommandCache relationalCommandCache, IReadOnlyList`1 readerColumns, Boolean detailedErrorsEnabled, SplitQueryResultCoordinator resultCoordinator, Func`3 childIdentifier, IReadOnlyList`1 identifierValueComparers, Func`5 innerShaper, Func`4 relatedDataLoaders, INavigationBase inverseNavigation, Action`2 fixup, Boolean trackingQuery)
         at Microsoft.EntityFrameworkCore.Query.RelationalShapedQueryCompilingExpressionVisitor.ShaperProcessingExpressionVisitor.TaskAwaiter(Func`1[] taskFactories)
         at Microsoft.EntityFrameworkCore.Query.Internal.SplitQueryingEnumerable`1.AsyncEnumerator.MoveNextAsync()
fail: WebApplication.Controllers.SupportController[0]
      Error loading ticket 1.
      System.InvalidCastException: Unable to cast object of type 'System.Int64' to type 'System.Int32'.
         at Microsoft.Data.SqlClient.SqlBuffer.get_Int32()
         at Microsoft.EntityFrameworkCore.Query.Internal.BufferedDataReader.BufferedDataRecord.ReadInt(DbDataReader reader, Int32 ordinal, ReaderColumn column)
         at Microsoft.EntityFrameworkCore.Query.Internal.BufferedDataReader.BufferedDataRecord.ReadRow()
         at Microsoft.EntityFrameworkCore.Query.Internal.BufferedDataReader.BufferedDataRecord.InitializeAsync(DbDataReader reader, IReadOnlyList`1 columns, CancellationToken cancellationToken)
         at Microsoft.EntityFrameworkCore.Query.Internal.BufferedDataReader.InitializeAsync(IReadOnlyList`1 columns, CancellationToken cancellationToken)
         at Microsoft.EntityFrameworkCore.Query.Internal.BufferedDataReader.InitializeAsync(IReadOnlyList`1 columns, CancellationToken cancellationToken)
         at Microsoft.EntityFrameworkCore.Storage.RelationalCommand.ExecuteReaderAsync(RelationalCommandParameterObject parameterObject, CancellationToken cancellationToken)
         at Microsoft.EntityFrameworkCore.Storage.RelationalCommand.ExecuteReaderAsync(RelationalCommandParameterObject parameterObject, CancellationToken cancellationToken)
         at Microsoft.EntityFrameworkCore.Query.RelationalShapedQueryCompilingExpressionVisitor.ShaperProcessingExpressionVisitor.<PopulateSplitIncludeCollectionAsync>g__InitializeReaderAsync|26_0[TIncludingEntity,TIncludedEntity](RelationalQueryContext queryContext, RelationalCommandCache relationalCommandCache, IReadOnlyList`1 readerColumns, Boolean detailedErrorsEnabled, CancellationToken cancellationToken)
         at Microsoft.EntityFrameworkCore.Storage.ExecutionStrategy.<>c__DisplayClass30_0`2.<<ExecuteAsync>b__0>d.MoveNext()
      --- End of stack trace from previous location ---
         at Microsoft.EntityFrameworkCore.Storage.ExecutionStrategy.ExecuteImplementationAsync[TState,TResult](Func`4 operation, Func`4 verifySucceeded, TState state, CancellationToken cancellationToken)
         at Microsoft.EntityFrameworkCore.Storage.ExecutionStrategy.ExecuteImplementationAsync[TState,TResult](Func`4 operation, Func`4 verifySucceeded, TState state, CancellationToken cancellationToken)
         at Microsoft.EntityFrameworkCore.Storage.ExecutionStrategy.ExecuteAsync[TState,TResult](TState state, Func`4 operation, Func`4 verifySucceeded, CancellationToken cancellationToken)
         at Microsoft.EntityFrameworkCore.Query.RelationalShapedQueryCompilingExpressionVisitor.ShaperProcessingExpressionVisitor.PopulateSplitIncludeCollectionAsync[TIncludingEntity,TIncludedEntity](Int32 collectionId, RelationalQueryContext queryContext, IExecutionStrategy executionStrategy, RelationalCommandCache relationalCommandCache, IReadOnlyList`1 readerColumns, Boolean detailedErrorsEnabled, SplitQueryResultCoordinator resultCoordinator, Func`3 childIdentifier, IReadOnlyList`1 identifierValueComparers, Func`5 innerShaper, Func`4 relatedDataLoaders, INavigationBase inverseNavigation, Action`2 fixup, Boolean trackingQuery)
         at Microsoft.EntityFrameworkCore.Query.RelationalShapedQueryCompilingExpressionVisitor.ShaperProcessingExpressionVisitor.TaskAwaiter(Func`1[] taskFactories)
         at Microsoft.EntityFrameworkCore.Query.Internal.SplitQueryingEnumerable`1.AsyncEnumerator.MoveNextAsync()
         at Microsoft.EntityFrameworkCore.Query.ShapedQueryCompilingExpressionVisitor.SingleOrDefaultAsync[TSource](IAsyncEnumerable`1 asyncEnumerable, CancellationToken cancellationToken)
         at Microsoft.EntityFrameworkCore.Query.ShapedQueryCompilingExpressionVisitor.SingleOrDefaultAsync[TSource](IAsyncEnumerable`1 asyncEnumerable, CancellationToken cancellationToken)
         at WebApplication.DataAccess.Repositories.SupportRepository.GetWithRepliesAsync(Int32 ticketId, CancellationToken cancellationToken) in C:\Users\user\Documents\ANDREI_FILES\PDM_FILES\2ND YEAR\2ND SEM\OOP\OOP-TaurusBikeShop\WebApplication\DataAccess\Repositories\SupportRepository.cs:line 54
         at WebApplication.BusinessLogic.Services.SupportService.GetDetailAsync(Int32 ticketId, Int32 userId, CancellationToken cancellationToken) in C:\Users\user\Documents\ANDREI_FILES\PDM_FILES\2ND YEAR\2ND SEM\OOP\OOP-TaurusBikeShop\WebApplication\BusinessLogic\Services\SupportService.cs:line 110
         at WebApplication.Controllers.SupportController.Detail(Int32 ticketId, CancellationToken cancellationToken) in C:\Users\user\Documents\ANDREI_FILES\PDM_FILES\2ND YEAR\2ND SEM\OOP\OOP-TaurusBikeShop\WebApplication\Controllers\SupportController.cs:line 135
info: Microsoft.AspNetCore.Mvc.RedirectToActionResult[1]
      Executing RedirectResult, redirecting to /Support.
info: Microsoft.AspNetCore.Mvc.Infrastructure.ControllerActionInvoker[105]
      Executed action WebApplication.Controllers.SupportController.Detail (WebApplication) in 307.936ms
info: Microsoft.AspNetCore.Routing.EndpointMiddleware[1]
      Executed endpoint 'WebApplication.Controllers.SupportController.Detail (WebApplication)'
info: Microsoft.AspNetCore.Hosting.Diagnostics[2]
      Request finished HTTP/2 GET https://localhost:7177/Support/Detail?ticketId=1 - 302 - - 321.8718ms
info: Microsoft.AspNetCore.Hosting.Diagnostics[1]
      Request starting HTTP/2 GET https://localhost:7177/Support - - -
info: Microsoft.AspNetCore.Routing.EndpointMiddleware[0]
      Executing endpoint 'WebApplication.Controllers.SupportController.Index (WebApplication)'
info: Microsoft.AspNetCore.Mvc.Infrastructure.ControllerActionInvoker[102]
      Route matched with {action = "Index", controller = "Support"}. Executing controller action with signature System.Threading.Tasks.Task`1[Microsoft.AspNetCore.Mvc.IActionResult] Index(System.Threading.CancellationToken) on controller WebApplication.Controllers.SupportController (WebApplication).
info: Microsoft.EntityFrameworkCore.Database.Command[20101]
      Executed DbCommand (61ms) [Parameters=[@__userId_0='?' (DbType = Int32)], CommandType='Text', CommandTimeout='30']
      SELECT [s].[TicketId], [s].[AssignedToUserId], [s].[AttachmentBucket], [s].[AttachmentPath], [s].[AttachmentUrl], [s].[CreatedAt], [s].[Description], [s].[OrderId], [s].[ResolvedAt], [s].[Subject], [s].[TicketCategory], [s].[TicketSource], [s].[TicketStatus], [s].[UpdatedAt], [s].[UserId]
      FROM [SupportTicket] AS [s]
      WHERE [s].[UserId] = @__userId_0
      ORDER BY [s].[CreatedAt] DESC
info: Microsoft.AspNetCore.Mvc.ViewFeatures.ViewResultExecutor[1]
      Executing ViewResult, running view ~/Views/Customer/SupportList.cshtml.
info: Microsoft.AspNetCore.Mvc.ViewFeatures.ViewResultExecutor[4]
      Executed ViewResult - view ~/Views/Customer/SupportList.cshtml executed in 9.073ms.
info: Microsoft.AspNetCore.Mvc.Infrastructure.ControllerActionInvoker[105]
      Executed action WebApplication.Controllers.SupportController.Index (WebApplication) in 76.0548ms
info: Microsoft.AspNetCore.Routing.EndpointMiddleware[1]
      Executed endpoint 'WebApplication.Controllers.SupportController.Index (WebApplication)'
info: Microsoft.AspNetCore.Hosting.Diagnostics[1]
      Request starting HTTP/2 GET https://localhost:7177/_framework/aspnetcore-browser-refresh.js - - -
info: Microsoft.AspNetCore.Hosting.Diagnostics[1]
      Request starting HTTP/2 GET https://localhost:7177/_vs/browserLink - - -
info: Microsoft.AspNetCore.Hosting.Diagnostics[2]
      Request finished HTTP/2 GET https://localhost:7177/Support - 200 - text/html;+charset=utf-8 91.5590ms
info: Microsoft.AspNetCore.Hosting.Diagnostics[2]
      Request finished HTTP/2 GET https://localhost:7177/_framework/aspnetcore-browser-refresh.js - 200 14933 application/javascript;+charset=utf-8 14.9346ms
info: Microsoft.AspNetCore.Hosting.Diagnostics[1]
      Request starting HTTP/2 GET https://localhost:7177/Cart/GetCartCount - - -
info: Microsoft.AspNetCore.Hosting.Diagnostics[1]
      Request starting HTTP/2 GET https://localhost:7177/antiforgery/token - - -
info: Microsoft.AspNetCore.Hosting.Diagnostics[1]
      Request starting HTTP/2 GET https://localhost:7177/Customer/NotificationCount - - -
info: Microsoft.AspNetCore.Routing.EndpointMiddleware[0]
      Executing endpoint 'HTTP: GET /antiforgery/token'
info: Microsoft.AspNetCore.Routing.EndpointMiddleware[0]
      Executing endpoint 'WebApplication.Controllers.CartController.GetCartCount (WebApplication)'
info: Microsoft.AspNetCore.Routing.EndpointMiddleware[0]
      Executing endpoint 'WebApplication.Controllers.CustomerController.NotificationCount (WebApplication)'
info: Microsoft.AspNetCore.Http.Result.OkObjectResult[1]
      Setting HTTP status code 200.
info: Microsoft.AspNetCore.Mvc.Infrastructure.ControllerActionInvoker[102]
      Route matched with {action = "GetCartCount", controller = "Cart"}. Executing controller action with signature System.Threading.Tasks.Task`1[Microsoft.AspNetCore.Mvc.IActionResult] GetCartCount(System.Threading.CancellationToken) on controller WebApplication.Controllers.CartController (WebApplication).
info: Microsoft.AspNetCore.Mvc.Infrastructure.ControllerActionInvoker[102]
      Route matched with {action = "NotificationCount", controller = "Customer"}. Executing controller action with signature System.Threading.Tasks.Task`1[Microsoft.AspNetCore.Mvc.IActionResult] NotificationCount(System.Threading.CancellationToken) on controller WebApplication.Controllers.CustomerController (WebApplication).
info: Microsoft.AspNetCore.Http.Result.OkObjectResult[3]
      Writing value of type '<>f__AnonymousType21`1' as Json.
info: Microsoft.AspNetCore.Routing.EndpointMiddleware[1]
      Executed endpoint 'HTTP: GET /antiforgery/token'
info: Microsoft.AspNetCore.Hosting.Diagnostics[2]
      Request finished HTTP/2 GET https://localhost:7177/antiforgery/token - 200 - application/json;+charset=utf-8 65.4287ms
info: Microsoft.AspNetCore.Hosting.Diagnostics[2]
      Request finished HTTP/2 GET https://localhost:7177/_vs/browserLink - 200 - text/javascript;+charset=UTF-8 92.7302ms
info: Microsoft.EntityFrameworkCore.Database.Command[20101]
      Executed DbCommand (57ms) [Parameters=[@__userId_Value_0='?' (DbType = Int32)], CommandType='Text', CommandTimeout='30']
      SELECT TOP(1) [c].[CartId], [c].[CreatedAt], [c].[GuestSessionId], [c].[IsCheckedOut], [c].[LastUpdatedAt], [c].[UserId]
      FROM [Cart] AS [c]
      WHERE [c].[IsCheckedOut] = CAST(0 AS bit) AND [c].[UserId] = @__userId_Value_0
      ORDER BY [c].[CartId]
info: Microsoft.EntityFrameworkCore.Database.Command[20101]
      Executed DbCommand (58ms) [Parameters=[@__userId_0='?' (DbType = Int32)], CommandType='Text', CommandTimeout='30']
      SELECT COUNT(*)
      FROM [Notification] AS [n]
      WHERE [n].[UserId] = @__userId_0 AND [n].[IsRead] = CAST(0 AS bit) AND [n].[Channel] = N'InApp'
info: Microsoft.AspNetCore.Mvc.Infrastructure.SystemTextJsonResultExecutor[1]
      Executing JsonResult, writing value of type 'WebApplication.Models.ApiResponse'.
info: Microsoft.AspNetCore.Mvc.Infrastructure.SystemTextJsonResultExecutor[1]
      Executing JsonResult, writing value of type '<>f__AnonymousType6`1[[System.Int32, System.Private.CoreLib, Version=8.0.0.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e]]'.
info: Microsoft.AspNetCore.Mvc.Infrastructure.ControllerActionInvoker[105]
      Executed action WebApplication.Controllers.CartController.GetCartCount (WebApplication) in 69.5076ms
info: Microsoft.AspNetCore.Mvc.Infrastructure.ControllerActionInvoker[105]
      Executed action WebApplication.Controllers.CustomerController.NotificationCount (WebApplication) in 69.2804ms
info: Microsoft.AspNetCore.Routing.EndpointMiddleware[1]
      Executed endpoint 'WebApplication.Controllers.CartController.GetCartCount (WebApplication)'
info: Microsoft.AspNetCore.Routing.EndpointMiddleware[1]
      Executed endpoint 'WebApplication.Controllers.CustomerController.NotificationCount (WebApplication)'
info: Microsoft.AspNetCore.Hosting.Diagnostics[2]
      Request finished HTTP/2 GET https://localhost:7177/Cart/GetCartCount - 200 - application/json;+charset=utf-8 128.5060ms
info: Microsoft.AspNetCore.Hosting.Diagnostics[2]
      Request finished HTTP/2 GET https://localhost:7177/Customer/NotificationCount - 200 - application/json;+charset=utf-8 130.0840ms
info: Microsoft.EntityFrameworkCore.Database.Command[20101]
      Executed DbCommand (58ms) [Parameters=[], CommandType='Text', CommandTimeout='30']
      SELECT COUNT(*)
      FROM [ProductVariant] AS [p]
info: Microsoft.AspNetCore.Hosting.Diagnostics[1]
      Request starting HTTP/2 GET https://localhost:7177/Support - - -
info: Microsoft.AspNetCore.Routing.EndpointMiddleware[0]
      Executing endpoint 'WebApplication.Controllers.SupportController.Index (WebApplication)'
info: Microsoft.AspNetCore.Mvc.Infrastructure.ControllerActionInvoker[102]
      Route matched with {action = "Index", controller = "Support"}. Executing controller action with signature System.Threading.Tasks.Task`1[Microsoft.AspNetCore.Mvc.IActionResult] Index(System.Threading.CancellationToken) on controller WebApplication.Controllers.SupportController (WebApplication).
info: Microsoft.EntityFrameworkCore.Database.Command[20101]
      Executed DbCommand (58ms) [Parameters=[@__userId_0='?' (DbType = Int32)], CommandType='Text', CommandTimeout='30']
      SELECT [s].[TicketId], [s].[AssignedToUserId], [s].[AttachmentBucket], [s].[AttachmentPath], [s].[AttachmentUrl], [s].[CreatedAt], [s].[Description], [s].[OrderId], [s].[ResolvedAt], [s].[Subject], [s].[TicketCategory], [s].[TicketSource], [s].[TicketStatus], [s].[UpdatedAt], [s].[UserId]
      FROM [SupportTicket] AS [s]
      WHERE [s].[UserId] = @__userId_0
      ORDER BY [s].[CreatedAt] DESC
info: Microsoft.AspNetCore.Mvc.ViewFeatures.ViewResultExecutor[1]
      Executing ViewResult, running view ~/Views/Customer/SupportList.cshtml.
info: Microsoft.AspNetCore.Mvc.ViewFeatures.ViewResultExecutor[4]
      Executed ViewResult - view ~/Views/Customer/SupportList.cshtml executed in 4.9014ms.
info: Microsoft.AspNetCore.Mvc.Infrastructure.ControllerActionInvoker[105]
      Executed action WebApplication.Controllers.SupportController.Index (WebApplication) in 67.1609ms
info: Microsoft.AspNetCore.Routing.EndpointMiddleware[1]
      Executed endpoint 'WebApplication.Controllers.SupportController.Index (WebApplication)'
info: Microsoft.AspNetCore.Hosting.Diagnostics[2]
      Request finished HTTP/2 GET https://localhost:7177/Support - 200 - text/html;+charset=utf-8 78.1202ms
info: Microsoft.AspNetCore.Hosting.Diagnostics[1]
      Request starting HTTP/2 GET https://localhost:7177/_vs/browserLink - - -
info: Microsoft.AspNetCore.Hosting.Diagnostics[1]
      Request starting HTTP/2 GET https://localhost:7177/_framework/aspnetcore-browser-refresh.js - - -
info: Microsoft.AspNetCore.Hosting.Diagnostics[2]
      Request finished HTTP/2 GET https://localhost:7177/_framework/aspnetcore-browser-refresh.js - 200 14933 application/javascript;+charset=utf-8 5.2837ms
info: Microsoft.AspNetCore.Hosting.Diagnostics[2]
      Request finished HTTP/2 GET https://localhost:7177/_vs/browserLink - 200 - text/javascript;+charset=UTF-8 15.4690ms
info: Microsoft.AspNetCore.Hosting.Diagnostics[1]
      Request starting HTTP/2 GET https://localhost:7177/antiforgery/token - - -
info: Microsoft.AspNetCore.Hosting.Diagnostics[1]
      Request starting HTTP/2 GET https://localhost:7177/Customer/NotificationCount - - -
info: Microsoft.AspNetCore.Hosting.Diagnostics[1]
      Request starting HTTP/2 GET https://localhost:7177/Cart/GetCartCount - - -
info: Microsoft.AspNetCore.Routing.EndpointMiddleware[0]
      Executing endpoint 'HTTP: GET /antiforgery/token'
info: Microsoft.AspNetCore.Routing.EndpointMiddleware[0]
      Executing endpoint 'WebApplication.Controllers.CustomerController.NotificationCount (WebApplication)'
info: Microsoft.AspNetCore.Routing.EndpointMiddleware[0]
      Executing endpoint 'WebApplication.Controllers.CartController.GetCartCount (WebApplication)'
info: Microsoft.AspNetCore.Http.Result.OkObjectResult[1]
      Setting HTTP status code 200.
info: Microsoft.AspNetCore.Mvc.Infrastructure.ControllerActionInvoker[102]
      Route matched with {action = "NotificationCount", controller = "Customer"}. Executing controller action with signature System.Threading.Tasks.Task`1[Microsoft.AspNetCore.Mvc.IActionResult] NotificationCount(System.Threading.CancellationToken) on controller WebApplication.Controllers.CustomerController (WebApplication).
info: Microsoft.AspNetCore.Http.Result.OkObjectResult[3]
      Writing value of type '<>f__AnonymousType21`1' as Json.
info: Microsoft.AspNetCore.Routing.EndpointMiddleware[1]
      Executed endpoint 'HTTP: GET /antiforgery/token'
info: Microsoft.AspNetCore.Mvc.Infrastructure.ControllerActionInvoker[102]
      Route matched with {action = "GetCartCount", controller = "Cart"}. Executing controller action with signature System.Threading.Tasks.Task`1[Microsoft.AspNetCore.Mvc.IActionResult] GetCartCount(System.Threading.CancellationToken) on controller WebApplication.Controllers.CartController (WebApplication).
info: Microsoft.AspNetCore.Hosting.Diagnostics[2]
      Request finished HTTP/2 GET https://localhost:7177/antiforgery/token - 200 - application/json;+charset=utf-8 18.0483ms
info: Microsoft.EntityFrameworkCore.Database.Command[20101]
      Executed DbCommand (58ms) [Parameters=[@__userId_0='?' (DbType = Int32)], CommandType='Text', CommandTimeout='30']
      SELECT COUNT(*)
      FROM [Notification] AS [n]
      WHERE [n].[UserId] = @__userId_0 AND [n].[IsRead] = CAST(0 AS bit) AND [n].[Channel] = N'InApp'
info: Microsoft.AspNetCore.Mvc.Infrastructure.SystemTextJsonResultExecutor[1]
      Executing JsonResult, writing value of type '<>f__AnonymousType6`1[[System.Int32, System.Private.CoreLib, Version=8.0.0.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e]]'.
info: Microsoft.AspNetCore.Mvc.Infrastructure.ControllerActionInvoker[105]
      Executed action WebApplication.Controllers.CustomerController.NotificationCount (WebApplication) in 65.1347ms
info: Microsoft.AspNetCore.Routing.EndpointMiddleware[1]
      Executed endpoint 'WebApplication.Controllers.CustomerController.NotificationCount (WebApplication)'
info: Microsoft.EntityFrameworkCore.Database.Command[20101]
      Executed DbCommand (56ms) [Parameters=[@__userId_Value_0='?' (DbType = Int32)], CommandType='Text', CommandTimeout='30']
      SELECT TOP(1) [c].[CartId], [c].[CreatedAt], [c].[GuestSessionId], [c].[IsCheckedOut], [c].[LastUpdatedAt], [c].[UserId]
      FROM [Cart] AS [c]
      WHERE [c].[IsCheckedOut] = CAST(0 AS bit) AND [c].[UserId] = @__userId_Value_0
      ORDER BY [c].[CartId]
info: Microsoft.AspNetCore.Hosting.Diagnostics[2]
      Request finished HTTP/2 GET https://localhost:7177/Customer/NotificationCount - 200 - application/json;+charset=utf-8 81.7772ms
info: Microsoft.AspNetCore.Mvc.Infrastructure.SystemTextJsonResultExecutor[1]
      Executing JsonResult, writing value of type 'WebApplication.Models.ApiResponse'.
info: Microsoft.AspNetCore.Mvc.Infrastructure.ControllerActionInvoker[105]
      Executed action WebApplication.Controllers.CartController.GetCartCount (WebApplication) in 67.9172ms
info: Microsoft.AspNetCore.Routing.EndpointMiddleware[1]
      Executed endpoint 'WebApplication.Controllers.CartController.GetCartCount (WebApplication)'
info: Microsoft.AspNetCore.Hosting.Diagnostics[2]
      Request finished HTTP/2 GET https://localhost:7177/Cart/GetCartCount - 200 - application/json;+charset=utf-8 90.8447ms
info: Microsoft.AspNetCore.Hosting.Diagnostics[1]
      Request starting HTTP/2 GET https://localhost:7177/Customer/Notifications - - -
info: Microsoft.AspNetCore.Routing.EndpointMiddleware[0]
      Executing endpoint 'WebApplication.Controllers.CustomerController.Notifications (WebApplication)'
info: Microsoft.AspNetCore.Mvc.Infrastructure.ControllerActionInvoker[102]
      Route matched with {action = "Notifications", controller = "Customer"}. Executing controller action with signature System.Threading.Tasks.Task`1[Microsoft.AspNetCore.Mvc.IActionResult] Notifications(Int32, System.Threading.CancellationToken) on controller WebApplication.Controllers.CustomerController (WebApplication).
info: Microsoft.EntityFrameworkCore.Database.Command[20101]
      Executed DbCommand (57ms) [Parameters=[@__userId_0='?' (DbType = Int32)], CommandType='Text', CommandTimeout='30']
      SELECT COUNT(*)
      FROM [Notification] AS [n]
      WHERE [n].[UserId] = @__userId_0 AND [n].[Channel] = N'InApp'
info: Microsoft.EntityFrameworkCore.Database.Command[20101]
      Executed DbCommand (57ms) [Parameters=[@__userId_0='?' (DbType = Int32), @__p_1='?' (DbType = Int32), @__p_2='?' (DbType = Int32)], CommandType='Text', CommandTimeout='30']
      SELECT [n].[NotificationId], [n].[Body], [n].[Channel], [n].[CreatedAt], [n].[FailureReason], [n].[IsRead], [n].[NotifType], [n].[OrderId], [n].[ReadAt], [n].[Recipient], [n].[RetryCount], [n].[SentAt], [n].[Status], [n].[Subject], [n].[TicketId], [n].[UserId]
      FROM [Notification] AS [n]
      WHERE [n].[UserId] = @__userId_0 AND [n].[Channel] = N'InApp'
      ORDER BY [n].[CreatedAt] DESC
      OFFSET @__p_1 ROWS FETCH NEXT @__p_2 ROWS ONLY
info: Microsoft.EntityFrameworkCore.Database.Command[20101]
      Executed DbCommand (56ms) [Parameters=[@__userId_0='?' (DbType = Int32)], CommandType='Text', CommandTimeout='30']
      SELECT COUNT(*)
      FROM [Notification] AS [n]
      WHERE [n].[UserId] = @__userId_0 AND [n].[IsRead] = CAST(0 AS bit) AND [n].[Channel] = N'InApp'
info: Microsoft.AspNetCore.Mvc.ViewFeatures.ViewResultExecutor[1]
      Executing ViewResult, running view Notifications.
info: Microsoft.AspNetCore.Mvc.ViewFeatures.ViewResultExecutor[4]
      Executed ViewResult - view Notifications executed in 10.8285ms.
info: Microsoft.AspNetCore.Mvc.Infrastructure.ControllerActionInvoker[105]
      Executed action WebApplication.Controllers.CustomerController.Notifications (WebApplication) in 197.653ms
info: Microsoft.AspNetCore.Routing.EndpointMiddleware[1]
      Executed endpoint 'WebApplication.Controllers.CustomerController.Notifications (WebApplication)'
info: Microsoft.AspNetCore.Hosting.Diagnostics[2]
      Request finished HTTP/2 GET https://localhost:7177/Customer/Notifications - 200 - text/html;+charset=utf-8 210.7381ms
info: Microsoft.AspNetCore.Hosting.Diagnostics[1]
      Request starting HTTP/2 GET https://localhost:7177/_vs/browserLink - - -
info: Microsoft.AspNetCore.Hosting.Diagnostics[1]
      Request starting HTTP/2 GET https://localhost:7177/_framework/aspnetcore-browser-refresh.js - - -
info: Microsoft.AspNetCore.Hosting.Diagnostics[2]
      Request finished HTTP/2 GET https://localhost:7177/_framework/aspnetcore-browser-refresh.js - 200 14933 application/javascript;+charset=utf-8 4.6573ms
info: Microsoft.AspNetCore.Hosting.Diagnostics[1]
      Request starting HTTP/2 GET https://localhost:7177/antiforgery/token - - -
info: Microsoft.AspNetCore.Hosting.Diagnostics[1]
      Request starting HTTP/2 GET https://localhost:7177/Cart/GetCartCount - - -
info: Microsoft.AspNetCore.Hosting.Diagnostics[1]
      Request starting HTTP/2 GET https://localhost:7177/Customer/NotificationCount - - -
info: Microsoft.AspNetCore.Routing.EndpointMiddleware[0]
      Executing endpoint 'WebApplication.Controllers.CartController.GetCartCount (WebApplication)'
info: Microsoft.AspNetCore.Routing.EndpointMiddleware[0]
      Executing endpoint 'HTTP: GET /antiforgery/token'
info: Microsoft.AspNetCore.Routing.EndpointMiddleware[0]
      Executing endpoint 'WebApplication.Controllers.CustomerController.NotificationCount (WebApplication)'
info: Microsoft.AspNetCore.Mvc.Infrastructure.ControllerActionInvoker[102]
      Route matched with {action = "GetCartCount", controller = "Cart"}. Executing controller action with signature System.Threading.Tasks.Task`1[Microsoft.AspNetCore.Mvc.IActionResult] GetCartCount(System.Threading.CancellationToken) on controller WebApplication.Controllers.CartController (WebApplication).
info: Microsoft.AspNetCore.Http.Result.OkObjectResult[1]
      Setting HTTP status code 200.
info: Microsoft.AspNetCore.Mvc.Infrastructure.ControllerActionInvoker[102]
      Route matched with {action = "NotificationCount", controller = "Customer"}. Executing controller action with signature System.Threading.Tasks.Task`1[Microsoft.AspNetCore.Mvc.IActionResult] NotificationCount(System.Threading.CancellationToken) on controller WebApplication.Controllers.CustomerController (WebApplication).
info: Microsoft.AspNetCore.Http.Result.OkObjectResult[3]
      Writing value of type '<>f__AnonymousType21`1' as Json.
info: Microsoft.AspNetCore.Hosting.Diagnostics[2]
      Request finished HTTP/2 GET https://localhost:7177/_vs/browserLink - 200 - text/javascript;+charset=UTF-8 33.9958ms
info: Microsoft.AspNetCore.Routing.EndpointMiddleware[1]
      Executed endpoint 'HTTP: GET /antiforgery/token'
info: Microsoft.AspNetCore.Hosting.Diagnostics[2]
      Request finished HTTP/2 GET https://localhost:7177/antiforgery/token - 200 - application/json;+charset=utf-8 23.9831ms
info: Microsoft.EntityFrameworkCore.Database.Command[20101]
      Executed DbCommand (57ms) [Parameters=[@__userId_0='?' (DbType = Int32)], CommandType='Text', CommandTimeout='30']
      SELECT COUNT(*)
      FROM [Notification] AS [n]
      WHERE [n].[UserId] = @__userId_0 AND [n].[IsRead] = CAST(0 AS bit) AND [n].[Channel] = N'InApp'
info: Microsoft.AspNetCore.Mvc.Infrastructure.SystemTextJsonResultExecutor[1]
      Executing JsonResult, writing value of type '<>f__AnonymousType6`1[[System.Int32, System.Private.CoreLib, Version=8.0.0.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e]]'.
info: Microsoft.AspNetCore.Mvc.Infrastructure.ControllerActionInvoker[105]
      Executed action WebApplication.Controllers.CustomerController.NotificationCount (WebApplication) in 66.6047ms
info: Microsoft.EntityFrameworkCore.Database.Command[20101]
      Executed DbCommand (59ms) [Parameters=[@__userId_Value_0='?' (DbType = Int32)], CommandType='Text', CommandTimeout='30']
      SELECT TOP(1) [c].[CartId], [c].[CreatedAt], [c].[GuestSessionId], [c].[IsCheckedOut], [c].[LastUpdatedAt], [c].[UserId]
      FROM [Cart] AS [c]
      WHERE [c].[IsCheckedOut] = CAST(0 AS bit) AND [c].[UserId] = @__userId_Value_0
      ORDER BY [c].[CartId]
info: Microsoft.AspNetCore.Routing.EndpointMiddleware[1]
      Executed endpoint 'WebApplication.Controllers.CustomerController.NotificationCount (WebApplication)'
info: Microsoft.AspNetCore.Mvc.Infrastructure.SystemTextJsonResultExecutor[1]
      Executing JsonResult, writing value of type 'WebApplication.Models.ApiResponse'.
info: Microsoft.AspNetCore.Hosting.Diagnostics[2]
      Request finished HTTP/2 GET https://localhost:7177/Customer/NotificationCount - 200 - application/json;+charset=utf-8 88.6680ms
info: Microsoft.AspNetCore.Mvc.Infrastructure.ControllerActionInvoker[105]
      Executed action WebApplication.Controllers.CartController.GetCartCount (WebApplication) in 69.7471ms
info: Microsoft.AspNetCore.Routing.EndpointMiddleware[1]
      Executed endpoint 'WebApplication.Controllers.CartController.GetCartCount (WebApplication)'
info: Microsoft.AspNetCore.Hosting.Diagnostics[2]
      Request finished HTTP/2 GET https://localhost:7177/Cart/GetCartCount - 200 - application/json;+charset=utf-8 94.2414ms
info: Microsoft.AspNetCore.Hosting.Diagnostics[1]
      Request starting HTTP/2 GET https://localhost:7177/Cart/ViewCart - - -
info: Microsoft.AspNetCore.Routing.EndpointMiddleware[0]
      Executing endpoint 'WebApplication.Controllers.CartController.ViewCart (WebApplication)'
info: Microsoft.AspNetCore.Mvc.Infrastructure.ControllerActionInvoker[102]
      Route matched with {action = "ViewCart", controller = "Cart"}. Executing controller action with signature System.Threading.Tasks.Task`1[Microsoft.AspNetCore.Mvc.IActionResult] ViewCart(System.Threading.CancellationToken) on controller WebApplication.Controllers.CartController (WebApplication).
info: Microsoft.EntityFrameworkCore.Database.Command[20101]
      Executed DbCommand (58ms) [Parameters=[@__userId_Value_0='?' (DbType = Int32)], CommandType='Text', CommandTimeout='30']
      SELECT TOP(1) [c].[CartId], [c].[CreatedAt], [c].[GuestSessionId], [c].[IsCheckedOut], [c].[LastUpdatedAt], [c].[UserId]
      FROM [Cart] AS [c]
      WHERE [c].[IsCheckedOut] = CAST(0 AS bit) AND [c].[UserId] = @__userId_Value_0
      ORDER BY [c].[CartId]
info: Microsoft.AspNetCore.Mvc.ViewFeatures.ViewResultExecutor[1]
      Executing ViewResult, running view ~/Views/Customer/Cart.cshtml.
info: Microsoft.AspNetCore.Mvc.ViewFeatures.ViewResultExecutor[4]
      Executed ViewResult - view ~/Views/Customer/Cart.cshtml executed in 10.6908ms.
info: Microsoft.AspNetCore.Mvc.Infrastructure.ControllerActionInvoker[105]
      Executed action WebApplication.Controllers.CartController.ViewCart (WebApplication) in 75.261ms
info: Microsoft.AspNetCore.Routing.EndpointMiddleware[1]
      Executed endpoint 'WebApplication.Controllers.CartController.ViewCart (WebApplication)'
info: Microsoft.AspNetCore.Hosting.Diagnostics[2]
      Request finished HTTP/2 GET https://localhost:7177/Cart/ViewCart - 200 - text/html;+charset=utf-8 88.8061ms
info: Microsoft.AspNetCore.Hosting.Diagnostics[1]
      Request starting HTTP/2 GET https://localhost:7177/_framework/aspnetcore-browser-refresh.js - - -
info: Microsoft.AspNetCore.Hosting.Diagnostics[1]
      Request starting HTTP/2 GET https://localhost:7177/_vs/browserLink - - -
info: Microsoft.AspNetCore.Hosting.Diagnostics[2]
      Request finished HTTP/2 GET https://localhost:7177/_framework/aspnetcore-browser-refresh.js - 200 14933 application/javascript;+charset=utf-8 7.3896ms
info: Microsoft.AspNetCore.Hosting.Diagnostics[1]
      Request starting HTTP/2 GET https://localhost:7177/Cart/GetCartCount - - -
info: Microsoft.AspNetCore.Hosting.Diagnostics[1]
      Request starting HTTP/2 GET https://localhost:7177/Customer/NotificationCount - - -
info: Microsoft.AspNetCore.Hosting.Diagnostics[2]
      Request finished HTTP/2 GET https://localhost:7177/_vs/browserLink - 200 - text/javascript;+charset=UTF-8 19.6746ms
info: Microsoft.AspNetCore.Routing.EndpointMiddleware[0]
      Executing endpoint 'WebApplication.Controllers.CartController.GetCartCount (WebApplication)'
info: Microsoft.AspNetCore.Routing.EndpointMiddleware[0]
      Executing endpoint 'WebApplication.Controllers.CustomerController.NotificationCount (WebApplication)'
info: Microsoft.AspNetCore.Mvc.Infrastructure.ControllerActionInvoker[102]
      Route matched with {action = "GetCartCount", controller = "Cart"}. Executing controller action with signature System.Threading.Tasks.Task`1[Microsoft.AspNetCore.Mvc.IActionResult] GetCartCount(System.Threading.CancellationToken) on controller WebApplication.Controllers.CartController (WebApplication).
info: Microsoft.AspNetCore.Mvc.Infrastructure.ControllerActionInvoker[102]
      Route matched with {action = "NotificationCount", controller = "Customer"}. Executing controller action with signature System.Threading.Tasks.Task`1[Microsoft.AspNetCore.Mvc.IActionResult] NotificationCount(System.Threading.CancellationToken) on controller WebApplication.Controllers.CustomerController (WebApplication).
info: Microsoft.EntityFrameworkCore.Database.Command[20101]
      Executed DbCommand (57ms) [Parameters=[@__userId_0='?' (DbType = Int32)], CommandType='Text', CommandTimeout='30']
      SELECT COUNT(*)
      FROM [Notification] AS [n]
      WHERE [n].[UserId] = @__userId_0 AND [n].[IsRead] = CAST(0 AS bit) AND [n].[Channel] = N'InApp'
info: Microsoft.EntityFrameworkCore.Database.Command[20101]
      Executed DbCommand (59ms) [Parameters=[@__userId_Value_0='?' (DbType = Int32)], CommandType='Text', CommandTimeout='30']
      SELECT TOP(1) [c].[CartId], [c].[CreatedAt], [c].[GuestSessionId], [c].[IsCheckedOut], [c].[LastUpdatedAt], [c].[UserId]
      FROM [Cart] AS [c]
      WHERE [c].[IsCheckedOut] = CAST(0 AS bit) AND [c].[UserId] = @__userId_Value_0
      ORDER BY [c].[CartId]
info: Microsoft.AspNetCore.Mvc.Infrastructure.SystemTextJsonResultExecutor[1]
      Executing JsonResult, writing value of type '<>f__AnonymousType6`1[[System.Int32, System.Private.CoreLib, Version=8.0.0.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e]]'.
info: Microsoft.AspNetCore.Mvc.Infrastructure.SystemTextJsonResultExecutor[1]
      Executing JsonResult, writing value of type 'WebApplication.Models.ApiResponse'.
info: Microsoft.AspNetCore.Mvc.Infrastructure.ControllerActionInvoker[105]
      Executed action WebApplication.Controllers.CustomerController.NotificationCount (WebApplication) in 64.6361ms
info: Microsoft.AspNetCore.Mvc.Infrastructure.ControllerActionInvoker[105]
      Executed action WebApplication.Controllers.CartController.GetCartCount (WebApplication) in 70.03ms
info: Microsoft.AspNetCore.Routing.EndpointMiddleware[1]
      Executed endpoint 'WebApplication.Controllers.CustomerController.NotificationCount (WebApplication)'
info: Microsoft.AspNetCore.Routing.EndpointMiddleware[1]
      Executed endpoint 'WebApplication.Controllers.CartController.GetCartCount (WebApplication)'
info: Microsoft.AspNetCore.Hosting.Diagnostics[2]
      Request finished HTTP/2 GET https://localhost:7177/Customer/NotificationCount - 200 - application/json;+charset=utf-8 83.6471ms
info: Microsoft.AspNetCore.Hosting.Diagnostics[2]
      Request finished HTTP/2 GET https://localhost:7177/Cart/GetCartCount - 200 - application/json;+charset=utf-8 86.7235ms
info: Microsoft.AspNetCore.Hosting.Diagnostics[1]
      Request starting HTTP/2 GET https://localhost:7177/Customer/Notifications - - -
info: Microsoft.AspNetCore.Routing.EndpointMiddleware[0]
      Executing endpoint 'WebApplication.Controllers.CustomerController.Notifications (WebApplication)'
info: Microsoft.AspNetCore.Mvc.Infrastructure.ControllerActionInvoker[102]
      Route matched with {action = "Notifications", controller = "Customer"}. Executing controller action with signature System.Threading.Tasks.Task`1[Microsoft.AspNetCore.Mvc.IActionResult] Notifications(Int32, System.Threading.CancellationToken) on controller WebApplication.Controllers.CustomerController (WebApplication).
info: Microsoft.EntityFrameworkCore.Database.Command[20101]
      Executed DbCommand (58ms) [Parameters=[@__userId_0='?' (DbType = Int32)], CommandType='Text', CommandTimeout='30']
      SELECT COUNT(*)
      FROM [Notification] AS [n]
      WHERE [n].[UserId] = @__userId_0 AND [n].[Channel] = N'InApp'
info: Microsoft.EntityFrameworkCore.Database.Command[20101]
      Executed DbCommand (58ms) [Parameters=[@__userId_0='?' (DbType = Int32), @__p_1='?' (DbType = Int32), @__p_2='?' (DbType = Int32)], CommandType='Text', CommandTimeout='30']
      SELECT [n].[NotificationId], [n].[Body], [n].[Channel], [n].[CreatedAt], [n].[FailureReason], [n].[IsRead], [n].[NotifType], [n].[OrderId], [n].[ReadAt], [n].[Recipient], [n].[RetryCount], [n].[SentAt], [n].[Status], [n].[Subject], [n].[TicketId], [n].[UserId]
      FROM [Notification] AS [n]
      WHERE [n].[UserId] = @__userId_0 AND [n].[Channel] = N'InApp'
      ORDER BY [n].[CreatedAt] DESC
      OFFSET @__p_1 ROWS FETCH NEXT @__p_2 ROWS ONLY
info: Microsoft.EntityFrameworkCore.Database.Command[20101]
      Executed DbCommand (58ms) [Parameters=[@__userId_0='?' (DbType = Int32)], CommandType='Text', CommandTimeout='30']
      SELECT COUNT(*)
      FROM [Notification] AS [n]
      WHERE [n].[UserId] = @__userId_0 AND [n].[IsRead] = CAST(0 AS bit) AND [n].[Channel] = N'InApp'
info: Microsoft.AspNetCore.Mvc.ViewFeatures.ViewResultExecutor[1]
      Executing ViewResult, running view Notifications.
info: Microsoft.AspNetCore.Mvc.ViewFeatures.ViewResultExecutor[4]
      Executed ViewResult - view Notifications executed in 5.8132ms.
info: Microsoft.AspNetCore.Mvc.Infrastructure.ControllerActionInvoker[105]
      Executed action WebApplication.Controllers.CustomerController.Notifications (WebApplication) in 189.5437ms
info: Microsoft.AspNetCore.Routing.EndpointMiddleware[1]
      Executed endpoint 'WebApplication.Controllers.CustomerController.Notifications (WebApplication)'
info: Microsoft.AspNetCore.Hosting.Diagnostics[2]
      Request finished HTTP/2 GET https://localhost:7177/Customer/Notifications - 200 - text/html;+charset=utf-8 200.7800ms
info: Microsoft.AspNetCore.Hosting.Diagnostics[1]
      Request starting HTTP/2 GET https://localhost:7177/_framework/aspnetcore-browser-refresh.js - - -
info: Microsoft.AspNetCore.Hosting.Diagnostics[1]
      Request starting HTTP/2 GET https://localhost:7177/_vs/browserLink - - -
info: Microsoft.AspNetCore.Hosting.Diagnostics[2]
      Request finished HTTP/2 GET https://localhost:7177/_framework/aspnetcore-browser-refresh.js - 200 14933 application/javascript;+charset=utf-8 4.6102ms
info: Microsoft.AspNetCore.Hosting.Diagnostics[1]
      Request starting HTTP/2 GET https://localhost:7177/Cart/GetCartCount - - -
info: Microsoft.AspNetCore.Hosting.Diagnostics[1]
      Request starting HTTP/2 GET https://localhost:7177/Customer/NotificationCount - - -
info: Microsoft.AspNetCore.Hosting.Diagnostics[1]
      Request starting HTTP/2 GET https://localhost:7177/antiforgery/token - - -
info: Microsoft.AspNetCore.Routing.EndpointMiddleware[0]
      Executing endpoint 'WebApplication.Controllers.CartController.GetCartCount (WebApplication)'
info: Microsoft.AspNetCore.Hosting.Diagnostics[2]
      Request finished HTTP/2 GET https://localhost:7177/_vs/browserLink - 200 - text/javascript;+charset=UTF-8 17.9311ms
info: Microsoft.AspNetCore.Routing.EndpointMiddleware[0]
      Executing endpoint 'WebApplication.Controllers.CustomerController.NotificationCount (WebApplication)'
info: Microsoft.AspNetCore.Mvc.Infrastructure.ControllerActionInvoker[102]
      Route matched with {action = "GetCartCount", controller = "Cart"}. Executing controller action with signature System.Threading.Tasks.Task`1[Microsoft.AspNetCore.Mvc.IActionResult] GetCartCount(System.Threading.CancellationToken) on controller WebApplication.Controllers.CartController (WebApplication).
info: Microsoft.AspNetCore.Routing.EndpointMiddleware[0]
      Executing endpoint 'HTTP: GET /antiforgery/token'
info: Microsoft.AspNetCore.Http.Result.OkObjectResult[1]
      Setting HTTP status code 200.
info: Microsoft.AspNetCore.Mvc.Infrastructure.ControllerActionInvoker[102]
      Route matched with {action = "NotificationCount", controller = "Customer"}. Executing controller action with signature System.Threading.Tasks.Task`1[Microsoft.AspNetCore.Mvc.IActionResult] NotificationCount(System.Threading.CancellationToken) on controller WebApplication.Controllers.CustomerController (WebApplication).
info: Microsoft.AspNetCore.Http.Result.OkObjectResult[3]
      Writing value of type '<>f__AnonymousType21`1' as Json.
info: Microsoft.AspNetCore.Routing.EndpointMiddleware[1]
      Executed endpoint 'HTTP: GET /antiforgery/token'
info: Microsoft.AspNetCore.Hosting.Diagnostics[2]
      Request finished HTTP/2 GET https://localhost:7177/antiforgery/token - 200 - application/json;+charset=utf-8 23.2681ms
info: Microsoft.EntityFrameworkCore.Database.Command[20101]
      Executed DbCommand (60ms) [Parameters=[@__userId_Value_0='?' (DbType = Int32)], CommandType='Text', CommandTimeout='30']
      SELECT TOP(1) [c].[CartId], [c].[CreatedAt], [c].[GuestSessionId], [c].[IsCheckedOut], [c].[LastUpdatedAt], [c].[UserId]
      FROM [Cart] AS [c]
      WHERE [c].[IsCheckedOut] = CAST(0 AS bit) AND [c].[UserId] = @__userId_Value_0
      ORDER BY [c].[CartId]
info: Microsoft.EntityFrameworkCore.Database.Command[20101]
      Executed DbCommand (58ms) [Parameters=[@__userId_0='?' (DbType = Int32)], CommandType='Text', CommandTimeout='30']
      SELECT COUNT(*)
      FROM [Notification] AS [n]
      WHERE [n].[UserId] = @__userId_0 AND [n].[IsRead] = CAST(0 AS bit) AND [n].[Channel] = N'InApp'
info: Microsoft.AspNetCore.Mvc.Infrastructure.SystemTextJsonResultExecutor[1]
      Executing JsonResult, writing value of type 'WebApplication.Models.ApiResponse'.
info: Microsoft.AspNetCore.Mvc.Infrastructure.SystemTextJsonResultExecutor[1]
      Executing JsonResult, writing value of type '<>f__AnonymousType6`1[[System.Int32, System.Private.CoreLib, Version=8.0.0.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e]]'.
info: Microsoft.AspNetCore.Mvc.Infrastructure.ControllerActionInvoker[105]
      Executed action WebApplication.Controllers.CartController.GetCartCount (WebApplication) in 72.2561ms
info: Microsoft.AspNetCore.Mvc.Infrastructure.ControllerActionInvoker[105]
      Executed action WebApplication.Controllers.CustomerController.NotificationCount (WebApplication) in 66.1203ms
info: Microsoft.AspNetCore.Routing.EndpointMiddleware[1]
      Executed endpoint 'WebApplication.Controllers.CartController.GetCartCount (WebApplication)'
info: Microsoft.AspNetCore.Routing.EndpointMiddleware[1]
      Executed endpoint 'WebApplication.Controllers.CustomerController.NotificationCount (WebApplication)'
info: Microsoft.AspNetCore.Hosting.Diagnostics[2]
      Request finished HTTP/2 GET https://localhost:7177/Cart/GetCartCount - 200 - application/json;+charset=utf-8 92.0986ms
info: Microsoft.AspNetCore.Hosting.Diagnostics[2]
      Request finished HTTP/2 GET https://localhost:7177/Customer/NotificationCount - 200 - application/json;+charset=utf-8 90.7920ms
info: Microsoft.AspNetCore.Hosting.Diagnostics[1]
      Request starting HTTP/2 GET https://localhost:7177/Customer/Dashboard - - -
info: Microsoft.AspNetCore.Routing.EndpointMiddleware[0]
      Executing endpoint 'WebApplication.Controllers.CustomerController.Dashboard (WebApplication)'
info: Microsoft.AspNetCore.Mvc.Infrastructure.ControllerActionInvoker[102]
      Route matched with {action = "Dashboard", controller = "Customer"}. Executing controller action with signature System.Threading.Tasks.Task`1[Microsoft.AspNetCore.Mvc.IActionResult] Dashboard(System.Threading.CancellationToken) on controller WebApplication.Controllers.CustomerController (WebApplication).
info: Microsoft.EntityFrameworkCore.Database.Command[20101]
      Executed DbCommand (118ms) [Parameters=[@__userId_0='?' (DbType = Int32), @__p_1='?' (DbType = Int32), @__p_2='?' (DbType = Int32)], CommandType='Text', CommandTimeout='30']
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
      Executed DbCommand (57ms) [Parameters=[@__userId_0='?' (DbType = Int32), @__p_1='?' (DbType = Int32), @__p_2='?' (DbType = Int32)], CommandType='Text', CommandTimeout='30']
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
      Executed DbCommand (56ms) [Parameters=[@__userId_0='?' (DbType = Int32)], CommandType='Text', CommandTimeout='30']
      SELECT COUNT(*)
      FROM [Order] AS [o]
      WHERE [o].[IsDeleted] = CAST(0 AS bit) AND [o].[UserId] = @__userId_0
info: Microsoft.EntityFrameworkCore.Database.Command[20101]
      Executed DbCommand (56ms) [Parameters=[@__userId_0='?' (DbType = Int32)], CommandType='Text', CommandTimeout='30']
      SELECT COUNT(*)
      FROM [Wishlist] AS [w]
      WHERE [w].[UserId] = @__userId_0
info: Microsoft.EntityFrameworkCore.Database.Command[20101]
      Executed DbCommand (56ms) [Parameters=[@__userId_0='?' (DbType = Int32)], CommandType='Text', CommandTimeout='30']
      SELECT [r].[ProductId]
      FROM [Review] AS [r]
      WHERE [r].[UserId] = @__userId_0
info: Microsoft.EntityFrameworkCore.Database.Command[20101]
      Executed DbCommand (57ms) [Parameters=[@__userId_0='?' (DbType = Int32), @__reviewedProductIds_1='?' (Size = 4000)], CommandType='Text', CommandTimeout='30']
      SELECT DISTINCT [o].[ProductId], [o].[OrderId], [p].[Name] AS [ProductName]
      FROM [OrderItem] AS [o]
      INNER JOIN (
          SELECT [o0].[OrderId], [o0].[OrderStatus], [o0].[UserId]
          FROM [Order] AS [o0]
          WHERE [o0].[IsDeleted] = CAST(0 AS bit)
      ) AS [t] ON [o].[OrderId] = [t].[OrderId]
      INNER JOIN [Product] AS [p] ON [o].[ProductId] = [p].[ProductId]
      WHERE [t].[UserId] = @__userId_0 AND [t].[OrderStatus] = N'Delivered' AND [o].[ProductId] NOT IN (
          SELECT [r].[value]
          FROM OPENJSON(@__reviewedProductIds_1) WITH ([value] int '$') AS [r]
      )
info: Microsoft.EntityFrameworkCore.Database.Command[20101]
      Executed DbCommand (56ms) [Parameters=[@__userId_0='?' (DbType = Int32)], CommandType='Text', CommandTimeout='30']
      SELECT [s].[TicketId], [s].[AssignedToUserId], [s].[AttachmentBucket], [s].[AttachmentPath], [s].[AttachmentUrl], [s].[CreatedAt], [s].[Description], [s].[OrderId], [s].[ResolvedAt], [s].[Subject], [s].[TicketCategory], [s].[TicketSource], [s].[TicketStatus], [s].[UpdatedAt], [s].[UserId]
      FROM [SupportTicket] AS [s]
      WHERE [s].[UserId] = @__userId_0
      ORDER BY [s].[CreatedAt] DESC
info: Microsoft.AspNetCore.Mvc.ViewFeatures.ViewResultExecutor[1]
      Executing ViewResult, running view Dashboard.
info: Microsoft.AspNetCore.Mvc.ViewFeatures.ViewResultExecutor[4]
      Executed ViewResult - view Dashboard executed in 6.2724ms.
info: Microsoft.AspNetCore.Mvc.Infrastructure.ControllerActionInvoker[105]
      Executed action WebApplication.Controllers.CustomerController.Dashboard (WebApplication) in 482.5382ms
info: Microsoft.AspNetCore.Routing.EndpointMiddleware[1]
      Executed endpoint 'WebApplication.Controllers.CustomerController.Dashboard (WebApplication)'
info: Microsoft.AspNetCore.Hosting.Diagnostics[1]
      Request starting HTTP/2 GET https://localhost:7177/_vs/browserLink - - -
info: Microsoft.AspNetCore.Hosting.Diagnostics[1]
      Request starting HTTP/2 GET https://localhost:7177/_framework/aspnetcore-browser-refresh.js - - -
info: Microsoft.AspNetCore.Hosting.Diagnostics[2]
      Request finished HTTP/2 GET https://localhost:7177/Customer/Dashboard - 200 - text/html;+charset=utf-8 493.7536ms
info: Microsoft.AspNetCore.Hosting.Diagnostics[2]
      Request finished HTTP/2 GET https://localhost:7177/_framework/aspnetcore-browser-refresh.js - 200 14933 application/javascript;+charset=utf-8 6.4546ms
info: Microsoft.AspNetCore.Hosting.Diagnostics[2]
      Request finished HTTP/2 GET https://localhost:7177/_vs/browserLink - 200 - text/javascript;+charset=UTF-8 15.3626ms
info: Microsoft.AspNetCore.Hosting.Diagnostics[1]
      Request starting HTTP/2 GET https://localhost:7177/Cart/GetCartCount - - -
info: Microsoft.AspNetCore.Hosting.Diagnostics[1]
      Request starting HTTP/2 GET https://localhost:7177/Customer/NotificationCount - - -
info: Microsoft.AspNetCore.Hosting.Diagnostics[1]
      Request starting HTTP/2 GET https://localhost:7177/antiforgery/token - - -
info: Microsoft.AspNetCore.Routing.EndpointMiddleware[0]
      Executing endpoint 'WebApplication.Controllers.CustomerController.NotificationCount (WebApplication)'
info: Microsoft.AspNetCore.Routing.EndpointMiddleware[0]
      Executing endpoint 'WebApplication.Controllers.CartController.GetCartCount (WebApplication)'
info: Microsoft.AspNetCore.Mvc.Infrastructure.ControllerActionInvoker[102]
      Route matched with {action = "NotificationCount", controller = "Customer"}. Executing controller action with signature System.Threading.Tasks.Task`1[Microsoft.AspNetCore.Mvc.IActionResult] NotificationCount(System.Threading.CancellationToken) on controller WebApplication.Controllers.CustomerController (WebApplication).
info: Microsoft.AspNetCore.Mvc.Infrastructure.ControllerActionInvoker[102]
      Route matched with {action = "GetCartCount", controller = "Cart"}. Executing controller action with signature System.Threading.Tasks.Task`1[Microsoft.AspNetCore.Mvc.IActionResult] GetCartCount(System.Threading.CancellationToken) on controller WebApplication.Controllers.CartController (WebApplication).
info: Microsoft.AspNetCore.Routing.EndpointMiddleware[0]
      Executing endpoint 'HTTP: GET /antiforgery/token'
info: Microsoft.AspNetCore.Http.Result.OkObjectResult[1]
      Setting HTTP status code 200.
info: Microsoft.AspNetCore.Http.Result.OkObjectResult[3]
      Writing value of type '<>f__AnonymousType21`1' as Json.
info: Microsoft.AspNetCore.Routing.EndpointMiddleware[1]
      Executed endpoint 'HTTP: GET /antiforgery/token'
info: Microsoft.AspNetCore.Hosting.Diagnostics[2]
      Request finished HTTP/2 GET https://localhost:7177/antiforgery/token - 200 - application/json;+charset=utf-8 27.5247ms
info: Microsoft.EntityFrameworkCore.Database.Command[20101]
      Executed DbCommand (57ms) [Parameters=[@__userId_0='?' (DbType = Int32)], CommandType='Text', CommandTimeout='30']
      SELECT COUNT(*)
      FROM [Notification] AS [n]
      WHERE [n].[UserId] = @__userId_0 AND [n].[IsRead] = CAST(0 AS bit) AND [n].[Channel] = N'InApp'
info: Microsoft.EntityFrameworkCore.Database.Command[20101]
      Executed DbCommand (59ms) [Parameters=[@__userId_Value_0='?' (DbType = Int32)], CommandType='Text', CommandTimeout='30']
      SELECT TOP(1) [c].[CartId], [c].[CreatedAt], [c].[GuestSessionId], [c].[IsCheckedOut], [c].[LastUpdatedAt], [c].[UserId]
      FROM [Cart] AS [c]
      WHERE [c].[IsCheckedOut] = CAST(0 AS bit) AND [c].[UserId] = @__userId_Value_0
      ORDER BY [c].[CartId]
info: Microsoft.AspNetCore.Mvc.Infrastructure.SystemTextJsonResultExecutor[1]
      Executing JsonResult, writing value of type '<>f__AnonymousType6`1[[System.Int32, System.Private.CoreLib, Version=8.0.0.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e]]'.
info: Microsoft.AspNetCore.Mvc.Infrastructure.SystemTextJsonResultExecutor[1]
      Executing JsonResult, writing value of type 'WebApplication.Models.ApiResponse'.
info: Microsoft.AspNetCore.Mvc.Infrastructure.ControllerActionInvoker[105]
      Executed action WebApplication.Controllers.CustomerController.NotificationCount (WebApplication) in 67.2348ms
info: Microsoft.AspNetCore.Mvc.Infrastructure.ControllerActionInvoker[105]
      Executed action WebApplication.Controllers.CartController.GetCartCount (WebApplication) in 66.7093ms
info: Microsoft.AspNetCore.Routing.EndpointMiddleware[1]
      Executed endpoint 'WebApplication.Controllers.CustomerController.NotificationCount (WebApplication)'
info: Microsoft.AspNetCore.Routing.EndpointMiddleware[1]
      Executed endpoint 'WebApplication.Controllers.CartController.GetCartCount (WebApplication)'
info: Microsoft.AspNetCore.Hosting.Diagnostics[2]
      Request finished HTTP/2 GET https://localhost:7177/Customer/NotificationCount - 200 - application/json;+charset=utf-8 83.8307ms
info: Microsoft.AspNetCore.Hosting.Diagnostics[2]
      Request finished HTTP/2 GET https://localhost:7177/Cart/GetCartCount - 200 - application/json;+charset=utf-8 85.5728ms
info: Microsoft.AspNetCore.Hosting.Diagnostics[1]
      Request starting HTTP/2 GET https://localhost:7177/Support - - -
info: Microsoft.AspNetCore.Routing.EndpointMiddleware[0]
      Executing endpoint 'WebApplication.Controllers.SupportController.Index (WebApplication)'
info: Microsoft.AspNetCore.Mvc.Infrastructure.ControllerActionInvoker[102]
      Route matched with {action = "Index", controller = "Support"}. Executing controller action with signature System.Threading.Tasks.Task`1[Microsoft.AspNetCore.Mvc.IActionResult] Index(System.Threading.CancellationToken) on controller WebApplication.Controllers.SupportController (WebApplication).
info: Microsoft.EntityFrameworkCore.Database.Command[20101]
      Executed DbCommand (58ms) [Parameters=[@__userId_0='?' (DbType = Int32)], CommandType='Text', CommandTimeout='30']
      SELECT [s].[TicketId], [s].[AssignedToUserId], [s].[AttachmentBucket], [s].[AttachmentPath], [s].[AttachmentUrl], [s].[CreatedAt], [s].[Description], [s].[OrderId], [s].[ResolvedAt], [s].[Subject], [s].[TicketCategory], [s].[TicketSource], [s].[TicketStatus], [s].[UpdatedAt], [s].[UserId]
      FROM [SupportTicket] AS [s]
      WHERE [s].[UserId] = @__userId_0
      ORDER BY [s].[CreatedAt] DESC
info: Microsoft.AspNetCore.Mvc.ViewFeatures.ViewResultExecutor[1]
      Executing ViewResult, running view ~/Views/Customer/SupportList.cshtml.
info: Microsoft.AspNetCore.Mvc.ViewFeatures.ViewResultExecutor[4]
      Executed ViewResult - view ~/Views/Customer/SupportList.cshtml executed in 3.9786ms.
info: Microsoft.AspNetCore.Mvc.Infrastructure.ControllerActionInvoker[105]
      Executed action WebApplication.Controllers.SupportController.Index (WebApplication) in 66.3784ms
info: Microsoft.AspNetCore.Routing.EndpointMiddleware[1]
      Executed endpoint 'WebApplication.Controllers.SupportController.Index (WebApplication)'
info: Microsoft.AspNetCore.Hosting.Diagnostics[2]
      Request finished HTTP/2 GET https://localhost:7177/Support - 200 - text/html;+charset=utf-8 76.9497ms
info: Microsoft.AspNetCore.Hosting.Diagnostics[1]
      Request starting HTTP/2 GET https://localhost:7177/_framework/aspnetcore-browser-refresh.js - - -
info: Microsoft.AspNetCore.Hosting.Diagnostics[1]
      Request starting HTTP/2 GET https://localhost:7177/_vs/browserLink - - -
info: Microsoft.AspNetCore.Hosting.Diagnostics[2]
      Request finished HTTP/2 GET https://localhost:7177/_framework/aspnetcore-browser-refresh.js - 200 14933 application/javascript;+charset=utf-8 4.5937ms
info: Microsoft.AspNetCore.Hosting.Diagnostics[1]
      Request starting HTTP/2 GET https://localhost:7177/antiforgery/token - - -
info: Microsoft.AspNetCore.Hosting.Diagnostics[1]
      Request starting HTTP/2 GET https://localhost:7177/Customer/NotificationCount - - -
info: Microsoft.AspNetCore.Hosting.Diagnostics[1]
      Request starting HTTP/2 GET https://localhost:7177/Cart/GetCartCount - - -
info: Microsoft.AspNetCore.Hosting.Diagnostics[2]
      Request finished HTTP/2 GET https://localhost:7177/_vs/browserLink - 200 - text/javascript;+charset=UTF-8 15.1983ms
info: Microsoft.AspNetCore.Routing.EndpointMiddleware[0]
      Executing endpoint 'WebApplication.Controllers.CustomerController.NotificationCount (WebApplication)'
info: Microsoft.AspNetCore.Routing.EndpointMiddleware[0]
      Executing endpoint 'HTTP: GET /antiforgery/token'
info: Microsoft.AspNetCore.Routing.EndpointMiddleware[0]
      Executing endpoint 'WebApplication.Controllers.CartController.GetCartCount (WebApplication)'
info: Microsoft.AspNetCore.Http.Result.OkObjectResult[1]
      Setting HTTP status code 200.
info: Microsoft.AspNetCore.Mvc.Infrastructure.ControllerActionInvoker[102]
      Route matched with {action = "GetCartCount", controller = "Cart"}. Executing controller action with signature System.Threading.Tasks.Task`1[Microsoft.AspNetCore.Mvc.IActionResult] GetCartCount(System.Threading.CancellationToken) on controller WebApplication.Controllers.CartController (WebApplication).
info: Microsoft.AspNetCore.Http.Result.OkObjectResult[3]
      Writing value of type '<>f__AnonymousType21`1' as Json.
info: Microsoft.AspNetCore.Routing.EndpointMiddleware[1]
      Executed endpoint 'HTTP: GET /antiforgery/token'
info: Microsoft.AspNetCore.Mvc.Infrastructure.ControllerActionInvoker[102]
      Route matched with {action = "NotificationCount", controller = "Customer"}. Executing controller action with signature System.Threading.Tasks.Task`1[Microsoft.AspNetCore.Mvc.IActionResult] NotificationCount(System.Threading.CancellationToken) on controller WebApplication.Controllers.CustomerController (WebApplication).
info: Microsoft.AspNetCore.Hosting.Diagnostics[2]
      Request finished HTTP/2 GET https://localhost:7177/antiforgery/token - 200 - application/json;+charset=utf-8 24.6328ms
info: Microsoft.EntityFrameworkCore.Database.Command[20101]
      Executed DbCommand (60ms) [Parameters=[@__userId_Value_0='?' (DbType = Int32)], CommandType='Text', CommandTimeout='30']
      SELECT TOP(1) [c].[CartId], [c].[CreatedAt], [c].[GuestSessionId], [c].[IsCheckedOut], [c].[LastUpdatedAt], [c].[UserId]
      FROM [Cart] AS [c]
      WHERE [c].[IsCheckedOut] = CAST(0 AS bit) AND [c].[UserId] = @__userId_Value_0
      ORDER BY [c].[CartId]
info: Microsoft.AspNetCore.Mvc.Infrastructure.SystemTextJsonResultExecutor[1]
      Executing JsonResult, writing value of type 'WebApplication.Models.ApiResponse'.
info: Microsoft.AspNetCore.Mvc.Infrastructure.ControllerActionInvoker[105]
      Executed action WebApplication.Controllers.CartController.GetCartCount (WebApplication) in 67.7178ms
info: Microsoft.EntityFrameworkCore.Database.Command[20101]
      Executed DbCommand (57ms) [Parameters=[@__userId_0='?' (DbType = Int32)], CommandType='Text', CommandTimeout='30']
      SELECT COUNT(*)
      FROM [Notification] AS [n]
      WHERE [n].[UserId] = @__userId_0 AND [n].[IsRead] = CAST(0 AS bit) AND [n].[Channel] = N'InApp'
info: Microsoft.AspNetCore.Routing.EndpointMiddleware[1]
      Executed endpoint 'WebApplication.Controllers.CartController.GetCartCount (WebApplication)'
info: Microsoft.AspNetCore.Mvc.Infrastructure.SystemTextJsonResultExecutor[1]
      Executing JsonResult, writing value of type '<>f__AnonymousType6`1[[System.Int32, System.Private.CoreLib, Version=8.0.0.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e]]'.
info: Microsoft.AspNetCore.Hosting.Diagnostics[2]
      Request finished HTTP/2 GET https://localhost:7177/Cart/GetCartCount - 200 - application/json;+charset=utf-8 90.8589ms
info: Microsoft.AspNetCore.Mvc.Infrastructure.ControllerActionInvoker[105]
      Executed action WebApplication.Controllers.CustomerController.NotificationCount (WebApplication) in 66.8288ms
info: Microsoft.AspNetCore.Routing.EndpointMiddleware[1]
      Executed endpoint 'WebApplication.Controllers.CustomerController.NotificationCount (WebApplication)'
info: Microsoft.AspNetCore.Hosting.Diagnostics[2]
      Request finished HTTP/2 GET https://localhost:7177/Customer/NotificationCount - 200 - application/json;+charset=utf-8 96.5281ms
info: Microsoft.AspNetCore.Hosting.Diagnostics[1]
      Request starting HTTP/2 GET https://localhost:7177/Support/Detail?ticketId=1 - - -
info: Microsoft.AspNetCore.Routing.EndpointMiddleware[0]
      Executing endpoint 'WebApplication.Controllers.SupportController.Detail (WebApplication)'
info: Microsoft.AspNetCore.Mvc.Infrastructure.ControllerActionInvoker[102]
      Route matched with {action = "Detail", controller = "Support"}. Executing controller action with signature System.Threading.Tasks.Task`1[Microsoft.AspNetCore.Mvc.IActionResult] Detail(Int32, System.Threading.CancellationToken) on controller WebApplication.Controllers.SupportController (WebApplication).
info: Microsoft.EntityFrameworkCore.Database.Command[20101]
      Executed DbCommand (57ms) [Parameters=[@__ticketId_0='?' (DbType = Int32)], CommandType='Text', CommandTimeout='30']
      SELECT TOP(1) [s].[TicketId], [s].[AssignedToUserId], [s].[AttachmentBucket], [s].[AttachmentPath], [s].[AttachmentUrl], [s].[CreatedAt], [s].[Description], [s].[OrderId], [s].[ResolvedAt], [s].[Subject], [s].[TicketCategory], [s].[TicketSource], [s].[TicketStatus], [s].[UpdatedAt], [s].[UserId], [t].[UserId], [t].[CreatedAt], [t].[DefaultAddressId], [t].[Email], [t].[FailedLoginAttempts], [t].[FirstName], [t].[IsActive], [t].[IsDeleted], [t].[IsWalkIn], [t].[LastLoginAt], [t].[LastName], [t].[LockoutUntil], [t].[PasswordHash], [t].[PhoneNumber], [t0].[OrderId], [t0].[CartId], [t0].[ContactPhone], [t0].[CreatedAt], [t0].[DeliveryInstructions], [t0].[DiscountAmount], [t0].[FulfillmentType], [t0].[GuestSessionId], [t0].[IsDeleted], [t0].[IsWalkIn], [t0].[OrderDate], [t0].[OrderNumber], [t0].[OrderStatus], [t0].[POSSessionId], [t0].[PaymentMethod], [t0].[ShippingAddressId], [t0].[ShippingFee], [t0].[SubTotal], [t0].[TotalAmount], [t0].[UpdatedAt], [t0].[UserId], [t1].[UserId], [t1].[CreatedAt], [t1].[DefaultAddressId], [t1].[Email], [t1].[FailedLoginAttempts], [t1].[FirstName], [t1].[IsActive], [t1].[IsDeleted], [t1].[IsWalkIn], [t1].[LastLoginAt], [t1].[LastName], [t1].[LockoutUntil], [t1].[PasswordHash], [t1].[PhoneNumber]
      FROM [SupportTicket] AS [s]
      INNER JOIN (
          SELECT [u].[UserId], [u].[CreatedAt], [u].[DefaultAddressId], [u].[Email], [u].[FailedLoginAttempts], [u].[FirstName], [u].[IsActive], [u].[IsDeleted], [u].[IsWalkIn], [u].[LastLoginAt], [u].[LastName], [u].[LockoutUntil], [u].[PasswordHash], [u].[PhoneNumber]
          FROM [User] AS [u]
          WHERE [u].[IsDeleted] = CAST(0 AS bit)
      ) AS [t] ON [s].[UserId] = [t].[UserId]
      LEFT JOIN (
          SELECT [o].[OrderId], [o].[CartId], [o].[ContactPhone], [o].[CreatedAt], [o].[DeliveryInstructions], [o].[DiscountAmount], [o].[FulfillmentType], [o].[GuestSessionId], [o].[IsDeleted], [o].[IsWalkIn], [o].[OrderDate], [o].[OrderNumber], [o].[OrderStatus], [o].[POSSessionId], [o].[PaymentMethod], [o].[ShippingAddressId], [o].[ShippingFee], [o].[SubTotal], [o].[TotalAmount], [o].[UpdatedAt], [o].[UserId]
          FROM [Order] AS [o]
          WHERE [o].[IsDeleted] = CAST(0 AS bit)
      ) AS [t0] ON [s].[OrderId] = [t0].[OrderId]
      LEFT JOIN (
          SELECT [u0].[UserId], [u0].[CreatedAt], [u0].[DefaultAddressId], [u0].[Email], [u0].[FailedLoginAttempts], [u0].[FirstName], [u0].[IsActive], [u0].[IsDeleted], [u0].[IsWalkIn], [u0].[LastLoginAt], [u0].[LastName], [u0].[LockoutUntil], [u0].[PasswordHash], [u0].[PhoneNumber]
          FROM [User] AS [u0]
          WHERE [u0].[IsDeleted] = CAST(0 AS bit)
      ) AS [t1] ON [s].[AssignedToUserId] = [t1].[UserId]
      WHERE [s].[TicketId] = @__ticketId_0
      ORDER BY [s].[TicketId], [t].[UserId], [t0].[OrderId], [t1].[UserId]
info: Microsoft.EntityFrameworkCore.Database.Command[20101]
      Executed DbCommand (57ms) [Parameters=[@__ticketId_0='?' (DbType = Int32)], CommandType='Text', CommandTimeout='30']
      SELECT [t3].[ReplyId], [t3].[AttachmentBucket], [t3].[AttachmentPath], [t3].[AttachmentUrl], [t3].[CreatedAt], [t3].[IsAdminReply], [t3].[Message], [t3].[TicketId], [t3].[UserId], [t3].[UserId0], [t3].[CreatedAt0], [t3].[DefaultAddressId], [t3].[Email], [t3].[FailedLoginAttempts], [t3].[FirstName], [t3].[IsActive], [t3].[IsDeleted], [t3].[IsWalkIn], [t3].[LastLoginAt], [t3].[LastName], [t3].[LockoutUntil], [t3].[PasswordHash], [t3].[PhoneNumber], [t2].[TicketId], [t2].[UserId], [t2].[OrderId], [t2].[UserId0]
      FROM (
          SELECT TOP(1) [s].[TicketId], [t].[UserId], [t0].[OrderId], [t1].[UserId] AS [UserId0]
          FROM [SupportTicket] AS [s]
          INNER JOIN (
              SELECT [u].[UserId]
              FROM [User] AS [u]
              WHERE [u].[IsDeleted] = CAST(0 AS bit)
          ) AS [t] ON [s].[UserId] = [t].[UserId]
          LEFT JOIN (
              SELECT [o].[OrderId]
              FROM [Order] AS [o]
              WHERE [o].[IsDeleted] = CAST(0 AS bit)
          ) AS [t0] ON [s].[OrderId] = [t0].[OrderId]
          LEFT JOIN (
              SELECT [u0].[UserId]
              FROM [User] AS [u0]
              WHERE [u0].[IsDeleted] = CAST(0 AS bit)
          ) AS [t1] ON [s].[AssignedToUserId] = [t1].[UserId]
          WHERE [s].[TicketId] = @__ticketId_0
      ) AS [t2]
      INNER JOIN (
          SELECT [s0].[ReplyId], [s0].[AttachmentBucket], [s0].[AttachmentPath], [s0].[AttachmentUrl], [s0].[CreatedAt], [s0].[IsAdminReply], [s0].[Message], [s0].[TicketId], [s0].[UserId], [t4].[UserId] AS [UserId0], [t4].[CreatedAt] AS [CreatedAt0], [t4].[DefaultAddressId], [t4].[Email], [t4].[FailedLoginAttempts], [t4].[FirstName], [t4].[IsActive], [t4].[IsDeleted], [t4].[IsWalkIn], [t4].[LastLoginAt], [t4].[LastName], [t4].[LockoutUntil], [t4].[PasswordHash], [t4].[PhoneNumber]
          FROM [SupportTicketReply] AS [s0]
          INNER JOIN (
              SELECT [u1].[UserId], [u1].[CreatedAt], [u1].[DefaultAddressId], [u1].[Email], [u1].[FailedLoginAttempts], [u1].[FirstName], [u1].[IsActive], [u1].[IsDeleted], [u1].[IsWalkIn], [u1].[LastLoginAt], [u1].[LastName], [u1].[LockoutUntil], [u1].[PasswordHash], [u1].[PhoneNumber]
              FROM [User] AS [u1]
              WHERE [u1].[IsDeleted] = CAST(0 AS bit)
          ) AS [t4] ON [s0].[UserId] = [t4].[UserId]
      ) AS [t3] ON [t2].[TicketId] = [t3].[TicketId]
      ORDER BY [t2].[TicketId], [t2].[UserId], [t2].[OrderId], [t2].[UserId0], [t3].[CreatedAt]
fail: Microsoft.EntityFrameworkCore.Query[10100]
      An exception occurred while iterating over the results of a query for context type 'WebApplication.DataAccess.Context.AppDbContext'.
      System.InvalidCastException: Unable to cast object of type 'System.Int64' to type 'System.Int32'.
         at Microsoft.Data.SqlClient.SqlBuffer.get_Int32()
         at Microsoft.EntityFrameworkCore.Query.Internal.BufferedDataReader.BufferedDataRecord.ReadInt(DbDataReader reader, Int32 ordinal, ReaderColumn column)
         at Microsoft.EntityFrameworkCore.Query.Internal.BufferedDataReader.BufferedDataRecord.ReadRow()
         at Microsoft.EntityFrameworkCore.Query.Internal.BufferedDataReader.BufferedDataRecord.InitializeAsync(DbDataReader reader, IReadOnlyList`1 columns, CancellationToken cancellationToken)
         at Microsoft.EntityFrameworkCore.Query.Internal.BufferedDataReader.InitializeAsync(IReadOnlyList`1 columns, CancellationToken cancellationToken)
         at Microsoft.EntityFrameworkCore.Query.Internal.BufferedDataReader.InitializeAsync(IReadOnlyList`1 columns, CancellationToken cancellationToken)
         at Microsoft.EntityFrameworkCore.Storage.RelationalCommand.ExecuteReaderAsync(RelationalCommandParameterObject parameterObject, CancellationToken cancellationToken)
         at Microsoft.EntityFrameworkCore.Storage.RelationalCommand.ExecuteReaderAsync(RelationalCommandParameterObject parameterObject, CancellationToken cancellationToken)
         at Microsoft.EntityFrameworkCore.Query.RelationalShapedQueryCompilingExpressionVisitor.ShaperProcessingExpressionVisitor.<PopulateSplitIncludeCollectionAsync>g__InitializeReaderAsync|26_0[TIncludingEntity,TIncludedEntity](RelationalQueryContext queryContext, RelationalCommandCache relationalCommandCache, IReadOnlyList`1 readerColumns, Boolean detailedErrorsEnabled, CancellationToken cancellationToken)
         at Microsoft.EntityFrameworkCore.Storage.ExecutionStrategy.<>c__DisplayClass30_0`2.<<ExecuteAsync>b__0>d.MoveNext()
      --- End of stack trace from previous location ---
         at Microsoft.EntityFrameworkCore.Storage.ExecutionStrategy.ExecuteImplementationAsync[TState,TResult](Func`4 operation, Func`4 verifySucceeded, TState state, CancellationToken cancellationToken)
         at Microsoft.EntityFrameworkCore.Storage.ExecutionStrategy.ExecuteImplementationAsync[TState,TResult](Func`4 operation, Func`4 verifySucceeded, TState state, CancellationToken cancellationToken)
         at Microsoft.EntityFrameworkCore.Storage.ExecutionStrategy.ExecuteAsync[TState,TResult](TState state, Func`4 operation, Func`4 verifySucceeded, CancellationToken cancellationToken)
         at Microsoft.EntityFrameworkCore.Query.RelationalShapedQueryCompilingExpressionVisitor.ShaperProcessingExpressionVisitor.PopulateSplitIncludeCollectionAsync[TIncludingEntity,TIncludedEntity](Int32 collectionId, RelationalQueryContext queryContext, IExecutionStrategy executionStrategy, RelationalCommandCache relationalCommandCache, IReadOnlyList`1 readerColumns, Boolean detailedErrorsEnabled, SplitQueryResultCoordinator resultCoordinator, Func`3 childIdentifier, IReadOnlyList`1 identifierValueComparers, Func`5 innerShaper, Func`4 relatedDataLoaders, INavigationBase inverseNavigation, Action`2 fixup, Boolean trackingQuery)
         at Microsoft.EntityFrameworkCore.Query.RelationalShapedQueryCompilingExpressionVisitor.ShaperProcessingExpressionVisitor.TaskAwaiter(Func`1[] taskFactories)
         at Microsoft.EntityFrameworkCore.Query.Internal.SplitQueryingEnumerable`1.AsyncEnumerator.MoveNextAsync()
      System.InvalidCastException: Unable to cast object of type 'System.Int64' to type 'System.Int32'.
         at Microsoft.Data.SqlClient.SqlBuffer.get_Int32()
         at Microsoft.EntityFrameworkCore.Query.Internal.BufferedDataReader.BufferedDataRecord.ReadInt(DbDataReader reader, Int32 ordinal, ReaderColumn column)
         at Microsoft.EntityFrameworkCore.Query.Internal.BufferedDataReader.BufferedDataRecord.ReadRow()
         at Microsoft.EntityFrameworkCore.Query.Internal.BufferedDataReader.BufferedDataRecord.InitializeAsync(DbDataReader reader, IReadOnlyList`1 columns, CancellationToken cancellationToken)
         at Microsoft.EntityFrameworkCore.Query.Internal.BufferedDataReader.InitializeAsync(IReadOnlyList`1 columns, CancellationToken cancellationToken)
         at Microsoft.EntityFrameworkCore.Query.Internal.BufferedDataReader.InitializeAsync(IReadOnlyList`1 columns, CancellationToken cancellationToken)
         at Microsoft.EntityFrameworkCore.Storage.RelationalCommand.ExecuteReaderAsync(RelationalCommandParameterObject parameterObject, CancellationToken cancellationToken)
         at Microsoft.EntityFrameworkCore.Storage.RelationalCommand.ExecuteReaderAsync(RelationalCommandParameterObject parameterObject, CancellationToken cancellationToken)
         at Microsoft.EntityFrameworkCore.Query.RelationalShapedQueryCompilingExpressionVisitor.ShaperProcessingExpressionVisitor.<PopulateSplitIncludeCollectionAsync>g__InitializeReaderAsync|26_0[TIncludingEntity,TIncludedEntity](RelationalQueryContext queryContext, RelationalCommandCache relationalCommandCache, IReadOnlyList`1 readerColumns, Boolean detailedErrorsEnabled, CancellationToken cancellationToken)
         at Microsoft.EntityFrameworkCore.Storage.ExecutionStrategy.<>c__DisplayClass30_0`2.<<ExecuteAsync>b__0>d.MoveNext()
      --- End of stack trace from previous location ---
         at Microsoft.EntityFrameworkCore.Storage.ExecutionStrategy.ExecuteImplementationAsync[TState,TResult](Func`4 operation, Func`4 verifySucceeded, TState state, CancellationToken cancellationToken)
         at Microsoft.EntityFrameworkCore.Storage.ExecutionStrategy.ExecuteImplementationAsync[TState,TResult](Func`4 operation, Func`4 verifySucceeded, TState state, CancellationToken cancellationToken)
         at Microsoft.EntityFrameworkCore.Storage.ExecutionStrategy.ExecuteAsync[TState,TResult](TState state, Func`4 operation, Func`4 verifySucceeded, CancellationToken cancellationToken)
         at Microsoft.EntityFrameworkCore.Query.RelationalShapedQueryCompilingExpressionVisitor.ShaperProcessingExpressionVisitor.PopulateSplitIncludeCollectionAsync[TIncludingEntity,TIncludedEntity](Int32 collectionId, RelationalQueryContext queryContext, IExecutionStrategy executionStrategy, RelationalCommandCache relationalCommandCache, IReadOnlyList`1 readerColumns, Boolean detailedErrorsEnabled, SplitQueryResultCoordinator resultCoordinator, Func`3 childIdentifier, IReadOnlyList`1 identifierValueComparers, Func`5 innerShaper, Func`4 relatedDataLoaders, INavigationBase inverseNavigation, Action`2 fixup, Boolean trackingQuery)
         at Microsoft.EntityFrameworkCore.Query.RelationalShapedQueryCompilingExpressionVisitor.ShaperProcessingExpressionVisitor.TaskAwaiter(Func`1[] taskFactories)
         at Microsoft.EntityFrameworkCore.Query.Internal.SplitQueryingEnumerable`1.AsyncEnumerator.MoveNextAsync()
fail: WebApplication.Controllers.SupportController[0]
      Error loading ticket 1.
      System.InvalidCastException: Unable to cast object of type 'System.Int64' to type 'System.Int32'.
         at Microsoft.Data.SqlClient.SqlBuffer.get_Int32()
         at Microsoft.EntityFrameworkCore.Query.Internal.BufferedDataReader.BufferedDataRecord.ReadInt(DbDataReader reader, Int32 ordinal, ReaderColumn column)
         at Microsoft.EntityFrameworkCore.Query.Internal.BufferedDataReader.BufferedDataRecord.ReadRow()
         at Microsoft.EntityFrameworkCore.Query.Internal.BufferedDataReader.BufferedDataRecord.InitializeAsync(DbDataReader reader, IReadOnlyList`1 columns, CancellationToken cancellationToken)
         at Microsoft.EntityFrameworkCore.Query.Internal.BufferedDataReader.InitializeAsync(IReadOnlyList`1 columns, CancellationToken cancellationToken)
         at Microsoft.EntityFrameworkCore.Query.Internal.BufferedDataReader.InitializeAsync(IReadOnlyList`1 columns, CancellationToken cancellationToken)
         at Microsoft.EntityFrameworkCore.Storage.RelationalCommand.ExecuteReaderAsync(RelationalCommandParameterObject parameterObject, CancellationToken cancellationToken)
         at Microsoft.EntityFrameworkCore.Storage.RelationalCommand.ExecuteReaderAsync(RelationalCommandParameterObject parameterObject, CancellationToken cancellationToken)
         at Microsoft.EntityFrameworkCore.Query.RelationalShapedQueryCompilingExpressionVisitor.ShaperProcessingExpressionVisitor.<PopulateSplitIncludeCollectionAsync>g__InitializeReaderAsync|26_0[TIncludingEntity,TIncludedEntity](RelationalQueryContext queryContext, RelationalCommandCache relationalCommandCache, IReadOnlyList`1 readerColumns, Boolean detailedErrorsEnabled, CancellationToken cancellationToken)
         at Microsoft.EntityFrameworkCore.Storage.ExecutionStrategy.<>c__DisplayClass30_0`2.<<ExecuteAsync>b__0>d.MoveNext()
      --- End of stack trace from previous location ---
         at Microsoft.EntityFrameworkCore.Storage.ExecutionStrategy.ExecuteImplementationAsync[TState,TResult](Func`4 operation, Func`4 verifySucceeded, TState state, CancellationToken cancellationToken)
         at Microsoft.EntityFrameworkCore.Storage.ExecutionStrategy.ExecuteImplementationAsync[TState,TResult](Func`4 operation, Func`4 verifySucceeded, TState state, CancellationToken cancellationToken)
         at Microsoft.EntityFrameworkCore.Storage.ExecutionStrategy.ExecuteAsync[TState,TResult](TState state, Func`4 operation, Func`4 verifySucceeded, CancellationToken cancellationToken)
         at Microsoft.EntityFrameworkCore.Query.RelationalShapedQueryCompilingExpressionVisitor.ShaperProcessingExpressionVisitor.PopulateSplitIncludeCollectionAsync[TIncludingEntity,TIncludedEntity](Int32 collectionId, RelationalQueryContext queryContext, IExecutionStrategy executionStrategy, RelationalCommandCache relationalCommandCache, IReadOnlyList`1 readerColumns, Boolean detailedErrorsEnabled, SplitQueryResultCoordinator resultCoordinator, Func`3 childIdentifier, IReadOnlyList`1 identifierValueComparers, Func`5 innerShaper, Func`4 relatedDataLoaders, INavigationBase inverseNavigation, Action`2 fixup, Boolean trackingQuery)
         at Microsoft.EntityFrameworkCore.Query.RelationalShapedQueryCompilingExpressionVisitor.ShaperProcessingExpressionVisitor.TaskAwaiter(Func`1[] taskFactories)
         at Microsoft.EntityFrameworkCore.Query.Internal.SplitQueryingEnumerable`1.AsyncEnumerator.MoveNextAsync()
         at Microsoft.EntityFrameworkCore.Query.ShapedQueryCompilingExpressionVisitor.SingleOrDefaultAsync[TSource](IAsyncEnumerable`1 asyncEnumerable, CancellationToken cancellationToken)
         at Microsoft.EntityFrameworkCore.Query.ShapedQueryCompilingExpressionVisitor.SingleOrDefaultAsync[TSource](IAsyncEnumerable`1 asyncEnumerable, CancellationToken cancellationToken)
         at WebApplication.DataAccess.Repositories.SupportRepository.GetWithRepliesAsync(Int32 ticketId, CancellationToken cancellationToken) in C:\Users\user\Documents\ANDREI_FILES\PDM_FILES\2ND YEAR\2ND SEM\OOP\OOP-TaurusBikeShop\WebApplication\DataAccess\Repositories\SupportRepository.cs:line 54
         at WebApplication.BusinessLogic.Services.SupportService.GetDetailAsync(Int32 ticketId, Int32 userId, CancellationToken cancellationToken) in C:\Users\user\Documents\ANDREI_FILES\PDM_FILES\2ND YEAR\2ND SEM\OOP\OOP-TaurusBikeShop\WebApplication\BusinessLogic\Services\SupportService.cs:line 110
         at WebApplication.Controllers.SupportController.Detail(Int32 ticketId, CancellationToken cancellationToken) in C:\Users\user\Documents\ANDREI_FILES\PDM_FILES\2ND YEAR\2ND SEM\OOP\OOP-TaurusBikeShop\WebApplication\Controllers\SupportController.cs:line 135
info: Microsoft.AspNetCore.Mvc.RedirectToActionResult[1]
      Executing RedirectResult, redirecting to /Support.
info: Microsoft.AspNetCore.Mvc.Infrastructure.ControllerActionInvoker[105]
      Executed action WebApplication.Controllers.SupportController.Detail (WebApplication) in 197.7422ms
info: Microsoft.AspNetCore.Routing.EndpointMiddleware[1]
      Executed endpoint 'WebApplication.Controllers.SupportController.Detail (WebApplication)'
info: Microsoft.AspNetCore.Hosting.Diagnostics[2]
      Request finished HTTP/2 GET https://localhost:7177/Support/Detail?ticketId=1 - 302 - - 208.9739ms
info: Microsoft.AspNetCore.Hosting.Diagnostics[1]
      Request starting HTTP/2 GET https://localhost:7177/Support - - -
info: Microsoft.AspNetCore.Routing.EndpointMiddleware[0]
      Executing endpoint 'WebApplication.Controllers.SupportController.Index (WebApplication)'
info: Microsoft.AspNetCore.Mvc.Infrastructure.ControllerActionInvoker[102]
      Route matched with {action = "Index", controller = "Support"}. Executing controller action with signature System.Threading.Tasks.Task`1[Microsoft.AspNetCore.Mvc.IActionResult] Index(System.Threading.CancellationToken) on controller WebApplication.Controllers.SupportController (WebApplication).
info: Microsoft.EntityFrameworkCore.Database.Command[20101]
      Executed DbCommand (57ms) [Parameters=[@__userId_0='?' (DbType = Int32)], CommandType='Text', CommandTimeout='30']
      SELECT [s].[TicketId], [s].[AssignedToUserId], [s].[AttachmentBucket], [s].[AttachmentPath], [s].[AttachmentUrl], [s].[CreatedAt], [s].[Description], [s].[OrderId], [s].[ResolvedAt], [s].[Subject], [s].[TicketCategory], [s].[TicketSource], [s].[TicketStatus], [s].[UpdatedAt], [s].[UserId]
      FROM [SupportTicket] AS [s]
      WHERE [s].[UserId] = @__userId_0
      ORDER BY [s].[CreatedAt] DESC
info: Microsoft.AspNetCore.Mvc.ViewFeatures.ViewResultExecutor[1]
      Executing ViewResult, running view ~/Views/Customer/SupportList.cshtml.
info: Microsoft.AspNetCore.Mvc.ViewFeatures.ViewResultExecutor[4]
      Executed ViewResult - view ~/Views/Customer/SupportList.cshtml executed in 4.7727ms.
info: Microsoft.AspNetCore.Mvc.Infrastructure.ControllerActionInvoker[105]
      Executed action WebApplication.Controllers.SupportController.Index (WebApplication) in 66.189ms
info: Microsoft.AspNetCore.Routing.EndpointMiddleware[1]
      Executed endpoint 'WebApplication.Controllers.SupportController.Index (WebApplication)'
info: Microsoft.AspNetCore.Hosting.Diagnostics[2]
      Request finished HTTP/2 GET https://localhost:7177/Support - 200 - text/html;+charset=utf-8 78.1236ms
info: Microsoft.AspNetCore.Hosting.Diagnostics[1]
      Request starting HTTP/2 GET https://localhost:7177/_vs/browserLink - - -
info: Microsoft.AspNetCore.Hosting.Diagnostics[1]
      Request starting HTTP/2 GET https://localhost:7177/_framework/aspnetcore-browser-refresh.js - - -
info: Microsoft.AspNetCore.Hosting.Diagnostics[2]
      Request finished HTTP/2 GET https://localhost:7177/_framework/aspnetcore-browser-refresh.js - 200 14933 application/javascript;+charset=utf-8 5.3397ms
info: Microsoft.AspNetCore.Hosting.Diagnostics[2]
      Request finished HTTP/2 GET https://localhost:7177/_vs/browserLink - 200 - text/javascript;+charset=UTF-8 13.6404ms
info: Microsoft.AspNetCore.Hosting.Diagnostics[1]
      Request starting HTTP/2 GET https://localhost:7177/antiforgery/token - - -
info: Microsoft.AspNetCore.Hosting.Diagnostics[1]
      Request starting HTTP/2 GET https://localhost:7177/Customer/NotificationCount - - -
info: Microsoft.AspNetCore.Hosting.Diagnostics[1]
      Request starting HTTP/2 GET https://localhost:7177/Cart/GetCartCount - - -
info: Microsoft.AspNetCore.Routing.EndpointMiddleware[0]
      Executing endpoint 'HTTP: GET /antiforgery/token'
info: Microsoft.AspNetCore.Routing.EndpointMiddleware[0]
      Executing endpoint 'WebApplication.Controllers.CustomerController.NotificationCount (WebApplication)'
info: Microsoft.AspNetCore.Routing.EndpointMiddleware[0]
      Executing endpoint 'WebApplication.Controllers.CartController.GetCartCount (WebApplication)'
info: Microsoft.AspNetCore.Mvc.Infrastructure.ControllerActionInvoker[102]
      Route matched with {action = "NotificationCount", controller = "Customer"}. Executing controller action with signature System.Threading.Tasks.Task`1[Microsoft.AspNetCore.Mvc.IActionResult] NotificationCount(System.Threading.CancellationToken) on controller WebApplication.Controllers.CustomerController (WebApplication).
info: Microsoft.AspNetCore.Mvc.Infrastructure.ControllerActionInvoker[102]
      Route matched with {action = "GetCartCount", controller = "Cart"}. Executing controller action with signature System.Threading.Tasks.Task`1[Microsoft.AspNetCore.Mvc.IActionResult] GetCartCount(System.Threading.CancellationToken) on controller WebApplication.Controllers.CartController (WebApplication).
info: Microsoft.AspNetCore.Http.Result.OkObjectResult[1]
      Setting HTTP status code 200.
info: Microsoft.AspNetCore.Http.Result.OkObjectResult[3]
      Writing value of type '<>f__AnonymousType21`1' as Json.
info: Microsoft.AspNetCore.Routing.EndpointMiddleware[1]
      Executed endpoint 'HTTP: GET /antiforgery/token'
info: Microsoft.AspNetCore.Hosting.Diagnostics[2]
      Request finished HTTP/2 GET https://localhost:7177/antiforgery/token - 200 - application/json;+charset=utf-8 24.7320ms
info: Microsoft.EntityFrameworkCore.Database.Command[20101]
      Executed DbCommand (58ms) [Parameters=[@__userId_0='?' (DbType = Int32)], CommandType='Text', CommandTimeout='30']
      SELECT COUNT(*)
      FROM [Notification] AS [n]
      WHERE [n].[UserId] = @__userId_0 AND [n].[IsRead] = CAST(0 AS bit) AND [n].[Channel] = N'InApp'
info: Microsoft.EntityFrameworkCore.Database.Command[20101]
      Executed DbCommand (59ms) [Parameters=[@__userId_Value_0='?' (DbType = Int32)], CommandType='Text', CommandTimeout='30']
      SELECT TOP(1) [c].[CartId], [c].[CreatedAt], [c].[GuestSessionId], [c].[IsCheckedOut], [c].[LastUpdatedAt], [c].[UserId]
      FROM [Cart] AS [c]
      WHERE [c].[IsCheckedOut] = CAST(0 AS bit) AND [c].[UserId] = @__userId_Value_0
      ORDER BY [c].[CartId]
info: Microsoft.AspNetCore.Mvc.Infrastructure.SystemTextJsonResultExecutor[1]
      Executing JsonResult, writing value of type '<>f__AnonymousType6`1[[System.Int32, System.Private.CoreLib, Version=8.0.0.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e]]'.
info: Microsoft.AspNetCore.Mvc.Infrastructure.SystemTextJsonResultExecutor[1]
      Executing JsonResult, writing value of type 'WebApplication.Models.ApiResponse'.
info: Microsoft.AspNetCore.Mvc.Infrastructure.ControllerActionInvoker[105]
      Executed action WebApplication.Controllers.CustomerController.NotificationCount (WebApplication) in 68.326ms
info: Microsoft.AspNetCore.Mvc.Infrastructure.ControllerActionInvoker[105]
      Executed action WebApplication.Controllers.CartController.GetCartCount (WebApplication) in 68.3631ms
info: Microsoft.AspNetCore.Routing.EndpointMiddleware[1]
      Executed endpoint 'WebApplication.Controllers.CustomerController.NotificationCount (WebApplication)'
info: Microsoft.AspNetCore.Routing.EndpointMiddleware[1]
      Executed endpoint 'WebApplication.Controllers.CartController.GetCartCount (WebApplication)'
info: Microsoft.AspNetCore.Hosting.Diagnostics[2]
      Request finished HTTP/2 GET https://localhost:7177/Customer/NotificationCount - 200 - application/json;+charset=utf-8 87.9983ms
info: Microsoft.AspNetCore.Hosting.Diagnostics[2]
      Request finished HTTP/2 GET https://localhost:7177/Cart/GetCartCount - 200 - application/json;+charset=utf-8 90.3324ms
info: Microsoft.EntityFrameworkCore.Database.Command[20101]
      Executed DbCommand (59ms) [Parameters=[@__p_0='?' (DbType = Int32)], CommandType='Text', CommandTimeout='30']
      SELECT TOP(@__p_0) [n].[NotificationId], [n].[Body], [n].[Channel], [n].[CreatedAt], [n].[FailureReason], [n].[IsRead], [n].[NotifType], [n].[OrderId], [n].[ReadAt], [n].[Recipient], [n].[RetryCount], [n].[SentAt], [n].[Status], [n].[Subject], [n].[TicketId], [n].[UserId]
      FROM [Notification] AS [n]
      WHERE [n].[Status] = N'Pending' AND [n].[RetryCount] < 3 AND [n].[Channel] = N'Email'
      ORDER BY [n].[CreatedAt]
info: WebApplication.BusinessLogic.Services.GmailEmailSender[0]
      Email sent to celonbrianhoward.pdm@gmail.com - Subject: Support ticket created - #1: Testing lang po.
info: WebApplication.BackgroundJobs.NotificationDispatchJob[0]
      Notification 70 sent to celonbrianhoward.pdm@gmail.com (SupportTicketCreated).
info: Microsoft.EntityFrameworkCore.Database.Command[20101]
      Executed DbCommand (59ms) [Parameters=[@p2='?' (DbType = Int32), @p0='?' (DbType = DateTime2), @p1='?' (Size = 20)], CommandType='Text', CommandTimeout='30']
      SET IMPLICIT_TRANSACTIONS OFF;
      SET NOCOUNT ON;
      UPDATE [Notification] SET [SentAt] = @p0, [Status] = @p1
      OUTPUT 1
      WHERE [NotificationId] = @p2;
