using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AoutoPlayer : MonoBehaviour
{
    public GamePlay gameplay;
    public StepPriorityCounter stepCounter;
    bool is_anamyStep = false;
    Dictionary<string, List<string>> stepPassability = new Dictionary<string, List<string>>();
    List<string> placer = new List<string>();
    List<string> keys = new List<string>();
    List<int> back_keys = new List<int>();
    public bool ISAnamyStep()
    {
        return is_anamyStep;
    }
    public void setAnamyStepBool(bool step)
    {
        /*if (step) Debug.Log("Anamy start to go");
        else Debug.Log("Anamy End his go");*/
        is_anamyStep = step;
    }
    void ClearAllList()
    {
        keys.Clear();
        stepPassability.Clear();
        back_keys.Clear();
    }

    public void StartStep()
    {
        Debug.Log("StartStep");
        ClearAllList();
        stepCounter.Init();
        int i = 0;
        int len = BoardBuilder.coordsW.Count;
        List<int> coords = stepCounter.getLstPlasesCoord();
        for( i= 0; i<len; i++)
        {
            string key = "Places" + coords[i];
            
            if(BoardBuilder.Places[key].getStatus() == gameplay.turn)
            {
                gameplay.stepCont.setFirstPlase(BoardBuilder.Places[key]);
                if (!gameplay.isitFromHead(BoardBuilder.Places[key])) { 
                    gameplay.stepCont.StepPrecalculate(BoardBuilder.Places[key]);
                    if (placer.Count != 0)
                    {
                        keys.Add(key);
                        back_keys.Add(i);
                        stepPassability.Add(key, placer);
                    }
                }
                placer = new List<string>(); ;
            }
            

        }
        Comparesteps();
    }

    public void Comparesteps()
    {
        int i = 0;
        int len = keys.Count;
        List<string> cmparer = new List<string>();
        Place f_p =null, l_p = null;
        float kof = -100;
        float back_kof = 0;
        for (i = 0; i < len; i++)
        {
            
            cmparer = stepPassability[keys[i]];
            int j = 0, len2 = cmparer.Count;
            for(j = 0; j<len2; j++)
            {
                if (BoardBuilder.Places[keys[i]].getChipsCount() == 1)
                    back_kof = stepCounter.getBackPriority(back_keys[i]);
                //Debug.Log(cmparer[j] + " = " + BoardBuilder.Places[cmparer[j]].getPriority() + " // " + kof + " - " + back_kof);
                if (BoardBuilder.Places[cmparer[j]].getPriority() - back_kof > kof)
                {
                    //Debug.Log("seted = " + BoardBuilder.Places[cmparer[j]].getPriority()+ ":" + kof);
                    kof = BoardBuilder.Places[cmparer[j]].getPriority();
                    f_p = BoardBuilder.Places[keys[i]];
                    l_p = BoardBuilder.Places[cmparer[j]];
                }
            }
        }
        if (f_p != null && l_p != null)
        {
            StartCoroutine(WaytBeforeStartMove(f_p, l_p));
        }
    }

    IEnumerator WaytBeforeStartMove(Place f_p, Place l_p)
    {
        yield return new WaitForSeconds(1.0f);
        gameplay.stepCont.setFirstPlase(f_p);
        gameplay.stepCont.MoveChip(l_p);
    }

    public void setfinalPlacesforStep(Place place)
    {
        placer.Add(place.name);
    }
}
