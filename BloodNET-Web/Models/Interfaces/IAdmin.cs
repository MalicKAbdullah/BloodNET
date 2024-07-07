namespace BloodNET_Web.Models.Interfaces
{
    public interface IAdmin
    {
        public List<MyUsers> GetUsers();
        public List<Donation> GetDonations();

        public void DeleteUser(string id);
    }
}
