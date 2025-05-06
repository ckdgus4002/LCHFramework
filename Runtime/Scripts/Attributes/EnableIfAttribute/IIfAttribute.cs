using System.Reflection;
using LCHFramework.Data;

namespace LCHFramework.Attributes
{
    public interface IIfAttribute
    {
        public string FieldName { get; }
        public ComparisonOperator ComparisonOperator { get; }
        public object ComparisonValue { get; }
        public bool? Force { get; }
        public FieldInfo FieldInfo { get; set; }
    }
}