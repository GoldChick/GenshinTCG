﻿namespace TCGCard
{
    public interface ICardBase
    {
        /// <summary>
        /// 卡牌的名字(a-z+_+0-9)
        /// </summary>
        public string Name { get; }
        /// <summary>
        /// 默认携带的各种tag<br/>
        /// 成员形如(minecraft:Nation,minecraft:Natlan)<br/>
        /// 或者(minecraft:)
        /// </summary>
        public HashSet<string> Tags { get; }
    }
    /// <summary>
    /// 可能会有用的客户端服务端分离？
    /// </summary>
    public interface ICardServer : ICardBase
    {

    }
}
