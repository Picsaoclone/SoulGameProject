using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPageObserver
{
    void OnPageCollected(int pageCount);
}

