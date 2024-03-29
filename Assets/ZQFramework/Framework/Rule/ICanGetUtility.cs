﻿using ZQFramework.Framework.Core;

namespace ZQFramework.Framework.Rule
{
    public interface ICanGetUtility : IBelongToArchitecture { }

    public static class CanGetUtilityExtension
    {
        public static T GetUtility<T>(this ICanGetUtility self) where T : class, IUtility =>
            self.GetArchitecture().GetUtility<T>();
    }
}