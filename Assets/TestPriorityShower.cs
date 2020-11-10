using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TestPriorityShower : MonoBehaviour
{
    public List<Text> TotalPriority = new List<Text>();
    public List<Text> BackPriority = new List<Text>();
    
    public void setTotatlPriority(List<float> priority)
    {
        for (int i = 0; i< TotalPriority.Count; i++)
        {
            TotalPriority[i].text ="P: " +  priority[i].ToString();
        }
    }

    public void setbackPriority(List<float> priority)
    {
        for (int i = 0; i < BackPriority.Count; i++)
        {
            BackPriority[i].text ="B: " + priority[i].ToString();
        }
    }
}
