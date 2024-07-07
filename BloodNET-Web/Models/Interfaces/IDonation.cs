namespace BloodNET_Web.Models.Interfaces
{
    public interface IDonation
    {
        public List<(string, DateTime)> GetDonors(int reqId);
        public List<(string donorId, int reqId)> GetDonations(string donorId);
        public  bool getDonorStatus(string donorId);
        public void UpdateStatus(int donationId, int status);
        public int getDonationId(string donorId, int reqId);
    }
}
