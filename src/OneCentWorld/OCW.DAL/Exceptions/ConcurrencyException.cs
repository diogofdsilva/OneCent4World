using System;

namespace OCW.DAL.Exceptions
{
    public class ConcurrencyException : Exception
    {
        #region Constructors
        public ConcurrencyException() {}
        public ConcurrencyException(string message) : base(message) { }
        #endregion
    }
}
