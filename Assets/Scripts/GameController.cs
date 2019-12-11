using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

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
    int score;
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
    public Text scoreText;
    public Text timeText;
    

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

        timeText.text = "TIME: " + turnTimer * turn;
        scoreText.text = "Score: " + score; 

        //Create array
        for (int k = 0; k < gridY; k++)
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

    //All PLUS logic uses the center of the plus as the reference for other cubes
    bool IsRainbowPlus (int x, int y)
    {
        Color a = cubeArr[x, y].GetComponent<Renderer>().material.color;
        Color b = cubeArr[x + 1, y].GetComponent<Renderer>().material.color;
        Color c = cubeArr[x - 1, y].GetComponent<Renderer>().material.color;
        Color d = cubeArr[x, y + 1].GetComponent<Renderer>().material.color;
        Color e =cubeArr[x, y - 1].GetComponent<Renderer>().material.color;

        if(a == Color.white || a == Color.black ||
           b == Color.white || b == Color.black ||
           c == Color.white || c == Color.black ||
           d == Color.white || d == Color.black ||
           e == Color.white || e == Color.black)
        {
            return false;
        }

        if(a != b && a!= c && a!= d && a != e &&
           b != c && b != d && b != e &&
           c != d && c != e && 
           d != e)
        {
            return true;
        }

        else
        {
            return false;
        }
    }

    //Checks using center cube for other, same color, cubes
    bool IsMonoColorPlus (int x, int y)
    {
        if (cubeArr[x, y].GetComponent<Renderer>().material.color != Color.white && cubeArr[x, y].GetComponent<Renderer>().material.color != Color.black &&
            cubeArr[x, y].GetComponent<Renderer>().material.color == cubeArr[x + 1, y].GetComponent<Renderer>().material.color &&
            cubeArr[x, y].GetComponent<Renderer>().material.color == cubeArr[x - 1, y].GetComponent<Renderer>().material.color &&
            cubeArr[x, y].GetComponent<Renderer>().material.color == cubeArr[x, y + 1].GetComponent<Renderer>().material.color &&
            cubeArr[x, y].GetComponent<Renderer>().material.color == cubeArr[x, y - 1].GetComponent<Renderer>().material.color)
        {
            return true;
        }

        else
        {
            return false;
        }
    }

    void MakeBlackPlus(int x, int y)
    {
        //Check that we are not on the edge of the grid (where no plus can be found)
        if(x == 0 || gridX - 1 == 0 || y == 0 || gridY - 1 == 0)
        {
            return; //End method
        }

        cubeArr[x, y].GetComponent<Renderer>().material.color = Color.black;
        cubeArr[x+1, y].GetComponent<Renderer>().material.color = Color.black;
        cubeArr[x-1, y].GetComponent<Renderer>().material.color = Color.black;
        cubeArr[x, y+1].GetComponent<Renderer>().material.color = Color.black;
        cubeArr[x, y-1].GetComponent<Renderer>().material.color = Color.black;
    }

    void Score()
    {
        //Reverse girdX and gridY????
        //Check entire grid for plus symbols (start at 1 because 0 is the edge of the grid)
        for (int y = 1; y < gridY - 1; y++)
        {
            for (int x = 1; x < gridX - 1; x++)
            {
                //when rainbow plus is found
                if(IsRainbowPlus (x,y))
                {
                    score += rainbowMatchPts;
                    MakeBlackPlus(x, y);
                }

                //when a mono color plus is found
                if (IsMonoColorPlus(x, y))
                {
                    score += monoMatchPts;
                    MakeBlackPlus(x, y);
                }
            }
        }
    }

    //Check game time. Exhange "Next Cube". Display text. Look for keyboard input. Check for matches. Check win/lose conditions.
    void Update()
    {

        if (Time.time < gameTimer)
        {
            ProcessKeyboardClick();

            //Check total game time against Unity Time.time and the the time for turns
            if (Time.time > turnTimer * turn)
            {
                timeText.text = "TIME: " + turnTimer * turn;
                turn++;
                Score();

                //if the nextCube has not been placed, lose score and place black cube
                if (nextCube != null)
                {
                    //FIX THIS ERROR FOR FINAL Black cubes need a to be stored in list so that they don't turn an already black cube black again
                    //Prevent score from being negative
                    if (score > 0)
                    {
                        score--;
                    }

                    blackCube = ChooseBlackCube();
                    blackCube.GetComponent<Renderer>().material.color = Color.black;
                    print("Black cube placed at: " + blackCube.GetComponent<CubeController>().myX + ", " + blackCube.GetComponent<CubeController>().myY);
                }

                GenerateNextCube();
            }

            if (cubeIsActive)
            {
                activeCube.GetComponent<Transform>().Rotate(Vector3.up, 150f * Time.deltaTime);
                activeCube.GetComponent<Transform>().Rotate(Vector3.forward, 150f * Time.deltaTime);
            }

            else if (cubeIsActive == false && lastActiveCube != null)
            {
                lastActiveCube.GetComponent<Transform>().rotation = Quaternion.identity;
            }

            if (activeCube != null && activeCube.GetComponent<Renderer>().material.color == Color.black)
            {
                activeCube = null;
                cubeIsActive = false;
            }

            scoreText.text = "Score: " + score; //CHANGE SCORE LOCATION
        }

        //After 60 seconds...
        else
        {
            if(score > 0)
            {
                //Function could be named better...
                endGame(true);
                print("You win: Time is up and your score was positive!");
            }

            else
            {
                endGame(false);
                print("You lose: Time is up and your score was zero!");
            }
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

        if((Input.GetKeyDown(KeyCode.Alpha1) || Input.GetKeyDown(KeyCode.Keypad1)) && nextCube != null)
        {
            ChooseWhiteCube(4);
        }

        if ((Input.GetKeyDown(KeyCode.Alpha2) || Input.GetKeyDown(KeyCode.Keypad2)) && nextCube != null)
        {
            ChooseWhiteCube(3);
        }

        if ((Input.GetKeyDown(KeyCode.Alpha3) || Input.GetKeyDown(KeyCode.Keypad3)) && nextCube != null)
        {
            ChooseWhiteCube(2);
        }

        if ((Input.GetKeyDown(KeyCode.Alpha4) || Input.GetKeyDown(KeyCode.Keypad4)) && nextCube != null)
        {
            ChooseWhiteCube(1);

        }

        if ((Input.GetKeyDown(KeyCode.Alpha5) || Input.GetKeyDown(KeyCode.Keypad5)) && nextCube != null)
        {
            ChooseWhiteCube(0);
        }
    }

    void ChooseWhiteCube(int rowReference) //Searches for white cube when keyboard is clicked
    {
        chosenCube = FindWhiteCubes(rowReference);

        if(chosenCube == null)
        {
            endGame(false);
            print("You lose: You tried to place a cube in a row that was full!");
        }

        //chosenCube will always be white, but using else if prevents this from running when FindWhiteCubes returns null
        else if(chosenCube.GetComponent<Renderer>().material.color == Color.white)
        {
            chosenCube.GetComponent<Renderer>().material.color = nextCube.GetComponent<Renderer>().material.color;
            print("The chosenCube was placed at: " + chosenCube.GetComponent<CubeController>().myX + "," + chosenCube.GetComponent<CubeController>().myY);
            Destroy(nextCube);
        }
    }

    GameObject FindWhiteCubes(int rowReference) //Returns a random white cube from a list
    {
        List<GameObject> whiteCubes = new List<GameObject>();

        //Run through chosen (y) row and put any white cubes into the list
        for (int i = 0; i < gridX; i++)
        {
            if (cubeArr[i, rowReference].GetComponent<Renderer>().material.color == Color.white)
            {
                whiteCubes.Add(cubeArr[i, rowReference]);
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
        Destroy(nextCube);
        nextCube = Instantiate(cubePrefab, new Vector3(7, 10, 0), Quaternion.identity);
        nextCube.GetComponent<Renderer>().material.color = cubeColorArr[Random.Range(0, 5)];
    }

    void endGame(bool win)
    {
        if(win)
        {
            SceneManager.LoadScene("WinScene", LoadSceneMode.Single);
        }

        else
        {
            SceneManager.LoadScene("LoseScene", LoadSceneMode.Single);
        }
    }

    GameObject ChooseBlackCube() //Returns a random white cube from a list to be turned black
    {
        List<GameObject> whiteCubes = new List<GameObject>();

        //Run through entire grid and add non-black cubes into list
        for (int i = 0; i < gridX; i++)
        {
            for(int k = 0; k < gridY; k++)
            {
                if (cubeArr[i, k].GetComponent<Renderer>().material.color == Color.white)
                {
                    whiteCubes.Add(cubeArr[i, k]);
                }
            }
        }

        //If grid is full
        if (whiteCubes.Count == 0)
        {
            return null;
        }


        //Returns a single random cube from the available black cubes
        return whiteCubes[Random.Range(0, whiteCubes.Count)];
    }
}