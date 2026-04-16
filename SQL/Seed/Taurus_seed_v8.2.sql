-- =============================================================================
-- Taurus Bike Shop  |  TaurusBikeShopDB
-- File    : Taurus_seed_v8.2.sql
-- Purpose : Seed all product catalog data sourced from PARTS-PRICES.xlsx
-- Schema  : v8.1 + v8.2 audit fixes
-- Products: 150 (all products from Excel, no additions)
-- Run after: Taurus_schema_8.1.sql → Taurus_schema_8.2_audit_fixes.sql
--
-- Reconciled from seed v7.1 to match final schema state (8.1 + 8.2).
-- Changes from v7.1:
--   * Added required SET options for filtered indexes & computed columns
--   * Added USE [database] statement
--   * All new NOT NULL columns (IsDeleted on User/Order/Payment,
--     FulfillmentType on Order) handled via schema DEFAULT constraints
--   * Computed column TotalAmount on [Order] is NOT inserted
--   * datetime2(7) columns handled via SYSDATETIME() defaults
--
-- Seed sections:
--   A. Role
--   B. User (admin + cashier + 2 customers + walk-in)
--   C. Address + DefaultAddressId update
--   D. UserRole
--   E. Category (13 from Excel + 1 parent)
--   F. Brand  (43 brands inferred from product names)
--   G. Product (150 from Excel)
--   H. ProductVariant (150 Default variants — one per product)
--   I. ProductImage (one placeholder image per product)
--   J. InventoryLog (opening Purchase entries per variant)
-- =============================================================================

USE [Taurus-bike-shop-sqlserver-2026]
GO

-- =============================================================================
-- Required SET options for filtered indexes & computed columns (schema 8.2)
-- =============================================================================
SET ANSI_NULLS ON;
SET ANSI_PADDING ON;
SET ANSI_WARNINGS ON;
SET ARITHABORT ON;
SET CONCAT_NULL_YIELDS_NULL ON;
SET QUOTED_IDENTIFIER ON;
SET NOCOUNT ON;
GO

-- =============================================================================
-- A. ROLE
-- =============================================================================
INSERT INTO [Role] (RoleName, [Description]) VALUES
    ('Admin',    'Full system access'),
    ('Manager',  'Manages orders, inventory, and staff'),
    ('Cashier',  'Operates walk-in POS'),
    ('Staff',    'Handles logistics and support'),
    ('Customer', 'Registered online customer');
GO
-- RoleId: Admin=1, Manager=2, Cashier=3, Staff=4, Customer=5

-- =============================================================================
-- B. USER
-- All passwords: admin/cashier = 'Admin123!' | customers = 'Customer123!'
-- Hashes generated with BCrypt.Net-Next v4.0.3, work factor 12.
-- =============================================================================
INSERT INTO [User] (Email, PasswordHash, FirstName, LastName, PhoneNumber, IsActive, IsWalkIn) VALUES
    ('taurusbikeshop89@gmail.com',   '$2a$12$rUkMGNZlldMncyg0rZNfkOoJRa2AJRmatZeC3ZSKHaN8I.Rg23sV2', 'Marco',  'Reyes',   '09171110001', 1, 0),  -- UserId=1
    ('cashier@taurusbikes.com', '$2a$12$HjIB6/5ltM99Vvymz.EXFuHEOyLZTvqIfDc8BWDo45GIvTMpz91Fi', 'Carlo',  'Santos',  '09173330003', 1, 0),  -- UserId=2
    ('juan.dc@email.com',       '$2a$12$hXWT5u.TPmFdgysc5fz6lOeIe.346MLtVI0ajYV21LOv/i0C14x/a',  'Juan',   'DelaCruz','09181234567', 1, 0),  -- UserId=3
    ('maria.g@email.com',       '$2a$12$Rn85VupjEaPcOdQlWKnxauOZeQ/BwtTYM2nBRp4LW6MrWTpbzFp8S', 'Maria',  'Garcia',  '09189876543', 1, 0),  -- UserId=4
    (NULL, NULL, 'Walk-In', 'Customer', NULL, 1, 1);                                                                                          -- UserId=5
GO

-- =============================================================================
-- C. ADDRESS + DefaultAddressId update
-- =============================================================================
INSERT INTO [Address] (UserId, Label, Street, City, PostalCode, Province, Country, IsDefault, IsSnapshot) VALUES
    (1, 'Home', '12 Admin Street, BGC',    'Taguig',     '1634', 'Metro Manila', 'Philippines', 1, 0),  -- AddressId=1
    (2, 'Home', '56 Cashier Road, Pasig',  'Pasig',      '1600', 'Metro Manila', 'Philippines', 1, 0),  -- AddressId=2
    (3, 'Home', '78 Sampaguita St, QC',    'Quezon City','1100', 'Metro Manila', 'Philippines', 1, 0),  -- AddressId=3
    (4, 'Home', '22 Mabini St, Manila',    'Manila',     '1000', 'Metro Manila', 'Philippines', 1, 0);  -- AddressId=4
GO
UPDATE [User] SET DefaultAddressId = 1 WHERE UserId = 1;
UPDATE [User] SET DefaultAddressId = 2 WHERE UserId = 2;
UPDATE [User] SET DefaultAddressId = 3 WHERE UserId = 3;
UPDATE [User] SET DefaultAddressId = 4 WHERE UserId = 4;
GO

-- =============================================================================
-- D. USERROLE
-- =============================================================================
INSERT INTO UserRole (UserId, RoleId) VALUES
    (1, 1),  -- Marco  → Admin
    (2, 3),  -- Carlo  → Cashier
    (3, 5),  -- Juan   → Customer
    (4, 5);  -- Maria  → Customer
GO

-- =============================================================================
-- E. CATEGORY
-- 13 categories from Excel + 1 parent 'BIKES' grouping all of them.
-- CategoryId assignment:
-- 1=BIKES(parent), 2=UNIT, 3=FRAME, 4=FORK, 5=HUB, 6=UPGKIT,
-- 7=STEM, 8=HBAR, 9=SADDLE, 10=GRIP, 11=PEDAL, 12=RIM, 13=TIRE, 14=CHAIN
-- =============================================================================
INSERT INTO Category (CategoryCode, [Name], [Description], ParentCategoryId, IsActive, DisplayOrder) VALUES
    ('BIKES',  'Bicycles & Parts',       'All bicycle products and components',         NULL, 1,  1),  -- CategoryId=1
    ('UNIT', 'Complete Units', 'Ready-to-ride complete bicycles', 1, 1, 2),  -- CategoryId=2
    ('FRAME', 'Frames', 'Bicycle framesets', 1, 1, 3),  -- CategoryId=3
    ('FORK', 'Forks', 'Front suspension and rigid forks', 1, 1, 4),  -- CategoryId=4
    ('HUB', 'Hubs', 'Front and rear wheel hubs', 1, 1, 5),  -- CategoryId=5
    ('UPGKIT', 'Upgrade Kits', 'Drivetrain upgrade packages', 1, 1, 6),  -- CategoryId=6
    ('STEM', 'Stems & Seatposts', 'Handlebar stems and seatposts', 1, 1, 7),  -- CategoryId=7
    ('HBAR', 'Handlebars', 'Flat, riser, drop, and gravel handlebars', 1, 1, 8),  -- CategoryId=8
    ('SADDLE', 'Saddles', 'Bicycle saddles and seats', 1, 1, 9),  -- CategoryId=9
    ('GRIP', 'Grips & Bar Tape', 'Handlebar grips and bar tape', 1, 1, 10),  -- CategoryId=10
    ('PEDAL', 'Pedals', 'Flat, platform, and touring pedals', 1, 1, 11),  -- CategoryId=11
    ('RIM', 'Rims & Wheelsets', 'Individual rims and complete wheelsets', 1, 1, 12),  -- CategoryId=12
    ('TIRE', 'Tires & Tubes', 'Bicycle tires and inner tubes', 1, 1, 13),  -- CategoryId=13
    ('CHAIN', 'Chains & Drivetrain', 'Chains and drivetrain components', 1, 1, 14);  -- CategoryId=14
GO

-- =============================================================================
-- F. BRAND (43 brands inferred from product names in Excel)
-- =============================================================================
INSERT INTO Brand (BrandName, Country, IsActive) VALUES
    ('Aeroic', 'Philippines', 1),  -- BrandId=1
    ('American Classic', 'USA', 1),  -- BrandId=2
    ('Answer', 'USA', 1),  -- BrandId=3
    ('Cole', 'Philippines', 1),  -- BrandId=4
    ('ControlTech', 'Taiwan', 1),  -- BrandId=5
    ('Cult', 'USA', 1),  -- BrandId=6
    ('Diamond', 'Philippines', 1),  -- BrandId=7
    ('Easydo', 'China', 1),  -- BrandId=8
    ('Elves', 'China', 1),  -- BrandId=9
    ('Garuda', 'Philippines', 1),  -- BrandId=10
    ('Generic', 'Philippines', 1),  -- BrandId=11
    ('Genova', 'Philippines', 1),  -- BrandId=12
    ('Giant', 'Taiwan', 1),  -- BrandId=13
    ('Jalco', 'Taiwan', 1),  -- BrandId=14
    ('KMC', 'Taiwan', 1),  -- BrandId=15
    ('Kespor', 'Philippines', 1),  -- BrandId=16
    ('Kore', 'Canada', 1),  -- BrandId=17
    ('LDCNC', 'Philippines', 1),  -- BrandId=18
    ('LTwoo', 'China', 1),  -- BrandId=19
    ('Leo', 'Philippines', 1),  -- BrandId=20
    ('MKS', 'Japan', 1),  -- BrandId=21
    ('Manitou', 'USA', 1),  -- BrandId=22
    ('Maxxis', 'Taiwan', 1),  -- BrandId=23
    ('MountainPeak', 'Philippines', 1),  -- BrandId=24
    ('Origin8', 'USA', 1),  -- BrandId=25
    ('Pinewood', 'Philippines', 1),  -- BrandId=26
    ('Ragusa', 'Philippines', 1),  -- BrandId=27
    ('Ryder', 'Philippines', 1),  -- BrandId=28
    ('SR Suntour', 'Taiwan', 1),  -- BrandId=29
    ('SRAM', 'USA', 1),  -- BrandId=30
    ('SUMC', 'China', 1),  -- BrandId=31
    ('Sagmit', 'Philippines', 1),  -- BrandId=32
    ('San Marco', 'Italy', 1),  -- BrandId=33
    ('Saturn', 'Philippines', 1),  -- BrandId=34
    ('Seer', 'Philippines', 1),  -- BrandId=35
    ('Shimano', 'Japan', 1),  -- BrandId=36
    ('Specialized', 'USA', 1),  -- BrandId=37
    ('Speedone', 'Philippines', 1),  -- BrandId=38
    ('Topgrade', 'Philippines', 1),  -- BrandId=39
    ('Toseek', 'China', 1),  -- BrandId=40
    ('Velo', 'Taiwan', 1),  -- BrandId=41
    ('WTB', 'USA', 1),  -- BrandId=42
    ('Weapon', 'Philippines', 1);  -- BrandId=43
GO

-- =============================================================================
-- G. PRODUCT (150 products from Excel)
-- CategoryId mapping: UNIT=2, FRAME=3, FORK=4, HUB=5, UPGKIT=6,
-- STEM=7, HBAR=8, SADDLE=9, GRIP=10, PEDAL=11, RIM=12, TIRE=13, CHAIN=14
-- =============================================================================
INSERT INTO Product (CategoryId, BrandId, SKU, [Name], ShortDescription, Price, Currency, IsActive, IsFeatured) VALUES
    (2, 26, 'UNIT-001', N'2021 Pinewood Climber CARBON 27.5', N'Complete bicycle unit — 2021 Pinewood Climber CARBON 27.5', 40000, 'PHP', 1, 1),  -- ProductId=1
    (2, 6, 'UNIT-002', N'Cult Odyssey Hydro Brakes 27.5', N'Complete bicycle unit — Cult Odyssey Hydro Brakes 27.5', 14500, 'PHP', 1, 1),  -- ProductId=2
    (2, 40, 'UNIT-003', N'Toseek Chester 700c Disc Brake ALLOY (2x9)', N'Complete bicycle unit — Toseek Chester 700c Disc Brake ALLOY (2x9)', 11000, 'PHP', 1, 1),  -- ProductId=3
    (2, 28, 'UNIT-004', N'Ryder X3 27.5 2021 MT200 Hydro Brakes (3x8)', N'Complete bicycle unit — Ryder X3 27.5 2021 MT200 Hydro Brakes (3x8)', 12500, 'PHP', 1, 1),  -- ProductId=4
    (2, 26, 'UNIT-005', N'Pinewood Trident Flux', N'Complete bicycle unit — Pinewood Trident Flux', 17500, 'PHP', 1, 1),  -- ProductId=5
    (2, 10, 'UNIT-006', N'Garuda Rampage', N'Complete bicycle unit — Garuda Rampage', 14500, 'PHP', 1, 1),  -- ProductId=6
    (2, 26, 'UNIT-007', N'Pinewood Challenger', N'Complete bicycle unit — Pinewood Challenger', 15500, 'PHP', 1, 1),  -- ProductId=7
    (2, 16, 'UNIT-008', N'Kespor Stork Feather CX 1.0 2022', N'Complete bicycle unit — Kespor Stork Feather CX 1.0 2022', 55000, 'PHP', 1, 1),  -- ProductId=8
    (2, 26, 'UNIT-009', N'Pinewood Lancer 1.0 2022 Gravel RX (2x9)', N'Complete bicycle unit — Pinewood Lancer 1.0 2022 Gravel RX (2x9)', 15500, 'PHP', 1, 1),  -- ProductId=9
    (3, 9, 'FRAME-001', N'ELVES NANDOR', N'Bicycle frameset — ELVES NANDOR', 32000, 'PHP', 1, 0),  -- ProductId=10
    (3, 28, 'FRAME-002', N'Ryder X2', N'Bicycle frameset — Ryder X2', 4500, 'PHP', 1, 0),  -- ProductId=11
    (3, 24, 'FRAME-003', N'MountainPeak Monster', N'Bicycle frameset — MountainPeak Monster', 6500, 'PHP', 1, 0),  -- ProductId=12
    (3, 24, 'FRAME-004', N'Mountainpeak Everest 2', N'Bicycle frameset — Mountainpeak Everest 2', 7800, 'PHP', 1, 0),  -- ProductId=13
    (3, 37, 'FRAME-005', N'Specialized Stumpjumper', N'Bicycle frameset — Specialized Stumpjumper', 22000, 'PHP', 1, 0),  -- ProductId=14
    (3, 43, 'FRAME-006', N'Weapon Stealth 29', N'Bicycle frameset — Weapon Stealth 29', 6500, 'PHP', 1, 0),  -- ProductId=15
    (3, 43, 'FRAME-007', N'Weapon Spartan 29', N'Bicycle frameset — Weapon Spartan 29', 8500, 'PHP', 1, 0),  -- ProductId=16
    (3, 34, 'FRAME-008', N'Saturn Calypso', N'Bicycle frameset — Saturn Calypso', 8800, 'PHP', 1, 0),  -- ProductId=17
    (3, 34, 'FRAME-009', N'Saturn Dione', N'Bicycle frameset — Saturn Dione', 7000, 'PHP', 1, 0),  -- ProductId=18
    (3, 32, 'FRAME-010', N'Sagmit Chaser', N'Bicycle frameset — Sagmit Chaser', 7500, 'PHP', 1, 0),  -- ProductId=19
    (3, 4, 'FRAME-011', N'COLE NX 27.5 TRI-FACTOR 2021', N'Bicycle frameset — COLE NX 27.5 TRI-FACTOR 2021', 6000, 'PHP', 1, 0),  -- ProductId=20
    (3, 38, 'FRAME-012', N'Speedone Floater BOOST', N'Bicycle frameset — Speedone Floater BOOST', 8500, 'PHP', 1, 0),  -- ProductId=21
    (4, 1, 'FORK-001', N'Aeroic AIR FORK', N'Suspension/rigid fork — Aeroic AIR FORK', 2900, 'PHP', 1, 0),  -- ProductId=22
    (4, 43, 'FORK-002', N'Weapon Cannon35 BOOST', N'Suspension/rigid fork — Weapon Cannon35 BOOST', 7000, 'PHP', 1, 0),  -- ProductId=23
    (4, 43, 'FORK-003', N'Weapon Rifle', N'Suspension/rigid fork — Weapon Rifle', 6000, 'PHP', 1, 0),  -- ProductId=24
    (4, 43, 'FORK-004', N'Weapon Rocket', N'Suspension/rigid fork — Weapon Rocket', 6000, 'PHP', 1, 0),  -- ProductId=25
    (4, 43, 'FORK-005', N'Fork Weapon Tower', N'Suspension/rigid fork — Fork Weapon Tower', 4000, 'PHP', 1, 0),  -- ProductId=26
    (4, 38, 'FORK-006', N'Speedone Soldier BOOST', N'Suspension/rigid fork — Speedone Soldier BOOST', 5900, 'PHP', 1, 0),  -- ProductId=27
    (4, 22, 'FORK-007', N'Manitou Markhor BOOST', N'Suspension/rigid fork — Manitou Markhor BOOST', 12000, 'PHP', 1, 0),  -- ProductId=28
    (4, 22, 'FORK-008', N'Manitou Machete Comp BOOST', N'Suspension/rigid fork — Manitou Machete Comp BOOST', 16500, 'PHP', 1, 0),  -- ProductId=29
    (4, 22, 'FORK-009', N'Manitou Mattoc Comp Boost', N'Suspension/rigid fork — Manitou Mattoc Comp Boost', 25000, 'PHP', 1, 0),  -- ProductId=30
    (4, 29, 'FORK-010', N'SR Suntour Epixon Stealth', N'Suspension/rigid fork — SR Suntour Epixon Stealth', 8600, 'PHP', 1, 0),  -- ProductId=31
    (4, 29, 'FORK-011', N'SR Suntour Raidon BOOST', N'Suspension/rigid fork — SR Suntour Raidon BOOST', 9000, 'PHP', 1, 0),  -- ProductId=32
    (4, 29, 'FORK-012', N'SR Suntour XCR 32 BOOST', N'Suspension/rigid fork — SR Suntour XCR 32 BOOST', 52000, 'PHP', 1, 0),  -- ProductId=33
    (5, 18, 'HUB-001', N'LDCNC 3.0', N'Wheel hub set — LDCNC 3.0', 2200, 'PHP', 1, 0),  -- ProductId=34
    (5, 38, 'HUB-002', N'SpeedOne Soldier BOOST', N'Wheel hub set — SpeedOne Soldier BOOST', 3400, 'PHP', 1, 0),  -- ProductId=35
    (5, 12, 'HUB-003', N'Genova Big Dipper', N'Wheel hub set — Genova Big Dipper', 3000, 'PHP', 1, 0),  -- ProductId=36
    (5, 43, 'HUB-004', N'Weapon Animal', N'Wheel hub set — Weapon Animal', 4500, 'PHP', 1, 0),  -- ProductId=37
    (5, 32, 'HUB-005', N'Sagmit EVO3', N'Wheel hub set — Sagmit EVO3', 3400, 'PHP', 1, 0),  -- ProductId=38
    (5, 27, 'HUB-006', N'Ragusa R100', N'Wheel hub set — Ragusa R100', 950, 'PHP', 1, 0),  -- ProductId=39
    (5, 38, 'HUB-007', N'SpeedOne Soldier', N'Wheel hub set — SpeedOne Soldier', 3000, 'PHP', 1, 0),  -- ProductId=40
    (5, 38, 'HUB-008', N'SpeedOne Pilot Carbon', N'Wheel hub set — SpeedOne Pilot Carbon', 3450, 'PHP', 1, 0),  -- ProductId=41
    (5, 38, 'HUB-009', N'SpeedOne Torpedo', N'Wheel hub set — SpeedOne Torpedo', 3600, 'PHP', 1, 0),  -- ProductId=42
    (5, 25, 'HUB-010', N'Origin8 NON BOOST', N'Wheel hub set — Origin8 NON BOOST', 5200, 'PHP', 1, 0),  -- ProductId=43
    (6, 32, 'UPGKIT-001', N'Sagmit edison 12spd up kit', N'Drivetrain upgrade kit — Sagmit edison 12spd up kit', 5200, 'PHP', 1, 0),  -- ProductId=44
    (6, 19, 'UPGKIT-002', N'12spd LTwoo AX Upkit with IXF crank', N'Drivetrain upgrade kit — 12spd LTwoo AX Upkit with IXF crank', 8000, 'PHP', 1, 0),  -- ProductId=45
    (6, 43, 'UPGKIT-003', N'Deore M6100 12spd Upkit with Weapon Cogs', N'Drivetrain upgrade kit — Deore M6100 12spd Upkit with Weapon Cogs', 9000, 'PHP', 1, 0),  -- ProductId=46
    (6, 36, 'UPGKIT-004', N'ZEE M640 10spd Upkit', N'Drivetrain upgrade kit — ZEE M640 10spd Upkit', 6800, 'PHP', 1, 0),  -- ProductId=47
    (6, 30, 'UPGKIT-005', N'SRAM NX Eagle 12spd WITH BB', N'Drivetrain upgrade kit — SRAM NX Eagle 12spd WITH BB', 13000, 'PHP', 1, 0),  -- ProductId=48
    (7, 5, 'STEM-001', N'Handle Post Controltech ONE', N'Handlebar stem / seatpost — Handle Post Controltech ONE', 1800, 'PHP', 1, 0),  -- ProductId=49
    (7, 5, 'STEM-002', N'HandlePost ControlTech', N'Handlebar stem / seatpost — HandlePost ControlTech', 1600, 'PHP', 1, 0),  -- ProductId=50
    (7, 5, 'STEM-003', N'SeatPost ControlTech ONE', N'Handlebar stem / seatpost — SeatPost ControlTech ONE', 1700, 'PHP', 1, 0),  -- ProductId=51
    (7, 5, 'STEM-004', N'HandlePost ControlTech CLS', N'Handlebar stem / seatpost — HandlePost ControlTech CLS', 1800, 'PHP', 1, 0),  -- ProductId=52
    (7, 43, 'STEM-005', N'Weapon Fury stem', N'Handlebar stem / seatpost — Weapon Fury stem', 850, 'PHP', 1, 0),  -- ProductId=53
    (7, 43, 'STEM-006', N'Weapon Ambush Stem', N'Handlebar stem / seatpost — Weapon Ambush Stem', 1000, 'PHP', 1, 0),  -- ProductId=54
    (7, 43, 'STEM-007', N'Weapon Savage Stem', N'Handlebar stem / seatpost — Weapon Savage Stem', 1100, 'PHP', 1, 0),  -- ProductId=55
    (7, 43, 'STEM-008', N'Weapon Beast Stem', N'Handlebar stem / seatpost — Weapon Beast Stem', 1200, 'PHP', 1, 0),  -- ProductId=56
    (7, 43, 'STEM-009', N'Weapon Animal Stem', N'Handlebar stem / seatpost — Weapon Animal Stem', 1100, 'PHP', 1, 0),  -- ProductId=57
    (7, 43, 'STEM-010', N'Weapon Predator Stem', N'Handlebar stem / seatpost — Weapon Predator Stem', 1000, 'PHP', 1, 0),  -- ProductId=58
    (8, 11, 'HBAR-001', N'Handle Bar Pro LT', N'Handlebar — Handle Bar Pro LT', 1800, 'PHP', 1, 0),  -- ProductId=59
    (8, 11, 'HBAR-002', N'Handle Bar Pro Koryak', N'Handlebar — Handle Bar Pro Koryak', 2200, 'PHP', 1, 0),  -- ProductId=60
    (8, 11, 'HBAR-003', N'Drop Bar Gravel Pro LT', N'Handlebar — Drop Bar Gravel Pro LT', 1800, 'PHP', 1, 0),  -- ProductId=61
    (8, 11, 'HBAR-004', N'Drop Bar Gravel Pro Discovery', N'Handlebar — Drop Bar Gravel Pro Discovery', 2200, 'PHP', 1, 0),  -- ProductId=62
    (8, 3, 'HBAR-005', N'Answer ProTaper 20x20', N'Handlebar — Answer ProTaper 20x20', 2000, 'PHP', 1, 0),  -- ProductId=63
    (8, 3, 'HBAR-006', N'Answer ProTaper', N'Handlebar — Answer ProTaper', 2000, 'PHP', 1, 0),  -- ProductId=64
    (8, 3, 'HBAR-007', N'Answer ProTaper 7050 series Aluminum', N'Handlebar — Answer ProTaper 7050 series Aluminum', 1600, 'PHP', 1, 0),  -- ProductId=65
    (8, 32, 'HBAR-008', N'Sagmit Brooklyn 3.0', N'Handlebar — Sagmit Brooklyn 3.0', 550, 'PHP', 1, 0),  -- ProductId=66
    (8, 32, 'HBAR-009', N'Sagmit shadow', N'Handlebar — Sagmit shadow', 550, 'PHP', 1, 0),  -- ProductId=67
    (8, 32, 'HBAR-010', N'Sagmit Static', N'Handlebar — Sagmit Static', 750, 'PHP', 1, 0),  -- ProductId=68
    (9, 32, 'SADDLE-001', N'Sagmit RedClassic skull', N'Bicycle saddle — Sagmit RedClassic skull', 450, 'PHP', 1, 0),  -- ProductId=69
    (9, 12, 'SADDLE-002', N'Genova Jupiter', N'Bicycle saddle — Genova Jupiter', 450, 'PHP', 1, 0),  -- ProductId=70
    (9, 13, 'SADDLE-003', N'Giant w/o Hole', N'Bicycle saddle — Giant w/o Hole', 450, 'PHP', 1, 0),  -- ProductId=71
    (9, 13, 'SADDLE-004', N'Giant with Hole', N'Bicycle saddle — Giant with Hole', 450, 'PHP', 1, 0),  -- ProductId=72
    (9, 27, 'SADDLE-005', N'Ragusa R-100', N'Bicycle saddle — Ragusa R-100', 450, 'PHP', 1, 0),  -- ProductId=73
    (9, 39, 'SADDLE-006', N'Topgrade', N'Bicycle saddle — Topgrade', 350, 'PHP', 1, 0),  -- ProductId=74
    (9, 7, 'SADDLE-007', N'Diamond', N'Bicycle saddle — Diamond', 350, 'PHP', 1, 0),  -- ProductId=75
    (9, 33, 'SADDLE-008', N'San Marco', N'Bicycle saddle — San Marco', 450, 'PHP', 1, 0),  -- ProductId=76
    (9, 41, 'SADDLE-009', N'Velo Plush', N'Bicycle saddle — Velo Plush', 450, 'PHP', 1, 0),  -- ProductId=77
    (9, 32, 'SADDLE-010', N'Sagmit Oilslick', N'Bicycle saddle — Sagmit Oilslick', 550, 'PHP', 1, 0),  -- ProductId=78
    (9, 12, 'SADDLE-011', N'Genova Mars', N'Bicycle saddle — Genova Mars', 450, 'PHP', 1, 0),  -- ProductId=79
    (9, 37, 'SADDLE-012', N'Specialized Black', N'Bicycle saddle — Specialized Black', 450, 'PHP', 1, 0),  -- ProductId=80
    (9, 37, 'SADDLE-013', N'Specialized M811', N'Bicycle saddle — Specialized M811', 450, 'PHP', 1, 0),  -- ProductId=81
    (9, 8, 'SADDLE-014', N'Easydo ES-01', N'Bicycle saddle — Easydo ES-01', 450, 'PHP', 1, 0),  -- ProductId=82
    (10, 35, 'GRIP-001', N'Silicon Grip Seer 5mm', N'Handlebar grip / bar tape — Silicon Grip Seer 5mm', 330, 'PHP', 1, 0),  -- ProductId=83
    (10, 35, 'GRIP-002', N'Seer Polymer Bartape', N'Handlebar grip / bar tape — Seer Polymer Bartape', 350, 'PHP', 1, 0),  -- ProductId=84
    (10, 35, 'GRIP-003', N'Silicon Grip Seer 7mm', N'Handlebar grip / bar tape — Silicon Grip Seer 7mm', 400, 'PHP', 1, 0),  -- ProductId=85
    (10, 35, 'GRIP-004', N'Seer Art Racing Edition BarTape', N'Handlebar grip / bar tape — Seer Art Racing Edition BarTape', 400, 'PHP', 1, 0),  -- ProductId=86
    (10, 35, 'GRIP-005', N'Seer SILICON Hornet Bar Tape', N'Handlebar grip / bar tape — Seer SILICON Hornet Bar Tape', 450, 'PHP', 1, 0),  -- ProductId=87
    (10, 35, 'GRIP-006', N'Seer POLYMER Hornet Bar Tape', N'Handlebar grip / bar tape — Seer POLYMER Hornet Bar Tape', 450, 'PHP', 1, 0),  -- ProductId=88
    (10, 35, 'GRIP-007', N'Seer Super Lite Bar tape', N'Handlebar grip / bar tape — Seer Super Lite Bar tape', 300, 'PHP', 1, 0),  -- ProductId=89
    (10, 11, 'GRIP-008', N'Handle Grip Attack Dual Lock-On', N'Handlebar grip / bar tape — Handle Grip Attack Dual Lock-On', 370, 'PHP', 1, 0),  -- ProductId=90
    (10, 43, 'GRIP-009', N'Weapon Race w/ Palm Rest', N'Handlebar grip / bar tape — Weapon Race w/ Palm Rest', 400, 'PHP', 1, 0),  -- ProductId=91
    (10, 43, 'GRIP-010', N'Weapon Rubix', N'Handlebar grip / bar tape — Weapon Rubix', 200, 'PHP', 1, 0),  -- ProductId=92
    (10, 43, 'GRIP-011', N'Weapon Wave', N'Handlebar grip / bar tape — Weapon Wave', 350, 'PHP', 1, 0),  -- ProductId=93
    (11, 38, 'PEDAL-001', N'Speedone Soldier', N'Bicycle pedal set — Speedone Soldier', 1200, 'PHP', 1, 0),  -- ProductId=94
    (11, 38, 'PEDAL-002', N'Speedone Pilot', N'Bicycle pedal set — Speedone Pilot', 1200, 'PHP', 1, 0),  -- ProductId=95
    (11, 32, 'PEDAL-003', N'Pedal Sagmit 610', N'Bicycle pedal set — Pedal Sagmit 610', 800, 'PHP', 1, 0),  -- ProductId=96
    (11, 32, 'PEDAL-004', N'Pedal Sagmit 614', N'Bicycle pedal set — Pedal Sagmit 614', 800, 'PHP', 1, 0),  -- ProductId=97
    (11, 27, 'PEDAL-005', N'Ragusa R700', N'Bicycle pedal set — Ragusa R700', 500, 'PHP', 1, 0),  -- ProductId=98
    (11, 27, 'PEDAL-006', N'Ragusa CNC Sealed Bearing 712', N'Bicycle pedal set — Ragusa CNC Sealed Bearing 712', 800, 'PHP', 1, 0),  -- ProductId=99
    (11, 32, 'PEDAL-007', N'Sagmit 622 CNC Sealed Bearing', N'Bicycle pedal set — Sagmit 622 CNC Sealed Bearing', 800, 'PHP', 1, 0),  -- ProductId=100
    (11, 32, 'PEDAL-008', N'Sagmit 618 CNC Sealed Bearing', N'Bicycle pedal set — Sagmit 618 CNC Sealed Bearing', 800, 'PHP', 1, 0),  -- ProductId=101
    (11, 21, 'PEDAL-009', N'MKS JAPAN GR-9', N'Bicycle pedal set — MKS JAPAN GR-9', 1200, 'PHP', 1, 0),  -- ProductId=102
    (11, 21, 'PEDAL-010', N'MKS Sylvan Touring', N'Bicycle pedal set — MKS Sylvan Touring', 1400, 'PHP', 1, 0),  -- ProductId=103
    (11, 21, 'PEDAL-011', N'MKS BM7', N'Bicycle pedal set — MKS BM7', 1550, 'PHP', 1, 0),  -- ProductId=104
    (12, 18, 'RIM-001', N'LDCNC RS300 700c Rim Brake', N'Rim / wheelset — LDCNC RS300 700c Rim Brake', 5100, 'PHP', 1, 0),  -- ProductId=105
    (12, 32, 'RIM-002', N'Sagmit Aero', N'Rim / wheelset — Sagmit Aero', 1500, 'PHP', 1, 0),  -- ProductId=106
    (12, 32, 'RIM-003', N'Sagmit Legend M32', N'Rim / wheelset — Sagmit Legend M32', 1600, 'PHP', 1, 0),  -- ProductId=107
    (12, 17, 'RIM-004', N'Kore Realm 4.2', N'Rim / wheelset — Kore Realm 4.2', 1100, 'PHP', 1, 0),  -- ProductId=108
    (12, 42, 'RIM-005', N'WTB i21 Tubeless Ready', N'Rim / wheelset — WTB i21 Tubeless Ready', 1600, 'PHP', 1, 0),  -- ProductId=109
    (12, 42, 'RIM-006', N'WTB i25 Tubeless Ready', N'Rim / wheelset — WTB i25 Tubeless Ready', 1600, 'PHP', 1, 0),  -- ProductId=110
    (12, 2, 'RIM-007', N'American Classic Feldspar 290 Tubeless Ready', N'Rim / wheelset — American Classic Feldspar 290 Tubeless Ready', 1800, 'PHP', 1, 0),  -- ProductId=111
    (12, 14, 'RIM-008', N'Jalco Wellington Tubeless Ready', N'Rim / wheelset — Jalco Wellington Tubeless Ready', 1400, 'PHP', 1, 0),  -- ProductId=112
    (12, 38, 'RIM-009', N'Speedone Pilot', N'Rim / wheelset — Speedone Pilot', 2000, 'PHP', 1, 0),  -- ProductId=113
    (12, 38, 'RIM-010', N'Speedone Bazooka', N'Rim / wheelset — Speedone Bazooka', 1700, 'PHP', 1, 0),  -- ProductId=114
    (12, 38, 'RIM-011', N'Speedone Soldier', N'Rim / wheelset — Speedone Soldier', 1900, 'PHP', 1, 0),  -- ProductId=115
    (12, 32, 'RIM-012', N'Sagmit Safarri', N'Rim / wheelset — Sagmit Safarri', 1700, 'PHP', 1, 0),  -- ProductId=116
    (12, 14, 'RIM-013', N'Jalco Maranello', N'Rim / wheelset — Jalco Maranello', 1500, 'PHP', 1, 0),  -- ProductId=117
    (12, 17, 'RIM-014', N'Kore Realm 2.4', N'Rim / wheelset — Kore Realm 2.4', 1200, 'PHP', 1, 0),  -- ProductId=118
    (13, 23, 'TIRE-001', N'Maxxis Ikon 27.5 x 2.20 Tanwall', N'Bicycle tire or tube — Maxxis Ikon 27.5 x 2.20 Tanwall', 1500, 'PHP', 1, 0),  -- ProductId=119
    (13, 23, 'TIRE-002', N'Maxxis Ardent 29 x 2.25 Tanwall', N'Bicycle tire or tube — Maxxis Ardent 29 x 2.25 Tanwall', 1500, 'PHP', 1, 0),  -- ProductId=120
    (13, 23, 'TIRE-003', N'Maxxis Ikon 29 x 2.20', N'Bicycle tire or tube — Maxxis Ikon 29 x 2.20', 1500, 'PHP', 1, 0),  -- ProductId=121
    (13, 23, 'TIRE-004', N'Maxxis Crossmark II 27.5 x 2.25', N'Bicycle tire or tube — Maxxis Crossmark II 27.5 x 2.25', 1500, 'PHP', 1, 0),  -- ProductId=122
    (13, 23, 'TIRE-005', N'Maxxis Ikon 29 x 2.20 Tanwall', N'Bicycle tire or tube — Maxxis Ikon 29 x 2.20 Tanwall', 1500, 'PHP', 1, 0),  -- ProductId=123
    (13, 23, 'TIRE-006', N'Maxxis Rekon Race 27.5 x 2.25', N'Bicycle tire or tube — Maxxis Rekon Race 27.5 x 2.25', 1500, 'PHP', 1, 0),  -- ProductId=124
    (13, 23, 'TIRE-007', N'Maxxis Ardent 27.5 x 2.25', N'Bicycle tire or tube — Maxxis Ardent 27.5 x 2.25', 1500, 'PHP', 1, 0),  -- ProductId=125
    (13, 23, 'TIRE-008', N'Maxxis Ardent Race 27.5 x 2.20', N'Bicycle tire or tube — Maxxis Ardent Race 27.5 x 2.20', 1500, 'PHP', 1, 0),  -- ProductId=126
    (13, 23, 'TIRE-009', N'MAXXIS TUBE', N'Bicycle tire or tube — MAXXIS TUBE', 250, 'PHP', 1, 0),  -- ProductId=127
    (13, 32, 'TIRE-010', N'SAGMIT TUBE', N'Bicycle tire or tube — SAGMIT TUBE', 150, 'PHP', 1, 0),  -- ProductId=128
    (13, 20, 'TIRE-011', N'SIZE 20 LEO TUBE', N'Bicycle tire or tube — SIZE 20 LEO TUBE', 85, 'PHP', 1, 0),  -- ProductId=129
    (13, 20, 'TIRE-012', N'SIZE 26 LEO TUBE', N'Bicycle tire or tube — SIZE 26 LEO TUBE', 100, 'PHP', 1, 0),  -- ProductId=130
    (13, 11, 'TIRE-013', N'SIZE 20 TIRE', N'Bicycle tire or tube — SIZE 20 TIRE', 380, 'PHP', 1, 0),  -- ProductId=131
    (13, 11, 'TIRE-014', N'SIZE 16 TIRE', N'Bicycle tire or tube — SIZE 16 TIRE', 280, 'PHP', 1, 0),  -- ProductId=132
    (13, 11, 'TIRE-015', N'SIZE 16 TUBE', N'Bicycle tire or tube — SIZE 16 TUBE', 85, 'PHP', 1, 0),  -- ProductId=133
    (14, 31, 'CHAIN-001', N'8 SPEED SUMC CP With Missing link', N'Bicycle chain — 8 SPEED SUMC CP With Missing link', 300, 'PHP', 1, 0),  -- ProductId=134
    (14, 31, 'CHAIN-002', N'9 SPEED SUMC CP With Missing link', N'Bicycle chain — 9 SPEED SUMC CP With Missing link', 380, 'PHP', 1, 0),  -- ProductId=135
    (14, 31, 'CHAIN-003', N'10 SPEED SUMC CP With Missing link', N'Bicycle chain — 10 SPEED SUMC CP With Missing link', 450, 'PHP', 1, 0),  -- ProductId=136
    (14, 31, 'CHAIN-004', N'11 SPEED SUMC CP With Missing link', N'Bicycle chain — 11 SPEED SUMC CP With Missing link', 650, 'PHP', 1, 0),  -- ProductId=137
    (14, 31, 'CHAIN-005', N'12 SPEED SUMC CP With Missing link', N'Bicycle chain — 12 SPEED SUMC CP With Missing link', 1050, 'PHP', 1, 0),  -- ProductId=138
    (14, 31, 'CHAIN-006', N'8 SPEED SUMC Oilslick 116Links With Missing link', N'Bicycle chain — 8 SPEED SUMC Oilslick 116Links With Missing link', 450, 'PHP', 1, 0),  -- ProductId=139
    (14, 31, 'CHAIN-007', N'10 SPEED SUMC Oilslick 116Links With Missing link', N'Bicycle chain — 10 SPEED SUMC Oilslick 116Links With Missing link', 750, 'PHP', 1, 0),  -- ProductId=140
    (14, 31, 'CHAIN-008', N'11 SPEED SUMC Oilslick 116Links With Missing link', N'Bicycle chain — 11 SPEED SUMC Oilslick 116Links With Missing link', 1200, 'PHP', 1, 0),  -- ProductId=141
    (14, 15, 'CHAIN-009', N'12SPD KMC Chain X Series GREY', N'Bicycle chain — 12SPD KMC Chain X Series GREY', 1500, 'PHP', 1, 0),  -- ProductId=142
    (14, 15, 'CHAIN-010', N'11SPD KMC Chain X Series GREY', N'Bicycle chain — 11SPD KMC Chain X Series GREY', 1100, 'PHP', 1, 0),  -- ProductId=143
    (14, 15, 'CHAIN-011', N'10SPD KMC Chain X Series GREY', N'Bicycle chain — 10SPD KMC Chain X Series GREY', 900, 'PHP', 1, 0),  -- ProductId=144
    (14, 15, 'CHAIN-012', N'9SPD KMC Chain X Series GOLD', N'Bicycle chain — 9SPD KMC Chain X Series GOLD', 1800, 'PHP', 1, 0),  -- ProductId=145
    (14, 15, 'CHAIN-013', N'10SPD KMC Chain X Series GOLD', N'Bicycle chain — 10SPD KMC Chain X Series GOLD', 1800, 'PHP', 1, 0),  -- ProductId=146
    (14, 15, 'CHAIN-014', N'12SPD KMC Chain X Series GOLD', N'Bicycle chain — 12SPD KMC Chain X Series GOLD', 2400, 'PHP', 1, 0),  -- ProductId=147
    (14, 11, 'CHAIN-015', N'CT-1HX 116links Heavy Duty for FIXIE', N'Bicycle chain — CT-1HX 116links Heavy Duty for FIXIE', 350, 'PHP', 1, 0),  -- ProductId=148
    (14, 11, 'CHAIN-016', N'GT-10 126links', N'Bicycle chain — GT-10 126links', 500, 'PHP', 1, 0),  -- ProductId=149
    (14, 11, 'CHAIN-017', N'GT-11 126links', N'Bicycle chain — GT-11 126links', 700, 'PHP', 1, 0);  -- ProductId=150
GO

-- =============================================================================
-- H. PRODUCTVARIANT
-- Every product gets one 'Default' variant (no size data in Excel).
-- StockQuantity: Units/Frames=5, Forks=8, Components=15, Consumables=25
-- ReorderThreshold: DEFAULT 5 (set by v7.1 patch — no override needed)
-- =============================================================================
INSERT INTO ProductVariant (ProductId, VariantName, SKU, AdditionalPrice, StockQuantity, IsActive) VALUES
    (1, 'Default', 'UNIT-001-DEF', 0.00, 5, 1),  -- VariantId=1
    (2, 'Default', 'UNIT-002-DEF', 0.00, 5, 1),  -- VariantId=2
    (3, 'Default', 'UNIT-003-DEF', 0.00, 5, 1),  -- VariantId=3
    (4, 'Default', 'UNIT-004-DEF', 0.00, 5, 1),  -- VariantId=4
    (5, 'Default', 'UNIT-005-DEF', 0.00, 5, 1),  -- VariantId=5
    (6, 'Default', 'UNIT-006-DEF', 0.00, 5, 1),  -- VariantId=6
    (7, 'Default', 'UNIT-007-DEF', 0.00, 5, 1),  -- VariantId=7
    (8, 'Default', 'UNIT-008-DEF', 0.00, 5, 1),  -- VariantId=8
    (9, 'Default', 'UNIT-009-DEF', 0.00, 5, 1),  -- VariantId=9
    (10, 'Default', 'FRAME-001-DEF', 0.00, 8, 1),  -- VariantId=10
    (11, 'Default', 'FRAME-002-DEF', 0.00, 8, 1),  -- VariantId=11
    (12, 'Default', 'FRAME-003-DEF', 0.00, 8, 1),  -- VariantId=12
    (13, 'Default', 'FRAME-004-DEF', 0.00, 8, 1),  -- VariantId=13
    (14, 'Default', 'FRAME-005-DEF', 0.00, 8, 1),  -- VariantId=14
    (15, 'Default', 'FRAME-006-DEF', 0.00, 8, 1),  -- VariantId=15
    (16, 'Default', 'FRAME-007-DEF', 0.00, 8, 1),  -- VariantId=16
    (17, 'Default', 'FRAME-008-DEF', 0.00, 8, 1),  -- VariantId=17
    (18, 'Default', 'FRAME-009-DEF', 0.00, 8, 1),  -- VariantId=18
    (19, 'Default', 'FRAME-010-DEF', 0.00, 8, 1),  -- VariantId=19
    (20, 'Default', 'FRAME-011-DEF', 0.00, 8, 1),  -- VariantId=20
    (21, 'Default', 'FRAME-012-DEF', 0.00, 8, 1),  -- VariantId=21
    (22, 'Default', 'FORK-001-DEF', 0.00, 8, 1),  -- VariantId=22
    (23, 'Default', 'FORK-002-DEF', 0.00, 8, 1),  -- VariantId=23
    (24, 'Default', 'FORK-003-DEF', 0.00, 8, 1),  -- VariantId=24
    (25, 'Default', 'FORK-004-DEF', 0.00, 8, 1),  -- VariantId=25
    (26, 'Default', 'FORK-005-DEF', 0.00, 8, 1),  -- VariantId=26
    (27, 'Default', 'FORK-006-DEF', 0.00, 8, 1),  -- VariantId=27
    (28, 'Default', 'FORK-007-DEF', 0.00, 8, 1),  -- VariantId=28
    (29, 'Default', 'FORK-008-DEF', 0.00, 8, 1),  -- VariantId=29
    (30, 'Default', 'FORK-009-DEF', 0.00, 8, 1),  -- VariantId=30
    (31, 'Default', 'FORK-010-DEF', 0.00, 8, 1),  -- VariantId=31
    (32, 'Default', 'FORK-011-DEF', 0.00, 8, 1),  -- VariantId=32
    (33, 'Default', 'FORK-012-DEF', 0.00, 8, 1),  -- VariantId=33
    (34, 'Default', 'HUB-001-DEF', 0.00, 10, 1),  -- VariantId=34
    (35, 'Default', 'HUB-002-DEF', 0.00, 10, 1),  -- VariantId=35
    (36, 'Default', 'HUB-003-DEF', 0.00, 10, 1),  -- VariantId=36
    (37, 'Default', 'HUB-004-DEF', 0.00, 10, 1),  -- VariantId=37
    (38, 'Default', 'HUB-005-DEF', 0.00, 10, 1),  -- VariantId=38
    (39, 'Default', 'HUB-006-DEF', 0.00, 10, 1),  -- VariantId=39
    (40, 'Default', 'HUB-007-DEF', 0.00, 10, 1),  -- VariantId=40
    (41, 'Default', 'HUB-008-DEF', 0.00, 10, 1),  -- VariantId=41
    (42, 'Default', 'HUB-009-DEF', 0.00, 10, 1),  -- VariantId=42
    (43, 'Default', 'HUB-010-DEF', 0.00, 10, 1),  -- VariantId=43
    (44, 'Default', 'UPGKIT-001-DEF', 0.00, 10, 1),  -- VariantId=44
    (45, 'Default', 'UPGKIT-002-DEF', 0.00, 10, 1),  -- VariantId=45
    (46, 'Default', 'UPGKIT-003-DEF', 0.00, 10, 1),  -- VariantId=46
    (47, 'Default', 'UPGKIT-004-DEF', 0.00, 10, 1),  -- VariantId=47
    (48, 'Default', 'UPGKIT-005-DEF', 0.00, 10, 1),  -- VariantId=48
    (49, 'Default', 'STEM-001-DEF', 0.00, 15, 1),  -- VariantId=49
    (50, 'Default', 'STEM-002-DEF', 0.00, 15, 1),  -- VariantId=50
    (51, 'Default', 'STEM-003-DEF', 0.00, 15, 1),  -- VariantId=51
    (52, 'Default', 'STEM-004-DEF', 0.00, 15, 1),  -- VariantId=52
    (53, 'Default', 'STEM-005-DEF', 0.00, 15, 1),  -- VariantId=53
    (54, 'Default', 'STEM-006-DEF', 0.00, 15, 1),  -- VariantId=54
    (55, 'Default', 'STEM-007-DEF', 0.00, 15, 1),  -- VariantId=55
    (56, 'Default', 'STEM-008-DEF', 0.00, 15, 1),  -- VariantId=56
    (57, 'Default', 'STEM-009-DEF', 0.00, 15, 1),  -- VariantId=57
    (58, 'Default', 'STEM-010-DEF', 0.00, 15, 1),  -- VariantId=58
    (59, 'Default', 'HBAR-001-DEF', 0.00, 15, 1),  -- VariantId=59
    (60, 'Default', 'HBAR-002-DEF', 0.00, 15, 1),  -- VariantId=60
    (61, 'Default', 'HBAR-003-DEF', 0.00, 15, 1),  -- VariantId=61
    (62, 'Default', 'HBAR-004-DEF', 0.00, 15, 1),  -- VariantId=62
    (63, 'Default', 'HBAR-005-DEF', 0.00, 15, 1),  -- VariantId=63
    (64, 'Default', 'HBAR-006-DEF', 0.00, 15, 1),  -- VariantId=64
    (65, 'Default', 'HBAR-007-DEF', 0.00, 15, 1),  -- VariantId=65
    (66, 'Default', 'HBAR-008-DEF', 0.00, 15, 1),  -- VariantId=66
    (67, 'Default', 'HBAR-009-DEF', 0.00, 15, 1),  -- VariantId=67
    (68, 'Default', 'HBAR-010-DEF', 0.00, 15, 1),  -- VariantId=68
    (69, 'Default', 'SADDLE-001-DEF', 0.00, 15, 1),  -- VariantId=69
    (70, 'Default', 'SADDLE-002-DEF', 0.00, 15, 1),  -- VariantId=70
    (71, 'Default', 'SADDLE-003-DEF', 0.00, 15, 1),  -- VariantId=71
    (72, 'Default', 'SADDLE-004-DEF', 0.00, 15, 1),  -- VariantId=72
    (73, 'Default', 'SADDLE-005-DEF', 0.00, 15, 1),  -- VariantId=73
    (74, 'Default', 'SADDLE-006-DEF', 0.00, 15, 1),  -- VariantId=74
    (75, 'Default', 'SADDLE-007-DEF', 0.00, 15, 1),  -- VariantId=75
    (76, 'Default', 'SADDLE-008-DEF', 0.00, 15, 1),  -- VariantId=76
    (77, 'Default', 'SADDLE-009-DEF', 0.00, 15, 1),  -- VariantId=77
    (78, 'Default', 'SADDLE-010-DEF', 0.00, 15, 1),  -- VariantId=78
    (79, 'Default', 'SADDLE-011-DEF', 0.00, 15, 1),  -- VariantId=79
    (80, 'Default', 'SADDLE-012-DEF', 0.00, 15, 1),  -- VariantId=80
    (81, 'Default', 'SADDLE-013-DEF', 0.00, 15, 1),  -- VariantId=81
    (82, 'Default', 'SADDLE-014-DEF', 0.00, 15, 1),  -- VariantId=82
    (83, 'Default', 'GRIP-001-DEF', 0.00, 20, 1),  -- VariantId=83
    (84, 'Default', 'GRIP-002-DEF', 0.00, 20, 1),  -- VariantId=84
    (85, 'Default', 'GRIP-003-DEF', 0.00, 20, 1),  -- VariantId=85
    (86, 'Default', 'GRIP-004-DEF', 0.00, 20, 1),  -- VariantId=86
    (87, 'Default', 'GRIP-005-DEF', 0.00, 20, 1),  -- VariantId=87
    (88, 'Default', 'GRIP-006-DEF', 0.00, 20, 1),  -- VariantId=88
    (89, 'Default', 'GRIP-007-DEF', 0.00, 20, 1),  -- VariantId=89
    (90, 'Default', 'GRIP-008-DEF', 0.00, 20, 1),  -- VariantId=90
    (91, 'Default', 'GRIP-009-DEF', 0.00, 20, 1),  -- VariantId=91
    (92, 'Default', 'GRIP-010-DEF', 0.00, 20, 1),  -- VariantId=92
    (93, 'Default', 'GRIP-011-DEF', 0.00, 20, 1),  -- VariantId=93
    (94, 'Default', 'PEDAL-001-DEF', 0.00, 15, 1),  -- VariantId=94
    (95, 'Default', 'PEDAL-002-DEF', 0.00, 15, 1),  -- VariantId=95
    (96, 'Default', 'PEDAL-003-DEF', 0.00, 15, 1),  -- VariantId=96
    (97, 'Default', 'PEDAL-004-DEF', 0.00, 15, 1),  -- VariantId=97
    (98, 'Default', 'PEDAL-005-DEF', 0.00, 15, 1),  -- VariantId=98
    (99, 'Default', 'PEDAL-006-DEF', 0.00, 15, 1),  -- VariantId=99
    (100, 'Default', 'PEDAL-007-DEF', 0.00, 15, 1),  -- VariantId=100
    (101, 'Default', 'PEDAL-008-DEF', 0.00, 15, 1),  -- VariantId=101
    (102, 'Default', 'PEDAL-009-DEF', 0.00, 15, 1),  -- VariantId=102
    (103, 'Default', 'PEDAL-010-DEF', 0.00, 15, 1),  -- VariantId=103
    (104, 'Default', 'PEDAL-011-DEF', 0.00, 15, 1),  -- VariantId=104
    (105, 'Default', 'RIM-001-DEF', 0.00, 10, 1),  -- VariantId=105
    (106, 'Default', 'RIM-002-DEF', 0.00, 10, 1),  -- VariantId=106
    (107, 'Default', 'RIM-003-DEF', 0.00, 10, 1),  -- VariantId=107
    (108, 'Default', 'RIM-004-DEF', 0.00, 10, 1),  -- VariantId=108
    (109, 'Default', 'RIM-005-DEF', 0.00, 10, 1),  -- VariantId=109
    (110, 'Default', 'RIM-006-DEF', 0.00, 10, 1),  -- VariantId=110
    (111, 'Default', 'RIM-007-DEF', 0.00, 10, 1),  -- VariantId=111
    (112, 'Default', 'RIM-008-DEF', 0.00, 10, 1),  -- VariantId=112
    (113, 'Default', 'RIM-009-DEF', 0.00, 10, 1),  -- VariantId=113
    (114, 'Default', 'RIM-010-DEF', 0.00, 10, 1),  -- VariantId=114
    (115, 'Default', 'RIM-011-DEF', 0.00, 10, 1),  -- VariantId=115
    (116, 'Default', 'RIM-012-DEF', 0.00, 10, 1),  -- VariantId=116
    (117, 'Default', 'RIM-013-DEF', 0.00, 10, 1),  -- VariantId=117
    (118, 'Default', 'RIM-014-DEF', 0.00, 10, 1),  -- VariantId=118
    (119, 'Default', 'TIRE-001-DEF', 0.00, 25, 1),  -- VariantId=119
    (120, 'Default', 'TIRE-002-DEF', 0.00, 25, 1),  -- VariantId=120
    (121, 'Default', 'TIRE-003-DEF', 0.00, 25, 1),  -- VariantId=121
    (122, 'Default', 'TIRE-004-DEF', 0.00, 25, 1),  -- VariantId=122
    (123, 'Default', 'TIRE-005-DEF', 0.00, 25, 1),  -- VariantId=123
    (124, 'Default', 'TIRE-006-DEF', 0.00, 25, 1),  -- VariantId=124
    (125, 'Default', 'TIRE-007-DEF', 0.00, 25, 1),  -- VariantId=125
    (126, 'Default', 'TIRE-008-DEF', 0.00, 25, 1),  -- VariantId=126
    (127, 'Default', 'TIRE-009-DEF', 0.00, 25, 1),  -- VariantId=127
    (128, 'Default', 'TIRE-010-DEF', 0.00, 25, 1),  -- VariantId=128
    (129, 'Default', 'TIRE-011-DEF', 0.00, 25, 1),  -- VariantId=129
    (130, 'Default', 'TIRE-012-DEF', 0.00, 25, 1),  -- VariantId=130
    (131, 'Default', 'TIRE-013-DEF', 0.00, 25, 1),  -- VariantId=131
    (132, 'Default', 'TIRE-014-DEF', 0.00, 25, 1),  -- VariantId=132
    (133, 'Default', 'TIRE-015-DEF', 0.00, 25, 1),  -- VariantId=133
    (134, 'Default', 'CHAIN-001-DEF', 0.00, 25, 1),  -- VariantId=134
    (135, 'Default', 'CHAIN-002-DEF', 0.00, 25, 1),  -- VariantId=135
    (136, 'Default', 'CHAIN-003-DEF', 0.00, 25, 1),  -- VariantId=136
    (137, 'Default', 'CHAIN-004-DEF', 0.00, 25, 1),  -- VariantId=137
    (138, 'Default', 'CHAIN-005-DEF', 0.00, 25, 1),  -- VariantId=138
    (139, 'Default', 'CHAIN-006-DEF', 0.00, 25, 1),  -- VariantId=139
    (140, 'Default', 'CHAIN-007-DEF', 0.00, 25, 1),  -- VariantId=140
    (141, 'Default', 'CHAIN-008-DEF', 0.00, 25, 1),  -- VariantId=141
    (142, 'Default', 'CHAIN-009-DEF', 0.00, 25, 1),  -- VariantId=142
    (143, 'Default', 'CHAIN-010-DEF', 0.00, 25, 1),  -- VariantId=143
    (144, 'Default', 'CHAIN-011-DEF', 0.00, 25, 1),  -- VariantId=144
    (145, 'Default', 'CHAIN-012-DEF', 0.00, 25, 1),  -- VariantId=145
    (146, 'Default', 'CHAIN-013-DEF', 0.00, 25, 1),  -- VariantId=146
    (147, 'Default', 'CHAIN-014-DEF', 0.00, 25, 1),  -- VariantId=147
    (148, 'Default', 'CHAIN-015-DEF', 0.00, 25, 1),  -- VariantId=148
    (149, 'Default', 'CHAIN-016-DEF', 0.00, 25, 1),  -- VariantId=149
    (150, 'Default', 'CHAIN-017-DEF', 0.00, 25, 1);  -- VariantId=150
GO

-- =============================================================================
-- I. PRODUCTIMAGE
-- One primary Full image per product. GCS paths are representative placeholders.
-- UploadedByUserId = 1 (admin).
-- =============================================================================
INSERT INTO ProductImage (ProductId, StorageBucket, StoragePath, ImageUrl, ImageType, IsPrimary, DisplayOrder, AltText, MimeType, UploadedByUserId) VALUES
    (1, 'taurus-product-images', 'products/unit/unit-001.webp',
        'https://storage.googleapis.com/taurus-product-images/products/unit/unit-001.webp',
        'Full', 1, 1, N'2021 Pinewood Climber CARBON 27.5', 'image/webp', 1),
    (2, 'taurus-product-images', 'products/unit/unit-002.webp',
        'https://storage.googleapis.com/taurus-product-images/products/unit/unit-002.webp',
        'Full', 1, 1, N'Cult Odyssey Hydro Brakes 27.5', 'image/webp', 1),
    (3, 'taurus-product-images', 'products/unit/unit-003.webp',
        'https://storage.googleapis.com/taurus-product-images/products/unit/unit-003.webp',
        'Full', 1, 1, N'Toseek Chester 700c Disc Brake ALLOY (2x9)', 'image/webp', 1),
    (4, 'taurus-product-images', 'products/unit/unit-004.webp',
        'https://storage.googleapis.com/taurus-product-images/products/unit/unit-004.webp',
        'Full', 1, 1, N'Ryder X3 27.5 2021 MT200 Hydro Brakes (3x8)', 'image/webp', 1),
    (5, 'taurus-product-images', 'products/unit/unit-005.webp',
        'https://storage.googleapis.com/taurus-product-images/products/unit/unit-005.webp',
        'Full', 1, 1, N'Pinewood Trident Flux', 'image/webp', 1),
    (6, 'taurus-product-images', 'products/unit/unit-006.webp',
        'https://storage.googleapis.com/taurus-product-images/products/unit/unit-006.webp',
        'Full', 1, 1, N'Garuda Rampage', 'image/webp', 1),
    (7, 'taurus-product-images', 'products/unit/unit-007.webp',
        'https://storage.googleapis.com/taurus-product-images/products/unit/unit-007.webp',
        'Full', 1, 1, N'Pinewood Challenger', 'image/webp', 1),
    (8, 'taurus-product-images', 'products/unit/unit-008.webp',
        'https://storage.googleapis.com/taurus-product-images/products/unit/unit-008.webp',
        'Full', 1, 1, N'Kespor Stork Feather CX 1.0 2022', 'image/webp', 1),
    (9, 'taurus-product-images', 'products/unit/unit-009.webp',
        'https://storage.googleapis.com/taurus-product-images/products/unit/unit-009.webp',
        'Full', 1, 1, N'Pinewood Lancer 1.0 2022 Gravel RX (2x9)', 'image/webp', 1),
    (10, 'taurus-product-images', 'products/frame/frame-001.webp',
        'https://storage.googleapis.com/taurus-product-images/products/frame/frame-001.webp',
        'Full', 1, 1, N'ELVES NANDOR', 'image/webp', 1),
    (11, 'taurus-product-images', 'products/frame/frame-002.webp',
        'https://storage.googleapis.com/taurus-product-images/products/frame/frame-002.webp',
        'Full', 1, 1, N'Ryder X2', 'image/webp', 1),
    (12, 'taurus-product-images', 'products/frame/frame-003.webp',
        'https://storage.googleapis.com/taurus-product-images/products/frame/frame-003.webp',
        'Full', 1, 1, N'MountainPeak Monster', 'image/webp', 1),
    (13, 'taurus-product-images', 'products/frame/frame-004.webp',
        'https://storage.googleapis.com/taurus-product-images/products/frame/frame-004.webp',
        'Full', 1, 1, N'Mountainpeak Everest 2', 'image/webp', 1),
    (14, 'taurus-product-images', 'products/frame/frame-005.webp',
        'https://storage.googleapis.com/taurus-product-images/products/frame/frame-005.webp',
        'Full', 1, 1, N'Specialized Stumpjumper', 'image/webp', 1),
    (15, 'taurus-product-images', 'products/frame/frame-006.webp',
        'https://storage.googleapis.com/taurus-product-images/products/frame/frame-006.webp',
        'Full', 1, 1, N'Weapon Stealth 29', 'image/webp', 1),
    (16, 'taurus-product-images', 'products/frame/frame-007.webp',
        'https://storage.googleapis.com/taurus-product-images/products/frame/frame-007.webp',
        'Full', 1, 1, N'Weapon Spartan 29', 'image/webp', 1),
    (17, 'taurus-product-images', 'products/frame/frame-008.webp',
        'https://storage.googleapis.com/taurus-product-images/products/frame/frame-008.webp',
        'Full', 1, 1, N'Saturn Calypso', 'image/webp', 1),
    (18, 'taurus-product-images', 'products/frame/frame-009.webp',
        'https://storage.googleapis.com/taurus-product-images/products/frame/frame-009.webp',
        'Full', 1, 1, N'Saturn Dione', 'image/webp', 1),
    (19, 'taurus-product-images', 'products/frame/frame-010.webp',
        'https://storage.googleapis.com/taurus-product-images/products/frame/frame-010.webp',
        'Full', 1, 1, N'Sagmit Chaser', 'image/webp', 1),
    (20, 'taurus-product-images', 'products/frame/frame-011.webp',
        'https://storage.googleapis.com/taurus-product-images/products/frame/frame-011.webp',
        'Full', 1, 1, N'COLE NX 27.5 TRI-FACTOR 2021', 'image/webp', 1),
    (21, 'taurus-product-images', 'products/frame/frame-012.webp',
        'https://storage.googleapis.com/taurus-product-images/products/frame/frame-012.webp',
        'Full', 1, 1, N'Speedone Floater BOOST', 'image/webp', 1),
    (22, 'taurus-product-images', 'products/fork/fork-001.webp',
        'https://storage.googleapis.com/taurus-product-images/products/fork/fork-001.webp',
        'Full', 1, 1, N'Aeroic AIR FORK', 'image/webp', 1),
    (23, 'taurus-product-images', 'products/fork/fork-002.webp',
        'https://storage.googleapis.com/taurus-product-images/products/fork/fork-002.webp',
        'Full', 1, 1, N'Weapon Cannon35 BOOST', 'image/webp', 1),
    (24, 'taurus-product-images', 'products/fork/fork-003.webp',
        'https://storage.googleapis.com/taurus-product-images/products/fork/fork-003.webp',
        'Full', 1, 1, N'Weapon Rifle', 'image/webp', 1),
    (25, 'taurus-product-images', 'products/fork/fork-004.webp',
        'https://storage.googleapis.com/taurus-product-images/products/fork/fork-004.webp',
        'Full', 1, 1, N'Weapon Rocket', 'image/webp', 1),
    (26, 'taurus-product-images', 'products/fork/fork-005.webp',
        'https://storage.googleapis.com/taurus-product-images/products/fork/fork-005.webp',
        'Full', 1, 1, N'Fork Weapon Tower', 'image/webp', 1),
    (27, 'taurus-product-images', 'products/fork/fork-006.webp',
        'https://storage.googleapis.com/taurus-product-images/products/fork/fork-006.webp',
        'Full', 1, 1, N'Speedone Soldier BOOST', 'image/webp', 1),
    (28, 'taurus-product-images', 'products/fork/fork-007.webp',
        'https://storage.googleapis.com/taurus-product-images/products/fork/fork-007.webp',
        'Full', 1, 1, N'Manitou Markhor BOOST', 'image/webp', 1),
    (29, 'taurus-product-images', 'products/fork/fork-008.webp',
        'https://storage.googleapis.com/taurus-product-images/products/fork/fork-008.webp',
        'Full', 1, 1, N'Manitou Machete Comp BOOST', 'image/webp', 1),
    (30, 'taurus-product-images', 'products/fork/fork-009.webp',
        'https://storage.googleapis.com/taurus-product-images/products/fork/fork-009.webp',
        'Full', 1, 1, N'Manitou Mattoc Comp Boost', 'image/webp', 1),
    (31, 'taurus-product-images', 'products/fork/fork-010.webp',
        'https://storage.googleapis.com/taurus-product-images/products/fork/fork-010.webp',
        'Full', 1, 1, N'SR Suntour Epixon Stealth', 'image/webp', 1),
    (32, 'taurus-product-images', 'products/fork/fork-011.webp',
        'https://storage.googleapis.com/taurus-product-images/products/fork/fork-011.webp',
        'Full', 1, 1, N'SR Suntour Raidon BOOST', 'image/webp', 1),
    (33, 'taurus-product-images', 'products/fork/fork-012.webp',
        'https://storage.googleapis.com/taurus-product-images/products/fork/fork-012.webp',
        'Full', 1, 1, N'SR Suntour XCR 32 BOOST', 'image/webp', 1),
    (34, 'taurus-product-images', 'products/hub/hub-001.webp',
        'https://storage.googleapis.com/taurus-product-images/products/hub/hub-001.webp',
        'Full', 1, 1, N'LDCNC 3.0', 'image/webp', 1),
    (35, 'taurus-product-images', 'products/hub/hub-002.webp',
        'https://storage.googleapis.com/taurus-product-images/products/hub/hub-002.webp',
        'Full', 1, 1, N'SpeedOne Soldier BOOST', 'image/webp', 1),
    (36, 'taurus-product-images', 'products/hub/hub-003.webp',
        'https://storage.googleapis.com/taurus-product-images/products/hub/hub-003.webp',
        'Full', 1, 1, N'Genova Big Dipper', 'image/webp', 1),
    (37, 'taurus-product-images', 'products/hub/hub-004.webp',
        'https://storage.googleapis.com/taurus-product-images/products/hub/hub-004.webp',
        'Full', 1, 1, N'Weapon Animal', 'image/webp', 1),
    (38, 'taurus-product-images', 'products/hub/hub-005.webp',
        'https://storage.googleapis.com/taurus-product-images/products/hub/hub-005.webp',
        'Full', 1, 1, N'Sagmit EVO3', 'image/webp', 1),
    (39, 'taurus-product-images', 'products/hub/hub-006.webp',
        'https://storage.googleapis.com/taurus-product-images/products/hub/hub-006.webp',
        'Full', 1, 1, N'Ragusa R100', 'image/webp', 1),
    (40, 'taurus-product-images', 'products/hub/hub-007.webp',
        'https://storage.googleapis.com/taurus-product-images/products/hub/hub-007.webp',
        'Full', 1, 1, N'SpeedOne Soldier', 'image/webp', 1),
    (41, 'taurus-product-images', 'products/hub/hub-008.webp',
        'https://storage.googleapis.com/taurus-product-images/products/hub/hub-008.webp',
        'Full', 1, 1, N'SpeedOne Pilot Carbon', 'image/webp', 1),
    (42, 'taurus-product-images', 'products/hub/hub-009.webp',
        'https://storage.googleapis.com/taurus-product-images/products/hub/hub-009.webp',
        'Full', 1, 1, N'SpeedOne Torpedo', 'image/webp', 1),
    (43, 'taurus-product-images', 'products/hub/hub-010.webp',
        'https://storage.googleapis.com/taurus-product-images/products/hub/hub-010.webp',
        'Full', 1, 1, N'Origin8 NON BOOST', 'image/webp', 1),
    (44, 'taurus-product-images', 'products/upgkit/upgkit-001.webp',
        'https://storage.googleapis.com/taurus-product-images/products/upgkit/upgkit-001.webp',
        'Full', 1, 1, N'Sagmit edison 12spd up kit', 'image/webp', 1),
    (45, 'taurus-product-images', 'products/upgkit/upgkit-002.webp',
        'https://storage.googleapis.com/taurus-product-images/products/upgkit/upgkit-002.webp',
        'Full', 1, 1, N'12spd LTwoo AX Upkit with IXF crank', 'image/webp', 1),
    (46, 'taurus-product-images', 'products/upgkit/upgkit-003.webp',
        'https://storage.googleapis.com/taurus-product-images/products/upgkit/upgkit-003.webp',
        'Full', 1, 1, N'Deore M6100 12spd Upkit with Weapon Cogs', 'image/webp', 1),
    (47, 'taurus-product-images', 'products/upgkit/upgkit-004.webp',
        'https://storage.googleapis.com/taurus-product-images/products/upgkit/upgkit-004.webp',
        'Full', 1, 1, N'ZEE M640 10spd Upkit', 'image/webp', 1),
    (48, 'taurus-product-images', 'products/upgkit/upgkit-005.webp',
        'https://storage.googleapis.com/taurus-product-images/products/upgkit/upgkit-005.webp',
        'Full', 1, 1, N'SRAM NX Eagle 12spd WITH BB', 'image/webp', 1),
    (49, 'taurus-product-images', 'products/stem/stem-001.webp',
        'https://storage.googleapis.com/taurus-product-images/products/stem/stem-001.webp',
        'Full', 1, 1, N'Handle Post Controltech ONE', 'image/webp', 1),
    (50, 'taurus-product-images', 'products/stem/stem-002.webp',
        'https://storage.googleapis.com/taurus-product-images/products/stem/stem-002.webp',
        'Full', 1, 1, N'HandlePost ControlTech', 'image/webp', 1),
    (51, 'taurus-product-images', 'products/stem/stem-003.webp',
        'https://storage.googleapis.com/taurus-product-images/products/stem/stem-003.webp',
        'Full', 1, 1, N'SeatPost ControlTech ONE', 'image/webp', 1),
    (52, 'taurus-product-images', 'products/stem/stem-004.webp',
        'https://storage.googleapis.com/taurus-product-images/products/stem/stem-004.webp',
        'Full', 1, 1, N'HandlePost ControlTech CLS', 'image/webp', 1),
    (53, 'taurus-product-images', 'products/stem/stem-005.webp',
        'https://storage.googleapis.com/taurus-product-images/products/stem/stem-005.webp',
        'Full', 1, 1, N'Weapon Fury stem', 'image/webp', 1),
    (54, 'taurus-product-images', 'products/stem/stem-006.webp',
        'https://storage.googleapis.com/taurus-product-images/products/stem/stem-006.webp',
        'Full', 1, 1, N'Weapon Ambush Stem', 'image/webp', 1),
    (55, 'taurus-product-images', 'products/stem/stem-007.webp',
        'https://storage.googleapis.com/taurus-product-images/products/stem/stem-007.webp',
        'Full', 1, 1, N'Weapon Savage Stem', 'image/webp', 1),
    (56, 'taurus-product-images', 'products/stem/stem-008.webp',
        'https://storage.googleapis.com/taurus-product-images/products/stem/stem-008.webp',
        'Full', 1, 1, N'Weapon Beast Stem', 'image/webp', 1),
    (57, 'taurus-product-images', 'products/stem/stem-009.webp',
        'https://storage.googleapis.com/taurus-product-images/products/stem/stem-009.webp',
        'Full', 1, 1, N'Weapon Animal Stem', 'image/webp', 1),
    (58, 'taurus-product-images', 'products/stem/stem-010.webp',
        'https://storage.googleapis.com/taurus-product-images/products/stem/stem-010.webp',
        'Full', 1, 1, N'Weapon Predator Stem', 'image/webp', 1),
    (59, 'taurus-product-images', 'products/hbar/hbar-001.webp',
        'https://storage.googleapis.com/taurus-product-images/products/hbar/hbar-001.webp',
        'Full', 1, 1, N'Handle Bar Pro LT', 'image/webp', 1),
    (60, 'taurus-product-images', 'products/hbar/hbar-002.webp',
        'https://storage.googleapis.com/taurus-product-images/products/hbar/hbar-002.webp',
        'Full', 1, 1, N'Handle Bar Pro Koryak', 'image/webp', 1),
    (61, 'taurus-product-images', 'products/hbar/hbar-003.webp',
        'https://storage.googleapis.com/taurus-product-images/products/hbar/hbar-003.webp',
        'Full', 1, 1, N'Drop Bar Gravel Pro LT', 'image/webp', 1),
    (62, 'taurus-product-images', 'products/hbar/hbar-004.webp',
        'https://storage.googleapis.com/taurus-product-images/products/hbar/hbar-004.webp',
        'Full', 1, 1, N'Drop Bar Gravel Pro Discovery', 'image/webp', 1),
    (63, 'taurus-product-images', 'products/hbar/hbar-005.webp',
        'https://storage.googleapis.com/taurus-product-images/products/hbar/hbar-005.webp',
        'Full', 1, 1, N'Answer ProTaper 20x20', 'image/webp', 1),
    (64, 'taurus-product-images', 'products/hbar/hbar-006.webp',
        'https://storage.googleapis.com/taurus-product-images/products/hbar/hbar-006.webp',
        'Full', 1, 1, N'Answer ProTaper', 'image/webp', 1),
    (65, 'taurus-product-images', 'products/hbar/hbar-007.webp',
        'https://storage.googleapis.com/taurus-product-images/products/hbar/hbar-007.webp',
        'Full', 1, 1, N'Answer ProTaper 7050 series Aluminum', 'image/webp', 1),
    (66, 'taurus-product-images', 'products/hbar/hbar-008.webp',
        'https://storage.googleapis.com/taurus-product-images/products/hbar/hbar-008.webp',
        'Full', 1, 1, N'Sagmit Brooklyn 3.0', 'image/webp', 1),
    (67, 'taurus-product-images', 'products/hbar/hbar-009.webp',
        'https://storage.googleapis.com/taurus-product-images/products/hbar/hbar-009.webp',
        'Full', 1, 1, N'Sagmit shadow', 'image/webp', 1),
    (68, 'taurus-product-images', 'products/hbar/hbar-010.webp',
        'https://storage.googleapis.com/taurus-product-images/products/hbar/hbar-010.webp',
        'Full', 1, 1, N'Sagmit Static', 'image/webp', 1),
    (69, 'taurus-product-images', 'products/saddle/saddle-001.webp',
        'https://storage.googleapis.com/taurus-product-images/products/saddle/saddle-001.webp',
        'Full', 1, 1, N'Sagmit RedClassic skull', 'image/webp', 1),
    (70, 'taurus-product-images', 'products/saddle/saddle-002.webp',
        'https://storage.googleapis.com/taurus-product-images/products/saddle/saddle-002.webp',
        'Full', 1, 1, N'Genova Jupiter', 'image/webp', 1),
    (71, 'taurus-product-images', 'products/saddle/saddle-003.webp',
        'https://storage.googleapis.com/taurus-product-images/products/saddle/saddle-003.webp',
        'Full', 1, 1, N'Giant w/o Hole', 'image/webp', 1),
    (72, 'taurus-product-images', 'products/saddle/saddle-004.webp',
        'https://storage.googleapis.com/taurus-product-images/products/saddle/saddle-004.webp',
        'Full', 1, 1, N'Giant with Hole', 'image/webp', 1),
    (73, 'taurus-product-images', 'products/saddle/saddle-005.webp',
        'https://storage.googleapis.com/taurus-product-images/products/saddle/saddle-005.webp',
        'Full', 1, 1, N'Ragusa R-100', 'image/webp', 1),
    (74, 'taurus-product-images', 'products/saddle/saddle-006.webp',
        'https://storage.googleapis.com/taurus-product-images/products/saddle/saddle-006.webp',
        'Full', 1, 1, N'Topgrade', 'image/webp', 1),
    (75, 'taurus-product-images', 'products/saddle/saddle-007.webp',
        'https://storage.googleapis.com/taurus-product-images/products/saddle/saddle-007.webp',
        'Full', 1, 1, N'Diamond', 'image/webp', 1),
    (76, 'taurus-product-images', 'products/saddle/saddle-008.webp',
        'https://storage.googleapis.com/taurus-product-images/products/saddle/saddle-008.webp',
        'Full', 1, 1, N'San Marco', 'image/webp', 1),
    (77, 'taurus-product-images', 'products/saddle/saddle-009.webp',
        'https://storage.googleapis.com/taurus-product-images/products/saddle/saddle-009.webp',
        'Full', 1, 1, N'Velo Plush', 'image/webp', 1),
    (78, 'taurus-product-images', 'products/saddle/saddle-010.webp',
        'https://storage.googleapis.com/taurus-product-images/products/saddle/saddle-010.webp',
        'Full', 1, 1, N'Sagmit Oilslick', 'image/webp', 1),
    (79, 'taurus-product-images', 'products/saddle/saddle-011.webp',
        'https://storage.googleapis.com/taurus-product-images/products/saddle/saddle-011.webp',
        'Full', 1, 1, N'Genova Mars', 'image/webp', 1),
    (80, 'taurus-product-images', 'products/saddle/saddle-012.webp',
        'https://storage.googleapis.com/taurus-product-images/products/saddle/saddle-012.webp',
        'Full', 1, 1, N'Specialized Black', 'image/webp', 1),
    (81, 'taurus-product-images', 'products/saddle/saddle-013.webp',
        'https://storage.googleapis.com/taurus-product-images/products/saddle/saddle-013.webp',
        'Full', 1, 1, N'Specialized M811', 'image/webp', 1),
    (82, 'taurus-product-images', 'products/saddle/saddle-014.webp',
        'https://storage.googleapis.com/taurus-product-images/products/saddle/saddle-014.webp',
        'Full', 1, 1, N'Easydo ES-01', 'image/webp', 1),
    (83, 'taurus-product-images', 'products/grip/grip-001.webp',
        'https://storage.googleapis.com/taurus-product-images/products/grip/grip-001.webp',
        'Full', 1, 1, N'Silicon Grip Seer 5mm', 'image/webp', 1),
    (84, 'taurus-product-images', 'products/grip/grip-002.webp',
        'https://storage.googleapis.com/taurus-product-images/products/grip/grip-002.webp',
        'Full', 1, 1, N'Seer Polymer Bartape', 'image/webp', 1),
    (85, 'taurus-product-images', 'products/grip/grip-003.webp',
        'https://storage.googleapis.com/taurus-product-images/products/grip/grip-003.webp',
        'Full', 1, 1, N'Silicon Grip Seer 7mm', 'image/webp', 1),
    (86, 'taurus-product-images', 'products/grip/grip-004.webp',
        'https://storage.googleapis.com/taurus-product-images/products/grip/grip-004.webp',
        'Full', 1, 1, N'Seer Art Racing Edition BarTape', 'image/webp', 1),
    (87, 'taurus-product-images', 'products/grip/grip-005.webp',
        'https://storage.googleapis.com/taurus-product-images/products/grip/grip-005.webp',
        'Full', 1, 1, N'Seer SILICON Hornet Bar Tape', 'image/webp', 1),
    (88, 'taurus-product-images', 'products/grip/grip-006.webp',
        'https://storage.googleapis.com/taurus-product-images/products/grip/grip-006.webp',
        'Full', 1, 1, N'Seer POLYMER Hornet Bar Tape', 'image/webp', 1),
    (89, 'taurus-product-images', 'products/grip/grip-007.webp',
        'https://storage.googleapis.com/taurus-product-images/products/grip/grip-007.webp',
        'Full', 1, 1, N'Seer Super Lite Bar tape', 'image/webp', 1),
    (90, 'taurus-product-images', 'products/grip/grip-008.webp',
        'https://storage.googleapis.com/taurus-product-images/products/grip/grip-008.webp',
        'Full', 1, 1, N'Handle Grip Attack Dual Lock-On', 'image/webp', 1),
    (91, 'taurus-product-images', 'products/grip/grip-009.webp',
        'https://storage.googleapis.com/taurus-product-images/products/grip/grip-009.webp',
        'Full', 1, 1, N'Weapon Race w/ Palm Rest', 'image/webp', 1),
    (92, 'taurus-product-images', 'products/grip/grip-010.webp',
        'https://storage.googleapis.com/taurus-product-images/products/grip/grip-010.webp',
        'Full', 1, 1, N'Weapon Rubix', 'image/webp', 1),
    (93, 'taurus-product-images', 'products/grip/grip-011.webp',
        'https://storage.googleapis.com/taurus-product-images/products/grip/grip-011.webp',
        'Full', 1, 1, N'Weapon Wave', 'image/webp', 1),
    (94, 'taurus-product-images', 'products/pedal/pedal-001.webp',
        'https://storage.googleapis.com/taurus-product-images/products/pedal/pedal-001.webp',
        'Full', 1, 1, N'Speedone Soldier', 'image/webp', 1),
    (95, 'taurus-product-images', 'products/pedal/pedal-002.webp',
        'https://storage.googleapis.com/taurus-product-images/products/pedal/pedal-002.webp',
        'Full', 1, 1, N'Speedone Pilot', 'image/webp', 1),
    (96, 'taurus-product-images', 'products/pedal/pedal-003.webp',
        'https://storage.googleapis.com/taurus-product-images/products/pedal/pedal-003.webp',
        'Full', 1, 1, N'Pedal Sagmit 610', 'image/webp', 1),
    (97, 'taurus-product-images', 'products/pedal/pedal-004.webp',
        'https://storage.googleapis.com/taurus-product-images/products/pedal/pedal-004.webp',
        'Full', 1, 1, N'Pedal Sagmit 614', 'image/webp', 1),
    (98, 'taurus-product-images', 'products/pedal/pedal-005.webp',
        'https://storage.googleapis.com/taurus-product-images/products/pedal/pedal-005.webp',
        'Full', 1, 1, N'Ragusa R700', 'image/webp', 1),
    (99, 'taurus-product-images', 'products/pedal/pedal-006.webp',
        'https://storage.googleapis.com/taurus-product-images/products/pedal/pedal-006.webp',
        'Full', 1, 1, N'Ragusa CNC Sealed Bearing 712', 'image/webp', 1),
    (100, 'taurus-product-images', 'products/pedal/pedal-007.webp',
        'https://storage.googleapis.com/taurus-product-images/products/pedal/pedal-007.webp',
        'Full', 1, 1, N'Sagmit 622 CNC Sealed Bearing', 'image/webp', 1),
    (101, 'taurus-product-images', 'products/pedal/pedal-008.webp',
        'https://storage.googleapis.com/taurus-product-images/products/pedal/pedal-008.webp',
        'Full', 1, 1, N'Sagmit 618 CNC Sealed Bearing', 'image/webp', 1),
    (102, 'taurus-product-images', 'products/pedal/pedal-009.webp',
        'https://storage.googleapis.com/taurus-product-images/products/pedal/pedal-009.webp',
        'Full', 1, 1, N'MKS JAPAN GR-9', 'image/webp', 1),
    (103, 'taurus-product-images', 'products/pedal/pedal-010.webp',
        'https://storage.googleapis.com/taurus-product-images/products/pedal/pedal-010.webp',
        'Full', 1, 1, N'MKS Sylvan Touring', 'image/webp', 1),
    (104, 'taurus-product-images', 'products/pedal/pedal-011.webp',
        'https://storage.googleapis.com/taurus-product-images/products/pedal/pedal-011.webp',
        'Full', 1, 1, N'MKS BM7', 'image/webp', 1),
    (105, 'taurus-product-images', 'products/rim/rim-001.webp',
        'https://storage.googleapis.com/taurus-product-images/products/rim/rim-001.webp',
        'Full', 1, 1, N'LDCNC RS300 700c Rim Brake', 'image/webp', 1),
    (106, 'taurus-product-images', 'products/rim/rim-002.webp',
        'https://storage.googleapis.com/taurus-product-images/products/rim/rim-002.webp',
        'Full', 1, 1, N'Sagmit Aero', 'image/webp', 1),
    (107, 'taurus-product-images', 'products/rim/rim-003.webp',
        'https://storage.googleapis.com/taurus-product-images/products/rim/rim-003.webp',
        'Full', 1, 1, N'Sagmit Legend M32', 'image/webp', 1),
    (108, 'taurus-product-images', 'products/rim/rim-004.webp',
        'https://storage.googleapis.com/taurus-product-images/products/rim/rim-004.webp',
        'Full', 1, 1, N'Kore Realm 4.2', 'image/webp', 1),
    (109, 'taurus-product-images', 'products/rim/rim-005.webp',
        'https://storage.googleapis.com/taurus-product-images/products/rim/rim-005.webp',
        'Full', 1, 1, N'WTB i21 Tubeless Ready', 'image/webp', 1),
    (110, 'taurus-product-images', 'products/rim/rim-006.webp',
        'https://storage.googleapis.com/taurus-product-images/products/rim/rim-006.webp',
        'Full', 1, 1, N'WTB i25 Tubeless Ready', 'image/webp', 1),
    (111, 'taurus-product-images', 'products/rim/rim-007.webp',
        'https://storage.googleapis.com/taurus-product-images/products/rim/rim-007.webp',
        'Full', 1, 1, N'American Classic Feldspar 290 Tubeless Ready', 'image/webp', 1),
    (112, 'taurus-product-images', 'products/rim/rim-008.webp',
        'https://storage.googleapis.com/taurus-product-images/products/rim/rim-008.webp',
        'Full', 1, 1, N'Jalco Wellington Tubeless Ready', 'image/webp', 1),
    (113, 'taurus-product-images', 'products/rim/rim-009.webp',
        'https://storage.googleapis.com/taurus-product-images/products/rim/rim-009.webp',
        'Full', 1, 1, N'Speedone Pilot', 'image/webp', 1),
    (114, 'taurus-product-images', 'products/rim/rim-010.webp',
        'https://storage.googleapis.com/taurus-product-images/products/rim/rim-010.webp',
        'Full', 1, 1, N'Speedone Bazooka', 'image/webp', 1),
    (115, 'taurus-product-images', 'products/rim/rim-011.webp',
        'https://storage.googleapis.com/taurus-product-images/products/rim/rim-011.webp',
        'Full', 1, 1, N'Speedone Soldier', 'image/webp', 1),
    (116, 'taurus-product-images', 'products/rim/rim-012.webp',
        'https://storage.googleapis.com/taurus-product-images/products/rim/rim-012.webp',
        'Full', 1, 1, N'Sagmit Safarri', 'image/webp', 1),
    (117, 'taurus-product-images', 'products/rim/rim-013.webp',
        'https://storage.googleapis.com/taurus-product-images/products/rim/rim-013.webp',
        'Full', 1, 1, N'Jalco Maranello', 'image/webp', 1),
    (118, 'taurus-product-images', 'products/rim/rim-014.webp',
        'https://storage.googleapis.com/taurus-product-images/products/rim/rim-014.webp',
        'Full', 1, 1, N'Kore Realm 2.4', 'image/webp', 1),
    (119, 'taurus-product-images', 'products/tire/tire-001.webp',
        'https://storage.googleapis.com/taurus-product-images/products/tire/tire-001.webp',
        'Full', 1, 1, N'Maxxis Ikon 27.5 x 2.20 Tanwall', 'image/webp', 1),
    (120, 'taurus-product-images', 'products/tire/tire-002.webp',
        'https://storage.googleapis.com/taurus-product-images/products/tire/tire-002.webp',
        'Full', 1, 1, N'Maxxis Ardent 29 x 2.25 Tanwall', 'image/webp', 1),
    (121, 'taurus-product-images', 'products/tire/tire-003.webp',
        'https://storage.googleapis.com/taurus-product-images/products/tire/tire-003.webp',
        'Full', 1, 1, N'Maxxis Ikon 29 x 2.20', 'image/webp', 1),
    (122, 'taurus-product-images', 'products/tire/tire-004.webp',
        'https://storage.googleapis.com/taurus-product-images/products/tire/tire-004.webp',
        'Full', 1, 1, N'Maxxis Crossmark II 27.5 x 2.25', 'image/webp', 1),
    (123, 'taurus-product-images', 'products/tire/tire-005.webp',
        'https://storage.googleapis.com/taurus-product-images/products/tire/tire-005.webp',
        'Full', 1, 1, N'Maxxis Ikon 29 x 2.20 Tanwall', 'image/webp', 1),
    (124, 'taurus-product-images', 'products/tire/tire-006.webp',
        'https://storage.googleapis.com/taurus-product-images/products/tire/tire-006.webp',
        'Full', 1, 1, N'Maxxis Rekon Race 27.5 x 2.25', 'image/webp', 1),
    (125, 'taurus-product-images', 'products/tire/tire-007.webp',
        'https://storage.googleapis.com/taurus-product-images/products/tire/tire-007.webp',
        'Full', 1, 1, N'Maxxis Ardent 27.5 x 2.25', 'image/webp', 1),
    (126, 'taurus-product-images', 'products/tire/tire-008.webp',
        'https://storage.googleapis.com/taurus-product-images/products/tire/tire-008.webp',
        'Full', 1, 1, N'Maxxis Ardent Race 27.5 x 2.20', 'image/webp', 1),
    (127, 'taurus-product-images', 'products/tire/tire-009.webp',
        'https://storage.googleapis.com/taurus-product-images/products/tire/tire-009.webp',
        'Full', 1, 1, N'MAXXIS TUBE', 'image/webp', 1),
    (128, 'taurus-product-images', 'products/tire/tire-010.webp',
        'https://storage.googleapis.com/taurus-product-images/products/tire/tire-010.webp',
        'Full', 1, 1, N'SAGMIT TUBE', 'image/webp', 1),
    (129, 'taurus-product-images', 'products/tire/tire-011.webp',
        'https://storage.googleapis.com/taurus-product-images/products/tire/tire-011.webp',
        'Full', 1, 1, N'SIZE 20 LEO TUBE', 'image/webp', 1),
    (130, 'taurus-product-images', 'products/tire/tire-012.webp',
        'https://storage.googleapis.com/taurus-product-images/products/tire/tire-012.webp',
        'Full', 1, 1, N'SIZE 26 LEO TUBE', 'image/webp', 1),
    (131, 'taurus-product-images', 'products/tire/tire-013.webp',
        'https://storage.googleapis.com/taurus-product-images/products/tire/tire-013.webp',
        'Full', 1, 1, N'SIZE 20 TIRE', 'image/webp', 1),
    (132, 'taurus-product-images', 'products/tire/tire-014.webp',
        'https://storage.googleapis.com/taurus-product-images/products/tire/tire-014.webp',
        'Full', 1, 1, N'SIZE 16 TIRE', 'image/webp', 1),
    (133, 'taurus-product-images', 'products/tire/tire-015.webp',
        'https://storage.googleapis.com/taurus-product-images/products/tire/tire-015.webp',
        'Full', 1, 1, N'SIZE 16 TUBE', 'image/webp', 1),
    (134, 'taurus-product-images', 'products/chain/chain-001.webp',
        'https://storage.googleapis.com/taurus-product-images/products/chain/chain-001.webp',
        'Full', 1, 1, N'8 SPEED SUMC CP With Missing link', 'image/webp', 1),
    (135, 'taurus-product-images', 'products/chain/chain-002.webp',
        'https://storage.googleapis.com/taurus-product-images/products/chain/chain-002.webp',
        'Full', 1, 1, N'9 SPEED SUMC CP With Missing link', 'image/webp', 1),
    (136, 'taurus-product-images', 'products/chain/chain-003.webp',
        'https://storage.googleapis.com/taurus-product-images/products/chain/chain-003.webp',
        'Full', 1, 1, N'10 SPEED SUMC CP With Missing link', 'image/webp', 1),
    (137, 'taurus-product-images', 'products/chain/chain-004.webp',
        'https://storage.googleapis.com/taurus-product-images/products/chain/chain-004.webp',
        'Full', 1, 1, N'11 SPEED SUMC CP With Missing link', 'image/webp', 1),
    (138, 'taurus-product-images', 'products/chain/chain-005.webp',
        'https://storage.googleapis.com/taurus-product-images/products/chain/chain-005.webp',
        'Full', 1, 1, N'12 SPEED SUMC CP With Missing link', 'image/webp', 1),
    (139, 'taurus-product-images', 'products/chain/chain-006.webp',
        'https://storage.googleapis.com/taurus-product-images/products/chain/chain-006.webp',
        'Full', 1, 1, N'8 SPEED SUMC Oilslick 116Links With Missing link', 'image/webp', 1),
    (140, 'taurus-product-images', 'products/chain/chain-007.webp',
        'https://storage.googleapis.com/taurus-product-images/products/chain/chain-007.webp',
        'Full', 1, 1, N'10 SPEED SUMC Oilslick 116Links With Missing link', 'image/webp', 1),
    (141, 'taurus-product-images', 'products/chain/chain-008.webp',
        'https://storage.googleapis.com/taurus-product-images/products/chain/chain-008.webp',
        'Full', 1, 1, N'11 SPEED SUMC Oilslick 116Links With Missing link', 'image/webp', 1),
    (142, 'taurus-product-images', 'products/chain/chain-009.webp',
        'https://storage.googleapis.com/taurus-product-images/products/chain/chain-009.webp',
        'Full', 1, 1, N'12SPD KMC Chain X Series GREY', 'image/webp', 1),
    (143, 'taurus-product-images', 'products/chain/chain-010.webp',
        'https://storage.googleapis.com/taurus-product-images/products/chain/chain-010.webp',
        'Full', 1, 1, N'11SPD KMC Chain X Series GREY', 'image/webp', 1),
    (144, 'taurus-product-images', 'products/chain/chain-011.webp',
        'https://storage.googleapis.com/taurus-product-images/products/chain/chain-011.webp',
        'Full', 1, 1, N'10SPD KMC Chain X Series GREY', 'image/webp', 1),
    (145, 'taurus-product-images', 'products/chain/chain-012.webp',
        'https://storage.googleapis.com/taurus-product-images/products/chain/chain-012.webp',
        'Full', 1, 1, N'9SPD KMC Chain X Series GOLD', 'image/webp', 1),
    (146, 'taurus-product-images', 'products/chain/chain-013.webp',
        'https://storage.googleapis.com/taurus-product-images/products/chain/chain-013.webp',
        'Full', 1, 1, N'10SPD KMC Chain X Series GOLD', 'image/webp', 1),
    (147, 'taurus-product-images', 'products/chain/chain-014.webp',
        'https://storage.googleapis.com/taurus-product-images/products/chain/chain-014.webp',
        'Full', 1, 1, N'12SPD KMC Chain X Series GOLD', 'image/webp', 1),
    (148, 'taurus-product-images', 'products/chain/chain-015.webp',
        'https://storage.googleapis.com/taurus-product-images/products/chain/chain-015.webp',
        'Full', 1, 1, N'CT-1HX 116links Heavy Duty for FIXIE', 'image/webp', 1),
    (149, 'taurus-product-images', 'products/chain/chain-016.webp',
        'https://storage.googleapis.com/taurus-product-images/products/chain/chain-016.webp',
        'Full', 1, 1, N'GT-10 126links', 'image/webp', 1),
    (150, 'taurus-product-images', 'products/chain/chain-017.webp',
        'https://storage.googleapis.com/taurus-product-images/products/chain/chain-017.webp',
        'Full', 1, 1, N'GT-11 126links', 'image/webp', 1);
GO

-- =============================================================================
-- J. INVENTORYLOG — opening Purchase stock entries for all variants
-- ChangedByUserId = 1 (admin), ChangeType = 'Purchase' (opening balance)
-- =============================================================================
INSERT INTO InventoryLog (ProductId, ProductVariantId, ChangeQuantity, ChangeType, ChangedByUserId, Notes) VALUES
    (1, 1, 5, 'Purchase', 1, N'Opening stock — 2021 Pinewood Climber CARBON 27.5'),
    (2, 2, 5, 'Purchase', 1, N'Opening stock — Cult Odyssey Hydro Brakes 27.5'),
    (3, 3, 5, 'Purchase', 1, N'Opening stock — Toseek Chester 700c Disc Brake ALLOY (2x9)'),
    (4, 4, 5, 'Purchase', 1, N'Opening stock — Ryder X3 27.5 2021 MT200 Hydro Brakes (3x8)'),
    (5, 5, 5, 'Purchase', 1, N'Opening stock — Pinewood Trident Flux'),
    (6, 6, 5, 'Purchase', 1, N'Opening stock — Garuda Rampage'),
    (7, 7, 5, 'Purchase', 1, N'Opening stock — Pinewood Challenger'),
    (8, 8, 5, 'Purchase', 1, N'Opening stock — Kespor Stork Feather CX 1.0 2022'),
    (9, 9, 5, 'Purchase', 1, N'Opening stock — Pinewood Lancer 1.0 2022 Gravel RX (2x9)'),
    (10, 10, 8, 'Purchase', 1, N'Opening stock — ELVES NANDOR'),
    (11, 11, 8, 'Purchase', 1, N'Opening stock — Ryder X2'),
    (12, 12, 8, 'Purchase', 1, N'Opening stock — MountainPeak Monster'),
    (13, 13, 8, 'Purchase', 1, N'Opening stock — Mountainpeak Everest 2'),
    (14, 14, 8, 'Purchase', 1, N'Opening stock — Specialized Stumpjumper'),
    (15, 15, 8, 'Purchase', 1, N'Opening stock — Weapon Stealth 29'),
    (16, 16, 8, 'Purchase', 1, N'Opening stock — Weapon Spartan 29'),
    (17, 17, 8, 'Purchase', 1, N'Opening stock — Saturn Calypso'),
    (18, 18, 8, 'Purchase', 1, N'Opening stock — Saturn Dione'),
    (19, 19, 8, 'Purchase', 1, N'Opening stock — Sagmit Chaser'),
    (20, 20, 8, 'Purchase', 1, N'Opening stock — COLE NX 27.5 TRI-FACTOR 2021'),
    (21, 21, 8, 'Purchase', 1, N'Opening stock — Speedone Floater BOOST'),
    (22, 22, 8, 'Purchase', 1, N'Opening stock — Aeroic AIR FORK'),
    (23, 23, 8, 'Purchase', 1, N'Opening stock — Weapon Cannon35 BOOST'),
    (24, 24, 8, 'Purchase', 1, N'Opening stock — Weapon Rifle'),
    (25, 25, 8, 'Purchase', 1, N'Opening stock — Weapon Rocket'),
    (26, 26, 8, 'Purchase', 1, N'Opening stock — Fork Weapon Tower'),
    (27, 27, 8, 'Purchase', 1, N'Opening stock — Speedone Soldier BOOST'),
    (28, 28, 8, 'Purchase', 1, N'Opening stock — Manitou Markhor BOOST'),
    (29, 29, 8, 'Purchase', 1, N'Opening stock — Manitou Machete Comp BOOST'),
    (30, 30, 8, 'Purchase', 1, N'Opening stock — Manitou Mattoc Comp Boost'),
    (31, 31, 8, 'Purchase', 1, N'Opening stock — SR Suntour Epixon Stealth'),
    (32, 32, 8, 'Purchase', 1, N'Opening stock — SR Suntour Raidon BOOST'),
    (33, 33, 8, 'Purchase', 1, N'Opening stock — SR Suntour XCR 32 BOOST'),
    (34, 34, 10, 'Purchase', 1, N'Opening stock — LDCNC 3.0'),
    (35, 35, 10, 'Purchase', 1, N'Opening stock — SpeedOne Soldier BOOST'),
    (36, 36, 10, 'Purchase', 1, N'Opening stock — Genova Big Dipper'),
    (37, 37, 10, 'Purchase', 1, N'Opening stock — Weapon Animal'),
    (38, 38, 10, 'Purchase', 1, N'Opening stock — Sagmit EVO3'),
    (39, 39, 10, 'Purchase', 1, N'Opening stock — Ragusa R100'),
    (40, 40, 10, 'Purchase', 1, N'Opening stock — SpeedOne Soldier'),
    (41, 41, 10, 'Purchase', 1, N'Opening stock — SpeedOne Pilot Carbon'),
    (42, 42, 10, 'Purchase', 1, N'Opening stock — SpeedOne Torpedo'),
    (43, 43, 10, 'Purchase', 1, N'Opening stock — Origin8 NON BOOST'),
    (44, 44, 10, 'Purchase', 1, N'Opening stock — Sagmit edison 12spd up kit'),
    (45, 45, 10, 'Purchase', 1, N'Opening stock — 12spd LTwoo AX Upkit with IXF crank'),
    (46, 46, 10, 'Purchase', 1, N'Opening stock — Deore M6100 12spd Upkit with Weapon Cogs'),
    (47, 47, 10, 'Purchase', 1, N'Opening stock — ZEE M640 10spd Upkit'),
    (48, 48, 10, 'Purchase', 1, N'Opening stock — SRAM NX Eagle 12spd WITH BB'),
    (49, 49, 15, 'Purchase', 1, N'Opening stock — Handle Post Controltech ONE'),
    (50, 50, 15, 'Purchase', 1, N'Opening stock — HandlePost ControlTech'),
    (51, 51, 15, 'Purchase', 1, N'Opening stock — SeatPost ControlTech ONE'),
    (52, 52, 15, 'Purchase', 1, N'Opening stock — HandlePost ControlTech CLS'),
    (53, 53, 15, 'Purchase', 1, N'Opening stock — Weapon Fury stem'),
    (54, 54, 15, 'Purchase', 1, N'Opening stock — Weapon Ambush Stem'),
    (55, 55, 15, 'Purchase', 1, N'Opening stock — Weapon Savage Stem'),
    (56, 56, 15, 'Purchase', 1, N'Opening stock — Weapon Beast Stem'),
    (57, 57, 15, 'Purchase', 1, N'Opening stock — Weapon Animal Stem'),
    (58, 58, 15, 'Purchase', 1, N'Opening stock — Weapon Predator Stem'),
    (59, 59, 15, 'Purchase', 1, N'Opening stock — Handle Bar Pro LT'),
    (60, 60, 15, 'Purchase', 1, N'Opening stock — Handle Bar Pro Koryak'),
    (61, 61, 15, 'Purchase', 1, N'Opening stock — Drop Bar Gravel Pro LT'),
    (62, 62, 15, 'Purchase', 1, N'Opening stock — Drop Bar Gravel Pro Discovery'),
    (63, 63, 15, 'Purchase', 1, N'Opening stock — Answer ProTaper 20x20'),
    (64, 64, 15, 'Purchase', 1, N'Opening stock — Answer ProTaper'),
    (65, 65, 15, 'Purchase', 1, N'Opening stock — Answer ProTaper 7050 series Aluminum'),
    (66, 66, 15, 'Purchase', 1, N'Opening stock — Sagmit Brooklyn 3.0'),
    (67, 67, 15, 'Purchase', 1, N'Opening stock — Sagmit shadow'),
    (68, 68, 15, 'Purchase', 1, N'Opening stock — Sagmit Static'),
    (69, 69, 15, 'Purchase', 1, N'Opening stock — Sagmit RedClassic skull'),
    (70, 70, 15, 'Purchase', 1, N'Opening stock — Genova Jupiter'),
    (71, 71, 15, 'Purchase', 1, N'Opening stock — Giant w/o Hole'),
    (72, 72, 15, 'Purchase', 1, N'Opening stock — Giant with Hole'),
    (73, 73, 15, 'Purchase', 1, N'Opening stock — Ragusa R-100'),
    (74, 74, 15, 'Purchase', 1, N'Opening stock — Topgrade'),
    (75, 75, 15, 'Purchase', 1, N'Opening stock — Diamond'),
    (76, 76, 15, 'Purchase', 1, N'Opening stock — San Marco'),
    (77, 77, 15, 'Purchase', 1, N'Opening stock — Velo Plush'),
    (78, 78, 15, 'Purchase', 1, N'Opening stock — Sagmit Oilslick'),
    (79, 79, 15, 'Purchase', 1, N'Opening stock — Genova Mars'),
    (80, 80, 15, 'Purchase', 1, N'Opening stock — Specialized Black'),
    (81, 81, 15, 'Purchase', 1, N'Opening stock — Specialized M811'),
    (82, 82, 15, 'Purchase', 1, N'Opening stock — Easydo ES-01'),
    (83, 83, 20, 'Purchase', 1, N'Opening stock — Silicon Grip Seer 5mm'),
    (84, 84, 20, 'Purchase', 1, N'Opening stock — Seer Polymer Bartape'),
    (85, 85, 20, 'Purchase', 1, N'Opening stock — Silicon Grip Seer 7mm'),
    (86, 86, 20, 'Purchase', 1, N'Opening stock — Seer Art Racing Edition BarTape'),
    (87, 87, 20, 'Purchase', 1, N'Opening stock — Seer SILICON Hornet Bar Tape'),
    (88, 88, 20, 'Purchase', 1, N'Opening stock — Seer POLYMER Hornet Bar Tape'),
    (89, 89, 20, 'Purchase', 1, N'Opening stock — Seer Super Lite Bar tape'),
    (90, 90, 20, 'Purchase', 1, N'Opening stock — Handle Grip Attack Dual Lock-On'),
    (91, 91, 20, 'Purchase', 1, N'Opening stock — Weapon Race w/ Palm Rest'),
    (92, 92, 20, 'Purchase', 1, N'Opening stock — Weapon Rubix'),
    (93, 93, 20, 'Purchase', 1, N'Opening stock — Weapon Wave'),
    (94, 94, 15, 'Purchase', 1, N'Opening stock — Speedone Soldier'),
    (95, 95, 15, 'Purchase', 1, N'Opening stock — Speedone Pilot'),
    (96, 96, 15, 'Purchase', 1, N'Opening stock — Pedal Sagmit 610'),
    (97, 97, 15, 'Purchase', 1, N'Opening stock — Pedal Sagmit 614'),
    (98, 98, 15, 'Purchase', 1, N'Opening stock — Ragusa R700'),
    (99, 99, 15, 'Purchase', 1, N'Opening stock — Ragusa CNC Sealed Bearing 712'),
    (100, 100, 15, 'Purchase', 1, N'Opening stock — Sagmit 622 CNC Sealed Bearing'),
    (101, 101, 15, 'Purchase', 1, N'Opening stock — Sagmit 618 CNC Sealed Bearing'),
    (102, 102, 15, 'Purchase', 1, N'Opening stock — MKS JAPAN GR-9'),
    (103, 103, 15, 'Purchase', 1, N'Opening stock — MKS Sylvan Touring'),
    (104, 104, 15, 'Purchase', 1, N'Opening stock — MKS BM7'),
    (105, 105, 10, 'Purchase', 1, N'Opening stock — LDCNC RS300 700c Rim Brake'),
    (106, 106, 10, 'Purchase', 1, N'Opening stock — Sagmit Aero'),
    (107, 107, 10, 'Purchase', 1, N'Opening stock — Sagmit Legend M32'),
    (108, 108, 10, 'Purchase', 1, N'Opening stock — Kore Realm 4.2'),
    (109, 109, 10, 'Purchase', 1, N'Opening stock — WTB i21 Tubeless Ready'),
    (110, 110, 10, 'Purchase', 1, N'Opening stock — WTB i25 Tubeless Ready'),
    (111, 111, 10, 'Purchase', 1, N'Opening stock — American Classic Feldspar 290 Tubeless Ready'),
    (112, 112, 10, 'Purchase', 1, N'Opening stock — Jalco Wellington Tubeless Ready'),
    (113, 113, 10, 'Purchase', 1, N'Opening stock — Speedone Pilot'),
    (114, 114, 10, 'Purchase', 1, N'Opening stock — Speedone Bazooka'),
    (115, 115, 10, 'Purchase', 1, N'Opening stock — Speedone Soldier'),
    (116, 116, 10, 'Purchase', 1, N'Opening stock — Sagmit Safarri'),
    (117, 117, 10, 'Purchase', 1, N'Opening stock — Jalco Maranello'),
    (118, 118, 10, 'Purchase', 1, N'Opening stock — Kore Realm 2.4'),
    (119, 119, 25, 'Purchase', 1, N'Opening stock — Maxxis Ikon 27.5 x 2.20 Tanwall'),
    (120, 120, 25, 'Purchase', 1, N'Opening stock — Maxxis Ardent 29 x 2.25 Tanwall'),
    (121, 121, 25, 'Purchase', 1, N'Opening stock — Maxxis Ikon 29 x 2.20'),
    (122, 122, 25, 'Purchase', 1, N'Opening stock — Maxxis Crossmark II 27.5 x 2.25'),
    (123, 123, 25, 'Purchase', 1, N'Opening stock — Maxxis Ikon 29 x 2.20 Tanwall'),
    (124, 124, 25, 'Purchase', 1, N'Opening stock — Maxxis Rekon Race 27.5 x 2.25'),
    (125, 125, 25, 'Purchase', 1, N'Opening stock — Maxxis Ardent 27.5 x 2.25'),
    (126, 126, 25, 'Purchase', 1, N'Opening stock — Maxxis Ardent Race 27.5 x 2.20'),
    (127, 127, 25, 'Purchase', 1, N'Opening stock — MAXXIS TUBE'),
    (128, 128, 25, 'Purchase', 1, N'Opening stock — SAGMIT TUBE'),
    (129, 129, 25, 'Purchase', 1, N'Opening stock — SIZE 20 LEO TUBE'),
    (130, 130, 25, 'Purchase', 1, N'Opening stock — SIZE 26 LEO TUBE'),
    (131, 131, 25, 'Purchase', 1, N'Opening stock — SIZE 20 TIRE'),
    (132, 132, 25, 'Purchase', 1, N'Opening stock — SIZE 16 TIRE'),
    (133, 133, 25, 'Purchase', 1, N'Opening stock — SIZE 16 TUBE'),
    (134, 134, 25, 'Purchase', 1, N'Opening stock — 8 SPEED SUMC CP With Missing link'),
    (135, 135, 25, 'Purchase', 1, N'Opening stock — 9 SPEED SUMC CP With Missing link'),
    (136, 136, 25, 'Purchase', 1, N'Opening stock — 10 SPEED SUMC CP With Missing link'),
    (137, 137, 25, 'Purchase', 1, N'Opening stock — 11 SPEED SUMC CP With Missing link'),
    (138, 138, 25, 'Purchase', 1, N'Opening stock — 12 SPEED SUMC CP With Missing link'),
    (139, 139, 25, 'Purchase', 1, N'Opening stock — 8 SPEED SUMC Oilslick 116Links With Missing link'),
    (140, 140, 25, 'Purchase', 1, N'Opening stock — 10 SPEED SUMC Oilslick 116Links With Missing link'),
    (141, 141, 25, 'Purchase', 1, N'Opening stock — 11 SPEED SUMC Oilslick 116Links With Missing link'),
    (142, 142, 25, 'Purchase', 1, N'Opening stock — 12SPD KMC Chain X Series GREY'),
    (143, 143, 25, 'Purchase', 1, N'Opening stock — 11SPD KMC Chain X Series GREY'),
    (144, 144, 25, 'Purchase', 1, N'Opening stock — 10SPD KMC Chain X Series GREY'),
    (145, 145, 25, 'Purchase', 1, N'Opening stock — 9SPD KMC Chain X Series GOLD'),
    (146, 146, 25, 'Purchase', 1, N'Opening stock — 10SPD KMC Chain X Series GOLD'),
    (147, 147, 25, 'Purchase', 1, N'Opening stock — 12SPD KMC Chain X Series GOLD'),
    (148, 148, 25, 'Purchase', 1, N'Opening stock — CT-1HX 116links Heavy Duty for FIXIE'),
    (149, 149, 25, 'Purchase', 1, N'Opening stock — GT-10 126links'),
    (150, 150, 25, 'Purchase', 1, N'Opening stock — GT-11 126links');
GO

-- =============================================================================
-- SUMMARY
-- =============================================================================
PRINT '==============================================';
PRINT 'Taurus_seed_v8.2.sql — Completed Successfully!';
PRINT '==============================================';
PRINT 'Schema: v8.1 + v8.2 audit fixes';
PRINT '----------------------------------------------';
PRINT 'Role           :  5 rows';
PRINT 'User           :  5 rows (admin, cashier, 2 customers, walk-in)';
PRINT 'Address        :  4 rows';
PRINT 'UserRole       :  4 rows';
PRINT 'Category       : 14 rows (1 parent + 13 from Excel)';
PRINT 'Brand          : 43 rows (inferred from product names)';
PRINT 'Product        : 150 rows (all from PARTS-PRICES.xlsx)';
PRINT 'ProductVariant : 150 rows (one Default per product)';
PRINT 'ProductImage   : 150 rows (one primary image per product)';
PRINT 'InventoryLog   : 150 rows (opening Purchase entries)';
PRINT '----------------------------------------------';
PRINT 'All 150 products from PARTS-PRICES.xlsx included.';
PRINT 'No extra products invented.';
PRINT 'Reconciled for schema 8.1 + 8.2 (audit fixes).';
PRINT '==============================================';
GO