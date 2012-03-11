using OCW.DAL.DTOs;

namespace OCW.DAL.Visitor
{
    public interface IVisitor
    {
        void Visit(Profile profile);
        void Visit(Company company);
        void Visit(Organization organization);
        void Visit(Person person);
    }
}
