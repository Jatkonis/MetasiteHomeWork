using System;

namespace TradingPlaces.Core.Exceptions
{
    public abstract class AppException : Exception
    {
        protected AppException(string message) : base(message)
        {
        }
    }

    public class NotFoundException : AppException
    {
        private NotFoundException(string message) : base(message)
        {
        }

        public static NotFoundException ExpectedExistingStrategy(string ticker)
        {
            return new NotFoundException($"Strategy with {ticker} was not found.");
        }
    }    
}
