using System;
using System.Collections;
using UnityEngine;

namespace 迭代测试过程文件夹.协程.Scripts
{
    public class 第一次协程 : MonoBehaviour
    {
        void Start()
        {
            StartCoroutine(Call());
        }

        IEnumerator Call()
        {
            print("Hello, World!");
            yield return new WaitForFixedUpdate();
            print("Call2!");
        }
    }
}