# MVC Blog Application

Учебное веб-приложение на ASP.NET Core MVC для ведения блога с системой логирования запросов.

## Функциональность

- Регистрация пользователей
- Создание постов в блоге
- Система логирования всех запросов с сохранением:
 - В консоль
 - В текстовый файл
 - В базу данных SQL Server
- Просмотр истории запросов
- Форма обратной связи

## Технологии

- ASP.NET Core MVC
- Entity Framework Core
- SQL Server
- Middleware компоненты
- Repository pattern
- Dependency Injection

## Структура базы данных

### Таблица Users
- Id (Guid)
- FirstName (string) 
- LastName (string)
- JoinDate (DateTime)

### Таблица UserPosts  
- Id (Guid)
- Date (DateTime)
- Title (string)
- Text (string) 
- UserId (Guid, FK)

### Таблица Requests
- Id (Guid)
- Date (DateTime)
- Url (string)

## Установка и запуск

1. Клонируйте репозиторий
2. В файле appsettings.json настройте строку подключения к SQL Server
3. Запустите приложение

## Логирование
Система логирования реализована через кастомный Middleware компонент и записывает все входящие запросы:
  - В консоль в реальном времени
  - В файл Logs/RequestLog.txt
  - В таблицу Requests базы данных

