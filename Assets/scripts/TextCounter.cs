using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextCounter : MonoBehaviour
{
    public static TextCounter instance { get; private set; }

    int count;

    Text text;

    private void Awake()
    {
        text = gameObject.GetComponent<Text>();
        instance = this;
        count = 0;
    }
    
    public void CounterAdd()
    {
        count++;
        text.text = count + "/8";
    }
}
