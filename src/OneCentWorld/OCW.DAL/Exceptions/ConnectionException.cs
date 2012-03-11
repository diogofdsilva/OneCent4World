using System;

namespace OCW.DAL.Exceptions
{
    public class ConnectionException : Exception
    {
        #region Constructors
        public ConnectionException() {}

        public ConnectionException(string message) : base(message) { }
        #endregion
    }
}
