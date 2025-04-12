using UnityEngine;
using echo17.EndlessBook;

public class PageTurnHandler : MonoBehaviour
{
    public EndlessBook endlessBook;

    void Start()
    {
        // 确保EndlessBook组件已分配
        if (endlessBook == null)
        {
            endlessBook = GetComponent<EndlessBook>();
        }

        // 绑定页面翻动事件
        if (endlessBook != null)
        {
            // 绑定到TurnForward和TurnBackward方法
            endlessBook.TurnForward(0.5f,
                onPageTurnStart: OnPageTurnStart,
                onPageTurnEnd: OnPageTurnEnd);

            endlessBook.TurnBackward(0.5f,
                onPageTurnStart: OnPageTurnStart,
                onPageTurnEnd: OnPageTurnEnd);
        }
    }

    // 页面开始翻动时的处理
    private void OnPageTurnStart(Page page, int pageNumberFront, int pageNumberBack,
        int pageNumberFirstVisible, int pageNumberLastVisible, Page.TurnDirectionEnum turnDirection)
    {
        Debug.Log($"页面开始翻动: 从 {pageNumberFront} 到 {pageNumberBack}");
    }

    // 页面翻动结束时的处理
    private void OnPageTurnEnd(Page page, int pageNumberFront, int pageNumberBack,
        int pageNumberFirstVisible, int pageNumberLastVisible, Page.TurnDirectionEnum turnDirection)
    {
        // 输出当前页数
        Debug.Log($"当前页数: {pageNumberBack}");
        
        // 你也可以在这里添加其他逻辑
        // 例如：
        // - 更新UI显示
        // - 加载新页面的内容
        // - 播放音效
    }

    // 向前翻页
    public void TurnPageForward()
    {
        if (endlessBook != null)
        {
            endlessBook.TurnForward(0.5f,
                onPageTurnStart: OnPageTurnStart,
                onPageTurnEnd: OnPageTurnEnd);
        }
    }

    // 向后翻页
    public void TurnPageBackward()
    {
        if (endlessBook != null)
        {
            endlessBook.TurnBackward(0.5f,
                onPageTurnStart: OnPageTurnStart,
                onPageTurnEnd: OnPageTurnEnd);
        }
    }
} 