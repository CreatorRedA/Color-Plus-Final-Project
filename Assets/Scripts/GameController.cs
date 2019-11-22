using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    //Declare variables
    float gameTimer;
    float turnTimer;
    int turn;
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
    int randomX;

    //Initialize variables. Create cube array. 
    void Start()
    {
        //initialize variables 
        gridX = 8;
        gridY = 5;
        vectorX = 0;
        vectorY = 0;
        randomX = Random.Range(0, 8);
        cubeArr = new GameObject[gridX, gridY];
        cubeColorArr = new Color[5];
        cubeColorArr[0] = Color.red;
        cubeColorArr[1] = Color.blue;
        cubeColorArr[2] = Color.yellow;
        cubeColorArr[3] = Color.green;
        cubeColorArr[4] = Color.magenta;

        gameTimer = 60f;
        turnTimer = 2.0f;
        turn = 1;
        monoMatchPts = 10;
        rainbowMatchPts = 5;

        //Create array
        for(int k = 0; k < gridY; k++)
        {

            for(int i = 0; i < gridX; i++)
            {
                cubeArr[i,k] = Instantiate(cubePrefab, new Vector3(vectorX, vectorY, 0), Quaternion.identity);
                vectorX += 2;

                cubeArr[i,k].GetComponent<CubeController>().myX = i;
                cubeArr[i,k].GetComponent<CubeController>().myY = k;
            }

            //reset x and y
            vectorX = 0;
            vectorY += 2;
        }

    }

    //Check game time. Exhange "Next Cube". Display text. Look for keyboard input. Check for matches. Check win/lose conditions.
    void Update()
    {
        //Check total game time against Unity Time.time and the the time for turns
        if(Time.time > turnTimer * turn)
        {
            Destroy(nextCube);
            GenerateNextCube();
            turn++;
        }

    }

    public static void ProcessMouseClick(GameObject clickedCube) //If color is != white and != black, then ClickedCube becomes active cube or deactivates if currently the activeCube. Highlight activeCube. 
    {
        print("Cube at " + clickedCube.GetComponent<CubeController>().myX + ", " + clickedCube.GetComponent<CubeController>().myY);
    }
    public void ProcessKeyboardClick(GameObject whiteCubeReference) //Look for keyboard input. Place nextCube in row (1/2/3/4/5). Change color of cube in the grid based on the next cube to spawn. Destroy the next cube to spawn. Utilizes ChooseWhiteCube(). Utilize Input.GetKeyDown() 
    {

    }
    void ChooseWhiteCube() //Grabs a white cube when turn ends if no button was pressed. Returns a GameObject for TurnCubeBlack(). Returns a GameObject for ProcessKeyboard Click().
    {

    }
    void GenerateNextCube() //Receives random color from Colors[] called nextColor. Creates next cube with nextColor.
    {
        nextCube = Instantiate(cubePrefab, new Vector3(7, 10, 0), Quaternion.identity);
        nextCube.GetComponent<Renderer>().material.color = cubeColorArr[Random.Range(0, 5)];
    }
    void checkGameState(bool timeIsUp, bool gridIsFull, bool scoreIsPositive) //Checks timer. Checks grid space. 
    {

    }
}
