using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static TaskManager.Models.Enums;

namespace TaskManager.Models
{
    public class TaskEventMessage
    {
      
        public Task Task { get; set; }

        public OperationType OperationType { get; set; }

        public object Sender { get; set; }
    }
}
