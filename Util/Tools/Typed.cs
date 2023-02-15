using UnityEngine;
using System;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;
[System.AttributeUsage(AttributeTargets.Field)]
public class TypedAttribute : PropertyAttribute
{
    public Type FieldType;

    public TypedAttribute(Type fieldType = null)
    {
        FieldType = fieldType;
    }
}

#if UNITY_EDITOR
namespace UnityEditor.Custom
{
    [CustomPropertyDrawer(typeof(TypedAttribute))]
    public class TypedDrawer : PropertyDrawer
    {
        private Type[] types;

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return 20 + EditorGUI.GetPropertyHeight(property, true);
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            if (types == null)
            {

                var type = (attribute as TypedAttribute).FieldType ?? fieldInfo.FieldType;
                var list = new List<Type>(GetImplementations(type)
                    .Where(impl => !impl.IsSubclassOf(typeof(UnityEngine.Object))));
                list.Insert(0, null);
                types = list.ToArray();

            }

            var index = 0;
            if (property.managedReferenceValue != null)
            {
                index = System.Array.IndexOf(types, property.managedReferenceValue.GetType());
            }
            var height = position.height;
            position.height = 20;
            var newIndex = EditorGUI.Popup(position, label.text,
                index, types.Select(impl => impl == null ? "None" : impl.Name).ToArray());
            position.y += 20;
            position.height = height;

            if (newIndex != index)
                property.managedReferenceValue = types[newIndex] == null ? null : Activator.CreateInstance(types[newIndex]);



            // if (GUI.Button(position, "Create instance"))
            // {
            //     property.managedReferenceValue = Activator.CreateInstance(_implementations[_implementationTypeIndex]);
            // }
            EditorGUI.indentLevel++;
            EditorGUI.PropertyField(position, property, true);
            EditorGUI.indentLevel--;
        }

        public static Type[] GetImplementations(Type interfaceType)
        {
            var types = AppDomain.CurrentDomain.GetAssemblies().SelectMany(assembly => assembly.GetTypes());
            return types.Where(p => interfaceType.IsAssignableFrom(p) && !p.IsAbstract).ToArray();
        }
    }
}
#endif