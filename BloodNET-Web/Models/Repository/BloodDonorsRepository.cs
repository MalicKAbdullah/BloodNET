﻿using BloodNET_Web.Data;
using BloodNET_Web.Models.Interfaces;
using Microsoft.AspNet.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;

namespace BloodNET_Web.Models.Repository
{
    public class BloodDonorsRepository:IBloodDonors
    {
        private readonly ApplicationDbContext _context;
        private SqlConnection sqlConnection;

        public BloodDonorsRepository(ApplicationDbContext context)
        {
            _context = context;
            sqlConnection = new SqlConnection(_context.Database.GetConnectionString());

        }

        public void Add(BloodDonors bloodDonors,(string name,string email) var)
        {
            string insertQuery = "INSERT INTO BloodDonors(DonorId, Name, DOB, Gender, Weight, WeightUnit, BloodGroup, PhoneNumber, Email, Address, City, Country, MedicalHistory, DonorStatus,imgUrl) VALUES(@donorId, @name, @dob, @gender, @weight, @weightUnit, @bloodGroup, @phoneNumber, @email, @address, @city, @country, @MedicalHistory, @DonorStatus,@imgurl)";

            sqlConnection.Open();
            SqlCommand insertCommand = new SqlCommand(insertQuery, sqlConnection);
            insertCommand.Parameters.AddWithValue("donorId", bloodDonors.DonorId);
            insertCommand.Parameters.AddWithValue("name", var.name);
            insertCommand.Parameters.AddWithValue("dob", bloodDonors.DOB);
            insertCommand.Parameters.AddWithValue("gender", bloodDonors.Gender);
            insertCommand.Parameters.AddWithValue("weight", bloodDonors.Weight);
            insertCommand.Parameters.AddWithValue("weightUnit", bloodDonors.WeightUnit);
            insertCommand.Parameters.AddWithValue("bloodGroup", bloodDonors.BloodGroup);
            insertCommand.Parameters.AddWithValue("phoneNumber", bloodDonors.PhoneNumber);
            insertCommand.Parameters.AddWithValue("email", var.email);
            insertCommand.Parameters.AddWithValue("address", bloodDonors.Address);
            insertCommand.Parameters.AddWithValue("city", bloodDonors.City);
            insertCommand.Parameters.AddWithValue("country", bloodDonors.Country);

            insertCommand.Parameters.AddWithValue("MedicalHistory", bloodDonors.MedicalHistory);
            insertCommand.Parameters.AddWithValue("DonorStatus", bloodDonors.DonorStatus);
            insertCommand.Parameters.AddWithValue("imgUrl", bloodDonors.ImgUrl);

            insertCommand.ExecuteNonQuery();
            sqlConnection.Close();
        }

        public BloodDonors GetDonorById(string id)
        {
            string selectQuery = "SELECT * FROM BloodDonors where Donorid = @uid";

            sqlConnection.Open();
            SqlCommand selectCommand = new SqlCommand(selectQuery, sqlConnection);
            selectCommand.Parameters.AddWithValue("uid", id);

            BloodDonors bloodDonor = new BloodDonors();

            SqlDataReader sqlDataReader = selectCommand.ExecuteReader();
            bool check = false; //Profile is complete or not
            while (sqlDataReader.Read())
            {
                check = true;
                bloodDonor.DonorId = sqlDataReader["donorId"].ToString();
                bloodDonor.Name = sqlDataReader["name"].ToString();
                bloodDonor.DOB = DateTime.Parse(sqlDataReader["dob"].ToString());
                bloodDonor.Gender = sqlDataReader["gender"].ToString();
                bloodDonor.Weight = float.Parse(sqlDataReader["weight"].ToString());
                bloodDonor.WeightUnit = sqlDataReader["weightunit"].ToString();
                bloodDonor.BloodGroup = sqlDataReader["BloodGroup"].ToString();
                bloodDonor.PhoneNumber = sqlDataReader["phoneNumber"].ToString();
                bloodDonor.Address = sqlDataReader["address"].ToString();
                bloodDonor.City = sqlDataReader["city"].ToString();
                bloodDonor.Country = sqlDataReader["country"].ToString();
                bloodDonor.MedicalHistory = sqlDataReader["MedicalHistory"].ToString();
                bloodDonor.DonorStatus = int.Parse(sqlDataReader["DonorStatus"].ToString());
                bloodDonor.ImgUrl = sqlDataReader["imgUrl"].ToString();
            }

            sqlConnection.Close();

            if (check == true)
                return bloodDonor;
            else
                return null;
        }

        public void UpdateStatus(string donorId, int status)
        {

            var query = $"update BloodDonors set Donorstatus={status}  where donorId = {donorId} ";
            using (var connection = new SqlConnection(_context.Database.GetConnectionString()))
            {
                connection.Open();
                var comm = new SqlCommand(query, connection);

                comm.ExecuteNonQuery();
            }
        }

        public  List<BloodDonors> GetDonorsById(List<string>ls)
        {
            sqlConnection.Open();

            List<BloodDonors> bloodDonors = new List<BloodDonors>();

            foreach(var donorid in ls)
            {
                string selectQuery = "SELECT * FROM BloodDonors where Donorid = @uid";
                SqlCommand selectCommand = new SqlCommand(selectQuery, sqlConnection);
                selectCommand.Parameters.AddWithValue("uid", donorid);
                BloodDonors bloodDonor = new BloodDonors();
                using (SqlDataReader sqlDataReader = selectCommand.ExecuteReader())
                {

   
                    while (sqlDataReader.Read())
                    {
                        bloodDonor.DonorId = sqlDataReader["donorId"].ToString();
                        bloodDonor.Name = sqlDataReader["name"].ToString();
                        bloodDonor.PhoneNumber = sqlDataReader["phoneNumber"].ToString();
                        bloodDonor.ImgUrl = sqlDataReader["imgUrl"].ToString();
                    }
                }
                bloodDonors.Add(bloodDonor);
            }

            sqlConnection.Close();
            return bloodDonors;
        }

        public int Eligibility(string userId)
        {
            string selectQuery = "SELECT * FROM BloodDonors where Donorid = @uid";

            sqlConnection.Open();
            SqlCommand selectCommand = new SqlCommand(selectQuery, sqlConnection);
            selectCommand.Parameters.AddWithValue("uid", userId);

            int check = 0;

            SqlDataReader sqlDataReader = selectCommand.ExecuteReader();
            while (sqlDataReader.Read())
            {
                check = int.Parse(sqlDataReader["DonorStatus"].ToString());
            }

            sqlConnection.Close();
            return check;
        }
    }
}
