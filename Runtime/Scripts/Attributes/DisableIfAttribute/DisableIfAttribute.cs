using System;
using UnityEngine;

namespace LCHFramework.Attributes
{
    //Original version of the ConditionalHideAttribute created by Brecht Lecluyse (www.brechtos.com)
    //Modified by: -
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property |
        AttributeTargets.Class | AttributeTargets.Struct, Inherited = true)]
    public class DisableIfAttribute : PropertyAttribute
    {
        public string ConditionalSourceField = "";
        public string ConditionalSourceField2 = "";
        public string[] ConditionalSourceFields = new string[] { };
        public bool[] ConditionalSourceFieldInverseBools = new bool[] { };
        public bool HideInInspector = false;
        public bool Inverse = false;
        public bool UseOrLogic = false;

        public bool InverseCondition1 = false;
        public bool InverseCondition2 = false;


	    // Use this for initialization
        public DisableIfAttribute(string conditionalSourceField)
        {
            this.ConditionalSourceField = conditionalSourceField;
            this.HideInInspector = false;
            this.Inverse = false;
        }

        public DisableIfAttribute(string conditionalSourceField, bool hideInInspector)
        {
            this.ConditionalSourceField = conditionalSourceField;
            this.HideInInspector = hideInInspector;
            this.Inverse = false;
        }

        public DisableIfAttribute(string conditionalSourceField, bool hideInInspector, bool inverse)
        {
            this.ConditionalSourceField = conditionalSourceField;
            this.HideInInspector = hideInInspector;
            this.Inverse = inverse;
        }

        public DisableIfAttribute(bool hideInInspector = false)
        {
            this.ConditionalSourceField = "";
            this.HideInInspector = hideInInspector;
            this.Inverse = false;
        }

        public DisableIfAttribute(string[] conditionalSourceFields,bool[] conditionalSourceFieldInverseBools, bool hideInInspector, bool inverse)
        {
            this.ConditionalSourceFields = conditionalSourceFields;
            this.ConditionalSourceFieldInverseBools = conditionalSourceFieldInverseBools;
            this.HideInInspector = hideInInspector;
            this.Inverse = inverse;
        }

        public DisableIfAttribute(string[] conditionalSourceFields, bool hideInInspector, bool inverse)
        {
            this.ConditionalSourceFields = conditionalSourceFields;        
            this.HideInInspector = hideInInspector;
            this.Inverse = inverse;
        }
    }
}