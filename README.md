# LedgerManager

Приложение для управления лицевыми считами (ЛС)


## 🛠 Технологии

- **Backend**: ASP.NET Core
- **ORM**: Entity Framework Core
- **Database**: SQLite
- **Containerization**: Docker
- **Documentation**: Swagger/OpenAPI

## 🚀 Быстрый старт

### Запуск с помощью Docker

1. **Сборка образа:**
   ```bash
   docker build -t ledger-manager .
    ```

2. **Запуск контейнера:**

    ```bash
   docker run -d -p 8080:8080 -e ASPNETCORE_ENVIRONMENT=Production --name ledger-manager ledger-manager
    ```
## Переменные окружения

ASPNETCORE_ENVIRONMENT - определяет среду выполнения:

1. Production - режим production (Swagger отключен)
2. Development - режим разработки (Swagger включен)

## 🌐 Доступ к приложению

После успешного запуска приложение доступно по адресу:

Основное API: http://localhost:8080
Swagger документация: http://localhost:8080/swagger