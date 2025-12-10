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
- [Principios Aplicados](#-principios-aplicados)
- [Patrones de Diseño](#-patrones-de-diseño)
- [Licencia](#-licencia)

## ✨ Características

- ✅ **Clean Architecture** - Separación clara de responsabilidades
- ✅ **Domain-Driven Design** - Entidades ricas, Value Objects
- ✅ **SOLID Principles** - Código mantenible y extensible
- ✅ **Entity Framework Core** - ORM moderno con SQL Server
- ✅ **JWT Authentication** - Autenticación basada en tokens
- ✅ **Unit of Work Pattern** - Gestión transaccional
- ✅ **Repository Pattern** - Abstracción de acceso a datos
- ✅ **Result Pattern** - Manejo robusto de errores
- ✅ **Dependency Injection** - Inversión de control
- ✅ **Unit Testing** - xUnit + Moq
- ✅ **Swagger/OpenAPI** - Documentación automática
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
| **Base de Datos** | SQL Server |
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
│   │   │   └── Product.cs              # Entidad de dominio
│   │   ├── ValueObjects/
│   │   │   └── Money.cs                # Value Object inmutable
│   │   ├── Repositories/
│   │   │   └── IProductRepository.cs   # Contrato del repositorio
│   │   ├── Specifications/             # Patrón Specification
│   │   ├── Common/
│   │   │   └── BaseEntity.cs           # Clase base
│   │   └── Exceptions/
│   │       └── DomainException.cs      # Excepciones de negocio
│   │
│   ├── Application/                     # Capa de Aplicación
│   │   ├── UseCases/                   # Casos de Uso
│   │   ├── DTOs/                       # Data Transfer Objects
│   │   ├── Interfaces/
│   │   │   └── IUnitOfWork.cs          # Patrón Unit of Work
│   │   └── Common/
│   │       └── Result.cs               # Patrón Result
│   │
│   ├── Infrastructure/                  # Capa de Infraestructura
│   │   ├── Data/
│   │   │   ├── ApplicationDbContext.cs
│   │   │   └── Configurations/
│   │   │       └── ProductConfiguration.cs
│   │   ├── Repositories/
│   │   │   └── ProductRepository.cs    # Implementación del repositorio
│   │   ├── Authentication/
│   │   │   ├── JwtSettings.cs
│   │   │   └── JwtTokenGenerator.cs
│   │   └── DependencyInjection.cs      # Configuración de DI
│   │
│   └── WebAPI/                          # Capa de Presentación
│       ├── Controllers/
│       │   └── ProductsController.cs   # API REST
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
- [SQL Server](https://www.microsoft.com/sql-server/sql-server-downloads) (Express o Developer Edition)
- [Visual Studio 2022](https://visualstudio.microsoft.com/) o [VS Code](https://code.visualstudio.com/)
- [Git](https://git-scm.com/)

Verifica las instalaciones:

```bash
dotnet --version  # Debe ser 8.0 o superior
```

## 🚀 Instalación

### 1. Clonar el repositorio

```bash
git clone https://github.com/ZielGit/CommerceCleanArchitectureNET.git
cd CommerceCleanArchitectureNET
```

### 2. Restaurar dependencias

```bash
dotnet restore
```

### 3. Compilar la solución

```bash
dotnet build
```

## ⚙️ Configuración

### 1. Configurar la Base de Datos

Edita el archivo `src/CommerceCleanArchitectureNET.WebAPI/appsettings.json`:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=CommerceCleanArchitectureDB;Trusted_Connection=True;TrustServerCertificate=True;"
  },
  "JwtSettings": {
    "SecretKey": "TuClaveSecretaSuperSeguraDeAlMenos32Caracteres!",
    "Issuer": "CommerceCleanArchAPI",
    "Audience": "CommerceCleanArchClient",
    "ExpirationMinutes": 60
  }
}
```

### 2. Aplicar Migraciones

```bash
# Crear la migración inicial
dotnet ef migrations add InitialCreate -p src/CommerceCleanArchitectureNET.Infrastructure -s src/CommerceCleanArchitectureNET.WebAPI

# Aplicar la migración a la base de datos
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
dotnet test /p:CollectCoverage=true /p:CoverletOutputFormat=opencover
```

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

### Products

| Método | Endpoint | Descripción |
|--------|----------|-------------|
| `POST` | `/api/products` | Crear producto |
| `GET` | `/api/products/{id}` | Obtener producto por ID |
| `GET` | `/api/products` | Listar todos los productos |
| `GET` | `/api/products/search` | Buscar productos con filtros (Specification Pattern) |
| `PUT` | `/api/products/{id}` | Actualizar stock del producto |
| `DELETE` | `/api/products/{id}` | Eliminar producto |

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
