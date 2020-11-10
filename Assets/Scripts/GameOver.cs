using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameOver : MonoBehaviour
{
    public Text Title_txt;
    public Text WhoWin_txt;

    public void setInit()
    {
        gameObject.SetActive(true);
    }

    public void setTtile(string text)
    {
        Title_txt.text = text;
    }

    public void setWhoWin(string text)
    {
        WhoWin_txt.text = text;
    }
}
