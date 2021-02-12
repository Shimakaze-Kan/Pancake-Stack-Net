using Microsoft.VisualStudio.TestTools.UnitTesting;
using IDE;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace IDE.Tests
{
    [TestClass()]
    public class EmbeddedInterpreterTests
    {
        EmbeddedInterpreter embeddedInterpreterCorrectCode;
        string[] sampleCorrectCode = {"Put this 1234567890 pancake on top!",
                                        "Put another pancake on top!",
                                        "Put another pancake on top!",
                                        "Put another pancake on top!",
                                        "Put the top pancakes together!",
                                        "Put another pancake on top!",
                                        "Put the top pancakes together!",
                                        "Put the top pancakes together!",
                                        "Put the top pancakes together!",
                                        "Put butter on the pancakes!",
                                        "Put butter on the pancakes!",
                                        "Put butter on the pancakes!",
                                        "Put butter on the pancakes!",
                                        "Put butter on the pancakes!",
                                        "Show me a pancake!",
                                        "Eat all of the pancakes!"};

        EmbeddedInterpreter embeddedInterpreterCorrectCodeWithLabel;
        string[] sampleCorrectCodeWithLabel = { "Put this 1234567890 pancake on top!",
                                                "[label]",
                                                "Eat all of the pancakes!"};

        EmbeddedInterpreter embeddedInterpreterCorrectCodeWithNumericInput;
        string[] sampleCorrectCodeWithNumericInput = {"Give me a pancake!",
                                                        "Eat all of the pancakes!"};

        EmbeddedInterpreter embeddedInterpreterCorrectCodeWithASCIIInput;
        string[] sampleCorrectCodeWithASCIIInput = {"How about a hotcake?",
                                                     "Eat all of the pancakes!"};

        EmbeddedInterpreter embeddedInterpreterIncorrectCodePopFromEmptyStack;
        string[] sampleIncorrectCodePopFromEmptyStack = { "Eat the pancake on top!", "Eat all of the pancakes!" };

        EmbeddedInterpreter embeddedInterpreterIncorrectCodeJumpToNonexistentLabel;
        string[] sampleIncorrectCodeJumpToNonexistentLabel = { "Put this X pancake on top!",
                                                "If the pancake is tasty, go over to \"label\".", "Eat all of the pancakes!" };

        [TestMethod()]
        public void ExecuteNext_ShouldReceiveEndOfExecutionEvent_WhenExecutingEndsInStepByStep()
        {
            embeddedInterpreterCorrectCode = new EmbeddedInterpreter(sampleCorrectCode);
            bool wasEventRaised = false;
            CancellationTokenSource _cancellationTokenSource = new CancellationTokenSource();
            CancellationToken cancellationToken = _cancellationTokenSource.Token;

            embeddedInterpreterCorrectCode.EndOfExecutionEvent += new EventHandler((o,e) => wasEventRaised = true);            
            
            for (int i = 0; i < sampleCorrectCode.Length; i++)
            {
                embeddedInterpreterCorrectCode.ExecuteNext(cancellationToken);
            }
            Assert.IsTrue(wasEventRaised);
        }

        [TestMethod()]
        public void ExecuteNext_ShouldReceivePancakeStackChangedEvent_WhenEveryIteration()
        {
            embeddedInterpreterCorrectCode = new EmbeddedInterpreter(sampleCorrectCode);
            Stack<int> result = null;

            embeddedInterpreterCorrectCode.PancakeStackChangedEvent += new EventHandler<Stack<int>>((_, e) => result = e);            

            embeddedInterpreterCorrectCode.ExecuteNext(new CancellationTokenSource().Token);

            Assert.IsNotNull(result);
        }

        [TestMethod()]
        public void ExecuteNext_ShouldReceiveLabelDictionaryChangedEvent_WhenNewLabelOccurs()
        {
            embeddedInterpreterCorrectCodeWithLabel = new EmbeddedInterpreter(sampleCorrectCodeWithLabel);
            Dictionary<string,int> result = null;

            embeddedInterpreterCorrectCodeWithLabel.LabelDictionaryChangedEvent += new EventHandler<Dictionary<string,int>>((_, e) => result = e);

            for (int i = 0; i < sampleCorrectCode.Length; i++)
            {
                embeddedInterpreterCorrectCodeWithLabel.ExecuteNext(new CancellationTokenSource().Token);
            }

            Assert.IsNotNull(result);
        }

        [TestMethod()]
        public void ExecuteNext_ShouldReceiveNewOutputEvent_WhenOutputOccurs()
        {
            embeddedInterpreterCorrectCode = new EmbeddedInterpreter(sampleCorrectCode);
            string result = string.Empty;

            embeddedInterpreterCorrectCode.NewOutputEvent += new EventHandler<OutputEventArgs>((_, e) => result = e.CharacterOutput.ToString());

            for (int i = 0; i < sampleCorrectCode.Length; i++)
            {
                embeddedInterpreterCorrectCode.ExecuteNext(new CancellationTokenSource().Token);
            }

            Assert.AreNotEqual(string.Empty, result);
        }

        [TestMethod()]
        public void ExecuteNext_ShouldReceiveWaitingForInputEvent_WhenNumericInputOccurs()
        {
            embeddedInterpreterCorrectCodeWithNumericInput = new EmbeddedInterpreter(sampleCorrectCodeWithNumericInput);
            bool wasEventRaised = false;

            embeddedInterpreterCorrectCodeWithNumericInput.WaitingForInputEvent += new EventHandler<WaitingForInputEventArgs>((_, e) =>
            { wasEventRaised = e.Type == InputType.Numeric;
                embeddedInterpreterCorrectCodeWithNumericInput.Input = "123";
                embeddedInterpreterCorrectCodeWithNumericInput.WaitHandle.Set();
            });

            embeddedInterpreterCorrectCodeWithNumericInput.ExecuteNext(new CancellationTokenSource().Token);

            Assert.IsTrue(wasEventRaised);
        }

        [TestMethod()]
        public void ExecuteNext_ShouldReceiveWaitingForInputEvent_WhenASCIIInputOccurs()
        {
            embeddedInterpreterCorrectCodeWithASCIIInput = new EmbeddedInterpreter(sampleCorrectCodeWithASCIIInput);
            bool wasEventRaised = false;

            embeddedInterpreterCorrectCodeWithASCIIInput.WaitingForInputEvent += new EventHandler<WaitingForInputEventArgs>((_, e) =>
            { wasEventRaised = e.Type == InputType.Alphanumeric;
                embeddedInterpreterCorrectCodeWithASCIIInput.Input = "test";
                embeddedInterpreterCorrectCodeWithASCIIInput.WaitHandle.Set();
            });

            embeddedInterpreterCorrectCodeWithASCIIInput.ExecuteNext(new CancellationTokenSource().Token);

            Assert.IsTrue(wasEventRaised);
        }

        [TestMethod()]
        public void ExecuteNext_ShouldReturnErrorMessage_WhenNonNumberPassedToNumericInput()
        {
            embeddedInterpreterCorrectCodeWithNumericInput = new EmbeddedInterpreter(sampleCorrectCodeWithNumericInput);
            bool wasErrorMessageReceived = false;

            embeddedInterpreterCorrectCodeWithNumericInput.WaitingForInputEvent += new EventHandler<WaitingForInputEventArgs>((_, e) =>
            {
                embeddedInterpreterCorrectCodeWithNumericInput.Input = "text";
                embeddedInterpreterCorrectCodeWithNumericInput.WaitHandle.Set();
            });
            embeddedInterpreterCorrectCodeWithNumericInput.NewOutputEvent += new EventHandler<OutputEventArgs>((_, e) => wasErrorMessageReceived = e.RuntimeError);

            embeddedInterpreterCorrectCodeWithNumericInput.ExecuteNext(new CancellationTokenSource().Token);

            Assert.IsTrue(wasErrorMessageReceived);
        }

        [TestMethod()]
        public void ExecuteNext_ShouldReturnErrorMessage_WhenPopFromEmptyStack()
        {
            embeddedInterpreterIncorrectCodePopFromEmptyStack = new EmbeddedInterpreter(sampleIncorrectCodePopFromEmptyStack);
            bool wasErrorMessageReceived = false;

            embeddedInterpreterIncorrectCodePopFromEmptyStack.NewOutputEvent += new EventHandler<OutputEventArgs>((_, e) => wasErrorMessageReceived = e.RuntimeError);

            for (int i = 0; i < sampleCorrectCode.Length; i++)
            {
                embeddedInterpreterIncorrectCodePopFromEmptyStack.ExecuteNext(new CancellationTokenSource().Token);
            }

            Assert.IsTrue(wasErrorMessageReceived);
        }

        [TestMethod()]
        public void ExecuteNext_ShouldReturnErrorMessage_WhenJumpToNonexistentLabel()
        {
            embeddedInterpreterIncorrectCodeJumpToNonexistentLabel = new EmbeddedInterpreter(sampleIncorrectCodeJumpToNonexistentLabel);
            bool wasErrorMessageReceived = false;

            embeddedInterpreterIncorrectCodeJumpToNonexistentLabel.NewOutputEvent += new EventHandler<OutputEventArgs>((_, e) => wasErrorMessageReceived = e.RuntimeError);

            for (int i = 0; i < sampleCorrectCode.Length; i++)
            {
                embeddedInterpreterIncorrectCodeJumpToNonexistentLabel.ExecuteNext(new CancellationTokenSource().Token);
            }

            Assert.IsTrue(wasErrorMessageReceived);
        }
    }
}