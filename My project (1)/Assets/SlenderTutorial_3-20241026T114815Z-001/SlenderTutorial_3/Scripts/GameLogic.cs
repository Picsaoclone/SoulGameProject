using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;  // Thêm thư viện TextMeshPro

public class GameLogic : MonoBehaviour
{
    public GameObject counter;

    public int pageCount;


    void Start()
    {
        pageCount = 0;
        
    }



    void Update()
    {
        counter.GetComponent<TextMeshProUGUI>().text = pageCount + "/8";


    }
}
