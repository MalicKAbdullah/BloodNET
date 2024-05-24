namespace BloodNET_Web.Models
{
    public class Donation
    {
        public int Id {  get; set; }

        public string DonorId { get; set; }

        public int RequestId {  get; set; }

        public DateTime Created { get; set; }

        public int Status { get; set; }

        public Donation()
        {
            Id = 0;
            RequestId = 0;
            DonorId = string.Empty;
            Status = 0;
            Created = DateTime.Now;
        }

        public Donation(Donation obj)
        {
            this.Id = obj.Id;
            this.DonorId = obj.DonorId;
            this.RequestId = obj.RequestId;
            this.Status = obj.Status;
            this.Created = obj.Created;
        }



    }
}
