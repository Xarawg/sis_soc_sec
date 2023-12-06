namespace SecurityService_AspNetCore.Configurations
{
    /// <summary>
    /// Результат.
    /// </summary>
    public class Result
    {
        /// <summary>
        /// Признак успеха.
        /// </summary>
        public virtual bool Success { get; set; }

        /// <summary>
        /// Ошибка. Если не переопределено, то возвращается первая ошибка из списка.
        /// </summary>
        public virtual string? Error
        {
            get
            {
                if (Errors.Count > 0)
                    return Errors[0];
                return null;
            }
        }

        /// <summary>
        /// Список ошибок.
        /// </summary>
        public virtual List<string> Errors { get; private set; } = new List<string>();

        /// <summary>
        /// Признак наличия ошибок.
        /// </summary>
        public virtual bool HasErrors
        {
            get
            {
                return Errors.Count > 0;
            }
        }

        /// <summary>
        /// Создает успешный результат.
        /// </summary>
        public static Result CreateSuccess()
        {
            return new Result()
            {
                Success = true
            };
        }

        /// <summary>
        /// Создает неуспешный результат.
        /// </summary>
        public static Result CreateFailure()
        {
            return new Result()
            {
                Success = false
            };
        }

        /// <summary>
        /// Создает неуспешный результат.
        /// </summary>
        /// <param name="error">Ошибка.</param>
        public static Result CreateFailure(string error)
        {
            var result = CreateFailure();
            result.AddError(error);
            return result;
        }

        /// <summary>
        /// Создает неуспешный результат.
        /// </summary>
        /// <param name="errors">Список ошибок.</param>
        public static Result CreateFailure(IEnumerable<string> errors)
        {
            var result = CreateFailure();
            result.AddErrors(errors);
            return result;
        }

        /// <summary>
        /// Добавить ошибку.
        /// </summary>
        /// <param name="error">Ошибка.</param>
        public virtual void AddError(string error)
        {
            Errors.Add(error);
        }

        /// <summary>
        /// Добавить ошибки.
        /// </summary>
        /// <param name="errors">Список ошибок.</param>
        public virtual void AddErrors(IEnumerable<string> errors)
        {
            Errors.AddRange(errors);
        }
    }

    /// <summary>
    /// Результат со значением.
    /// </summary>
    public class Result<TValue> : Result
    {

        /// <summary>
        /// Значение (содержимое результата).
        /// </summary>
        public TValue? Value { get; set; }

        /// <summary>
        /// Создает успешный результат.
        /// </summary>
        public new static Result<TValue> CreateSuccess()
        {
            return new Result<TValue>()
            {
                Success = true
            };
        }

        /// <summary>
        /// Создает успешный результат.
        /// </summary>
        /// <param name="value">Значение.</param>
        public static Result<TValue> CreateSuccess(TValue value)
        {
            var result = CreateSuccess();
            result.Value = value;
            return result;
        }

        /// <summary>
        /// Создает неуспешный результат.
        /// </summary>
        public new static Result<TValue> CreateFailure()
        {
            return new Result<TValue>()
            {
                Success = false
            };
        }

        /// <summary>
        /// Создает неуспешный результат.
        /// </summary>
        /// <param name="error">Ошибка.</param>
        public new static Result<TValue> CreateFailure(string error)
        {
            var result = CreateFailure();
            result.AddError(error);
            return result;
        }

        /// <summary>
        /// Создает неуспешный результат.
        /// </summary>
        /// <param name="errors">Список ошибок.</param>
        public new static Result<TValue> CreateFailure(IEnumerable<string> errors)
        {
            var result = CreateFailure();
            result.AddErrors(errors);
            return result;
        }
    }
}
