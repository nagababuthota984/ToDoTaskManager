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
