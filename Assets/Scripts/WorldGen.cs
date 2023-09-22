using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldGen : MonoBehaviour
{
    public Transform levelPrefab;

    [Range(0, 5)]
    public int numUndergroundLevels;
    public Level[] levels;

    void Start()
    {
        GenerateWorld();
    }

    public void GenerateWorld()
    {
        string holderName = "Generated World";
        if (transform.Find(holderName))
        {
            DestroyImmediate(transform.Find(holderName).gameObject);
        }

        Transform worldHolder = new GameObject(holderName).transform;
        worldHolder.parent = transform;

        //Generate levels
        for (int x = 0; x < levels.Length; x++)
        {
            Level currentLevel = levels[x];

            for (int y = 0; y < levels[x].floorBlocks.Length; y++)
            {
                FloorBlock currentFloor = currentLevel.floorBlocks[y];


                //how to add floors seemlessly together?
                Transform floorBlock = Instantiate(levelPrefab, worldHolder);
                floorBlock.localPosition = new Vector3(currentLevel.levelPosition.x + currentFloor.floorPosition.x, (x - numUndergroundLevels) * 5, currentLevel.levelPosition.y + currentLevel.floorBlocks[y].floorPosition.y);
                floorBlock.localScale = currentFloor.floorScale;
            }

        }

    }

    //Generate a level (will be moved here eventually!!! >.<
    public void GenerateLevel()
    {

    }
}


[System.Serializable]
public class Level
{
    public Vector2 levelPosition;
    public Vector3 levelScale;
    public FloorBlock[] floorBlocks;
}


[System.Serializable]
public class FloorBlock
{
    public Vector2 floorPosition;
    public Vector3 floorScale;
}
