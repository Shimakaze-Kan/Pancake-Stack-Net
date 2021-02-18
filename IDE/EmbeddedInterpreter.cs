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
        public event EventHandler<WaitingForInputEventArgs> WaitingForInputEvent;
        public event EventHandler<OutputEventArgs> NewOutputEvent;
        public event EventHandler<Stack<int>> PancakeStackChangedEvent;
        public event EventHandler<Dictionary<string, int>> LabelDictionaryChangedEvent;
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
            while (ProgramIterator < _programCode.Length)
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
                    if (CheckPancakeStackCount(1))
                        PancakeStack.Pop();
                    break;
                case "Put the top pancakes together!":
                    {
                        if (CheckPancakeStackCount(2))
                        {
                            var firstTopItem = PancakeStack.Pop();
                            var secondTopItem = PancakeStack.Pop();
                            PancakeStack.Push(firstTopItem + secondTopItem);
                        }
                        break;
                    }
                case "Give me a pancake!":
                    Input = "";

                    while (string.IsNullOrEmpty(Input))
                    {
                        if (ct.IsCancellationRequested)
                            break;

                        WaitingForInputEvent?.Invoke(this, new WaitingForInputEventArgs() { Type = InputType.Numeric });
                        WaitHandle.WaitOne();
                    }

                    if (!ct.IsCancellationRequested)
                    {
                        if (CheckIfValueIsInt32(Input))
                        {
                            PancakeStack.Push(Convert.ToInt32(Input));
                            Input = "";
                        }
                    }
                    break;
                case "How about a hotcake?":
                    while (string.IsNullOrEmpty(Input))
                    {
                        if (ct.IsCancellationRequested)
                            break;

                        WaitingForInputEvent?.Invoke(this, new WaitingForInputEventArgs() { Type = InputType.Alphanumeric });
                        WaitHandle.WaitOne();
                    }

                    if (!ct.IsCancellationRequested)
                    {
                        PancakeStack.Push(Input[0]);
                        Input = Input.Remove(0, 1); // imitate console buffer
                    }
                    break;
                case "Show me a pancake!":
                    if (CheckPancakeStackCount(1))
                        NewOutputEvent?.Invoke(this, new OutputEventArgs() { Type = OutputType.Character, CharacterOutput = (char)PancakeStack.Peek() });
                    break;
                case "Take from the top pancakes!":
                    {
                        if (CheckPancakeStackCount(2))
                        {
                            var firstTopItem = PancakeStack.Pop();
                            var secondTopItem = PancakeStack.Pop();
                            PancakeStack.Push(firstTopItem - secondTopItem);
                        }
                        break;
                    }
                case "Flip the pancakes on top!":
                    {
                        if (CheckPancakeStackCount(2))
                        {
                            var firstTopItem = PancakeStack.Pop();
                            var secondTopItem = PancakeStack.Pop();
                            PancakeStack.Push(firstTopItem);
                            PancakeStack.Push(secondTopItem);
                        }
                        break;
                    }
                case "Put another pancake on top!":
                    {
                        if (CheckPancakeStackCount(1))
                        {
                            var topItem = PancakeStack.Pop();
                            PancakeStack.Push(topItem);
                            PancakeStack.Push(topItem);
                        }
                        break;
                    }
                case var label when new Regex(@"\[(.*)\]").IsMatch(label):
                    {
                        if (CheckPancakeStackCount(1))
                        {
                            var match = Regex.Match(label, @"\[(.*)\]");
                            if (Labels.Any(item => item.Key == match.Groups[1].Value))
                            {
                                break;
                            }

                            Labels[match.Groups[1].Value] = PancakeStack.Peek() - 2; //Pancake stack language start counting lines from 1
                            LabelDictionaryChangedEvent?.Invoke(this, Labels);
                        }
                        break;
                    }
                case var label when new Regex(@"If the pancake isn't tasty, go over to ""(.*)"".").IsMatch(label):
                    {
                        var match = Regex.Match(label, @"If the pancake isn't tasty, go over to ""(.*)"".");

                        if (CheckPancakeStackCount(1) && CheckIfLabelExist(match.Groups[1].Value))
                        {
                            if (PancakeStack.Peek() == 0)
                            {
                                ProgramIterator = Labels[match.Groups[1].Value];
                            }
                        }
                        break;
                    }
                case var label when new Regex(@"If the pancake is tasty, go over to ""(.*)"".").IsMatch(label):
                    {
                        var match = Regex.Match(label, @"If the pancake is tasty, go over to ""(.*)"".");

                        if (CheckPancakeStackCount(1) && CheckIfLabelExist(match.Groups[1].Value))
                        {
                            if (PancakeStack.Peek() != 0)
                            {
                                ProgramIterator = Labels[match.Groups[1].Value];
                            }
                        }
                        break;
                    }
                case "Put syrup on the pancakes!":
                    PancakeStack = new Stack<int>(PancakeStack.Select(item => item + 1).Reverse());
                    break;
                case "Put butter on the pancakes!":
                    {
                        if (CheckPancakeStackCount(1))
                        {
                            var topItem = PancakeStack.Pop();
                            PancakeStack.Push(topItem + 1);
                        }
                        break;
                    }
                case "Take off the syrup!":
                    PancakeStack = new Stack<int>(PancakeStack.Select(item => item - 1).Reverse());
                    break;
                case "Take off the butter!":
                    {
                        if (CheckPancakeStackCount(1))
                        {
                            var topItem = PancakeStack.Pop();
                            PancakeStack.Push(topItem - 1);
                        }
                        break;
                    }
                case "Show me a numeric pancake!":
                    if (CheckPancakeStackCount(1))
                        NewOutputEvent?.Invoke(this, new OutputEventArgs() { Type = OutputType.Line, LineOutput = PancakeStack.Peek().ToString() });
                    break;
                case "Eat all of the pancakes!":
                    ProgramIterator = _programCode.Length;
                    return;
            }

            PancakeStackChangedEvent?.Invoke(this, PancakeStack);
            ProgramIterator++;
        }

        private bool CheckPancakeStackCount(int requiredCount)
        {
            if (PancakeStack.Count < requiredCount)
            {
                NewOutputEvent(this, new OutputEventArgs()
                {
                    Type = OutputType.Line,
                    LineOutput = "Runtime error: stack is empty, line ",
                    RuntimeError = true,
                    LineNumberErrorHandling = ProgramIterator
                });
                ProgramIterator = _programCode.Length;
                return false;
            }

            return true;
        }

        private bool CheckIfLabelExist(string requiredLabel)
        {
            if (!Labels.TryGetValue(requiredLabel, out _))
            {
                NewOutputEvent(this, new OutputEventArgs()
                {
                    Type = OutputType.Line,
                    LineOutput = string.Format("Runtime error: label '{0}' doesn't exist, line ", requiredLabel),
                    RuntimeError = true,
                    LineNumberErrorHandling = ProgramIterator
                });
                ProgramIterator = _programCode.Length;
                return false;
            }

            return true;
        }

        private bool CheckIfValueIsInt32(string value)
        {
            if (!int.TryParse(value, out _))
            {
                NewOutputEvent(this, new OutputEventArgs()
                {
                    Type = OutputType.Line,
                    LineOutput = string.Format("Runtime error: value '{0}' isn't Int32, line ",
                    value.Replace(Environment.NewLine, "")),
                    RuntimeError = true,
                    LineNumberErrorHandling = ProgramIterator
                });
                ProgramIterator = _programCode.Length;
                return false;
            }

            return true;
        }
    }
}
