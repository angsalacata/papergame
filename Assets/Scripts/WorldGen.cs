using System;
using UnityEngine;
using UnityEngine.UIElements;

public class WorldGen : MonoBehaviour
{
    public Transform levelPrefab;
    public Material[] floorMaterials;

    [Range(0, 5)]
    public int numUndergroundLevels;
    //how close a floor block needs to be to its magnetic floor block before sticking. Must be greater than 0.
    public float magneticTolerance;
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

        // Set the position of the floor
        
        // A floor will be magnetic if magenticTo is 1. less than the length of total floors 2. greater than zero 3. not equal to the current floor index
        // Otherwise the position can be set wherever

        //TODO: make it so there can't be a loop of magneticism (that might be really hard....)
        bool lessTotalFloors = currentFloor.magneticTo < levels[currentLevel.index].floorBlocks.Length;
        bool gteZero = currentFloor.magneticTo >= 0;
        bool notCurrentFloor = !(currentFloor.magneticTo == index);

        //I am 80% sure this is unnecessary but I don't want to break anything so im leaving it! .-.
        currentFloor.truePosition = new Vector2(currentFloor.floorPosition.x,currentFloor.floorPosition.y);
        if (lessTotalFloors && gteZero && notCurrentFloor)
        {
            
            FloorBlock magneticFloor = currentLevel.floorBlocks[currentFloor.magneticTo];

            //Magneticism is potentially possible by checking if the area of two floor blocks are interecting and then have them not intersect (easier said than done >-<)
            //This could also be done with colliders but that might be expensive...
            //The important this is that this works even though this is a lot of if statements

            //difference between the x position of the NORTH edge of the current floor block and the x position of the SOUTH edge of the magnetic floor block.
            float northEdgeDiff = (currentFloor.floorPosition.y + currentFloor.floorScale.z / 2) - (magneticFloor.truePosition.y - magneticFloor.floorScale.z / 2);
            //difference between the x position of the SOUTH edge of the current floor block and the x position of the NORTH edge of the magnetic floor block.
            float southEdgeDiff = (currentFloor.floorPosition.y - currentFloor.floorScale.z / 2) - (magneticFloor.truePosition.y + magneticFloor.floorScale.z / 2);
            //difference between the x position of the WEST edge of the current floor block and the x position of the EAST edge of the magnetic floor block.
            float westEdgeDiff = (currentFloor.floorPosition.x - currentFloor.floorScale.x / 2) - (magneticFloor.truePosition.x + magneticFloor.floorScale.x / 2);
            //difference between the x position of the EAST edge of the current floor block and the x position of the WEST edge of the magnetic floor block.
            float eastEdgeDiff = (currentFloor.floorPosition.x + currentFloor.floorScale.x / 2) - (magneticFloor.truePosition.x - magneticFloor.floorScale.x / 2);

            //This checks that the north and south edges of the current block fall inside the north and south edges of the magnetic block.
            if (southEdgeDiff < 0 && northEdgeDiff > 0)
            {
                float positionx = currentFloor.floorPosition.x;
                float positiony = (currentLevel.index - numUndergroundLevels) * 5;
                float positionz = currentFloor.floorPosition.y;

                //if the WEST edge of the current floor block is within magneticTolerance units of the EAST edge of the magnetic block
                //the current floor block will stick to the magnetic floor block.
                if (westEdgeDiff < magneticTolerance && westEdgeDiff > 0)
                {
                    positionx = magneticFloor.truePosition.x + magneticFloor.floorScale.x / 2 + currentFloor.floorScale.x / 2;
                }
                //if the EAST edge of the current floor block is within magneticTolerance units of the WEST edge of the magnetic block
                //the current floor block will stick to the magnetic floor block.
                else if (eastEdgeDiff > -magneticTolerance && eastEdgeDiff < 0)
                {
                    positionx = magneticFloor.truePosition.x - magneticFloor.floorScale.x / 2 - currentFloor.floorScale.x / 2;
                }
                floorBlock.localPosition = new Vector3(currentLevel.levelPosition.x + positionx, positiony, currentLevel.levelPosition.y + positionz);
                currentFloor.truePosition = new Vector2(positionx, positionz);
            }
            //This checks that the east and west edges of the current block fall inside the east and west edges of the magnetic block
            else if (westEdgeDiff < 0 && eastEdgeDiff > 0)
            {
                float positionx = currentFloor.floorPosition.x;
                float positiony = (currentLevel.index - numUndergroundLevels) * 5;
                float positionz = currentFloor.floorPosition.y;

                //if the SOUTH edge of the current floor block is within magneticTolerance units of the NORTH edge of the magnetic block
                //the current floor block will stick to the magnetic floor block.
                if (southEdgeDiff < magneticTolerance && southEdgeDiff > 0)
                {
                    positionz = magneticFloor.truePosition.y + magneticFloor.floorScale.z / 2 + currentFloor.floorScale.z / 2;
                }
                //if the NORTH edge of the current floor block is within magneticTolerance units of the SOUTH edge of the magnetic block
                //the current floor block will stick to the magnetic floor block.
                else if (northEdgeDiff > -magneticTolerance && northEdgeDiff < 0)
                {
                    positionz = magneticFloor.truePosition.y - magneticFloor.floorScale.z / 2 - currentFloor.floorScale.z / 2;
                }
                floorBlock.localPosition = new Vector3(currentLevel.levelPosition.x + positionx, positiony, currentLevel.levelPosition.y + positionz);
                currentFloor.truePosition = new Vector2(positionx, positionz);
            }

            //If none of these conditions are met, the current block will be placed at the floor position.
            else
            {
                //same as below... (there's probably a way to combine them, but i never learned oops)
                float positionx = currentFloor.floorPosition.x;
                float positiony = (currentLevel.index - numUndergroundLevels) * 5;
                float positionz = currentFloor.floorPosition.y;
                floorBlock.localPosition = new Vector3(currentLevel.levelPosition.x + positionx, positiony, currentLevel.levelPosition.y + positionz);
                currentFloor.truePosition = new Vector2(positionx, positionz);
            }
        }
        else
        {
            float positionx = currentFloor.floorPosition.x;
            float positiony = (currentLevel.index - numUndergroundLevels) * 5;
            float positionz = currentFloor.floorPosition.y;
            floorBlock.localPosition = new Vector3(currentLevel.levelPosition.x + positionx, positiony, currentLevel.levelPosition.y + positionz);
            currentFloor.truePosition = new Vector2(positionx, positionz);
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