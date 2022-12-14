using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cave_generation : MonoBehaviour, IActionable {

    [SerializeField]
    private int caveDifficulty = 3;

    private int numEnemies = 0;

    [SerializeField]
    private int CAVE_DIMENSIONS = 20;

    public Vector3 caveEntranceVector;

    [SerializeField]
    GameObject caveEvent;
    [SerializeField]
    GameObject caveWall;
    [SerializeField]
    GameObject caveFloor;
    [SerializeField]
    GameObject caveEntrance;

    [SerializeField]
    GameObject enemy;

    private int DEPTH = -100;



    [SerializeField]
    GameObject gold;

    [SerializeField]
    GameObject diamond;

    private bool[,] cave;

    private int caveSize = 150;

    [SerializeField]
    private int chanceToStartAlive = 40;
    [SerializeField]
    private int deathLimit = 3;
    [SerializeField]
    private int birthLimit = 4;
    [SerializeField]
    private int numSteps = 6;

    private bool doorPlaced = false;

    private int entranceXPos;
    private int entranceZPos;

    private GameObject caveParent;

    public void recieveAction()
    {
        MetaScript.preTeleport();
        MetaScript.getPlayer().GetComponent<DynamicGeneration>().enabled = false;
        MetaScript.getPlayer().transform.position = new Vector3(entranceXPos, DEPTH, entranceZPos);
        MetaScript.getPlayer().GetComponent<InBuilding>().setPlayerInBuilding(true);
        MetaScript.postTeleport();

    }

	// Use this for initialization
	void Start () {
        caveParent = new GameObject(gameObject.transform.position.x.ToString() + " + "  + gameObject.transform.position.z.ToString() +" cave");
        cave = new bool[CAVE_DIMENSIONS, CAVE_DIMENSIONS];
        initialiseCave();
        
        for(int i=0; i < numSteps; i++)
        {
            cave = updateCave();
        }
        instantiateCave();
        spawnMonsters();
        generatePickups();
        
    }

    public void instantiateCave()
    {
        int xStart = (int)gameObject.transform.position.x * CAVE_DIMENSIONS;
        int zStart = (int)gameObject.transform.position.z * CAVE_DIMENSIONS;
        GameObject temp;
        for (int x = -1; x < CAVE_DIMENSIONS+1; x++)
        {

            for (int z = -1; z < CAVE_DIMENSIONS + 1; z++)
            {

                if (z == -1 || x == -1 || z == CAVE_DIMENSIONS || x == CAVE_DIMENSIONS)
                {
                    temp = Instantiate(caveWall, new Vector3(x + xStart, DEPTH, z + zStart), Quaternion.identity);
                    temp.transform.SetParent(caveParent.transform);
                }
                else {
                    if (cave[x, z])
                    {
                        temp = Instantiate(caveWall, new Vector3(x + xStart, DEPTH, z + zStart), Quaternion.identity);
                        temp.transform.SetParent(caveParent.transform);
                    }
                    else
                    {
                        if (doorPlaced)
                        {
                            temp = Instantiate(caveFloor, new Vector3(x + xStart, DEPTH, z + zStart), Quaternion.identity);
                            caveEntranceVector = new Vector3(x + xStart, DEPTH, z + zStart);
                            temp.transform.SetParent(caveParent.transform);
                        }
                        else
                        {
                            GameObject door = Instantiate(caveEntrance, new Vector3(x + xStart, DEPTH, z + zStart), Quaternion.identity);
                            temp = Instantiate(caveFloor, new Vector3(x + xStart, DEPTH, z + zStart), Quaternion.identity);
                            doorPlaced = true;
                            entranceXPos = x + xStart;
                           
                            entranceZPos = z + zStart;
                            
                            door.GetComponentInChildren<door>().setReturn((int)gameObject.transform.position.x, 0, (int)gameObject.transform.position.z);
                            door.transform.SetParent(caveParent.transform);
                            temp.transform.SetParent(caveParent.transform);
                        }

                    }
                }
            }
            
        }
    }

    public bool [,] updateCave()
    {
        bool[,] newCave = new bool[CAVE_DIMENSIONS, CAVE_DIMENSIONS];
        for (int x = 0; x < CAVE_DIMENSIONS; x++)
        {
            for (int z = 0; z < CAVE_DIMENSIONS; z++)
            {
                if (cave[x, z])
                {
                    if (countAliveNeighbours(x, z) < deathLimit)
                    {
                        newCave[x, z] = false;
                    }
                    else
                    {
                        newCave[x, z] = true;
                    }
                }
                else
                {
                    if(countAliveNeighbours(x,z) > birthLimit)
                    {
                        newCave[x, z] = true;
                    }
                    else
                    {
                        newCave[x, z] = false;
                    }
                }
            }
        }
        return newCave;
    }

    public void spawnEvent()
    {
        GameObject temp;
        int xStart = (int)gameObject.transform.position.x * CAVE_DIMENSIONS;
        int zStart = (int)gameObject.transform.position.z * CAVE_DIMENSIONS;
        bool eventplaced = false;
        while (eventplaced == false) { }
        for (int x = 0; x < CAVE_DIMENSIONS; x++)
        {
            for (int z = 0; z < CAVE_DIMENSIONS; z++)
            {
                int randNum = Random.Range(0, 100);
                if (randNum < 2 && cave[x, z])
                {
                    temp = Instantiate(caveEvent, new Vector3(x + xStart, DEPTH, z + zStart), Quaternion.identity);
                    temp.transform.SetParent(caveParent.transform);


                }
            }
        }
    

    }

    public void initialiseCave()
    {
        
        for (int x = 0; x < CAVE_DIMENSIONS; x++)
        {
            for (int z= 0; z< CAVE_DIMENSIONS; z++)
            {
                int randNum = Random.Range(0, 100);
                if (randNum < chanceToStartAlive)
                {
                    
                    cave[x,z] = true;
                }
                else
                {
                    cave[x, z] = false;
                }
            }
        }
    }

    public int countAliveNeighbours(int x, int y)
    {
        int count = 0;
        for (int i = -1; i < 2; i++)
        {
            for (int j = -1; j < 2; j++)
            {
                int neighbourx = x + i;
                int neighboury = y + j;
                
            
                if (i == 0 && j == 0)
                {
                    //Do nothing, we don't want to add ourselves in!
                }
                //In case the index we're looking at it off the edge of the map
                else if (neighbourx < 0 || neighboury < 0 || neighbourx >= CAVE_DIMENSIONS -1 || neighboury >= CAVE_DIMENSIONS -1)
                {
                    count = count + 1;
                }
                //Otherwise, a normal check of the neighbour
                else if (cave[neighbourx, neighboury])
                {
                    count = count + 1;
                }
            }
        }
        return count;
    }
    public void generatePickups()
    {
        GameObject temp;
        int xStart = (int)gameObject.transform.position.x * CAVE_DIMENSIONS;
        int zStart = (int)gameObject.transform.position.z * CAVE_DIMENSIONS;
        for (int i = 0; i < CAVE_DIMENSIONS; i++)
        {
            for (int j = 0; j < CAVE_DIMENSIONS; j++)
            {
                if (!cave[i, j])
                {
                    if (Random.Range(0, 100) < caveDifficulty * 3)
                    {
                        temp = Instantiate(gold, new Vector3(i + xStart, DEPTH, j + zStart), Quaternion.identity);
                        temp.transform.SetParent(caveParent.transform);
                    }
                }
            }
        }
    }

    public void spawnMonsters()
    {
        int xStart = (int)gameObject.transform.position.x * CAVE_DIMENSIONS;
        int zStart = (int)gameObject.transform.position.z * CAVE_DIMENSIONS;
        for (int i =0; i < CAVE_DIMENSIONS; i++)
        {
            for (int j =0; j < CAVE_DIMENSIONS; j++)
            {
                if (!cave[i, j])
                {
                    if(Random.Range(0, 100) < caveDifficulty*3)
                    {
                        GameObject monster = Instantiate(enemy, new Vector3(i + xStart, DEPTH,j + zStart), Quaternion.identity);
                        monster.GetComponent<enemyMovement>().enabled = false;
                        monster.GetComponent<caveMovement>().enabled = true;
                        monster.transform.SetParent(caveParent.transform);
                    }
                }
            }
        }
    }
}
