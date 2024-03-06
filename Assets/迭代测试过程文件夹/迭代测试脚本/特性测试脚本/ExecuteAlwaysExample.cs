using UnityEngine;
using 迭代测试过程文件夹.说明工具脚本;

namespace 迭代测试过程文件夹.迭代测试脚本.特性测试脚本
{
    [AddDescriptionButton]
    [ExecuteAlways]
    public class ExecuteAlwaysExample : MonoBehaviour
    {
        void Update()
        {
            Debug.Log("Update method called always.");
        }
    }
}