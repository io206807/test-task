# Terminal Import Service

Фоновый сервис на .NET для импорта терминалов из JSON файла и сохранения
данных в базу PostgreSQL.

## Описание

Сервис периодически загружает файл `terminals.json`, десериализует
данные, преобразует их в доменные сущности и выполняет bulk-insert в
базу данных.

Импорт выполняется по расписанию (cron) с учетом тайм-зоны.

## Используемые технологии

-   .NET Worker Service
-   PostgreSQL
-   Entity Framework Core
-   EFCore.BulkExtensions
-   Serilog
-   Cronos

## Архитектура

    Worker
      ↓
    ImportService
      ↓
    Source → Mapping → Repository → DbContext

**Source** --- загрузка данных (JSON)\
**Mapping** --- преобразование DTO → Entity\
**Repository** --- работа с БД\
**Worker** --- планировщик задач

## Конфигурация

Основные параметры задаются в `appsettings.json`.

Пример:

``` json
{
  "Scheduler": {
    "Cron": "0 2 * * *",
    "TimeZone": "Europe/Moscow"
  },
  "TerminalSource": {
    "FilePath": "files/terminals.json"
  },
  "ConnectionStrings": {
    "Default": "Host=localhost;Port=5432;Database=TerminalsDb;Username=login;Password=password"
  }
}
```

## Запуск

Перед запуском необходимо настроить строку подключения в
`appsettings.json`.

Применить миграции:

    dotnet ef database update

Запустить сервис:

    dotnet run

## Логирование

Используется Serilog.\
Все логи выводятся в консоль в UTC.
