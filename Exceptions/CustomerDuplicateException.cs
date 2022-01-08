using System;

namespace BuildRestApiNetCore.Exceptions
{
    public class CustomerDuplicateException : Exception
    {
        public CustomerDuplicateException()
        {
        }

        public CustomerDuplicateException(string message) : base(message)
        {
        }

        public CustomerDuplicateException(string message, Exception inner): base(message, inner)
        {
        }
    }
}