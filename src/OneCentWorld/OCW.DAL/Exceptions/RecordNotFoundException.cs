using System;

namespace OCW.DAL.Exceptions
{
    public class RecordNotFoundException<TKey> : Exception
    {
        #region Properties
        public TKey Key { get; private set; }
        public string Record { get; private set;}
        #endregion

        #region Constructors
        public RecordNotFoundException(string record, TKey key)
            :base(string.Format("{0} with id {1} not found", record, key))
        {
            Key = key;
            Record = record;
        }
        #endregion
    }
}
