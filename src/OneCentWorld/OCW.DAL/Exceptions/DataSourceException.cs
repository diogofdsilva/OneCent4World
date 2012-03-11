using System;

namespace OCW.DAL.Exceptions
{
    public class DataSourceException : Exception
    {
        #region Constructors
        public DataSourceException() {}
        public DataSourceException(string message) :base(message) {}
        #endregion
    }
}
