using System;
using UnityEditor;
using UnityEngine;

namespace Simplei18n
{
    public static class EditorWindowHelper
    {
        public static void DebugShowWindowBounds(Rect position)
        {
            EditorGUILayout.LabelField("Debug Bounds:", EditorStyles.boldLabel);
            EditorGUILayout.LabelField(string.Format("Position ({0}, {1})", position.x, position.y));
            EditorGUILayout.LabelField(string.Format("Width: {0}. Height: {1}", position.width, position.height));
            DrawUILine(Color.grey);
        }
    
        public static GUIContent IconContent(string name, string tooltip)
        {
            var builtinIcon = EditorGUIUtility.IconContent (name);
            return new GUIContent(builtinIcon.image, tooltip);
        }
        
        public static void DrawUILine(Color color, int thickness = 2, int padding = 10)
        {
            var r = EditorGUILayout.GetControlRect(GUILayout.Height(padding + thickness));
            r.height = thickness;
            r.y+= (float)padding / 2;
            r.x-= 2;
            r.width += 6;
            EditorGUI.DrawRect(r, color);
        }

        public static Rect HorizontalLayout(Action action)
        {
            Rect rect = EditorGUILayout.BeginHorizontal();
            action.Invoke();
            EditorGUILayout.EndHorizontal();

            return rect;
        }

        public static bool DetectKey(KeyCode keycode)
        {
            Event e = Event.current;
            switch (e.type)
            {
                case EventType.KeyDown:
                {
                    if (Event.current.keyCode == keycode)
                    {
                        e.Use();
                        return true;
                    }
                    break;
                }
            }

            return false;
        }
    }
}