namespace PancakeStack_Interpreter
{
    class Program
    {
        static void Main(string[] args)
        {
            var interpreter = new Interpreter(@"Put this old pancake on top!
[CAT]
Eat the pancake on top!
Give me a pancake!
Show me a pancake!
If the pancake is tasty, go over to "+"CAT"+@".
Eat all of the pancakes!");

            interpreter.Execute();
            System.Console.ReadLine();
        }
    }
}
