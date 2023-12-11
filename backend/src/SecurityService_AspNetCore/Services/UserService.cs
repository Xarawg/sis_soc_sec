using AutoMapper;
using SecurityService_Core.Interfaces;
using SecurityService_Core.Models.ControllerDTO.User;
using SecurityService_Core.Models.DB;
using SecurityService_Core.Models.DTO;
using System.Diagnostics;
using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Identity;
using SecurityService_Core.Models.Enums;
using SecurityService_AspNetCore.Services.Communication;
using SecurityService_Core.Models.ControllerDTO.Administrator;

namespace Security_Service_AspNetCore.Services
{
    /// <summary>
    /// Сервис обработки файлов и связанных сущностей
    /// </summary>
    public class UserService
    {
        private readonly IMapper _mapper;
        private readonly IUserStore _userStore;
        private readonly ITokenHandler _tokenHandler;

        public const int SALT_SIZE = 32;
        public const int HASH_SIZE = 32;
        public const int ITERATIONS = 100000;
        private readonly HashAlgorithmName HASH_ALGORITHM = HashAlgorithmName.SHA512;

        public const int TEMPORARY_ACCESS_EXPIRATION_TIME = 30 * 60; // длительность временного доступа после сброса пароля
        public const int BLOCK_TIME = 15 * 60; // время блокировки
        public const int ATTEMPT_DATE = 180; // интервал между попытками ввода
        public const int ATTEMPT_COUNT = 5; // кол-во неудачных попыток

        /// <summary>
        /// Конструктор сервиса файлов
        /// </summary>
        /// <param name="mapper"></param>
        /// <param name="userStore"></param>
        /// <param name="tokenHandler"></param>
        public UserService(
            IMapper mapper
            , IUserStore userStore
            , ITokenHandler tokenHandler
            )
        {
            _mapper = mapper;
            _userStore = userStore;
            _tokenHandler = tokenHandler;
        }

        public async Task<bool> RegisterAsync(UserRegistrationInputModel? userModel = null, AdminRegistrationInputModel? adminModel = null)
        {
            try
            {
                var userName = userModel != null ? userModel.UserName : adminModel.UserName;
                var userPassword = userModel != null ? userModel.Password : adminModel.Password;
                var idUser = Guid.NewGuid();

                // Проверка на существование пользователя с указанным логином
                UserDB? existingUser = await CheckUserByLoginAsync(userName);
                if (existingUser != null) throw new Exception("Access denied."); // Неудачная попытка регистрации. Такой пользователь уже зарегистрирован.

                string salt = GetSalt();
                byte[] hash = GetHash(userPassword, salt);

                // Проверка уникальности хэша, если хэш не уникален – генерируется уникальный.
                // БД спроектирована так, что хэш всегда уникален (Hash поле имеет статус Uniq),
                // но подобный функционал всё равно требуется для соответствия требованиям безопасности.
                (string, byte[]) tuple = await GenerateUniqHashAsync(userPassword, salt, hash);
                salt = tuple.Item1;
                hash = tuple.Item2;

                await _userStore.InsertUserAsync(idUser, salt, hash, userName, userModel, adminModel);
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public async Task<bool> ChangeUserAsync(AdminRegistrationInputModel model)
        {
            try
            {
                // Проверка на существование пользователя с указанным логином
                UserDB? existingUser = await CheckUserByLoginAsync(model.UserName);
                if (existingUser == null) throw new Exception("Access denied."); // Неудачная попытка изменения. Такого пользователя не существует.

                existingUser = _mapper.Map<AdminRegistrationInputModel, UserDB>(model);
                await _userStore.UpdateUserAsync(existingUser);
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public async Task<bool> CreateTemporaryUserPasswordAsync(ChangePasswordDTO model)
        {
            try
            {
                // Проверка на существование пользователя с указанным логином
                UserDB? existingUser = await CheckUserByLoginAsync(model.UserName);
                if (existingUser == null) throw new Exception("Access denied."); // Неудачная попытка изменения. Такого пользователя не существует.

                string salt = GetSalt();
                byte[] hash = GetHash(model.Password, salt);

                // Проверка уникальности хэша, если хэш не уникален – генерируется уникальный.
                // БД спроектирована так, что хэш всегда уникален (Hash поле имеет статус Uniq),
                // но подобный функционал всё равно требуется для соответствия требованиям безопасности.
                (string, byte[]) tuple = await GenerateUniqHashAsync(model.Password, salt, hash);
                salt = tuple.Item1;
                hash = tuple.Item2;

                await _userStore.CreateTemporaryUserPasswordAsync(model, salt, hash, TEMPORARY_ACCESS_EXPIRATION_TIME);
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public async Task<bool> ChangePasswordAsync(ChangePasswordDTO model)
        {
            try
            {
                // Проверка на существование пользователя с указанным логином
                UserDB? existingUser = await CheckUserByLoginAsync(model.UserName);
                if (existingUser == null) throw new Exception("Access denied."); // Неудачная попытка изменения. Такого пользователя не существует.

                string salt = GetSalt();
                byte[] hash = GetHash(model.Password, salt);

                // Проверка уникальности хэша, если хэш не уникален – генерируется уникальный.
                // БД спроектирована так, что хэш всегда уникален (Hash поле имеет статус Uniq),
                // но подобный функционал всё равно требуется для соответствия требованиям безопасности.
                (string, byte[]) tuple = await GenerateUniqHashAsync(model.Password, salt, hash);
                salt = tuple.Item1;
                hash = tuple.Item2;

                await _userStore.ChangePasswordAsync(model, salt, hash);
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public async Task<TokenResponse> AuthAsync(UserAuthInputModel model)
        {
            try
            {
                var user = await CheckUserByLoginAsync(model.UserName);
                if (user == null) throw new Exception("Access denied."); // Пользователь не зарегистрирован.

                var accessAllowed = await CheckPasswordAsync(model.UserName, model.Password);

                var tokenOptions = new TokenOptions();
                var response = await CreateAccessTokenAsync(user);
                return response;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public async Task<List<UserDTO>> GetUsersAsync()
        {
            var users = await _userStore.GetUsersAsync();
            var userRoles = await _userStore.GetUserRolesAsync();
            var userStatuses = await _userStore.GetUserStatusesAsync();
            if (users == null)
            {
                throw new Exception("Пользователи не найдены");
            }
            var result = _mapper.Map<IEnumerable<UserDB>, IEnumerable<UserDTO>>(users).ToList();
            result.ForEach(u =>
            {
                u.UserRole = userRoles[int.Parse(u.UserRole)];
                u.Status = userStatuses[int.Parse(u.Status)];
            });

            return result;
        }

        /// <summary>
        /// Получение роли пользователя по логину
        /// </summary>
        /// <param name="login"></param>
        /// <returns></returns>
        public async Task<int?> GetUserRoleByLoginAsync(string login)
        {
            var user = await _userStore.GetUserByLoginAsync(login);
            return user?.UserRole;
        }

        /// <summary>
        /// Получение роли пользователя по логину
        /// </summary>
        /// <param name="login"></param>
        /// <returns></returns>
        public async Task<int?> GetUserStatusByLoginAsync(string login)
        {
            var user = await _userStore.GetUserByLoginAsync(login);
            return user?.Status;
        }

        /// <summary>
        /// Проверка пользователя по логину
        /// </summary>
        /// <param name="login"></param>
        /// <returns></returns>
        private async Task<UserDB?> CheckUserByLoginAsync(string login)
        {
            var user = await _userStore.CheckUserByLoginAsync(login);
            return user;
        }

        /// <summary>
        /// Возвращает true если хэш найден в БД.
        /// </summary>
        /// <param name="hash">хэш</param>
        private async Task<bool> GetExistsUserHashAsync(byte[] hash)
        {
            var result = await _userStore.GetExistsUserHashAsync(hash);
            return result != null ? true : false;
        }

        /// <summary>
        /// Генерирую уникальный хэш по паролю
        /// </summary>
        /// <param name="password"></param>
        /// <param name="salt"></param>
        /// <param name="hash"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        private async Task<(string, byte[])> GenerateUniqHashAsync(string password, string salt, byte[] hash)
        {
            try
            {
                (string, byte[]) tuple = (salt, hash);

                // Проверка на существование такого хэша
                var isExist = await GetExistsUserHashAsync(hash);
                // Если хэш не уникален, то начинается генерация уникального.
                if (isExist)
                {
                    var sw = new Stopwatch();
                    sw.Start();
                    while (isExist && sw.ElapsedMilliseconds < 5000)
                    {
                        hash = GetHash(password, salt);
                        isExist = await GetExistsUserHashAsync(hash);
                    }
                    sw.Stop();
                    tuple = (salt, hash);
                }
                return tuple;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Генерация хеша для пароля с применением растягивания.
        /// </summary>
        /// <param name="password">Пароль</param>
        /// <param name="salt">Соль</param>
        private byte[] GetHash(string password, string salt)
        {
            // генерация соли
            byte[] saltBytes = Encoding.UTF8.GetBytes(salt);

            // генерация хэша
            Rfc2898DeriveBytes pbkdf2 = new Rfc2898DeriveBytes(password, saltBytes, ITERATIONS, HASH_ALGORITHM);
            return pbkdf2.GetBytes(HASH_SIZE);
        }

        /// <summary>
        /// Сгенерировать соль.
        /// </summary>
        /// <returns></returns>
        private string GetSalt()
        {
            var cryptoProvider = new RNGCryptoServiceProvider();
            byte[] b_salt = new byte[SALT_SIZE];
            cryptoProvider.GetBytes(b_salt);
            return Convert.ToBase64String(b_salt);
        }

        /// <summary>
        /// Проверка соответствия логина и пароля аккаунту
        /// </summary>
        /// <param name="userName">Логин пользователя</param>
        /// <param name="password">Пароль пользователя</param>
        /// <remarks>TODO: Метод можно забрутфорсить если не поставить ограничение на вызов метода с одного IP адреса.</remarks>
        /// <returns>true or false</returns>
        public async Task<bool> CheckPasswordAsync(string userName, string password)
        {
            UserDB? user = await _userStore.CheckUserByLoginAsync(userName);
            UserHashDB? userHash = await _userStore.GetUserHashAsync(userName);
            if (userHash == null || user == null)
            {
                throw new Exception("Access denied."); // Пользователь с таким логином не зарегистрирован
            }

            switch ((UserStatus)userHash.Status!)
            {
                case UserStatus.Blocked:
                    throw new Exception("Access denied."); // Пользователь с таким логином заблокирован.
                case UserStatus.Declined:
                    throw new Exception("Access denied."); // Пользователь с таким логином отклонён администрацией.
            }

            try
            {
                if (await this.CheckBlockedAsync(user))
                {
                    throw new Exception("Ваша учетная запись заблокирована на " + (BLOCK_TIME / 60).ToString() + " мин.");
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }

            var salt = userHash!.Salt;
            byte[] hash = GetHash(password, salt!);

            // Проверка хэш пароля на соответствие, если не соответствует - добавляется счётчик неудачных входов
            if (!hash.SequenceEqual(userHash.Hash!))
            {
                try
                {
                    await this.AccessFailedAsync(user);
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message, ex);
                }
                throw new Exception("Access denied."); // Логин или пароль указаны неверно.
            }

            return true;
        }

        /// <summary>
        /// Проверка блокировки
        /// </summary>
        private async Task<bool> CheckBlockedAsync(UserDB user)
        {
            if (user.LockoutEnabled && user.LockoutEnd != null)
            {
                var blockTime = DateTime.UtcNow - user.LockoutEnd.Value.ToUniversalTime();
                if (blockTime.TotalSeconds >= BLOCK_TIME)
                {
                    user.AccessFailedCount = 0;
                    user.LockoutEnd = null;
                    user.LockoutEnabled = false;
                    await _userStore.UpdateUserAsync(user);

                    return false;
                }
                else
                {
                    return true;
                }
            }
            else if (user.LockoutEnabled)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// Добавить неудачную попытку входа
        /// </summary>
        private async Task AccessFailedAsync(UserDB user)
        {
            user.AccessFailedCount++;
            if (user.AccessFailedCount >= ATTEMPT_COUNT)
            {
                var blockedTime = DateTime.UtcNow.AddSeconds(BLOCK_TIME);
                user.LockoutEnd = blockedTime;
                user.LockoutEnabled = true;
            }
            await _userStore.UpdateUserAsync(user);
        }

        /// <summary>
        /// Создание токена при входе в аккант с проверкой логина и пароля
        /// </summary>
        /// <returns>Возвращает токен при успехе</returns>
        public async Task<TokenResponse> CreateAccessTokenAsync(UserDB user)
        {
            try
            {
                // Формирование токена
                var token = _tokenHandler.CreateAccessToken(user);

                return new TokenResponse(true, null, token);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }
    }
}
