/*
 * FancyScrollView (https://github.com/setchi/FancyScrollView)
 * Copyright (c) 2020 setchi
 * Licensed under MIT (https://github.com/setchi/FancyScrollView/blob/master/LICENSE)
 */
using UnityEngine;
namespace FancyScrollView.Example08
{
    class ItemData
    {
        public int Index { get; }
        public BagSystem.ItemMessage itemMessage;
        public ItemData(int index) => Index = index;
        public ItemData(BagSystem.ItemMessage message,int i)
        {
            itemMessage = message;
            Index = i;
        }
    }
}
