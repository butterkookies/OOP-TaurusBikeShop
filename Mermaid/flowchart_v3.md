# Taurus Bike Shop Flowchart

```mermaid
%%{init: {'flowchart': {'curve': 'linear'}}}%%

flowchart TD

%% =========================
%% DATABASES / EXTERNAL
%% =========================

DB1[(User Database)]
DB2[(Product Database)]
DB3[(Order Database)]
DB4[(Inventory Database)]

EXT1[[GCash Payment Gateway]]

%% =========================
%% USER JOURNEY
%% =========================

A0([Start]) --> A1[/Open App/]
A1 --> A2{Have Account?}

%% SIGNUP
A2 -->|No| A3[/Enter Email & Password/]
A3 --> A4[System Sends OTP]
A4 --> A5[/Enter OTP/]
A5 --> A6{OTP Valid?}

A6 -->|No| A5
A6 -->|Yes| A7[Create Account]
A7 --> DB1

%% LOGIN
A2 -->|Yes| A8[/Enter Credentials/]
A8 --> A11{Credentials Valid?}

DB1 --> A11
A11 --> DB1

A11 -->|No| A8
A11 -->|Yes| A12[Login Success]

%% SHOPPING
A12 --> B1[/Display Product Catalog/]
B1 --> DB2
DB2 --> B2[/Select Product/]

B2 --> DB4
DB4 --> BINV{Item In Stock?}

BINV -->|No| OUT[/Display Out of Stock/]
OUT --> B1

BINV -->|Yes| B3[/Display Product Details/]

B3 --> B4{Action}
B4 -->|Add to Cart| B5[Add Item to Cart]
B5 --> B6{Continue Shopping?}

B6 -->|Yes| B1
B6 -->|Checkout| B7[/Open Cart/]
B4 -->|Buy Now| B7

%% CHECKOUT
B7 --> B8[/Display Cart & Review Order/]
B8 --> B9[/Proceed to Checkout/]
B9 --> B10[/Select Payment Method/]

%% PAYMENT TYPES
B10 -->|Cash on Delivery| B14[Choose Delivery Option]
B10 -->|GCash| B11[Redirect to Payment Gateway]
B11 --> EXT1
EXT1 --> B12{Payment Successful?}

B12 -->|No| B13{Retry Payment?}
B13 -->|Yes| B10
B13 -->|Cancel| Z1([Order Cancelled])
B12 -->|Yes| B14

%% DELIVERY / PICKUP DECISION
B14 --> B20{Pickup or Delivery?}
B20 -->|Pickup| B21[Submit Order for Pickup]
B20 -->|Delivery| B22[/Select Delivery Company: LBC or Lalamove/]
B22 --> B23[/Process Delivery Payment/]
B23 --> B21

%% ORDER PROCESS
B21 --> B15[Create Order Record]
B15 --> DB3
DB3 --> B16[Order Status: Pending]
B16 --> B17[Order Processing]
B17 --> B18[/Display Order Confirmation/]
B18 --> B19([Order Completed])

%% =========================
%% ADMIN JOURNEY
%% =========================

C0([Admin Start]) --> C1[/Enter Admin Credentials/]
C1 --> C2{Credentials Valid?}
C2 --> DB1
C2 -->|No| C1
C2 -->|Yes| C3[/Display Admin Dashboard/]

C3 --> C4{Select Module}
C4 -->|Orders| D1
C4 -->|Order History| E1
C4 -->|Shop Overview| F1
C4 -->|Logout| C5([End])

%% =========================
%% ORDERS MANAGEMENT
%% =========================

D1[/Display Active Orders/]
D1 --> DB3
DB3 --> D2[/Select Order/]

D2 --> D3{Update Status}
D3 -->|Processing| D4
D3 -->|Packed| D4
D3 -->|Out for Delivery| D4
D3 -->|Delivered| D4
D3 -->|Cancelled| D4

D4[Save Status Update]
D4 --> DB3
DB3 --> C3

%% =========================
%% ORDER HISTORY
%% =========================

E1[/Display Order History/]
E1 --> DB3
DB3 --> E2{Sort By}

E2 -->|Latest| E3[/Sorted Latest/]
E2 -->|Oldest| E4[/Sorted Oldest/]
E2 -->|Customer Name| E5[/Sorted by Name/]

E3 --> E6[/View Order Details/]
E4 --> E6
E5 --> E6
E6 --> C3

%% =========================
%% SHOP OVERVIEW
%% =========================

F1[/Display Shop Analytics/]
F1 --> DB2
F1 --> DB3
F1 --> DB4
F1 --> C3

%% =========================
%% COLOR DEFINITIONS
%% =========================

classDef startend fill:#7ED957,stroke:#2E7D32,stroke-width:2px,color:#000
classDef process fill:#BBDEFB,stroke:#1E88E5,stroke-width:2px,color:#000
classDef decision fill:#FFF59D,stroke:#F9A825,stroke-width:2px,color:#000
classDef io fill:#FFE0B2,stroke:#EF6C00,stroke-width:2px,color:#000
classDef database fill:#E0E0E0,stroke:#616161,stroke-width:2px,color:#000
classDef external fill:#D1C4E9,stroke:#5E35B1,stroke-width:2px,color:#000
classDef admin fill:#B2DFDB,stroke:#00695C,stroke-width:2px,color:#000
classDef order fill:#F8BBD0,stroke:#AD1457,stroke-width:2px,color:#000
classDef cancel fill:#FFCDD2,stroke:#C62828,stroke-width:2px,color:#000

%% =========================
%% CLASS ASSIGNMENTS
%% =========================

class A0,B19,C0,C5 startend
class A4,A7,A8,A12,B5,B15,B16,B17,D4 process
class A2,A6,A11,B4,B6,B12,B13,BINV,B20 decision
class A1,A3,A5,A9,A10,B1,B2,B3,B7,B8,B9,B10,B18,C1,C3,D1,D2,E1,E3,E4,E5,E6,F1,B22,B23,B21 io
class DB1,DB2,DB3,DB4 database
class EXT1 external
class C0,C1,C2,C3,C4,C5,D1,D2,D3,D4,E1,E2,E3,E4,E5,E6,F1 admin
class B14,B15,B16,B17,B18,B21 order
class Z1 cancel