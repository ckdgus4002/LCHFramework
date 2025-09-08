using System.Reflection;
using LCHFramework.Data;

namespace LCHFramework.Attributes
{
    public interface IInInspectorAttribute
    {
        public string TargetName { get; }
        public bool NeedInitializeComparison { get; }
        public ComparisonOperator ComparisonOperator { get; set; }
        public object ComparisonValue { get; set; }
        public bool? Result { get; }
        public FieldInfo FieldInfo { get; set; }
        public PropertyInfo PropertyInfo { get; set; }
        public MethodInfo MethodInfo { get; set; }
    }
}