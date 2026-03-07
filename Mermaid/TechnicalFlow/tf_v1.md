flowchart TD

%% =====================================================
%% DATABASES / STORAGE
%% =====================================================
DB[(Product Database)]
ST[(Image Storage / CDN)]

%% =====================================================
%% ADMIN POS / DASHBOARD
%% =====================================================
subgraph ADMIN["Admin POS / Dashboard"]
A1[/Open Product Management/]
A2[/Add or Edit Product/]
A3[/Upload Product Image/]
A4[/Save Product Details/]
end

%% =====================================================
%% FLOW
%% =====================================================
A1 --> A2
A2 --> A3
A3 --> ST
A3 --> DB
A4 --> DB

%% =====================================================
%% WEBSITE DISPLAY
%% =====================================================
subgraph WEBSITE["Customer Website"]
W1[/Fetch Product List/]
W2[/Display Product Details & Image/]
end

DB --> W1
ST --> W2
W1 --> W2