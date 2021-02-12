using Microsoft.VisualStudio.TestTools.UnitTesting;
using IDE;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

namespace IDE.Tests
{
    [TestClass()]
    public class ValidateSourceCodeTests
    {
        ValidateSourceCode testClassWithoutComments;
        string sourceCodeWithoutComments = @"Put this X pancake on top!
                                            Eat the pancake on top!
                                            Put the top pancakes together!
                                            Give me a pancake!
                                            How about a hotcake?
                                            Show me a pancake!
                                            Show me a numeric pancake!
                                            Take from the top pancakes!
                                            Flip the pancakes on top!
                                            Put another pancake on top!
                                            [label]
                                            If the pancake isn't tasty, go over to ""label"".
                                            If the pancake is tasty, go over to ""label"".
                                            Put syrup on the pancakes!
                                            Put butter on the pancakes!
                                            Take off the syrup!
                                            Take off the butter!
                                            Eat all of the pancakes!";

        ValidateSourceCode testClassWithInlineComments;
        string sourceCodeWithInlineComments = @"Put this X pancake on top!
                                            Eat the pancake on top!
                                            Put the top pancakes together!// test
                                            Give me a pancake!//test
                                            How about a hotcake?
                                            Show me a pancake!          //test
                                            Show me a numeric pancake!
                                            Take from the top pancakes!
                                            Flip the pancakes on top!
                                            Put another pancake on top!
                                            [label]
                                            If the pancake isn't tasty, go over to ""label"".
                                            If the pancake is tasty, go over to ""label"".
                                            Put syrup on the pancakes!
                                            Put butter on the pancakes!
                                            Take off the syrup!
                                            Take off the butter!
                                            Eat all of the pancakes!";

        ValidateSourceCode testClassWithInlineCommentsAndBlankSpaces;
        string sourceCodeWithInlineCommentsAndBlankSpaces = @"Put this X pancake on top!
                                            Eat the pancake on top!
                                            Put the top pancakes together!// test
                                            Give me a pancake!//test
                                            How about a hotcake?
                                            Show me a pancake!          //test
                                            Show me a numeric pancake!
                                            Take from the top pancakes!


                                            Flip the pancakes on top!
                                            Put another pancake on top!
                                            [label]

                                            //comment in blank
                                            If the pancake isn't tasty, go over to ""label"".
                                            If the pancake is tasty, go over to ""label"".
                                            Put syrup on the pancakes!
                                            Put butter on the pancakes!
                                            Take off the syrup!
                                            Take off the butter!
                                            Eat all of the pancakes!";

        ValidateSourceCode testClassWithoutWithEatAllOfThePancakesInstructionAtTheEnd;
        string sourceCodeWithoutWithEatAllOfThePancakesInstructionAtTheEnd = "Put this X pancake on top!";

        [TestInitialize()]
        public void Initialize()
        {
            testClassWithoutComments = new ValidateSourceCode(sourceCodeWithoutComments);
            testClassWithInlineComments = new ValidateSourceCode(sourceCodeWithInlineComments);
            testClassWithInlineCommentsAndBlankSpaces = new ValidateSourceCode(sourceCodeWithInlineCommentsAndBlankSpaces);
            testClassWithoutWithEatAllOfThePancakesInstructionAtTheEnd = new ValidateSourceCode(sourceCodeWithoutWithEatAllOfThePancakesInstructionAtTheEnd);
        }

        [TestMethod()]
        public void ValidateInstructions_ShouldPass_WhenNoComment()
        {
            var result = testClassWithoutComments.ValidateInstructions();
            Assert.IsTrue(result.Item1);
        }

        [TestMethod()]
        public void ValidateInstructions_ShouldPass_WhenInlineComment()
        {
            var result = testClassWithInlineComments.ValidateInstructions();
            Assert.IsTrue(result.Item1);
        }

        [TestMethod()]
        public void ValidateInstructions_ShouldPass_WhenInlineCommentAndBlankSpaces()
        {
            var result = testClassWithInlineCommentsAndBlankSpaces.ValidateInstructions();
            Assert.IsTrue(result.Item1);
        }

        [TestMethod()]
        public void MapRealLineNumbersWithRaw_ShouldMapRawCodeLinesWithPreparedAndBeTheSame_WhenInlineComment()
        {
            var result = testClassWithInlineComments.MapRealLineNumbersWithRaw();
            var values = result.Values.ToList();
            var keys = result.Keys.ToList();
            Assert.IsTrue(Enumerable.SequenceEqual(values, keys));
        }

        [TestMethod()]
        public void MapRealLineNumbersWithRaw_ShouldMapRawCodeLinesWithPreparedAndBeDifferent_WhenInlineCommentAndBlankSpaces()
        {
            var result = testClassWithInlineCommentsAndBlankSpaces.MapRealLineNumbersWithRaw();
            var values = result.Values.ToList();
            var keys = result.Keys.ToList();
            Assert.IsFalse(Enumerable.SequenceEqual(values, keys));
        }

        [TestMethod()]
        public void CheckIfCodeEndsWithEatAllOfThePancakesInstruction_WhenNoEndInstruction()
        {
            var result = testClassWithoutWithEatAllOfThePancakesInstructionAtTheEnd.CheckIfCodeEndsWithEatAllOfThePancakesInstruction();
            Assert.IsFalse(result);
        }
    }
}