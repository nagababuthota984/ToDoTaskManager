using System.ComponentModel;

namespace TaskManager.Models
{
    public class Enums
    {
        public enum Status
        {
            [Description("New")]
            New,
            [Description("In Progress")]
            InProgress,
            [Description("Completed")]
            Completed
        }

        public enum Priority
        {
            Low,
            Medium,
            High
        }

        public enum UserRole
        {
            Create,
            Edit
        }

        public enum Category
        {
            [Description("New Feature")]
            NewFeature,
            [Description("Bug Fix")]
            BugFix,
            [Description("Learning Task")]
            LearningTask,
            [Description("Others")]
            Others
        }

        public enum OperationType
        {
            Create,
            Delete,
            Update,
            Display
        }

    }
}