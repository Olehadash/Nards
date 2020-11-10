using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StepPriorityCounter : MonoBehaviour
{
    public GamePlay gameplay;
    public TestPriorityShower test;
    List<float> start = new List<float>();
    List<float> nearStart = new List<float>();
    List<float> mars = new List<float>();
    List<float> priority = new List<float>();
    List<float> anamyStrike = new List<float>();
    List<float> backPrior = new List<float>();
    void ClearAll()
    {
        start.Clear();
        nearStart.Clear();
        mars.Clear();
        priority.Clear();
        anamyStrike.Clear();
        backPrior.Clear();
    }
    public void Init()
    {
        //Debug.Log("Init");
        ClearAll();
        setStartPriority();
        CalculateNearAnamyPrioryti();
        CalculateMarsAnamy();
        CalculateAnamyStrike();
        
        int i = 0;
        int len = BoardBuilder.coordsW.Count;
        for (i = 0; i < len; i++)
        {
            priority.Add(start[i] + nearStart[i]+ mars[i] + anamyStrike[i]);
        }
        IsPassaabilitiesComeToHome();
        test.setTotatlPriority(priority);
        test.setbackPriority(backPrior);
        setPriority();
    }

    void setStartPriority()
    {
        start.Add(0);
        start.Add(0.6f);
        start.Add(0.9f);
        start.Add(1f);
        start.Add(1.5f);
        start.Add(1.6f);

        start.Add(1.2f);
        start.Add(0.5f);
        start.Add(0.5f);
        start.Add(1);
        start.Add(1);
        start.Add(1);

        start.Add(0.5f);
        start.Add(1f);
        start.Add(1f);
        start.Add(1.2f);
        start.Add(1.3f);
        start.Add(1.5f);

        start.Add(1.2f);
        start.Add(0.7f);
        start.Add(0.7f);
        start.Add(1.1f);
        start.Add(1.3f);
        start.Add(1.5f);
    }
    public void setPriority()
    {
        int i = 0;
        int len = BoardBuilder.coordsW.Count;
        for(i = 0; i<len; i++)
        {
            string key = "";
            switch(gameplay.turn)
            {
                case PlaseStstus.White:
                    key = "Places" + BoardBuilder.coordsW[i];
                    break;
                case PlaseStstus.Black:
                    key = "Places" + BoardBuilder.coordsB[i];
                    break;
            }
            
            BoardBuilder.Places[key].setPriority(gameplay.turn, priority[i]);
        }
    }
    public List<int> getLstPlasesCoord()
    {
        List<int> coord = new List<int>();
        switch(gameplay.turn)
        {
            case PlaseStstus.White:
                coord = BoardBuilder.coordsW;
                break;
            case PlaseStstus.Black:
                coord = BoardBuilder.coordsB;
                break;

        }
        return coord;
    }
    public List<int> getLstPlasesRevertCoord()
    {
        List<int> coord = new List<int>();
        switch (gameplay.turn)
        {
            case PlaseStstus.White:
                coord = BoardBuilder.coordsB;
                break;
            case PlaseStstus.Black:
                coord = BoardBuilder.coordsW;
                break;

        }
        return coord;
    }
    PlaseStstus getRevertStat()
    {
        PlaseStstus st = PlaseStstus.Black;
        if (gameplay.turn == st)
            st = PlaseStstus.White;
        return st;
    }
    public void CalculateNearAnamyPrioryti()
    {
        int i = 0;
        int len = BoardBuilder.coordsW.Count;
        List<int> cords = getLstPlasesCoord();
        for (i = 0; i < len; i++)
        {
            nearStart.Add(0);
            string key = "Places" + cords[i];
            if(BoardBuilder.Places[key].getStatus() == PlaseStstus.None)
            {
                for(int j = i; j<i-6; j--)
                {
                    key = "Places" + cords[j];
                    if (BoardBuilder.Places[key].getStatus() == getRevertStat())
                        nearStart[i] += 0.2f;
                    if (j == 0)
                        break;
                }
            }
        }
    }
    public void CalculateMarsAnamy()
    {
        int i = 0;
        int len = BoardBuilder.coordsW.Count;
        List<int> cords = getLstPlasesCoord();
        int kof = 0;
        for(i = 0; i<len; i++)
        {
            mars.Add(0);
            string key = "Places" + cords[i];
            if (BoardBuilder.Places[key].getStatus() == PlaseStstus.None)
            {
                for(int j = i+1; j<i+5; j++)
                {
                    if(j>= len)
                        break;
                    key = "Places" + cords[j];
                    if (BoardBuilder.Places[key].getStatus() == getRevertStat())
                        kof++;
                    else
                        break;

                }
                for (int j = i - 1; j < i - 5; j--)
                {
                    if (j < 0)
                        break;
                    key = "Places" + cords[j];
                    if (BoardBuilder.Places[key].getStatus() == getRevertStat())
                        kof++;
                    else
                        break;

                }
                if (kof >= 5)
                    mars[i] = 1.0f;
            }
        }
    }
    public void CalculateAnamyStrike()
    {
        List<int> coordMy = getLstPlasesCoord();
        List<int> coordA = getLstPlasesRevertCoord();
        float kof = 0;
        int i = 0, j = 0, len = coordMy.Count;
        for (i = 0; i < len; i++)
            anamyStrike.Add(0);
        for (i = 0; i<len; i++)
        {
            backPrior.Add(0);
            string key = "Places" + coordMy[i];
            if(BoardBuilder.Places[key].getStatus() == gameplay.turn)
            {
                anamyStrike[i] -= 1f;
                int b_kof = 0;
                for(j = i; j>i-6;j--)
                {
                    if (j < 0)b_kof = 23;
                    string key2 = "Places" + coordMy[b_kof+j];
                    if (BoardBuilder.Places[key2].getStatus() == getRevertStat())
                    {
                        Debug.Log("Have anamy on pos = " + key2);
                        backPrior[i] += 0.3f;
                        anamyStrike[i] += 0.3f;
                    }
                }
                b_kof = 0;
                for (j = i; j<i+6; j++)
                {
                    if (j > 23) b_kof = 24;
                    key = "Places" + coordMy[j - b_kof];
                    if (BoardBuilder.Places[key].getStatus() == PlaseStstus.None)
                    {
                        anamyStrike[j-b_kof] += 0.2f;
                    }
                }
                key = "Places" + coordMy[i];
                if (BoardBuilder.Places[key].getChipsCount() == 1)
                {
                    backPrior[i] += 0.5f;
                }
                
            }
            
        }
    }
    public void CalculatePrefoce()
    {
        int i = 6;
        List<int> coords = getLstPlasesCoord();
        for(i = 6; i<12; i++)
        {
            string key = "Places"+coords[i];
            if (BoardBuilder.Places[key].getStatus() == gameplay.turn)
            {

            }
        }
    }
    public float getBackPriority(int id)
    {
        return backPrior[id];
    }
    int getcoordByAnemy(int coord )
    {
        List<int> coordA = getLstPlasesRevertCoord();
        int cord = -1, i;
        for(i = 0; i<coordA.Count;i++)
        {
            if(coord == coordA[i])
            {
                cord = i;
                break;
            }    
        }
        return cord;
    }
    public void IsPassaabilitiesComeToHome()
    {
       // Debug.Log("IsPassaabilitiesComeToHome");
        List<int> coordMy = getLstPlasesCoord();
        int len = coordMy.Count - 12;
        int kof = 0;
        for (int i = coordMy.Count - 6; i > len; i--)
        {
            string key = "Places" + coordMy[i];
           /* Debug.Log((BoardBuilder.Places[key].getStatus() == gameplay.turn) + "  " +
                (BoardBuilder.Places[key].getChipsCount() == 1) + " " +
                getAnyChipOfAnamyBefore());*/
            if (BoardBuilder.Places[key].getStatus() == gameplay.turn &&
                BoardBuilder.Places[key].getChipsCount() == 1 &&
                getAnyChipOfAnamyBefore()
                )
            {
                backPrior[i] += 5;
            }
            if (BoardBuilder.Places[key].getStatus() == gameplay.turn)
                kof++;
        }
        if (kof<2)
        {
            for (int i = coordMy.Count-1; i > coordMy.Count - 7; i-- )
            {
                priority[i] = 0.1f;
            }
        }
    }
    bool getAnyChipOfAnamyBefore()
    {
        List<int> coord = getLstPlasesRevertCoord();
        for(int i = 0; i<6; i++)
        {
            string key = "Places" + coord[i];
            if (BoardBuilder.Places[key].getStatus() == getRevertStat())
                return true;
            else if (BoardBuilder.Places[key].getStatus() == gameplay.turn)
                break;
        }
        return false;
    }

}
