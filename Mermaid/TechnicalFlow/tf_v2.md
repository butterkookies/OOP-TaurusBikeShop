flowchart TD

%% =====================================================
%% DATABASES / STORAGE
%% =====================================================
DB[(Product Database)]
ST[(Image Storage / CDN)]
ORD[(Order Database)]
USR[(User Database)]

%% =====================================================
%% CUSTOMER WEBSITE / API
%% =====================================================
subgraph WEBSITE["Customer Web Application"]
C1["Open Product Page"]
C2["Call API: GET /products"]
C3["Call API: GET /products/{id}"]
C4["Display Product Info & Image"]
C5["Add Items to Cart"]
C6["Checkout: POST /orders"]
C7["Show Confirmation & Tracking"]
end

%% =====================================================
%% ADMIN POS
%% =====================================================
subgraph ADMIN["Admin POS"]
A1["Add/Edit Product"]
A2["Upload Product Image"]
A3["Update DB & Storage"]
end

%% Admin Flow
A1 -->|Edit Flow| A2:::adminLine
A2 -->|Image Upload| ST:::mediaLine
A2 -->|Save Flow| A3:::adminLine
A3 -->|Update DB| DB:::dbLine

%% =====================================================
%% FLOW: PRODUCT FETCH
%% =====================================================
C1 -->|Open Page| C2:::apiLine
C2 -->|Fetch List| DB:::dbLine
C2 -->|Send to Frontend| C4:::apiLine
C3 -->|Fetch Details| DB:::dbLine
C3 -->|Fetch Image| ST:::mediaLine
C3 -->|Display| C4:::apiLine
C4 -->|Load Image| ST:::mediaLine

%% =====================================================
%% FLOW: ORDER PROCESS
%% =====================================================
C5 -->|Cart Items| C6:::apiLine
C6 -->|Save Order| ORD:::dbLine
C6 -->|Link User| USR:::dbLine

%% =====================================================
%% EXTERNAL SERVICES
%% =====================================================
subgraph EXTERNAL["Payment & Delivery"]
P1[[GCash Payment Gateway]]
P2[[Bank Transfer Verification]]
P3[[Courier APIs]]
end

C6 -->|Payment| P1:::paymentLine
C6 -->|Payment| P2:::paymentLine
C6 -->|Delivery| P3:::deliveryLine
P1 -->|Payment Confirmed| C7:::apiLine
P2 -->|Payment Confirmed| C7:::apiLine
P3 -->|Delivery Status| C7:::apiLine
ORD -->|Order Record| C7:::apiLine

%% =====================================================
%% CLASS DEFINITIONS FOR COLOR
%% =====================================================
classDef dbLine stroke:#6A1B9A,stroke-width:2px,color:#6A1B9A;
classDef apiLine stroke:#1565C0,stroke-width:2px,color:#1565C0;
classDef adminLine stroke:#00695C,stroke-width:2px,color:#00695C;
classDef mediaLine stroke:#EF6C00,stroke-width:2px,color:#EF6C00;
classDef paymentLine stroke:#C62828,stroke-width:2px,color:#C62828;
classDef deliveryLine stroke:#00838F,stroke-width:2px,color:#00838F;