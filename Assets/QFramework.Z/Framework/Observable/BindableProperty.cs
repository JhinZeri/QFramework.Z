using System;
using QFramework.Z.Framework.EventSystemIntegration;

namespace QFramework.Z.Framework.Observable
{
    // 定义了两个接口：IBindableProperty<T> 和 IReadonlyBindableProperty<T>，用于表示可绑定属性和只读可绑定属性。
    public interface IBindableProperty<T> : IReadonlyBindableProperty<T>
    {
        // 设置属性值但不触发值变化事件
        void SetValueWithoutEvent(T newValue);
    }

    public interface IReadonlyBindableProperty<T> : IEasyEvent
    {
        // 获取属性的值
        T Value { get; }

        // 注册属性值变化事件的回调函数，并立即执行回调函数传入当前属性值
        IUnRegister RegisterWithInitValue(Action<T> action);

        // 取消注册属性值变化事件的回调函数
        void UnRegister(Action<T> onValueChanged);

        // 注册属性值变化事件的回调函数
        IUnRegister Register(Action<T> onValueChanged);
    }

    /// <summary>
    /// 泛型类 BindableProperty，用于表示可绑定监测的属性。
    /// </summary>
    /// <typeparam name="T"> 被监测值的类型 </typeparam>
    public class BindableProperty<T> : IBindableProperty<T>
    {
        /* 用于处理值变化事件的 EasyEvent<T> 字段 */
        readonly EasyEvent<T> _onValueChanged = new();

        /* 存储属性的值 */
        protected T ObservableValue;

        /* 构造函数，初始化属性的默认值为 defaultValue */
        protected BindableProperty(T defaultValue = default)
        {
            ObservableValue = defaultValue;
        }

        /* 定义比较两个值是否相等的方法，默认使用 Equals 进行比较 */
        public static Func<T, T, bool> Comparer { get; set; } = (a, b) => a.Equals(b);

        /* 属性 Value，用于获取和设置属性的值 */
        public T Value
        {
            get => GetValue();
            set
            {
                if (value == null && ObservableValue == null) return;
                if (value != null && Comparer(value, ObservableValue)) return;

                SetValue(value);
                _onValueChanged.Trigger(value);
            }
        }

        /* 设置属性的值但不触发值变化事件 */
        public void SetValueWithoutEvent(T newValue)
        {
            ObservableValue = newValue;
        }

        /* 注册属性值变化事件的回调函数 */
        public IUnRegister Register(Action<T> onValueChanged) => _onValueChanged.Register(onValueChanged);

        /* 注册属性值变化事件的回调函数，并立即执行回调函数传入当前属性值 */
        public IUnRegister RegisterWithInitValue(Action<T> onValueChanged)
        {
            onValueChanged(ObservableValue);
            return Register(onValueChanged);
        }

        /* 取消注册属性值变化事件的回调函数 */
        public void UnRegister(Action<T> onValueChanged)
        {
            _onValueChanged.UnRegister(onValueChanged);
        }

        /* 实现接口方法，用于注册事件 */
        IUnRegister IEasyEvent.Register(Action onEvent)
        {
            return Register(Action);

            void Action(T _)
            {
                onEvent();
            }
        }

        /* 设置自定义的比较器 */
        public BindableProperty<T> WithComparer(Func<T, T, bool> comparer)
        {
            Comparer = comparer;
            return this;
        }

        /* 设置属性的值 */
        protected virtual void SetValue(T newValue)
        {
            ObservableValue = newValue;
        }

        /* 获取属性的值 */
        protected virtual T GetValue() => ObservableValue;

        /* 重写 ToString 方法，返回当前属性值的字符串表示形式 */
        public override string ToString() => Value.ToString();
    }


    /// <summary>
    /// 扩展 Comparer 规则
    /// </summary>
    internal static class ComparerAutoRegister
    {
#if UNITY_5_6_OR_NEWER
        [UnityEngine.RuntimeInitializeOnLoadMethod(UnityEngine.RuntimeInitializeLoadType.BeforeSceneLoad)]
        public static void AutoRegister()
        {
            BindableProperty<int>.Comparer = (a, b) => a == b;
            BindableProperty<float>.Comparer = (a, b) => Math.Abs(a - b) < float.Epsilon;
            BindableProperty<double>.Comparer = (a, b) => Math.Abs(a - b) < double.Epsilon;
            BindableProperty<string>.Comparer = (a, b) => a == b;
            BindableProperty<long>.Comparer = (a, b) => a == b;
            BindableProperty<UnityEngine.Vector2>.Comparer = (a, b) => a == b;
            BindableProperty<UnityEngine.Vector3>.Comparer = (a, b) => a == b;
            BindableProperty<UnityEngine.Vector4>.Comparer = (a, b) => a == b;
            BindableProperty<UnityEngine.Color>.Comparer = (a, b) => a == b;
            BindableProperty<UnityEngine.Color32>.Comparer =
                (a, b) => a.r == b.r && a.g == b.g && a.b == b.b && a.a == b.a;
            BindableProperty<UnityEngine.Bounds>.Comparer = (a, b) => a == b;
            BindableProperty<UnityEngine.Rect>.Comparer = (a, b) => a == b;
            BindableProperty<UnityEngine.Quaternion>.Comparer = (a, b) => a == b;
            BindableProperty<UnityEngine.Vector2Int>.Comparer = (a, b) => a == b;
            BindableProperty<UnityEngine.Vector3Int>.Comparer = (a, b) => a == b;
            BindableProperty<UnityEngine.BoundsInt>.Comparer = (a, b) => a == b;
            BindableProperty<UnityEngine.RangeInt>.Comparer = (a, b) => a.start == b.start && a.length == b.length;
            BindableProperty<UnityEngine.RectInt>.Comparer = (a, b) => a.Equals(b);
        }

#endif
    }
}