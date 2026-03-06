```mermaid
flowchart TD

%% =====================================================
%% DATABASES
%% =====================================================

DB1[(User Database)]
DB2[(Product Database)]
DB3[(Order Database)]
DB4[(Inventory Database)]

%% =====================================================
%% EXTERNAL SERVICES
%% =====================================================

EXT1[[GCash Payment Gateway]]
EXT2[[Lalamove Delivery Service]]
EXT3[[LBC Delivery Service]]
EXT4[[Gmail Notification Service]]
EXT5[[Bank Transfer to Admin's Bank Account]]

%% =====================================================
%% CUSTOMER ONLINE FLOW
%% =====================================================

subgraph CUSTOMER["Online Customer Ordering"]

A0([Start]) --> A1[/Open App/]

A1 --> A2{Have Account?}

A2 -->|No| A3[/Enter Email & Password/]
A3 --> A4[Send OTP]
A4 --> A5[/Enter OTP/]
A5 --> A6{OTP Valid?}

A6 -->|No| A5
A6 -->|Yes| A6A[/Enter Full Address/]
A6A --> A7[Create Account]

A7 --> DB1
DB1 --> A8[Login]

A2 -->|Yes| A9[/Enter Credentials/]
A8 --> A10[/Enter Credentials/]
A9 --> A11{Credentials Valid?}

A11 -->|No| A9
A11 -->|Yes| A12[Login Success]

%% SHOPPING

A12 --> B1[/View Product Catalog/]
B1 --> DB2
DB2 --> B2[/Select Product/]

B2 --> BINV{Item In Stock?}
BINV --> DB4

BINV -->|No| OUT[/Out of Stock/]
OUT --> B1

BINV -->|Yes| B3[/View Product Details/]

B3 --> B4{Action}

B4 -->|Add to Cart| B5[Add to Cart]
B5 --> B6{Continue Shopping?}

B6 -->|Yes| B1
B6 -->|Checkout| B7[/Open Cart/]

B4 -->|Buy Now| B7

%% CHECKOUT

B7 --> B8[/Review Order/]
B8 --> B9[/Proceed to Checkout/]

B9 --> B20{Order Type}
B20 -->|Pickup| B21[Pickup in Store]
B20 -->|Delivery| B22[/System: Determine Courier Based on Delivery Address/]

B22 --> B23[/Enter Delivery Address/]
B23 --> B24{Delivery Location?}
B24 -->|Within Bulacan or Manila| B31[/System message: "Lalamove selected for your delivery (based on your address)"/]
B24 -->|Outside Bulacan & Manila| B32[/System message: "LBC selected for your delivery (based on your address)"/]
B31 --> B10[Confirm Payment Method]
B32 --> B10
B21 --> B10

%% PAYMENT

B10 -->|GCash| B11[Pay via GCash]
B11 --> EXT1
EXT1 --> B12{Payment Successful?}

B10 -->|Bank Transfer| B29[/Make bank transfer to the admin's bank account and upload proof/]
B29 --> EXT5
EXT5 --> B30{Payment Verified by Admin?}

B12 -->|Yes| B14[Create Order Record]
B12 -->|No| B13{Retry Payment?}
B13 -->|Yes| B10
B13 -->|Cancel| Z1([Order Cancelled])

B30 -->|Yes| B14
B30 -->|No| B29

%% ORDER RECORD

B14 --> B15[Set Order Status: Pending]
B15 --> DB3
DB3 --> B16[Processing Order]
B16 --> B17[/Order Confirmation Sent/]

B17 --> B18[/Wait for Delivery or Pickup/]

B18 --> B25{Order Received?}
B25 -->|No| B18
B25 -->|Yes| B26[/Confirm Order Received/]
B26 --> B27[/Add Customer Review/]

B27 --> DB3
B27 --> B28([Transaction Complete])

end

%% =====================================================
%% ADMIN FLOW
%% =====================================================

subgraph ADMIN["Admin System"]

C0([Admin Start]) --> C1[/Admin Login/]
C1 --> C2{Credentials Valid?}
C2 -->|No| C1
C2 -->|Yes| C3[/Admin Dashboard/]

C3 --> C4{Select Module}
C4 -->|Online Orders| D1
C4 -->|Walk-In POS| W1
C4 -->|Online History| E1
C4 -->|Walk-In History| H1
C4 -->|Analytics| F1
C4 -->|Logout| C5([End])

end

%% =====================================================
%% ONLINE ORDER MANAGEMENT
%% =====================================================

subgraph ORDERS["Online Orders"]

D1[/View Online Orders/] --> DB3
DB3 --> D2[/Select Specific Order/]

D2 --> D5{Delivery Type?}
D5 -->|Pickup| D3[Update Status for Pickup]
D5 -->|Delivery| D6[/Courier assigned by system based on customer's delivery address/]

D6 -->|Lalamove| D7[Admin books Lalamove (system suggested)]
D7 --> EXT2

D6 -->|LBC| D8[Admin arranges LBC (system suggested)]
D8 --> EXT3

D7 --> D9[Generate Tracking Link]
D8 --> D9
D9 --> D10[Send Tracking Email]
D10 --> EXT4

D10 --> D11[Courier Delivers Order]
D11 --> D12[Customer Pays Delivery Fee on Delivery]
D12 --> D3

D3 -->|Processing| D4[Save Order Status]
D3 -->|Packed| D4
D3 -->|Out for Delivery| D4
D3 -->|Delivered| D4
D3 -->|Cancelled| D4

D4 --> DB3
DB3 --> C3

end

%% =====================================================
%% WALK-IN POS
%% =====================================================

subgraph POS["Walk-In POS System"]

W1[/Open POS/]
W1 --> W2[/Scan Product/]
W2 --> DB2

DB2 --> W3{Stock Available?}
W3 -->|No| W2
W3 -->|Yes| W4[Add Product to Cart]

W4 --> W5{Add More Items?}
W5 -->|Yes| W2
W5 -->|No| W6[/Show Total/]

W6 --> W7{Payment Method?}
W7 -->|Cash| W8[Accept Cash Payment]
W7 -->|GCash| W9[Process GCash Payment]
W9 --> EXT1

W8 --> W10[Create Walk-In Order]
W9 --> W10

W10 --> DB3
W10 --> W11[Update Inventory]
W11 --> DB4

W11 --> W12[/Print Receipt/]
W12 --> W13([Transaction Complete])
W13 --> C3

end

%% =====================================================
%% ORDER HISTORY
%% =====================================================

subgraph HISTORY["Order History"]

E1[/View Online Order History/] --> DB3
DB3 --> C3

H1[/View Walk-In Order History/] --> DB3
DB3 --> C3

end

%% =====================================================
%% ANALYTICS
%% =====================================================

F1[/Shop Analytics/]
F1 --> DB2
F1 --> DB3
F1 --> DB4
F1 --> C3

%% =====================================================
%% COLOR DEFINITIONS
%% =====================================================

classDef startend fill:#7ED957,stroke:#2E7D32,stroke-width:2px,color:#000
classDef process fill:#90CAF9,stroke:#1565C0,stroke-width:2px,color:#000
classDef decision fill:#FFF59D,stroke:#F9A825,stroke-width:2px,color:#000
classDef io fill:#FFCC80,stroke:#EF6C00,stroke-width:2px,color:#000
classDef database fill:#CE93D8,stroke:#6A1B9A,stroke-width:2px,color:#000
classDef external fill:#80DEEA,stroke:#00838F,stroke-width:2px,color:#000
classDef cancel fill:#EF9A9A,stroke:#B71C1C,stroke-width:2px,color:#000
classDef admin fill:#B2DFDB,stroke:#00695C,stroke-width:2px,color:#000

%% =====================================================
%% CLASS ASSIGNMENTS
%% =====================================================

class A0,B28,C0,C5,W13 startend
class Z1 cancel
class A4,A7,A8,A12,B5,B14,B15,B16,B17,B18,B27,D4,W4,W10,W11 process
class A2,A6,A11,B4,B6,B12,B13,C2,D3,BINV,B20,B22,W3,W5,W7,D5,D6,B30 decision
class A1,A3,A5,A9,A10,A6A,B1,B2,B3,B7,B8,B9,B10,B18,B21,B23,W1,W2,W6,W12,B26,B29 io
class DB1,DB2,DB3,DB4 database
class EXT1,EXT2,EXT3,EXT4,EXT5 external
class C0,C1,C2,C3,C4,C5,D1,D2,D3,D4,F1,W1,W2,W3,W4,W5,W6,W7,W8,W9,W10,W11,W12,E1,H1 admin
```