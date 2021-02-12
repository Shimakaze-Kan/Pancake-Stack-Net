using Microsoft.VisualStudio.TestTools.UnitTesting;
using IDE.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IDE.Models;
using IDE.Views;
using System.Windows.Input;
using System.Threading;

namespace IDE.ViewModels.Tests
{
    [TestClass()]
    public class DebuggerViewModelTests
    {
        DocumentModel _document;
        DebugDocumentModel _debugDocumentModel;
        ConsoleModel _console;
        DebuggerModel _debugger;
        object _currentView;
        EditorView _editorView;
        EditorDebugView _editorDebugView;
        DebuggerFlagsModel _debuggerFlags;

        DebuggerViewModel _debuggerViewModel;

        [TestInitialize()]
        public void Initialize()
        {
            _document = new DocumentModel();
            _debugger = new DebuggerModel();
            _console = new ConsoleModel();
            _debugDocumentModel = new DebugDocumentModel();
            _debuggerFlags = new DebuggerFlagsModel();
            _editorView = null;
            _editorDebugView = null;
            _currentView = _editorView;

            _debuggerViewModel = new DebuggerViewModel(_document, _debugDocumentModel, _console, _debugger, _currentView, _editorView, _editorDebugView, _debuggerFlags);
        }

        [TestMethod()]
        public void RunInterpreter_ShouldStopCurrentInterpreterTaskInContinuousDebuggingMode_WhenWaitingForInputAndEndCurrentTaskCommandExecuted()
        {
            _document.Text = @"How about a hotcake?
                                Eat all of the pancakes!";

            _debuggerViewModel.RunInterpreterCommand.Execute(null);
            Thread.Sleep(200);
            Assert.IsTrue(_debuggerViewModel.IsTaskWaitingForInput);
            _debuggerViewModel.EndCurrentTaskCommand.Execute(null);
            Assert.IsFalse(_debuggerViewModel.IsTaskWaitingForInput);
        }

        [TestMethod()]
        public void RunInterpreter_ShouldStopCurrentInterpreterTaskInStepByStepDebuggingMode_WhenWaitingForInputAndEndCurrentTaskCommandExecuted()
        {
            _document.Text = @"How about a hotcake?
                                Eat all of the pancakes!";

            _debuggerViewModel.NextInstructionCommand.Execute(null);
            _debuggerViewModel.NextInstructionCommand.Execute(null);
            Thread.Sleep(200);
            Assert.IsTrue(_debuggerViewModel.IsTaskWaitingForInput);
            _debuggerViewModel.EndCurrentTaskCommand.Execute(null);
            Assert.IsFalse(_debuggerViewModel.IsTaskWaitingForInput);
        }

        [TestMethod()]
        public void RunInterpreter_ShouldStopCurrentInterpreterTaskInStepByStepDebuggingMode_WhenWaitingForInputAndSendInputCommand()
        {
            _document.Text = @"How about a hotcake?
                                Eat all of the pancakes!";
            _console.ConsoleText = "test";

            _debuggerViewModel.NextInstructionCommand.Execute(null);
            _debuggerViewModel.NextInstructionCommand.Execute(null);
            Thread.Sleep(200);
            Assert.IsTrue(_debuggerViewModel.IsTaskWaitingForInput);
            _debuggerViewModel.SendInputCommand.Execute(null);
            Assert.IsFalse(_debuggerViewModel.IsTaskWaitingForInput);
        }
    }
}