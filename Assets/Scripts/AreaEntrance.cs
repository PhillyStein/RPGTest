using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaEntrance : MonoBehaviour
{

    public string transitionName;
    public UIFade fade;

    // Start is called before the first frame update
    void Start()
    {
        if(transitionName == PlayerController.instance.areaTransitionName)
        {
            PlayerController.instance.transform.position = transform.position;
        }

        if(fade == null) {
            fade = FindObjectOfType<UIFade>();
        }

        fade.FadeFromBlack();
        GameManager.instance.fadingBetweenAreas = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
