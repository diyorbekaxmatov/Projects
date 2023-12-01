using Project_ado.net.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project_ado.net.DAL
{
    internal class ExpenseService
    {
        private const string TABLE_NAME = "Expense";
        public static async Task CreateExpense(Expense newExpense)
        {
            ThrowIfNull(newExpense);

            string formattedDate = newExpense.Date.ToString("yyyy-MM-dd HH:mm:ss");

            string command = $"INSERT INTO {TABLE_NAME} (Description, Amount, Date, CategoryId) VALUES ('{newExpense.Description}', {newExpense.Amount},'{formattedDate}',{newExpense.Category_Id})";

            await DataAccessLayer.ExecuteNonQueryAsync(command);
        }
        public static async Task UpdateExpense(Expense expense)
        {
            ThrowIfNull(expense);

            string command = $"UPDATE {TABLE_NAME} SET Description = '{expense.Description}', Amount ={expense.Amount}, Categoryid={expense.Category_Id} " +
                $"WHERE Id = {expense.Id}";

            await DataAccessLayer.ExecuteNonQueryAsync(command);
        }
        public static async Task DeleteExpense(int id)
        {
            string command = $"DELETE {TABLE_NAME} WHERE Id = {id}";

            await DataAccessLayer.ExecuteNonQueryAsync(command);
        }
        public static async Task<Expense> GetExpenseById(int id)
        {
            string query = $"SELECT *" +
                $"FROM {TABLE_NAME}" +
                $" WHERE Id = {id}";

            return await DataAccessLayer.ExecuteQueryAsync(query,  ReadToExpense);
        }
        private static Expense ReadToExpense(SqlDataReader reader)
        {
            ThrowIfNull(reader);

            if (reader.HasRows)
            {
                Expense expense= null;
                while (reader.Read())
                {
                    expense = new Expense();
                    {
                        int id = reader.GetInt32(0);
                        string description = reader.GetString(1);
                        decimal amount = reader.GetDecimal(2);
                        DateTime date = reader.GetDateTime(3);
                        int categoryId = reader.GetInt32(4);
                    };
                }
                return expense;
            }

            return null;
        }
        public static async Task<List<Expense>> GetAllExpenseAsync()
        {
            string query = $"SELECT * FROM {TABLE_NAME};";

            return await DataAccessLayer.ExecuteQueryAsync(query, ReaderToExpenseList);
        }
        private static List<Expense> ReaderToExpenseList(SqlDataReader reader)
        {
            ThrowIfNull(reader);

            List<Expense> result = new List<Expense>();
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    int id = reader.GetInt32(0);
                    string description = reader.GetString(1);
                    decimal amount = reader.GetDecimal(2);
                    DateTime date = reader.GetDateTime(3);
                    int categoryId = reader.GetInt32(4);
                    Expense expense = new Expense(id, description, amount, date, categoryId);
                    result.Add(expense);
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
