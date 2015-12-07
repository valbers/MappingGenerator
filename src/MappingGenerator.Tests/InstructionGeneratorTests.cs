using MappingGenerator.LangObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace MappingGenerator.Tests
{
    public class InstructionGenerator_ReturnProperty_Tests
    {
        [Fact]
        public void builds_correct_instruction_code_for_returning_property()
        {
            var instructionGenerator = new InstructionGenerator();
            var code = instructionGenerator.ReturnProperty("myVariable", "MyProperty").Code;

            Assert.Equal("return myVariable.MyProperty;", code);
        }
    }

    public class InstructionGenerator_ReturnValue_Tests
    {
        [Fact]
        public void builds_correct_instruction_code_for_returning_value()
        {
            var instructionGenerator = new InstructionGenerator();
            var code = instructionGenerator.ReturnValue("123").Code;

            Assert.Equal("return 123;", code);
        }
    }

    public class InstructionGenerator_YieldReturn_Tests
    {
        [Fact]
        public void builds_correct_instruction_code_for_yield_return()
        {
            var instructionGenerator = new InstructionGenerator();
            var code = instructionGenerator.YieldReturn("myEnumerable").Code;

            Assert.Equal("yield return myEnumerable;", code);
        }
    }

    public class InstructionGenerator_DeclareVariable_Tests
    {
        [Fact]
        public void builds_correct_string()
        {
            var instructionGenerator = new InstructionGenerator();
            var code = instructionGenerator.DeclareVariable("myString", typeof(string)).Code;

            Assert.Equal("System.String myString;", code);
        }

        [Fact]
        public void builds_correct_list_of_string()
        {
            var instructionGenerator = new InstructionGenerator();
            var code = instructionGenerator.DeclareVariable("myListOfString", typeof(List<string>)).Code;

            Assert.Equal("System.Collections.Generic.List<System.String> myListOfString;", code);
        }

        [Fact]
        public void builds_correct_list_of_dictionary_of_object_and_list_of_int()
        {
            var instructionGenerator = new InstructionGenerator();
            var code = instructionGenerator.DeclareVariable("myAwkwardVariable", typeof(List<Dictionary<object, List<int>>>)).Code;

            Assert.Equal("System.Collections.Generic.List<System.Collections.Generic.Dictionary<System.Object, System.Collections.Generic.List<System.Int32>>> myAwkwardVariable;", code);
        }

        [Fact]
        public void builds_correct_ilist_of_idictionary_of_object_and_ilist_of_int()
        {
            var instructionGenerator = new InstructionGenerator();
            var code = instructionGenerator.DeclareVariable("myAwkwardVariable", typeof(IList<IDictionary<object, IList<int>>>)).Code;

            Assert.Equal("System.Collections.Generic.IList<System.Collections.Generic.IDictionary<System.Object, System.Collections.Generic.IList<System.Int32>>> myAwkwardVariable;", code);
        }

        [Fact]
        public void builds_correct_meta_type()
        {
            var baseMapper = new ClassDefinition
            {
                Name = "BaseMapper",
                Namespace = "MappingGenerator"
            };
            var instructionGenerator = new InstructionGenerator();
            var code = instructionGenerator.DeclareVariable("baseMapper", baseMapper).Code;

            Assert.Equal("MappingGenerator.BaseMapper baseMapper;", code);
        }

        [Fact]
        public void builds_correct_meta_type_with_generic_parameters()
        {
            var baseMapper = new ClassDefinition
            {
                Name = "BaseMapper",
                Namespace = "MappingGenerator",
                GenericArguments = new[] 
                {
                    new ClassDefinition
                    {
                        Name = "SomeOtherType",
                        Namespace = "Somewhere"
                    },
                    new ClassDefinition
                    {
                        Name = "SomeOtherType1",
                        Namespace = "Somewhere"
                    }
                }
            };
            var instructionGenerator = new InstructionGenerator();
            var code = instructionGenerator.DeclareVariable("baseMapper", baseMapper).Code;

            Assert.Equal("MappingGenerator.BaseMapper<Somewhere.SomeOtherType, Somewhere.SomeOtherType1> baseMapper;", code);
        }
    }

    public class InstructionGenerator_SetVariableWithReturnValueFromMethod_Tests
    {
        [Fact]
        public void builds_correct_code()
        {
            var instructionGenerator = new InstructionGenerator();
            var methodDefinition = new MethodDefinition
            {
                Signature = new MethodSignature
                {
                    Name = "Add"
                }
            };

            var code = instructionGenerator.SetVariableWithReturnValueFromMethod("sum", methodDefinition, "12", "34").Code;

            Assert.Equal("sum = Add(12, 34);", code);
        }
    }

    public class InstructionGenerator_SetVariable_Tests
    {
        [Fact]
        public void builds_correct_code()
        {
            var instructionGenerator = new InstructionGenerator();
            var code = instructionGenerator.SetVariable("myVar", "\"value\"").Code;

            Assert.Equal("myVar = \"value\";", code);
        }
    }

    public class InstructionGenerator_CallMethod_Tests
    {
        [Fact]
        public void builds_correct_code()
        {
            var instructionGenerator = new InstructionGenerator();
            var code = instructionGenerator.CallMethod("myDependency", "Work", "param1", "param2").Code;

            Assert.Equal("myDependency.Work(param1, param2);", code);
        }
    }

    public class InstructionGenerator_GenerateIfBlock_Tests
    {
        [Fact]
        public void code_equals_null()
        {
            var instructionGenerator = new InstructionGenerator();
            var ifBlock = instructionGenerator.GenerateIfBlock(new BooleanExpression { LeftOperand = "myVar", Operator = "==", RightOperand = "42" }, new Instruction { Code = "Save the world" }, new Instruction { Code = "Flee" });

            Assert.Null(ifBlock.Code);
        }

        [Fact]
        public void code_with_both_if_and_else_block_bodies_has_truecase_code_block()
        {
            var instructionGenerator = new InstructionGenerator();
            var ifBlock = instructionGenerator.GenerateIfBlock(new BooleanExpression { LeftOperand = "myVar", Operator = "==", RightOperand = "42" }, new Instruction { Code = "Save the world" }, new Instruction { Code = "Flee" });

            Assert.Equal("Save the world", ifBlock.TrueCaseInstruction.Code);
        }

        [Fact]
        public void code_with_both_if_and_else_block_bodies_has_elsecase_code_block()
        {
            var instructionGenerator = new InstructionGenerator();
            var ifBlock = instructionGenerator.GenerateIfBlock(new BooleanExpression { LeftOperand = "myVar", Operator = "==", RightOperand = "42" }, new Instruction { Code = "Save the world" }, new Instruction { Code = "Flee" });

            Assert.Equal("Flee", ifBlock.ElseCaseInstruction.Code);
        }
    }

    public class InstructionGenerator_GenerateForEachBlock_Tests
    {
        [Fact]
        public void code_equals_null()
        {
            var instructionGenerator = new InstructionGenerator();
            var foreachBlock = instructionGenerator.GenerateForEachBlock("myEnumerable", new Instruction { Code = "i++" });

            Assert.Null(foreachBlock.Code);
        }

        [Fact]
        public void ienumerableVariable_is_the_one_given()
        {
            var instructionGenerator = new InstructionGenerator();
            var foreachBlock = instructionGenerator.GenerateForEachBlock("myEnumerable", new Instruction { Code = "i++" });

            Assert.Equal("myEnumerable", foreachBlock.IEnumerableVariable);
        }

        [Fact]
        public void blockBody_is_the_one_given()
        {
            var instructionGenerator = new InstructionGenerator();
            var blockBody = new Instruction { Code = "i++" };
            var foreachBlock = instructionGenerator.GenerateForEachBlock("myEnumerable", blockBody);

            Assert.Equal(blockBody, foreachBlock.BlockBody);
        }
    }
}
