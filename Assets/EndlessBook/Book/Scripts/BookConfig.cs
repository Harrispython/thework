using UnityEngine;
using echo17.EndlessBook;

public class BookConfig : MonoBehaviour
{
    [Header("翻页配置")]
    [Tooltip("翻页动画时间（秒）")]
    public float pageTurnTime = 0.5f;
    
    [Tooltip("是否启用翻页音效")]
    public bool enablePageSound = true;
    
    [Tooltip("翻页音效")]
    public AudioClip pageTurnSound;
    
    [Header("页面内容配置")]
    [Tooltip("页面内容预制体")]
    public GameObject[] pageContentPrefabs;
    
    private EndlessBook endlessBook;
    private AudioSource audioSource;
    
    void Start()
    {
        // 获取组件
        endlessBook = GetComponent<EndlessBook>();
        audioSource = GetComponent<AudioSource>();
        
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
        
        // 配置翻页事件
        ConfigurePageEvents();
    }
    
    private void ConfigurePageEvents()
    {
        if (endlessBook != null)
        {
            // 绑定翻页事件
            endlessBook.TurnToPage(1, EndlessBook.PageTurnTimeTypeEnum.TimePerPage, pageTurnTime,
                onPageTurnStart: OnPageTurnStart,
                onPageTurnEnd: OnPageTurnEnd);
        }
    }
    
    private void OnPageTurnStart(Page page, int pageNumberFront, int pageNumberBack,
        int pageNumberFirstVisible, int pageNumberLastVisible, Page.TurnDirectionEnum turnDirection)
    {
        Debug.Log($"开始翻页: 从 {pageNumberFront} 到 {pageNumberBack}");
        
        // 播放翻页音效
        if (enablePageSound && pageTurnSound != null)
        {
            audioSource.PlayOneShot(pageTurnSound);
        }
    }
    
    private void OnPageTurnEnd(Page page, int pageNumberFront, int pageNumberBack,
        int pageNumberFirstVisible, int pageNumberLastVisible, Page.TurnDirectionEnum turnDirection)
    {
        Debug.Log($"翻页完成: 从 {pageNumberFront} 到 {pageNumberBack}");
        
        // 加载新页面内容
        LoadPageContent(pageNumberBack);
    }
    
    private void LoadPageContent(int pageNumber)
    {
        if (pageContentPrefabs != null && pageNumber <= pageContentPrefabs.Length)
        {
            // 销毁旧内容
            ClearPageContent();
            
            // 实例化新内容
            if (pageContentPrefabs[pageNumber - 1] != null)
            {
                Instantiate(pageContentPrefabs[pageNumber - 1], transform);
            }
        }
    }
    
    private void ClearPageContent()
    {
        // 销毁所有子物体
        foreach (Transform child in transform)
        {
            if (child.GetComponent<Page>() == null) // 保留Page组件
            {
                Destroy(child.gameObject);
            }
        }
    }
    
    // 公共方法供外部调用
    public void TurnPageForward()
    {
        if (endlessBook != null)
        {
            endlessBook.TurnForward(pageTurnTime,
                onPageTurnStart: OnPageTurnStart,
                onPageTurnEnd: OnPageTurnEnd);
        }
    }
    
    public void TurnPageBackward()
    {
        if (endlessBook != null)
        {
            endlessBook.TurnBackward(pageTurnTime,
                onPageTurnStart: OnPageTurnStart,
                onPageTurnEnd: OnPageTurnEnd);
        }
    }
    
    public void TurnToPage(int pageNumber)
    {
        if (endlessBook != null)
        {
            endlessBook.TurnToPage(pageNumber, EndlessBook.PageTurnTimeTypeEnum.TimePerPage, pageTurnTime,
                onPageTurnStart: OnPageTurnStart,
                onPageTurnEnd: OnPageTurnEnd);
        }
    }
} 