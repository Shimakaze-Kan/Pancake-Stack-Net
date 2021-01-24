using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace PancakeStack_Interpreter
{
    public class Interpreter
    {
        private string[] _programCode;
        public Dictionary<string, int> Labels { get; set; }
        public Stack<int> PancakeStack { get; private set; }

        public Interpreter(string input)
        {
            PancakeStack = new Stack<int>();
            Labels = new Dictionary<string, int>();
            _programCode = input.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
            _programCode = _programCode.Select(elem => elem.Trim()).ToArray();
        }

        public void Execute()
        {
            for(int i=0; i< _programCode.Length; i++)
            {
                switch(_programCode[i])
                {
                    case var word when new Regex(@"Put this ([^ ]*?) pancake on top!").IsMatch(word):
                        {
                            var match = Regex.Match(word, @"Put this ([^ ]*?) pancake on top!");
                            PancakeStack.Push(match.Groups[1].Value.Length);
                            break;
                        }
                    case "Eat the pancake on top!":
                        PancakeStack.Pop();
                        break;
                    case "Put the top pancakes together!":
                        {
                            var firstTopItem = PancakeStack.Pop();
                            var secondTopItem = PancakeStack.Pop();
                            PancakeStack.Push(firstTopItem + secondTopItem);
                            break;
                        }
                    case "Give me a pancake!":
                        PancakeStack.Push(Convert.ToInt32(Console.Read()));
                        break;
                    case "How about a hotcake?":
                        PancakeStack.Push(Console.Read());
                        break;
                    case "Show me a pancake!":
                        Console.Write((char)PancakeStack.Peek());
                        break;
                    case "Take from the top pancakes!":
                        {
                            var firstTopItem = PancakeStack.Pop();
                            var secondTopItem = PancakeStack.Pop();
                            PancakeStack.Push(firstTopItem - secondTopItem);
                            break;
                        }
                    case "Flip the pancakes on top!":
                        {
                            var firstTopItem = PancakeStack.Pop();
                            var secondTopItem = PancakeStack.Pop();
                            PancakeStack.Push(firstTopItem);
                            PancakeStack.Push(secondTopItem);
                            break;
                        }
                    case "Put another pancake on top!":
                        {
                            var topItem = PancakeStack.Pop();
                            PancakeStack.Push(topItem);
                            PancakeStack.Push(topItem);
                            break;
                        }
                    case var label when new Regex(@"\[(.*)\]").IsMatch(label):
                        {
                            var match = Regex.Match(label, @"\[(.*)\]");
                            if(Labels.Any(item => item.Key == match.Groups[1].Value))
                            {
                                break;
                            }

                            Labels[match.Groups[1].Value] = PancakeStack.Peek() - 1; //Pancake stack language start counting lines from 1
                            break;
                        }
                    case var label when new Regex(@"If the pancake isn't tasty, go over to (.*).").IsMatch(label):
                        {
                            var match = Regex.Match(label, @"If the pancake isn't tasty, go over to (.*).");
                            if(PancakeStack.Peek() == 0)
                            {
                                i = Labels[match.Groups[1].Value];
                            }
                            break;
                        }
                    case var label when new Regex(@"If the pancake is tasty, go over to (.*).").IsMatch(label):
                        {
                            var match = Regex.Match(label, @"If the pancake is tasty, go over to (.*).");
                            if(PancakeStack.Peek() != 0)
                            {
                                i = Labels[match.Groups[1].Value];
                            }
                            break;
                        }
                    case "Put syrup on the pancakes!":
                        PancakeStack = new Stack<int>(PancakeStack.Select(item => item+1));
                        break;
                    case "Put butter on the pancakes!":
                        {
                            var topItem = PancakeStack.Pop();
                            PancakeStack.Push(topItem+1);
                            break;
                        }
                    case "Take off the syrup!":
                        PancakeStack = new Stack<int>(PancakeStack.Select(item => item - 1));
                        break;
                    case "Take off the butter!":
                        {
                            var topItem = PancakeStack.Pop();
                            PancakeStack.Push(topItem - 1);
                            break;
                        }
                    case "Eat all of the pancakes!":
                        return;
                }
            }
        }

    }
}
