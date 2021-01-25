namespace PancakeStack_Interpreter
{
    class Program
    {
        static void Main(string[] args)
        {
            var interpreter = new Interpreter(@"Put this old pancake on top!
Put another pancake on top!
Take from the top pancakes!
Put this 12345678 pancake on top!
[JUMP]
Eat the pancake on top!
Give me a pancake!
Put another pancake on top!
Take off the butter!
If the pancake is tasty, go over to ""JUMP"".
Put this 12345678911234 pancake on top!
[ADD]
Eat the pancake on top!
Put the top pancakes together!
Flip the pancakes on top!
If the pancake is tasty, go over to ""ADD"".
Flip the pancakes on top!
Show me a numeric pancake!
Eat all of the pancakes!");

            interpreter.Execute();
            System.Console.ReadLine();
        }
    }
}
