using Banking.Library;

public class Bank
{
    private List<Customer> customers = new List<Customer>();

    public void AddCustomer()
    {
        Console.WriteLine("Enter First Name:");
        string firstName = Console.ReadLine();

        Console.WriteLine("Enter Last Name:");
        string lastName = Console.ReadLine();

        Console.WriteLine("Enter Email:");
        string email = Console.ReadLine();

        Customer newCustomer = new Customer(firstName, lastName);
        customers.Add(newCustomer);
        newCustomer.SaveCustomerData();
        Console.WriteLine($"Customer created with Account Number: {newCustomer.AccountNumber} and Pin: {newCustomer.Pin}");
    }

    public void DeleteCustomer(string accountNumber)
    {
        var customer = customers.FirstOrDefault(c => c.AccountNumber == accountNumber);
        if (customer == null)
        {
            Console.WriteLine("Customer not found.");
            return;
        }
        if (customer.SavingsBalance > 0 || customer.CurrentBalance > 0)
        {
            Console.WriteLine("Cannot delete customer with non-zero balances.");
            return;
        }
        customers.Remove(customer);
        Console.WriteLine("Customer deleted.");
    }

    public void ShowCustomers()
    {
        foreach (var customer in customers)
        {
            Console.WriteLine($"Name: {customer.FirstName} {customer.LastName}, Account Number: {customer.AccountNumber}, Savings: {customer.SavingsBalance}, Current: {customer.CurrentBalance}");
        }
    }

    public Customer AuthenticateCustomer(string accountNumber, string pin)
    {
        return customers.FirstOrDefault(c => c.AccountNumber == accountNumber && c.Pin == pin);
    }

    public void AddTransaction(Customer customer, string accountType, decimal amount)
    {
        if (accountType.ToLower() == "savings")
        {
            customer.SavingsBalance += amount;
            customer.RecordTransaction("savings", "Lodgement", amount, customer.SavingsBalance);
        }
        else if (accountType.ToLower() == "current")
        {
            customer.CurrentBalance += amount;
            customer.RecordTransaction("current", "Lodgement", amount, customer.CurrentBalance);
        }
        Console.WriteLine("Transaction completed.");
    }

    public void WithdrawTransaction(Customer customer, string accountType, decimal amount)
    {
        if (accountType.ToLower() == "savings" && customer.SavingsBalance >= amount)
        {
            customer.SavingsBalance -= amount;
            customer.RecordTransaction("savings", "Withdrawal", amount, customer.SavingsBalance);
        }
        else if (accountType.ToLower() == "current" && customer.CurrentBalance >= amount)
        {
            customer.CurrentBalance -= amount;
            customer.RecordTransaction("current", "Withdrawal", amount, customer.CurrentBalance);
        }
        else
        {
            Console.WriteLine("Insufficient balance.");
            return;
        }
        Console.WriteLine("Transaction completed.");
    }

    public void Run()
    {
        Console.WriteLine("Welcome to the Banking Application");

        while (true)
        {
            Console.WriteLine("Are you a Bank Employee or Customer? (Enter 1 for Employee, 2 for Customer, 0 to Exit)");
            string choice = Console.ReadLine();

            if (choice == "0") break;
            if (choice == "1")
            {
                Console.WriteLine("Enter Employee PIN:");
                string pin = Console.ReadLine();

                if (pin != "A1234")
                {
                    Console.WriteLine("Invalid PIN.");
                    continue;
                }

                while (true)
                {
                    Console.WriteLine("1. Create Customer\n2. Delete Customer\n3. Show Customers\n4. Exit");
                    string empChoice = Console.ReadLine();

                    if (empChoice == "4") break;
                    switch (empChoice)
                    {
                        case "1":
                            AddCustomer();
                            break;
                        case "2":
                            Console.WriteLine("Enter Account Number to delete:");
                            string accountNumber = Console.ReadLine();
                            DeleteCustomer(accountNumber);
                            break;
                        case "3":
                            ShowCustomers();
                            break;
                        default:
                            Console.WriteLine("Invalid choice.");
                            break;
                    }
                }
            }
            else if (choice == "2")
            {
                Console.WriteLine("Enter Account Number and PIN:");
                string accountNumber = Console.ReadLine();
                string pin = Console.ReadLine();

                var customer = AuthenticateCustomer(accountNumber, pin);
                if (customer == null)
                {
                    Console.WriteLine("Invalid login.");
                    continue;
                }

                while (true)
                {
                    Console.WriteLine("1. View Transaction History\n2. Add Money\n3. Withdraw Money\n4. Exit");
                    string custChoice = Console.ReadLine();

                    if (custChoice == "4") break;
                    switch (custChoice)
                    {
                        case "1":
                            Console.WriteLine("Enter account type (savings/current):");
                            string accountType = Console.ReadLine();
                            string fileName = accountType == "savings" ? $"{customer.AccountNumber}-savings.txt" : $"{customer.AccountNumber}-current.txt";
                            if (File.Exists(fileName))
                            {
                                string[] history = File.ReadAllLines(fileName);
                                foreach (string line in history)
                                {
                                    Console.WriteLine(line);
                                }
                            }
                            else
                            {
                                Console.WriteLine("No transactions found.");
                            }
                            break;
                        case "2":
                            Console.WriteLine("Enter account type (savings/current) and amount:");
                            string[] addDetails = Console.ReadLine().Split(',');
                            AddTransaction(customer, addDetails[0], decimal.Parse(addDetails[1]));
                            break;
                        case "3":
                            Console.WriteLine("Enter account type (savings/current) and amount:");
                            string[] withdrawDetails = Console.ReadLine().Split(',');
                            WithdrawTransaction(customer, withdrawDetails[0], decimal.Parse(withdrawDetails[1]));
                            break;
                        default:
                            Console.WriteLine("Invalid choice.");
                            break;
                    }
                }
            }
            else
            {
                Console.WriteLine("Invalid choice.");
            }
        }
    }
}
