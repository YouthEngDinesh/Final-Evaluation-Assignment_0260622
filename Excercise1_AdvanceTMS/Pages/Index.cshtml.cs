using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;
using Dapper;
using Excercise1_AdvanceTMS.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace Excercise1_AdvanceTMS.Pages
{
    public class IndexModel : PageModel
    {
        // Database connection 
        private readonly string _connectionString = "Server=(localdb)\\MSSQLLocalDB;Database=TMS;Trusted_Connection=True;TrustServerCertificate=True;";

        public List<TaskModel> Tasks { get; set; } = new List<TaskModel>();

        // /?SearchAssignee=王&SearchStatus=進行中
        [BindProperty(SupportsGet = true)]
        public string SearchAssignee { get; set; }

        [BindProperty(SupportsGet = true)]
        public string SearchStatus { get; set; }
       

            public async Task OnGetAsync()
            {

            //Using the using statement guarantees that the database connection will be closed and discarded safely even if an error occurs, 
            //preventing database resource leaks. QueryAsync runs asynchronously, keeping the web application fast and responsive under load.
            // using ステートメントを使用することで、エラーが発生した場合でもデータベース接続が安全に閉じられ、破棄されることが保証されます。
            //これにより、データベースリソースのリークを防ぎます。QueryAsync は非同期で実行されるため、負荷がかかった状態でも Web アプリケーションは高速かつ応答性を維持します。
            using (var connection = new SqlConnection(_connectionString))
                {
                // 1. Dapper raw data fetch execution
                //Opens a secure pipe to SQL Server and pulls down every single task record using Dapper's QueryAsync<T> extension method.
                // 1. Dapperによる生データ取得の実行
                // DapperのQueryAsync<T>拡張メソッドを使用して、SQL Serverへのセキュアなパイプを開き、すべてのタスクレコードを取得します。
                var sql = "SELECT * FROM TASKS ORDER BY DUE_DATE ASC";
                    var allTasks = await connection.QueryAsync<TaskModel>(sql);

                // 2. MANDATORY REQUIREMENT: Pure LINQ pipeline execution for filtering (No loop blocks)
                // 2. 必須要件：フィルタリングには純粋なLINQパイプライン実行（ループブロックは使用しない）
                var query = allTasks.AsEnumerable();

                   
                    if (!string.IsNullOrWhiteSpace(SearchAssignee))
                    {
                      // Reference https://qiita.com/miswil/items/9e139202337ce881ca5f
                      //Reference https://learn.microsoft.com/ja-jp/dotnet/api/system.stringcomparer.ordinalignorecase?view=net-10.0
                    query = query.Where(t => t.ASSIGNEE.Contains(SearchAssignee, System.StringComparison.OrdinalIgnoreCase));
                    }

                    if (!string.IsNullOrWhiteSpace(SearchStatus))
                    {
                        query = query.Where(t => t.STATUS == SearchStatus);
                    }

                    Tasks = query.ToList();
                }
            }
        
    


    }
}
