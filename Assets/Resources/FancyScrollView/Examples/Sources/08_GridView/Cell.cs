/*
 * FancyScrollView (https://github.com/setchi/FancyScrollView)
 * Copyright (c) 2020 setchi
 * Licensed under MIT (https://github.com/setchi/FancyScrollView/blob/master/LICENSE)
 */

using UnityEngine;
using UnityEngine.UI;

namespace FancyScrollView.Example08
{
    class Cell : FancyGridViewCell<ItemData, Context>
    {
        [SerializeField] Text message = default;
        [SerializeField] Image image = default;
        [SerializeField] Button button = default;
        [SerializeField] BagSystem.ItemMessage itemMessage= default;

        public override void Initialize()
        {
            button.onClick.AddListener(() => Context.OnCellClicked?.Invoke(Index));
            button.onClick.AddListener(()=>BagSystem.instance.SetDescripiton(itemMessage));
        }

        public override void UpdateContent(ItemData itemData)
        {
            message.text = itemData.Index.ToString();
            image.sprite = itemData.itemMessage.Image;
            itemMessage = itemData.itemMessage;

            var selected = Context.SelectedIndex == Index;//是否选中
            //image.color = selected
            //    ? new Color32(0, 255, 255, 100)
            //    : new Color32(255, 255, 255, 77);
        }

        protected override void UpdatePosition(float normalizedPosition, float localPosition)
        {
            base.UpdatePosition(normalizedPosition, localPosition);

            var wave = Mathf.Sin(normalizedPosition * Mathf.PI * 2) * 65;
            transform.localPosition += Vector3.right * wave;
        }
    }
}
