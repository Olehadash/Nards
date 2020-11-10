using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collisator : MonoBehaviour
{
    public GamePlay gamePlay;

    Place place = null;
    void Start()
    {
        
    }

   /* private void OnTriggerEnter2D(Collider2D collision)
    {
        if (gamePlay.moveChipController.getPicked())
        {
            //Debug.Log(collision.gameObject.name);
            string key = collision.gameObject.name;
            Dictionary<string, Chip> chips = gamePlay.moveChipController.getChips();
            if (BoardBuilder.Places.ContainsKey(key))
            {
                //Debug.Log(gamePlay.moveChipController.getChip().place.ID + " " + BoardBuilder.Places[key].ID);
                if (gamePlay.moveChipController.getChip().place.ID != BoardBuilder.Places[key].ID)
                {
                    place = BoardBuilder.Places[key];
                    SetColorRed(place);
                    gamePlay.moveChipController.SetFinalPlace(place);
                }
            }
            if (chips.ContainsKey(key))
            {
                if (!chips[key].name.Equals(gamePlay.moveChipController.getChip().name)
                    && chips[key].place.ID != gamePlay.moveChipController.getChip().place.ID)
                {
                    place = chips[key].place;
                    SetColorRed(place);
                    gamePlay.moveChipController.SetFinalPlace(place);
                }
            }
            
        }
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (gamePlay.moveChipController.getPicked())
        {
            string key = collision.gameObject.name;
            Dictionary<string, Chip> chips = gamePlay.moveChipController.getChips();
            if (BoardBuilder.Places.ContainsKey(key))
            {
                if (gamePlay.moveChipController.getChip().place.ID != BoardBuilder.Places[key].ID)
                {
                    place = BoardBuilder.Places[key];
                    SetColorRed(place);
                    gamePlay.moveChipController.SetFinalPlace(place);
                }
            }
            if (chips.ContainsKey(key))
            {
                if (!chips[key].name.Equals(gamePlay.moveChipController.getChip().name)
                    && chips[key].place.ID != gamePlay.moveChipController.getChip().place.ID)
                {
                    place = chips[key].place;
                    SetColorRed(place);
                    gamePlay.moveChipController.SetFinalPlace(place);
                }
            }
        }
    }*/

    void SetColorRed(Place _place)
    {
        //BoardBuilder.SetAllPlacesDefauilt();
        
        if (!_place.is_clickable)
            _place.setColor(gamePlay.stepCont.Red_color);
    }

}
