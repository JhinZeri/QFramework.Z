using Sirenix.OdinInspector;
using UnityEngine;

namespace 迭代测试过程文件夹.说明工具脚本
{
    public class InspectorDescription : MonoBehaviour
    {
        [Title("说明文本", bold: false)]
        [HideLabel]
        [MultiLineProperty(7)]
        public string TestDescription;
    }
}