using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using System;
using System.IO;
using System.Windows;

namespace TaskManager.Common
{
    public static class Constant
    {
        public const string create = "Create";
        public const string update = "Update";
        public const string confirmDeleteMsg = "Are you sure you want to delete the task?";
        public const string confirmDeleteWinTitle = "Delete task?";
        public const string deleteFailedMsg = "Couldn't delete the task. Please try again";
        public const string deleteFailedWinTitle = "Delete Unsuccessful";
        public const string database = "Database";
        public const string sqlite = "Sqlite";
        public const string sqlserver = "Sqlserver";
        public const string changeDbTitleMsg = "Database Changed!";
        public const string dbChangeResultsIn = "This action results in switching to ";
        public const string confirmChangeDb = "Are you sure you want to change the database?";
        public const string today = "Today";
        public const string tomorrow = "Tomorrow";
        public const string taskCompletedWinTitle = "Task Completed?";
        public const string taskCompletedMsg = "Once a task moved to Completed stage,it can't be brought back. are you sure that you want to move it to completed stage?";
        public const string overdue = "Overdue";
        public const string errorOccured = "Error Occured";
        public const string dbSwitchSuccessMsg = "Successfully switched to ";
        public const string taskDue = "Task due";
        public static Uri iconPath = new(Path.GetFullPath(@"Images/app_icon.ico"));
        public const string updateFailed = "Task can't be updated";
        public const string defaultBrowserRegistryKeyName = @"HKEY_CURRENT_USER\Software\Microsoft\Windows\Shell\Associations\UrlAssociations\https\UserChoice";
        public const string gitHubRepoUrl = "https://github.com/nagababuthota984/ToDoTaskManager";
        public const string progId = "ProgId";
        public const string iEBrowserRegistryKeyName = @"HKEY_CLASSES_ROOT\https\shell\open\command";
        public static string projectDirectory = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.Parent.FullName;
        
       
    }
}
