# Observed Angular Frontend Map

The supplied `clientApp` is an Angular/TypeScript application organized by feature modules plus shared reusable infrastructure.

## Structure observed
- Feature areas include `payment/` and `ecommerce/`.
- Shared reusable components/classes/services live under `shared/`.
- Feature components commonly have separate `.ts` and `.html` files.
- Existing list/detail components reuse shared base classes such as `Selectable` and `BaseList`.
- Feature services and models are kept close to their feature or in shared `_services` / `_models` locations.

## Conventions to preserve
- Reuse existing shared base components and services before creating new infrastructure.
- Match existing routing/module/component patterns in the nearest analogous feature.
- Preserve existing naming and folder conventions even when they are not ideal.
- Do not rewrite generated JavaScript/map files unless the existing build workflow explicitly requires them.
- Before adding a new UI pattern, find an existing component with the same interaction pattern.

## Verification
Use the actual frontend package/build metadata available in the repository. The supplied archive did not expose a root `package.json` or Angular CLI configuration at the inspected paths, so do not invent npm commands until the relevant build configuration is present.
