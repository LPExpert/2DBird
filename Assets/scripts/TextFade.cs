using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextFade : MonoBehaviour
{

    public float time = 3.0f;
    float timer;

    Text text;
    Color baseColor;

    private void Awake()
    {
        timer = time;
        text = gameObject.GetComponent<Text>();
        baseColor = text.color;
    }

    // Update is called once per frame
    void Update()
    {
        if(timer - Time.deltaTime > 0)
        {
            timer -= Time.deltaTime;
        }
        else
        {
            baseColor.a -= 0.75f * Time.deltaTime;
            text.color = baseColor;
        }
    }
}
