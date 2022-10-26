﻿using BankApp.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace BankApp.Implementations
{
    public class Validation : IValidation
    {
        string pattName = @"^[a-zA-Z][a-zA-Z]*\s?'?-?[a-zA-Z,]{1,}$";
        string pattMail = @"^[a-z]\w+@\w+\.[a-z]{2,3}";
        string pattPassword = @"^(?=.*[A-Za-z])(?=.*\d)(?=.*[@$!%*#?&])[A-Za-z\d@$!%*#?&]{6,}$";
        Regex rg;
        public bool ValidateEmail(string email)
        {
            if(email == null)
            {
                throw new ArgumentNullException("email cannot be blank");
            }
            rg = new Regex(pattMail);
            return rg.IsMatch(email);
        }

        public bool ValidateName(string name)
        {
            if(name == null)
            {
                throw new ArgumentNullException("name cannot be blank");
            }
            if (string.IsNullOrEmpty(name) || name.ToLower()[0] == name[0]) return false;
            rg = new Regex(pattName);
            return rg.IsMatch(name);
        }

        public bool ValidatePassword(string password)
        {
            if(password == null)
            {
                throw new ArgumentNullException("password cannot be blank");
            }
            rg = new Regex(pattPassword);
            return rg.IsMatch(password);
        }
    }
}
