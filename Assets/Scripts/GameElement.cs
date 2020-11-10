using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameElement
{
    GameObject element;
    SpriteRenderer renderer;

    float scale_x, scale_y;
    private Sprite bricksBlock;
    private Vector3 vector3;

    BoxCollider2D collider;
    CircleCollider2D c_collider;
    Collisator colisat;
    Rigidbody2D body;

    public void AddCollisator(GamePlay gmp)
    {
        colisat = element.AddComponent<Collisator>();
        colisat.gamePlay = gmp;
    }
    public BoxCollider2D getCollider()
    {
        return this.collider;
    }

    public CircleCollider2D getCColllider()
    {
        return this.c_collider;
    }
    public GameElement()
    {

    }

    public void setBoxColleder(Vector2 offset, Vector2 size)
    {
        collider = element.AddComponent<BoxCollider2D>();
        collider.offset = offset;
        collider.size = size;
        collider.isTrigger = true;
    }

    public void setCirclCollider( float Radius)
    {
        c_collider = element.AddComponent<CircleCollider2D>();
        c_collider.radius = Radius; // 1.25f;
        c_collider.isTrigger = true;
    }

    public void AddOject(Sprite sprite, Vector2 position)
    {
        element = new GameObject();
        renderer = element.AddComponent<SpriteRenderer>();
        renderer.sprite = sprite;
        element.transform.localPosition = position;
    }

    public void AddRigidBody()
    {
        body = element.AddComponent<Rigidbody2D>();
        //body.bodyType = RigidbodyType2D.Static;
        body.mass = 0.0001f;
        body.gravityScale = 0;
    }

    public void SetAlpha()
    {
        Color cl = new Color();
        cl.r = renderer.color.r;
        cl.g = renderer.color.g;
        cl.b = renderer.color.b;
        cl.a = 125;
        renderer.color = cl;
    }

    public void retAlpha()
    {
        Color cl = new Color();
        cl.r = renderer.color.r;
        cl.g = renderer.color.g;
        cl.b = renderer.color.b;
        cl.a = 255;
        renderer.color = cl;
    }

    public GameElement(Sprite bricksBlock, Vector3 vector3)
    {
        this.bricksBlock = bricksBlock;
        this.vector3 = vector3;
    }

    public void setScale(Vector2 scale)
    {
        element.transform.localScale = scale;
    }

    public void setPArent(GameObject parent)
    {
        element.transform.parent = parent.transform;
    }

    public void setColor(Color cl)
    {
        renderer.color = cl;
    }

    public void setName(string name)
    {
        element.name = name;
    }

    public GameObject getGameObject()
    {
        return element;
    }
    public void setLayer(int i)
    {
        renderer.sortingOrder = i;
    }
    public void setLayer(string layer)
    {
        renderer.sortingLayerName = layer;
    }
    public void Rotate()
    {
        element.transform.Rotate(new Vector3(0, 0, 180));
    }
}
