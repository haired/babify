namespace Babify.Services
{
    public class ServiceResult
    {
        public bool IsSuccess { get; set; }
        public string Error { get; set; }
        public static ServiceResult ValidationError(string message = null)
        {
            return new ServiceResult { IsSuccess = false, Error = message ?? "Data supplied was incorrect" };
        }

        public static ServiceResult<T> ValidationError<T>(string message = null)
        {
            return new ServiceResult<T> { IsSuccess = false, Error = message ?? "Data supplied was incorrect" };
        }

        public static ServiceResult Succes()
        {
            return new ServiceResult { IsSuccess = true };
        }

        public static ServiceResult<T> Succes<T>(T result)
        {
            return new ServiceResult<T> { IsSuccess = true, Result = result };
        }
    }

    public class ServiceResult<T> : ServiceResult
    {
        public T Result { get; set; }       
    }
}
