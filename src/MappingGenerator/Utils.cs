using MappingGenerator.LangObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MappingGenerator
{
    public class Utils
    {
        public static string BuildClassDefinitionName(ClassDefinition classDefinition)
        {
            if (!classDefinition.GenericArguments.Any())
                return classDefinition.Name;

            return string.Concat(classDefinition.Name, "<", string.Join(", ", classDefinition.GenericArguments.Select(BuildTypeNameOfAVariable)), ">");
        }

        public static string BuildTypeNameOfAVariable(ClassDefinition classDefinition)
        {
            var nameWithoutGenericPart = !string.IsNullOrWhiteSpace(classDefinition.Namespace) ?
                                         string.Concat(classDefinition.Namespace, ".", classDefinition.Name) :
                                         classDefinition.Name;
            if (!classDefinition.GenericArguments.Any())
                return nameWithoutGenericPart;

            return string.Concat(nameWithoutGenericPart, "<", string.Join(", ", classDefinition.GenericArguments.Select(BuildTypeNameOfAVariable)), ">");
        }
        public static string BuildTypeNameOfClassDefinition(ClassDefinition classDefinition)
        {
            var nameWithoutGenericPart = classDefinition.Name;
            if (!classDefinition.GenericArguments.Any())
                return nameWithoutGenericPart;

            return string.Concat(nameWithoutGenericPart, "<", string.Join(", ", classDefinition.GenericArguments.Select(BuildTypeNameOfAVariable)), ">");
        }

        public static string GetVariableNameFromType(ClassDefinition type)
        {
            var name = type.Name;
            var firstChar = name[0];
            name = name.Remove(0, 1).Insert(0, firstChar.ToString().ToLowerInvariant());
            if (!type.GenericArguments.Any())
                return name;

            return string.Concat(name, "Of", string.Join("And", type.GenericArguments.Select(GetVariableNameFromType)));
        }

        public static string GetPropertyNameFromType(ClassDefinition type)
        {
            var name = type.Name;
            if (!type.GenericArguments.Any())
                return name;

            return string.Concat(name, "Of", string.Join("And", type.GenericArguments.Select(GetPropertyNameFromType)));
        }
    }
}
