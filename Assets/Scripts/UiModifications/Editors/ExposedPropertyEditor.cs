using Assets.Scripts.UiModifications.Attributes;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace Assets.Scripts.UiModifications.Editors
{
#if UNITY_EDITOR
    /// <summary>
    /// Custom editor for displaying and editing properties with the <see cref="ExposePropertyAttribute"/> attribute in the Unity Inspector.
    /// </summary>
    [CustomEditor(typeof(MonoBehaviour), true)]
    public class ExposedPropertyDrawer : Editor
    {
        /// <summary>
        /// Draws the inspector GUI for the target object, including properties with the <see cref="ExposePropertyAttribute"/> attribute.
        /// </summary>
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            // Get the type of the target object
            var type = target.GetType();

            // Get all properties of the target object
            var properties = type.GetProperties(BindingFlags.Public | BindingFlags.Instance);

            foreach (var property in properties)
            {
                // Check if the property has the ExposeProperty attribute
                if (property.IsDefined(typeof(ExposePropertyAttribute), true))
                {
                    var propertyType = property.PropertyType;

                    // Handle different types
                    if (propertyType == typeof(float))
                    {
                        DisplayFloatProperty(property);
                    }
                    else if (propertyType == typeof(int))
                    {
                        DisplayIntProperty(property);
                    }
                    else if (propertyType == typeof(string))
                    {
                        DisplayStringProperty(property);
                    }
                    else if (propertyType == typeof(bool))
                    {
                        DisplayBoolProperty(property);
                    }
                    else if (propertyType.IsEnum)
                    {
                        DisplayEnumProperty(property);
                    }
                    else if (typeof(IList).IsAssignableFrom(propertyType) && propertyType.IsGenericType)
                    {
                        DisplayListProperty(property);
                    }
                    else if (propertyType.IsArray)
                    {
                        DisplayArrayProperty(property);
                    }
                    else
                    {
                        EditorGUILayout.LabelField(property.Name, $"Type {propertyType.Name} not supported.");
                    }
                }
            }
        }

        /// <summary>
        /// Displays a float property in the inspector and allows it to be edited.
        /// </summary>
        /// <param name="property">The property to display.</param>
        private void DisplayFloatProperty(PropertyInfo property)
        {
            float currentValue = (float)property.GetValue(target, null);
            float newValue = EditorGUILayout.FloatField(property.Name, currentValue);
            if (newValue != currentValue)
            {
                property.SetValue(target, newValue, null);
            }
        }

        /// <summary>
        /// Displays an integer property in the inspector and allows it to be edited.
        /// </summary>
        /// <param name="property">The property to display.</param>
        private void DisplayIntProperty(PropertyInfo property)
        {
            int currentValue = (int)property.GetValue(target, null);
            int newValue = EditorGUILayout.IntField(property.Name, currentValue);
            if (newValue != currentValue)
            {
                property.SetValue(target, newValue, null);
            }
        }

        /// <summary>
        /// Displays a string property in the inspector and allows it to be edited.
        /// </summary>
        /// <param name="property">The property to display.</param>
        private void DisplayStringProperty(PropertyInfo property)
        {
            string currentValue = (string)property.GetValue(target, null);
            string newValue = EditorGUILayout.TextField(property.Name, currentValue);
            if (newValue != currentValue)
            {
                property.SetValue(target, newValue, null);
            }
        }

        /// <summary>
        /// Displays a boolean property in the inspector and allows it to be edited.
        /// </summary>
        /// <param name="property">The property to display.</param>
        private void DisplayBoolProperty(PropertyInfo property)
        {
            bool currentValue = (bool)property.GetValue(target, null);
            bool newValue = EditorGUILayout.Toggle(property.Name, currentValue);
            if (newValue != currentValue)
            {
                property.SetValue(target, newValue, null);
            }
        }

        /// <summary>
        /// Displays an enum property in the inspector and allows it to be edited.
        /// </summary>
        /// <param name="property">The property to display.</param>
        private void DisplayEnumProperty(PropertyInfo property)
        {
            Enum currentValue = (Enum)property.GetValue(target, null);
            Enum newValue = EditorGUILayout.EnumPopup(property.Name, currentValue);
            if (!newValue.Equals(currentValue))
            {
                property.SetValue(target, newValue, null);
            }
        }

        /// <summary>
        /// Displays a list property in the inspector and allows its size and elements to be edited.
        /// </summary>
        /// <param name="property">The property to display.</param>
        private void DisplayListProperty(PropertyInfo property)
        {
            IList list = (IList)property.GetValue(target, null);
            Type elementType = property.PropertyType.GetGenericArguments()[0];

            EditorGUILayout.LabelField(property.Name);

            if (list == null)
            {
                if (GUILayout.Button("Create List"))
                {
                    list = (IList)Activator.CreateInstance(typeof(List<>).MakeGenericType(elementType));
                    property.SetValue(target, list, null);
                }
                return;
            }

            EditorGUI.indentLevel++;
            int newCount = EditorGUILayout.IntField("Size", list.Count);
            if (newCount != list.Count)
            {
                IList newList = (IList)Activator.CreateInstance(typeof(List<>).MakeGenericType(elementType));
                for (int i = 0; i < newCount; i++)
                {
                    newList.Add(i < list.Count ? list[i] : CreateDefault(elementType));
                }
                list = newList;
                property.SetValue(target, list, null);
            }

            for (int i = 0; i < list.Count; i++)
            {
                object element = list[i];
                object newElement = DisplayField(elementType, $"Element {i}", element);
                if (!newElement.Equals(element))
                {
                    list[i] = newElement;
                    property.SetValue(target, list, null); // Update the property with the modified list
                }
            }

            EditorGUI.indentLevel--;
        }

        /// <summary>
        /// Displays an array property in the inspector and allows its size and elements to be edited.
        /// </summary>
        /// <param name="property">The property to display.</param>
        private void DisplayArrayProperty(PropertyInfo property)
        {
            Array array = (Array)property.GetValue(target, null);
            Type elementType = property.PropertyType.GetElementType();

            EditorGUILayout.LabelField(property.Name);

            if (array == null)
            {
                if (GUILayout.Button("Create Array"))
                {
                    array = Array.CreateInstance(elementType, 0);
                    property.SetValue(target, array, null);
                }
                return;
            }

            EditorGUI.indentLevel++;
            int newLength = EditorGUILayout.IntField("Size", array.Length);
            if (newLength != array.Length)
            {
                Array newArray = Array.CreateInstance(elementType, newLength);
                Array.Copy(array, newArray, Math.Min(array.Length, newArray.Length));
                array = newArray;
                property.SetValue(target, array, null);
            }

            for (int i = 0; i < array.Length; i++)
            {
                object element = array.GetValue(i);
                object newElement = DisplayField(elementType, $"Element {i}", element);
                if (!newElement.Equals(element))
                {
                    array.SetValue(newElement, i);
                    property.SetValue(target, array, null); // Update the property with the modified array
                }
            }
            EditorGUI.indentLevel--;
        }

        /// <summary>
        /// Displays a field in the inspector based on its type.
        /// </summary>
        /// <param name="type">The type of the field.</param>
        /// <param name="label">The label to display.</param>
        /// <param name="value">The current value of the field.</param>
        /// <returns>The new value of the field.</returns>
        private object DisplayField(Type type, string label, object value)
        {
            if (type == typeof(float))
            {
                return EditorGUILayout.FloatField(label, (float)value);
            }
            else if (type == typeof(int))
            {
                return EditorGUILayout.IntField(label, (int)value);
            }
            else if (type == typeof(string))
            {
                return EditorGUILayout.TextField(label, (string)value);
            }
            else if (type == typeof(bool))
            {
                return EditorGUILayout.Toggle(label, (bool)value);
            }
            else if (type.IsEnum)
            {
                return EditorGUILayout.EnumPopup(label, (Enum)value);
            }
            else
            {
                EditorGUILayout.LabelField(label, $"Type {type.Name} not supported.");
                return value;
            }
        }

        /// <summary>
        /// Creates a default value for a given type.
        /// </summary>
        /// <param name="type">The type for which to create the default value.</param>
        /// <returns>The default value for the specified type.</returns>
        private object CreateDefault(Type type)
        {
            if (type.IsValueType)
            {
                return Activator.CreateInstance(type);
            }
            else if (type == typeof(string))
            {
                return string.Empty;
            }
            else if (type.IsEnum)
            {
                return Enum.GetValues(type).GetValue(0);
            }
            return null;
        }
    }
#endif
}