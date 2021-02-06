using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using System.Threading;

namespace IDE
{
    public class EmbeddedInterpreter
    {
        public event EventHandler EndOfExecutionEvent;
        public event EventHandler WaitingForInputEvent;
        public event EventHandler<OutputEventArgs> NewOutputEvent;
        public event EventHandler<Stack<int>> PancakeStackChangedEvent;
        public event EventHandler<Dictionary<string,int>> LabelDictionaryChangedEvent;
        public EventWaitHandle WaitHandle;

        private string[] _programCode;
        public Dictionary<string, int> Labels { get; set; }
        public Stack<int> PancakeStack { get; set; }
        public int ProgramIterator { get; set; }
        public string Input { private get; set; }


        public EmbeddedInterpreter(string[] input)
        {
            PancakeStack = new Stack<int>();
            Labels = new Dictionary<string, int>();
            _programCode = input;
            ProgramIterator = 0;
            WaitHandle = new EventWaitHandle(false, EventResetMode.AutoReset);
        }

        public void StartExecuting(CancellationToken ct)
        {
            while(ProgramIterator < _programCode.Length)
            {
                if (ct.IsCancellationRequested)
                    break;

                ExecuteNext(ct);                
            }

            EndOfExecutionEvent?.Invoke(this, EventArgs.Empty);
        }

        public void ExecuteNext(CancellationToken ct)
        {
            if (ProgramIterator >= _programCode.Length - 1 || ct.IsCancellationRequested)
            {
                EndOfExecutionEvent?.Invoke(this, EventArgs.Empty);
                return;
            }               

            switch (_programCode[ProgramIterator])
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
                    Input = "";

                    while (string.IsNullOrEmpty(Input))
                    {
                        if (ct.IsCancellationRequested)
                            break;

                        WaitingForInputEvent?.Invoke(this, EventArgs.Empty);
                        WaitHandle.WaitOne();
                    }

                    if (!ct.IsCancellationRequested)
                    {
                        PancakeStack.Push(Convert.ToInt32(Input));
                        Input = "";
                    }
                    break;
                case "How about a hotcake?":
                    while (string.IsNullOrEmpty(Input))
                    {
                        if (ct.IsCancellationRequested)
                            break;

                        WaitingForInputEvent?.Invoke(this, EventArgs.Empty);
                        WaitHandle.WaitOne();
                    }

                    if (!ct.IsCancellationRequested)
                    {
                        PancakeStack.Push(Input[0]);
                        Input = Input.Remove(0, 1); // imitate console buffer
                    }
                    break;
                case "Show me a pancake!":
                    NewOutputEvent(this, new OutputEventArgs() { Type = OutputType.Character, CharacterOutput = (char)PancakeStack.Peek() });
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
                        if (Labels.Any(item => item.Key == match.Groups[1].Value))
                        {
                            break;
                        }

                        Labels[match.Groups[1].Value] = PancakeStack.Peek() - 2; //Pancake stack language start counting lines from 1
                        LabelDictionaryChangedEvent?.Invoke(this, Labels);
                        break;
                    }
                case var label when new Regex(@"If the pancake isn't tasty, go over to ""(.*)"".").IsMatch(label):
                    {
                        var match = Regex.Match(label, @"If the pancake isn't tasty, go over to ""(.*)"".");
                        if (PancakeStack.Peek() == 0)
                        {
                            ProgramIterator = Labels[match.Groups[1].Value];
                        }
                        break;
                    }
                case var label when new Regex(@"If the pancake is tasty, go over to ""(.*)"".").IsMatch(label):
                    {
                        var match = Regex.Match(label, @"If the pancake is tasty, go over to ""(.*)"".");
                        if (PancakeStack.Peek() != 0)
                        {
                            ProgramIterator = Labels[match.Groups[1].Value];
                        }
                        break;
                    }
                case "Put syrup on the pancakes!":
                    PancakeStack = new Stack<int>(PancakeStack.Select(item => item + 1));
                    break;
                case "Put butter on the pancakes!":
                    {
                        var topItem = PancakeStack.Pop();
                        PancakeStack.Push(topItem + 1);
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
                case "Show me a numeric pancake!":
                    NewOutputEvent?.Invoke(this, new OutputEventArgs() { Type = OutputType.Line, LineOutput = PancakeStack.Peek().ToString() });
                    //Console.Write(PancakeStack.Peek());
                    break;
                case "Eat all of the pancakes!":
                    ProgramIterator = _programCode.Length;
                    return;
            }

            PancakeStackChangedEvent?.Invoke(this, PancakeStack);
            ProgramIterator++;
        }

    }
}
