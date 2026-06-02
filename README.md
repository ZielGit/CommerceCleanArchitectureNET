# 🏗️ CommerceCleanArchitectureNET

![.NET](https://img.shields.io/badge/.NET-8.0-512BD4?logo=dotnet)
![C#](https://img.shields.io/badge/C%23-12.0-239120?logo=csharp)
![License](https://img.shields.io/badge/license-MIT-green)

Una implementación completa de **Clean Architecture** con **Domain-Driven Design (DDD)**, principios **SOLID** y **Clean Code** en .NET 8.

Este proyecto es una plantilla educativa que demuestra las mejores prácticas de arquitectura de software empresarial.

## 📋 Tabla de Contenidos

- [Características](#-características)
- [Arquitectura](#-arquitectura)
- [Tecnologías](#-tecnologías)
- [Estructura del Proyecto](#-estructura-del-proyecto)
- [Requisitos Previos](#-requisitos-previos)
- [Instalación](#-instalación)
- [Configuración](#-configuración)
- [Ejecución](#-ejecución)
- [Testing](#-testing)
- [API Endpoints](#-api-endpoints)
  - [Auth — Público](#auth--público)
  - [Auth — Requiere JWT](#auth--requiere-jwt)
  - [Products — Requiere JWT](#products--requiere-jwt)
- [Principios Aplicados](#-principios-aplicados)
- [Patrones de Diseño](#-patrones-de-diseño)
- [Licencia](#-licencia)

## ✨ Características

- ✅ **Clean Architecture** - Separación clara de responsabilidades
- ✅ **Domain-Driven Design** - Entidades ricas, Value Objects
- ✅ **SOLID Principles** - Código mantenible y extensible
- ✅ **Entity Framework Core** - ORM moderno con SQL Server
- ✅ **JWT Authentication** - Registro, login y protección de endpoints con Bearer tokens
- ✅ **Paginación** - Listado de productos paginado (`page`/`pageSize`) con metadatos de total y páginas
- ✅ **Password Hashing** - PBKDF2/SHA-256 con salt aleatorio
- ✅ **Unit of Work Pattern** - Gestión transaccional
- ✅ **Repository Pattern** - Abstracción de acceso a datos
- ✅ **Result Pattern** - Manejo robusto de errores
- ✅ **Specification Pattern** - Consultas y reglas de negocio encapsuladas
- ✅ **Dependency Injection** - Inversión de control
- ✅ **Unit Testing** - xUnit + Moq
- ✅ **Swagger/OpenAPI** - Documentación automática con soporte Bearer
- ✅ **Clean Code** - Código legible y mantenible

## 🏛️ Arquitectura

El proyecto sigue los principios de Clean Architecture de Robert C. Martin (Uncle Bob), organizando el código en capas concéntricas con dependencias que apuntan hacia el interior.

```
┌─────────────────────────────────────────────┐
│          Presentation Layer (API)           │  ← Capa externa
│         Controllers, Middleware             │
├─────────────────────────────────────────────┤
│         Application Layer                   │  ← Orquestación
│     Use Cases, DTOs, Interfaces             │
├─────────────────────────────────────────────┤
│            Domain Layer                     │  ← NÚCLEO
│   Entities, Value Objects, Domain Logic     │
└─────────────────────────────────────────────┘
                    ↑
                    │ implementa interfaces
                    │
           Infrastructure Layer (fuera del núcleo)
      Repositories, DbContext, External
```

### Flujo de Dependencias

```
API → Application → Domain
         ↑
   Infrastructure
```

**Regla de Oro:** Las dependencias solo pueden apuntar hacia adentro. El dominio no conoce nada de las capas externas.

## 🛠️ Tecnologías

| Categoría | Tecnología |
|-----------|-----------|
| **Framework** | .NET 8.0 |
| **Lenguaje** | C# 12.0 |
| **Base de Datos** | SQL Server 2022 Express |
| **Contenedor BD** | Docker |
| **ORM** | Entity Framework Core 8.0 |
| **Autenticación** | JWT Bearer |
| **Testing** | xUnit, Moq |
| **API Documentation** | Swagger/OpenAPI |
| **Logging** | Microsoft.Extensions.Logging |

## 📁 Estructura del Proyecto

```
CommerceCleanArchitectureNET/
├── src/
│   ├── Domain/                          # Capa de Dominio
│   │   ├── Entities/
│   │   │   ├── Product.cs              # Entidad de dominio: producto
│   │   │   └── User.cs                 # Entidad de dominio: usuario
│   │   ├── ValueObjects/
│   │   │   └── Money.cs                # Value Object inmutable
│   │   ├── Repositories/
│   │   │   ├── IProductRepository.cs   # Contrato del repositorio de productos
│   │   │   └── IUserRepository.cs      # Contrato del repositorio de usuarios
│   │   ├── Specifications/             # Patrón Specification
│   │   ├── Common/
│   │   │   └── BaseEntity.cs           # Clase base (Id, CreatedAt, UpdatedAt)
│   │   └── Exceptions/
│   │       └── DomainException.cs      # Excepciones de negocio
│   │
│   ├── Application/                     # Capa de Aplicación
│   │   ├── UseCases/
│   │   │   ├── Products/               # Casos de uso de productos
│   │   │   │   ├── CreateProduct/
│   │   │   │   ├── GetAllProducts/
│   │   │   │   ├── GetProductById/
│   │   │   │   ├── UpdateProduct/
│   │   │   │   ├── DeleteProduct/
│   │   │   │   └── SearchProducts/
│   │   │   └── Users/                  # Casos de uso de autenticación
│   │   │       ├── RegisterUser/
│   │   │       ├── LoginUser/
│   │   │       ├── GetCurrentUser/
│   │   │       └── LogoutUser/
│   │   ├── DTOs/                       # Data Transfer Objects
│   │   │   ├── ProductDto.cs
│   │   │   ├── PagedProductsDto.cs
│   │   │   ├── CreateProductDto.cs
│   │   │   ├── UpdateProductDto.cs
│   │   │   ├── ProductSearchDto.cs
│   │   │   ├── RegisterUserDto.cs
│   │   │   ├── LoginUserDto.cs
│   │   │   ├── UserDto.cs
│   │   │   └── AuthResponseDto.cs
│   │   ├── Interfaces/
│   │   │   ├── IUnitOfWork.cs          # Patrón Unit of Work
│   │   │   ├── IPasswordHasher.cs      # Contrato de hashing
│   │   │   └── ITokenGenerator.cs      # Contrato de generación de tokens
│   │   └── Common/
│   │       └── Result.cs               # Patrón Result
│   │
│   ├── Infrastructure/                  # Capa de Infraestructura
│   │   ├── Data/
│   │   │   ├── ApplicationDbContext.cs
│   │   │   ├── Migrations/
│   │   │   └── Configurations/
│   │   │       ├── ProductConfiguration.cs
│   │   │       └── UserConfiguration.cs
│   │   ├── Repositories/
│   │   │   ├── ProductRepository.cs
│   │   │   └── UserRepository.cs
│   │   ├── Authentication/
│   │   │   ├── JwtSettings.cs
│   │   │   └── JwtTokenGenerator.cs    # Genera tokens HMAC-SHA256
│   │   ├── Security/
│   │   │   └── PasswordHasher.cs       # PBKDF2/SHA-256 con salt
│   │   └── DependencyInjection.cs      # Configuración de DI
│   │
│   └── WebAPI/                          # Capa de Presentación
│       ├── Controllers/
│       │   ├── ProductsController.cs   # Endpoints de productos (requiere JWT)
│       │   └── AuthController.cs       # Endpoints de autenticación (público y protegido)
│       ├── Middleware/
│       │   └── ErrorHandlingMiddleware.cs
│       ├── Models/
│       │   └── ErrorResponse.cs
│       ├── Program.cs                  # Punto de entrada
│       └── appsettings.json            # Configuración
│
└── tests/
    ├── Domain.Tests/                    # Tests de dominio
    │   ├── Entities/
    │   │   └── ProductTests.cs
    │   ├── Specifications/
    │   └── ValueObjects/
    │       └── MoneyTests.cs
    │
    ├── Application.Tests/               # Tests de aplicación
    │   └── UseCases/
    │
    └── Infrastructure.Tests/            # Tests de infraestructura
        └── Repositories/
            └── ProductRepositoryTests.cs
```

## 📋 Requisitos Previos

Antes de comenzar, asegúrate de tener instalado:

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- [Docker Desktop](https://www.docker.com/products/docker-desktop/) (para SQL Server Express en contenedor)
- [Visual Studio 2022](https://visualstudio.microsoft.com/) o [VS Code](https://code.visualstudio.com/)
- [Git](https://git-scm.com/)

Verifica las instalaciones:

```bash
dotnet --version  # Debe ser 8.0 o superior
docker --version  # Debe ser 20.0 o superior
```

## 🚀 Instalación

### 1. Clonar el repositorio

```bash
git clone https://github.com/ZielGit/CommerceCleanArchitectureNET.git
cd CommerceCleanArchitectureNET
```

### 2. Levantar SQL Server con Docker

```bash
docker compose up -d
```

Esto inicia un contenedor de **SQL Server 2022 Express** en el puerto `1433`. Puedes verificar que está listo con:

```bash
docker compose logs -f sqlserver
```

Espera hasta ver el mensaje `SQL Server is now ready for client connections`.

### 3. Restaurar dependencias

```bash
dotnet restore
```

### 4. Compilar la solución

```bash
dotnet build
```

## ⚙️ Configuración

### 1. Configurar la Base de Datos

Crea el archivo `src/CommerceCleanArchitectureNET.WebAPI/appsettings.Development.json` (está en `.gitignore`, no se versiona):

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=CommerceCleanArchitectureDB;User Id=sa;Password=TU_PASSWORD_SA;TrustServerCertificate=True;"
  },
  "JwtSettings": {
    "SecretKey": "TuClaveSecretaSuperSeguraDeAlMenos32Caracteres!",
    "Issuer": "CommerceCleanArchitectureAPI",
    "Audience": "CommerceCleanArchitectureClient",
    "ExpirationMinutes": 60
  }
}
```

> El `Password` debe coincidir con el valor de `SA_PASSWORD` definido en tu archivo `.env` local.

### 2. Aplicar Migraciones

La migración inicial ya existe en el proyecto. Solo aplícala a la base de datos:

```bash
dotnet ef database update -p src/CommerceCleanArchitectureNET.Infrastructure -s src/CommerceCleanArchitectureNET.WebAPI
```

## ▶️ Ejecución

### Ejecutar la API

```bash
dotnet run --project src/CommerceCleanArchitectureNET.WebAPI
```

La API estará disponible en:
- **HTTP:** `http://localhost:5000`
- **HTTPS:** `https://localhost:5001`
- **Swagger:** `https://localhost:5001/swagger`

### Ejecutar con Hot Reload

```bash
dotnet watch run --project src/CommerceCleanArchitectureNET.WebAPI
```

## 🧪 Testing

### Ejecutar todos los tests

```bash
dotnet test
```

### Ejecutar tests con cobertura

```bash
dotnet test --collect:"XPlat Code Coverage"
```

Genera `coverage.cobertura.xml` en cada proyecto de test.

#### Visualizar reporte HTML

```bash
# Recoger todos los XML y generar reporte
reportgenerator -reports:"**/coverage.cobertura.xml" -targetdir:"coverage-report" -reporttypes:Html

# Abrir en el navegador
start coverage-report/index.html
```

> Requiere ReportGenerator instalado globalmente:
> `dotnet tool install --global dotnet-reportgenerator-globaltool`

### Ejecutar tests de una capa específica

```bash
# Tests de dominio
dotnet test tests/CommerceCleanArchitectureNET.Domain.Tests

# Tests de aplicación
dotnet test tests/CommerceCleanArchitectureNET.Application.Tests

# Tests de infraestructura
dotnet test tests/CommerceCleanArchitectureNET.Infrastructure.Tests
```

### Ejecutar un test específico

```bash
dotnet test --filter "FullyQualifiedName~ProductTests"
```

## 📡 API Endpoints

### Auth — Público

| Método | Endpoint | Descripción | Body |
|--------|----------|-------------|------|
| `POST` | `/api/auth/register` | Registrar nuevo usuario | `{ email, password, firstName, lastName }` |
| `POST` | `/api/auth/login` | Iniciar sesión y obtener token JWT | `{ email, password }` |

### Auth — Requiere JWT

> Estos endpoints requieren el header `Authorization: Bearer <token>`.

| Método | Endpoint | Descripción | Respuesta |
|--------|----------|-------------|-----------|
| `GET` | `/api/auth/me` | Obtener perfil del usuario autenticado | `UserDto` |
| `POST` | `/api/auth/logout` | Cerrar sesión (invalida el token en el cliente) | `204 No Content` |

> **Nota sobre logout:** La API usa JWT sin estado. El servidor confirma la identidad del usuario y responde `204`; la responsabilidad de descartar el token recae en el cliente.

### Products — Requiere JWT

> Todos los endpoints de productos requieren el header `Authorization: Bearer <token>`.

| Método | Endpoint | Descripción |
|--------|----------|-------------|
| `POST` | `/api/products` | Crear producto |
| `GET` | `/api/products` | Listar productos (paginado) |
| `GET` | `/api/products/{id}` | Obtener producto por ID |
| `PUT` | `/api/products/{id}` | Actualizar producto |
| `DELETE` | `/api/products/{id}` | Eliminar producto |
| `GET` | `/api/products/search` | Buscar con filtros (Specification Pattern) |

**Parámetros de paginación (`GET /api/products`):**

| Query param | Tipo | Default | Descripción |
|-------------|------|---------|-------------|
| `page` | `int` | `1` | Número de página (mínimo 1) |
| `pageSize` | `int` | `10` | Tamaño de página (máximo 100) |

Solo devuelve productos activos (`IsActive = true`), ordenados por nombre. Respuesta (`PagedProductsDto`):

```json
{
  "data": [ { "id": "...", "name": "...", "price": 0, "stock": 0, "isActive": true } ],
  "total": 42,
  "page": 1,
  "pageSize": 10,
  "totalPages": 5
}
```

**Parámetros de búsqueda (`/api/products/search`):**

| Query param | Tipo | Descripción |
|-------------|------|-------------|
| `name` | `string` | Filtrar por nombre (parcial) |
| `minPrice` | `decimal` | Precio mínimo |
| `maxPrice` | `decimal` | Precio máximo |
| `onlyInStock` | `bool` | Solo productos con stock > 0 |
| `onlyActive` | `bool` | Solo productos activos |

### Usar la API con JWT

**1. Registrar un usuario:**
```bash
curl -X POST https://localhost:5001/api/auth/register \
  -H "Content-Type: application/json" \
  -d '{"email":"user@example.com","password":"MiPassword123!","firstName":"Frans","lastName":"Vilcahuaman"}'
```

**2. Obtener el token:**
```bash
curl -X POST https://localhost:5001/api/auth/login \
  -H "Content-Type: application/json" \
  -d '{"email":"user@example.com","password":"MiPassword123!"}'
```

**3. Consultar el perfil autenticado:**
```bash
curl https://localhost:5001/api/auth/me \
  -H "Authorization: Bearer eyJhbGci..."
```

**4. Cerrar sesión:**
```bash
curl -X POST https://localhost:5001/api/auth/logout \
  -H "Authorization: Bearer eyJhbGci..."
```

**5. Usar el token en endpoints de productos:**
```bash
curl https://localhost:5001/api/products \
  -H "Authorization: Bearer eyJhbGci..."
```

**6. Listar productos con paginación:**
```bash
curl "https://localhost:5001/api/products?page=2&pageSize=20" \
  -H "Authorization: Bearer eyJhbGci..."
```

**En Swagger:** haz clic en el botón **Authorize** (🔒), pega el token y confirma. Todos los requests siguientes lo incluirán automáticamente.

## 🎯 Principios Aplicados

### SOLID

#### 1. **S**ingle Responsibility Principle
Cada clase tiene una única razón para cambiar:
- `Product` - Maneja lógica de negocio del producto
- `ProductRepository` - Maneja persistencia
- `CreateProductUseCase` - Ejecuta caso de uso específico

#### 2. **O**pen/Closed Principle
Abierto para extensión, cerrado para modificación:
- Uso de interfaces (`IProductRepository`)
- Extensión mediante herencia y composición

#### 3. **L**iskov Substitution Principle
Las implementaciones pueden sustituirse por sus abstracciones:
- `ProductRepository` implementa `IProductRepository`
- Funcionamiento correcto garantizado

#### 4. **I**nterface Segregation Principle
Interfaces específicas y cohesivas:
- `IProductRepository` - Solo operaciones de productos
- `IUnitOfWork` - Solo gestión transaccional

#### 5. **D**ependency Inversion Principle
Dependencia de abstracciones, no de concreciones:
- Use Cases dependen de `IProductRepository`
- Controllers dependen de interfaces de Use Cases

### Clean Code

- ✅ Nombres descriptivos y reveladores de intención
- ✅ Funciones pequeñas y enfocadas
- ✅ Comentarios solo cuando sea necesario
- ✅ Manejo de errores apropiado
- ✅ Sin código duplicado (DRY)
- ✅ Código autoexplicativo

### Domain-Driven Design

- ✅ **Ubiquitous Language** - Vocabulario compartido
- ✅ **Entities** - Objetos con identidad (`Product`)
- ✅ **Value Objects** - Objetos inmutables (`Money`)
- ✅ **Aggregates** - Consistencia de datos
- ✅ **Domain Events** - Comunicación entre agregados
- ✅ **Repositories** - Abstracción de persistencia
- ✅ **Specifications** - Reglas de negocio encapsuladas y reutilizables

## 🎨 Patrones de Diseño

| Patrón | Ubicación | Propósito |
|--------|-----------|-----------|
| **Repository** | `Infrastructure/Repositories` | Abstrae acceso a datos |
| **Unit of Work** | `Infrastructure/Data` | Gestiona transacciones |
| **Dependency Injection** | Toda la aplicación | Inversión de control |
| **Result** | `Application/Common` | Manejo de errores funcional |
| **Factory** | `Domain/Entities` | Creación de objetos complejos |
| **Specification** | `Domain/Specifications` | Encapsula lógica de consultas y reglas de negocio |
| **Strategy** | `Infrastructure/Security` | Algoritmo de hashing intercambiable (PBKDF2) |

### Patrón Specification en Detalle

El patrón Specification permite encapsular reglas de negocio reutilizables y combinarlas de forma flexible:

```csharp
// Crear especificaciones individuales
var activeSpec = new ActiveProductsSpecification();
var stockSpec = new ProductInStockSpecification();
var priceSpec = new ProductByPriceRangeSpecification(100, 500);

// Combinarlas con operadores lógicos
var complexSpec = activeSpec
    .And(stockSpec)
    .And(priceSpec);

// Usar en consultas
var products = await repository.FindAsync(complexSpec);
```

**Especificaciones disponibles:**
- `ActiveProductsSpecification` - Productos activos
- `ProductInStockSpecification` - Productos con stock disponible
- `ProductByPriceRangeSpecification` - Productos en rango de precio
- `ProductByNameSpecification` - Búsqueda por nombre
- `ProductByMinimumStockSpecification` - Stock mínimo requerido

**Operadores de composición:**
- `And()` - Combina especificaciones con lógica AND
- `Or()` - Combina especificaciones con lógica OR
- `Not()` - Niega una especificación

## 📚 Recursos Adicionales

### Libros Recomendados
- **Clean Architecture** - Robert C. Martin
- **Domain-Driven Design** - Eric Evans
- **Clean Code** - Robert C. Martin
- **Implementing Domain-Driven Design** - Vaughn Vernon

### Artículos
- [The Clean Architecture](https://blog.cleancoder.com/uncle-bob/2012/08/13/the-clean-architecture.html)
- [Domain-Driven Design Reference](https://www.domainlanguage.com/ddd/reference/)

## 📝 Licencia

Este proyecto está bajo la Licencia MIT. Ver el archivo [LICENSE](LICENSE.md) para más detalles.

## 👤 Autor

**Frans J. Vilcahuamán Rojas**
- GitHub: [@ZielGit](https://github.com/ZielGit)
- LinkedIn: [in/frans-vilcahuaman](https://www.linkedin.com/in/frans-vilcahuaman/)
