[WARNING] Google Cloud Storage unavailable: Your default credentials were not found. To set up Application Default Credentials, see https://cloud.google.com/docs/authentication/external/set-up-adc.
[WARNING] File upload features will be disabled. Set GOOGLE_APPLICATION_CREDENTIALS to enable.
info: Microsoft.AspNetCore.DataProtection.KeyManagement.XmlKeyManager[63]
      User profile is available. Using 'C:\Users\user\AppData\Local\ASP.NET\DataProtection-Keys' as key repository and Windows DPAPI to encrypt keys at rest.
info: WebApplication.BackgroundJobs.InventorySyncJob[0]
      InventorySyncJob started.
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
info: Microsoft.EntityFrameworkCore.Database.Command[20101]
      Executed DbCommand (87ms) [Parameters=[], CommandType='Text', CommandTimeout='30']
      SELECT COUNT(*)
      FROM [ProductVariant] AS [p]
info: Microsoft.AspNetCore.Hosting.Diagnostics[1]
      Request starting HTTP/2 GET https://localhost:7177/ - - -
info: Microsoft.EntityFrameworkCore.Database.Command[20101]
      Executed DbCommand (92ms) [Parameters=[@p0='?' (DbType = DateTime2), @p1='?' (Size = 1000), @p2='?' (Size = 100), @p3='?' (DbType = Int32), @p4='?' (DbType = DateTime2), @p5='?' (Size = 1000), @p6='?' (Size = 100), @p7='?' (DbType = Int32)], CommandType='Text', CommandTimeout='30']
      SET IMPLICIT_TRANSACTIONS OFF;
      SET NOCOUNT ON;
      MERGE [SystemLog] USING (
      VALUES (@p0, @p1, @p2, @p3, 0),
      (@p4, @p5, @p6, @p7, 1)) AS i ([CreatedAt], [EventDescription], [EventType], [UserId], _Position) ON 1=0
      WHEN NOT MATCHED THEN
      INSERT ([CreatedAt], [EventDescription], [EventType], [UserId])
      VALUES (i.[CreatedAt], i.[EventDescription], i.[EventType], i.[UserId])
      OUTPUT INSERTED.[SystemLogId], i._Position;
info: Microsoft.AspNetCore.Routing.EndpointMiddleware[0]
      Executing endpoint 'WebApplication.Controllers.HomeController.Index (WebApplication)'
info: Microsoft.AspNetCore.Mvc.Infrastructure.ControllerActionInvoker[102]
      Route matched with {action = "Index", controller = "Home"}. Executing controller action with signature System.Threading.Tasks.Task`1[Microsoft.AspNetCore.Mvc.IActionResult] Index(System.Threading.CancellationToken) on controller WebApplication.Controllers.HomeController (WebApplication).
info: Microsoft.EntityFrameworkCore.Database.Command[20101]
      Executed DbCommand (61ms) [Parameters=[@p0='?' (DbType = DateTime2), @p1='?' (Size = 1000), @p2='?' (Size = 100), @p3='?' (DbType = Int32)], CommandType='Text', CommandTimeout='30']
      SET IMPLICIT_TRANSACTIONS OFF;
      SET NOCOUNT ON;
      INSERT INTO [SystemLog] ([CreatedAt], [EventDescription], [EventType], [UserId])
      OUTPUT INSERTED.[SystemLogId]
      VALUES (@p0, @p1, @p2, @p3);
info: Microsoft.EntityFrameworkCore.Database.Command[20101]
      Executed DbCommand (62ms) [Parameters=[@p0='?' (DbType = DateTime2), @p1='?' (Size = 1000), @p2='?' (Size = 100), @p3='?' (DbType = Int32)], CommandType='Text', CommandTimeout='30']
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
      Executed DbCommand (60ms) [Parameters=[@__p_0='?' (DbType = Int32)], CommandType='Text', CommandTimeout='30']
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
      Executed DbCommand (58ms) [Parameters=[@p0='?' (DbType = DateTime2), @p1='?' (Size = 1000), @p2='?' (Size = 100), @p3='?' (DbType = Int32)], CommandType='Text', CommandTimeout='30']
      SET IMPLICIT_TRANSACTIONS OFF;
      SET NOCOUNT ON;
      INSERT INTO [SystemLog] ([CreatedAt], [EventDescription], [EventType], [UserId])
      OUTPUT INSERTED.[SystemLogId]
      VALUES (@p0, @p1, @p2, @p3);
info: Microsoft.EntityFrameworkCore.Database.Command[20101]
      Executed DbCommand (60ms) [Parameters=[@__p_0='?' (DbType = Int32)], CommandType='Text', CommandTimeout='30']
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
      Executed DbCommand (59ms) [Parameters=[@__p_0='?' (DbType = Int32)], CommandType='Text', CommandTimeout='30']
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
      Executed DbCommand (56ms) [Parameters=[@__p_0='?' (DbType = Int32)], CommandType='Text', CommandTimeout='30']
      SELECT TOP(@__p_0) [n].[NotificationId], [n].[Body], [n].[Channel], [n].[CreatedAt], [n].[FailureReason], [n].[IsRead], [n].[NotifType], [n].[OrderId], [n].[ReadAt], [n].[Recipient], [n].[RetryCount], [n].[SentAt], [n].[Status], [n].[Subject], [n].[TicketId], [n].[UserId]
      FROM [Notification] AS [n]
      WHERE [n].[Status] = N'Pending' AND [n].[RetryCount] < 3 AND [n].[Channel] = N'Email'
      ORDER BY [n].[CreatedAt]
info: Microsoft.EntityFrameworkCore.Database.Command[20101]
      Executed DbCommand (58ms) [Parameters=[@p0='?' (DbType = DateTime2), @p1='?' (Size = 1000), @p2='?' (Size = 100), @p3='?' (DbType = Int32)], CommandType='Text', CommandTimeout='30']
      SET IMPLICIT_TRANSACTIONS OFF;
      SET NOCOUNT ON;
      INSERT INTO [SystemLog] ([CreatedAt], [EventDescription], [EventType], [UserId])
      OUTPUT INSERTED.[SystemLogId]
      VALUES (@p0, @p1, @p2, @p3);
info: Microsoft.EntityFrameworkCore.Database.Command[20101]
      Executed DbCommand (60ms) [Parameters=[@p0='?' (DbType = DateTime2), @p1='?' (Size = 1000), @p2='?' (Size = 100), @p3='?' (DbType = Int32)], CommandType='Text', CommandTimeout='30']
      SET IMPLICIT_TRANSACTIONS OFF;
      SET NOCOUNT ON;
      INSERT INTO [SystemLog] ([CreatedAt], [EventDescription], [EventType], [UserId])
      OUTPUT INSERTED.[SystemLogId]
      VALUES (@p0, @p1, @p2, @p3);
info: Microsoft.EntityFrameworkCore.Database.Command[20101]
      Executed DbCommand (59ms) [Parameters=[], CommandType='Text', CommandTimeout='30']
      SELECT [p].[ProductVariantId], [p].[VariantName], [p].[StockQuantity], [p].[ReorderThreshold], [p].[ProductId], [p0].[Name] AS [ProductName]
      FROM [ProductVariant] AS [p]
      INNER JOIN [Product] AS [p0] ON [p].[ProductId] = [p0].[ProductId]
      WHERE [p].[IsActive] = CAST(1 AS bit) AND [p0].[IsActive] = CAST(1 AS bit) AND [p].[StockQuantity] < [p].[ReorderThreshold]
info: Microsoft.AspNetCore.Hosting.Diagnostics[1]
      Request starting HTTP/2 GET https://localhost:7177/css/output.css?v=CXRbrZ1s69tJ8KRR4cAcYi6LIRaK_lqzLjiWJmykSb8 - - -
info: Microsoft.AspNetCore.Hosting.Diagnostics[1]
      Request starting HTTP/2 GET https://localhost:7177/_framework/aspnetcore-browser-refresh.js - - -
info: Microsoft.AspNetCore.StaticFiles.StaticFileMiddleware[6]
      The file /css/output.css was not modified
info: Microsoft.EntityFrameworkCore.Database.Command[20101]
      Executed DbCommand (58ms) [Parameters=[], CommandType='Text', CommandTimeout='30']
      SELECT [d].[DeliveryId], [d].[ActualDeliveryTime], [d].[Courier], [d].[CreatedAt], [d].[DelayedUntil], [d].[DeliveryStatus], [d].[EstimatedDeliveryTime], [d].[IsDelayed], [d].[OrderId], [l].[DeliveryId], [l].[BookingRef], [l].[DriverName], [l].[DriverPhone], [l0].[DeliveryId], [l0].[TrackingNumber], [t].[OrderId], [t].[CartId], [t].[ContactPhone], [t].[CreatedAt], [t].[DeliveryInstructions], [t].[DiscountAmount], [t].[FulfillmentType], [t].[GuestSessionId], [t].[IsDeleted], [t].[IsWalkIn], [t].[OrderDate], [t].[OrderNumber], [t].[OrderStatus], [t].[POSSessionId], [t].[ShippingAddressId], [t].[ShippingFee], [t].[SubTotal], [t].[TotalAmount], [t].[UpdatedAt], [t].[UserId], [t0].[UserId], [t0].[CreatedAt], [t0].[DefaultAddressId], [t0].[Email], [t0].[FailedLoginAttempts], [t0].[FirstName], [t0].[IsActive], [t0].[IsDeleted], [t0].[IsWalkIn], [t0].[LastLoginAt], [t0].[LastName], [t0].[LockoutUntil], [t0].[PasswordHash], [t0].[PhoneNumber]
      FROM [Delivery] AS [d]
      LEFT JOIN [LalamoveDelivery] AS [l] ON [d].[DeliveryId] = [l].[DeliveryId]
      LEFT JOIN [LBCDelivery] AS [l0] ON [d].[DeliveryId] = [l0].[DeliveryId]
      INNER JOIN (
          SELECT [o].[OrderId], [o].[CartId], [o].[ContactPhone], [o].[CreatedAt], [o].[DeliveryInstructions], [o].[DiscountAmount], [o].[FulfillmentType], [o].[GuestSessionId], [o].[IsDeleted], [o].[IsWalkIn], [o].[OrderDate], [o].[OrderNumber], [o].[OrderStatus], [o].[POSSessionId], [o].[ShippingAddressId], [o].[ShippingFee], [o].[SubTotal], [o].[TotalAmount], [o].[UpdatedAt], [o].[UserId]
          FROM [Order] AS [o]
          WHERE [o].[IsDeleted] = CAST(0 AS bit)
      ) AS [t] ON [d].[OrderId] = [t].[OrderId]
      INNER JOIN (
          SELECT [u].[UserId], [u].[CreatedAt], [u].[DefaultAddressId], [u].[Email], [u].[FailedLoginAttempts], [u].[FirstName], [u].[IsActive], [u].[IsDeleted], [u].[IsWalkIn], [u].[LastLoginAt], [u].[LastName], [u].[LockoutUntil], [u].[PasswordHash], [u].[PhoneNumber]
          FROM [User] AS [u]
          WHERE [u].[IsDeleted] = CAST(0 AS bit)
      ) AS [t0] ON [t].[UserId] = [t0].[UserId]
      WHERE [d].[DeliveryStatus] IN (N'PickedUp', N'InTransit')
info: Microsoft.AspNetCore.Mvc.ViewFeatures.ViewResultExecutor[4]
      Executed ViewResult - view Index executed in 142.7889ms.
info: Microsoft.AspNetCore.Hosting.Diagnostics[2]
      Request finished HTTP/2 GET https://localhost:7177/css/output.css?v=CXRbrZ1s69tJ8KRR4cAcYi6LIRaK_lqzLjiWJmykSb8 - 304 - text/css 19.7263ms
info: Microsoft.AspNetCore.Hosting.Diagnostics[1]
      Request starting HTTP/2 GET https://localhost:7177/_vs/browserLink - - -
info: Microsoft.AspNetCore.Hosting.Diagnostics[2]
      Request finished HTTP/2 GET https://localhost:7177/_framework/aspnetcore-browser-refresh.js - 200 14933 application/javascript;+charset=utf-8 15.6573ms
info: Microsoft.AspNetCore.Mvc.Infrastructure.ControllerActionInvoker[105]
      Executed action WebApplication.Controllers.HomeController.Index (WebApplication) in 467.4774ms
info: Microsoft.AspNetCore.Routing.EndpointMiddleware[1]
      Executed endpoint 'WebApplication.Controllers.HomeController.Index (WebApplication)'
info: Microsoft.AspNetCore.Hosting.Diagnostics[2]
      Request finished HTTP/2 GET https://localhost:7177/ - 200 - text/html;+charset=utf-8 571.6448ms
info: Microsoft.AspNetCore.Hosting.Diagnostics[2]
      Request finished HTTP/2 GET https://localhost:7177/_vs/browserLink - 200 - text/javascript;+charset=UTF-8 37.6855ms
info: Microsoft.EntityFrameworkCore.Database.Command[20101]
      Executed DbCommand (59ms) [Parameters=[], CommandType='Text', CommandTimeout='30']
      SELECT [u].[UserId], [u].[Email], [u].[FirstName]
      FROM [User] AS [u]
      WHERE [u].[IsDeleted] = CAST(0 AS bit) AND [u].[IsActive] = CAST(1 AS bit) AND [u].[IsWalkIn] = CAST(0 AS bit) AND [u].[Email] IS NOT NULL AND EXISTS (
          SELECT 1
          FROM [UserRole] AS [u0]
          INNER JOIN [Role] AS [r] ON [u0].[RoleId] = [r].[RoleId]
          WHERE [u].[UserId] = [u0].[UserId] AND [r].[RoleName] IN (N'Admin', N'Manager'))
info: Microsoft.EntityFrameworkCore.Database.Command[20101]
      Executed DbCommand (57ms) [Parameters=[@p0='?' (DbType = DateTime2), @p1='?' (Size = 1000), @p2='?' (Size = 100), @p3='?' (DbType = Int32)], CommandType='Text', CommandTimeout='30']
      SET IMPLICIT_TRANSACTIONS OFF;
      SET NOCOUNT ON;
      INSERT INTO [SystemLog] ([CreatedAt], [EventDescription], [EventType], [UserId])
      OUTPUT INSERTED.[SystemLogId]
      VALUES (@p0, @p1, @p2, @p3);
warn: WebApplication.BackgroundJobs.StockMonitorJob[0]
      StockMonitorJob: low stock - variant 1 (2021 Pinewood Climber CARBON 27.5 / Default): 4 remaining (threshold 5).
info: Microsoft.AspNetCore.Hosting.Diagnostics[1]
      Request starting HTTP/2 GET https://localhost:7177/Cart/GetCartCount - - -
info: Microsoft.AspNetCore.Hosting.Diagnostics[1]
      Request starting HTTP/2 GET https://localhost:7177/antiforgery/token - - -
info: Microsoft.AspNetCore.Hosting.Diagnostics[1]
      Request starting HTTP/2 GET https://localhost:7177/Customer/NotificationCount - - -
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
info: Microsoft.AspNetCore.Mvc.Infrastructure.ControllerActionInvoker[102]
      Route matched with {action = "NotificationCount", controller = "Customer"}. Executing controller action with signature System.Threading.Tasks.Task`1[Microsoft.AspNetCore.Mvc.IActionResult] NotificationCount(System.Threading.CancellationToken) on controller WebApplication.Controllers.CustomerController (WebApplication).
info: Microsoft.AspNetCore.Http.Result.OkObjectResult[3]
      Writing value of type '<>f__AnonymousType20`1' as Json.
info: Microsoft.AspNetCore.Routing.EndpointMiddleware[1]
      Executed endpoint 'HTTP: GET /antiforgery/token'
info: Microsoft.AspNetCore.Hosting.Diagnostics[2]
      Request finished HTTP/2 GET https://localhost:7177/antiforgery/token - 200 - application/json;+charset=utf-8 46.4100ms
info: Microsoft.EntityFrameworkCore.Database.Command[20101]
      Executed DbCommand (56ms) [Parameters=[@__8__locals1_cooldownStart_0='?' (DbType = DateTime2), @__Format_2='?' (Size = 1000)], CommandType='Text', CommandTimeout='30']
      SELECT CASE
          WHEN EXISTS (
              SELECT 1
              FROM [SystemLog] AS [s]
              WHERE [s].[EventType] = N'LowStockTriggered' AND [s].[CreatedAt] > @__8__locals1_cooldownStart_0 AND [s].[EventDescription] LIKE @__Format_2) THEN CAST(1 AS bit)
          ELSE CAST(0 AS bit)
      END
info: Microsoft.EntityFrameworkCore.Database.Command[20101]
      Executed DbCommand (59ms) [Parameters=[@__userId_0='?' (DbType = Int32)], CommandType='Text', CommandTimeout='30']
      SELECT COUNT(*)
      FROM [Notification] AS [n]
      WHERE [n].[UserId] = @__userId_0 AND [n].[IsRead] = CAST(0 AS bit)
info: Microsoft.AspNetCore.Mvc.Infrastructure.SystemTextJsonResultExecutor[1]
      Executing JsonResult, writing value of type '<>f__AnonymousType6`1[[System.Int32, System.Private.CoreLib, Version=8.0.0.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e]]'.
info: Microsoft.EntityFrameworkCore.Database.Command[20101]
      Executed DbCommand (58ms) [Parameters=[@__now_0='?' (DbType = DateTime2)], CommandType='Text', CommandTimeout='30']
      SELECT [o].[OrderId], [o].[CartId], [o].[ContactPhone], [o].[CreatedAt], [o].[DeliveryInstructions], [o].[DiscountAmount], [o].[FulfillmentType], [o].[GuestSessionId], [o].[IsDeleted], [o].[IsWalkIn], [o].[OrderDate], [o].[OrderNumber], [o].[OrderStatus], [o].[POSSessionId], [o].[ShippingAddressId], [o].[ShippingFee], [o].[SubTotal], [o].[TotalAmount], [o].[UpdatedAt], [o].[UserId], [t].[UserId], [t].[CreatedAt], [t].[DefaultAddressId], [t].[Email], [t].[FailedLoginAttempts], [t].[FirstName], [t].[IsActive], [t].[IsDeleted], [t].[IsWalkIn], [t].[LastLoginAt], [t].[LastName], [t].[LockoutUntil], [t].[PasswordHash], [t].[PhoneNumber]
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
info: Microsoft.AspNetCore.Mvc.Infrastructure.ControllerActionInvoker[105]
      Executed action WebApplication.Controllers.CustomerController.NotificationCount (WebApplication) in 80.9603ms
info: Microsoft.AspNetCore.Routing.EndpointMiddleware[1]
      Executed endpoint 'WebApplication.Controllers.CustomerController.NotificationCount (WebApplication)'
info: Microsoft.AspNetCore.Hosting.Diagnostics[2]
      Request finished HTTP/2 GET https://localhost:7177/Customer/NotificationCount - 200 - application/json;+charset=utf-8 122.3206ms
info: Microsoft.EntityFrameworkCore.Database.Command[20101]
      Executed DbCommand (57ms) [Parameters=[@__userId_Value_0='?' (DbType = Int32)], CommandType='Text', CommandTimeout='30']
      SELECT TOP(1) [c].[CartId], [c].[CreatedAt], [c].[GuestSessionId], [c].[IsCheckedOut], [c].[LastUpdatedAt], [c].[UserId]
      FROM [Cart] AS [c]
      WHERE [c].[IsCheckedOut] = CAST(0 AS bit) AND [c].[UserId] = @__userId_Value_0
      ORDER BY [c].[CartId]
info: Microsoft.AspNetCore.Mvc.Infrastructure.SystemTextJsonResultExecutor[1]
      Executing JsonResult, writing value of type 'WebApplication.Models.ApiResponse'.
info: Microsoft.AspNetCore.Mvc.Infrastructure.ControllerActionInvoker[105]
      Executed action WebApplication.Controllers.CartController.GetCartCount (WebApplication) in 121.469ms
info: Microsoft.AspNetCore.Routing.EndpointMiddleware[1]
      Executed endpoint 'WebApplication.Controllers.CartController.GetCartCount (WebApplication)'
info: Microsoft.AspNetCore.Hosting.Diagnostics[2]
      Request finished HTTP/2 GET https://localhost:7177/Cart/GetCartCount - 200 - application/json;+charset=utf-8 149.2584ms
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
      WHERE [n].[UserId] = @__userId_0
info: Microsoft.EntityFrameworkCore.Database.Command[20101]
      Executed DbCommand (57ms) [Parameters=[@__userId_0='?' (DbType = Int32), @__p_1='?' (DbType = Int32), @__p_2='?' (DbType = Int32)], CommandType='Text', CommandTimeout='30']
      SELECT [n].[NotificationId], [n].[Body], [n].[Channel], [n].[CreatedAt], [n].[FailureReason], [n].[IsRead], [n].[NotifType], [n].[OrderId], [n].[ReadAt], [n].[Recipient], [n].[RetryCount], [n].[SentAt], [n].[Status], [n].[Subject], [n].[TicketId], [n].[UserId]
      FROM [Notification] AS [n]
      WHERE [n].[UserId] = @__userId_0
      ORDER BY [n].[CreatedAt] DESC
      OFFSET @__p_1 ROWS FETCH NEXT @__p_2 ROWS ONLY
info: Microsoft.EntityFrameworkCore.Database.Command[20101]
      Executed DbCommand (57ms) [Parameters=[@__userId_0='?' (DbType = Int32)], CommandType='Text', CommandTimeout='30']
      SELECT COUNT(*)
      FROM [Notification] AS [n]
      WHERE [n].[UserId] = @__userId_0 AND [n].[IsRead] = CAST(0 AS bit)
info: Microsoft.AspNetCore.Mvc.ViewFeatures.ViewResultExecutor[1]
      Executing ViewResult, running view Notifications.
info: Microsoft.AspNetCore.Mvc.ViewFeatures.ViewResultExecutor[4]
      Executed ViewResult - view Notifications executed in 18.5764ms.
info: Microsoft.AspNetCore.Mvc.Infrastructure.ControllerActionInvoker[105]
      Executed action WebApplication.Controllers.CustomerController.Notifications (WebApplication) in 216.3433ms
info: Microsoft.AspNetCore.Routing.EndpointMiddleware[1]
      Executed endpoint 'WebApplication.Controllers.CustomerController.Notifications (WebApplication)'
info: Microsoft.AspNetCore.Hosting.Diagnostics[2]
      Request finished HTTP/2 GET https://localhost:7177/Customer/Notifications - 200 - text/html;+charset=utf-8 235.7676ms
info: Microsoft.AspNetCore.Hosting.Diagnostics[1]
      Request starting HTTP/2 GET https://localhost:7177/_framework/aspnetcore-browser-refresh.js - - -
info: Microsoft.AspNetCore.Hosting.Diagnostics[1]
      Request starting HTTP/2 GET https://localhost:7177/_vs/browserLink - - -
info: Microsoft.AspNetCore.Hosting.Diagnostics[2]
      Request finished HTTP/2 GET https://localhost:7177/_framework/aspnetcore-browser-refresh.js - 200 14933 application/javascript;+charset=utf-8 4.0104ms
info: Microsoft.AspNetCore.Hosting.Diagnostics[1]
      Request starting HTTP/2 GET https://localhost:7177/Customer/NotificationCount - - -
info: Microsoft.AspNetCore.Hosting.Diagnostics[1]
      Request starting HTTP/2 GET https://localhost:7177/Cart/GetCartCount - - -
info: Microsoft.AspNetCore.Hosting.Diagnostics[2]
      Request finished HTTP/2 GET https://localhost:7177/_vs/browserLink - 200 - text/javascript;+charset=UTF-8 22.6175ms
info: Microsoft.AspNetCore.Routing.EndpointMiddleware[0]
      Executing endpoint 'WebApplication.Controllers.CartController.GetCartCount (WebApplication)'
info: Microsoft.AspNetCore.Mvc.Infrastructure.ControllerActionInvoker[102]
      Route matched with {action = "GetCartCount", controller = "Cart"}. Executing controller action with signature System.Threading.Tasks.Task`1[Microsoft.AspNetCore.Mvc.IActionResult] GetCartCount(System.Threading.CancellationToken) on controller WebApplication.Controllers.CartController (WebApplication).
info: Microsoft.AspNetCore.Routing.EndpointMiddleware[0]
      Executing endpoint 'WebApplication.Controllers.CustomerController.NotificationCount (WebApplication)'
info: Microsoft.AspNetCore.Mvc.Infrastructure.ControllerActionInvoker[102]
      Route matched with {action = "NotificationCount", controller = "Customer"}. Executing controller action with signature System.Threading.Tasks.Task`1[Microsoft.AspNetCore.Mvc.IActionResult] NotificationCount(System.Threading.CancellationToken) on controller WebApplication.Controllers.CustomerController (WebApplication).
info: Microsoft.EntityFrameworkCore.Database.Command[20101]
      Executed DbCommand (59ms) [Parameters=[@__userId_Value_0='?' (DbType = Int32)], CommandType='Text', CommandTimeout='30']
      SELECT TOP(1) [c].[CartId], [c].[CreatedAt], [c].[GuestSessionId], [c].[IsCheckedOut], [c].[LastUpdatedAt], [c].[UserId]
      FROM [Cart] AS [c]
      WHERE [c].[IsCheckedOut] = CAST(0 AS bit) AND [c].[UserId] = @__userId_Value_0
      ORDER BY [c].[CartId]
info: Microsoft.AspNetCore.Mvc.Infrastructure.SystemTextJsonResultExecutor[1]
      Executing JsonResult, writing value of type 'WebApplication.Models.ApiResponse'.
info: Microsoft.AspNetCore.Mvc.Infrastructure.ControllerActionInvoker[105]
      Executed action WebApplication.Controllers.CartController.GetCartCount (WebApplication) in 68.0798ms
info: Microsoft.AspNetCore.Routing.EndpointMiddleware[1]
      Executed endpoint 'WebApplication.Controllers.CartController.GetCartCount (WebApplication)'
info: Microsoft.AspNetCore.Hosting.Diagnostics[2]
      Request finished HTTP/2 GET https://localhost:7177/Cart/GetCartCount - 200 - application/json;+charset=utf-8 80.0627ms
info: Microsoft.EntityFrameworkCore.Database.Command[20101]
      Executed DbCommand (57ms) [Parameters=[@__userId_0='?' (DbType = Int32)], CommandType='Text', CommandTimeout='30']
      SELECT COUNT(*)
      FROM [Notification] AS [n]
      WHERE [n].[UserId] = @__userId_0 AND [n].[IsRead] = CAST(0 AS bit)
info: Microsoft.AspNetCore.Mvc.Infrastructure.SystemTextJsonResultExecutor[1]
      Executing JsonResult, writing value of type '<>f__AnonymousType6`1[[System.Int32, System.Private.CoreLib, Version=8.0.0.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e]]'.
info: Microsoft.AspNetCore.Mvc.Infrastructure.ControllerActionInvoker[105]
      Executed action WebApplication.Controllers.CustomerController.NotificationCount (WebApplication) in 72.4132ms
info: Microsoft.AspNetCore.Routing.EndpointMiddleware[1]
      Executed endpoint 'WebApplication.Controllers.CustomerController.NotificationCount (WebApplication)'
info: Microsoft.AspNetCore.Hosting.Diagnostics[2]
      Request finished HTTP/2 GET https://localhost:7177/Customer/NotificationCount - 200 - application/json;+charset=utf-8 87.4061ms
info: Microsoft.AspNetCore.Hosting.Diagnostics[1]
      Request starting HTTP/2 POST https://localhost:7177/Customer/MarkAllNotificationsRead - application/x-www-form-urlencoded 225
info: Microsoft.AspNetCore.Cors.Infrastructure.CorsService[4]
      CORS policy execution successful.
info: Microsoft.AspNetCore.Routing.EndpointMiddleware[0]
      Executing endpoint 'WebApplication.Controllers.CustomerController.MarkAllNotificationsRead (WebApplication)'
info: Microsoft.AspNetCore.Mvc.Infrastructure.ControllerActionInvoker[102]
      Route matched with {action = "MarkAllNotificationsRead", controller = "Customer"}. Executing controller action with signature System.Threading.Tasks.Task`1[Microsoft.AspNetCore.Mvc.IActionResult] MarkAllNotificationsRead(System.Threading.CancellationToken) on controller WebApplication.Controllers.CustomerController (WebApplication).
info: Microsoft.EntityFrameworkCore.Database.Command[20101]
      Executed DbCommand (71ms) [Parameters=[@__userId_0='?' (DbType = Int32)], CommandType='Text', CommandTimeout='30']
      UPDATE [n]
      SET [n].[ReadAt] = GETUTCDATE(),
          [n].[IsRead] = CAST(1 AS bit)
      FROM [Notification] AS [n]
      WHERE [n].[UserId] = @__userId_0 AND [n].[IsRead] = CAST(0 AS bit)
info: Microsoft.AspNetCore.Mvc.RedirectToActionResult[1]
      Executing RedirectResult, redirecting to /Customer/Notifications.
info: Microsoft.AspNetCore.Mvc.Infrastructure.ControllerActionInvoker[105]
      Executed action WebApplication.Controllers.CustomerController.MarkAllNotificationsRead (WebApplication) in 97.561ms
info: Microsoft.AspNetCore.Routing.EndpointMiddleware[1]
      Executed endpoint 'WebApplication.Controllers.CustomerController.MarkAllNotificationsRead (WebApplication)'
info: Microsoft.AspNetCore.Hosting.Diagnostics[2]
      Request finished HTTP/2 POST https://localhost:7177/Customer/MarkAllNotificationsRead - 302 - - 114.5017ms
info: Microsoft.AspNetCore.Hosting.Diagnostics[1]
      Request starting HTTP/2 GET https://localhost:7177/Customer/Notifications - - -
info: Microsoft.AspNetCore.Routing.EndpointMiddleware[0]
      Executing endpoint 'WebApplication.Controllers.CustomerController.Notifications (WebApplication)'
info: Microsoft.AspNetCore.Mvc.Infrastructure.ControllerActionInvoker[102]
      Route matched with {action = "Notifications", controller = "Customer"}. Executing controller action with signature System.Threading.Tasks.Task`1[Microsoft.AspNetCore.Mvc.IActionResult] Notifications(Int32, System.Threading.CancellationToken) on controller WebApplication.Controllers.CustomerController (WebApplication).
info: Microsoft.EntityFrameworkCore.Database.Command[20101]
      Executed DbCommand (55ms) [Parameters=[@__userId_0='?' (DbType = Int32)], CommandType='Text', CommandTimeout='30']
      SELECT COUNT(*)
      FROM [Notification] AS [n]
      WHERE [n].[UserId] = @__userId_0
info: Microsoft.EntityFrameworkCore.Database.Command[20101]
      Executed DbCommand (56ms) [Parameters=[@__userId_0='?' (DbType = Int32), @__p_1='?' (DbType = Int32), @__p_2='?' (DbType = Int32)], CommandType='Text', CommandTimeout='30']
      SELECT [n].[NotificationId], [n].[Body], [n].[Channel], [n].[CreatedAt], [n].[FailureReason], [n].[IsRead], [n].[NotifType], [n].[OrderId], [n].[ReadAt], [n].[Recipient], [n].[RetryCount], [n].[SentAt], [n].[Status], [n].[Subject], [n].[TicketId], [n].[UserId]
      FROM [Notification] AS [n]
      WHERE [n].[UserId] = @__userId_0
      ORDER BY [n].[CreatedAt] DESC
      OFFSET @__p_1 ROWS FETCH NEXT @__p_2 ROWS ONLY
info: Microsoft.EntityFrameworkCore.Database.Command[20101]
      Executed DbCommand (55ms) [Parameters=[@__userId_0='?' (DbType = Int32)], CommandType='Text', CommandTimeout='30']
      SELECT COUNT(*)
      FROM [Notification] AS [n]
      WHERE [n].[UserId] = @__userId_0 AND [n].[IsRead] = CAST(0 AS bit)
info: Microsoft.AspNetCore.Mvc.ViewFeatures.ViewResultExecutor[1]
      Executing ViewResult, running view Notifications.
info: Microsoft.AspNetCore.Mvc.ViewFeatures.ViewResultExecutor[4]
      Executed ViewResult - view Notifications executed in 6.8784ms.
info: Microsoft.AspNetCore.Mvc.Infrastructure.ControllerActionInvoker[105]
      Executed action WebApplication.Controllers.CustomerController.Notifications (WebApplication) in 183.402ms
info: Microsoft.AspNetCore.Routing.EndpointMiddleware[1]
      Executed endpoint 'WebApplication.Controllers.CustomerController.Notifications (WebApplication)'
info: Microsoft.AspNetCore.Hosting.Diagnostics[2]
      Request finished HTTP/2 GET https://localhost:7177/Customer/Notifications - 200 - text/html;+charset=utf-8 192.7007ms
info: Microsoft.AspNetCore.Hosting.Diagnostics[1]
      Request starting HTTP/2 GET https://localhost:7177/_vs/browserLink - - -
info: Microsoft.AspNetCore.Hosting.Diagnostics[1]
      Request starting HTTP/2 GET https://localhost:7177/_framework/aspnetcore-browser-refresh.js - - -
info: Microsoft.AspNetCore.Hosting.Diagnostics[2]
      Request finished HTTP/2 GET https://localhost:7177/_framework/aspnetcore-browser-refresh.js - 200 14933 application/javascript;+charset=utf-8 3.7153ms
info: Microsoft.AspNetCore.Hosting.Diagnostics[1]
      Request starting HTTP/2 GET https://localhost:7177/Customer/NotificationCount - - -
info: Microsoft.AspNetCore.Hosting.Diagnostics[1]
      Request starting HTTP/2 GET https://localhost:7177/antiforgery/token - - -
info: Microsoft.AspNetCore.Hosting.Diagnostics[1]
      Request starting HTTP/2 GET https://localhost:7177/Cart/GetCartCount - - -
info: Microsoft.AspNetCore.Routing.EndpointMiddleware[0]
      Executing endpoint 'WebApplication.Controllers.CustomerController.NotificationCount (WebApplication)'
info: Microsoft.AspNetCore.Routing.EndpointMiddleware[0]
      Executing endpoint 'WebApplication.Controllers.CartController.GetCartCount (WebApplication)'
info: Microsoft.AspNetCore.Routing.EndpointMiddleware[0]
      Executing endpoint 'HTTP: GET /antiforgery/token'
info: Microsoft.AspNetCore.Mvc.Infrastructure.ControllerActionInvoker[102]
      Route matched with {action = "GetCartCount", controller = "Cart"}. Executing controller action with signature System.Threading.Tasks.Task`1[Microsoft.AspNetCore.Mvc.IActionResult] GetCartCount(System.Threading.CancellationToken) on controller WebApplication.Controllers.CartController (WebApplication).
info: Microsoft.AspNetCore.Http.Result.OkObjectResult[1]
      Setting HTTP status code 200.
info: Microsoft.AspNetCore.Mvc.Infrastructure.ControllerActionInvoker[102]
      Route matched with {action = "NotificationCount", controller = "Customer"}. Executing controller action with signature System.Threading.Tasks.Task`1[Microsoft.AspNetCore.Mvc.IActionResult] NotificationCount(System.Threading.CancellationToken) on controller WebApplication.Controllers.CustomerController (WebApplication).
info: Microsoft.AspNetCore.Http.Result.OkObjectResult[3]
      Writing value of type '<>f__AnonymousType20`1' as Json.
info: Microsoft.AspNetCore.Hosting.Diagnostics[2]
      Request finished HTTP/2 GET https://localhost:7177/_vs/browserLink - 200 - text/javascript;+charset=UTF-8 30.8099ms
info: Microsoft.AspNetCore.Routing.EndpointMiddleware[1]
      Executed endpoint 'HTTP: GET /antiforgery/token'
info: Microsoft.AspNetCore.Hosting.Diagnostics[2]
      Request finished HTTP/2 GET https://localhost:7177/antiforgery/token - 200 - application/json;+charset=utf-8 20.6271ms
info: Microsoft.EntityFrameworkCore.Database.Command[20101]
      Executed DbCommand (56ms) [Parameters=[@__userId_0='?' (DbType = Int32)], CommandType='Text', CommandTimeout='30']
      SELECT COUNT(*)
      FROM [Notification] AS [n]
      WHERE [n].[UserId] = @__userId_0 AND [n].[IsRead] = CAST(0 AS bit)
info: Microsoft.AspNetCore.Mvc.Infrastructure.SystemTextJsonResultExecutor[1]
      Executing JsonResult, writing value of type '<>f__AnonymousType6`1[[System.Int32, System.Private.CoreLib, Version=8.0.0.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e]]'.
info: Microsoft.AspNetCore.Mvc.Infrastructure.ControllerActionInvoker[105]
      Executed action WebApplication.Controllers.CustomerController.NotificationCount (WebApplication) in 63.4057ms
info: Microsoft.EntityFrameworkCore.Database.Command[20101]
      Executed DbCommand (57ms) [Parameters=[@__userId_Value_0='?' (DbType = Int32)], CommandType='Text', CommandTimeout='30']
      SELECT TOP(1) [c].[CartId], [c].[CreatedAt], [c].[GuestSessionId], [c].[IsCheckedOut], [c].[LastUpdatedAt], [c].[UserId]
      FROM [Cart] AS [c]
      WHERE [c].[IsCheckedOut] = CAST(0 AS bit) AND [c].[UserId] = @__userId_Value_0
      ORDER BY [c].[CartId]
info: Microsoft.AspNetCore.Routing.EndpointMiddleware[1]
      Executed endpoint 'WebApplication.Controllers.CustomerController.NotificationCount (WebApplication)'
info: Microsoft.AspNetCore.Mvc.Infrastructure.SystemTextJsonResultExecutor[1]
      Executing JsonResult, writing value of type 'WebApplication.Models.ApiResponse'.
info: Microsoft.AspNetCore.Hosting.Diagnostics[2]
      Request finished HTTP/2 GET https://localhost:7177/Customer/NotificationCount - 200 - application/json;+charset=utf-8 82.9116ms
info: Microsoft.AspNetCore.Mvc.Infrastructure.ControllerActionInvoker[105]
      Executed action WebApplication.Controllers.CartController.GetCartCount (WebApplication) in 74.7298ms
info: Microsoft.AspNetCore.Routing.EndpointMiddleware[1]
      Executed endpoint 'WebApplication.Controllers.CartController.GetCartCount (WebApplication)'
info: Microsoft.AspNetCore.Hosting.Diagnostics[2]
      Request finished HTTP/2 GET https://localhost:7177/Cart/GetCartCount - 200 - application/json;+charset=utf-8 87.5294ms
info: Microsoft.EntityFrameworkCore.Database.Command[20101]
      Executed DbCommand (57ms) [Parameters=[], CommandType='Text', CommandTimeout='30']
      SELECT COUNT(*)
      FROM [ProductVariant] AS [p]
info: Microsoft.AspNetCore.Hosting.Diagnostics[1]
      Request starting HTTP/2 GET https://localhost:7177/Order/History - - -
info: Microsoft.AspNetCore.Routing.EndpointMiddleware[0]
      Executing endpoint 'WebApplication.Controllers.OrderController.History (WebApplication)'
info: Microsoft.AspNetCore.Mvc.Infrastructure.ControllerActionInvoker[102]
      Route matched with {action = "History", controller = "Order"}. Executing controller action with signature System.Threading.Tasks.Task`1[Microsoft.AspNetCore.Mvc.IActionResult] History(Int32, System.Threading.CancellationToken) on controller WebApplication.Controllers.OrderController (WebApplication).
info: Microsoft.EntityFrameworkCore.Database.Command[20101]
      Executed DbCommand (58ms) [Parameters=[@__userId_0='?' (DbType = Int32), @__p_1='?' (DbType = Int32), @__p_2='?' (DbType = Int32)], CommandType='Text', CommandTimeout='30']
      SELECT [t].[OrderId], [t].[CartId], [t].[ContactPhone], [t].[CreatedAt], [t].[DeliveryInstructions], [t].[DiscountAmount], [t].[FulfillmentType], [t].[GuestSessionId], [t].[IsDeleted], [t].[IsWalkIn], [t].[OrderDate], [t].[OrderNumber], [t].[OrderStatus], [t].[POSSessionId], [t].[ShippingAddressId], [t].[ShippingFee], [t].[SubTotal], [t].[TotalAmount], [t].[UpdatedAt], [t].[UserId], [p].[PickupOrderId], [p].[OrderId], [p].[PickupConfirmedAt], [p].[PickupExpiresAt], [p].[PickupReadyAt]
      FROM (
          SELECT [o].[OrderId], [o].[CartId], [o].[ContactPhone], [o].[CreatedAt], [o].[DeliveryInstructions], [o].[DiscountAmount], [o].[FulfillmentType], [o].[GuestSessionId], [o].[IsDeleted], [o].[IsWalkIn], [o].[OrderDate], [o].[OrderNumber], [o].[OrderStatus], [o].[POSSessionId], [o].[ShippingAddressId], [o].[ShippingFee], [o].[SubTotal], [o].[TotalAmount], [o].[UpdatedAt], [o].[UserId]
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
      Executed DbCommand (57ms) [Parameters=[@__userId_0='?' (DbType = Int32)], CommandType='Text', CommandTimeout='30']
      SELECT COUNT(*)
      FROM [Order] AS [o]
      WHERE [o].[IsDeleted] = CAST(0 AS bit) AND [o].[UserId] = @__userId_0
info: Microsoft.AspNetCore.Mvc.ViewFeatures.ViewResultExecutor[1]
      Executing ViewResult, running view ~/Views/Customer/OrderHistory.cshtml.
info: Microsoft.AspNetCore.Mvc.ViewFeatures.ViewResultExecutor[4]
      Executed ViewResult - view ~/Views/Customer/OrderHistory.cshtml executed in 13.9366ms.
info: Microsoft.AspNetCore.Mvc.Infrastructure.ControllerActionInvoker[105]
      Executed action WebApplication.Controllers.OrderController.History (WebApplication) in 238.9432ms
info: Microsoft.AspNetCore.Routing.EndpointMiddleware[1]
      Executed endpoint 'WebApplication.Controllers.OrderController.History (WebApplication)'
info: Microsoft.AspNetCore.Hosting.Diagnostics[2]
      Request finished HTTP/2 GET https://localhost:7177/Order/History - 200 - text/html;+charset=utf-8 251.5198ms
info: Microsoft.AspNetCore.Hosting.Diagnostics[1]
      Request starting HTTP/2 GET https://localhost:7177/_framework/aspnetcore-browser-refresh.js - - -
info: Microsoft.AspNetCore.Hosting.Diagnostics[1]
      Request starting HTTP/2 GET https://localhost:7177/_vs/browserLink - - -
info: Microsoft.AspNetCore.Hosting.Diagnostics[2]
      Request finished HTTP/2 GET https://localhost:7177/_framework/aspnetcore-browser-refresh.js - 200 14933 application/javascript;+charset=utf-8 3.6385ms
info: Microsoft.AspNetCore.Hosting.Diagnostics[2]
      Request finished HTTP/2 GET https://localhost:7177/_vs/browserLink - 200 - text/javascript;+charset=UTF-8 13.3027ms
info: Microsoft.AspNetCore.Hosting.Diagnostics[1]
      Request starting HTTP/2 GET https://localhost:7177/Cart/GetCartCount - - -
info: Microsoft.AspNetCore.Hosting.Diagnostics[1]
      Request starting HTTP/2 GET https://localhost:7177/antiforgery/token - - -
info: Microsoft.AspNetCore.Hosting.Diagnostics[1]
      Request starting HTTP/2 GET https://localhost:7177/Customer/NotificationCount - - -
info: Microsoft.AspNetCore.Routing.EndpointMiddleware[0]
      Executing endpoint 'WebApplication.Controllers.CustomerController.NotificationCount (WebApplication)'
info: Microsoft.AspNetCore.Routing.EndpointMiddleware[0]
      Executing endpoint 'WebApplication.Controllers.CartController.GetCartCount (WebApplication)'
info: Microsoft.AspNetCore.Routing.EndpointMiddleware[0]
      Executing endpoint 'HTTP: GET /antiforgery/token'
info: Microsoft.AspNetCore.Mvc.Infrastructure.ControllerActionInvoker[102]
      Route matched with {action = "NotificationCount", controller = "Customer"}. Executing controller action with signature System.Threading.Tasks.Task`1[Microsoft.AspNetCore.Mvc.IActionResult] NotificationCount(System.Threading.CancellationToken) on controller WebApplication.Controllers.CustomerController (WebApplication).
info: Microsoft.AspNetCore.Mvc.Infrastructure.ControllerActionInvoker[102]
      Route matched with {action = "GetCartCount", controller = "Cart"}. Executing controller action with signature System.Threading.Tasks.Task`1[Microsoft.AspNetCore.Mvc.IActionResult] GetCartCount(System.Threading.CancellationToken) on controller WebApplication.Controllers.CartController (WebApplication).
info: Microsoft.AspNetCore.Http.Result.OkObjectResult[1]
      Setting HTTP status code 200.
info: Microsoft.AspNetCore.Http.Result.OkObjectResult[3]
      Writing value of type '<>f__AnonymousType20`1' as Json.
info: Microsoft.AspNetCore.Routing.EndpointMiddleware[1]
      Executed endpoint 'HTTP: GET /antiforgery/token'
info: Microsoft.AspNetCore.Hosting.Diagnostics[2]
      Request finished HTTP/2 GET https://localhost:7177/antiforgery/token - 200 - application/json;+charset=utf-8 18.4233ms
info: Microsoft.EntityFrameworkCore.Database.Command[20101]
      Executed DbCommand (57ms) [Parameters=[@__userId_0='?' (DbType = Int32)], CommandType='Text', CommandTimeout='30']
      SELECT COUNT(*)
      FROM [Notification] AS [n]
      WHERE [n].[UserId] = @__userId_0 AND [n].[IsRead] = CAST(0 AS bit)
info: Microsoft.EntityFrameworkCore.Database.Command[20101]
      Executed DbCommand (56ms) [Parameters=[@__userId_Value_0='?' (DbType = Int32)], CommandType='Text', CommandTimeout='30']
      SELECT TOP(1) [c].[CartId], [c].[CreatedAt], [c].[GuestSessionId], [c].[IsCheckedOut], [c].[LastUpdatedAt], [c].[UserId]
      FROM [Cart] AS [c]
      WHERE [c].[IsCheckedOut] = CAST(0 AS bit) AND [c].[UserId] = @__userId_Value_0
      ORDER BY [c].[CartId]
info: Microsoft.AspNetCore.Mvc.Infrastructure.SystemTextJsonResultExecutor[1]
      Executing JsonResult, writing value of type '<>f__AnonymousType6`1[[System.Int32, System.Private.CoreLib, Version=8.0.0.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e]]'.
info: Microsoft.AspNetCore.Mvc.Infrastructure.SystemTextJsonResultExecutor[1]
      Executing JsonResult, writing value of type 'WebApplication.Models.ApiResponse'.
info: Microsoft.AspNetCore.Mvc.Infrastructure.ControllerActionInvoker[105]
      Executed action WebApplication.Controllers.CustomerController.NotificationCount (WebApplication) in 66.3318ms
info: Microsoft.AspNetCore.Mvc.Infrastructure.ControllerActionInvoker[105]
      Executed action WebApplication.Controllers.CartController.GetCartCount (WebApplication) in 65.9608ms
info: Microsoft.AspNetCore.Routing.EndpointMiddleware[1]
      Executed endpoint 'WebApplication.Controllers.CustomerController.NotificationCount (WebApplication)'
info: Microsoft.AspNetCore.Routing.EndpointMiddleware[1]
      Executed endpoint 'WebApplication.Controllers.CartController.GetCartCount (WebApplication)'
info: Microsoft.AspNetCore.Hosting.Diagnostics[2]
      Request finished HTTP/2 GET https://localhost:7177/Customer/NotificationCount - 200 - application/json;+charset=utf-8 80.3913ms
info: Microsoft.AspNetCore.Hosting.Diagnostics[2]
      Request finished HTTP/2 GET https://localhost:7177/Cart/GetCartCount - 200 - application/json;+charset=utf-8 81.6758ms
info: Microsoft.AspNetCore.Hosting.Diagnostics[1]
      Request starting HTTP/2 GET https://localhost:7177/Order/Detail?orderId=2 - - -
info: Microsoft.AspNetCore.Routing.EndpointMiddleware[0]
      Executing endpoint 'WebApplication.Controllers.OrderController.Detail (WebApplication)'
info: Microsoft.AspNetCore.Mvc.Infrastructure.ControllerActionInvoker[102]
      Route matched with {action = "Detail", controller = "Order"}. Executing controller action with signature System.Threading.Tasks.Task`1[Microsoft.AspNetCore.Mvc.IActionResult] Detail(Int32, System.Threading.CancellationToken) on controller WebApplication.Controllers.OrderController (WebApplication).
info: Microsoft.EntityFrameworkCore.Database.Command[20101]
      Executed DbCommand (56ms) [Parameters=[@__orderId_0='?' (DbType = Int32)], CommandType='Text', CommandTimeout='30']
      SELECT TOP(1) [o].[OrderId], [o].[CartId], [o].[ContactPhone], [o].[CreatedAt], [o].[DeliveryInstructions], [o].[DiscountAmount], [o].[FulfillmentType], [o].[GuestSessionId], [o].[IsDeleted], [o].[IsWalkIn], [o].[OrderDate], [o].[OrderNumber], [o].[OrderStatus], [o].[POSSessionId], [o].[ShippingAddressId], [o].[ShippingFee], [o].[SubTotal], [o].[TotalAmount], [o].[UpdatedAt], [o].[UserId], [p].[PickupOrderId], [a].[AddressId], [p].[OrderId], [p].[PickupConfirmedAt], [p].[PickupExpiresAt], [p].[PickupReadyAt], [a].[City], [a].[Country], [a].[CreatedAt], [a].[IsDefault], [a].[IsSnapshot], [a].[Label], [a].[PostalCode], [a].[Province], [a].[Street], [a].[UserId]
      FROM [Order] AS [o]
      LEFT JOIN [PickupOrder] AS [p] ON [o].[OrderId] = [p].[OrderId]
      LEFT JOIN [Address] AS [a] ON [o].[ShippingAddressId] = [a].[AddressId]
      WHERE [o].[IsDeleted] = CAST(0 AS bit) AND [o].[OrderId] = @__orderId_0
      ORDER BY [o].[OrderId], [p].[PickupOrderId], [a].[AddressId]
info: Microsoft.EntityFrameworkCore.Database.Command[20101]
      Executed DbCommand (57ms) [Parameters=[@__orderId_0='?' (DbType = Int32)], CommandType='Text', CommandTimeout='30']
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
      Executed DbCommand (57ms) [Parameters=[@__orderId_0='?' (DbType = Int32)], CommandType='Text', CommandTimeout='30']
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
      Executed DbCommand (56ms) [Parameters=[@__orderId_0='?' (DbType = Int32)], CommandType='Text', CommandTimeout='30']
      SELECT [t0].[PaymentId], [t0].[Amount], [t0].[CreatedAt], [t0].[OrderId], [t0].[PaymentDate], [t0].[PaymentMethod], [t0].[PaymentStage], [t0].[PaymentStatus], [t0].[PaymentId0], [t0].[GcashTransactionId], [t0].[ScreenshotUrl], [t0].[StorageBucket], [t0].[StoragePath], [t0].[SubmittedAt], [t0].[PaymentId1], [t0].[BpiReferenceNumber], [t0].[ProofStorageBucket], [t0].[ProofStoragePath], [t0].[ProofUrl], [t0].[SubmittedAt0], [t0].[VerificationDeadline], [t0].[VerificationNotes], [t0].[VerifiedAt], [t0].[VerifiedByUserId], [t].[OrderId], [t].[PickupOrderId], [t].[AddressId]
      FROM (
          SELECT TOP(1) [o].[OrderId], [p].[PickupOrderId], [a].[AddressId]
          FROM [Order] AS [o]
          LEFT JOIN [PickupOrder] AS [p] ON [o].[OrderId] = [p].[OrderId]
          LEFT JOIN [Address] AS [a] ON [o].[ShippingAddressId] = [a].[AddressId]
          WHERE [o].[IsDeleted] = CAST(0 AS bit) AND [o].[OrderId] = @__orderId_0
      ) AS [t]
      INNER JOIN (
          SELECT [p0].[PaymentId], [p0].[Amount], [p0].[CreatedAt], [p0].[OrderId], [p0].[PaymentDate], [p0].[PaymentMethod], [p0].[PaymentStage], [p0].[PaymentStatus], [g].[PaymentId] AS [PaymentId0], [g].[GcashTransactionId], [g].[ScreenshotUrl], [g].[StorageBucket], [g].[StoragePath], [g].[SubmittedAt], [b].[PaymentId] AS [PaymentId1], [b].[BpiReferenceNumber], [b].[ProofStorageBucket], [b].[ProofStoragePath], [b].[ProofUrl], [b].[SubmittedAt] AS [SubmittedAt0], [b].[VerificationDeadline], [b].[VerificationNotes], [b].[VerifiedAt], [b].[VerifiedByUserId]
          FROM [Payment] AS [p0]
          LEFT JOIN [GCashPayment] AS [g] ON [p0].[PaymentId] = [g].[PaymentId]
          LEFT JOIN [BankTransferPayment] AS [b] ON [p0].[PaymentId] = [b].[PaymentId]
      ) AS [t0] ON [t].[OrderId] = [t0].[OrderId]
      ORDER BY [t].[OrderId], [t].[PickupOrderId], [t].[AddressId]
info: Microsoft.EntityFrameworkCore.Database.Command[20101]
      Executed DbCommand (57ms) [Parameters=[@__orderId_0='?' (DbType = Int32)], CommandType='Text', CommandTimeout='30']
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
info: Microsoft.AspNetCore.Mvc.ViewFeatures.ViewResultExecutor[4]
      Executed ViewResult - view ~/Views/Customer/OrderDetail.cshtml executed in 13.8726ms.
info: Microsoft.AspNetCore.Mvc.Infrastructure.ControllerActionInvoker[105]
      Executed action WebApplication.Controllers.OrderController.Detail (WebApplication) in 358.6053ms
info: Microsoft.AspNetCore.Routing.EndpointMiddleware[1]
      Executed endpoint 'WebApplication.Controllers.OrderController.Detail (WebApplication)'
info: Microsoft.AspNetCore.Hosting.Diagnostics[2]
      Request finished HTTP/2 GET https://localhost:7177/Order/Detail?orderId=2 - 200 - text/html;+charset=utf-8 370.6960ms
info: Microsoft.AspNetCore.Hosting.Diagnostics[1]
      Request starting HTTP/2 GET https://localhost:7177/_framework/aspnetcore-browser-refresh.js - - -
info: Microsoft.AspNetCore.Hosting.Diagnostics[1]
      Request starting HTTP/2 GET https://localhost:7177/_vs/browserLink - - -
info: Microsoft.AspNetCore.Hosting.Diagnostics[2]
      Request finished HTTP/2 GET https://localhost:7177/_framework/aspnetcore-browser-refresh.js - 200 14933 application/javascript;+charset=utf-8 4.3474ms
info: Microsoft.AspNetCore.Hosting.Diagnostics[2]
      Request finished HTTP/2 GET https://localhost:7177/_vs/browserLink - 200 - text/javascript;+charset=UTF-8 12.9223ms
info: Microsoft.AspNetCore.Hosting.Diagnostics[1]
      Request starting HTTP/2 GET https://localhost:7177/Customer/NotificationCount - - -
info: Microsoft.AspNetCore.Hosting.Diagnostics[1]
      Request starting HTTP/2 GET https://localhost:7177/Cart/GetCartCount - - -
info: Microsoft.AspNetCore.Routing.EndpointMiddleware[0]
      Executing endpoint 'WebApplication.Controllers.CartController.GetCartCount (WebApplication)'
info: Microsoft.AspNetCore.Routing.EndpointMiddleware[0]
      Executing endpoint 'WebApplication.Controllers.CustomerController.NotificationCount (WebApplication)'
info: Microsoft.AspNetCore.Mvc.Infrastructure.ControllerActionInvoker[102]
      Route matched with {action = "GetCartCount", controller = "Cart"}. Executing controller action with signature System.Threading.Tasks.Task`1[Microsoft.AspNetCore.Mvc.IActionResult] GetCartCount(System.Threading.CancellationToken) on controller WebApplication.Controllers.CartController (WebApplication).
info: Microsoft.AspNetCore.Mvc.Infrastructure.ControllerActionInvoker[102]
      Route matched with {action = "NotificationCount", controller = "Customer"}. Executing controller action with signature System.Threading.Tasks.Task`1[Microsoft.AspNetCore.Mvc.IActionResult] NotificationCount(System.Threading.CancellationToken) on controller WebApplication.Controllers.CustomerController (WebApplication).
info: Microsoft.EntityFrameworkCore.Database.Command[20101]
      Executed DbCommand (56ms) [Parameters=[@__userId_Value_0='?' (DbType = Int32)], CommandType='Text', CommandTimeout='30']
      SELECT TOP(1) [c].[CartId], [c].[CreatedAt], [c].[GuestSessionId], [c].[IsCheckedOut], [c].[LastUpdatedAt], [c].[UserId]
      FROM [Cart] AS [c]
      WHERE [c].[IsCheckedOut] = CAST(0 AS bit) AND [c].[UserId] = @__userId_Value_0
      ORDER BY [c].[CartId]
info: Microsoft.EntityFrameworkCore.Database.Command[20101]
      Executed DbCommand (57ms) [Parameters=[@__userId_0='?' (DbType = Int32)], CommandType='Text', CommandTimeout='30']
      SELECT COUNT(*)
      FROM [Notification] AS [n]
      WHERE [n].[UserId] = @__userId_0 AND [n].[IsRead] = CAST(0 AS bit)
info: Microsoft.AspNetCore.Mvc.Infrastructure.SystemTextJsonResultExecutor[1]
      Executing JsonResult, writing value of type 'WebApplication.Models.ApiResponse'.
info: Microsoft.AspNetCore.Mvc.Infrastructure.SystemTextJsonResultExecutor[1]
      Executing JsonResult, writing value of type '<>f__AnonymousType6`1[[System.Int32, System.Private.CoreLib, Version=8.0.0.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e]]'.
info: Microsoft.AspNetCore.Mvc.Infrastructure.ControllerActionInvoker[105]
      Executed action WebApplication.Controllers.CartController.GetCartCount (WebApplication) in 63.2564ms
info: Microsoft.AspNetCore.Mvc.Infrastructure.ControllerActionInvoker[105]
      Executed action WebApplication.Controllers.CustomerController.NotificationCount (WebApplication) in 63.0262ms
info: Microsoft.AspNetCore.Routing.EndpointMiddleware[1]
      Executed endpoint 'WebApplication.Controllers.CartController.GetCartCount (WebApplication)'
info: Microsoft.AspNetCore.Routing.EndpointMiddleware[1]
      Executed endpoint 'WebApplication.Controllers.CustomerController.NotificationCount (WebApplication)'
info: Microsoft.AspNetCore.Hosting.Diagnostics[2]
      Request finished HTTP/2 GET https://localhost:7177/Cart/GetCartCount - 200 - application/json;+charset=utf-8 76.1092ms
info: Microsoft.AspNetCore.Hosting.Diagnostics[2]
      Request finished HTTP/2 GET https://localhost:7177/Customer/NotificationCount - 200 - application/json;+charset=utf-8 77.3032ms
info: Microsoft.AspNetCore.Hosting.Diagnostics[1]
      Request starting HTTP/2 GET https://localhost:7177/_framework/aspnetcore-browser-refresh.js - - -
info: Microsoft.AspNetCore.Hosting.Diagnostics[2]
      Request finished HTTP/2 GET https://localhost:7177/_framework/aspnetcore-browser-refresh.js - 200 14933 application/javascript;+charset=utf-8 2.8321ms
info: Microsoft.AspNetCore.Hosting.Diagnostics[1]
      Request starting HTTP/2 GET https://localhost:7177/antiforgery/token - - -
info: Microsoft.AspNetCore.Routing.EndpointMiddleware[0]
      Executing endpoint 'HTTP: GET /antiforgery/token'
info: Microsoft.AspNetCore.Http.Result.OkObjectResult[1]
      Setting HTTP status code 200.
info: Microsoft.AspNetCore.Http.Result.OkObjectResult[3]
      Writing value of type '<>f__AnonymousType20`1' as Json.
info: Microsoft.AspNetCore.Routing.EndpointMiddleware[1]
      Executed endpoint 'HTTP: GET /antiforgery/token'
info: Microsoft.AspNetCore.Hosting.Diagnostics[2]
      Request finished HTTP/2 GET https://localhost:7177/antiforgery/token - 200 - application/json;+charset=utf-8 9.7880ms
info: Microsoft.AspNetCore.Hosting.Diagnostics[1]
      Request starting HTTP/2 GET https://localhost:7177/_framework/aspnetcore-browser-refresh.js - - -
info: Microsoft.AspNetCore.Hosting.Diagnostics[2]
      Request finished HTTP/2 GET https://localhost:7177/_framework/aspnetcore-browser-refresh.js - 200 14933 application/javascript;+charset=utf-8 2.8500ms
info: Microsoft.AspNetCore.Hosting.Diagnostics[1]
      Request starting HTTP/2 GET https://localhost:7177/antiforgery/token - - -
info: Microsoft.AspNetCore.Routing.EndpointMiddleware[0]
      Executing endpoint 'HTTP: GET /antiforgery/token'
info: Microsoft.AspNetCore.Http.Result.OkObjectResult[1]
      Setting HTTP status code 200.
info: Microsoft.AspNetCore.Http.Result.OkObjectResult[3]
      Writing value of type '<>f__AnonymousType20`1' as Json.
info: Microsoft.AspNetCore.Routing.EndpointMiddleware[1]
      Executed endpoint 'HTTP: GET /antiforgery/token'
info: Microsoft.AspNetCore.Hosting.Diagnostics[2]
      Request finished HTTP/2 GET https://localhost:7177/antiforgery/token - 200 - application/json;+charset=utf-8 8.6992ms
info: Microsoft.AspNetCore.Hosting.Diagnostics[1]
      Request starting HTTP/2 GET https://localhost:7177/Product/List - - -
info: Microsoft.AspNetCore.Routing.EndpointMiddleware[0]
      Executing endpoint 'WebApplication.Controllers.ProductController.List (WebApplication)'
info: Microsoft.AspNetCore.Mvc.Infrastructure.ControllerActionInvoker[102]
      Route matched with {action = "List", controller = "Product"}. Executing controller action with signature System.Threading.Tasks.Task`1[Microsoft.AspNetCore.Mvc.IActionResult] List(System.Nullable`1[System.Int32], System.Nullable`1[System.Int32], System.Nullable`1[System.Decimal], System.Nullable`1[System.Decimal], System.String, System.String, Boolean, Int32, System.Threading.CancellationToken) on controller WebApplication.Controllers.ProductController (WebApplication).
info: Microsoft.EntityFrameworkCore.Database.Command[20101]
      Executed DbCommand (57ms) [Parameters=[@__userId_0='?' (DbType = Int32)], CommandType='Text', CommandTimeout='30']
      SELECT [w].[ProductId]
      FROM [Wishlist] AS [w]
      WHERE [w].[UserId] = @__userId_0
info: Microsoft.EntityFrameworkCore.Database.Command[20101]
      Executed DbCommand (59ms) [Parameters=[@__p_0='?' (DbType = Int32), @__p_1='?' (DbType = Int32)], CommandType='Text', CommandTimeout='30']
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
      Executed DbCommand (63ms) [Parameters=[@__p_0='?' (DbType = Int32), @__p_1='?' (DbType = Int32)], CommandType='Text', CommandTimeout='30']
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
      Executed DbCommand (58ms) [Parameters=[@__p_0='?' (DbType = Int32), @__p_1='?' (DbType = Int32)], CommandType='Text', CommandTimeout='30']
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
      Executed DbCommand (57ms) [Parameters=[], CommandType='Text', CommandTimeout='30']
      SELECT COUNT(*)
      FROM [Product] AS [p]
      INNER JOIN [Category] AS [c] ON [p].[CategoryId] = [c].[CategoryId]
      WHERE [p].[IsActive] = CAST(1 AS bit) AND [c].[IsActive] = CAST(1 AS bit)
info: Microsoft.EntityFrameworkCore.Database.Command[20101]
      Executed DbCommand (57ms) [Parameters=[], CommandType='Text', CommandTimeout='30']
      SELECT [b].[BrandId], [b].[BrandName], [b].[Country], [b].[CreatedAt], [b].[Description], [b].[IsActive], [b].[Website]
      FROM [Brand] AS [b]
      WHERE [b].[IsActive] = CAST(1 AS bit)
      ORDER BY [b].[BrandName]
info: Microsoft.EntityFrameworkCore.Database.Command[20101]
      Executed DbCommand (58ms) [Parameters=[], CommandType='Text', CommandTimeout='30']
      SELECT [c].[CategoryId], [c].[CategoryCode], [c].[Description], [c].[DisplayOrder], [c].[IsActive], [c].[Name], [c].[ParentCategoryId]
      FROM [Category] AS [c]
      WHERE [c].[IsActive] = CAST(1 AS bit)
      ORDER BY [c].[DisplayOrder], [c].[Name]
info: Microsoft.AspNetCore.Mvc.ViewFeatures.ViewResultExecutor[1]
      Executing ViewResult, running view ~/Views/Customer/ProductCatalog.cshtml.
info: Microsoft.AspNetCore.Mvc.ViewFeatures.ViewResultExecutor[4]
      Executed ViewResult - view ~/Views/Customer/ProductCatalog.cshtml executed in 40.323ms.
info: Microsoft.AspNetCore.Mvc.Infrastructure.ControllerActionInvoker[105]
      Executed action WebApplication.Controllers.ProductController.List (WebApplication) in 518.5891ms
info: Microsoft.AspNetCore.Routing.EndpointMiddleware[1]
      Executed endpoint 'WebApplication.Controllers.ProductController.List (WebApplication)'
info: Microsoft.AspNetCore.Hosting.Diagnostics[2]
      Request finished HTTP/2 GET https://localhost:7177/Product/List - 200 - text/html;+charset=utf-8 531.4627ms
info: Microsoft.AspNetCore.Hosting.Diagnostics[1]
      Request starting HTTP/2 GET https://localhost:7177/_framework/aspnetcore-browser-refresh.js - - -
info: Microsoft.AspNetCore.Hosting.Diagnostics[1]
      Request starting HTTP/2 GET https://localhost:7177/_vs/browserLink - - -
info: Microsoft.AspNetCore.Hosting.Diagnostics[2]
      Request finished HTTP/2 GET https://localhost:7177/_framework/aspnetcore-browser-refresh.js - 200 14933 application/javascript;+charset=utf-8 4.8304ms
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
      Request finished HTTP/2 GET https://localhost:7177/_vs/browserLink - 200 - text/javascript;+charset=UTF-8 24.3840ms
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
      WHERE [n].[UserId] = @__userId_0 AND [n].[IsRead] = CAST(0 AS bit)
info: Microsoft.AspNetCore.Mvc.Infrastructure.SystemTextJsonResultExecutor[1]
      Executing JsonResult, writing value of type 'WebApplication.Models.ApiResponse'.
info: Microsoft.AspNetCore.Mvc.Infrastructure.SystemTextJsonResultExecutor[1]
      Executing JsonResult, writing value of type '<>f__AnonymousType6`1[[System.Int32, System.Private.CoreLib, Version=8.0.0.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e]]'.
info: Microsoft.AspNetCore.Mvc.Infrastructure.ControllerActionInvoker[105]
      Executed action WebApplication.Controllers.CartController.GetCartCount (WebApplication) in 66.1515ms
info: Microsoft.AspNetCore.Mvc.Infrastructure.ControllerActionInvoker[105]
      Executed action WebApplication.Controllers.CustomerController.NotificationCount (WebApplication) in 66.0361ms
info: Microsoft.AspNetCore.Routing.EndpointMiddleware[1]
      Executed endpoint 'WebApplication.Controllers.CartController.GetCartCount (WebApplication)'
info: Microsoft.AspNetCore.Routing.EndpointMiddleware[1]
      Executed endpoint 'WebApplication.Controllers.CustomerController.NotificationCount (WebApplication)'
info: Microsoft.AspNetCore.Hosting.Diagnostics[2]
      Request finished HTTP/2 GET https://localhost:7177/Cart/GetCartCount - 200 - application/json;+charset=utf-8 79.8873ms
info: Microsoft.AspNetCore.Hosting.Diagnostics[2]
      Request finished HTTP/2 GET https://localhost:7177/Customer/NotificationCount - 200 - application/json;+charset=utf-8 81.1351ms
info: Microsoft.EntityFrameworkCore.Database.Command[20101]
      Executed DbCommand (56ms) [Parameters=[], CommandType='Text', CommandTimeout='30']
      SELECT COUNT(*)
      FROM [ProductVariant] AS [p]
info: Microsoft.AspNetCore.Hosting.Diagnostics[1]
      Request starting HTTP/2 GET https://localhost:7177/Customer/Logout - - -
info: Microsoft.AspNetCore.Routing.EndpointMiddleware[0]
      Executing endpoint 'WebApplication.Controllers.CustomerController.Logout (WebApplication)'
info: Microsoft.AspNetCore.Mvc.Infrastructure.ControllerActionInvoker[102]
      Route matched with {action = "Logout", controller = "Customer"}. Executing controller action with signature System.Threading.Tasks.Task`1[Microsoft.AspNetCore.Mvc.IActionResult] Logout() on controller WebApplication.Controllers.CustomerController (WebApplication).
info: Microsoft.AspNetCore.Authentication.Cookies.CookieAuthenticationHandler[11]
      AuthenticationScheme: Cookies signed out.
info: Microsoft.AspNetCore.Mvc.RedirectToActionResult[1]
      Executing RedirectResult, redirecting to /.
info: Microsoft.AspNetCore.Mvc.Infrastructure.ControllerActionInvoker[105]
      Executed action WebApplication.Controllers.CustomerController.Logout (WebApplication) in 6.1677ms
info: Microsoft.AspNetCore.Routing.EndpointMiddleware[1]
      Executed endpoint 'WebApplication.Controllers.CustomerController.Logout (WebApplication)'
info: Microsoft.AspNetCore.Hosting.Diagnostics[2]
      Request finished HTTP/2 GET https://localhost:7177/Customer/Logout - 302 - - 18.1057ms
info: Microsoft.AspNetCore.Hosting.Diagnostics[1]
      Request starting HTTP/2 GET https://localhost:7177/ - - -
info: Microsoft.AspNetCore.Routing.EndpointMiddleware[0]
      Executing endpoint 'WebApplication.Controllers.HomeController.Index (WebApplication)'
info: Microsoft.AspNetCore.Mvc.Infrastructure.ControllerActionInvoker[102]
      Route matched with {action = "Index", controller = "Home"}. Executing controller action with signature System.Threading.Tasks.Task`1[Microsoft.AspNetCore.Mvc.IActionResult] Index(System.Threading.CancellationToken) on controller WebApplication.Controllers.HomeController (WebApplication).
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
      Executed ViewResult - view Index executed in 12.3807ms.
info: Microsoft.AspNetCore.Mvc.Infrastructure.ControllerActionInvoker[105]
      Executed action WebApplication.Controllers.HomeController.Index (WebApplication) in 196.4971ms
info: Microsoft.AspNetCore.Routing.EndpointMiddleware[1]
      Executed endpoint 'WebApplication.Controllers.HomeController.Index (WebApplication)'
info: Microsoft.AspNetCore.Hosting.Diagnostics[2]
      Request finished HTTP/2 GET https://localhost:7177/ - 200 - text/html;+charset=utf-8 205.3583ms
info: Microsoft.AspNetCore.Hosting.Diagnostics[1]
      Request starting HTTP/2 GET https://localhost:7177/_framework/aspnetcore-browser-refresh.js - - -
info: Microsoft.AspNetCore.Hosting.Diagnostics[1]
      Request starting HTTP/2 GET https://localhost:7177/_vs/browserLink - - -
info: Microsoft.AspNetCore.Hosting.Diagnostics[2]
      Request finished HTTP/2 GET https://localhost:7177/_framework/aspnetcore-browser-refresh.js - 200 14933 application/javascript;+charset=utf-8 3.5936ms
info: Microsoft.AspNetCore.Hosting.Diagnostics[2]
      Request finished HTTP/2 GET https://localhost:7177/_vs/browserLink - 200 - text/javascript;+charset=UTF-8 13.0153ms
info: Microsoft.AspNetCore.Hosting.Diagnostics[1]
      Request starting HTTP/2 GET https://localhost:7177/antiforgery/token - - -
info: Microsoft.AspNetCore.Hosting.Diagnostics[1]
      Request starting HTTP/2 GET https://localhost:7177/Cart/GetCartCount - - -
info: Microsoft.AspNetCore.Routing.EndpointMiddleware[0]
      Executing endpoint 'HTTP: GET /antiforgery/token'
info: Microsoft.AspNetCore.Routing.EndpointMiddleware[0]
      Executing endpoint 'WebApplication.Controllers.CartController.GetCartCount (WebApplication)'
info: Microsoft.AspNetCore.Http.Result.OkObjectResult[1]
      Setting HTTP status code 200.
info: Microsoft.AspNetCore.Mvc.Infrastructure.ControllerActionInvoker[102]
      Route matched with {action = "GetCartCount", controller = "Cart"}. Executing controller action with signature System.Threading.Tasks.Task`1[Microsoft.AspNetCore.Mvc.IActionResult] GetCartCount(System.Threading.CancellationToken) on controller WebApplication.Controllers.CartController (WebApplication).
info: Microsoft.AspNetCore.Http.Result.OkObjectResult[3]
      Writing value of type '<>f__AnonymousType20`1' as Json.
info: Microsoft.AspNetCore.Routing.EndpointMiddleware[1]
      Executed endpoint 'HTTP: GET /antiforgery/token'
info: Microsoft.AspNetCore.Hosting.Diagnostics[2]
      Request finished HTTP/2 GET https://localhost:7177/antiforgery/token - 200 - application/json;+charset=utf-8 12.3753ms
info: Microsoft.EntityFrameworkCore.Database.Command[20101]
      Executed DbCommand (58ms) [Parameters=[@__guestSessionId_Value_0='?' (DbType = Int32)], CommandType='Text', CommandTimeout='30']
      SELECT TOP(1) [c].[CartId], [c].[CreatedAt], [c].[GuestSessionId], [c].[IsCheckedOut], [c].[LastUpdatedAt], [c].[UserId]
      FROM [Cart] AS [c]
      WHERE [c].[IsCheckedOut] = CAST(0 AS bit) AND [c].[GuestSessionId] = @__guestSessionId_Value_0
      ORDER BY [c].[CartId]
info: Microsoft.AspNetCore.Mvc.Infrastructure.SystemTextJsonResultExecutor[1]
      Executing JsonResult, writing value of type 'WebApplication.Models.ApiResponse'.
info: Microsoft.AspNetCore.Mvc.Infrastructure.ControllerActionInvoker[105]
      Executed action WebApplication.Controllers.CartController.GetCartCount (WebApplication) in 84.5479ms
info: Microsoft.AspNetCore.Routing.EndpointMiddleware[1]
      Executed endpoint 'WebApplication.Controllers.CartController.GetCartCount (WebApplication)'
info: Microsoft.AspNetCore.Hosting.Diagnostics[2]
      Request finished HTTP/2 GET https://localhost:7177/Cart/GetCartCount - 200 - application/json;+charset=utf-8 96.2893ms
info: Microsoft.EntityFrameworkCore.Database.Command[20101]
      Executed DbCommand (57ms) [Parameters=[], CommandType='Text', CommandTimeout='30']
      SELECT COUNT(*)
      FROM [ProductVariant] AS [p]
info: Microsoft.EntityFrameworkCore.Database.Command[20101]
      Executed DbCommand (57ms) [Parameters=[@__p_0='?' (DbType = Int32)], CommandType='Text', CommandTimeout='30']
      SELECT TOP(@__p_0) [n].[NotificationId], [n].[Body], [n].[Channel], [n].[CreatedAt], [n].[FailureReason], [n].[IsRead], [n].[NotifType], [n].[OrderId], [n].[ReadAt], [n].[Recipient], [n].[RetryCount], [n].[SentAt], [n].[Status], [n].[Subject], [n].[TicketId], [n].[UserId]
      FROM [Notification] AS [n]
      WHERE [n].[Status] = N'Pending' AND [n].[RetryCount] < 3 AND [n].[Channel] = N'Email'
      ORDER BY [n].[CreatedAt]
info: Microsoft.AspNetCore.Hosting.Diagnostics[1]
      Request starting HTTP/2 GET https://localhost:7177/Customer/Register - - -
info: Microsoft.AspNetCore.Routing.EndpointMiddleware[0]
      Executing endpoint 'WebApplication.Controllers.CustomerController.Register (WebApplication)'
info: Microsoft.AspNetCore.Mvc.Infrastructure.ControllerActionInvoker[102]
      Route matched with {action = "Register", controller = "Customer"}. Executing controller action with signature Microsoft.AspNetCore.Mvc.IActionResult Register() on controller WebApplication.Controllers.CustomerController (WebApplication).
info: Microsoft.AspNetCore.Mvc.ViewFeatures.ViewResultExecutor[1]
      Executing ViewResult, running view Register.
info: Microsoft.AspNetCore.Mvc.ViewFeatures.ViewResultExecutor[4]
      Executed ViewResult - view Register executed in 50.0823ms.
info: Microsoft.AspNetCore.Mvc.Infrastructure.ControllerActionInvoker[105]
      Executed action WebApplication.Controllers.CustomerController.Register (WebApplication) in 53.7792ms
info: Microsoft.AspNetCore.Routing.EndpointMiddleware[1]
      Executed endpoint 'WebApplication.Controllers.CustomerController.Register (WebApplication)'
info: Microsoft.AspNetCore.Hosting.Diagnostics[2]
      Request finished HTTP/2 GET https://localhost:7177/Customer/Register - 200 - text/html;+charset=utf-8 66.0735ms
info: Microsoft.AspNetCore.Hosting.Diagnostics[1]
      Request starting HTTP/2 GET https://localhost:7177/_framework/aspnetcore-browser-refresh.js - - -
info: Microsoft.AspNetCore.Hosting.Diagnostics[1]
      Request starting HTTP/2 GET https://localhost:7177/_vs/browserLink - - -
info: Microsoft.AspNetCore.Hosting.Diagnostics[2]
      Request finished HTTP/2 GET https://localhost:7177/_framework/aspnetcore-browser-refresh.js - 200 14933 application/javascript;+charset=utf-8 3.5809ms
info: Microsoft.AspNetCore.Hosting.Diagnostics[2]
      Request finished HTTP/2 GET https://localhost:7177/_vs/browserLink - 200 - text/javascript;+charset=UTF-8 15.2405ms
info: Microsoft.AspNetCore.Hosting.Diagnostics[1]
      Request starting HTTP/2 GET https://localhost:7177/Cart/GetCartCount - - -
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
      Executed action WebApplication.Controllers.CartController.GetCartCount (WebApplication) in 62.2196ms
info: Microsoft.AspNetCore.Routing.EndpointMiddleware[1]
      Executed endpoint 'WebApplication.Controllers.CartController.GetCartCount (WebApplication)'
info: Microsoft.AspNetCore.Hosting.Diagnostics[2]
      Request finished HTTP/2 GET https://localhost:7177/Cart/GetCartCount - 200 - application/json;+charset=utf-8 70.2589ms
info: Microsoft.EntityFrameworkCore.Database.Command[20101]
      Executed DbCommand (57ms) [Parameters=[], CommandType='Text', CommandTimeout='30']
      SELECT COUNT(*)
      FROM [ProductVariant] AS [p]
info: Microsoft.EntityFrameworkCore.Database.Command[20101]
      Executed DbCommand (57ms) [Parameters=[], CommandType='Text', CommandTimeout='30']
      SELECT COUNT(*)
      FROM [ProductVariant] AS [p]
info: Microsoft.AspNetCore.Hosting.Diagnostics[1]
      Request starting HTTP/2 POST https://localhost:7177/Customer/Register - application/x-www-form-urlencoded 439
info: Microsoft.AspNetCore.Cors.Infrastructure.CorsService[4]
      CORS policy execution successful.
info: Microsoft.AspNetCore.Routing.EndpointMiddleware[0]
      Executing endpoint 'WebApplication.Controllers.CustomerController.Register (WebApplication)'
info: Microsoft.AspNetCore.Mvc.Infrastructure.ControllerActionInvoker[102]
      Route matched with {action = "Register", controller = "Customer"}. Executing controller action with signature System.Threading.Tasks.Task`1[Microsoft.AspNetCore.Mvc.IActionResult] Register(WebApplication.Models.ViewModels.RegisterViewModel, System.Threading.CancellationToken) on controller WebApplication.Controllers.CustomerController (WebApplication).
info: Microsoft.EntityFrameworkCore.Database.Command[20101]
      Executed DbCommand (57ms) [Parameters=[@__email_0='?' (Size = 255)], CommandType='Text', CommandTimeout='30']
      SELECT TOP(1) [u].[UserId], [u].[CreatedAt], [u].[DefaultAddressId], [u].[Email], [u].[FailedLoginAttempts], [u].[FirstName], [u].[IsActive], [u].[IsDeleted], [u].[IsWalkIn], [u].[LastLoginAt], [u].[LastName], [u].[LockoutUntil], [u].[PasswordHash], [u].[PhoneNumber]
      FROM [User] AS [u]
      WHERE [u].[IsDeleted] = CAST(0 AS bit) AND [u].[Email] = @__email_0 AND [u].[IsActive] = CAST(1 AS bit)
info: Microsoft.EntityFrameworkCore.Database.Command[20101]
      Executed DbCommand (61ms) [Parameters=[@__email_0='?' (Size = 255)], CommandType='Text', CommandTimeout='30']
      SELECT [o].[OTPId], [o].[CreatedAt], [o].[Email], [o].[ExpiresAt], [o].[IsUsed], [o].[OTPCode]
      FROM [OTPVerification] AS [o]
      WHERE [o].[Email] = @__email_0 AND [o].[IsUsed] = CAST(0 AS bit)
info: Microsoft.EntityFrameworkCore.Database.Command[20101]
      Executed DbCommand (62ms) [Parameters=[@p0='?' (DbType = DateTime2), @p1='?' (Size = 255), @p2='?' (DbType = DateTime2), @p3='?' (DbType = Boolean), @p4='?' (Size = 10)], CommandType='Text', CommandTimeout='30']
      SET IMPLICIT_TRANSACTIONS OFF;
      SET NOCOUNT ON;
      INSERT INTO [OTPVerification] ([CreatedAt], [Email], [ExpiresAt], [IsUsed], [OTPCode])
      OUTPUT INSERTED.[OTPId]
      VALUES (@p0, @p1, @p2, @p3, @p4);
info: Microsoft.EntityFrameworkCore.Database.Command[20101]
      Executed DbCommand (57ms) [Parameters=[], CommandType='Text', CommandTimeout='30']
      SELECT COUNT(*)
      FROM [ProductVariant] AS [p]
info: Microsoft.EntityFrameworkCore.Database.Command[20101]
      Executed DbCommand (57ms) [Parameters=[@__p_0='?' (DbType = Int32)], CommandType='Text', CommandTimeout='30']
      SELECT TOP(@__p_0) [n].[NotificationId], [n].[Body], [n].[Channel], [n].[CreatedAt], [n].[FailureReason], [n].[IsRead], [n].[NotifType], [n].[OrderId], [n].[ReadAt], [n].[Recipient], [n].[RetryCount], [n].[SentAt], [n].[Status], [n].[Subject], [n].[TicketId], [n].[UserId]
      FROM [Notification] AS [n]
      WHERE [n].[Status] = N'Pending' AND [n].[RetryCount] < 3 AND [n].[Channel] = N'Email'
      ORDER BY [n].[CreatedAt]
info: WebApplication.BusinessLogic.Services.GmailEmailSender[0]
      Email sent to geronimojoan002@gmail.com - Subject: Your Taurus Bike Shop Verification Code
info: Microsoft.AspNetCore.Mvc.RedirectToActionResult[1]
      Executing RedirectResult, redirecting to /Customer/Register.
info: Microsoft.AspNetCore.Mvc.Infrastructure.ControllerActionInvoker[105]
      Executed action WebApplication.Controllers.CustomerController.Register (WebApplication) in 4205.9574ms
info: Microsoft.AspNetCore.Routing.EndpointMiddleware[1]
      Executed endpoint 'WebApplication.Controllers.CustomerController.Register (WebApplication)'
info: Microsoft.AspNetCore.Hosting.Diagnostics[2]
      Request finished HTTP/2 POST https://localhost:7177/Customer/Register - 302 - - 4224.1777ms
info: Microsoft.AspNetCore.Hosting.Diagnostics[1]
      Request starting HTTP/2 GET https://localhost:7177/Customer/Register - - -
info: Microsoft.AspNetCore.Routing.EndpointMiddleware[0]
      Executing endpoint 'WebApplication.Controllers.CustomerController.Register (WebApplication)'
info: Microsoft.AspNetCore.Mvc.Infrastructure.ControllerActionInvoker[102]
      Route matched with {action = "Register", controller = "Customer"}. Executing controller action with signature Microsoft.AspNetCore.Mvc.IActionResult Register() on controller WebApplication.Controllers.CustomerController (WebApplication).
info: Microsoft.AspNetCore.Mvc.ViewFeatures.ViewResultExecutor[1]
      Executing ViewResult, running view Register.
info: Microsoft.AspNetCore.Mvc.ViewFeatures.ViewResultExecutor[4]
      Executed ViewResult - view Register executed in 9.4241ms.
info: Microsoft.AspNetCore.Mvc.Infrastructure.ControllerActionInvoker[105]
      Executed action WebApplication.Controllers.CustomerController.Register (WebApplication) in 13.7849ms
info: Microsoft.AspNetCore.Routing.EndpointMiddleware[1]
      Executed endpoint 'WebApplication.Controllers.CustomerController.Register (WebApplication)'
info: Microsoft.AspNetCore.Hosting.Diagnostics[2]
      Request finished HTTP/2 GET https://localhost:7177/Customer/Register - 200 - text/html;+charset=utf-8 26.1220ms
info: Microsoft.AspNetCore.Hosting.Diagnostics[1]
      Request starting HTTP/2 GET https://localhost:7177/_framework/aspnetcore-browser-refresh.js - - -
info: Microsoft.AspNetCore.Hosting.Diagnostics[1]
      Request starting HTTP/2 GET https://localhost:7177/_vs/browserLink - - -
info: Microsoft.AspNetCore.Hosting.Diagnostics[2]
      Request finished HTTP/2 GET https://localhost:7177/_framework/aspnetcore-browser-refresh.js - 200 14933 application/javascript;+charset=utf-8 7.4059ms
info: Microsoft.AspNetCore.Hosting.Diagnostics[1]
      Request starting HTTP/2 GET https://localhost:7177/Cart/GetCartCount - - -
info: Microsoft.AspNetCore.Routing.EndpointMiddleware[0]
      Executing endpoint 'WebApplication.Controllers.CartController.GetCartCount (WebApplication)'
info: Microsoft.AspNetCore.Mvc.Infrastructure.ControllerActionInvoker[102]
      Route matched with {action = "GetCartCount", controller = "Cart"}. Executing controller action with signature System.Threading.Tasks.Task`1[Microsoft.AspNetCore.Mvc.IActionResult] GetCartCount(System.Threading.CancellationToken) on controller WebApplication.Controllers.CartController (WebApplication).
info: Microsoft.AspNetCore.Hosting.Diagnostics[2]
      Request finished HTTP/2 GET https://localhost:7177/_vs/browserLink - 200 - text/javascript;+charset=UTF-8 23.0784ms
info: Microsoft.EntityFrameworkCore.Database.Command[20101]
      Executed DbCommand (57ms) [Parameters=[@__guestSessionId_Value_0='?' (DbType = Int32)], CommandType='Text', CommandTimeout='30']
      SELECT TOP(1) [c].[CartId], [c].[CreatedAt], [c].[GuestSessionId], [c].[IsCheckedOut], [c].[LastUpdatedAt], [c].[UserId]
      FROM [Cart] AS [c]
      WHERE [c].[IsCheckedOut] = CAST(0 AS bit) AND [c].[GuestSessionId] = @__guestSessionId_Value_0
      ORDER BY [c].[CartId]
info: Microsoft.AspNetCore.Mvc.Infrastructure.SystemTextJsonResultExecutor[1]
      Executing JsonResult, writing value of type 'WebApplication.Models.ApiResponse'.
info: Microsoft.AspNetCore.Mvc.Infrastructure.ControllerActionInvoker[105]
      Executed action WebApplication.Controllers.CartController.GetCartCount (WebApplication) in 65.1768ms
info: Microsoft.AspNetCore.Routing.EndpointMiddleware[1]
      Executed endpoint 'WebApplication.Controllers.CartController.GetCartCount (WebApplication)'
info: Microsoft.AspNetCore.Hosting.Diagnostics[2]
      Request finished HTTP/2 GET https://localhost:7177/Cart/GetCartCount - 200 - application/json;+charset=utf-8 74.2629ms
info: Microsoft.EntityFrameworkCore.Database.Command[20101]
      Executed DbCommand (57ms) [Parameters=[], CommandType='Text', CommandTimeout='30']
      SELECT COUNT(*)
      FROM [ProductVariant] AS [p]
info: Microsoft.AspNetCore.Hosting.Diagnostics[1]
      Request starting HTTP/2 POST https://localhost:7177/Customer/VerifyOTP - application/x-www-form-urlencoded 228
info: Microsoft.AspNetCore.Cors.Infrastructure.CorsService[4]
      CORS policy execution successful.
info: Microsoft.AspNetCore.Routing.EndpointMiddleware[0]
      Executing endpoint 'WebApplication.Controllers.CustomerController.VerifyOTP (WebApplication)'
info: Microsoft.AspNetCore.Mvc.Infrastructure.ControllerActionInvoker[102]
      Route matched with {action = "VerifyOTP", controller = "Customer"}. Executing controller action with signature System.Threading.Tasks.Task`1[Microsoft.AspNetCore.Mvc.IActionResult] VerifyOTP(System.String, System.String, System.Threading.CancellationToken) on controller WebApplication.Controllers.CustomerController (WebApplication).
info: Microsoft.EntityFrameworkCore.Database.Command[20101]
      Executed DbCommand (75ms) [Parameters=[@__email_0='?' (Size = 255), @__code_1='?' (Size = 10)], CommandType='Text', CommandTimeout='30']
      SELECT TOP(1) [o].[OTPId], [o].[CreatedAt], [o].[Email], [o].[ExpiresAt], [o].[IsUsed], [o].[OTPCode]
      FROM [OTPVerification] AS [o]
      WHERE [o].[Email] = @__email_0 AND [o].[OTPCode] = @__code_1 AND [o].[IsUsed] = CAST(0 AS bit) AND [o].[ExpiresAt] > GETUTCDATE()
info: Microsoft.EntityFrameworkCore.Database.Command[20101]
      Executed DbCommand (62ms) [Parameters=[@p1='?' (DbType = Int32), @p0='?' (DbType = Boolean), @p2='?' (DbType = DateTime2), @p3='?' (DbType = Int32), @p4='?' (Size = 255), @p5='?' (DbType = Int32), @p6='?' (Size = 100), @p7='?' (DbType = Boolean), @p8='?' (DbType = Boolean), @p9='?' (DbType = Boolean), @p10='?' (DbType = DateTime2), @p11='?' (Size = 100), @p12='?' (DbType = DateTime2), @p13='?' (Size = 255), @p14='?' (Size = 20)], CommandType='Text', CommandTimeout='30']
      SET NOCOUNT ON;
      UPDATE [OTPVerification] SET [IsUsed] = @p0
      OUTPUT 1
      WHERE [OTPId] = @p1;
      INSERT INTO [User] ([CreatedAt], [DefaultAddressId], [Email], [FailedLoginAttempts], [FirstName], [IsActive], [IsDeleted], [IsWalkIn], [LastLoginAt], [LastName], [LockoutUntil], [PasswordHash], [PhoneNumber])
      OUTPUT INSERTED.[UserId]
      VALUES (@p2, @p3, @p4, @p5, @p6, @p7, @p8, @p9, @p10, @p11, @p12, @p13, @p14);
info: Microsoft.EntityFrameworkCore.Database.Command[20101]
      Executed DbCommand (59ms) [Parameters=[@p15='?' (Size = 100), @p16='?' (Size = 100), @p17='?' (DbType = DateTime2), @p18='?' (DbType = Boolean), @p19='?' (DbType = Boolean), @p20='?' (Size = 50), @p21='?' (Size = 20), @p22='?' (Size = 100), @p23='?' (Size = 500), @p24='?' (DbType = Int32)], CommandType='Text', CommandTimeout='30']
      SET IMPLICIT_TRANSACTIONS OFF;
      SET NOCOUNT ON;
      INSERT INTO [Address] ([City], [Country], [CreatedAt], [IsDefault], [IsSnapshot], [Label], [PostalCode], [Province], [Street], [UserId])
      OUTPUT INSERTED.[AddressId]
      VALUES (@p15, @p16, @p17, @p18, @p19, @p20, @p21, @p22, @p23, @p24);
info: Microsoft.EntityFrameworkCore.Database.Command[20101]
      Executed DbCommand (61ms) [Parameters=[@p1='?' (DbType = Int32), @p0='?' (DbType = Int32)], CommandType='Text', CommandTimeout='30']
      SET IMPLICIT_TRANSACTIONS OFF;
      SET NOCOUNT ON;
      UPDATE [User] SET [DefaultAddressId] = @p0
      OUTPUT 1
      WHERE [UserId] = @p1;
info: Microsoft.EntityFrameworkCore.Database.Command[20101]
      Executed DbCommand (59ms) [Parameters=[@p0='?' (Size = 4000), @p1='?' (Size = 20), @p2='?' (DbType = DateTime2), @p3='?' (Size = 500), @p4='?' (DbType = Boolean), @p5='?' (Size = 100), @p6='?' (DbType = Int32), @p7='?' (DbType = DateTime2), @p8='?' (Size = 255), @p9='?' (DbType = Int32), @p10='?' (DbType = DateTime2), @p11='?' (Size = 20), @p12='?' (Size = 255), @p13='?' (DbType = Int32), @p14='?' (DbType = Int32)], CommandType='Text', CommandTimeout='30']
      SET IMPLICIT_TRANSACTIONS OFF;
      SET NOCOUNT ON;
      INSERT INTO [Notification] ([Body], [Channel], [CreatedAt], [FailureReason], [IsRead], [NotifType], [OrderId], [ReadAt], [Recipient], [RetryCount], [SentAt], [Status], [Subject], [TicketId], [UserId])
      OUTPUT INSERTED.[NotificationId]
      VALUES (@p0, @p1, @p2, @p3, @p4, @p5, @p6, @p7, @p8, @p9, @p10, @p11, @p12, @p13, @p14);
info: Microsoft.AspNetCore.Authentication.Cookies.CookieAuthenticationHandler[10]
      AuthenticationScheme: Cookies signed in.
info: Microsoft.AspNetCore.Mvc.RedirectToActionResult[1]
      Executing RedirectResult, redirecting to /Customer/Dashboard.
info: Microsoft.AspNetCore.Mvc.Infrastructure.ControllerActionInvoker[105]
      Executed action WebApplication.Controllers.CustomerController.VerifyOTP (WebApplication) in 1070.5594ms
info: Microsoft.AspNetCore.Routing.EndpointMiddleware[1]
      Executed endpoint 'WebApplication.Controllers.CustomerController.VerifyOTP (WebApplication)'
info: Microsoft.AspNetCore.Hosting.Diagnostics[2]
      Request finished HTTP/2 POST https://localhost:7177/Customer/VerifyOTP - 302 - - 1084.4795ms
info: Microsoft.AspNetCore.Hosting.Diagnostics[1]
      Request starting HTTP/2 GET https://localhost:7177/Customer/Dashboard - - -
info: Microsoft.AspNetCore.Routing.EndpointMiddleware[0]
      Executing endpoint 'WebApplication.Controllers.CustomerController.Dashboard (WebApplication)'
info: Microsoft.AspNetCore.Mvc.Infrastructure.ControllerActionInvoker[102]
      Route matched with {action = "Dashboard", controller = "Customer"}. Executing controller action with signature System.Threading.Tasks.Task`1[Microsoft.AspNetCore.Mvc.IActionResult] Dashboard(System.Threading.CancellationToken) on controller WebApplication.Controllers.CustomerController (WebApplication).
info: Microsoft.EntityFrameworkCore.Database.Command[20101]
      Executed DbCommand (57ms) [Parameters=[@__userId_0='?' (DbType = Int32), @__p_1='?' (DbType = Int32), @__p_2='?' (DbType = Int32)], CommandType='Text', CommandTimeout='30']
      SELECT [t].[OrderId], [t].[CartId], [t].[ContactPhone], [t].[CreatedAt], [t].[DeliveryInstructions], [t].[DiscountAmount], [t].[FulfillmentType], [t].[GuestSessionId], [t].[IsDeleted], [t].[IsWalkIn], [t].[OrderDate], [t].[OrderNumber], [t].[OrderStatus], [t].[POSSessionId], [t].[ShippingAddressId], [t].[ShippingFee], [t].[SubTotal], [t].[TotalAmount], [t].[UpdatedAt], [t].[UserId], [p].[PickupOrderId], [p].[OrderId], [p].[PickupConfirmedAt], [p].[PickupExpiresAt], [p].[PickupReadyAt]
      FROM (
          SELECT [o].[OrderId], [o].[CartId], [o].[ContactPhone], [o].[CreatedAt], [o].[DeliveryInstructions], [o].[DiscountAmount], [o].[FulfillmentType], [o].[GuestSessionId], [o].[IsDeleted], [o].[IsWalkIn], [o].[OrderDate], [o].[OrderNumber], [o].[OrderStatus], [o].[POSSessionId], [o].[ShippingAddressId], [o].[ShippingFee], [o].[SubTotal], [o].[TotalAmount], [o].[UpdatedAt], [o].[UserId]
          FROM [Order] AS [o]
          WHERE [o].[IsDeleted] = CAST(0 AS bit) AND [o].[UserId] = @__userId_0
          ORDER BY [o].[OrderDate] DESC
          OFFSET @__p_1 ROWS FETCH NEXT @__p_2 ROWS ONLY
      ) AS [t]
      LEFT JOIN [PickupOrder] AS [p] ON [t].[OrderId] = [p].[OrderId]
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
      Executed DbCommand (57ms) [Parameters=[@__userId_0='?' (DbType = Int32)], CommandType='Text', CommandTimeout='30']
      SELECT [s].[TicketId], [s].[AssignedToUserId], [s].[AttachmentBucket], [s].[AttachmentPath], [s].[AttachmentUrl], [s].[CreatedAt], [s].[Description], [s].[OrderId], [s].[ResolvedAt], [s].[Subject], [s].[TicketCategory], [s].[TicketSource], [s].[TicketStatus], [s].[UpdatedAt], [s].[UserId]
      FROM [SupportTicket] AS [s]
      WHERE [s].[UserId] = @__userId_0
      ORDER BY [s].[CreatedAt] DESC
info: Microsoft.AspNetCore.Mvc.ViewFeatures.ViewResultExecutor[1]
      Executing ViewResult, running view Dashboard.
info: Microsoft.AspNetCore.Mvc.ViewFeatures.ViewResultExecutor[4]
      Executed ViewResult - view Dashboard executed in 13.4055ms.
info: Microsoft.AspNetCore.Mvc.Infrastructure.ControllerActionInvoker[105]
      Executed action WebApplication.Controllers.CustomerController.Dashboard (WebApplication) in 404.8755ms
info: Microsoft.AspNetCore.Routing.EndpointMiddleware[1]
      Executed endpoint 'WebApplication.Controllers.CustomerController.Dashboard (WebApplication)'
info: Microsoft.AspNetCore.Hosting.Diagnostics[2]
      Request finished HTTP/2 GET https://localhost:7177/Customer/Dashboard - 200 - text/html;+charset=utf-8 418.0412ms
info: Microsoft.AspNetCore.Hosting.Diagnostics[1]
      Request starting HTTP/2 GET https://localhost:7177/_framework/aspnetcore-browser-refresh.js - - -
info: Microsoft.AspNetCore.Hosting.Diagnostics[1]
      Request starting HTTP/2 GET https://localhost:7177/_vs/browserLink - - -
info: Microsoft.AspNetCore.Hosting.Diagnostics[2]
      Request finished HTTP/2 GET https://localhost:7177/_framework/aspnetcore-browser-refresh.js - 200 14933 application/javascript;+charset=utf-8 3.5909ms
info: Microsoft.AspNetCore.Hosting.Diagnostics[1]
      Request starting HTTP/2 GET https://localhost:7177/Cart/GetCartCount - - -
info: Microsoft.AspNetCore.Hosting.Diagnostics[1]
      Request starting HTTP/2 GET https://localhost:7177/antiforgery/token - - -
info: Microsoft.AspNetCore.Hosting.Diagnostics[1]
      Request starting HTTP/2 GET https://localhost:7177/Customer/NotificationCount - - -
info: Microsoft.AspNetCore.Routing.EndpointMiddleware[0]
      Executing endpoint 'WebApplication.Controllers.CartController.GetCartCount (WebApplication)'
info: Microsoft.AspNetCore.Routing.EndpointMiddleware[0]
      Executing endpoint 'WebApplication.Controllers.CustomerController.NotificationCount (WebApplication)'
info: Microsoft.AspNetCore.Routing.EndpointMiddleware[0]
      Executing endpoint 'HTTP: GET /antiforgery/token'
info: Microsoft.AspNetCore.Mvc.Infrastructure.ControllerActionInvoker[102]
      Route matched with {action = "GetCartCount", controller = "Cart"}. Executing controller action with signature System.Threading.Tasks.Task`1[Microsoft.AspNetCore.Mvc.IActionResult] GetCartCount(System.Threading.CancellationToken) on controller WebApplication.Controllers.CartController (WebApplication).
info: Microsoft.AspNetCore.Mvc.Infrastructure.ControllerActionInvoker[102]
      Route matched with {action = "NotificationCount", controller = "Customer"}. Executing controller action with signature System.Threading.Tasks.Task`1[Microsoft.AspNetCore.Mvc.IActionResult] NotificationCount(System.Threading.CancellationToken) on controller WebApplication.Controllers.CustomerController (WebApplication).
info: Microsoft.AspNetCore.Http.Result.OkObjectResult[1]
      Setting HTTP status code 200.
info: Microsoft.AspNetCore.Hosting.Diagnostics[2]
      Request finished HTTP/2 GET https://localhost:7177/_vs/browserLink - 200 - text/javascript;+charset=UTF-8 33.0842ms
info: Microsoft.AspNetCore.Http.Result.OkObjectResult[3]
      Writing value of type '<>f__AnonymousType20`1' as Json.
info: Microsoft.AspNetCore.Routing.EndpointMiddleware[1]
      Executed endpoint 'HTTP: GET /antiforgery/token'
info: Microsoft.AspNetCore.Hosting.Diagnostics[2]
      Request finished HTTP/2 GET https://localhost:7177/antiforgery/token - 200 - application/json;+charset=utf-8 26.2580ms
info: Microsoft.EntityFrameworkCore.Database.Command[20101]
      Executed DbCommand (57ms) [Parameters=[@__userId_0='?' (DbType = Int32)], CommandType='Text', CommandTimeout='30']
      SELECT COUNT(*)
      FROM [Notification] AS [n]
      WHERE [n].[UserId] = @__userId_0 AND [n].[IsRead] = CAST(0 AS bit)
info: Microsoft.AspNetCore.Mvc.Infrastructure.SystemTextJsonResultExecutor[1]
      Executing JsonResult, writing value of type '<>f__AnonymousType6`1[[System.Int32, System.Private.CoreLib, Version=8.0.0.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e]]'.
info: Microsoft.EntityFrameworkCore.Database.Command[20101]
      Executed DbCommand (59ms) [Parameters=[@__userId_Value_0='?' (DbType = Int32)], CommandType='Text', CommandTimeout='30']
      SELECT TOP(1) [c].[CartId], [c].[CreatedAt], [c].[GuestSessionId], [c].[IsCheckedOut], [c].[LastUpdatedAt], [c].[UserId]
      FROM [Cart] AS [c]
      WHERE [c].[IsCheckedOut] = CAST(0 AS bit) AND [c].[UserId] = @__userId_Value_0
      ORDER BY [c].[CartId]
info: Microsoft.AspNetCore.Mvc.Infrastructure.ControllerActionInvoker[105]
      Executed action WebApplication.Controllers.CustomerController.NotificationCount (WebApplication) in 64.5632ms
info: Microsoft.AspNetCore.Mvc.Infrastructure.SystemTextJsonResultExecutor[1]
      Executing JsonResult, writing value of type 'WebApplication.Models.ApiResponse'.
info: Microsoft.AspNetCore.Routing.EndpointMiddleware[1]
      Executed endpoint 'WebApplication.Controllers.CustomerController.NotificationCount (WebApplication)'
info: Microsoft.AspNetCore.Mvc.Infrastructure.ControllerActionInvoker[105]
      Executed action WebApplication.Controllers.CartController.GetCartCount (WebApplication) in 72.3968ms
info: Microsoft.AspNetCore.Hosting.Diagnostics[2]
      Request finished HTTP/2 GET https://localhost:7177/Customer/NotificationCount - 200 - application/json;+charset=utf-8 85.0285ms
info: Microsoft.AspNetCore.Routing.EndpointMiddleware[1]
      Executed endpoint 'WebApplication.Controllers.CartController.GetCartCount (WebApplication)'
info: Microsoft.AspNetCore.Hosting.Diagnostics[2]
      Request finished HTTP/2 GET https://localhost:7177/Cart/GetCartCount - 200 - application/json;+charset=utf-8 89.7262ms
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
      WHERE [n].[UserId] = @__userId_0
info: Microsoft.EntityFrameworkCore.Database.Command[20101]
      Executed DbCommand (57ms) [Parameters=[@__userId_0='?' (DbType = Int32), @__p_1='?' (DbType = Int32), @__p_2='?' (DbType = Int32)], CommandType='Text', CommandTimeout='30']
      SELECT [n].[NotificationId], [n].[Body], [n].[Channel], [n].[CreatedAt], [n].[FailureReason], [n].[IsRead], [n].[NotifType], [n].[OrderId], [n].[ReadAt], [n].[Recipient], [n].[RetryCount], [n].[SentAt], [n].[Status], [n].[Subject], [n].[TicketId], [n].[UserId]
      FROM [Notification] AS [n]
      WHERE [n].[UserId] = @__userId_0
      ORDER BY [n].[CreatedAt] DESC
      OFFSET @__p_1 ROWS FETCH NEXT @__p_2 ROWS ONLY
info: Microsoft.EntityFrameworkCore.Database.Command[20101]
      Executed DbCommand (56ms) [Parameters=[@__userId_0='?' (DbType = Int32)], CommandType='Text', CommandTimeout='30']
      SELECT COUNT(*)
      FROM [Notification] AS [n]
      WHERE [n].[UserId] = @__userId_0 AND [n].[IsRead] = CAST(0 AS bit)
info: Microsoft.AspNetCore.Mvc.ViewFeatures.ViewResultExecutor[1]
      Executing ViewResult, running view Notifications.
info: Microsoft.AspNetCore.Mvc.ViewFeatures.ViewResultExecutor[4]
      Executed ViewResult - view Notifications executed in 6.7492ms.
info: Microsoft.AspNetCore.Mvc.Infrastructure.ControllerActionInvoker[105]
      Executed action WebApplication.Controllers.CustomerController.Notifications (WebApplication) in 184.6883ms
info: Microsoft.AspNetCore.Routing.EndpointMiddleware[1]
      Executed endpoint 'WebApplication.Controllers.CustomerController.Notifications (WebApplication)'
info: Microsoft.AspNetCore.Hosting.Diagnostics[2]
      Request finished HTTP/2 GET https://localhost:7177/Customer/Notifications - 200 - text/html;+charset=utf-8 194.9214ms
info: Microsoft.AspNetCore.Hosting.Diagnostics[1]
      Request starting HTTP/2 GET https://localhost:7177/_vs/browserLink - - -
info: Microsoft.AspNetCore.Hosting.Diagnostics[1]
      Request starting HTTP/2 GET https://localhost:7177/_framework/aspnetcore-browser-refresh.js - - -
info: Microsoft.AspNetCore.Hosting.Diagnostics[2]
      Request finished HTTP/2 GET https://localhost:7177/_framework/aspnetcore-browser-refresh.js - 200 14933 application/javascript;+charset=utf-8 3.8426ms
info: Microsoft.AspNetCore.Hosting.Diagnostics[2]
      Request finished HTTP/2 GET https://localhost:7177/_vs/browserLink - 200 - text/javascript;+charset=UTF-8 13.3465ms
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
      Executed DbCommand (55ms) [Parameters=[@__userId_Value_0='?' (DbType = Int32)], CommandType='Text', CommandTimeout='30']
      SELECT TOP(1) [c].[CartId], [c].[CreatedAt], [c].[GuestSessionId], [c].[IsCheckedOut], [c].[LastUpdatedAt], [c].[UserId]
      FROM [Cart] AS [c]
      WHERE [c].[IsCheckedOut] = CAST(0 AS bit) AND [c].[UserId] = @__userId_Value_0
      ORDER BY [c].[CartId]
info: Microsoft.EntityFrameworkCore.Database.Command[20101]
      Executed DbCommand (56ms) [Parameters=[@__userId_0='?' (DbType = Int32)], CommandType='Text', CommandTimeout='30']
      SELECT COUNT(*)
      FROM [Notification] AS [n]
      WHERE [n].[UserId] = @__userId_0 AND [n].[IsRead] = CAST(0 AS bit)
info: Microsoft.AspNetCore.Mvc.Infrastructure.SystemTextJsonResultExecutor[1]
      Executing JsonResult, writing value of type 'WebApplication.Models.ApiResponse'.
info: Microsoft.AspNetCore.Mvc.Infrastructure.SystemTextJsonResultExecutor[1]
      Executing JsonResult, writing value of type '<>f__AnonymousType6`1[[System.Int32, System.Private.CoreLib, Version=8.0.0.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e]]'.
info: Microsoft.AspNetCore.Mvc.Infrastructure.ControllerActionInvoker[105]
      Executed action WebApplication.Controllers.CartController.GetCartCount (WebApplication) in 61.4125ms
info: Microsoft.AspNetCore.Mvc.Infrastructure.ControllerActionInvoker[105]
      Executed action WebApplication.Controllers.CustomerController.NotificationCount (WebApplication) in 64.2864ms
info: Microsoft.AspNetCore.Routing.EndpointMiddleware[1]
      Executed endpoint 'WebApplication.Controllers.CartController.GetCartCount (WebApplication)'
info: Microsoft.AspNetCore.Routing.EndpointMiddleware[1]
      Executed endpoint 'WebApplication.Controllers.CustomerController.NotificationCount (WebApplication)'
info: Microsoft.AspNetCore.Hosting.Diagnostics[2]
      Request finished HTTP/2 GET https://localhost:7177/Cart/GetCartCount - 200 - application/json;+charset=utf-8 77.0202ms
info: Microsoft.AspNetCore.Hosting.Diagnostics[2]
      Request finished HTTP/2 GET https://localhost:7177/Customer/NotificationCount - 200 - application/json;+charset=utf-8 78.2179ms
info: Microsoft.EntityFrameworkCore.Database.Command[20101]
      Executed DbCommand (57ms) [Parameters=[], CommandType='Text', CommandTimeout='30']
      SELECT COUNT(*)
      FROM [ProductVariant] AS [p]
info: Microsoft.AspNetCore.Hosting.Diagnostics[1]
      Request starting HTTP/2 GET https://localhost:7177/Product/List - - -
info: Microsoft.AspNetCore.Routing.EndpointMiddleware[0]
      Executing endpoint 'WebApplication.Controllers.ProductController.List (WebApplication)'
info: Microsoft.AspNetCore.Mvc.Infrastructure.ControllerActionInvoker[102]
      Route matched with {action = "List", controller = "Product"}. Executing controller action with signature System.Threading.Tasks.Task`1[Microsoft.AspNetCore.Mvc.IActionResult] List(System.Nullable`1[System.Int32], System.Nullable`1[System.Int32], System.Nullable`1[System.Decimal], System.Nullable`1[System.Decimal], System.String, System.String, Boolean, Int32, System.Threading.CancellationToken) on controller WebApplication.Controllers.ProductController (WebApplication).
info: Microsoft.EntityFrameworkCore.Database.Command[20101]
      Executed DbCommand (57ms) [Parameters=[@__userId_0='?' (DbType = Int32)], CommandType='Text', CommandTimeout='30']
      SELECT [w].[ProductId]
      FROM [Wishlist] AS [w]
      WHERE [w].[UserId] = @__userId_0
info: Microsoft.EntityFrameworkCore.Database.Command[20101]
      Executed DbCommand (59ms) [Parameters=[@__p_0='?' (DbType = Int32), @__p_1='?' (DbType = Int32)], CommandType='Text', CommandTimeout='30']
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
      Executed DbCommand (58ms) [Parameters=[@__p_0='?' (DbType = Int32), @__p_1='?' (DbType = Int32)], CommandType='Text', CommandTimeout='30']
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
      Executed DbCommand (58ms) [Parameters=[@__p_0='?' (DbType = Int32), @__p_1='?' (DbType = Int32)], CommandType='Text', CommandTimeout='30']
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
      Executed DbCommand (57ms) [Parameters=[], CommandType='Text', CommandTimeout='30']
      SELECT COUNT(*)
      FROM [Product] AS [p]
      INNER JOIN [Category] AS [c] ON [p].[CategoryId] = [c].[CategoryId]
      WHERE [p].[IsActive] = CAST(1 AS bit) AND [c].[IsActive] = CAST(1 AS bit)
info: Microsoft.EntityFrameworkCore.Database.Command[20101]
      Executed DbCommand (57ms) [Parameters=[], CommandType='Text', CommandTimeout='30']
      SELECT [b].[BrandId], [b].[BrandName], [b].[Country], [b].[CreatedAt], [b].[Description], [b].[IsActive], [b].[Website]
      FROM [Brand] AS [b]
      WHERE [b].[IsActive] = CAST(1 AS bit)
      ORDER BY [b].[BrandName]
info: Microsoft.EntityFrameworkCore.Database.Command[20101]
      Executed DbCommand (57ms) [Parameters=[], CommandType='Text', CommandTimeout='30']
      SELECT [c].[CategoryId], [c].[CategoryCode], [c].[Description], [c].[DisplayOrder], [c].[IsActive], [c].[Name], [c].[ParentCategoryId]
      FROM [Category] AS [c]
      WHERE [c].[IsActive] = CAST(1 AS bit)
      ORDER BY [c].[DisplayOrder], [c].[Name]
info: Microsoft.AspNetCore.Mvc.ViewFeatures.ViewResultExecutor[1]
      Executing ViewResult, running view ~/Views/Customer/ProductCatalog.cshtml.
info: Microsoft.AspNetCore.Mvc.ViewFeatures.ViewResultExecutor[4]
      Executed ViewResult - view ~/Views/Customer/ProductCatalog.cshtml executed in 8.4753ms.
info: Microsoft.AspNetCore.Mvc.Infrastructure.ControllerActionInvoker[105]
      Executed action WebApplication.Controllers.ProductController.List (WebApplication) in 427.8548ms
info: Microsoft.AspNetCore.Routing.EndpointMiddleware[1]
      Executed endpoint 'WebApplication.Controllers.ProductController.List (WebApplication)'
info: Microsoft.AspNetCore.Hosting.Diagnostics[2]
      Request finished HTTP/2 GET https://localhost:7177/Product/List - 200 - text/html;+charset=utf-8 437.8051ms
info: Microsoft.AspNetCore.Hosting.Diagnostics[1]
      Request starting HTTP/2 GET https://localhost:7177/_framework/aspnetcore-browser-refresh.js - - -
info: Microsoft.AspNetCore.Hosting.Diagnostics[1]
      Request starting HTTP/2 GET https://localhost:7177/_vs/browserLink - - -
info: Microsoft.AspNetCore.Hosting.Diagnostics[2]
      Request finished HTTP/2 GET https://localhost:7177/_framework/aspnetcore-browser-refresh.js - 200 14933 application/javascript;+charset=utf-8 3.7381ms
info: Microsoft.AspNetCore.Hosting.Diagnostics[2]
      Request finished HTTP/2 GET https://localhost:7177/_vs/browserLink - 200 - text/javascript;+charset=UTF-8 12.7778ms
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
      Executed DbCommand (56ms) [Parameters=[@__userId_Value_0='?' (DbType = Int32)], CommandType='Text', CommandTimeout='30']
      SELECT TOP(1) [c].[CartId], [c].[CreatedAt], [c].[GuestSessionId], [c].[IsCheckedOut], [c].[LastUpdatedAt], [c].[UserId]
      FROM [Cart] AS [c]
      WHERE [c].[IsCheckedOut] = CAST(0 AS bit) AND [c].[UserId] = @__userId_Value_0
      ORDER BY [c].[CartId]
info: Microsoft.EntityFrameworkCore.Database.Command[20101]
      Executed DbCommand (57ms) [Parameters=[@__userId_0='?' (DbType = Int32)], CommandType='Text', CommandTimeout='30']
      SELECT COUNT(*)
      FROM [Notification] AS [n]
      WHERE [n].[UserId] = @__userId_0 AND [n].[IsRead] = CAST(0 AS bit)
info: Microsoft.AspNetCore.Mvc.Infrastructure.SystemTextJsonResultExecutor[1]
      Executing JsonResult, writing value of type 'WebApplication.Models.ApiResponse'.
info: Microsoft.AspNetCore.Mvc.Infrastructure.SystemTextJsonResultExecutor[1]
      Executing JsonResult, writing value of type '<>f__AnonymousType6`1[[System.Int32, System.Private.CoreLib, Version=8.0.0.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e]]'.
info: Microsoft.AspNetCore.Mvc.Infrastructure.ControllerActionInvoker[105]
      Executed action WebApplication.Controllers.CartController.GetCartCount (WebApplication) in 61.7662ms
info: Microsoft.AspNetCore.Mvc.Infrastructure.ControllerActionInvoker[105]
      Executed action WebApplication.Controllers.CustomerController.NotificationCount (WebApplication) in 64.4554ms
info: Microsoft.AspNetCore.Routing.EndpointMiddleware[1]
      Executed endpoint 'WebApplication.Controllers.CartController.GetCartCount (WebApplication)'
info: Microsoft.AspNetCore.Routing.EndpointMiddleware[1]
      Executed endpoint 'WebApplication.Controllers.CustomerController.NotificationCount (WebApplication)'
info: Microsoft.AspNetCore.Hosting.Diagnostics[2]
      Request finished HTTP/2 GET https://localhost:7177/Cart/GetCartCount - 200 - application/json;+charset=utf-8 77.0817ms
info: Microsoft.AspNetCore.Hosting.Diagnostics[2]
      Request finished HTTP/2 GET https://localhost:7177/Customer/NotificationCount - 200 - application/json;+charset=utf-8 78.3654ms
info: Microsoft.AspNetCore.Hosting.Diagnostics[1]
      Request starting HTTP/2 GET https://localhost:7177/Product/Detail/2 - - -
info: Microsoft.AspNetCore.Routing.EndpointMiddleware[0]
      Executing endpoint 'WebApplication.Controllers.ProductController.Detail (WebApplication)'
info: Microsoft.AspNetCore.Mvc.Infrastructure.ControllerActionInvoker[102]
      Route matched with {action = "Detail", controller = "Product"}. Executing controller action with signature System.Threading.Tasks.Task`1[Microsoft.AspNetCore.Mvc.IActionResult] Detail(Int32, System.Threading.CancellationToken) on controller WebApplication.Controllers.ProductController (WebApplication).
info: Microsoft.EntityFrameworkCore.Database.Command[20101]
      Executed DbCommand (57ms) [Parameters=[@__productId_0='?' (DbType = Int32)], CommandType='Text', CommandTimeout='30']
      SELECT TOP(1) [p].[ProductId], [p].[AdditionalSpecs], [p].[AxleStandard], [p].[BoostCompatible], [p].[BrakeType], [p].[BrandId], [p].[CategoryId], [p].[Color], [p].[CreatedAt], [p].[Currency], [p].[Description], [p].[IsActive], [p].[IsFeatured], [p].[Material], [p].[Name], [p].[Price], [p].[SKU], [p].[ShortDescription], [p].[SpeedCompatibility], [p].[SuspensionTravel], [p].[TubelessReady], [p].[UpdatedAt], [p].[WheelSize], [b].[BrandId], [b].[BrandName], [b].[Country], [b].[CreatedAt], [b].[Description], [b].[IsActive], [b].[Website], [c].[CategoryId], [c].[CategoryCode], [c].[Description], [c].[DisplayOrder], [c].[IsActive], [c].[Name], [c].[ParentCategoryId]
      FROM [Product] AS [p]
      LEFT JOIN [Brand] AS [b] ON [p].[BrandId] = [b].[BrandId]
      INNER JOIN [Category] AS [c] ON [p].[CategoryId] = [c].[CategoryId]
      WHERE [p].[ProductId] = @__productId_0 AND [p].[IsActive] = CAST(1 AS bit)
      ORDER BY [p].[ProductId], [b].[BrandId], [c].[CategoryId]
info: Microsoft.EntityFrameworkCore.Database.Command[20101]
      Executed DbCommand (57ms) [Parameters=[@__productId_0='?' (DbType = Int32)], CommandType='Text', CommandTimeout='30']
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
      Executed DbCommand (57ms) [Parameters=[@__productId_0='?' (DbType = Int32)], CommandType='Text', CommandTimeout='30']
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
      Executed DbCommand (57ms) [Parameters=[@__productId_0='?' (DbType = Int32)], CommandType='Text', CommandTimeout='30']
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
      Executed DbCommand (57ms) [Parameters=[@__productId_0='?' (DbType = Int32)], CommandType='Text', CommandTimeout='30']
      SELECT CASE
          WHEN EXISTS (
              SELECT 1
              FROM [Review] AS [r]
              WHERE [r].[ProductId] = @__productId_0) THEN CAST(1 AS bit)
          ELSE CAST(0 AS bit)
      END
info: Microsoft.EntityFrameworkCore.Database.Command[20101]
      Executed DbCommand (56ms) [Parameters=[@__productId_0='?' (DbType = Int32)], CommandType='Text', CommandTimeout='30']
      SELECT COUNT(*)
      FROM [Review] AS [r]
      WHERE [r].[ProductId] = @__productId_0
info: Microsoft.EntityFrameworkCore.Database.Command[20101]
      Executed DbCommand (57ms) [Parameters=[@__userId_0='?' (DbType = Int32)], CommandType='Text', CommandTimeout='30']
      SELECT [w].[ProductId]
      FROM [Wishlist] AS [w]
      WHERE [w].[UserId] = @__userId_0
info: Microsoft.EntityFrameworkCore.Database.Command[20101]
      Executed DbCommand (70ms) [Parameters=[@__productId_0='?' (DbType = Int32), @__userId_1='?' (DbType = Int32)], CommandType='Text', CommandTimeout='30']
      SELECT CASE
          WHEN EXISTS (
              SELECT 1
              FROM [OrderItem] AS [o]
              INNER JOIN (
                  SELECT [o0].[OrderId], [o0].[CartId], [o0].[ContactPhone], [o0].[CreatedAt], [o0].[DeliveryInstructions], [o0].[DiscountAmount], [o0].[FulfillmentType], [o0].[GuestSessionId], [o0].[IsDeleted], [o0].[IsWalkIn], [o0].[OrderDate], [o0].[OrderNumber], [o0].[OrderStatus], [o0].[POSSessionId], [o0].[ShippingAddressId], [o0].[ShippingFee], [o0].[SubTotal], [o0].[TotalAmount], [o0].[UpdatedAt], [o0].[UserId]
                  FROM [Order] AS [o0]
                  WHERE [o0].[IsDeleted] = CAST(0 AS bit)
              ) AS [t] ON [o].[OrderId] = [t].[OrderId]
              WHERE [o].[ProductId] = @__productId_0 AND [t].[UserId] = @__userId_1 AND [t].[OrderStatus] = N'Delivered') THEN CAST(1 AS bit)
          ELSE CAST(0 AS bit)
      END
info: Microsoft.AspNetCore.Mvc.ViewFeatures.ViewResultExecutor[1]
      Executing ViewResult, running view ~/Views/Customer/ProductDetails.cshtml.
info: Microsoft.AspNetCore.Mvc.ViewFeatures.ViewResultExecutor[4]
      Executed ViewResult - view ~/Views/Customer/ProductDetails.cshtml executed in 18.2236ms.
info: Microsoft.AspNetCore.Mvc.Infrastructure.ControllerActionInvoker[105]
      Executed action WebApplication.Controllers.ProductController.Detail (WebApplication) in 613.637ms
info: Microsoft.AspNetCore.Routing.EndpointMiddleware[1]
      Executed endpoint 'WebApplication.Controllers.ProductController.Detail (WebApplication)'
info: Microsoft.AspNetCore.Hosting.Diagnostics[2]
      Request finished HTTP/2 GET https://localhost:7177/Product/Detail/2 - 200 - text/html;+charset=utf-8 625.2823ms
info: Microsoft.AspNetCore.Hosting.Diagnostics[1]
      Request starting HTTP/2 GET https://localhost:7177/_framework/aspnetcore-browser-refresh.js - - -
info: Microsoft.AspNetCore.Hosting.Diagnostics[1]
      Request starting HTTP/2 GET https://localhost:7177/_vs/browserLink - - -
info: Microsoft.AspNetCore.Hosting.Diagnostics[2]
      Request finished HTTP/2 GET https://localhost:7177/_framework/aspnetcore-browser-refresh.js - 200 14933 application/javascript;+charset=utf-8 4.1097ms
info: Microsoft.AspNetCore.Hosting.Diagnostics[2]
      Request finished HTTP/2 GET https://localhost:7177/_vs/browserLink - 200 - text/javascript;+charset=UTF-8 14.8537ms
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
      Executed DbCommand (56ms) [Parameters=[@__userId_Value_0='?' (DbType = Int32)], CommandType='Text', CommandTimeout='30']
      SELECT TOP(1) [c].[CartId], [c].[CreatedAt], [c].[GuestSessionId], [c].[IsCheckedOut], [c].[LastUpdatedAt], [c].[UserId]
      FROM [Cart] AS [c]
      WHERE [c].[IsCheckedOut] = CAST(0 AS bit) AND [c].[UserId] = @__userId_Value_0
      ORDER BY [c].[CartId]
info: Microsoft.EntityFrameworkCore.Database.Command[20101]
      Executed DbCommand (57ms) [Parameters=[@__userId_0='?' (DbType = Int32)], CommandType='Text', CommandTimeout='30']
      SELECT COUNT(*)
      FROM [Notification] AS [n]
      WHERE [n].[UserId] = @__userId_0 AND [n].[IsRead] = CAST(0 AS bit)
info: Microsoft.AspNetCore.Mvc.Infrastructure.SystemTextJsonResultExecutor[1]
      Executing JsonResult, writing value of type 'WebApplication.Models.ApiResponse'.
info: Microsoft.AspNetCore.Mvc.Infrastructure.SystemTextJsonResultExecutor[1]
      Executing JsonResult, writing value of type '<>f__AnonymousType6`1[[System.Int32, System.Private.CoreLib, Version=8.0.0.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e]]'.
info: Microsoft.AspNetCore.Mvc.Infrastructure.ControllerActionInvoker[105]
      Executed action WebApplication.Controllers.CartController.GetCartCount (WebApplication) in 60.891ms
info: Microsoft.AspNetCore.Mvc.Infrastructure.ControllerActionInvoker[105]
      Executed action WebApplication.Controllers.CustomerController.NotificationCount (WebApplication) in 63.8546ms
info: Microsoft.AspNetCore.Routing.EndpointMiddleware[1]
      Executed endpoint 'WebApplication.Controllers.CartController.GetCartCount (WebApplication)'
info: Microsoft.AspNetCore.Routing.EndpointMiddleware[1]
      Executed endpoint 'WebApplication.Controllers.CustomerController.NotificationCount (WebApplication)'
info: Microsoft.AspNetCore.Hosting.Diagnostics[2]
      Request finished HTTP/2 GET https://localhost:7177/Cart/GetCartCount - 200 - application/json;+charset=utf-8 75.6045ms
info: Microsoft.AspNetCore.Hosting.Diagnostics[2]
      Request finished HTTP/2 GET https://localhost:7177/Customer/NotificationCount - 200 - application/json;+charset=utf-8 76.9694ms
info: Microsoft.EntityFrameworkCore.Database.Command[20101]
      Executed DbCommand (57ms) [Parameters=[@__p_0='?' (DbType = Int32)], CommandType='Text', CommandTimeout='30']
      SELECT TOP(@__p_0) [n].[NotificationId], [n].[Body], [n].[Channel], [n].[CreatedAt], [n].[FailureReason], [n].[IsRead], [n].[NotifType], [n].[OrderId], [n].[ReadAt], [n].[Recipient], [n].[RetryCount], [n].[SentAt], [n].[Status], [n].[Subject], [n].[TicketId], [n].[UserId]
      FROM [Notification] AS [n]
      WHERE [n].[Status] = N'Pending' AND [n].[RetryCount] < 3 AND [n].[Channel] = N'Email'
      ORDER BY [n].[CreatedAt]
info: Microsoft.EntityFrameworkCore.Database.Command[20101]
      Executed DbCommand (55ms) [Parameters=[], CommandType='Text', CommandTimeout='30']
      SELECT COUNT(*)
      FROM [ProductVariant] AS [p]
info: Microsoft.AspNetCore.Hosting.Diagnostics[1]
      Request starting HTTP/2 POST https://localhost:7177/Cart/AddToCart - application/json 37
info: Microsoft.AspNetCore.Cors.Infrastructure.CorsService[4]
      CORS policy execution successful.
info: Microsoft.AspNetCore.Routing.EndpointMiddleware[0]
      Executing endpoint 'WebApplication.Controllers.CartController.AddToCart (WebApplication)'
info: Microsoft.AspNetCore.Mvc.Infrastructure.ControllerActionInvoker[102]
      Route matched with {action = "AddToCart", controller = "Cart"}. Executing controller action with signature System.Threading.Tasks.Task`1[Microsoft.AspNetCore.Mvc.IActionResult] AddToCart(AddToCartRequest, System.Threading.CancellationToken) on controller WebApplication.Controllers.CartController (WebApplication).
info: Microsoft.EntityFrameworkCore.Database.Command[20101]
      Executed DbCommand (56ms) [Parameters=[@__variantId_0='?' (DbType = Int32)], CommandType='Text', CommandTimeout='30']
      SELECT TOP(1) [p].[ProductVariantId], [p].[AdditionalPrice], [p].[CreatedAt], [p].[IsActive], [p].[ProductId], [p].[ReorderThreshold], [p].[SKU], [p].[StockQuantity], [p].[UpdatedAt], [p].[VariantName]
      FROM [ProductVariant] AS [p]
      WHERE [p].[ProductVariantId] = @__variantId_0 AND [p].[IsActive] = CAST(1 AS bit)
info: Microsoft.EntityFrameworkCore.Database.Command[20101]
      Executed DbCommand (55ms) [Parameters=[@__userId_Value_0='?' (DbType = Int32)], CommandType='Text', CommandTimeout='30']
      SELECT TOP(1) [c].[CartId], [c].[CreatedAt], [c].[GuestSessionId], [c].[IsCheckedOut], [c].[LastUpdatedAt], [c].[UserId]
      FROM [Cart] AS [c]
      WHERE [c].[IsCheckedOut] = CAST(0 AS bit) AND [c].[UserId] = @__userId_Value_0
      ORDER BY [c].[CartId]
info: Microsoft.EntityFrameworkCore.Database.Command[20101]
      Executed DbCommand (59ms) [Parameters=[@p0='?' (DbType = DateTime2), @p1='?' (DbType = Int32), @p2='?' (DbType = Boolean), @p3='?' (DbType = DateTime2), @p4='?' (DbType = Int32)], CommandType='Text', CommandTimeout='30']
      SET IMPLICIT_TRANSACTIONS OFF;
      SET NOCOUNT ON;
      INSERT INTO [Cart] ([CreatedAt], [GuestSessionId], [IsCheckedOut], [LastUpdatedAt], [UserId])
      OUTPUT INSERTED.[CartId]
      VALUES (@p0, @p1, @p2, @p3, @p4);
info: Microsoft.EntityFrameworkCore.Database.Command[20101]
      Executed DbCommand (56ms) [Parameters=[@__cart_CartId_0='?' (DbType = Int32), @__productId_1='?' (DbType = Int32), @__resolvedVariantId_2='?' (DbType = Int32)], CommandType='Text', CommandTimeout='30']
      SELECT TOP(1) [c].[CartItemId], [c].[AddedAt], [c].[CartId], [c].[PriceAtAdd], [c].[ProductId], [c].[ProductVariantId], [c].[Quantity]
      FROM [CartItem] AS [c]
      WHERE [c].[CartId] = @__cart_CartId_0 AND [c].[ProductId] = @__productId_1 AND [c].[ProductVariantId] = @__resolvedVariantId_2
info: Microsoft.EntityFrameworkCore.Database.Command[20101]
      Executed DbCommand (56ms) [Parameters=[@__variantId_0='?' (DbType = Int32)], CommandType='Text', CommandTimeout='30']
      SELECT TOP(1) [p].[ProductVariantId], [p].[AdditionalPrice], [p].[CreatedAt], [p].[IsActive], [p].[ProductId], [p].[ReorderThreshold], [p].[SKU], [p].[StockQuantity], [p].[UpdatedAt], [p].[VariantName], [p0].[ProductId], [p0].[AdditionalSpecs], [p0].[AxleStandard], [p0].[BoostCompatible], [p0].[BrakeType], [p0].[BrandId], [p0].[CategoryId], [p0].[Color], [p0].[CreatedAt], [p0].[Currency], [p0].[Description], [p0].[IsActive], [p0].[IsFeatured], [p0].[Material], [p0].[Name], [p0].[Price], [p0].[SKU], [p0].[ShortDescription], [p0].[SpeedCompatibility], [p0].[SuspensionTravel], [p0].[TubelessReady], [p0].[UpdatedAt], [p0].[WheelSize]
      FROM [ProductVariant] AS [p]
      INNER JOIN [Product] AS [p0] ON [p].[ProductId] = [p0].[ProductId]
      WHERE [p].[ProductVariantId] = @__variantId_0 AND [p].[IsActive] = CAST(1 AS bit)
info: Microsoft.EntityFrameworkCore.Database.Command[20101]
      Executed DbCommand (56ms) [Parameters=[@p1='?' (DbType = Int32), @p0='?' (DbType = DateTime2), @p2='?' (DbType = DateTime2), @p3='?' (DbType = Int32), @p4='?' (Precision = 18) (Scale = 2) (DbType = Decimal), @p5='?' (DbType = Int32), @p6='?' (DbType = Int32), @p7='?' (DbType = Int32)], CommandType='Text', CommandTimeout='30']
      SET NOCOUNT ON;
      UPDATE [Cart] SET [LastUpdatedAt] = @p0
      OUTPUT 1
      WHERE [CartId] = @p1;
      INSERT INTO [CartItem] ([AddedAt], [CartId], [PriceAtAdd], [ProductId], [ProductVariantId], [Quantity])
      OUTPUT INSERTED.[CartItemId]
      VALUES (@p2, @p3, @p4, @p5, @p6, @p7);
info: Microsoft.EntityFrameworkCore.Database.Command[20101]
      Executed DbCommand (55ms) [Parameters=[@__userId_Value_0='?' (DbType = Int32)], CommandType='Text', CommandTimeout='30']
      SELECT TOP(1) [c].[CartId], [c].[CreatedAt], [c].[GuestSessionId], [c].[IsCheckedOut], [c].[LastUpdatedAt], [c].[UserId]
      FROM [Cart] AS [c]
      WHERE [c].[IsCheckedOut] = CAST(0 AS bit) AND [c].[UserId] = @__userId_Value_0
      ORDER BY [c].[CartId]
info: Microsoft.EntityFrameworkCore.Database.Command[20101]
      Executed DbCommand (67ms) [Parameters=[@__userId_Value_0='?' (DbType = Int32)], CommandType='Text', CommandTimeout='30']
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
      Executed DbCommand (55ms) [Parameters=[@__cartId_0='?' (DbType = Int32)], CommandType='Text', CommandTimeout='30']
      SELECT COALESCE(SUM([c].[Quantity]), 0)
      FROM [CartItem] AS [c]
      WHERE [c].[CartId] = @__cartId_0
info: Microsoft.AspNetCore.Mvc.Infrastructure.SystemTextJsonResultExecutor[1]
      Executing JsonResult, writing value of type 'WebApplication.Models.ApiResponse'.
info: Microsoft.AspNetCore.Mvc.Infrastructure.ControllerActionInvoker[105]
      Executed action WebApplication.Controllers.CartController.AddToCart (WebApplication) in 819.3651ms
info: Microsoft.AspNetCore.Routing.EndpointMiddleware[1]
      Executed endpoint 'WebApplication.Controllers.CartController.AddToCart (WebApplication)'
info: Microsoft.AspNetCore.Hosting.Diagnostics[2]
      Request finished HTTP/2 POST https://localhost:7177/Cart/AddToCart - 200 - application/json;+charset=utf-8 831.1889ms
info: Microsoft.AspNetCore.Hosting.Diagnostics[1]
      Request starting HTTP/2 GET https://localhost:7177/Cart/GetCartCount - - -
info: Microsoft.AspNetCore.Routing.EndpointMiddleware[0]
      Executing endpoint 'WebApplication.Controllers.CartController.GetCartCount (WebApplication)'
info: Microsoft.AspNetCore.Mvc.Infrastructure.ControllerActionInvoker[102]
      Route matched with {action = "GetCartCount", controller = "Cart"}. Executing controller action with signature System.Threading.Tasks.Task`1[Microsoft.AspNetCore.Mvc.IActionResult] GetCartCount(System.Threading.CancellationToken) on controller WebApplication.Controllers.CartController (WebApplication).
info: Microsoft.EntityFrameworkCore.Database.Command[20101]
      Executed DbCommand (55ms) [Parameters=[@__userId_Value_0='?' (DbType = Int32)], CommandType='Text', CommandTimeout='30']
      SELECT TOP(1) [c].[CartId], [c].[CreatedAt], [c].[GuestSessionId], [c].[IsCheckedOut], [c].[LastUpdatedAt], [c].[UserId]
      FROM [Cart] AS [c]
      WHERE [c].[IsCheckedOut] = CAST(0 AS bit) AND [c].[UserId] = @__userId_Value_0
      ORDER BY [c].[CartId]
info: Microsoft.EntityFrameworkCore.Database.Command[20101]
      Executed DbCommand (56ms) [Parameters=[@__userId_Value_0='?' (DbType = Int32)], CommandType='Text', CommandTimeout='30']
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
      Executed DbCommand (55ms) [Parameters=[@__cartId_0='?' (DbType = Int32)], CommandType='Text', CommandTimeout='30']
      SELECT COALESCE(SUM([c].[Quantity]), 0)
      FROM [CartItem] AS [c]
      WHERE [c].[CartId] = @__cartId_0
info: Microsoft.AspNetCore.Mvc.Infrastructure.SystemTextJsonResultExecutor[1]
      Executing JsonResult, writing value of type 'WebApplication.Models.ApiResponse'.
info: Microsoft.AspNetCore.Mvc.Infrastructure.ControllerActionInvoker[105]
      Executed action WebApplication.Controllers.CartController.GetCartCount (WebApplication) in 233.4112ms
info: Microsoft.AspNetCore.Routing.EndpointMiddleware[1]
      Executed endpoint 'WebApplication.Controllers.CartController.GetCartCount (WebApplication)'
info: Microsoft.AspNetCore.Hosting.Diagnostics[2]
      Request finished HTTP/2 GET https://localhost:7177/Cart/GetCartCount - 200 - application/json;+charset=utf-8 240.7765ms
info: Microsoft.AspNetCore.Hosting.Diagnostics[1]
      Request starting HTTP/2 GET https://localhost:7177/Checkout - - -
info: Microsoft.AspNetCore.Routing.EndpointMiddleware[0]
      Executing endpoint 'WebApplication.Controllers.CheckoutController.Index (WebApplication)'
info: Microsoft.AspNetCore.Mvc.Infrastructure.ControllerActionInvoker[102]
      Route matched with {action = "Index", controller = "Checkout"}. Executing controller action with signature System.Threading.Tasks.Task`1[Microsoft.AspNetCore.Mvc.IActionResult] Index(System.Threading.CancellationToken) on controller WebApplication.Controllers.CheckoutController (WebApplication).
info: Microsoft.EntityFrameworkCore.Database.Command[20101]
      Executed DbCommand (56ms) [Parameters=[@__userId_Value_0='?' (DbType = Int32)], CommandType='Text', CommandTimeout='30']
      SELECT TOP(1) [c].[CartId], [c].[CreatedAt], [c].[GuestSessionId], [c].[IsCheckedOut], [c].[LastUpdatedAt], [c].[UserId]
      FROM [Cart] AS [c]
      WHERE [c].[IsCheckedOut] = CAST(0 AS bit) AND [c].[UserId] = @__userId_Value_0
      ORDER BY [c].[CartId]
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
      Executed DbCommand (56ms) [Parameters=[@__userId_0='?' (DbType = Int32)], CommandType='Text', CommandTimeout='30']
      SELECT TOP(1) [u].[UserId], [u].[CreatedAt], [u].[DefaultAddressId], [u].[Email], [u].[FailedLoginAttempts], [u].[FirstName], [u].[IsActive], [u].[IsDeleted], [u].[IsWalkIn], [u].[LastLoginAt], [u].[LastName], [u].[LockoutUntil], [u].[PasswordHash], [u].[PhoneNumber]
      FROM [User] AS [u]
      WHERE [u].[IsDeleted] = CAST(0 AS bit) AND [u].[UserId] = @__userId_0
      ORDER BY [u].[UserId]
info: Microsoft.EntityFrameworkCore.Database.Command[20101]
      Executed DbCommand (56ms) [Parameters=[@__userId_0='?' (DbType = Int32)], CommandType='Text', CommandTimeout='30']
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
info: Microsoft.AspNetCore.Mvc.ViewFeatures.ViewResultExecutor[1]
      Executing ViewResult, running view ~/Views/Customer/Checkout.cshtml.
info: Microsoft.AspNetCore.Mvc.ViewFeatures.ViewResultExecutor[4]
      Executed ViewResult - view ~/Views/Customer/Checkout.cshtml executed in 18.184ms.
info: Microsoft.AspNetCore.Mvc.Infrastructure.ControllerActionInvoker[105]
      Executed action WebApplication.Controllers.CheckoutController.Index (WebApplication) in 327.1597ms
info: Microsoft.AspNetCore.Routing.EndpointMiddleware[1]
      Executed endpoint 'WebApplication.Controllers.CheckoutController.Index (WebApplication)'
info: Microsoft.AspNetCore.Hosting.Diagnostics[2]
      Request finished HTTP/2 GET https://localhost:7177/Checkout - 200 - text/html;+charset=utf-8 339.1306ms
info: Microsoft.AspNetCore.Hosting.Diagnostics[1]
      Request starting HTTP/2 GET https://localhost:7177/_framework/aspnetcore-browser-refresh.js - - -
info: Microsoft.AspNetCore.Hosting.Diagnostics[1]
      Request starting HTTP/2 GET https://localhost:7177/_vs/browserLink - - -
info: Microsoft.AspNetCore.Hosting.Diagnostics[2]
      Request finished HTTP/2 GET https://localhost:7177/_framework/aspnetcore-browser-refresh.js - 200 14933 application/javascript;+charset=utf-8 3.9616ms
info: Microsoft.AspNetCore.Hosting.Diagnostics[2]
      Request finished HTTP/2 GET https://localhost:7177/_vs/browserLink - 200 - text/javascript;+charset=UTF-8 12.0737ms
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
      Executed DbCommand (55ms) [Parameters=[@__userId_0='?' (DbType = Int32)], CommandType='Text', CommandTimeout='30']
      SELECT COUNT(*)
      FROM [Notification] AS [n]
      WHERE [n].[UserId] = @__userId_0 AND [n].[IsRead] = CAST(0 AS bit)
info: Microsoft.EntityFrameworkCore.Database.Command[20101]
      Executed DbCommand (57ms) [Parameters=[@__userId_Value_0='?' (DbType = Int32)], CommandType='Text', CommandTimeout='30']
      SELECT TOP(1) [c].[CartId], [c].[CreatedAt], [c].[GuestSessionId], [c].[IsCheckedOut], [c].[LastUpdatedAt], [c].[UserId]
      FROM [Cart] AS [c]
      WHERE [c].[IsCheckedOut] = CAST(0 AS bit) AND [c].[UserId] = @__userId_Value_0
      ORDER BY [c].[CartId]
info: Microsoft.AspNetCore.Mvc.Infrastructure.SystemTextJsonResultExecutor[1]
      Executing JsonResult, writing value of type '<>f__AnonymousType6`1[[System.Int32, System.Private.CoreLib, Version=8.0.0.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e]]'.
info: Microsoft.AspNetCore.Mvc.Infrastructure.ControllerActionInvoker[105]
      Executed action WebApplication.Controllers.CustomerController.NotificationCount (WebApplication) in 62.1229ms
info: Microsoft.AspNetCore.Routing.EndpointMiddleware[1]
      Executed endpoint 'WebApplication.Controllers.CustomerController.NotificationCount (WebApplication)'
info: Microsoft.AspNetCore.Hosting.Diagnostics[2]
      Request finished HTTP/2 GET https://localhost:7177/Customer/NotificationCount - 200 - application/json;+charset=utf-8 72.7987ms
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
      Executed DbCommand (56ms) [Parameters=[@__cartId_0='?' (DbType = Int32)], CommandType='Text', CommandTimeout='30']
      SELECT COALESCE(SUM([c].[Quantity]), 0)
      FROM [CartItem] AS [c]
      WHERE [c].[CartId] = @__cartId_0
info: Microsoft.AspNetCore.Mvc.Infrastructure.SystemTextJsonResultExecutor[1]
      Executing JsonResult, writing value of type 'WebApplication.Models.ApiResponse'.
info: Microsoft.AspNetCore.Mvc.Infrastructure.ControllerActionInvoker[105]
      Executed action WebApplication.Controllers.CartController.GetCartCount (WebApplication) in 239.1193ms
info: Microsoft.AspNetCore.Routing.EndpointMiddleware[1]
      Executed endpoint 'WebApplication.Controllers.CartController.GetCartCount (WebApplication)'
info: Microsoft.AspNetCore.Hosting.Diagnostics[2]
      Request finished HTTP/2 GET https://localhost:7177/Cart/GetCartCount - 200 - application/json;+charset=utf-8 251.4489ms
info: WebApplication.BusinessLogic.Services.GmailEmailSender[0]
      Email sent to geronimojoan002@gmail.com - Subject: Welcome to Taurus Bike Shop!
info: WebApplication.BackgroundJobs.NotificationDispatchJob[0]
      Notification 4 sent to geronimojoan002@gmail.com (WelcomeEmail).
info: Microsoft.EntityFrameworkCore.Database.Command[20101]
      Executed DbCommand (58ms) [Parameters=[@p2='?' (DbType = Int32), @p0='?' (DbType = DateTime2), @p1='?' (Size = 20)], CommandType='Text', CommandTimeout='30']
      SET IMPLICIT_TRANSACTIONS OFF;
      SET NOCOUNT ON;
      UPDATE [Notification] SET [SentAt] = @p0, [Status] = @p1
      OUTPUT 1
      WHERE [NotificationId] = @p2;
info: Microsoft.EntityFrameworkCore.Database.Command[20101]
      Executed DbCommand (57ms) [Parameters=[], CommandType='Text', CommandTimeout='30']
      SELECT COUNT(*)
      FROM [ProductVariant] AS [p]
info: Microsoft.AspNetCore.Hosting.Diagnostics[1]
      Request starting HTTP/2 POST https://localhost:7177/Checkout/PlaceOrder - application/x-www-form-urlencoded 319
info: Microsoft.AspNetCore.Cors.Infrastructure.CorsService[4]
      CORS policy execution successful.
info: Microsoft.AspNetCore.Routing.EndpointMiddleware[0]
      Executing endpoint 'WebApplication.Controllers.CheckoutController.PlaceOrder (WebApplication)'
info: Microsoft.AspNetCore.Mvc.Infrastructure.ControllerActionInvoker[102]
      Route matched with {action = "PlaceOrder", controller = "Checkout"}. Executing controller action with signature System.Threading.Tasks.Task`1[Microsoft.AspNetCore.Mvc.IActionResult] PlaceOrder(WebApplication.Models.ViewModels.CheckoutViewModel, System.Threading.CancellationToken) on controller WebApplication.Controllers.CheckoutController (WebApplication).
info: Microsoft.EntityFrameworkCore.Database.Command[20101]
      Executed DbCommand (57ms) [Parameters=[@__userId_Value_0='?' (DbType = Int32)], CommandType='Text', CommandTimeout='30']
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
      Executed DbCommand (57ms) [Parameters=[@__item_ProductVariantId_0='?' (DbType = Int32)], CommandType='Text', CommandTimeout='30']
      SELECT TOP(1) [p].[ProductVariantId], [p].[AdditionalPrice], [p].[CreatedAt], [p].[IsActive], [p].[ProductId], [p].[ReorderThreshold], [p].[SKU], [p].[StockQuantity], [p].[UpdatedAt], [p].[VariantName]
      FROM [ProductVariant] AS [p]
      WHERE [p].[ProductVariantId] = @__item_ProductVariantId_0
info: Microsoft.EntityFrameworkCore.Database.Command[20101]
      Executed DbCommand (61ms) [Parameters=[@__8__locals1_vm_SelectedAddressId_0='?' (DbType = Int32), @__8__locals1_userId_1='?' (DbType = Int32)], CommandType='Text', CommandTimeout='30']
      SELECT TOP(1) [a].[AddressId], [a].[City], [a].[Country], [a].[CreatedAt], [a].[IsDefault], [a].[IsSnapshot], [a].[Label], [a].[PostalCode], [a].[Province], [a].[Street], [a].[UserId]
      FROM [Address] AS [a]
      WHERE [a].[AddressId] = @__8__locals1_vm_SelectedAddressId_0 AND [a].[UserId] = @__8__locals1_userId_1 AND [a].[IsSnapshot] = CAST(0 AS bit)
warn: Microsoft.EntityFrameworkCore.Database.Transaction[30004]
      Savepoints are disabled because Multiple Active Result Sets (MARS) is enabled. If 'SaveChanges' fails, then the transaction cannot be automatically rolled back to a known clean state. Instead, the transaction should be rolled back by the application before retrying 'SaveChanges'. See https://go.microsoft.com/fwlink/?linkid=2149338 for more information and examples. To identify the code which triggers this warning, call 'ConfigureWarnings(w => w.Throw(SqlServerEventId.SavepointsDisabledBecauseOfMARS))'.
info: Microsoft.EntityFrameworkCore.Database.Command[20101]
      Executed DbCommand (60ms) [Parameters=[@p0='?' (Size = 100), @p1='?' (Size = 100), @p2='?' (DbType = DateTime2), @p3='?' (DbType = Boolean), @p4='?' (DbType = Boolean), @p5='?' (Size = 50), @p6='?' (Size = 20), @p7='?' (Size = 100), @p8='?' (Size = 500), @p9='?' (DbType = Int32)], CommandType='Text', CommandTimeout='30']
      SET IMPLICIT_TRANSACTIONS OFF;
      SET NOCOUNT ON;
      INSERT INTO [Address] ([City], [Country], [CreatedAt], [IsDefault], [IsSnapshot], [Label], [PostalCode], [Province], [Street], [UserId])
      OUTPUT INSERTED.[AddressId]
      VALUES (@p0, @p1, @p2, @p3, @p4, @p5, @p6, @p7, @p8, @p9);
info: Microsoft.EntityFrameworkCore.Database.Command[20101]
      Executed DbCommand (61ms) [Parameters=[], CommandType='Text', CommandTimeout='30']
      SELECT COUNT(*)
      FROM [Order] AS [o]
      WHERE [o].[IsDeleted] = CAST(0 AS bit)
warn: Microsoft.EntityFrameworkCore.Database.Transaction[30004]
      Savepoints are disabled because Multiple Active Result Sets (MARS) is enabled. If 'SaveChanges' fails, then the transaction cannot be automatically rolled back to a known clean state. Instead, the transaction should be rolled back by the application before retrying 'SaveChanges'. See https://go.microsoft.com/fwlink/?linkid=2149338 for more information and examples. To identify the code which triggers this warning, call 'ConfigureWarnings(w => w.Throw(SqlServerEventId.SavepointsDisabledBecauseOfMARS))'.
info: Microsoft.EntityFrameworkCore.Database.Command[20101]
      Executed DbCommand (67ms) [Parameters=[@p0='?' (DbType = Int32), @p1='?' (Size = 20), @p2='?' (DbType = DateTime2), @p3='?' (Size = 500), @p4='?' (Precision = 18) (Scale = 2) (DbType = Decimal), @p5='?' (Size = 20), @p6='?' (DbType = Int32), @p7='?' (DbType = Boolean), @p8='?' (DbType = Boolean), @p9='?' (DbType = DateTime2), @p10='?' (Size = 50), @p11='?' (Size = 50), @p12='?' (DbType = Int32), @p13='?' (DbType = Int32), @p14='?' (Precision = 18) (Scale = 2) (DbType = Decimal), @p15='?' (Precision = 18) (Scale = 2) (DbType = Decimal), @p16='?' (DbType = DateTime2), @p17='?' (DbType = Int32)], CommandType='Text', CommandTimeout='30']
      SET IMPLICIT_TRANSACTIONS OFF;
      SET NOCOUNT ON;
      INSERT INTO [Order] ([CartId], [ContactPhone], [CreatedAt], [DeliveryInstructions], [DiscountAmount], [FulfillmentType], [GuestSessionId], [IsDeleted], [IsWalkIn], [OrderDate], [OrderNumber], [OrderStatus], [POSSessionId], [ShippingAddressId], [ShippingFee], [SubTotal], [UpdatedAt], [UserId])
      OUTPUT INSERTED.[OrderId], INSERTED.[TotalAmount]
      VALUES (@p0, @p1, @p2, @p3, @p4, @p5, @p6, @p7, @p8, @p9, @p10, @p11, @p12, @p13, @p14, @p15, @p16, @p17);
warn: Microsoft.EntityFrameworkCore.Database.Transaction[30004]
      Savepoints are disabled because Multiple Active Result Sets (MARS) is enabled. If 'SaveChanges' fails, then the transaction cannot be automatically rolled back to a known clean state. Instead, the transaction should be rolled back by the application before retrying 'SaveChanges'. See https://go.microsoft.com/fwlink/?linkid=2149338 for more information and examples. To identify the code which triggers this warning, call 'ConfigureWarnings(w => w.Throw(SqlServerEventId.SavepointsDisabledBecauseOfMARS))'.
info: Microsoft.EntityFrameworkCore.Database.Command[20101]
      Executed DbCommand (57ms) [Parameters=[@p0='?' (DbType = Int32), @p1='?' (DbType = Int32), @p2='?' (DbType = Int32), @p3='?' (DbType = Int32), @p4='?' (Precision = 18) (Scale = 2) (DbType = Decimal)], CommandType='Text', CommandTimeout='30']
      SET IMPLICIT_TRANSACTIONS OFF;
      SET NOCOUNT ON;
      INSERT INTO [OrderItem] ([OrderId], [ProductId], [ProductVariantId], [Quantity], [UnitPrice])
      OUTPUT INSERTED.[OrderItemId]
      VALUES (@p0, @p1, @p2, @p3, @p4);
info: Microsoft.EntityFrameworkCore.Database.Command[20101]
      Executed DbCommand (58ms) [Parameters=[@__cartItem_ProductVariantId_0='?' (DbType = Int32)], CommandType='Text', CommandTimeout='30']
      SELECT TOP(1) [p].[ProductVariantId], [p].[AdditionalPrice], [p].[CreatedAt], [p].[IsActive], [p].[ProductId], [p].[ReorderThreshold], [p].[SKU], [p].[StockQuantity], [p].[UpdatedAt], [p].[VariantName]
      FROM [ProductVariant] AS [p]
      WHERE [p].[ProductVariantId] = @__cartItem_ProductVariantId_0
warn: Microsoft.EntityFrameworkCore.Database.Transaction[30004]
      Savepoints are disabled because Multiple Active Result Sets (MARS) is enabled. If 'SaveChanges' fails, then the transaction cannot be automatically rolled back to a known clean state. Instead, the transaction should be rolled back by the application before retrying 'SaveChanges'. See https://go.microsoft.com/fwlink/?linkid=2149338 for more information and examples. To identify the code which triggers this warning, call 'ConfigureWarnings(w => w.Throw(SqlServerEventId.SavepointsDisabledBecauseOfMARS))'.
info: Microsoft.EntityFrameworkCore.Database.Command[20101]
      Executed DbCommand (58ms) [Parameters=[@p0='?' (DbType = Int32), @p1='?' (Size = 50), @p2='?' (DbType = Int32), @p3='?' (DbType = DateTime2), @p4='?' (Size = 500), @p5='?' (DbType = Int32), @p6='?' (DbType = Int32), @p7='?' (DbType = Int32), @p8='?' (DbType = Int32), @p10='?' (DbType = Int32), @p9='?' (DbType = Int32)], CommandType='Text', CommandTimeout='30']
      SET NOCOUNT ON;
      INSERT INTO [InventoryLog] ([ChangeQuantity], [ChangeType], [ChangedByUserId], [CreatedAt], [Notes], [OrderId], [ProductId], [ProductVariantId], [PurchaseOrderId])
      OUTPUT INSERTED.[InventoryLogId]
      VALUES (@p0, @p1, @p2, @p3, @p4, @p5, @p6, @p7, @p8);
      UPDATE [ProductVariant] SET [StockQuantity] = @p9
      OUTPUT 1
      WHERE [ProductVariantId] = @p10;
warn: Microsoft.EntityFrameworkCore.Database.Transaction[30004]
      Savepoints are disabled because Multiple Active Result Sets (MARS) is enabled. If 'SaveChanges' fails, then the transaction cannot be automatically rolled back to a known clean state. Instead, the transaction should be rolled back by the application before retrying 'SaveChanges'. See https://go.microsoft.com/fwlink/?linkid=2149338 for more information and examples. To identify the code which triggers this warning, call 'ConfigureWarnings(w => w.Throw(SqlServerEventId.SavepointsDisabledBecauseOfMARS))'.
info: Microsoft.EntityFrameworkCore.Database.Command[20101]
      Executed DbCommand (57ms) [Parameters=[@p2='?' (DbType = Int32), @p0='?' (DbType = Boolean), @p1='?' (DbType = DateTime2), @p3='?' (DbType = Int32)], CommandType='Text', CommandTimeout='30']
      SET NOCOUNT ON;
      UPDATE [Cart] SET [IsCheckedOut] = @p0, [LastUpdatedAt] = @p1
      OUTPUT 1
      WHERE [CartId] = @p2;
      DELETE FROM [CartItem]
      OUTPUT 1
      WHERE [CartItemId] = @p3;
warn: Microsoft.EntityFrameworkCore.Database.Transaction[30004]
      Savepoints are disabled because Multiple Active Result Sets (MARS) is enabled. If 'SaveChanges' fails, then the transaction cannot be automatically rolled back to a known clean state. Instead, the transaction should be rolled back by the application before retrying 'SaveChanges'. See https://go.microsoft.com/fwlink/?linkid=2149338 for more information and examples. To identify the code which triggers this warning, call 'ConfigureWarnings(w => w.Throw(SqlServerEventId.SavepointsDisabledBecauseOfMARS))'.
info: Microsoft.EntityFrameworkCore.Database.Command[20101]
      Executed DbCommand (57ms) [Parameters=[@p0='?' (DbType = DateTime2), @p1='?' (Size = 1000), @p2='?' (Size = 100), @p3='?' (DbType = Int32)], CommandType='Text', CommandTimeout='30']
      SET IMPLICIT_TRANSACTIONS OFF;
      SET NOCOUNT ON;
      INSERT INTO [SystemLog] ([CreatedAt], [EventDescription], [EventType], [UserId])
      OUTPUT INSERTED.[SystemLogId]
      VALUES (@p0, @p1, @p2, @p3);
info: Microsoft.EntityFrameworkCore.Database.Command[20101]
      Executed DbCommand (60ms) [Parameters=[@__8__locals1_userId_0='?' (DbType = Int32)], CommandType='Text', CommandTimeout='30']
      SELECT TOP(1) [u].[UserId], [u].[CreatedAt], [u].[DefaultAddressId], [u].[Email], [u].[FailedLoginAttempts], [u].[FirstName], [u].[IsActive], [u].[IsDeleted], [u].[IsWalkIn], [u].[LastLoginAt], [u].[LastName], [u].[LockoutUntil], [u].[PasswordHash], [u].[PhoneNumber]
      FROM [User] AS [u]
      WHERE [u].[IsDeleted] = CAST(0 AS bit) AND [u].[UserId] = @__8__locals1_userId_0
info: Microsoft.EntityFrameworkCore.Database.Command[20101]
      Executed DbCommand (60ms) [Parameters=[@p0='?' (Size = 4000), @p1='?' (Size = 20), @p2='?' (DbType = DateTime2), @p3='?' (Size = 500), @p4='?' (DbType = Boolean), @p5='?' (Size = 100), @p6='?' (DbType = Int32), @p7='?' (DbType = DateTime2), @p8='?' (Size = 255), @p9='?' (DbType = Int32), @p10='?' (DbType = DateTime2), @p11='?' (Size = 20), @p12='?' (Size = 255), @p13='?' (DbType = Int32), @p14='?' (DbType = Int32)], CommandType='Text', CommandTimeout='30']
      SET IMPLICIT_TRANSACTIONS OFF;
      SET NOCOUNT ON;
      INSERT INTO [Notification] ([Body], [Channel], [CreatedAt], [FailureReason], [IsRead], [NotifType], [OrderId], [ReadAt], [Recipient], [RetryCount], [SentAt], [Status], [Subject], [TicketId], [UserId])
      OUTPUT INSERTED.[NotificationId]
      VALUES (@p0, @p1, @p2, @p3, @p4, @p5, @p6, @p7, @p8, @p9, @p10, @p11, @p12, @p13, @p14);
info: Microsoft.AspNetCore.Mvc.RedirectToActionResult[1]
      Executing RedirectResult, redirecting to /Order/Confirmation?orderId=3.
info: Microsoft.AspNetCore.Mvc.Infrastructure.ControllerActionInvoker[105]
      Executed action WebApplication.Controllers.CheckoutController.PlaceOrder (WebApplication) in 1167.711ms
info: Microsoft.AspNetCore.Routing.EndpointMiddleware[1]
      Executed endpoint 'WebApplication.Controllers.CheckoutController.PlaceOrder (WebApplication)'
info: Microsoft.AspNetCore.Hosting.Diagnostics[2]
      Request finished HTTP/2 POST https://localhost:7177/Checkout/PlaceOrder - 302 - - 1200.0813ms
info: Microsoft.AspNetCore.Hosting.Diagnostics[1]
      Request starting HTTP/2 GET https://localhost:7177/Order/Confirmation?orderId=3 - - -
info: Microsoft.AspNetCore.Routing.EndpointMiddleware[0]
      Executing endpoint 'WebApplication.Controllers.OrderController.Confirmation (WebApplication)'
info: Microsoft.AspNetCore.Mvc.Infrastructure.ControllerActionInvoker[102]
      Route matched with {action = "Confirmation", controller = "Order"}. Executing controller action with signature System.Threading.Tasks.Task`1[Microsoft.AspNetCore.Mvc.IActionResult] Confirmation(Int32, System.Threading.CancellationToken) on controller WebApplication.Controllers.OrderController (WebApplication).
info: Microsoft.EntityFrameworkCore.Database.Command[20101]
      Executed DbCommand (58ms) [Parameters=[@__orderId_0='?' (DbType = Int32)], CommandType='Text', CommandTimeout='30']
      SELECT TOP(1) [o].[OrderId], [o].[CartId], [o].[ContactPhone], [o].[CreatedAt], [o].[DeliveryInstructions], [o].[DiscountAmount], [o].[FulfillmentType], [o].[GuestSessionId], [o].[IsDeleted], [o].[IsWalkIn], [o].[OrderDate], [o].[OrderNumber], [o].[OrderStatus], [o].[POSSessionId], [o].[ShippingAddressId], [o].[ShippingFee], [o].[SubTotal], [o].[TotalAmount], [o].[UpdatedAt], [o].[UserId], [p].[PickupOrderId], [a].[AddressId], [p].[OrderId], [p].[PickupConfirmedAt], [p].[PickupExpiresAt], [p].[PickupReadyAt], [a].[City], [a].[Country], [a].[CreatedAt], [a].[IsDefault], [a].[IsSnapshot], [a].[Label], [a].[PostalCode], [a].[Province], [a].[Street], [a].[UserId]
      FROM [Order] AS [o]
      LEFT JOIN [PickupOrder] AS [p] ON [o].[OrderId] = [p].[OrderId]
      LEFT JOIN [Address] AS [a] ON [o].[ShippingAddressId] = [a].[AddressId]
      WHERE [o].[IsDeleted] = CAST(0 AS bit) AND [o].[OrderId] = @__orderId_0
      ORDER BY [o].[OrderId], [p].[PickupOrderId], [a].[AddressId]
info: Microsoft.EntityFrameworkCore.Database.Command[20101]
      Executed DbCommand (58ms) [Parameters=[@__orderId_0='?' (DbType = Int32)], CommandType='Text', CommandTimeout='30']
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
      Executed DbCommand (58ms) [Parameters=[@__orderId_0='?' (DbType = Int32)], CommandType='Text', CommandTimeout='30']
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
      Executed DbCommand (57ms) [Parameters=[@__orderId_0='?' (DbType = Int32)], CommandType='Text', CommandTimeout='30']
      SELECT [t0].[PaymentId], [t0].[Amount], [t0].[CreatedAt], [t0].[OrderId], [t0].[PaymentDate], [t0].[PaymentMethod], [t0].[PaymentStage], [t0].[PaymentStatus], [t0].[PaymentId0], [t0].[GcashTransactionId], [t0].[ScreenshotUrl], [t0].[StorageBucket], [t0].[StoragePath], [t0].[SubmittedAt], [t0].[PaymentId1], [t0].[BpiReferenceNumber], [t0].[ProofStorageBucket], [t0].[ProofStoragePath], [t0].[ProofUrl], [t0].[SubmittedAt0], [t0].[VerificationDeadline], [t0].[VerificationNotes], [t0].[VerifiedAt], [t0].[VerifiedByUserId], [t].[OrderId], [t].[PickupOrderId], [t].[AddressId]
      FROM (
          SELECT TOP(1) [o].[OrderId], [p].[PickupOrderId], [a].[AddressId]
          FROM [Order] AS [o]
          LEFT JOIN [PickupOrder] AS [p] ON [o].[OrderId] = [p].[OrderId]
          LEFT JOIN [Address] AS [a] ON [o].[ShippingAddressId] = [a].[AddressId]
          WHERE [o].[IsDeleted] = CAST(0 AS bit) AND [o].[OrderId] = @__orderId_0
      ) AS [t]
      INNER JOIN (
          SELECT [p0].[PaymentId], [p0].[Amount], [p0].[CreatedAt], [p0].[OrderId], [p0].[PaymentDate], [p0].[PaymentMethod], [p0].[PaymentStage], [p0].[PaymentStatus], [g].[PaymentId] AS [PaymentId0], [g].[GcashTransactionId], [g].[ScreenshotUrl], [g].[StorageBucket], [g].[StoragePath], [g].[SubmittedAt], [b].[PaymentId] AS [PaymentId1], [b].[BpiReferenceNumber], [b].[ProofStorageBucket], [b].[ProofStoragePath], [b].[ProofUrl], [b].[SubmittedAt] AS [SubmittedAt0], [b].[VerificationDeadline], [b].[VerificationNotes], [b].[VerifiedAt], [b].[VerifiedByUserId]
          FROM [Payment] AS [p0]
          LEFT JOIN [GCashPayment] AS [g] ON [p0].[PaymentId] = [g].[PaymentId]
          LEFT JOIN [BankTransferPayment] AS [b] ON [p0].[PaymentId] = [b].[PaymentId]
      ) AS [t0] ON [t].[OrderId] = [t0].[OrderId]
      ORDER BY [t].[OrderId], [t].[PickupOrderId], [t].[AddressId]
info: Microsoft.EntityFrameworkCore.Database.Command[20101]
      Executed DbCommand (57ms) [Parameters=[@__orderId_0='?' (DbType = Int32)], CommandType='Text', CommandTimeout='30']
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
      Executed ViewResult - view ~/Views/Customer/Confirmation.cshtml executed in 8.3736ms.
info: Microsoft.AspNetCore.Mvc.Infrastructure.ControllerActionInvoker[105]
      Executed action WebApplication.Controllers.OrderController.Confirmation (WebApplication) in 309.4629ms
info: Microsoft.AspNetCore.Routing.EndpointMiddleware[1]
      Executed endpoint 'WebApplication.Controllers.OrderController.Confirmation (WebApplication)'
info: Microsoft.AspNetCore.Hosting.Diagnostics[2]
      Request finished HTTP/2 GET https://localhost:7177/Order/Confirmation?orderId=3 - 200 - text/html;+charset=utf-8 320.0709ms
info: Microsoft.AspNetCore.Hosting.Diagnostics[1]
      Request starting HTTP/2 GET https://localhost:7177/_framework/aspnetcore-browser-refresh.js - - -
info: Microsoft.AspNetCore.Hosting.Diagnostics[1]
      Request starting HTTP/2 GET https://localhost:7177/_vs/browserLink - - -
info: Microsoft.AspNetCore.Hosting.Diagnostics[2]
      Request finished HTTP/2 GET https://localhost:7177/_framework/aspnetcore-browser-refresh.js - 200 14933 application/javascript;+charset=utf-8 3.4992ms
info: Microsoft.AspNetCore.Hosting.Diagnostics[2]
      Request finished HTTP/2 GET https://localhost:7177/_vs/browserLink - 200 - text/javascript;+charset=UTF-8 13.0550ms
info: Microsoft.AspNetCore.Hosting.Diagnostics[1]
      Request starting HTTP/2 GET https://localhost:7177/antiforgery/token - - -
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
info: Microsoft.AspNetCore.Routing.EndpointMiddleware[0]
      Executing endpoint 'HTTP: GET /antiforgery/token'
info: Microsoft.AspNetCore.Http.Result.OkObjectResult[1]
      Setting HTTP status code 200.
info: Microsoft.AspNetCore.Http.Result.OkObjectResult[3]
      Writing value of type '<>f__AnonymousType20`1' as Json.
info: Microsoft.AspNetCore.Routing.EndpointMiddleware[1]
      Executed endpoint 'HTTP: GET /antiforgery/token'
info: Microsoft.AspNetCore.Hosting.Diagnostics[2]
      Request finished HTTP/2 GET https://localhost:7177/antiforgery/token - 200 - application/json;+charset=utf-8 21.2314ms
info: Microsoft.EntityFrameworkCore.Database.Command[20101]
      Executed DbCommand (56ms) [Parameters=[@__userId_0='?' (DbType = Int32)], CommandType='Text', CommandTimeout='30']
      SELECT COUNT(*)
      FROM [Notification] AS [n]
      WHERE [n].[UserId] = @__userId_0 AND [n].[IsRead] = CAST(0 AS bit)
info: Microsoft.EntityFrameworkCore.Database.Command[20101]
      Executed DbCommand (57ms) [Parameters=[@__userId_Value_0='?' (DbType = Int32)], CommandType='Text', CommandTimeout='30']
      SELECT TOP(1) [c].[CartId], [c].[CreatedAt], [c].[GuestSessionId], [c].[IsCheckedOut], [c].[LastUpdatedAt], [c].[UserId]
      FROM [Cart] AS [c]
      WHERE [c].[IsCheckedOut] = CAST(0 AS bit) AND [c].[UserId] = @__userId_Value_0
      ORDER BY [c].[CartId]
info: Microsoft.AspNetCore.Mvc.Infrastructure.SystemTextJsonResultExecutor[1]
      Executing JsonResult, writing value of type '<>f__AnonymousType6`1[[System.Int32, System.Private.CoreLib, Version=8.0.0.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e]]'.
info: Microsoft.AspNetCore.Mvc.Infrastructure.SystemTextJsonResultExecutor[1]
      Executing JsonResult, writing value of type 'WebApplication.Models.ApiResponse'.
info: Microsoft.AspNetCore.Mvc.Infrastructure.ControllerActionInvoker[105]
      Executed action WebApplication.Controllers.CustomerController.NotificationCount (WebApplication) in 63.1127ms
info: Microsoft.AspNetCore.Mvc.Infrastructure.ControllerActionInvoker[105]
      Executed action WebApplication.Controllers.CartController.GetCartCount (WebApplication) in 65.8831ms
info: Microsoft.AspNetCore.Routing.EndpointMiddleware[1]
      Executed endpoint 'WebApplication.Controllers.CustomerController.NotificationCount (WebApplication)'
info: Microsoft.AspNetCore.Routing.EndpointMiddleware[1]
      Executed endpoint 'WebApplication.Controllers.CartController.GetCartCount (WebApplication)'
info: Microsoft.AspNetCore.Hosting.Diagnostics[2]
      Request finished HTTP/2 GET https://localhost:7177/Customer/NotificationCount - 200 - application/json;+charset=utf-8 77.3281ms
info: Microsoft.AspNetCore.Hosting.Diagnostics[2]
      Request finished HTTP/2 GET https://localhost:7177/Cart/GetCartCount - 200 - application/json;+charset=utf-8 78.6316ms
info: Microsoft.AspNetCore.Hosting.Diagnostics[1]
      Request starting HTTP/2 GET https://localhost:7177/Payment/Submit?orderId=3 - - -
info: Microsoft.AspNetCore.Routing.EndpointMiddleware[0]
      Executing endpoint 'WebApplication.Controllers.PaymentController.Submit (WebApplication)'
info: Microsoft.AspNetCore.Mvc.Infrastructure.ControllerActionInvoker[102]
      Route matched with {action = "Submit", controller = "Payment"}. Executing controller action with signature System.Threading.Tasks.Task`1[Microsoft.AspNetCore.Mvc.IActionResult] Submit(Int32, System.Threading.CancellationToken) on controller WebApplication.Controllers.PaymentController (WebApplication).
info: Microsoft.EntityFrameworkCore.Database.Command[20101]
      Executed DbCommand (57ms) [Parameters=[@__orderId_0='?' (DbType = Int32)], CommandType='Text', CommandTimeout='30']
      SELECT TOP(1) [o].[OrderId], [o].[CartId], [o].[ContactPhone], [o].[CreatedAt], [o].[DeliveryInstructions], [o].[DiscountAmount], [o].[FulfillmentType], [o].[GuestSessionId], [o].[IsDeleted], [o].[IsWalkIn], [o].[OrderDate], [o].[OrderNumber], [o].[OrderStatus], [o].[POSSessionId], [o].[ShippingAddressId], [o].[ShippingFee], [o].[SubTotal], [o].[TotalAmount], [o].[UpdatedAt], [o].[UserId], [p].[PickupOrderId], [a].[AddressId], [p].[OrderId], [p].[PickupConfirmedAt], [p].[PickupExpiresAt], [p].[PickupReadyAt], [a].[City], [a].[Country], [a].[CreatedAt], [a].[IsDefault], [a].[IsSnapshot], [a].[Label], [a].[PostalCode], [a].[Province], [a].[Street], [a].[UserId]
      FROM [Order] AS [o]
      LEFT JOIN [PickupOrder] AS [p] ON [o].[OrderId] = [p].[OrderId]
      LEFT JOIN [Address] AS [a] ON [o].[ShippingAddressId] = [a].[AddressId]
      WHERE [o].[IsDeleted] = CAST(0 AS bit) AND [o].[OrderId] = @__orderId_0
      ORDER BY [o].[OrderId], [p].[PickupOrderId], [a].[AddressId]
info: Microsoft.EntityFrameworkCore.Database.Command[20101]
      Executed DbCommand (58ms) [Parameters=[@__orderId_0='?' (DbType = Int32)], CommandType='Text', CommandTimeout='30']
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
      Executed DbCommand (57ms) [Parameters=[@__orderId_0='?' (DbType = Int32)], CommandType='Text', CommandTimeout='30']
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
      Executed DbCommand (59ms) [Parameters=[@__orderId_0='?' (DbType = Int32)], CommandType='Text', CommandTimeout='30']
      SELECT [t0].[PaymentId], [t0].[Amount], [t0].[CreatedAt], [t0].[OrderId], [t0].[PaymentDate], [t0].[PaymentMethod], [t0].[PaymentStage], [t0].[PaymentStatus], [t0].[PaymentId0], [t0].[GcashTransactionId], [t0].[ScreenshotUrl], [t0].[StorageBucket], [t0].[StoragePath], [t0].[SubmittedAt], [t0].[PaymentId1], [t0].[BpiReferenceNumber], [t0].[ProofStorageBucket], [t0].[ProofStoragePath], [t0].[ProofUrl], [t0].[SubmittedAt0], [t0].[VerificationDeadline], [t0].[VerificationNotes], [t0].[VerifiedAt], [t0].[VerifiedByUserId], [t].[OrderId], [t].[PickupOrderId], [t].[AddressId]
      FROM (
          SELECT TOP(1) [o].[OrderId], [p].[PickupOrderId], [a].[AddressId]
          FROM [Order] AS [o]
          LEFT JOIN [PickupOrder] AS [p] ON [o].[OrderId] = [p].[OrderId]
          LEFT JOIN [Address] AS [a] ON [o].[ShippingAddressId] = [a].[AddressId]
          WHERE [o].[IsDeleted] = CAST(0 AS bit) AND [o].[OrderId] = @__orderId_0
      ) AS [t]
      INNER JOIN (
          SELECT [p0].[PaymentId], [p0].[Amount], [p0].[CreatedAt], [p0].[OrderId], [p0].[PaymentDate], [p0].[PaymentMethod], [p0].[PaymentStage], [p0].[PaymentStatus], [g].[PaymentId] AS [PaymentId0], [g].[GcashTransactionId], [g].[ScreenshotUrl], [g].[StorageBucket], [g].[StoragePath], [g].[SubmittedAt], [b].[PaymentId] AS [PaymentId1], [b].[BpiReferenceNumber], [b].[ProofStorageBucket], [b].[ProofStoragePath], [b].[ProofUrl], [b].[SubmittedAt] AS [SubmittedAt0], [b].[VerificationDeadline], [b].[VerificationNotes], [b].[VerifiedAt], [b].[VerifiedByUserId]
          FROM [Payment] AS [p0]
          LEFT JOIN [GCashPayment] AS [g] ON [p0].[PaymentId] = [g].[PaymentId]
          LEFT JOIN [BankTransferPayment] AS [b] ON [p0].[PaymentId] = [b].[PaymentId]
      ) AS [t0] ON [t].[OrderId] = [t0].[OrderId]
      ORDER BY [t].[OrderId], [t].[PickupOrderId], [t].[AddressId]
info: Microsoft.EntityFrameworkCore.Database.Command[20101]
      Executed DbCommand (57ms) [Parameters=[@__orderId_0='?' (DbType = Int32)], CommandType='Text', CommandTimeout='30']
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
      Executing ViewResult, running view ~/Views/Customer/Payment.cshtml.
info: Microsoft.AspNetCore.Mvc.ViewFeatures.ViewResultExecutor[4]
      Executed ViewResult - view ~/Views/Customer/Payment.cshtml executed in 12.2659ms.
info: Microsoft.AspNetCore.Mvc.Infrastructure.ControllerActionInvoker[105]
      Executed action WebApplication.Controllers.PaymentController.Submit (WebApplication) in 319.1364ms
info: Microsoft.AspNetCore.Routing.EndpointMiddleware[1]
      Executed endpoint 'WebApplication.Controllers.PaymentController.Submit (WebApplication)'
info: Microsoft.AspNetCore.Hosting.Diagnostics[2]
      Request finished HTTP/2 GET https://localhost:7177/Payment/Submit?orderId=3 - 200 - text/html;+charset=utf-8 331.0406ms
info: Microsoft.AspNetCore.Hosting.Diagnostics[1]
      Request starting HTTP/2 GET https://localhost:7177/_framework/aspnetcore-browser-refresh.js - - -
info: Microsoft.AspNetCore.Hosting.Diagnostics[1]
      Request starting HTTP/2 GET https://localhost:7177/_vs/browserLink - - -
info: Microsoft.AspNetCore.Hosting.Diagnostics[2]
      Request finished HTTP/2 GET https://localhost:7177/_framework/aspnetcore-browser-refresh.js - 200 14933 application/javascript;+charset=utf-8 4.7151ms
info: Microsoft.AspNetCore.Hosting.Diagnostics[2]
      Request finished HTTP/2 GET https://localhost:7177/_vs/browserLink - 200 - text/javascript;+charset=UTF-8 14.5364ms
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
info: Microsoft.EntityFrameworkCore.Database.Command[20101]
      Executed DbCommand (56ms) [Parameters=[@__userId_0='?' (DbType = Int32)], CommandType='Text', CommandTimeout='30']
      SELECT COUNT(*)
      FROM [Notification] AS [n]
      WHERE [n].[UserId] = @__userId_0 AND [n].[IsRead] = CAST(0 AS bit)
info: Microsoft.EntityFrameworkCore.Database.Command[20101]
      Executed DbCommand (57ms) [Parameters=[@__userId_Value_0='?' (DbType = Int32)], CommandType='Text', CommandTimeout='30']
      SELECT TOP(1) [c].[CartId], [c].[CreatedAt], [c].[GuestSessionId], [c].[IsCheckedOut], [c].[LastUpdatedAt], [c].[UserId]
      FROM [Cart] AS [c]
      WHERE [c].[IsCheckedOut] = CAST(0 AS bit) AND [c].[UserId] = @__userId_Value_0
      ORDER BY [c].[CartId]
info: Microsoft.AspNetCore.Mvc.Infrastructure.SystemTextJsonResultExecutor[1]
      Executing JsonResult, writing value of type '<>f__AnonymousType6`1[[System.Int32, System.Private.CoreLib, Version=8.0.0.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e]]'.
info: Microsoft.AspNetCore.Mvc.Infrastructure.SystemTextJsonResultExecutor[1]
      Executing JsonResult, writing value of type 'WebApplication.Models.ApiResponse'.
info: Microsoft.AspNetCore.Mvc.Infrastructure.ControllerActionInvoker[105]
      Executed action WebApplication.Controllers.CustomerController.NotificationCount (WebApplication) in 62.1473ms
info: Microsoft.AspNetCore.Mvc.Infrastructure.ControllerActionInvoker[105]
      Executed action WebApplication.Controllers.CartController.GetCartCount (WebApplication) in 65.15ms
info: Microsoft.AspNetCore.Routing.EndpointMiddleware[1]
      Executed endpoint 'WebApplication.Controllers.CustomerController.NotificationCount (WebApplication)'
info: Microsoft.AspNetCore.Routing.EndpointMiddleware[1]
      Executed endpoint 'WebApplication.Controllers.CartController.GetCartCount (WebApplication)'
info: Microsoft.AspNetCore.Hosting.Diagnostics[2]
      Request finished HTTP/2 GET https://localhost:7177/Customer/NotificationCount - 200 - application/json;+charset=utf-8 78.0569ms
info: Microsoft.AspNetCore.Hosting.Diagnostics[2]
      Request finished HTTP/2 GET https://localhost:7177/Cart/GetCartCount - 200 - application/json;+charset=utf-8 79.6572ms
info: Microsoft.EntityFrameworkCore.Database.Command[20101]
      Executed DbCommand (57ms) [Parameters=[], CommandType='Text', CommandTimeout='30']
      SELECT COUNT(*)
      FROM [ProductVariant] AS [p]
info: Microsoft.EntityFrameworkCore.Database.Command[20101]
      Executed DbCommand (57ms) [Parameters=[], CommandType='Text', CommandTimeout='30']
      SELECT COUNT(*)
      FROM [ProductVariant] AS [p]
info: Microsoft.EntityFrameworkCore.Database.Command[20101]
      Executed DbCommand (57ms) [Parameters=[@__p_0='?' (DbType = Int32)], CommandType='Text', CommandTimeout='30']
      SELECT TOP(@__p_0) [n].[NotificationId], [n].[Body], [n].[Channel], [n].[CreatedAt], [n].[FailureReason], [n].[IsRead], [n].[NotifType], [n].[OrderId], [n].[ReadAt], [n].[Recipient], [n].[RetryCount], [n].[SentAt], [n].[Status], [n].[Subject], [n].[TicketId], [n].[UserId]
      FROM [Notification] AS [n]
      WHERE [n].[Status] = N'Pending' AND [n].[RetryCount] < 3 AND [n].[Channel] = N'Email'
      ORDER BY [n].[CreatedAt]
info: WebApplication.BusinessLogic.Services.GmailEmailSender[0]
      Email sent to geronimojoan002@gmail.com - Subject: Order Confirmed - TBS-2026-00002
info: WebApplication.BackgroundJobs.NotificationDispatchJob[0]
      Notification 5 sent to geronimojoan002@gmail.com (OrderConfirmation).
info: Microsoft.EntityFrameworkCore.Database.Command[20101]
      Executed DbCommand (58ms) [Parameters=[@p2='?' (DbType = Int32), @p0='?' (DbType = DateTime2), @p1='?' (Size = 20)], CommandType='Text', CommandTimeout='30']
      SET IMPLICIT_TRANSACTIONS OFF;
      SET NOCOUNT ON;
      UPDATE [Notification] SET [SentAt] = @p0, [Status] = @p1
      OUTPUT 1
      WHERE [NotificationId] = @p2;
info: Microsoft.EntityFrameworkCore.Database.Command[20101]
      Executed DbCommand (56ms) [Parameters=[], CommandType='Text', CommandTimeout='30']
      SELECT COUNT(*)
      FROM [ProductVariant] AS [p]
info: Microsoft.EntityFrameworkCore.Database.Command[20101]
      Executed DbCommand (57ms) [Parameters=[], CommandType='Text', CommandTimeout='30']
      SELECT COUNT(*)
      FROM [ProductVariant] AS [p]
info: Microsoft.EntityFrameworkCore.Database.Command[20101]
      Executed DbCommand (57ms) [Parameters=[], CommandType='Text', CommandTimeout='30']
      SELECT COUNT(*)
      FROM [ProductVariant] AS [p]
info: Microsoft.EntityFrameworkCore.Database.Command[20101]
      Executed DbCommand (57ms) [Parameters=[@__p_0='?' (DbType = Int32)], CommandType='Text', CommandTimeout='30']
      SELECT TOP(@__p_0) [n].[NotificationId], [n].[Body], [n].[Channel], [n].[CreatedAt], [n].[FailureReason], [n].[IsRead], [n].[NotifType], [n].[OrderId], [n].[ReadAt], [n].[Recipient], [n].[RetryCount], [n].[SentAt], [n].[Status], [n].[Subject], [n].[TicketId], [n].[UserId]
      FROM [Notification] AS [n]
      WHERE [n].[Status] = N'Pending' AND [n].[RetryCount] < 3 AND [n].[Channel] = N'Email'
      ORDER BY [n].[CreatedAt]
info: Microsoft.EntityFrameworkCore.Database.Command[20101]
      Executed DbCommand (57ms) [Parameters=[], CommandType='Text', CommandTimeout='30']
      SELECT COUNT(*)
      FROM [ProductVariant] AS [p]
info: Microsoft.AspNetCore.Hosting.Diagnostics[1]
      Request starting HTTP/2 GET https://localhost:7177/Customer/NotificationCount - - -
info: Microsoft.AspNetCore.Routing.EndpointMiddleware[0]
      Executing endpoint 'WebApplication.Controllers.CustomerController.NotificationCount (WebApplication)'
info: Microsoft.AspNetCore.Mvc.Infrastructure.ControllerActionInvoker[102]
      Route matched with {action = "NotificationCount", controller = "Customer"}. Executing controller action with signature System.Threading.Tasks.Task`1[Microsoft.AspNetCore.Mvc.IActionResult] NotificationCount(System.Threading.CancellationToken) on controller WebApplication.Controllers.CustomerController (WebApplication).
info: Microsoft.EntityFrameworkCore.Database.Command[20101]
      Executed DbCommand (57ms) [Parameters=[@__userId_0='?' (DbType = Int32)], CommandType='Text', CommandTimeout='30']
      SELECT COUNT(*)
      FROM [Notification] AS [n]
      WHERE [n].[UserId] = @__userId_0 AND [n].[IsRead] = CAST(0 AS bit)
info: Microsoft.AspNetCore.Mvc.Infrastructure.SystemTextJsonResultExecutor[1]
      Executing JsonResult, writing value of type '<>f__AnonymousType6`1[[System.Int32, System.Private.CoreLib, Version=8.0.0.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e]]'.
info: Microsoft.AspNetCore.Mvc.Infrastructure.ControllerActionInvoker[105]
      Executed action WebApplication.Controllers.CustomerController.NotificationCount (WebApplication) in 60.7212ms
info: Microsoft.AspNetCore.Routing.EndpointMiddleware[1]
      Executed endpoint 'WebApplication.Controllers.CustomerController.NotificationCount (WebApplication)'
info: Microsoft.AspNetCore.Hosting.Diagnostics[2]
      Request finished HTTP/2 GET https://localhost:7177/Customer/NotificationCount - 200 - application/json;+charset=utf-8 71.4158ms
info: Microsoft.EntityFrameworkCore.Database.Command[20101]
      Executed DbCommand (57ms) [Parameters=[], CommandType='Text', CommandTimeout='30']
      SELECT COUNT(*)
      FROM [ProductVariant] AS [p]
info: Microsoft.EntityFrameworkCore.Database.Command[20101]
      Executed DbCommand (56ms) [Parameters=[], CommandType='Text', CommandTimeout='30']
      SELECT COUNT(*)
      FROM [ProductVariant] AS [p]
info: Microsoft.EntityFrameworkCore.Database.Command[20101]
      Executed DbCommand (57ms) [Parameters=[@__p_0='?' (DbType = Int32)], CommandType='Text', CommandTimeout='30']
      SELECT TOP(@__p_0) [n].[NotificationId], [n].[Body], [n].[Channel], [n].[CreatedAt], [n].[FailureReason], [n].[IsRead], [n].[NotifType], [n].[OrderId], [n].[ReadAt], [n].[Recipient], [n].[RetryCount], [n].[SentAt], [n].[Status], [n].[Subject], [n].[TicketId], [n].[UserId]
      FROM [Notification] AS [n]
      WHERE [n].[Status] = N'Pending' AND [n].[RetryCount] < 3 AND [n].[Channel] = N'Email'
      ORDER BY [n].[CreatedAt]
info: Microsoft.EntityFrameworkCore.Database.Command[20101]
      Executed DbCommand (58ms) [Parameters=[], CommandType='Text', CommandTimeout='30']
      SELECT COUNT(*)
      FROM [ProductVariant] AS [p]
info: Microsoft.EntityFrameworkCore.Database.Command[20101]
      Executed DbCommand (59ms) [Parameters=[], CommandType='Text', CommandTimeout='30']
      SELECT COUNT(*)
      FROM [ProductVariant] AS [p]
info: Microsoft.EntityFrameworkCore.Database.Command[20101]
      Executed DbCommand (58ms) [Parameters=[], CommandType='Text', CommandTimeout='30']
      SELECT COUNT(*)
      FROM [ProductVariant] AS [p]
info: Microsoft.EntityFrameworkCore.Database.Command[20101]
      Executed DbCommand (59ms) [Parameters=[@__p_0='?' (DbType = Int32)], CommandType='Text', CommandTimeout='30']
      SELECT TOP(@__p_0) [n].[NotificationId], [n].[Body], [n].[Channel], [n].[CreatedAt], [n].[FailureReason], [n].[IsRead], [n].[NotifType], [n].[OrderId], [n].[ReadAt], [n].[Recipient], [n].[RetryCount], [n].[SentAt], [n].[Status], [n].[Subject], [n].[TicketId], [n].[UserId]
      FROM [Notification] AS [n]
      WHERE [n].[Status] = N'Pending' AND [n].[RetryCount] < 3 AND [n].[Channel] = N'Email'
      ORDER BY [n].[CreatedAt]
info: Microsoft.EntityFrameworkCore.Database.Command[20101]
      Executed DbCommand (58ms) [Parameters=[], CommandType='Text', CommandTimeout='30']
      SELECT COUNT(*)
      FROM [ProductVariant] AS [p]
info: Microsoft.AspNetCore.Hosting.Diagnostics[1]
      Request starting HTTP/2 GET https://localhost:7177/Customer/NotificationCount - - -
info: Microsoft.AspNetCore.Routing.EndpointMiddleware[0]
      Executing endpoint 'WebApplication.Controllers.CustomerController.NotificationCount (WebApplication)'
info: Microsoft.AspNetCore.Mvc.Infrastructure.ControllerActionInvoker[102]
      Route matched with {action = "NotificationCount", controller = "Customer"}. Executing controller action with signature System.Threading.Tasks.Task`1[Microsoft.AspNetCore.Mvc.IActionResult] NotificationCount(System.Threading.CancellationToken) on controller WebApplication.Controllers.CustomerController (WebApplication).
info: Microsoft.EntityFrameworkCore.Database.Command[20101]
      Executed DbCommand (59ms) [Parameters=[@__userId_0='?' (DbType = Int32)], CommandType='Text', CommandTimeout='30']
      SELECT COUNT(*)
      FROM [Notification] AS [n]
      WHERE [n].[UserId] = @__userId_0 AND [n].[IsRead] = CAST(0 AS bit)
info: Microsoft.AspNetCore.Mvc.Infrastructure.SystemTextJsonResultExecutor[1]
      Executing JsonResult, writing value of type '<>f__AnonymousType6`1[[System.Int32, System.Private.CoreLib, Version=8.0.0.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e]]'.
info: Microsoft.AspNetCore.Mvc.Infrastructure.ControllerActionInvoker[105]
      Executed action WebApplication.Controllers.CustomerController.NotificationCount (WebApplication) in 62.2817ms
info: Microsoft.AspNetCore.Routing.EndpointMiddleware[1]
      Executed endpoint 'WebApplication.Controllers.CustomerController.NotificationCount (WebApplication)'
info: Microsoft.AspNetCore.Hosting.Diagnostics[2]
      Request finished HTTP/2 GET https://localhost:7177/Customer/NotificationCount - 200 - application/json;+charset=utf-8 73.5496ms
info: Microsoft.EntityFrameworkCore.Database.Command[20101]
      Executed DbCommand (58ms) [Parameters=[], CommandType='Text', CommandTimeout='30']
      SELECT COUNT(*)
      FROM [ProductVariant] AS [p]
info: Microsoft.EntityFrameworkCore.Database.Command[20101]
      Executed DbCommand (58ms) [Parameters=[], CommandType='Text', CommandTimeout='30']
      SELECT COUNT(*)
      FROM [ProductVariant] AS [p]
info: Microsoft.EntityFrameworkCore.Database.Command[20101]
      Executed DbCommand (59ms) [Parameters=[@__p_0='?' (DbType = Int32)], CommandType='Text', CommandTimeout='30']
      SELECT TOP(@__p_0) [n].[NotificationId], [n].[Body], [n].[Channel], [n].[CreatedAt], [n].[FailureReason], [n].[IsRead], [n].[NotifType], [n].[OrderId], [n].[ReadAt], [n].[Recipient], [n].[RetryCount], [n].[SentAt], [n].[Status], [n].[Subject], [n].[TicketId], [n].[UserId]
      FROM [Notification] AS [n]
      WHERE [n].[Status] = N'Pending' AND [n].[RetryCount] < 3 AND [n].[Channel] = N'Email'
      ORDER BY [n].[CreatedAt]
info: Microsoft.EntityFrameworkCore.Database.Command[20101]
      Executed DbCommand (58ms) [Parameters=[], CommandType='Text', CommandTimeout='30']
      SELECT COUNT(*)
      FROM [ProductVariant] AS [p]
info: Microsoft.EntityFrameworkCore.Database.Command[20101]
      Executed DbCommand (58ms) [Parameters=[], CommandType='Text', CommandTimeout='30']
      SELECT COUNT(*)
      FROM [ProductVariant] AS [p]
info: Microsoft.EntityFrameworkCore.Database.Command[20101]
      Executed DbCommand (58ms) [Parameters=[], CommandType='Text', CommandTimeout='30']
      SELECT COUNT(*)
      FROM [ProductVariant] AS [p]
info: Microsoft.EntityFrameworkCore.Database.Command[20101]
      Executed DbCommand (59ms) [Parameters=[@__p_0='?' (DbType = Int32)], CommandType='Text', CommandTimeout='30']
      SELECT TOP(@__p_0) [n].[NotificationId], [n].[Body], [n].[Channel], [n].[CreatedAt], [n].[FailureReason], [n].[IsRead], [n].[NotifType], [n].[OrderId], [n].[ReadAt], [n].[Recipient], [n].[RetryCount], [n].[SentAt], [n].[Status], [n].[Subject], [n].[TicketId], [n].[UserId]
      FROM [Notification] AS [n]
      WHERE [n].[Status] = N'Pending' AND [n].[RetryCount] < 3 AND [n].[Channel] = N'Email'
      ORDER BY [n].[CreatedAt]
info: Microsoft.EntityFrameworkCore.Database.Command[20101]
      Executed DbCommand (58ms) [Parameters=[], CommandType='Text', CommandTimeout='30']
      SELECT COUNT(*)
      FROM [ProductVariant] AS [p]
info: Microsoft.AspNetCore.Hosting.Diagnostics[1]
      Request starting HTTP/2 GET https://localhost:7177/Customer/NotificationCount - - -
info: Microsoft.AspNetCore.Routing.EndpointMiddleware[0]
      Executing endpoint 'WebApplication.Controllers.CustomerController.NotificationCount (WebApplication)'
info: Microsoft.AspNetCore.Mvc.Infrastructure.ControllerActionInvoker[102]
      Route matched with {action = "NotificationCount", controller = "Customer"}. Executing controller action with signature System.Threading.Tasks.Task`1[Microsoft.AspNetCore.Mvc.IActionResult] NotificationCount(System.Threading.CancellationToken) on controller WebApplication.Controllers.CustomerController (WebApplication).
info: Microsoft.EntityFrameworkCore.Database.Command[20101]
      Executed DbCommand (59ms) [Parameters=[@__userId_0='?' (DbType = Int32)], CommandType='Text', CommandTimeout='30']
      SELECT COUNT(*)
      FROM [Notification] AS [n]
      WHERE [n].[UserId] = @__userId_0 AND [n].[IsRead] = CAST(0 AS bit)
info: Microsoft.AspNetCore.Mvc.Infrastructure.SystemTextJsonResultExecutor[1]
      Executing JsonResult, writing value of type '<>f__AnonymousType6`1[[System.Int32, System.Private.CoreLib, Version=8.0.0.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e]]'.
info: Microsoft.AspNetCore.Mvc.Infrastructure.ControllerActionInvoker[105]
      Executed action WebApplication.Controllers.CustomerController.NotificationCount (WebApplication) in 62.6023ms
info: Microsoft.AspNetCore.Routing.EndpointMiddleware[1]
      Executed endpoint 'WebApplication.Controllers.CustomerController.NotificationCount (WebApplication)'
info: Microsoft.AspNetCore.Hosting.Diagnostics[2]
      Request finished HTTP/2 GET https://localhost:7177/Customer/NotificationCount - 200 - application/json;+charset=utf-8 73.6397ms
info: Microsoft.EntityFrameworkCore.Database.Command[20101]
      Executed DbCommand (58ms) [Parameters=[], CommandType='Text', CommandTimeout='30']
      SELECT COUNT(*)
      FROM [ProductVariant] AS [p]
info: Microsoft.EntityFrameworkCore.Database.Command[20101]
      Executed DbCommand (62ms) [Parameters=[@p0='?' (DbType = DateTime2), @p1='?' (Size = 1000), @p2='?' (Size = 100), @p3='?' (DbType = Int32)], CommandType='Text', CommandTimeout='30']
      SET IMPLICIT_TRANSACTIONS OFF;
      SET NOCOUNT ON;
      INSERT INTO [SystemLog] ([CreatedAt], [EventDescription], [EventType], [UserId])
      OUTPUT INSERTED.[SystemLogId]
      VALUES (@p0, @p1, @p2, @p3);
info: Microsoft.EntityFrameworkCore.Database.Command[20101]
      Executed DbCommand (58ms) [Parameters=[@__staleThreshold_0='?' (DbType = DateTime2)], CommandType='Text', CommandTimeout='30']
      SELECT [o].[OrderId], [o].[OrderNumber]
      FROM [Order] AS [o]
      WHERE [o].[IsDeleted] = CAST(0 AS bit) AND [o].[OrderStatus] = N'Pending' AND [o].[OrderDate] < @__staleThreshold_0
info: Microsoft.EntityFrameworkCore.Database.Command[20101]
      Executed DbCommand (60ms) [Parameters=[@p0='?' (DbType = DateTime2), @p1='?' (Size = 1000), @p2='?' (Size = 100), @p3='?' (DbType = Int32)], CommandType='Text', CommandTimeout='30']
      SET IMPLICIT_TRANSACTIONS OFF;
      SET NOCOUNT ON;
      INSERT INTO [SystemLog] ([CreatedAt], [EventDescription], [EventType], [UserId])
      OUTPUT INSERTED.[SystemLogId]
      VALUES (@p0, @p1, @p2, @p3);
info: Microsoft.EntityFrameworkCore.Database.Command[20101]
      Executed DbCommand (61ms) [Parameters=[@p0='?' (DbType = DateTime2), @p1='?' (Size = 1000), @p2='?' (Size = 100), @p3='?' (DbType = Int32)], CommandType='Text', CommandTimeout='30']
      SET IMPLICIT_TRANSACTIONS OFF;
      SET NOCOUNT ON;
      INSERT INTO [SystemLog] ([CreatedAt], [EventDescription], [EventType], [UserId])
      OUTPUT INSERTED.[SystemLogId]
      VALUES (@p0, @p1, @p2, @p3);
info: Microsoft.EntityFrameworkCore.Database.Command[20101]
      Executed DbCommand (59ms) [Parameters=[], CommandType='Text', CommandTimeout='30']
      SELECT [d].[DeliveryId], [d].[ActualDeliveryTime], [d].[Courier], [d].[CreatedAt], [d].[DelayedUntil], [d].[DeliveryStatus], [d].[EstimatedDeliveryTime], [d].[IsDelayed], [d].[OrderId], [l].[DeliveryId], [l].[BookingRef], [l].[DriverName], [l].[DriverPhone], [l0].[DeliveryId], [l0].[TrackingNumber], [t].[OrderId], [t].[CartId], [t].[ContactPhone], [t].[CreatedAt], [t].[DeliveryInstructions], [t].[DiscountAmount], [t].[FulfillmentType], [t].[GuestSessionId], [t].[IsDeleted], [t].[IsWalkIn], [t].[OrderDate], [t].[OrderNumber], [t].[OrderStatus], [t].[POSSessionId], [t].[ShippingAddressId], [t].[ShippingFee], [t].[SubTotal], [t].[TotalAmount], [t].[UpdatedAt], [t].[UserId], [t0].[UserId], [t0].[CreatedAt], [t0].[DefaultAddressId], [t0].[Email], [t0].[FailedLoginAttempts], [t0].[FirstName], [t0].[IsActive], [t0].[IsDeleted], [t0].[IsWalkIn], [t0].[LastLoginAt], [t0].[LastName], [t0].[LockoutUntil], [t0].[PasswordHash], [t0].[PhoneNumber]
      FROM [Delivery] AS [d]
      LEFT JOIN [LalamoveDelivery] AS [l] ON [d].[DeliveryId] = [l].[DeliveryId]
      LEFT JOIN [LBCDelivery] AS [l0] ON [d].[DeliveryId] = [l0].[DeliveryId]
      INNER JOIN (
          SELECT [o].[OrderId], [o].[CartId], [o].[ContactPhone], [o].[CreatedAt], [o].[DeliveryInstructions], [o].[DiscountAmount], [o].[FulfillmentType], [o].[GuestSessionId], [o].[IsDeleted], [o].[IsWalkIn], [o].[OrderDate], [o].[OrderNumber], [o].[OrderStatus], [o].[POSSessionId], [o].[ShippingAddressId], [o].[ShippingFee], [o].[SubTotal], [o].[TotalAmount], [o].[UpdatedAt], [o].[UserId]
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
      Executed DbCommand (61ms) [Parameters=[@p0='?' (DbType = DateTime2), @p1='?' (Size = 1000), @p2='?' (Size = 100), @p3='?' (DbType = Int32)], CommandType='Text', CommandTimeout='30']
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
      SELECT [o].[OrderId], [o].[CartId], [o].[ContactPhone], [o].[CreatedAt], [o].[DeliveryInstructions], [o].[DiscountAmount], [o].[FulfillmentType], [o].[GuestSessionId], [o].[IsDeleted], [o].[IsWalkIn], [o].[OrderDate], [o].[OrderNumber], [o].[OrderStatus], [o].[POSSessionId], [o].[ShippingAddressId], [o].[ShippingFee], [o].[SubTotal], [o].[TotalAmount], [o].[UpdatedAt], [o].[UserId], [t].[UserId], [t].[CreatedAt], [t].[DefaultAddressId], [t].[Email], [t].[FailedLoginAttempts], [t].[FirstName], [t].[IsActive], [t].[IsDeleted], [t].[IsWalkIn], [t].[LastLoginAt], [t].[LastName], [t].[LockoutUntil], [t].[PasswordHash], [t].[PhoneNumber]
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
      Executed DbCommand (56ms) [Parameters=[], CommandType='Text', CommandTimeout='30']
      SELECT COUNT(*)
      FROM [ProductVariant] AS [p]
info: Microsoft.EntityFrameworkCore.Database.Command[20101]
      Executed DbCommand (58ms) [Parameters=[@p0='?' (DbType = DateTime2), @p1='?' (Size = 1000), @p2='?' (Size = 100), @p3='?' (DbType = Int32), @p4='?' (DbType = DateTime2), @p5='?' (Size = 1000), @p6='?' (Size = 100), @p7='?' (DbType = Int32)], CommandType='Text', CommandTimeout='30']
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
      Executed DbCommand (56ms) [Parameters=[@__p_0='?' (DbType = Int32)], CommandType='Text', CommandTimeout='30']
      SELECT TOP(@__p_0) [n].[NotificationId], [n].[Body], [n].[Channel], [n].[CreatedAt], [n].[FailureReason], [n].[IsRead], [n].[NotifType], [n].[OrderId], [n].[ReadAt], [n].[Recipient], [n].[RetryCount], [n].[SentAt], [n].[Status], [n].[Subject], [n].[TicketId], [n].[UserId]
      FROM [Notification] AS [n]
      WHERE [n].[Status] = N'Pending' AND [n].[RetryCount] < 3 AND [n].[Channel] = N'Email'
      ORDER BY [n].[CreatedAt]
info: Microsoft.EntityFrameworkCore.Database.Command[20101]
      Executed DbCommand (56ms) [Parameters=[], CommandType='Text', CommandTimeout='30']
      SELECT COUNT(*)
      FROM [ProductVariant] AS [p]
info: Microsoft.EntityFrameworkCore.Database.Command[20101]
      Executed DbCommand (56ms) [Parameters=[], CommandType='Text', CommandTimeout='30']
      SELECT COUNT(*)
      FROM [ProductVariant] AS [p]
info: Microsoft.AspNetCore.Hosting.Diagnostics[1]
      Request starting HTTP/2 POST https://localhost:7177/Payment/SubmitBankTransfer - multipart/form-data;+boundary=----WebKitFormBoundarySBrWIM2RZjTkmpUO 86093
info: Microsoft.AspNetCore.Cors.Infrastructure.CorsService[4]
      CORS policy execution successful.
info: Microsoft.AspNetCore.Routing.EndpointMiddleware[0]
      Executing endpoint 'WebApplication.Controllers.PaymentController.SubmitBankTransfer (WebApplication)'
info: Microsoft.AspNetCore.Mvc.Infrastructure.ControllerActionInvoker[102]
      Route matched with {action = "SubmitBankTransfer", controller = "Payment"}. Executing controller action with signature System.Threading.Tasks.Task`1[Microsoft.AspNetCore.Mvc.IActionResult] SubmitBankTransfer(Int32, System.String, System.String, Microsoft.AspNetCore.Http.IFormFile, System.Threading.CancellationToken) on controller WebApplication.Controllers.PaymentController (WebApplication).
info: Microsoft.EntityFrameworkCore.Database.Command[20101]
      Executed DbCommand (59ms) [Parameters=[@__orderId_0='?' (DbType = Int32)], CommandType='Text', CommandTimeout='30']
      SELECT TOP(1) [o].[OrderId], [o].[CartId], [o].[ContactPhone], [o].[CreatedAt], [o].[DeliveryInstructions], [o].[DiscountAmount], [o].[FulfillmentType], [o].[GuestSessionId], [o].[IsDeleted], [o].[IsWalkIn], [o].[OrderDate], [o].[OrderNumber], [o].[OrderStatus], [o].[POSSessionId], [o].[ShippingAddressId], [o].[ShippingFee], [o].[SubTotal], [o].[TotalAmount], [o].[UpdatedAt], [o].[UserId]
      FROM [Order] AS [o]
      WHERE [o].[IsDeleted] = CAST(0 AS bit) AND [o].[OrderId] = @__orderId_0
info: Microsoft.EntityFrameworkCore.Database.Command[20101]
      Executed DbCommand (60ms) [Parameters=[@__orderId_0='?' (DbType = Int32)], CommandType='Text', CommandTimeout='30']
      SELECT CASE
          WHEN EXISTS (
              SELECT 1
              FROM [Payment] AS [p]
              WHERE [p].[OrderId] = @__orderId_0 AND [p].[PaymentStatus] <> N'Failed') THEN CAST(1 AS bit)
          ELSE CAST(0 AS bit)
      END
info: Microsoft.EntityFrameworkCore.Database.Command[20101]
      Executed DbCommand (64ms) [Parameters=[@__orderId_0='?' (DbType = Int32)], CommandType='Text', CommandTimeout='30']
      SELECT TOP(1) [o].[OrderId], [o].[CartId], [o].[ContactPhone], [o].[CreatedAt], [o].[DeliveryInstructions], [o].[DiscountAmount], [o].[FulfillmentType], [o].[GuestSessionId], [o].[IsDeleted], [o].[IsWalkIn], [o].[OrderDate], [o].[OrderNumber], [o].[OrderStatus], [o].[POSSessionId], [o].[ShippingAddressId], [o].[ShippingFee], [o].[SubTotal], [o].[TotalAmount], [o].[UpdatedAt], [o].[UserId]
      FROM [Order] AS [o]
      WHERE [o].[IsDeleted] = CAST(0 AS bit) AND [o].[OrderId] = @__orderId_0
      ORDER BY [o].[OrderId]
info: Microsoft.EntityFrameworkCore.Database.Command[20101]
      Executed DbCommand (60ms) [Parameters=[@__orderId_0='?' (DbType = Int32)], CommandType='Text', CommandTimeout='30']
      SELECT [o0].[OrderItemId], [o0].[OrderId], [o0].[ProductId], [o0].[ProductVariantId], [o0].[Quantity], [o0].[UnitPrice], [t].[OrderId]
      FROM (
          SELECT TOP(1) [o].[OrderId]
          FROM [Order] AS [o]
          WHERE [o].[IsDeleted] = CAST(0 AS bit) AND [o].[OrderId] = @__orderId_0
      ) AS [t]
      INNER JOIN [OrderItem] AS [o0] ON [t].[OrderId] = [o0].[OrderId]
      ORDER BY [t].[OrderId]
fail: Microsoft.EntityFrameworkCore.Update[10000]
      An exception occurred in the database while saving changes for context type 'WebApplication.DataAccess.Context.AppDbContext'.
      System.InvalidOperationException: The configured execution strategy 'SqlServerRetryingExecutionStrategy' does not support user-initiated transactions. Use the execution strategy returned by 'DbContext.Database.CreateExecutionStrategy()' to execute all the operations in the transaction as a retriable unit.
         at Microsoft.EntityFrameworkCore.Storage.ExecutionStrategy.OnFirstExecution()
         at Microsoft.EntityFrameworkCore.Storage.ExecutionStrategy.ExecuteAsync[TState,TResult](TState state, Func`4 operation, Func`4 verifySucceeded, CancellationToken cancellationToken)
         at Microsoft.EntityFrameworkCore.DbContext.SaveChangesAsync(Boolean acceptAllChangesOnSuccess, CancellationToken cancellationToken)
      System.InvalidOperationException: The configured execution strategy 'SqlServerRetryingExecutionStrategy' does not support user-initiated transactions. Use the execution strategy returned by 'DbContext.Database.CreateExecutionStrategy()' to execute all the operations in the transaction as a retriable unit.
         at Microsoft.EntityFrameworkCore.Storage.ExecutionStrategy.OnFirstExecution()
         at Microsoft.EntityFrameworkCore.Storage.ExecutionStrategy.ExecuteAsync[TState,TResult](TState state, Func`4 operation, Func`4 verifySucceeded, CancellationToken cancellationToken)
         at Microsoft.EntityFrameworkCore.DbContext.SaveChangesAsync(Boolean acceptAllChangesOnSuccess, CancellationToken cancellationToken)
info: Microsoft.AspNetCore.Mvc.RedirectToActionResult[1]
      Executing RedirectResult, redirecting to /Payment/Submit?orderId=3.
info: Microsoft.AspNetCore.Mvc.Infrastructure.ControllerActionInvoker[105]
      Executed action WebApplication.Controllers.PaymentController.SubmitBankTransfer (WebApplication) in 2653.9772ms
info: Microsoft.AspNetCore.Routing.EndpointMiddleware[1]
      Executed endpoint 'WebApplication.Controllers.PaymentController.SubmitBankTransfer (WebApplication)'
info: Microsoft.AspNetCore.Hosting.Diagnostics[2]
      Request finished HTTP/2 POST https://localhost:7177/Payment/SubmitBankTransfer - 302 - - 2712.3920ms
info: Microsoft.AspNetCore.Hosting.Diagnostics[1]
      Request starting HTTP/2 GET https://localhost:7177/Payment/Submit?orderId=3 - - -
info: Microsoft.AspNetCore.Routing.EndpointMiddleware[0]
      Executing endpoint 'WebApplication.Controllers.PaymentController.Submit (WebApplication)'
info: Microsoft.AspNetCore.Mvc.Infrastructure.ControllerActionInvoker[102]
      Route matched with {action = "Submit", controller = "Payment"}. Executing controller action with signature System.Threading.Tasks.Task`1[Microsoft.AspNetCore.Mvc.IActionResult] Submit(Int32, System.Threading.CancellationToken) on controller WebApplication.Controllers.PaymentController (WebApplication).
info: Microsoft.EntityFrameworkCore.Database.Command[20101]
      Executed DbCommand (56ms) [Parameters=[@__orderId_0='?' (DbType = Int32)], CommandType='Text', CommandTimeout='30']
      SELECT TOP(1) [o].[OrderId], [o].[CartId], [o].[ContactPhone], [o].[CreatedAt], [o].[DeliveryInstructions], [o].[DiscountAmount], [o].[FulfillmentType], [o].[GuestSessionId], [o].[IsDeleted], [o].[IsWalkIn], [o].[OrderDate], [o].[OrderNumber], [o].[OrderStatus], [o].[POSSessionId], [o].[ShippingAddressId], [o].[ShippingFee], [o].[SubTotal], [o].[TotalAmount], [o].[UpdatedAt], [o].[UserId], [p].[PickupOrderId], [a].[AddressId], [p].[OrderId], [p].[PickupConfirmedAt], [p].[PickupExpiresAt], [p].[PickupReadyAt], [a].[City], [a].[Country], [a].[CreatedAt], [a].[IsDefault], [a].[IsSnapshot], [a].[Label], [a].[PostalCode], [a].[Province], [a].[Street], [a].[UserId]
      FROM [Order] AS [o]
      LEFT JOIN [PickupOrder] AS [p] ON [o].[OrderId] = [p].[OrderId]
      LEFT JOIN [Address] AS [a] ON [o].[ShippingAddressId] = [a].[AddressId]
      WHERE [o].[IsDeleted] = CAST(0 AS bit) AND [o].[OrderId] = @__orderId_0
      ORDER BY [o].[OrderId], [p].[PickupOrderId], [a].[AddressId]
info: Microsoft.EntityFrameworkCore.Database.Command[20101]
      Executed DbCommand (57ms) [Parameters=[@__orderId_0='?' (DbType = Int32)], CommandType='Text', CommandTimeout='30']
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
      Executed DbCommand (56ms) [Parameters=[@__orderId_0='?' (DbType = Int32)], CommandType='Text', CommandTimeout='30']
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
      Executed DbCommand (56ms) [Parameters=[@__orderId_0='?' (DbType = Int32)], CommandType='Text', CommandTimeout='30']
      SELECT [t0].[PaymentId], [t0].[Amount], [t0].[CreatedAt], [t0].[OrderId], [t0].[PaymentDate], [t0].[PaymentMethod], [t0].[PaymentStage], [t0].[PaymentStatus], [t0].[PaymentId0], [t0].[GcashTransactionId], [t0].[ScreenshotUrl], [t0].[StorageBucket], [t0].[StoragePath], [t0].[SubmittedAt], [t0].[PaymentId1], [t0].[BpiReferenceNumber], [t0].[ProofStorageBucket], [t0].[ProofStoragePath], [t0].[ProofUrl], [t0].[SubmittedAt0], [t0].[VerificationDeadline], [t0].[VerificationNotes], [t0].[VerifiedAt], [t0].[VerifiedByUserId], [t].[OrderId], [t].[PickupOrderId], [t].[AddressId]
      FROM (
          SELECT TOP(1) [o].[OrderId], [p].[PickupOrderId], [a].[AddressId]
          FROM [Order] AS [o]
          LEFT JOIN [PickupOrder] AS [p] ON [o].[OrderId] = [p].[OrderId]
          LEFT JOIN [Address] AS [a] ON [o].[ShippingAddressId] = [a].[AddressId]
          WHERE [o].[IsDeleted] = CAST(0 AS bit) AND [o].[OrderId] = @__orderId_0
      ) AS [t]
      INNER JOIN (
          SELECT [p0].[PaymentId], [p0].[Amount], [p0].[CreatedAt], [p0].[OrderId], [p0].[PaymentDate], [p0].[PaymentMethod], [p0].[PaymentStage], [p0].[PaymentStatus], [g].[PaymentId] AS [PaymentId0], [g].[GcashTransactionId], [g].[ScreenshotUrl], [g].[StorageBucket], [g].[StoragePath], [g].[SubmittedAt], [b].[PaymentId] AS [PaymentId1], [b].[BpiReferenceNumber], [b].[ProofStorageBucket], [b].[ProofStoragePath], [b].[ProofUrl], [b].[SubmittedAt] AS [SubmittedAt0], [b].[VerificationDeadline], [b].[VerificationNotes], [b].[VerifiedAt], [b].[VerifiedByUserId]
          FROM [Payment] AS [p0]
          LEFT JOIN [GCashPayment] AS [g] ON [p0].[PaymentId] = [g].[PaymentId]
          LEFT JOIN [BankTransferPayment] AS [b] ON [p0].[PaymentId] = [b].[PaymentId]
      ) AS [t0] ON [t].[OrderId] = [t0].[OrderId]
      ORDER BY [t].[OrderId], [t].[PickupOrderId], [t].[AddressId]
info: Microsoft.EntityFrameworkCore.Database.Command[20101]
      Executed DbCommand (56ms) [Parameters=[@__orderId_0='?' (DbType = Int32)], CommandType='Text', CommandTimeout='30']
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
      Executing ViewResult, running view ~/Views/Customer/Payment.cshtml.
info: Microsoft.AspNetCore.Mvc.ViewFeatures.ViewResultExecutor[4]
      Executed ViewResult - view ~/Views/Customer/Payment.cshtml executed in 6.1239ms.
info: Microsoft.AspNetCore.Mvc.Infrastructure.ControllerActionInvoker[105]
      Executed action WebApplication.Controllers.PaymentController.Submit (WebApplication) in 300.3432ms
info: Microsoft.AspNetCore.Routing.EndpointMiddleware[1]
      Executed endpoint 'WebApplication.Controllers.PaymentController.Submit (WebApplication)'
info: Microsoft.AspNetCore.Hosting.Diagnostics[2]
      Request finished HTTP/2 GET https://localhost:7177/Payment/Submit?orderId=3 - 200 - text/html;+charset=utf-8 312.5423ms
info: Microsoft.AspNetCore.Hosting.Diagnostics[1]
      Request starting HTTP/2 GET https://localhost:7177/_framework/aspnetcore-browser-refresh.js - - -
info: Microsoft.AspNetCore.Hosting.Diagnostics[1]
      Request starting HTTP/2 GET https://localhost:7177/_vs/browserLink - - -
info: Microsoft.AspNetCore.Hosting.Diagnostics[2]
      Request finished HTTP/2 GET https://localhost:7177/_framework/aspnetcore-browser-refresh.js - 200 14933 application/javascript;+charset=utf-8 4.3871ms
info: Microsoft.AspNetCore.Hosting.Diagnostics[2]
      Request finished HTTP/2 GET https://localhost:7177/_vs/browserLink - 200 - text/javascript;+charset=UTF-8 13.2670ms
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
info: Microsoft.EntityFrameworkCore.Database.Command[20101]
      Executed DbCommand (56ms) [Parameters=[@__userId_Value_0='?' (DbType = Int32)], CommandType='Text', CommandTimeout='30']
      SELECT TOP(1) [c].[CartId], [c].[CreatedAt], [c].[GuestSessionId], [c].[IsCheckedOut], [c].[LastUpdatedAt], [c].[UserId]
      FROM [Cart] AS [c]
      WHERE [c].[IsCheckedOut] = CAST(0 AS bit) AND [c].[UserId] = @__userId_Value_0
      ORDER BY [c].[CartId]
info: Microsoft.AspNetCore.Mvc.Infrastructure.SystemTextJsonResultExecutor[1]
      Executing JsonResult, writing value of type 'WebApplication.Models.ApiResponse'.
info: Microsoft.EntityFrameworkCore.Database.Command[20101]
      Executed DbCommand (58ms) [Parameters=[@__userId_0='?' (DbType = Int32)], CommandType='Text', CommandTimeout='30']
      SELECT COUNT(*)
      FROM [Notification] AS [n]
      WHERE [n].[UserId] = @__userId_0 AND [n].[IsRead] = CAST(0 AS bit)
info: Microsoft.AspNetCore.Mvc.Infrastructure.ControllerActionInvoker[105]
      Executed action WebApplication.Controllers.CartController.GetCartCount (WebApplication) in 61.6174ms
info: Microsoft.AspNetCore.Mvc.Infrastructure.SystemTextJsonResultExecutor[1]
      Executing JsonResult, writing value of type '<>f__AnonymousType6`1[[System.Int32, System.Private.CoreLib, Version=8.0.0.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e]]'.
info: Microsoft.AspNetCore.Routing.EndpointMiddleware[1]
      Executed endpoint 'WebApplication.Controllers.CartController.GetCartCount (WebApplication)'
info: Microsoft.AspNetCore.Mvc.Infrastructure.ControllerActionInvoker[105]
      Executed action WebApplication.Controllers.CustomerController.NotificationCount (WebApplication) in 64.0121ms
info: Microsoft.AspNetCore.Hosting.Diagnostics[2]
      Request finished HTTP/2 GET https://localhost:7177/Cart/GetCartCount - 200 - application/json;+charset=utf-8 75.5232ms
info: Microsoft.AspNetCore.Routing.EndpointMiddleware[1]
      Executed endpoint 'WebApplication.Controllers.CustomerController.NotificationCount (WebApplication)'
info: Microsoft.AspNetCore.Hosting.Diagnostics[2]
      Request finished HTTP/2 GET https://localhost:7177/Customer/NotificationCount - 200 - application/json;+charset=utf-8 78.8293ms
info: Microsoft.EntityFrameworkCore.Database.Command[20101]
      Executed DbCommand (58ms) [Parameters=[], CommandType='Text', CommandTimeout='30']
      SELECT COUNT(*)
      FROM [ProductVariant] AS [p]
info: Microsoft.AspNetCore.Hosting.Diagnostics[1]
      Request starting HTTP/2 POST https://localhost:7177/Payment/SubmitBankTransfer - multipart/form-data;+boundary=----WebKitFormBoundaryTAA83ywuB2W964XD 810
info: Microsoft.AspNetCore.Cors.Infrastructure.CorsService[4]
      CORS policy execution successful.
info: Microsoft.AspNetCore.Routing.EndpointMiddleware[0]
      Executing endpoint 'WebApplication.Controllers.PaymentController.SubmitBankTransfer (WebApplication)'
info: Microsoft.AspNetCore.Mvc.Infrastructure.ControllerActionInvoker[102]
      Route matched with {action = "SubmitBankTransfer", controller = "Payment"}. Executing controller action with signature System.Threading.Tasks.Task`1[Microsoft.AspNetCore.Mvc.IActionResult] SubmitBankTransfer(Int32, System.String, System.String, Microsoft.AspNetCore.Http.IFormFile, System.Threading.CancellationToken) on controller WebApplication.Controllers.PaymentController (WebApplication).
info: Microsoft.EntityFrameworkCore.Database.Command[20101]
      Executed DbCommand (59ms) [Parameters=[@__orderId_0='?' (DbType = Int32)], CommandType='Text', CommandTimeout='30']
      SELECT TOP(1) [o].[OrderId], [o].[CartId], [o].[ContactPhone], [o].[CreatedAt], [o].[DeliveryInstructions], [o].[DiscountAmount], [o].[FulfillmentType], [o].[GuestSessionId], [o].[IsDeleted], [o].[IsWalkIn], [o].[OrderDate], [o].[OrderNumber], [o].[OrderStatus], [o].[POSSessionId], [o].[ShippingAddressId], [o].[ShippingFee], [o].[SubTotal], [o].[TotalAmount], [o].[UpdatedAt], [o].[UserId]
      FROM [Order] AS [o]
      WHERE [o].[IsDeleted] = CAST(0 AS bit) AND [o].[OrderId] = @__orderId_0
info: Microsoft.EntityFrameworkCore.Database.Command[20101]
      Executed DbCommand (58ms) [Parameters=[@__orderId_0='?' (DbType = Int32)], CommandType='Text', CommandTimeout='30']
      SELECT CASE
          WHEN EXISTS (
              SELECT 1
              FROM [Payment] AS [p]
              WHERE [p].[OrderId] = @__orderId_0 AND [p].[PaymentStatus] <> N'Failed') THEN CAST(1 AS bit)
          ELSE CAST(0 AS bit)
      END
info: Microsoft.AspNetCore.Mvc.RedirectToActionResult[1]
      Executing RedirectResult, redirecting to /Payment/Submit?orderId=3.
info: Microsoft.AspNetCore.Mvc.Infrastructure.ControllerActionInvoker[105]
      Executed action WebApplication.Controllers.PaymentController.SubmitBankTransfer (WebApplication) in 125.0283ms
info: Microsoft.AspNetCore.Routing.EndpointMiddleware[1]
      Executed endpoint 'WebApplication.Controllers.PaymentController.SubmitBankTransfer (WebApplication)'
info: Microsoft.AspNetCore.Hosting.Diagnostics[2]
      Request finished HTTP/2 POST https://localhost:7177/Payment/SubmitBankTransfer - 302 - - 135.2623ms
info: Microsoft.AspNetCore.Hosting.Diagnostics[1]
      Request starting HTTP/2 GET https://localhost:7177/Payment/Submit?orderId=3 - - -
info: Microsoft.AspNetCore.Routing.EndpointMiddleware[0]
      Executing endpoint 'WebApplication.Controllers.PaymentController.Submit (WebApplication)'
info: Microsoft.AspNetCore.Mvc.Infrastructure.ControllerActionInvoker[102]
      Route matched with {action = "Submit", controller = "Payment"}. Executing controller action with signature System.Threading.Tasks.Task`1[Microsoft.AspNetCore.Mvc.IActionResult] Submit(Int32, System.Threading.CancellationToken) on controller WebApplication.Controllers.PaymentController (WebApplication).
info: Microsoft.EntityFrameworkCore.Database.Command[20101]
      Executed DbCommand (59ms) [Parameters=[@__orderId_0='?' (DbType = Int32)], CommandType='Text', CommandTimeout='30']
      SELECT TOP(1) [o].[OrderId], [o].[CartId], [o].[ContactPhone], [o].[CreatedAt], [o].[DeliveryInstructions], [o].[DiscountAmount], [o].[FulfillmentType], [o].[GuestSessionId], [o].[IsDeleted], [o].[IsWalkIn], [o].[OrderDate], [o].[OrderNumber], [o].[OrderStatus], [o].[POSSessionId], [o].[ShippingAddressId], [o].[ShippingFee], [o].[SubTotal], [o].[TotalAmount], [o].[UpdatedAt], [o].[UserId], [p].[PickupOrderId], [a].[AddressId], [p].[OrderId], [p].[PickupConfirmedAt], [p].[PickupExpiresAt], [p].[PickupReadyAt], [a].[City], [a].[Country], [a].[CreatedAt], [a].[IsDefault], [a].[IsSnapshot], [a].[Label], [a].[PostalCode], [a].[Province], [a].[Street], [a].[UserId]
      FROM [Order] AS [o]
      LEFT JOIN [PickupOrder] AS [p] ON [o].[OrderId] = [p].[OrderId]
      LEFT JOIN [Address] AS [a] ON [o].[ShippingAddressId] = [a].[AddressId]
      WHERE [o].[IsDeleted] = CAST(0 AS bit) AND [o].[OrderId] = @__orderId_0
      ORDER BY [o].[OrderId], [p].[PickupOrderId], [a].[AddressId]
info: Microsoft.EntityFrameworkCore.Database.Command[20101]
      Executed DbCommand (59ms) [Parameters=[@__orderId_0='?' (DbType = Int32)], CommandType='Text', CommandTimeout='30']
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
      Executed DbCommand (59ms) [Parameters=[@__orderId_0='?' (DbType = Int32)], CommandType='Text', CommandTimeout='30']
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
      Executed DbCommand (59ms) [Parameters=[@__orderId_0='?' (DbType = Int32)], CommandType='Text', CommandTimeout='30']
      SELECT [t0].[PaymentId], [t0].[Amount], [t0].[CreatedAt], [t0].[OrderId], [t0].[PaymentDate], [t0].[PaymentMethod], [t0].[PaymentStage], [t0].[PaymentStatus], [t0].[PaymentId0], [t0].[GcashTransactionId], [t0].[ScreenshotUrl], [t0].[StorageBucket], [t0].[StoragePath], [t0].[SubmittedAt], [t0].[PaymentId1], [t0].[BpiReferenceNumber], [t0].[ProofStorageBucket], [t0].[ProofStoragePath], [t0].[ProofUrl], [t0].[SubmittedAt0], [t0].[VerificationDeadline], [t0].[VerificationNotes], [t0].[VerifiedAt], [t0].[VerifiedByUserId], [t].[OrderId], [t].[PickupOrderId], [t].[AddressId]
      FROM (
          SELECT TOP(1) [o].[OrderId], [p].[PickupOrderId], [a].[AddressId]
          FROM [Order] AS [o]
          LEFT JOIN [PickupOrder] AS [p] ON [o].[OrderId] = [p].[OrderId]
          LEFT JOIN [Address] AS [a] ON [o].[ShippingAddressId] = [a].[AddressId]
          WHERE [o].[IsDeleted] = CAST(0 AS bit) AND [o].[OrderId] = @__orderId_0
      ) AS [t]
      INNER JOIN (
          SELECT [p0].[PaymentId], [p0].[Amount], [p0].[CreatedAt], [p0].[OrderId], [p0].[PaymentDate], [p0].[PaymentMethod], [p0].[PaymentStage], [p0].[PaymentStatus], [g].[PaymentId] AS [PaymentId0], [g].[GcashTransactionId], [g].[ScreenshotUrl], [g].[StorageBucket], [g].[StoragePath], [g].[SubmittedAt], [b].[PaymentId] AS [PaymentId1], [b].[BpiReferenceNumber], [b].[ProofStorageBucket], [b].[ProofStoragePath], [b].[ProofUrl], [b].[SubmittedAt] AS [SubmittedAt0], [b].[VerificationDeadline], [b].[VerificationNotes], [b].[VerifiedAt], [b].[VerifiedByUserId]
          FROM [Payment] AS [p0]
          LEFT JOIN [GCashPayment] AS [g] ON [p0].[PaymentId] = [g].[PaymentId]
          LEFT JOIN [BankTransferPayment] AS [b] ON [p0].[PaymentId] = [b].[PaymentId]
      ) AS [t0] ON [t].[OrderId] = [t0].[OrderId]
      ORDER BY [t].[OrderId], [t].[PickupOrderId], [t].[AddressId]
info: Microsoft.EntityFrameworkCore.Database.Command[20101]
      Executed DbCommand (58ms) [Parameters=[@__orderId_0='?' (DbType = Int32)], CommandType='Text', CommandTimeout='30']
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
      Executing ViewResult, running view ~/Views/Customer/Payment.cshtml.
info: Microsoft.AspNetCore.Mvc.ViewFeatures.ViewResultExecutor[4]
      Executed ViewResult - view ~/Views/Customer/Payment.cshtml executed in 5.7932ms.
info: Microsoft.AspNetCore.Mvc.Infrastructure.ControllerActionInvoker[105]
      Executed action WebApplication.Controllers.PaymentController.Submit (WebApplication) in 311.1336ms
info: Microsoft.AspNetCore.Routing.EndpointMiddleware[1]
      Executed endpoint 'WebApplication.Controllers.PaymentController.Submit (WebApplication)'
info: Microsoft.AspNetCore.Hosting.Diagnostics[2]
      Request finished HTTP/2 GET https://localhost:7177/Payment/Submit?orderId=3 - 200 - text/html;+charset=utf-8 320.3236ms
info: Microsoft.AspNetCore.Hosting.Diagnostics[1]
      Request starting HTTP/2 GET https://localhost:7177/_framework/aspnetcore-browser-refresh.js - - -
info: Microsoft.AspNetCore.Hosting.Diagnostics[1]
      Request starting HTTP/2 GET https://localhost:7177/_vs/browserLink - - -
info: Microsoft.AspNetCore.Hosting.Diagnostics[2]
      Request finished HTTP/2 GET https://localhost:7177/_framework/aspnetcore-browser-refresh.js - 200 14933 application/javascript;+charset=utf-8 3.6311ms
info: Microsoft.AspNetCore.Hosting.Diagnostics[2]
      Request finished HTTP/2 GET https://localhost:7177/_vs/browserLink - 200 - text/javascript;+charset=UTF-8 12.1012ms
info: Microsoft.AspNetCore.Hosting.Diagnostics[1]
      Request starting HTTP/2 GET https://localhost:7177/Cart/GetCartCount - - -
info: Microsoft.AspNetCore.Routing.EndpointMiddleware[0]
      Executing endpoint 'WebApplication.Controllers.CartController.GetCartCount (WebApplication)'
info: Microsoft.AspNetCore.Hosting.Diagnostics[1]
      Request starting HTTP/2 GET https://localhost:7177/Customer/NotificationCount - - -
info: Microsoft.AspNetCore.Mvc.Infrastructure.ControllerActionInvoker[102]
      Route matched with {action = "GetCartCount", controller = "Cart"}. Executing controller action with signature System.Threading.Tasks.Task`1[Microsoft.AspNetCore.Mvc.IActionResult] GetCartCount(System.Threading.CancellationToken) on controller WebApplication.Controllers.CartController (WebApplication).
info: Microsoft.AspNetCore.Routing.EndpointMiddleware[0]
      Executing endpoint 'WebApplication.Controllers.CustomerController.NotificationCount (WebApplication)'
info: Microsoft.AspNetCore.Mvc.Infrastructure.ControllerActionInvoker[102]
      Route matched with {action = "NotificationCount", controller = "Customer"}. Executing controller action with signature System.Threading.Tasks.Task`1[Microsoft.AspNetCore.Mvc.IActionResult] NotificationCount(System.Threading.CancellationToken) on controller WebApplication.Controllers.CustomerController (WebApplication).
info: Microsoft.EntityFrameworkCore.Database.Command[20101]
      Executed DbCommand (58ms) [Parameters=[@__userId_Value_0='?' (DbType = Int32)], CommandType='Text', CommandTimeout='30']
      SELECT TOP(1) [c].[CartId], [c].[CreatedAt], [c].[GuestSessionId], [c].[IsCheckedOut], [c].[LastUpdatedAt], [c].[UserId]
      FROM [Cart] AS [c]
      WHERE [c].[IsCheckedOut] = CAST(0 AS bit) AND [c].[UserId] = @__userId_Value_0
      ORDER BY [c].[CartId]
info: Microsoft.EntityFrameworkCore.Database.Command[20101]
      Executed DbCommand (56ms) [Parameters=[@__userId_0='?' (DbType = Int32)], CommandType='Text', CommandTimeout='30']
      SELECT COUNT(*)
      FROM [Notification] AS [n]
      WHERE [n].[UserId] = @__userId_0 AND [n].[IsRead] = CAST(0 AS bit)
info: Microsoft.AspNetCore.Mvc.Infrastructure.SystemTextJsonResultExecutor[1]
      Executing JsonResult, writing value of type 'WebApplication.Models.ApiResponse'.
info: Microsoft.AspNetCore.Mvc.Infrastructure.SystemTextJsonResultExecutor[1]
      Executing JsonResult, writing value of type '<>f__AnonymousType6`1[[System.Int32, System.Private.CoreLib, Version=8.0.0.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e]]'.
info: Microsoft.AspNetCore.Mvc.Infrastructure.ControllerActionInvoker[105]
      Executed action WebApplication.Controllers.CartController.GetCartCount (WebApplication) in 65.6174ms
info: Microsoft.AspNetCore.Mvc.Infrastructure.ControllerActionInvoker[105]
      Executed action WebApplication.Controllers.CustomerController.NotificationCount (WebApplication) in 63.6792ms
info: Microsoft.AspNetCore.Routing.EndpointMiddleware[1]
      Executed endpoint 'WebApplication.Controllers.CartController.GetCartCount (WebApplication)'
info: Microsoft.AspNetCore.Routing.EndpointMiddleware[1]
      Executed endpoint 'WebApplication.Controllers.CustomerController.NotificationCount (WebApplication)'
info: Microsoft.AspNetCore.Hosting.Diagnostics[2]
      Request finished HTTP/2 GET https://localhost:7177/Cart/GetCartCount - 200 - application/json;+charset=utf-8 78.1370ms
info: Microsoft.AspNetCore.Hosting.Diagnostics[2]
      Request finished HTTP/2 GET https://localhost:7177/Customer/NotificationCount - 200 - application/json;+charset=utf-8 76.9545ms
info: Microsoft.EntityFrameworkCore.Database.Command[20101]
      Executed DbCommand (56ms) [Parameters=[@__p_0='?' (DbType = Int32)], CommandType='Text', CommandTimeout='30']
      SELECT TOP(@__p_0) [n].[NotificationId], [n].[Body], [n].[Channel], [n].[CreatedAt], [n].[FailureReason], [n].[IsRead], [n].[NotifType], [n].[OrderId], [n].[ReadAt], [n].[Recipient], [n].[RetryCount], [n].[SentAt], [n].[Status], [n].[Subject], [n].[TicketId], [n].[UserId]
      FROM [Notification] AS [n]
      WHERE [n].[Status] = N'Pending' AND [n].[RetryCount] < 3 AND [n].[Channel] = N'Email'
      ORDER BY [n].[CreatedAt]
info: Microsoft.EntityFrameworkCore.Database.Command[20101]
      Executed DbCommand (56ms) [Parameters=[], CommandType='Text', CommandTimeout='30']
      SELECT COUNT(*)
      FROM [ProductVariant] AS [p]
info: Microsoft.EntityFrameworkCore.Database.Command[20101]
      Executed DbCommand (56ms) [Parameters=[], CommandType='Text', CommandTimeout='30']
      SELECT COUNT(*)
      FROM [ProductVariant] AS [p]
info: Microsoft.AspNetCore.Hosting.Diagnostics[1]
      Request starting HTTP/2 POST https://localhost:7177/Payment/SubmitBankTransfer - multipart/form-data;+boundary=----WebKitFormBoundaryV0CSIKTlUipoHJcT 86093
info: Microsoft.AspNetCore.Cors.Infrastructure.CorsService[4]
      CORS policy execution successful.
info: Microsoft.AspNetCore.Routing.EndpointMiddleware[0]
      Executing endpoint 'WebApplication.Controllers.PaymentController.SubmitBankTransfer (WebApplication)'
info: Microsoft.AspNetCore.Mvc.Infrastructure.ControllerActionInvoker[102]
      Route matched with {action = "SubmitBankTransfer", controller = "Payment"}. Executing controller action with signature System.Threading.Tasks.Task`1[Microsoft.AspNetCore.Mvc.IActionResult] SubmitBankTransfer(Int32, System.String, System.String, Microsoft.AspNetCore.Http.IFormFile, System.Threading.CancellationToken) on controller WebApplication.Controllers.PaymentController (WebApplication).
info: Microsoft.EntityFrameworkCore.Database.Command[20101]
      Executed DbCommand (57ms) [Parameters=[@__orderId_0='?' (DbType = Int32)], CommandType='Text', CommandTimeout='30']
      SELECT TOP(1) [o].[OrderId], [o].[CartId], [o].[ContactPhone], [o].[CreatedAt], [o].[DeliveryInstructions], [o].[DiscountAmount], [o].[FulfillmentType], [o].[GuestSessionId], [o].[IsDeleted], [o].[IsWalkIn], [o].[OrderDate], [o].[OrderNumber], [o].[OrderStatus], [o].[POSSessionId], [o].[ShippingAddressId], [o].[ShippingFee], [o].[SubTotal], [o].[TotalAmount], [o].[UpdatedAt], [o].[UserId]
      FROM [Order] AS [o]
      WHERE [o].[IsDeleted] = CAST(0 AS bit) AND [o].[OrderId] = @__orderId_0
info: Microsoft.EntityFrameworkCore.Database.Command[20101]
      Executed DbCommand (56ms) [Parameters=[@__orderId_0='?' (DbType = Int32)], CommandType='Text', CommandTimeout='30']
      SELECT CASE
          WHEN EXISTS (
              SELECT 1
              FROM [Payment] AS [p]
              WHERE [p].[OrderId] = @__orderId_0 AND [p].[PaymentStatus] <> N'Failed') THEN CAST(1 AS bit)
          ELSE CAST(0 AS bit)
      END
info: Microsoft.EntityFrameworkCore.Database.Command[20101]
      Executed DbCommand (57ms) [Parameters=[@__orderId_0='?' (DbType = Int32)], CommandType='Text', CommandTimeout='30']
      SELECT TOP(1) [o].[OrderId], [o].[CartId], [o].[ContactPhone], [o].[CreatedAt], [o].[DeliveryInstructions], [o].[DiscountAmount], [o].[FulfillmentType], [o].[GuestSessionId], [o].[IsDeleted], [o].[IsWalkIn], [o].[OrderDate], [o].[OrderNumber], [o].[OrderStatus], [o].[POSSessionId], [o].[ShippingAddressId], [o].[ShippingFee], [o].[SubTotal], [o].[TotalAmount], [o].[UpdatedAt], [o].[UserId]
      FROM [Order] AS [o]
      WHERE [o].[IsDeleted] = CAST(0 AS bit) AND [o].[OrderId] = @__orderId_0
      ORDER BY [o].[OrderId]
info: Microsoft.EntityFrameworkCore.Database.Command[20101]
      Executed DbCommand (56ms) [Parameters=[@__orderId_0='?' (DbType = Int32)], CommandType='Text', CommandTimeout='30']
      SELECT [o0].[OrderItemId], [o0].[OrderId], [o0].[ProductId], [o0].[ProductVariantId], [o0].[Quantity], [o0].[UnitPrice], [t].[OrderId]
      FROM (
          SELECT TOP(1) [o].[OrderId]
          FROM [Order] AS [o]
          WHERE [o].[IsDeleted] = CAST(0 AS bit) AND [o].[OrderId] = @__orderId_0
      ) AS [t]
      INNER JOIN [OrderItem] AS [o0] ON [t].[OrderId] = [o0].[OrderId]
      ORDER BY [t].[OrderId]
fail: Microsoft.EntityFrameworkCore.Update[10000]
      An exception occurred in the database while saving changes for context type 'WebApplication.DataAccess.Context.AppDbContext'.
      System.InvalidOperationException: The configured execution strategy 'SqlServerRetryingExecutionStrategy' does not support user-initiated transactions. Use the execution strategy returned by 'DbContext.Database.CreateExecutionStrategy()' to execute all the operations in the transaction as a retriable unit.
         at Microsoft.EntityFrameworkCore.Storage.ExecutionStrategy.OnFirstExecution()
         at Microsoft.EntityFrameworkCore.Storage.ExecutionStrategy.ExecuteAsync[TState,TResult](TState state, Func`4 operation, Func`4 verifySucceeded, CancellationToken cancellationToken)
         at Microsoft.EntityFrameworkCore.DbContext.SaveChangesAsync(Boolean acceptAllChangesOnSuccess, CancellationToken cancellationToken)
      System.InvalidOperationException: The configured execution strategy 'SqlServerRetryingExecutionStrategy' does not support user-initiated transactions. Use the execution strategy returned by 'DbContext.Database.CreateExecutionStrategy()' to execute all the operations in the transaction as a retriable unit.
         at Microsoft.EntityFrameworkCore.Storage.ExecutionStrategy.OnFirstExecution()
         at Microsoft.EntityFrameworkCore.Storage.ExecutionStrategy.ExecuteAsync[TState,TResult](TState state, Func`4 operation, Func`4 verifySucceeded, CancellationToken cancellationToken)
         at Microsoft.EntityFrameworkCore.DbContext.SaveChangesAsync(Boolean acceptAllChangesOnSuccess, CancellationToken cancellationToken)
info: Microsoft.AspNetCore.Mvc.RedirectToActionResult[1]
      Executing RedirectResult, redirecting to /Payment/Submit?orderId=3.
info: Microsoft.AspNetCore.Mvc.Infrastructure.ControllerActionInvoker[105]
      Executed action WebApplication.Controllers.PaymentController.SubmitBankTransfer (WebApplication) in 2173.6876ms
info: Microsoft.AspNetCore.Routing.EndpointMiddleware[1]
      Executed endpoint 'WebApplication.Controllers.PaymentController.SubmitBankTransfer (WebApplication)'
info: Microsoft.AspNetCore.Hosting.Diagnostics[2]
      Request finished HTTP/2 POST https://localhost:7177/Payment/SubmitBankTransfer - 302 - - 2184.0226ms
info: Microsoft.AspNetCore.Hosting.Diagnostics[1]
      Request starting HTTP/2 GET https://localhost:7177/Payment/Submit?orderId=3 - - -
info: Microsoft.AspNetCore.Routing.EndpointMiddleware[0]
      Executing endpoint 'WebApplication.Controllers.PaymentController.Submit (WebApplication)'
info: Microsoft.AspNetCore.Mvc.Infrastructure.ControllerActionInvoker[102]
      Route matched with {action = "Submit", controller = "Payment"}. Executing controller action with signature System.Threading.Tasks.Task`1[Microsoft.AspNetCore.Mvc.IActionResult] Submit(Int32, System.Threading.CancellationToken) on controller WebApplication.Controllers.PaymentController (WebApplication).
info: Microsoft.EntityFrameworkCore.Database.Command[20101]
      Executed DbCommand (57ms) [Parameters=[@__orderId_0='?' (DbType = Int32)], CommandType='Text', CommandTimeout='30']
      SELECT TOP(1) [o].[OrderId], [o].[CartId], [o].[ContactPhone], [o].[CreatedAt], [o].[DeliveryInstructions], [o].[DiscountAmount], [o].[FulfillmentType], [o].[GuestSessionId], [o].[IsDeleted], [o].[IsWalkIn], [o].[OrderDate], [o].[OrderNumber], [o].[OrderStatus], [o].[POSSessionId], [o].[ShippingAddressId], [o].[ShippingFee], [o].[SubTotal], [o].[TotalAmount], [o].[UpdatedAt], [o].[UserId], [p].[PickupOrderId], [a].[AddressId], [p].[OrderId], [p].[PickupConfirmedAt], [p].[PickupExpiresAt], [p].[PickupReadyAt], [a].[City], [a].[Country], [a].[CreatedAt], [a].[IsDefault], [a].[IsSnapshot], [a].[Label], [a].[PostalCode], [a].[Province], [a].[Street], [a].[UserId]
      FROM [Order] AS [o]
      LEFT JOIN [PickupOrder] AS [p] ON [o].[OrderId] = [p].[OrderId]
      LEFT JOIN [Address] AS [a] ON [o].[ShippingAddressId] = [a].[AddressId]
      WHERE [o].[IsDeleted] = CAST(0 AS bit) AND [o].[OrderId] = @__orderId_0
      ORDER BY [o].[OrderId], [p].[PickupOrderId], [a].[AddressId]
info: Microsoft.EntityFrameworkCore.Database.Command[20101]
      Executed DbCommand (57ms) [Parameters=[@__orderId_0='?' (DbType = Int32)], CommandType='Text', CommandTimeout='30']
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
      Executed DbCommand (58ms) [Parameters=[@__orderId_0='?' (DbType = Int32)], CommandType='Text', CommandTimeout='30']
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
      Executed DbCommand (56ms) [Parameters=[@__orderId_0='?' (DbType = Int32)], CommandType='Text', CommandTimeout='30']
      SELECT [t0].[PaymentId], [t0].[Amount], [t0].[CreatedAt], [t0].[OrderId], [t0].[PaymentDate], [t0].[PaymentMethod], [t0].[PaymentStage], [t0].[PaymentStatus], [t0].[PaymentId0], [t0].[GcashTransactionId], [t0].[ScreenshotUrl], [t0].[StorageBucket], [t0].[StoragePath], [t0].[SubmittedAt], [t0].[PaymentId1], [t0].[BpiReferenceNumber], [t0].[ProofStorageBucket], [t0].[ProofStoragePath], [t0].[ProofUrl], [t0].[SubmittedAt0], [t0].[VerificationDeadline], [t0].[VerificationNotes], [t0].[VerifiedAt], [t0].[VerifiedByUserId], [t].[OrderId], [t].[PickupOrderId], [t].[AddressId]
      FROM (
          SELECT TOP(1) [o].[OrderId], [p].[PickupOrderId], [a].[AddressId]
          FROM [Order] AS [o]
          LEFT JOIN [PickupOrder] AS [p] ON [o].[OrderId] = [p].[OrderId]
          LEFT JOIN [Address] AS [a] ON [o].[ShippingAddressId] = [a].[AddressId]
          WHERE [o].[IsDeleted] = CAST(0 AS bit) AND [o].[OrderId] = @__orderId_0
      ) AS [t]
      INNER JOIN (
          SELECT [p0].[PaymentId], [p0].[Amount], [p0].[CreatedAt], [p0].[OrderId], [p0].[PaymentDate], [p0].[PaymentMethod], [p0].[PaymentStage], [p0].[PaymentStatus], [g].[PaymentId] AS [PaymentId0], [g].[GcashTransactionId], [g].[ScreenshotUrl], [g].[StorageBucket], [g].[StoragePath], [g].[SubmittedAt], [b].[PaymentId] AS [PaymentId1], [b].[BpiReferenceNumber], [b].[ProofStorageBucket], [b].[ProofStoragePath], [b].[ProofUrl], [b].[SubmittedAt] AS [SubmittedAt0], [b].[VerificationDeadline], [b].[VerificationNotes], [b].[VerifiedAt], [b].[VerifiedByUserId]
          FROM [Payment] AS [p0]
          LEFT JOIN [GCashPayment] AS [g] ON [p0].[PaymentId] = [g].[PaymentId]
          LEFT JOIN [BankTransferPayment] AS [b] ON [p0].[PaymentId] = [b].[PaymentId]
      ) AS [t0] ON [t].[OrderId] = [t0].[OrderId]
      ORDER BY [t].[OrderId], [t].[PickupOrderId], [t].[AddressId]
info: Microsoft.EntityFrameworkCore.Database.Command[20101]
      Executed DbCommand (56ms) [Parameters=[@__orderId_0='?' (DbType = Int32)], CommandType='Text', CommandTimeout='30']
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
      Executing ViewResult, running view ~/Views/Customer/Payment.cshtml.
info: Microsoft.AspNetCore.Mvc.ViewFeatures.ViewResultExecutor[4]
      Executed ViewResult - view ~/Views/Customer/Payment.cshtml executed in 5.5335ms.
info: Microsoft.AspNetCore.Mvc.Infrastructure.ControllerActionInvoker[105]
      Executed action WebApplication.Controllers.PaymentController.Submit (WebApplication) in 300.3095ms
info: Microsoft.AspNetCore.Routing.EndpointMiddleware[1]
      Executed endpoint 'WebApplication.Controllers.PaymentController.Submit (WebApplication)'
info: Microsoft.AspNetCore.Hosting.Diagnostics[2]
      Request finished HTTP/2 GET https://localhost:7177/Payment/Submit?orderId=3 - 200 - text/html;+charset=utf-8 308.9254ms
info: Microsoft.AspNetCore.Hosting.Diagnostics[1]
      Request starting HTTP/2 GET https://localhost:7177/_vs/browserLink - - -
info: Microsoft.AspNetCore.Hosting.Diagnostics[1]
      Request starting HTTP/2 GET https://localhost:7177/_framework/aspnetcore-browser-refresh.js - - -
info: Microsoft.AspNetCore.Hosting.Diagnostics[2]
      Request finished HTTP/2 GET https://localhost:7177/_framework/aspnetcore-browser-refresh.js - 200 14933 application/javascript;+charset=utf-8 4.1950ms
info: Microsoft.AspNetCore.Hosting.Diagnostics[2]
      Request finished HTTP/2 GET https://localhost:7177/_vs/browserLink - 200 - text/javascript;+charset=UTF-8 12.8331ms
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
      WHERE [n].[UserId] = @__userId_0 AND [n].[IsRead] = CAST(0 AS bit)
info: Microsoft.AspNetCore.Mvc.Infrastructure.SystemTextJsonResultExecutor[1]
      Executing JsonResult, writing value of type '<>f__AnonymousType6`1[[System.Int32, System.Private.CoreLib, Version=8.0.0.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e]]'.
info: Microsoft.EntityFrameworkCore.Database.Command[20101]
      Executed DbCommand (58ms) [Parameters=[@__userId_Value_0='?' (DbType = Int32)], CommandType='Text', CommandTimeout='30']
      SELECT TOP(1) [c].[CartId], [c].[CreatedAt], [c].[GuestSessionId], [c].[IsCheckedOut], [c].[LastUpdatedAt], [c].[UserId]
      FROM [Cart] AS [c]
      WHERE [c].[IsCheckedOut] = CAST(0 AS bit) AND [c].[UserId] = @__userId_Value_0
      ORDER BY [c].[CartId]
info: Microsoft.AspNetCore.Mvc.Infrastructure.ControllerActionInvoker[105]
      Executed action WebApplication.Controllers.CustomerController.NotificationCount (WebApplication) in 61.3651ms
info: Microsoft.AspNetCore.Mvc.Infrastructure.SystemTextJsonResultExecutor[1]
      Executing JsonResult, writing value of type 'WebApplication.Models.ApiResponse'.
info: Microsoft.AspNetCore.Routing.EndpointMiddleware[1]
      Executed endpoint 'WebApplication.Controllers.CustomerController.NotificationCount (WebApplication)'
info: Microsoft.AspNetCore.Mvc.Infrastructure.ControllerActionInvoker[105]
      Executed action WebApplication.Controllers.CartController.GetCartCount (WebApplication) in 63.6976ms
info: Microsoft.AspNetCore.Hosting.Diagnostics[2]
      Request finished HTTP/2 GET https://localhost:7177/Customer/NotificationCount - 200 - application/json;+charset=utf-8 73.9646ms
info: Microsoft.AspNetCore.Routing.EndpointMiddleware[1]
      Executed endpoint 'WebApplication.Controllers.CartController.GetCartCount (WebApplication)'
info: Microsoft.AspNetCore.Hosting.Diagnostics[2]
      Request finished HTTP/2 GET https://localhost:7177/Cart/GetCartCount - 200 - application/json;+charset=utf-8 77.6068ms
info: Microsoft.EntityFrameworkCore.Database.Command[20101]
      Executed DbCommand (58ms) [Parameters=[], CommandType='Text', CommandTimeout='30']
      SELECT COUNT(*)
      FROM [ProductVariant] AS [p]
info: Microsoft.EntityFrameworkCore.Database.Command[20101]
      Executed DbCommand (59ms) [Parameters=[@__p_0='?' (DbType = Int32)], CommandType='Text', CommandTimeout='30']
      SELECT TOP(@__p_0) [n].[NotificationId], [n].[Body], [n].[Channel], [n].[CreatedAt], [n].[FailureReason], [n].[IsRead], [n].[NotifType], [n].[OrderId], [n].[ReadAt], [n].[Recipient], [n].[RetryCount], [n].[SentAt], [n].[Status], [n].[Subject], [n].[TicketId], [n].[UserId]
      FROM [Notification] AS [n]
      WHERE [n].[Status] = N'Pending' AND [n].[RetryCount] < 3 AND [n].[Channel] = N'Email'
      ORDER BY [n].[CreatedAt]
info: Microsoft.EntityFrameworkCore.Database.Command[20101]
      Executed DbCommand (59ms) [Parameters=[], CommandType='Text', CommandTimeout='30']
      SELECT COUNT(*)
      FROM [ProductVariant] AS [p]
