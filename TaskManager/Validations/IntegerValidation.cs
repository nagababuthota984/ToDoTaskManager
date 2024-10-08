﻿using System.Globalization;
using System.Windows.Controls;

namespace TaskManager.Validations
{
    public class IntegerValidation : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            if (!int.TryParse(value.ToString(), out int intValue))
            {
                return new(false, "Please enter a valid integer value ");
            }

            return new(true, null);
        }
    }
}
