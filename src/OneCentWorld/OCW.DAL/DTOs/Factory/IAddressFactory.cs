namespace OCW.DAL.DTOs.Factory
{
    public interface IAddressFactory
    {
        Address Create(int id, string address1, string address2, string postalCode, string city, int countryId);
    }
}
