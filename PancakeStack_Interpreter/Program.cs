namespace PancakeStack_Interpreter
{
    class Program
    {
        static void Main(string[] args)
        {
            var interpreter = new Interpreter(@"Put this old pancake on top!
[CAT]
Eat the pancake on top!
How about a hotcake?
Show me a pancake!
Put this incredible pancake on top!
Put another pancake on top!
Put another pancake on top!
Put another pancake on top!
Put another pancake on top!
Take off the butter!
Take off the butter!
Put the top pancakes together!
Put the top pancakes together!
Put the top pancakes together!
Put the top pancakes together!
Flip the pancakes on top!
Take from the top pancakes!
If the pancake is tasty, go over to ""CAT"".
Eat all of the pancakes!");

            interpreter.Execute();
            System.Console.ReadLine();
        }
    }
}
