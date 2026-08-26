namespace CarStore.Shared.CommonResult
{
    public enum ErrorType
    {
        Failure = 0,
        NotFound = 1,
        Unauthorized = 2,
        Validation = 3,
        Forbidden = 4,
        InvalidCredentials = 5,
        Conflict = 6 //Validate Business Rule
    }
}
