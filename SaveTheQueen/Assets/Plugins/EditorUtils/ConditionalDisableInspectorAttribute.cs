using UnityEngine;
using System;
using System.Collections;

//Original version of the ConditionalHideInspectorAttribute created by Brecht Lecluyse (www.brechtos.com)
//Modified by: -

[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property |
    AttributeTargets.Class | AttributeTargets.Struct, Inherited = true)]
public class ConditionalDisableInspectorAttribute : PropertyAttribute
{
    public string ConditionalSourceField = "";
    public string ConditionalSourceField2 = "";
    public bool DisableInInspector = false;
    public bool Inverse = false;

    // Use this for initialization
    public ConditionalDisableInspectorAttribute(string conditionalSourceField)
    {
        this.ConditionalSourceField = conditionalSourceField;
        this.DisableInInspector = false;
        this.Inverse = false;
    }

    public ConditionalDisableInspectorAttribute(string conditionalSourceField, bool disableInInspector)
    {
        this.ConditionalSourceField = conditionalSourceField;
        this.DisableInInspector = disableInInspector;
        this.Inverse = false;
    }

    public ConditionalDisableInspectorAttribute(string conditionalSourceField, bool disableInInspector, bool inverse)
    {
        this.ConditionalSourceField = conditionalSourceField;
        this.DisableInInspector = disableInInspector;
        this.Inverse = inverse;
    }
}



