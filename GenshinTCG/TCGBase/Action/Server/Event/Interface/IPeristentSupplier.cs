﻿namespace TCGBase
{
    /// <summary>
    /// 提供给targetrecord使用
    /// </summary>
    internal interface IPeristentSupplier
    {
        public Persistent Persistent { get; }
    }
}