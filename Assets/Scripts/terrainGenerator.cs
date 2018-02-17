﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class terrainGenerator : MonoBehaviour
{

    //The map that shows the terrain value at each existing coordinate
    private Dictionary<string, terrain> terrainMap;
    private Dictionary<string, resource> resourceMap;
    public Dictionary<string, Chunk> loadedChunks;


    //Starting positions of the player
    public int xPlayerPos;
    public int yPlayerPos;
    public int xPLayerChunkPos;
    public int yPlayerChunkPos;

    /// Number of chunks that make up the world
    private static int WORLD_SIZE = 1;
    /// World starting point
    public static int SEED = 0;

    public static float chunkIntervalSeed = 5f;

    public bool DEBUG = false;

    public int xChunk;
    public int yChunk;
    /*
    //The y size of a generated section of the map
    public int chunkSizeY;
    //The x size of a generated section of the map
    public int chunkSizeX;
	*/

    //A divisor that determines the amount of water generated by the perlin noise function
    public float waterAmount;

    //A divisor that determines amount of specific terrain based on temperature
    public float terrainAmount;

    //A divisor that determines amount of trees 
    public float treeAmount;

    private float[] thresholds = new float[(int)terrain.GRASS];


    //terrains
    public GameObject Water;

    public GameObject Grass;

    public GameObject Dirt;

    public GameObject Mountain;

    public GameObject Snow;

    public GameObject Sand;

    public GameObject Desert;

    public GameObject Campsite;

    public GameObject Plot;


    //resources
    public GameObject Tree;

    public GameObject Stone;

    public GameObject Iron;

    public GameObject Gold;

    public GameObject Fish;

    public GameObject Berries;

    public GameObject Player;

    public GameObject Enemy;

    public GameObject NPC;


    //Affects the types of terrain that are generated
    public float terrainSeed;

    public float terrainSeed2;

    public float waterSeed;

    public float resourceSeed;

    public float resourceSeed2;

    // Enumerate terrain
    private enum terrain
    {
        WATER,
        DIRT,
        SNOW,
        MOUNTAIN,
        GRASS,
        SAND,
        DESERT,
        CAMPSITE,
        PLOT
    }

    private enum resource
    {
        TREE,
        IRON,
        STONE,
        GOLD,
        FISH,
        MEAT,
        BERRIES,
        NPC,
        ENEMY
    }

    private GameObject getResourceObject(resource r)
    {
        switch (r)
        {
            case resource.FISH:
                return Fish;
            case resource.MEAT:
            //return Meat;
            case resource.GOLD:
                return Gold;
            case resource.IRON:
                return Iron;
            case resource.BERRIES:
                return Berries;
            case resource.TREE:
                return Tree;
            case resource.STONE:
            return Stone;
            case resource.ENEMY:
                return Enemy;
            case resource.NPC:
                return NPC;
            default:
                return Tree;
        }
    }

    private float getThreshold(terrain t)
    {
        switch (t)
        {
            case terrain.WATER:
                return 0.2f;
            case terrain.SAND:
                return 0.23f;
            case terrain.DIRT:
                return 0.80f;
            case terrain.MOUNTAIN:
                return 0.50f;
            case terrain.SNOW:
                return 1.0f;
            case terrain.GRASS:
                return 0.65f;
            case terrain.DESERT:
                return 0.30f;
            default:
                return 0.1f;
        }
    }

    private float getResourceThreshold(resource r)
    {
        switch (r)
        {
            case resource.BERRIES:
                return 0.40f;
            case resource.TREE:
                return 0.45f;
            case resource.STONE:
                return 0.90f;
            case resource.MEAT:
                return 0.10f;
            case resource.IRON:
                return 0.46f;
            case resource.GOLD:
                return 0.10f;
            case resource.FISH:
                return 0.10f;
            default:
                return 1.0f;
        }
    }

    private GameObject getObject(terrain t)
    {
        switch (t)
        {
            case terrain.WATER:
                return Water;
            case terrain.SAND:
                return Sand;
            case terrain.DIRT:
                return Dirt;
            case terrain.MOUNTAIN:
                return Mountain;
            case terrain.SNOW:
                return Snow;
            case terrain.GRASS:
                return Grass;
            case terrain.DESERT:
                return Desert;
            case terrain.CAMPSITE:
                return Campsite;
            case terrain.PLOT:
                return Plot;
            default:
                return Grass;
        }
    }

    /// <summary>
    /// Gets the position to sample noise.
    /// </summary>
    /// <returns>The noise.</returns>
    /// <param name="val">Value.</param>
    float posNoise(int val, int chunk)
    {
        return (float)(val + chunk * Chunk.SIZE) / 20f + SEED;
    }
    // Use this for initialization
    public void Start()
    {
        loadedChunks = new Dictionary<string, Chunk>();
        terrainMap = new Dictionary<string, terrain>();
        resourceMap = new Dictionary<string, resource>();
        generateChunk(xChunk, yChunk);
        
    }

    public void FixedUpdate()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            int x = (int)player.transform.position.x / 25;
            int y = (int)player.transform.position.y / 25;
            
            if (!loadedChunks.ContainsKey(x + 1 + " " + y))
            {
                generateChunk(x + 1, y);
            }
            if (!loadedChunks.ContainsKey(x - 1 + " " + y))
            {
                generateChunk(x - 1, y);
            }
            if (!loadedChunks.ContainsKey(x + " " + y + 1))
            {
                generateChunk(x, y + 1);
            }
            if (!loadedChunks.ContainsKey(x + " " + (y - 1)))
            {
                generateChunk(x, y - 1);
            }
        }
    }


    /// <summary>
    /// Generates the chunk at xy chunk position.
    /// </summary>
    /// <returns>The chunk.</returns>
    /// <param name="xPos">X position.</param>
    /// <param name="yPos">Y position.</param>
    void generateChunk(int xPos, int yPos)
    {

        // If the chunk is alreaady loaded on screen return
        if (loadedChunks.ContainsKey(xPos + " " + yPos))
        {
            return;// loadedChunks [xPos + " " + yPos];
        }


        // Create chunk
        Chunk chunkMap = new Chunk();

        for (int y = 0; y < Chunk.SIZE; y++)
        {
            for (int x = 0; x < Chunk.SIZE; x++)
            {


                // Check if this tile is edited already
                if (!addTerrain(x, y, xPos, yPos, chunkMap, terrainMap))
                {
                    Debug.Log("Terrain was not correctly added to tile " + x + " " + y);
                }


                //Generate resources
                addResource(x, y, xPos, yPos, chunkMap, resourceMap);

            }
        }
        loadedChunks[xPos + " " + yPos] = chunkMap;
        GameObject tempTile;
        GameObject tempResource;
        for (int y = 0; y < Chunk.SIZE; y++)
        {
            for (int x = 0; x < Chunk.SIZE; x++)
            {
                Position worldPos = new Position(xPos * Chunk.SIZE + x, yPos * Chunk.SIZE + y);
                string key = worldPos.xCoord + " " + worldPos.yCoord;
                tempTile = Instantiate(getObject(terrainMap[key]), new Vector3(worldPos.xCoord, worldPos.yCoord, 0), Quaternion.identity);
                //Adds the terrain into the correct chunk into the first layer
                chunkMap.addTileAt(tempTile, x, y, 0);
                if(terrainMap[key] == terrain.CAMPSITE)
                {
                    Instantiate(Player ,new Vector3(worldPos.xCoord, worldPos.yCoord, 0), Quaternion.identity);
                }
                if (resourceMap.ContainsKey(key))
                {
                    tempResource = Instantiate(getResourceObject(resourceMap[key]), new Vector3(worldPos.xCoord, worldPos.yCoord, 0), Quaternion.identity);
                    chunkMap.addTileAt(tempTile, x, y, 1);
                }
                
            }

        }
    }

    bool addTerrain(int xCoord, int yCoord, int xChunkCoord, int yChunkCoord, Chunk chunk, Dictionary<string, terrain> terrainMap)
    {
        
        GameObject tempTile = null;
        Position worldPos = new Position(xChunkCoord * Chunk.SIZE + xCoord, yChunkCoord * Chunk.SIZE + yCoord);
        string key = worldPos.xCoord + " " + worldPos.yCoord;
        float xNoiseValue = posNoise(xCoord, xChunkCoord);
        float yNoiseValue = posNoise(yCoord, yChunkCoord);
        float waterVal = Mathf.PerlinNoise(xNoiseValue + waterSeed + chunkIntervalSeed * xChunk, yNoiseValue + waterSeed + chunkIntervalSeed * yChunk) / waterAmount;
        float terrainVal = Mathf.PerlinNoise(xNoiseValue + terrainSeed + chunkIntervalSeed * xChunk, yNoiseValue + terrainSeed + chunkIntervalSeed * yChunk) / terrainAmount;
        float terrainVal2 = Mathf.PerlinNoise(xNoiseValue + terrainSeed2 + chunkIntervalSeed * xChunk, yNoiseValue + terrainSeed2 + chunkIntervalSeed * yChunk) / terrainAmount;

        if (terrainMap.ContainsKey(key))
        {
            // Instantiate saved game object from terrain
            tempTile = getObject(terrainMap[key]);
        } else if (xChunkCoord == xPLayerChunkPos && yChunkCoord == yPlayerChunkPos && xCoord == xPlayerPos && yCoord == yPlayerPos)
        {
            for (int i = xPlayerPos - 1; i <= xPlayerPos + 1; i++)
            {
                for (int j = yPlayerPos - 1; j <= yPlayerPos + 1; j++)
                {
                    
                    if (i == xPlayerPos && j == yPlayerPos)
                    {
                       
                        worldPos = new Position(xChunkCoord * Chunk.SIZE + i, yChunkCoord * Chunk.SIZE + j);
                        key = worldPos.xCoord + " " + worldPos.yCoord;
                        if (terrainMap.ContainsKey(key))
                        {
                            terrainMap[key] = terrain.CAMPSITE;
                        }
                        else
                        {
                            terrainMap.Add(key, terrain.CAMPSITE);
                        }

                    }
                    else
                    {
                        
                        worldPos = new Position(xChunkCoord * Chunk.SIZE + i, yChunkCoord * Chunk.SIZE + j);
                        key = worldPos.xCoord + " " + worldPos.yCoord;
                        if (terrainMap.ContainsKey(key))
                        {
                            terrainMap[key] = terrain.PLOT;
                        }
                        else
                        {
                            terrainMap.Add(key, terrain.PLOT);
                        }
                    }
                }
            }
        }
        else if (waterVal < getThreshold(terrain.WATER))
        {
            if (!terrainMap.ContainsKey(key)) { 
            terrainMap.Add(key, terrain.WATER);
            
        }
        }
        else if (waterVal < getThreshold(terrain.SAND))
        {
            if (!terrainMap.ContainsKey(key))
            {
                terrainMap.Add(key, terrain.SAND);
                tempTile = Sand;
            }
        }

        else
        {
            if (terrainVal <= getThreshold(terrain.DESERT) && terrainVal2 <= getThreshold(terrain.DESERT))
            {
                if (!terrainMap.ContainsKey(key))
                {
                    terrainMap.Add(key, terrain.DESERT);
                    tempTile = Desert;
                }
            }
            else if (getThreshold(terrain.DESERT) < terrainVal && terrainVal <= getThreshold(terrain.MOUNTAIN)&& getThreshold(terrain.DESERT) < terrainVal2 && terrainVal2 <= getThreshold(terrain.MOUNTAIN))
            {
                if (!terrainMap.ContainsKey(key))
                {
                    terrainMap.Add(key, terrain.MOUNTAIN);
                    tempTile = Mountain;
                }
            }
            else 
            {
                if (!terrainMap.ContainsKey(key))
                {
                    terrainMap.Add(key, terrain.GRASS);
                    tempTile = Grass;
                }
            }           
        }
        
        //tempTile = Instantiate(tempTile, new Vector3(worldPos.xCoord, worldPos.yCoord, 0), Quaternion.identity);
        //Adds the terrain into the correct chunk into the first layer
        //chunk.addTileAt(tempTile, xCoord, yCoord, 0);
        return true;

    }


    void addResource(int xCoord, int yCoord, int xChunkCoord, int yChunkCoord, Chunk chunk, Dictionary<string, resource> resourceMap)
    {
        
        GameObject tempResource = null;
        Position worldPos = new Position(xChunkCoord * Chunk.SIZE + xCoord, yChunkCoord * Chunk.SIZE + yCoord);
        string key = worldPos.xCoord + " " + worldPos.yCoord;
        float xNoiseValue = posNoise(xCoord, xChunkCoord);
        float yNoiseValue = posNoise(yCoord, yChunkCoord);
        float waterVal = Mathf.PerlinNoise(xNoiseValue + waterSeed + chunkIntervalSeed * xChunk, yNoiseValue + waterSeed + chunkIntervalSeed * yChunk) / waterAmount;
        float resourceVal = Mathf.PerlinNoise(xNoiseValue + resourceSeed + chunkIntervalSeed * xChunk, yNoiseValue + resourceSeed + chunkIntervalSeed * yChunk) / treeAmount;
        float resourceVal2 = Mathf.PerlinNoise(xNoiseValue + resourceSeed2 + chunkIntervalSeed * xChunk, yNoiseValue + resourceSeed2 + chunkIntervalSeed * yChunk) / treeAmount;
        if (resourceMap.ContainsKey(key))
        {
            tempResource = getResourceObject(resourceMap[key]);
        }
        else if (waterVal < getThreshold(terrain.WATER))
        {

            if(Random.Range(0,30)< 2)
            {
                tempResource = Fish;
                resourceMap.Add(key, resource.FISH);
            }

        }
        else if (0 <= resourceVal && resourceVal < getResourceThreshold(resource.TREE) && 0 <= resourceVal2 && resourceVal2 < getResourceThreshold(resource.TREE))
        {

            if (terrainMap[key] == terrain.GRASS && !resourceMap.ContainsKey(key))
            {
                if (Random.Range(0, 30) < 2)
                {
                    tempResource = Berries;
                    resourceMap.Add(key, resource.BERRIES);
                }
                else
                {
                    tempResource = Tree;
                    resourceMap.Add(key, resource.TREE);
                }
                
            }
            
        }
        else if (0 <= resourceVal && resourceVal < getResourceThreshold(resource.IRON) && 0 <= resourceVal2 && resourceVal2 < getResourceThreshold(resource.IRON))
        {
            if (terrainMap[key] == terrain.MOUNTAIN)
            {
                tempResource = Iron;
                resourceMap.Add(key, resource.IRON);
            }
        }
        else if (getResourceThreshold(resource.STONE) - 0.4 <= resourceVal && resourceVal < getResourceThreshold(resource.STONE) && getResourceThreshold(resource.STONE) - 0.4 <= resourceVal2 && resourceVal2 < getResourceThreshold(resource.STONE))
        {
            if (terrainMap[key] == terrain.MOUNTAIN && !resourceMap.ContainsKey(key))
            {
                tempResource = Stone;
                resourceMap.Add(key, resource.STONE);
            }
        }
        else if (0 <= resourceVal && resourceVal < getResourceThreshold(resource.GOLD) && 0 <= resourceVal2 && resourceVal2 < getResourceThreshold(resource.GOLD))
        {
            tempResource = Gold;
            resourceMap.Add(key, resource.GOLD);
        }

            if (tempResource == null)
        {

            if (Random.Range(0, 100)< 3)
            {
                resourceMap.Add(key, resource.NPC);
            }
            if (Random.Range(0, 100) < 3)
            {
                resourceMap.Add(key, resource.ENEMY);
            }
        }
    }


        // Update is called once per frame
        void Update()
    {
        if (DEBUG)
        {
            Position changePos = new Position(0, 0);
            changePos.xCoord += Input.GetKeyDown(KeyCode.J) ? -1 : 0;
            changePos.xCoord += Input.GetKeyDown(KeyCode.L) ? 1 : 0;

            changePos.yCoord += Input.GetKeyDown(KeyCode.K) ? -1 : 0;
            changePos.yCoord += Input.GetKeyDown(KeyCode.I) ? 1 : 0;

            if (changePos.xCoord != 0 || changePos.yCoord != 0)
            {
                xChunk += changePos.xCoord;
                yChunk += changePos.yCoord;
                generateChunk(xChunk, yChunk);
            }
        }
    }
}
