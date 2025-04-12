using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using echo17.EndlessBook;
public class BookContorl : MonoBehaviour
{
    public EndlessBook EndlessBook;
    // Start is called before the first frame update
    void Start()
    {

    }

    public void AddPage()
    {
        EndlessBook.AddPageData();
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
