using UnityEngine;
using UnityEditor;
using Codice.CM.Client.Gui;
using System.Runtime.Remoting.Messaging;
using Unity.VisualScripting;

[CustomPropertyDrawer(typeof(GameSession<,>))]
public class SerializableDictionaryDrawer: PropertyDrawer
{
     public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
     {
               EditorGUI.BeginProperty(position, label, property);    
               SerializedProperty keysProp = property.FindPropertyRelative("keys");
               SerializedProperty valuesProp = property.FindPropertyRelative("values");

               int size = keysProp.arraySize;

               Rect headerRect = new Rect(position.x, position.y, position.width, EditorGUIUtility.singleLineHeight);

               EditorGUI.LabelField(new Rect(headerRect.x, headerRect.y, headerRect.width - 60, headerRect.height), label, EditorStyles.boldLabel);

               if(GUI.Button(new Rect(headerRect.x + headerRect.width - 55, headerRect.y, 25, headerRect.height), "+"))
               {
                    keysProp.InsertArrayElementAtIndex(size);
                    valuesProp.InsertArrayElementAtIndex(size);
               }

               if(GUI.Button(new Rect(headerRect.x + headerRect.width - 2, headerRect.y, 25, headerRect.height), "-") && size > 0)
          {
               keysProp.DeleteArrayElementAtIndex(size -1 );
               valuesProp.DeleteArrayElementAtIndex(size - 1);
          }

          EditorGUI.indentLevel++;
          float currentY = position.y + EditorGUIUtility.singleLineHeight + 4;

          //Draw Key value pair sid by side
          for(int i = 0; i < keysProp.arraySize; i++)
          {
               SerializedProperty key = keysProp.GetArrayElementAtIndex(i);
               SerializedProperty val = valuesProp.GetArrayElementAtIndex(i);

               float width = (position.width - 30)/2;
               Rect keyRect = new Rect(position.x, currentY, width, EditorGUIUtility.singleLineHeight);
               Rect valRect = new Rect(position.x + width + 10, currentY, width, EditorGUIUtility.singleLineHeight);

               EditorGUI.PropertyField(keyRect, key, GUIContent.none);
               EditorGUI.PropertyField(valRect, val, GUIContent.none);

               currentY += EditorGUIUtility.singleLineHeight + 2;
          }

          EditorGUI.indentLevel--;
          EditorGUI.EndProperty();
     }

     public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
     {
         SerializedProperty keysProp = property.FindPropertyRelative("keys");
         int lines = 1 + (keysProp != null ? keysProp.arraySize : 0);
         return lines * (EditorGUIUtility.singleLineHeight + 2) + 4;
     }
}
