#if UNITY_EDITOR
using Sirenix.OdinInspector;
using UnityEngine;

namespace 迭代测试过程文件夹.说明工具脚本
{
    public class InspectorDescription : MonoBehaviour
    {
        [Title("说明脚本的名称")]
        public string TestName;

        [Title("说明文本", bold: false)]
        [HideLabel]
        [MultiLineProperty(7)]
        public string TestDescription;

        // [Button("添加到全局信息存档")]
        // public void Add()
        // {
        //     foreach (var data in DescriptionSO.Instance.Descriptions)
        //     {
        //         if (data.Name == TestName)
        //         {
        //             DescriptionSO.Instance.Descriptions.Remove(data);
        //             break;
        //         }
        //     }
        //
        //     DescriptionSO.Instance.Descriptions.Add(new DescriptionData
        //     {
        //         Name = TestName,
        //         Description = TestDescription
        //     });
        // }
    }
}
#endif