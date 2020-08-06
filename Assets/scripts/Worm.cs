using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Worm : MonoBehaviour
{
    Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        animator = gameObject.GetComponent<Animator>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        BirdController controller = other.GetComponent<BirdController>();

        if (controller != null)
        {
            controller.hungerRecover();
            WormSpawner.instance.wormDestroyed();
            Destroy(gameObject);
        }
    }

    
}
