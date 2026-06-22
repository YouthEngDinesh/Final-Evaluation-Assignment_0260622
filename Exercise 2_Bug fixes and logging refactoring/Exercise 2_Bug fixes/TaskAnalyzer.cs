using System;
using NLog;                        //Added this 
using System.Collections.Generic;
using System.Text;

namespace Exercise_2_Bug_fixes
{
    public class TaskAnalyzer
    {
        private readonly Logger _logger;
        public TaskAnalyzer(Logger logger)
        {
            // ロガー
            //Legacy / Defective Code
           //logger = logger;   //logger = logger; simply signs the parameter variable back to itself. It never assigns the value to the private member _logger, resulting in a NullReferenceException crash the first time any logging function is invoked.


           //Target parameter assigned correctly to class property
           _logger = logger;    //  Refactored / Correct Code	

        }
        /// <summary>
        /// 期限切れの未完了タスクをチェックし、警告ログを出力する
        /// </summary>
        public void CheckOverdueTasks(List<TaskModel> tasks)
        {
            _logger.Info("期限切れタスクのチェックを開始します。");
            foreach (var task in tasks)
            {
                // ステータスが完了以外を対象とする
                if (task.STATUS != "完了")
                {
                    // 期限日の判定
                    //if (task.DUE_DATE > DateTime.Today)     //Legacy / Defective Code
                    //{
                    //    // 警告ログ出力
                    //    _logger.Info(\$"【警告】期限切れタスクを発見: {task.TASK_NAME} (担当: {task.ASSIGNEE}, 期限: {task.DUE_DATE:yyyy/MM/dd})");
                    //}

                    if (task.DUE_DATE < DateTime.Today)     //Refactored / Correct Code
                    {
                        // 警告ログ出力
                        _logger.Warn($"【警告】期限切れタスクを発見: {task.TASK_NAME} (担当: {task.ASSIGNEE}, 期限: {task.DUE_DATE:yyyy/MM/dd})");
                    }
                }
            }
        }
        /// <summary>
        /// 担当者ごとの未完了タスク数を集計してログ出力する
        /// </summary>
        public void LogTaskCountByAssignee(List<TaskModel> tasks)
        {
            _logger.Info("担当者ごとの未完了タスク数集計を出力します。");
            // 担当者ごとの未完了リスト
            //var assigneeList = new List<string>();
            //foreach (var t in tasks)                       // Legacy / Defective Code (O(N²) Nested Loops)
            //{
            //    if (!assigneeList.Contains(t.ASSIGNEE))

            //    {
            //        assigneeList.Add(t.ASSIGNEE);
            //    }
            //}
            //// 集計
            //foreach (var assignee in assigneeList)
            //{
            //    int count = 0;
            //    foreach (var t in tasks)
            //    {
            //        if (t.ASSIGNEE == assignee && t.STATUS != "完了")
            //        {
            //            count++;
            //        }
            //    }
            //    _logger.Info(\$"担当者: {assignee} / 未完了タスク数: {count}件");
            //}



            //  Refactored / Correct Code (O(N) Streaming Pipeline)			
            var aggregationResults = tasks
                .Where(t => t.STATUS != "完了")
                .GroupBy(t => t.ASSIGNEE)
                .Select(group => new
                {
                    AssigneeName = group.Key,
                    IncompleteCount = group.Count()
                });


            foreach (var summaryItem in aggregationResults)
            {
                _logger.Info($"担当者: {summaryItem.AssigneeName} / 未完了タスク数: {summaryItem.IncompleteCount}件");
            }



        }
    }
}






