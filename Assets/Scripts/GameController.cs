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
    int points;
    public GameObject cubePrefab;
    public static GameObject activeCube;
    public static GameObject nextCube;
    public static GameObject chosenCube;
    public static GameObject blackCube;
    public static GameObject lastActiveCube;
    bool cubeIsActive;
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

        GenerateNextCube();

    }

    //Check game time. Exhange "Next Cube". Display text. Look for keyboard input. Check for matches. Check win/lose conditions.
    void Update()
    {
        //Check total game time against Unity Time.time and the the time for turns
        if(Time.time > turnTimer * turn)
        {
            //if the nextCube has not been placed, lose points and place black cube
            if (nextCube != null)
            {
                points--;
                blackCube = cubeArr[Random.Range(0, 8), Random.Range(0, 5)];
                blackCube.GetComponent<Renderer>().material.color = Color.black;
                print("Black cube placed at: " + blackCube.GetComponent<CubeController>().myX + ", " + blackCube.GetComponent<CubeController>().myY);
            }

            Destroy(nextCube);
            GenerateNextCube();
            turn++;
        }

        ProcessKeyboardClick();

        if(cubeIsActive)
        {
            activeCube.GetComponent<Transform>().Rotate(Vector3.up, 150f * Time.deltaTime);
            activeCube.GetComponent<Transform>().Rotate(Vector3.forward, 150f * Time.deltaTime);
        }

        else if(cubeIsActive == false && lastActiveCube != null)
        {
            lastActiveCube.GetComponent<Transform>().rotation = Quaternion.identity;
        }
    }

    public void ProcessMouseClick(GameObject clickedCube) //If color is != white and != black, then ClickedCube becomes active cube or deactivates if currently the activeCube. Highlight activeCube. 
    {
        print("Cube at " + clickedCube.GetComponent<CubeController>().myX + ", " + clickedCube.GetComponent<CubeController>().myY);
        
        if(clickedCube.GetComponent<Renderer>().material.color != Color.white && clickedCube.GetComponent<Renderer>().material.color != Color.black && (clickedCube != nextCube))
        {
            if(clickedCube != activeCube)
            {
                activeCube = clickedCube;
                cubeIsActive = true;
            }

            else
            {
                //Store the last activeCube to stop it's rotation/change its size after no longer active
                //Store activeCube in lastActiveCube for later reference, then deactive it
                lastActiveCube = activeCube;
                activeCube = null;
                cubeIsActive = false;
            }
        }

        else if(clickedCube.GetComponent<Renderer>().material.color == Color.white && activeCube != null)
        {
            int xDist = clickedCube.GetComponent<CubeController>().myX - activeCube.GetComponent<CubeController>().myX;
            int yDist = clickedCube.GetComponent<CubeController>().myY - activeCube.GetComponent<CubeController>().myY;

            //if within 1 space
            if (Mathf.Abs (yDist) <= 1 && Mathf.Abs (xDist) <= 1)
            {
                //make clickedCube the same color as the current activeCube
                clickedCube.GetComponent<Renderer>().material.color = activeCube.GetComponent<Renderer>().material.color;

                //set old activeCube to be white and change activeCube
                activeCube.GetComponent<Renderer>().material.color = Color.white;
                activeCube = clickedCube;
                cubeIsActive = true;

            }
        }
    }
    public void ProcessKeyboardClick() //Look for keyboard input. Place nextCube in row (1/2/3/4/5). Change color of cube in the grid based on the next cube to spawn. Destroy the next cube to spawn. Utilizes ChooseWhiteCube(). Utilize Input.GetKeyDown() 
    {
        int numKeyPressed = 0;

        if((Input.GetKeyDown(KeyCode.Alpha1) || Input.GetKeyDown(KeyCode.Keypad1)) && nextCube != null)
        {
            numKeyPressed = 1;
            ChooseWhiteCube(0);
        }

        if ((Input.GetKeyDown(KeyCode.Alpha2) || Input.GetKeyDown(KeyCode.Keypad2)) && nextCube != null)
        {
            numKeyPressed = 2;
            ChooseWhiteCube(1);
        }

        if ((Input.GetKeyDown(KeyCode.Alpha3) || Input.GetKeyDown(KeyCode.Keypad3)) && nextCube != null)
        {
            numKeyPressed = 3;
            ChooseWhiteCube(2);
        }

        if ((Input.GetKeyDown(KeyCode.Alpha4) || Input.GetKeyDown(KeyCode.Keypad4)) && nextCube != null)
        {
            numKeyPressed = 4;
            ChooseWhiteCube(3);

        }

        if ((Input.GetKeyDown(KeyCode.Alpha5) || Input.GetKeyDown(KeyCode.Keypad5)) && nextCube != null)
        {
            numKeyPressed = 5;
            ChooseWhiteCube(4);
        }
    }

    void ChooseWhiteCube(int rowReference) //Searches for white cube when keyboard is clicked
    {
        chosenCube = FindWhiteCubes(rowReference);

        if(chosenCube == null)
        {
            endGame(true);
        }

        //chosenCube will always be white, but using else if prevents this from running when FindWhiteCubes returns null
        else if(chosenCube.GetComponent<Renderer>().material.color == Color.white)
        {
            chosenCube.GetComponent<Renderer>().material.color = nextCube.GetComponent<Renderer>().material.color;
            print("The chosenCube was placed at: " + chosenCube.GetComponent<CubeController>().myX + "," + chosenCube.GetComponent<CubeController>().myY);
            Destroy(nextCube);
        }
    }

    GameObject FindWhiteCubes(int y) //Returns a random white cube from a list
    {
        List<GameObject> whiteCubes = new List<GameObject>();

        //Run through chosen (y) row and put any white cubes into the list
        for (int i = 0; i < gridX; i++)
        {
            if (cubeArr[i, y].GetComponent<Renderer>().material.color == Color.white)
            {
                whiteCubes.Add(cubeArr[i, y]);
            }
        }

        //If no white cubes are found...
        if (whiteCubes.Count == 0)
        {
            return null;
        }

        //Returns a single random cube from the available white cubes
        return whiteCubes[Random.Range(0, whiteCubes.Count)];
    }

    void GenerateNextCube() //Receives random color from Colors[] called nextColor. Creates next cube with nextColor.
    {
        nextCube = Instantiate(cubePrefab, new Vector3(7, 10, 0), Quaternion.identity);
        nextCube.GetComponent<Renderer>().material.color = cubeColorArr[Random.Range(0, 5)];
    }

    void endGame(bool lose)
    {
        if(lose)
        {
            print("You lose! Try again");
        }

        else
        {
            print("YOU WON!");
        }
    }
}

/* 
 * public void ProcessMouseClick(GameObject clickedCube) //If color is != white and != black, then ClickedCube becomes active cube or deactivates if currently the activeCube. Highlight activeCube. 
    {
        print("Cube at " + clickedCube.GetComponent<CubeController>().myX + ", " + clickedCube.GetComponent<CubeController>().myY);
        
        if(clickedCube.GetComponent<Renderer>().material.color != Color.white && clickedCube.GetComponent<Renderer>().material.color != Color.black)
        {
            if(clickedCube != activeCube)
            {
                activeCube = clickedCube;
                cubeIsActive = true;
                while(cubeIsActive)
                {
                    activeCube.GetComponent<Transform>().Rotate(Vector3.up, 150f * Time.deltaTime);
                    activeCube.GetComponent<Transform>().Rotate(Vector3.forward, 150f * Time.deltaTime);

                    if(clickedCube == activeCube)
                    {
                        //reset activeCube rotation
                        activeCube.GetComponent<Transform>().rotation = Quaternion.identity;
                        //turn off activeCube
                        activeCube = null;
                        cubeIsActive = false;
                        //Reset rotation of last cube
                    }
                }
            }
        }
    }
*/