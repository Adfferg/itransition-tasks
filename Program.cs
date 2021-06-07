using System;
using System.Security.Cryptography;
using System.Text;

namespace task3
{
    class Program
    {

        static void Main(string[] args)
        {
            if(args.Length == 1 || args.Length % 2 == 0)
                Console.WriteLine("Error! Wrong amount of moves");
            else if (checkForRepetitions(args))
                Console.WriteLine("Error! Moves must be different");
            else
            {
                byte[] rand = new byte[16];
                RandomNumberGenerator.Create().GetBytes(rand);
                string key = "";
                for (int i = 0; i < rand.Length; i++)
                {
                    key += rand[i];
                }
                int computerMove = new Random().Next(0, args.Length);
                Console.WriteLine("HMAC:\n"+ calculateHMAC(args[computerMove], key));
                Console.WriteLine("Available moves:");
                for (int i = 0; i < args.Length; i++)
                {
                    Console.WriteLine(Convert.ToString(i + 1)+" - "+args[i]);
                }
                Console.WriteLine("0 - exit");
                int userMove;
                while (true)
                {
                    try
                    {
                        Console.Write("Enter your move:");
                        userMove = Convert.ToInt32(Console.ReadLine());
                        if (userMove < 0 || userMove > args.Length)
                            throw new Exception();
                        break;
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Try again");
                    }
                   
                }
                if (userMove != 0)
                {
                    userMove -= 1;
                    Console.WriteLine("Your move:" + args[userMove]);
                    Console.WriteLine("Computer move:" + args[computerMove]);
                    if (userMove == computerMove)
                        Console.WriteLine("Draw!");
                    else
                    {
                        if (checkForVictory(userMove + 1, computerMove + 1, args.Length))
                            Console.WriteLine("You win!");
                        else Console.WriteLine("You lose!");
                    }
                    Console.WriteLine("HMAC key:"+key);
                }
            }
        }

        static bool checkForVictory(int userMove,int computerMove,int length)
        {
            int half = length / 2;
            if (computerMove > userMove && computerMove <= userMove + half || computerMove < userMove && computerMove < userMove - half)
                return true;
            else return false;
        }

        static bool checkForRepetitions(string[] args)
        {
            for (int i = 0; i < args.Length; i++)
            {
                for (int j = 0; j < args.Length; j++)
                {
                    if (i != j && args[i] == args[j])
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        static string calculateHMAC(string str, string key)
        {
            byte[] bkey = Encoding.Default.GetBytes(key);
            using (var hmac = new HMACSHA256(bkey))
            {
                byte[] bstr = Encoding.Default.GetBytes(str);
                var bhash = hmac.ComputeHash(bstr);
                return BitConverter.ToString(bhash).Replace("-", string.Empty).ToLower();
            }
        }

    }
}
