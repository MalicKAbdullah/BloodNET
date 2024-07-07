namespace BloodNET_Web.Models.Interfaces
{
    public interface IBloodRequests
    {
        public void Add(BloodRequests bloodRequests);
        public List<BloodRequests> SearchByType(string type, string userId);
        public BloodRequests GetbyId(int rid);
        public void UpdateStatus(int reqId, string status);
        public List<BloodRequests> Get(string id);
        public List<BloodRequests> GetAll();
        public List<BloodRequests> GetAllExcept(string userId);
        public void availibleRequests(List<BloodRequests> bloodRequests, List<(string donorId, int reqId)> donations);
    }
}
