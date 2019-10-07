using Dapper;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using MyAddressBookPlus.Models;
using Microsoft.Azure.Services.AppAuthentication;

namespace MyAddressBookPlus.Data
{
    public class ContactRepository : IContactRepository
    {
        private IDbConnection db;

        public ContactRepository()
        {
            var connectionstring = ConfigurationManager.ConnectionStrings["SqlDataConnection"].ConnectionString;
            var accesstoken = (new AzureServiceTokenProvider()).GetAccessTokenAsync("https://database.windows.net/").Result;
            db = new SqlConnection()
            {
                AccessToken = accesstoken,
                ConnectionString = connectionstring
            };
        }

        public int AddContact(Contact contact)
        {
            var sql = "INSERT INTO dbo.[Contact] ([Name] ,[Email] ,[Phone] ,[Address] ,[PictureName]) VALUES" +
                "(@Name, @Email, @Phone, @Address, @Picturename); " +
                "SELECT CAST(SCOPE_IDENTITY() AS INT)";

            //Replacing the dynamic parameters so that ADO.NET can replace the encrypted value
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@Name", contact.Name, DbType.String, ParameterDirection.Input);
            parameters.Add("@Email", contact.Email, DbType.String, ParameterDirection.Input);
            parameters.Add("@Phone", contact.Phone, DbType.String, ParameterDirection.Input);
            parameters.Add("@Address", contact.Address, DbType.String, ParameterDirection.Input);
            parameters.Add("@Picturename", contact.PictureName, DbType.String, ParameterDirection.Input);
            parameters.Add("@Ssn", contact.Ssn, DbType.String, ParameterDirection.Input);

            var id = this.db.Query<int>(sql, parameters).Single();
            contact.Id = id;
            return id;
        }

        public bool DeleteContact(int id)
        {
            var sql = "DELETE FROM dbo.[Contact] WHERE id = @id";
            var result = db.Execute(sql, new { Id = id });

            return true;
        }

        public Contact GetContact(int id)
        {
            var sql = "SELECT * FROM dbo.[Contact] WHERE id = @id";
            var result = db.Query<Contact>(sql, new { Id = id })
                .SingleOrDefault();

            return result;
        }

        public List<Contact> GetContacts()
        {
            var sql = "SELECT * FROM dbo.[Contact] order by id";
            var result = db.Query<Contact>(sql).ToList();

            return result;
        }
    }
}