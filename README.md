```mermaid
stateDiagram
    direction LR
    [*] --> IngestionAPI
    IngestionAPI --> WebApi
    WebApi --> ClientApp
    
```
