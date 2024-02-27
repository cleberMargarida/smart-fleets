```mermaid
flowchart LR
    classDef ingestion fill:#007bff,stroke:#0056b3,stroke-width:2px;
    classDef webapi fill:#28a745,stroke:#1e7e34,stroke-width:2px;
    classDef clientapp fill:#ffc107,stroke:#d39e00,stroke-width:2px;

    IngestionAPI(🔄 Ingestion API) --> WebApi(🌐 Web API)
    WebApi --> ClientApp(📱 Client App in React)

    class IngestionAPI ingestion;
    class WebApi webapi;
    class ClientApp clientapp;
```