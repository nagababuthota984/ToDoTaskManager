namespace TaskManager.Models
{
    public class Enums
    {
        public enum Status
        {
            New,
            InProgress,
            Completed
        }
        public enum Priority
        {
            Low,
            Medium,
            High
        }
        public enum Category
        {
            NewFeature,
            BugFix,
            LearningTask,
            Others
        }
    }
}