using System;
using System.Text.RegularExpressions;
using UnityEngine;

namespace LCHFramework.Attributes
{
    [AttributeUsage(AttributeTargets.Field)]
    public class InspectorNameAttribute : PropertyAttribute
    {
        public InspectorNameAttribute(string name, bool isFromUpperChar = false)
        {
            var regex = Regex.Match(name, "[A-Z].*");
            Name = !isFromUpperChar || !regex.Success ? name : regex.Value;
        }
        
        
        
        public string Name { get; private set; }
    }
}
