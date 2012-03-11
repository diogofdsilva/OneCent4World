namespace OCW.DAL.DTOs.Factory
{
    public interface ICountryFactory
    {
        Country Create(int id, string name, string alpha2, string alpha3, string numeric);
    }
}
