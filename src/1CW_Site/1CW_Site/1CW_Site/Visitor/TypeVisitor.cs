using OCW.DAL.DTOs;
using OCW.DAL.Visitor;

namespace _1CW_Site.Visitor
{
    public class TypeVisitor : IVisitor
    {
        public string TypeOfProfile { get; private set; }

        #region Implementation of IVisitor

        public void Visit(Profile profile)
        {
            profile.Accept(this);
        }

        public void Visit(Company company)
        {
            TypeOfProfile = "Company";
        }

        public void Visit(Organization organization)
        {
            TypeOfProfile = "ONG";
        }

        public void Visit(Person person)
        {
            TypeOfProfile = "Person";
        }

        #endregion
    }
}