// Solution Structure
// Banking.Library contains the core logic and models
// Banking.Console contains the program to interact with the user

// Banking.Library
namespace Banking.Library
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;

    public class Customer
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string AccountNumber { get; set; }
        public string Pin { get; set; }
        public decimal SavingsBalance { get; set; } = 0;
        public decimal CurrentBalance { get; set; } = 0;

        public Customer(string firstName, string lastName)
        {
            FirstName = firstName;
            LastName = lastName;
            AccountNumber = GenerateAccountNumber();
            Pin = GeneratePin();
        }

        private string GenerateAccountNumber()
        {
            string initials = FirstName.Substring(0, 1).ToLower() + LastName.Substring(0, 1).ToLower();
            int nameLength = (FirstName + LastName).Length;
            int firstInitialPos = char.ToUpper(FirstName[0]) - 'A' + 1;
            int secondInitialPos = char.ToUpper(LastName[0]) - 'A' + 1;
            return $"{initials}-{nameLength}-{firstInitialPos}-{secondInitialPos}";
        }

        private string GeneratePin()
        {
            int firstInitialPos = char.ToUpper(FirstName[0]) - 'A' + 1;
            int secondInitialPos = char.ToUpper(LastName[0]) - 'A' + 1;
            return $"{firstInitialPos}{secondInitialPos}";
        }

        public void SaveCustomerData()
        {
            string customerData = $"{AccountNumber}\t{FirstName}\t{LastName}\t{Pin}\t{SavingsBalance}\t{CurrentBalance}";
            File.AppendAllLines("customers.txt", new[] { customerData });
        }

        public void RecordTransaction(string accountType, string action, decimal amount, decimal finalBalance)
        {
            string fileName = accountType == "savings" ? $"{AccountNumber}-savings.txt" : $"{AccountNumber}-current.txt";
            string transactionRecord = $"{DateTime.Now:dd-MM-yyyy}\t{action}\t{amount}\t{finalBalance}";
            File.AppendAllLines(fileName, new[] { transactionRecord });
        }
    }
}