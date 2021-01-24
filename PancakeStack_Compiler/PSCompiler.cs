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
        private static FieldBuilder swapPancakeStack;
        private static Dictionary<string, Label> labelDictionary = new Dictionary<string, Label>();

        public static void Compile(string assemblyName, string outputFileName, string[] sourceCode, bool compilerFlag)
        {
            var asm = AppDomain.CurrentDomain.DefineDynamicAssembly(new AssemblyName(assemblyName), AssemblyBuilderAccess.Save);
            var module = asm.DefineDynamicModule(assemblyName, outputFileName);

            var mainClassTypeName = assemblyName + ".PSProgram";
            var type = module.DefineType(mainClassTypeName, TypeAttributes.Class | TypeAttributes.Sealed | TypeAttributes.Abstract | TypeAttributes.Public); //cheat to make it static

            accumulator1 = type.DefineField("accumulator1", typeof(int), FieldAttributes.Static | FieldAttributes.Private);
            accumulator2 = type.DefineField("accumulator2", typeof(int), FieldAttributes.Static | FieldAttributes.Private);
            swapPancakeStack = type.DefineField("swapPancakeStack", typeof(Stack<int>), FieldAttributes.Static | FieldAttributes.Private);

            var pancakeStackField = type.DefineField("pancakeStack", typeof(Stack<int>), FieldAttributes.Static | FieldAttributes.Private);
            var constructor = type.DefineConstructor(MethodAttributes.Static, CallingConventions.Standard, null); //static constructor parameterless

            var cctorIlGen = constructor.GetILGenerator();
            GenerateConstructorBody(cctorIlGen, pancakeStackField);

            var mainMethod = type.DefineMethod("Execute", MethodAttributes.Public | MethodAttributes.Static);
            var ilGen = mainMethod.GetILGenerator();

            //var testMethod = type.DefineMethod("Test", MethodAttributes.Public | MethodAttributes.Static);
            //var ilGenTest = testMethod.GetILGenerator();


            for (int i = 0; i < sourceCode.Length; i++)
            {
                switch (sourceCode[i])
                {
                    case var word when new Regex(@"Put this ([^ ]*?) pancake on top!").IsMatch(word):
                        {
                            var match = Regex.Match(word, @"Put this ([^ ]*?) pancake on top!");
                            GeneratePutThisXPancakeOnTopInstruction(ilGen, pancakeStackField, match.Groups[1].Value.Length);
                            break;
                        }
                    case "Eat the pancake on top!":
                        GenerateEatThePancakeOnTopInstruction(ilGen, pancakeStackField);
                        break;
                    case "Put the top pancakes together!":
                        {
                            GeneratePutTheTopPancakesTogetherInstruction(ilGen, pancakeStackField);
                            break;
                        }
                    case "Give me a pancake!":
                        GenerateGiveMeAPancakeInstruction(ilGen, pancakeStackField);
                        break;
                    case "How about a hotcake?":
                        GenerateHowAboutAHotcake(ilGen, pancakeStackField);
                        break;
                    case "Show me a pancake!":
                        GenerateShowMeAPancakeInstruction(ilGen, pancakeStackField);
                        break;
                    case "Take from the top pancakes!":
                        {
                            GenerateTakeFromTheTopPancakesInstruction(ilGen, pancakeStackField);
                            break;
                        }
                    case "Flip the pancakes on top!":
                        {
                            GenerateFlipThePancakesOnTopInstruction(ilGen, pancakeStackField);
                            break;
                        }
                    case "Put another pancake on top!":
                        {
                            GeneratePutAnotherPancakeOnTopInstruction(ilGen, pancakeStackField);
                            break;
                        }
                    case var label when new Regex(@"\[(.*)\]").IsMatch(label):
                        {
                            var match = Regex.Match(label, @"\[(.*)\]");
                            GenerateLabelInstruction(ilGen, match.Groups[1].Value);
                            break;
                        }
                    case var label when new Regex("If the pancake isn't tasty, go over to \"(.*)\".").IsMatch(label):
                        {                            
                            var match = Regex.Match(label, "If the pancake isn't tasty, go over to \"(.*)\".");
                            GenerateIfThePancakeIsntTastyGoOverToInstruction(ilGen, pancakeStackField, match.Groups[1].Value);
                            break;
                        }
                    case var label when new Regex("If the pancake is tasty, go over to \"(.*)\".").IsMatch(label):
                        {
                            var match = Regex.Match(label, "If the pancake is tasty, go over to \"(.*)\".");
                            GenerateIfThePancakeIsTastyGoOverToInstruction(ilGen, pancakeStackField, match.Groups[1].Value);
                            break;
                        }
                    case "Put syrup on the pancakes!":
                        GeneratePutSyrupOnThePancakesInstruction(ilGen, pancakeStackField);
                        break;
                    case "Put butter on the pancakes!":
                        {
                            GeneratePutButterOnThePancakes(ilGen, pancakeStackField);
                            break;
                        }
                    case "Take off the syrup!":
                        GenerateTakeOffTheSyrupInstruction(ilGen, pancakeStackField);
                        break;
                    case "Take off the butter!":
                        {
                            GenerateTakeOffTheButterInstruction(ilGen, pancakeStackField);
                            break;
                        }
                    case "Eat all of the pancakes!":
                        goto end_of_compile_loop; //Probably the only situation where goto is a good practice
                }
            }
            end_of_compile_loop:

            if(compilerFlag)
            {
                ilGen.Emit(OpCodes.Call, typeof(Console).GetMethod("ReadLine", BindingFlags.Public | BindingFlags.Static));
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

            ilGen.Emit(OpCodes.Ret);
        }

        private static void GeneratePutThisXPancakeOnTopInstruction(ILGenerator ilGen, FieldInfo pancakeStack, int number)
        {
            ilGen.Emit(OpCodes.Ldsfld, pancakeStack);
            ilGen.Emit(OpCodes.Ldc_I4, number);
            ilGen.Emit(OpCodes.Callvirt, typeof(Stack<int>).GetMethod("Push"));
        }

        private static void GenerateEatThePancakeOnTopInstruction(ILGenerator ilGen, FieldInfo pancakeStack)
        {
            ilGen.Emit(OpCodes.Ldsfld, pancakeStack);
            ilGen.Emit(OpCodes.Callvirt, typeof(Stack<int>).GetMethod("Pop"));
            ilGen.Emit(OpCodes.Pop);
        }

        private static void GeneratePutTheTopPancakesTogetherInstruction(ILGenerator ilGen, FieldInfo pancakeStack)
        {
            ilGen.Emit(OpCodes.Ldsfld, pancakeStack);
            ilGen.Emit(OpCodes.Ldsfld, pancakeStack);
            ilGen.Emit(OpCodes.Callvirt, typeof(Stack<int>).GetMethod("Pop"));
            ilGen.Emit(OpCodes.Ldsfld, pancakeStack);
            ilGen.Emit(OpCodes.Callvirt, typeof(Stack<int>).GetMethod("Pop"));
            ilGen.Emit(OpCodes.Add);
            ilGen.Emit(OpCodes.Callvirt, typeof(Stack<int>).GetMethod("Push"));
            ilGen.Emit(OpCodes.Nop);
        }

        private static void GenerateGiveMeAPancakeInstruction(ILGenerator ilGen, FieldInfo pancakeStack)
        {
            ilGen.Emit(OpCodes.Ldsfld, pancakeStack);
            ilGen.Emit(OpCodes.Call, typeof(Console).GetMethod("Read", BindingFlags.Public | BindingFlags.Static));
            ilGen.Emit(OpCodes.Call, typeof(Convert).GetMethod("ToInt32", BindingFlags.Public | BindingFlags.Static, null, new Type[] { typeof(Int32) }, null));
            ilGen.Emit(OpCodes.Callvirt, typeof(Stack<int>).GetMethod("Push"));
        }

        private static void GenerateHowAboutAHotcake(ILGenerator ilGen, FieldInfo pancakeStack)
        {
            ilGen.Emit(OpCodes.Ldsfld, pancakeStack);
            ilGen.Emit(OpCodes.Call, typeof(Console).GetMethod("Read", BindingFlags.Public | BindingFlags.Static));            
            ilGen.Emit(OpCodes.Callvirt, typeof(Stack<int>).GetMethod("Push"));
        }

        private static void GenerateShowMeAPancakeInstruction(ILGenerator ilGen, FieldInfo pancakeStack)
        {
            ilGen.Emit(OpCodes.Ldsfld, pancakeStack);
            ilGen.Emit(OpCodes.Callvirt, typeof(Stack<int>).GetMethod("Peek"));
            ilGen.Emit(OpCodes.Conv_U2);
            ilGen.Emit(OpCodes.Call, typeof(Console).GetMethod("Write", BindingFlags.Public | BindingFlags.Static, null, new Type[] { typeof(Char) }, null));
        }

        private static void GenerateTakeFromTheTopPancakesInstruction(ILGenerator ilGen, FieldInfo pancakeStack)
        {
            ilGen.Emit(OpCodes.Ldsfld, pancakeStack);
            ilGen.Emit(OpCodes.Ldsfld, pancakeStack);
            ilGen.Emit(OpCodes.Callvirt, typeof(Stack<int>).GetMethod("Pop"));
            ilGen.Emit(OpCodes.Ldsfld, pancakeStack);
            ilGen.Emit(OpCodes.Callvirt, typeof(Stack<int>).GetMethod("Pop"));
            ilGen.Emit(OpCodes.Sub);
            ilGen.Emit(OpCodes.Callvirt, typeof(Stack<int>).GetMethod("Push"));
        }

        private static void GenerateFlipThePancakesOnTopInstruction(ILGenerator ilGen, FieldInfo pancakeStack)
        {
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
        }

        private static void GeneratePutAnotherPancakeOnTopInstruction(ILGenerator ilGen, FieldInfo pancakeStack)
        {
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
        }

        private static void GenerateLabelInstruction(ILGenerator ilGen, string labelName)
        {
            labelDictionary[labelName] = ilGen.DefineLabel();
            ilGen.MarkLabel(labelDictionary[labelName]);
        }

        private static void GeneratePutSyrupOnThePancakesInstruction(ILGenerator ilGen, FieldInfo pancakeStack)
        {
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
        }

        private static void GeneratePutButterOnThePancakes(ILGenerator ilGen, FieldInfo pancakeStack)
        {
            ilGen.Emit(OpCodes.Ldsfld, pancakeStack);
            ilGen.Emit(OpCodes.Ldsfld, pancakeStack);
            ilGen.Emit(OpCodes.Callvirt, typeof(Stack<int>).GetMethod("Pop"));

            ilGen.Emit(OpCodes.Ldc_I4_1);
            ilGen.Emit(OpCodes.Add);
            ilGen.Emit(OpCodes.Callvirt, typeof(Stack<int>).GetMethod("Push"));
        }

        private static void GenerateTakeOffTheSyrupInstruction(ILGenerator ilGen, FieldInfo pancakeStack)
        {
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
        }

        private static void GenerateTakeOffTheButterInstruction(ILGenerator ilGen, FieldInfo pancakeStack)
        {
            ilGen.Emit(OpCodes.Ldsfld, pancakeStack);
            ilGen.Emit(OpCodes.Ldsfld, pancakeStack);
            ilGen.Emit(OpCodes.Callvirt, typeof(Stack<int>).GetMethod("Pop"));

            ilGen.Emit(OpCodes.Ldc_I4_1);
            ilGen.Emit(OpCodes.Sub);
            ilGen.Emit(OpCodes.Callvirt, typeof(Stack<int>).GetMethod("Push"));
        }

        private static void GenerateIfThePancakeIsntTastyGoOverToInstruction(ILGenerator ilGen, FieldInfo pancakeStack, string labelName)
        {
            var endLabel = ilGen.DefineLabel();
            ilGen.Emit(OpCodes.Ldsfld, pancakeStack);
            ilGen.Emit(OpCodes.Callvirt, typeof(Stack<int>).GetMethod("Peek"));
            ilGen.Emit(OpCodes.Ldc_I4_0);
            ilGen.Emit(OpCodes.Ceq);
            ilGen.Emit(OpCodes.Brfalse, endLabel);
            ilGen.Emit(OpCodes.Br, labelDictionary[labelName]);
            ilGen.MarkLabel(endLabel);
        }

        private static void GenerateIfThePancakeIsTastyGoOverToInstruction(ILGenerator ilGen, FieldInfo pancakeStack, string labelName)
        {
            var endLabel = ilGen.DefineLabel();
            ilGen.Emit(OpCodes.Ldsfld, pancakeStack);
            ilGen.Emit(OpCodes.Callvirt, typeof(Stack<int>).GetMethod("Peek"));
            ilGen.Emit(OpCodes.Ldc_I4_0);
            ilGen.Emit(OpCodes.Cgt);
            ilGen.Emit(OpCodes.Brfalse, endLabel);
            ilGen.Emit(OpCodes.Br, labelDictionary[labelName]);
            ilGen.MarkLabel(endLabel);
        }
    }
}
