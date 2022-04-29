using System;

namespace switchCaseAssignment
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Welcome to Switch Case Assignment.");
            Console.WriteLine("What is the all-time greatest cereal ever made?");
            var a = Console.ReadLine();

            switch (a)
            {
                case "frosted flakes":
                    Console.WriteLine("Their grrreeaatt! But not the best.");
                    break;
                case "cinnamon toast crunch":
                    Console.WriteLine("The left over milk is the best part, but try again!");
                    break;
                case "wheaties":
                    Console.WriteLine("You better not eat your wheaties. They are horrible.");
                    break;
                case "capn crunch":
                    Console.WriteLine("Oops it's berries or peanut butter but either way, they are #1!");
                    break;
                default:
                    Console.WriteLine("Not quite. Whatever you entered is far from the best");
                    break;
            }
        }
    }
}
