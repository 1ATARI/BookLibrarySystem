# Book Library System 📚

A modern, scalable library management system built with .NET Core, enabling users to borrow/return books, manage authors/genres, and receive email notifications. Designed with Clean Architecture, CQRS, and Domain-Driven Design (DDD) principles.


## Book Library System Overview

**Figure 1: High-Level Architecture Diagram**

## ✨ Features

### Core Operations

#### User Management 🔐
- JWT Authentication/Authorization
- User registration with email verification
- Profile updates and management

#### Book Operations 📖
- Borrow/return books with due date tracking
- Book availability status (Available/Borrowed)
- Multi-genre categorization (e.g., Fiction, Sci-Fi)

#### Email Notifications 📧
- Welcome emails on successful registration
- Borrow/return confirmation emails
- Graceful error handling with Result pattern

#### Advanced Search 🔍
- Paginated listings for books/authors/genres
- Filter by availability, publication date, or author

## 🛠 Technology Stack

### Architectural Patterns
- Clean Architecture (Domain > Application > Infrastructure > Presentation)
- CQRS with MediatR implementation
- Repository Pattern + Unit of Work
- Domain-Driven Design (Aggregates, Value Objects)

### Core Components

| Component | Technologies |
|-----------|-------------|
| Backend Framework | ASP.NET Core 9 |
| Database | Entity Framework Core + SQL Server |
| Authentication | JWT, ASP.NET Core Identity |
| Validation | FluentValidation |
| Patterns | Mediator, Result, Specification |
| Email Service | MailKit + SMTP |

## 🏗 System Architecture

### Domain Layer
```
📁 Domain/            - Core business logic for each entity
├── 📁 Book/          - Core book entity with business rules
├── 📁 Author/        - Author management with book relationships
├── 📁 Genre/         - Genre classification system
└── 📁 UserBook/      - Borrow/return tracking entity

```

### Application Layer (CQRS)
```
📁 Application/
├── 📁 Commands         - Write operations (e.g., AddAuthorCommand)
├── 📁 Queries          - Read operations (e.g., GetAllBooksQuery)
├── 📁 Behaviors        - MediatR pipeline (e.g., logging/validation)
└── 📁 Validators       - FluentValidation rules

```

### Infrastructure Layer
```
📁 Infrastructure/
├── 📁 Repositories     - EF Core implementations for data access
├── 📁 Email            - SMTP service integration for email notifications
└── 📁 Identity         - JWT token generation and user management

```


## 🚀 Getting Started

### Prerequisites
- .NET Core 9 SDK
- SQL Server 2019+
- SMTP server credentials (e.g., Gmail)

### Installation

#### Clone the Repository:
```bash
git clone https://github.com/1ATARI/BookLibrarySystem.git
cd BookLibrarySystem
```

#### Configure the Database:
Update `appsettings.json` with the connection string:
```json
"ConnectionStrings": {
  "DefaultConnection": "Server=...;Database=BookLibrary;Trusted_Connection=True;"
}
```

#### Configure Email Settings:
Update `appsettings.json` with mail settings:
```json
"Mail": {
  "Email": "your-email@gmail.com",
  "DisplayName": "Book Library System",
  "Password": "your-email-password",
  "Host": "smtp.gmail.com",
  "Port": 587
}
```

#### Run Migrations:
```bash
dotnet ef database update
```

#### Start the Application:
```bash
dotnet run
```

## 🔗 API Endpoints

### Auth
- **POST** `/api/Auth/Register`
- **POST** `/api/Auth/Login`

### Author
- **GET** `/api/Author`
- **POST** `/api/Author`
- **GET** `/api/Author/{authorId}`
- **PUT** `/api/Author/{authorId}`
- **DELETE** `/api/Author/{authorId}`
- **POST** `/api/Author/{authorId}/books`

### Book
- **GET** `/api/Book`
- **POST** `/api/Book`
- **DELETE** `/api/Book`
- **GET** `/api/Book/{bookId}`
- **PUT** `/api/Book/{bookId}`

### Genre
- **GET** `/api/Genre`
- **POST** `/api/Genre`
- **GET** `/api/Genre/{genreId}`
- **POST** `/api/Genre/{genreId}`
- **DELETE** `/api/Genre/{genreId}`

### UserBook
- **GET** `/api/UserBook`
- **GET** `/api/UserBook/{userBookId}`
- **DELETE** `/api/UserBook/{userBookId}`
- **GET** `/api/UserBook/book/{bookId}`
- **GET** `/api/UserBook/user/{userId}`
- **POST** `/api/UserBook/borrow`
- **PUT** `/api/UserBook/borrow/{userBookId}`
- **PUT** `/api/UserBook/return/{userBookId}`

### User
- **GET** `/api/User`
- **GET** `/api/User/{userId}`
- **PUT** `/api/User/{userId}`

## 🧩 Design Patterns

### 1. Result Pattern
```csharp
public Result<Book> BorrowBook(Guid bookId)
{
    if (!IsAvailable) 
        return Result.Failure(BookErrors.Unavailable);
    
    return Result.Success(book);
}
```
**Benefits:** Unified error handling, compile-time safety.

### 2. MediatR Pipeline
```
Request → [Logging] → [Validation] → Handler → Response
```
- **LoggingBehavior:** Logs command execution
- **ValidationBehavior:** Auto-validates requests

### 3. Repository Pattern
```csharp
public interface IRepository<T> where T : IIdentifiable
{
    Task<T?> GetByIdAsync(Guid entityId, string? includeProperties = null, CancellationToken cancellationToken = default);
    Task<IEnumerable<T>> GetAllAsync(Expression<Func<T, bool>>? filter = null, string? includeProperties = null, int skip = 0, int take = 10, CancellationToken cancellationToken = default);
    Task AddAsync(T entity, CancellationToken cancellationToken = default);
    void Update(T entity, CancellationToken cancellationToken = default);
    Task<bool> DeleteAsync(Guid entityId, CancellationToken cancellationToken);
    Task<bool> ExistsByIdAsync(Guid entityId, CancellationToken cancellationToken);
}
```

## 📈 Future Roadmap

### Role-Based Access Control 👮
- Librarian/Admin roles
- Fine-grained permissions

### Enhanced Reporting 📊
- Borrowing history tracking
- Popular book analytics

