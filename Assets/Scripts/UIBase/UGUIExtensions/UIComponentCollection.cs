using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnityEngine.UI.Extensions
{
    /// <summary>
    /// ColaFramework框架，UI组件序列化存储脚本
    /// </summary>
    sealed public class UIComponentCollection : MonoBehaviour
    {
        [SerializeField]
        internal List<Component> components = new List<Component>();

        public T Get<T>(int index) where T : Component
        {
            return (T)components[index];
        }
    }
}

