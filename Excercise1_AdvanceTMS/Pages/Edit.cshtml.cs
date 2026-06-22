using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;
using Dapper;
using Excercise1_AdvanceTMS.Models;
using System;
using System.Threading.Tasks;


namespace Excercise1_AdvanceTMS.Pages
{
    public class EditModel : PageModel
    {
        private readonly string _connectionString = "Server=(localdb)\\MSSQLLocalDB;Database=TMS;Trusted_Connection=True;TrustServerCertificate=True;";

        [BindProperty]
        public TaskModel EditableTask { get; set; }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                // Fetch targeted record content context mapping
                //This prevents SQL Injection vulnerabilities
                var sql = "SELECT * FROM TASKS WHERE TASK_ID = @Id";
                EditableTask = await connection.QueryFirstOrDefaultAsync<TaskModel>(sql, new { Id = id });

                if (EditableTask == null)
                {
                    return RedirectToPage("./Index");
                }
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            using (var connection = new SqlConnection(_connectionString))
            {
                // MANDATORY REQUIREMENT: Secure Dapper UPDATE query execution mapping parameters explicitly
                var sql = @"UPDATE TASKS 
                            SET TASK_NAME = @TASK_NAME,
                                ASSIGNEE = @ASSIGNEE, 
                                DUE_DATE = @DUE_DATE,
                                STATUS = @STATUS, 
                                UPDATE_DATETIME = GETDATE() 
                            WHERE TASK_ID = @TASK_ID";

                //connection.ExecuteAsync is used because we are making a change to the database rows rather than fetching data back.
                await connection.ExecuteAsync(sql, EditableTask);
            }

            // MANDATORY REQUIREMENT: Automatically redirect to the list page to confirm results
            return RedirectToPage("./Index");
        }
    }
}



    

