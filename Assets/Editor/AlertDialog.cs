using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace ColaFrame
{
    /// <summary>
    /// 编辑器中的弹出提示框
    /// </summary>
    public class AlertDialog : EditorWindow
    {
        private static AlertDialog window;
        private string alertText;
        private Color defaultColor;

        public static void PopUp(string alertText)
        {
            window = EditorWindow.GetWindow(typeof(AlertDialog), true, "提示框") as AlertDialog;
            window.position = new Rect(new Vector2(Screen.width / 2, Screen.height / 2), new Vector2(500, 100));
            window.SetText(alertText);
            window.Show();
            window.Focus();
        }

        private void SetText(string alertText)
        {
            this.alertText = alertText;
            defaultColor = GUI.color;
            GUI.color = Color.red;
        }

        private void OnGUI()
        {
            DrawEditorGUI();
        }

        private void DrawEditorGUI()
        {
            GUI.color = Color.red;
            GUILayout.Space(12);
            GUILayout.BeginHorizontal();
            EditorGUILayout.LabelField(alertText);
            GUILayout.EndHorizontal();
            GUI.color = defaultColor;
        }

        private void OnDestroy()
        {
            GUI.color = defaultColor;
        }

    }

}

