-- =============================================================================
-- Taurus Bike Shop  |  TaurusBikeShopDB
-- File   : Taurus_seed.sql
-- Purpose: Populate all reference and product data
-- MSSQL  : SQL Server 2016+ (FreeASPHosting.net compatible)
-- Version: 3.2 - Aligned to schema v3.1 (Payment & Delivery changes)
--
-- HOW TO RUN:
--   1. Run Taurus_schema.sql FIRST to create all tables.
--   2. Connect to the same database.
--   3. Execute this script.
--
-- RECORDS INSERTED:
--    5 Roles
--   13 Categories  (from PARTS-PRICES.xlsx column headers)
--   43 Brands      (derived + web-researched)
--  148 Products    (enriched from PARTS-PRICES.xlsx)
--    2 Users       (1 Admin staff + 1 Customer, for test)
--    2 UserRoles
--    2 ProductVariants
--    2 ProductImages
--    2 Suppliers
--    2 PurchaseOrders
--    2 PurchaseOrderItems
--    2 Vouchers
--    2 UserVouchers
--    2 Carts
--    2 CartItems
--    2 Wishlist entries
--    3 Orders
--    3 OrderItems
--    2 VoucherUsages
--    2 InventoryLogs
--    2 Payments
--    2 Deliveries  (online orders only; walk-in has no delivery row)
--    2 Reviews
--    2 POS_Sessions
--    2 SystemLogs
-- =============================================================================
SET NOCOUNT ON;
GO


-- =============================================
-- SECTION 1: Roles
-- =============================================
INSERT INTO [Role] (RoleName, [Description]) VALUES
('Admin',    'Full system access - manage all aspects'),
('Manager',  'Store manager - access to reports and inventory'),
('Cashier',  'POS operator - process sales and returns'),
('Staff',    'General staff - limited access'),
('Customer', 'Customer account - web and POS purchases');
GO


-- =============================================
-- SECTION 2: Categories  (from Excel column headers)
-- =============================================
SET IDENTITY_INSERT Category ON;
GO

INSERT INTO Category (CategoryId, CategoryCode, [Name], [Description], DisplayOrder)
VALUES
( 1,'UNIT',  'Complete Bike Units',  'Fully assembled bicycles ready to ride — MTB, gravel, and CX builds.',                                   1),
( 2,'FRAME', 'Frames',               'Bicycle framesets in alloy, carbon, and steel for MTB and road/gravel disciplines.',                       2),
( 3,'FORK',  'Forks',                'Suspension and rigid forks; coil, air, and carbon options for mountain bikes.',                            3),
( 4,'HUB',   'Hubs',                 'Front and rear wheel hubs in standard QR and Boost axle standards.',                                      4),
( 5,'UPGKIT','Upgrade Kits',         'Drivetrain upgrade kits: shifter, derailleur, cassette, and chain bundles for 9–12 speed.',               5),
( 6,'STEM',  'Stems & Seatposts',    'Handlebar stems and seatposts in alloy and composite materials.',                                         6),
( 7,'HBAR',  'Handlebars',           'Flat bars, riser bars, and drop bars for MTB, gravel, and road bicycles.',                                7),
( 8,'SADDLE','Saddles',              'Bicycle seats in various widths and padding levels for trail, road, and enduro riding.',                   8),
( 9,'GRIP',  'Grips & Bar Tape',     'Handlebar grips, lock-on grips, and bar tape for MTB, gravel, and road bikes.',                          9),
(10,'PEDAL', 'Pedals',               'Platform, touring, and clipless pedals in alloy and composite materials.',                                10),
(11,'RIM',   'Rims & Wheelsets',     'Single rims and complete tubeless-ready wheelsets for 27.5", 29", and 700c standards.',                  11),
(12,'TIRE',  'Tires & Tubes',        'Mountain bike, gravel, and BMX tires plus inner tubes across multiple sizes.',                            12),
(13,'CHAIN', 'Chains',               'Bicycle drive chains from 8-speed to 12-speed in standard and oil-slick finishes.',                       13);
GO

SET IDENTITY_INSERT Category OFF;
GO


-- =============================================
-- SECTION 3: Brands
-- =============================================
SET IDENTITY_INSERT Brand ON;
GO

INSERT INTO Brand (BrandId, BrandName, Country, Website, [Description])
VALUES
( 1,'Pinewood',         'Philippines', 'https://www.facebook.com/PINEWOODBIKE/',    'Philippine bicycle brand offering quality MTB, gravel, and road bikes at competitive prices.'),
( 2,'Cult',             'USA',         'https://cultcrew.com/',                      'American BMX and MTB brand known for durable components and complete builds.'),
( 3,'Toseek',           'China',       'https://www.toseek.com/',                   'Chinese OEM bicycle component manufacturer specializing in alloy and carbon parts.'),
( 4,'Ryder',            'Philippines', NULL,                                         'Philippine bicycle brand offering affordable alloy MTB framesets and complete bikes.'),
( 5,'Garuda',           'Philippines', NULL,                                         'Philippine bicycle brand known for trail and enduro MTB builds.'),
( 6,'Kespor',           'Taiwan',      'https://www.kespor.com/',                   'Taiwanese bicycle brand producing gravel and CX bikes with high-grade components.'),
( 7,'Elves',            'China',       'https://www.elvesbike.com/',                'Chinese high-performance carbon MTB frame manufacturer.'),
( 8,'MountainPeak',     'Philippines', NULL,                                         'Philippine MTB frameset brand offering alloy frames for trail and enduro riders.'),
( 9,'Specialized',      'USA',         'https://www.specialized.com/',              'Global bicycle brand producing high-performance bikes and components for all disciplines.'),
(10,'Weapon',           'Taiwan',      NULL,                                         'Taiwanese bicycle component brand producing frames, forks, stems, and handlebars.'),
(11,'Saturn',           'Philippines', NULL,                                         'Philippine MTB frameset brand offering alloy and carbon frame options.'),
(12,'Sagmit',           'Philippines', NULL,                                         'Philippine bicycle component brand offering affordable hubs, saddles, rims, and drivetrain parts.'),
(13,'COLE',             'Taiwan',      'https://www.cole-bikes.com/',               'Taiwanese premium MTB frame manufacturer known for carbon and alloy trail geometry.'),
(14,'Speedone',         'Philippines', NULL,                                         'Philippine bicycle component brand offering hubs, rims, and pedals for MTB and road use.'),
(15,'Genova',           'Philippines', NULL,                                         'Philippine bicycle component brand offering hubs, saddles, and accessories.'),
(16,'Origin8',          'USA',         'https://www.origin8.com/',                  'American bicycle component brand offering hubs, handlebars, and BMX/urban parts.'),
(17,'LDCNC',            'Taiwan',      NULL,                                         'Taiwanese precision CNC-machined hub and wheelset manufacturer.'),
(18,'LTwoo',            'China',       'https://www.ltwoo.cn/',                     'Chinese drivetrain component manufacturer offering Shimano-compatible groupsets.'),
(19,'SRAM',             'USA',         'https://www.sram.com/',                     'American drivetrain brand producing Eagle, NX, X01, and XX1 MTB groupsets.'),
(20,'Shimano',          'Japan',       'https://www.shimano.com/',                  'Japanese component giant producing Deore, SLX, XT, XTR, and ZEE MTB groupsets.'),
(21,'Controltech',      'Taiwan',      'https://www.controltechbikes.com/',         'Taiwanese OEM manufacturer of stems, seatposts, and handlebars.'),
(22,'Answer',           'USA',         'https://www.answerproducts.com/',           'American bicycle component brand known for ProTaper handlebars and MTB products.'),
(23,'Ragusa',           'Philippines', NULL,                                         'Philippine bicycle component brand offering saddles, cranks, and pedals.'),
(24,'Seer',             'Philippines', NULL,                                         'Philippine bicycle component brand producing bar tape, grips, and accessories.'),
(25,'Attack',           'Philippines', NULL,                                         'Philippine bicycle accessories brand offering grips and handlebar components.'),
(26,'MKS',              'Japan',       'https://www.mkspedal.com/',                 'Japanese pedal manufacturer renowned for touring and platform pedals since 1943.'),
(27,'WTB',              'USA',         'https://www.wtb.com/',                      'American rim, tire, and saddle brand; pioneer of tubeless-ready technology.'),
(28,'American Classic', 'USA',         'https://www.americanclassic.com/',          'American wheel and hub manufacturer producing lightweight tubeless-ready wheelsets.'),
(29,'Jalco',            'Taiwan',      NULL,                                         'Taiwanese rim manufacturer producing double-wall alloy and tubeless-ready rims.'),
(30,'Kore',             'Taiwan',      NULL,                                         'Taiwanese bicycle component brand offering rims, stems, and handlebars.'),
(31,'Maxxis',           'Taiwan',      'https://www.maxxis.com/',                   'Taiwanese tire manufacturer; global leader in MTB, road, and BMX tires.'),
(32,'Leo',              'Taiwan',      NULL,                                         'Taiwanese inner tube manufacturer for BMX, kids, and MTB applications.'),
(33,'SUMC',             'China',       NULL,                                         'Chinese chain manufacturer producing standard and oil-slick finished drivetrain chains.'),
(34,'KMC',              'Taiwan',      'https://www.kmcchain.com/',                 'Taiwanese premium chain manufacturer offering X-Series chains for 8- to 12-speed drivetrains.'),
(35,'CT',               'Taiwan',      NULL,                                         'Taiwanese chain manufacturer specializing in fixie and single-speed heavy-duty chains.'),
(36,'GT',               'Taiwan',      NULL,                                         'Taiwanese chain manufacturer offering 10- and 11-speed standard chains.'),
(37,'Manitou',          'USA',         'https://www.manitoumtb.com/',               'American fork manufacturer producing trail and enduro suspension forks.'),
(38,'SR Suntour',       'Taiwan',      'https://www.srsuntour-cycling.com/',        'Taiwanese suspension fork manufacturer widely used in entry-to-mid-level MTBs.'),
(39,'Aeroic',           'Philippines', NULL,                                         'Philippine bicycle component brand offering forks and cranksets.'),
(40,'San Marco',        'Italy',       'https://www.selle-sanmarco.it/',            'Italian premium saddle manufacturer producing road and MTB saddles since 1935.'),
(41,'Velo',             'Taiwan',      'https://www.velo.com.tw/',                  'Taiwanese OEM saddle manufacturer supplying major bicycle brands worldwide.'),
(42,'Easydo',           'China',       NULL,                                         'Chinese bicycle accessory brand offering ergonomic saddles and cycling components.'),
(43,'Giant',            'Taiwan',      'https://www.giant-bicycles.com/',           'World''s largest bicycle manufacturer producing bikes and OEM components for all disciplines.');
GO

SET IDENTITY_INSERT Brand OFF;
GO


-- =============================================
-- SECTION 4: Products  (148 rows from PARTS-PRICES.xlsx,
--  enriched with brand, material, specs, etc.)
-- =============================================
SET IDENTITY_INSERT Product ON;
GO

-- ---- Category 1: Complete Bike Units ----
INSERT INTO Product
    (ProductId,CategoryId,BrandId,SKU,[Name],ShortDescription,[Description],
     Material,Color,WheelSize,SpeedCompatibility,BoostCompatible,TubelessReady,
     AxleStandard,SuspensionTravel,BrakeType,AdditionalSpecs,Price,StockQuantity)
VALUES
(1,1,1,'UNIT-PIN-001','2021 Pinewood Climber CARBON 27.5',
 'Full-carbon XC mountain bike with hydraulic disc brakes and 27.5" wheels.',
 'High-performance XC hardtail featuring a T800 carbon frameset, 12-speed drivetrain, and hydraulic disc brakes. Competitive trail and XC riding.',
 'Carbon Fiber T800','Matte Black / Red','27.5"','12-speed',1,1,
 'TA 12x148mm (R) / 12x100mm (F)','N/A – Hardtail','Hydraulic Disc',
 'Headset: Tapered 44-56mm; BB: BSA; Crankset: Carbon Hollowtech; Seatpost: 30.9mm',
 40000.00,5),

(2,1,2,'UNIT-CUL-001','Cult Odyssey Hydro Brakes 27.5',
 'Trail MTB complete build with hydraulic brakes and 27.5" wheels.',
 'Sturdy alloy frame with Shimano Altus 9-speed drivetrain and hydraulic disc brakes. Great intermediate trail bike.',
 'Aluminum Alloy 6061 T6','Black','27.5"','9-speed',0,0,
 'QR 9x135mm (R) / 9x100mm (F)','100mm','Hydraulic Disc',
 'Fork: SR Suntour XCT; Drivetrain: Shimano Altus 9s; Tires: 27.5x2.1',
 14500.00,8),

(3,1,3,'UNIT-TOS-001','Toseek Chester 700c Disc Brake ALLOY (2x9)',
 'Alloy gravel/road bike with 2x9 drivetrain and disc brakes.',
 'Versatile 700c alloy gravel-road bike with Shimano 2x9 drivetrain and mechanical disc brakes. Agile geometry for paved and light gravel roads.',
 'Aluminum Alloy 6061 T6','Silver / Black','700c','2x9-speed',0,0,
 'QR 9x135mm (R) / 9x100mm (F)','N/A – Rigid','Mechanical Disc',
 'Crank: 50/34T; Cassette: 11-32T; Rim: 700c double-wall 32H; Tire: 700x32c',
 11000.00,6),

(4,1,4,'UNIT-RYD-001','Ryder X3 27.5 2021 MT200 Hydro Brakes (3x8)',
 'Budget MTB with Shimano MT200 hydraulic brakes and 3x8 drivetrain.',
 'Entry-level hardtail with MT200 hydraulic disc brakes, 3x8 drivetrain, and 6061 alloy frame. Nimble on city streets and light trails.',
 'Aluminum Alloy 6061 T6','Blue / Black','27.5"','3x8-speed',0,0,
 'QR 9x135mm (R) / 9x100mm (F)','100mm','Hydraulic Disc (Shimano MT200)',
 'Fork: SR Suntour XCT Lockout; Crankset: 3x Alloy; Tires: 27.5x2.1',
 12500.00,10),

(5,1,1,'UNIT-PIN-002','Pinewood Trident Flux',
 '1x9 alloy hardtail MTB with hydraulic brakes — trail-ready.',
 '6061 T6 alloy hardtail with tapered headtube, internal cable routing, 9-speed 1x drivetrain, and hydraulic disc brakes.',
 'Aluminum Alloy 6061 T6','Matte Grey','29"','1x9-speed',0,0,
 'QR 10x135mm (R) / 9x100mm (F)','100mm','Hydraulic Disc',
 'Fork: SR Suntour XCM30 Lockout; Shifter: Shimano Alivio 1x9; Crank: 34T Hollowtech',
 17500.00,7),

(6,1,5,'UNIT-GAR-001','Garuda Rampage',
 'Philippine trail MTB with aggressive geometry and hydraulic braking.',
 'Locally made trail MTB built for confident descents and technical terrain. Air suspension and hydraulic disc brakes at a mid-range price.',
 'Aluminum Alloy 6061 T6','Black / Neon Orange','27.5"','1x10-speed',0,0,
 'QR 9x135mm (R) / 9x100mm (F)','120mm','Hydraulic Disc',
 'Fork: Air 120mm; Drivetrain: 1x10; Tires: 27.5x2.25; Rim: Double-wall CNC',
 14500.00,5),

(7,1,1,'UNIT-PIN-003','Pinewood Challenger',
 'Value trail hardtail with Shimano Altus 2x9 and hydraulic disc brakes.',
 'Double-butted 6061 alloy frame with internal routing, MT200 hydraulic brakes, and Altus 2x9 drivetrain. Excellent all-rounder for beginner-intermediate riders.',
 'Aluminum Alloy 6061 T6','Matte Black','27.5" / 29"','2x9-speed',0,0,
 'QR 9x135mm (R) / 9x100mm (F)','100mm','Hydraulic Disc (Shimano MT200)',
 'Fork: Coil Lockout; Shifter: Shimano Altus M2000 2x9; Crank: Alloy Hollowtech 170mm',
 15500.00,9),

(8,1,6,'UNIT-KES-001','Kespor Stork Feather CX 1.0 2022',
 'Premium cyclocross / gravel bike with carbon fork and 2x11 groupset.',
 'Race-oriented CX and gravel bike with triple-butted 6069 T6 alloy frame, carbon fork, and Shimano 105 2x11 drivetrain. Tubeless-ready for fast gravel racing.',
 'Aluminum Alloy 6069 T6 Triple-Butted','Gloss White / Red','700c','2x11-speed',0,1,
 'TA 12x142mm (R) / 12x100mm (F)','N/A – Rigid','Hydraulic Disc (Flat Mount)',
 'Fork: Carbon flat-mount; Shifter: Shimano 105 STI; Cassette: 11-34T; Tires: 700x38c TLR',
 55000.00,3),

(9,1,1,'UNIT-PIN-004','Pinewood Lancer 1.0 2022 Gravel RX (2x9)',
 'Alloy gravel bike with 2x9 drivetrain, disc brakes, and mixed-terrain geometry.',
 'Lightweight triple-butted 6061 T6 alloy gravel bike with tapered headtube, internal routing, and 2x9 Shimano drivetrain. Handles gravel roads and light trails with ease.',
 'Aluminum Alloy 6061 T6 Triple-Butted','Matte Green','700c','2x9-speed',0,0,
 'QR 9x135mm (R) / 9x100mm (F)','N/A – Rigid','Mechanical Disc',
 'Fork: Alloy Tapered Gravel; Crank: 34/46T 170mm; Cassette: 9s 11-40T; Hubs: Novatec sealed',
 15500.00,6);
GO

-- ---- Category 2: Frames ----
INSERT INTO Product
    (ProductId,CategoryId,BrandId,SKU,[Name],ShortDescription,[Description],
     Material,Color,WheelSize,SpeedCompatibility,BoostCompatible,TubelessReady,
     AxleStandard,SuspensionTravel,BrakeType,AdditionalSpecs,Price,StockQuantity)
VALUES
(10,2,7,'FRAME-ELV-001','Elves Nandor Carbon MTB Frame',
 'High-end full carbon MTB frame with aggressive trail geometry.',
 'Monocoque T800 carbon frameset engineered for aggressive trail and enduro. Slack geometry and generous stack for confidence on technical terrain.',
 'Carbon Fiber T800','Raw Carbon / Black','27.5" / 29"','12-speed',1,1,
 'TA 12x148mm (R)','N/A – Hardtail','Post-Mount Disc',
 'Headtube: Tapered 44-56mm; Seat tube: 30.9mm; BB: BSA; Cable: Internal; Sizes: S/M/L',
 32000.00,4),

(11,2,4,'FRAME-RYD-001','Ryder X2 Alloy MTB Frame',
 'Budget 27.5" alloy hardtail frame with disc brake mounts.',
 '6061 alloy trail-geometry hardtail with disc brake compatibility. Ideal base for a beginner-intermediate trail build.',
 'Aluminum Alloy 6061 T6','Matte Black','27.5"','8-9-speed',0,0,
 'QR 9x135mm (R)','N/A – Hardtail','Post-Mount Disc',
 'Headtube: 1-1/8" straight; Seat tube: 27.2mm; BB: BSA; Sizes: S/M/L',
 4500.00,12),

(12,2,8,'FRAME-MNP-001','MountainPeak Monster 27.5 Frame',
 'Aggressive-geometry 27.5" alloy trail/enduro hardtail frame.',
 'Slack head angle and low BB drop for a stable, confident riding feel on technical trails. Post-mount disc brakes and Boost 148mm spacing.',
 'Aluminum Alloy 6061 T6','Gloss Black','27.5"','9-11-speed',1,0,
 'TA 12x148mm (R)','N/A – Hardtail','Post-Mount Disc',
 'Headtube: Tapered 44-56mm; BB: BSA; Internal cable routing; Boost 148mm',
 6500.00,8),

(13,2,8,'FRAME-MNP-002','MountainPeak Everest 2 Alloy Frame',
 'Trail alloy hardtail with Boost spacing and tapered headtube.',
 'Progressive trail geometry, Boost 148mm axle, and tapered headtube. Versatile platform for trail and light enduro builds.',
 'Aluminum Alloy 6061 T6','Matte Grey / Red','27.5" / 29"','10-12-speed',1,0,
 'TA 12x148mm (R)','N/A – Hardtail','Post-Mount Disc',
 'Headtube: Tapered 44-56mm; BB: BSA; Sizes: S/M/L/XL',
 7800.00,6),

(14,2,9,'FRAME-SPZ-001','Specialized Stumpjumper Alloy Frame',
 'Iconic trail MTB frameset with FACT alloy construction and carbon fork.',
 'FACT 10m alloy construction with full-carbon fork, progressive trail geometry, and Boost spacing. Optimized for 27.5" or 29" wheels.',
 'FACT 10m Aluminum Alloy','Gloss Black','27.5" / 29"','11-12-speed',1,1,
 'TA 12x148mm (R)','N/A – Hardtail','Post-Mount Disc',
 'Fork: Full carbon tapered; Sizes: S1–S6; BB: T47; Internal routing for Di2 / dropper',
 22000.00,3),

(15,2,10,'FRAME-WPN-001','Weapon Stealth 29 Alloy Frame',
 'Modern-geometry 29er alloy hardtail with internal cable routing.',
 'Tapered headtube and Boost-compatible rear for XC and light trail builds. Clean internal cable routing and matte stealth finish.',
 'Aluminum Alloy 6061 T6','Stealth Matte Black','29"','11-12-speed',1,0,
 'TA 12x148mm (R)','N/A – Hardtail','Post-Mount Disc',
 'Headtube: Tapered 44-56mm; BB: BSA; Internal cable; Sizes: S/M/L',
 6500.00,10),

(16,2,10,'FRAME-WPN-002','Weapon Spartan 29 Alloy Frame',
 'Premium 29er trail hardtail with Boost spacing and tapered headtube.',
 'More progressive trail geometry than the Stealth, with Boost 148mm rear and a wider headtube for stiffer fork pairing.',
 'Aluminum Alloy 6061 T6','Matte Army Green','29"','11-12-speed',1,0,
 'TA 12x148mm (R) – Boost','N/A – Hardtail','Post-Mount Disc',
 'Headtube: Tapered; BB: BSA; Internal routing; Boost rear axle',
 8500.00,7),

(17,2,11,'FRAME-SAT-001','Saturn Calypso Alloy MTB Frame',
 'Philippine-made alloy trail hardtail — smooth welds, modern sizing.',
 'Locally designed alloy hardtail with modern geometry for Philippine trail systems. Smooth-weld construction and consistent quality.',
 'Aluminum Alloy 6061 T6','Gloss Pearl White','27.5"','9-11-speed',0,0,
 'QR 9x135mm (R)','N/A – Hardtail','Post-Mount Disc',
 'Headtube: Tapered 44-56mm; BB: BSA; Sizes: S/M/L',
 8800.00,5),

(18,2,11,'FRAME-SAT-002','Saturn Dione Alloy MTB Frame',
 'Aggressive alloy hardtail for faster trail riding — 29er.',
 'Slacker head angle than the Calypso for faster, more confident trail descents. Boost rear spacing and 6061 alloy construction.',
 'Aluminum Alloy 6061 T6','Matte Black / Gold','29"','10-12-speed',1,0,
 'TA 12x148mm (R)','N/A – Hardtail','Post-Mount Disc',
 'Headtube: Tapered 44-56mm; Boost 148mm; Sizes: S/M/L',
 7000.00,6),

(19,2,12,'FRAME-SAG-001','Sagmit Chaser Alloy MTB Frame',
 'Affordable Philippine alloy hardtail frame for trail builds.',
 'Value 6061 alloy MTB frame with trail-friendly geometry and good stiffness. Ideal base for a budget trail build.',
 'Aluminum Alloy 6061 T6','Black','27.5"','9-10-speed',0,0,
 'QR 9x135mm (R)','N/A – Hardtail','Post-Mount Disc',
 'Headtube: Semi-integrated; BB: BSA; Sizes: S/M/L',
 7500.00,9),

(20,2,13,'FRAME-COL-001','COLE NX 27.5 TRI-FACTOR 2021 Frame',
 'Taiwanese trail alloy frame with Tri-Factor tube shaping.',
 'Proprietary triangular tube shaping results in a frameset lighter and stiffer than conventional alloy. Boost axle and tubeless-compatible.',
 'Aluminum Alloy 6069 T6 Tri-Factor','Matte Black','27.5"','11-12-speed',1,1,
 'TA 12x148mm (R)','N/A – Hardtail','Post-Mount Disc',
 'Headtube: Tapered 1-1/8 to 1.5"; BB: BSA; Sizes: 15/17/19"; Internal routing',
 6000.00,6),

(21,2,14,'FRAME-SPD-001','Speedone Floater BOOST Alloy Frame',
 'Boost-compatible alloy hardtail with modern trail geometry.',
 'Boost 148mm rear spacing and tapered headtube on a Philippine-brand 6061 alloy hardtail. Excellent base for a capable mid-range trail build.',
 'Aluminum Alloy 6061 T6','Matte Black','27.5" / 29"','11-12-speed',1,0,
 'TA 12x148mm (R) – Boost','N/A – Hardtail','Post-Mount Disc',
 'Headtube: Tapered 44-56mm; BB: BSA; Sizes: S/M/L; Internal routing',
 8500.00,5);
GO

-- ---- Category 3: Forks ----
INSERT INTO Product
    (ProductId,CategoryId,BrandId,SKU,[Name],ShortDescription,[Description],
     Material,Color,WheelSize,SpeedCompatibility,BoostCompatible,TubelessReady,
     AxleStandard,SuspensionTravel,BrakeType,AdditionalSpecs,Price,StockQuantity)
VALUES
(22,3,39,'FORK-AER-001','Aeroic Air Fork 100mm',
 'Air-sprung 100mm fork with lockout — XC and trail use.',
 'Lightweight air fork with remote lockout compatibility. Suitable for 27.5" and 29" wheels on trail and XC builds.',
 'Alloy 6061 / Steel Stanchion','Black','27.5" / 29"','Universal',0,0,
 'QR 9x100mm (F)','100mm','Post-Mount Disc',
 'Stanchion: 32mm; Steerer: 1-1/8" straight; Crown-to-axle: 465mm; Weight: ~1,950g',
 2900.00,8),

(23,3,10,'FORK-WPN-001','Weapon Cannon35 BOOST Air Fork 130mm',
 '35mm stanchion Boost air fork for aggressive trail riding.',
 'Oversized 35mm stanchions deliver significant stiffness over 32mm forks. Boost 15x110mm thru-axle and 130mm travel for trail to enduro.',
 'Alloy / Magnesium Lowers','Gloss Black','27.5" / 29"','Universal',1,0,
 'TA 15x110mm (F) – Boost','130mm','Post-Mount Disc',
 'Stanchion: 35mm; Steerer: Tapered 1-1/8 to 1.5"; Rebound: External; Weight: ~2,100g',
 7000.00,5),

(24,3,10,'FORK-WPN-002','Weapon Rifle Air Fork 120mm',
 '120mm air trail fork with tapered steerer.',
 'Mid-travel air fork with 32mm stanchions and tapered steerer for modern frameset compatibility. Smooth action for trail MTBs.',
 'Aluminum Alloy','Matte Black','27.5" / 29"','Universal',0,0,
 'QR 9x100mm or TA 15x100mm (F)','120mm','Post-Mount Disc',
 'Stanchion: 32mm; Steerer: Tapered; Rebound: External; Lockout: Remote-compatible; Weight: ~1,950g',
 6000.00,7),

(25,3,10,'FORK-WPN-003','Weapon Rocket Air Fork 120mm BOOST',
 'Boost 120mm air fork with 32mm stanchions.',
 'Boost 15x110mm thru-axle version of the Rifle. Improved wheel stiffness on Boost-spaced frames without sacrificing travel or weight.',
 'Aluminum Alloy','Matte Grey','27.5" / 29"','Universal',1,0,
 'TA 15x110mm (F) – Boost','120mm','Post-Mount Disc',
 'Stanchion: 32mm; Steerer: Tapered 44-56mm; Rebound: External knob; Weight: ~1,980g',
 6000.00,6),

(26,3,10,'FORK-WPN-004','Weapon Tower Air Fork 100mm',
 'Entry-level 100mm air fork — XC and light trail.',
 'Lightweight 100mm air fork on a straight steerer. Good value upgrade for budget XC and trail builds seeking air suspension feel.',
 'Aluminum Alloy','Black / Grey','27.5"','Universal',0,0,
 'QR 9x100mm (F)','100mm','Post-Mount Disc',
 'Stanchion: 32mm; Steerer: 1-1/8" straight; Lockout: Manual top-cap; Weight: ~1,900g',
 4000.00,10),

(27,3,14,'FORK-SPD-001','Speedone Soldier BOOST Air Fork 120mm',
 'Boost air fork with external rebound — popular trail upgrade.',
 'Philippine-market Boost air fork with tapered steerer and external rebound adjustment. A budget-friendly Boost upgrade for trail hardtails.',
 'Aluminum Alloy','Matte Black','27.5" / 29"','Universal',1,0,
 'TA 15x110mm (F) – Boost','120mm','Post-Mount Disc',
 'Stanchion: 32mm; Steerer: Tapered; Rebound: External; Lockout: Remote-compatible',
 5900.00,8),

(28,3,37,'FORK-MAN-001','Manitou Markhor BOOST Air Fork 120mm',
 'American 120mm Boost fork with Dorado-derived damping.',
 'Proven Dorado-derived damping in an accessible trail fork. 120mm travel, Boost axle, and tapered steerer for premium mid-range feel.',
 'Alloy / Magnesium Lowers','Gloss Black','27.5" / 29"','Universal',1,0,
 'TA 15x110mm (F) – Boost','120mm','Post-Mount Disc',
 'Stanchion: 34mm; Steerer: Tapered 1-1/8 to 1.5"; Damper: IFP; Rebound: External; Weight: ~1,900g',
 12000.00,4),

(29,3,37,'FORK-MAN-002','Manitou Machete Comp BOOST 130mm',
 'Trail-enduro Boost fork with MRD damper — 130mm travel.',
 'MRD damper with precise rebound tuning. 130mm travel and Boost thru-axle suit aggressive trail and light enduro riding.',
 'Alloy / Magnesium Lowers','Matte Black / Red','27.5" / 29"','Universal',1,0,
 'TA 15x110mm (F) – Boost','130mm','Post-Mount Disc',
 'Stanchion: 34mm; Damper: MRD; Rebound and compression: External; Weight: ~1,950g',
 16500.00,3),

(30,3,37,'FORK-MAN-003','Manitou Mattoc Comp Boost 140mm',
 'Enduro-grade 140mm Boost fork with Dorado Pro damper.',
 'Serious enduro fork with 140mm plush travel. Dorado Pro damper delivers superb control on rough terrain. Top-tier choice for technical enduro builds.',
 'Alloy / Magnesium Lowers','Gloss Black','27.5" / 29"','Universal',1,0,
 'TA 15x110mm (F) – Boost','140mm','Post-Mount Disc',
 'Stanchion: 34mm; Damper: Dorado Pro; Rebound + Compression: External; Weight: ~2,000g',
 25000.00,2),

(31,3,38,'FORK-SRS-001','SR Suntour Epixon Stealth 120mm',
 'Mid-range air fork with Stealth lower legs and hydraulic damping.',
 '120mm air-sprung travel with hydraulic bottom-out and clean Stealth lower design. Solid mid-range performance for trail MTBs.',
 'Aluminum Alloy','Matte Black (Stealth)','27.5" / 29"','Universal',1,0,
 'TA 15x110mm (F) – Boost','120mm','Post-Mount Disc',
 'Stanchion: 34mm; Steerer: Tapered; LO: Remote compatible; Rebound: External; Weight: ~1,920g',
 8600.00,6),

(32,3,38,'FORK-SRS-002','SR Suntour Raidon BOOST 130mm',
 '34mm stanchion trail-grade BOOST air fork — 130mm travel.',
 '130mm smooth travel with 34mm stanchions and Boost thru-axle. Improved stiffness and control over entry-level options.',
 'Alloy / Magnesium Lowers','Matte Black','27.5" / 29"','Universal',1,0,
 'TA 15x110mm (F) – Boost','130mm','Post-Mount Disc',
 'Stanchion: 34mm; Steerer: Tapered; Rebound: External; Lockout: Remote; Weight: ~2,050g',
 9000.00,5),

(33,3,38,'FORK-SRS-003','SR Suntour XCR 32 BOOST 120mm',
 'XC-race Boost fork — ultra-light carbon crown and 32mm stanchions.',
 'SR Suntour''s top-tier XC race fork. Carbon crown, ultra-lightweight chassis, Boost stanchions, and TurnKey remote lockout. For competitive XC where grams matter.',
 'Carbon Crown / Magnesium Lowers','White / Black','29"','Universal',1,0,
 'TA 15x110mm (F) – Boost','120mm','Post-Mount Disc',
 'Stanchion: 32mm; Crown: Carbon; Weight: ~1,600g; Rebound: External; Lockout: TurnKey',
 52000.00,1);
GO

-- ---- Category 4: Hubs ----
INSERT INTO Product
    (ProductId,CategoryId,BrandId,SKU,[Name],ShortDescription,[Description],
     Material,Color,WheelSize,SpeedCompatibility,BoostCompatible,TubelessReady,
     AxleStandard,SuspensionTravel,BrakeType,AdditionalSpecs,Price,StockQuantity)
VALUES
(34,4,17,'HUB-LDC-001','LDCNC 3.0 Sealed Bearing Hub Set',
 'Precision CNC-machined sealed bearing hub pair — front and rear.',
 'CNC-machined from 6061 alloy with 4 sealed cartridge bearings per hub. Sold as front + rear pair. Standard QR spacing.',
 'CNC Aluminum Alloy 6061','Black / Silver','Universal','9-11-speed',0,0,
 'QR 9x100mm (F) / 9x135mm (R)',NULL,'Center Lock / 6-Bolt',
 'Bearing: 4 sealed cartridge per hub; Holes: 32H; Finish: Anodized; Sold as pair',
 2200.00,10),

(35,4,14,'HUB-SPD-001','SpeedOne Soldier BOOST Hub Set',
 'Boost 110/148 sealed bearing hub set for trail MTBs.',
 'Boost-spaced hub set with 15x110mm front and 12x148mm rear thru-axles. Sealed cartridge bearings and 32-hole flanges.',
 'CNC Aluminum Alloy 6061','Black','27.5" / 29"','10-12-speed',1,0,
 'TA 15x110mm (F) / 12x148mm (R)',NULL,'6-Bolt Disc',
 'Holes: 32H; Bearing: Sealed; Freehub: Shimano HG; Sold as pair',
 3400.00,8),

(36,4,15,'HUB-GEN-001','Genova Big Dipper Sealed Bearing Hub Set',
 'Large-flange alloy hub set for improved wheel strength.',
 'Larger hub flanges for improved spoke bracing angle and stiffer wheel build. Suitable for trail and enduro applications.',
 'CNC Aluminum Alloy','Black / Red','27.5" / 29"','9-11-speed',0,0,
 'QR 9x100mm (F) / 9x135mm (R)',NULL,'6-Bolt Disc',
 'Flange: Large diameter; Holes: 32H; Bearing: Sealed; Freehub: Shimano HG; Sold as pair',
 3000.00,9),

(37,4,10,'HUB-WPN-001','Weapon Animal BOOST Hub Set',
 'CNC Boost hub set with high flanges for maximum wheel stiffness.',
 'High-flange Boost hub set CNC-machined from 6061 alloy with 4 sealed bearings per hub. Maximises wheel stiffness for trail and enduro.',
 'CNC Aluminum Alloy 6061','Black / Gold','27.5" / 29"','11-12-speed',1,0,
 'TA 15x110mm (F) / 12x148mm (R)',NULL,'6-Bolt Disc',
 'Holes: 32H; Bearing: 4 sealed per hub; Freehub: Shimano Micro Spline / HG; Sold as pair',
 4500.00,6),

(38,4,12,'HUB-SAG-001','Sagmit EVO3 Sealed Bearing Hub Set',
 '3-pawl Philippine hub set — reliable and affordable.',
 '3-pawl ratchet mechanism with sealed cartridge bearings. Popular value choice for budget trail builds in the Philippines.',
 'Aluminum Alloy','Black','Universal','9-11-speed',0,0,
 'QR 9x100mm (F) / 9x135mm (R)',NULL,'6-Bolt Disc',
 'Ratchet: 3-pawl; Holes: 32H; Freehub: Shimano HG; Bearing: Sealed; Sold as pair',
 3400.00,15),

(39,4,23,'HUB-RAG-001','Ragusa R100 Alloy Hub Set',
 'Budget alloy hub set for general trail and commuter builds.',
 'Entry-level QR alloy hub set with 32-hole flanges. Compatible with most entry-level rim builds.',
 'Aluminum Alloy','Black','Universal','7-9-speed',0,0,
 'QR 9x100mm (F) / 9x135mm (R)',NULL,'6-Bolt Disc',
 'Holes: 32H; Freehub: Shimano HG 7/8/9s; Bearing: Sealed; Sold as pair',
 950.00,20),

(40,4,14,'HUB-SPD-002','SpeedOne Soldier Standard QR Hub Set',
 'Affordable QR sealed bearing hub set for standard frames.',
 'QR axle sealed-bearing hub set for standard 135mm rear spacing. Strong seller for budget trail builds.',
 'Aluminum Alloy 6061','Black','Universal','9-11-speed',0,0,
 'QR 9x100mm (F) / 9x135mm (R)',NULL,'6-Bolt Disc',
 'Holes: 32H; Freehub: Shimano HG; Bearing: Sealed; Sold as pair',
 3000.00,12),

(41,4,14,'HUB-SPD-003','SpeedOne Pilot Carbon Hub Set',
 'Carbon-shell hub set for XC and race builds — lightweight.',
 'Carbon composite shell for reduced rotational weight. Ideal for XC race and gravel builds. Ceramic-grade sealed bearings.',
 'Carbon Composite / Alloy Axle','Black / Red','Universal','10-12-speed',1,0,
 'TA 15x110mm (F) / 12x148mm (R)',NULL,'6-Bolt Disc',
 'Holes: 28H / 32H; Freehub: Shimano HG / Micro Spline; Bearing: Ceramic-grade sealed; Sold as pair',
 3450.00,5),

(42,4,14,'HUB-SPD-004','SpeedOne Torpedo Sealed Bearing Hub Set',
 '4-pawl 36T engagement hub set — responsive trail feel.',
 '4-pawl ratchet for faster engagement and more responsive trail feel. CNC-machined 6061 alloy with 4 sealed bearings per hub.',
 'CNC Aluminum Alloy 6061','Black / Blue','27.5" / 29"','10-12-speed',1,0,
 'TA 15x110mm (F) / 12x148mm (R)',NULL,'6-Bolt Disc',
 'Ratchet: 4-pawl 36T; Holes: 32H; Freehub: Shimano HG / Micro Spline; Sold as pair',
 3600.00,7),

(43,4,16,'HUB-OR8-001','Origin8 NON-BOOST Sealed Bearing Hub Set',
 'American-brand QR alloy hub set for standard 100/135mm frames.',
 'Designed for older framesets with standard QR axle spacing. Sealed cartridge bearings and wide flange for structural integrity.',
 'CNC Aluminum Alloy 6061','Black / Silver','Universal','9-11-speed',0,0,
 'QR 9x100mm (F) / 9x135mm (R)',NULL,'6-Bolt Disc',
 'Holes: 32H; Freehub: Shimano HG 9/10/11s; Bearing: Sealed cartridge; Sold as pair',
 5200.00,6);
GO

-- ---- Category 5: Upgrade Kits ----
INSERT INTO Product
    (ProductId,CategoryId,BrandId,SKU,[Name],ShortDescription,[Description],
     Material,Color,WheelSize,SpeedCompatibility,BoostCompatible,TubelessReady,
     AxleStandard,SuspensionTravel,BrakeType,AdditionalSpecs,Price,StockQuantity)
VALUES
(44,5,12,'UPGKIT-SAG-001','Sagmit Edison 12-Speed Upgrade Kit',
 'Complete 1x12 drivetrain upkit — Shimano-compatible.',
 'Shifter, rear derailleur, 12s cassette 11-50T, and 12s chain in a single bundle. Compatible with Shimano Micro Spline and HG freehubs.',
 'Alloy / Steel','Black',NULL,'12-speed',NULL,NULL,NULL,NULL,NULL,
 'Includes: Shifter 1x12, RD, Cassette 11-50T, Chain; BB sold separately',
 5200.00,8),

(45,5,18,'UPGKIT-LTW-001','LTwoo AX 12-Speed Upkit with IXF Crank',
 'Full 1x12 upgrade kit with IXF hollow-tech crankset.',
 'LTwoo AX 12s shifter and RD paired with an IXF hollowtech crankset, 12s cassette, and chain. Quality Shimano-compatible value alternative.',
 'Alloy / CNC Alloy','Black / Silver',NULL,'12-speed',NULL,NULL,NULL,NULL,NULL,
 'Includes: LTwoo AX Shifter 1x12, RD, IXF Crankset 34T, Cassette 11-50T, Chain; BB: BSA',
 8000.00,5),

(46,5,20,'UPGKIT-SHM-001','Deore M6100 12-Speed Upkit with Weapon Cogs',
 'Shimano Deore M6100 12s upkit — benchmark trail shifting.',
 'Deore M6100 shifter and RD with Weapon Micro Spline cassette and 12s chain. The benchmark for durable, precise trail shifting.',
 'Alloy / Steel','Black',NULL,'12-speed (Micro Spline)',NULL,NULL,NULL,NULL,NULL,
 'Includes: SL-M6100 Shifter, RD-M6100, Weapon Cassette 11-50T (Micro Spline), 12s Chain',
 9000.00,4),

(47,5,20,'UPGKIT-SHM-002','ZEE M640 10-Speed Downhill Upkit',
 'Shimano ZEE M640 10s gravity-tuned drivetrain upkit.',
 'ZEE gravity-tuned 10s shifter and RD designed to withstand DH and freeride abuse. Includes compatible 10s cassette and chain.',
 'Alloy / Steel','Black',NULL,'10-speed',NULL,NULL,NULL,NULL,NULL,
 'Includes: ZEE SL-M640 Shifter, ZEE RD-M640, Cassette 11-36T, 10s Chain; Rated for DH/gravity',
 6800.00,5),

(48,5,19,'UPGKIT-SRM-001','SRAM NX Eagle 12-Speed Upkit with BB',
 'SRAM NX Eagle 1x12 complete upkit — includes bottom bracket.',
 'NX Eagle trigger shifter, RD, 11-50T XD cassette, chain, and SRAM BSA bottom bracket. Eagle X-Sync chain retention at an accessible price.',
 'Alloy / Steel','Black / Red',NULL,'12-speed (SRAM XD)',NULL,NULL,NULL,NULL,NULL,
 'Includes: NX Eagle Shifter, RD, Cassette 11-50T (XD), Chain, BSA BB; Requires XD freehub',
 13000.00,3);
GO

-- ---- Category 6: Stems & Seatposts ----
INSERT INTO Product
    (ProductId,CategoryId,BrandId,SKU,[Name],ShortDescription,[Description],
     Material,Color,WheelSize,SpeedCompatibility,BoostCompatible,TubelessReady,
     AxleStandard,SuspensionTravel,BrakeType,AdditionalSpecs,Price,StockQuantity)
VALUES
(49,6,21,'STEM-CTL-001','Controltech ONE Handlebar Post 31.8mm',
 'Lightweight 31.8mm alloy stem — 0° rise for trail geometry.',
 'Stiff, lightweight alloy stem with 31.8mm bar clamp and 1-1/8" steerer clamp. Direct, responsive steering feel for trail MTBs.',
 'CNC Aluminum Alloy 6061 T6','Black',NULL,NULL,NULL,NULL,NULL,NULL,NULL,
 'Bar clamp: 31.8mm; Steerer: 28.6mm (1-1/8"); Rise: 0°; Length: 70/80mm; Bolts: 4x M5',
 1800.00,15),

(50,6,21,'STEM-CTL-002','HandlePost ControlTech Standard',
 'Alloy stem with 6° rise — comfortable trail and XC position.',
 '6° rise and 31.8mm bar clamp for a slightly upright bar position on XC and trail MTBs.',
 'Aluminum Alloy 6061','Black / Silver',NULL,NULL,NULL,NULL,NULL,NULL,NULL,
 'Bar clamp: 31.8mm; Steerer: 28.6mm; Rise: 6°; Length: 70–90mm; Weight: ~120g',
 1600.00,18),

(51,6,21,'STEM-CTL-003','Controltech ONE Seatpost 30.9mm',
 '30.9mm alloy seatpost for trail and enduro MTBs.',
 'Lightweight straight alloy post with two-bolt saddle clamp for precise saddle angle. Diameter 30.9mm.',
 'CNC Aluminum Alloy 6061 T6','Black',NULL,NULL,NULL,NULL,NULL,NULL,NULL,
 'Diameter: 30.9mm; Length: 350/400mm; Clamp: Dual-bolt; Offset: 0mm; Weight: ~220g',
 1700.00,12),

(52,6,21,'STEM-CTL-004','HandlePost ControlTech CLS',
 'CLS-style alloy stem with integrated cable guide.',
 'Integrated cable guide system for clean internal routing setups. Compatible with 31.8mm bars and tapered steerers.',
 'CNC Aluminum Alloy 6061 T6','Black',NULL,NULL,NULL,NULL,NULL,NULL,NULL,
 'Bar clamp: 31.8mm; Rise: 6°; Cable guide: Integrated; Length: 70/80/90mm; Weight: ~135g',
 1800.00,10),

(53,6,10,'STEM-WPN-001','Weapon Fury Stem 31.8mm',
 'Budget alloy trail stem — solid construction at low price.',
 'Entry-level 31.8mm alloy stem in multiple lengths. Ideal for value trail builds requiring a clean, reliable stem.',
 'Aluminum Alloy 6061','Black',NULL,NULL,NULL,NULL,NULL,NULL,NULL,
 'Bar clamp: 31.8mm; Steerer: 28.6mm; Rise: 6°; Length: 60/70/80mm; Weight: ~130g',
 850.00,20),

(54,6,10,'STEM-WPN-002','Weapon Ambush Stem 31.8mm',
 'CNC alloy MTB stem with -6° rise — low and aggressive.',
 '-6° negative-rise alloy stem for a lower, more aerodynamic bar position. CNC machined from 6061.',
 'CNC Aluminum Alloy 6061','Black',NULL,NULL,NULL,NULL,NULL,NULL,NULL,
 'Bar clamp: 31.8mm; Steerer: 28.6mm; Rise: -6°; Length: 60/70/80/90mm; Weight: ~125g',
 1000.00,15),

(55,6,10,'STEM-WPN-003','Weapon Savage Stem 35mm',
 '35mm clamp alloy stem for oversized trail handlebars.',
 'Wider 35mm bar clamp provides increased stiffness over 31.8mm stems. Ideal for 35mm handlebars on aggressive trail and enduro builds.',
 'CNC Aluminum Alloy 6061','Black',NULL,NULL,NULL,NULL,NULL,NULL,NULL,
 'Bar clamp: 35mm; Steerer: 28.6mm; Rise: 0°; Length: 50/60/70mm; Weight: ~130g',
 1100.00,12),

(56,6,10,'STEM-WPN-004','Weapon Beast Stem 35mm',
 'Oversized 35mm bar clamp alloy stem — maximum stiffness.',
 '7075 alloy beast stem with 35mm clamp and reinforced body. Designed for enduro riders demanding precise, direct steering.',
 'CNC Aluminum Alloy 7075','Black / Red',NULL,NULL,NULL,NULL,NULL,NULL,NULL,
 'Bar clamp: 35mm; Steerer: 28.6mm; Rise: 0°; Length: 50/60/70/80mm; Weight: ~140g',
 1200.00,10),

(57,6,10,'STEM-WPN-005','Weapon Animal Stem 31.8mm',
 'Mid-range alloy trail stem — 31.8mm, 6° rise.',
 'Balanced between the Ambush and Beast: 31.8mm clamp and 6° positive rise for a comfortable trail bar height.',
 'CNC Aluminum Alloy 6061','Black',NULL,NULL,NULL,NULL,NULL,NULL,NULL,
 'Bar clamp: 31.8mm; Steerer: 28.6mm; Rise: 6°; Length: 60/70/80mm; Weight: ~128g',
 1100.00,14),

(58,6,10,'STEM-WPN-006','Weapon Predator Stem 31.8mm',
 'Reinforced 4-bolt steerer clamp alloy stem — rowdy trail use.',
 'Reinforced 4-bolt steerer clamp for enhanced torsional stiffness. Great for trail and enduro with 31.8mm bars.',
 'CNC Aluminum Alloy 6061','Black',NULL,NULL,NULL,NULL,NULL,NULL,NULL,
 'Bar clamp: 31.8mm; Steerer: 28.6mm; Rise: 0° / 6°; Length: 50/60/70/80mm; Weight: ~135g',
 1000.00,15);
GO

-- ---- Category 7: Handlebars ----
INSERT INTO Product
    (ProductId,CategoryId,BrandId,SKU,[Name],ShortDescription,[Description],
     Material,Color,WheelSize,SpeedCompatibility,BoostCompatible,TubelessReady,
     AxleStandard,SuspensionTravel,BrakeType,AdditionalSpecs,Price,StockQuantity)
VALUES
(59,7,21,'HBAR-CTL-001','Handle Bar Pro LT 31.8mm 720mm',
 'Lightweight 31.8mm alloy flat riser bar — 720mm wide.',
 'Lightweight alloy trail flat bar at 720mm width with 20mm rise. Comfortable, controlled steering for everyday trail riding.',
 'Aluminum Alloy 6061 T6','Black',NULL,NULL,NULL,NULL,NULL,NULL,NULL,
 'Clamp: 31.8mm; Width: 720mm; Rise: 20mm; Sweep: 9° back / 5° up; Weight: ~210g',
 1800.00,18),

(60,7,21,'HBAR-CTL-002','Handle Bar Pro Koryak 31.8mm 740mm',
 'Wide 31.8mm butted alloy riser bar — 740mm for enduro.',
 'Wider, stiffer butted alloy bar at 740mm. Carbon-like stiffness profile from double-butted tube construction.',
 'Butted Aluminum Alloy 6061 T6','Black',NULL,NULL,NULL,NULL,NULL,NULL,NULL,
 'Clamp: 31.8mm; Width: 740mm; Rise: 25mm; Sweep: 9° back / 5° up; Weight: ~225g',
 2200.00,14),

(61,7,21,'HBAR-CTL-003','Drop Bar Gravel Pro LT 31.8mm 420mm',
 'Compact alloy drop bar for gravel and road — 420mm.',
 'Short-reach, shallow-drop profile optimized for gravel riding. Suits 31.8mm stems.',
 'Aluminum Alloy 6061 T6','Black',NULL,NULL,NULL,NULL,NULL,NULL,NULL,
 'Clamp: 31.8mm; Width: 420mm; Reach: 75mm; Drop: 128mm; Flare: 6°; Weight: ~270g',
 1800.00,12),

(62,7,21,'HBAR-CTL-004','Drop Bar Gravel Pro Discovery 31.8mm 440mm',
 'Wide flared alloy gravel drop bar — 440mm, 16° flare.',
 '16° flare for improved grip and leverage on rough gravel. Great for adventure and bikepacking setups.',
 'Aluminum Alloy 6061 T6','Black',NULL,NULL,NULL,NULL,NULL,NULL,NULL,
 'Clamp: 31.8mm; Width: 440mm tops / 460mm drops; Flare: 16°; Reach: 80mm; Weight: ~285g',
 2200.00,10),

(63,7,22,'HBAR-ANS-001','Answer ProTaper 20x20 Alloy Bar 31.8mm 800mm',
 'Wide 800mm ProTaper bar — 20mm rise, 20° back sweep.',
 'Maximum-control DH and enduro riser at 800mm. 20mm rise and 20° back sweep for outstanding stability on technical terrain.',
 'Aluminum Alloy 7050 T6','Black',NULL,NULL,NULL,NULL,NULL,NULL,NULL,
 'Clamp: 31.8mm; Width: 800mm; Rise: 20mm; Back sweep: 20°; Up sweep: 5°; Weight: ~280g',
 2000.00,10),

(64,7,22,'HBAR-ANS-002','Answer ProTaper Alloy Bar 31.8mm 780mm',
 'Classic ProTaper 780mm riser bar — 15mm rise.',
 'Classic 780mm ProTaper profile trusted by DH and trail riders worldwide. 15mm rise and 31.8mm clamp.',
 'Aluminum Alloy 7050 T6','Black',NULL,NULL,NULL,NULL,NULL,NULL,NULL,
 'Clamp: 31.8mm; Width: 780mm; Rise: 15mm; Weight: ~265g',
 2000.00,12),

(65,7,22,'HBAR-ANS-003','Answer ProTaper 7050 Series Aluminum Bar 31.8mm 760mm',
 '7050 alloy ProTaper bar — stiff and lightweight, 760mm.',
 '7050-series alloy for improved stiffness and impact resistance over 6061 bars. 760mm width suits trail and enduro riders preferring a narrower bar.',
 'Aluminum Alloy 7050 T6','Black / Silver',NULL,NULL,NULL,NULL,NULL,NULL,NULL,
 'Clamp: 31.8mm; Width: 760mm; Rise: 15mm; Back sweep: 9°; Weight: ~255g',
 1600.00,10),

(66,7,12,'HBAR-SAG-001','Sagmit Brooklyn 3.0 Flat Bar 31.8mm 720mm',
 'Affordable Philippine alloy flat bar — 720mm trail width.',
 'Lightweight alloy flat bar for trail and urban MTB builds at an accessible Philippine price.',
 'Aluminum Alloy 6061','Black',NULL,NULL,NULL,NULL,NULL,NULL,NULL,
 'Clamp: 31.8mm; Width: 720mm; Rise: 15mm; Sweep: 9° back; Weight: ~220g',
 550.00,25),

(67,7,12,'HBAR-SAG-002','Sagmit Shadow Riser Bar 31.8mm 740mm',
 'Mid-rise alloy bar — 740mm, comfortable trail position.',
 'Mid-rise profile at 740mm balancing comfort and control for trail riding.',
 'Aluminum Alloy 6061','Matte Black',NULL,NULL,NULL,NULL,NULL,NULL,NULL,
 'Clamp: 31.8mm; Width: 740mm; Rise: 20mm; Back sweep: 9°; Weight: ~230g',
 550.00,22),

(68,7,12,'HBAR-SAG-003','Sagmit Static Riser Bar 35mm 760mm',
 'Oversized 35mm alloy enduro bar — wide and stiff.',
 'Wider 35mm clamp for improved stiffness. 760mm width suits enduro and aggressive trail riders.',
 'Aluminum Alloy 6061','Black',NULL,NULL,NULL,NULL,NULL,NULL,NULL,
 'Clamp: 35mm; Width: 760mm; Rise: 20mm; Weight: ~245g',
 750.00,15);
GO

-- ---- Category 8: Saddles ----
INSERT INTO Product
    (ProductId,CategoryId,BrandId,SKU,[Name],ShortDescription,[Description],
     Material,Color,WheelSize,SpeedCompatibility,BoostCompatible,TubelessReady,
     AxleStandard,SuspensionTravel,BrakeType,AdditionalSpecs,Price,StockQuantity)
VALUES
(69,8,12,'SADL-SAG-001','Sagmit RedClassic Skull Edition Saddle',
 'Flat MTB saddle with skull graphic and comfort foam.',
 'Firm foam core flat saddle with a PU cover and distinctive skull graphic. Steel rails.',
 'PU Cover / Foam / Steel Rails','Black / Red Skull',NULL,NULL,NULL,NULL,NULL,NULL,NULL,
 'Width: 138mm; Rail: Steel; Padding: Firm foam; Length: 268mm; Weight: ~320g',
 450.00,30),

(70,8,15,'SADL-GEN-001','Genova Jupiter MTB Saddle',
 'Lightweight trail saddle with ergonomic pressure-relief channel.',
 'Central pressure-relief channel and ergonomic padding for trail and XC riding. Alloy rails.',
 'PU Cover / Foam / Alloy Rails','Black',NULL,NULL,NULL,NULL,NULL,NULL,NULL,
 'Width: 140mm; Rail: 7mm Alloy; Length: 270mm; Channel: Central relief; Weight: ~280g',
 450.00,28),

(71,8,43,'SADL-GNT-001','Giant MTB Saddle (No Cut-Out)',
 'OEM Giant flat saddle — even weight distribution.',
 'Flat-profile OEM saddle with moderate foam padding. Suits trail riders who prefer even weight distribution without a channel.',
 'PU Cover / Foam / Steel Rails','Black',NULL,NULL,NULL,NULL,NULL,NULL,NULL,
 'Width: 142mm; Rail: Steel 7x9mm; Length: 268mm; Profile: Flat; Weight: ~330g',
 450.00,25),

(72,8,43,'SADL-GNT-002','Giant MTB Saddle (With Cut-Out)',
 'OEM Giant saddle with central relief channel.',
 'Central pressure-relief channel reduces soft-tissue pressure on longer trail and XC rides.',
 'PU Cover / Foam / Steel Rails','Black',NULL,NULL,NULL,NULL,NULL,NULL,NULL,
 'Width: 142mm; Rail: Steel; Length: 268mm; Channel: Central cut-out; Weight: ~325g',
 450.00,22),

(73,8,23,'SADL-RAG-001','Ragusa R-100 Lightweight MTB Saddle',
 'Slim, lightweight XC race saddle — narrow nose.',
 'Minimalist race saddle for XC riders who prefer a light, low-interference feel. Narrow nose reduces inner-thigh chafing.',
 'Microfiber / Foam / Alloy Rails','Black',NULL,NULL,NULL,NULL,NULL,NULL,NULL,
 'Width: 136mm; Rail: Alloy 7mm; Length: 265mm; Profile: Flat/race; Weight: ~245g',
 450.00,20),

(74,8,12,'SADL-SAG-002','Sagmit Oilslick Saddle',
 'Iridescent oilslick finish saddle for custom builds.',
 'Oil-slick iridescent rail and trim finish. Ergonomic medium-width profile for trail and daily riding.',
 'PU Cover / Foam / Anodized Alloy Rails','Black / Oilslick',NULL,NULL,NULL,NULL,NULL,NULL,NULL,
 'Width: 140mm; Rail: Anodized alloy 7mm; Length: 268mm; Finish: Oilslick; Weight: ~290g',
 550.00,15),

(75,8,15,'SADL-GEN-002','Genova Mars Trail Saddle',
 'Widened comfort trail saddle with soft foam padding.',
 'Slightly wider rear profile for better pressure distribution during long trail sessions. High-density foam padding.',
 'PU Cover / High-Density Foam / Steel Rails','Black / Grey',NULL,NULL,NULL,NULL,NULL,NULL,NULL,
 'Width: 145mm; Rail: Steel; Length: 272mm; Padding: High-density; Weight: ~340g',
 450.00,22),

(76,8,9,'SADL-SPZ-001','Specialized Body Geometry Black Saddle',
 'Specialized BG saddle with pressure-relief channel.',
 'Ergonomic BG research-optimized shape for reduced nerve pressure and improved blood flow. Quality OEM replacement.',
 'Microfiber / Foam / Steel Rails','Black',NULL,NULL,NULL,NULL,NULL,NULL,NULL,
 'Width: 143mm; Rail: Cr-Mo Steel; BG Channel: Yes; Length: 270mm; Weight: ~305g',
 450.00,18),

(77,8,9,'SADL-SPZ-002','Specialized M811 Trail MTB Saddle',
 'Specialized M811 durable trail saddle with BG channel.',
 'BG pressure-relief channel and durable microfiber cover. Upgraded OEM-spec saddle for trail and enduro builds.',
 'Microfiber / Multi-Density Foam / Steel Rails','Black',NULL,NULL,NULL,NULL,NULL,NULL,NULL,
 'Width: 143mm; Rail: Cr-Mo Steel; BG Channel: Yes; Length: 272mm; Weight: ~310g',
 450.00,15),

(78,8,40,'SADL-SMR-001','San Marco Aspide Racing Saddle',
 'Italian racing saddle — slim, lightweight, performance-oriented.',
 'Performance road/gravel saddle with slim profile and Italian heritage craftsmanship. Suitable for gravel, road, and CX builds.',
 'Microfiber / Foam / Carbon-Reinforced Nylon Shell','Black',NULL,NULL,NULL,NULL,NULL,NULL,NULL,
 'Width: 130mm; Rail: Alloy 7mm; Shell: Carbon-reinforced nylon; Length: 272mm; Weight: ~220g',
 450.00,12),

(79,8,41,'SADL-VLO-001','Velo Plush Comfort MTB Saddle',
 'Gel-padded comfort saddle for all-day trail and enduro.',
 'Generous gel padding for maximum all-day comfort. Wider profile distributes weight over a larger area.',
 'PU / Gel Foam Composite / Steel Rails','Black',NULL,NULL,NULL,NULL,NULL,NULL,NULL,
 'Width: 148mm; Rail: Steel; Gel: Yes; Length: 275mm; Weight: ~380g',
 450.00,20),

(80,8,42,'SADL-ESD-001','Easydo ES-01 Ergonomic Saddle',
 'Ergonomic saddle with central cut-out — value choice.',
 'Central relief cut-out to reduce perineal pressure. Available in multiple widths. Suits both MTB and commuter builds.',
 'PU Cover / Foam / Steel Rails','Black / Grey',NULL,NULL,NULL,NULL,NULL,NULL,NULL,
 'Width: 143mm; Rail: Steel; Cut-out: Central; Length: 270mm; Weight: ~340g',
 450.00,20),

(81,8,12,'SADL-SAG-003','Sagmit Diamond MTB Saddle',
 'Diamond-stitched flat saddle — premium look, budget price.',
 'Classic diamond stitching on a flat profile saddle. Medium foam padding for trail and daily riding.',
 'PU Cover / Foam / Steel Rails','Black',NULL,NULL,NULL,NULL,NULL,NULL,NULL,
 'Width: 140mm; Rail: Steel; Profile: Flat; Stitching: Diamond; Weight: ~330g',
 350.00,25),

(82,8,12,'SADL-TOP-001','Topgrade Trail Saddle',
 'Entry-level alloy-rail flat saddle for budget trail builds.',
 'No-frills flat saddle with adequate foam and alloy rails. Ideal for budget trail and commuter MTBs.',
 'PU Cover / Foam / Alloy Rails','Black',NULL,NULL,NULL,NULL,NULL,NULL,NULL,
 'Width: 140mm; Rail: Alloy 7mm; Profile: Flat; Length: 268mm; Weight: ~280g',
 350.00,30);
GO

-- ---- Category 9: Grips & Bar Tape ----
INSERT INTO Product
    (ProductId,CategoryId,BrandId,SKU,[Name],ShortDescription,[Description],
     Material,Color,WheelSize,SpeedCompatibility,BoostCompatible,TubelessReady,
     AxleStandard,SuspensionTravel,BrakeType,AdditionalSpecs,Price,StockQuantity)
VALUES
(83,9,24,'GRIP-SER-001','Silicon Grip Seer 5mm Lock-On',
 '5mm silicone waffle lock-on MTB grips — pair.',
 'Waffle-pattern silicone compound for vibration damping. Alloy lock rings keep grips secure on the most demanding trails.',
 'Silicone / Alloy Lock Ring','Black / Grey',NULL,NULL,NULL,NULL,NULL,NULL,NULL,
 'Diameter: 33mm; Length: 130mm; Lock ring: Alloy; Compound: 5mm Silicone; Sold as pair',
 330.00,30),

(84,9,24,'GRIP-SER-002','Seer Polymer Bartape 3mm',
 '3mm polymer bar tape for road and gravel drop bars.',
 'Firm-compound 3mm tape for road and gravel drop bars. Slim feel with reliable grip.',
 'Polymer Compound','Black',NULL,NULL,NULL,NULL,NULL,NULL,NULL,
 'Thickness: 3mm; Width: 30mm; Length: 2.5m/roll; Finish plug: Included',
 350.00,25),

(85,9,24,'GRIP-SER-003','Silicon Grip Seer 7mm Lock-On',
 '7mm silicone lock-on grips — maximum vibration damping.',
 'Thickest silicone compound in the Seer lineup for enduro riders seeking maximum hand comfort and vibration absorption.',
 'Silicone / Alloy Lock Ring','Black / Red',NULL,NULL,NULL,NULL,NULL,NULL,NULL,
 'Diameter: 35mm; Length: 130mm; Lock ring: Alloy; Compound: 7mm Silicone; Sold as pair',
 400.00,22),

(86,9,24,'GRIP-SER-004','Seer Art Racing Edition BarTape',
 'Premium art-print bar tape for race and gravel builds.',
 'Dense foam substrate with stylized art-print overcoat for a premium aesthetic. Suitable for road, gravel, and CX builds.',
 'Foam / PU Print Layer','Black / White Art',NULL,NULL,NULL,NULL,NULL,NULL,NULL,
 'Thickness: 3.5mm; Length: 2.5m; Finish plug: Cork; Includes end tape',
 400.00,18),

(87,9,24,'GRIP-SER-005','Seer SILICON Hornet Bar Tape 4mm',
 '4mm silicone bar tape for gravel drop bars — non-slip.',
 'Silicone compound provides a non-slip grip even in wet conditions. Generous 4mm cushioning for rough gravel surfaces.',
 'Silicone Compound','Black',NULL,NULL,NULL,NULL,NULL,NULL,NULL,
 'Thickness: 4mm; Length: 2.5m; Grip: Non-slip Silicone; Includes end tape',
 450.00,15),

(88,9,24,'GRIP-SER-006','Seer POLYMER Hornet Bar Tape 3.5mm',
 'Hexagonal hornet-pattern polymer tape for road and gravel bars.',
 'Hexagonal texture for enhanced grip and a distinctive look. 3.5mm balances comfort and bar-feel.',
 'Polymer Compound','Black / Yellow',NULL,NULL,NULL,NULL,NULL,NULL,NULL,
 'Thickness: 3.5mm; Texture: Hornet hexagonal; Length: 2.5m; Includes cork plug',
 450.00,18),

(89,9,24,'GRIP-SER-007','Seer Super Lite Bar tape 2mm',
 'Ultra-thin 2mm road race bar tape — natural bar feel.',
 'Designed for road racers wanting minimal bulk and a direct connection to the bar. 2mm flat-compound tape.',
 'Thin Polymer Compound','Black',NULL,NULL,NULL,NULL,NULL,NULL,NULL,
 'Thickness: 2mm; Length: 2.5m; Suitable for: Road race; Includes end tape',
 300.00,20),

(90,9,25,'GRIP-ATK-001','Handle Grip Attack Dual Lock-On',
 'Dual lock-on alloy ring grips — maximum grip security.',
 'Two independent alloy lock rings (inner and outer) eliminate grip rotation during aggressive riding.',
 'Rubber / Dual Alloy Lock Rings','Black / Orange',NULL,NULL,NULL,NULL,NULL,NULL,NULL,
 'Diameter: 33mm; Length: 130mm; Lock rings: 2x Alloy; Sold as pair',
 370.00,25),

(91,9,10,'GRIP-WPN-001','Weapon Race w/ Palm Rest Lock-On Grip',
 'Ergonomic palm-rest lock-on grips — reduces ulnar nerve pressure.',
 'Lateral palm rest wing reduces hand pressure during long trail rides. Dual lock-on alloy rings.',
 'Rubber / Alloy Lock Rings','Black / Red',NULL,NULL,NULL,NULL,NULL,NULL,NULL,
 'Diameter: 34mm grip / 38mm palm rest; Length: 140mm; Lock rings: 2x Alloy; Sold as pair',
 400.00,20),

(92,9,10,'GRIP-WPN-002','Weapon Rubix Slip-On Grip',
 'Slip-on rubber grip for BMX and budget trail builds.',
 'Simple slip-on rubber grip. No lock rings — installed with grip glue or hairspray. Suits BMX and budget MTBs.',
 'Rubber Compound','Black',NULL,NULL,NULL,NULL,NULL,NULL,NULL,
 'Diameter: 30mm; Length: 135mm; Type: Slip-on; Sold as pair',
 200.00,40),

(93,9,10,'GRIP-WPN-003','Weapon Wave Lock-On Grip',
 'Wave-pattern textured lock-on grip — secure in wet/muddy conditions.',
 'Wave-pattern texture for additional grip in wet or muddy conditions. Single alloy lock ring per side.',
 'Rubber / Alloy Lock Ring','Black / Blue',NULL,NULL,NULL,NULL,NULL,NULL,NULL,
 'Diameter: 33mm; Length: 130mm; Lock ring: 1x Alloy; Pattern: Wave; Sold as pair',
 350.00,22);
GO

-- ---- Category 10: Pedals ----
INSERT INTO Product
    (ProductId,CategoryId,BrandId,SKU,[Name],ShortDescription,[Description],
     Material,Color,WheelSize,SpeedCompatibility,BoostCompatible,TubelessReady,
     AxleStandard,SuspensionTravel,BrakeType,AdditionalSpecs,Price,StockQuantity)
VALUES
(94,10,14,'PEDL-SPD-001','Speedone Soldier Platform Pedal',
 'Wide alloy platform pedal with replaceable pins — trail use.',
 'Wide, grippy platform with replaceable steel pins and sealed bearings. Alloy body keeps weight low for trail riding.',
 'CNC Aluminum Alloy 6061','Black',NULL,NULL,NULL,NULL,NULL,NULL,NULL,
 'Platform: 100x110mm; Pins: Steel replaceable 10/side; Spindle: Cr-Mo 9/16"; Bearing: Sealed; Weight: ~390g/pair',
 1200.00,20),

(95,10,14,'PEDL-SPD-002','Speedone Pilot Alloy Platform Pedal',
 'Slim low-profile alloy pedal — confidence for technical trail.',
 'Thinner profile than the Soldier reduces foot-to-ground clearance for more confident manuals and technical trail riding.',
 'CNC Aluminum Alloy 6061','Black / Silver',NULL,NULL,NULL,NULL,NULL,NULL,NULL,
 'Platform: 102x108mm; Thickness: 15mm; Spindle: Cr-Mo 9/16"; Bearing: Sealed; Weight: ~370g/pair',
 1200.00,18),

(96,10,12,'PEDL-SAG-001','Pedal Sagmit 610 Alloy Platform',
 'Budget alloy trail pedal with 8 pins per side.',
 'Value-priced alloy platform with multiple body pins for confident traction on trails and commutes.',
 'Aluminum Alloy','Black',NULL,NULL,NULL,NULL,NULL,NULL,NULL,
 'Platform: 100x110mm; Spindle: Steel 9/16"; Pins: 8/side; Bearing: Sealed; Weight: ~420g/pair',
 800.00,25),

(97,10,12,'PEDL-SAG-002','Pedal Sagmit 614 Alloy Platform',
 'Upgraded alloy pedal — 12 pins per side, wider platform.',
 'More pins than the 610 and a slightly wider platform for improved shoe contact and traction.',
 'Aluminum Alloy','Black / Red',NULL,NULL,NULL,NULL,NULL,NULL,NULL,
 'Platform: 105x112mm; Spindle: Steel 9/16"; Pins: 12/side; Bearing: Sealed; Weight: ~440g/pair',
 800.00,20),

(98,10,23,'PEDL-RAG-001','Ragusa R700 Composite Platform Pedal',
 'Lightweight composite body trail pedal — entry-level value.',
 'Fiber-reinforced nylon body for reduced weight over alloy options. Entry-level trail and commuter pedal.',
 'Fiber-Reinforced Nylon / Steel Spindle','Black',NULL,NULL,NULL,NULL,NULL,NULL,NULL,
 'Platform: 98x110mm; Spindle: Steel 9/16"; Bearing: Bushing; Weight: ~310g/pair',
 500.00,30),

(99,10,23,'PEDL-RAG-002','Ragusa CNC Sealed Bearing 712 Platform Pedal',
 'CNC alloy pedal with sealed bearings and replaceable pins.',
 'CNC-machined alloy body with sealed cartridge bearings and replaceable pins for a longer service life.',
 'CNC Aluminum Alloy 6061','Black',NULL,NULL,NULL,NULL,NULL,NULL,NULL,
 'Platform: 102x112mm; Spindle: Cr-Mo 9/16"; Pins: Replaceable steel; Bearing: Sealed; Weight: ~380g/pair',
 800.00,20),

(100,10,26,'PEDL-MKS-001','MKS JAPAN GR-9 Alloy Platform Pedal',
 'Japanese MKS alloy road platform pedal — smooth sealed bearings.',
 'Made in Japan. Smooth sealed-bearing performance and clean aesthetic for touring cyclists and commuters worldwide.',
 'Aluminum Alloy / Cr-Mo Spindle','Silver',NULL,NULL,NULL,NULL,NULL,NULL,NULL,
 'Platform: 93x90mm; Spindle: Cr-Mo 9/16"; Bearing: Sealed ball; Weight: ~345g/pair; Made in Japan',
 1200.00,12),

(101,10,26,'PEDL-MKS-002','MKS Sylvan Touring Platform Pedal',
 'Classic MKS dual-sided touring pedal — lightweight and easy to mount.',
 'Made in Japan. Double-sided platform for easy foot placement in urban and touring use.',
 'Aluminum Alloy / Cr-Mo Spindle','Silver',NULL,NULL,NULL,NULL,NULL,NULL,NULL,
 'Platform: Dual-sided 92x90mm; Spindle: Cr-Mo 9/16"; Bearing: Sealed; Weight: ~330g/pair; Made in Japan',
 1400.00,10),

(102,10,26,'PEDL-MKS-003','MKS BM7 BMX / Freestyle Platform Pedal',
 'Wide MKS alloy BMX pedal — durable and lightweight.',
 'Made in Japan. Wide-body BMX platform pedal with aggressive grip for freestyle, dirt jump, and BMX riding.',
 'Aluminum Alloy / Steel Spindle','Black',NULL,NULL,NULL,NULL,NULL,NULL,NULL,
 'Platform: 115x108mm; Spindle: Steel 9/16"; Pins: Fixed; Bearing: Ball; Weight: ~420g/pair; Made in Japan',
 1550.00,8);
GO

-- ---- Category 11: Rims & Wheelsets ----
INSERT INTO Product
    (ProductId,CategoryId,BrandId,SKU,[Name],ShortDescription,[Description],
     Material,Color,WheelSize,SpeedCompatibility,BoostCompatible,TubelessReady,
     AxleStandard,SuspensionTravel,BrakeType,AdditionalSpecs,Price,StockQuantity)
VALUES
(103,11,17,'RIM-LDC-001','LDCNC RS300 700c Rim Brake Wheelset',
 'Complete CNC 700c rim-brake wheelset for road and gravel.',
 'CNC-machined brake tracks for consistent stopping. Double-wall alloy rim and sealed bearing hubs for durable road/gravel use.',
 'CNC Aluminum Alloy Double-Wall','Silver / Black','700c','Universal',0,0,
 'QR 9x100mm (F) / 9x130mm (R)',NULL,'Rim Brake',
 'ERD: 584mm; Holes: 32H; Rim depth: 25mm; Width: 22mm; Sold as complete wheelset',
 5100.00,5),

(104,11,12,'RIM-SAG-001','Sagmit Aero Double Wall Rim 27.5"',
 '27.5" double-wall aero alloy rim — single rim.',
 '27.5" double-wall aero alloy rim with CNC brake track. Suitable for disc and rim-brake builds.',
 'Aluminum Alloy 6061 Double-Wall','Black','27.5"','Universal',NULL,0,
 NULL,NULL,NULL,
 'ERD: 559mm; Holes: 32H; Depth: 28mm; Width: 25mm; Matte black; Single rim',
 1500.00,20),

(105,11,12,'RIM-SAG-002','Sagmit Legend M32 Double Wall Rim 29"',
 '29" double-wall alloy trail and XC rim — single rim.',
 'Reliable disc-brake-compatible 29" rim. Usable in tubeless setup with rim tape and sealant.',
 'Aluminum Alloy Double-Wall','Black','29"','Universal',NULL,0,
 NULL,NULL,NULL,
 'ERD: 584mm; Holes: 32H; Width: 25mm; Depth: 25mm; Single rim',
 1600.00,18),

(106,11,30,'RIM-KOR-001','Kore Realm 4.2 Plus Rim 27.5"',
 '27.5+ wide alloy rim — 42mm internal width for plus tires.',
 'Wide 42mm internal rim for 27.5+ plus tires. Lower pressure for improved traction on loose terrain. Tubeless-ready.',
 'Aluminum Alloy Double-Wall','Black','27.5+"','Universal',NULL,1,
 NULL,NULL,NULL,
 'ERD: 559mm; Internal width: 42mm; Holes: 32H; Tubeless-ready; Depth: 25mm; Single rim',
 1100.00,12),

(107,11,27,'RIM-WTB-001','WTB i21 Tubeless Ready 27.5" Rim',
 'WTB i21 TCS tubeless-ready XC/trail rim — 21mm internal width.',
 'TCS tubeless-compatible 21mm internal-width XC trail rim. Lightweight and stiff for fast XC and light trail setups.',
 'Aluminum Alloy','Black','27.5"','Universal',NULL,1,
 NULL,NULL,NULL,
 'ERD: 559mm; Internal width: 21mm; Holes: 28H / 32H; TCS tubeless-ready; Single rim',
 1600.00,15),

(108,11,27,'RIM-WTB-002','WTB i25 Tubeless Ready 29" Rim',
 'WTB i25 TCS tubeless-ready trail rim — 25mm internal width.',
 '25mm internal width for trail-to-enduro tire widths 2.2"–2.6". Tubeless-compatible hooked rim profile.',
 'Aluminum Alloy','Black','29"','Universal',NULL,1,
 NULL,NULL,NULL,
 'ERD: 584mm; Internal width: 25mm; Holes: 32H; TCS tubeless-ready; Single rim',
 1600.00,14),

(109,11,28,'RIM-AMC-001','American Classic Feldspar 290 Tubeless Ready 29"',
 'Lightweight American Classic 29" TLR XC rim — 23mm internal.',
 'Feldspar technology reinforced bead hook for reliable tubeless seating. Lightweight XC race and marathon rim.',
 'Aluminum Alloy','Black / Silver','29"','Universal',NULL,1,
 NULL,NULL,NULL,
 'ERD: 584mm; Internal width: 23mm; Holes: 24H; Tubeless-ready; Weight: ~400g; Single rim',
 1800.00,8),

(110,11,29,'RIM-JAL-001','Jalco Wellington Tubeless Ready 27.5" Rim',
 'CNC-sidewall tubeless-ready 27.5" alloy rim — single rim.',
 'Tubeless-compatible 27.5" double-wall rim with CNC-machined sidewalls for precise disc brake performance.',
 'Aluminum Alloy Double-Wall CNC','Black','27.5"','Universal',NULL,1,
 NULL,NULL,NULL,
 'ERD: 559mm; Internal width: 25mm; Holes: 32H; CNC sidewall; Tubeless-ready; Single rim',
 1400.00,12),

(111,11,14,'RIM-SPD-001','Speedone Pilot Double Wall Rim 700c',
 '700c double-wall alloy rim for road and gravel builds.',
 'Double-wall alloy rim for road, gravel, and CX builds. Pairs with Speedone hubs for a complete custom build.',
 'Aluminum Alloy Double-Wall','Black','700c','Universal',NULL,0,
 NULL,NULL,NULL,
 'ERD: 622mm; Holes: 32H; Width: 22mm; Finish: Matte black; Single rim',
 2000.00,10),

(112,11,14,'RIM-SPD-002','Speedone Bazooka Wide Rim 27.5"',
 '30mm internal wide rim for aggressive trail and enduro.',
 'Wide 30mm internal rim supports aggressive trail tires up to 2.6". Tubeless-ready double-wall alloy.',
 'Aluminum Alloy Double-Wall','Black','27.5"','Universal',NULL,1,
 NULL,NULL,NULL,
 'ERD: 559mm; Internal width: 30mm; Holes: 32H; Tubeless-ready; Single rim',
 1700.00,10),

(113,11,14,'RIM-SPD-003','Speedone Soldier Double Wall Rim 29"',
 '29" double-wall alloy trail rim — durable and affordable.',
 'Sturdy 29" double-wall rim for trail and light enduro. Disc brake compatible and tubeless-convertible.',
 'Aluminum Alloy Double-Wall','Black','29"','Universal',NULL,0,
 NULL,NULL,NULL,
 'ERD: 584mm; Internal width: 25mm; Holes: 32H; Single rim',
 1900.00,8),

(114,11,12,'RIM-SAG-003','Sagmit Safarri Wide Alloy Rim 27.5"',
 '27.5" wide 28mm internal rim for tires up to 2.6".',
 'Wide-profile trail rim for tires up to 2.6". Designed for tubeless setup with stable low-pressure bead.',
 'Aluminum Alloy Double-Wall','Black','27.5"','Universal',NULL,1,
 NULL,NULL,NULL,
 'ERD: 559mm; Internal width: 28mm; Holes: 32H; Single rim',
 1700.00,12),

(115,11,29,'RIM-JAL-002','Jalco Maranello Double Wall Rim 700c',
 '700c double-wall alloy road rim with CNC sidewall.',
 'Dependable double-wall road rim with CNC brake track. Popular OEM-spec choice for road and commuter builds.',
 'Aluminum Alloy Double-Wall CNC','Silver','700c','Universal',NULL,0,
 NULL,NULL,NULL,
 'ERD: 622mm; Holes: 32H; Width: 22mm; CNC sidewall; Single rim',
 1500.00,14),

(116,11,30,'RIM-KOR-002','Kore Realm 2.4 Alloy Rim 27.5"',
 '24mm internal trail rim — balances weight and tire support.',
 '24mm internal-width rim for tires 2.0"–2.4" and disc brake drivetrains.',
 'Aluminum Alloy Double-Wall','Black','27.5"','Universal',NULL,0,
 NULL,NULL,NULL,
 'ERD: 559mm; Internal width: 24mm; Holes: 32H; Single rim',
 1200.00,15);
GO

-- ---- Category 12: Tires & Tubes ----
INSERT INTO Product
    (ProductId,CategoryId,BrandId,SKU,[Name],ShortDescription,[Description],
     Material,Color,WheelSize,SpeedCompatibility,BoostCompatible,TubelessReady,
     AxleStandard,SuspensionTravel,BrakeType,AdditionalSpecs,Price,StockQuantity)
VALUES
(117,12,31,'TIRE-MAX-001','Maxxis Ikon 27.5 x 2.20 Tanwall',
 'Maxxis Ikon XC/trail tire — 27.5" 2.20" tan sidewall.',
 'Versatile XC-to-trail tire: fast rolling with reliable cornering grip. Tan sidewall (TanWall) edition for a classic aesthetic and lighter weight.',
 'Dual Compound EXO / Tanwall Casing','Black / Tan','27.5"','Universal',NULL,1,
 NULL,NULL,NULL,
 'Size: 27.5x2.20; Compound: Dual EXO; TPI: 60; Tubeless: EXO-ready; Bead: Folding; Weight: ~670g',
 1500.00,20),

(118,12,31,'TIRE-MAX-002','Maxxis Ardent 29 x 2.25 Tanwall',
 'Maxxis Ardent trail tire — 29" 2.25" tan sidewall.',
 'Fast rolling on hard-pack with reliable mid-corner grip on loose terrain. 2.25 width suits trail and XC riders.',
 'Dual Compound EXO / Tanwall Casing','Black / Tan','29"','Universal',NULL,1,
 NULL,NULL,NULL,
 'Size: 29x2.25; TPI: 60; Compound: Dual EXO; Tubeless-ready; Folding; Weight: ~740g',
 1500.00,18),

(119,12,31,'TIRE-MAX-003','Maxxis Ikon 29 x 2.20',
 'Maxxis Ikon XC tire — 29" 2.20" black sidewall.',
 'Lightweight XC race tire with fast rolling and adequate cornering grip for hardpack and mixed terrain. More casing protection than TanWall.',
 'Dual Compound EXO / Black Casing','Black','29"','Universal',NULL,1,
 NULL,NULL,NULL,
 'Size: 29x2.20; TPI: 60; Compound: Dual EXO; Tubeless-ready; Weight: ~680g',
 1500.00,16),

(120,12,31,'TIRE-MAX-004','Maxxis Crossmark II 27.5 x 2.25',
 'Maxxis Crossmark II trail tire — 27.5" 2.25" fast rolling.',
 'Updated tread over the original Crossmark. Wide corner knobs for confident lean-angle grip on loose, dry trails.',
 'Dual Compound EXO Casing','Black','27.5"','Universal',NULL,1,
 NULL,NULL,NULL,
 'Size: 27.5x2.25; TPI: 60; EXO; Tubeless-ready; Weight: ~700g',
 1500.00,18),

(121,12,31,'TIRE-MAX-005','Maxxis Ikon 29 x 2.20 Tanwall',
 'Maxxis Ikon XC tire — 29" 2.20" tan sidewall.',
 'Fast rolling Ikon tread with classic tan sidewall. Popular for gravel and XC riders wanting style alongside performance.',
 'Dual Compound EXO / Tanwall Casing','Black / Tan','29"','Universal',NULL,1,
 NULL,NULL,NULL,
 'Size: 29x2.20; TPI: 60; EXO; Tubeless-ready; Folding; Weight: ~660g',
 1500.00,15),

(122,12,31,'TIRE-MAX-006','Maxxis Rekon Race 27.5 x 2.25',
 'Maxxis Rekon Race XC-race tire — 27.5" 2.25" lightweight.',
 'High-performance XC race tire. Low-profile center knobs for minimal rolling resistance; shoulder knobs for confident cornering at speed.',
 'MaxxSpeed Dual Compound / EXO+ Casing','Black','27.5"','Universal',NULL,1,
 NULL,NULL,NULL,
 'Size: 27.5x2.25; TPI: 120; Compound: MaxxSpeed; EXO+; Tubeless-ready; Weight: ~625g',
 1500.00,12),

(123,12,31,'TIRE-MAX-007','Maxxis Ardent 27.5 x 2.25',
 'Maxxis Ardent trail tire — 27.5" 2.25" all-rounder.',
 'Go-to trail tire for mixed conditions. Fast enough for XC-influenced rides, grippy enough for loose descents.',
 'Dual Compound EXO Casing','Black','27.5"','Universal',NULL,1,
 NULL,NULL,NULL,
 'Size: 27.5x2.25; TPI: 60; Compound: Dual EXO; Tubeless-ready; Weight: ~720g',
 1500.00,18),

(124,12,31,'TIRE-MAX-008','Maxxis Ardent Race 27.5 x 2.20',
 'Maxxis Ardent Race XC-trail tire — 27.5" 2.20" lightweight.',
 'Bridges XC and trail with a faster-rolling profile and lightweight EXO+ casing. Minimal weight penalty for trail capability.',
 'MaxxSpeed Dual Compound / EXO+ Casing','Black','27.5"','Universal',NULL,1,
 NULL,NULL,NULL,
 'Size: 27.5x2.20; TPI: 120; MaxxSpeed; EXO+; Tubeless-ready; Weight: ~640g',
 1500.00,14),

(125,12,31,'TUBE-MAX-001','MAXXIS TUBE 27.5/29 Presta',
 'Maxxis premium MTB inner tube — 27.5" and 29", Presta valve.',
 'Premium butyl rubber tube for reliable performance and puncture resistance. Compatible with 27.5" and 29" tires.',
 'Butyl Rubber','Black','27.5" / 29"','Universal',NULL,NULL,
 NULL,NULL,NULL,
 'Valve: Presta 48mm; Size: 26/27.5/29 x 1.9–2.35; Valve: Removable core',
 250.00,40),

(126,12,12,'TUBE-SAG-001','SAGMIT TUBE 27.5/29 Schrader',
 'Sagmit MTB inner tube — Schrader valve.',
 'Value Schrader-valve tube for standard MTB rims. Compatible with most 27.5" and 29" rims.',
 'Butyl Rubber','Black','27.5" / 29"','Universal',NULL,NULL,
 NULL,NULL,NULL,
 'Valve: Schrader 33mm; Size: 27.5/29 x 1.9–2.35; Material: Butyl',
 150.00,50),

(127,12,32,'TUBE-LEO-001','SIZE 20 LEO TUBE',
 'Leo inner tube for 20" BMX and kids bikes.',
 'Standard butyl Schrader-valve tube for 20" BMX and children''s bikes.',
 'Butyl Rubber','Black','20"','Universal',NULL,NULL,
 NULL,NULL,NULL,
 'Valve: Schrader; Size: 20 x 1.75–2.125; Material: Butyl',
 85.00,60),

(128,12,32,'TUBE-LEO-002','SIZE 26 LEO TUBE',
 'Leo inner tube for 26" MTB and commuter bikes.',
 'Compatible with most 26" mountain, city, and hybrid bikes. Schrader valve for easy inflation at any service station.',
 'Butyl Rubber','Black','26"','Universal',NULL,NULL,
 NULL,NULL,NULL,
 'Valve: Schrader; Size: 26 x 1.75–2.125; Material: Butyl',
 100.00,50),

(129,12,32,'TIRE-LEO-001','SIZE 20 TIRE',
 'Leo 20" BMX tire — street, park, and kids bikes.',
 'Durable knobbed tread for 20" BMX and children''s bikes. Mixed surface traction.',
 'Rubber Compound','Black','20"','Universal',NULL,NULL,
 NULL,NULL,NULL,
 'Size: 20 x 2.125; TPI: 30; Bead: Wire; Tread: Knobbed; Weight: ~500g',
 380.00,30),

(130,12,32,'TIRE-LEO-002','SIZE 16 TIRE',
 'Leo 16" tire for children''s bikes and small-wheel folders.',
 'Knobbed tread for 16" children''s bicycles and small-wheel city bikes. Traction on paved roads and light gravel.',
 'Rubber Compound','Black','16"','Universal',NULL,NULL,
 NULL,NULL,NULL,
 'Size: 16 x 1.75; TPI: 30; Bead: Wire; Weight: ~370g',
 280.00,35),

(131,12,32,'TUBE-LEO-003','SIZE 16 TUBE',
 'Leo inner tube for 16" children''s bikes.',
 'Standard butyl Schrader-valve tube for 16" children''s and folding bikes.',
 'Butyl Rubber','Black','16"','Universal',NULL,NULL,
 NULL,NULL,NULL,
 'Valve: Schrader; Size: 16 x 1.75–2.125; Material: Butyl',
 85.00,60);
GO

-- ---- Category 13: Chains ----
INSERT INTO Product
    (ProductId,CategoryId,BrandId,SKU,[Name],ShortDescription,[Description],
     Material,Color,WheelSize,SpeedCompatibility,BoostCompatible,TubelessReady,
     AxleStandard,SuspensionTravel,BrakeType,AdditionalSpecs,Price,StockQuantity)
VALUES
(132,13,33,'CHAIN-SUM-001','8 SPEED SUMC CP With Missing link',
 '116-link 8-speed chrome-plated chain with quick-link.',
 'Chrome-plated finish for corrosion resistance and smooth shifting. Includes quick-release missing link.',
 'Steel / Chrome Plated','Silver',NULL,'8-speed',NULL,NULL,NULL,NULL,NULL,
 'Links: 116; Speed: 8s; Inner width: 2.38mm; Pin: Solid; Includes missing link',
 300.00,30),

(133,13,33,'CHAIN-SUM-002','9 SPEED SUMC CP With Missing link',
 '116-link 9-speed chrome-plated chain with quick-link.',
 'Reliable shifting across 9-speed drivetrains with chrome-plated durability. Includes missing link.',
 'Steel / Chrome Plated','Silver',NULL,'9-speed',NULL,NULL,NULL,NULL,NULL,
 'Links: 116; Speed: 9s; Inner width: 2.18mm; Includes missing link',
 380.00,28),

(134,13,33,'CHAIN-SUM-003','10 SPEED SUMC CP With Missing link',
 '116-link 10-speed chrome-plated chain with quick-link.',
 'Narrowed for 10-speed drivetrains. Chrome-plated for smooth, low-friction shifting.',
 'Steel / Chrome Plated','Silver',NULL,'10-speed',NULL,NULL,NULL,NULL,NULL,
 'Links: 116; Speed: 10s; Inner width: 1.96mm; Includes missing link',
 450.00,25),

(135,13,33,'CHAIN-SUM-004','11 SPEED SUMC CP With Missing link',
 '116-link 11-speed chrome-plated chain with quick-link.',
 'Compatible with Shimano HG, SRAM, and Campagnolo 11s systems. Hollow inner plates reduce weight.',
 'Steel / Chrome Plated / Hollow Plates','Silver',NULL,'11-speed',NULL,NULL,NULL,NULL,NULL,
 'Links: 116; Speed: 11s; Hollow outer plates; Includes missing link',
 650.00,20),

(136,13,33,'CHAIN-SUM-005','12 SPEED SUMC CP With Missing link',
 '116-link 12-speed chrome-plated chain with quick-link.',
 'Compatible with Shimano 12s (Micro Spline) and SRAM 12s (XD / HG). Hollow outer plates for weight savings.',
 'Steel / Chrome Plated / Hollow Plates','Silver',NULL,'12-speed',NULL,NULL,NULL,NULL,NULL,
 'Links: 116; Speed: 12s; Hollow outer plates; Includes missing link',
 1050.00,18),

(137,13,33,'CHAIN-SUM-006','8 SPEED SUMC Oilslick 116Links With Missing link',
 '116-link 8-speed titanium-nitride oilslick iridescent chain.',
 'Striking iridescent oilslick finish via titanium nitride coating. Added corrosion resistance with a visually unique aesthetic.',
 'Steel / Titanium Nitride Coated','Oilslick Iridescent',NULL,'8-speed',NULL,NULL,NULL,NULL,NULL,
 'Links: 116; Speed: 8s; Finish: TiN Oilslick; Inner width: 2.38mm; Includes missing link',
 450.00,20),

(138,13,33,'CHAIN-SUM-007','10 SPEED SUMC Oilslick 116Links With Missing link',
 '116-link 10-speed titanium-nitride oilslick chain.',
 'TiN oilslick coating for corrosion resistance and the iconic iridescent look. Includes matching oilslick missing link.',
 'Steel / Titanium Nitride Coated','Oilslick Iridescent',NULL,'10-speed',NULL,NULL,NULL,NULL,NULL,
 'Links: 116; Speed: 10s; Finish: TiN Oilslick; Includes missing link',
 750.00,18),

(139,13,33,'CHAIN-SUM-008','11 SPEED SUMC Oilslick 116Links With Missing link',
 '116-link 11-speed titanium-nitride oilslick chain.',
 'Compatible with all major 11s groupsets. Eye-catching TiN oilslick finish. Includes oilslick missing link.',
 'Steel / Titanium Nitride Coated','Oilslick Iridescent',NULL,'11-speed',NULL,NULL,NULL,NULL,NULL,
 'Links: 116; Speed: 11s; Finish: TiN Oilslick; Hollow plates; Includes missing link',
 1200.00,15),

(140,13,34,'CHAIN-KMC-001','12SPD KMC Chain X Series GREY',
 'KMC X12 12-speed chain — grey nickel-plated, 116 links.',
 'KMC X-12 compatible with Shimano 12s and SRAM Eagle 12s. Grey nickel plating for solid corrosion protection.',
 'High Tensile Steel / Grey Nickel Plated','Grey',NULL,'12-speed',NULL,NULL,NULL,NULL,NULL,
 'Links: 116; Speed: 12s; Width: 5.2mm; Finish: Grey Nickel; Includes missing link CL562R; Weight: ~260g',
 1500.00,15),

(141,13,34,'CHAIN-KMC-002','11SPD KMC Chain X Series GREY',
 'KMC X11 11-speed chain — grey nickel-plated, 116 links.',
 'Smooth-shifting 11s chain compatible with Shimano, SRAM, and Campagnolo. Nickel-plated for corrosion resistance.',
 'High Tensile Steel / Grey Nickel Plated','Grey',NULL,'11-speed',NULL,NULL,NULL,NULL,NULL,
 'Links: 116; Speed: 11s; Width: 5.5mm; Finish: Grey Nickel; Includes missing link CL551R; Weight: ~252g',
 1100.00,18),

(142,13,34,'CHAIN-KMC-003','10SPD KMC Chain X Series GREY',
 'KMC X10 10-speed chain — grey nickel-plated, 116 links.',
 'Trusted 10s chain for MTB and road groupsets. Nickel-plated outer plates for reliable shifting performance.',
 'High Tensile Steel / Grey Nickel Plated','Grey',NULL,'10-speed',NULL,NULL,NULL,NULL,NULL,
 'Links: 116; Speed: 10s; Width: 5.9mm; Finish: Grey Nickel; Includes missing link CL547R; Weight: ~246g',
 900.00,20),

(143,13,34,'CHAIN-KMC-004','9SPD KMC Chain X Series GOLD',
 'KMC X9 9-speed chain — premium gold zinc-plated, 116 links.',
 'Gold zinc-plated 9s chain for a distinctive build. Suitable for all 9-speed drivetrains from Shimano, SRAM, and Campagnolo.',
 'High Tensile Steel / Gold Zinc Plated','Gold',NULL,'9-speed',NULL,NULL,NULL,NULL,NULL,
 'Links: 116; Speed: 9s; Width: 6.6mm; Finish: Gold Zinc; Includes missing link CL531G; Weight: ~283g',
 1800.00,15),

(144,13,34,'CHAIN-KMC-005','10SPD KMC Chain X Series GOLD',
 'KMC X10 10-speed chain — gold zinc-plated, 116 links.',
 'Same reliable 10s shifting as the Grey version, with the premium aesthetic of a gold zinc-plated finish.',
 'High Tensile Steel / Gold Zinc Plated','Gold',NULL,'10-speed',NULL,NULL,NULL,NULL,NULL,
 'Links: 116; Speed: 10s; Width: 5.9mm; Finish: Gold Zinc; Includes missing link; Weight: ~248g',
 1800.00,12),

(145,13,34,'CHAIN-KMC-006','12SPD KMC Chain X Series GOLD',
 'KMC X12 12-speed chain — premium gold zinc-plated, 116 links.',
 'Gold finish on KMC''s flagship 12s chain. Compatible with Shimano, SRAM Eagle, and Campagnolo 12-speed systems.',
 'High Tensile Steel / Gold Zinc Plated','Gold',NULL,'12-speed',NULL,NULL,NULL,NULL,NULL,
 'Links: 116; Speed: 12s; Width: 5.2mm; Finish: Gold Zinc; Includes missing link; Weight: ~262g',
 2400.00,10),

(146,13,35,'CHAIN-CTX-001','CT-1HX 116links Heavy Duty for FIXIE',
 'Heavy-duty 1-speed chain for fixie and single-speed bikes.',
 'Reinforced 1/8" single-speed chain for fixie, fixed-gear, and single-speed drivetrains. Wider inner plate for high-torque stability.',
 'High Carbon Steel / Nickel Plated','Silver',NULL,'1-speed / Fixie',NULL,NULL,NULL,NULL,NULL,
 'Links: 116; Width: 1/8"; Pitch: 1/2"; Finish: Nickel; For: Fixie / SS / BMX',
 350.00,30),

(147,13,36,'CHAIN-GTX-001','GT-10 126links',
 '126-link standard 10-speed chain — extra links for custom builds.',
 'Standard 10s chain with 126 links for builds requiring extra link count. Nickel-plated for corrosion resistance.',
 'High Tensile Steel / Nickel Plated','Silver',NULL,'10-speed',NULL,NULL,NULL,NULL,NULL,
 'Links: 126; Speed: 10s; Inner width: 1.96mm; Finish: Nickel; No missing link included',
 500.00,22),

(148,13,36,'CHAIN-GTX-002','GT-11 126links',
 '126-link standard 11-speed chain — extra links for custom builds.',
 'Standard 11s chain with 126 links. Compatible with Shimano and SRAM 11-speed groupsets.',
 'High Tensile Steel / Nickel Plated','Silver',NULL,'11-speed',NULL,NULL,NULL,NULL,NULL,
 'Links: 126; Speed: 11s; Inner width: 1.78mm; Finish: Nickel; No missing link included',
 700.00,18);
GO

SET IDENTITY_INSERT Product OFF;
GO


-- =============================================================================
-- SECTION 5: Test Data — 2 rows per remaining table
-- Dependencies resolved in FK order:
--   User → UserRole → Supplier → PurchaseOrder → PurchaseOrderItem
--   Voucher → UserVoucher → Cart → CartItem → Wishlist
--   Order → OrderItem → VoucherUsage → InventoryLog
--   Payment → Delivery → Review → POS_Session → SystemLog
-- =============================================================================

-- =============================================
-- Users  (1 admin staff, 1 customer)
-- =============================================
INSERT INTO [User]
    (Email, PasswordHash, FirstName, LastName, PhoneNumber, [Address], City, PostalCode, IsActive, IsWalkIn)
VALUES
('admin@taurusbikes.ph',
 '$2b$12$sampleHashForAdminAccountOnlyX',
 'Marco', 'Reyes',
 '09171234567',
 '123 Kalayaan Ave, Diliman',
 'Quezon City', '1101',
 1, 0),

('juan.dela.cruz@gmail.com',
 '$2b$12$sampleHashForCustomerAccountX',
 'Juan', 'Dela Cruz',
 '09281234567',
 '456 Rizal St, Poblacion',
 'Makati', '1210',
 1, 0);
GO

-- =============================================
-- UserRoles  (UserId 1 = Admin, UserId 2 = Customer)
-- =============================================
INSERT INTO UserRole (UserId, RoleId)
VALUES
(1, 1),   -- Marco Reyes  → Admin   (RoleId 1)
(2, 5);   -- Juan De Cruz → Customer (RoleId 5)
GO

-- =============================================
-- ProductVariants  (size variants for complete bikes)
-- =============================================
INSERT INTO ProductVariant (ProductId, VariantName, SKU, AdditionalPrice, StockQuantity)
VALUES
(1, 'Medium (17")',  'UNIT-PIN-001-M',  0.00, 3),
(1, 'Large (19")',   'UNIT-PIN-001-L',  500.00, 2);
GO

-- =============================================
-- ProductImages
-- =============================================
INSERT INTO ProductImage (ProductId, ImageUrl, ImageType, IsPrimary, DisplayOrder, AltText)
VALUES
(1,
 'https://cdn.taurusbikes.ph/products/unit-pin-001-full.jpg',
 'Full', 1, 1,
 '2021 Pinewood Climber CARBON 27.5 – Full Image'),

(1,
 'https://cdn.taurusbikes.ph/products/unit-pin-001-thumb.jpg',
 'Thumbnail', 0, 2,
 '2021 Pinewood Climber CARBON 27.5 – Thumbnail');
GO

-- =============================================
-- Suppliers
-- =============================================
INSERT INTO Supplier ([Name], ContactPerson, PhoneNumber, Email, [Address])
VALUES
('Pinewood Bicycles PH',
 'Ana Santos',
 '02-8123-4567',
 'supply@pinewoodbike.ph',
 'Unit 5B, Industrial Zone, Valenzuela City, Metro Manila'),

('Sagmit Components',
 'Ben Ocampo',
 '09171112222',
 'orders@sagmit.ph',
 '789 Banawe St, Quezon City, Metro Manila');
GO

-- =============================================
-- PurchaseOrders  (CreatedByUserId = 1, Admin)
-- =============================================
INSERT INTO PurchaseOrder (SupplierId, ExpectedDeliveryDate, [Status], TotalAmount, CreatedByUserId)
VALUES
(1, DATEADD(DAY, 7, GETDATE()), 'Pending',  200000.00, 1),
(2, DATEADD(DAY,14, GETDATE()), 'Pending',   52000.00, 1);
GO

-- =============================================
-- PurchaseOrderItems
-- =============================================
INSERT INTO PurchaseOrderItem (PurchaseOrderId, ProductId, ProductVariantId, Quantity, UnitPrice, Subtotal)
VALUES
(1, 1, NULL, 5, 40000.00, 200000.00),   -- 5x Pinewood Climber Carbon 27.5
(2, 44, NULL, 10,  5200.00,  52000.00); -- 10x Sagmit Edison 12-Speed Kit
GO

-- =============================================
-- Vouchers
-- =============================================
INSERT INTO Voucher
    (Code, [Description], DiscountType, DiscountValue, MinimumOrderAmount,
     MaxUses, MaxUsesPerUser, StartDate, EndDate, IsActive)
VALUES
('TAURUS10',
 '10% off on orders above PHP 5,000',
 'Percentage', 10.00, 5000.00, 200, 1,
 GETDATE(), DATEADD(MONTH, 3, GETDATE()), 1),

('FREESHIP150',
 'PHP 150 shipping discount on any order',
 'Fixed', 150.00, NULL, 500, 2,
 GETDATE(), DATEADD(MONTH, 6, GETDATE()), 1);
GO

-- =============================================
-- UserVouchers  (assign both vouchers to customer UserId 2)
-- =============================================
INSERT INTO UserVoucher (UserId, VoucherId, ExpiresAt)
VALUES
(2, 1, DATEADD(MONTH, 3, GETDATE())),
(2, 2, DATEADD(MONTH, 6, GETDATE()));
GO

-- =============================================
-- Carts  (active cart for customer UserId 2)
-- =============================================
INSERT INTO Cart (UserId, IsCheckedOut)
VALUES
(2, 0),   -- active cart
(2, 1);   -- a previously checked-out cart (historical)
GO
-- NOTE: UX_Cart_User_Active allows only one IsCheckedOut=0 per user,
--       so the second row (IsCheckedOut=1) is permitted.

-- =============================================
-- CartItems  (items in the active cart, CartId 1)
-- =============================================
INSERT INTO CartItem (CartId, ProductId, ProductVariantId, Quantity, PriceAtAdd)
VALUES
(1, 1,   1, 1, 40000.00),  -- Pinewood Climber, Medium variant
(1, 117, NULL, 2,  1500.00);  -- 2x Maxxis Ikon 27.5 Tanwall
GO

-- =============================================
-- Wishlist  (customer saved items)
-- =============================================
INSERT INTO Wishlist (UserId, ProductId)
VALUES
(2, 28),   -- Manitou Markhor BOOST Air Fork
(2, 140);  -- 12SPD KMC Chain X Series GREY
GO

-- =============================================
-- Orders
--   Order 1: Online, Makati (Metro Manila)  → Lalamove delivery
--   Order 2: Walk-in, no delivery row needed
--   Order 3: Online, Pampanga (outside NCR/Bulacan) → LBC delivery
-- =============================================
INSERT INTO [Order]
    (UserId, OrderNumber, OrderDate, OrderStatus,
     SubTotal, DiscountAmount, ShippingFee, TotalAmount,
     ShippingAddress, ShippingCity, ShippingPostalCode,
     ContactPhone, IsWalkIn)
VALUES
(2, 'ORD-2026-00001', GETDATE(), 'Pending',
 43000.00, 4300.00, 150.00, 38850.00,
 '456 Rizal St, Poblacion', 'Makati', '1210',
 '09281234567', 0),

(1, 'ORD-2026-00002', GETDATE(), 'Delivered',
 1500.00, 0.00, 0.00, 1500.00,
 NULL, NULL, NULL,
 '09171234567', 1),   -- walk-in, no delivery record

(2, 'ORD-2026-00003', GETDATE(), 'Processing',
 6000.00, 0.00, 200.00, 6200.00,
 '12 Sto. Rosario St, San Fernando', 'San Fernando, Pampanga', '2000',
 '09281234567', 0);   -- outside NCR/Bulacan → LBC
GO

-- =============================================
-- OrderItems
-- =============================================
INSERT INTO OrderItem (OrderId, ProductId, ProductVariantId, Quantity, UnitPrice, Subtotal)
VALUES
(1, 1,   1, 1, 40000.00, 40000.00),  -- Order 1: Pinewood Climber, Medium variant
(1, 117, NULL, 2,  1500.00,  3000.00),  -- Order 1: 2x Maxxis Ikon 27.5 Tanwall
(3, 24,  NULL, 1,  6000.00,  6000.00);  -- Order 3: Weapon Rifle Air Fork 120mm
GO

-- =============================================
-- VoucherUsages  (discount applied to Order 1)
-- =============================================
INSERT INTO VoucherUsage (VoucherId, UserId, OrderId, DiscountAmount)
VALUES
(1, 2, 1, 4300.00),   -- TAURUS10 applied on Order 1
(2, 2, 1,  150.00);   -- FREESHIP150 applied on Order 1
GO

-- =============================================
-- InventoryLog  (auto-populated by trigger on price change,
--               manual entries here for sale and purchase events)
-- =============================================
INSERT INTO InventoryLog
    (ProductId, ProductVariantId, ChangeQuantity, ChangeType, OrderId, PurchaseOrderId, ChangedByUserId, Notes)
VALUES
(1, 1, -1, 'Sale',     1, NULL, NULL,
 'Unit sold via online order ORD-2026-00001'),

(44, NULL, 10, 'Purchase', NULL, 2, 1,
 'Stock received from Sagmit Components PO-2');
GO

-- =============================================
-- Payments
-- Delivery orders: GCash or BankTransfer, PaymentStage = 'Upfront'
-- Walk-in POS:     Cash, PaymentStage = 'Upfront'
-- =============================================
INSERT INTO Payment
    (OrderId, PaymentMethod, PaymentStage, PaymentStatus, Amount, GcashTransactionId, BpiReferenceNumber, PaymentDate)
VALUES
(1, 'GCash',        'Upfront', 'Completed', 38850.00, 'GC20260311-ABCDE', NULL,              GETDATE()),
(2, 'Cash',         'Upfront', 'Completed',  1500.00,  NULL,              NULL,              GETDATE());
GO

-- =============================================
-- Deliveries
-- Only online orders get a Delivery record.
-- Walk-in orders (IsWalkIn = 1) have no Delivery row.
--   Order 1: Makati (Metro Manila)         → Lalamove, status Pending
--   Order 3: San Fernando, Pampanga        → LBC, status InTransit
-- =============================================
INSERT INTO Delivery
    (OrderId, Courier, LalamoveBookingRef, LbcTrackingNumber,
     DeliveryStatus, DriverName, DriverPhone, EstimatedDeliveryTime, ActualDeliveryTime)
VALUES
(1, 'Lalamove', 'LLM-2026-00123', NULL,
 'Pending', NULL, NULL, DATEADD(HOUR, 4, GETDATE()), NULL),

(3, 'LBC', NULL, 'LBC-20260311-78901',
 'InTransit', NULL, NULL, DATEADD(DAY, 3, GETDATE()), NULL);
GO

-- =============================================
-- Reviews  (customer reviews delivered orders)
-- Walk-in order (OrderId 2) used here; online order
-- OrderId 1 is still Pending so it cannot be reviewed yet
-- in production, but we include it for test coverage.
-- =============================================
INSERT INTO Review (UserId, ProductId, OrderId, Rating, Comment, IsVerifiedPurchase)
VALUES
(2, 117, 1, 5,
 'Maxxis tyres roll fast and hook up great on loose dirt. Worth every peso.',
 1),

(2, 1, 1, 5,
 'Carbon frame feels incredibly stiff and light. Very happy with the Pinewood Climber.',
 1);
GO

-- =============================================
-- POS_Sessions
-- =============================================
INSERT INTO POS_Session (UserId, TerminalName, ShiftStart, ShiftEnd, TotalSales)
VALUES
(1, 'POS-TERMINAL-01', DATEADD(HOUR, -8, GETDATE()), GETDATE(),         1500.00),
(1, 'POS-TERMINAL-01', DATEADD(HOUR,-16, GETDATE()), DATEADD(HOUR,-8,GETDATE()), 0.00);
GO

-- =============================================
-- SystemLog
-- =============================================
INSERT INTO SystemLog (UserId, EventType, EventDescription)
VALUES
(1, 'Login',         'Admin user Marco Reyes logged in from POS-TERMINAL-01'),
(2, 'Login',         'Customer Juan Dela Cruz logged in via web portal');
GO


PRINT '==============================================';
PRINT 'Taurus_seed.sql v3.2  —  Completed!';
PRINT '==============================================';
PRINT 'Roles           :   5 inserted';
PRINT 'Categories      :  13 inserted';
PRINT 'Brands          :  43 inserted';
PRINT 'Products        : 148 inserted';
PRINT '----------------------------------------------';
PRINT 'Users           :   2 inserted';
PRINT 'UserRoles       :   2 inserted';
PRINT 'ProductVariants :   2 inserted';
PRINT 'ProductImages   :   2 inserted';
PRINT 'Suppliers       :   2 inserted';
PRINT 'PurchaseOrders  :   2 inserted';
PRINT 'POItems         :   2 inserted';
PRINT 'Vouchers        :   2 inserted';
PRINT 'UserVouchers    :   2 inserted';
PRINT 'Carts           :   2 inserted';
PRINT 'CartItems       :   2 inserted';
PRINT 'Wishlists       :   2 inserted';
PRINT 'Orders          :   3 inserted';
PRINT 'OrderItems      :   3 inserted';
PRINT 'VoucherUsages   :   2 inserted';
PRINT 'InventoryLogs   :   2 inserted';
PRINT 'Payments        :   2 inserted';
PRINT 'Deliveries      :   2 inserted (online orders only)';
PRINT 'Reviews         :   2 inserted';
PRINT 'POS_Sessions    :   2 inserted';
PRINT 'SystemLogs      :   2 inserted';
PRINT '==============================================';
PRINT 'NOTE: PriceHistory is auto-populated by the';
PRINT '      TR_Product_PriceAudit trigger when any';
PRINT '      Product.Price is updated — no manual';
PRINT '      inserts needed.';
PRINT '==============================================';
GO
