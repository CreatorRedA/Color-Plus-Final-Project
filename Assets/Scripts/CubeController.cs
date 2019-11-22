using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeController : MonoBehaviour
{
    //Track individual cube positions
    public int myX;
    public int myY;
    public bool isMovable;

    //Empty
    void Start()
    {

    }

    //Empty
    void Update()
    {
        
    }

    void OnMouseDown() //Search for mouse clicks. Call ProcessMouseClick (GameController)
    {
        GameController.ProcessMouseClick(gameObject);
    }
    void OnMouseOver() //Changes size of cubePrefab to show it can be interacted with
    {
        GetComponent<Transform>().localScale = new Vector3(1.5f, 1.5f, 1.5f);
    }
    void OnMouseExit() //Resets size of cubePrefab when no longer interacting with it
    {
        GetComponent<Transform>().localScale = new Vector3(1, 1, 1);
    }
}
