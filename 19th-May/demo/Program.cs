using System;
using System.Reflection;

public class Greeter
{
    public static void Invoke()
    {
        Console.WriteLine("Please enter your name:");
        string? name = Console.ReadLine();
        Console.WriteLine($"Hello, {name}! Welcome.");
    }
}

public class Larger
{
    public static void Invoke()
    {
        Console.WriteLine("enter two numbers");
        string? line = Console.ReadLine();
        if (string.IsNullOrWhiteSpace(line))
        {
            Console.WriteLine("No input provided.");
            return;
        }
        string[] inputs = line.Split(' ');
        string? input1 = inputs[0].Trim();
        string? input2 = inputs[1].Trim();
        int num1, num2;
        if (!int.TryParse(input1, out num1) || !int.TryParse(input2, out num2))
        {
            Console.WriteLine("Invalid input");
            return;
        }
        if (num1 > num2)
        {
            Console.WriteLine($"{num1} is larger than {num2}");
        }
        else if (num2 > num1)
        {
            Console.WriteLine($"{num2} is larger than {num1}");
        }
        else if (num1 == num2)
        {
            Console.WriteLine("Both numbers are equal.");
        }
    }
}
public class Calci
{
    public static void Invoke()
    {
        Console.WriteLine("enter expression with 2 no and a opr.");
        string? line = Console.ReadLine();
        if (string.IsNullOrWhiteSpace(line))
        {
            Console.WriteLine("No input provided.");
            return;
        }
        string[] inputs = line.Split(' ');
        string? input1 = inputs[0].Trim();
        string? input2 = inputs[2].Trim();
        char opr = Convert.ToChar(inputs[1]);
        int num1, num2;
        if (!int.TryParse(input1, out num1) || !int.TryParse(input2, out num2))
        {
            Console.WriteLine("Invalid input");
            return;
        }
        if (opr == '+')
        {
            Console.WriteLine($"{num1} + {num2} = {num1 + num2}");
        }
        else if (opr == '-')
        {
            Console.WriteLine($"{num1} - {num2} = {num1 - num2}");
        }
        else if (opr == '*')
        {
            Console.WriteLine($"{num1} * {num2} = {num1 * num2}");
        }
        else if (opr == '/')
        {
            if (num2 != 0)
            {
                Console.WriteLine($"{num1} / {num2} = {num1 / num2}");
            }
            else
            {
                Console.WriteLine("Division by zero is not allowed.");
            }
        }
        else
        {
            Console.WriteLine("Invalid operator");
        }
    }
}

public class PwdChecker
{
    public static void Invoke()
    {
        int tries = 3;
        Console.WriteLine("Enter username:");
        string? username = Console.ReadLine();
        Console.WriteLine("Enter password:");
        string? password = Console.ReadLine();
        string correctUsername = "Admin";
        string correctPassword = "pass";
        while (tries > 0)
        {
            if (username == correctUsername && password == correctPassword)
            {
                Console.WriteLine("Access granted.");
                break;
            }
            else
            {
                tries -= 1;
                if (tries > 0)
                {
                    Console.WriteLine($"Incorrect credentials. You have {tries} tries left.");
                    Console.WriteLine("Enter username:");
                    username = Console.ReadLine();
                    Console.WriteLine("Enter password:");
                    password = Console.ReadLine();
                }
                else
                {
                    Console.WriteLine("Access denied.");
                }
            }
        }
    }
}

public class Div7
{
    public static void Invoke()
    {
        Console.WriteLine("Enter a int array : ");
        string? line = Console.ReadLine();
        if (string.IsNullOrWhiteSpace(line))
        {
            Console.WriteLine("No input provided.");
            return;
        }
        string[] inputs = line.Split(' ');
        for (int i = 0; i < inputs.Length; i++)
        {
            if (int.TryParse(inputs[i], out int num))
            {
                if (num % 7 == 0)
                {
                    Console.WriteLine($"{num} is divisible by 7");
                }
                // else
                // {
                //     Console.WriteLine($"{num} is not divisible by 7");
                // }
            }
            else
            {
                Console.WriteLine("Invalid input");
                return;
            }
        }
    }
}

public class CountFreq
{
    public static void Invoke()
    {
        Console.WriteLine("Pls enter an int array : ");
        string? line = Console.ReadLine();
        if (string.IsNullOrWhiteSpace(line))
        {
            Console.WriteLine("No input provided.");
            return;
        }
        string[] inputs = line.Trim().Split(' ');
        Dictionary<int, int> frequency = new Dictionary<int, int>();
        for (int i = 0; i < inputs.Length; i++)
        {
            if (int.TryParse(inputs[i], out int num))
            {
                if (frequency.ContainsKey(num))
                {
                    frequency[num]++;
                }
                else
                {
                    frequency[num] = 1;
                }
            }
            else
            {
                Console.WriteLine($"Invalid input so ignoring... {inputs[i]}");
            }
        }

        foreach (var kvp in frequency)
        {
            Console.WriteLine($"{kvp.Key} occurs {kvp.Value} times.");
        }
    }
}

public class Rotate
{
    public static void Invoke()
    {
        Console.WriteLine("Pls enter an int array : ");
        string? line = Console.ReadLine();
        if (string.IsNullOrWhiteSpace(line))
        {
            Console.WriteLine("No input provided.");
            return;
        }
        string[] inputs = line.Trim().Split(' ');
        int[] arr = new int[inputs.Length];
        for (int i = 0; i < inputs.Length; i++)
        {
            if (int.TryParse(inputs[i], out int num))
            {
                if (i - 1 == -1)
                {
                    arr[inputs.Length - 1] = num;
                }
                else
                {
                    arr[i - 1] = num;
                }
            }
            else
            {
                Console.WriteLine($"Invalid input... {inputs[i]}");
                return;
            }
        }
        Console.WriteLine("Rotated array: " + string.Join(", ", arr));
    }
}

public class Merge
{
    public static void Invoke()
    {
        Console.WriteLine("Pls enter first int array : ");
        string? line = Console.ReadLine();
        if (string.IsNullOrWhiteSpace(line))
        {
            Console.WriteLine("No input provided.");
            return;
        }
        string[] input1 = line.Trim().Split(' ');
        int[] arr1 = new int[input1.Length];
        for (int i = 0; i < input1.Length; i++)
        {
            if (int.TryParse(input1[i], out int num))
            {
                arr1[i] = num;
            }
            else
            {
                Console.WriteLine($"Invalid input... {input1[i]}");
                return;
            }
        }
        Console.WriteLine("Pls enter second int array : ");
        line = Console.ReadLine();
        if (string.IsNullOrWhiteSpace(line))
        {
            Console.WriteLine("No input provided.");
            return;
        }
        string[] input2 = line.Trim().Split(' ');
        int[] arr2 = new int[input2.Length];
        for (int i = 0; i < input2.Length; i++)
        {
            if (int.TryParse(input2[i], out int num))
            {
                arr2[i] = num;
            }
            else
            {
                Console.WriteLine($"Invalid input... {input2[i]}");
                return;
            }
        }
    }
}

public class Game
{
    public static void Invoke()
    {
        const string secretWord = "GAME";
        int attempts = 0;
        int wordLength = secretWord.Length;

        Console.WriteLine($"Welcome to the game");

        while (true)
        {
            Console.Write($"Attempt {attempts + 1}: Enter your guess: ");
            string? guessInput = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(guessInput))
            {
                Console.WriteLine("Input cannot be empty. Try again.");
                continue;
            }
            string guess = guessInput.ToUpper();
            if (guess.Length != wordLength)
            {
                Console.WriteLine($"Guess must be {wordLength} letters long. Try again.");
                continue;
            }
            bool isValidGuessChars = true;
            foreach (char c in guess)
            {
                if (!char.IsLetter(c))
                {
                    isValidGuessChars = false;
                    break;
                }
            }
            if (!isValidGuessChars)
            {
                Console.WriteLine("Guess must contain only letters. Try again.");
                continue;
            }
            attempts++;

            int bulls = 0;
            int cows = 0;


            Dictionary<char, int> secretCharFreq = new Dictionary<char, int>();
            Dictionary<char, int> guessCharFreq = new Dictionary<char, int>();
            for (int i = 0; i < wordLength; i++)
            {
                if (secretWord[i] == guess[i])
                {
                    bulls++;
                }
                else
                {
                    secretCharFreq[secretWord[i]] = secretCharFreq.GetValueOrDefault(secretWord[i], 0) + 1;
                    guessCharFreq[guess[i]] = guessCharFreq.GetValueOrDefault(guess[i], 0) + 1;
                }
            }

            foreach (var kvp in secretCharFreq)
            {
                char secretChar = kvp.Key;
                if (guessCharFreq.ContainsKey(secretChar))
                {
                    cows += Math.Min(kvp.Value, guessCharFreq[secretChar]);
                }
            }
            foreach (var kvp in guessCharFreq)
            {
                char guessChar = kvp.Key;
                if (secretCharFreq.ContainsKey(guessChar))
                {
                    cows += Math.Min(secretCharFreq[guessChar], kvp.Value);
                }   
            }

            Console.WriteLine($"{bulls} Bulls, {cows} Cows");

            if (bulls == wordLength)
            {
                Console.WriteLine($"Congratulations! You guessed the word '{secretWord}' in {attempts} attempts.");
                break;
            }
        }
    }
}
public class Program
{
    public static void Main(string[] args)
    {
        while (true)
        {
            Console.WriteLine("Enter the name of the method to execute (Greet (1), Large (2), Cal (3), Pwd (4), Div (5), CountFreq (6), Rotate (7), Merge (8)): ");
            string? methodName = Console.ReadLine();

            switch (methodName)
            {
                case "1":
                    Greeter.Invoke();
                    break;
                case "2":
                    Larger.Invoke();
                    break;
                case "3":
                    Calci.Invoke();
                    break;
                case "4":
                    PwdChecker.Invoke();
                    break;
                case "5":
                    Div7.Invoke();
                    break;
                case "6":
                    CountFreq.Invoke();
                    break;
                case "7":
                    Rotate.Invoke();
                    break;
                case "8":
                    Merge.Invoke();
                    break;
                default:
                    Console.WriteLine("Method not found.");
                    break;
            }
        }
    }
}