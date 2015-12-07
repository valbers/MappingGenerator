using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MappingGenerator.LangObjects
{
    public class ClassDefinition
    {
        public bool IsInterface { get; set; }

        public string Namespace { get; set; }

        public AccessModifier AccessModifier { get; set; }

        public IEnumerable<Modifier> OtherModifiers { get; set; }

        public string Name { get; set; }

        public ClassDefinition BaseClass { get; set; }

        public IEnumerable<InstanceVariable> InstanceVariables { get; set; }

        public IEnumerable<MethodDefinition> Methods { get; set; }

        public IEnumerable<ClassDefinition> GenericArguments { get; set; }

        public bool IsConcreteType { get; set; }

        public ClassDefinition()
        {
            OtherModifiers = new List<Modifier>();
            InstanceVariables = new List<InstanceVariable>();
            Methods = new List<MethodDefinition>();
            GenericArguments = new List<ClassDefinition>();
        }

        public ClassDefinition(Type type)
        {
            IsConcreteType = true;
            IsInterface = type.IsInterface;
            Namespace = type.Namespace;
            Name = GetClassDefinitionName(type);
            GenericArguments = type.IsGenericType ?
                               type.GetGenericArguments().Select(x => new ClassDefinition(x))
                               : new List<ClassDefinition>();
        }

        public static bool operator==(ClassDefinition classDef, Type type)
        {
            if (classDef == (ClassDefinition)null && type != (Type)null)
                return false;
            if (classDef != (ClassDefinition)null && type == (Type)null)
                return false;
            if (classDef == (ClassDefinition)null && type == (Type)null)
                return true;

            var typeAsClassDef = new ClassDefinition(type);
            return typeAsClassDef.Equals(classDef);
        }
        public static bool operator !=(ClassDefinition classDef, Type type) { return !(classDef == type); }
        public static bool operator ==(Type type, ClassDefinition classDef) { return classDef == type; }
        public static bool operator !=(Type type, ClassDefinition classDef) { return !(classDef == type); }

        public override bool Equals(object obj)
        {
            var objAsClassDef = obj as ClassDefinition;
            if (objAsClassDef == null)
                return false;

            if (IsInterface != objAsClassDef.IsInterface)
                return false;
            if (Namespace != objAsClassDef.Namespace)
                return false;
            if (Name != objAsClassDef.Name)
                return false;

            var enumerator1 = objAsClassDef.GenericArguments.GetEnumerator();
            var enumerator2 = GenericArguments.GetEnumerator();
            while (enumerator1.MoveNext() & enumerator2.MoveNext())
            {
                if (!enumerator1.Current.Equals(enumerator2.Current))
                    return false;
            }
            return true;
        }

        public override int GetHashCode()
        {
            return string.Concat(IsInterface, Namespace, Name, string.Join("", GenericArguments.Select(x => string.Concat(x.Namespace, x.Name)))).GetHashCode();
        }

        public static implicit operator ClassDefinition(Type type)
        {
            if (type == null)
                return null;
            return new ClassDefinition(type);
        }
        
        private static string GetClassDefinitionName(Type type)
        {
            if (!type.IsGenericType)
                return type.Name;

            return type.Name.Split('`')[0];
            //var genericArguments = type.GetGenericArguments();
            //return string.Format("{0}<{1}>", name, string.Join(", ", genericArguments.Select(GetClassDefinitionName)));
        }
    }
}
