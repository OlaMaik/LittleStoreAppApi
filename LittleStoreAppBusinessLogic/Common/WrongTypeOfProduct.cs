using System;

namespace LittleStoreAppBusinessLogic.Common
{
    public class WrongTypeOfProduct : Exception
    {
        public WrongTypeOfProduct()
        {
        }

        public WrongTypeOfProduct(string message)
            : base(message)
        {
        }

        public WrongTypeOfProduct(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
