using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stick : MonoBehaviour
{

    public Sprite stick1;
    public Sprite stick2;


    // Start is called before the first frame update
    void Start()
    {
        SpriteRenderer spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        Sprite[] spriteList = {stick1, stick2 };
        int image = Mathf.RoundToInt(Random.value);
        spriteRenderer.sprite = spriteList[image];
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        BirdController controller = other.GetComponent<BirdController>();

        if (controller != null)
        {
            BirdNestStateController.instance.StickAdd();
            TextCounter.instance.CounterAdd();
            Destroy(gameObject);
        }
    }

}
