using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using SecurityService_AspNetCore.Services.Communication;
using SecurityService_Core.Interfaces;
using SecurityService_Core.Models.ControllerDTO.Administrator;
using SecurityService_Core.Models.ControllerDTO.Operator;
using SecurityService_Core.Models.ControllerDTO.User;
using SecurityService_Core.Models.DB;
using SecurityService_Core.Models.DTO;
using SecurityService_Core.Models.Enums;
using SecurityService_Core.Security.Auth;
using SecurityService_Core_Stores.Stores;
using System.Diagnostics;
using System.Security.Cryptography;
using System.Text;

namespace Security_Service_AspNetCore.Services
{
    /// <summary>
    /// Сервис обработки файлов и связанных сущностей
    /// </summary>
    public class UserService
    {
        private readonly IMapper _mapper;
        private readonly IUserStore _userStore;
        private readonly IAdministratorStore _adminStore;
        private readonly ITokenHandler _tokenHandler;

        private readonly AuthOptions authOptions = new();
        public IConfiguration Configuration { get; }

        public const int SALT_SIZE = 32;
        public const int HASH_SIZE = 32;
        public const int ITERATIONS = 100000;
        private readonly HashAlgorithmName HASH_ALGORITHM = HashAlgorithmName.SHA512;

        public const int TEMPORARY_ACCESS_EXPIRATION_TIME = 30 * 60; // длительность временного доступа после сброса пароля
        public const int BLOCK_TIME = 15 * 60; // время блокировки

        /// <summary>
        /// Конструктор сервиса файлов
        /// </summary>
        /// <param name="mapper"></param>
        /// <param name="userStore"></param>
        /// <param name="adminStore"></param>
        /// <param name="tokenHandler"></param>
        /// <param name="configuration"></param>
        public UserService(
            IMapper mapper
            , IUserStore userStore
            , IAdministratorStore adminStore
            , ITokenHandler tokenHandler
            , IConfiguration configuration
            )
        {
            Configuration = configuration;
            _mapper = mapper;
            _userStore = userStore;
            _adminStore = adminStore;
            _tokenHandler = tokenHandler;
            Configuration.GetSection("AuthOptions").Bind(authOptions);
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
                if (existingUser != null) throw new Exception("Неудачная попытка регистрации. Такой пользователь уже зарегистрирован.");

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

        public async Task<bool> ChangeUserAsync(AdminChangeInputModel model)
        {
            try
            {
                // Проверка на существование пользователя с указанным логином
                UserDB? existingUser = await CheckUserByLoginAsync(model.UserName);
                if (existingUser == null) throw new Exception("Неудачная попытка изменения. Такого пользователя не существует.");
                existingUser.UserRole = model.Role;
                existingUser.State = model.State;
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
                if (existingUser == null) throw new Exception("Неудачная попытка изменения. Такого пользователя не существует."); 

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
                if (existingUser == null) throw new Exception("Неудачная попытка изменения. Такого пользователя не существует."); 

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
                if (user == null) throw new Exception("Логин или пароль указаны неверно.");

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
            if (users == null)
            {
                throw new Exception("Пользователи не найдены");
            }
            var result = _mapper.Map<IEnumerable<UserDB>, IEnumerable<UserDTO>>(users).ToList();
            result.ForEach(u =>
            {
                u.UserRole = userRoles[int.Parse(u.UserRole)];
            });

            return result;
        }

        public async Task<int?> GetRoleAsync(string userName)
        {
            var user = await _userStore.GetUserByLoginAsync(userName);
            if (user == null) throw new Exception("Пользователь не найден");
            return user.UserRole;
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
            return user?.State;
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
        /// <returns>true or false</returns>
        public async Task<bool> CheckPasswordAsync(string userName, string password)
        {
            UserDB? user = await _userStore.CheckUserByLoginAsync(userName);
            UserHashDB? userHash = await _userStore.GetUserHashAsync(userName);
            if (userHash == null || user == null)
            {
                throw new Exception("Логин или пароль указаны неверно.");
            }

            bool isUserPasswordExpired = await CheckIsUserPasswordExpiredAsync(user);
            if (isUserPasswordExpired)
            {
                throw new Exception("Временный пароль для доступа к вашей учётной записи истёк.");
            }

            var salt = userHash!.Salt;
            byte[] hash = GetHash(password, salt!);

            // Проверка хэш пароля на соответствие, если не соответствует - добавляется счётчик неудачных входов
            if (!hash.SequenceEqual(userHash.Hash!))
            {
                try
                {
                    await AddFailedAuthAsync(user);
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message, ex);
                }
                throw new Exception("Логин или пароль указаны неверно.");
            }

            bool isUserBlocked = await CheckIsUserBlockedAsync(user);
            if (isUserBlocked)
            {
                throw new Exception("Ваша учетная запись заблокирована на " + (authOptions.BlockTime).ToString() + " мин.");
            }

            switch ((UserStatus)user.State)
            {
                case UserStatus.Blocked:
                    throw new Exception("Пользователь с таким логином заблокирован."); // 
                case UserStatus.Declined:
                    throw new Exception("Пользователь с таким логином отклонён администрацией."); // 
            }

            return true;
        }

        /// <summary>
        /// Проверка блокировки
        /// </summary>
        private async Task<bool> CheckIsUserBlockedAsync(UserDB user)
        {
            // не нулевая дата и признак блокировки - маркер текущей блокировки, но она могла закончиться по времени
            if (user.LockoutEnabled ?? false
                && user.AccessFailedAttemptDate != null)
            {
                // проверяем кончилась ли блокировка
                var dateNow = DateTime.UtcNow;
                var dateWhenBlocked = user.AccessFailedAttemptDate.Value;
                var blockTimeDiff = dateNow - dateWhenBlocked;
                // разница текущего времени должна превышать дату блокировки на время блокировки
                if (blockTimeDiff.TotalSeconds >= authOptions.BlockTime * 60)
                {
                    await WhenBlockIsOutdated(user);
                    return false;
                }
                // блокировка ещё не закончилась
                else
                {
                    return true;
                }
            }
            // если время блокировки null, значит блокировка вечная
            else if (user.LockoutEnabled ?? false && user.AccessFailedAttemptDate == null)
            {
                return true;
            }
            // если блокировка не включена, а дата последней попытки входа с ошибкой не нулевая
            // тогда пропускаем пользователя
            return false;
        }

        /// <summary>
        /// Проверка истечения срока действия пароля для входа в аккаунт
        /// </summary>
        private async Task<bool> CheckIsUserPasswordExpiredAsync(UserDB user)
        {
            // Если переменная со временным доступом к аккаунту нулевая, значит аккаунт без ограничений доступа
            // И если включен флаг IsTemporaryAccess
            if (user.TemporaryAccessExpirationTime != null || user.IsTemporaryAccess == true)
            {
                // проверяем кончилась ли блокировка
                var dateNow = DateTime.UtcNow;
                var dateWhenAccessWillBeFired = user.TemporaryAccessExpirationTime.Value;
                var blockTimeDiff = dateNow - dateWhenAccessWillBeFired;

                // разница текущего времени должна быть меньше даты истечения срока годности пароля на время блокировки
                if (blockTimeDiff.TotalSeconds < authOptions.BlockTime * 60)
                {
                    return false;
                }
                // Если разница больше - значит пользователь опоздал с аутентификацией.
                else
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Снимаем ограничения с пользователя если блокировка устарела
        /// </summary>
        /// <param name="user">Пользователь</param>
        /// <returns>true заблокирован, false не заблокирован</returns>
        private async Task WhenBlockIsOutdated(UserDB user)
        {
            // Время блокировки прошло. Снимаем ограничения, обнуляем дату и счётчик блокировки
            user.AccessFailedCount = 0;
            user.AccessFailedAttemptDate = null;
            user.LockoutEnabled = false;
            await _userStore.UpdateUserAsync(user);
        }

        /// <summary>
        /// Засчитать ошибочную попытку входа в аккаунт, если они превышают лимит - пользователь получает блокировку
        /// </summary>
        private async Task AddFailedAuthAsync(UserDB user)
        {
            // если есть данные о последней неудачной попытке входа - значит попытка не первая,
            // и нужно проверить её актуальность, или обнулить её
            if (user.AccessFailedAttemptDate != null)
            {
                // если последняя попытка входа была давно, то считаем эту провальную попытку первой
                var dateNow = DateTime.UtcNow;
                var dateWhenBlocked = user.AccessFailedAttemptDate.Value;
                var blockTimeDiff = dateNow - dateWhenBlocked;
                if (blockTimeDiff.TotalSeconds >= authOptions.BlockTime * 60)
                {
                    user.AccessFailedCount = 1;
                }
                else
                {
                    user.AccessFailedCount++; // инкрементируем количество неудачных попыток входа.
                }
            }
            // если дата null, значит провальная попытка входа - первая.
            else
            {
                user.AccessFailedCount = 1;
            }

            // если ошибок ввода пароля больше лимита - пользователь получает блокировку
            if (user.AccessFailedCount >= authOptions.FailedAttempCountLimit)
            {
                user.LockoutEnabled = true;
            }
            user.AccessFailedAttemptDate = DateTime.UtcNow;
            // обновляем данные о блокировке пользователя в БД
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

                return new TokenResponse(true, null, token, user.UserRole);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }
    }
}
