using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveChipController : MonoBehaviour
{
    public GamePlay gameplay;
    bool ispick = false;
    bool ismovable = false;
    float xpos = 0, ypos = 0;
    float disx = 0, disy = 0;
    Chip chip;
    Place finalPlace = null;

    public bool getPicked()
    {
        return ispick;
    }
    public Chip getChip()
    {
        return chip;
    }
    public void SetFinalPlace(Place f_pos)
    {
        finalPlace = f_pos;
    }
    public void StartMove()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 mousePos2D = new Vector2(mousePos.x, mousePos.y);

        if (chip == null)
        {
            RaycastHit2D hit = Physics2D.Raycast(mousePos2D, Vector2.zero);
            if (hit.collider != null)
            {
                finalPlace = null;
                //Debug.Log(hit.collider.gameObject.name);
                chip = LastChip(hit.collider.gameObject.name);
            }
        } else if (chip != null)
        {
            if (!ispick)
            {
                GameObject gm_obj = chip.getGameObject();
                xpos = gm_obj.transform.position.x;
                ypos = gm_obj.transform.position.y;
                disx = gm_obj.transform.position.x - mousePos2D.x;
                disy = gm_obj.transform.position.y - mousePos2D.y;
                //Debug.Log(xpos + "" + ypos);
                ispick = true;
            }
            chip.getGameObject().transform.position = mousePos2D + new Vector2(disx,disy);
            if(Vector3.Distance(chip.getGameObject().transform.position, new Vector3(xpos, ypos, 0)) >0.5)
            {
                ismovable = true;
                //Debug.Log("Moved");
            }
        }
    }
    public void MouseUp()
    {
        if (chip != null)
        {
            if (finalPlace != null && (finalPlace.is_clickable))
            {
                Vector2 vec = finalPlace.getGameObject().transform.position;
                chip.getGameObject().transform.position = new Vector2(vec.x, vec.y);
                finalPlace.setStatus(gameplay.turn);
                gameplay.stepCont.EndStepAnimation(finalPlace);
            }
            else
            {
                chip.getGameObject().transform.position = new Vector2(xpos, ypos);
                if (ismovable)
                {
                    chip.place.UnChooseChip();
                    gameplay.stepCont.Unclicable();
                    ismovable = false;
                }
            }
            ispick = false;
            chip = null;
        }
        if (ismovable)
            gameplay.SetAllPlacesDefauilt();
    }
    Chip LastChip(string key)
    {

        Dictionary<string, Chip> chips = getChips();
        if (chips.ContainsKey(key))
        {
            Place place = chips[key].place;
            //Debug.Log(key+ " "+ place.getLastChip().name);
            if (place.getLastChip().name == key)
                return chips[key];
        }else if(BoardBuilder.Places.ContainsKey(key))
        {
            return BoardBuilder.Places[key].getLastChip();
        }
        return null;
    }
    public Dictionary<string, Chip> getChips()
    {
        Dictionary<string, Chip> chips = new Dictionary<string, Chip>();
        switch (gameplay.turn)
        {
            case PlaseStstus.White:
                chips = BoardBuilder.WhiteChip;
                break;
            case PlaseStstus.Black:
                chips = BoardBuilder.BlackChip;
                break;
        }
        return chips;
    }

    public void CheckCollision()
    {
        if (!ismovable)
            return;
        if (!getPicked())
            return;
        Place place1 = null;
        Place place2 = null;
        int i = 0, len = BoardBuilder.Places.Count;
        for (i = 0; i < len; i++)
        {
            string key = "Places" + i.ToString();
            if (BoardBuilder.Places.ContainsKey(key))
            {
                if (chip.getCColllider().IsTouching(BoardBuilder.Places[key].getCollider()))
                {
                    if (place1 == null)
                    {
                        place1 = BoardBuilder.Places[key];
                    } else if (place2 == null && place1.name != key)
                    {
                        place2 = BoardBuilder.Places[key];
                        break;
                    }
                }
            } else if (chip.getCColllider().IsTouching(BoardBuilder.homeB.getCollider()))
            {
                if (place1 == null)
                {
                    place1 = BoardBuilder.homeB;
                }
                else if (place2 == null && place1.name != BoardBuilder.homeB.name)
                {
                    place2 = BoardBuilder.homeB;
                    break;
                }
            }
            else if (chip.getCColllider().IsTouching(BoardBuilder.homeW.getCollider()))
            {
                if (place1 == null)
                {
                    place1 = BoardBuilder.homeW;
                }
                else if (place2 == null && place1.name != BoardBuilder.homeW.name)
                {
                    place2 = BoardBuilder.homeW;
                    break;
                }
            }
        }
        if (place1 == null || place2 == null)
        {

            string key1 = "Chip_White_";
            string key2 = "Chip_Black_";
            int len1 = BoardBuilder.WhiteChip.Count;
            for (i = 0; i < len1; i++)
            {
                if (chip == null)
                    break;
                
                if (BoardBuilder.WhiteChip.ContainsKey(key1 + i.ToString()))
                {
                    Chip ch = BoardBuilder.WhiteChip[key1 + i.ToString()];
                    if (chip.getCColllider().IsTouching(ch.getCColllider()))
                    {
                        if (place1 == null)
                        {
                            place1 = BoardBuilder.WhiteChip[key1 + i.ToString()].place;
                        }
                        else if (place2 == null && place1.name != ch.place.name)
                        {
                            place2 = ch.place;
                            break;
                        }
                    }
                }
                if (BoardBuilder.BlackChip.ContainsKey(key2 + i.ToString()))
                {
                    Chip ch = BoardBuilder.BlackChip[key2 + i.ToString()];
                    if (chip.getCColllider().IsTouching(ch.getCColllider()))
                    {
                        if (place1 == null)
                        {
                            place1 = ch.place;
                        }
                        else if (place2 == null && place1.name != ch.place.name)
                        {
                            place2 = ch.place;
                            break;
                        }
                    }
                }
            }
        }
        float vec1 = 0;
        float vec2 = 0;
        if (place1 != null)
            vec1 = Vector3.Distance(chip.getGameObject().transform.position, place1.getGameObject().transform.position);
        if (place2 != null)
            vec2 = Vector3.Distance(chip.getGameObject().transform.position, place2.getGameObject().transform.position);

        if (place1 == null && place2 == null)
        {
            SetFinalPlace(null);
            return;
        }
        else if(place1 != null && place2 == null)
        {
            setColorOfColidPlace(place1);
            SetFinalPlace(place1);
            return;
        }
        else
        {
            if (vec1 < vec2)
            {
                setColorOfColidPlace(place1);
                SetFinalPlace(place1);
            }
            else
            {
                setColorOfColidPlace(place2);
                SetFinalPlace(place2);
            }
        }
    }

    void setColorOfColidPlace(Place place)
    {
        gameplay.SetAllPlacesDefauilt();
        if (place.is_clickable)
            place.setColor(gameplay.stepCont.color);
        else
            place.setColor(gameplay.stepCont.Red_color);
    }
}
