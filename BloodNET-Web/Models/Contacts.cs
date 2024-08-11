using BloodNET_Web.Models.CustomAttributes;

namespace BloodNET_Web.Models
{
    public class Contacts
    {
        public string Name { get; set; }
        public string Email { get; set; }


        [PhoneFormat()]
        public string PhoneNumber { get; set; }

        public string Subject { get; set; }

        public string Body { get; set; }

        public Contacts()
        {
            Name = string.Empty;
            Email = string.Empty;
            PhoneNumber = string.Empty;
            Subject = string.Empty;
            Body = string.Empty;
        }

        public void Add(Contacts contact)
        {
            if (contact == null) throw new ArgumentNullException();

            this.Name = contact.Name;
            this.Email = contact.Email;
            this.PhoneNumber = contact.PhoneNumber;
            this.Subject = contact.Subject;
            this.Body = contact.Body;
        }

    }
}
