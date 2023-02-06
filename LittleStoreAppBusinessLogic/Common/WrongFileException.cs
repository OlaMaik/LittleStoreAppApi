using System;

namespace LittleStoreAppBusinessLogic.Common
{
    public class WrongFileException : Exception
    {
        public WrongFileException()
        {
        }

        public WrongFileException(string message)
            : base(message)
        {
        }

        public WrongFileException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
