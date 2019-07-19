using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class WallTransparency : MonoBehaviour
{
    public bool playerTouch;

    // Start is called before the first frame update
    void Start()
    {
        playerTouch = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(playerTouch)
        {
            //this.GetComponent<SpriteRenderer>().material.color = new Color(1.0f, 1.0f, 1.0f, 0.5f);
            this.GetComponent<Tilemap>().color = new Color(1.0f, 1.0f, 1.0f, 0.5f);
        }
        else
        {
            //this.GetComponent<SpriteRenderer>().material.color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
            this.GetComponent<Tilemap>().color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
        }
    }
    
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            playerTouch = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            playerTouch = true;
        } 
    }
}
