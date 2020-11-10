using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public delegate void EndStapDelegate(Place f_pos);
    
public class Chip : GameElement
{
    public string name;
    public int ID;
    private PlaseStstus status;
    public Place place;

    public PlaseStstus getStat()
    {
        return status;
    }

    public void setStatus(PlaseStstus stat)
    {
        this.status = stat;
    }
   public Chip(string nam)
    {
        name = nam;
    }


    public IEnumerator MoveChipToPosition(Place f_pos, EndStapDelegate callBack)
    {
        float i = 0.0f;
        float j = 0;
        Vector2 pos1 = this.getGameObject().transform.position;
        Vector2 pos2 = f_pos.getFinishPosition();
        float step = Vector2.Distance(pos1,pos2);
        float spret = Vector2.Distance(place.getFinishPosition(), f_pos.getFinishPosition());
        while (i < 10)
        {
            var change = step*2.5f * Time.deltaTime;
            this.getGameObject().transform.position = Vector2.MoveTowards(this.getGameObject().transform.position, f_pos.getFinishPosition(), change);
            i += 0.2f;
            if (this.place.is_reverted == f_pos.is_reverted)
            {
                int cof = place.is_reverted ? -1 : 1;
                if (i <= 5)
                    j += .2f;
                else
                {
                    j -= .2f;
                    cof = place.is_reverted ? 1 : -1;
                }

                float Ychange = cof * Mathf.Sqrt(Mathf.Pow(spret / 2, 2) * 2 - 4 * (spret / 2) * Mathf.Cos(18 * j)) / 50;
                Vector3 vec = new Vector3(0, Ychange, 0);
                if(!float.IsNaN(Ychange))
                    this.getGameObject().transform.position += vec;
                if (this.getGameObject().transform.position.x.Equals(f_pos.getFinishPosition().x))
                    break;
            }
            else
            {
                if (this.getGameObject().transform.position.Equals(f_pos.getFinishPosition()))
                    break;
            }
            yield return null;
        }
        f_pos.setStatus(status);
        callBack(f_pos);
    }

}
