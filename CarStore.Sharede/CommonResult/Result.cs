namespace CarStore.Shared.CommonResult
{
    public class Result
    {
        public bool IsSucceeded { get; }

        public bool IsFailed => !IsSucceeded;
        private List<Error> _errors = [];
        public string? SuccessMessage { get; }

        public IReadOnlyList<Error> Errors
        {
            get { return _errors; }

        }

        //constructors
        protected Result()
        {
            IsSucceeded = true;
        }
        protected Result(Error error)
        {
            _errors.Add(error);
        }
        protected Result(List<Error> errors)
        {
            _errors.AddRange(errors);
        }
        protected Result(string successMessage)
        {
            IsSucceeded = true;
            SuccessMessage = successMessage;
        }


        //Factory of Methods
        public static Result Ok() => new Result();

        public static Result Ok(string message) => new Result(message);
        public  static  Result Fail(Error error) => new Result(error);
        public static Result Fail(List<Error> errors) => new Result(errors);

        ////Implicit Casting Operator 
        public static implicit operator Result(Error error)=>Fail(error);
        public static implicit operator Result(List<Error> errors)=>Fail(errors);


    }
}
