using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum PlaseStstus
{
    None = 0,
    White = 1,
    Black = 2
}
public class Place :GameElement
{

    public string name;
    public int ID;
    public bool is_reverted =false;
    private PlaseStstus stat = PlaseStstus.None;
    public bool is_clickable = false;
    public int cordw, cordb;
    public bool isSelected = false;
    private bool is_white_head = false, is_black_head = false;
    private bool is_Last = false;
    float priority_w = 0;
    float priority_b = 0;
    List<Chip> chips = new List<Chip>();
    Chip chip;

    public void setPriority(PlaseStstus status, float kof)
    {
        priority_w = kof; 
    }

    public float getPriority ()
    {
        return priority_w;
    }

    public int getChipsCount()
    {
        //Debug.Log(name + " - -- -"+this.chips.Count);
        return this.chips.Count;
    }
    public void SetLast()
    {
        is_Last = true;
    }
    public bool isLast()
    {
        return is_Last;
    }

    
    public void setBlackhead(bool head)
    {
        is_black_head = head;
    }

    public void setWhitehead(bool head)
    {
        is_white_head = head;
    }

    public bool getIsWhiteHead()
    {
        return is_white_head;
    }

    public bool getIsBlackHead()
    {
        return is_black_head;
    }

    public Place()
    {

    }

    public Chip getLastChip()
    {
        if (isAnyChip())
            return chips[chips.Count-1];
        return null;
    }
   
    public bool isAnyChip()
    {
        return chips.Count > 0;
    }
    public void ChooseChip()
    {
        if (chips.Count == 0)
            return;
        int cof = is_reverted ? -1 : 1;
        float scaler = chips[chips.Count - 1].getGameObject().transform.localScale.x / 2;
        chips[chips.Count - 1].getGameObject().transform.localPosition = new Vector3(chips[chips.Count - 1].getGameObject().transform.localPosition.x,
            chips[chips.Count - 1].getGameObject().transform.localPosition.y + cof*scaler, 0);
        chips[chips.Count - 1].setLayer(3+ chips.Count);
        isSelected = true;
    }

    public Vector2 getFinishPosition()
    {
        Vector2 vec = getGameObject().transform.localPosition;
        if (chips.Count > 0)
            vec = chips[chips.Count - 1].getGameObject().transform.localPosition;

        return vec;
    }

    public Chip GetAChip()
    {
        if (chips.Count > 0)
            return chips[chips.Count - 1];
        return null;
    }

    public void UnChooseChip()
    {
        if (chips.Count == 0)
            return;
        int cof = is_reverted ? 1 : -1;
        float scaler = chips[chips.Count - 1].getGameObject().transform.localScale.x / 2;
        chips[chips.Count - 1].getGameObject().transform.localPosition = new Vector3(chips[chips.Count - 1].getGameObject().transform.localPosition.x,
            chips[chips.Count - 1].getGameObject().transform.localPosition.y + cof * scaler, 0);
        chips[chips.Count - 1].setLayer(3 + chips.Count-1);
        isSelected = false;
    }

    public void Revert()
    {
        Rotate();
        is_reverted = true;
    }

    public PlaseStstus getStatus()
    {
        return this.stat;
    }

    public void setStatus(PlaseStstus stat)
    {
        this.stat = stat;
    }

    public void AddChip(Chip chip)
    {
        chip.setLayer(3+ chips.Count);
        this.chips.Add(chip);
    }

    public Chip PopChip()
    {
        Chip ch = chips[chips.Count - 1];
        chips.RemoveAt(chips.Count - 1);
        return ch;
    }

    public void Sorting ()
    {
        int cof = is_reverted ? 1 : -1;
        float step = chips[0].getGameObject().transform.localScale.x * cof;
        int i = 0, len = chips.Count;
        float startPos = getGameObject().transform.position.y + step * 1.8f;
        float lent = 1;
        if (len <= 4)
            lent = 2f;
        else if (len <= 8)
            lent = 1f;
        if (len >= 12)
            lent = .5f;

        for (i = 0; i<len; i++)
        {
           
            chips[i].getGameObject().transform.position = new Vector3(getGameObject().transform.position.x, startPos - step* lent * i, 0);
            
        }
    }

}
