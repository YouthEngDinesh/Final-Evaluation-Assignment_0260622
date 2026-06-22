using System;
namespace Excercise1_AdvanceTMS.Models
{
    public class TaskModel
    {
        public int TASK_ID { get; set; }
        public string TASK_NAME { get; set; }
        public string ASSIGNEE { get; set; }
        public DateTime DUE_DATE { get; set; }
        public string STATUS { get; set; }
        public DateTime CREATE_DATETIME { get; set; }
        public DateTime UPDATE_DATETIME { get; set; }

    }
}
