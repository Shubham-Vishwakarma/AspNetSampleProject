using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BuildRestApiNetCore.Database;
using BuildRestApiNetCore.Models;
using MySqlConnector;

namespace BuildRestApiNetCore.Services
{
    public class ProductService
    {

        private const string insertQuery = "insert into product ('productNumber', 'name', 'price', 'department') values (@productNumber, @name, @price, @department);";
        private const string selectQuery = "select productNumber, name, price, department from product;";

        internal AppDatabase Database { get; set; }

        public ProductService(AppDatabase database)
        {
            Database = database;
        }

        public async Task InsertAsync(Product product)
        {
            using( MySqlConnection connection = Database.Connection)
            {
                MySqlCommand command = connection.CreateCommand();
                command.CommandText = insertQuery;
                command.Parameters.AddWithValue("@productNumber", product.ProductNumber);
            
                await connection.OpenAsync();
                await command.ExecuteNonQueryAsync();
            }
        }

        public async Task<IEnumerable<Product>> SelectAsync()
        {
            LinkedList<Product> products = new LinkedList<Product>();

            using(MySqlConnection connection = Database.Connection)
            {
                MySqlCommand command = connection.CreateCommand();
                command.CommandText = selectQuery;

                await connection.OpenAsync();

                MySqlDataReader reader = await command.ExecuteReaderAsync();

                while(await reader.ReadAsync())
                {
                    Product product = new Product {
                        ProductNumber = reader.GetString("productNumber"),
                        Name = reader.GetString("name"),
                        Price = reader.GetDouble("price"),
                        Department = reader.GetString("department")
                    };

                    products.AddLast(product);
                }

                await reader.CloseAsync();
            }

            return products;
        }
    }
}