namespace BloodNET_Web.Models
{
    public class BloodRequests
    {
        public int Id { get; set; }
        public string BloodGroup { get; set; }
        public DateTime DTime { get; set; }

        public string RecipientName {  get; set; }

        public string RecipientPhone { get; set; }

        public string Location { get; set; }

        public string Description {  get; set; }

        public int Status { get; set; }

        public BloodRequests(string bloodGroup, DateTime dTime, string recipientName, string recipientPhone, string location, string description)
        {
            BloodGroup = bloodGroup;
            DTime = dTime;
            RecipientName = recipientName;
            RecipientPhone = recipientPhone;
            Location = location;
            Description = description;
            Status = 1;
        }
    }
}
