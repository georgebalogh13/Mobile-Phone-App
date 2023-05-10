using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;
using Phone_project_API.DTOs;
using Phone_project_API.Models;

namespace Phone_project_API.Services
{
    public class PhonesService
    {
        private readonly IConfiguration _configuration;

        public PhonesService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<List<Phone>> GetPhones()
        {
            string query = @"
                    select * from phones";

            List<Phone> phones = new List<Phone>();
            string sqlDataSource = _configuration.GetConnectionString("DefaultConnection");

            using (MySqlConnection mySqlConnection = new MySqlConnection(sqlDataSource))
            {
                await mySqlConnection.OpenAsync();
                using (MySqlCommand sqlCommand = new MySqlCommand(query, mySqlConnection))
                {
                    using (MySqlDataReader myReader = (MySqlDataReader)await sqlCommand.ExecuteReaderAsync())
                    {
                        while (await myReader.ReadAsync())
                        {
                            phones.Add(new Phone
                            {
                                Id = myReader.GetInt32("Id"),
                                Model = myReader.GetString("Model"),
                                Brand = myReader.GetString("Brand"),
                                Price = myReader.GetInt32("Price")
                            });
                        }
                    }
                }
            }

            return phones;
        }

        public async Task<Phone> GetPhoneById(int id)
        {
            string query = "SELECT * FROM phones WHERE Id = @Id;";
            Phone phone = null;

            using (MySqlConnection connection = new MySqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                await connection.OpenAsync();
                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Id", id);
                    using (MySqlDataReader reader = (MySqlDataReader)await command.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            phone = new Phone
                            {
                                Id = reader.GetInt32("Id"),
                                Model = reader.GetString("Model"),
                                Brand = reader.GetString("Brand"),
                                Price = reader.GetInt32("Price")
                            };
                        }
                    }
                }
            }

            return phone;
        }

        public async Task<Phone> AddPhone(PhoneDto phoneDto)
        {
            string query = "INSERT INTO phones (Model, Brand, Price) VALUES (@Model, @Brand, @Price); SELECT LAST_INSERT_ID();";

            using (MySqlConnection connection = new MySqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                await connection.OpenAsync();
                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Model", phoneDto.Model);
                    command.Parameters.AddWithValue("@Brand", phoneDto.Brand);
                    command.Parameters.AddWithValue("@Price", phoneDto.Price);

                    int newId = Convert.ToInt32(await command.ExecuteScalarAsync());

                    // create a new Phone object from the PhoneDto object, with the newly generated ID
                    var phone = new Phone
                    {
                        Id = newId,
                        Model = phoneDto.Model,
                        Brand = phoneDto.Brand,
                        Price = phoneDto.Price
                    };

                    return phone;
                }
            }
        }

        public async Task<bool> UpdatePhone(int id, PhoneDto phone)
        {

            string query = "UPDATE phones SET Model = @Model, Brand = @Brand, Price = @Price WHERE Id = @Id;";

            using (MySqlConnection connection = new MySqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                await connection.OpenAsync();
                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Id", id);
                    command.Parameters.AddWithValue("@Model", phone.Model);
                    command.Parameters.AddWithValue("@Brand", phone.Brand);
                    command.Parameters.AddWithValue("@Price", phone.Price);

                    int affectedRows = await command.ExecuteNonQueryAsync();
                    if (affectedRows == 0)
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        public async Task<bool> DeletePhone(int id)
        {
            string query = "DELETE FROM phones WHERE Id = @Id;";
            int affectedRows;

            using (MySqlConnection connection = new MySqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                await connection.OpenAsync();
                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Id", id);

                    affectedRows = await command.ExecuteNonQueryAsync();
                }
            }

            return affectedRows > 0;
        }
    }
}
