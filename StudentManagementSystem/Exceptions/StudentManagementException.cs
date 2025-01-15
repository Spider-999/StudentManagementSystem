using System;

namespace StudentManagementSystem.Exceptions
{
    public class StudentControllerException : Exception
    {
        public int ErrorCode { get; }

        // Constructor
        public StudentControllerException() { }

        // Constructor with error message
        public StudentControllerException(string message)
            : base(message) { }

        // Constructor with error message and error code
        public StudentControllerException(string message, int errorCode)
            : base(message)
        {
            ErrorCode = errorCode;
        }

        // Override ToString for custom error message
        public override string ToString()
        {
            return $"Error Code: {ErrorCode}, Message: {Message}";
        }
    }

    public class ProfessorControllerException : Exception
    {
        //Same principles as StudentControllerException
        public int ErrorCode { get; }

        // Constructor
        public ProfessorControllerException() { }

        // Constructor with error message
        public ProfessorControllerException(string message)
            : base(message) { }

        // Constructor with error message and error code
        public ProfessorControllerException(string message, int errorCode)
            : base(message)
        {
            ErrorCode = errorCode;
        }

        // Override ToString for custom error message
        public override string ToString()
        {
            return $"Error Code: {ErrorCode}, Message: {Message}";
        }
    }
}
