using Project_ado.net.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Project_ado.net.Modules
{
    internal class MainApplication
    {
        public static async Task Start()
        {
            Console.WriteLine("1. Manage Incomes     2. Manage Expenses     3. Manage Categories");
            int input = ConsoleHelper.GetOptionInput();
            switch (input)
            {
                case 1:
                    await IncomeModule.ShowOptionsIncomeAsync();
                    break;
                case 2:
                    await ExpenseModul.ShowOptionsExpenseAsync();
                    break;
                case 3:
                    await  CategoryModule.ShowOptionsCategoryAsync();
                    break;
                default:
                    return;
            }
        }
    }
}
