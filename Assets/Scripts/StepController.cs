using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class StepController : MonoBehaviour
{
    public bool staepActivate = false;
    public GamePlay gamePlay;

    public Color color;
    public Color Default_color;
    public Color Red_color;

    

    Place place_First;

    public void setFirstPlase(Place f_p)
    {
        place_First = f_p;
    }

    public void StartStep()
    {
        
        if (staepActivate)
        {
            
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 mousePos2D = new Vector2(mousePos.x, mousePos.y);

            RaycastHit2D hit = Physics2D.Raycast(mousePos2D, Vector2.zero);
            
            if (hit.collider != null)
            {
                //Debug.Log(hit.collider.gameObject.name);
                
                Place place = GetObjectByName(hit.collider.gameObject.name);
                if (place != null && place.is_clickable)
                {
                    MoveChip(place);
                }
                else if (place != null && !place.is_clickable && place.isAnyChip() && !gamePlay.isitFromHead(place))
                {
                    Unclicable();
                    place_First = place;
                    StepPrecalculate(place);
                }
                else
                {
                    Unclicable();

                }
                
            }
            else
            {
                Unclicable();
            }
        }
    }
    public bool CanStepByCuchFrom(Place place, bool picked = true)
    {
        int coord = GetCoord(place);
        List<int> coords = GetCoords();
        List<Dice> dices = new List<Dice>();
        dices.Add(BoardBuilder.dice_1);
        dices.Add(BoardBuilder.dice_2);
        dices.Add(BoardBuilder.dice_3);
        dices.Add(BoardBuilder.dice_4);
        int def = 0;
        int j = 0;
        bool edject = false;

        for (int i = 0; i < dices.Count; i++)
        {
            if (dices[i].Value != 0)
            {
                j++;
                def = dices[i].Value;
                if (isPasableToStep(coord + def * (j), coords) && gamePlay.CanBlockForMurs(place))
                {
                    if (picked) SetColorOnPlace(coord + def * (j), coords);
                    edject = true;
                }
                else
                {
                    break;
                }
            }
        }
        return edject;
    }
    public bool CanStepFrom(Place place, bool picked = true)
    {
        int coord = GetCoord(place);
        List<int> coords = GetCoords();
        bool edject = false;
        if (BoardBuilder.dice_1.Value != 0 && isPasableToStep(coord + BoardBuilder.dice_1.Value, coords) && gamePlay.CanBlockForMurs(place))
        {
            if (picked) SetColorOnPlace(coord + BoardBuilder.dice_1.Value, coords);
            edject = true;
        }
        if (BoardBuilder.dice_2.Value != 0 && isPasableToStep(coord + BoardBuilder.dice_2.Value, coords) && gamePlay.CanBlockForMurs(place))
        {
            if (picked) SetColorOnPlace(coord + BoardBuilder.dice_2.Value, coords);
            edject = true;
        }
        if ((BoardBuilder.dice_1.Value != 0) && (BoardBuilder.dice_2.Value != 0)
            && (isPasableToStep(coord + BoardBuilder.dice_1.Value, coords)
            || isPasableToStep(coord + BoardBuilder.dice_2.Value, coords))
            && isPasableToStep(coord + BoardBuilder.dice_1.Value + BoardBuilder.dice_2.Value, coords)
             && gamePlay.CanBlockForMurs(place))
        {
            if (picked) SetColorOnPlace(coord + BoardBuilder.dice_1.Value + BoardBuilder.dice_2.Value, coords);
        }

        return edject;
    }
    public void MoveChip(Place f_pos)
    {
         StartCoroutine( place_First.GetAChip().MoveChipToPosition(f_pos, EndStepAnimation));
    }
    public void EndStepAnimation(Place f_pos)
    {
        if (place_First == null) return;
        if (!place_First.isAnyChip())
            return;
        Chip ch = place_First.PopChip();
        if (gamePlay.Head(place_First)) gamePlay.setIsFisrst(true);
        ch.place = f_pos;
        f_pos.AddChip(ch);
        f_pos.setStatus(ch.getStat());
        f_pos.Sorting();
        Unclicable(false);
        if (f_pos.isLast())
            CheckDiceByLast(f_pos);
        else
            CheckDice(f_pos);
    }
    void CheckDiceByLast(Place f_pos)
    {
        int f_id = place_First.ID;
        int pre = 0;
        List<int> values = new List<int>();
        values.Add(BoardBuilder.dice_1.Value);
        values.Add(BoardBuilder.dice_2.Value);

        if (gamePlay.dicecontroller.isItCuch())
        {
            values.Add(BoardBuilder.dice_3.Value);
            values.Add(BoardBuilder.dice_4.Value);
        }

        List<int> home = getHomeIdList();
        for (int i = 0; i < home.Count-1; i++)
        {
            if(f_id == home[i])
            {
                pre = i+1;
                break;
            }
        }
        bool onepubchAcivated = true;
        for(int i = 0; i<values.Count; i++)
        {
            if (pre == values[i] || (pre<values[i] && CheckIfHaveChipsAfter(pre, home)))
            {
                //Debug.Log(pre + " " + values[i]);
                onepubchAcivated = false;
                gamePlay.EndStep(values[i]);
                break;
            }
        }
        if (onepubchAcivated)
        {
            if (!gamePlay.dicecontroller.isItCuch())
            {
                //Debug.Log(pre + " " + (values[0] + values[1]));
                if ((pre == values[0] + values[1]) || (pre < (values[0] + values[1]) && CheckIfHaveChipsAfter(pre, home)))
                {
                    gamePlay.EndStep(values[0] + values[1]);
                }
            }
            else
            {
                int kof2 = 0;
                for (int i = 0; i < values.Count; i++)
                {
                    kof2 += values[i];
                    Debug.Log("End step cuch"+pre + " " + kof2);
                    if ((pre == kof2) || (pre < (kof2) && CheckIfHaveChipsAfter(pre, home)))
                    {
                        gamePlay.EndStep(kof2);
                        break;
                    }
                }
            }
        }
    }
    void CheckDice(Place f_pos)
    {
        //Debug.Log("CheckDice");
        int coord = 0, coord_f = 0;
        if (gamePlay.turn == PlaseStstus.White)
        {
            coord = place_First.cordw;
            coord_f = f_pos.cordw;
        }
        else if (gamePlay.turn == PlaseStstus.Black)
        {
            coord = place_First.cordb;
            coord_f = f_pos.cordb;
        }
        
        //Debug.Log("PreAdject  = " + coord_f + " : " + coord);
        place_First = null;
        gamePlay.EndStep( Math.Abs(coord_f - coord));

    }
    public void Unclicable(bool indef = true)
    {
        string key = "Places";
        for (int i = 0; i < BoardBuilder.Places.Count; i++)
        {
            if (BoardBuilder.Places.ContainsKey(key + i.ToString()))
            {
                if (BoardBuilder.Places[key + i].isSelected)
                {
                    if (indef) BoardBuilder.Places[key + i].UnChooseChip();
                    else BoardBuilder.Places[key + i].isSelected = false;
                }
                if (!BoardBuilder.Places[key + i].isAnyChip())
                {
                    BoardBuilder.Places[key + i].setStatus(PlaseStstus.None);
                }
                BoardBuilder.Places[key + i].setColor(Default_color);
                BoardBuilder.Places[key + i].is_clickable = false;
            }
        }
        BoardBuilder.homeW.setColor(Default_color);
        BoardBuilder.homeB.setColor(Default_color);
        BoardBuilder.homeW.is_clickable = false;
        BoardBuilder.homeB.is_clickable = false;
    }
    public void StepPrecalculate(Place place)
    {
        if (place.isLast())
            return;
        if (!gamePlay.autoPlayer.ISAnamyStep()) 
            place.ChooseChip();
        if (gamePlay.dicecontroller.isItCuch())
        {
            CanStepByCuchFrom(place);
        }
        else
        {
            CanStepFrom(place);
        }
    }
    bool isPasableToStep(int id_coord, List<int> coords)
    {
        if (id_coord >= 24)
            return true;
            string key = "Places" + coords[id_coord].ToString();
        return (BoardBuilder.Places[key].getStatus() == gamePlay.turn || BoardBuilder.Places[key].getStatus() == PlaseStstus.None);
    }
    void SetColorOnPlace(int id_coord, List<int> coords)
    {
        if (id_coord < 24 )
        {
            string key = "Places" + coords[id_coord].ToString();

            if (gamePlay.autoPlayer.ISAnamyStep())
                gamePlay.autoPlayer.setfinalPlacesforStep(BoardBuilder.Places[key]);
            else
            {
                BoardBuilder.Places[key].setColor(color);
                BoardBuilder.Places[key].is_clickable = true;
            }

        }
        else
        {
            passibleLastStep(id_coord);
        }
    }
    void passibleLastStep(int id_coord)
    {
        if (!gamePlay.isAllChipsAtHome()) return;
        Place home = getHomePlace();
        List<int> home_id = getHomeIdList();
        int kof = Math.Abs( 24 - id_coord);
        bool b = false;
        if (kof == 0)
            b = true;
        else
        {
            b = CheckIfHaveChipsAfter(kof, home_id);
        }
        
        if(b)
        {
            home.setColor(color);
            home.is_clickable = true;
        }

    }
    Place getHomePlace()
    {
        Place home = new Place();
        switch (gamePlay.turn)
        {
            case PlaseStstus.White:
                home = BoardBuilder.homeW;
                break;
            case PlaseStstus.Black:
                home = BoardBuilder.homeB;
                break;
        }
        return home;
    }
    List<int> getHomeIdList()
    {
        List<int> home_id = new List<int>();
        switch (gamePlay.turn)
        {
            case PlaseStstus.White:
                home_id = BoardBuilder.home_W_id;
                break;
            case PlaseStstus.Black:
                home_id = BoardBuilder.home_B_id;
                break;
        }
        return home_id;
    }
    bool CheckIfHaveChipsAfter( int kof, List<int> home_id)
    {
        int i = 0;
        bool b = true, a = false;
        string name_of_f = place_First.name;
        for (i = 0; i < home_id.Count; i++)
        {
            string key = "Places" + (home_id[i]).ToString();
            if (a)
            {
                if (BoardBuilder.Places.ContainsKey(key))
                {
                    if (BoardBuilder.Places[key].isAnyChip())
                    {
                        b = false;
                        break;
                    }
                }
            }
            if (name_of_f.Equals(key))
                a = true;
        }
        return b;
    }
    Place GetObjectByName(string name)
    {
        if (BoardBuilder.Places.ContainsKey(name))
        {
            return BoardBuilder.Places[name];
        }
        Dictionary<string, Chip> chips = new Dictionary<string, Chip>();
        if (gamePlay.turn == PlaseStstus.White)
            chips = BoardBuilder.WhiteChip;
        else if (gamePlay.turn == PlaseStstus.Black)
            chips = BoardBuilder.BlackChip;

        if(chips.ContainsKey(name))
            return chips[name].place;

        return null;
    }
    public bool CheckCanStep()
    {
        bool edject = false;
        List<int> coords = GetCoords();
        
        for(int i = 0; i< coords.Count; i++)
        {
            string key = "Places" + coords[i].ToString();
            bool turnCheck = BoardBuilder.Places[key].getStatus() == gamePlay.turn;
            bool headCheck = gamePlay.isitFromHead(BoardBuilder.Places[key]);
            if (turnCheck)
            {
                //Debug.Log("ChipsChecked   " + key);
                if (gamePlay.dicecontroller.isItCuch())
                {
                    if(!headCheck) 
                        edject = CanStepByCuchFrom(BoardBuilder.Places[key], false);
                }
                else
                {
                    if (!headCheck) 
                        edject = CanStepFrom(BoardBuilder.Places[key], false);
                }
                if (edject)
                    break;
            }
        }
        if(!edject) Debug.Log("No more Steps");
        return edject;
    }

    int GetCoord(Place place)
    {
        int coord = 0;
        if (gamePlay.turn == PlaseStstus.White)
            coord = place.cordw;
        else if (gamePlay.turn == PlaseStstus.Black)
            coord = place.cordb;

        return coord;
    }
    List<int> GetCoords()
    {
        List<int> coords = new List<int>();
        if (gamePlay.turn == PlaseStstus.White)
            coords = BoardBuilder.coordsW;
        else if (gamePlay.turn == PlaseStstus.Black)
            coords = BoardBuilder.coordsB;
        return coords;

    }

}