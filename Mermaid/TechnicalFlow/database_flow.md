flowchart TD
    A[Admin POS / Inventory] --> B[Upload Product & Images]
    B --> C[Cloud Storage (Azure Blob / CDN)]
    C --> D[Database: Product + ProductImage Table]
    D --> E[Web App Queries Database]
    E --> F[Frontend / Customer View]
    
    subgraph Images
        C -->|Stores URLs| D
        D -->|Primary, Gallery, Thumbnail| E
    end

    style A fill:#f9f,stroke:#333,stroke-width:1px
    style F fill:#9f9,stroke:#333,stroke-width:1px