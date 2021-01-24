﻿namespace PancakeStack_Interpreter
{
    class Program
    {
        static void Main(string[] args)
        {
            var interpreter = new Interpreter(@"Put this heavenly pancake on top!
Put another pancake on top!
Put another pancake on top!
Put another pancake on top!
Put another pancake on top!
Put another pancake on top!
Put another pancake on top!
Put another pancake on top!
Put syrup on the pancakes!
Put the top pancakes together!
Put the top pancakes together!
Put the top pancakes together!
Put the top pancakes together!
Put the top pancakes together!
Put the top pancakes together!
Put the top pancakes together!
Show me a pancake!
Put this appetizing pancake on top!
Put this delectable pancake on top!
Put this delicious pancake on top!
Put the top pancakes together!
Put the top pancakes together!
Put the top pancakes together!
Show me a pancake!
Put this wonderful pancake on top!
Take off the syrup!
Put the top pancakes together!
Show me a pancake!
Show me a pancake!
Put this rich pancake on top!
Take off the butter!
Put the top pancakes together!
Show me a pancake!
Put this delightful pancake on top!
Put this dainty pancake on top!
Put the top pancakes together!
Put another pancake on top!
Put the top pancakes together!
Show me a pancake!
Put this tasty pancake on top!
Put another pancake on top!
Put another pancake on top!
Put another pancake on top!
Put another pancake on top!
Put another pancake on top!
Put another pancake on top!
Put another pancake on top!
Put another pancake on top!
Put another pancake on top!
Put another pancake on top!
Put the top pancakes together!
Put the top pancakes together!
Put the top pancakes together!
Put the top pancakes together!
Put the top pancakes together!
Put the top pancakes together!
Put the top pancakes together!
Put the top pancakes together!
Put the top pancakes together!
Put the top pancakes together!
Put the top pancakes together!
Show me a pancake!
Eat the pancake on top!
Show me a pancake!
Put this good pancake on top!
Take off the butter!
Put the top pancakes together!
Show me a pancake!
Put this divine pancake on top!
Flip the pancakes on top!
Take from the top pancakes!
Show me a pancake!
Put this pleasant pancake on top!
Flip the pancakes on top!
Take from the top pancakes!
Show me a pancake!
Put this mouthwatering pancake on top!
Put this scrumptious pancake on top!
Put this enjoyable pancake on top!
Put the top pancakes together!
Put the top pancakes together!
Show me a pancake!
Eat all of the pancakes!");

            interpreter.Execute();
            System.Console.ReadLine();
        }
    }
}
