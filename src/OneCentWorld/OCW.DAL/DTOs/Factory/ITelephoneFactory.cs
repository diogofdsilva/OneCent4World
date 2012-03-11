namespace OCW.DAL.DTOs.Factory
{
    public interface ITelephoneFactory
    {
        Telephone Create(int id, string telephone, int profileId);
    }
}
