using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiceController : MonoBehaviour
{

    public GamePlay gameplay;
    public bool OnlyCuch = false;

    bool turn_eject = false;
    public bool dicer = false;
    float time = 3.0f;
    bool inTheGame = false;

    bool iscuch = false;
    bool droped = false;

    public void StartToos()
    {
        if (!turn_eject)
        {
            StartCoroutine("StartDising", false);
            turn_eject = true;
        }
        if(dicer)
        {
            DiceDrop();
            dicer = false;
            
        }
    }

    IEnumerator StartDising(bool trig)
    {
        if (trig)
            yield return new WaitForSeconds(6 * 0.2f);
        BoardBuilder.dice_1.Active(true);
        BoardBuilder.dice_1.DropDice(new Vector2(0 + (ScreenResolution.CameraWidth * .8f) * .6f, 0));
        StartCoroutine("DropDiceCourutin", BoardBuilder.dice_1);
        yield return new WaitForSeconds(6 * 0.2f);
        BoardBuilder.dice_2.Active(true);
        BoardBuilder.dice_2.DropDice(new Vector2(0 - ScreenResolution.CameraWidth * .8f * .6f, 0));
        StartCoroutine("DropDiceCourutin", BoardBuilder.dice_2);
        yield return new WaitForSeconds(6 * 0.2f);
        RejectTurn();
    }

    void RejectTurn()
    {
        if (BoardBuilder.dice_1.Value == BoardBuilder.dice_2.Value)
        {
            StartCoroutine("StartDising", true);
            return;
        }
        if (BoardBuilder.dice_1.Value > BoardBuilder.dice_2.Value)
        {
            gameplay.turn = PlaseStstus.Black;
            gameplay.autoPlayer.setAnamyStepBool(true);
        }
        else if (BoardBuilder.dice_1.Value < BoardBuilder.dice_2.Value)
        {
            gameplay.turn = PlaseStstus.White;
            gameplay.autoPlayer.setAnamyStepBool(false);
        }
        inTheGame = true;
        DiceDrop();
    }

    public void DiceDrop()
    {
        int cof = 1;
        if (gameplay.turn == PlaseStstus.White)
            cof = -1;
        else if (gameplay.turn == PlaseStstus.Black)
            cof = 1;

        BoardBuilder.dice_3.Active(false);
        BoardBuilder.dice_4.Active(false);

        BoardBuilder.dice_1.DropDice(new Vector2(0 + cof * (ScreenResolution.CameraWidth * .8f) * .5f, 0));
        BoardBuilder.dice_2.DropDice(new Vector2(0 + cof * (ScreenResolution.CameraWidth * .8f) * .7f, 0));
        BoardBuilder.dice_3.DropDice(new Vector2(0 + cof * (ScreenResolution.CameraWidth * .8f) * .3f, 0));
        BoardBuilder.dice_4.DropDice(new Vector2(0 + cof * (ScreenResolution.CameraWidth * .8f) * .9f, 0));


        StartCoroutine("DropDiceCourutin", BoardBuilder.dice_1);
        if(!OnlyCuch)
            StartCoroutine("DropDiceCourutin", BoardBuilder.dice_2);

        StartCoroutine("WaiytForDiceDrop");
    }

    IEnumerator WaiytForDiceDrop()
    {
        while (!droped)
            yield return null;
        yield return new WaitForSeconds(1.25f);

        if (gameplay.stepCont.CheckCanStep())
        {
            gameplay.stepCont.staepActivate = true;

        }
        else
        {
            gameplay.stepCont.staepActivate = false;
            gameplay.SwitchTurn();
        }
        
    }

    IEnumerator DropDiceCourutin(Dice dice)
    {
        float step = 0.2f, i = 0;
        while (i < time)
        {
            int dicer = dice.randomization();
            dice.OnDiceNum(dicer);
            yield return new WaitForSeconds(0.05f);
            i += step;
        }
        
        if (inTheGame)
            IsItCuch();
        droped = true;
    }
    public void IsItCuch()
    {
        if (OnlyCuch)
        {
            BoardBuilder.dice_2.OnDiceNum(BoardBuilder.dice_1.Value);
        }
        if (BoardBuilder.dice_1.Value == BoardBuilder.dice_2.Value)
        {
            BoardBuilder.dice_3.Active(true);
            BoardBuilder.dice_3.OnDiceNum(BoardBuilder.dice_1.Value);
            BoardBuilder.dice_4.Active(true);
            BoardBuilder.dice_4.OnDiceNum(BoardBuilder.dice_1.Value);
            iscuch = true;
        }else
        {
            iscuch = false;
        }
    }

    public bool isItCuch()
    {
        return iscuch;
    }
}
