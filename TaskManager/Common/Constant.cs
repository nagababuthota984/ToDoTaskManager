﻿using System;
using System.IO;

namespace TaskManager.Common
{
    public static class Constant
    {
        public const string Create = "Create";
        public const string Update = "Update";
        public const string ConfirmDeleteMsg = "Are you sure you want to delete the task?";
        public const string ConfirmDeleteWinTitle = "Delete task?";
        public const string DeleteFailedMsg = "Couldn't delete the task. Please try again";
        public const string DeleteFailedWinTitle = "Delete Unsuccessful";
        public const string Database = "Database";
        public const string Sqlite = "Sqlite";
        public const string Sqlserver = "Sqlserver";
        public const string ChangeDbTitleMsg = "Database Changed!";
        public const string DbChangeResultsIn = "This action results in switching to ";
        public const string ConfirmChangeDb = "Are you sure you want to change the database?";
        public const string Today = "Today";
        public const string Tomorrow = "Tomorrow";
        public const string TaskCompletedWinTitle = "Task Completed?";
        public const string TaskCompletedMsg = "Once a task moved to Completed stage,it can't be brought back. are you sure that you want to move it to completed stage?";
        public const string Overdue = "Overdue";
        public const string ErrorOccured = "Error Occured";
        public const string DbSwitchSuccessMsg = "Successfully switched to ";
        public const string TaskDue = "Task due";
        public static Uri IconPath = new(Path.GetFullPath(@"Images/app_icon.ico"));

        public const string UpdateFailed = "Task can't be updated";
    }
}
