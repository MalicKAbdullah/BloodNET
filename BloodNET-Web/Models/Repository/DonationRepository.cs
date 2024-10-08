﻿using BloodNET_Web.Data;
using BloodNET_Web.Models.Interfaces;
using Dapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace BloodNET_Web.Models.Repository
{
    public class DonationRepository:IDonation
    {
        private readonly ApplicationDbContext _context;
        private SqlConnection sqlConnection;

        public DonationRepository(ApplicationDbContext context)
        {
            _context = context;
            sqlConnection = new SqlConnection(_context.Database.GetConnectionString());

        }

        public List<(string,DateTime)> GetDonors(int reqId)
        {
            var Donorlist = new List<(string,DateTime)>();
            string selectQuery = "SELECT * FROM Donation where Requestid = @rid";

            sqlConnection.Open();
            SqlCommand selectCommand = new SqlCommand(selectQuery, sqlConnection);
            selectCommand.Parameters.AddWithValue("rid", reqId);

            SqlDataReader sqlDataReader = selectCommand.ExecuteReader();
            while (sqlDataReader.Read())
            {
                (string, DateTime) obj;
                obj.Item1 = sqlDataReader["donorId"].ToString();
                obj.Item2 = DateTime.Parse(sqlDataReader["Created"].ToString());
                Donorlist.Add(obj);
            }

            sqlConnection.Close();
            return Donorlist;
        }

        public List<(string donorId, int reqId)> GetDonations(string donorId)
        {
            var DonationList = new List<(string, int)>();
            string selectQuery = "SELECT * FROM Donation where donorid = @donorId";
            using (SqlConnection sqlConnection = new SqlConnection(_context.Database.GetConnectionString()))
            {
                List<Donation> donations = sqlConnection.Query<Donation>(selectQuery, new { donorId}).ToList();

                foreach (var donation in donations)
                {
                    (string,int) obj = (donation.DonorId, donation.RequestId);
                    DonationList.Add(obj);
                }
            }
            return DonationList;
        }

        public  bool getDonorStatus(string donorId)
        {
            bool status = false;

            string selectQuery = "SELECT * FROM Donation where donorid = @donorId";
            using (SqlConnection sqlConnection = new SqlConnection(_context.Database.GetConnectionString()))
            {
                List<Donation> donations = sqlConnection.Query<Donation>(selectQuery, new { donorId }).ToList();

                foreach (var donation in donations)
                {
                    int check = donation.Status;

                    if (check == 0)
                        status = false;
                    else
                    {
                        status = true;
                        break;
                    }
                }
            }

            return status;
        }

        public void UpdateStatus(int donationId,int status)
        {

            var query = $"update BloodRequests set status={status}  where Id = {donationId} ";
            using (var connection = new SqlConnection(_context.Database.GetConnectionString()))
            {
                connection.Open();
                var comm = new SqlCommand(query, connection);

                comm.ExecuteNonQuery();
            }
        }

        public int getDonationId(string donorId,int reqId)
        {
            int id = 0;
            string selectQuery = $"SELECT * FROM Donation where donorid = {donorId} and RequestId = {reqId}";
            using (SqlConnection sqlConnection = new SqlConnection(_context.Database.GetConnectionString()))
            {
                SqlCommand sqlCommand = new SqlCommand(selectQuery, sqlConnection);

                sqlCommand.ExecuteReader();

                List<Donation> donations = new List<Donation>();

                foreach (var donation in donations)
                {
                    id = donation.Id;
                    break;
                }
            }

            return id;
        }

    }
}
