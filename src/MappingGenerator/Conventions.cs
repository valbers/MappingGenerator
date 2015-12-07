using MappingGenerator.LangObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MappingGenerator
{
    public class Conventions
    {
        public static ClassDefinition GlobalMapperInterfaceDefinition()
        {
            return new ClassDefinition
            {
                Name = "IMapper",
                Namespace = "AutoGeneration",
                IsInterface = true
            };
        }

        public static ClassDefinition IndividualMapperInterfaceDefinition()
        {
            var tSourceGenericParameter = new ClassDefinition { Name = "TSource" };
            var tDestinationGenericParameter = new ClassDefinition { Name = "TDestination" };

            return IndividualMapperInterfaceDefinition(tSourceGenericParameter, tDestinationGenericParameter);
        }

        public static ClassDefinition IndividualMapperInterfaceDefinition(ClassDefinition tSource, ClassDefinition tDestination)
        {
            return new ClassDefinition
            {
                Name = "IMapper",
                Namespace = "AutoGeneration",
                IsInterface = true,
                GenericArguments = new[]
                {
                    tSource,
                    tDestination
                },
                Methods = new[]
                {
                    new MethodDefinition
                    {
                        ReturnType = tDestination,
                        Signature = new MethodSignature
                        {
                            Name = "Map",
                            GenericArguments = new []
                            {
                                tSource,
                                tDestination
                            },
                            Parameters = new []
                            {
                                new MethodParameter
                                {
                                    Name = Conventions.MainMappingMethodSourceParameterName(),
                                    ParameterType = new ClassDefinition
                                    {
                                        Namespace = "System",
                                        Name = "Func",
                                        GenericArguments = new []
                                        {
                                            tDestination,
                                            tSource
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            };
        }

        public static string FieldNameForGlobalMapper()
        {
            return FieldName(GlobalMapperInterfaceDefinition());
        }

        public static string ConstructorParameterNameForGlobalMapper()
        {
            return ConstructorParameterName(GlobalMapperInterfaceDefinition());
        }

        public static string FieldName(ClassDefinition classDefinition)
        {
            string name = classDefinition.Name.ToLowerInvariant();
            if (classDefinition.IsInterface)
                name = name.TrimStart('i');

            return string.Concat("_", name);
        }

        public static string FieldName(string originalName)
        {
            return string.Concat("_", originalName.TrimStart('_'));
        }

        public static string ConstructorParameterName(ClassDefinition classDefinition)
        {
            string name = classDefinition.Name.ToLowerInvariant();
            if (classDefinition.IsInterface)
                name = name.TrimStart('i');

            return name;
        }

        public static string MapToPropertyMethodName(string sourceName, string destinationName)
        {
            return string.Format("Map{0}", destinationName);
        }

        public static string MapToPropertySourceParameterName()
        {
            return "source";
        }

        public static string MapToPropertyDestinationVariableName()
        {
            return "destination";
        }

        public static string AtoBMapperBaseClassName(ClassDefinition source, ClassDefinition destination)
        {
            return string.Concat(AtoBMapperClassName(source, destination), "Base");
        }

        public static string AtoBMapperClassName(ClassDefinition source, ClassDefinition destination)
        {
            return string.Concat(Utils.GetPropertyNameFromType(source), "To", Utils.GetPropertyNameFromType(destination), "Mapper");
        }

        public static string MethodNameForMapPropertyAtoPropertyB(ClassDefinition source, ClassDefinition destination)
        {
            return "Map";
        }

        public static string MainMappingMethodName()
        {
            return string.Concat("Map");
        }

        public static string MainMappingMethodSourceParameterName()
        {
            return "sourceSelector";
        }

        public static string MainMappingMethodDestinationVariableName()
        {
            return "destination";
        }

        public static MethodDefinition MethodForCreateDestination(ClassDefinition source, ClassDefinition destination)
        {
            return new MethodDefinition
            {
                AccessModifier = AccessModifier.Protected,
                OtherModifiers = new[] { Modifier.Virtual },
                ReturnType = destination,
                Signature = new MethodSignature
                {
                    Name = "CreateDestination",
                    Parameters = new[]
                    {
                        new MethodParameter
                        {
                            Name = "source",
                            ParameterType = source
                        }
                    }
                }
            };
        }

        public static string ItemNameInForEachBlock()
        {
            return "item";
        }
    }
}
