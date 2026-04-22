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
      Executed DbCommand (174ms) [Parameters=[], CommandType='Text', CommandTimeout='30']
      SELECT COUNT(*)
      FROM [ProductVariant] AS [p]
info: Microsoft.EntityFrameworkCore.Database.Command[20101]
      Executed DbCommand (157ms) [Parameters=[@__p_0='?' (DbType = Int32)], CommandType='Text', CommandTimeout='30']
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
      Executed DbCommand (143ms) [Parameters=[@p0='?' (DbType = DateTime2), @p1='?' (Size = 1000), @p2='?' (Size = 100), @p3='?' (DbType = Int32), @p4='?' (DbType = DateTime2), @p5='?' (Size = 1000), @p6='?' (Size = 100), @p7='?' (DbType = Int32)], CommandType='Text', CommandTimeout='30']
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
      Executed DbCommand (77ms) [Parameters=[@__p_0='?' (DbType = Int32)], CommandType='Text', CommandTimeout='30']
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
      Executed DbCommand (74ms) [Parameters=[@__p_0='?' (DbType = Int32)], CommandType='Text', CommandTimeout='30']
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
info: Microsoft.AspNetCore.Hosting.Diagnostics[1]
      Request starting HTTP/2 GET https://localhost:7177/css/output.css?v=CXRbrZ1s69tJ8KRR4cAcYi6LIRaK_lqzLjiWJmykSb8 - - -
info: Microsoft.AspNetCore.Hosting.Diagnostics[1]
      Request starting HTTP/2 GET https://localhost:7177/FULL-COLOR.png - - -
info: Microsoft.AspNetCore.Hosting.Diagnostics[1]
      Request starting HTTP/2 GET https://localhost:7177/lib/jquery/dist/jquery.min.js - - -
info: Microsoft.AspNetCore.StaticFiles.StaticFileMiddleware[6]
      The file /FULL-COLOR.png was not modified
info: Microsoft.AspNetCore.StaticFiles.StaticFileMiddleware[6]
      The file /lib/jquery/dist/jquery.min.js was not modified
info: Microsoft.AspNetCore.StaticFiles.StaticFileMiddleware[6]
      The file /css/output.css was not modified
info: Microsoft.AspNetCore.Mvc.ViewFeatures.ViewResultExecutor[4]
      Executed ViewResult - view Index executed in 150.2153ms.
info: Microsoft.AspNetCore.Hosting.Diagnostics[2]
      Request finished HTTP/2 GET https://localhost:7177/css/output.css?v=CXRbrZ1s69tJ8KRR4cAcYi6LIRaK_lqzLjiWJmykSb8 - 304 - text/css 16.6419ms
info: Microsoft.AspNetCore.Hosting.Diagnostics[2]
      Request finished HTTP/2 GET https://localhost:7177/FULL-COLOR.png - 304 - image/png 16.5716ms
info: Microsoft.AspNetCore.Hosting.Diagnostics[2]
      Request finished HTTP/2 GET https://localhost:7177/lib/jquery/dist/jquery.min.js - 304 - text/javascript 12.6168ms
info: Microsoft.AspNetCore.Hosting.Diagnostics[1]
      Request starting HTTP/2 GET https://localhost:7177/js/utils.js?v=FmKd-Gz4ukVHy90T9zxoGNgcaXIdIxgYnAmXep_Oz9M - - -
info: Microsoft.AspNetCore.Hosting.Diagnostics[1]
      Request starting HTTP/2 GET https://localhost:7177/js/app.js?v=1bdQNEGxNUuq-HEnummF5Nt5kesG1De1sKfR4TzW1ek - - -
info: Microsoft.AspNetCore.StaticFiles.StaticFileMiddleware[6]
      The file /js/utils.js was not modified
info: Microsoft.AspNetCore.Mvc.Infrastructure.ControllerActionInvoker[105]
      Executed action WebApplication.Controllers.HomeController.Index (WebApplication) in 2811.2477ms
info: Microsoft.AspNetCore.StaticFiles.StaticFileMiddleware[6]
      The file /js/app.js was not modified
info: Microsoft.AspNetCore.Hosting.Diagnostics[1]
      Request starting HTTP/2 GET https://localhost:7177/_framework/aspnetcore-browser-refresh.js - - -
info: Microsoft.AspNetCore.Hosting.Diagnostics[2]
      Request finished HTTP/2 GET https://localhost:7177/js/utils.js?v=FmKd-Gz4ukVHy90T9zxoGNgcaXIdIxgYnAmXep_Oz9M - 304 - text/javascript 34.6558ms
info: Microsoft.AspNetCore.Hosting.Diagnostics[1]
      Request starting HTTP/2 GET https://localhost:7177/_vs/browserLink - - -
info: Microsoft.AspNetCore.Hosting.Diagnostics[1]
      Request starting HTTP/2 GET https://localhost:7177/js/site.js?v=h3a0Utrh7E1bD8mlNvz6owDBe9CR7mm4koHNGtwJHd8 - - -
info: Microsoft.AspNetCore.Hosting.Diagnostics[2]
      Request finished HTTP/2 GET https://localhost:7177/js/app.js?v=1bdQNEGxNUuq-HEnummF5Nt5kesG1De1sKfR4TzW1ek - 304 - text/javascript 34.0032ms
info: Microsoft.AspNetCore.StaticFiles.StaticFileMiddleware[6]
      The file /js/site.js was not modified
info: Microsoft.AspNetCore.Hosting.Diagnostics[2]
      Request finished HTTP/2 GET https://localhost:7177/js/site.js?v=h3a0Utrh7E1bD8mlNvz6owDBe9CR7mm4koHNGtwJHd8 - 304 - text/javascript 10.8824ms
info: Microsoft.AspNetCore.Routing.EndpointMiddleware[1]
      Executed endpoint 'WebApplication.Controllers.HomeController.Index (WebApplication)'
info: Microsoft.AspNetCore.Hosting.Diagnostics[2]
      Request finished HTTP/2 GET https://localhost:7177/_framework/aspnetcore-browser-refresh.js - 200 14933 application/javascript;+charset=utf-8 19.6857ms
info: Microsoft.AspNetCore.Hosting.Diagnostics[2]
      Request finished HTTP/2 GET https://localhost:7177/ - 200 - text/html;+charset=utf-8 3036.4865ms
info: Microsoft.AspNetCore.Hosting.Diagnostics[1]
      Request starting HTTP/2 GET https://localhost:7177/antiforgery/token - - -
info: Microsoft.AspNetCore.Hosting.Diagnostics[1]
      Request starting HTTP/2 GET https://localhost:7177/Cart/GetCartCount - - -
info: Microsoft.AspNetCore.Routing.EndpointMiddleware[0]
      Executing endpoint 'HTTP: GET /antiforgery/token'
info: Microsoft.AspNetCore.Routing.EndpointMiddleware[0]
      Executing endpoint 'WebApplication.Controllers.CartController.GetCartCount (WebApplication)'
info: Microsoft.AspNetCore.Mvc.Infrastructure.ControllerActionInvoker[102]
      Route matched with {action = "GetCartCount", controller = "Cart"}. Executing controller action with signature System.Threading.Tasks.Task`1[Microsoft.AspNetCore.Mvc.IActionResult] GetCartCount(System.Threading.CancellationToken) on controller WebApplication.Controllers.CartController (WebApplication).
info: Microsoft.AspNetCore.Hosting.Diagnostics[2]
      Request finished HTTP/2 GET https://localhost:7177/_vs/browserLink - 200 - text/javascript;+charset=UTF-8 112.6863ms
info: Microsoft.AspNetCore.Http.Result.OkObjectResult[1]
      Setting HTTP status code 200.
info: Microsoft.AspNetCore.Mvc.Infrastructure.SystemTextJsonResultExecutor[1]
      Executing JsonResult, writing value of type 'WebApplication.Models.ApiResponse'.
info: Microsoft.AspNetCore.Http.Result.OkObjectResult[3]
      Writing value of type '<>f__AnonymousType19`1' as Json.
info: Microsoft.AspNetCore.Routing.EndpointMiddleware[1]
      Executed endpoint 'HTTP: GET /antiforgery/token'
info: Microsoft.AspNetCore.Hosting.Diagnostics[2]
      Request finished HTTP/2 GET https://localhost:7177/antiforgery/token - 200 - application/json;+charset=utf-8 159.3329ms
info: Microsoft.AspNetCore.Mvc.Infrastructure.ControllerActionInvoker[105]
      Executed action WebApplication.Controllers.CartController.GetCartCount (WebApplication) in 115.9214ms
info: Microsoft.AspNetCore.Routing.EndpointMiddleware[1]
      Executed endpoint 'WebApplication.Controllers.CartController.GetCartCount (WebApplication)'
info: Microsoft.AspNetCore.Hosting.Diagnostics[2]
      Request finished HTTP/2 GET https://localhost:7177/Cart/GetCartCount - 200 - application/json;+charset=utf-8 162.7893ms
info: Microsoft.EntityFrameworkCore.Database.Command[20101]
      Executed DbCommand (66ms) [Parameters=[@p0='?' (DbType = DateTime2), @p1='?' (Size = 1000), @p2='?' (Size = 100), @p3='?' (DbType = Int32)], CommandType='Text', CommandTimeout='30']
      SET IMPLICIT_TRANSACTIONS OFF;
      SET NOCOUNT ON;
      INSERT INTO [SystemLog] ([CreatedAt], [EventDescription], [EventType], [UserId])
      OUTPUT INSERTED.[SystemLogId]
      VALUES (@p0, @p1, @p2, @p3);
info: Microsoft.EntityFrameworkCore.Database.Command[20101]
      Executed DbCommand (65ms) [Parameters=[], CommandType='Text', CommandTimeout='30']
      SELECT [p].[ProductVariantId], [p].[VariantName], [p].[StockQuantity], [p].[ReorderThreshold], [p].[ProductId], [p0].[Name] AS [ProductName]
      FROM [ProductVariant] AS [p]
      INNER JOIN [Product] AS [p0] ON [p].[ProductId] = [p0].[ProductId]
      WHERE [p].[IsActive] = CAST(1 AS bit) AND [p0].[IsActive] = CAST(1 AS bit) AND [p].[StockQuantity] < [p].[ReorderThreshold]
info: Microsoft.EntityFrameworkCore.Database.Command[20101]
      Executed DbCommand (96ms) [Parameters=[], CommandType='Text', CommandTimeout='30']
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
      Executed DbCommand (74ms) [Parameters=[@__8__locals1_cooldownStart_0='?' (DbType = DateTime2), @__Format_2='?' (Size = 1000)], CommandType='Text', CommandTimeout='30']
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
      Executed DbCommand (99ms) [Parameters=[@p0='?' (Size = 4000), @p1='?' (Size = 20), @p2='?' (DbType = DateTime2), @p3='?' (Size = 500), @p4='?' (DbType = Boolean), @p5='?' (Size = 100), @p6='?' (DbType = Int32), @p7='?' (DbType = DateTime2), @p8='?' (Size = 255), @p9='?' (DbType = Int32), @p10='?' (DbType = DateTime2), @p11='?' (Size = 20), @p12='?' (Size = 255), @p13='?' (DbType = Int32), @p14='?' (DbType = Int32)], CommandType='Text', CommandTimeout='30']
      SET IMPLICIT_TRANSACTIONS OFF;
      SET NOCOUNT ON;
      INSERT INTO [Notification] ([Body], [Channel], [CreatedAt], [FailureReason], [IsRead], [NotifType], [OrderId], [ReadAt], [Recipient], [RetryCount], [SentAt], [Status], [Subject], [TicketId], [UserId])
      OUTPUT INSERTED.[NotificationId]
      VALUES (@p0, @p1, @p2, @p3, @p4, @p5, @p6, @p7, @p8, @p9, @p10, @p11, @p12, @p13, @p14);
warn: WebApplication.BackgroundJobs.StockMonitorJob[0]
      StockMonitorJob: low stock - variant 2 (Cult Odyssey Hydro Brakes 27.5 / Default): 0 remaining (threshold 5).
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
      Executed DbCommand (60ms) [Parameters=[@p0='?' (Size = 4000), @p1='?' (Size = 20), @p2='?' (DbType = DateTime2), @p3='?' (Size = 500), @p4='?' (DbType = Boolean), @p5='?' (Size = 100), @p6='?' (DbType = Int32), @p7='?' (DbType = DateTime2), @p8='?' (Size = 255), @p9='?' (DbType = Int32), @p10='?' (DbType = DateTime2), @p11='?' (Size = 20), @p12='?' (Size = 255), @p13='?' (DbType = Int32), @p14='?' (DbType = Int32)], CommandType='Text', CommandTimeout='30']
      SET IMPLICIT_TRANSACTIONS OFF;
      SET NOCOUNT ON;
      INSERT INTO [Notification] ([Body], [Channel], [CreatedAt], [FailureReason], [IsRead], [NotifType], [OrderId], [ReadAt], [Recipient], [RetryCount], [SentAt], [Status], [Subject], [TicketId], [UserId])
      OUTPUT INSERTED.[NotificationId]
      VALUES (@p0, @p1, @p2, @p3, @p4, @p5, @p6, @p7, @p8, @p9, @p10, @p11, @p12, @p13, @p14);
warn: WebApplication.BackgroundJobs.StockMonitorJob[0]
      StockMonitorJob: low stock - variant 3 (Toseek Chester 700c Disc Brake ALLOY (2x9) / Default): 3 remaining (threshold 5).
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
info: Microsoft.AspNetCore.Hosting.Diagnostics[1]
      Request starting HTTP/2 GET https://localhost:7177/Customer/Login - - -
info: Microsoft.EntityFrameworkCore.Database.Command[20101]
      Executed DbCommand (62ms) [Parameters=[@p0='?' (Size = 4000), @p1='?' (Size = 20), @p2='?' (DbType = DateTime2), @p3='?' (Size = 500), @p4='?' (DbType = Boolean), @p5='?' (Size = 100), @p6='?' (DbType = Int32), @p7='?' (DbType = DateTime2), @p8='?' (Size = 255), @p9='?' (DbType = Int32), @p10='?' (DbType = DateTime2), @p11='?' (Size = 20), @p12='?' (Size = 255), @p13='?' (DbType = Int32), @p14='?' (DbType = Int32)], CommandType='Text', CommandTimeout='30']
      SET IMPLICIT_TRANSACTIONS OFF;
      SET NOCOUNT ON;
      INSERT INTO [Notification] ([Body], [Channel], [CreatedAt], [FailureReason], [IsRead], [NotifType], [OrderId], [ReadAt], [Recipient], [RetryCount], [SentAt], [Status], [Subject], [TicketId], [UserId])
      OUTPUT INSERTED.[NotificationId]
      VALUES (@p0, @p1, @p2, @p3, @p4, @p5, @p6, @p7, @p8, @p9, @p10, @p11, @p12, @p13, @p14);
info: Microsoft.AspNetCore.Routing.EndpointMiddleware[0]
      Executing endpoint 'WebApplication.Controllers.CustomerController.Login (WebApplication)'
info: Microsoft.AspNetCore.Mvc.Infrastructure.ControllerActionInvoker[102]
      Route matched with {action = "Login", controller = "Customer"}. Executing controller action with signature Microsoft.AspNetCore.Mvc.IActionResult Login(System.String) on controller WebApplication.Controllers.CustomerController (WebApplication).
info: Microsoft.AspNetCore.Mvc.ViewFeatures.ViewResultExecutor[1]
      Executing ViewResult, running view Login.
info: Microsoft.AspNetCore.Mvc.ViewFeatures.ViewResultExecutor[4]
      Executed ViewResult - view Login executed in 28.2334ms.
info: Microsoft.AspNetCore.Mvc.Infrastructure.ControllerActionInvoker[105]
      Executed action WebApplication.Controllers.CustomerController.Login (WebApplication) in 35.681ms
info: Microsoft.AspNetCore.Routing.EndpointMiddleware[1]
      Executed endpoint 'WebApplication.Controllers.CustomerController.Login (WebApplication)'
info: Microsoft.AspNetCore.Hosting.Diagnostics[2]
      Request finished HTTP/2 GET https://localhost:7177/Customer/Login - 200 - text/html;+charset=utf-8 54.6755ms
info: Microsoft.AspNetCore.Hosting.Diagnostics[1]
      Request starting HTTP/2 GET https://localhost:7177/lib/jquery-validation-unobtrusive/dist/jquery.validate.unobtrusive.min.js - - -
info: Microsoft.AspNetCore.Hosting.Diagnostics[1]
      Request starting HTTP/2 GET https://localhost:7177/lib/jquery-validation/dist/jquery.validate.min.js - - -
info: Microsoft.AspNetCore.Hosting.Diagnostics[1]
      Request starting HTTP/2 GET https://localhost:7177/_vs/browserLink - - -
info: Microsoft.AspNetCore.Hosting.Diagnostics[1]
      Request starting HTTP/2 GET https://localhost:7177/_framework/aspnetcore-browser-refresh.js - - -
info: Microsoft.AspNetCore.StaticFiles.StaticFileMiddleware[6]
      The file /lib/jquery-validation-unobtrusive/dist/jquery.validate.unobtrusive.min.js was not modified
info: Microsoft.AspNetCore.StaticFiles.StaticFileMiddleware[6]
      The file /lib/jquery-validation/dist/jquery.validate.min.js was not modified
info: Microsoft.AspNetCore.Hosting.Diagnostics[2]
      Request finished HTTP/2 GET https://localhost:7177/lib/jquery-validation-unobtrusive/dist/jquery.validate.unobtrusive.min.js - 304 - text/javascript 8.0749ms
info: Microsoft.AspNetCore.Hosting.Diagnostics[2]
      Request finished HTTP/2 GET https://localhost:7177/lib/jquery-validation/dist/jquery.validate.min.js - 304 - text/javascript 9.5770ms
info: Microsoft.AspNetCore.Hosting.Diagnostics[2]
      Request finished HTTP/2 GET https://localhost:7177/_framework/aspnetcore-browser-refresh.js - 200 14933 application/javascript;+charset=utf-8 11.7891ms
info: Microsoft.EntityFrameworkCore.Database.Command[20101]
      Executed DbCommand (70ms) [Parameters=[@__8__locals2_variant_ProductId_0='?' (DbType = Int32)], CommandType='Text', CommandTimeout='30']
      SELECT [u].[UserId], [u].[Email], [u].[FirstName]
      FROM [User] AS [u]
      WHERE [u].[IsDeleted] = CAST(0 AS bit) AND [u].[IsActive] = CAST(1 AS bit) AND [u].[IsWalkIn] = CAST(0 AS bit) AND [u].[Email] IS NOT NULL AND EXISTS (
          SELECT 1
          FROM [Wishlist] AS [w]
          WHERE [u].[UserId] = [w].[UserId] AND [w].[ProductId] = @__8__locals2_variant_ProductId_0)
warn: WebApplication.BackgroundJobs.StockMonitorJob[0]
      StockMonitorJob: low stock - variant 4 (Ryder X3 27.5 2021 MT200 Hydro Brakes (3x8) / Default): 2 remaining (threshold 5).
info: Microsoft.AspNetCore.Hosting.Diagnostics[2]
      Request finished HTTP/2 GET https://localhost:7177/_vs/browserLink - 200 - text/javascript;+charset=UTF-8 24.5398ms
info: Microsoft.AspNetCore.Hosting.Diagnostics[1]
      Request starting HTTP/2 GET https://localhost:7177/Cart/GetCartCount - - -
info: Microsoft.AspNetCore.Routing.EndpointMiddleware[0]
      Executing endpoint 'WebApplication.Controllers.CartController.GetCartCount (WebApplication)'
info: Microsoft.AspNetCore.Mvc.Infrastructure.ControllerActionInvoker[102]
      Route matched with {action = "GetCartCount", controller = "Cart"}. Executing controller action with signature System.Threading.Tasks.Task`1[Microsoft.AspNetCore.Mvc.IActionResult] GetCartCount(System.Threading.CancellationToken) on controller WebApplication.Controllers.CartController (WebApplication).
info: Microsoft.AspNetCore.Mvc.Infrastructure.SystemTextJsonResultExecutor[1]
      Executing JsonResult, writing value of type 'WebApplication.Models.ApiResponse'.
info: Microsoft.AspNetCore.Mvc.Infrastructure.ControllerActionInvoker[105]
      Executed action WebApplication.Controllers.CartController.GetCartCount (WebApplication) in 3.4844ms
info: Microsoft.AspNetCore.Routing.EndpointMiddleware[1]
      Executed endpoint 'WebApplication.Controllers.CartController.GetCartCount (WebApplication)'
info: Microsoft.AspNetCore.Hosting.Diagnostics[2]
      Request finished HTTP/2 GET https://localhost:7177/Cart/GetCartCount - 200 - application/json;+charset=utf-8 16.0402ms
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
      Executed DbCommand (60ms) [Parameters=[@p0='?' (Size = 4000), @p1='?' (Size = 20), @p2='?' (DbType = DateTime2), @p3='?' (Size = 500), @p4='?' (DbType = Boolean), @p5='?' (Size = 100), @p6='?' (DbType = Int32), @p7='?' (DbType = DateTime2), @p8='?' (Size = 255), @p9='?' (DbType = Int32), @p10='?' (DbType = DateTime2), @p11='?' (Size = 20), @p12='?' (Size = 255), @p13='?' (DbType = Int32), @p14='?' (DbType = Int32)], CommandType='Text', CommandTimeout='30']
      SET IMPLICIT_TRANSACTIONS OFF;
      SET NOCOUNT ON;
      INSERT INTO [Notification] ([Body], [Channel], [CreatedAt], [FailureReason], [IsRead], [NotifType], [OrderId], [ReadAt], [Recipient], [RetryCount], [SentAt], [Status], [Subject], [TicketId], [UserId])
      OUTPUT INSERTED.[NotificationId]
      VALUES (@p0, @p1, @p2, @p3, @p4, @p5, @p6, @p7, @p8, @p9, @p10, @p11, @p12, @p13, @p14);
info: Microsoft.EntityFrameworkCore.Database.Command[20101]
      Executed DbCommand (57ms) [Parameters=[@__8__locals2_variant_ProductId_0='?' (DbType = Int32)], CommandType='Text', CommandTimeout='30']
      SELECT [u].[UserId], [u].[Email], [u].[FirstName]
      FROM [User] AS [u]
      WHERE [u].[IsDeleted] = CAST(0 AS bit) AND [u].[IsActive] = CAST(1 AS bit) AND [u].[IsWalkIn] = CAST(0 AS bit) AND [u].[Email] IS NOT NULL AND EXISTS (
          SELECT 1
          FROM [Wishlist] AS [w]
          WHERE [u].[UserId] = [w].[UserId] AND [w].[ProductId] = @__8__locals2_variant_ProductId_0)
info: Microsoft.EntityFrameworkCore.Database.Command[20101]
      Executed DbCommand (61ms) [Parameters=[@p0='?' (Size = 4000), @p1='?' (Size = 20), @p2='?' (DbType = DateTime2), @p3='?' (Size = 500), @p4='?' (DbType = Boolean), @p5='?' (Size = 100), @p6='?' (DbType = Int32), @p7='?' (DbType = DateTime2), @p8='?' (Size = 255), @p9='?' (DbType = Int32), @p10='?' (DbType = DateTime2), @p11='?' (Size = 20), @p12='?' (Size = 255), @p13='?' (DbType = Int32), @p14='?' (DbType = Int32)], CommandType='Text', CommandTimeout='30']
      SET IMPLICIT_TRANSACTIONS OFF;
      SET NOCOUNT ON;
      INSERT INTO [Notification] ([Body], [Channel], [CreatedAt], [FailureReason], [IsRead], [NotifType], [OrderId], [ReadAt], [Recipient], [RetryCount], [SentAt], [Status], [Subject], [TicketId], [UserId])
      OUTPUT INSERTED.[NotificationId]
      VALUES (@p0, @p1, @p2, @p3, @p4, @p5, @p6, @p7, @p8, @p9, @p10, @p11, @p12, @p13, @p14);
warn: WebApplication.BackgroundJobs.StockMonitorJob[0]
      StockMonitorJob: low stock - variant 5 (Pinewood Trident Flux / Default): 2 remaining (threshold 5).
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
      Executed DbCommand (60ms) [Parameters=[@p0='?' (Size = 4000), @p1='?' (Size = 20), @p2='?' (DbType = DateTime2), @p3='?' (Size = 500), @p4='?' (DbType = Boolean), @p5='?' (Size = 100), @p6='?' (DbType = Int32), @p7='?' (DbType = DateTime2), @p8='?' (Size = 255), @p9='?' (DbType = Int32), @p10='?' (DbType = DateTime2), @p11='?' (Size = 20), @p12='?' (Size = 255), @p13='?' (DbType = Int32), @p14='?' (DbType = Int32)], CommandType='Text', CommandTimeout='30']
      SET IMPLICIT_TRANSACTIONS OFF;
      SET NOCOUNT ON;
      INSERT INTO [Notification] ([Body], [Channel], [CreatedAt], [FailureReason], [IsRead], [NotifType], [OrderId], [ReadAt], [Recipient], [RetryCount], [SentAt], [Status], [Subject], [TicketId], [UserId])
      OUTPUT INSERTED.[NotificationId]
      VALUES (@p0, @p1, @p2, @p3, @p4, @p5, @p6, @p7, @p8, @p9, @p10, @p11, @p12, @p13, @p14);
info: Microsoft.EntityFrameworkCore.Database.Command[20101]
      Executed DbCommand (58ms) [Parameters=[@__8__locals2_variant_ProductId_0='?' (DbType = Int32)], CommandType='Text', CommandTimeout='30']
      SELECT [u].[UserId], [u].[Email], [u].[FirstName]
      FROM [User] AS [u]
      WHERE [u].[IsDeleted] = CAST(0 AS bit) AND [u].[IsActive] = CAST(1 AS bit) AND [u].[IsWalkIn] = CAST(0 AS bit) AND [u].[Email] IS NOT NULL AND EXISTS (
          SELECT 1
          FROM [Wishlist] AS [w]
          WHERE [u].[UserId] = [w].[UserId] AND [w].[ProductId] = @__8__locals2_variant_ProductId_0)
info: Microsoft.EntityFrameworkCore.Database.Command[20101]
      Executed DbCommand (60ms) [Parameters=[@p0='?' (Size = 4000), @p1='?' (Size = 20), @p2='?' (DbType = DateTime2), @p3='?' (Size = 500), @p4='?' (DbType = Boolean), @p5='?' (Size = 100), @p6='?' (DbType = Int32), @p7='?' (DbType = DateTime2), @p8='?' (Size = 255), @p9='?' (DbType = Int32), @p10='?' (DbType = DateTime2), @p11='?' (Size = 20), @p12='?' (Size = 255), @p13='?' (DbType = Int32), @p14='?' (DbType = Int32)], CommandType='Text', CommandTimeout='30']
      SET IMPLICIT_TRANSACTIONS OFF;
      SET NOCOUNT ON;
      INSERT INTO [Notification] ([Body], [Channel], [CreatedAt], [FailureReason], [IsRead], [NotifType], [OrderId], [ReadAt], [Recipient], [RetryCount], [SentAt], [Status], [Subject], [TicketId], [UserId])
      OUTPUT INSERTED.[NotificationId]
      VALUES (@p0, @p1, @p2, @p3, @p4, @p5, @p6, @p7, @p8, @p9, @p10, @p11, @p12, @p13, @p14);
warn: WebApplication.BackgroundJobs.StockMonitorJob[0]
      StockMonitorJob: low stock - variant 7 (Pinewood Challenger / Default): 2 remaining (threshold 5).
info: Microsoft.EntityFrameworkCore.Database.Command[20101]
      Executed DbCommand (60ms) [Parameters=[@__8__locals1_cooldownStart_0='?' (DbType = DateTime2), @__Format_2='?' (Size = 1000)], CommandType='Text', CommandTimeout='30']
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
      Executed DbCommand (59ms) [Parameters=[@p0='?' (Size = 4000), @p1='?' (Size = 20), @p2='?' (DbType = DateTime2), @p3='?' (Size = 500), @p4='?' (DbType = Boolean), @p5='?' (Size = 100), @p6='?' (DbType = Int32), @p7='?' (DbType = DateTime2), @p8='?' (Size = 255), @p9='?' (DbType = Int32), @p10='?' (DbType = DateTime2), @p11='?' (Size = 20), @p12='?' (Size = 255), @p13='?' (DbType = Int32), @p14='?' (DbType = Int32)], CommandType='Text', CommandTimeout='30']
      SET IMPLICIT_TRANSACTIONS OFF;
      SET NOCOUNT ON;
      INSERT INTO [Notification] ([Body], [Channel], [CreatedAt], [FailureReason], [IsRead], [NotifType], [OrderId], [ReadAt], [Recipient], [RetryCount], [SentAt], [Status], [Subject], [TicketId], [UserId])
      OUTPUT INSERTED.[NotificationId]
      VALUES (@p0, @p1, @p2, @p3, @p4, @p5, @p6, @p7, @p8, @p9, @p10, @p11, @p12, @p13, @p14);
info: Microsoft.EntityFrameworkCore.Database.Command[20101]
      Executed DbCommand (58ms) [Parameters=[@__8__locals2_variant_ProductId_0='?' (DbType = Int32)], CommandType='Text', CommandTimeout='30']
      SELECT [u].[UserId], [u].[Email], [u].[FirstName]
      FROM [User] AS [u]
      WHERE [u].[IsDeleted] = CAST(0 AS bit) AND [u].[IsActive] = CAST(1 AS bit) AND [u].[IsWalkIn] = CAST(0 AS bit) AND [u].[Email] IS NOT NULL AND EXISTS (
          SELECT 1
          FROM [Wishlist] AS [w]
          WHERE [u].[UserId] = [w].[UserId] AND [w].[ProductId] = @__8__locals2_variant_ProductId_0)
info: Microsoft.EntityFrameworkCore.Database.Command[20101]
      Executed DbCommand (60ms) [Parameters=[@p0='?' (Size = 4000), @p1='?' (Size = 20), @p2='?' (DbType = DateTime2), @p3='?' (Size = 500), @p4='?' (DbType = Boolean), @p5='?' (Size = 100), @p6='?' (DbType = Int32), @p7='?' (DbType = DateTime2), @p8='?' (Size = 255), @p9='?' (DbType = Int32), @p10='?' (DbType = DateTime2), @p11='?' (Size = 20), @p12='?' (Size = 255), @p13='?' (DbType = Int32), @p14='?' (DbType = Int32)], CommandType='Text', CommandTimeout='30']
      SET IMPLICIT_TRANSACTIONS OFF;
      SET NOCOUNT ON;
      INSERT INTO [Notification] ([Body], [Channel], [CreatedAt], [FailureReason], [IsRead], [NotifType], [OrderId], [ReadAt], [Recipient], [RetryCount], [SentAt], [Status], [Subject], [TicketId], [UserId])
      OUTPUT INSERTED.[NotificationId]
      VALUES (@p0, @p1, @p2, @p3, @p4, @p5, @p6, @p7, @p8, @p9, @p10, @p11, @p12, @p13, @p14);
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
info: Microsoft.EntityFrameworkCore.Database.Command[20101]
      Executed DbCommand (60ms) [Parameters=[@p0='?' (DbType = DateTime2), @p1='?' (Size = 1000), @p2='?' (Size = 100), @p3='?' (DbType = Int32)], CommandType='Text', CommandTimeout='30']
      SET IMPLICIT_TRANSACTIONS OFF;
      SET NOCOUNT ON;
      INSERT INTO [SystemLog] ([CreatedAt], [EventDescription], [EventType], [UserId])
      OUTPUT INSERTED.[SystemLogId]
      VALUES (@p0, @p1, @p2, @p3);
info: Microsoft.EntityFrameworkCore.Database.Command[20101]
      Executed DbCommand (59ms) [Parameters=[@p0='?' (Size = 4000), @p1='?' (Size = 20), @p2='?' (DbType = DateTime2), @p3='?' (Size = 500), @p4='?' (DbType = Boolean), @p5='?' (Size = 100), @p6='?' (DbType = Int32), @p7='?' (DbType = DateTime2), @p8='?' (Size = 255), @p9='?' (DbType = Int32), @p10='?' (DbType = DateTime2), @p11='?' (Size = 20), @p12='?' (Size = 255), @p13='?' (DbType = Int32), @p14='?' (DbType = Int32)], CommandType='Text', CommandTimeout='30']
      SET IMPLICIT_TRANSACTIONS OFF;
      SET NOCOUNT ON;
      INSERT INTO [Notification] ([Body], [Channel], [CreatedAt], [FailureReason], [IsRead], [NotifType], [OrderId], [ReadAt], [Recipient], [RetryCount], [SentAt], [Status], [Subject], [TicketId], [UserId])
      OUTPUT INSERTED.[NotificationId]
      VALUES (@p0, @p1, @p2, @p3, @p4, @p5, @p6, @p7, @p8, @p9, @p10, @p11, @p12, @p13, @p14);
info: Microsoft.EntityFrameworkCore.Database.Command[20101]
      Executed DbCommand (57ms) [Parameters=[@__8__locals2_variant_ProductId_0='?' (DbType = Int32)], CommandType='Text', CommandTimeout='30']
      SELECT [u].[UserId], [u].[Email], [u].[FirstName]
      FROM [User] AS [u]
      WHERE [u].[IsDeleted] = CAST(0 AS bit) AND [u].[IsActive] = CAST(1 AS bit) AND [u].[IsWalkIn] = CAST(0 AS bit) AND [u].[Email] IS NOT NULL AND EXISTS (
          SELECT 1
          FROM [Wishlist] AS [w]
          WHERE [u].[UserId] = [w].[UserId] AND [w].[ProductId] = @__8__locals2_variant_ProductId_0)
warn: WebApplication.BackgroundJobs.StockMonitorJob[0]
      StockMonitorJob: low stock - variant 9 (Pinewood Lancer 1.0 2022 Gravel RX (2x9) / Default): 3 remaining (threshold 5).
info: Microsoft.EntityFrameworkCore.Database.Command[20101]
      Executed DbCommand (58ms) [Parameters=[@__8__locals1_cooldownStart_0='?' (DbType = DateTime2), @__Format_2='?' (Size = 1000)], CommandType='Text', CommandTimeout='30']
      SELECT CASE
          WHEN EXISTS (
              SELECT 1
              FROM [SystemLog] AS [s]
              WHERE [s].[EventType] = N'LowStockTriggered' AND [s].[CreatedAt] > @__8__locals1_cooldownStart_0 AND [s].[EventDescription] LIKE @__Format_2) THEN CAST(1 AS bit)
          ELSE CAST(0 AS bit)
      END
info: Microsoft.EntityFrameworkCore.Database.Command[20101]
      Executed DbCommand (65ms) [Parameters=[@p0='?' (DbType = DateTime2), @p1='?' (Size = 1000), @p2='?' (Size = 100), @p3='?' (DbType = Int32)], CommandType='Text', CommandTimeout='30']
      SET IMPLICIT_TRANSACTIONS OFF;
      SET NOCOUNT ON;
      INSERT INTO [SystemLog] ([CreatedAt], [EventDescription], [EventType], [UserId])
      OUTPUT INSERTED.[SystemLogId]
      VALUES (@p0, @p1, @p2, @p3);
info: Microsoft.EntityFrameworkCore.Database.Command[20101]
      Executed DbCommand (59ms) [Parameters=[@p0='?' (Size = 4000), @p1='?' (Size = 20), @p2='?' (DbType = DateTime2), @p3='?' (Size = 500), @p4='?' (DbType = Boolean), @p5='?' (Size = 100), @p6='?' (DbType = Int32), @p7='?' (DbType = DateTime2), @p8='?' (Size = 255), @p9='?' (DbType = Int32), @p10='?' (DbType = DateTime2), @p11='?' (Size = 20), @p12='?' (Size = 255), @p13='?' (DbType = Int32), @p14='?' (DbType = Int32)], CommandType='Text', CommandTimeout='30']
      SET IMPLICIT_TRANSACTIONS OFF;
      SET NOCOUNT ON;
      INSERT INTO [Notification] ([Body], [Channel], [CreatedAt], [FailureReason], [IsRead], [NotifType], [OrderId], [ReadAt], [Recipient], [RetryCount], [SentAt], [Status], [Subject], [TicketId], [UserId])
      OUTPUT INSERTED.[NotificationId]
      VALUES (@p0, @p1, @p2, @p3, @p4, @p5, @p6, @p7, @p8, @p9, @p10, @p11, @p12, @p13, @p14);
info: Microsoft.AspNetCore.Hosting.Diagnostics[1]
      Request starting HTTP/2 POST https://localhost:7177/Customer/Login - application/x-www-form-urlencoded 245
info: Microsoft.AspNetCore.Cors.Infrastructure.CorsService[4]
      CORS policy execution successful.
info: Microsoft.AspNetCore.Routing.EndpointMiddleware[0]
      Executing endpoint 'WebApplication.Controllers.CustomerController.Login (WebApplication)'
info: Microsoft.AspNetCore.Mvc.Infrastructure.ControllerActionInvoker[102]
      Route matched with {action = "Login", controller = "Customer"}. Executing controller action with signature System.Threading.Tasks.Task`1[Microsoft.AspNetCore.Mvc.IActionResult] Login(WebApplication.Models.ViewModels.LoginViewModel, System.Threading.CancellationToken) on controller WebApplication.Controllers.CustomerController (WebApplication).
info: Microsoft.EntityFrameworkCore.Database.Command[20101]
      Executed DbCommand (58ms) [Parameters=[@__8__locals2_variant_ProductId_0='?' (DbType = Int32)], CommandType='Text', CommandTimeout='30']
      SELECT [u].[UserId], [u].[Email], [u].[FirstName]
      FROM [User] AS [u]
      WHERE [u].[IsDeleted] = CAST(0 AS bit) AND [u].[IsActive] = CAST(1 AS bit) AND [u].[IsWalkIn] = CAST(0 AS bit) AND [u].[Email] IS NOT NULL AND EXISTS (
          SELECT 1
          FROM [Wishlist] AS [w]
          WHERE [u].[UserId] = [w].[UserId] AND [w].[ProductId] = @__8__locals2_variant_ProductId_0)
info: Microsoft.EntityFrameworkCore.Database.Command[20101]
      Executed DbCommand (61ms) [Parameters=[@p0='?' (DbType = DateTime2), @p1='?' (Size = 1000), @p2='?' (Size = 100), @p3='?' (DbType = Int32)], CommandType='Text', CommandTimeout='30']
      SET IMPLICIT_TRANSACTIONS OFF;
      SET NOCOUNT ON;
      INSERT INTO [SystemLog] ([CreatedAt], [EventDescription], [EventType], [UserId])
      OUTPUT INSERTED.[SystemLogId]
      VALUES (@p0, @p1, @p2, @p3);
info: Microsoft.EntityFrameworkCore.Database.Command[20101]
      Executed DbCommand (68ms) [Parameters=[@__email_0='?' (Size = 255)], CommandType='Text', CommandTimeout='30']
      SELECT TOP(1) [u].[UserId], [u].[CreatedAt], [u].[DefaultAddressId], [u].[Email], [u].[FailedLoginAttempts], [u].[FirstName], [u].[IsActive], [u].[IsDeleted], [u].[IsWalkIn], [u].[LastLoginAt], [u].[LastName], [u].[LockoutUntil], [u].[PasswordHash], [u].[PhoneNumber]
      FROM [User] AS [u]
      WHERE [u].[IsDeleted] = CAST(0 AS bit) AND [u].[Email] = @__email_0 AND [u].[IsActive] = CAST(1 AS bit)
info: Microsoft.EntityFrameworkCore.Database.Command[20101]
      Executed DbCommand (64ms) [Parameters=[@p0='?' (DbType = DateTime2), @p1='?' (Size = 1000), @p2='?' (Size = 100), @p3='?' (DbType = Int32)], CommandType='Text', CommandTimeout='30']
      SET IMPLICIT_TRANSACTIONS OFF;
      SET NOCOUNT ON;
      INSERT INTO [SystemLog] ([CreatedAt], [EventDescription], [EventType], [UserId])
      OUTPUT INSERTED.[SystemLogId]
      VALUES (@p0, @p1, @p2, @p3);
info: Microsoft.EntityFrameworkCore.Database.Command[20101]
      Executed DbCommand (61ms) [Parameters=[@__p_0='?' (DbType = Int32)], CommandType='Text', CommandTimeout='30']
      SELECT TOP(1) [u].[UserId], [u].[CreatedAt], [u].[DefaultAddressId], [u].[Email], [u].[FailedLoginAttempts], [u].[FirstName], [u].[IsActive], [u].[IsDeleted], [u].[IsWalkIn], [u].[LastLoginAt], [u].[LastName], [u].[LockoutUntil], [u].[PasswordHash], [u].[PhoneNumber]
      FROM [User] AS [u]
      WHERE [u].[IsDeleted] = CAST(0 AS bit) AND [u].[UserId] = @__p_0
info: Microsoft.AspNetCore.Authentication.Cookies.CookieAuthenticationHandler[10]
      AuthenticationScheme: Cookies signed in.
info: Microsoft.AspNetCore.Mvc.Infrastructure.RedirectResultExecutor[1]
      Executing RedirectResult, redirecting to /Customer/Dashboard.
info: Microsoft.AspNetCore.Mvc.Infrastructure.ControllerActionInvoker[105]
      Executed action WebApplication.Controllers.CustomerController.Login (WebApplication) in 676.505ms
info: Microsoft.AspNetCore.Routing.EndpointMiddleware[1]
      Executed endpoint 'WebApplication.Controllers.CustomerController.Login (WebApplication)'
info: Microsoft.AspNetCore.Hosting.Diagnostics[2]
      Request finished HTTP/2 POST https://localhost:7177/Customer/Login - 302 - - 697.0937ms
info: Microsoft.AspNetCore.Hosting.Diagnostics[1]
      Request starting HTTP/2 GET https://localhost:7177/Customer/Dashboard - - -
info: Microsoft.AspNetCore.Routing.EndpointMiddleware[0]
      Executing endpoint 'WebApplication.Controllers.CustomerController.Dashboard (WebApplication)'
info: Microsoft.AspNetCore.Mvc.Infrastructure.ControllerActionInvoker[102]
      Route matched with {action = "Dashboard", controller = "Customer"}. Executing controller action with signature System.Threading.Tasks.Task`1[Microsoft.AspNetCore.Mvc.IActionResult] Dashboard(System.Threading.CancellationToken) on controller WebApplication.Controllers.CustomerController (WebApplication).
info: Microsoft.EntityFrameworkCore.Database.Command[20101]
      Executed DbCommand (110ms) [Parameters=[@__userId_0='?' (DbType = Int32), @__p_1='?' (DbType = Int32), @__p_2='?' (DbType = Int32)], CommandType='Text', CommandTimeout='30']
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
      Executed DbCommand (202ms) [Parameters=[@__now_0='?' (DbType = DateTime2)], CommandType='Text', CommandTimeout='30']
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
      Executed DbCommand (94ms) [Parameters=[@__userId_0='?' (DbType = Int32), @__p_1='?' (DbType = Int32), @__p_2='?' (DbType = Int32)], CommandType='Text', CommandTimeout='30']
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
      Executed DbCommand (63ms) [Parameters=[@p0='?' (DbType = DateTime2), @p1='?' (Size = 1000), @p2='?' (Size = 100), @p3='?' (DbType = Int32)], CommandType='Text', CommandTimeout='30']
      SET IMPLICIT_TRANSACTIONS OFF;
      SET NOCOUNT ON;
      INSERT INTO [SystemLog] ([CreatedAt], [EventDescription], [EventType], [UserId])
      OUTPUT INSERTED.[SystemLogId]
      VALUES (@p0, @p1, @p2, @p3);
info: Microsoft.EntityFrameworkCore.Database.Command[20101]
      Executed DbCommand (67ms) [Parameters=[@__userId_0='?' (DbType = Int32)], CommandType='Text', CommandTimeout='30']
      SELECT COUNT(*)
      FROM [Order] AS [o]
      WHERE [o].[IsDeleted] = CAST(0 AS bit) AND [o].[UserId] = @__userId_0
info: Microsoft.EntityFrameworkCore.Database.Command[20101]
      Executed DbCommand (60ms) [Parameters=[@__userId_0='?' (DbType = Int32)], CommandType='Text', CommandTimeout='30']
      SELECT COUNT(*)
      FROM [Wishlist] AS [w]
      WHERE [w].[UserId] = @__userId_0
info: Microsoft.EntityFrameworkCore.Database.Command[20101]
      Executed DbCommand (63ms) [Parameters=[@__userId_0='?' (DbType = Int32)], CommandType='Text', CommandTimeout='30']
      SELECT [r].[ProductId]
      FROM [Review] AS [r]
      WHERE [r].[UserId] = @__userId_0
info: Microsoft.EntityFrameworkCore.Database.Command[20101]
      Executed DbCommand (219ms) [Parameters=[@__userId_0='?' (DbType = Int32), @__reviewedProductIds_1='?' (Size = 4000)], CommandType='Text', CommandTimeout='30']
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
      Executed DbCommand (68ms) [Parameters=[@__userId_0='?' (DbType = Int32)], CommandType='Text', CommandTimeout='30']
      SELECT [s].[TicketId], [s].[AssignedToUserId], [s].[AttachmentBucket], [s].[AttachmentPath], [s].[AttachmentUrl], [s].[CreatedAt], [s].[Description], [s].[OrderId], [s].[ResolvedAt], [s].[Subject], [s].[TicketCategory], [s].[TicketSource], [s].[TicketStatus], [s].[UpdatedAt], [s].[UserId]
      FROM [SupportTicket] AS [s]
      WHERE [s].[UserId] = @__userId_0
      ORDER BY [s].[CreatedAt] DESC
info: Microsoft.AspNetCore.Mvc.ViewFeatures.ViewResultExecutor[1]
      Executing ViewResult, running view Dashboard.
info: Microsoft.AspNetCore.Mvc.ViewFeatures.ViewResultExecutor[4]
      Executed ViewResult - view Dashboard executed in 27.7287ms.
info: Microsoft.AspNetCore.Mvc.Infrastructure.ControllerActionInvoker[105]
      Executed action WebApplication.Controllers.CustomerController.Dashboard (WebApplication) in 788.5168ms
info: Microsoft.AspNetCore.Routing.EndpointMiddleware[1]
      Executed endpoint 'WebApplication.Controllers.CustomerController.Dashboard (WebApplication)'
info: Microsoft.AspNetCore.Hosting.Diagnostics[1]
      Request starting HTTP/2 GET https://localhost:7177/_framework/aspnetcore-browser-refresh.js - - -
info: Microsoft.AspNetCore.Hosting.Diagnostics[1]
      Request starting HTTP/2 GET https://localhost:7177/js/customer.js?v=7Zh6mYXwHhsuB8ZSapLOSj7hIGUbeBzxNamE_svPPRI - - -
info: Microsoft.AspNetCore.Hosting.Diagnostics[1]
      Request starting HTTP/2 GET https://localhost:7177/_vs/browserLink - - -
info: Microsoft.AspNetCore.Hosting.Diagnostics[2]
      Request finished HTTP/2 GET https://localhost:7177/Customer/Dashboard - 200 - text/html;+charset=utf-8 805.3725ms
info: Microsoft.AspNetCore.StaticFiles.StaticFileMiddleware[6]
      The file /js/customer.js was not modified
info: Microsoft.AspNetCore.Hosting.Diagnostics[2]
      Request finished HTTP/2 GET https://localhost:7177/js/customer.js?v=7Zh6mYXwHhsuB8ZSapLOSj7hIGUbeBzxNamE_svPPRI - 304 - text/javascript 6.9423ms
info: Microsoft.AspNetCore.Hosting.Diagnostics[2]
      Request finished HTTP/2 GET https://localhost:7177/_framework/aspnetcore-browser-refresh.js - 200 14933 application/javascript;+charset=utf-8 8.2391ms
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
info: Microsoft.AspNetCore.Http.Result.OkObjectResult[1]
      Setting HTTP status code 200.
info: Microsoft.AspNetCore.Hosting.Diagnostics[2]
      Request finished HTTP/2 GET https://localhost:7177/_vs/browserLink - 200 - text/javascript;+charset=UTF-8 57.8600ms
info: Microsoft.AspNetCore.Http.Result.OkObjectResult[3]
      Writing value of type '<>f__AnonymousType19`1' as Json.
info: Microsoft.AspNetCore.Routing.EndpointMiddleware[1]
      Executed endpoint 'HTTP: GET /antiforgery/token'
info: Microsoft.AspNetCore.Hosting.Diagnostics[2]
      Request finished HTTP/2 GET https://localhost:7177/antiforgery/token - 200 - application/json;+charset=utf-8 44.3233ms
info: Microsoft.AspNetCore.Mvc.Infrastructure.ControllerActionInvoker[102]
      Route matched with {action = "NotificationCount", controller = "Customer"}. Executing controller action with signature System.Threading.Tasks.Task`1[Microsoft.AspNetCore.Mvc.IActionResult] NotificationCount(System.Threading.CancellationToken) on controller WebApplication.Controllers.CustomerController (WebApplication).
info: Microsoft.EntityFrameworkCore.Database.Command[20101]
      Executed DbCommand (64ms) [Parameters=[@__userId_0='?' (DbType = Int32)], CommandType='Text', CommandTimeout='30']
      SELECT COUNT(*)
      FROM [Notification] AS [n]
      WHERE [n].[UserId] = @__userId_0 AND [n].[IsRead] = CAST(0 AS bit) AND [n].[Channel] = N'InApp'
info: Microsoft.AspNetCore.Mvc.Infrastructure.SystemTextJsonResultExecutor[1]
      Executing JsonResult, writing value of type '<>f__AnonymousType5`1[[System.Int32, System.Private.CoreLib, Version=8.0.0.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e]]'.
info: Microsoft.AspNetCore.Mvc.Infrastructure.ControllerActionInvoker[105]
      Executed action WebApplication.Controllers.CustomerController.NotificationCount (WebApplication) in 71.6922ms
info: Microsoft.AspNetCore.Routing.EndpointMiddleware[1]
      Executed endpoint 'WebApplication.Controllers.CustomerController.NotificationCount (WebApplication)'
info: Microsoft.AspNetCore.Hosting.Diagnostics[2]
      Request finished HTTP/2 GET https://localhost:7177/Customer/NotificationCount - 200 - application/json;+charset=utf-8 122.4489ms
info: Microsoft.EntityFrameworkCore.Database.Command[20101]
      Executed DbCommand (76ms) [Parameters=[@__userId_Value_0='?' (DbType = Int32)], CommandType='Text', CommandTimeout='30']
      SELECT TOP(1) [c].[CartId], [c].[CreatedAt], [c].[GuestSessionId], [c].[IsCheckedOut], [c].[LastUpdatedAt], [c].[UserId]
      FROM [Cart] AS [c]
      WHERE [c].[IsCheckedOut] = CAST(0 AS bit) AND [c].[UserId] = @__userId_Value_0
      ORDER BY [c].[CartId]
info: Microsoft.AspNetCore.Mvc.Infrastructure.SystemTextJsonResultExecutor[1]
      Executing JsonResult, writing value of type 'WebApplication.Models.ApiResponse'.
info: Microsoft.AspNetCore.Mvc.Infrastructure.ControllerActionInvoker[105]
      Executed action WebApplication.Controllers.CartController.GetCartCount (WebApplication) in 117.4601ms
info: Microsoft.AspNetCore.Routing.EndpointMiddleware[1]
      Executed endpoint 'WebApplication.Controllers.CartController.GetCartCount (WebApplication)'
info: Microsoft.AspNetCore.Hosting.Diagnostics[2]
      Request finished HTTP/2 GET https://localhost:7177/Cart/GetCartCount - 200 - application/json;+charset=utf-8 155.8246ms
info: Microsoft.EntityFrameworkCore.Database.Command[20101]
      Executed DbCommand (64ms) [Parameters=[@p0='?' (DbType = DateTime2), @p1='?' (Size = 1000), @p2='?' (Size = 100), @p3='?' (DbType = Int32)], CommandType='Text', CommandTimeout='30']
      SET IMPLICIT_TRANSACTIONS OFF;
      SET NOCOUNT ON;
      INSERT INTO [SystemLog] ([CreatedAt], [EventDescription], [EventType], [UserId])
      OUTPUT INSERTED.[SystemLogId]
      VALUES (@p0, @p1, @p2, @p3);
info: Microsoft.EntityFrameworkCore.Database.Command[20101]
      Executed DbCommand (75ms) [Parameters=[@__cancelThreshold_0='?' (DbType = DateTime2)], CommandType='Text', CommandTimeout='30']
      SELECT [o].[OrderId], [o].[CartId], [o].[ContactPhone], [o].[CreatedAt], [o].[DeliveryInstructions], [o].[DiscountAmount], [o].[FulfillmentType], [o].[GuestSessionId], [o].[IsDeleted], [o].[IsWalkIn], [o].[OrderDate], [o].[OrderNumber], [o].[OrderStatus], [o].[POSSessionId], [o].[PaymentMethod], [o].[ShippingAddressId], [o].[ShippingFee], [o].[SubTotal], [o].[TotalAmount], [o].[UpdatedAt], [o].[UserId], [t].[UserId], [t].[CreatedAt], [t].[DefaultAddressId], [t].[Email], [t].[FailedLoginAttempts], [t].[FirstName], [t].[IsActive], [t].[IsDeleted], [t].[IsWalkIn], [t].[LastLoginAt], [t].[LastName], [t].[LockoutUntil], [t].[PasswordHash], [t].[PhoneNumber]
      FROM [Order] AS [o]
      INNER JOIN (
          SELECT [u].[UserId], [u].[CreatedAt], [u].[DefaultAddressId], [u].[Email], [u].[FailedLoginAttempts], [u].[FirstName], [u].[IsActive], [u].[IsDeleted], [u].[IsWalkIn], [u].[LastLoginAt], [u].[LastName], [u].[LockoutUntil], [u].[PasswordHash], [u].[PhoneNumber]
          FROM [User] AS [u]
          WHERE [u].[IsDeleted] = CAST(0 AS bit)
      ) AS [t] ON [o].[UserId] = [t].[UserId]
      WHERE [o].[IsDeleted] = CAST(0 AS bit) AND [o].[OrderStatus] = N'Pending' AND [o].[OrderDate] < @__cancelThreshold_0
info: Microsoft.EntityFrameworkCore.Database.Command[20101]
      Executed DbCommand (64ms) [Parameters=[], CommandType='Text', CommandTimeout='30']
      SELECT DISTINCT [n].[OrderId]
      FROM [Notification] AS [n]
      WHERE [n].[NotifType] = N'PendingOrderReminder' AND [n].[OrderId] IS NOT NULL
info: Microsoft.EntityFrameworkCore.Database.Command[20101]
      Executed DbCommand (90ms) [Parameters=[@__reminderThreshold_0='?' (DbType = DateTime2), @__cancelThreshold_1='?' (DbType = DateTime2), @__alreadyReminded_2='?' (Size = 4000)], CommandType='Text', CommandTimeout='30']
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
      Executed DbCommand (74ms) [Parameters=[@p2='?' (DbType = Int32), @p0='?' (Size = 50), @p1='?' (DbType = DateTime2), @p3='?' (DbType = DateTime2), @p4='?' (Size = 1000), @p5='?' (Size = 100), @p6='?' (DbType = Int32)], CommandType='Text', CommandTimeout='30']
      SET NOCOUNT ON;
      UPDATE [Order] SET [OrderStatus] = @p0, [UpdatedAt] = @p1
      OUTPUT INSERTED.[TotalAmount]
      WHERE [OrderId] = @p2;
      INSERT INTO [SystemLog] ([CreatedAt], [EventDescription], [EventType], [UserId])
      OUTPUT INSERTED.[SystemLogId]
      VALUES (@p3, @p4, @p5, @p6);
info: Microsoft.EntityFrameworkCore.Database.Command[20101]
      Executed DbCommand (81ms) [Parameters=[@p0='?' (Size = 4000), @p1='?' (Size = 20), @p2='?' (DbType = DateTime2), @p3='?' (Size = 500), @p4='?' (DbType = Boolean), @p5='?' (Size = 100), @p6='?' (DbType = Int32), @p7='?' (DbType = DateTime2), @p8='?' (Size = 255), @p9='?' (DbType = Int32), @p10='?' (DbType = DateTime2), @p11='?' (Size = 20), @p12='?' (Size = 255), @p13='?' (DbType = Int32), @p14='?' (DbType = Int32)], CommandType='Text', CommandTimeout='30']
      SET IMPLICIT_TRANSACTIONS OFF;
      SET NOCOUNT ON;
      INSERT INTO [Notification] ([Body], [Channel], [CreatedAt], [FailureReason], [IsRead], [NotifType], [OrderId], [ReadAt], [Recipient], [RetryCount], [SentAt], [Status], [Subject], [TicketId], [UserId])
      OUTPUT INSERTED.[NotificationId]
      VALUES (@p0, @p1, @p2, @p3, @p4, @p5, @p6, @p7, @p8, @p9, @p10, @p11, @p12, @p13, @p14);
fail: Microsoft.EntityFrameworkCore.Update[10000]
      An exception occurred in the database while saving changes for context type 'WebApplication.DataAccess.Context.AppDbContext'.
      Microsoft.EntityFrameworkCore.DbUpdateException: An error occurred while saving the entity changes. See the inner exception for details.
       ---> Microsoft.Data.SqlClient.SqlException (0x80131904): The INSERT statement conflicted with the CHECK constraint "CK_Notif_Type". The conflict occurred in database "Taurus-bike-shop-sqlserver-2026", table "dbo.Notification", column 'NotifType'.
         at Microsoft.Data.SqlClient.SqlConnection.OnError(SqlException exception, Boolean breakConnection, Action`1 wrapCloseInAction)
         at Microsoft.Data.SqlClient.SqlInternalConnection.OnError(SqlException exception, Boolean breakConnection, Action`1 wrapCloseInAction)
         at Microsoft.Data.SqlClient.TdsParser.ThrowExceptionAndWarning(TdsParserStateObject stateObj, Boolean callerHasConnectionLock, Boolean asyncClose)
         at Microsoft.Data.SqlClient.TdsParser.TryRun(RunBehavior runBehavior, SqlCommand cmdHandler, SqlDataReader dataStream, BulkCopySimpleResultSet bulkCopyHandler, TdsParserStateObject stateObj, Boolean& dataReady)
         at Microsoft.Data.SqlClient.SqlDataReader.TryHasMoreRows(Boolean& moreRows)
         at Microsoft.Data.SqlClient.SqlDataReader.TryReadInternal(Boolean setTimeout, Boolean& more)
         at Microsoft.Data.SqlClient.SqlDataReader.ReadAsyncExecute(Task task, Object state)
         at Microsoft.Data.SqlClient.SqlDataReader.InvokeAsyncCall[T](SqlDataReaderBaseAsyncCallContext`1 context)
      --- End of stack trace from previous location ---
         at Microsoft.EntityFrameworkCore.Update.AffectedCountModificationCommandBatch.ConsumeResultSetAsync(Int32 startCommandIndex, RelationalDataReader reader, CancellationToken cancellationToken)
      ClientConnectionId:85508ea5-4556-41c0-a008-6cab0b844725
      Error Number:547,State:0,Class:16
         --- End of inner exception stack trace ---
         at Microsoft.EntityFrameworkCore.Update.AffectedCountModificationCommandBatch.ConsumeResultSetAsync(Int32 startCommandIndex, RelationalDataReader reader, CancellationToken cancellationToken)
         at Microsoft.EntityFrameworkCore.Update.AffectedCountModificationCommandBatch.ConsumeAsync(RelationalDataReader reader, CancellationToken cancellationToken)
         at Microsoft.EntityFrameworkCore.Update.ReaderModificationCommandBatch.ExecuteAsync(IRelationalConnection connection, CancellationToken cancellationToken)
         at Microsoft.EntityFrameworkCore.Update.ReaderModificationCommandBatch.ExecuteAsync(IRelationalConnection connection, CancellationToken cancellationToken)
         at Microsoft.EntityFrameworkCore.SqlServer.Update.Internal.SqlServerModificationCommandBatch.ExecuteAsync(IRelationalConnection connection, CancellationToken cancellationToken)
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
      Microsoft.EntityFrameworkCore.DbUpdateException: An error occurred while saving the entity changes. See the inner exception for details.
       ---> Microsoft.Data.SqlClient.SqlException (0x80131904): The INSERT statement conflicted with the CHECK constraint "CK_Notif_Type". The conflict occurred in database "Taurus-bike-shop-sqlserver-2026", table "dbo.Notification", column 'NotifType'.
         at Microsoft.Data.SqlClient.SqlConnection.OnError(SqlException exception, Boolean breakConnection, Action`1 wrapCloseInAction)
         at Microsoft.Data.SqlClient.SqlInternalConnection.OnError(SqlException exception, Boolean breakConnection, Action`1 wrapCloseInAction)
         at Microsoft.Data.SqlClient.TdsParser.ThrowExceptionAndWarning(TdsParserStateObject stateObj, Boolean callerHasConnectionLock, Boolean asyncClose)
         at Microsoft.Data.SqlClient.TdsParser.TryRun(RunBehavior runBehavior, SqlCommand cmdHandler, SqlDataReader dataStream, BulkCopySimpleResultSet bulkCopyHandler, TdsParserStateObject stateObj, Boolean& dataReady)
         at Microsoft.Data.SqlClient.SqlDataReader.TryHasMoreRows(Boolean& moreRows)
         at Microsoft.Data.SqlClient.SqlDataReader.TryReadInternal(Boolean setTimeout, Boolean& more)
         at Microsoft.Data.SqlClient.SqlDataReader.ReadAsyncExecute(Task task, Object state)
         at Microsoft.Data.SqlClient.SqlDataReader.InvokeAsyncCall[T](SqlDataReaderBaseAsyncCallContext`1 context)
      --- End of stack trace from previous location ---
         at Microsoft.EntityFrameworkCore.Update.AffectedCountModificationCommandBatch.ConsumeResultSetAsync(Int32 startCommandIndex, RelationalDataReader reader, CancellationToken cancellationToken)
      ClientConnectionId:85508ea5-4556-41c0-a008-6cab0b844725
      Error Number:547,State:0,Class:16
         --- End of inner exception stack trace ---
         at Microsoft.EntityFrameworkCore.Update.AffectedCountModificationCommandBatch.ConsumeResultSetAsync(Int32 startCommandIndex, RelationalDataReader reader, CancellationToken cancellationToken)
         at Microsoft.EntityFrameworkCore.Update.AffectedCountModificationCommandBatch.ConsumeAsync(RelationalDataReader reader, CancellationToken cancellationToken)
         at Microsoft.EntityFrameworkCore.Update.ReaderModificationCommandBatch.ExecuteAsync(IRelationalConnection connection, CancellationToken cancellationToken)
         at Microsoft.EntityFrameworkCore.Update.ReaderModificationCommandBatch.ExecuteAsync(IRelationalConnection connection, CancellationToken cancellationToken)
         at Microsoft.EntityFrameworkCore.SqlServer.Update.Internal.SqlServerModificationCommandBatch.ExecuteAsync(IRelationalConnection connection, CancellationToken cancellationToken)
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
      PendingOrderMonitorJob: failed to queue OrderAutoCancelled notification for order 36.
      Microsoft.EntityFrameworkCore.DbUpdateException: An error occurred while saving the entity changes. See the inner exception for details.
       ---> Microsoft.Data.SqlClient.SqlException (0x80131904): The INSERT statement conflicted with the CHECK constraint "CK_Notif_Type". The conflict occurred in database "Taurus-bike-shop-sqlserver-2026", table "dbo.Notification", column 'NotifType'.
         at Microsoft.Data.SqlClient.SqlConnection.OnError(SqlException exception, Boolean breakConnection, Action`1 wrapCloseInAction)
         at Microsoft.Data.SqlClient.SqlInternalConnection.OnError(SqlException exception, Boolean breakConnection, Action`1 wrapCloseInAction)
         at Microsoft.Data.SqlClient.TdsParser.ThrowExceptionAndWarning(TdsParserStateObject stateObj, Boolean callerHasConnectionLock, Boolean asyncClose)
         at Microsoft.Data.SqlClient.TdsParser.TryRun(RunBehavior runBehavior, SqlCommand cmdHandler, SqlDataReader dataStream, BulkCopySimpleResultSet bulkCopyHandler, TdsParserStateObject stateObj, Boolean& dataReady)
         at Microsoft.Data.SqlClient.SqlDataReader.TryHasMoreRows(Boolean& moreRows)
         at Microsoft.Data.SqlClient.SqlDataReader.TryReadInternal(Boolean setTimeout, Boolean& more)
         at Microsoft.Data.SqlClient.SqlDataReader.ReadAsyncExecute(Task task, Object state)
         at Microsoft.Data.SqlClient.SqlDataReader.InvokeAsyncCall[T](SqlDataReaderBaseAsyncCallContext`1 context)
      --- End of stack trace from previous location ---
         at Microsoft.EntityFrameworkCore.Update.AffectedCountModificationCommandBatch.ConsumeResultSetAsync(Int32 startCommandIndex, RelationalDataReader reader, CancellationToken cancellationToken)
      ClientConnectionId:85508ea5-4556-41c0-a008-6cab0b844725
      Error Number:547,State:0,Class:16
         --- End of inner exception stack trace ---
         at Microsoft.EntityFrameworkCore.Update.AffectedCountModificationCommandBatch.ConsumeResultSetAsync(Int32 startCommandIndex, RelationalDataReader reader, CancellationToken cancellationToken)
         at Microsoft.EntityFrameworkCore.Update.AffectedCountModificationCommandBatch.ConsumeAsync(RelationalDataReader reader, CancellationToken cancellationToken)
         at Microsoft.EntityFrameworkCore.Update.ReaderModificationCommandBatch.ExecuteAsync(IRelationalConnection connection, CancellationToken cancellationToken)
         at Microsoft.EntityFrameworkCore.Update.ReaderModificationCommandBatch.ExecuteAsync(IRelationalConnection connection, CancellationToken cancellationToken)
         at Microsoft.EntityFrameworkCore.SqlServer.Update.Internal.SqlServerModificationCommandBatch.ExecuteAsync(IRelationalConnection connection, CancellationToken cancellationToken)
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
         at WebApplication.BusinessLogic.Services.NotificationService.QueueAsync(String channel, String notifType, String recipient, String subject, String body, Int32 userId, Nullable`1 orderId, Nullable`1 ticketId, CancellationToken cancellationToken) in C:\Users\user\Documents\ANDREI_FILES\PDM_FILES\2ND YEAR\2ND SEM\OOP\OOP-TaurusBikeShop\WebApplication\BusinessLogic\Services\NotificationService.cs:line 80
         at WebApplication.BackgroundJobs.PendingOrderMonitorJob.RunCycleAsync(CancellationToken cancellationToken) in C:\Users\user\Documents\ANDREI_FILES\PDM_FILES\2ND YEAR\2ND SEM\OOP\OOP-TaurusBikeShop\WebApplication\BackgroundJobs\PendingOrderMonitorJob.cs:line 162
info: WebApplication.BackgroundJobs.PendingOrderMonitorJob[0]
      PendingOrderMonitorJob: cycle complete - cancelled 1, reminded 0.
info: Microsoft.EntityFrameworkCore.Database.Command[20101]
      Executed DbCommand (70ms) [Parameters=[@p0='?' (Size = 4000), @p1='?' (Size = 20), @p2='?' (DbType = DateTime2), @p3='?' (Size = 500), @p4='?' (DbType = Boolean), @p5='?' (Size = 100), @p6='?' (DbType = Int32), @p7='?' (DbType = DateTime2), @p8='?' (Size = 255), @p9='?' (DbType = Int32), @p10='?' (DbType = DateTime2), @p11='?' (Size = 20), @p12='?' (Size = 255), @p13='?' (DbType = Int32), @p14='?' (DbType = Int32), @p15='?' (DbType = DateTime2), @p16='?' (Size = 1000), @p17='?' (Size = 100), @p18='?' (DbType = Int32)], CommandType='Text', CommandTimeout='30']
      SET NOCOUNT ON;
      INSERT INTO [Notification] ([Body], [Channel], [CreatedAt], [FailureReason], [IsRead], [NotifType], [OrderId], [ReadAt], [Recipient], [RetryCount], [SentAt], [Status], [Subject], [TicketId], [UserId])
      OUTPUT INSERTED.[NotificationId]
      VALUES (@p0, @p1, @p2, @p3, @p4, @p5, @p6, @p7, @p8, @p9, @p10, @p11, @p12, @p13, @p14);
      INSERT INTO [SystemLog] ([CreatedAt], [EventDescription], [EventType], [UserId])
      OUTPUT INSERTED.[SystemLogId]
      VALUES (@p15, @p16, @p17, @p18);
fail: Microsoft.EntityFrameworkCore.Update[10000]
      An exception occurred in the database while saving changes for context type 'WebApplication.DataAccess.Context.AppDbContext'.
      Microsoft.EntityFrameworkCore.DbUpdateException: An error occurred while saving the entity changes. See the inner exception for details.
       ---> Microsoft.Data.SqlClient.SqlException (0x80131904): The INSERT statement conflicted with the CHECK constraint "CK_Notif_Type". The conflict occurred in database "Taurus-bike-shop-sqlserver-2026", table "dbo.Notification", column 'NotifType'.
         at Microsoft.Data.SqlClient.SqlConnection.OnError(SqlException exception, Boolean breakConnection, Action`1 wrapCloseInAction)
         at Microsoft.Data.SqlClient.SqlInternalConnection.OnError(SqlException exception, Boolean breakConnection, Action`1 wrapCloseInAction)
         at Microsoft.Data.SqlClient.TdsParser.ThrowExceptionAndWarning(TdsParserStateObject stateObj, Boolean callerHasConnectionLock, Boolean asyncClose)
         at Microsoft.Data.SqlClient.TdsParser.TryRun(RunBehavior runBehavior, SqlCommand cmdHandler, SqlDataReader dataStream, BulkCopySimpleResultSet bulkCopyHandler, TdsParserStateObject stateObj, Boolean& dataReady)
         at Microsoft.Data.SqlClient.SqlDataReader.TryHasMoreRows(Boolean& moreRows)
         at Microsoft.Data.SqlClient.SqlDataReader.TryReadInternal(Boolean setTimeout, Boolean& more)
         at Microsoft.Data.SqlClient.SqlDataReader.ReadAsyncExecute(Task task, Object state)
         at Microsoft.Data.SqlClient.SqlDataReader.InvokeAsyncCall[T](SqlDataReaderBaseAsyncCallContext`1 context)
      --- End of stack trace from previous location ---
         at Microsoft.EntityFrameworkCore.Update.AffectedCountModificationCommandBatch.ConsumeResultSetAsync(Int32 startCommandIndex, RelationalDataReader reader, CancellationToken cancellationToken)
      ClientConnectionId:85508ea5-4556-41c0-a008-6cab0b844725
      Error Number:547,State:0,Class:16
         --- End of inner exception stack trace ---
         at Microsoft.EntityFrameworkCore.Update.AffectedCountModificationCommandBatch.ConsumeResultSetAsync(Int32 startCommandIndex, RelationalDataReader reader, CancellationToken cancellationToken)
         at Microsoft.EntityFrameworkCore.Update.AffectedCountModificationCommandBatch.ConsumeAsync(RelationalDataReader reader, CancellationToken cancellationToken)
         at Microsoft.EntityFrameworkCore.Update.ReaderModificationCommandBatch.ExecuteAsync(IRelationalConnection connection, CancellationToken cancellationToken)
         at Microsoft.EntityFrameworkCore.Update.ReaderModificationCommandBatch.ExecuteAsync(IRelationalConnection connection, CancellationToken cancellationToken)
         at Microsoft.EntityFrameworkCore.SqlServer.Update.Internal.SqlServerModificationCommandBatch.ExecuteAsync(IRelationalConnection connection, CancellationToken cancellationToken)
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
      Microsoft.EntityFrameworkCore.DbUpdateException: An error occurred while saving the entity changes. See the inner exception for details.
       ---> Microsoft.Data.SqlClient.SqlException (0x80131904): The INSERT statement conflicted with the CHECK constraint "CK_Notif_Type". The conflict occurred in database "Taurus-bike-shop-sqlserver-2026", table "dbo.Notification", column 'NotifType'.
         at Microsoft.Data.SqlClient.SqlConnection.OnError(SqlException exception, Boolean breakConnection, Action`1 wrapCloseInAction)
         at Microsoft.Data.SqlClient.SqlInternalConnection.OnError(SqlException exception, Boolean breakConnection, Action`1 wrapCloseInAction)
         at Microsoft.Data.SqlClient.TdsParser.ThrowExceptionAndWarning(TdsParserStateObject stateObj, Boolean callerHasConnectionLock, Boolean asyncClose)
         at Microsoft.Data.SqlClient.TdsParser.TryRun(RunBehavior runBehavior, SqlCommand cmdHandler, SqlDataReader dataStream, BulkCopySimpleResultSet bulkCopyHandler, TdsParserStateObject stateObj, Boolean& dataReady)
         at Microsoft.Data.SqlClient.SqlDataReader.TryHasMoreRows(Boolean& moreRows)
         at Microsoft.Data.SqlClient.SqlDataReader.TryReadInternal(Boolean setTimeout, Boolean& more)
         at Microsoft.Data.SqlClient.SqlDataReader.ReadAsyncExecute(Task task, Object state)
         at Microsoft.Data.SqlClient.SqlDataReader.InvokeAsyncCall[T](SqlDataReaderBaseAsyncCallContext`1 context)
      --- End of stack trace from previous location ---
         at Microsoft.EntityFrameworkCore.Update.AffectedCountModificationCommandBatch.ConsumeResultSetAsync(Int32 startCommandIndex, RelationalDataReader reader, CancellationToken cancellationToken)
      ClientConnectionId:85508ea5-4556-41c0-a008-6cab0b844725
      Error Number:547,State:0,Class:16
         --- End of inner exception stack trace ---
         at Microsoft.EntityFrameworkCore.Update.AffectedCountModificationCommandBatch.ConsumeResultSetAsync(Int32 startCommandIndex, RelationalDataReader reader, CancellationToken cancellationToken)
         at Microsoft.EntityFrameworkCore.Update.AffectedCountModificationCommandBatch.ConsumeAsync(RelationalDataReader reader, CancellationToken cancellationToken)
         at Microsoft.EntityFrameworkCore.Update.ReaderModificationCommandBatch.ExecuteAsync(IRelationalConnection connection, CancellationToken cancellationToken)
         at Microsoft.EntityFrameworkCore.Update.ReaderModificationCommandBatch.ExecuteAsync(IRelationalConnection connection, CancellationToken cancellationToken)
         at Microsoft.EntityFrameworkCore.SqlServer.Update.Internal.SqlServerModificationCommandBatch.ExecuteAsync(IRelationalConnection connection, CancellationToken cancellationToken)
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
      PendingOrderMonitorJob cycle failed.
      Microsoft.EntityFrameworkCore.DbUpdateException: An error occurred while saving the entity changes. See the inner exception for details.
       ---> Microsoft.Data.SqlClient.SqlException (0x80131904): The INSERT statement conflicted with the CHECK constraint "CK_Notif_Type". The conflict occurred in database "Taurus-bike-shop-sqlserver-2026", table "dbo.Notification", column 'NotifType'.
         at Microsoft.Data.SqlClient.SqlConnection.OnError(SqlException exception, Boolean breakConnection, Action`1 wrapCloseInAction)
         at Microsoft.Data.SqlClient.SqlInternalConnection.OnError(SqlException exception, Boolean breakConnection, Action`1 wrapCloseInAction)
         at Microsoft.Data.SqlClient.TdsParser.ThrowExceptionAndWarning(TdsParserStateObject stateObj, Boolean callerHasConnectionLock, Boolean asyncClose)
         at Microsoft.Data.SqlClient.TdsParser.TryRun(RunBehavior runBehavior, SqlCommand cmdHandler, SqlDataReader dataStream, BulkCopySimpleResultSet bulkCopyHandler, TdsParserStateObject stateObj, Boolean& dataReady)
         at Microsoft.Data.SqlClient.SqlDataReader.TryHasMoreRows(Boolean& moreRows)
         at Microsoft.Data.SqlClient.SqlDataReader.TryReadInternal(Boolean setTimeout, Boolean& more)
         at Microsoft.Data.SqlClient.SqlDataReader.ReadAsyncExecute(Task task, Object state)
         at Microsoft.Data.SqlClient.SqlDataReader.InvokeAsyncCall[T](SqlDataReaderBaseAsyncCallContext`1 context)
      --- End of stack trace from previous location ---
         at Microsoft.EntityFrameworkCore.Update.AffectedCountModificationCommandBatch.ConsumeResultSetAsync(Int32 startCommandIndex, RelationalDataReader reader, CancellationToken cancellationToken)
      ClientConnectionId:85508ea5-4556-41c0-a008-6cab0b844725
      Error Number:547,State:0,Class:16
         --- End of inner exception stack trace ---
         at Microsoft.EntityFrameworkCore.Update.AffectedCountModificationCommandBatch.ConsumeResultSetAsync(Int32 startCommandIndex, RelationalDataReader reader, CancellationToken cancellationToken)
         at Microsoft.EntityFrameworkCore.Update.AffectedCountModificationCommandBatch.ConsumeAsync(RelationalDataReader reader, CancellationToken cancellationToken)
         at Microsoft.EntityFrameworkCore.Update.ReaderModificationCommandBatch.ExecuteAsync(IRelationalConnection connection, CancellationToken cancellationToken)
         at Microsoft.EntityFrameworkCore.Update.ReaderModificationCommandBatch.ExecuteAsync(IRelationalConnection connection, CancellationToken cancellationToken)
         at Microsoft.EntityFrameworkCore.SqlServer.Update.Internal.SqlServerModificationCommandBatch.ExecuteAsync(IRelationalConnection connection, CancellationToken cancellationToken)
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
         at WebApplication.BackgroundJobs.PendingOrderMonitorJob.RunCycleAsync(CancellationToken cancellationToken) in C:\Users\user\Documents\ANDREI_FILES\PDM_FILES\2ND YEAR\2ND SEM\OOP\OOP-TaurusBikeShop\WebApplication\BackgroundJobs\PendingOrderMonitorJob.cs:line 231
info: Microsoft.AspNetCore.Hosting.Diagnostics[1]
      Request starting HTTP/2 GET https://localhost:7177/Product/List - - -
info: Microsoft.AspNetCore.Routing.EndpointMiddleware[0]
      Executing endpoint 'WebApplication.Controllers.ProductController.List (WebApplication)'
info: Microsoft.AspNetCore.Mvc.Infrastructure.ControllerActionInvoker[102]
      Route matched with {action = "List", controller = "Product"}. Executing controller action with signature System.Threading.Tasks.Task`1[Microsoft.AspNetCore.Mvc.IActionResult] List(System.Nullable`1[System.Int32], System.Nullable`1[System.Int32], System.Nullable`1[System.Decimal], System.Nullable`1[System.Decimal], System.String, System.String, Boolean, Int32, System.Threading.CancellationToken) on controller WebApplication.Controllers.ProductController (WebApplication).
info: Microsoft.EntityFrameworkCore.Database.Command[20101]
      Executed DbCommand (63ms) [Parameters=[@__userId_0='?' (DbType = Int32)], CommandType='Text', CommandTimeout='30']
      SELECT [w].[ProductId]
      FROM [Wishlist] AS [w]
      WHERE [w].[UserId] = @__userId_0
info: Microsoft.EntityFrameworkCore.Database.Command[20101]
      Executed DbCommand (73ms) [Parameters=[@__p_0='?' (DbType = Int32), @__p_1='?' (DbType = Int32)], CommandType='Text', CommandTimeout='30']
      SELECT [t].[ProductId], [t].[AdditionalSpecs], [t].[AxleStandard], [t].[BoostCompatible], [t].[BrakeType], [t].[BrandId], [t].[CategoryId], [t].[Color], [t].[CreatedAt], [t].[Currency], [t].[Description], [t].[IsActive], [t].[IsFeatured], [t].[Material], [t].[Name], [t].[Price], [t].[SKU], [t].[ShortDescription], [t].[SpeedCompatibility], [t].[SuspensionTravel], [t].[TubelessReady], [t].[UpdatedAt], [t].[WheelSize], [b].[BrandId], [b].[BrandName], [b].[Country], [b].[CreatedAt], [b].[Description], [b].[IsActive], [b].[Website], [t].[CategoryId0], [t].[CategoryCode], [t].[Description0], [t].[DisplayOrder], [t].[IsActive0], [t].[Name0], [t].[ParentCategoryId]
      FROM (
          SELECT [p].[ProductId], [p].[AdditionalSpecs], [p].[AxleStandard], [p].[BoostCompatible], [p].[BrakeType], [p].[BrandId], [p].[CategoryId], [p].[Color], [p].[CreatedAt], [p].[Currency], [p].[Description], [p].[IsActive], [p].[IsFeatured], [p].[Material], [p].[Name], [p].[Price], [p].[SKU], [p].[ShortDescription], [p].[SpeedCompatibility], [p].[SuspensionTravel], [p].[TubelessReady], [p].[UpdatedAt], [p].[WheelSize], [c].[CategoryId] AS [CategoryId0], [c].[CategoryCode], [c].[Description] AS [Description0], [c].[DisplayOrder], [c].[IsActive] AS [IsActive0], [c].[Name] AS [Name0], [c].[ParentCategoryId]
          FROM [Product] AS [p]
          INNER JOIN [Category] AS [c] ON [p].[CategoryId] = [c].[CategoryId]
          WHERE [p].[IsActive] = CAST(1 AS bit) AND [c].[IsActive] = CAST(1 AS bit)
          ORDER BY [p].[IsFeatured] DESC, [p].[Name]
          OFFSET @__p_0 ROWS FETCH NEXT @__p_1 ROWS ONLY
      ) AS [t]
      LEFT JOIN [Brand] AS [b] ON [t].[BrandId] = [b].[BrandId]
      ORDER BY [t].[IsFeatured] DESC, [t].[Name], [t].[ProductId], [t].[CategoryId0], [b].[BrandId]
info: Microsoft.EntityFrameworkCore.Database.Command[20101]
      Executed DbCommand (76ms) [Parameters=[@__p_0='?' (DbType = Int32), @__p_1='?' (DbType = Int32)], CommandType='Text', CommandTimeout='30']
      SELECT [t0].[ProductImageId], [t0].[AltText], [t0].[CreatedAt], [t0].[DisplayOrder], [t0].[FileSizeBytes], [t0].[Height], [t0].[ImageType], [t0].[ImageUrl], [t0].[IsPrimary], [t0].[MimeType], [t0].[ProductId], [t0].[StorageBucket], [t0].[StoragePath], [t0].[UploadedByUserId], [t0].[Width], [t].[ProductId], [t].[CategoryId0], [b].[BrandId]
      FROM (
          SELECT [p].[ProductId], [p].[BrandId], [p].[IsFeatured], [p].[Name], [c].[CategoryId] AS [CategoryId0]
          FROM [Product] AS [p]
          INNER JOIN [Category] AS [c] ON [p].[CategoryId] = [c].[CategoryId]
          WHERE [p].[IsActive] = CAST(1 AS bit) AND [c].[IsActive] = CAST(1 AS bit)
          ORDER BY [p].[IsFeatured] DESC, [p].[Name]
          OFFSET @__p_0 ROWS FETCH NEXT @__p_1 ROWS ONLY
      ) AS [t]
      LEFT JOIN [Brand] AS [b] ON [t].[BrandId] = [b].[BrandId]
      INNER JOIN (
          SELECT [p0].[ProductImageId], [p0].[AltText], [p0].[CreatedAt], [p0].[DisplayOrder], [p0].[FileSizeBytes], [p0].[Height], [p0].[ImageType], [p0].[ImageUrl], [p0].[IsPrimary], [p0].[MimeType], [p0].[ProductId], [p0].[StorageBucket], [p0].[StoragePath], [p0].[UploadedByUserId], [p0].[Width]
          FROM [ProductImage] AS [p0]
          WHERE [p0].[IsPrimary] = CAST(1 AS bit)
      ) AS [t0] ON [t].[ProductId] = [t0].[ProductId]
      ORDER BY [t].[IsFeatured] DESC, [t].[Name], [t].[ProductId], [t].[CategoryId0], [b].[BrandId]
info: Microsoft.EntityFrameworkCore.Database.Command[20101]
      Executed DbCommand (71ms) [Parameters=[@__p_0='?' (DbType = Int32), @__p_1='?' (DbType = Int32)], CommandType='Text', CommandTimeout='30']
      SELECT [t0].[ProductVariantId], [t0].[AdditionalPrice], [t0].[CreatedAt], [t0].[IsActive], [t0].[ProductId], [t0].[ReorderThreshold], [t0].[SKU], [t0].[StockQuantity], [t0].[UpdatedAt], [t0].[VariantName], [t].[ProductId], [t].[CategoryId0], [b].[BrandId]
      FROM (
          SELECT [p].[ProductId], [p].[BrandId], [p].[IsFeatured], [p].[Name], [c].[CategoryId] AS [CategoryId0]
          FROM [Product] AS [p]
          INNER JOIN [Category] AS [c] ON [p].[CategoryId] = [c].[CategoryId]
          WHERE [p].[IsActive] = CAST(1 AS bit) AND [c].[IsActive] = CAST(1 AS bit)
          ORDER BY [p].[IsFeatured] DESC, [p].[Name]
          OFFSET @__p_0 ROWS FETCH NEXT @__p_1 ROWS ONLY
      ) AS [t]
      LEFT JOIN [Brand] AS [b] ON [t].[BrandId] = [b].[BrandId]
      INNER JOIN (
          SELECT [p0].[ProductVariantId], [p0].[AdditionalPrice], [p0].[CreatedAt], [p0].[IsActive], [p0].[ProductId], [p0].[ReorderThreshold], [p0].[SKU], [p0].[StockQuantity], [p0].[UpdatedAt], [p0].[VariantName]
          FROM [ProductVariant] AS [p0]
          WHERE [p0].[IsActive] = CAST(1 AS bit)
      ) AS [t0] ON [t].[ProductId] = [t0].[ProductId]
      ORDER BY [t].[IsFeatured] DESC, [t].[Name], [t].[ProductId], [t].[CategoryId0], [b].[BrandId]
info: Microsoft.EntityFrameworkCore.Database.Command[20101]
      Executed DbCommand (69ms) [Parameters=[], CommandType='Text', CommandTimeout='30']
      SELECT COUNT(*)
      FROM [Product] AS [p]
      INNER JOIN [Category] AS [c] ON [p].[CategoryId] = [c].[CategoryId]
      WHERE [p].[IsActive] = CAST(1 AS bit) AND [c].[IsActive] = CAST(1 AS bit)
info: Microsoft.EntityFrameworkCore.Database.Command[20101]
      Executed DbCommand (67ms) [Parameters=[], CommandType='Text', CommandTimeout='30']
      SELECT [b].[BrandId], [b].[BrandName], [b].[Country], [b].[CreatedAt], [b].[Description], [b].[IsActive], [b].[Website]
      FROM [Brand] AS [b]
      WHERE [b].[IsActive] = CAST(1 AS bit)
      ORDER BY [b].[BrandName]
info: Microsoft.EntityFrameworkCore.Database.Command[20101]
      Executed DbCommand (65ms) [Parameters=[], CommandType='Text', CommandTimeout='30']
      SELECT [c].[CategoryId], [c].[CategoryCode], [c].[Description], [c].[DisplayOrder], [c].[IsActive], [c].[Name], [c].[ParentCategoryId]
      FROM [Category] AS [c]
      WHERE [c].[IsActive] = CAST(1 AS bit)
      ORDER BY [c].[DisplayOrder], [c].[Name]
info: Microsoft.AspNetCore.Mvc.ViewFeatures.ViewResultExecutor[1]
      Executing ViewResult, running view ~/Views/Customer/ProductCatalog.cshtml.
info: Microsoft.AspNetCore.Mvc.ViewFeatures.ViewResultExecutor[4]
      Executed ViewResult - view ~/Views/Customer/ProductCatalog.cshtml executed in 41.8005ms.
info: Microsoft.AspNetCore.Mvc.Infrastructure.ControllerActionInvoker[105]
      Executed action WebApplication.Controllers.ProductController.List (WebApplication) in 682.1355ms
info: Microsoft.AspNetCore.Routing.EndpointMiddleware[1]
      Executed endpoint 'WebApplication.Controllers.ProductController.List (WebApplication)'
info: Microsoft.AspNetCore.Hosting.Diagnostics[1]
      Request starting HTTP/2 GET https://localhost:7177/_vs/browserLink - - -
info: Microsoft.AspNetCore.Hosting.Diagnostics[1]
      Request starting HTTP/2 GET https://localhost:7177/_framework/aspnetcore-browser-refresh.js - - -
info: Microsoft.AspNetCore.Hosting.Diagnostics[1]
      Request starting HTTP/2 GET https://localhost:7177/js/product-catalog.js?v=slQbJ-C3Jq2jWQeZYTTuKLAEdxf5QUZr-BFt6KC2UlM - - -
info: Microsoft.AspNetCore.Hosting.Diagnostics[2]
      Request finished HTTP/2 GET https://localhost:7177/Product/List - 200 - text/html;+charset=utf-8 697.3174ms
info: Microsoft.AspNetCore.StaticFiles.StaticFileMiddleware[6]
      The file /js/product-catalog.js was not modified
info: Microsoft.AspNetCore.Hosting.Diagnostics[2]
      Request finished HTTP/2 GET https://localhost:7177/js/product-catalog.js?v=slQbJ-C3Jq2jWQeZYTTuKLAEdxf5QUZr-BFt6KC2UlM - 304 - text/javascript 5.9073ms
info: Microsoft.AspNetCore.Hosting.Diagnostics[2]
      Request finished HTTP/2 GET https://localhost:7177/_framework/aspnetcore-browser-refresh.js - 200 14933 application/javascript;+charset=utf-8 10.2342ms
info: Microsoft.AspNetCore.Hosting.Diagnostics[2]
      Request finished HTTP/2 GET https://localhost:7177/_vs/browserLink - 200 - text/javascript;+charset=UTF-8 20.0083ms
info: Microsoft.EntityFrameworkCore.Database.Command[20101]
      Executed DbCommand (64ms) [Parameters=[@p0='?' (DbType = DateTime2), @p1='?' (Size = 1000), @p2='?' (Size = 100), @p3='?' (DbType = Int32)], CommandType='Text', CommandTimeout='30']
      SET IMPLICIT_TRANSACTIONS OFF;
      SET NOCOUNT ON;
      INSERT INTO [SystemLog] ([CreatedAt], [EventDescription], [EventType], [UserId])
      OUTPUT INSERTED.[SystemLogId]
      VALUES (@p0, @p1, @p2, @p3);
info: Microsoft.AspNetCore.Hosting.Diagnostics[1]
      Request starting HTTP/2 GET https://localhost:7177/Cart/GetCartCount - - -
info: Microsoft.AspNetCore.Hosting.Diagnostics[1]
      Request starting HTTP/2 GET https://localhost:7177/Customer/NotificationCount - - -
info: Microsoft.AspNetCore.Routing.EndpointMiddleware[0]
      Executing endpoint 'WebApplication.Controllers.CartController.GetCartCount (WebApplication)'
info: Microsoft.AspNetCore.Mvc.Infrastructure.ControllerActionInvoker[102]
      Route matched with {action = "GetCartCount", controller = "Cart"}. Executing controller action with signature System.Threading.Tasks.Task`1[Microsoft.AspNetCore.Mvc.IActionResult] GetCartCount(System.Threading.CancellationToken) on controller WebApplication.Controllers.CartController (WebApplication).
info: Microsoft.AspNetCore.Routing.EndpointMiddleware[0]
      Executing endpoint 'WebApplication.Controllers.CustomerController.NotificationCount (WebApplication)'
info: Microsoft.AspNetCore.Mvc.Infrastructure.ControllerActionInvoker[102]
      Route matched with {action = "NotificationCount", controller = "Customer"}. Executing controller action with signature System.Threading.Tasks.Task`1[Microsoft.AspNetCore.Mvc.IActionResult] NotificationCount(System.Threading.CancellationToken) on controller WebApplication.Controllers.CustomerController (WebApplication).
info: Microsoft.EntityFrameworkCore.Database.Command[20101]
      Executed DbCommand (62ms) [Parameters=[@__userId_Value_0='?' (DbType = Int32)], CommandType='Text', CommandTimeout='30']
      SELECT TOP(1) [c].[CartId], [c].[CreatedAt], [c].[GuestSessionId], [c].[IsCheckedOut], [c].[LastUpdatedAt], [c].[UserId]
      FROM [Cart] AS [c]
      WHERE [c].[IsCheckedOut] = CAST(0 AS bit) AND [c].[UserId] = @__userId_Value_0
      ORDER BY [c].[CartId]
info: Microsoft.AspNetCore.Mvc.Infrastructure.SystemTextJsonResultExecutor[1]
      Executing JsonResult, writing value of type 'WebApplication.Models.ApiResponse'.
info: Microsoft.AspNetCore.Mvc.Infrastructure.ControllerActionInvoker[105]
      Executed action WebApplication.Controllers.CartController.GetCartCount (WebApplication) in 67.0003ms
info: Microsoft.AspNetCore.Routing.EndpointMiddleware[1]
      Executed endpoint 'WebApplication.Controllers.CartController.GetCartCount (WebApplication)'
info: Microsoft.AspNetCore.Hosting.Diagnostics[2]
      Request finished HTTP/2 GET https://localhost:7177/Cart/GetCartCount - 200 - application/json;+charset=utf-8 77.1944ms
info: Microsoft.EntityFrameworkCore.Database.Command[20101]
      Executed DbCommand (79ms) [Parameters=[], CommandType='Text', CommandTimeout='30']
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
      Executed DbCommand (60ms) [Parameters=[@p0='?' (DbType = DateTime2), @p1='?' (Size = 1000), @p2='?' (Size = 100), @p3='?' (DbType = Int32)], CommandType='Text', CommandTimeout='30']
      SET IMPLICIT_TRANSACTIONS OFF;
      SET NOCOUNT ON;
      INSERT INTO [SystemLog] ([CreatedAt], [EventDescription], [EventType], [UserId])
      OUTPUT INSERTED.[SystemLogId]
      VALUES (@p0, @p1, @p2, @p3);
info: Microsoft.EntityFrameworkCore.Database.Command[20101]
      Executed DbCommand (56ms) [Parameters=[@__userId_0='?' (DbType = Int32)], CommandType='Text', CommandTimeout='30']
      SELECT COUNT(*)
      FROM [Notification] AS [n]
      WHERE [n].[UserId] = @__userId_0 AND [n].[IsRead] = CAST(0 AS bit) AND [n].[Channel] = N'InApp'
info: Microsoft.AspNetCore.Mvc.Infrastructure.SystemTextJsonResultExecutor[1]
      Executing JsonResult, writing value of type '<>f__AnonymousType5`1[[System.Int32, System.Private.CoreLib, Version=8.0.0.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e]]'.
info: Microsoft.AspNetCore.Mvc.Infrastructure.ControllerActionInvoker[105]
      Executed action WebApplication.Controllers.CustomerController.NotificationCount (WebApplication) in 400.0423ms
info: Microsoft.AspNetCore.Routing.EndpointMiddleware[1]
      Executed endpoint 'WebApplication.Controllers.CustomerController.NotificationCount (WebApplication)'
info: Microsoft.AspNetCore.Hosting.Diagnostics[2]
      Request finished HTTP/2 GET https://localhost:7177/Customer/NotificationCount - 200 - application/json;+charset=utf-8 415.2230ms
info: Microsoft.EntityFrameworkCore.Database.Command[20101]
      Executed DbCommand (64ms) [Parameters=[@__p_0='?' (DbType = Int32)], CommandType='Text', CommandTimeout='30']
      SELECT TOP(@__p_0) [n].[NotificationId], [n].[Body], [n].[Channel], [n].[CreatedAt], [n].[FailureReason], [n].[IsRead], [n].[NotifType], [n].[OrderId], [n].[ReadAt], [n].[Recipient], [n].[RetryCount], [n].[SentAt], [n].[Status], [n].[Subject], [n].[TicketId], [n].[UserId]
      FROM [Notification] AS [n]
      WHERE [n].[Status] = N'Pending' AND [n].[RetryCount] < 3 AND [n].[Channel] = N'Email'
      ORDER BY [n].[CreatedAt]
info: Microsoft.AspNetCore.Hosting.Diagnostics[1]
      Request starting HTTP/2 GET https://localhost:7177/Product/Detail/146 - - -
info: Microsoft.AspNetCore.Routing.EndpointMiddleware[0]
      Executing endpoint 'WebApplication.Controllers.ProductController.Detail (WebApplication)'
info: Microsoft.AspNetCore.Mvc.Infrastructure.ControllerActionInvoker[102]
      Route matched with {action = "Detail", controller = "Product"}. Executing controller action with signature System.Threading.Tasks.Task`1[Microsoft.AspNetCore.Mvc.IActionResult] Detail(Int32, System.Threading.CancellationToken) on controller WebApplication.Controllers.ProductController (WebApplication).
info: Microsoft.EntityFrameworkCore.Database.Command[20101]
      Executed DbCommand (62ms) [Parameters=[@__productId_0='?' (DbType = Int32)], CommandType='Text', CommandTimeout='30']
      SELECT TOP(1) [p].[ProductId], [p].[AdditionalSpecs], [p].[AxleStandard], [p].[BoostCompatible], [p].[BrakeType], [p].[BrandId], [p].[CategoryId], [p].[Color], [p].[CreatedAt], [p].[Currency], [p].[Description], [p].[IsActive], [p].[IsFeatured], [p].[Material], [p].[Name], [p].[Price], [p].[SKU], [p].[ShortDescription], [p].[SpeedCompatibility], [p].[SuspensionTravel], [p].[TubelessReady], [p].[UpdatedAt], [p].[WheelSize], [b].[BrandId], [b].[BrandName], [b].[Country], [b].[CreatedAt], [b].[Description], [b].[IsActive], [b].[Website], [c].[CategoryId], [c].[CategoryCode], [c].[Description], [c].[DisplayOrder], [c].[IsActive], [c].[Name], [c].[ParentCategoryId]
      FROM [Product] AS [p]
      LEFT JOIN [Brand] AS [b] ON [p].[BrandId] = [b].[BrandId]
      INNER JOIN [Category] AS [c] ON [p].[CategoryId] = [c].[CategoryId]
      WHERE [p].[ProductId] = @__productId_0 AND [p].[IsActive] = CAST(1 AS bit)
      ORDER BY [p].[ProductId], [b].[BrandId], [c].[CategoryId]
info: Microsoft.EntityFrameworkCore.Database.Command[20101]
      Executed DbCommand (63ms) [Parameters=[@__productId_0='?' (DbType = Int32)], CommandType='Text', CommandTimeout='30']
      SELECT [t0].[ProductVariantId], [t0].[AdditionalPrice], [t0].[CreatedAt], [t0].[IsActive], [t0].[ProductId], [t0].[ReorderThreshold], [t0].[SKU], [t0].[StockQuantity], [t0].[UpdatedAt], [t0].[VariantName], [t].[ProductId], [t].[BrandId], [t].[CategoryId]
      FROM (
          SELECT TOP(1) [p].[ProductId], [b].[BrandId], [c].[CategoryId]
          FROM [Product] AS [p]
          LEFT JOIN [Brand] AS [b] ON [p].[BrandId] = [b].[BrandId]
          INNER JOIN [Category] AS [c] ON [p].[CategoryId] = [c].[CategoryId]
          WHERE [p].[ProductId] = @__productId_0 AND [p].[IsActive] = CAST(1 AS bit)
      ) AS [t]
      INNER JOIN (
          SELECT [p0].[ProductVariantId], [p0].[AdditionalPrice], [p0].[CreatedAt], [p0].[IsActive], [p0].[ProductId], [p0].[ReorderThreshold], [p0].[SKU], [p0].[StockQuantity], [p0].[UpdatedAt], [p0].[VariantName]
          FROM [ProductVariant] AS [p0]
          WHERE [p0].[IsActive] = CAST(1 AS bit)
      ) AS [t0] ON [t].[ProductId] = [t0].[ProductId]
      ORDER BY [t].[ProductId], [t].[BrandId], [t].[CategoryId]
info: Microsoft.EntityFrameworkCore.Database.Command[20101]
      Executed DbCommand (66ms) [Parameters=[@__productId_0='?' (DbType = Int32)], CommandType='Text', CommandTimeout='30']
      SELECT [p0].[ProductImageId], [p0].[AltText], [p0].[CreatedAt], [p0].[DisplayOrder], [p0].[FileSizeBytes], [p0].[Height], [p0].[ImageType], [p0].[ImageUrl], [p0].[IsPrimary], [p0].[MimeType], [p0].[ProductId], [p0].[StorageBucket], [p0].[StoragePath], [p0].[UploadedByUserId], [p0].[Width], [t].[ProductId], [t].[BrandId], [t].[CategoryId]
      FROM (
          SELECT TOP(1) [p].[ProductId], [b].[BrandId], [c].[CategoryId]
          FROM [Product] AS [p]
          LEFT JOIN [Brand] AS [b] ON [p].[BrandId] = [b].[BrandId]
          INNER JOIN [Category] AS [c] ON [p].[CategoryId] = [c].[CategoryId]
          WHERE [p].[ProductId] = @__productId_0 AND [p].[IsActive] = CAST(1 AS bit)
      ) AS [t]
      INNER JOIN [ProductImage] AS [p0] ON [t].[ProductId] = [p0].[ProductId]
      ORDER BY [t].[ProductId], [t].[BrandId], [t].[CategoryId], [p0].[IsPrimary] DESC, [p0].[DisplayOrder]
info: Microsoft.EntityFrameworkCore.Database.Command[20101]
      Executed DbCommand (68ms) [Parameters=[@__productId_0='?' (DbType = Int32)], CommandType='Text', CommandTimeout='30']
      SELECT [t0].[PriceHistoryId], [t0].[ChangedAt], [t0].[ChangedByUserId], [t0].[NewPrice], [t0].[Notes], [t0].[OldPrice], [t0].[ProductId], [t].[ProductId], [t].[BrandId], [t].[CategoryId]
      FROM (
          SELECT TOP(1) [p].[ProductId], [b].[BrandId], [c].[CategoryId]
          FROM [Product] AS [p]
          LEFT JOIN [Brand] AS [b] ON [p].[BrandId] = [b].[BrandId]
          INNER JOIN [Category] AS [c] ON [p].[CategoryId] = [c].[CategoryId]
          WHERE [p].[ProductId] = @__productId_0 AND [p].[IsActive] = CAST(1 AS bit)
      ) AS [t]
      INNER JOIN (
          SELECT [t1].[PriceHistoryId], [t1].[ChangedAt], [t1].[ChangedByUserId], [t1].[NewPrice], [t1].[Notes], [t1].[OldPrice], [t1].[ProductId]
          FROM (
              SELECT [p0].[PriceHistoryId], [p0].[ChangedAt], [p0].[ChangedByUserId], [p0].[NewPrice], [p0].[Notes], [p0].[OldPrice], [p0].[ProductId], ROW_NUMBER() OVER(PARTITION BY [p0].[ProductId] ORDER BY [p0].[ChangedAt] DESC) AS [row]
              FROM [PriceHistory] AS [p0]
          ) AS [t1]
          WHERE [t1].[row] <= 10
      ) AS [t0] ON [t].[ProductId] = [t0].[ProductId]
      ORDER BY [t].[ProductId], [t].[BrandId], [t].[CategoryId], [t0].[ProductId], [t0].[ChangedAt] DESC
info: Microsoft.EntityFrameworkCore.Database.Command[20101]
      Executed DbCommand (63ms) [Parameters=[@__productId_0='?' (DbType = Int32), @__p_1='?' (DbType = Int32), @__p_2='?' (DbType = Int32)], CommandType='Text', CommandTimeout='30']
      SELECT [t].[ReviewId], [t].[Comment], [t].[CreatedAt], [t].[IsVerifiedPurchase], [t].[OrderId], [t].[ProductId], [t].[Rating], [t].[UserId], [t0].[UserId], [t0].[CreatedAt], [t0].[DefaultAddressId], [t0].[Email], [t0].[FailedLoginAttempts], [t0].[FirstName], [t0].[IsActive], [t0].[IsDeleted], [t0].[IsWalkIn], [t0].[LastLoginAt], [t0].[LastName], [t0].[LockoutUntil], [t0].[PasswordHash], [t0].[PhoneNumber]
      FROM (
          SELECT [r].[ReviewId], [r].[Comment], [r].[CreatedAt], [r].[IsVerifiedPurchase], [r].[OrderId], [r].[ProductId], [r].[Rating], [r].[UserId]
          FROM [Review] AS [r]
          WHERE [r].[ProductId] = @__productId_0
          ORDER BY [r].[CreatedAt] DESC
          OFFSET @__p_1 ROWS FETCH NEXT @__p_2 ROWS ONLY
      ) AS [t]
      INNER JOIN (
          SELECT [u].[UserId], [u].[CreatedAt], [u].[DefaultAddressId], [u].[Email], [u].[FailedLoginAttempts], [u].[FirstName], [u].[IsActive], [u].[IsDeleted], [u].[IsWalkIn], [u].[LastLoginAt], [u].[LastName], [u].[LockoutUntil], [u].[PasswordHash], [u].[PhoneNumber]
          FROM [User] AS [u]
          WHERE [u].[IsDeleted] = CAST(0 AS bit)
      ) AS [t0] ON [t].[UserId] = [t0].[UserId]
      ORDER BY [t].[CreatedAt] DESC
info: Microsoft.EntityFrameworkCore.Database.Command[20101]
      Executed DbCommand (60ms) [Parameters=[@__productId_0='?' (DbType = Int32)], CommandType='Text', CommandTimeout='30']
      SELECT CASE
          WHEN EXISTS (
              SELECT 1
              FROM [Review] AS [r]
              WHERE [r].[ProductId] = @__productId_0) THEN CAST(1 AS bit)
          ELSE CAST(0 AS bit)
      END
info: Microsoft.EntityFrameworkCore.Database.Command[20101]
      Executed DbCommand (57ms) [Parameters=[@__productId_0='?' (DbType = Int32)], CommandType='Text', CommandTimeout='30']
      SELECT COUNT(*)
      FROM [Review] AS [r]
      WHERE [r].[ProductId] = @__productId_0
info: Microsoft.EntityFrameworkCore.Database.Command[20101]
      Executed DbCommand (56ms) [Parameters=[@__userId_0='?' (DbType = Int32)], CommandType='Text', CommandTimeout='30']
      SELECT [w].[ProductId]
      FROM [Wishlist] AS [w]
      WHERE [w].[UserId] = @__userId_0
info: Microsoft.EntityFrameworkCore.Database.Command[20101]
      Executed DbCommand (75ms) [Parameters=[@__productId_0='?' (DbType = Int32), @__userId_1='?' (DbType = Int32)], CommandType='Text', CommandTimeout='30']
      SELECT CASE
          WHEN EXISTS (
              SELECT 1
              FROM [OrderItem] AS [o]
              INNER JOIN (
                  SELECT [o0].[OrderId], [o0].[CartId], [o0].[ContactPhone], [o0].[CreatedAt], [o0].[DeliveryInstructions], [o0].[DiscountAmount], [o0].[FulfillmentType], [o0].[GuestSessionId], [o0].[IsDeleted], [o0].[IsWalkIn], [o0].[OrderDate], [o0].[OrderNumber], [o0].[OrderStatus], [o0].[POSSessionId], [o0].[PaymentMethod], [o0].[ShippingAddressId], [o0].[ShippingFee], [o0].[SubTotal], [o0].[TotalAmount], [o0].[UpdatedAt], [o0].[UserId]
                  FROM [Order] AS [o0]
                  WHERE [o0].[IsDeleted] = CAST(0 AS bit)
              ) AS [t] ON [o].[OrderId] = [t].[OrderId]
              WHERE [o].[ProductId] = @__productId_0 AND [t].[UserId] = @__userId_1 AND [t].[OrderStatus] = N'Delivered') THEN CAST(1 AS bit)
          ELSE CAST(0 AS bit)
      END
info: Microsoft.AspNetCore.Mvc.ViewFeatures.ViewResultExecutor[1]
      Executing ViewResult, running view ~/Views/Customer/ProductDetails.cshtml.
info: Microsoft.AspNetCore.Mvc.ViewFeatures.ViewResultExecutor[4]
      Executed ViewResult - view ~/Views/Customer/ProductDetails.cshtml executed in 20.3172ms.
info: Microsoft.AspNetCore.Mvc.Infrastructure.ControllerActionInvoker[105]
      Executed action WebApplication.Controllers.ProductController.Detail (WebApplication) in 660.7707ms
info: Microsoft.AspNetCore.Routing.EndpointMiddleware[1]
      Executed endpoint 'WebApplication.Controllers.ProductController.Detail (WebApplication)'
info: Microsoft.AspNetCore.Hosting.Diagnostics[2]
      Request finished HTTP/2 GET https://localhost:7177/Product/Detail/146 - 200 - text/html;+charset=utf-8 674.7486ms
info: Microsoft.AspNetCore.Hosting.Diagnostics[1]
      Request starting HTTP/2 GET https://localhost:7177/js/review.js?v=47DEQpj8HBSa-_TImW-5JCeuQeRkm5NMpJWZG3hSuFU - - -
info: Microsoft.AspNetCore.Hosting.Diagnostics[1]
      Request starting HTTP/2 GET https://localhost:7177/_framework/aspnetcore-browser-refresh.js - - -
info: Microsoft.AspNetCore.Hosting.Diagnostics[1]
      Request starting HTTP/2 GET https://localhost:7177/_vs/browserLink - - -
info: Microsoft.AspNetCore.StaticFiles.StaticFileMiddleware[6]
      The file /js/review.js was not modified
info: Microsoft.AspNetCore.Hosting.Diagnostics[2]
      Request finished HTTP/2 GET https://localhost:7177/js/review.js?v=47DEQpj8HBSa-_TImW-5JCeuQeRkm5NMpJWZG3hSuFU - 304 - text/javascript 6.2132ms
info: Microsoft.AspNetCore.Hosting.Diagnostics[2]
      Request finished HTTP/2 GET https://localhost:7177/_framework/aspnetcore-browser-refresh.js - 200 14933 application/javascript;+charset=utf-8 10.0520ms
info: Microsoft.AspNetCore.Hosting.Diagnostics[2]
      Request finished HTTP/2 GET https://localhost:7177/_vs/browserLink - 200 - text/javascript;+charset=UTF-8 22.1576ms
info: Microsoft.AspNetCore.Hosting.Diagnostics[1]
      Request starting HTTP/2 GET https://localhost:7177/Cart/GetCartCount - - -
info: Microsoft.AspNetCore.Hosting.Diagnostics[1]
      Request starting HTTP/2 GET https://localhost:7177/Customer/NotificationCount - - -
info: Microsoft.AspNetCore.Routing.EndpointMiddleware[0]
      Executing endpoint 'WebApplication.Controllers.CustomerController.NotificationCount (WebApplication)'
info: Microsoft.AspNetCore.Routing.EndpointMiddleware[0]
      Executing endpoint 'WebApplication.Controllers.CartController.GetCartCount (WebApplication)'
info: Microsoft.AspNetCore.Mvc.Infrastructure.ControllerActionInvoker[102]
      Route matched with {action = "NotificationCount", controller = "Customer"}. Executing controller action with signature System.Threading.Tasks.Task`1[Microsoft.AspNetCore.Mvc.IActionResult] NotificationCount(System.Threading.CancellationToken) on controller WebApplication.Controllers.CustomerController (WebApplication).
info: Microsoft.AspNetCore.Mvc.Infrastructure.ControllerActionInvoker[102]
      Route matched with {action = "GetCartCount", controller = "Cart"}. Executing controller action with signature System.Threading.Tasks.Task`1[Microsoft.AspNetCore.Mvc.IActionResult] GetCartCount(System.Threading.CancellationToken) on controller WebApplication.Controllers.CartController (WebApplication).
info: Microsoft.EntityFrameworkCore.Database.Command[20101]
      Executed DbCommand (56ms) [Parameters=[@__userId_0='?' (DbType = Int32)], CommandType='Text', CommandTimeout='30']
      SELECT COUNT(*)
      FROM [Notification] AS [n]
      WHERE [n].[UserId] = @__userId_0 AND [n].[IsRead] = CAST(0 AS bit) AND [n].[Channel] = N'InApp'
info: Microsoft.AspNetCore.Mvc.Infrastructure.SystemTextJsonResultExecutor[1]
      Executing JsonResult, writing value of type '<>f__AnonymousType5`1[[System.Int32, System.Private.CoreLib, Version=8.0.0.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e]]'.
info: Microsoft.EntityFrameworkCore.Database.Command[20101]
      Executed DbCommand (58ms) [Parameters=[@__userId_Value_0='?' (DbType = Int32)], CommandType='Text', CommandTimeout='30']
      SELECT TOP(1) [c].[CartId], [c].[CreatedAt], [c].[GuestSessionId], [c].[IsCheckedOut], [c].[LastUpdatedAt], [c].[UserId]
      FROM [Cart] AS [c]
      WHERE [c].[IsCheckedOut] = CAST(0 AS bit) AND [c].[UserId] = @__userId_Value_0
      ORDER BY [c].[CartId]
info: Microsoft.AspNetCore.Mvc.Infrastructure.ControllerActionInvoker[105]
      Executed action WebApplication.Controllers.CustomerController.NotificationCount (WebApplication) in 61.7325ms
info: Microsoft.AspNetCore.Mvc.Infrastructure.SystemTextJsonResultExecutor[1]
      Executing JsonResult, writing value of type 'WebApplication.Models.ApiResponse'.
info: Microsoft.AspNetCore.Routing.EndpointMiddleware[1]
      Executed endpoint 'WebApplication.Controllers.CustomerController.NotificationCount (WebApplication)'
info: Microsoft.AspNetCore.Mvc.Infrastructure.ControllerActionInvoker[105]
      Executed action WebApplication.Controllers.CartController.GetCartCount (WebApplication) in 64.5146ms
info: Microsoft.AspNetCore.Hosting.Diagnostics[2]
      Request finished HTTP/2 GET https://localhost:7177/Customer/NotificationCount - 200 - application/json;+charset=utf-8 76.3580ms
info: Microsoft.AspNetCore.Routing.EndpointMiddleware[1]
      Executed endpoint 'WebApplication.Controllers.CartController.GetCartCount (WebApplication)'
info: Microsoft.AspNetCore.Hosting.Diagnostics[2]
      Request finished HTTP/2 GET https://localhost:7177/Cart/GetCartCount - 200 - application/json;+charset=utf-8 79.7388ms
info: Microsoft.AspNetCore.Hosting.Diagnostics[1]
      Request starting HTTP/2 POST https://localhost:7177/Cart/AddToCart - application/json 41
info: Microsoft.AspNetCore.Cors.Infrastructure.CorsService[4]
      CORS policy execution successful.
info: Microsoft.AspNetCore.Routing.EndpointMiddleware[0]
      Executing endpoint 'WebApplication.Controllers.CartController.AddToCart (WebApplication)'
info: Microsoft.AspNetCore.Mvc.Infrastructure.ControllerActionInvoker[102]
      Route matched with {action = "AddToCart", controller = "Cart"}. Executing controller action with signature System.Threading.Tasks.Task`1[Microsoft.AspNetCore.Mvc.IActionResult] AddToCart(AddToCartRequest, System.Threading.CancellationToken) on controller WebApplication.Controllers.CartController (WebApplication).
info: Microsoft.EntityFrameworkCore.Database.Command[20101]
      Executed DbCommand (60ms) [Parameters=[@__variantId_0='?' (DbType = Int32)], CommandType='Text', CommandTimeout='30']
      SELECT TOP(1) [p].[ProductVariantId], [p].[AdditionalPrice], [p].[CreatedAt], [p].[IsActive], [p].[ProductId], [p].[ReorderThreshold], [p].[SKU], [p].[StockQuantity], [p].[UpdatedAt], [p].[VariantName]
      FROM [ProductVariant] AS [p]
      WHERE [p].[ProductVariantId] = @__variantId_0 AND [p].[IsActive] = CAST(1 AS bit)
info: Microsoft.EntityFrameworkCore.Database.Command[20101]
      Executed DbCommand (58ms) [Parameters=[@__userId_Value_0='?' (DbType = Int32)], CommandType='Text', CommandTimeout='30']
      SELECT TOP(1) [c].[CartId], [c].[CreatedAt], [c].[GuestSessionId], [c].[IsCheckedOut], [c].[LastUpdatedAt], [c].[UserId]
      FROM [Cart] AS [c]
      WHERE [c].[IsCheckedOut] = CAST(0 AS bit) AND [c].[UserId] = @__userId_Value_0
      ORDER BY [c].[CartId]
info: Microsoft.EntityFrameworkCore.Database.Command[20101]
      Executed DbCommand (67ms) [Parameters=[@p0='?' (DbType = DateTime2), @p1='?' (DbType = Int32), @p2='?' (DbType = Boolean), @p3='?' (DbType = DateTime2), @p4='?' (DbType = Int32)], CommandType='Text', CommandTimeout='30']
      SET IMPLICIT_TRANSACTIONS OFF;
      SET NOCOUNT ON;
      INSERT INTO [Cart] ([CreatedAt], [GuestSessionId], [IsCheckedOut], [LastUpdatedAt], [UserId])
      OUTPUT INSERTED.[CartId]
      VALUES (@p0, @p1, @p2, @p3, @p4);
info: Microsoft.EntityFrameworkCore.Database.Command[20101]
      Executed DbCommand (64ms) [Parameters=[@__cart_CartId_0='?' (DbType = Int32), @__productId_1='?' (DbType = Int32), @__resolvedVariantId_2='?' (DbType = Int32)], CommandType='Text', CommandTimeout='30']
      SELECT TOP(1) [c].[CartItemId], [c].[AddedAt], [c].[CartId], [c].[PriceAtAdd], [c].[ProductId], [c].[ProductVariantId], [c].[Quantity]
      FROM [CartItem] AS [c]
      WHERE [c].[CartId] = @__cart_CartId_0 AND [c].[ProductId] = @__productId_1 AND [c].[ProductVariantId] = @__resolvedVariantId_2
info: Microsoft.EntityFrameworkCore.Database.Command[20101]
      Executed DbCommand (63ms) [Parameters=[@__variantId_0='?' (DbType = Int32)], CommandType='Text', CommandTimeout='30']
      SELECT TOP(1) [p].[ProductVariantId], [p].[AdditionalPrice], [p].[CreatedAt], [p].[IsActive], [p].[ProductId], [p].[ReorderThreshold], [p].[SKU], [p].[StockQuantity], [p].[UpdatedAt], [p].[VariantName], [p0].[ProductId], [p0].[AdditionalSpecs], [p0].[AxleStandard], [p0].[BoostCompatible], [p0].[BrakeType], [p0].[BrandId], [p0].[CategoryId], [p0].[Color], [p0].[CreatedAt], [p0].[Currency], [p0].[Description], [p0].[IsActive], [p0].[IsFeatured], [p0].[Material], [p0].[Name], [p0].[Price], [p0].[SKU], [p0].[ShortDescription], [p0].[SpeedCompatibility], [p0].[SuspensionTravel], [p0].[TubelessReady], [p0].[UpdatedAt], [p0].[WheelSize]
      FROM [ProductVariant] AS [p]
      INNER JOIN [Product] AS [p0] ON [p].[ProductId] = [p0].[ProductId]
      WHERE [p].[ProductVariantId] = @__variantId_0 AND [p].[IsActive] = CAST(1 AS bit)
info: Microsoft.EntityFrameworkCore.Database.Command[20101]
      Executed DbCommand (71ms) [Parameters=[@p1='?' (DbType = Int32), @p0='?' (DbType = DateTime2), @p2='?' (DbType = DateTime2), @p3='?' (DbType = Int32), @p4='?' (Precision = 18) (Scale = 2) (DbType = Decimal), @p5='?' (DbType = Int32), @p6='?' (DbType = Int32), @p7='?' (DbType = Int32)], CommandType='Text', CommandTimeout='30']
      SET NOCOUNT ON;
      UPDATE [Cart] SET [LastUpdatedAt] = @p0
      OUTPUT 1
      WHERE [CartId] = @p1;
      INSERT INTO [CartItem] ([AddedAt], [CartId], [PriceAtAdd], [ProductId], [ProductVariantId], [Quantity])
      OUTPUT INSERTED.[CartItemId]
      VALUES (@p2, @p3, @p4, @p5, @p6, @p7);
info: Microsoft.EntityFrameworkCore.Database.Command[20101]
      Executed DbCommand (57ms) [Parameters=[@__userId_Value_0='?' (DbType = Int32)], CommandType='Text', CommandTimeout='30']
      SELECT TOP(1) [c].[CartId], [c].[CreatedAt], [c].[GuestSessionId], [c].[IsCheckedOut], [c].[LastUpdatedAt], [c].[UserId]
      FROM [Cart] AS [c]
      WHERE [c].[IsCheckedOut] = CAST(0 AS bit) AND [c].[UserId] = @__userId_Value_0
      ORDER BY [c].[CartId]
info: Microsoft.EntityFrameworkCore.Database.Command[20101]
      Executed DbCommand (70ms) [Parameters=[@__userId_Value_0='?' (DbType = Int32)], CommandType='Text', CommandTimeout='30']
      SELECT [t0].[CartItemId], [t0].[AddedAt], [t0].[CartId], [t0].[PriceAtAdd], [t0].[ProductId], [t0].[ProductVariantId], [t0].[Quantity], [t0].[ProductId0], [t0].[AdditionalSpecs], [t0].[AxleStandard], [t0].[BoostCompatible], [t0].[BrakeType], [t0].[BrandId], [t0].[CategoryId], [t0].[Color], [t0].[CreatedAt], [t0].[Currency], [t0].[Description], [t0].[IsActive], [t0].[IsFeatured], [t0].[Material], [t0].[Name], [t0].[Price], [t0].[SKU], [t0].[ShortDescription], [t0].[SpeedCompatibility], [t0].[SuspensionTravel], [t0].[TubelessReady], [t0].[UpdatedAt], [t0].[WheelSize], [t].[CartId], [t0].[ProductVariantId0], [t0].[AdditionalPrice], [t0].[CreatedAt0], [t0].[IsActive0], [t0].[ProductId1], [t0].[ReorderThreshold], [t0].[SKU0], [t0].[StockQuantity], [t0].[UpdatedAt0], [t0].[VariantName]
      FROM (
          SELECT TOP(1) [c].[CartId]
          FROM [Cart] AS [c]
          WHERE [c].[IsCheckedOut] = CAST(0 AS bit) AND [c].[UserId] = @__userId_Value_0
      ) AS [t]
      INNER JOIN (
          SELECT [c0].[CartItemId], [c0].[AddedAt], [c0].[CartId], [c0].[PriceAtAdd], [c0].[ProductId], [c0].[ProductVariantId], [c0].[Quantity], [p].[ProductId] AS [ProductId0], [p].[AdditionalSpecs], [p].[AxleStandard], [p].[BoostCompatible], [p].[BrakeType], [p].[BrandId], [p].[CategoryId], [p].[Color], [p].[CreatedAt], [p].[Currency], [p].[Description], [p].[IsActive], [p].[IsFeatured], [p].[Material], [p].[Name], [p].[Price], [p].[SKU], [p].[ShortDescription], [p].[SpeedCompatibility], [p].[SuspensionTravel], [p].[TubelessReady], [p].[UpdatedAt], [p].[WheelSize], [p0].[ProductVariantId] AS [ProductVariantId0], [p0].[AdditionalPrice], [p0].[CreatedAt] AS [CreatedAt0], [p0].[IsActive] AS [IsActive0], [p0].[ProductId] AS [ProductId1], [p0].[ReorderThreshold], [p0].[SKU] AS [SKU0], [p0].[StockQuantity], [p0].[UpdatedAt] AS [UpdatedAt0], [p0].[VariantName]
          FROM [CartItem] AS [c0]
          INNER JOIN [Product] AS [p] ON [c0].[ProductId] = [p].[ProductId]
          LEFT JOIN [ProductVariant] AS [p0] ON [c0].[ProductVariantId] = [p0].[ProductVariantId]
      ) AS [t0] ON [t].[CartId] = [t0].[CartId]
      ORDER BY [t].[CartId], [t0].[CartItemId], [t0].[ProductId0], [t0].[ProductVariantId0]
info: Microsoft.EntityFrameworkCore.Database.Command[20101]
      Executed DbCommand (67ms) [Parameters=[@__userId_Value_0='?' (DbType = Int32)], CommandType='Text', CommandTimeout='30']
      SELECT [p1].[ProductImageId], [p1].[AltText], [p1].[CreatedAt], [p1].[DisplayOrder], [p1].[FileSizeBytes], [p1].[Height], [p1].[ImageType], [p1].[ImageUrl], [p1].[IsPrimary], [p1].[MimeType], [p1].[ProductId], [p1].[StorageBucket], [p1].[StoragePath], [p1].[UploadedByUserId], [p1].[Width], [t].[CartId], [t0].[CartItemId], [t0].[ProductId0], [t0].[ProductVariantId0]
      FROM (
          SELECT TOP(1) [c].[CartId]
          FROM [Cart] AS [c]
          WHERE [c].[IsCheckedOut] = CAST(0 AS bit) AND [c].[UserId] = @__userId_Value_0
      ) AS [t]
      INNER JOIN (
          SELECT [c0].[CartItemId], [c0].[CartId], [p].[ProductId] AS [ProductId0], [p0].[ProductVariantId] AS [ProductVariantId0]
          FROM [CartItem] AS [c0]
          INNER JOIN [Product] AS [p] ON [c0].[ProductId] = [p].[ProductId]
          LEFT JOIN [ProductVariant] AS [p0] ON [c0].[ProductVariantId] = [p0].[ProductVariantId]
      ) AS [t0] ON [t].[CartId] = [t0].[CartId]
      INNER JOIN [ProductImage] AS [p1] ON [t0].[ProductId0] = [p1].[ProductId]
      ORDER BY [t].[CartId], [t0].[CartItemId], [t0].[ProductId0], [t0].[ProductVariantId0]
info: Microsoft.EntityFrameworkCore.Database.Command[20101]
      Executed DbCommand (61ms) [Parameters=[@__cartId_0='?' (DbType = Int32)], CommandType='Text', CommandTimeout='30']
      SELECT COALESCE(SUM([c].[Quantity]), 0)
      FROM [CartItem] AS [c]
      WHERE [c].[CartId] = @__cartId_0
info: Microsoft.AspNetCore.Mvc.Infrastructure.SystemTextJsonResultExecutor[1]
      Executing JsonResult, writing value of type 'WebApplication.Models.ApiResponse'.
info: Microsoft.AspNetCore.Mvc.Infrastructure.ControllerActionInvoker[105]
      Executed action WebApplication.Controllers.CartController.AddToCart (WebApplication) in 913.7621ms
info: Microsoft.AspNetCore.Routing.EndpointMiddleware[1]
      Executed endpoint 'WebApplication.Controllers.CartController.AddToCart (WebApplication)'
info: Microsoft.AspNetCore.Hosting.Diagnostics[2]
      Request finished HTTP/2 POST https://localhost:7177/Cart/AddToCart - 200 - application/json;+charset=utf-8 926.6421ms
info: Microsoft.AspNetCore.Hosting.Diagnostics[1]
      Request starting HTTP/2 GET https://localhost:7177/Cart/GetCartCount - - -
info: Microsoft.AspNetCore.Routing.EndpointMiddleware[0]
      Executing endpoint 'WebApplication.Controllers.CartController.GetCartCount (WebApplication)'
info: Microsoft.AspNetCore.Mvc.Infrastructure.ControllerActionInvoker[102]
      Route matched with {action = "GetCartCount", controller = "Cart"}. Executing controller action with signature System.Threading.Tasks.Task`1[Microsoft.AspNetCore.Mvc.IActionResult] GetCartCount(System.Threading.CancellationToken) on controller WebApplication.Controllers.CartController (WebApplication).
info: Microsoft.EntityFrameworkCore.Database.Command[20101]
      Executed DbCommand (58ms) [Parameters=[@__userId_Value_0='?' (DbType = Int32)], CommandType='Text', CommandTimeout='30']
      SELECT TOP(1) [c].[CartId], [c].[CreatedAt], [c].[GuestSessionId], [c].[IsCheckedOut], [c].[LastUpdatedAt], [c].[UserId]
      FROM [Cart] AS [c]
      WHERE [c].[IsCheckedOut] = CAST(0 AS bit) AND [c].[UserId] = @__userId_Value_0
      ORDER BY [c].[CartId]
info: Microsoft.EntityFrameworkCore.Database.Command[20101]
      Executed DbCommand (58ms) [Parameters=[@__userId_Value_0='?' (DbType = Int32)], CommandType='Text', CommandTimeout='30']
      SELECT [t0].[CartItemId], [t0].[AddedAt], [t0].[CartId], [t0].[PriceAtAdd], [t0].[ProductId], [t0].[ProductVariantId], [t0].[Quantity], [t0].[ProductId0], [t0].[AdditionalSpecs], [t0].[AxleStandard], [t0].[BoostCompatible], [t0].[BrakeType], [t0].[BrandId], [t0].[CategoryId], [t0].[Color], [t0].[CreatedAt], [t0].[Currency], [t0].[Description], [t0].[IsActive], [t0].[IsFeatured], [t0].[Material], [t0].[Name], [t0].[Price], [t0].[SKU], [t0].[ShortDescription], [t0].[SpeedCompatibility], [t0].[SuspensionTravel], [t0].[TubelessReady], [t0].[UpdatedAt], [t0].[WheelSize], [t].[CartId], [t0].[ProductVariantId0], [t0].[AdditionalPrice], [t0].[CreatedAt0], [t0].[IsActive0], [t0].[ProductId1], [t0].[ReorderThreshold], [t0].[SKU0], [t0].[StockQuantity], [t0].[UpdatedAt0], [t0].[VariantName]
      FROM (
          SELECT TOP(1) [c].[CartId]
          FROM [Cart] AS [c]
          WHERE [c].[IsCheckedOut] = CAST(0 AS bit) AND [c].[UserId] = @__userId_Value_0
      ) AS [t]
      INNER JOIN (
          SELECT [c0].[CartItemId], [c0].[AddedAt], [c0].[CartId], [c0].[PriceAtAdd], [c0].[ProductId], [c0].[ProductVariantId], [c0].[Quantity], [p].[ProductId] AS [ProductId0], [p].[AdditionalSpecs], [p].[AxleStandard], [p].[BoostCompatible], [p].[BrakeType], [p].[BrandId], [p].[CategoryId], [p].[Color], [p].[CreatedAt], [p].[Currency], [p].[Description], [p].[IsActive], [p].[IsFeatured], [p].[Material], [p].[Name], [p].[Price], [p].[SKU], [p].[ShortDescription], [p].[SpeedCompatibility], [p].[SuspensionTravel], [p].[TubelessReady], [p].[UpdatedAt], [p].[WheelSize], [p0].[ProductVariantId] AS [ProductVariantId0], [p0].[AdditionalPrice], [p0].[CreatedAt] AS [CreatedAt0], [p0].[IsActive] AS [IsActive0], [p0].[ProductId] AS [ProductId1], [p0].[ReorderThreshold], [p0].[SKU] AS [SKU0], [p0].[StockQuantity], [p0].[UpdatedAt] AS [UpdatedAt0], [p0].[VariantName]
          FROM [CartItem] AS [c0]
          INNER JOIN [Product] AS [p] ON [c0].[ProductId] = [p].[ProductId]
          LEFT JOIN [ProductVariant] AS [p0] ON [c0].[ProductVariantId] = [p0].[ProductVariantId]
      ) AS [t0] ON [t].[CartId] = [t0].[CartId]
      ORDER BY [t].[CartId], [t0].[CartItemId], [t0].[ProductId0], [t0].[ProductVariantId0]
info: Microsoft.EntityFrameworkCore.Database.Command[20101]
      Executed DbCommand (58ms) [Parameters=[@__userId_Value_0='?' (DbType = Int32)], CommandType='Text', CommandTimeout='30']
      SELECT [p1].[ProductImageId], [p1].[AltText], [p1].[CreatedAt], [p1].[DisplayOrder], [p1].[FileSizeBytes], [p1].[Height], [p1].[ImageType], [p1].[ImageUrl], [p1].[IsPrimary], [p1].[MimeType], [p1].[ProductId], [p1].[StorageBucket], [p1].[StoragePath], [p1].[UploadedByUserId], [p1].[Width], [t].[CartId], [t0].[CartItemId], [t0].[ProductId0], [t0].[ProductVariantId0]
      FROM (
          SELECT TOP(1) [c].[CartId]
          FROM [Cart] AS [c]
          WHERE [c].[IsCheckedOut] = CAST(0 AS bit) AND [c].[UserId] = @__userId_Value_0
      ) AS [t]
      INNER JOIN (
          SELECT [c0].[CartItemId], [c0].[CartId], [p].[ProductId] AS [ProductId0], [p0].[ProductVariantId] AS [ProductVariantId0]
          FROM [CartItem] AS [c0]
          INNER JOIN [Product] AS [p] ON [c0].[ProductId] = [p].[ProductId]
          LEFT JOIN [ProductVariant] AS [p0] ON [c0].[ProductVariantId] = [p0].[ProductVariantId]
      ) AS [t0] ON [t].[CartId] = [t0].[CartId]
      INNER JOIN [ProductImage] AS [p1] ON [t0].[ProductId0] = [p1].[ProductId]
      ORDER BY [t].[CartId], [t0].[CartItemId], [t0].[ProductId0], [t0].[ProductVariantId0]
info: Microsoft.EntityFrameworkCore.Database.Command[20101]
      Executed DbCommand (57ms) [Parameters=[@__cartId_0='?' (DbType = Int32)], CommandType='Text', CommandTimeout='30']
      SELECT COALESCE(SUM([c].[Quantity]), 0)
      FROM [CartItem] AS [c]
      WHERE [c].[CartId] = @__cartId_0
info: Microsoft.AspNetCore.Mvc.Infrastructure.SystemTextJsonResultExecutor[1]
      Executing JsonResult, writing value of type 'WebApplication.Models.ApiResponse'.
info: Microsoft.AspNetCore.Mvc.Infrastructure.ControllerActionInvoker[105]
      Executed action WebApplication.Controllers.CartController.GetCartCount (WebApplication) in 241.7678ms
info: Microsoft.AspNetCore.Routing.EndpointMiddleware[1]
      Executed endpoint 'WebApplication.Controllers.CartController.GetCartCount (WebApplication)'
info: Microsoft.AspNetCore.Hosting.Diagnostics[2]
      Request finished HTTP/2 GET https://localhost:7177/Cart/GetCartCount - 200 - application/json;+charset=utf-8 249.7268ms
info: Microsoft.AspNetCore.Hosting.Diagnostics[1]
      Request starting HTTP/2 GET https://localhost:7177/Checkout - - -
info: Microsoft.AspNetCore.Routing.EndpointMiddleware[0]
      Executing endpoint 'WebApplication.Controllers.CheckoutController.Index (WebApplication)'
info: Microsoft.AspNetCore.Mvc.Infrastructure.ControllerActionInvoker[102]
      Route matched with {action = "Index", controller = "Checkout"}. Executing controller action with signature System.Threading.Tasks.Task`1[Microsoft.AspNetCore.Mvc.IActionResult] Index(System.String, System.Threading.CancellationToken) on controller WebApplication.Controllers.CheckoutController (WebApplication).
info: Microsoft.EntityFrameworkCore.Database.Command[20101]
      Executed DbCommand (58ms) [Parameters=[@__userId_Value_0='?' (DbType = Int32)], CommandType='Text', CommandTimeout='30']
      SELECT TOP(1) [c].[CartId], [c].[CreatedAt], [c].[GuestSessionId], [c].[IsCheckedOut], [c].[LastUpdatedAt], [c].[UserId]
      FROM [Cart] AS [c]
      WHERE [c].[IsCheckedOut] = CAST(0 AS bit) AND [c].[UserId] = @__userId_Value_0
      ORDER BY [c].[CartId]
info: Microsoft.EntityFrameworkCore.Database.Command[20101]
      Executed DbCommand (58ms) [Parameters=[@__userId_Value_0='?' (DbType = Int32)], CommandType='Text', CommandTimeout='30']
      SELECT [t0].[CartItemId], [t0].[AddedAt], [t0].[CartId], [t0].[PriceAtAdd], [t0].[ProductId], [t0].[ProductVariantId], [t0].[Quantity], [t0].[ProductId0], [t0].[AdditionalSpecs], [t0].[AxleStandard], [t0].[BoostCompatible], [t0].[BrakeType], [t0].[BrandId], [t0].[CategoryId], [t0].[Color], [t0].[CreatedAt], [t0].[Currency], [t0].[Description], [t0].[IsActive], [t0].[IsFeatured], [t0].[Material], [t0].[Name], [t0].[Price], [t0].[SKU], [t0].[ShortDescription], [t0].[SpeedCompatibility], [t0].[SuspensionTravel], [t0].[TubelessReady], [t0].[UpdatedAt], [t0].[WheelSize], [t].[CartId], [t0].[ProductVariantId0], [t0].[AdditionalPrice], [t0].[CreatedAt0], [t0].[IsActive0], [t0].[ProductId1], [t0].[ReorderThreshold], [t0].[SKU0], [t0].[StockQuantity], [t0].[UpdatedAt0], [t0].[VariantName]
      FROM (
          SELECT TOP(1) [c].[CartId]
          FROM [Cart] AS [c]
          WHERE [c].[IsCheckedOut] = CAST(0 AS bit) AND [c].[UserId] = @__userId_Value_0
      ) AS [t]
      INNER JOIN (
          SELECT [c0].[CartItemId], [c0].[AddedAt], [c0].[CartId], [c0].[PriceAtAdd], [c0].[ProductId], [c0].[ProductVariantId], [c0].[Quantity], [p].[ProductId] AS [ProductId0], [p].[AdditionalSpecs], [p].[AxleStandard], [p].[BoostCompatible], [p].[BrakeType], [p].[BrandId], [p].[CategoryId], [p].[Color], [p].[CreatedAt], [p].[Currency], [p].[Description], [p].[IsActive], [p].[IsFeatured], [p].[Material], [p].[Name], [p].[Price], [p].[SKU], [p].[ShortDescription], [p].[SpeedCompatibility], [p].[SuspensionTravel], [p].[TubelessReady], [p].[UpdatedAt], [p].[WheelSize], [p0].[ProductVariantId] AS [ProductVariantId0], [p0].[AdditionalPrice], [p0].[CreatedAt] AS [CreatedAt0], [p0].[IsActive] AS [IsActive0], [p0].[ProductId] AS [ProductId1], [p0].[ReorderThreshold], [p0].[SKU] AS [SKU0], [p0].[StockQuantity], [p0].[UpdatedAt] AS [UpdatedAt0], [p0].[VariantName]
          FROM [CartItem] AS [c0]
          INNER JOIN [Product] AS [p] ON [c0].[ProductId] = [p].[ProductId]
          LEFT JOIN [ProductVariant] AS [p0] ON [c0].[ProductVariantId] = [p0].[ProductVariantId]
      ) AS [t0] ON [t].[CartId] = [t0].[CartId]
      ORDER BY [t].[CartId], [t0].[CartItemId], [t0].[ProductId0], [t0].[ProductVariantId0]
info: Microsoft.EntityFrameworkCore.Database.Command[20101]
      Executed DbCommand (59ms) [Parameters=[@__userId_Value_0='?' (DbType = Int32)], CommandType='Text', CommandTimeout='30']
      SELECT [p1].[ProductImageId], [p1].[AltText], [p1].[CreatedAt], [p1].[DisplayOrder], [p1].[FileSizeBytes], [p1].[Height], [p1].[ImageType], [p1].[ImageUrl], [p1].[IsPrimary], [p1].[MimeType], [p1].[ProductId], [p1].[StorageBucket], [p1].[StoragePath], [p1].[UploadedByUserId], [p1].[Width], [t].[CartId], [t0].[CartItemId], [t0].[ProductId0], [t0].[ProductVariantId0]
      FROM (
          SELECT TOP(1) [c].[CartId]
          FROM [Cart] AS [c]
          WHERE [c].[IsCheckedOut] = CAST(0 AS bit) AND [c].[UserId] = @__userId_Value_0
      ) AS [t]
      INNER JOIN (
          SELECT [c0].[CartItemId], [c0].[CartId], [p].[ProductId] AS [ProductId0], [p0].[ProductVariantId] AS [ProductVariantId0]
          FROM [CartItem] AS [c0]
          INNER JOIN [Product] AS [p] ON [c0].[ProductId] = [p].[ProductId]
          LEFT JOIN [ProductVariant] AS [p0] ON [c0].[ProductVariantId] = [p0].[ProductVariantId]
      ) AS [t0] ON [t].[CartId] = [t0].[CartId]
      INNER JOIN [ProductImage] AS [p1] ON [t0].[ProductId0] = [p1].[ProductId]
      ORDER BY [t].[CartId], [t0].[CartItemId], [t0].[ProductId0], [t0].[ProductVariantId0]
info: Microsoft.EntityFrameworkCore.Database.Command[20101]
      Executed DbCommand (61ms) [Parameters=[@__userId_0='?' (DbType = Int32)], CommandType='Text', CommandTimeout='30']
      SELECT TOP(1) [u].[UserId], [u].[CreatedAt], [u].[DefaultAddressId], [u].[Email], [u].[FailedLoginAttempts], [u].[FirstName], [u].[IsActive], [u].[IsDeleted], [u].[IsWalkIn], [u].[LastLoginAt], [u].[LastName], [u].[LockoutUntil], [u].[PasswordHash], [u].[PhoneNumber]
      FROM [User] AS [u]
      WHERE [u].[IsDeleted] = CAST(0 AS bit) AND [u].[UserId] = @__userId_0
      ORDER BY [u].[UserId]
info: Microsoft.EntityFrameworkCore.Database.Command[20101]
      Executed DbCommand (67ms) [Parameters=[@__userId_0='?' (DbType = Int32)], CommandType='Text', CommandTimeout='30']
      SELECT [t0].[AddressId], [t0].[City], [t0].[Country], [t0].[CreatedAt], [t0].[IsDefault], [t0].[IsSnapshot], [t0].[Label], [t0].[PostalCode], [t0].[Province], [t0].[Street], [t0].[UserId], [t].[UserId]
      FROM (
          SELECT TOP(1) [u].[UserId]
          FROM [User] AS [u]
          WHERE [u].[IsDeleted] = CAST(0 AS bit) AND [u].[UserId] = @__userId_0
      ) AS [t]
      INNER JOIN (
          SELECT [a].[AddressId], [a].[City], [a].[Country], [a].[CreatedAt], [a].[IsDefault], [a].[IsSnapshot], [a].[Label], [a].[PostalCode], [a].[Province], [a].[Street], [a].[UserId]
          FROM [Address] AS [a]
          WHERE [a].[IsSnapshot] = CAST(0 AS bit)
      ) AS [t0] ON [t].[UserId] = [t0].[UserId]
      ORDER BY [t].[UserId]
info: Microsoft.EntityFrameworkCore.Database.Command[20101]
      Executed DbCommand (75ms) [Parameters=[@__userId_0='?' (DbType = Int32), @__now_1='?' (DbType = DateTime2), @__now_2='?' (DbType = DateTime2)], CommandType='Text', CommandTimeout='30']
      SELECT [u].[UserVoucherId], [u].[AssignedAt], [u].[ExpiresAt], [u].[UserId], [u].[VoucherId], [v].[VoucherId], [v].[Code], [v].[CreatedAt], [v].[Description], [v].[DiscountType], [v].[DiscountValue], [v].[EndDate], [v].[IsActive], [v].[MaxUses], [v].[MaxUsesPerUser], [v].[MinimumOrderAmount], [v].[StartDate]
      FROM [UserVoucher] AS [u]
      INNER JOIN [Voucher] AS [v] ON [u].[VoucherId] = [v].[VoucherId]
      WHERE [u].[UserId] = @__userId_0 AND [v].[IsActive] = CAST(1 AS bit) AND [v].[StartDate] <= @__now_1 AND ([v].[EndDate] IS NULL OR [v].[EndDate] > @__now_2) AND ([u].[ExpiresAt] IS NULL OR [u].[ExpiresAt] > @__now_2)
      ORDER BY COALESCE([u].[ExpiresAt], [v].[EndDate])
info: Microsoft.AspNetCore.Mvc.ViewFeatures.ViewResultExecutor[1]
      Executing ViewResult, running view ~/Views/Customer/Checkout.cshtml.
info: Microsoft.AspNetCore.Mvc.ViewFeatures.ViewResultExecutor[4]
      Executed ViewResult - view ~/Views/Customer/Checkout.cshtml executed in 20.0776ms.
info: Microsoft.AspNetCore.Mvc.Infrastructure.ControllerActionInvoker[105]
      Executed action WebApplication.Controllers.CheckoutController.Index (WebApplication) in 441.311ms
info: Microsoft.AspNetCore.Routing.EndpointMiddleware[1]
      Executed endpoint 'WebApplication.Controllers.CheckoutController.Index (WebApplication)'
info: Microsoft.AspNetCore.Session.DistributedSession[3]
      Session started; Key:c4a8292d-6fdc-c132-86d1-0d231e04aa12, Id:b5d0feaa-8e97-1ed3-75fc-8b5755ad0e5d
info: Microsoft.AspNetCore.Hosting.Diagnostics[1]
      Request starting HTTP/2 GET https://localhost:7177/js/voucher.js?v=G8xV5C13hEzdSnI1IKgyYWNHPuoWoLfT6B1Yu72Fb7g - - -
info: Microsoft.AspNetCore.Hosting.Diagnostics[1]
      Request starting HTTP/2 GET https://localhost:7177/js/checkout.js?v=ANICVlvgNx4dba_5s46x0T_A0szwFE-0Cm7ydz7YViw - - -
info: Microsoft.AspNetCore.StaticFiles.StaticFileMiddleware[6]
      The file /js/voucher.js was not modified
info: Microsoft.AspNetCore.Hosting.Diagnostics[2]
      Request finished HTTP/2 GET https://localhost:7177/js/voucher.js?v=G8xV5C13hEzdSnI1IKgyYWNHPuoWoLfT6B1Yu72Fb7g - 304 - text/javascript 5.4222ms
info: Microsoft.AspNetCore.Hosting.Diagnostics[1]
      Request starting HTTP/2 GET https://localhost:7177/_framework/aspnetcore-browser-refresh.js - - -
info: Microsoft.AspNetCore.Hosting.Diagnostics[1]
      Request starting HTTP/2 GET https://localhost:7177/_vs/browserLink - - -
info: Microsoft.AspNetCore.Hosting.Diagnostics[2]
      Request finished HTTP/2 GET https://localhost:7177/Checkout - 200 - text/html;+charset=utf-8 461.0150ms
info: Microsoft.AspNetCore.Hosting.Diagnostics[2]
      Request finished HTTP/2 GET https://localhost:7177/_framework/aspnetcore-browser-refresh.js - 200 14933 application/javascript;+charset=utf-8 6.6426ms
info: Microsoft.AspNetCore.Hosting.Diagnostics[2]
      Request finished HTTP/2 GET https://localhost:7177/_vs/browserLink - 200 - text/javascript;+charset=UTF-8 15.4791ms
info: Microsoft.AspNetCore.StaticFiles.StaticFileMiddleware[2]
      Sending file. Request path: '/js/checkout.js'. Physical path: 'C:\Users\user\Documents\ANDREI_FILES\PDM_FILES\2ND YEAR\2ND SEM\OOP\OOP-TaurusBikeShop\WebApplication\wwwroot\js\checkout.js'
info: Microsoft.AspNetCore.Hosting.Diagnostics[2]
      Request finished HTTP/2 GET https://localhost:7177/js/checkout.js?v=ANICVlvgNx4dba_5s46x0T_A0szwFE-0Cm7ydz7YViw - 200 3954 text/javascript 25.5183ms
info: Microsoft.AspNetCore.Hosting.Diagnostics[1]
      Request starting HTTP/2 GET https://localhost:7177/Cart/GetCartCount - - -
info: Microsoft.AspNetCore.Hosting.Diagnostics[1]
      Request starting HTTP/2 GET https://localhost:7177/Customer/NotificationCount - - -
info: Microsoft.AspNetCore.Routing.EndpointMiddleware[0]
      Executing endpoint 'WebApplication.Controllers.CartController.GetCartCount (WebApplication)'
info: Microsoft.AspNetCore.Routing.EndpointMiddleware[0]
      Executing endpoint 'WebApplication.Controllers.CustomerController.NotificationCount (WebApplication)'
info: Microsoft.AspNetCore.Mvc.Infrastructure.ControllerActionInvoker[102]
      Route matched with {action = "NotificationCount", controller = "Customer"}. Executing controller action with signature System.Threading.Tasks.Task`1[Microsoft.AspNetCore.Mvc.IActionResult] NotificationCount(System.Threading.CancellationToken) on controller WebApplication.Controllers.CustomerController (WebApplication).
info: Microsoft.AspNetCore.Mvc.Infrastructure.ControllerActionInvoker[102]
      Route matched with {action = "GetCartCount", controller = "Cart"}. Executing controller action with signature System.Threading.Tasks.Task`1[Microsoft.AspNetCore.Mvc.IActionResult] GetCartCount(System.Threading.CancellationToken) on controller WebApplication.Controllers.CartController (WebApplication).
info: Microsoft.EntityFrameworkCore.Database.Command[20101]
      Executed DbCommand (56ms) [Parameters=[@__userId_Value_0='?' (DbType = Int32)], CommandType='Text', CommandTimeout='30']
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
      Executing JsonResult, writing value of type '<>f__AnonymousType5`1[[System.Int32, System.Private.CoreLib, Version=8.0.0.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e]]'.
info: Microsoft.AspNetCore.Mvc.Infrastructure.ControllerActionInvoker[105]
      Executed action WebApplication.Controllers.CustomerController.NotificationCount (WebApplication) in 63.7571ms
info: Microsoft.AspNetCore.Routing.EndpointMiddleware[1]
      Executed endpoint 'WebApplication.Controllers.CustomerController.NotificationCount (WebApplication)'
info: Microsoft.AspNetCore.Hosting.Diagnostics[2]
      Request finished HTTP/2 GET https://localhost:7177/Customer/NotificationCount - 200 - application/json;+charset=utf-8 75.3005ms
info: Microsoft.EntityFrameworkCore.Database.Command[20101]
      Executed DbCommand (57ms) [Parameters=[@__userId_Value_0='?' (DbType = Int32)], CommandType='Text', CommandTimeout='30']
      SELECT [t0].[CartItemId], [t0].[AddedAt], [t0].[CartId], [t0].[PriceAtAdd], [t0].[ProductId], [t0].[ProductVariantId], [t0].[Quantity], [t0].[ProductId0], [t0].[AdditionalSpecs], [t0].[AxleStandard], [t0].[BoostCompatible], [t0].[BrakeType], [t0].[BrandId], [t0].[CategoryId], [t0].[Color], [t0].[CreatedAt], [t0].[Currency], [t0].[Description], [t0].[IsActive], [t0].[IsFeatured], [t0].[Material], [t0].[Name], [t0].[Price], [t0].[SKU], [t0].[ShortDescription], [t0].[SpeedCompatibility], [t0].[SuspensionTravel], [t0].[TubelessReady], [t0].[UpdatedAt], [t0].[WheelSize], [t].[CartId], [t0].[ProductVariantId0], [t0].[AdditionalPrice], [t0].[CreatedAt0], [t0].[IsActive0], [t0].[ProductId1], [t0].[ReorderThreshold], [t0].[SKU0], [t0].[StockQuantity], [t0].[UpdatedAt0], [t0].[VariantName]
      FROM (
          SELECT TOP(1) [c].[CartId]
          FROM [Cart] AS [c]
          WHERE [c].[IsCheckedOut] = CAST(0 AS bit) AND [c].[UserId] = @__userId_Value_0
      ) AS [t]
      INNER JOIN (
          SELECT [c0].[CartItemId], [c0].[AddedAt], [c0].[CartId], [c0].[PriceAtAdd], [c0].[ProductId], [c0].[ProductVariantId], [c0].[Quantity], [p].[ProductId] AS [ProductId0], [p].[AdditionalSpecs], [p].[AxleStandard], [p].[BoostCompatible], [p].[BrakeType], [p].[BrandId], [p].[CategoryId], [p].[Color], [p].[CreatedAt], [p].[Currency], [p].[Description], [p].[IsActive], [p].[IsFeatured], [p].[Material], [p].[Name], [p].[Price], [p].[SKU], [p].[ShortDescription], [p].[SpeedCompatibility], [p].[SuspensionTravel], [p].[TubelessReady], [p].[UpdatedAt], [p].[WheelSize], [p0].[ProductVariantId] AS [ProductVariantId0], [p0].[AdditionalPrice], [p0].[CreatedAt] AS [CreatedAt0], [p0].[IsActive] AS [IsActive0], [p0].[ProductId] AS [ProductId1], [p0].[ReorderThreshold], [p0].[SKU] AS [SKU0], [p0].[StockQuantity], [p0].[UpdatedAt] AS [UpdatedAt0], [p0].[VariantName]
          FROM [CartItem] AS [c0]
          INNER JOIN [Product] AS [p] ON [c0].[ProductId] = [p].[ProductId]
          LEFT JOIN [ProductVariant] AS [p0] ON [c0].[ProductVariantId] = [p0].[ProductVariantId]
      ) AS [t0] ON [t].[CartId] = [t0].[CartId]
      ORDER BY [t].[CartId], [t0].[CartItemId], [t0].[ProductId0], [t0].[ProductVariantId0]
info: Microsoft.EntityFrameworkCore.Database.Command[20101]
      Executed DbCommand (56ms) [Parameters=[@__userId_Value_0='?' (DbType = Int32)], CommandType='Text', CommandTimeout='30']
      SELECT [p1].[ProductImageId], [p1].[AltText], [p1].[CreatedAt], [p1].[DisplayOrder], [p1].[FileSizeBytes], [p1].[Height], [p1].[ImageType], [p1].[ImageUrl], [p1].[IsPrimary], [p1].[MimeType], [p1].[ProductId], [p1].[StorageBucket], [p1].[StoragePath], [p1].[UploadedByUserId], [p1].[Width], [t].[CartId], [t0].[CartItemId], [t0].[ProductId0], [t0].[ProductVariantId0]
      FROM (
          SELECT TOP(1) [c].[CartId]
          FROM [Cart] AS [c]
          WHERE [c].[IsCheckedOut] = CAST(0 AS bit) AND [c].[UserId] = @__userId_Value_0
      ) AS [t]
      INNER JOIN (
          SELECT [c0].[CartItemId], [c0].[CartId], [p].[ProductId] AS [ProductId0], [p0].[ProductVariantId] AS [ProductVariantId0]
          FROM [CartItem] AS [c0]
          INNER JOIN [Product] AS [p] ON [c0].[ProductId] = [p].[ProductId]
          LEFT JOIN [ProductVariant] AS [p0] ON [c0].[ProductVariantId] = [p0].[ProductVariantId]
      ) AS [t0] ON [t].[CartId] = [t0].[CartId]
      INNER JOIN [ProductImage] AS [p1] ON [t0].[ProductId0] = [p1].[ProductId]
      ORDER BY [t].[CartId], [t0].[CartItemId], [t0].[ProductId0], [t0].[ProductVariantId0]
info: Microsoft.EntityFrameworkCore.Database.Command[20101]
      Executed DbCommand (56ms) [Parameters=[@__cartId_0='?' (DbType = Int32)], CommandType='Text', CommandTimeout='30']
      SELECT COALESCE(SUM([c].[Quantity]), 0)
      FROM [CartItem] AS [c]
      WHERE [c].[CartId] = @__cartId_0
info: Microsoft.AspNetCore.Mvc.Infrastructure.SystemTextJsonResultExecutor[1]
      Executing JsonResult, writing value of type 'WebApplication.Models.ApiResponse'.
info: Microsoft.AspNetCore.Mvc.Infrastructure.ControllerActionInvoker[105]
      Executed action WebApplication.Controllers.CartController.GetCartCount (WebApplication) in 237.6415ms
info: Microsoft.AspNetCore.Routing.EndpointMiddleware[1]
      Executed endpoint 'WebApplication.Controllers.CartController.GetCartCount (WebApplication)'
info: Microsoft.AspNetCore.Hosting.Diagnostics[2]
      Request finished HTTP/2 GET https://localhost:7177/Cart/GetCartCount - 200 - application/json;+charset=utf-8 250.9459ms
info: WebApplication.BusinessLogic.Services.GmailEmailSender[0]
      Email sent to taurusbikeshop89@gmail.com - Subject: Low Stock Alert: 2021 Pinewood Climber CARBON 27.5
info: WebApplication.BackgroundJobs.NotificationDispatchJob[0]
      Notification 139 sent to taurusbikeshop89@gmail.com (LowStockAlert).
info: WebApplication.BusinessLogic.Services.GmailEmailSender[0]
      Email sent to taurusbikeshop89@gmail.com - Subject: Low Stock Alert: Cult Odyssey Hydro Brakes 27.5
info: WebApplication.BackgroundJobs.NotificationDispatchJob[0]
      Notification 140 sent to taurusbikeshop89@gmail.com (LowStockAlert).
info: WebApplication.BusinessLogic.Services.GmailEmailSender[0]
      Email sent to taurusbikeshop89@gmail.com - Subject: Low Stock Alert: Toseek Chester 700c Disc Brake ALLOY (2x9)
info: WebApplication.BackgroundJobs.NotificationDispatchJob[0]
      Notification 141 sent to taurusbikeshop89@gmail.com (LowStockAlert).
info: WebApplication.BusinessLogic.Services.GmailEmailSender[0]
      Email sent to taurusbikeshop89@gmail.com - Subject: Low Stock Alert: Ryder X3 27.5 2021 MT200 Hydro Brakes (3x8)
info: WebApplication.BackgroundJobs.NotificationDispatchJob[0]
      Notification 142 sent to taurusbikeshop89@gmail.com (LowStockAlert).
info: WebApplication.BusinessLogic.Services.GmailEmailSender[0]
      Email sent to geronimojoan002@gmail.com - Subject: Hurry - Ryder X3 27.5 2021 MT200 Hydro Brakes (3x8) is almost sold out!
info: WebApplication.BackgroundJobs.NotificationDispatchJob[0]
      Notification 143 sent to geronimojoan002@gmail.com (WishlistRestock).
info: WebApplication.BusinessLogic.Services.GmailEmailSender[0]
      Email sent to taurusbikeshop89@gmail.com - Subject: Low Stock Alert: Pinewood Trident Flux
info: WebApplication.BackgroundJobs.NotificationDispatchJob[0]
      Notification 144 sent to taurusbikeshop89@gmail.com (LowStockAlert).
info: WebApplication.BusinessLogic.Services.GmailEmailSender[0]
      Email sent to geronimojoan002@gmail.com - Subject: Hurry - Pinewood Trident Flux is almost sold out!
info: WebApplication.BackgroundJobs.NotificationDispatchJob[0]
      Notification 145 sent to geronimojoan002@gmail.com (WishlistRestock).
info: WebApplication.BusinessLogic.Services.GmailEmailSender[0]
      Email sent to taurusbikeshop89@gmail.com - Subject: Low Stock Alert: Pinewood Challenger
info: WebApplication.BackgroundJobs.NotificationDispatchJob[0]
      Notification 146 sent to taurusbikeshop89@gmail.com (LowStockAlert).
info: Microsoft.EntityFrameworkCore.Database.Command[20101]
      Executed DbCommand (57ms) [Parameters=[], CommandType='Text', CommandTimeout='30']
      SELECT COUNT(*)
      FROM [ProductVariant] AS [p]
info: WebApplication.BusinessLogic.Services.GmailEmailSender[0]
      Email sent to geronimojoan002@gmail.com - Subject: Hurry - Pinewood Challenger is almost sold out!
info: WebApplication.BackgroundJobs.NotificationDispatchJob[0]
      Notification 147 sent to geronimojoan002@gmail.com (WishlistRestock).
info: WebApplication.BusinessLogic.Services.GmailEmailSender[0]
      Email sent to taurusbikeshop89@gmail.com - Subject: Low Stock Alert: Kespor Stork Feather CX 1.0 2022
info: WebApplication.BackgroundJobs.NotificationDispatchJob[0]
      Notification 148 sent to taurusbikeshop89@gmail.com (LowStockAlert).
info: Microsoft.EntityFrameworkCore.Database.Command[20101]
      Executed DbCommand (91ms) [Parameters=[@p2='?' (DbType = Int32), @p0='?' (DbType = DateTime2), @p1='?' (Size = 20), @p5='?' (DbType = Int32), @p3='?' (DbType = DateTime2), @p4='?' (Size = 20), @p8='?' (DbType = Int32), @p6='?' (DbType = DateTime2), @p7='?' (Size = 20), @p11='?' (DbType = Int32), @p9='?' (DbType = DateTime2), @p10='?' (Size = 20), @p14='?' (DbType = Int32), @p12='?' (DbType = DateTime2), @p13='?' (Size = 20), @p17='?' (DbType = Int32), @p15='?' (DbType = DateTime2), @p16='?' (Size = 20), @p20='?' (DbType = Int32), @p18='?' (DbType = DateTime2), @p19='?' (Size = 20), @p23='?' (DbType = Int32), @p21='?' (DbType = DateTime2), @p22='?' (Size = 20), @p26='?' (DbType = Int32), @p24='?' (DbType = DateTime2), @p25='?' (Size = 20), @p29='?' (DbType = Int32), @p27='?' (DbType = DateTime2), @p28='?' (Size = 20)], CommandType='Text', CommandTimeout='30']
      SET NOCOUNT ON;
      UPDATE [Notification] SET [SentAt] = @p0, [Status] = @p1
      OUTPUT 1
      WHERE [NotificationId] = @p2;
      UPDATE [Notification] SET [SentAt] = @p3, [Status] = @p4
      OUTPUT 1
      WHERE [NotificationId] = @p5;
      UPDATE [Notification] SET [SentAt] = @p6, [Status] = @p7
      OUTPUT 1
      WHERE [NotificationId] = @p8;
      UPDATE [Notification] SET [SentAt] = @p9, [Status] = @p10
      OUTPUT 1
      WHERE [NotificationId] = @p11;
      UPDATE [Notification] SET [SentAt] = @p12, [Status] = @p13
      OUTPUT 1
      WHERE [NotificationId] = @p14;
      UPDATE [Notification] SET [SentAt] = @p15, [Status] = @p16
      OUTPUT 1
      WHERE [NotificationId] = @p17;
      UPDATE [Notification] SET [SentAt] = @p18, [Status] = @p19
      OUTPUT 1
      WHERE [NotificationId] = @p20;
      UPDATE [Notification] SET [SentAt] = @p21, [Status] = @p22
      OUTPUT 1
      WHERE [NotificationId] = @p23;
      UPDATE [Notification] SET [SentAt] = @p24, [Status] = @p25
      OUTPUT 1
      WHERE [NotificationId] = @p26;
      UPDATE [Notification] SET [SentAt] = @p27, [Status] = @p28
      OUTPUT 1
      WHERE [NotificationId] = @p29;
info: Microsoft.AspNetCore.Hosting.Diagnostics[1]
      Request starting HTTP/2 POST https://localhost:7177/Checkout/PlaceOrder - application/x-www-form-urlencoded 326
info: Microsoft.AspNetCore.Cors.Infrastructure.CorsService[4]
      CORS policy execution successful.
info: Microsoft.AspNetCore.Routing.EndpointMiddleware[0]
      Executing endpoint 'WebApplication.Controllers.CheckoutController.PlaceOrder (WebApplication)'
info: Microsoft.AspNetCore.Mvc.Infrastructure.ControllerActionInvoker[102]
      Route matched with {action = "PlaceOrder", controller = "Checkout"}. Executing controller action with signature System.Threading.Tasks.Task`1[Microsoft.AspNetCore.Mvc.IActionResult] PlaceOrder(WebApplication.Models.ViewModels.CheckoutViewModel, System.Threading.CancellationToken) on controller WebApplication.Controllers.CheckoutController (WebApplication).
info: Microsoft.EntityFrameworkCore.Database.Command[20101]
      Executed DbCommand (56ms) [Parameters=[@__userId_Value_0='?' (DbType = Int32)], CommandType='Text', CommandTimeout='30']
      SELECT TOP(1) [c].[CartId], [c].[CreatedAt], [c].[GuestSessionId], [c].[IsCheckedOut], [c].[LastUpdatedAt], [c].[UserId]
      FROM [Cart] AS [c]
      WHERE [c].[IsCheckedOut] = CAST(0 AS bit) AND [c].[UserId] = @__userId_Value_0
      ORDER BY [c].[CartId]
info: Microsoft.EntityFrameworkCore.Database.Command[20101]
      Executed DbCommand (58ms) [Parameters=[@__userId_Value_0='?' (DbType = Int32)], CommandType='Text', CommandTimeout='30']
      SELECT [t0].[CartItemId], [t0].[AddedAt], [t0].[CartId], [t0].[PriceAtAdd], [t0].[ProductId], [t0].[ProductVariantId], [t0].[Quantity], [t0].[ProductId0], [t0].[AdditionalSpecs], [t0].[AxleStandard], [t0].[BoostCompatible], [t0].[BrakeType], [t0].[BrandId], [t0].[CategoryId], [t0].[Color], [t0].[CreatedAt], [t0].[Currency], [t0].[Description], [t0].[IsActive], [t0].[IsFeatured], [t0].[Material], [t0].[Name], [t0].[Price], [t0].[SKU], [t0].[ShortDescription], [t0].[SpeedCompatibility], [t0].[SuspensionTravel], [t0].[TubelessReady], [t0].[UpdatedAt], [t0].[WheelSize], [t].[CartId], [t0].[ProductVariantId0], [t0].[AdditionalPrice], [t0].[CreatedAt0], [t0].[IsActive0], [t0].[ProductId1], [t0].[ReorderThreshold], [t0].[SKU0], [t0].[StockQuantity], [t0].[UpdatedAt0], [t0].[VariantName]
      FROM (
          SELECT TOP(1) [c].[CartId]
          FROM [Cart] AS [c]
          WHERE [c].[IsCheckedOut] = CAST(0 AS bit) AND [c].[UserId] = @__userId_Value_0
      ) AS [t]
      INNER JOIN (
          SELECT [c0].[CartItemId], [c0].[AddedAt], [c0].[CartId], [c0].[PriceAtAdd], [c0].[ProductId], [c0].[ProductVariantId], [c0].[Quantity], [p].[ProductId] AS [ProductId0], [p].[AdditionalSpecs], [p].[AxleStandard], [p].[BoostCompatible], [p].[BrakeType], [p].[BrandId], [p].[CategoryId], [p].[Color], [p].[CreatedAt], [p].[Currency], [p].[Description], [p].[IsActive], [p].[IsFeatured], [p].[Material], [p].[Name], [p].[Price], [p].[SKU], [p].[ShortDescription], [p].[SpeedCompatibility], [p].[SuspensionTravel], [p].[TubelessReady], [p].[UpdatedAt], [p].[WheelSize], [p0].[ProductVariantId] AS [ProductVariantId0], [p0].[AdditionalPrice], [p0].[CreatedAt] AS [CreatedAt0], [p0].[IsActive] AS [IsActive0], [p0].[ProductId] AS [ProductId1], [p0].[ReorderThreshold], [p0].[SKU] AS [SKU0], [p0].[StockQuantity], [p0].[UpdatedAt] AS [UpdatedAt0], [p0].[VariantName]
          FROM [CartItem] AS [c0]
          INNER JOIN [Product] AS [p] ON [c0].[ProductId] = [p].[ProductId]
          LEFT JOIN [ProductVariant] AS [p0] ON [c0].[ProductVariantId] = [p0].[ProductVariantId]
      ) AS [t0] ON [t].[CartId] = [t0].[CartId]
      ORDER BY [t].[CartId], [t0].[CartItemId], [t0].[ProductId0], [t0].[ProductVariantId0]
info: Microsoft.EntityFrameworkCore.Database.Command[20101]
      Executed DbCommand (57ms) [Parameters=[@__userId_Value_0='?' (DbType = Int32)], CommandType='Text', CommandTimeout='30']
      SELECT [p1].[ProductImageId], [p1].[AltText], [p1].[CreatedAt], [p1].[DisplayOrder], [p1].[FileSizeBytes], [p1].[Height], [p1].[ImageType], [p1].[ImageUrl], [p1].[IsPrimary], [p1].[MimeType], [p1].[ProductId], [p1].[StorageBucket], [p1].[StoragePath], [p1].[UploadedByUserId], [p1].[Width], [t].[CartId], [t0].[CartItemId], [t0].[ProductId0], [t0].[ProductVariantId0]
      FROM (
          SELECT TOP(1) [c].[CartId]
          FROM [Cart] AS [c]
          WHERE [c].[IsCheckedOut] = CAST(0 AS bit) AND [c].[UserId] = @__userId_Value_0
      ) AS [t]
      INNER JOIN (
          SELECT [c0].[CartItemId], [c0].[CartId], [p].[ProductId] AS [ProductId0], [p0].[ProductVariantId] AS [ProductVariantId0]
          FROM [CartItem] AS [c0]
          INNER JOIN [Product] AS [p] ON [c0].[ProductId] = [p].[ProductId]
          LEFT JOIN [ProductVariant] AS [p0] ON [c0].[ProductVariantId] = [p0].[ProductVariantId]
      ) AS [t0] ON [t].[CartId] = [t0].[CartId]
      INNER JOIN [ProductImage] AS [p1] ON [t0].[ProductId0] = [p1].[ProductId]
      ORDER BY [t].[CartId], [t0].[CartItemId], [t0].[ProductId0], [t0].[ProductVariantId0]
info: Microsoft.EntityFrameworkCore.Database.Command[20101]
      Executed DbCommand (59ms) [Parameters=[@__item_ProductVariantId_0='?' (DbType = Int32)], CommandType='Text', CommandTimeout='30']
      SELECT TOP(1) [p].[ProductVariantId], [p].[AdditionalPrice], [p].[CreatedAt], [p].[IsActive], [p].[ProductId], [p].[ReorderThreshold], [p].[SKU], [p].[StockQuantity], [p].[UpdatedAt], [p].[VariantName]
      FROM [ProductVariant] AS [p]
      WHERE [p].[ProductVariantId] = @__item_ProductVariantId_0
info: Microsoft.EntityFrameworkCore.Database.Command[20101]
      Executed DbCommand (59ms) [Parameters=[@__8__locals1_vm_SelectedAddressId_0='?' (DbType = Int32), @__8__locals1_userId_1='?' (DbType = Int32)], CommandType='Text', CommandTimeout='30']
      SELECT TOP(1) [a].[AddressId], [a].[City], [a].[Country], [a].[CreatedAt], [a].[IsDefault], [a].[IsSnapshot], [a].[Label], [a].[PostalCode], [a].[Province], [a].[Street], [a].[UserId]
      FROM [Address] AS [a]
      WHERE [a].[AddressId] = @__8__locals1_vm_SelectedAddressId_0 AND [a].[UserId] = @__8__locals1_userId_1 AND [a].[IsSnapshot] = CAST(0 AS bit)
info: Microsoft.EntityFrameworkCore.Database.Command[20101]
      Executed DbCommand (61ms) [Parameters=[@p0='?' (Size = 100), @p1='?' (Size = 100), @p2='?' (DbType = DateTime2), @p3='?' (DbType = Boolean), @p4='?' (DbType = Boolean), @p5='?' (Size = 50), @p6='?' (Size = 20), @p7='?' (Size = 100), @p8='?' (Size = 500), @p9='?' (DbType = Int32)], CommandType='Text', CommandTimeout='30']
      SET IMPLICIT_TRANSACTIONS OFF;
      SET NOCOUNT ON;
      INSERT INTO [Address] ([City], [Country], [CreatedAt], [IsDefault], [IsSnapshot], [Label], [PostalCode], [Province], [Street], [UserId])
      OUTPUT INSERTED.[AddressId]
      VALUES (@p0, @p1, @p2, @p3, @p4, @p5, @p6, @p7, @p8, @p9);
info: Microsoft.EntityFrameworkCore.Database.Command[20101]
      Executed DbCommand (60ms) [Parameters=[], CommandType='Text', CommandTimeout='30']
      SELECT COUNT(*)
      FROM [Order] AS [o]
      WHERE [o].[IsDeleted] = CAST(0 AS bit)
info: Microsoft.EntityFrameworkCore.Database.Command[20101]
      Executed DbCommand (74ms) [Parameters=[@p0='?' (DbType = Int32), @p1='?' (Size = 20), @p2='?' (DbType = DateTime2), @p3='?' (Size = 500), @p4='?' (Precision = 18) (Scale = 2) (DbType = Decimal), @p5='?' (Size = 20), @p6='?' (DbType = Int32), @p7='?' (DbType = Boolean), @p8='?' (DbType = Boolean), @p9='?' (DbType = DateTime2), @p10='?' (Size = 50), @p11='?' (Size = 50), @p12='?' (DbType = Int32), @p13='?' (Size = 50), @p14='?' (DbType = Int32), @p15='?' (Precision = 18) (Scale = 2) (DbType = Decimal), @p16='?' (Precision = 18) (Scale = 2) (DbType = Decimal), @p17='?' (DbType = DateTime2), @p18='?' (DbType = Int32)], CommandType='Text', CommandTimeout='30']
      SET IMPLICIT_TRANSACTIONS OFF;
      SET NOCOUNT ON;
      INSERT INTO [Order] ([CartId], [ContactPhone], [CreatedAt], [DeliveryInstructions], [DiscountAmount], [FulfillmentType], [GuestSessionId], [IsDeleted], [IsWalkIn], [OrderDate], [OrderNumber], [OrderStatus], [POSSessionId], [PaymentMethod], [ShippingAddressId], [ShippingFee], [SubTotal], [UpdatedAt], [UserId])
      OUTPUT INSERTED.[OrderId], INSERTED.[TotalAmount]
      VALUES (@p0, @p1, @p2, @p3, @p4, @p5, @p6, @p7, @p8, @p9, @p10, @p11, @p12, @p13, @p14, @p15, @p16, @p17, @p18);
info: Microsoft.EntityFrameworkCore.Database.Command[20101]
      Executed DbCommand (63ms) [Parameters=[@p0='?' (DbType = Int32), @p1='?' (DbType = Int32), @p2='?' (DbType = Int32), @p3='?' (DbType = Int32), @p4='?' (Precision = 18) (Scale = 2) (DbType = Decimal)], CommandType='Text', CommandTimeout='30']
      SET IMPLICIT_TRANSACTIONS OFF;
      SET NOCOUNT ON;
      INSERT INTO [OrderItem] ([OrderId], [ProductId], [ProductVariantId], [Quantity], [UnitPrice])
      OUTPUT INSERTED.[OrderItemId]
      VALUES (@p0, @p1, @p2, @p3, @p4);
info: Microsoft.EntityFrameworkCore.Database.Command[20101]
      Executed DbCommand (57ms) [Parameters=[@__cartItem_ProductVariantId_0='?' (DbType = Int32)], CommandType='Text', CommandTimeout='30']
      SELECT TOP(1) [p].[ProductVariantId], [p].[AdditionalPrice], [p].[CreatedAt], [p].[IsActive], [p].[ProductId], [p].[ReorderThreshold], [p].[SKU], [p].[StockQuantity], [p].[UpdatedAt], [p].[VariantName]
      FROM [ProductVariant] AS [p]
      WHERE [p].[ProductVariantId] = @__cartItem_ProductVariantId_0
info: Microsoft.EntityFrameworkCore.Database.Command[20101]
      Executed DbCommand (84ms) [Parameters=[@p0='?' (DbType = Int32), @p1='?' (Size = 50), @p2='?' (DbType = Int32), @p3='?' (DbType = DateTime2), @p4='?' (Size = 500), @p5='?' (DbType = Int32), @p6='?' (DbType = Int32), @p7='?' (DbType = Int32), @p8='?' (DbType = Int32), @p10='?' (DbType = Int32), @p9='?' (DbType = Int32)], CommandType='Text', CommandTimeout='30']
      SET NOCOUNT ON;
      INSERT INTO [InventoryLog] ([ChangeQuantity], [ChangeType], [ChangedByUserId], [CreatedAt], [Notes], [OrderId], [ProductId], [ProductVariantId], [PurchaseOrderId])
      OUTPUT INSERTED.[InventoryLogId]
      VALUES (@p0, @p1, @p2, @p3, @p4, @p5, @p6, @p7, @p8);
      UPDATE [ProductVariant] SET [StockQuantity] = @p9
      OUTPUT 1
      WHERE [ProductVariantId] = @p10;
info: Microsoft.EntityFrameworkCore.Database.Command[20101]
      Executed DbCommand (63ms) [Parameters=[@p2='?' (DbType = Int32), @p0='?' (DbType = Boolean), @p1='?' (DbType = DateTime2), @p3='?' (DbType = Int32)], CommandType='Text', CommandTimeout='30']
      SET NOCOUNT ON;
      UPDATE [Cart] SET [IsCheckedOut] = @p0, [LastUpdatedAt] = @p1
      OUTPUT 1
      WHERE [CartId] = @p2;
      DELETE FROM [CartItem]
      OUTPUT 1
      WHERE [CartItemId] = @p3;
info: Microsoft.EntityFrameworkCore.Database.Command[20101]
      Executed DbCommand (61ms) [Parameters=[@p0='?' (DbType = DateTime2), @p1='?' (Size = 1000), @p2='?' (Size = 100), @p3='?' (DbType = Int32)], CommandType='Text', CommandTimeout='30']
      SET IMPLICIT_TRANSACTIONS OFF;
      SET NOCOUNT ON;
      INSERT INTO [SystemLog] ([CreatedAt], [EventDescription], [EventType], [UserId])
      OUTPUT INSERTED.[SystemLogId]
      VALUES (@p0, @p1, @p2, @p3);
info: Microsoft.EntityFrameworkCore.Database.Command[20101]
      Executed DbCommand (66ms) [Parameters=[@__8__locals1_userId_0='?' (DbType = Int32)], CommandType='Text', CommandTimeout='30']
      SELECT TOP(1) [u].[UserId], [u].[CreatedAt], [u].[DefaultAddressId], [u].[Email], [u].[FailedLoginAttempts], [u].[FirstName], [u].[IsActive], [u].[IsDeleted], [u].[IsWalkIn], [u].[LastLoginAt], [u].[LastName], [u].[LockoutUntil], [u].[PasswordHash], [u].[PhoneNumber]
      FROM [User] AS [u]
      WHERE [u].[IsDeleted] = CAST(0 AS bit) AND [u].[UserId] = @__8__locals1_userId_0
info: Microsoft.EntityFrameworkCore.Database.Command[20101]
      Executed DbCommand (58ms) [Parameters=[@p0='?' (Size = 4000), @p1='?' (Size = 20), @p2='?' (DbType = DateTime2), @p3='?' (Size = 500), @p4='?' (DbType = Boolean), @p5='?' (Size = 100), @p6='?' (DbType = Int32), @p7='?' (DbType = DateTime2), @p8='?' (Size = 255), @p9='?' (DbType = Int32), @p10='?' (DbType = DateTime2), @p11='?' (Size = 20), @p12='?' (Size = 255), @p13='?' (DbType = Int32), @p14='?' (DbType = Int32)], CommandType='Text', CommandTimeout='30']
      SET IMPLICIT_TRANSACTIONS OFF;
      SET NOCOUNT ON;
      INSERT INTO [Notification] ([Body], [Channel], [CreatedAt], [FailureReason], [IsRead], [NotifType], [OrderId], [ReadAt], [Recipient], [RetryCount], [SentAt], [Status], [Subject], [TicketId], [UserId])
      OUTPUT INSERTED.[NotificationId]
      VALUES (@p0, @p1, @p2, @p3, @p4, @p5, @p6, @p7, @p8, @p9, @p10, @p11, @p12, @p13, @p14);
info: Microsoft.AspNetCore.Mvc.RedirectToActionResult[1]
      Executing RedirectResult, redirecting to /Order/Confirmation?orderId=37.
info: Microsoft.AspNetCore.Mvc.Infrastructure.ControllerActionInvoker[105]
      Executed action WebApplication.Controllers.CheckoutController.PlaceOrder (WebApplication) in 1565.0744ms
info: Microsoft.AspNetCore.Routing.EndpointMiddleware[1]
      Executed endpoint 'WebApplication.Controllers.CheckoutController.PlaceOrder (WebApplication)'
info: Microsoft.AspNetCore.Hosting.Diagnostics[2]
      Request finished HTTP/2 POST https://localhost:7177/Checkout/PlaceOrder - 302 - - 1597.8276ms
info: Microsoft.AspNetCore.Hosting.Diagnostics[1]
      Request starting HTTP/2 GET https://localhost:7177/Order/Confirmation?orderId=37 - - -
info: Microsoft.AspNetCore.Routing.EndpointMiddleware[0]
      Executing endpoint 'WebApplication.Controllers.OrderController.Confirmation (WebApplication)'
info: Microsoft.AspNetCore.Mvc.Infrastructure.ControllerActionInvoker[102]
      Route matched with {action = "Confirmation", controller = "Order"}. Executing controller action with signature System.Threading.Tasks.Task`1[Microsoft.AspNetCore.Mvc.IActionResult] Confirmation(Int32, System.Threading.CancellationToken) on controller WebApplication.Controllers.OrderController (WebApplication).
info: Microsoft.EntityFrameworkCore.Database.Command[20101]
      Executed DbCommand (68ms) [Parameters=[@__orderId_0='?' (DbType = Int32)], CommandType='Text', CommandTimeout='30']
      SELECT TOP(1) [o].[OrderId], [o].[CartId], [o].[ContactPhone], [o].[CreatedAt], [o].[DeliveryInstructions], [o].[DiscountAmount], [o].[FulfillmentType], [o].[GuestSessionId], [o].[IsDeleted], [o].[IsWalkIn], [o].[OrderDate], [o].[OrderNumber], [o].[OrderStatus], [o].[POSSessionId], [o].[PaymentMethod], [o].[ShippingAddressId], [o].[ShippingFee], [o].[SubTotal], [o].[TotalAmount], [o].[UpdatedAt], [o].[UserId], [p].[PickupOrderId], [a].[AddressId], [p].[OrderId], [p].[PickupConfirmedAt], [p].[PickupExpiresAt], [p].[PickupReadyAt], [a].[City], [a].[Country], [a].[CreatedAt], [a].[IsDefault], [a].[IsSnapshot], [a].[Label], [a].[PostalCode], [a].[Province], [a].[Street], [a].[UserId]
      FROM [Order] AS [o]
      LEFT JOIN [PickupOrder] AS [p] ON [o].[OrderId] = [p].[OrderId]
      LEFT JOIN [Address] AS [a] ON [o].[ShippingAddressId] = [a].[AddressId]
      WHERE [o].[IsDeleted] = CAST(0 AS bit) AND [o].[OrderId] = @__orderId_0
      ORDER BY [o].[OrderId], [p].[PickupOrderId], [a].[AddressId]
info: Microsoft.EntityFrameworkCore.Database.Command[20101]
      Executed DbCommand (68ms) [Parameters=[@__orderId_0='?' (DbType = Int32)], CommandType='Text', CommandTimeout='30']
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
      Executed DbCommand (82ms) [Parameters=[@__orderId_0='?' (DbType = Int32)], CommandType='Text', CommandTimeout='30']
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
      Executed DbCommand (73ms) [Parameters=[@__orderId_0='?' (DbType = Int32)], CommandType='Text', CommandTimeout='30']
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
      Executed DbCommand (64ms) [Parameters=[@__orderId_0='?' (DbType = Int32)], CommandType='Text', CommandTimeout='30']
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
      Executing ViewResult, running view ~/Views/Customer/Confirmation.cshtml.
info: Microsoft.AspNetCore.Mvc.ViewFeatures.ViewResultExecutor[4]
      Executed ViewResult - view ~/Views/Customer/Confirmation.cshtml executed in 8.7283ms.
info: Microsoft.AspNetCore.Mvc.Infrastructure.ControllerActionInvoker[105]
      Executed action WebApplication.Controllers.OrderController.Confirmation (WebApplication) in 420.7126ms
info: Microsoft.AspNetCore.Routing.EndpointMiddleware[1]
      Executed endpoint 'WebApplication.Controllers.OrderController.Confirmation (WebApplication)'
info: Microsoft.AspNetCore.Hosting.Diagnostics[1]
      Request starting HTTP/2 GET https://localhost:7177/_framework/aspnetcore-browser-refresh.js - - -
info: Microsoft.AspNetCore.Hosting.Diagnostics[1]
      Request starting HTTP/2 GET https://localhost:7177/_vs/browserLink - - -
info: Microsoft.AspNetCore.Hosting.Diagnostics[2]
      Request finished HTTP/2 GET https://localhost:7177/Order/Confirmation?orderId=37 - 200 - text/html;+charset=utf-8 434.1130ms
info: Microsoft.AspNetCore.Hosting.Diagnostics[2]
      Request finished HTTP/2 GET https://localhost:7177/_framework/aspnetcore-browser-refresh.js - 200 14933 application/javascript;+charset=utf-8 6.6110ms
info: Microsoft.AspNetCore.Hosting.Diagnostics[2]
      Request finished HTTP/2 GET https://localhost:7177/_vs/browserLink - 200 - text/javascript;+charset=UTF-8 19.9430ms
info: Microsoft.AspNetCore.Hosting.Diagnostics[1]
      Request starting HTTP/2 GET https://localhost:7177/Customer/NotificationCount - - -
info: Microsoft.AspNetCore.Hosting.Diagnostics[1]
      Request starting HTTP/2 GET https://localhost:7177/antiforgery/token - - -
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
info: Microsoft.AspNetCore.Http.Result.OkObjectResult[1]
      Setting HTTP status code 200.
info: Microsoft.AspNetCore.Http.Result.OkObjectResult[3]
      Writing value of type '<>f__AnonymousType19`1' as Json.
info: Microsoft.AspNetCore.Mvc.Infrastructure.ControllerActionInvoker[102]
      Route matched with {action = "GetCartCount", controller = "Cart"}. Executing controller action with signature System.Threading.Tasks.Task`1[Microsoft.AspNetCore.Mvc.IActionResult] GetCartCount(System.Threading.CancellationToken) on controller WebApplication.Controllers.CartController (WebApplication).
info: Microsoft.AspNetCore.Routing.EndpointMiddleware[1]
      Executed endpoint 'HTTP: GET /antiforgery/token'
info: Microsoft.AspNetCore.Hosting.Diagnostics[2]
      Request finished HTTP/2 GET https://localhost:7177/antiforgery/token - 200 - application/json;+charset=utf-8 19.0592ms
info: Microsoft.EntityFrameworkCore.Database.Command[20101]
      Executed DbCommand (56ms) [Parameters=[@__userId_0='?' (DbType = Int32)], CommandType='Text', CommandTimeout='30']
      SELECT COUNT(*)
      FROM [Notification] AS [n]
      WHERE [n].[UserId] = @__userId_0 AND [n].[IsRead] = CAST(0 AS bit) AND [n].[Channel] = N'InApp'
info: Microsoft.AspNetCore.Mvc.Infrastructure.SystemTextJsonResultExecutor[1]
      Executing JsonResult, writing value of type '<>f__AnonymousType5`1[[System.Int32, System.Private.CoreLib, Version=8.0.0.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e]]'.
info: Microsoft.AspNetCore.Mvc.Infrastructure.ControllerActionInvoker[105]
      Executed action WebApplication.Controllers.CustomerController.NotificationCount (WebApplication) in 61.9676ms
info: Microsoft.AspNetCore.Routing.EndpointMiddleware[1]
      Executed endpoint 'WebApplication.Controllers.CustomerController.NotificationCount (WebApplication)'
info: Microsoft.AspNetCore.Hosting.Diagnostics[2]
      Request finished HTTP/2 GET https://localhost:7177/Customer/NotificationCount - 200 - application/json;+charset=utf-8 74.9166ms
info: Microsoft.EntityFrameworkCore.Database.Command[20101]
      Executed DbCommand (57ms) [Parameters=[@__userId_Value_0='?' (DbType = Int32)], CommandType='Text', CommandTimeout='30']
      SELECT TOP(1) [c].[CartId], [c].[CreatedAt], [c].[GuestSessionId], [c].[IsCheckedOut], [c].[LastUpdatedAt], [c].[UserId]
      FROM [Cart] AS [c]
      WHERE [c].[IsCheckedOut] = CAST(0 AS bit) AND [c].[UserId] = @__userId_Value_0
      ORDER BY [c].[CartId]
info: Microsoft.AspNetCore.Mvc.Infrastructure.SystemTextJsonResultExecutor[1]
      Executing JsonResult, writing value of type 'WebApplication.Models.ApiResponse'.
info: Microsoft.AspNetCore.Mvc.Infrastructure.ControllerActionInvoker[105]
      Executed action WebApplication.Controllers.CartController.GetCartCount (WebApplication) in 63.7094ms
info: Microsoft.AspNetCore.Routing.EndpointMiddleware[1]
      Executed endpoint 'WebApplication.Controllers.CartController.GetCartCount (WebApplication)'
info: Microsoft.AspNetCore.Hosting.Diagnostics[2]
      Request finished HTTP/2 GET https://localhost:7177/Cart/GetCartCount - 200 - application/json;+charset=utf-8 83.5398ms
