using System;
using UnityEngine;

public class WorldGen : MonoBehaviour
{
    public Transform levelPrefab;
    public Material[] floorMaterials;



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
            GenerateLevel(x);
        }

    }


    //ok there seriously has to be a better way of doing this...
    public Material GetFloorMaterial(FloorBlock.Color color) {

        //ensures that there are more materials than floorblock colors as otherwise there can be a null reference exception
        if(Enum.GetNames(typeof(FloorBlock.Color)).Length <= floorMaterials.Length){
            if (color == FloorBlock.Color.BLUE) {
                return floorMaterials[0];
            }
            if (color == FloorBlock.Color.BROWN)
            {
                return floorMaterials[1];
            }
            if (color == FloorBlock.Color.GRAY)
            {
                return floorMaterials[2];
            }
            if (color == FloorBlock.Color.GREEN)
            {
                return floorMaterials[3];
            }
            if (color == FloorBlock.Color.PINK)
            {
                return floorMaterials[4];
            }
            if (color == FloorBlock.Color.PURPLE)
            {
                return floorMaterials[5];
            }
            if (color == FloorBlock.Color.YELLOW)
            {
                return floorMaterials[6];
            }
        }
        return null;
    }


    //Generate a level (will be moved here eventually!!! >.<
    private void GenerateLevel(int index)
    {
        Level currentLevel = levels[index];
        currentLevel.index = index;
        for (int y = 0; y < levels[index].floorBlocks.Length; y++)
        {
            GenerateFloor(y, currentLevel);
        }
    }

    //Index = the floor's index within the current levels floorBlocks[]
    private void GenerateFloor(int index, Level currentLevel)
    {
        FloorBlock currentFloor = currentLevel.floorBlocks[index];
        //Floorblock is the actual transform we are modifying rn
        Transform floorBlock = Instantiate(levelPrefab, transform.GetChild(0));

        //Set the scale of the floor
        if (currentFloor.floorScale.x >= 0 && currentFloor.floorScale.y >= 0 && currentFloor.floorScale.z >= 0)
        {
            floorBlock.localScale = currentFloor.floorScale;
        }
        else
        {
            //Currently this will be spammed... but that's ok!! it really gets the point across then.
            Debug.Log("Floorblock scale must be greater than 0");
            return;
        }

        //  Set the position of the floor
        // A floor will be magnetic if magenticTo is 1. less than the length of total floors 2. greater than zero 3. not equal to the current floor index
        // Otherwise the position can be set wherever

        //TODO: make it so there can't be a loop of magneticism (that might be really hard....)
        bool lessTotalFloors = currentFloor.magneticTo < levels[currentLevel.index].floorBlocks.Length;
        bool gteZero = currentFloor.magneticTo >= 0;
        bool notCurrentFloor = !(currentFloor.magneticTo == index);

        if (lessTotalFloors && gteZero && notCurrentFloor)
        {
            
            FloorBlock magneticFloor = currentLevel.floorBlocks[currentFloor.magneticTo];

            float positionx = magneticFloor.floorPosition.x + magneticFloor.floorScale.x / 2 + currentFloor.floorScale.x / 2;
            float positiony = (currentLevel.index - numUndergroundLevels) * 5;
            float positionz = magneticFloor.floorPosition.y;

            currentFloor.floorPosition = new Vector2(positionx, positionz);

            floorBlock.localPosition = new Vector3(currentLevel.levelPosition.x + positionx, positiony, currentLevel.levelPosition.y + positionz);
            

        }
        else
        {
            float positionx = currentFloor.floorPosition.x;
            float positiony = (currentLevel.index - numUndergroundLevels) * 5;
            float positionz = currentFloor.floorPosition.y;
            floorBlock.localPosition = new Vector3(currentLevel.levelPosition.x + positionx, positiony, currentLevel.levelPosition.y + positionz);
            currentFloor.truePosition = new Vector3(positionx, positiony, positionz);
        }

        //Set the material of the floor

        //This is assuming the transform that takes the floor tiles is the first child of the floorblock prefab (WHICH IT SHOULD!!!!)
        Transform floorTile = floorBlock.GetChild(0);
        if (floorTile)
        {
            if (GetFloorMaterial(currentFloor.color))
            {
                floorTile.GetComponent<MeshRenderer>().material = GetFloorMaterial(currentFloor.color);
            }
        }
    }

    private void GenerateEnvironment()
    {
        //WOW very full and cool
    }
}


[System.Serializable]
public class Level
{
    public Vector2 levelPosition;
    public FloorBlock[] floorBlocks;

    [HideInInspector]
    public int index;
}

[System.Serializable]
public class FloorBlock
{
    public Vector2 floorPosition;

    [HideInInspector]
    public Vector2 truePosition;

    //all of the scales components must be >= to 0
    public Vector3 floorScale;
    public enum Color { BLUE, BROWN, GRAY, GREEN, PINK, PURPLE, YELLOW };
    public Color color;

    //it would be so cool for this to be a range between 0 and floorBlocks.length but idk how
    public int magneticTo;
}