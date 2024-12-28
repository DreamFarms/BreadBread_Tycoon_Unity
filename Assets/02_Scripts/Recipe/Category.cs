using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Category : MonoBehaviour
{
    public List<RecipeBookPage> pages = new List<RecipeBookPage>();
    private int _currentPgae;

    private void Awake()
    {
        _currentPgae = 0;
    }

    public void InitPage()
    {
        for (int i = pages.Count-1; i > 1; i--) 
        {
            pages[i].gameObject.SetActive(false);
        }
        pages[0].gameObject.SetActive(true);

    }

    public void AddPages(RecipeBookPage page)
    {
        pages.Add(page);
    }

    public void NextPage()
    {
        pages[_currentPgae].gameObject.SetActive(false);

        _currentPgae++;
        if(_currentPgae >= pages.Count)
        {
            _currentPgae = 0;
        }

        pages[_currentPgae].gameObject.SetActive(true);
    }
}
