using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dice : GameElement
{
    List<GameElement> Dises  = new List<GameElement>();
    public int Value = 1;

    public Dice()
    {

    }

    public void Active(bool triger)
    {
        Dises[0].getGameObject().SetActive(triger);
    }


    public void CreateDice(Vector2 position, Vector2 scale, List<Sprite> sprites)
    {
        for(int i = 0; i< sprites.Count; i++)
        {
            GameElement elem = new GameElement();
            elem.AddOject(sprites[i], position);
            elem.setScale(scale);
            if (i > 0)
            {
                elem.setPArent(Dises[0].getGameObject());
                elem.setLayer(4);
            }
            else elem.setLayer(3);
            Dises.Add(elem);
            Dises[i].setLayer("DiceLayer");
        }
        
        OnDiceNum(1);
    }

    public void SetAlpha()
    {
        Dises[0].SetAlpha();
    }

    public void resetAlpha()
    {
        Dises[0].retAlpha();
    }

    void HideNums()
    {
        for (int i = 1; i < Dises.Count; i++)
        {
            Dises[i].getGameObject().SetActive(false);
        }
    }

    public void OnDiceNum(int i)
    {
        HideNums();
        Value = i;
            Dises[i].getGameObject().SetActive(true);
      }

    public void DropDice( Vector2 position)
    {
        int i = 0, len = Dises.Count;
        for(i = 0; i<len; i++)
        {
            Dises[i].getGameObject().transform.position = position;
        }
        
    }

    public int  randomization()
    {
        Value = Random.Range(1, 7);
        return Value;
    }

}
