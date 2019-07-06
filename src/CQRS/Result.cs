using System;

namespace CQRS
{
    public class Result
    {
        protected Result(bool success, string errorMessage)
        {
            if (success && errorMessage != null) throw new ArgumentException("Result cannot be successful and have an errormessage");
            if (!success && errorMessage == null) throw new ArgumentException("Result cannot be failure and not have an errormessage");
            Success = success;
            ErrorMessage = errorMessage;
        }

        public bool Success { get; }
        public string ErrorMessage { get; }
        public bool Failure => !Success;

        public static Result Fail(string message) => new Result(false, message);
        public static Result<T> Complete<T>(T value) => new Result<T>(true, value);
        public static Result Complete() => new Result(true, null);
    }

    public class Result<T> : Result
    {
        public T Value { get; set; }

        protected internal Result(bool success, T value) : base(success, null)
        {
            Value = value;
        }
    }
}