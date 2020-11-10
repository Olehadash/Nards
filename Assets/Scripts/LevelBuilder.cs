using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelBuilder : MonoBehaviour
{
    public bool AlAtHome = false;
    public GamePlay gamePlay;
    public Sprite BricksBlock;
    public Sprite PlaceBlock;
    public Sprite ChipSprite;


    public List<Sprite> Dices = new List<Sprite>();

    public Color BoardColor;
    public Color DefaultPColor;

    public Color WhiteColor;
    public Color BlackColor;

    GameElement bg_fild = new GameElement();
    GameElement r_board = new GameElement();
    GameElement l_board = new GameElement();

    private float x_resolution, y_resolution;
    // Start is called before the first frame update
    void Start()
    {
        x_resolution = ScreenResolution.CameraWidth* .8f;
        y_resolution = ScreenResolution.CameraHeight * .8f;
        BuilldGameField();
    }
    
    void BuilldGameField()
    {
        bg_fild.AddOject(BricksBlock, new Vector3(0, 0, 0));
        bg_fild.setScale(new Vector2(x_resolution, y_resolution));
        bg_fild.setName("bg_fild");
        bg_fild.getGameObject().transform.localPosition = new Vector3(0, 0, 1);
        bg_fild.setLayer("BoardLayer");

        r_board.AddOject(BricksBlock, new Vector3(0 - x_resolution * .61f, 0, 0));
        r_board.setScale(new Vector2(x_resolution * .5f, y_resolution * .9f));
        r_board.setName("r_board");
        r_board.setPArent(bg_fild.getGameObject());
        r_board.setColor(BoardColor);
        //r_board.getGameObject().AddComponent<BoxCollider2D>();
        r_board.setLayer("BoardLayer");

        l_board.AddOject(BricksBlock, new Vector3(0 + x_resolution * .61f, 0, 0));
        l_board.setScale(new Vector2(x_resolution * .5f, y_resolution * .9f));
        //l_board.setName("l_board");
        l_board.setPArent(bg_fild.getGameObject());
        l_board.setColor(BoardColor);
        l_board.getGameObject().AddComponent<BoxCollider2D>();
        l_board.setLayer("BoardLayer");

        SetAPlaces();

    }

    void SetAPlaces()
    {
        //Place place = new Place(PlaceBlock);
        //place.SetAPlace(0 - x_resolution*1.1f, 0 - y_resolution*.8f, x_resolution*.12f, y_resolution*.25f);
        int id = 0, len = 24, j = 0, i = 0; ;
        List<Vector2> startPos = new List<Vector2>();
        startPos.Add(new Vector2(0 - x_resolution * 1.0f, 0 - y_resolution * .8f));
        startPos.Add(new Vector2(0 + x_resolution * .25f,   0 - y_resolution * .8f));
        startPos.Add(new Vector2(0 - x_resolution * 1.0f, 0 + y_resolution * .8f));
        startPos.Add(new Vector2(0 + x_resolution * .25f,   0 + y_resolution * .8f));

        Vector2 scale = new Vector2(x_resolution * .12f, y_resolution * .25f);
        Vector2 step = new Vector2(x_resolution * 0.15f,0);

        for (id = 0; id<len; id++)
        {
            string key = "Places" + id.ToString();
            Place place = new Place();
            place.AddOject(PlaceBlock, startPos[j] + step*i);
            place.ID = id;
            if (id >=12)
                place.Revert();
            place.setLayer(2);
            place.setColor(DefaultPColor);
            place.setName(key);
            place.name = key;
            place.setBoxColleder(new Vector2(0.01f, 0), new Vector2(1.01f, 2.5f));
            place.setLayer("PlaceLayer");
            place.AddRigidBody();
            place.AddCollisator(gamePlay);
            BoardBuilder.Places.Add(key, place);

            if (id == 11)
            {
                BoardBuilder.homeB.AddOject(PlaceBlock, startPos[j] + step * (i + 1));
                BoardBuilder.homeB.setBoxColleder(new Vector2(0.01f, 0), new Vector2(1.01f, 2.5f));
                BoardBuilder.homeB.setLayer("PlaceLayer");
                BoardBuilder.homeB.setLayer(2);
                BoardBuilder.homeB.setColor(DefaultPColor);
                BoardBuilder.homeB.ID = -2;
                key = "Places_B_Home";
                BoardBuilder.homeB.setName(key);
                BoardBuilder.homeB.name = key;
                BoardBuilder.homeB.SetLast();
                BoardBuilder.homeB.AddRigidBody();
                BoardBuilder.homeB.AddCollisator(gamePlay);
                BoardBuilder.Places.Add(key, BoardBuilder.homeB);
            }
            if (id == 12)
            {
                BoardBuilder.homeW.AddOject(PlaceBlock, startPos[j] - step * (i + 1));
                BoardBuilder.homeW.setBoxColleder(new Vector2(0.01f, 0), new Vector2(1.01f, 2.5f));
                BoardBuilder.homeW.setLayer("PlaceLayer");
                BoardBuilder.homeW.setLayer(2);
                BoardBuilder.homeW.setColor(DefaultPColor);
                BoardBuilder.homeW.Revert();
                BoardBuilder.homeW.ID = -2;
                key = "Places_W_Home";
                BoardBuilder.homeW.setName(key);
                BoardBuilder.homeW.name = key;
                BoardBuilder.homeW.SetLast();
                BoardBuilder.homeW.AddRigidBody();
                BoardBuilder.homeW.AddCollisator(gamePlay);
                BoardBuilder.Places.Add(key, BoardBuilder.homeW);
            }
            i++;
            if (i == 6) { i = 0; j++; }
            
        }
        SetHomeIds();

        SetChips("Places"+0, PlaseStstus.White);
        SetChips("Places"+23, PlaseStstus.Black);
        
        CreateDices();

    }

    void CreateDices()
    {
        Dice dice = new Dice();
        dice.CreateDice(new Vector2(0 - x_resolution * .15f, 0), new Vector2(x_resolution * .05f, x_resolution * .05f), Dices);
        BoardBuilder.dice_1 = dice;

        dice = new Dice();
        dice.CreateDice(new Vector2(0 + x_resolution * .15f, 0), new Vector2(x_resolution * .05f, x_resolution * .05f), Dices);
        BoardBuilder.dice_2 = dice;

        dice = new Dice();
        dice.CreateDice(new Vector2(0 - 2*x_resolution * .15f, 0), new Vector2(x_resolution * .05f, x_resolution * .05f), Dices);
        BoardBuilder.dice_3 = dice;

        dice = new Dice();
        dice.CreateDice(new Vector2(0 + 2 * x_resolution * .15f, 0), new Vector2(x_resolution * .05f, x_resolution * .05f), Dices);
        BoardBuilder.dice_4 = dice;

        BoardBuilder.dice_1.Active(false);
        BoardBuilder.dice_2.Active(false);
        BoardBuilder.dice_3.Active(false);
        BoardBuilder.dice_4.Active(false);

        SetCoords();
       
    }

    void SetHomeIds()
    {
        BoardBuilder.home_W_id.Add(12);
        BoardBuilder.home_W_id.Add(13);
        BoardBuilder.home_W_id.Add(14);
        BoardBuilder.home_W_id.Add(15);
        BoardBuilder.home_W_id.Add(16);
        BoardBuilder.home_W_id.Add(17);
        BoardBuilder.home_W_id.Add(-2);

        BoardBuilder.home_B_id.Add(11);
        BoardBuilder.home_B_id.Add(10);
        BoardBuilder.home_B_id.Add(9);
        BoardBuilder.home_B_id.Add(8);
        BoardBuilder.home_B_id.Add(7);
        BoardBuilder.home_B_id.Add(6);
        BoardBuilder.home_B_id.Add(-2);
    }

    void SetChips( string place_id, PlaseStstus status )
    {
        int i = 0, len = 15;
        List<int> home = new List<int>();
        if (status == PlaseStstus.Black)
        {
            BoardBuilder.Places[place_id].setBlackhead(true);
            home = BoardBuilder.home_B_id;
        }
        else if (status == PlaseStstus.White)
        {
            BoardBuilder.Places[place_id].setWhitehead(true);
            home = BoardBuilder.home_W_id;
        }
        string Idd = place_id;
        for (i = 0; i < len; i++)
        {
            string key = "Chip_" + status.ToString() + "_" + i.ToString();
            Chip chip = new Chip(key);
            chip.ID = i;
            //Debug.Log(key);
            if (AlAtHome)
            {
                int rand = Random.Range(0, 6);
                Idd = "Places" + home[rand].ToString();
            }
            chip.AddOject(ChipSprite, BoardBuilder.Places[Idd].getGameObject().transform.position);
            chip.setScale(new Vector2(x_resolution * .05f, x_resolution * .05f));
            chip.setLayer(3+i);
            chip.setStatus(status);
            chip.place = BoardBuilder.Places[Idd];
            BoardBuilder.Places[Idd].AddChip(chip);
            BoardBuilder.Places[Idd].setStatus(status);
            chip.setName("Chip_"+ status.ToString() + "_" + i.ToString());
            chip.setCirclCollider(1.25f);
            chip.AddCollisator(gamePlay);
            chip.setLayer("ChipLayer");
            
            chip.AddRigidBody();
            switch (status)
            {
                case PlaseStstus.White:
                    chip.setColor(WhiteColor);
                    BoardBuilder.WhiteChip.Add(key, chip);
                    break;
                case PlaseStstus.Black:
                    chip.setColor(BlackColor);
                    BoardBuilder.BlackChip.Add(key, chip);
                    break;
            }
            if (AlAtHome)
                BoardBuilder.Places[Idd].Sorting();
        }
        if (!AlAtHome)
            BoardBuilder.Places[Idd].Sorting();
    }

    void SetCoords()
    {
        int i = 0, j = 23, l = 0;
        string key = "Places";
        for(i = 0; i<12; i++)
        {
            BoardBuilder.coordsW.Add(i);
            BoardBuilder.coordsB.Add(j);
            BoardBuilder.Places[key+i].cordw = i;
            BoardBuilder.Places[key+j].cordb = i;
            j--;
            l++;
        }
        j = 23;
        for (i = 0; i < 12; i++)
        {
            BoardBuilder.coordsB.Add(i);
            BoardBuilder.coordsW.Add(j);
            BoardBuilder.Places[key+j].cordw = i + 12;
            BoardBuilder.Places[key+i].cordb = i + 12;
            j--;
            l++;
        }
        
    }
}
