using System.Reflection;
using LCHFramework.Data;

namespace LCHFramework.Attributes
{
    public interface IIfAttribute
    {
        public string TargetName { get; }
        public ComparisonOperator ComparisonOperator { get; }
        public object ComparisonValue { get; }
        public bool? Result { get; set; }
        public FieldInfo FieldInfo { get; set; }
        public MethodInfo MethodInfo { get; set; }
    }
}