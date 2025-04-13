using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PageList : MonoBehaviour
{
    public List<BookAnimator> Pages;
    public static PageList instance;
    private void Start()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void AcitveList(Material PageMateral)
    {
        foreach(BookAnimator page in Pages)
        {
            page.Active(PageMateral);
        }
    }

}
