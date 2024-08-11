using BloodNET_Web.Models.CustomAttributes;

namespace BloodNET_Web.Models
{
    public class BloodDonors
    {
        public string DonorId {  get; set; }
        public string Name {  get; set; }
        public DateTime DOB {  get; set; }
        public string Gender {  get; set; }

        public float Weight { get; set; }

        public string WeightUnit { get; set; }

        public string BloodGroup { get; set; }

        public string Email {  get; set; }

        public string Address { get; set; }

        public string City { get; set; }

        public string Country { get; set; }

        public string MedicalHistory {  get; set; }

        public int DonorStatus { get; set; }

        public IFormFile Image { get; set; }

        public string ImgUrl { get; set; }

        [PhoneFormat(ErrorMessage = "Please enter a valid phone number.")]
        public string PhoneNumber { get; set; }

        public BloodDonors()
        {
            DOB = new DateTime();
            Gender = "---";
            Weight = 0.0f;
            WeightUnit = "kg";
            BloodGroup = "---";
            PhoneNumber = "---";
            Address = "---";
            City = "---";
            Country = "---";
            MedicalHistory = "---";
            DonorStatus = 1;

        }

        public BloodDonors(string userId)
        {

        }

        public BloodDonors(BloodDonors bloodDonors)
        {
            this.DonorId = bloodDonors.DonorId;
            this.Name = bloodDonors.Name;
            this.DOB = bloodDonors.DOB;
            this.Gender = bloodDonors.Gender;
            this.Weight = bloodDonors.Weight;
            this.WeightUnit = bloodDonors.WeightUnit;
            this.BloodGroup = bloodDonors.BloodGroup;
            this.PhoneNumber = bloodDonors.PhoneNumber;
            this.Email = bloodDonors.Email;
            this.Address = bloodDonors.Address;
            this.Country = bloodDonors.Country;
            this.City = bloodDonors.City;
            this.MedicalHistory = bloodDonors.MedicalHistory;
            this.DonorStatus = bloodDonors.DonorStatus;
            this.ImgUrl = bloodDonors.ImgUrl;
        }

    }
}
