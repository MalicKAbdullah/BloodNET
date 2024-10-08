﻿using BloodNET_Web.Models.CustomAttributes;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using NuGet.Protocol.Core.Types;

namespace BloodNET_Web.Models
{
    public class BloodRequests
    {
        public int Id { get; set; }
        public string BloodGroup { get; set; }
        public DateTime DTime { get; set; }

        public string RecipientName {  get; set; }

        [PhoneFormat()]
        public string RecipientPhone { get; set; }

        public string Location { get; set; }

        public string Description {  get; set; }

        public int Status { get; set; }

        public string userId { get; set; }

        public BloodRequests()
        {

        }

        public BloodRequests(int id,string bloodGroup, DateTime dTime, string recipientName, string recipientPhone, string location, string description,string userID)
        {
            Id = id;
            BloodGroup = bloodGroup;
            DTime = dTime;
            RecipientName = recipientName;
            RecipientPhone = recipientPhone;
            Location = location;
            Description = description;
            Status = 1;
            userId = userID;
        }

        public BloodRequests(string bloodGroup, DateTime dTime, string recipientName, string recipientPhone, string location, string description, string userID)
        {

            BloodGroup = bloodGroup;
            DTime = dTime;
            RecipientName = recipientName;
            RecipientPhone = recipientPhone;
            Location = location;
            Description = description;
            Status = 1;
            userId = userID;
        }
    }
}
