using System.Collections.Generic;
using System.IO;
using System;



class BankTransaction
{
    public readonly DateTime Date;
    public readonly decimal Amount;

    public BankTransaction(decimal amount)
    {
        this.Date = DateTime.Now;
        this.Amount = amount;
    }
}

class BankAccount : IDisposable
{
    private static int accountNumberCounter = 1000;

    private int accountNumber;
    private string accountType;
    private decimal balance;
    private readonly Queue<BankTransaction> transactions;
    private bool disposed = false;

    public BankAccount()
    {
        GenerateAccountNumber();
        transactions = new Queue<BankTransaction>();
    }

    public BankAccount(string accountType) : this()
    {
        this.accountType = accountType;
    }

    public BankAccount(decimal balance) : this()
    {
        this.balance = balance;
    }

    public BankAccount(string accountType, decimal balance) : this()
    {
        this.accountType = accountType;
        this.balance = balance;
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!disposed)
        {
            if (disposing)
            {
                SaveTransactionDataToFile("transactions.txt");
            }
            disposed = true;
        }
    }

    private void SaveTransactionDataToFile(string fileName)
    {
        using (StreamWriter writer = new StreamWriter(fileName))
        {
            foreach (var transaction in transactions)
            {
                writer.WriteLine($"{transaction.Date},{transaction.Amount}");
            }
        }
    }

    private void GenerateAccountNumber()
    {
        accountNumber = accountNumberCounter++;
    }

    public void DisplayAccountDetails()
    {
        Console.WriteLine("Тип аккаунта: " + accountType);
        Console.WriteLine("Баланс: " + balance);
    }

    public void Deposit(decimal amount)
    {
        if (amount > 0)
        {
            balance += amount;
            var transaction = new BankTransaction(amount);
            transactions.Enqueue(transaction);
            Console.WriteLine("Успешно зачислено " + amount + " на счет");
            Console.WriteLine("Время операции: " + transaction.Date);
        }
        else
        {
            Console.WriteLine("Некорректная сумма");
        }
    }

    public void Withdraw(decimal amount)
    {
        if (amount > 0)
        {
            if (balance >= amount)
            {
                balance -= amount;
                var transaction = new BankTransaction(-amount);
                transactions.Enqueue(transaction);
                Console.WriteLine("Успешно снято " + amount + " со счета");
                Console.WriteLine("Время операции: " + transaction.Date);
            }
            else
            {
                Console.WriteLine("Недостаточно средств на счете.");
            }
        }
        else
        {
            Console.WriteLine("Некорректная сумма");
        }
    }
}

class Program
{
    static void Main(string[] args)
    {
        BankAccount account1 = new BankAccount("Сберегательный", 1000000);
        BankAccount account2 = new BankAccount("Основной", 57500);

        Console.WriteLine("Информация о счете 1:");
        account1.DisplayAccountDetails();

        Console.WriteLine();

        Console.WriteLine("Информация о счете 2:");
        account2.DisplayAccountDetails();

        Console.WriteLine();

        Console.WriteLine("Выберите операцию:");
        Console.WriteLine("1. Положить на сберегательный счет");
        Console.WriteLine("2. Снять со сберегательного счета");
        Console.WriteLine("3. Положить на основной счет");
        Console.WriteLine("4. Снять с основного счета");
        int choice = int.Parse(Console.ReadLine());
        Console.WriteLine("Введите сумму:");
        decimal amount = decimal.Parse(Console.ReadLine());

        switch (choice)
        {
            case 1:
                account1.Deposit(amount);
                Console.WriteLine("Обновленная информация о сберегательном счете:");
                account1.DisplayAccountDetails();
                break;
            case 2:
                account1.Withdraw(amount);
                Console.WriteLine("Обновленная информация о сберегательном счете:");
                account1.DisplayAccountDetails();
                break;
            case 3:
                account2.Deposit(amount);
                Console.WriteLine("Обновленная информация об основном счете:");
                account2.DisplayAccountDetails();
                break;
            case 4:
                account2.Withdraw(amount);
                Console.WriteLine("Обновленная информация о счете:");
                account2.DisplayAccountDetails();
                break;
            default:
                Console.WriteLine("Некорректный выбор операции.");
                break;
        }

        Console.ReadKey();

        Console.WriteLine("Задание 2");
        List<Song> songs = new List<Song>();

        Song song1 = new Song("Она не твоя", "Григорий Лепс", null);
        Song song2 = new Song("Она не твоя", "Григорий Лепс", song1);
        Song song3 = new Song("Просто друг", "Чвай вдвоем", song2);
        Song song4 = new Song("Я буду", "Лоя", song3);

        songs.Add(song1);
        songs.Add(song2);
        songs.Add(song3);
        songs.Add(song4);

        foreach (Song song in songs)
        {
            Console.WriteLine(song.Title());
        }

        if (song1.Equals(song2))
        {
            Console.WriteLine("Песни 1 и 2 одинаковы");
        }
        else
        {
            Console.WriteLine("Песни 1 и 2 разные");
        }


        Song mySong = new Song();


        Console.WriteLine(mySong.Title());
        Console.ReadKey();
    }
    public class Song
    {
        public string Name { get; set; }
        public string Author { get; set; }
        public Song Prev { get; set; }

        // Первый конструктор
        public Song(string name, string author, Song prev = null)
        {
            Name = name;
            Author = author;
            Prev = prev;
        }

        // Второй конструктор
        public Song(string name, string author)
        {
            Name = name;
            Author = author;
            Prev = null;
        }

        // Конструктор по умолчанию
        public Song()
        {
            Name = "Unknown";
            Author = "Unknown";
            Prev = null;
        }

        public string Title()
        {
            return Name + " ; " + Author;
        }

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }
            Song other = (Song)obj;
            return Name == other.Name && Author == other.Author;
        }
    }

}
