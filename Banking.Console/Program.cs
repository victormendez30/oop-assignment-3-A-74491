using Banking.Library;

namespace Banking.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            Bank bank = new Bank();
            bank.Run();
        }
    }
}