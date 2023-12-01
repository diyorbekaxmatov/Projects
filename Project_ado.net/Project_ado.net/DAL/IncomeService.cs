using Project_ado.net.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project_ado.net.DAL
{
    internal class IncomeService
    {
        private const string TABLE_NAME = "Income";
        public static async Task CreateIncome(Income newIncome)
        {
            ThrowIfNull(newIncome);

            string formattedDate = newIncome.Date.ToString("yyyy-MM-dd HH:mm:ss");

            string command = $"INSERT INTO {TABLE_NAME} (Description, Amount, Date, CategoryId) VALUES ('{newIncome.Description}', {newIncome.Amount},'{formattedDate}',{newIncome.Category_Id})";

            await DataAccessLayer.ExecuteNonQueryAsync(command);
        }
        public static async Task UpdateIncome(Income income)
        {
            ThrowIfNull(income);

            string command = $"UPDATE {TABLE_NAME} SET Description = '{income.Description}', Amount ={income.Amount}, Categoryid={income.Category_Id} " +
                $"WHERE Id = {income.Id}";

            await DataAccessLayer.ExecuteNonQueryAsync(command);
        }
        public static async Task DeleteIncome(int id)
        {
            string command = $"DELETE {TABLE_NAME} WHERE Id = {id}";

            await DataAccessLayer.ExecuteNonQueryAsync(command);
        }
        public static async Task<Income> GetIncomeById(int id)
        {
            string query = $"SELECT *" +
                $"FROM {TABLE_NAME}" +
                $" WHERE Id = {id}";

            return await DataAccessLayer.ExecuteQueryAsync(query, ReadToIncome);
        }
        private static Income ReadToIncome(SqlDataReader reader)
        {
            ThrowIfNull(reader);

            if (reader.HasRows)
            {
                Income income = null;
                while (reader.Read())
                {
                    income= new Income();
                    {
                        int id = reader.GetInt32(0);
                        string description = reader.GetString(1);
                        decimal amount = reader.GetDecimal(2);
                        DateTime date = reader.GetDateTime(3);
                        int categoryId = reader.GetInt32(4);
                    };
                }
                return income;
            }

            return null;
        }
        public static async Task<List<Income>> GetAllIncomeAsync()
        {
            string query = $"SELECT * FROM {TABLE_NAME};";

            return await DataAccessLayer.ExecuteQueryAsync(query,ReaderToIncomeList);
        }
        private static List<Income> ReaderToIncomeList(SqlDataReader reader)
        {
            ThrowIfNull(reader);

            List<Income> result = new List<Income>();
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    int id = reader.GetInt32(0);
                    string description = reader.GetString(1);
                    decimal amount = reader.GetDecimal(2);
                    DateTime date = reader.GetDateTime(3);
                    int categoryId = reader.GetInt32(4);
                    Income income = new Income(id, description, amount, date, categoryId);
                    result.Add(income);
                }
            }
            return result;
        }
        private static void ThrowIfNull<T>(T value) where T : class
        {
            if (value is null)
            {
                throw new ArgumentNullException(nameof(value));
            }
        }
    }
}
