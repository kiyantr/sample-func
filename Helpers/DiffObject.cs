using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LocalFunctionProj.Helpers;

public class PropertyDifference
{
    public string PropertyName { get; private set; }
    public object OldValue { get; private set; }
    public object NewValue { get; private set; }
    public Dictionary<string, PropertyDifference> ChildDifferences { get; private set; }

    public PropertyDifference(string propertyName, object oldValue, object newValue)
    {
        PropertyName = propertyName;
        OldValue = oldValue;
        NewValue = newValue;
        ChildDifferences = new Dictionary<string, PropertyDifference>();
    }
}

public static class PropertyDifferenceExtensions
{
    public static PropertyDifference CompareObjects(object obj1, object obj2)
    {
        if (obj1 == null || obj2 == null)
        {
            return new PropertyDifference(null, obj1, obj2); // Handle null values
        }

        var type1 = obj1.GetType();
        var type2 = obj2.GetType();

        if (type1 != type2)
        {
            throw new ArgumentException("Models must be of the same type for comparison.");
        }

        var difference = new PropertyDifference(type1.Name, null, null);
        if (obj1.IsDictionary())
        {

        }

        var properties = type1.GetProperties();

        foreach (var property in properties)
        {
            var oldValue = property.GetValue(obj1);
            var newValue = property.GetValue(obj2);

            if (!Equals(oldValue, newValue))
            {
                if (property.PropertyType.IsClass && property.PropertyType != typeof(string))
                {
                    // Recursively compare child objects
                    difference.ChildDifferences.Add(property.Name, CompareObjects(oldValue, newValue));
                }
                else
                {
                    difference.ChildDifferences.Add(property.Name, new PropertyDifference(property.Name, oldValue, newValue));
                }
            }
        }

        return difference;
    }

    public static bool IsDictionary(this object o)
    {
        if (o == null) return false;
        return o is IDictionary &&
               o.GetType().IsGenericType &&
               o.GetType().GetGenericTypeDefinition().IsAssignableFrom(typeof(Dictionary<,>));
    }

    public static bool IsList(this object o)
    {
        if (o == null) return false;
        return o is IList &&
               o.GetType().IsGenericType &&
               o.GetType().GetGenericTypeDefinition().IsAssignableFrom(typeof(List<>));
    }

    public class KeyValuePairComparer : IEqualityComparer<KeyValuePair<string, object>>
    {
        public bool Equals(KeyValuePair<string, object> x, KeyValuePair<string, object> y)
        {
            return x.Key == y.Key && Equals(x.Value, y.Value);
        }

        public int GetHashCode(KeyValuePair<string, object> obj)
        {
            return obj.Key.GetHashCode() ^ (obj.Value?.GetHashCode() ?? 0);
        }
    }

    public static bool AreDictionaries(object obj1, object obj2)
    {
        return obj1 is Dictionary<string, object> && obj2 is Dictionary<string, object>;
    }

    private static Dictionary<string, object> ToDictionary(PropertyDifference difference)
    {
        var dictionary = new Dictionary<string, object>();

        if (difference.ChildDifferences.Count > 0)
        {
            var childDiffs = new Dictionary<string, object>();
            foreach (var childDiff in difference.ChildDifferences)
            {
                childDiffs.Add(childDiff.Key, ToDictionary(childDiff.Value));
            }
            dictionary.Add(difference.PropertyName, childDiffs);
        }
        else
        {
            dictionary.Add(difference.PropertyName, new List<object> { difference.OldValue, difference.NewValue });
        }

        return dictionary;
    }
}