using System;

namespace OCW.DAL.Exceptions
{
    public class ConfigurationException : Exception
    {
        #region Constructors
        public ConfigurationException() { }

        public ConfigurationException(string message) : base(message) { }
        #endregion
    }
}
