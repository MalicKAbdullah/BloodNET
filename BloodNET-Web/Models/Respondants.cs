namespace BloodNET_Web.Models
{
    public class Respondants
    {
        public int Id { get; set; }
        public List<List<BloodDonors>> BloodDonors { get; set; }

        public List<BloodRequests> BloodRequests { get; set; }

        public List<DateTime> DateTimes { get; set; }
    }
}
