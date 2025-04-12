using UnityEngine;
using echo17.EndlessBook;

public class PageTurnExample : MonoBehaviour
{
    public EndlessBook endlessBook;
    private PageTurnHandler pageTurnHandler;

    void Start()
    {
        // 确保EndlessBook组件已分配
        if (endlessBook == null)
        {
            endlessBook = GetComponent<EndlessBook>();
        }

        // 添加页面翻动处理器
        pageTurnHandler = gameObject.AddComponent<PageTurnHandler>();
        pageTurnHandler.endlessBook = endlessBook;

        // 示例：在页面翻动结束时执行特定操作
        endlessBook.TurnToPage(1, EndlessBook.PageTurnTimeTypeEnum.TimePerPage, 0.5f,
            onPageTurnEnd: OnPageTurnEnd);
    }

    // 页面翻动结束时的处理
    private void OnPageTurnEnd(Page page, int pageNumberFront, int pageNumberBack,
        int pageNumberFirstVisible, int pageNumberLastVisible, Page.TurnDirectionEnum turnDirection)
    {
        Debug.Log($"页面翻动结束: 从 {pageNumberFront} 到 {pageNumberBack}");
        
        // 示例：在页面翻动结束时执行的操作
        // 1. 更新UI显示
        UpdatePageNumberUI(pageNumberBack);
        
        // 2. 播放翻页音效
        PlayPageTurnSound();
        
        // 3. 加载新页面的内容
        LoadPageContent(pageNumberBack);
    }

    // 示例方法：更新UI显示
    private void UpdatePageNumberUI(int pageNumber)
    {
        Debug.Log($"更新UI显示: 当前页面 {pageNumber}");
        // 在这里添加更新UI的逻辑
    }

    // 示例方法：播放翻页音效
    private void PlayPageTurnSound()
    {
        Debug.Log("播放翻页音效");
        // 在这里添加播放音效的逻辑
    }

    // 示例方法：加载页面内容
    private void LoadPageContent(int pageNumber)
    {
        Debug.Log($"加载页面内容: {pageNumber}");
        // 在这里添加加载页面内容的逻辑
    }

    // 示例：向前翻页
    public void TurnPageForward()
    {
        if (endlessBook != null)
        {
            endlessBook.TurnForward(0.5f,
                onPageTurnEnd: OnPageTurnEnd);
        }
    }

    // 示例：向后翻页
    public void TurnPageBackward()
    {
        if (endlessBook != null)
        {
            endlessBook.TurnBackward(0.5f,
                onPageTurnEnd: OnPageTurnEnd);
        }
    }
} 