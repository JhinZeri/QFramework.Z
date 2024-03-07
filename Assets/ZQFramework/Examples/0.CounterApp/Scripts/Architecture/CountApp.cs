using UnityEngine;
using ZQFramework.Framework.Core;

namespace ZQFramework.Examples._0.CounterApp.Scripts.Architecture
{
    // 2.定义一个架构（提供 MVC、分层、模块管理等）
    public class CounterApp : Architecture<CounterApp>
    {
        protected override void Init()
        {
            // 注册 System 
            RegisterSystem<AchievementSystem>();

            // 注册 Model
            RegisterModel<CounterAppModel>();

            // 注册存储工具的对象
            RegisterUtility<EasySaveStorage>();
        }

        protected override void ExecuteCommand(ICommand command)
        {
            Debug.Log("Before " + command.GetType().Name + "Execute");
            base.ExecuteCommand(command);
            Debug.Log("After " + command.GetType().Name + "Execute");
        }

        protected override TResult ExecuteCommand<TResult>(ICommand<TResult> command)
        {
            Debug.Log("Before " + command.GetType().Name + "Execute");
            var result = base.ExecuteCommand(command);
            Debug.Log("After " + command.GetType().Name + "Execute");
            return result;
        }
    }
}