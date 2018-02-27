using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace SSSTools.TriggleDesign
{
    /// <summary>
    /// 触发器编辑器通用枚举选择框
    /// </summary>
    public class SSSTriggerEnumPopup : EditorWindow
    {
        private static SSSTriggerEnumPopup window;
        private string searchEnum = "";
        private string searchName = "";
        private bool isSearchMode = false;
        private List<string> searchResult = new List<string>();
        private static Vector2 minResolution = new Vector2(440, 1000);
        private Vector2 scroll = Vector2.zero;
        private static IEnumSelecter enumSelecter;
        private static EditorWindow parentWindow;
        private static int order;

        public static void PopUp(EditorWindow parentWindow, int order, IEnumSelecter enumSelecter)
        {
            window = EditorWindow.GetWindow(typeof(SSSTriggerEnumPopup), true, "参数选择栏") as SSSTriggerEnumPopup;
            window.minSize = minResolution;
            window.Show();
            SSSTriggerEnumPopup.enumSelecter = enumSelecter;
            SSSTriggerEnumPopup.parentWindow = parentWindow;
            SSSTriggerEnumPopup.order = order;
        }

        private void OnGUI()
        {
            ShowEditorGUI();
            ShowEnumsGUI();
        }

        /// <summary>
        /// 绘制编辑器UI
        /// </summary>
        private void ShowEditorGUI()
        {
            GUILayout.Space(12);

            GUILayout.BeginHorizontal();
            if (GUILayout.Button("Enum搜索", GUILayout.Width(80)))
            {
                isSearchMode = true;
                SearchEnum();
            }
            searchEnum = EditorGUILayout.TextField(searchEnum, GUILayout.Width(100));
            if (GUILayout.Button("清空搜索", GUILayout.Width(80)))
            {
                isSearchMode = false;
            }
            GUILayout.EndHorizontal();
        }

        /// <summary>
        /// 绘制枚举ScrollView
        /// </summary>
        private void ShowEnumsGUI()
        {
            scroll = EditorGUILayout.BeginScrollView(scroll, "box");
            int index = 0;
            //for (int i = 0; i < triggleType.Count(); i++)
            //{
            //    GUI.color = index % 2 == 1 ? Color.green : Color.white;
            //    if (isSearchMode && !searchResult.Contains(triggleType.idList[i]))
            //    {
            //        continue;
            //    }
            //    string itemDes = string.Format("枚举:{0}  类型:{1}  描述:{2}", triggleType.idList[i], triggleType.nameList[i], triggleType.descList[i]);
            //    if (GUILayout.Button(itemDes, EditorStyles.largeLabel))
            //    {
            //        enumSelecter.EnumSelectCallback(i, order);
            //        parentWindow.Focus();
            //        window.Close();
            //    }
            //    index++;
            //}
            //GUI.color = Color.white;

            EditorGUILayout.EndScrollView();
        }

        private void SearchEnum()
        {
            //if (string.IsNullOrEmpty(searchEnum))
            //{
            //    searchResult.Clear();
            //    isSearchMode = false;
            //}

            //searchResult.Clear();
            //for (int i = 0; i < triggleType.idList.Count; i++)
            //{
            //    string enumID = triggleType.idList[i];
            //    if (enumID.Contains(searchEnum))
            //    {
            //        searchResult.Add(enumID);
            //    }
            //}
        }
    }

    /// <summary>
    /// 选中选项后的回调
    /// </summary>
    public interface IEnumSelecter
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="selectIndex"></param>选中了哪个选项
        /// <param name="order"></param>是当前页面中的第几个EnumPop
        void EnumSelectCallback(int selectIndex, int order);
    }

}
