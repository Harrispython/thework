namespace echo17.EndlessBook.Demo02
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using echo17.EndlessBook;
    using static echo17.EndlessBook.Demo02.PageView_02;

    /// <summary>
    /// Table of contents page.
    /// Handles clicks on the chapters to jump to pages in the book
    /// </summary>
    public class PageView_02 : PageView
    {
        /// <summary>
        /// The name of the collider and what page number
        /// it is associated with
        /// </summary>
        [Serializable]
        public struct ChapterJump
        {
            public string gameObjectName;
            public int pageNumber;
        }
        public EndlessBook book;
        public Demo02 Demo02;
        public ChapterJump[] chapterJumps;

        protected override bool HandleHit(RaycastHit hit, BookActionDelegate action)
        {
            // no action, just return
            Debug.Log("Enter");
            if (action == null) return false;

            if (hit.collider.gameObject.name == "LastPage")
            {
                action(BookActionTypeEnum.TurnPage,book.LastPageNumber);
                return true;
            }
            if (hit.collider.gameObject.name == "Add")
            {
                //if (book&&Demo02)
                //{
                //    book.InsertPageData(book.CurrentLeftPageNumber, Materals[0]);
                //    book.InsertPageData(book.CurrentLeftPageNumber, Materals[0]);
                //    Demo02.pageViews[0].gameObject.name = $"PageView_{book.LastPageNumber - 1}";
                //}
                BagSystem.instance.transform.parent.gameObject.SetActive(true);//Active BagCavans
                TouchPad.instance.SetTouchMask(false);//close TouchMask
                return true;
            }

            // check each collider and jump to a page if that collider was hit.

            foreach (var chapterJump in chapterJumps)
            {
                if (chapterJump.gameObjectName == hit.collider.gameObject.name)
                {
                    action(BookActionTypeEnum.TurnPage, chapterJump.pageNumber);
                    return true;
                }
            }

            return false;
        }
    }
}
