using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BirdNestStateController : MonoBehaviour
{
    public static BirdNestStateController instance { get; private set; }

    public Sprite nest1;
    public Sprite nest2;
    public Sprite nest3;
    public Sprite nest4;
    public Sprite nest5;
    public Sprite nest6;
    public Sprite nest7;
    public Sprite nest8;


    int stickCount = 0;
    Sprite[] spriteSheet = new Sprite[8];
    SpriteRenderer spriteRenderer;

    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        stickCount = 0;
        spriteSheet[0] = nest1;
        spriteSheet[1] = nest2;
        spriteSheet[2] = nest3;
        spriteSheet[3] = nest4;
        spriteSheet[4] = nest5;
        spriteSheet[5] = nest6;
        spriteSheet[6] = nest7;
        spriteSheet[7] = nest8;
    }

    private void Awake()
    {
        instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        
        if(stickCount != 0)
        {
            spriteRenderer.sprite = spriteSheet[stickCount - 1];
        }
    }

    public void StickAdd()
    {
        stickCount += 1;
    }
}
