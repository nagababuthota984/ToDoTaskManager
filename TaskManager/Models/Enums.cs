using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
    }
}
