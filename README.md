```mermaid
stateDiagram
    direction LR
    [*] --> Ingestion.Api
    Ingestion.Api --> WebApi
    WebApi --> ClientApp
    
```

