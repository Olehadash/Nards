using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GamePlay : MonoBehaviour
{

    public DiceController dicecontroller;
    public StepController stepCont;
    public PlaseStstus turn = PlaseStstus.None;
    public MoveChipController moveChipController;
    public bool startGame = true;
    public bool isFirst = false;
    public bool gameIsStarted = false;
    public GameOver gameover;
    public AoutoPlayer autoPlayer;

    private float x_resolution, y_resolution;
    private bool w_f = true, b_f = true;
    private bool chipMover = false;
    private bool isgameover = false;
    private bool anamy = true;
    private int movePerCuch = 0;

    public  bool CheckIsItFirstStepOfGame()
    {
        bool b = false;
        if (turn == PlaseStstus.Black)
            b = b_f;
        else if (turn == PlaseStstus.White)
            b = w_f;
        return b;
    }
    // Start is called before the first frame update
    void Start()
    {
        x_resolution = ScreenResolution.CameraWidth * .8f;
        y_resolution = ScreenResolution.CameraHeight * .8f;
    }

    public bool IsFirst()
    {
        return isFirst;
    }

    public void setIsFisrst(bool fr)
    {
        movePerCuch++;
        isFirst = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (!isgameover)
        {
            dicecontroller.StartToos();
            if (!autoPlayer.ISAnamyStep())
            {
                if (Input.GetMouseButtonDown(0))
                {
                    stepCont.StartStep();
                    chipMover = true;
                }
                if (chipMover)
                {
                    moveChipController.StartMove();
                    moveChipController.CheckCollision();
                }

                if (Input.GetMouseButtonUp(0))
                {
                    chipMover = false;
                    moveChipController.MouseUp();
                }
            }else if (stepCont.staepActivate && anamy && autoPlayer.ISAnamyStep())
            {
                anamy = false;
                StartCoroutine("WAityBiforeAnameStep");
            }
        }
        

    }
    IEnumerator WAityBiforeAnameStep()
    {
        yield return new WaitForSeconds(1.0f);
        //Debug.Log("Anmeny Turn" + BoardBuilder.dice_1.Value + "  " + BoardBuilder.dice_2.Value);
        autoPlayer.StartStep();
    }

    public void EndStep(int def)
    {
        checkifAllAtHome();
        
        if (dicecontroller.isItCuch())
        {
            List<Dice> dices = new List<Dice>();
            dices.Add(BoardBuilder.dice_1);
            dices.Add(BoardBuilder.dice_2);
            dices.Add(BoardBuilder.dice_3);
            dices.Add(BoardBuilder.dice_4);
            int i, ii, j = 0, jj = 0;
            for(i = 0; i<dices.Count; i++)
            {
                if(dices[i].Value != 0)
                {
                    j++;
                    if(dices[i].Value*j == def){
                        for (ii = 0; ii < dices.Count; ii++)
                        {
                            if (dices[ii].Value != 0)
                            {
                                jj++;
                                dices[ii].SetAlpha();
                                dices[ii].Value = 0;
                            }
                            if (jj == j)
                                break;
                        }
                        break;
                    }

                }
            }
            if (BoardBuilder.dice_1.Value + BoardBuilder.dice_2.Value + BoardBuilder.dice_3.Value + BoardBuilder.dice_4.Value == 0)
            {
                SwitchTurn();
                return;
            }
        }
        else {
            if (BoardBuilder.dice_1.Value == def)
            {
                BoardBuilder.dice_1.SetAlpha();
                BoardBuilder.dice_1.Value = 0;
            }
            else if (BoardBuilder.dice_2.Value == def)
            {
                BoardBuilder.dice_2.SetAlpha();
                BoardBuilder.dice_2.Value = 0;
            }
            else if ((BoardBuilder.dice_1.Value + BoardBuilder.dice_2.Value == def))
            {
                BoardBuilder.dice_1.Value = 0;
                BoardBuilder.dice_2.Value = 0;
            }
            if (BoardBuilder.dice_1.Value + BoardBuilder.dice_2.Value == 0)
            {
                SwitchTurn();
                return;
            }

        }
        //Debug.Log("Step Finished by dice value " + def.ToString());
        if(autoPlayer.ISAnamyStep())
            anamy = true;
        StartCoroutine("WaitAndCheck");
    }

    IEnumerator WaitAndCheck()
    {
        yield return new WaitForSeconds(0.2f);
        bool canStep = stepCont.CheckCanStep();

        if (!canStep)
        {
            stepCont.staepActivate = false;
            SwitchTurn();
        }
    }

    bool checkifAllAtHome()
    {
        bool b = false;
        //Debug.Log(BoardBuilder.homeW.getChipsCount() + " " + BoardBuilder.homeB.getChipsCount() + " " + BoardBuilder.WhiteChip.Count);
        if(BoardBuilder.homeW.getChipsCount() == BoardBuilder.WhiteChip.Count)
        {
            turn = PlaseStstus.White;
            b = true;
        }
        if(BoardBuilder.homeB.getChipsCount() == BoardBuilder.BlackChip.Count)
        {
            turn = PlaseStstus.Black;
            b = true;
        }
        if(b)ShowGameOver();
        return b;
    }
    void ShowGameOver()
    {
        gameover.setInit();
        string text = "";
        switch(turn)
        {
            case PlaseStstus.White:
                text = "White Win";
                break;
            case PlaseStstus.Black:
                text = "Black Win";
                break;
        }

        gameover.setWhoWin(text);
        isgameover = true;
    }
    public void SwitchTurn()
    {
        //Debug.Log("EndStep");
        if (turn == PlaseStstus.White)
        {
            turn = PlaseStstus.Black;
            autoPlayer.setAnamyStepBool(true);
            w_f = false;
            
        }
        else
        {
            turn = PlaseStstus.White;
            autoPlayer.setAnamyStepBool(false);
            b_f = false;
        }
        movePerCuch = 0;
        isFirst = false;
        anamy = true;
        BoardBuilder.dice_1.resetAlpha();
        BoardBuilder.dice_2.resetAlpha();
        this.stepCont.staepActivate = false;
        this.dicecontroller.dicer = true;

    }
    public bool isitFromHead(Place place)
    {
        if (isHeadRoool())
            isFirst = false;
        bool b =  (((turn == PlaseStstus.Black && place.getIsBlackHead())
            || (turn == PlaseStstus.White && place.getIsWhiteHead())) && isFirst);
        return b ;
    }
    public bool Head(Place place)
    {
        return ((turn == PlaseStstus.Black && place.getIsBlackHead())
            || (turn == PlaseStstus.White && place.getIsWhiteHead()));
    }
    bool isHeadRoool()
    {
        bool trep = CheckIsItFirstStepOfGame();// ((gamePlay.turn == PlaseStstus.Black && place_First.getIsBlackHead()) || (gamePlay.turn == PlaseStstus.White && place_First.getIsWhiteHead()));
        bool grep = dicecontroller.isItCuch();
        bool drep = (getDiceValue() == 3) || (getDiceValue() == 4) || (getDiceValue() == 6);
        //Debug.Log("isHeadRool all = " + trep + " " + grep + " " +drep);
        return trep && grep && drep && (movePerCuch < 2);

    }

    int getDiceValue()
    {
        int value = 0;
        if (BoardBuilder.dice_1.Value != 0)
            value = BoardBuilder.dice_1.Value;
        else if (BoardBuilder.dice_2.Value != 0)
            value = BoardBuilder.dice_2.Value;
        else if (BoardBuilder.dice_3.Value != 0)
            value = BoardBuilder.dice_3.Value;
        else 
            value = BoardBuilder.dice_4.Value;
        return value;
    }

    bool isAnyAnemyInHome()
    {
        List<int> homeCords = new List<int>();
        PlaseStstus stat = PlaseStstus.None;
        bool b = false;
        if(turn == PlaseStstus.Black)
        {
            homeCords = BoardBuilder.home_W_id;
            stat = PlaseStstus.White;
        }else if(turn == PlaseStstus.White)
        {
            homeCords = BoardBuilder.home_B_id;
            stat = PlaseStstus.Black;
        }

        int i = 0;
        for(i = 0; i<homeCords.Count; i++)
        {
            string key = "Places" + homeCords[i].ToString();
            if (BoardBuilder.Places.ContainsKey(key))
            {
                if (BoardBuilder.Places[key].isAnyChip() && BoardBuilder.Places[key].getStatus() == stat)
                {
                    b = true;
                    break;
                }
            }
        }

        return b;
    }
    public bool CanBlockForMurs(Place place)
    {
        if (isAnyAnemyInHome())
            return true;
        int id = place.ID;
        int tr = 0;
        int counter = 1;
        bool condition = (counter <= 5) && (id - counter >= 0);
        bool condition2 = (counter <= 5) && (id + counter < 24);
        while (condition)
        {
            string key = "Places" + (id - counter).ToString();
            if (BoardBuilder.Places.ContainsKey(key))
            {
                if (BoardBuilder.Places[key].getStatus() == turn)
                {
                    tr++;
                }
                else if (BoardBuilder.Places[key].getStatus() == PlaseStstus.None)
                {
                    break;
                }
            }else
            {
                break;
            }
            counter++;
        }
        counter = 1;
        while (condition2)
        {
            string key = "Places" + (id + counter).ToString();
            if (BoardBuilder.Places.ContainsKey(key))
            {
                if (BoardBuilder.Places[key].getStatus() == turn)
                {
                    tr++;
                }
                else if (BoardBuilder.Places[key].getStatus() == PlaseStstus.None)
                {
                    break;
                }
            }
            else
            {
                break;
            }
            counter++;
        }

        return !(tr >= 5);
    }
    public bool isAllChipsAtHome()
    {
        bool b = true;
        List<int> home = new List<int>();
        Dictionary<string, Chip> chips = new Dictionary<string, Chip>();
        string nam = "";
        bool br = false;
        if(turn == PlaseStstus.White)
        {
            home = BoardBuilder.home_W_id;
            chips = BoardBuilder.WhiteChip;
            nam = "Chip_White_";
        }else if(turn == PlaseStstus.Black)
        {
            home = BoardBuilder.home_B_id;
            chips = BoardBuilder.BlackChip;
            nam = "Chip_Black_";
        }
        int i = 0, j = 0;
        for(i = 0; i< chips.Count; i++)
        {
            string key = nam + i.ToString();
            br = false;
            for (j=0; j< home.Count; j++)
            {
                
                if(chips[key].place.ID == home[j])
                {
                    br = true;
                    break;
                }
            }
            if(!br)
            {
                b = false;
                break;
            }
        }
        return b;
    }
    public  void SetAllPlacesDefauilt()
    {
        int i = 0, len = BoardBuilder.Places.Count;
        for (i = 0; i < len; i++)
        {
            string key = "Places" + i;
            if (BoardBuilder.Places.ContainsKey(key))
            {
                BoardBuilder.Places[key].setColor(stepCont.Default_color);
            }
        }
        BoardBuilder.homeW.setColor(stepCont.Default_color);
        BoardBuilder.homeB.setColor(stepCont.Default_color);
    }
}
