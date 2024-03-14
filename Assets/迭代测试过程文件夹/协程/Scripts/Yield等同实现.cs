using System.Collections;
using UnityEngine;

namespace 迭代测试过程文件夹.协程.Scripts
{
    public class Yield等同实现 : MonoBehaviour
    {
        // Start is called before the first frame update
        void Start()
        {
            // 实现一个可以迭代的类
            var enumeratorTest = new IEnumeratorTest();

            foreach (object item in enumeratorTest)
            {
                Debug.Log(item);
            }
        }


        class IEnumeratorTest
        {
            public IEnumerator GetEnumerator()
            {
                yield return 1;
                yield return 2;
                yield return "枚举器";
            }
        }
    }
}