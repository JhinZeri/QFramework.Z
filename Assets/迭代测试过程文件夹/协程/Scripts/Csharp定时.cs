using System;
using System.Collections;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

namespace 迭代测试过程文件夹.协程.Scripts
{
    public class Csharp定时 : MonoBehaviour
    {
        void Start()
        {
            var source = DelayedPrint(1000, () => Debug.Log("Finished!"));
            // 1秒后取消任务
            source.Cancel();
        }

        CancellationTokenSource DelayedPrint(int time, Action finish)
        {
            CancellationTokenSource source = new CancellationTokenSource();
            InternalDelay(time, source.Token, finish);
            return source;
        }

        async void InternalDelay(int time, CancellationToken token, Action finish)
        {
            try
            {
                await Task.Delay(time, token);
                finish?.Invoke();
            }
            catch (TaskCanceledException e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }
}