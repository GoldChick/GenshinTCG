using System;
using System.Collections.Generic;

namespace TCGBase
{
    public static class Dice
    {
        //TODO:随机数产生问题
        //不能一次产生多个
        public static ElementType GetRandomElementType()
        {
            Random rd = new();
            return (ElementType)rd.Next(0, 8);
        }
        /// <summary>
        /// 尽可能智能（当然不智能也没关系）选取所需的骰子
        /// 不会优先选取currType骰子
        /// <para></para>
        /// 优先选取普通骰子而非万能骰子
        /// sameDice==false则优先选取不同色骰子
        /// </summary>
        /// <param name="sameDice">控制need中的Trival骰子的要求</param>
        /// <param name="mine">传入的List需要已经排序过的(同种元素都排在一起)</param>
        /// <returns>如果返回为null证明失败,否则返回位置</returns>
        public static int[] AutoCheckExpense(List<ElementType> need, bool sameDice, List<ElementType> mine, ElementType currType)
        {
            if (need.Count > mine.Count)
            {
                return null;
            }
            int[] needArray = { 0, 0, 0, 0, 0, 0, 0, 0 };
            need.ForEach(ele => needArray[(int)ele]++);

            List<ElementType> leftMine = new(mine);

            List<int> list = new List<int>();

            //指定元素的选择
            for (int i = 1; i < 8; i++)
            {
                if (needArray[i] > 0)
                {
                    List<ElementType> mineList = mine.FindAll(ele => (int)ele == i);
                    int dif = mineList.Count - needArray[i];
                    if (dif >= 0)
                    {
                        for (int j = 0; j < needArray[i]; j++)
                        {
                            list.Add(j + mine.IndexOf((ElementType)i));
                            leftMine.Remove((ElementType)i);
                        }
                    }
                    else//如果不够，从最后一个万能骰开始
                    {
                        if (needArray[0] + dif < 0)//万能骰不够
                        {
                            return null;
                        }

                        for (int j = 0; j > dif; j--)
                        {
                            list.Add(j + mine.LastIndexOf(ElementType.Trival));
                            leftMine.Remove(0);
                        }
                    }
                }
            }

            //Trival元素的选择
            if (!sameDice)
            {
                //TODO
            }
            return list.ToArray();
        }
    }
}
