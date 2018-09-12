using UnityEngine;
using UnityEditor;

//Original version of the ConditionalDisableInspectorAttribute created by Brecht Lecluyse (www.brechtos.com)
//Modified by: -

[CustomPropertyDrawer(typeof(ConditionalDisableInspectorAttribute))]
public class ConditionalDisableInspectorDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        ConditionalDisableInspectorAttribute cdiAttribute = (ConditionalDisableInspectorAttribute)attribute;
        bool enabled = GetConditionalDisableInspectorAttributeResult(cdiAttribute, property);

        bool wasEnabled = GUI.enabled;
        GUI.enabled = enabled;

        EditorGUI.BeginDisabledGroup(!cdiAttribute.DisableInInspector);
        EditorGUI.PropertyField(position, property, label, true);
        EditorGUI.EndDisabledGroup();

        GUI.enabled = wasEnabled;
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        return EditorGUI.GetPropertyHeight(property, label);
    }

    private bool GetConditionalDisableInspectorAttributeResult(ConditionalDisableInspectorAttribute cdiAttribute, SerializedProperty property)
    {
        bool enabled = true;
        SerializedProperty sourcePropertyValue = property.serializedObject.FindProperty(cdiAttribute.ConditionalSourceField);
        if (sourcePropertyValue != null)
        {
            enabled = CheckPropertyType(sourcePropertyValue);
        }
        else
        {
            Debug.LogWarning("Attempting to use a ConditionalDisableInspectorAttribute but no matching SourcePropertyValue found in object: " + cdiAttribute.ConditionalSourceField);
        }

        SerializedProperty sourcePropertyValue2 = property.serializedObject.FindProperty(cdiAttribute.ConditionalSourceField2);
        if (sourcePropertyValue2 != null)
        {
            enabled = enabled && CheckPropertyType(sourcePropertyValue2);
        }
        else
        {
            //Debug.LogWarning("Attempting to use a ConditionalDisableInspectorAttribute but no matching SourcePropertyValue found in object: " + cdiAttribute.ConditionalSourceField);
        }

        if (cdiAttribute.Inverse) enabled = !enabled;

        return enabled;
    }

    private bool CheckPropertyType(SerializedProperty sourcePropertyValue)
    {
        switch (sourcePropertyValue.propertyType)
        {
            case SerializedPropertyType.Boolean:
                return sourcePropertyValue.boolValue;
            case SerializedPropertyType.ObjectReference:
                return sourcePropertyValue.objectReferenceValue != null;
            default:
                Debug.LogError("Data type of the property used for conditional hiding [" + sourcePropertyValue.propertyType + "] is currently not supported");
                return true;
        }
    }
}
