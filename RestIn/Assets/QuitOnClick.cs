using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuitOnClick : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void OnMouseUpAsButton()
    {
        Application.Quit();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
