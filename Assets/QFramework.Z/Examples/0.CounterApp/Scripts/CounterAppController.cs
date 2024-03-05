using QFramework.Z.Framework.Core;
using QFramework.Z.Framework.EventSystemIntegration;
using QFramework.Z.Framework.Observable;
using QFramework.Z.Framework.Rule;
using UnityEngine;
using UnityEngine.UI;

namespace QFramework.Z.Examples._0.CounterApp
{
    // 1. 定义一个 Model 对象
    public class CounterAppModel : AbstractModel
    {
        public BindableProperty<int> Count { get; } = new();

        protected override void OnInit()
        {
            var storage = this.GetUtility<EasySaveStorage>();

            // 设置初始值（不触发事件）
            Count.SetValueWithoutEvent(storage.LoadInt(nameof(Count)));

            // 当数据变更时 存储数据
            Count.Register(newCount =>
            {
                storage.SaveInt(nameof(Count), newCount);
            });
        }
    }


    public class AchievementSystem : AbstractSystem
    {
        protected override void OnInit()
        {
            this.GetModel<CounterAppModel>() // -+
                .Count
                .Register(newCount =>
                {
                    if (newCount == 10)
                        Debug.Log("触发 点击达人 成就");
                    else if (newCount == 20)
                        Debug.Log("触发 点击专家 成就");
                    else if (newCount == -10) Debug.Log("触发 点击菜鸟 成就");
                });
        }
    }

    public class EasySaveStorage : IUtility
    {
        public void SaveInt(string key, int value)
        {
            PlayerPrefs.SetInt(key, value);
        }

        public int LoadInt(string key, int defaultValue = 0) => PlayerPrefs.GetInt(key, defaultValue);
    }
    
    // 引入 Command
    public class IncreaseCountCommand : AbstractCommand
    {
        protected override void OnExecute()
        {
            var model = this.GetModel<CounterAppModel>();

            model.Count.Value++;
        }
    }

    public class DecreaseCountCommand : AbstractCommand
    {
        protected override void OnExecute()
        {
            this.GetModel<CounterAppModel>().Count.Value--;
        }
    }

    // Controller
    public class CounterAppController : AbstractController /* 3.实现 IController 接口 */
    {
        // View
        Button mBtnAdd;
        Button mBtnSub;
        Text mCountText;

        // 4. Model
        CounterAppModel mModel;

        void Start()
        {
            // 5. 获取模型
            mModel = this.GetModel<CounterAppModel>();

            // View 组件获取
            mBtnAdd = transform.Find("BtnAdd").GetComponent<Button>();
            mBtnSub = transform.Find("BtnSub").GetComponent<Button>();
            mCountText = transform.Find("CountText").GetComponent<Text>();


            // 监听输入
            mBtnAdd.onClick.AddListener(this.SendCommand<IncreaseCountCommand>);

            mBtnSub.onClick.AddListener(() =>
            {
                // 交互逻辑
                this.SendCommand(new DecreaseCountCommand( /* 这里可以传参（如果有） */));
            });

            // 表现逻辑
            mModel.Count.RegisterWithInitValue(newCount => // -+
                  {
                      UpdateView();
                  })
                  .UnRegisterWhenGameObjectDestroyed(gameObject);
        }

        void OnDestroy()
        {
            // 8. 将 Model 设置为空
            mModel = null;
        }

        void UpdateView()
        {
            mCountText.text = mModel.Count.ToString();
        }

        // 3.
        protected override IArchitecture SetControllerArchitecture() => CounterApp.Interface;
    }
}