using BloodNET_Web.Models;

namespace BloodNET_Web.Models.Interfaces
{
    public interface IBloodDonors
    {
        public void Add(BloodDonors bloodDonors, (string name, string email) var);

        public BloodDonors GetDonorById(string id);


        public void UpdateStatus(string donorId, int status);

        public List<BloodDonors> GetDonorsById(List<string> ls);

        public int Eligibility(string userId);




    }
}
