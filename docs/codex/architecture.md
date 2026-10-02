# Observed Architecture Map

This document records only facts observed in the supplied archive. It is not a replacement for the source code.

## Backend
- `backEnd/Data/Yashil.Core`: shared entities, enums, DTO/domain models, interfaces, configuration/support code and generated/service integration artifacts.
- `backEnd/Modules/YashilCommerce/YashilCommerce.Core`: Commerce contracts/services/repositories.
- `backEnd/Modules/YashilCommerce/YashilCommerce.Infrastructure`: Commerce service/repository implementations and infrastructure integrations.
- `backEnd/Modules/YashilCommerce/YashilCommerce.Web`: Commerce HTTP/API areas, controllers and view models.
- `backEnd/Yashil.WebHost`: application host and references to a broader Yashil module set.

## Observed dependency pattern
For Commerce features, the supplied source commonly follows:
`Core contracts -> Infrastructure implementations -> Web/API controllers`
with shared models/entities in `Yashil.Core`.

Examples in the supplied code include `IHekmatTransactionService`, `HekmatTransactionService`, and Commerce controllers.

## Existing implementation style
- Services commonly inherit from `GenericService<TEntity, TKey>` and use `IUnitOfWork`/repository abstractions.
- Repositories and services are separated into Core interfaces and Infrastructure implementations.
- Web endpoints use area-specific controllers and view models.
- Existing code uses dependency injection, async methods, EF Core/SQL Server, and project-specific Yashil common abstractions.
- Preserve existing conventions unless the feature specification explicitly requires a change.

## Important limitation
The archive contains project references to other Yashil modules that are not included in the supplied source. Do not infer their internals from project references alone.
