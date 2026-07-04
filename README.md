# Gestion Microfinance

API ASP.NET Core 8 pour la gestion de revenus/dépenses d'un utilisateur
(déclaration d'opérations, catégorisation, authentification JWT), structurée
selon les principes de la **Clean Architecture**.

## Structure de la solution

```
src/
├─ GestionMicrofinance.Domain          Entités et enums, aucune dépendance externe
├─ GestionMicrofinance.Application     Cas d'usage (services), DTOs, interfaces (ports)
├─ GestionMicrofinance.Infrastructure  EF Core, JWT, hachage de mot de passe
└─ GestionMicrofinance.Api             Contrôleurs, Program.cs, configuration
tests/
└─ GestionMicrofinance.Application.UnitTests   Tests xUnit + Moq de la couche Application
```

Règle de dépendance : `Api` → `Application` + `Infrastructure` → `Application` → `Domain`.
`Domain` et `Application` ne référencent ni EF Core ni ASP.NET Core.

## Prérequis

- [.NET SDK 8.0](https://dotnet.microsoft.com/download)
- SQL Server LocalDB (installé avec Visual Studio) ou toute instance SQL Server accessible

## Configuration locale (secrets)

La clé de signature JWT ne doit **jamais** être committée. En local, utilisez les
[User Secrets](https://learn.microsoft.com/aspnet/core/security/app-secrets) :

```bash
cd src/GestionMicrofinance.Api
dotnet user-secrets set "Jwt:Key" "<une-clé-aléatoire-longue>"
```

En production, définissez `Jwt__Key` en variable d'environnement (ou via le
gestionnaire de secrets de votre hébergeur).

## Lancer le projet

```bash
dotnet restore
dotnet ef database update --project src/GestionMicrofinance.Infrastructure --startup-project src/GestionMicrofinance.Api
dotnet run --project src/GestionMicrofinance.Api
```

Swagger est disponible sur `/swagger` en environnement `Development`.

## Tests

```bash
dotnet test
```

## Principaux endpoints

| Méthode | Route                            | Auth | Description                          |
|---------|-----------------------------------|------|---------------------------------------|
| POST    | `/api/auth/register`              | Non  | Créer un compte                       |
| POST    | `/api/auth/login`                 | Non  | Connexion, retourne un token JWT      |
| GET     | `/api/categorie`                  | Oui  | Lister les catégories                 |
| POST    | `/api/categorie/create`           | Oui  | Créer une catégorie (revenu/dépense)  |
| POST    | `/api/depense/declare`            | Oui  | Déclarer une dépense                  |
| GET     | `/api/depense/operations-recentes`| Oui  | Dépenses des 3 derniers mois          |
| POST    | `/api/revenu/declare`             | Oui  | Déclarer un revenu                    |
| GET     | `/api/revenu/operations-recentes` | Oui  | Revenus des 3 derniers mois           |

## Migrations EF Core

Pour ajouter une nouvelle migration après une modification des entités :

```bash
dotnet ef migrations add <NomMigration> --project src/GestionMicrofinance.Infrastructure --startup-project src/GestionMicrofinance.Api --output-dir Persistence/Migrations
```
