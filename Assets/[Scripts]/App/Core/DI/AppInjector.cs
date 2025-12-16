using System;
using System.Reflection;
using System.Linq;
using System.Collections.Generic;

namespace Serjbal.Core
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    public class InjectDependencyAttribute : Attribute { }

    public class AppInjector
    {
        public void Inject(object dependency, object target)
        {
            if (dependency == null || target == null) return;
         
            InjectFields(dependency, target);
            InjectProperties(dependency, target);
        }


        void InjectProperties(object dependency, object target)
        {
            var properties = FindProperties(target.GetType(), dependency.GetType());

            properties.ForEach(property =>
            {
                if (property.GetValue(target) != null && !property.PropertyType.IsInstanceOfType(dependency))
                    throw new Exception($"Failed to inject dependency into property {property.Name}");

                property.SetValue(target, dependency);
            });
        }

        void InjectFields(object dependency, object target)
        {
            var fields = FindFields(target.GetType(), dependency.GetType());

            fields.ForEach(field =>
            {
                if (field.GetValue(target) != null && !field.FieldType.IsInstanceOfType(dependency))
                    throw new Exception($"Failed to inject dependency into field {field.Name}");

                field.SetValue(target, dependency);
            });
        }

        List<PropertyInfo> FindProperties(Type dependencyType, Type targetType)
        {
            return targetType.GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)
                .Where(p =>
                p.CanWrite &&
                p.GetCustomAttribute<InjectDependencyAttribute>() != null &&
                p.PropertyType.IsAssignableFrom(dependencyType)).ToList();
        }

        List<FieldInfo> FindFields(Type targetType, Type dependencyType)
        {
            return targetType.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)
                .Where(f =>
                !f.IsInitOnly &&
                f.GetCustomAttribute<InjectDependencyAttribute>() != null &&
                f.FieldType.IsAssignableFrom(dependencyType))
                .ToList();
        }
    }
}