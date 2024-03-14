namespace 迭代测试过程文件夹.协程.Scripts
{
    // public class 使用Moroutine : MonoBehaviour
    // {
    //     // Start is called before the first frame update
    //     IEnumerator Start()
    //     {
    //         // 创造一个协程，但是不使用它
    //         // 它会在协程池中等待被调用
    //         // 它返回的是一个特殊类型，EasyCoroutine 
    //         var moroutine = EasyCoroutine.Create(TickEnumerator());
    //         moroutine.Run();
    //         yield return new WaitForSeconds(3.1f); // Wait 1 second
    //         moroutine.Stop();
    //         yield return new WaitForSeconds(1.5f);
    //         moroutine.Run();
    //     }
    //
    //     static IEnumerator Call()
    //     {
    //         print("Hello, World!");
    //         yield return new WaitForFixedUpdate();
    //         print("Call 2");
    //     }
    //
    //     static IEnumerator TickEnumerator()
    //     {
    //         while (true)
    //         {
    //             yield return new WaitForSeconds(1f);
    //             print("Tick!");
    //         }
    //     }
    // }
}