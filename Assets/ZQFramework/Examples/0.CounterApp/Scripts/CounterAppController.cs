using UnityEngine;
using UnityEngine.UI;
using ZQFramework.Framework.Core;
using ZQFramework.Framework.EventSystemIntegration;
using ZQFramework.Framework.Observable;
using ZQFramework.Framework.Rule;

namespace ZQFramework.Examples._0.CounterApp.Scripts
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

    public class DebugAEvent { }

    // Controller
    public class CounterAppController : MonoBehaviour, IController /* 3.实现 IController 接口 */
    {
        // View
        Button m_BtnAdd;
        Button m_BtnSub;
        Text m_CountText;

        // 4. Model
        CounterAppModel m_Model;

        void Start()
        {
            // 5. 获取模型
            m_Model = this.GetModel<CounterAppModel>();


            this.RegisterEvent<DebugAEvent>(_ =>
            {
                Debug.Log("Debug A");
            });

            // View 组件获取
            m_BtnAdd = transform.Find("BtnAdd").GetComponent<Button>();
            m_BtnSub = transform.Find("BtnSub").GetComponent<Button>();
            m_CountText = transform.Find("CountText").GetComponent<Text>();

            // 监听输入
            m_BtnAdd.onClick.AddListener(this.SendCommand<IncreaseCountCommand>);

            m_BtnSub.onClick.AddListener(() =>
            {
                // 交互逻辑
                this.SendCommand(new DecreaseCountCommand( /* 这里可以传参（如果有） */));
            });

            // 表现逻辑
            m_Model.Count.RegisterWithInitValue(_ => // -+
                   {
                       UpdateView();
                   })
                   .UnRegisterWhenGameObjectDestroyed(gameObject);
        }

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Q)) GetArchitecture().SendEvent<DebugAEvent>();
        }

        void OnDestroy()
        {
            // 8. 将 Model 设置为空
            m_Model = null;
        }

        // 3.

        public IArchitecture GetArchitecture() => Architecture.CounterApp.Interface;

        void UpdateView()
        {
            m_CountText.text = m_Model.Count.ToString();
        }
    }
}