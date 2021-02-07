using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace PancakeStack_Compiler
{
    public static class PSCompiler
    {
        private static FieldBuilder accumulator1;
        private static FieldBuilder accumulator2;
        private static FieldBuilder numInFirst;
        private static FieldBuilder swapPancakeStack;
        private static FieldBuilder instructionList;
        private static FieldBuilder programInstructionIterator;
        private static FieldBuilder labelDictionary;
        //private static Dictionary<string, Label> labelDictionary = new Dictionary<string, Label>();
        private static Dictionary<string, MethodBuilder> methodDictionary = new Dictionary<string, MethodBuilder>();

        public static void Compile(string assemblyName, string outputFileName, string[] sourceCode, List<string> compilerFlags)
        {
            bool noNewLineFlag = compilerFlags.Any(item => item == "-nonewline");
            bool waitFlag = compilerFlags.Any(item => item == "-wait");

            var asm = AppDomain.CurrentDomain.DefineDynamicAssembly(new AssemblyName(assemblyName), AssemblyBuilderAccess.Save);
            var module = asm.DefineDynamicModule(assemblyName, outputFileName);

            var mainClassTypeName = assemblyName + ".PSProgram";
            var type = module.DefineType(mainClassTypeName, TypeAttributes.Class | TypeAttributes.Sealed | TypeAttributes.Abstract | TypeAttributes.Public); //cheat to make it static

            accumulator1 = type.DefineField("accumulator1", typeof(int), FieldAttributes.Static | FieldAttributes.Private);
            accumulator2 = type.DefineField("accumulator2", typeof(int), FieldAttributes.Static | FieldAttributes.Private);
            numInFirst = type.DefineField("numInFirst", typeof(bool), FieldAttributes.Static | FieldAttributes.Private);
            swapPancakeStack = type.DefineField("swapPancakeStack", typeof(Stack<int>), FieldAttributes.Static | FieldAttributes.Private);
            instructionList = type.DefineField("instructionList", typeof(List<Action>), FieldAttributes.Static | FieldAttributes.Private);
            programInstructionIterator = type.DefineField("programInstructionIterator", typeof(int), FieldAttributes.Static | FieldAttributes.Private);
            labelDictionary = type.DefineField("labelDictionary", typeof(Dictionary<string,int>), FieldAttributes.Static | FieldAttributes.Private);

            var pancakeStackField = type.DefineField("pancakeStack", typeof(Stack<int>), FieldAttributes.Static | FieldAttributes.Private);
            var constructor = type.DefineConstructor(MethodAttributes.Static, CallingConventions.Standard, null); //static constructor parameterless

            var cctorIlGen = constructor.GetILGenerator();
            GenerateConstructorBody(cctorIlGen, pancakeStackField);

            var mainMethod = type.DefineMethod("Execute", MethodAttributes.Public | MethodAttributes.Static);
            var ilGen = mainMethod.GetILGenerator();
            
            
            ilGen.Emit(OpCodes.Newobj, typeof(List<Action>).GetConstructor(Type.EmptyTypes));
            ilGen.Emit(OpCodes.Stsfld, instructionList);
            
            ilGen.Emit(OpCodes.Newobj, typeof(Dictionary<string,int>).GetConstructor(Type.EmptyTypes));
            ilGen.Emit(OpCodes.Stsfld, labelDictionary);
            //////ilGen.Emit(OpCodes.Call, testMethod);
            ///


            //ilGen.Emit(OpCodes.Ldsfld, instructionList);
            //ilGen.Emit(OpCodes.Ldc_I4_0);
            //ilGen.Emit(OpCodes.Callvirt, typeof(List<Action>).GetMethod("get_Item"));
            //ilGen.Emit(OpCodes.Callvirt, typeof(Action).GetMethod("Invoke"));

            
            MethodBuilder method = null;

            for (int i = 0; i < sourceCode.Length; i++)
            {
                
                switch (sourceCode[i])
                {
                    case var word when new Regex(@"Put this ([^ ]*?) pancake on top!").IsMatch(word):
                        {
                            var match = Regex.Match(word, @"Put this ([^ ]*?) pancake on top!");
                                                        
                            if (!methodDictionary.TryGetValue("PutThisXPancakeOnTop"+ match.Groups[1].Value.Length.ToString(), out method))
                            {
                                GeneratePutThisXPancakeOnTopMethod(pancakeStackField, match.Groups[1].Value.Length, type);
                                method = methodDictionary["PutThisXPancakeOnTop" + match.Groups[1].Value.Length.ToString()];
                            }                            
                            break;
                        }
                    case "Eat the pancake on top!":
                        if (!methodDictionary.TryGetValue("EatThePancakeOnTop", out method))
                        {
                            GenerateEatThePancakeOnTopMethod(pancakeStackField, type);
                            method = methodDictionary["EatThePancakeOnTop"];
                        }
                        break;
                    case "Put the top pancakes together!":
                        if (!methodDictionary.TryGetValue("PutTheTopPancakesTogether", out method))
                        {
                            GeneratePutTheTopPancakesTogetherMethod(pancakeStackField, type);
                            method = methodDictionary["PutTheTopPancakesTogether"];
                        }
                        break;
                     
                    case "Give me a pancake!":
                        if (!methodDictionary.TryGetValue("GiveMeAPancake", out method))
                        {
                            GenerateGiveMeAPancakeMethod(pancakeStackField, type);
                            method = methodDictionary["GiveMeAPancake"];
                        }
                        break;
                    case "How about a hotcake?":
                        if (!methodDictionary.TryGetValue("HowAboutAHotcake", out method))
                        {
                            GenerateHowAboutAHotcakeMethod(pancakeStackField, noNewLineFlag, type);
                            method = methodDictionary["HowAboutAHotcake"];
                        }
                        break;
                    case "Show me a pancake!":
                        if (!methodDictionary.TryGetValue("ShowMeAPancake", out method))
                        {
                            GenerateShowMeAPancakeMethod(pancakeStackField, type);
                            method = methodDictionary["ShowMeAPancake"];
                        }
                        break;
                    case "Take from the top pancakes!":
                        if (!methodDictionary.TryGetValue("TakeFromTheTopPancakes", out method))
                        {
                            GenerateTakeFromTheTopPancakesMethod(pancakeStackField, type);
                            method = methodDictionary["TakeFromTheTopPancakes"];
                        }
                        break;                        
                    case "Flip the pancakes on top!":
                        if (!methodDictionary.TryGetValue("FlipThePancakesOnTop", out method))
                        {
                            GenerateFlipThePancakesOnTopMethod(pancakeStackField, type);
                            method = methodDictionary["FlipThePancakesOnTop"];
                        }
                        break;                   
                    case "Put another pancake on top!":
                        if (!methodDictionary.TryGetValue("PutAnotherPancakeOnTop", out method))
                        {
                            GeneratePutAnotherPancakeOnTopMethod(pancakeStackField, type);
                            method = methodDictionary["PutAnotherPancakeOnTop"];
                        }
                        break;
                        
                    case var label when new Regex(@"\[(.*)\]").IsMatch(label):
                        {
                            var match = Regex.Match(label, @"\[(.*)\]");

                            if (!methodDictionary.TryGetValue("Label" + match.Groups[1].Value, out method))
                            {
                                GenerateLabelMethod(pancakeStackField, match.Groups[1].Value, type);
                                method = methodDictionary["Label" + match.Groups[1].Value];
                            }
                            break;
                        }
                    case var label when new Regex("If the pancake isn't tasty, go over to \"(.*)\".").IsMatch(label):
                        {                         
                            var match = Regex.Match(label, "If the pancake isn't tasty, go over to \"(.*)\".");
                            if (!methodDictionary.TryGetValue("IfThePancakeIsntTastyGoOverTo" + match.Groups[1].Value, out method))
                            {
                                GenerateIfThePancakeIsntTastyGoOverToMethod(pancakeStackField, match.Groups[1].Value, type);
                                method = methodDictionary["IfThePancakeIsntTastyGoOverTo" + match.Groups[1].Value];
                            }
                            break;
                        }
                    case var label when new Regex("If the pancake is tasty, go over to \"(.*)\".").IsMatch(label):
                        {
                            var match = Regex.Match(label, "If the pancake is tasty, go over to \"(.*)\".");
                            if (!methodDictionary.TryGetValue("IfThePancakeIsTastyGoOverTo" + match.Groups[1].Value, out method))
                            {
                                GenerateIfThePancakeIsTastyGoOverToMethod(pancakeStackField, match.Groups[1].Value, type);
                                method = methodDictionary["IfThePancakeIsTastyGoOverTo" + match.Groups[1].Value];
                            }
                            break;
                        }
                    case "Put syrup on the pancakes!":
                        if (!methodDictionary.TryGetValue("PutSyrupOnThePancakes", out method))
                        {
                            GeneratePutSyrupOnThePancakesMethod(pancakeStackField, type);
                            method = methodDictionary["PutSyrupOnThePancakes"];
                        }                        
                        break;
                    case "Put butter on the pancakes!":
                        if (!methodDictionary.TryGetValue("PutButterOnThePancakes", out method))
                        {
                            GeneratePutButterOnThePancakesMethod(pancakeStackField, type);
                            method = methodDictionary["PutButterOnThePancakes"];
                        }
                        break;
                        
                    case "Take off the syrup!":
                        if (!methodDictionary.TryGetValue("TakeOffTheSyrup", out method))
                        {
                            GenerateTakeOffTheSyrupMethod(pancakeStackField, type);
                            method = methodDictionary["TakeOffTheSyrup"];
                        }
                        break;
                    case "Take off the butter!":
                        if (!methodDictionary.TryGetValue("TakeOffTheButter", out method))
                        {
                            GenerateTakeOffTheButterMethod(pancakeStackField, type);
                            method = methodDictionary["TakeOffTheButter"];
                        }
                        break;
                    case "Show me a numeric pancake!":
                        if (!methodDictionary.TryGetValue("ShowMeANumericPancake", out method))
                        {
                            GenerateShowMeANumericPancakeMethod(pancakeStackField, type);
                            method = methodDictionary["ShowMeANumericPancake"];
                        }
                        break;

                    case "Eat all of the pancakes!":
                        if (!methodDictionary.TryGetValue("EatAllOfThePancakes", out method))
                        {
                            GenerateEatAllOfThePancakesMethod(waitFlag, type);
                            method = methodDictionary["EatAllOfThePancakes"];
                        }
                        break;
                }

                GenerateAddInstructionToInstructionList(ilGen, method);
            }


            ilGen.Emit(OpCodes.Ldc_I4_0);
            ilGen.Emit(OpCodes.Stsfld, programInstructionIterator); //initialize program instruction iterator

            var startLoopLabel = ilGen.DefineLabel();
            var checkConditionLabel = ilGen.DefineLabel();

            ilGen.Emit(OpCodes.Br, checkConditionLabel);
            ilGen.MarkLabel(startLoopLabel);

            ilGen.Emit(OpCodes.Ldsfld, instructionList);
            ilGen.Emit(OpCodes.Ldsfld, programInstructionIterator);

            ilGen.Emit(OpCodes.Callvirt, typeof(List<Action>).GetMethod("get_Item"));
            ilGen.Emit(OpCodes.Callvirt, typeof(Action).GetMethod("Invoke"));

            ilGen.Emit(OpCodes.Ldsfld, programInstructionIterator);
            ilGen.Emit(OpCodes.Ldc_I4_1);
            ilGen.Emit(OpCodes.Add);
            ilGen.Emit(OpCodes.Stsfld, programInstructionIterator);

            ilGen.MarkLabel(checkConditionLabel);
            ilGen.Emit(OpCodes.Ldsfld, programInstructionIterator);
            ilGen.Emit(OpCodes.Ldsfld, instructionList);
            ilGen.Emit(OpCodes.Callvirt, typeof(List<Action>).GetMethod("get_Count"));
            ilGen.Emit(OpCodes.Clt);
            ilGen.Emit(OpCodes.Brtrue, startLoopLabel);

            if (waitFlag)
            {
                if(methodDictionary.TryGetValue("HowAboutAHotcake", out _))
                {
                    ilGen.Emit(OpCodes.Call, typeof(Console).GetMethod("ReadLine"));
                    ilGen.Emit(OpCodes.Pop);
                }

                ilGen.Emit(OpCodes.Call, typeof(Console).GetMethod("ReadLine"));
                ilGen.Emit(OpCodes.Pop);
            }

            ilGen.Emit(OpCodes.Ret);
            type.CreateType();
            asm.SetEntryPoint(mainMethod);
            asm.Save(outputFileName);         
        }

        private static void GenerateConstructorBody(ILGenerator ilGen, FieldInfo pancakeStackField)
        {
            ilGen.Emit(OpCodes.Nop);

            ilGen.Emit(OpCodes.Newobj, typeof(Stack<int>).GetConstructor(Type.EmptyTypes));
            ilGen.Emit(OpCodes.Stsfld, pancakeStackField);

            ilGen.Emit(OpCodes.Newobj, typeof(List<Action>).GetConstructor(Type.EmptyTypes));
            ilGen.Emit(OpCodes.Stsfld, instructionList);

            ilGen.Emit(OpCodes.Ret);
        }

        private static void GeneratePutThisXPancakeOnTopMethod(FieldInfo pancakeStack, int number, TypeBuilder type)
        {
            var method = type.DefineMethod("PutThisXPancakeOnTop" + number.ToString(), MethodAttributes.Public | MethodAttributes.Static);
            methodDictionary["PutThisXPancakeOnTop" + number.ToString()] = method;

            var ilGen = method.GetILGenerator();
            
            ilGen.Emit(OpCodes.Ldsfld, pancakeStack);
            ilGen.Emit(OpCodes.Ldc_I4, number);
            ilGen.Emit(OpCodes.Callvirt, typeof(Stack<int>).GetMethod("Push"));

            ilGen.Emit(OpCodes.Ret);
        }

        private static void GenerateEatThePancakeOnTopMethod(FieldInfo pancakeStack, TypeBuilder type)
        {
            var method = type.DefineMethod("EatThePancakeOnTop", MethodAttributes.Public | MethodAttributes.Static);
            methodDictionary["EatThePancakeOnTop"] = method;

            var ilGen = method.GetILGenerator();

            ilGen.Emit(OpCodes.Ldsfld, pancakeStack);
            ilGen.Emit(OpCodes.Callvirt, typeof(Stack<int>).GetMethod("Pop"));
            ilGen.Emit(OpCodes.Pop);

            ilGen.Emit(OpCodes.Ret);
        }

        private static void GeneratePutTheTopPancakesTogetherMethod(FieldInfo pancakeStack, TypeBuilder type)
        {
            var method = type.DefineMethod("PutTheTopPancakesTogether", MethodAttributes.Public | MethodAttributes.Static);
            methodDictionary["PutTheTopPancakesTogether"] = method;

            var ilGen = method.GetILGenerator();

            ilGen.Emit(OpCodes.Ldsfld, pancakeStack);
            ilGen.Emit(OpCodes.Ldsfld, pancakeStack);
            ilGen.Emit(OpCodes.Callvirt, typeof(Stack<int>).GetMethod("Pop"));
            ilGen.Emit(OpCodes.Ldsfld, pancakeStack);
            ilGen.Emit(OpCodes.Callvirt, typeof(Stack<int>).GetMethod("Pop"));
            ilGen.Emit(OpCodes.Add);
            ilGen.Emit(OpCodes.Callvirt, typeof(Stack<int>).GetMethod("Push"));
            ilGen.Emit(OpCodes.Nop);

            ilGen.Emit(OpCodes.Ret);
        }

        private static void GenerateGiveMeAPancakeMethod(FieldInfo pancakeStack, TypeBuilder type)
        {
            var method = type.DefineMethod("GiveMeAPancake", MethodAttributes.Public | MethodAttributes.Static);
            methodDictionary["GiveMeAPancake"] = method;

            var ilGen = method.GetILGenerator();
            var startLabel = ilGen.DefineLabel();
            var conditionLabel = ilGen.DefineLabel();
            var endIfLabel = ilGen.DefineLabel();
            var loopLabel = ilGen.DefineLabel();

            ilGen.Emit(OpCodes.Ldsfld, numInFirst);
            ilGen.Emit(OpCodes.Ldc_I4_0);
            ilGen.Emit(OpCodes.Ceq);
            ilGen.Emit(OpCodes.Brfalse_S, loopLabel);

            ilGen.Emit(OpCodes.Ldc_I4_1);
            ilGen.Emit(OpCodes.Stsfld, numInFirst);
            ilGen.Emit(OpCodes.Br, endIfLabel);

            ilGen.MarkLabel(loopLabel);
            ilGen.Emit(OpCodes.Br, conditionLabel);
            ilGen.MarkLabel(startLabel);
            ilGen.Emit(OpCodes.Call, typeof(Console).GetMethod("get_In"));
            ilGen.Emit(OpCodes.Callvirt, typeof(System.IO.TextReader).GetMethod("Read", new Type[] { }));
            ilGen.Emit(OpCodes.Pop);

            ilGen.MarkLabel(conditionLabel);
            ilGen.Emit(OpCodes.Call, typeof(Console).GetMethod("get_In"));
            ilGen.Emit(OpCodes.Callvirt, typeof(System.IO.TextReader).GetMethod("Peek"));
            ilGen.Emit(OpCodes.Ldc_I4_M1);
            ilGen.Emit(OpCodes.Ceq);
            ilGen.Emit(OpCodes.Ldc_I4_0);
            ilGen.Emit(OpCodes.Ceq);
            ilGen.Emit(OpCodes.Brtrue, startLabel);

            ilGen.MarkLabel(endIfLabel);

            //ilGen.Emit(OpCodes.Call, typeof(Console).GetMethod("ReadLine", BindingFlags.Public | BindingFlags.Static));
            //ilGen.Emit(OpCodes.Pop);
            //ilGen.MarkLabel(endOfIfLabel);


            ilGen.Emit(OpCodes.Ldsfld, pancakeStack);
            ilGen.Emit(OpCodes.Call, typeof(Console).GetMethod("ReadLine", BindingFlags.Public | BindingFlags.Static));
            ilGen.Emit(OpCodes.Call, typeof(Convert).GetMethod("ToInt32", BindingFlags.Public | BindingFlags.Static, null, new Type[] { typeof(string) }, null));
            ilGen.Emit(OpCodes.Callvirt, typeof(Stack<int>).GetMethod("Push"));

            ilGen.Emit(OpCodes.Ret);
        }

        private static void GenerateHowAboutAHotcakeMethod(FieldInfo pancakeStack, bool noNewLineFlag, TypeBuilder type)
        {
            var method = type.DefineMethod("HowAboutAHotcake", MethodAttributes.Public | MethodAttributes.Static);
            methodDictionary["HowAboutAHotcake"] = method;

            var ilGen = method.GetILGenerator();

            ilGen.Emit(OpCodes.Ldc_I4_1);
            ilGen.Emit(OpCodes.Stsfld, numInFirst);

            if (noNewLineFlag)
            {
                var startLabel = ilGen.DefineLabel();
                var firstConditionLabel = ilGen.DefineLabel();
                var secondConditionLabel = ilGen.DefineLabel();

                ilGen.MarkLabel(startLabel);
                ilGen.Emit(OpCodes.Call, typeof(Console).GetMethod("Read"));
                ilGen.Emit(OpCodes.Stsfld, accumulator1);
                ilGen.Emit(OpCodes.Ldsfld, accumulator1);
                ilGen.Emit(OpCodes.Ldc_I4_S, 10);
                ilGen.Emit(OpCodes.Beq, firstConditionLabel);

                ilGen.Emit(OpCodes.Ldsfld, accumulator1);
                ilGen.Emit(OpCodes.Ldc_I4_S, 13);
                ilGen.Emit(OpCodes.Ceq);
                ilGen.Emit(OpCodes.Br, secondConditionLabel);

                ilGen.MarkLabel(firstConditionLabel);
                ilGen.Emit(OpCodes.Ldc_I4_1);

                ilGen.MarkLabel(secondConditionLabel);
                ilGen.Emit(OpCodes.Brtrue, startLabel);

                ilGen.Emit(OpCodes.Ldsfld, pancakeStack);
                ilGen.Emit(OpCodes.Ldsfld, accumulator1);
                ilGen.Emit(OpCodes.Callvirt, typeof(Stack<int>).GetMethod("Push"));
            }
            else
            {
                ilGen.Emit(OpCodes.Ldsfld, pancakeStack);
                ilGen.Emit(OpCodes.Call, typeof(Console).GetMethod("Read"));
                ilGen.Emit(OpCodes.Callvirt, typeof(Stack<int>).GetMethod("Push"));
            }

            ilGen.Emit(OpCodes.Ret);
        }

        private static void GenerateShowMeAPancakeMethod(FieldInfo pancakeStack, TypeBuilder type)
        {
            var method = type.DefineMethod("ShowMeAPancake", MethodAttributes.Public | MethodAttributes.Static);
            methodDictionary["ShowMeAPancake"] = method;

            var ilGen = method.GetILGenerator();

            ilGen.Emit(OpCodes.Ldsfld, pancakeStack);
            ilGen.Emit(OpCodes.Callvirt, typeof(Stack<int>).GetMethod("Peek"));
            ilGen.Emit(OpCodes.Conv_U2);
            ilGen.Emit(OpCodes.Call, typeof(Console).GetMethod("Write", BindingFlags.Public | BindingFlags.Static, null, new Type[] { typeof(Char) }, null));

            ilGen.Emit(OpCodes.Ret);
        }

        private static void GenerateTakeFromTheTopPancakesMethod(FieldInfo pancakeStack, TypeBuilder type)
        {
            var method = type.DefineMethod("TakeFromTheTopPancakes", MethodAttributes.Public | MethodAttributes.Static);
            methodDictionary["TakeFromTheTopPancakes"] = method;

            var ilGen = method.GetILGenerator();

            ilGen.Emit(OpCodes.Ldsfld, pancakeStack);
            ilGen.Emit(OpCodes.Ldsfld, pancakeStack);
            ilGen.Emit(OpCodes.Callvirt, typeof(Stack<int>).GetMethod("Pop"));
            ilGen.Emit(OpCodes.Ldsfld, pancakeStack);
            ilGen.Emit(OpCodes.Callvirt, typeof(Stack<int>).GetMethod("Pop"));
            ilGen.Emit(OpCodes.Sub);
            ilGen.Emit(OpCodes.Callvirt, typeof(Stack<int>).GetMethod("Push"));

            ilGen.Emit(OpCodes.Ret);
        }

        private static void GenerateFlipThePancakesOnTopMethod(FieldInfo pancakeStack, TypeBuilder type)
        {
            var method = type.DefineMethod("FlipThePancakesOnTop", MethodAttributes.Public | MethodAttributes.Static);
            methodDictionary["FlipThePancakesOnTop"] = method;

            var ilGen = method.GetILGenerator();

            ilGen.Emit(OpCodes.Ldsfld, pancakeStack);
            ilGen.Emit(OpCodes.Callvirt, typeof(Stack<int>).GetMethod("Pop"));
            ilGen.Emit(OpCodes.Stsfld, accumulator1);

            ilGen.Emit(OpCodes.Ldsfld, pancakeStack);
            ilGen.Emit(OpCodes.Callvirt, typeof(Stack<int>).GetMethod("Pop"));
            ilGen.Emit(OpCodes.Stsfld, accumulator2);

            ilGen.Emit(OpCodes.Ldsfld, pancakeStack);
            ilGen.Emit(OpCodes.Ldsfld, accumulator1);
            ilGen.Emit(OpCodes.Callvirt, typeof(Stack<int>).GetMethod("Push"));
            ilGen.Emit(OpCodes.Nop);

            ilGen.Emit(OpCodes.Ldsfld, pancakeStack);
            ilGen.Emit(OpCodes.Ldsfld, accumulator2);
            ilGen.Emit(OpCodes.Callvirt, typeof(Stack<int>).GetMethod("Push"));
            ilGen.Emit(OpCodes.Nop);

            ilGen.Emit(OpCodes.Ret);
        }

        private static void GeneratePutAnotherPancakeOnTopMethod(FieldInfo pancakeStack, TypeBuilder type)
        {
            var method = type.DefineMethod("PutAnotherPancakeOnTop", MethodAttributes.Public | MethodAttributes.Static);
            methodDictionary["PutAnotherPancakeOnTop"] = method;

            var ilGen = method.GetILGenerator();

            ilGen.Emit(OpCodes.Ldsfld, pancakeStack);
            ilGen.Emit(OpCodes.Callvirt, typeof(Stack<int>).GetMethod("Pop"));
            ilGen.Emit(OpCodes.Stsfld, accumulator1);

            ilGen.Emit(OpCodes.Ldsfld, pancakeStack);
            ilGen.Emit(OpCodes.Ldsfld, accumulator1);
            ilGen.Emit(OpCodes.Callvirt, typeof(Stack<int>).GetMethod("Push"));
            ilGen.Emit(OpCodes.Nop);

            ilGen.Emit(OpCodes.Ldsfld, pancakeStack);
            ilGen.Emit(OpCodes.Ldsfld, accumulator1);
            ilGen.Emit(OpCodes.Callvirt, typeof(Stack<int>).GetMethod("Push"));
            ilGen.Emit(OpCodes.Nop);

            ilGen.Emit(OpCodes.Ret);
        }

        private static void GenerateLabelMethod(FieldInfo pancakeStack, string labelName, TypeBuilder type)
        {
            var method = type.DefineMethod("Label"+labelName, MethodAttributes.Public | MethodAttributes.Static);
            methodDictionary["Label"+labelName] = method;

            var ilGen = method.GetILGenerator();

            ilGen.Emit(OpCodes.Ldsfld, labelDictionary);
            ilGen.Emit(OpCodes.Ldstr, labelName);
            ilGen.Emit(OpCodes.Ldsfld, pancakeStack);
            ilGen.Emit(OpCodes.Callvirt, typeof(Stack<int>).GetMethod("Peek"));
            ilGen.Emit(OpCodes.Ldc_I4_2);
            ilGen.Emit(OpCodes.Sub); // Pancake stack language starts counting lines from 1
            ilGen.Emit(OpCodes.Callvirt, typeof(Dictionary<string, int>).GetMethod("Add"));

            ilGen.Emit(OpCodes.Ret);
        }

        private static void GeneratePutSyrupOnThePancakesMethod(FieldInfo pancakeStack, TypeBuilder type)
        {
            var method = type.DefineMethod("PutSyrupOnThePancakes", MethodAttributes.Public | MethodAttributes.Static);
            methodDictionary["PutSyrupOnThePancakes"] = method;

            var ilGen = method.GetILGenerator();

            ilGen.Emit(OpCodes.Newobj, typeof(Stack<int>).GetConstructor(Type.EmptyTypes)); //temporary stack
            ilGen.Emit(OpCodes.Stsfld, swapPancakeStack);

            var loopConditionsLabel = ilGen.DefineLabel();
            var loopStartLabel = ilGen.DefineLabel();

            ilGen.Emit(OpCodes.Br_S, loopConditionsLabel);

            ilGen.MarkLabel(loopStartLabel);
            ilGen.Emit(OpCodes.Ldsfld, swapPancakeStack);
            ilGen.Emit(OpCodes.Ldsfld, pancakeStack);
            ilGen.Emit(OpCodes.Callvirt, typeof(Stack<int>).GetMethod("Pop"));

            ilGen.Emit(OpCodes.Ldc_I4_1);
            ilGen.Emit(OpCodes.Add);
            ilGen.Emit(OpCodes.Callvirt, typeof(Stack<int>).GetMethod("Push")); //stack2.Push(stack1.Pop() + 1)

            ilGen.MarkLabel(loopConditionsLabel);
            ilGen.Emit(OpCodes.Ldsfld, pancakeStack);
            ilGen.Emit(OpCodes.Callvirt, typeof(Stack<int>).GetMethod("get_Count"));
            ilGen.Emit(OpCodes.Ldc_I4_0);
            ilGen.Emit(OpCodes.Cgt);
            ilGen.Emit(OpCodes.Stsfld, accumulator1);
            ilGen.Emit(OpCodes.Ldsfld, accumulator1);
            ilGen.Emit(OpCodes.Brtrue_S, loopStartLabel);           

            ilGen.Emit(OpCodes.Ldsfld, swapPancakeStack);
            ilGen.Emit(OpCodes.Stsfld, pancakeStack);

            ilGen.Emit(OpCodes.Ret);
        }

        private static void GeneratePutButterOnThePancakesMethod(FieldInfo pancakeStack, TypeBuilder type)
        {
            var method = type.DefineMethod("PutButterOnThePancakes", MethodAttributes.Public | MethodAttributes.Static);
            methodDictionary["PutButterOnThePancakes"] = method;

            var ilGen = method.GetILGenerator();

            ilGen.Emit(OpCodes.Ldsfld, pancakeStack);
            ilGen.Emit(OpCodes.Ldsfld, pancakeStack);
            ilGen.Emit(OpCodes.Callvirt, typeof(Stack<int>).GetMethod("Pop"));

            ilGen.Emit(OpCodes.Ldc_I4_1);
            ilGen.Emit(OpCodes.Add);
            ilGen.Emit(OpCodes.Callvirt, typeof(Stack<int>).GetMethod("Push"));

            ilGen.Emit(OpCodes.Ret);
        }

        private static void GenerateTakeOffTheSyrupMethod(FieldInfo pancakeStack, TypeBuilder type)
        {
            var method = type.DefineMethod("TakeOffTheSyrup", MethodAttributes.Public | MethodAttributes.Static);
            methodDictionary["TakeOffTheSyrup"] = method;

            var ilGen = method.GetILGenerator();

            ilGen.Emit(OpCodes.Newobj, typeof(Stack<int>).GetConstructor(Type.EmptyTypes)); //temporary stack
            ilGen.Emit(OpCodes.Stsfld, swapPancakeStack);

            var loopConditionsLabel = ilGen.DefineLabel();
            var loopStartLabel = ilGen.DefineLabel();

            ilGen.Emit(OpCodes.Br_S, loopConditionsLabel);

            ilGen.MarkLabel(loopStartLabel);
            ilGen.Emit(OpCodes.Ldsfld, swapPancakeStack);
            ilGen.Emit(OpCodes.Ldsfld, pancakeStack);
            ilGen.Emit(OpCodes.Callvirt, typeof(Stack<int>).GetMethod("Pop"));

            ilGen.Emit(OpCodes.Ldc_I4_1);
            ilGen.Emit(OpCodes.Sub);
            ilGen.Emit(OpCodes.Callvirt, typeof(Stack<int>).GetMethod("Push")); //stack2.Push(stack1.Pop() - 1)

            ilGen.MarkLabel(loopConditionsLabel);
            ilGen.Emit(OpCodes.Ldsfld, pancakeStack);
            ilGen.Emit(OpCodes.Callvirt, typeof(Stack<int>).GetMethod("get_Count"));
            ilGen.Emit(OpCodes.Ldc_I4_0);
            ilGen.Emit(OpCodes.Cgt);
            ilGen.Emit(OpCodes.Stsfld, accumulator1);
            ilGen.Emit(OpCodes.Ldsfld, accumulator1);
            ilGen.Emit(OpCodes.Brtrue_S, loopStartLabel);

            ilGen.Emit(OpCodes.Ldsfld, swapPancakeStack);
            ilGen.Emit(OpCodes.Stsfld, pancakeStack);

            ilGen.Emit(OpCodes.Ret);
        }

        private static void GenerateTakeOffTheButterMethod(FieldInfo pancakeStack, TypeBuilder type)
        {
            var method = type.DefineMethod("TakeOffTheButter", MethodAttributes.Public | MethodAttributes.Static);
            methodDictionary["TakeOffTheButter"] = method;

            var ilGen = method.GetILGenerator();

            ilGen.Emit(OpCodes.Ldsfld, pancakeStack);
            ilGen.Emit(OpCodes.Ldsfld, pancakeStack);
            ilGen.Emit(OpCodes.Callvirt, typeof(Stack<int>).GetMethod("Pop"));

            ilGen.Emit(OpCodes.Ldc_I4_1);
            ilGen.Emit(OpCodes.Sub);
            ilGen.Emit(OpCodes.Callvirt, typeof(Stack<int>).GetMethod("Push"));

            ilGen.Emit(OpCodes.Ret);
        }

        private static void GenerateIfThePancakeIsntTastyGoOverToMethod(FieldInfo pancakeStack, string labelName, TypeBuilder type)
        {
            var method = type.DefineMethod("IfThePancakeIsntTastyGoOverTo" + labelName, MethodAttributes.Public | MethodAttributes.Static);
            methodDictionary["IfThePancakeIsntTastyGoOverTo" + labelName] = method;

            var ilGen = method.GetILGenerator();

            var endLabel = ilGen.DefineLabel();
            ilGen.Emit(OpCodes.Ldsfld, pancakeStack);
            ilGen.Emit(OpCodes.Callvirt, typeof(Stack<int>).GetMethod("Peek"));
            ilGen.Emit(OpCodes.Ldc_I4_0);
            ilGen.Emit(OpCodes.Ceq);
            ilGen.Emit(OpCodes.Brfalse, endLabel);

            ilGen.Emit(OpCodes.Ldsfld, labelDictionary);
            ilGen.Emit(OpCodes.Ldstr, labelName);
            ilGen.Emit(OpCodes.Callvirt, typeof(Dictionary<string, int>).GetMethod("get_Item"));
            ilGen.Emit(OpCodes.Stsfld, programInstructionIterator);
            ilGen.MarkLabel(endLabel);

            ilGen.Emit(OpCodes.Ret);
        }

        private static void GenerateIfThePancakeIsTastyGoOverToMethod(FieldInfo pancakeStack, string labelName, TypeBuilder type)
        {
            var method = type.DefineMethod("IfThePancakeIsTastyGoOverTo" + labelName, MethodAttributes.Public | MethodAttributes.Static);
            methodDictionary["IfThePancakeIsTastyGoOverTo" + labelName] = method;

            var ilGen = method.GetILGenerator();

            var endLabel = ilGen.DefineLabel();
            ilGen.Emit(OpCodes.Ldsfld, pancakeStack);
            ilGen.Emit(OpCodes.Callvirt, typeof(Stack<int>).GetMethod("Peek"));
            ilGen.Emit(OpCodes.Ldc_I4_0);
            ilGen.Emit(OpCodes.Ceq);
            ilGen.Emit(OpCodes.Ldc_I4_0);
            ilGen.Emit(OpCodes.Ceq);
            ilGen.Emit(OpCodes.Brfalse, endLabel);

            ilGen.Emit(OpCodes.Ldsfld, labelDictionary);
            ilGen.Emit(OpCodes.Ldstr, labelName);
            ilGen.Emit(OpCodes.Callvirt, typeof(Dictionary<string, int>).GetMethod("get_Item"));
            ilGen.Emit(OpCodes.Stsfld, programInstructionIterator);
            ilGen.MarkLabel(endLabel);
            
            ilGen.Emit(OpCodes.Ret);
        }

        private static void GenerateShowMeANumericPancakeMethod(FieldInfo pancakeStack, TypeBuilder type)
        {
            var method = type.DefineMethod("ShowMeANumericPancake", MethodAttributes.Public | MethodAttributes.Static);
            methodDictionary["ShowMeANumericPancake"] = method;

            var ilGen = method.GetILGenerator();

            ilGen.Emit(OpCodes.Ldsfld, pancakeStack);
            ilGen.Emit(OpCodes.Callvirt, typeof(Stack<int>).GetMethod("Peek"));
            ilGen.Emit(OpCodes.Call, typeof(Console).GetMethod("Write", BindingFlags.Public | BindingFlags.Static, null, new Type[] { typeof(Int32) }, null));

            ilGen.Emit(OpCodes.Ret);
        }

        private static void GenerateEatAllOfThePancakesMethod(bool waitFlag, TypeBuilder type)
        {
            var method = type.DefineMethod("EatAllOfThePancakes", MethodAttributes.Public | MethodAttributes.Static);
            methodDictionary["EatAllOfThePancakes"] = method;

            var ilGen = method.GetILGenerator();

            if(waitFlag)
            {
                if (methodDictionary.TryGetValue("HowAboutAHotcake", out _))
                {
                    ilGen.Emit(OpCodes.Call, typeof(Console).GetMethod("ReadLine"));
                    ilGen.Emit(OpCodes.Pop);
                }

                ilGen.Emit(OpCodes.Call, typeof(Console).GetMethod("ReadLine"));
                ilGen.Emit(OpCodes.Pop);
            }

            ilGen.Emit(OpCodes.Ldc_I4_0);
            ilGen.Emit(OpCodes.Call, typeof(Environment).GetMethod("Exit"));

            ilGen.Emit(OpCodes.Ret);
        }

        private static void GenerateAddInstructionToInstructionList(ILGenerator ilGen, MethodBuilder method)
        {
            ilGen.Emit(OpCodes.Ldsfld, instructionList);
            ilGen.Emit(OpCodes.Ldnull);
            ilGen.Emit(OpCodes.Ldftn, method);
            ilGen.Emit(OpCodes.Newobj, typeof(Action).GetConstructors()[0]);
            ilGen.Emit(OpCodes.Callvirt, typeof(List<Action>).GetMethod("Add"));
        }      
    }
}
