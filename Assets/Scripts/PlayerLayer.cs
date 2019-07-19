using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLayer : MonoBehaviour
{
    public string fromLayer;
    public string newLayer;
    private SpriteRenderer sprite;

    // Start is called before the first frame update
    void Start()
    {
        sprite = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            if(fromLayer == "PlayerStart")
            {
                newLayer = "PlayerMidway";
                fromLayer = "PlayerMidway";
            } else if(fromLayer == "PlayerMidway") {
                newLayer = "PlayerStart";
                fromLayer = "PlayerStart";
            }

            sprite.sortingLayerName = newLayer;
            
        }
    }
}
