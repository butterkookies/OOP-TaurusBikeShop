-- =============================================================================
-- TAURUS BIKE SHOP - SEED DATA
-- Compatible with: Microsoft SQL Server
-- Description: Enriched product data sourced from PARTS-PRICES.xlsx
--              and supplemented with web research for missing attributes.
-- Currency: Philippine Peso (PHP)
-- =============================================================================

USE TaurusBikeShop;
GO

SET NOCOUNT ON;
SET IDENTITY_INSERT Categories ON;
GO

-- =============================================================================
-- SEED: Categories
-- =============================================================================
INSERT INTO Categories (CategoryID, CategoryCode, CategoryName, Description, DisplayOrder)
VALUES
    ( 1, 'UNIT',     'Complete Bike Units',  'Fully assembled bicycles ready to ride, including MTB, gravel, and CX builds.',                                  1),
    ( 2, 'FRAME',    'Frames',               'Bicycle framesets including alloy, carbon, and steel options for MTB and road/gravel disciplines.',              2),
    ( 3, 'FORK',     'Forks',                'Suspension and rigid forks for mountain bikes; includes coil, air, and carbon options.',                         3),
    ( 4, 'HUB',      'Hubs',                 'Front and rear wheel hubs in standard QR and Boost axle standards; alloy and carbon options.',                  4),
    ( 5, 'UPGKIT',   'Upgrade Kits',         'Drivetrain upgrade kits bundling shifters, derailleurs, cassettes, and chains for 9–12 speed configurations.',  5),
    ( 6, 'STEM',     'Stems & Seatposts',    'Handlebar stems and seatposts; alloy and composite options with varying rise and reach.',                        6),
    ( 7, 'HBAR',     'Handlebars',           'Flat bars, riser bars, and drop bars for MTB, gravel, and road bicycles.',                                      7),
    ( 8, 'SADDLE',   'Saddles',              'Bicycle seats in various widths and padding levels for trail, road, and enduro riding.',                         8),
    ( 9, 'GRIP',     'Grips & Bar Tape',     'Handlebar grips, lock-on grips, and bar tape for MTB, gravel, and road bikes.',                                 9),
    (10, 'PEDAL',    'Pedals',               'Platform, touring, and clipless pedals in alloy and composite materials.',                                      10),
    (11, 'RIM',      'Rims & Wheelsets',     'Single rims and complete tubeless-ready wheelsets for 27.5", 29", and 700c standards.',                        11),
    (12, 'TIRE',     'Tires & Tubes',        'Mountain bike, gravel, and BMX tires plus inner tubes across multiple sizes.',                                  12),
    (13, 'CHAIN',    'Chains',               'Bicycle drive chains from 8-speed to 12-speed in standard and oil-slick finishes.',                             13);
GO

SET IDENTITY_INSERT Categories OFF;
GO

SET IDENTITY_INSERT Brands ON;
GO

-- =============================================================================
-- SEED: Brands
-- =============================================================================
INSERT INTO Brands (BrandID, BrandName, Country, Website, Description)
VALUES
    ( 1, 'Pinewood',        'Philippines',  'https://www.facebook.com/PINEWOODBIKE/',          'Philippine bicycle brand offering quality MTB, gravel, and road bikes at competitive prices.'),
    ( 2, 'Cult',            'USA',          'https://cultcrew.com/',                            'American BMX and MTB brand known for durable components and complete builds.'),
    ( 3, 'Toseek',          'China',        'https://www.toseek.com/',                          'Chinese OEM bicycle component manufacturer specializing in alloy and carbon parts.'),
    ( 4, 'Ryder',           'Philippines',  NULL,                                               'Philippine bicycle brand offering affordable alloy MTB framesets and complete bikes.'),
    ( 5, 'Garuda',          'Philippines',  NULL,                                               'Philippine bicycle brand known for trail and enduro MTB builds.'),
    ( 6, 'Kespor',          'Taiwan',       'https://www.kespor.com/',                          'Taiwanese bicycle brand producing gravel and CX bikes with high-grade components.'),
    ( 7, 'Elves',           'China',        'https://www.elvesbike.com/',                       'Chinese high-performance carbon MTB frame manufacturer.'),
    ( 8, 'MountainPeak',    'Philippines',  NULL,                                               'Philippine MTB frameset brand offering alloy frames for trail and enduro riders.'),
    ( 9, 'Specialized',     'USA',          'https://www.specialized.com/',                     'Global bicycle brand producing high-performance bikes and components for all disciplines.'),
    (10, 'Weapon',          'Taiwan',       NULL,                                               'Taiwanese bicycle component brand producing frames, forks, stems, and handlebars.'),
    (11, 'Saturn',          'Philippines',  NULL,                                               'Philippine MTB frameset brand offering alloy and carbon frame options.'),
    (12, 'Sagmit',          'Philippines',  NULL,                                               'Philippine bicycle component brand offering affordable hubs, saddles, rims, and drivetrain parts.'),
    (13, 'COLE',            'Taiwan',       'https://www.cole-bikes.com/',                      'Taiwanese premium MTB frame manufacturer; known for carbon and alloy trail geometry.'),
    (14, 'Speedone',        'Philippines',  NULL,                                               'Philippine bicycle component brand offering hubs, rims, and pedals for MTB and road use.'),
    (15, 'Genova',          'Philippines',  NULL,                                               'Philippine bicycle component brand offering hubs, saddles, and accessories.'),
    (16, 'Origin8',         'USA',          'https://www.origin8.com/',                         'American bicycle component brand offering hubs, handlebars, and BMX/urban parts.'),
    (17, 'LDCNC',           'Taiwan',       NULL,                                               'Taiwanese precision-machined hub and wheelset manufacturer.'),
    (18, 'LTwoo',           'China',        'https://www.ltwoo.cn/',                            'Chinese drivetrain component manufacturer offering Shimano-compatible groupsets.'),
    (19, 'SRAM',            'USA',          'https://www.sram.com/',                            'American drivetrain brand producing Eagle, XX1, X01, and NX groupsets for MTB.'),
    (20, 'Shimano',         'Japan',        'https://www.shimano.com/',                         'Japanese component giant producing Deore, SLX, XT, XTR, and ZEE MTB groupsets.'),
    (21, 'Controltech',     'Taiwan',       'https://www.controltechbikes.com/',                'Taiwanese OEM component manufacturer producing stems, seatposts, and handlebars.'),
    (22, 'Answer',          'USA',          'https://www.answerproducts.com/',                  'American bicycle component brand known for ProTaper handlebars and BMX/MTB products.'),
    (23, 'Ragusa',          'Philippines',  NULL,                                               'Philippine bicycle component brand offering saddles, cranks, and pedals.'),
    (24, 'Seer',            'Philippines',  NULL,                                               'Philippine bicycle component brand producing bar tape, grips, and accessories.'),
    (25, 'Attack',          'Philippines',  NULL,                                               'Philippine bicycle accessories brand offering grips and handlebar components.'),
    (26, 'MKS',             'Japan',        'https://www.mkspedal.com/',                        'Japanese pedal manufacturer renowned for touring and platform pedals since 1943.'),
    (27, 'WTB',             'USA',          'https://www.wtb.com/',                             'American rim, tire, and saddle brand; pioneer of tubeless-ready technology.'),
    (28, 'American Classic', 'USA',         'https://www.americanclassic.com/',                 'American wheel and hub manufacturer producing lightweight tubeless-ready wheelsets.'),
    (29, 'Jalco',           'Taiwan',       NULL,                                               'Taiwanese rim manufacturer producing double-wall alloy and tubeless-ready rims.'),
    (30, 'Kore',            'Taiwan',       NULL,                                               'Taiwanese bicycle component brand offering rims, stems, and handlebars.'),
    (31, 'Maxxis',          'Taiwan',       'https://www.maxxis.com/',                          'Taiwanese tire manufacturer; global leader in MTB, road, and BMX tires.'),
    (32, 'Leo',             'Taiwan',       NULL,                                               'Taiwanese inner tube manufacturer offering tubes for BMX, kids, and MTB applications.'),
    (33, 'SUMC',            'China',        NULL,                                               'Chinese chain manufacturer producing standard and oil-slick finished drivetrain chains.'),
    (34, 'KMC',             'Taiwan',       'https://www.kmcchain.com/',                        'Taiwanese premium chain manufacturer offering X-Series chains for 8- to 12-speed drivetrains.'),
    (35, 'CT',              'Taiwan',       NULL,                                               'Taiwanese chain manufacturer specializing in fixie and single-speed heavy-duty chains.'),
    (36, 'GT',              'Taiwan',       NULL,                                               'Taiwanese chain manufacturer offering 10- and 11-speed standard chains.'),
    (37, 'Manitou',         'USA',          'https://www.manitoumtb.com/',                      'American fork manufacturer producing trail and enduro suspension forks.'),
    (38, 'SR Suntour',      'Taiwan',       'https://www.srsuntour-cycling.com/',               'Taiwanese suspension fork manufacturer; widely used in entry-to-mid-level MTBs.'),
    (39, 'Aeroic',          'Philippines',  NULL,                                               'Philippine bicycle component brand offering forks and cranksets.'),
    (40, 'San Marco',       'Italy',        'https://www.selle-sanmarco.it/',                   'Italian premium saddle manufacturer producing road and MTB saddles since 1935.'),
    (41, 'Velo',            'Taiwan',       'https://www.velo.com.tw/',                         'Taiwanese OEM saddle manufacturer supplying saddles to major bicycle brands worldwide.'),
    (42, 'Easydo',          'China',        NULL,                                               'Chinese bicycle accessory brand offering saddles and ergonomic cycling components.'),
    (43, 'Giant',           'Taiwan',       'https://www.giant-bicycles.com/',                  'World''s largest bicycle manufacturer producing bikes and OEM components for all disciplines.');
GO

SET IDENTITY_INSERT Brands OFF;
GO

SET IDENTITY_INSERT Products ON;
GO

-- =============================================================================
-- SEED: Products — Category 1: Complete Bike Units
-- =============================================================================
INSERT INTO Products (ProductID, CategoryID, BrandID, SKU, ProductName, ShortDescription, FullDescription,
                      Material, Color, WheelSize, SpeedCompatibility, BoostCompatible, TubelessReady,
                      AxleStandard, FrameMaterial, SuspensionTravel, BrakeType, AdditionalSpecs,
                      Price, StockQuantity)
VALUES
( 1,  1,  1, 'UNIT-PIN-001', '2021 Pinewood Climber CARBON 27.5',
    'Full-carbon XC mountain bike with hydraulic disc brakes and 27.5" wheels.',
    'The Pinewood Climber CARBON 27.5 is a high-performance cross-country mountain bike featuring a full T800 carbon frameset, premium 12-speed drivetrain, and hydraulic disc brakes. Ideal for competitive trail and XC riders demanding lightweight performance.',
    'Carbon Fiber T800', 'Matte Black / Red', '27.5"', '12-speed', 1, 1,
    'Thru-Axle 12x148mm Rear / 12x100mm Front', 'Carbon Fiber T800',
    'N/A (Hardtail)', 'Hydraulic Disc',
    'Geometry: XC race; Headset: Tapered 44-56mm sealed bearing; Crankset: Carbon Hollowtech; Bottom Bracket: BSA threaded; Seat Tube: 30.9mm',
    40000.00, 5),

( 2,  1,  2, 'UNIT-CUL-001', 'Cult Odyssey Hydro Brakes 27.5',
    'Trail MTB complete build with hydraulic brakes and 27.5" wheels.',
    'The Cult Odyssey is a capable trail mountain bike built around a sturdy alloy frame and equipped with hydraulic disc brakes for confident stopping power. A great choice for intermediate riders seeking trail performance on a moderate budget.',
    'Aluminum Alloy 6061 T6', 'Black', '27.5"', '9-speed', 0, 0,
    'Quick Release 9x135mm Rear / 9x100mm Front', 'Aluminum Alloy 6061',
    '100mm Travel', 'Hydraulic Disc',
    'Drivetrain: Shimano Altus 9-speed; Fork: SR Suntour XCT; Saddle: Generic MTB; Tires: 27.5x2.1',
    14500.00, 8),

( 3,  1,  3, 'UNIT-TOS-001', 'Toseek Chester 700c Disc Brake ALLOY (2x9)',
    'Alloy gravel/road bike with 2x9 drivetrain and disc brakes.',
    'The Toseek Chester is a versatile 700c alloy gravel-road bike featuring a 2x9-speed Shimano Claris drivetrain and mechanical disc brakes. Suitable for paved roads and light gravel, offering an agile ride with commuter-friendly geometry.',
    'Aluminum Alloy 6061 T6', 'Silver / Black', '700c', '2x9-speed', 0, 0,
    'Quick Release 9x135mm Rear / 9x100mm Front', 'Aluminum Alloy 6061',
    'N/A (Rigid)', 'Mechanical Disc',
    'Drivetrain: Shimano 2x9; Crank: 50/34T; Cassette: 11-32T; Rim: Double wall 700c 32H; Tire: 700x32c',
    11000.00, 6),

( 4,  1,  4, 'UNIT-RYD-001', 'Ryder X3 27.5 2021 MT200 Hydro Brakes (3x8)',
    'Budget MTB with 3x8 drivetrain, MT200 hydraulic brakes, and 27.5" wheels.',
    'The Ryder X3 is an entry-level hardtail mountain bike that punches above its price with Shimano MT200 hydraulic disc brakes and a 3x8-speed drivetrain. The lightweight 6061 alloy frame and 27.5" wheels deliver a nimble ride on city streets and light trails.',
    'Aluminum Alloy 6061 T6', 'Blue / Black', '27.5"', '3x8-speed', 0, 0,
    'Quick Release 9x135mm Rear / 9x100mm Front', 'Aluminum Alloy 6061',
    '100mm Travel', 'Hydraulic Disc (Shimano MT200)',
    'Fork: SR Suntour XCT Lockout; Crankset: 3x Alloy; Saddle: Generic; Tires: 27.5x2.1',
    12500.00, 10),

( 5,  1,  1, 'UNIT-PIN-002', 'Pinewood Trident Flux',
    '1x9 alloy hardtail MTB with hydraulic brakes — trail-ready on a budget.',
    'The Pinewood Trident Flux is a single-chainring hardtail designed for trail riders who want simplicity and reliability. Built around a 6061 T6 alloy frame with tapered headtube and internal cable routing, it delivers a clean aesthetic with a capable 9-speed drivetrain and hydraulic disc brakes.',
    'Aluminum Alloy 6061 T6', 'Matte Grey', '29"', '1x9-speed', 0, 0,
    'Quick Release 10x135mm Rear / 9x100mm Front', 'Aluminum Alloy 6061 T6',
    '100mm Travel', 'Hydraulic Disc (Karasawa)',
    'Fork: SR Suntour XCM30 Lockout; Shifter: Shimano Alivio 1x9; RD: Shimano Altus; Crankset: 34T Hollowtech',
    17500.00, 7),

( 6,  1,  5, 'UNIT-GAR-001', 'Garuda Rampage',
    'Philippine trail MTB offering aggressive geometry and hydraulic braking.',
    'The Garuda Rampage is a Philippine-made trail MTB designed for aggressive riders who demand a sturdy alloy frame and reliable hydraulic disc brakes. Its progressive geometry suits fast trail descents and technical terrain.',
    'Aluminum Alloy 6061 T6', 'Black / Neon Orange', '27.5"', '1x10-speed', 0, 0,
    'Quick Release 9x135mm Rear / 9x100mm Front', 'Aluminum Alloy 6061 T6',
    '120mm Travel', 'Hydraulic Disc',
    'Fork: Air suspension 120mm; Drivetrain: 1x10; Tires: 27.5x2.25; Rim: Double wall CNC',
    14500.00, 5),

( 7,  1,  1, 'UNIT-PIN-003', 'Pinewood Challenger',
    'Value hardtail MTB with Shimano Altus 2x9 and hydraulic disc brakes.',
    'The Pinewood Challenger is a well-equipped trail hardtail featuring a tapered double-butted 6061 alloy frame, internal cable routing, and a Shimano MT200 hydraulic brakeset paired to a Shimano Altus 2x9 drivetrain. A great all-around bike for beginner-to-intermediate riders.',
    'Aluminum Alloy 6061 T6', 'Matte Black', '27.5" / 29"', '2x9-speed', 0, 0,
    'Quick Release 9x135mm Rear / 9x100mm Front', 'Aluminum Alloy 6061 T6',
    '100mm Travel', 'Hydraulic Disc (Shimano MT200)',
    'Fork: Taiwan Uding Coil Lockout; Shifter: Shimano Altus M2000 2x9; Crankset: Alloy 170mm Hollowtech; Saddle: Sagmit',
    15500.00, 9),

( 8,  1,  6, 'UNIT-KES-001', 'Kespor Stork Feather CX 1.0 2022',
    'Premium cyclocross / gravel bike with carbon fork and high-end groupset.',
    'The Kespor Stork Feather CX 1.0 2022 is a race-oriented cyclocross and gravel bike featuring a lightweight alloy frame, carbon fork, and a premium drivetrain. Designed for fast-paced gravel racing and CX competition, it balances stiffness, comfort, and aerodynamics.',
    'Aluminum Alloy 6069 T6 Triple-Butted', 'Gloss White / Red', '700c', '2x11-speed', 0, 1,
    'Thru-Axle 12x142mm Rear / 12x100mm Front', 'Aluminum Alloy 6069 T6',
    'N/A (Rigid)', 'Hydraulic Disc (Flat Mount)',
    'Fork: Carbon flat-mount; Shifter: Shimano 105 STI 2x11; Cassette: 11-34T; Saddle: Ergonomic CX; Tires: 700x38c tubeless',
    55000.00, 3),

( 9,  1,  1, 'UNIT-PIN-004', 'Pinewood Lancer 1.0 2022 Gravel RX (2x9)',
    'Alloy gravel bike with 2x9 drivetrain, disc brakes, and versatile geometry.',
    'The Pinewood Lancer 1.0 Gravel RX is designed for mixed-terrain adventures, featuring a lightweight 6061 T6 triple-butted alloy frame, tapered headtube, and internal cable routing. Equipped with a 2x9-speed Sensah/Shimano drivetrain and reliable disc brakes, it handles gravel roads and light trails with ease.',
    'Aluminum Alloy 6061 T6 Triple-Butted', 'Matte Green', '700c', '2x9-speed', 0, 0,
    'Quick Release 9x135mm Rear / 9x100mm Front', 'Aluminum Alloy 6061 T6',
    'N/A (Rigid)', 'Mechanical Disc',
    'Fork: Alloy Tapered Gravel; Crank: Prowheel RPL Gravel 34/46T 170mm; Cassette: MTB 9s 11-40T; Hubs: Novatec sealed bearing',
    15500.00, 6);
GO

-- =============================================================================
-- SEED: Products — Category 2: Frames
-- =============================================================================
INSERT INTO Products (ProductID, CategoryID, BrandID, SKU, ProductName, ShortDescription, FullDescription,
                      Material, Color, WheelSize, SpeedCompatibility, BoostCompatible, TubelessReady,
                      AxleStandard, FrameMaterial, SuspensionTravel, BrakeType, AdditionalSpecs,
                      Price, StockQuantity)
VALUES
(10,  2,  7, 'FRAME-ELV-001', 'Elves Nandor Carbon MTB Frame',
    'High-end full carbon MTB frame with aggressive trail geometry.',
    'The Elves Nandor is a premium full-carbon MTB frameset engineered for aggressive trail and enduro riding. The monocoque carbon construction provides excellent stiffness-to-weight ratio, while the slack geometry and generous stack inspire confidence on technical descents.',
    'Carbon Fiber T800', 'Raw Carbon / Black', '27.5" / 29"', '12-speed', 1, 1,
    'Thru-Axle 12x148mm', 'Carbon Fiber T800',
    'N/A (Hardtail)', 'Post-Mount Disc',
    'Headtube: Tapered 44-56mm; Seat Tube: 30.9mm; BB: BSA threaded; Cable: Internal routing; Sizes: S/M/L',
    32000.00, 4),

(11,  2,  4, 'FRAME-RYD-001', 'Ryder X2 Alloy MTB Frame',
    'Affordable 27.5" alloy hardtail frame with disc brake mounts.',
    'The Ryder X2 is a budget-friendly 6061 alloy hardtail frame offering trail-oriented geometry with disc brake compatibility. Suitable for beginner to intermediate MTB riders building their first trail bike.',
    'Aluminum Alloy 6061 T6', 'Matte Black', '27.5"', '8-9-speed', 0, 0,
    'Quick Release 9x135mm', 'Aluminum Alloy 6061 T6',
    'N/A (Hardtail)', 'Post-Mount Disc',
    'Headtube: 1-1/8" straight; Seat tube: 27.2mm; BB: BSA threaded; Sizes: S/M/L',
    4500.00, 12),

(12,  2,  8, 'FRAME-MNP-001', 'MountainPeak Monster 27.5 Frame',
    'Aggressive-geometry 27.5" alloy trail/enduro frame.',
    'The MountainPeak Monster is a trail-enduro hardtail frame with a slack head angle and low BB drop for a stable, aggressive riding feel. Built from 6061 T6 alloy with a post-mount disc brake standard.',
    'Aluminum Alloy 6061 T6', 'Gloss Black', '27.5"', '9-11-speed', 1, 0,
    'Thru-Axle 12x148mm', 'Aluminum Alloy 6061 T6',
    'N/A (Hardtail)', 'Post-Mount Disc',
    'Headtube: Tapered 44-56mm; BB: BSA; Internal cable routing; Boost 148mm rear spacing',
    6500.00, 8),

(13,  2,  8, 'FRAME-MNP-002', 'MountainPeak Everest 2 Alloy Frame',
    'Trail alloy MTB frame with tapered headtube and Boost spacing.',
    'The MountainPeak Everest 2 is a capable trail hardtail offering Boost axle compatibility and a tapered headtube for added fork compatibility and stiffness. A versatile platform for trail and light enduro builds.',
    'Aluminum Alloy 6061 T6', 'Matte Grey / Red', '27.5" / 29"', '10-12-speed', 1, 0,
    'Thru-Axle 12x148mm', 'Aluminum Alloy 6061 T6',
    'N/A (Hardtail)', 'Post-Mount Disc',
    'Headtube: Tapered 44-56mm; BB: BSA; Sizes: S/M/L/XL',
    7800.00, 6),

(14,  2,  9, 'FRAME-SPZ-001', 'Specialized Stumpjumper Alloy Frame',
    'Iconic trail MTB frameset with FACT alloy construction and full-carbon fork.',
    'The Specialized Stumpjumper is an iconic trail MTB frame featuring FACT 10m alloy construction, a full-carbon fork, and progressive trail geometry. Optimized for 27.5" or 29" wheels with 130-140mm suspension travel.',
    'FACT 10m Aluminum Alloy', 'Gloss Black', '27.5" / 29"', '11-12-speed', 1, 1,
    'Thru-Axle 12x148mm', 'FACT 10m Aluminum Alloy',
    'N/A (Hardtail / Short-Travel FS)', 'Post-Mount Disc (Flat Mount compatible)',
    'Fork: Full carbon tapered; Sizes: S1–S6; BB: T47 threaded; Internal routing for Di2/dropper',
    22000.00, 3),

(15,  2, 10, 'FRAME-WPN-001', 'Weapon Stealth 29 Alloy Frame',
    'Lightweight 29er alloy frame with modern trail geometry.',
    'The Weapon Stealth 29 is a modern-geometry alloy hardtail for 29" wheels. Features internal cable routing and a tapered headtube, ideal for cross-country and light trail riding.',
    'Aluminum Alloy 6061 T6', 'Stealth Matte Black', '29"', '11-12-speed', 1, 0,
    'Thru-Axle 12x148mm', 'Aluminum Alloy 6061 T6',
    'N/A (Hardtail)', 'Post-Mount Disc',
    'Headtube: Tapered 44-56mm; BB: BSA; Cable: Internal; Sizes: S/M/L',
    6500.00, 10),

(16,  2, 10, 'FRAME-WPN-002', 'Weapon Spartan 29 Alloy Frame',
    'Premium 29er trail hardtail with Boost spacing and tapered headtube.',
    'The Weapon Spartan 29 is a step up from the Stealth, featuring Boost 148mm rear spacing and a more progressive trail geometry. Pairs well with air-suspension forks for a lively trail ride.',
    'Aluminum Alloy 6061 T6', 'Matte Army Green', '29"', '11-12-speed', 1, 0,
    'Thru-Axle 12x148mm (Boost)', 'Aluminum Alloy 6061 T6',
    'N/A (Hardtail)', 'Post-Mount Disc',
    'Headtube: Tapered; BB: BSA threaded; Internal cable routing; Boost rear axle',
    8500.00, 7),

(17,  2, 11, 'FRAME-SAT-001', 'Saturn Calypso Alloy MTB Frame',
    'Philippine-made alloy trail frame with smooth welds and modern sizing.',
    'The Saturn Calypso is a locally designed alloy hardtail frame built for trail performance. Smooth-weld construction and modern geometry deliver confident handling on Philippine trail systems.',
    'Aluminum Alloy 6061 T6', 'Gloss Pearl White', '27.5"', '9-11-speed', 0, 0,
    'Quick Release 9x135mm', 'Aluminum Alloy 6061 T6',
    'N/A (Hardtail)', 'Post-Mount Disc',
    'Headtube: Tapered 44-56mm; BB: BSA; Sizes: S/M/L',
    8800.00, 5),

(18,  2, 11, 'FRAME-SAT-002', 'Saturn Dione Alloy MTB Frame',
    'Trail-oriented alloy hardtail with aggressive geometry for Philippine terrain.',
    'The Saturn Dione offers a more aggressive geometry than the Calypso, with a slacker head angle for faster trail riding. Built from 6061 alloy with quality welds and disc brake compatibility.',
    'Aluminum Alloy 6061 T6', 'Matte Black / Gold', '29"', '10-12-speed', 1, 0,
    'Thru-Axle 12x148mm', 'Aluminum Alloy 6061 T6',
    'N/A (Hardtail)', 'Post-Mount Disc',
    'Headtube: Tapered 44-56mm; Boost 148mm rear; Sizes: S/M/L',
    7000.00, 6),

(19,  2, 12, 'FRAME-SAG-001', 'Sagmit Chaser Alloy MTB Frame',
    'Affordable Philippine alloy hardtail frame for trail building.',
    'The Sagmit Chaser is a value-oriented 6061 alloy MTB frame offering good stiffness and a trail-friendly geometry. An ideal platform for a budget trail build.',
    'Aluminum Alloy 6061 T6', 'Black', '27.5"', '9-10-speed', 0, 0,
    'Quick Release 9x135mm', 'Aluminum Alloy 6061 T6',
    'N/A (Hardtail)', 'Post-Mount Disc',
    'Headtube: Semi-integrated; BB: BSA; Sizes: S/M/L',
    7500.00, 9),

(20,  2, 13, 'FRAME-COL-001', 'COLE NX 27.5 TRI-FACTOR 2021 Frame',
    'Taiwanese trail alloy frame with Tri-Factor tube shaping for added stiffness.',
    'The COLE NX 27.5 Tri-Factor uses COLE''s patented triangular tube-shaping process, resulting in a frameset that is lighter and stiffer than conventional alloy frames. Suitable for trail and XC builds with Boost axle compatibility.',
    'Aluminum Alloy 6069 T6 Tri-Factor', 'Matte Black', '27.5"', '11-12-speed', 1, 1,
    'Thru-Axle 12x148mm', 'Aluminum Alloy 6069 T6',
    'N/A (Hardtail)', 'Post-Mount Disc',
    'Headtube: Tapered 1-1/8 to 1.5"; BB: BSA; Sizes: 15/17/19"; Internal cable routing',
    6000.00, 6),

(21,  2, 14, 'FRAME-SPD-001', 'Speedone Floater BOOST Alloy Frame',
    'Boost-compatible alloy MTB frame with modern trail geometry.',
    'The Speedone Floater BOOST is a Philippine-brand alloy hardtail frame built around Boost 148mm rear spacing and a tapered headtube. An excellent base for a capable mid-range trail build.',
    'Aluminum Alloy 6061 T6', 'Matte Black', '27.5" / 29"', '11-12-speed', 1, 0,
    'Thru-Axle 12x148mm (Boost)', 'Aluminum Alloy 6061 T6',
    'N/A (Hardtail)', 'Post-Mount Disc',
    'Headtube: Tapered 44-56mm; BB: BSA; Sizes: S/M/L; Internal routing',
    8500.00, 5);
GO

-- =============================================================================
-- SEED: Products — Category 3: Forks
-- =============================================================================
INSERT INTO Products (ProductID, CategoryID, BrandID, SKU, ProductName, ShortDescription, FullDescription,
                      Material, Color, WheelSize, SpeedCompatibility, BoostCompatible, TubelessReady,
                      AxleStandard, FrameMaterial, SuspensionTravel, BrakeType, AdditionalSpecs,
                      Price, StockQuantity)
VALUES
(22,  3, 39, 'FORK-AER-001', 'Aeroic Air Fork 100mm',
    'Air-sprung 100mm travel fork with lockout — lightweight trail performance.',
    'The Aeroic Air Fork provides 100mm of smooth air-sprung travel, making it suitable for XC and trail MTB builds. Equipped with a remote lockout for efficiency on climbs and full damping on descents.',
    'Aluminum Alloy 6061 / Steel Stanchion', 'Black', '27.5" / 29"', 'Universal', 0, 0,
    'Quick Release 9x100mm', NULL, '100mm', 'Post-Mount Disc',
    'Stanchion: 32mm alloy; Steerer: 1-1/8" straight; Crown-to-axle: 465mm; Weight: ~1,950g',
    2900.00, 8),

(23,  3, 10, 'FORK-WPN-001', 'Weapon Cannon35 BOOST Air Fork 130mm',
    'Boost-compatible 35mm stanchion air fork for aggressive trail riding.',
    'The Weapon Cannon35 BOOST features oversized 35mm stanchions for a significant stiffness upgrade over conventional 32mm forks. 130mm of travel and Boost axle compatibility make it ideal for trail and enduro builds.',
    'Aluminum Alloy / Magnesium Lowers', 'Gloss Black', '27.5" / 29"', 'Universal', 1, 0,
    'Thru-Axle 15x110mm (Boost)', NULL, '130mm', 'Post-Mount Disc',
    'Stanchion: 35mm; Steerer: Tapered 1-1/8 to 1.5"; Rebound: External adjustment; Weight: ~2,100g',
    7000.00, 5),

(24,  3, 10, 'FORK-WPN-002', 'Weapon Rifle Air Fork 120mm',
    '120mm air-sprung trail fork with tapered steerer and Boost axle.',
    'The Weapon Rifle is a capable mid-travel air fork optimized for trail MTBs. Its 32mm stanchions and tapered steerer ensure stiffness and compatibility with modern framesets.',
    'Aluminum Alloy', 'Matte Black', '27.5" / 29"', 'Universal', 0, 0,
    'Quick Release 9x100mm / Thru-Axle 15x100mm', NULL, '120mm', 'Post-Mount Disc',
    'Stanchion: 32mm; Steerer: Tapered; Rebound: External; Lockout: Remote-compatible; Weight: ~1,950g',
    6000.00, 7),

(25,  3, 10, 'FORK-WPN-003', 'Weapon Rocket Air Fork 120mm BOOST',
    'BOOST 120mm air fork with 32mm stanchions for trail hardtails.',
    'The Weapon Rocket brings Boost axle compatibility to the Rifle platform, pairing 32mm stanchions with a 15x110mm Boost thru-axle for enhanced wheel stiffness on Boost-spaced frames.',
    'Aluminum Alloy', 'Matte Grey', '27.5" / 29"', 'Universal', 1, 0,
    'Thru-Axle 15x110mm (Boost)', NULL, '120mm', 'Post-Mount Disc',
    'Stanchion: 32mm; Steerer: Tapered 44-56mm; Rebound: External knob; Weight: ~1,980g',
    6000.00, 6),

(26,  3, 10, 'FORK-WPN-004', 'Weapon Tower Air Fork 100mm',
    'Entry-level air fork for light trail and XC mountain biking.',
    'The Weapon Tower is a lightweight air fork offering 100mm of travel on a straight steerer. Suited for budget-to-mid XC and trail builds seeking the smooth feel of air suspension.',
    'Aluminum Alloy', 'Black / Grey', '27.5"', 'Universal', 0, 0,
    'Quick Release 9x100mm', NULL, '100mm', 'Post-Mount Disc',
    'Stanchion: 32mm; Steerer: 1-1/8" straight; Lockout: Manual top-cap; Weight: ~1,900g',
    4000.00, 10),

(27,  3, 14, 'FORK-SPD-001', 'Speedone Soldier BOOST Air Fork 120mm',
    'BOOST air fork with external rebound for trail MTBs.',
    'The Speedone Soldier BOOST is a Philippine-market air suspension fork with Boost thru-axle, tapered steerer, and external rebound adjustment. A popular upgrade for trail builds on a budget.',
    'Aluminum Alloy', 'Matte Black', '27.5" / 29"', 'Universal', 1, 0,
    'Thru-Axle 15x110mm (Boost)', NULL, '120mm', 'Post-Mount Disc',
    'Stanchion: 32mm; Steerer: Tapered; Rebound: External; Lockout: Remote-compatible',
    5900.00, 8),

(28,  3, 37, 'FORK-MAN-001', 'Manitou Markhor BOOST Air Fork 120mm',
    'American-engineered 120mm Boost fork with Dorado-derived damping.',
    'The Manitou Markhor BOOST features Manitou''s proven Dorado-derived damping technology in an accessible trail fork. With 120mm of travel, Boost axle spacing, and a tapered steerer, it is a premium choice for intermediate trail riders.',
    'Aluminum Alloy / Magnesium Lowers', 'Gloss Black', '27.5" / 29"', 'Universal', 1, 0,
    'Thru-Axle 15x110mm (Boost)', NULL, '120mm', 'Post-Mount Disc',
    'Stanchion: 34mm; Steerer: Tapered 1-1/8 to 1.5"; Damper: IFP; Rebound: External; Weight: ~1,900g',
    12000.00, 4),

(29,  3, 37, 'FORK-MAN-002', 'Manitou Machete Comp BOOST 130mm',
    'Trail-enduro Boost fork with 130mm travel and MRD damper.',
    'The Manitou Machete Comp is an enduro-capable fork built with Manitou''s MRD damper for precise rebound tuning. Its 130mm travel and Boost thru-axle suit aggressive trail and light enduro riding.',
    'Aluminum Alloy / Magnesium Lowers', 'Matte Black / Red', '27.5" / 29"', 'Universal', 1, 0,
    'Thru-Axle 15x110mm (Boost)', NULL, '130mm', 'Post-Mount Disc',
    'Stanchion: 34mm; Damper: MRD; Rebound and compression: External; Weight: ~1,950g',
    16500.00, 3),

(30,  3, 37, 'FORK-MAN-003', 'Manitou Mattoc Comp Boost 140mm',
    'Enduro-grade 140mm Boost fork with Dorado Pro damper system.',
    'The Manitou Mattoc Comp is a serious enduro fork offering 140mm of plush travel. The Dorado Pro damper delivers superb control on rough terrain, while Boost spacing enhances wheel stiffness. A top-tier choice for technical enduro builds.',
    'Aluminum Alloy / Magnesium Lowers', 'Gloss Black', '27.5" / 29"', 'Universal', 1, 0,
    'Thru-Axle 15x110mm (Boost)', NULL, '140mm', 'Post-Mount Disc',
    'Stanchion: 34mm; Damper: Dorado Pro; Rebound and compression: External adjust; Weight: ~2,000g',
    25000.00, 2),

(31,  3, 38, 'FORK-SRS-001', 'SR Suntour Epixon Stealth 120mm',
    'Mid-range air fork with Stealth lower legs and hydraulic damping.',
    'The SR Suntour Epixon Stealth offers 120mm of air-sprung travel with hydraulic bottom-out and Stealth lower design for a clean aesthetic. Solid mid-range performance for trail MTBs.',
    'Aluminum Alloy', 'Matte Black (Stealth)', '27.5" / 29"', 'Universal', 1, 0,
    'Thru-Axle 15x110mm (Boost)', NULL, '120mm', 'Post-Mount Disc',
    'Stanchion: 34mm; Steerer: Tapered; LO: Remote compatible; Rebound: External; Weight: ~1,920g',
    8600.00, 6),

(32,  3, 38, 'FORK-SRS-002', 'SR Suntour Raidon BOOST 130mm',
    'Trail-grade BOOST air fork with 34mm stanchions and lockout.',
    'The SR Suntour Raidon BOOST is a trail-oriented air fork delivering 130mm of smooth travel. With 34mm stanchions and a Boost thru-axle, it offers improved stiffness and control over entry-level options.',
    'Aluminum Alloy / Magnesium Lowers', 'Matte Black', '27.5" / 29"', 'Universal', 1, 0,
    'Thru-Axle 15x110mm (Boost)', NULL, '130mm', 'Post-Mount Disc',
    'Stanchion: 34mm; Steerer: Tapered; Rebound: External; Lockout: Remote; Weight: ~2,050g',
    9000.00, 5),

(33,  3, 38, 'FORK-SRS-003', 'SR Suntour XCR 32 BOOST 120mm',
    'XC-race air fork with 32mm Boost stanchions and ultra-light design.',
    'The SR Suntour XCR 32 BOOST is SR Suntour''s top-tier XC race fork, featuring an ultra-lightweight chassis, 32mm Boost stanchions, and a precise air spring. Ideal for competitive XC racing where every gram counts.',
    'Carbon / Magnesium Lowers', 'White / Black', '29"', 'Universal', 1, 0,
    'Thru-Axle 15x110mm (Boost)', NULL, '120mm', 'Post-Mount Disc',
    'Stanchion: 32mm; Crown: Carbon; Weight: ~1,600g; Rebound: External; Lockout: TurnKey remote',
    52000.00, 1);
GO

-- =============================================================================
-- SEED: Products — Category 4: Hubs
-- =============================================================================
INSERT INTO Products (ProductID, CategoryID, BrandID, SKU, ProductName, ShortDescription, FullDescription,
                      Material, Color, WheelSize, SpeedCompatibility, BoostCompatible, TubelessReady,
                      AxleStandard, FrameMaterial, SuspensionTravel, BrakeType, AdditionalSpecs,
                      Price, StockQuantity)
VALUES
(34,  4, 17, 'HUB-LDC-001', 'LDCNC 3.0 Sealed Bearing Hub Set',
    'Precision CNC-machined sealed bearing hub set — front and rear pair.',
    'The LDCNC 3.0 is a precision CNC-machined hub set featuring sealed cartridge bearings for smooth, low-maintenance rolling. Available in standard QR and Boost configurations. Sold as a front + rear pair.',
    'CNC Aluminum Alloy 6061', 'Black / Silver', 'Universal', '9-11-speed', 0, 0,
    'Quick Release 9x100mm (F) / 9x135mm (R)', NULL, NULL, 'Center Lock / 6-Bolt Disc',
    'Bearing: 4 sealed cartridge per hub; Flange: Double-wall; Finish: Anodized; Sold as pair',
    2200.00, 10),

(35,  4, 14, 'HUB-SPD-001', 'SpeedOne Soldier BOOST Hub Set',
    'BOOST 110/148 sealed bearing hub set for trail MTBs.',
    'The SpeedOne Soldier BOOST hub set features Boost spacing (15x110mm front, 12x148mm rear) with sealed cartridge bearings and 32-hole flanges for strong, reliable wheels.',
    'CNC Aluminum Alloy 6061', 'Black', '27.5" / 29"', '10-12-speed', 1, 0,
    'Thru-Axle 15x110mm (F) / 12x148mm (R)', NULL, NULL, '6-Bolt Disc',
    'Holes: 32H; Bearing: Sealed cartridge; Freehub: Shimano HG compatible; Sold as pair',
    3400.00, 8),

(36,  4, 15, 'HUB-GEN-001', 'Genova Big Dipper Sealed Bearing Hub Set',
    'Alloy hub set with large-flange design for improved wheel strength.',
    'The Genova Big Dipper features larger hub flanges compared to standard hubs, providing improved spoke bracing angle and a stiffer, stronger wheel build. Suitable for trail and enduro applications.',
    'CNC Aluminum Alloy', 'Black / Red', '27.5" / 29"', '9-11-speed', 0, 0,
    'Quick Release 9x100mm (F) / 9x135mm (R)', NULL, NULL, '6-Bolt Disc',
    'Flange: Large diameter; Holes: 32H; Bearing: Sealed; Freehub: Shimano HG; Sold as pair',
    3000.00, 9),

(37,  4, 10, 'HUB-WPN-001', 'Weapon Animal BOOST Hub Set',
    'High-flange Boost hub set with CNC construction for trail and enduro.',
    'The Weapon Animal BOOST hub set pairs high-flange hubs with Boost axle spacing for maximum wheel stiffness. CNC-machined from 6061 alloy with 4 sealed bearings per hub.',
    'CNC Aluminum Alloy 6061', 'Black / Gold', '27.5" / 29"', '11-12-speed', 1, 0,
    'Thru-Axle 15x110mm (F) / 12x148mm (R)', NULL, NULL, '6-Bolt Disc',
    'Holes: 32H; Bearing: 4 sealed per hub; Freehub: Shimano Micro Spline / HG; Sold as pair',
    4500.00, 6),

(38,  4, 12, 'HUB-SAG-001', 'Sagmit EVO3 Sealed Bearing Hub Set',
    'Philippine alloy hub set with 3-pawl mechanism and sealed bearings.',
    'The Sagmit EVO3 is an affordable and reliable hub set with a 3-pawl ratchet engagement mechanism and sealed cartridge bearings. A popular value choice for budget trail builds in the Philippines.',
    'Aluminum Alloy', 'Black', 'Universal', '9-11-speed', 0, 0,
    'Quick Release 9x100mm (F) / 9x135mm (R)', NULL, NULL, '6-Bolt Disc',
    'Ratchet: 3-pawl; Holes: 32H; Freehub: Shimano HG; Bearing: Sealed cartridge; Sold as pair',
    3400.00, 15),

(39,  4, 23, 'HUB-RAG-001', 'Ragusa R100 Alloy Hub Set',
    'Budget alloy hub set for general trail and commuter builds.',
    'The Ragusa R100 is a basic alloy hub set designed for value builds. Features standard QR axles and 32-hole flanges compatible with most entry-level rim builds.',
    'Aluminum Alloy', 'Black', 'Universal', '7-9-speed', 0, 0,
    'Quick Release 9x100mm (F) / 9x135mm (R)', NULL, NULL, '6-Bolt Disc',
    'Holes: 32H; Freehub: Shimano HG 7/8/9s; Bearing: Sealed; Sold as pair',
    950.00, 20),

(40,  4, 14, 'HUB-SPD-002', 'SpeedOne Soldier Standard QR Hub Set',
    'Affordable QR hub set for standard trail frames.',
    'The SpeedOne Soldier Standard offers QR axle compatibility and sealed bearings at a value price. A strong seller for budget trail builds with standard 135mm rear spacing.',
    'Aluminum Alloy 6061', 'Black', 'Universal', '9-11-speed', 0, 0,
    'Quick Release 9x100mm (F) / 9x135mm (R)', NULL, NULL, '6-Bolt Disc',
    'Holes: 32H; Freehub: Shimano HG; Bearing: Sealed; Sold as pair',
    3000.00, 12),

(41,  4, 14, 'HUB-SPD-003', 'SpeedOne Pilot Carbon Hub Set',
    'Lightweight carbon-shell hub set for XC and race builds.',
    'The SpeedOne Pilot Carbon is a premium hub set featuring a carbon composite shell for reduced rotational weight. Ideal for XC race and gravel builds where saving weight is paramount.',
    'Carbon Composite / Alloy Axle', 'Black / Red', 'Universal', '10-12-speed', 1, 0,
    'Thru-Axle 15x110mm (F) / 12x148mm (R)', NULL, NULL, '6-Bolt Disc',
    'Holes: 28H / 32H; Freehub: Shimano HG / Micro Spline; Bearing: Ceramic-grade sealed; Sold as pair',
    3450.00, 5),

(42,  4, 14, 'HUB-SPD-004', 'SpeedOne Torpedo Sealed Bearing Hub Set',
    'Mid-range alloy hub set with high-speed 4-pawl engagement.',
    'The SpeedOne Torpedo features a 4-pawl ratchet mechanism for faster engagement and a more responsive feel on technical trails. CNC-machined from 6061 alloy with 4 sealed bearings.',
    'CNC Aluminum Alloy 6061', 'Black / Blue', '27.5" / 29"', '10-12-speed', 1, 0,
    'Thru-Axle 15x110mm (F) / 12x148mm (R)', NULL, NULL, '6-Bolt Disc',
    'Ratchet: 4-pawl 36T; Holes: 32H; Freehub: Shimano HG / Micro Spline; Sold as pair',
    3600.00, 7),

(43,  4, 16, 'HUB-OR8-001', 'Origin8 NON-BOOST Sealed Bearing Hub Set',
    'American-brand QR alloy hub set for standard 100/135mm frames.',
    'The Origin8 non-BOOST hub set is designed for older and entry-level framesets with standard QR axle spacing. Features sealed cartridge bearings and a wide flange for structural integrity.',
    'CNC Aluminum Alloy 6061', 'Black / Silver', 'Universal', '9-11-speed', 0, 0,
    'Quick Release 9x100mm (F) / 9x135mm (R)', NULL, NULL, '6-Bolt Disc',
    'Holes: 32H; Freehub: Shimano HG 9/10/11s; Bearing: Sealed cartridge; Sold as pair',
    5200.00, 6);
GO

-- =============================================================================
-- SEED: Products — Category 5: Upgrade Kits
-- =============================================================================
INSERT INTO Products (ProductID, CategoryID, BrandID, SKU, ProductName, ShortDescription, FullDescription,
                      Material, Color, WheelSize, SpeedCompatibility, BoostCompatible, TubelessReady,
                      AxleStandard, FrameMaterial, SuspensionTravel, BrakeType, AdditionalSpecs,
                      Price, StockQuantity)
VALUES
(44,  5, 12, 'UPGKIT-SAG-001', 'Sagmit Edison 12-Speed Upgrade Kit',
    'Complete 12-speed drivetrain upgrade kit for MTB framesets.',
    'The Sagmit Edison 12-Speed Upgrade Kit includes a shifter, rear derailleur, cassette, and chain — everything needed to convert a bike to a modern 1x12 drivetrain. Compatible with Shimano Micro Spline and standard HG freehubs.',
    'Alloy / Steel', 'Black', 'Universal', '12-speed', NULL, NULL,
    NULL, NULL, NULL, NULL,
    'Includes: Shifter (1x12), RD, 12sp Cassette 11-50T, 12sp Chain; BB sold separately; Compatible: Shimano-style freehub',
    5200.00, 8),

(45,  5, 18, 'UPGKIT-LTW-001', 'LTwoo AX 12-Speed Upkit with IXF Crank',
    'Full 12-speed upgrade kit with IXF crankset — Shimano-compatible.',
    'The LTwoo AX 12-Speed Upkit is a comprehensive 1x12 drivetrain upgrade including a LTwoo AX shifter, rear derailleur, IXF hollow-tech crankset, 12-speed cassette, and chain. A great Shimano-compatible value alternative.',
    'Alloy / CNC Alloy', 'Black / Silver', 'Universal', '12-speed', NULL, NULL,
    NULL, NULL, NULL, NULL,
    'Includes: LTwoo AX Shifter 1x12, RD 12sp, IXF Crankset 34T Hollowtech, 12sp Cassette 11-50T, Chain; BB: BSA threaded',
    8000.00, 5),

(46,  5, 20, 'UPGKIT-SHM-001', 'Deore M6100 12-Speed Upkit with Weapon Cogs',
    'Shimano Deore M6100 12-speed drivetrain upkit — reliable trail performance.',
    'The Deore M6100 upkit bundles Shimano Deore''s acclaimed 12-speed shifter and rear derailleur with a Weapon Micro Spline cassette and a quality 12-speed chain. Shimano Deore M6100 is the benchmark for durable, precise trail shifting.',
    'Alloy / Steel', 'Black', 'Universal', '12-speed (Micro Spline)', NULL, NULL,
    NULL, NULL, NULL, NULL,
    'Includes: Shimano SL-M6100 Shifter, RD-M6100, Weapon 12sp Cassette 11-50T (Micro Spline), 12sp Chain; BB sold separately',
    9000.00, 4),

(47,  5, 20, 'UPGKIT-SHM-002', 'ZEE M640 10-Speed Downhill Upkit',
    'Shimano ZEE M640 downhill/freeride 10-speed drivetrain upkit.',
    'The ZEE M640 upkit includes Shimano''s gravity-tuned ZEE 10-speed shifter and rear derailleur — designed to withstand the abuse of downhill and freeride riding. Paired with a compatible 10-speed cassette and chain.',
    'Alloy / Steel', 'Black', 'Universal', '10-speed', NULL, NULL,
    NULL, NULL, NULL, NULL,
    'Includes: ZEE SL-M640 Shifter, ZEE RD-M640, 10sp Cassette 11-36T, 10sp Chain; Rated for DH/gravity use',
    6800.00, 5),

(48,  5, 19, 'UPGKIT-SRM-001', 'SRAM NX Eagle 12-Speed Upkit with BB',
    'SRAM NX Eagle 12-speed 1x drivetrain kit — includes bottom bracket.',
    'The SRAM NX Eagle 12-speed upkit brings SRAM''s acclaimed 1x12 Eagle platform to a more accessible price point. Includes trigger shifter, rear derailleur, 11-50T cassette, chain, and SRAM BSA bottom bracket. Eagle X-Sync compatibility provides flawless chain retention.',
    'Alloy / Steel', 'Black / Red', 'Universal', '12-speed (SRAM XD / Shimano HG)', NULL, NULL,
    NULL, NULL, NULL, NULL,
    'Includes: SRAM NX Eagle Shifter, RD, Cassette 11-50T (XD), Chain, SRAM BSA BB; Note: Requires XD freehub',
    13000.00, 3);
GO

-- =============================================================================
-- SEED: Products — Category 6: Stems & Seatposts
-- =============================================================================
INSERT INTO Products (ProductID, CategoryID, BrandID, SKU, ProductName, ShortDescription, FullDescription,
                      Material, Color, WheelSize, SpeedCompatibility, BoostCompatible, TubelessReady,
                      AxleStandard, FrameMaterial, SuspensionTravel, BrakeType, AdditionalSpecs,
                      Price, StockQuantity)
VALUES
(49,  6, 21, 'STEM-CTL-001', 'Controltech ONE Handlebar Post (Stem) 31.8mm',
    'Lightweight 31.8mm alloy stem — 0° rise for aggressive trail geometry.',
    'The Controltech ONE handlebar stem offers a stiff, lightweight alloy construction with a 31.8mm bar clamp and 1-1/8" steerer clamp. Designed for trail MTB riders seeking a direct, responsive steering feel.',
    'CNC Aluminum Alloy 6061 T6', 'Black', NULL, 'Universal', NULL, NULL,
    NULL, NULL, NULL, NULL,
    'Bar clamp: 31.8mm; Steerer clamp: 28.6mm (1-1/8"); Rise: 0°; Length: 70mm / 80mm; Bolts: 4x M5',
    1800.00, 15),

(50,  6, 21, 'STEM-CTL-002', 'Controltech Standard Handlebar Post (Stem)',
    'Alloy stem with 6° rise for comfortable trail and XC riding.',
    'The Controltech standard stem features a 6° rise and 31.8mm bar clamp, suitable for XC and trail MTBs seeking an upright, comfortable position.',
    'Aluminum Alloy 6061', 'Black / Silver', NULL, 'Universal', NULL, NULL,
    NULL, NULL, NULL, NULL,
    'Bar clamp: 31.8mm; Steerer: 28.6mm; Rise: 6°; Length: 70–90mm; Weight: ~120g',
    1600.00, 18),

(51,  6, 21, 'STEM-CTL-003', 'Controltech ONE Seatpost 30.9mm',
    '30.9mm alloy seatpost for trail and enduro MTBs.',
    'The Controltech ONE seatpost is a lightweight straight alloy post with a two-bolt saddle clamp for precise saddle angle adjustment. Diameter 30.9mm, compatible with most trail MTB frames.',
    'CNC Aluminum Alloy 6061 T6', 'Black', NULL, 'Universal', NULL, NULL,
    NULL, NULL, NULL, NULL,
    'Diameter: 30.9mm; Length: 350mm / 400mm; Clamp: Dual-bolt; Offset: 0mm; Weight: ~220g',
    1700.00, 12),

(52,  6, 21, 'STEM-CTL-004', 'Controltech CLS Handlebar Post (Stem)',
    'CLS-style alloy stem with integrated cable guide for clean builds.',
    'The Controltech CLS stem features an integrated cable guide system for clean-looking internal routing setups. Compatible with 31.8mm handlebars and 1-1/8" to 1.5" tapered steerers.',
    'CNC Aluminum Alloy 6061 T6', 'Black', NULL, 'Universal', NULL, NULL,
    NULL, NULL, NULL, NULL,
    'Bar clamp: 31.8mm; Rise: 6°; Cable guide: Integrated; Length: 70/80/90mm; Weight: ~135g',
    1800.00, 10),

(53,  6, 10, 'STEM-WPN-001', 'Weapon Fury Stem 31.8mm',
    'Budget alloy trail stem with 31.8mm clamp — lightweight and clean.',
    'The Weapon Fury is an entry-level alloy stem offering solid construction at a low price. With a 31.8mm bar clamp and available in multiple lengths, it is ideal for value trail builds.',
    'Aluminum Alloy 6061', 'Black', NULL, 'Universal', NULL, NULL,
    NULL, NULL, NULL, NULL,
    'Bar clamp: 31.8mm; Steerer: 28.6mm; Rise: 6°; Length: 60/70/80mm; Weight: ~130g',
    850.00, 20),

(54,  6, 10, 'STEM-WPN-002', 'Weapon Ambush Stem 31.8mm',
    'Lightweight CNC alloy MTB stem with -6° rise.',
    'The Weapon Ambush stem provides an aggressive, negative-rise profile for riders seeking a lower, more aerodynamic bar position. CNC machined from 6061 alloy.',
    'CNC Aluminum Alloy 6061', 'Black', NULL, 'Universal', NULL, NULL,
    NULL, NULL, NULL, NULL,
    'Bar clamp: 31.8mm; Steerer: 28.6mm; Rise: -6°; Length: 60/70/80/90mm; Weight: ~125g',
    1000.00, 15),

(55,  6, 10, 'STEM-WPN-003', 'Weapon Savage Stem 35mm',
    '35mm clamp alloy stem for oversized trail handlebars.',
    'The Weapon Savage is built around the wider 35mm bar clamp standard, providing increased stiffness over conventional 31.8mm stems. Ideal for riders pairing with 35mm handlebars for aggressive trail riding.',
    'CNC Aluminum Alloy 6061', 'Black', NULL, 'Universal', NULL, NULL,
    NULL, NULL, NULL, NULL,
    'Bar clamp: 35mm; Steerer: 28.6mm; Rise: 0°; Length: 50/60/70mm; Weight: ~130g',
    1100.00, 12),

(56,  6, 10, 'STEM-WPN-004', 'Weapon Beast Stem 35mm',
    'Oversized 35mm bar clamp alloy stem with extended reach.',
    'The Weapon Beast provides maximum stiffness with its 35mm clamp and reinforced body geometry. Designed for enduro and aggressive trail riders demanding precise, direct steering.',
    'CNC Aluminum Alloy 7075', 'Black / Red', NULL, 'Universal', NULL, NULL,
    NULL, NULL, NULL, NULL,
    'Bar clamp: 35mm; Steerer: 28.6mm (1-1/8"); Rise: 0°; Length: 50/60/70/80mm; Weight: ~140g',
    1200.00, 10),

(57,  6, 10, 'STEM-WPN-005', 'Weapon Animal Stem 31.8mm',
    'Mid-range alloy trail stem with 31.8mm bar clamp.',
    'The Weapon Animal is a mid-range alloy stem balanced between the Ambush and Beast, with a 31.8mm clamp and 6° positive rise for a comfortable trail bar height.',
    'CNC Aluminum Alloy 6061', 'Black', NULL, 'Universal', NULL, NULL,
    NULL, NULL, NULL, NULL,
    'Bar clamp: 31.8mm; Steerer: 28.6mm; Rise: 6°; Length: 60/70/80mm; Weight: ~128g',
    1100.00, 14),

(58,  6, 10, 'STEM-WPN-006', 'Weapon Predator Stem 31.8mm',
    'Aggressive trail alloy stem with reinforced steerer clamp.',
    'The Weapon Predator features a reinforced 4-bolt steerer clamp for enhanced torsional stiffness, making it a great option for rowdy trail and enduro riding with 31.8mm bars.',
    'CNC Aluminum Alloy 6061', 'Black', NULL, 'Universal', NULL, NULL,
    NULL, NULL, NULL, NULL,
    'Bar clamp: 31.8mm; Steerer: 28.6mm; Rise: 0° / 6°; Length: 50/60/70/80mm; Weight: ~135g',
    1000.00, 15);
GO

-- =============================================================================
-- SEED: Products — Category 7: Handlebars
-- =============================================================================
INSERT INTO Products (ProductID, CategoryID, BrandID, SKU, ProductName, ShortDescription, FullDescription,
                      Material, Color, WheelSize, SpeedCompatibility, BoostCompatible, TubelessReady,
                      AxleStandard, FrameMaterial, SuspensionTravel, BrakeType, AdditionalSpecs,
                      Price, StockQuantity)
VALUES
(59,  7, 21, 'HBAR-CTL-001', 'Controltech Handle Bar Pro LT 31.8mm 720mm',
    'Lightweight 31.8mm alloy flat riser bar — 720mm wide.',
    'The Controltech Pro LT is a lightweight alloy flat bar designed for trail MTBs. At 720mm wide with a 20mm rise, it provides a comfortable and controlled steering position for trail rides.',
    'Aluminum Alloy 6061 T6', 'Black', NULL, 'Universal', NULL, NULL,
    NULL, NULL, NULL, NULL,
    'Clamp: 31.8mm; Width: 720mm; Rise: 20mm; Sweep: 9° back / 5° up; Weight: ~210g',
    1800.00, 18),

(60,  7, 21, 'HBAR-CTL-002', 'Controltech Handle Bar Pro Koryak 31.8mm 740mm',
    'Wide 31.8mm alloy riser bar for aggressive trail and enduro riding.',
    'The Controltech Koryak is a wider, stiffer version of the Pro LT, at 740mm. A more carbon-like stiffness profile due to butted alloy tube construction.',
    'Butted Aluminum Alloy 6061 T6', 'Black', NULL, 'Universal', NULL, NULL,
    NULL, NULL, NULL, NULL,
    'Clamp: 31.8mm; Width: 740mm; Rise: 25mm; Sweep: 9° back / 5° up; Weight: ~225g',
    2200.00, 14),

(61,  7, 21, 'HBAR-CTL-003', 'Drop Bar Gravel Pro LT 31.8mm 420mm',
    'Compact alloy drop bar for gravel and road builds — 420mm wide.',
    'The Controltech Gravel Pro LT drop bar offers a short-reach, shallow-drop profile optimized for gravel riding. Suitable for 31.8mm clamp stems.',
    'Aluminum Alloy 6061 T6', 'Black', NULL, 'Universal', NULL, NULL,
    NULL, NULL, NULL, NULL,
    'Clamp: 31.8mm; Width: 420mm; Reach: 75mm; Drop: 128mm; Flare: 6°; Weight: ~270g',
    1800.00, 12),

(62,  7, 21, 'HBAR-CTL-004', 'Drop Bar Gravel Pro Discovery 31.8mm 440mm',
    'Wider flared alloy gravel drop bar — 440mm for enhanced off-road control.',
    'The Gravel Pro Discovery adds 16° of flare compared to the LT, improving grip and leverage on rough gravel surfaces. Suitable for adventure and bikepacking setups.',
    'Aluminum Alloy 6061 T6', 'Black', NULL, 'Universal', NULL, NULL,
    NULL, NULL, NULL, NULL,
    'Clamp: 31.8mm; Width: 440mm (tops) / 460mm (drops); Flare: 16°; Reach: 80mm; Drop: 130mm; Weight: ~285g',
    2200.00, 10),

(63,  7, 22, 'HBAR-ANS-001', 'Answer ProTaper 20x20 Alloy Bar 31.8mm 800mm',
    'Wide 800mm ProTaper alloy riser bar for enduro and DH.',
    'The Answer ProTaper 20x20 features a 20mm rise and 20mm back sweep, providing maximum control for DH and enduro riding. At 800mm wide, it offers outstanding stability on technical terrain.',
    'Aluminum Alloy 7050 T6', 'Black', NULL, 'Universal', NULL, NULL,
    NULL, NULL, NULL, NULL,
    'Clamp: 31.8mm; Width: 800mm; Rise: 20mm; Back sweep: 20°; Up sweep: 5°; Weight: ~280g',
    2000.00, 10),

(64,  7, 22, 'HBAR-ANS-002', 'Answer ProTaper Alloy Bar 31.8mm 780mm',
    'Classic ProTaper alloy riser bar at 780mm width.',
    'The Answer ProTaper is a classic riser bar offering a 780mm width, 15mm rise, and the ProTaper profile that has been trusted by DH and trail riders worldwide.',
    'Aluminum Alloy 7050 T6', 'Black', NULL, 'Universal', NULL, NULL,
    NULL, NULL, NULL, NULL,
    'Clamp: 31.8mm; Width: 780mm; Rise: 15mm; Weight: ~265g',
    2000.00, 12),

(65,  7, 22, 'HBAR-ANS-003', 'Answer ProTaper 7050 Series Aluminum Bar 31.8mm 760mm',
    '7050 series alloy ProTaper bar — stiff, lightweight, 760mm wide.',
    'The Answer ProTaper 7050 Series uses harder 7050-series aluminum for improved stiffness and impact resistance over conventional 6061 bars. At 760mm, it suits trail and enduro riders who prefer a slightly narrower bar.',
    'Aluminum Alloy 7050 T6', 'Black / Silver', NULL, 'Universal', NULL, NULL,
    NULL, NULL, NULL, NULL,
    'Clamp: 31.8mm; Width: 760mm; Rise: 15mm; Back sweep: 9°; Weight: ~255g',
    1600.00, 10),

(66,  7, 12, 'HBAR-SAG-001', 'Sagmit Brooklyn 3.0 Flat Bar 31.8mm 720mm',
    'Affordable Philippine alloy flat bar for trail MTBs.',
    'The Sagmit Brooklyn 3.0 is a lightweight alloy flat bar suited for trail and urban MTB builds. At 720mm, it provides a comfortable width for trail riding.',
    'Aluminum Alloy 6061', 'Black', NULL, 'Universal', NULL, NULL,
    NULL, NULL, NULL, NULL,
    'Clamp: 31.8mm; Width: 720mm; Rise: 15mm; Sweep: 9° back; Weight: ~220g',
    550.00, 25),

(67,  7, 12, 'HBAR-SAG-002', 'Sagmit Shadow Riser Bar 31.8mm 740mm',
    'Mid-rise 31.8mm alloy bar for trail MTBs and hardtails.',
    'The Sagmit Shadow offers a mid-rise profile at 740mm width, balancing comfort and control for trail riding.',
    'Aluminum Alloy 6061', 'Matte Black', NULL, 'Universal', NULL, NULL,
    NULL, NULL, NULL, NULL,
    'Clamp: 31.8mm; Width: 740mm; Rise: 20mm; Back sweep: 9°; Weight: ~230g',
    550.00, 22),

(68,  7, 12, 'HBAR-SAG-003', 'Sagmit Static Riser Bar 35mm 760mm',
    'Oversized 35mm alloy bar for enduro — wide and stiff.',
    'The Sagmit Static is built around the 35mm clamp standard for improved stiffness. At 760mm, it suits enduro and aggressive trail riders seeking direct, reliable steering.',
    'Aluminum Alloy 6061', 'Black', NULL, 'Universal', NULL, NULL,
    NULL, NULL, NULL, NULL,
    'Clamp: 35mm; Width: 760mm; Rise: 20mm; Weight: ~245g',
    750.00, 15);
GO

-- =============================================================================
-- SEED: Products — Category 8: Saddles
-- =============================================================================
INSERT INTO Products (ProductID, CategoryID, BrandID, SKU, ProductName, ShortDescription, FullDescription,
                      Material, Color, WheelSize, SpeedCompatibility, BoostCompatible, TubelessReady,
                      AxleStandard, FrameMaterial, SuspensionTravel, BrakeType, AdditionalSpecs,
                      Price, StockQuantity)
VALUES
(69,  8, 12, 'SADL-SAG-001', 'Sagmit RedClassic Skull Edition Saddle',
    'Flat MTB saddle with skull graphic and comfort foam padding.',
    'The Sagmit RedClassic Skull Edition is a flat saddle featuring a firm foam core, durable PU cover, and a distinctive skull graphic for riders looking to personalize their build.',
    'PU Cover / Foam Core / Steel Rails', 'Black / Red Skull', NULL, 'Universal', NULL, NULL,
    NULL, NULL, NULL, NULL,
    'Width: 138mm; Rail: Steel; Padding: Firm foam; Length: 268mm; Weight: ~320g',
    450.00, 30),

(70,  8, 15, 'SADL-GEN-001', 'Genova Jupiter MTB Saddle',
    'Lightweight trail MTB saddle with ergonomic cut-out.',
    'The Genova Jupiter features a central pressure-relief channel and ergonomic padding for trail and XC riding. Suitable for daily trail use.',
    'PU Cover / Foam Core / Alloy Rails', 'Black', NULL, 'Universal', NULL, NULL,
    NULL, NULL, NULL, NULL,
    'Width: 140mm; Rail: 7mm Alloy; Length: 270mm; Channel: Central relief; Weight: ~280g',
    450.00, 28),

(71,  8, 43, 'SADL-GNT-001', 'Giant MTB Saddle (No Cut-Out)',
    'OEM Giant flat saddle — firm support for trail rides.',
    'The Giant MTB Saddle (No Cut-Out) is a flat-profile saddle with moderate foam padding, suitable for trail riders who prefer even weight distribution without a channel.',
    'PU Cover / Foam Core / Steel Rails', 'Black', NULL, 'Universal', NULL, NULL,
    NULL, NULL, NULL, NULL,
    'Width: 142mm; Rail: Steel 7x9mm; Length: 268mm; Flat profile; Weight: ~330g',
    450.00, 25),

(72,  8, 43, 'SADL-GNT-002', 'Giant MTB Saddle (With Cut-Out)',
    'OEM Giant saddle with central relief channel for pressure reduction.',
    'The Giant MTB Saddle With Cut-Out features a central pressure-relief channel that reduces soft-tissue pressure on longer rides. Suitable for trail and cross-country riding.',
    'PU Cover / Foam Core / Steel Rails', 'Black', NULL, 'Universal', NULL, NULL,
    NULL, NULL, NULL, NULL,
    'Width: 142mm; Rail: Steel; Length: 268mm; Channel: Central cut-out; Weight: ~325g',
    450.00, 22),

(73,  8, 23, 'SADL-RAG-001', 'Ragusa R-100 Lightweight MTB Saddle',
    'Slim, lightweight saddle for cross-country and trail riders.',
    'The Ragusa R-100 is a slim, minimalist saddle designed for XC riders who prefer a light, interference-free feel. Narrow nose reduces inner-thigh chafing during pedaling.',
    'Microfiber Cover / Foam / Alloy Rails', 'Black', NULL, 'Universal', NULL, NULL,
    NULL, NULL, NULL, NULL,
    'Width: 136mm; Rail: Alloy 7mm; Length: 265mm; Profile: Flat/race; Weight: ~245g',
    450.00, 20),

(74,  8, 12, 'SADL-SAG-002', 'Sagmit Oilslick Saddle',
    'Special-edition oilslick finish saddle with ergonomic foam.',
    'The Sagmit Oilslick saddle features a striking oil-slick iridescent finish on the rail and cover trim, making it a popular choice for custom builds. Ergonomic medium-width profile.',
    'PU Cover / Foam / Anodized Alloy Rails', 'Black / Oilslick', NULL, 'Universal', NULL, NULL,
    NULL, NULL, NULL, NULL,
    'Width: 140mm; Rail: Anodized alloy 7mm; Length: 268mm; Finish: Oilslick; Weight: ~290g',
    550.00, 15),

(75,  8, 15, 'SADL-GEN-002', 'Genova Mars Trail Saddle',
    'Widened comfort trail saddle with soft foam padding.',
    'The Genova Mars is a slightly wider saddle designed for trail riders who spend longer hours in the saddle. Extra foam padding and a wider rear profile distribute pressure more evenly.',
    'PU Cover / High-Density Foam / Steel Rails', 'Black / Grey', NULL, 'Universal', NULL, NULL,
    NULL, NULL, NULL, NULL,
    'Width: 145mm; Rail: Steel; Length: 272mm; Padding: High-density; Weight: ~340g',
    450.00, 22),

(76,  8, 9,  'SADL-SPZ-001', 'Specialized Body Geometry Black Saddle',
    'Specialized Body Geometry saddle with pressure-relief channel.',
    'The Specialized Body Geometry saddle uses the brand''s proprietary ergonomic research to optimize saddle shape for reduced nerve pressure and improved blood flow. A quality OEM replacement.',
    'Microfiber Cover / Foam / Steel Rails', 'Black', NULL, 'Universal', NULL, NULL,
    NULL, NULL, NULL, NULL,
    'Width: 143mm; Rail: Steel Cr-Mo; Length: 270mm; BG Channel: Yes; Weight: ~305g',
    450.00, 18),

(77,  8, 9,  'SADL-SPZ-002', 'Specialized M811 Trail MTB Saddle',
    'Specialized mid-range trail saddle with BG relief channel.',
    'The Specialized M811 is a durable trail saddle featuring the BG pressure-relief channel and a durable microfiber cover. An upgraded OEM-spec saddle suitable for trail and enduro builds.',
    'Microfiber Cover / Multi-Density Foam / Steel Rails', 'Black', NULL, 'Universal', NULL, NULL,
    NULL, NULL, NULL, NULL,
    'Width: 143mm; Rail: Cr-Mo Steel; BG Channel: Yes; Length: 272mm; Weight: ~310g',
    450.00, 15),

(78,  8, 40, 'SADL-SMR-001', 'San Marco Aspide Racing Saddle',
    'Italian racing saddle — lightweight and performance-oriented.',
    'The San Marco Aspide is a performance road/gravel saddle featuring a slim profile, lightweight construction, and San Marco''s heritage Italian craftsmanship. Suitable for gravel, road, and CX builds.',
    'Microfiber Cover / Foam / Carbon-Reinforced Nylon Shell', 'Black', NULL, 'Universal', NULL, NULL,
    NULL, NULL, NULL, NULL,
    'Width: 130mm; Rail: Alloy 7mm; Shell: Carbon-reinforced nylon; Length: 272mm; Weight: ~220g',
    450.00, 12),

(79,  8, 41, 'SADL-VLO-001', 'Velo Plush Comfort MTB Saddle',
    'Gel-padded comfort saddle for trail and enduro rides.',
    'The Velo Plush offers generous gel padding for maximum all-day comfort on trail bikes. Wider profile distributes weight over a larger area, reducing pressure points.',
    'PU / Gel Foam Composite / Steel Rails', 'Black', NULL, 'Universal', NULL, NULL,
    NULL, NULL, NULL, NULL,
    'Width: 148mm; Rail: Steel; Gel padding: Yes; Length: 275mm; Weight: ~380g',
    450.00, 20),

(80,  8, 42, 'SADL-ESD-001', 'Easydo ES-01 Ergonomic Saddle',
    'Chinese ergonomic saddle with central channel — value choice.',
    'The Easydo ES-01 is an ergonomic saddle designed to reduce perineal pressure through a central relief cut-out. Available in multiple widths and suitable for both MTB and commuter builds.',
    'PU Cover / Foam / Steel Rails', 'Black / Grey', NULL, 'Universal', NULL, NULL,
    NULL, NULL, NULL, NULL,
    'Width: 143mm; Rail: Steel; Cut-out: Central; Length: 270mm; Weight: ~340g',
    450.00, 20),

(81,  8, 12, 'SADL-SAG-003', 'Sagmit Diamond MTB Saddle',
    'Diamond-stitched flat saddle for trail and hardtail builds.',
    'The Sagmit Diamond features classic diamond stitching on its flat profile, giving a premium look at an affordable price. Medium foam padding for trail and daily riding.',
    'PU Cover / Foam / Steel Rails', 'Black', NULL, 'Universal', NULL, NULL,
    NULL, NULL, NULL, NULL,
    'Width: 140mm; Rail: Steel; Profile: Flat; Stitching: Diamond; Weight: ~330g',
    350.00, 25),

(82,  8, 12, 'SADL-SAG-004', 'Topgrade Trail Saddle',
    'Entry-level alloy-rail flat saddle for budget trail builds.',
    'The Topgrade Trail Saddle is a no-frills flat saddle with adequate foam padding and alloy rails. Ideal for riders equipping a budget trail build or commuter MTB.',
    'PU Cover / Foam / Alloy Rails', 'Black', NULL, 'Universal', NULL, NULL,
    NULL, NULL, NULL, NULL,
    'Width: 140mm; Rail: Alloy 7mm; Profile: Flat; Length: 268mm; Weight: ~280g',
    350.00, 30);
GO

-- =============================================================================
-- SEED: Products — Category 9: Grips & Bar Tape
-- =============================================================================
INSERT INTO Products (ProductID, CategoryID, BrandID, SKU, ProductName, ShortDescription, FullDescription,
                      Material, Color, WheelSize, SpeedCompatibility, BoostCompatible, TubelessReady,
                      AxleStandard, FrameMaterial, SuspensionTravel, BrakeType, AdditionalSpecs,
                      Price, StockQuantity)
VALUES
(83,  9, 24, 'GRIP-SER-001', 'Seer Silicon Grip 5mm — Lock-On',
    '5mm silicone waffle-pattern lock-on grips for MTB handlebars.',
    'The Seer Silicon Grip 5mm Lock-On features a 5mm waffle silicone compound for excellent vibration damping. Aluminum alloy lock rings ensure the grips stay in place on the most demanding trails.',
    'Silicone Compound / Alloy Lock Ring', 'Black / Grey', NULL, 'Universal', NULL, NULL,
    NULL, NULL, NULL, NULL,
    'Diameter: 33mm; Length: 130mm; Lock ring: Alloy; Compound: Silicone 5mm; Sold as pair',
    330.00, 30),

(84,  9, 24, 'GRIP-SER-002', 'Seer Polymer Bar Tape 3mm',
    '3mm polymer road/gravel bar tape — firm feel for drop bars.',
    'The Seer Polymer Bar Tape is a 3mm firm-compound tape designed for road and gravel drop bars. Provides grip and comfort while maintaining a slim diameter feel.',
    'Polymer Compound', 'Black', NULL, 'Universal', NULL, NULL,
    NULL, NULL, NULL, NULL,
    'Thickness: 3mm; Width: 30mm; Length: 2.5m per roll; Finish plug: Included; Includes end tape',
    350.00, 25),

(85,  9, 24, 'GRIP-SER-003', 'Seer Silicon Grip 7mm — Lock-On',
    'Thick 7mm silicone lock-on grips for maximum trail vibration damping.',
    'The Seer Silicon Grip 7mm offers the thickest silicone compound in the Seer grip lineup, ideal for enduro riders seeking maximum hand comfort and vibration absorption.',
    'Silicone Compound / Alloy Lock Ring', 'Black / Red', NULL, 'Universal', NULL, NULL,
    NULL, NULL, NULL, NULL,
    'Diameter: 35mm; Length: 130mm; Lock ring: Alloy; Compound: Silicone 7mm; Sold as pair',
    400.00, 22),

(86,  9, 24, 'GRIP-SER-004', 'Seer Art Racing Edition Bar Tape',
    'Premium art-print bar tape for race and gravel builds.',
    'The Seer Art Racing Edition bar tape uses a dense foam substrate with a stylized art print overcoat for a premium aesthetic. Suitable for road racing, gravel, and CX builds.',
    'Foam / PU Print Layer', 'Black / White Art', NULL, 'Universal', NULL, NULL,
    NULL, NULL, NULL, NULL,
    'Thickness: 3.5mm; Length: 2.5m; Finish plug: Cork; Includes end tape',
    400.00, 18),

(87,  9, 24, 'GRIP-SER-005', 'Seer SILICON Hornet Bar Tape 4mm',
    '4mm silicone MTB-style bar tape for gravel drop bars.',
    'The Seer Hornet Silicon bar tape is thicker at 4mm, offering generous cushioning for rough gravel surfaces. Silicone compound provides a non-slip grip even in wet conditions.',
    'Silicone Compound', 'Black', NULL, 'Universal', NULL, NULL,
    NULL, NULL, NULL, NULL,
    'Thickness: 4mm; Length: 2.5m; Grip: Silicone non-slip; Suitable for: Drop bars; Includes end tape',
    450.00, 15),

(88,  9, 24, 'GRIP-SER-006', 'Seer POLYMER Hornet Bar Tape 3.5mm',
    '3.5mm polymer hornet-pattern tape for road and gravel bars.',
    'The Seer Polymer Hornet tape features a hexagonal "hornet" texture for enhanced grip and a distinctive look. 3.5mm thickness balances comfort and bar-feel.',
    'Polymer Compound', 'Black / Yellow', NULL, 'Universal', NULL, NULL,
    NULL, NULL, NULL, NULL,
    'Thickness: 3.5mm; Texture: Hornet hexagonal; Length: 2.5m; Includes cork plug and end tape',
    450.00, 18),

(89,  9, 24, 'GRIP-SER-007', 'Seer Super Lite Bar Tape 2mm',
    'Ultra-thin 2mm bar tape for road race builds.',
    'The Seer Super Lite bar tape is designed for road racers seeking a natural bar feel with minimal bulk. At 2mm thick, it delivers a direct connection to the road with minimal hand fatigue on flat courses.',
    'Thin Polymer Compound', 'Black', NULL, 'Universal', NULL, NULL,
    NULL, NULL, NULL, NULL,
    'Thickness: 2mm; Length: 2.5m; Suitable for: Road race; Includes end tape',
    300.00, 20),

(90,  9, 25, 'GRIP-ATK-001', 'Attack Dual Lock-On Handle Grip',
    'Dual lock-on alloy ring grips for trail and enduro MTBs.',
    'The Attack Dual Lock-On grips feature two independent alloy lock rings (inner and outer) for maximum security, eliminating grip rotation during aggressive riding.',
    'Rubber Compound / Dual Alloy Lock Rings', 'Black / Orange', NULL, 'Universal', NULL, NULL,
    NULL, NULL, NULL, NULL,
    'Diameter: 33mm; Length: 130mm; Lock rings: 2x Alloy; Compound: Rubber; Sold as pair',
    370.00, 25),

(91,  9, 10, 'GRIP-WPN-001', 'Weapon Race Palm Rest Lock-On Grip',
    'Ergonomic palm-rest grips with dual lock-on design.',
    'The Weapon Race grips feature a lateral palm rest wing that reduces hand pressure on the ulnar nerve during long trail rides. Dual lock-on alloy rings ensure a firm fit.',
    'Rubber Compound / Alloy Lock Rings', 'Black / Red', NULL, 'Universal', NULL, NULL,
    NULL, NULL, NULL, NULL,
    'Diameter: 34mm (at grip) / 38mm (at palm rest); Length: 140mm; Lock rings: 2x Alloy; Sold as pair',
    400.00, 20),

(92,  9, 10, 'GRIP-WPN-002', 'Weapon Rubix Slip-On Grip',
    'Slip-on rubber grip for BMX and budget trail builds.',
    'The Weapon Rubix is a simple slip-on rubber grip suitable for BMX and budget MTB builds. No lock rings required — installed with grip glue or hairspray.',
    'Rubber Compound', 'Black', NULL, 'Universal', NULL, NULL,
    NULL, NULL, NULL, NULL,
    'Diameter: 30mm; Length: 135mm; Type: Slip-on; Sold as pair; No lock rings',
    200.00, 40),

(93,  9, 10, 'GRIP-WPN-003', 'Weapon Wave Lock-On Grip',
    'Wave-pattern textured lock-on grip for trail MTBs.',
    'The Weapon Wave lock-on grip features a wave-pattern texture for additional grip security in wet or muddy conditions. Single alloy lock ring per side.',
    'Rubber Compound / Alloy Lock Ring', 'Black / Blue', NULL, 'Universal', NULL, NULL,
    NULL, NULL, NULL, NULL,
    'Diameter: 33mm; Length: 130mm; Lock ring: 1x Alloy; Pattern: Wave; Sold as pair',
    350.00, 22);
GO

-- =============================================================================
-- SEED: Products — Category 10: Pedals
-- =============================================================================
INSERT INTO Products (ProductID, CategoryID, BrandID, SKU, ProductName, ShortDescription, FullDescription,
                      Material, Color, WheelSize, SpeedCompatibility, BoostCompatible, TubelessReady,
                      AxleStandard, FrameMaterial, SuspensionTravel, BrakeType, AdditionalSpecs,
                      Price, StockQuantity)
VALUES
(94, 10, 14, 'PEDL-SPD-001', 'Speedone Soldier Platform Pedal',
    'Lightweight alloy platform pedal for trail and everyday MTB use.',
    'The Speedone Soldier platform pedal features a wide, grippy platform with replaceable steel pins. Alloy body keeps weight low while providing a robust platform for trail riding.',
    'CNC Aluminum Alloy 6061', 'Black', NULL, 'Universal', NULL, NULL,
    NULL, NULL, NULL, NULL,
    'Platform: 100x110mm; Pins: Steel replaceable 10 per side; Spindle: Chromoly 9/16"; Bearing: Sealed; Weight: ~390g/pair',
    1200.00, 20),

(95, 10, 14, 'PEDL-SPD-002', 'Speedone Pilot Alloy Platform Pedal',
    'Slim alloy platform pedal — low-profile trail design.',
    'The Speedone Pilot has a thinner profile than the Soldier, reducing foot-to-ground clearance for more confident manuals and technical trail riding.',
    'CNC Aluminum Alloy 6061', 'Black / Silver', NULL, 'Universal', NULL, NULL,
    NULL, NULL, NULL, NULL,
    'Platform: 102x108mm; Thickness: 15mm; Spindle: Cr-Mo 9/16"; Bearing: Sealed; Weight: ~370g/pair',
    1200.00, 18),

(96, 10, 12, 'PEDL-SAG-001', 'Sagmit 610 Alloy Platform Pedal',
    'Budget alloy trail pedal with steel body pins.',
    'The Sagmit 610 provides a value-priced alloy platform for trail and commuter builds. Wide platform and multiple body pins deliver confident traction.',
    'Aluminum Alloy', 'Black', NULL, 'Universal', NULL, NULL,
    NULL, NULL, NULL, NULL,
    'Platform: 100x110mm; Spindle: Steel 9/16"; Pins: 8 per side; Bearing: Sealed; Weight: ~420g/pair',
    800.00, 25),

(97, 10, 12, 'PEDL-SAG-002', 'Sagmit 614 Alloy Platform Pedal',
    'Upgraded alloy trail pedal with more pins for traction.',
    'The Sagmit 614 features more steel pins than the 610 and a slightly wider platform for improved shoe contact and traction on demanding trails.',
    'Aluminum Alloy', 'Black / Red', NULL, 'Universal', NULL, NULL,
    NULL, NULL, NULL, NULL,
    'Platform: 105x112mm; Spindle: Steel 9/16"; Pins: 12 per side; Bearing: Sealed; Weight: ~440g/pair',
    800.00, 20),

(98, 10, 23, 'PEDL-RAG-001', 'Ragusa R700 Composite Platform Pedal',
    'Lightweight composite body trail pedal — entry-level value.',
    'The Ragusa R700 composite platform pedal provides a lightweight entry-level option for trail builds. Composite body reduces weight compared to alloy options while maintaining sufficient rigidity.',
    'Fiber-Reinforced Nylon / Steel Spindle', 'Black', NULL, 'Universal', NULL, NULL,
    NULL, NULL, NULL, NULL,
    'Platform: 98x110mm; Spindle: Steel 9/16"; Bearing: Bushing; Weight: ~310g/pair',
    500.00, 30),

(99, 10, 23, 'PEDL-RAG-002', 'Ragusa CNC 712 Sealed Bearing Platform Pedal',
    'CNC alloy pedal with sealed bearings and replaceable pins.',
    'The Ragusa CNC 712 features a CNC-machined alloy body, sealed cartridge bearings, and replaceable pins for a longer service life. A quality choice for trail riders on a modest budget.',
    'CNC Aluminum Alloy 6061', 'Black', NULL, 'Universal', NULL, NULL,
    NULL, NULL, NULL, NULL,
    'Platform: 102x112mm; Spindle: Cr-Mo 9/16"; Pins: Steel replaceable; Bearing: Sealed cartridge; Weight: ~380g/pair',
    800.00, 20),

(100, 10, 26, 'PEDL-MKS-001', 'MKS GR-9 Alloy Platform Pedal (Japan)',
    'Japanese MKS alloy platform pedal — road touring and commuter.',
    'The MKS GR-9 is a Japanese-made alloy road platform pedal renowned for its smooth sealed-bearing performance and clean aesthetic. A trusted choice for touring cyclists and commuters worldwide.',
    'Aluminum Alloy / Chromoly Spindle', 'Silver', NULL, 'Universal', NULL, NULL,
    NULL, NULL, NULL, NULL,
    'Platform: 93x90mm; Spindle: Cr-Mo 9/16"; Bearing: Sealed ball; Weight: ~345g/pair; Made in Japan',
    1200.00, 12),

(101, 10, 26, 'PEDL-MKS-002', 'MKS Sylvan Touring Platform Pedal (Japan)',
    'Classic MKS touring pedal with dual-sided platform.',
    'The MKS Sylvan Touring is a lightweight, double-sided touring pedal offering an easy stepping-on surface for urban and touring cyclists. Made in Japan with MKS''s characteristic quality and durability.',
    'Aluminum Alloy / Chromoly Spindle', 'Silver', NULL, 'Universal', NULL, NULL,
    NULL, NULL, NULL, NULL,
    'Platform: Dual-sided 92x90mm; Spindle: Cr-Mo 9/16"; Bearing: Sealed; Weight: ~330g/pair; Made in Japan',
    1400.00, 10),

(102, 10, 26, 'PEDL-MKS-003', 'MKS BM7 BMX / Freestyle Platform Pedal',
    'Wide MKS alloy BMX pedal — durable and lightweight.',
    'The MKS BM7 is a wide-body BMX platform pedal delivering aggressive grip and MKS''s Japanese quality. Suitable for BMX, dirt jumping, and freestyle riding.',
    'Aluminum Alloy / Steel Spindle', 'Black', NULL, 'Universal', NULL, NULL,
    NULL, NULL, NULL, NULL,
    'Platform: 115x108mm; Spindle: Steel 9/16"; Pins: Fixed; Bearing: Ball; Weight: ~420g/pair; Made in Japan',
    1550.00, 8);
GO

-- =============================================================================
-- SEED: Products — Category 11: Rims & Wheelsets
-- =============================================================================
INSERT INTO Products (ProductID, CategoryID, BrandID, SKU, ProductName, ShortDescription, FullDescription,
                      Material, Color, WheelSize, SpeedCompatibility, BoostCompatible, TubelessReady,
                      AxleStandard, FrameMaterial, SuspensionTravel, BrakeType, AdditionalSpecs,
                      Price, StockQuantity)
VALUES
(103, 11, 17, 'RIM-LDC-001', 'LDCNC RS300 700c Rim Brake Wheelset',
    'Complete CNC 700c rim-brake wheelset for road and gravel builds.',
    'The LDCNC RS300 is a complete 700c wheelset designed for rim-brake road and gravel bikes. CNC-machined brake tracks provide consistent braking, and the double-wall alloy rim construction delivers durability for long-distance riding.',
    'CNC Aluminum Alloy Double Wall', 'Silver / Black', '700c', 'Universal', 0, 0,
    'Quick Release 9x100mm (F) / 9x130mm (R)', NULL, NULL, 'Rim Brake',
    'ERD: 584mm; Holes: 32H; Rim depth: 25mm; Rim width: 22mm; Sold as complete wheelset',
    5100.00, 5),

(104, 11, 12, 'RIM-SAG-001', 'Sagmit Aero Double Wall Rim 27.5"',
    '27.5" double-wall alloy rim with aero profile — single rim.',
    'The Sagmit Aero is a 27.5" double-wall alloy rim with an aerodynamic profile and CNC brake track. Suitable for disc and rim-brake builds in 32H lacing.',
    'Aluminum Alloy 6061 Double Wall', 'Black', '27.5"', 'Universal', NULL, 0,
    NULL, NULL, NULL, NULL,
    'ERD: 559mm; Holes: 32H; Rim depth: 28mm; Rim width: 25mm; Finish: Matte black; Single rim',
    1500.00, 20),

(105, 11, 12, 'RIM-SAG-002', 'Sagmit Legend M32 Double Wall Rim 29"',
    '29" double-wall alloy rim — strong and lightweight.',
    'The Sagmit Legend M32 is a 29" double-wall alloy rim suited for trail and XC applications. Reliable for disc brake systems and compatible with tubeless setups with rim tape.',
    'Aluminum Alloy Double Wall', 'Black', '29"', 'Universal', NULL, 0,
    NULL, NULL, NULL, NULL,
    'ERD: 584mm; Holes: 32H; Width: 25mm; Depth: 25mm; Single rim',
    1600.00, 18),

(106, 11, 30, 'RIM-KOR-001', 'Kore Realm 4.2 Plus Rim 27.5"',
    '27.5+ wide alloy rim for fat-contact plus tires.',
    'The Kore Realm 4.2 is a 42mm internal width rim designed for 27.5+ trail tires. The extra-wide rim profile supports a large tire footprint and lower pressure for improved traction on loose terrain.',
    'Aluminum Alloy Double Wall', 'Black', '27.5+"', 'Universal', NULL, 1,
    NULL, NULL, NULL, NULL,
    'ERD: 559mm; Internal width: 42mm; Holes: 32H; Tubeless-ready; Depth: 25mm; Single rim',
    1100.00, 12),

(107, 11, 27, 'RIM-WTB-001', 'WTB i21 TCS Tubeless Ready 27.5" Rim',
    'WTB i21 tubeless-ready trail rim — 21mm internal width.',
    'The WTB i21 TCS rim is a tubeless-compatible XC/trail rim with a 21mm internal width. Lightweight and stiff, it is ideal for fast XC builds and light trail riding with a tubeless tire setup.',
    'Aluminum Alloy', 'Black', '27.5"', 'Universal', NULL, 1,
    NULL, NULL, NULL, NULL,
    'ERD: 559mm; Internal width: 21mm; Holes: 28H / 32H; TCS tubeless-ready; Single rim',
    1600.00, 15),

(108, 11, 27, 'RIM-WTB-002', 'WTB i25 TCS Tubeless Ready 29" Rim',
    'WTB i25 tubeless-ready trail rim — 25mm internal width.',
    'The WTB i25 TCS provides a 25mm internal width, suitable for trail-to-enduro tire widths from 2.2" to 2.6". Tubeless-compatible hooked rim profile.',
    'Aluminum Alloy', 'Black', '29"', 'Universal', NULL, 1,
    NULL, NULL, NULL, NULL,
    'ERD: 584mm; Internal width: 25mm; Holes: 32H; TCS tubeless-ready; Single rim',
    1600.00, 14),

(109, 11, 28, 'RIM-AMC-001', 'American Classic Feldspar 290 Tubeless Ready Rim 29"',
    'Lightweight American Classic 29" tubeless-ready XC rim.',
    'The American Classic Feldspar 290 is a lightweight tubeless-ready 29" rim engineered for XC race and marathon riding. Feldspar technology provides a reinforced bead hook for reliable tubeless seating.',
    'Aluminum Alloy', 'Black / Silver', '29"', 'Universal', NULL, 1,
    NULL, NULL, NULL, NULL,
    'ERD: 584mm; Internal width: 23mm; Holes: 24H; Tubeless-ready; Weight: ~400g; Single rim',
    1800.00, 8),

(110, 11, 29, 'RIM-JAL-001', 'Jalco Wellington Tubeless Ready 27.5" Rim',
    'Tubeless-ready 27.5" alloy rim with CNC sidewall.',
    'The Jalco Wellington is a tubeless-compatible 27.5" double-wall rim featuring CNC-machined sidewalls for precise disc brake performance. A reliable tubeless platform for trail builds.',
    'Aluminum Alloy Double Wall CNC', 'Black', '27.5"', 'Universal', NULL, 1,
    NULL, NULL, NULL, NULL,
    'ERD: 559mm; Internal width: 25mm; Holes: 32H; CNC sidewall; Tubeless-ready; Single rim',
    1400.00, 12),

(111, 11, 14, 'RIM-SPD-001', 'Speedone Pilot Double Wall Rim 700c',
    '700c double-wall alloy rim for road and gravel builds.',
    'The Speedone Pilot 700c is a double-wall alloy rim designed for road, gravel, and CX builds. Paired with matching Speedone hubs for a complete wheelset build.',
    'Aluminum Alloy Double Wall', 'Black', '700c', 'Universal', NULL, 0,
    NULL, NULL, NULL, NULL,
    'ERD: 622mm; Holes: 32H; Width: 22mm; Finish: Matte black; Single rim',
    2000.00, 10),

(112, 11, 14, 'RIM-SPD-002', 'Speedone Bazooka Wide Rim 27.5"',
    '27.5" wide-profile alloy rim for aggressive trail and enduro.',
    'The Speedone Bazooka features a wide 30mm internal width for improved tire support on aggressive trail tires up to 2.6". Built from double-wall alloy for durability under hard riding conditions.',
    'Aluminum Alloy Double Wall', 'Black', '27.5"', 'Universal', NULL, 1,
    NULL, NULL, NULL, NULL,
    'ERD: 559mm; Internal width: 30mm; Holes: 32H; Tubeless-ready; Single rim',
    1700.00, 10),

(113, 11, 14, 'RIM-SPD-003', 'Speedone Soldier Double Wall Rim 29"',
    '29" double-wall alloy trail rim — durable and affordable.',
    'The Speedone Soldier 29" is a sturdy double-wall alloy rim built for trail and light enduro riding. Compatible with disc brakes and tubeless conversion with rim tape and sealant.',
    'Aluminum Alloy Double Wall', 'Black', '29"', 'Universal', NULL, 0,
    NULL, NULL, NULL, NULL,
    'ERD: 584mm; Internal width: 25mm; Holes: 32H; Single rim',
    1900.00, 8),

(114, 11, 12, 'RIM-SAG-003', 'Sagmit Safarri Wide Alloy Rim 27.5"',
    '27.5" wide alloy rim for trail tire widths up to 2.6".',
    'The Sagmit Safarri is a wide-profile trail rim suited for tires up to 2.6". Designed for tubeless setup with rim tape, it provides a stable bead for low-pressure trail riding.',
    'Aluminum Alloy Double Wall', 'Black', '27.5"', 'Universal', NULL, 1,
    NULL, NULL, NULL, NULL,
    'ERD: 559mm; Internal width: 28mm; Holes: 32H; Single rim',
    1700.00, 12),

(115, 11, 29, 'RIM-JAL-001B', 'Jalco Maranello Double Wall Rim 700c',
    '700c double-wall alloy rim for road and commuter builds.',
    'The Jalco Maranello 700c is a dependable double-wall road rim with a CNC brake track for reliable stopping with rim-brake calipers. A popular OEM-spec road rim.',
    'Aluminum Alloy Double Wall CNC', 'Silver', '700c', 'Universal', NULL, 0,
    NULL, NULL, NULL, NULL,
    'ERD: 622mm; Holes: 32H; Width: 22mm; CNC sidewall; Single rim',
    1500.00, 14),

(116, 11, 30, 'RIM-KOR-002', 'Kore Realm 2.4 Alloy Rim 27.5"',
    '27.5" mid-width alloy trail rim — 24mm internal width.',
    'The Kore Realm 2.4 is a 24mm internal-width trail rim balancing weight and tire support. Suitable for tires from 2.0" to 2.4" and disc brake drivetrains.',
    'Aluminum Alloy Double Wall', 'Black', '27.5"', 'Universal', NULL, 0,
    NULL, NULL, NULL, NULL,
    'ERD: 559mm; Internal width: 24mm; Holes: 32H; Single rim',
    1200.00, 15);
GO

-- =============================================================================
-- SEED: Products — Category 12: Tires & Tubes
-- =============================================================================
INSERT INTO Products (ProductID, CategoryID, BrandID, SKU, ProductName, ShortDescription, FullDescription,
                      Material, Color, WheelSize, SpeedCompatibility, BoostCompatible, TubelessReady,
                      AxleStandard, FrameMaterial, SuspensionTravel, BrakeType, AdditionalSpecs,
                      Price, StockQuantity)
VALUES
(117, 12, 31, 'TIRE-MAX-001', 'Maxxis Ikon 27.5 x 2.20 TanWall',
    'Maxxis Ikon XC/trail tire — 27.5" 2.20" with tan sidewall.',
    'The Maxxis Ikon is a versatile XC-to-trail tire known for fast rolling and reliable cornering grip. The tan sidewall (TanWall) version provides a classic aesthetic and slightly lighter weight.',
    'Dual Compound Rubber / Tanwall Casing', 'Black / Tan', '27.5"', 'Universal', NULL, 1,
    NULL, NULL, NULL, NULL,
    'Size: 27.5x2.20; Compound: Dual EXO; TPI: 60; Tubeless: EXO-ready; Bead: Folding; Weight: ~670g',
    1500.00, 20),

(118, 12, 31, 'TIRE-MAX-002', 'Maxxis Ardent 29 x 2.25 TanWall',
    'Maxxis Ardent trail tire — 29" 2.25" fast-rolling with tan sidewall.',
    'The Maxxis Ardent 29" delivers fast rolling on hard-pack and reliable mid-corner grip on loose terrain. The 2.25 width suits trail and XC riders needing a versatile tire for varied conditions.',
    'Dual Compound Rubber / Tanwall Casing', 'Black / Tan', '29"', 'Universal', NULL, 1,
    NULL, NULL, NULL, NULL,
    'Size: 29x2.25; TPI: 60; Compound: Dual; EXO sidewall protection; Tubeless-ready; Folding bead; Weight: ~740g',
    1500.00, 18),

(119, 12, 31, 'TIRE-MAX-003', 'Maxxis Ikon 29 x 2.20',
    'Maxxis Ikon XC tire — 29" 2.20" black sidewall.',
    'The Maxxis Ikon 29x2.20 (black sidewall) is a lightweight XC race tire offering fast rolling and adequate cornering grip for hardpack and mixed terrain. The black sidewall version has slightly more casing protection than the TanWall.',
    'Dual Compound Rubber / Black Casing', 'Black', '29"', 'Universal', NULL, 1,
    NULL, NULL, NULL, NULL,
    'Size: 29x2.20; TPI: 60; Compound: Dual; EXO; Tubeless-ready; Weight: ~680g',
    1500.00, 16),

(120, 12, 31, 'TIRE-MAX-004', 'Maxxis Crossmark II 27.5 x 2.25',
    'Maxxis Crossmark II trail tire — 27.5" 2.25" fast-rolling.',
    'The Maxxis Crossmark II is a fast-rolling trail tire with an updated tread pattern over the original Crossmark. Wide corner knobs provide confident lean-angle grip on loose, dry trails.',
    'Dual Compound Rubber', 'Black', '27.5"', 'Universal', NULL, 1,
    NULL, NULL, NULL, NULL,
    'Size: 27.5x2.25; TPI: 60; EXO; Tubeless-ready; Weight: ~700g',
    1500.00, 18),

(121, 12, 31, 'TIRE-MAX-005', 'Maxxis Ikon 29 x 2.20 TanWall',
    'Maxxis Ikon XC/trail tire — 29" 2.20" tan sidewall edition.',
    'The Maxxis Ikon 29x2.20 TanWall combines the fast rolling Ikon tread with the classic tan sidewall aesthetic. A popular choice for gravel and XC riders wanting style alongside performance.',
    'Dual Compound Rubber / Tanwall Casing', 'Black / Tan', '29"', 'Universal', NULL, 1,
    NULL, NULL, NULL, NULL,
    'Size: 29x2.20; TPI: 60; EXO; Tubeless-ready; Folding bead; Weight: ~660g',
    1500.00, 15),

(122, 12, 31, 'TIRE-MAX-006', 'Maxxis Rekon Race 27.5 x 2.25',
    'Maxxis Rekon Race XC-race tire — 27.5" 2.25" lightweight.',
    'The Maxxis Rekon Race is a high-performance XC race tire designed for fast-paced competitive MTB. Low-profile center knobs deliver minimal rolling resistance, while shoulder knobs ensure confident cornering at speed.',
    'MaxxSpeed Dual Compound / Exo+ Casing', 'Black', '27.5"', 'Universal', NULL, 1,
    NULL, NULL, NULL, NULL,
    'Size: 27.5x2.25; TPI: 120; Compound: MaxxSpeed; EXO+; Tubeless-ready; Weight: ~625g',
    1500.00, 12),

(123, 12, 31, 'TIRE-MAX-007', 'Maxxis Ardent 27.5 x 2.25',
    'Maxxis Ardent trail tire — 27.5" 2.25" versatile all-rounder.',
    'The Maxxis Ardent 27.5x2.25 is a go-to trail tire for mixed conditions. Fast enough for XC-influenced rides and grippy enough for loose descents, it is one of Maxxis''s most popular trail options.',
    'Dual Compound Rubber / EXO Casing', 'Black', '27.5"', 'Universal', NULL, 1,
    NULL, NULL, NULL, NULL,
    'Size: 27.5x2.25; TPI: 60; Compound: Dual; EXO; Tubeless-ready; Weight: ~720g',
    1500.00, 18),

(124, 12, 31, 'TIRE-MAX-008', 'Maxxis Ardent Race 27.5 x 2.20',
    'Maxxis Ardent Race XC-trail tire — 27.5" 2.20" lightweight.',
    'The Maxxis Ardent Race 27.5x2.20 bridges XC and trail with a faster-rolling profile and lightweight Exo+ casing. Suitable for riders wanting trail capability with minimal weight penalty.',
    'MaxxSpeed Dual Compound / EXO+ Casing', 'Black', '27.5"', 'Universal', NULL, 1,
    NULL, NULL, NULL, NULL,
    'Size: 27.5x2.20; TPI: 120; MaxxSpeed; EXO+; Tubeless-ready; Weight: ~640g',
    1500.00, 14),

(125, 12, 31, 'TUBE-MAX-001', 'Maxxis MTB Inner Tube 27.5/29 Presta',
    'Maxxis MTB inner tube for 27.5" and 29" tires — Presta valve.',
    'The Maxxis MTB inner tube is made from premium butyl rubber for reliable performance and puncture resistance. Compatible with 27.5" and 29" tires. Presta valve for use with road and tubeless-compatible rims.',
    'Butyl Rubber', 'Black', '27.5" / 29"', 'Universal', NULL, NULL,
    NULL, NULL, NULL, NULL,
    'Valve: Presta 48mm; Size: 26/27.5/29 x 1.9–2.35; Material: Butyl; Valve: Removable core',
    250.00, 40),

(126, 12, 12, 'TUBE-SAG-001', 'Sagmit MTB Inner Tube 27.5/29 Schrader',
    'Sagmit inner tube for MTB — Schrader valve.',
    'The Sagmit MTB inner tube is a value option for trail builds using standard Schrader (car-type) valves. Compatible with most 27.5" and 29" MTB rims with standard valve holes.',
    'Butyl Rubber', 'Black', '27.5" / 29"', 'Universal', NULL, NULL,
    NULL, NULL, NULL, NULL,
    'Valve: Schrader 33mm; Size: 27.5/29 x 1.9–2.35; Material: Butyl',
    150.00, 50),

(127, 12, 32, 'TUBE-LEO-001', 'Leo Inner Tube Size 20" BMX',
    'Leo inner tube for 20" BMX and kids bikes.',
    'The Leo 20" inner tube is a durable Schrader-valve tube designed for BMX and children''s bikes. Made from standard butyl rubber for good air retention.',
    'Butyl Rubber', 'Black', '20"', 'Universal', NULL, NULL,
    NULL, NULL, NULL, NULL,
    'Valve: Schrader; Size: 20 x 1.75–2.125; Material: Butyl',
    85.00, 60),

(128, 12, 32, 'TUBE-LEO-002', 'Leo Inner Tube Size 26"',
    'Leo inner tube for 26" MTB and commuter bikes.',
    'The Leo 26" inner tube is compatible with most 26" mountain, city, and hybrid bikes. Schrader valve for easy inflation at any service station.',
    'Butyl Rubber', 'Black', '26"', 'Universal', NULL, NULL,
    NULL, NULL, NULL, NULL,
    'Valve: Schrader; Size: 26 x 1.75–2.125; Material: Butyl',
    100.00, 50),

(129, 12, 32, 'TIRE-LEO-001', 'Leo Tire Size 20" BMX',
    'Leo BMX tire — 20" for freestyle, street, and kids bikes.',
    'The Leo 20" BMX tire is a durable street and park tire for 20" BMX and children''s bikes. Standard knobbed tread for mixed surface traction.',
    'Rubber Compound', 'Black', '20"', 'Universal', NULL, NULL,
    NULL, NULL, NULL, NULL,
    'Size: 20 x 2.125; TPI: 30; Bead: Wire; Tread: Knobbed; Weight: ~500g',
    380.00, 30),

(130, 12, 32, 'TIRE-LEO-002', 'Leo Tire Size 16"',
    'Leo 16" tire for children''s bikes and small-wheel folding bikes.',
    'The Leo 16" tire is suitable for children''s bicycles and small-wheel city bikes. Standard knobbed tread provides traction on paved roads and light gravel.',
    'Rubber Compound', 'Black', '16"', 'Universal', NULL, NULL,
    NULL, NULL, NULL, NULL,
    'Size: 16 x 1.75; TPI: 30; Bead: Wire; Weight: ~370g',
    280.00, 35),

(131, 12, 32, 'TUBE-LEO-003', 'Leo Inner Tube Size 16"',
    'Leo inner tube for 16" children''s bikes.',
    'The Leo 16" inner tube is a standard butyl tube for 16" children''s and folding bikes. Schrader valve.',
    'Butyl Rubber', 'Black', '16"', 'Universal', NULL, NULL,
    NULL, NULL, NULL, NULL,
    'Valve: Schrader; Size: 16 x 1.75–2.125; Material: Butyl',
    85.00, 60);
GO

-- =============================================================================
-- SEED: Products — Category 13: Chains
-- =============================================================================
INSERT INTO Products (ProductID, CategoryID, BrandID, SKU, ProductName, ShortDescription, FullDescription,
                      Material, Color, WheelSize, SpeedCompatibility, BoostCompatible, TubelessReady,
                      AxleStandard, FrameMaterial, SuspensionTravel, BrakeType, AdditionalSpecs,
                      Price, StockQuantity)
VALUES
(132, 13, 33, 'CHAIN-SUM-001', 'SUMC CP 8-Speed Chain w/ Missing Link 116L',
    '116-link 8-speed standard finish chain with quick-link.',
    'The SUMC CP 8-speed chain features a CP (chrome plated) finish for corrosion resistance and smooth shifting. Includes a quick-release missing link for easy removal and reinstallation.',
    'Steel / Chrome Plated', 'Silver', NULL, '8-speed', NULL, NULL,
    NULL, NULL, NULL, NULL,
    'Links: 116; Speed: 8s; Finish: Chrome Plated; Inner width: 2.38mm; Pin: Solid; Includes missing link',
    300.00, 30),

(133, 13, 33, 'CHAIN-SUM-002', 'SUMC CP 9-Speed Chain w/ Missing Link 116L',
    '116-link 9-speed chrome-plated chain with quick-link.',
    'The SUMC CP 9-speed chain provides reliable shifting across 9-speed drivetrains with a chrome-plated finish for durability. Includes missing link.',
    'Steel / Chrome Plated', 'Silver', NULL, '9-speed', NULL, NULL,
    NULL, NULL, NULL, NULL,
    'Links: 116; Speed: 9s; Finish: Chrome Plated; Inner width: 2.18mm; Includes missing link',
    380.00, 28),

(134, 13, 33, 'CHAIN-SUM-003', 'SUMC CP 10-Speed Chain w/ Missing Link 116L',
    '116-link 10-speed chrome-plated chain with quick-link.',
    'The SUMC CP 10-speed chain is narrowed to suit 10-speed drivetrains with compatible cassettes. Chrome plated finish for smooth, low-friction shifting.',
    'Steel / Chrome Plated', 'Silver', NULL, '10-speed', NULL, NULL,
    NULL, NULL, NULL, NULL,
    'Links: 116; Speed: 10s; Finish: Chrome Plated; Inner width: 1.96mm; Includes missing link',
    450.00, 25),

(135, 13, 33, 'CHAIN-SUM-004', 'SUMC CP 11-Speed Chain w/ Missing Link 116L',
    '116-link 11-speed chrome-plated chain with quick-link.',
    'The SUMC CP 11-speed chain is compatible with Shimano HG, SRAM, and Campagnolo 11-speed systems. Chrome plated finish and hollow inner plates reduce weight.',
    'Steel / Chrome Plated / Hollow Plates', 'Silver', NULL, '11-speed', NULL, NULL,
    NULL, NULL, NULL, NULL,
    'Links: 116; Speed: 11s; Finish: Chrome Plated; Hollow outer plates; Includes missing link',
    650.00, 20),

(136, 13, 33, 'CHAIN-SUM-005', 'SUMC CP 12-Speed Chain w/ Missing Link 116L',
    '116-link 12-speed chrome-plated chain with quick-link.',
    'The SUMC CP 12-speed chain provides compatibility with Shimano 12s (Micro Spline) and SRAM 12s (XD / HG) drivetrains. Chrome plated with hollow outer plates for weight savings.',
    'Steel / Chrome Plated / Hollow Plates', 'Silver', NULL, '12-speed', NULL, NULL,
    NULL, NULL, NULL, NULL,
    'Links: 116; Speed: 12s; Finish: Chrome Plated; Hollow outer plates; Includes missing link',
    1050.00, 18),

(137, 13, 33, 'CHAIN-SUM-006', 'SUMC Oilslick 8-Speed Chain 116L w/ Missing Link',
    '116-link 8-speed oil-slick iridescent finish chain.',
    'The SUMC Oilslick 8-speed chain features a striking iridescent oil-slick finish achieved through titanium nitride coating. Provides added corrosion resistance alongside a visually unique aesthetic.',
    'Steel / Titanium Nitride Coated', 'Oilslick Iridescent', NULL, '8-speed', NULL, NULL,
    NULL, NULL, NULL, NULL,
    'Links: 116; Speed: 8s; Finish: TiN Oilslick; Inner width: 2.38mm; Includes missing link',
    450.00, 20),

(138, 13, 33, 'CHAIN-SUM-007', 'SUMC Oilslick 10-Speed Chain 116L w/ Missing Link',
    '116-link 10-speed oil-slick titanium-nitride coated chain.',
    'The SUMC Oilslick 10-speed chain delivers corrosion resistance and the iconic iridescent look through its titanium nitride coating. Includes a matching oilslick missing link.',
    'Steel / Titanium Nitride Coated', 'Oilslick Iridescent', NULL, '10-speed', NULL, NULL,
    NULL, NULL, NULL, NULL,
    'Links: 116; Speed: 10s; Finish: TiN Oilslick; Includes missing link',
    750.00, 18),

(139, 13, 33, 'CHAIN-SUM-008', 'SUMC Oilslick 11-Speed Chain 116L w/ Missing Link',
    '116-link 11-speed oil-slick titanium-nitride coated chain.',
    'The SUMC Oilslick 11-speed chain is compatible with all major 11s groupsets and features the eye-catching TiN oilslick finish. Includes oilslick missing link.',
    'Steel / Titanium Nitride Coated', 'Oilslick Iridescent', NULL, '11-speed', NULL, NULL,
    NULL, NULL, NULL, NULL,
    'Links: 116; Speed: 11s; Finish: TiN Oilslick; Hollow plates; Includes missing link',
    1200.00, 15),

(140, 13, 34, 'CHAIN-KMC-001', 'KMC X12 Grey 12-Speed Chain 116L',
    'KMC X12 Series 12-speed chain — grey finish, 116 links.',
    'The KMC X-12 is KMC''s standard 12-speed chain compatible with Shimano 12s and SRAM 12s Eagle drivetrains. The grey finish offers a subtle look with solid corrosion protection.',
    'High Tensile Steel / Grey Nickel Plated', 'Grey', NULL, '12-speed', NULL, NULL,
    NULL, NULL, NULL, NULL,
    'Links: 116; Speed: 12s; Width: 5.2mm; Finish: Grey Nickel; Includes missing link CL562R; Weight: ~260g',
    1500.00, 15),

(141, 13, 34, 'CHAIN-KMC-002', 'KMC X11 Grey 11-Speed Chain 116L',
    'KMC X11 Series 11-speed chain — grey finish, 116 links.',
    'The KMC X-11 Grey is a smooth-shifting 11-speed chain compatible with Shimano, SRAM, and Campagnolo 11s groupsets. Nickel-plated for corrosion resistance.',
    'High Tensile Steel / Grey Nickel Plated', 'Grey', NULL, '11-speed', NULL, NULL,
    NULL, NULL, NULL, NULL,
    'Links: 116; Speed: 11s; Width: 5.5mm; Finish: Grey Nickel; Includes missing link CL551R; Weight: ~252g',
    1100.00, 18),

(142, 13, 34, 'CHAIN-KMC-003', 'KMC X10 Grey 10-Speed Chain 116L',
    'KMC X10 Series 10-speed chain — grey finish, 116 links.',
    'The KMC X-10 Grey is a trusted 10-speed chain for MTB and road groupsets. Nickel-plated outer plates and smooth inner links deliver reliable shifting performance.',
    'High Tensile Steel / Grey Nickel Plated', 'Grey', NULL, '10-speed', NULL, NULL,
    NULL, NULL, NULL, NULL,
    'Links: 116; Speed: 10s; Width: 5.9mm; Finish: Grey Nickel; Includes missing link CL547R; Weight: ~246g',
    900.00, 20),

(143, 13, 34, 'CHAIN-KMC-004', 'KMC X9 Gold 9-Speed Chain 116L',
    'KMC X9 Series 9-speed chain — gold finish, 116 links.',
    'The KMC X-9 Gold is a premium-looking 9-speed chain with a gold-colored zinc plating for a distinctive build. Suitable for all 9-speed drivetrains from Shimano, SRAM, and Campagnolo.',
    'High Tensile Steel / Gold Zinc Plated', 'Gold', NULL, '9-speed', NULL, NULL,
    NULL, NULL, NULL, NULL,
    'Links: 116; Speed: 9s; Width: 6.6mm; Finish: Gold Zinc; Includes missing link CL531G; Weight: ~283g',
    1800.00, 15),

(144, 13, 34, 'CHAIN-KMC-005', 'KMC X10 Gold 10-Speed Chain 116L',
    'KMC X10 Series 10-speed chain — gold finish, 116 links.',
    'The KMC X-10 Gold provides the same reliable 10-speed shifting as the Grey version with the premium aesthetic of a gold zinc-plated finish.',
    'High Tensile Steel / Gold Zinc Plated', 'Gold', NULL, '10-speed', NULL, NULL,
    NULL, NULL, NULL, NULL,
    'Links: 116; Speed: 10s; Width: 5.9mm; Finish: Gold Zinc; Includes missing link; Weight: ~248g',
    1800.00, 12),

(145, 13, 34, 'CHAIN-KMC-006', 'KMC X12 Gold 12-Speed Chain 116L',
    'KMC X12 Series 12-speed chain — premium gold finish, 116 links.',
    'The KMC X-12 Gold brings a premium gold finish to KMC''s flagship 12-speed chain. Compatible with all major 12-speed drivetrains (Shimano, SRAM Eagle, Campagnolo).',
    'High Tensile Steel / Gold Zinc Plated', 'Gold', NULL, '12-speed', NULL, NULL,
    NULL, NULL, NULL, NULL,
    'Links: 116; Speed: 12s; Width: 5.2mm; Finish: Gold Zinc; Includes missing link; Weight: ~262g',
    2400.00, 10),

(146, 13, 35, 'CHAIN-CTX-001', 'CT-1HX 116L Heavy Duty Chain for Fixie / Single Speed',
    'Heavy-duty 1-speed chain for fixie and single-speed bikes.',
    'The CT-1HX is a reinforced single-speed chain designed for fixie, fixed-gear, and single-speed drivetrains. The wider inner plate provides increased lateral stiffness under high torque.',
    'High Carbon Steel / Nickel Plated', 'Silver', NULL, '1-speed / Fixie', NULL, NULL,
    NULL, NULL, NULL, NULL,
    'Links: 116; Width: 1/8"; Pitch: 1/2"; Finish: Nickel Plated; For: Fixie / SS / BMX',
    350.00, 30),

(147, 13, 36, 'CHAIN-GTX-001', 'GT-10 126-Link 10-Speed Standard Chain',
    '126-link standard 10-speed chain for MTB and road.',
    'The GT-10 is a standard 10-speed chain offering 126 links for longer builds or those who prefer to cut to exact length. Nickel-plated for corrosion resistance.',
    'High Tensile Steel / Nickel Plated', 'Silver', NULL, '10-speed', NULL, NULL,
    NULL, NULL, NULL, NULL,
    'Links: 126; Speed: 10s; Inner width: 1.96mm; Finish: Nickel Plated; No missing link included',
    500.00, 22),

(148, 13, 36, 'CHAIN-GTX-002', 'GT-11 126-Link 11-Speed Standard Chain',
    '126-link standard 11-speed chain for MTB and road.',
    'The GT-11 is a standard 11-speed chain with 126 links for builds requiring extra link count. Compatible with major 11-speed groupsets from Shimano and SRAM.',
    'High Tensile Steel / Nickel Plated', 'Silver', NULL, '11-speed', NULL, NULL,
    NULL, NULL, NULL, NULL,
    'Links: 126; Speed: 11s; Inner width: 1.78mm; Finish: Nickel Plated; No missing link included',
    700.00, 18);
GO

SET IDENTITY_INSERT Products OFF;
GO

PRINT 'Seed data inserted successfully.';
PRINT 'Categories: 13 records';
PRINT 'Brands:     43 records';
PRINT 'Products:   148 records';
GO
