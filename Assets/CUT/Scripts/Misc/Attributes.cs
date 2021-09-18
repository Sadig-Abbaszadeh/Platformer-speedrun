using UnityEngine;
using System;

namespace DartsGames
{ 
    public class ReadonlyAttribute : PropertyAttribute { }

    /// <summary>
    /// Add this attribute to the monobehaviour classes to be able to use Extended Editor features
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class ExtendEditorAttribute : Attribute
    { }

    /// <summary>
    /// Only works on methods with no parameter
    /// </summary>
    [AttributeUsage(AttributeTargets.Method)]
    public class InspectorButtonAttribute : Attribute
    {
    }

    /// <summary>
    /// Implement this attribute to quickly pack fields into horizontal area
    /// </summary>
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Field)]
    public class HorizontalAreaAttribute : Attribute
    {
        /// <summary>
        /// Name of the horizontal group
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// Order inside area (ascending)
        /// </summary>
        public int order = 0;

        public HorizontalAreaAttribute(string name)
        {
            Name = name;
        }
    }

    /// <summary>
    /// Use this to serialize a field with condition. MemberName is method or field name, and value is the value which will be compared to field/method return value
    /// </summary>
    [AttributeUsage(AttributeTargets.Field)]
    public class InspectConditionAttribute : PropertyAttribute
    {
        public string MemberName { get; private set; } = "";
        public object Value { get; private set; } = default;

        public InspectConditionAttribute(string memberName, object value)
        {
            MemberName = memberName;
            Value = value;
        }
    }
}