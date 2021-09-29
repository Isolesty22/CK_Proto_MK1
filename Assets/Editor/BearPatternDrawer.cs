using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

//[CustomPropertyDrawer(typeof(BearPattern))]
//public class BearPatternDrawer : PropertyDrawer
//{
//    BearPattern bearPattern = new BearPattern();
//    SerializedProperty waitTime, state;

//    string displayName;
//    bool cache = false;

//    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
//    {
//        //데이터 캐시
//        if (!cache)
//        {
//            displayName = property.displayName;

//            property.Next(true);
//            waitTime = property.Copy();
//            property.Next(true);
//            state = property.Copy();

//            cache = true;
//        }

//        Rect contentPos = EditorGUI.PrefixLabel(position, new GUIContent());

//        if (position.height > 16f)
//        {
//            position.height = 16f;
//           // EditorGUI.indentLevel += 1;
//            contentPos = EditorGUI.IndentedRect(position);
//        }

//        contentPos.y += 4f;
//        float contentWidth = contentPos.width;

//        GUI.skin.label.padding = new RectOffset(3, 3, 6, 6);

//        EditorGUIUtility.labelWidth = 14f;
//        contentPos.width = contentWidth * 0.1f;
//        EditorGUI.indentLevel = 0;


//        EditorGUI.BeginProperty(contentPos, label, waitTime);
//        {
//            EditorGUI.BeginChangeCheck();
//            bearPattern.waitTime = EditorGUI.FloatField(contentPos, bearPattern.waitTime);
//            if (EditorGUI.EndChangeCheck())
//            {
//                waitTime.floatValue = bearPattern.waitTime;
//            }
//        }

//        EditorGUI.EndProperty();

//        contentPos.width = contentWidth - contentPos.width;
//        contentPos.x += (contentWidth * 0.1f) + 5f;
//        EditorGUI.BeginProperty(contentPos, label, state);
//        {
//            EditorGUI.BeginChangeCheck();
//            bearPattern.state = (eBossState)EditorGUI.EnumPopup(contentPos, bearPattern.state);
//            if (EditorGUI.EndChangeCheck())
//            {
//                state.enumValueIndex = (int)bearPattern.state;
//            }
//        }
//        EditorGUI.EndProperty();
//    }
//    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
//    {
//        return 25f;//Screen.width < 333 ? (16f + 18f) : 16f;
//    }
//}

