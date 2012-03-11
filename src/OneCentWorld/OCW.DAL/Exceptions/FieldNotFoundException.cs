using System;

namespace OCW.DAL.Exceptions
{
    public class FieldNotFoundException<TField> : Exception
    {
        #region Properties
        public TField Field { get; private set; }
        public string FieldName { get; private set; }
        public string Record { get; private set; }
        #endregion

        #region Constructors
        public FieldNotFoundException(string record, TField field, string fieldName)
            : base(string.Format("{0} with {1} {2} not found", record, fieldName, field))
        {
            Field = field;
            FieldName = fieldName;
            Record = record;
        }
        #endregion
    }
}
