namespace CarStore.Shared.CommonResult
{
    public class Result<TValue> : Result
    {
        private TValue _value;
        public TValue Value => IsSucceeded ? _value : throw new InvalidOperationException("Operation Is Not Succeeded");


        private Result(TValue value) : base()
        {
            _value = value;

        }
        private Result(Error error) : base(error)
        {
            _value = default!;
        }
        private Result(List<Error> errors) : base(errors)
        {
            _value = default!;
        }

        //Methods Factory
        public static Result<TValue> Ok(TValue value) => new Result<TValue>(value);
        public new static Result<TValue> Fail(Error error) => new Result<TValue>(error);
        public new static Result<TValue> Fail(List<Error> errors) => new Result<TValue>(errors);

        ////Implicit Casting Operator 
        public static implicit operator Result<TValue>(TValue value) => Ok(value);
        public static implicit operator Result<TValue>(Error error) => Fail(error);
        public static implicit operator Result<TValue>(List<Error> errors) => Fail(errors);



    }
}
