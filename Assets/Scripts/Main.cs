using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//public class Main : MonoBehaviour
//{
//    public int num = 42;
//    public Text numText;
//    public GameObject popupPanel;
//    public PopUp popUpScript;

//    public void Popup()
//    {
//        popupPanel.SetActive(true);
//        popUpScript.GetNum(num);
//        popUpScript.OnResult += ReceiveResult;
//    }
//    void ReceiveResult(int value)
//    {
//        num = value;
//        numText.text = num.ToString();
//        popupPanel.SetActive(false);
//    }
//}

public class Main : MonoBehaviour
{
    public int num = 42;
    public Text numText;
    public GameObject popupPanel;
    public PopUpTemp popUpScript;
    // Start is called before the first frame update
    public void Popup()
    {
        popupPanel.SetActive(true);
        popUpScript.GetNum(num,OnPopUpResult);
    }

    private void OnPopUpResult(int finalValue)
    {
        num = finalValue;
        numText.text = num.ToString();
        popupPanel.SetActive(false);
    }

}