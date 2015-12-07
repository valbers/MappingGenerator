using MappingGenerator.LangObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MappingGenerator
{
    public class ClassModifier : IClassModifier
    {
        private IInstructionGenerator _instructionGenerator;

        public ClassModifier(IInstructionGenerator instructionGenerator)
        {
            _instructionGenerator = instructionGenerator;
        }

        public MethodDefinition AddConstructor(ClassDefinition classDefinition, params ClassDefinition[] parameters)
        {
            return AddConstructor(classDefinition, parameters.Select(x => new MethodParameter { Name = Utils.GetVariableNameFromType(x), ParameterType = x }));
        }

        public MethodDefinition AddConstructor(ClassDefinition classDefinition, IEnumerable<MethodParameter> parameters)
        {
            var constructor = new MethodDefinition
            {
                Signature = new MethodSignature
                {
                    Name = classDefinition.Name
                }
            };

            constructor.Signature.Parameters = parameters.Select(x => new MethodParameter { Name = x.Name, ParameterType = x.ParameterType });

            classDefinition.Methods = classDefinition.Methods.Union(new[] { constructor });

            return constructor;
        }

        public void ReimplementBaseClassConstructors(ClassDefinition classDefinition, ClassDefinition baseClass)
        {
            foreach(var constructor in GetConstructors(baseClass))
            {
                var constr = AddConstructor(classDefinition, constructor.Signature.Parameters);
                constr.IsBaseConstructorReimplementation = true;
            }
        }

        private IEnumerable<MethodDefinition> GetConstructors(ClassDefinition classDefinition)
        {
            return classDefinition.Methods.Where(x => x.ReturnType == null);
        }

        public void AddDependency(ClassDefinition dependency, string name, ClassDefinition classDefinition)
        {
            var constructors = classDefinition.Methods.Where(x => x.ReturnType == null);
            foreach (var constructor in constructors)
            {
                InjectDependency(classDefinition, constructor, dependency, name);
            }
            if (constructors.Any())
                return;
            
            var newConstructor = AddConstructor(classDefinition);
            InjectDependency(classDefinition, newConstructor, dependency, name);
        }

        private void InjectDependency(ClassDefinition classDefinition,  MethodDefinition constructor, ClassDefinition dependency, string dependencyName)
        {
            var dependencyFieldName = Conventions.FieldName(dependencyName);
            classDefinition.InstanceVariables = classDefinition.InstanceVariables.Union(
                new[]
                {
                    new InstanceVariable
                    {
                        InstanceVariableType = InstanceVariableType.Field,
                        AccessModifier = AccessModifier.Private,
                        Type = dependency,
                        Name = dependencyFieldName
                    }
                }
            );

            var dependencyParameterName = dependencyName;
            constructor.Signature.Parameters = constructor.Signature.Parameters.Union(
                new[]
                {
                    new MethodParameter
                    {
                        Name = dependencyParameterName,
                        ParameterType = dependency
                    }
                }
            );

            constructor.Body = constructor.Body.Union(new[] { _instructionGenerator.SetVariable(dependencyFieldName, dependencyParameterName) });
        }
    }
}
