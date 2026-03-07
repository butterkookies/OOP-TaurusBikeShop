flowchart TD
%% =====================================================
%% DATABASES / STORAGE (top-left)
%% =====================================================
DB[(Product Database)]
ST[(Image Storage / CDN)]
ORD[(Order Database)]
USR[(User Database)]

DB --- ST
ORD --- USR

%% =====================================================
%% ADMIN POS / DASHBOARD (top-right)
%% =====================================================
subgraph ADMIN["Admin POS / Dashboard"]
A1[/Add/Edit Product/]
A2[/Upload Product Image/]
A3[/Save Product Info & Image Path/]
end

A1 --> A2
A2 --> A3
A2 --> ST
A3 --> DB

%% =====================================================
%% CUSTOMER WEBSITE (center-left)
%% =====================================================
subgraph WEBSITE["Customer Web Application"]
W1[/Request Product List via API/]
W2[/Request Product Details via API/]
W3[/Display Product Info & Image/]
W4[/Place Order via API/]
end

W1 --> DB
W2 --> DB
W3 --> DB
W3 --> ST
W4 --> ORD
W4 --> USR

%% =====================================================
%% EXTERNAL SERVICES (bottom)
%% =====================================================
subgraph EXTERNAL["Payment & Delivery"]
P1[[GCash Payment Gateway]]
P2[[Bank Transfer Verification]]
P3[[Courier APIs]]
end

W4 -->|Payment| P1
W4 -->|Payment| P2
W4 -->|Delivery| P3

%% =====================================================
%% SPACING / LAYOUT HINTS
%% =====================================================
DB -.-> W1
ST -.-> W3
ORD -.-> W4
USR -.-> W4