using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

//public class PopUp : MonoBehaviour
//{
//    private int numPop = 0;
//    public Main mainScript;

//    public event System.Action<int> OnResult;
//    public void GetNum(int a)
//    {
//        numPop = a;
//    }

//    public void add()
//    {
//        numPop += 100;
//        PublishResult(numPop);
//    }

//    public void minus()
//    {
//        numPop -= 100;
//        PublishResult(numPop);
//    }

//    void PublishResult(int value)
//    {
//        OnResult?.Invoke(value);
//    }

//}

public class PopUpTemp : MonoBehaviour
{
    private int numPop = 0;
    private System.Action<int> onResultCallback;
    public void GetNum(int number, System.Action<int> callback)
    {
        numPop = number;
        this.onResultCallback = callback;
    }

    public void Add()
    {
        numPop += 100;
        ShowResult(numPop);
    }

    public void Minus()
    {
        numPop -= 100;
        ShowResult(numPop);
    }

    void ShowResult(int numPop)
    {
        onResultCallback?.Invoke(numPop);
    }
}
