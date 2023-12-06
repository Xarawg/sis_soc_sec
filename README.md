## Запуск frontend на .NET Core:

* Клонируете репозиторий.
* Открываете каталог backend через Visual Studio
* Открываете файл appsettings.json и редактируете ConnectionString, вставляя строки соединения с вашей БД (PostgreSQL)
* Запускаете node package manager терминал внизу IDE или сверху через Tools - NuGet Package Manager как на скриншоте ниже

![image](https://github.com/Xarawg/sis_soc_sec/assets/92927559/13cbaa8e-673f-426b-b551-d3b4e1fa5500)


* Выбираете внизу IDE проект SecurityService_Core_Stores как на скриншоте ниже

![image](https://github.com/Xarawg/sis_soc_sec/assets/92927559/2eae6d94-7c81-42e3-906d-9d512d7c1756)


* Вводите в терминал `Update-Database`
* Выбираете профиль запуска http как на скриншоте ниже

![image](https://github.com/Xarawg/sis_soc_sec/assets/92927559/639ee4ef-9b50-4554-a0b7-b70c77c8c463)

* Для получения авторизационного токена зарегистрируйте пользователя через User/Auth, полученный токен вставьте в Swagger через кнопку Authorize, дописав перед токеном `Bearer `. Например, при регистрации пары логин-пароль testLogin testPassword нужно будет ввести

`Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJJZCI6ImEzNzNkZDNiLWE5MDItNDViMi1iNjBhLWI3YmExYmUwNDNjOCIsIkxvZ2luIjoidGVzdExvZ2luIiwiaXNBdXRoZW50aWNhdGVkIjoidHJ1ZSIsIm5iZiI6MTcwMTg2MTY3MiwiZXhwIjoxNzAxOTYxNjcyLCJpYXQiOjE3MDE4NjE2NzIsImlzcyI6IlNvY1NlY0F1dGhTZXJ2ZXIiLCJhdWQiOiJodHRwOi8vbG9jYWxob3N0OjQyMDAifQ.oRBQBHY-zWBhBb_gMyN6P0aLaDlIb8PgjRmh_M0_Cmk`

![image](https://github.com/Xarawg/sis_soc_sec/assets/92927559/be6cb4fd-4bdd-4aae-8b04-ad23f2078994)




## Запуск frontend на Angular:

* Клонируете репозиторий.
* Открываете каталог frontend через Visual Studio
* Запускаете терминал
* Вводите в терминал `npm i`
* После установки вводите в терминал `ng serve --open`

Флаг `--open` нужен для того, чтобы приложение запустилось в окне браузера сразу после запуска.
