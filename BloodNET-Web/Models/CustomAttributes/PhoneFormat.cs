using System.ComponentModel.DataAnnotations;

namespace BloodNET_Web.Models.CustomAttributes
{
    public class PhoneFormat:ValidationAttribute
    {

        public PhoneFormat() { }


        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {

            string phone = value.ToString();

            Console.WriteLine("Phone: " + phone);


            if (phone.Length == 11)
            {
                foreach (var digit in phone)
                {
                    if (!(digit >= '0' && digit <= '9'))
                        return new ValidationResult("Only (0 - 9) are allowed");
                }
                return ValidationResult.Success;
            }
            return new ValidationResult("Invalid Format (Length should be 11)");

        }

    }
}
