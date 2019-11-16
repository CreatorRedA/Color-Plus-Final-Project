using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{

    float gameTimer;
    float turnTimer;
    GameObject[,] cubeArr;
    Color[] cubeColorArr;
    int gridX;
    int gridY;
    int monoMatchPts;
    int rainbowMatchPts;
    public GameObject cubePrefab;
    public static GameObject activeCube;
    public static GameObject nextCube; //Needed?
    int vectorX;
    int vectorY;

    // Start is called before the first frame update
    void Start()
    {
        //initialize variables 
        gridX = 8;
        gridY = 5;
        cubeArr = new GameObject[gridX-1, gridY-1];
        cubeColorArr = new Color[5];
        cubeColorArr[0] = Color.red;
        cubeColorArr[1] = Color.blue;
        cubeColorArr[2] = Color.yellow;
        cubeColorArr[3] = Color.green;
        cubeColorArr[4] = Color.magenta;

        gameTimer = 60f;
        turnTimer = 2.0f;
        monoMatchPts = 10;
        rainbowMatchPts = 5;

        //Create array
        for(int i = 0; i < gridX; i++)
        {

            for(int k = 0; k < gridY; k++)
            {
                SpawnCube();
                vectorX += 2;
            }

            //reset x and y
            vectorX = 0;
            vectorY += 2;
        }

    }

    // Update is called once per frame
    void Update()
    {
        

    }

    void SpawnCube() //Instantiates new cubes
    {
        Instantiate(cubePrefab,new Vector3(vectorX,vectorY,0),Quaternion.identity);
    }
    void OnMouseDown() //Search for mouse clicks
    {

    }
    void ProcessMouseClick(GameObject clickedCube) //ClickedCube becomes active cube or deactivates current activeCube. Highlight activeCube. 
    {

    }
    void ProcessKeyboardClick() //Look for keyboard input. Change color of cube in the grid based on the next cube to spawn. Destroy the next cube to spawn. 
    {

    }
    void ChooseWhiteCube() //Grabs white cube when turn ends if no button was pressed. Returns a GameObject for TurnCubeBlack().
    {

    }
    void TurnCubeBlack(GameObject randomWhiteCube) //Check for white cubes. Turn a random white cube black when turn ends if no button was pressed.
    {

    }
    void GenerateNextCube(Color nextColor) //Receives random color from Colors[] called nextColor. Creates next cube with nextColor.
    {

    }
    void checkGameState(bool timeIsUp, bool gridIsFull) //Checks timer. Checks grid space. 
    {

    }
    void CheckScore() //Does X
    {

    }
}
