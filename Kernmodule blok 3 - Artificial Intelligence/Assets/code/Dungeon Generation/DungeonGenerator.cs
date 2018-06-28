using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public enum Tile
{
    EMPTY = 0,
    FLOOR = 1,
    HALLFLOOR = 2,
    WALL = 3,
    DOOR = 4
}

[System.Serializable]
public enum RoomType
{
    Player = 0,
    EnemyOne = 1,
    EnemyTwo = 2,
    Key = 3,
    Exit = 4
}

public class DungeonGenerator : MonoBehaviour {

    [Header("Miscellaneous")]
    public GameObject roof;
    Tile[,] tiles;
    public List<DungeonRoom> rooms = new List<DungeonRoom>(2);
    public UnityAction dungeonGenerated;

    [Header("Dungeon settings")]
    public float dungeonScale;
    public int roomsToGenerate;
    public RoomType[] roomTypes;
    public int maxGenerationTries; ////the max amount of tries the while loop gets before it quits.
    int generationTries;
    

    [Header("Room settings")]
    public int gridSize;
    public int roomW;
    public int roomH;
    public GameObject geoDoorPrefab;


    [Header("Materials")]
    public Material dungeonWallMat;
    public Material dungeonFloorMat;
    

    ////gameobjects to add materials to
    List<GameObject> floors = new List<GameObject>(1);
    List<GameObject> walls = new List<GameObject>(1);

    [Header("Pathfinding")]
    public GameObject nodePrefab;
    public GameObject doorPrefab;

    

    private void Start()
    {
        GenerateDungeon();
    }

    private void GenerateDungeon()
    {
        tiles = new Tile[gridSize, gridSize];

        GenerateRooms();
        GenerateHallways();
        GenerateWalls();
        GenerateGeometry();
        GeneratePathfindingNodes();
        AddMaterials();
        RemakeRoof();
        dungeonGenerated();
        
    }

    private void RemakeRoof()
    {
        roof.transform.position = new Vector3(0, 1 * dungeonScale, 0);
        roof.transform.localScale = new Vector3(gridSize * dungeonScale, gridSize * dungeonScale, gridSize * dungeonScale);// * ;

        Material mat = roof.GetComponent<MeshRenderer>().material;
        mat.mainTextureScale = new Vector2(gridSize * dungeonScale, gridSize * dungeonScale);
    }

    private void GenerateRooms() /////Generates rooms in tiles[] at random locations
    {
        generationTries = 0;

        while (rooms.Count < roomsToGenerate && generationTries <= maxGenerationTries)
        {
            generationTries++;
            int rdmX = Random.Range(0, gridSize);
            int rdmY = Random.Range(0, gridSize);

            if (CanSpawnRoomHere(rdmX, rdmY))
            {
                GenerateRoom(rdmX, rdmY, roomW, roomH);
            }
        }
    }

    private bool CanSpawnRoomHere(int x, int y) /////checks if rooms can be spawned in position xy
    {
        bool eval = true;

        for (int i = x; i < x + roomW; i++)
        {
            for (int j = y; j < y + roomH; j++)
            {
                
                if (i < gridSize && j < gridSize && i > 0 && j > 0)
                {
                    if (tiles[i, j] == Tile.FLOOR)
                    {
                        eval = false;
                    }
                }
                else
                {
                    eval = false;
                }
            }
        }

        return eval;
    }

    private void GenerateRoom(int roomX, int roomY, int roomWidth, int roomHeight) /////Generates rooms in tiles[]
    {
        DungeonRoom newRoom = new DungeonRoom(roomWidth, roomHeight);
        newRoom.xPos = roomX;
        newRoom.yPos = roomY;
        newRoom.roomType = roomTypes[rooms.Count];

        for (int x = 0; x < roomWidth; x++)
        {
            for (int y = 0; y < roomHeight; y++)
            {
                if (x + roomX < gridSize && y + roomY < gridSize)
                {
                    tiles[x + roomX, y + roomY] = Tile.FLOOR;
                    newRoom.roomTiles[x, y] = Tile.FLOOR;
                }
            }
        }

        rooms.Add(newRoom);

    }


    private void GenerateHallways() /////Connects all rooms together with hallways
    {
        for(int i = 0; i < rooms.Count - 1; i++)
        {
            
            int smallestX = GetSmallestInt(rooms[i].xPos, rooms[i + 1].xPos);
            int biggestX = GetBiggestInt(rooms[i].xPos, rooms[i + 1].xPos);

            int smallestY = GetSmallestInt(rooms[i].yPos, rooms[i + 1].yPos);
            int biggestY = GetBiggestInt(rooms[i].yPos, rooms[i + 1].yPos);

            for (int x = smallestX; x < biggestX; x++)
            {
               
                for(int y = smallestY; y < biggestY; y++)
                {
                    if(rooms[i].xPos == smallestX) ////if rooms[i] is west of rooms[i + 1]
                    {
                        if(rooms[i].yPos == smallestY) /////if rooms[i] is south of rooms[i + 1]
                        {
                            /////SOUTHWEST
                            CreateHallwayUpward(rooms[i].xPos, rooms[i].yPos, biggestY - smallestY);
                            CreateHallwayRightward(rooms[i].xPos, biggestY, biggestX - smallestX);
                        }
                        else if(rooms[i].yPos == biggestY) /////if rooms[i] is north of rooms[i + 1]
                        {
                            /////NORTHWEST
                            CreateHallwayDownward(rooms[i].xPos, rooms[i].yPos, biggestY - smallestY);
                            CreateHallwayRightward(rooms[i].xPos, smallestY, biggestX - smallestX);
                        }
                    }
                    else if(rooms[i].xPos == biggestX) ////if rooms[i] is east of rooms[i + 1]
                    {
                        if (rooms[i].yPos == smallestY) /////if rooms[i] is south of rooms[i + 1]
                        {
                            /////SOUTHEAST
                            CreateHallwayUpward(rooms[i].xPos, rooms[i].yPos, biggestY - smallestY);
                            CreateHallwayLeftward(rooms[i].xPos, biggestY, biggestX - smallestX);
                        }
                        else if (rooms[i].yPos == biggestY) /////if rooms[i] is north of rooms[i + 1]
                        {
                            /////NORTHEAST
                            CreateHallwayDownward(rooms[i].xPos, rooms[i].yPos, biggestY - smallestY);
                            CreateHallwayLeftward(rooms[i].xPos, smallestY, biggestX - smallestX);
                        }
                    }          
                }
            }
        }
    }

    private void CreateHallwayUpward(int x, int y, int length)
    {
        for(int i = y; i <= y + length; i++)
        {
            if(tiles[x, i] != Tile.FLOOR)
            {
                tiles[x, i] = Tile.HALLFLOOR;
            }
            
        }
    }

    private void CreateHallwayDownward(int x, int y, int length)
    {
        for(int i = y; i >= y - length; i--)
        {
            if(tiles[x, i] != Tile.FLOOR)
            {
                tiles[x, i] = Tile.HALLFLOOR;
            }
            
        }
    }

    private void CreateHallwayRightward(int x, int y, int length)
    {
        for(int i = x; i <= x + length; i++)
        {
            if(tiles[i, y] != Tile.FLOOR)
            {
                tiles[i, y] = Tile.HALLFLOOR;
            }
            
        }
    }

    private void CreateHallwayLeftward(int x, int y, int length)
    {
        for(int i = x; i >= x - length; i--)
        {
            if (tiles[i, y] != Tile.FLOOR)
            {
                tiles[i, y] = Tile.HALLFLOOR;
            }
        }
    }


    private int GetSmallestInt(int x1, int x2)
    {
        int smallestInt = 0;

        if(x1 > x2)
        {
            smallestInt = x2;
            return smallestInt;
        }
        else if(x1 < x2)
        {
            smallestInt = x1;
            return smallestInt;
        }

        return smallestInt;
    }

    private int GetBiggestInt(int x1, int x2)
    {
        int biggestInt = 0;

        if (x1 > x2)
        {
            biggestInt = x1;
            return biggestInt;
        }
        else if (x1 < x2)
        {
            biggestInt = x2;
            return biggestInt;
        }

        return biggestInt;
    }

    private int GetXDistance(int biggest, int smallest)
    {
        int xDist = biggest - smallest;

        return xDist;
    }


    private void GenerateWalls() /////Identifies walls and corners in tiles[]
    {
        for(int i = 0; i < 3; i++)
        {
            
            for(int x = 1; x < gridSize - 1; x++)
            {
                for(int y = 1; y < gridSize - 1; y++)
                {
                    if(tiles[x,y] == Tile.FLOOR || tiles[x, y] == Tile.HALLFLOOR)
                    {
                        
                        if (tiles[x - 1, y] == Tile.EMPTY) ////if tile to the left is empty
                        {
                            tiles[x - 1, y] = Tile.WALL;
                        }
                        else if (tiles[x, y - 1] == Tile.EMPTY) ////if tile above is empty
                        {
                            tiles[x, y - 1] = Tile.WALL;
                        }
                        else if (tiles[x + 1, y] == Tile.EMPTY) ////if tile to the right is empty
                        {
                            tiles[x + 1, y] = Tile.WALL;
                        }
                        else if (tiles[x, y + 1] == Tile.EMPTY) ////if tile below is empty
                        {
                            tiles[x, y + 1] = Tile.WALL;
                        }
                    }
                    


                }
            }
        }
    }
    
    private void GenerateGeometry() ////spawns, scales & rotates primitives based on tiles[]
    {
        for(int x = 0; x < gridSize; x++)
        {
            for(int y = 0; y < gridSize; y++)
            {
                switch (tiles[x, y])
                {
                    case Tile.FLOOR:

                        GameObject goFloor = GameObject.CreatePrimitive(PrimitiveType.Quad);

                        Vector3 newPosFloor = new Vector3(0, 0, 0);
                        newPosFloor.x = x * dungeonScale;
                        newPosFloor.z = y * dungeonScale;

                        goFloor.transform.localScale *= dungeonScale;

                        goFloor.transform.position = newPosFloor;
                        goFloor.transform.Rotate(transform.right * 90);

                        goFloor.name = "floor";

                        floors.Add(goFloor);
                        

                    break;

                    case Tile.HALLFLOOR:
                        GameObject goHallFloor = GameObject.CreatePrimitive(PrimitiveType.Quad);

                        Vector3 newPosHallFloor = new Vector3(0, 0, 0);
                        newPosHallFloor.x = x * dungeonScale;
                        newPosHallFloor.z = y * dungeonScale;

                        goHallFloor.transform.localScale *= dungeonScale;

                        goHallFloor.transform.position = newPosHallFloor;
                        goHallFloor.transform.Rotate(transform.right * 90);

                        goHallFloor.name = "hallfloor";

                        floors.Add(goHallFloor);
                        break;

                    case Tile.WALL:
                        GameObject goWall = GameObject.CreatePrimitive(PrimitiveType.Cube);
                        Vector3 newPosWall = new Vector3(0, 0, 0);
                        newPosWall.x = x * dungeonScale;
                        newPosWall.z = y * dungeonScale;

                        newPosWall.y += 0.5f * dungeonScale;

                        goWall.transform.localScale *= dungeonScale;

                        goWall.transform.position = newPosWall;

                        walls.Add(goWall);
                        break;
                        
                }
                
            }
        }
        GenerateGeoDoor();
    }


    private void AddMaterials() ////Applies the correct materials to the geometry
    {
        for(int i = 0; i < floors.Count; i++)
        {
            floors[i].GetComponent<Renderer>().material = dungeonFloorMat;
        }

        for(int j = 0; j < walls.Count; j++)
        {
            walls[j].GetComponent<Renderer>().material = dungeonWallMat;
        }

    }

    private void GenerateGeoDoor() ///////DIT MOET VERBETERD WORDEN!! MISSCHIEN IN ELKE HOEK KIJKEN OF ER EEN HALLWAY ZIT, EN ZO JA DAN SPAWNEN. DAARNAAST MOET JE DAN ENEMIES, KEYS EN EXITS IN MIDDEN VAN ROOM SPAWNEN
    {
        //Vector3 pos = new Vector3(room.xPos * dungeonScale, -1 * dungeonScale, room.yPos * dungeonScale);


        

        for(int i = 0; i < floors.Count; i++)
        {
            Collider[] hitColliders = Physics.OverlapBox(floors[i].transform.position, (floors[i].transform.localScale) * 0.6f, Quaternion.identity);
            bool hasFloor = false;
            bool hasHall = false;
            for(int j = 0; j < hitColliders.Length; j++)
            {
                if(hitColliders[j].gameObject.name == "floor")
                {
                    hasFloor = true;
                }
                else if(hitColliders[j].gameObject.name == "hallfloor")
                {
                    hasHall = true;
                }
            }

            if (hasHall && hasFloor && floors[i].gameObject.name == "hallfloor")
            {
                Vector3 pos = new Vector3(floors[i].transform.position.x, -1 * dungeonScale, floors[i].transform.position.z);
                GameObject door = Instantiate(geoDoorPrefab, pos, Quaternion.identity);
                door.transform.localScale *= dungeonScale;
                AnimatedDoor animdoor = door.GetComponent<AnimatedDoor>();
                animdoor.scaleMultiplier = dungeonScale;
            }
        }

        
        
    }

    private void GeneratePathfindingNodes() ////Spawns the walkable spaces for AI
    {
        for(int x = 0; x < gridSize; x++)
        {
            for(int y = 0; y < gridSize; y++)
            {
                if(tiles[x, y] == Tile.FLOOR)
                {
                    GameObject node = Instantiate(nodePrefab, new Vector3(x * dungeonScale, 0.025f * dungeonScale, y * dungeonScale), Quaternion.identity);
                    node.transform.localScale *= dungeonScale;
                    node.name = "room";

                    GeneratePathfindingDoor(x, y);
                }
            }
        }
    }

    private void GeneratePathfindingDoor(int x, int y) ////Spawns the connections between walkable spaces for AI
    {
        if(x + 1 < gridSize && y + 1 < gridSize)
        {        
            if (tiles[x + 1, y] == Tile.FLOOR) ////Spawn door to the right of the current node if there's another node there.
            {
                GameObject door = Instantiate(doorPrefab, new Vector3((x + 0.5f) * dungeonScale, 0.025f * dungeonScale, y * dungeonScale), Quaternion.identity);
                door.transform.localScale *= dungeonScale;
                door.name = "door";

            }

            if(tiles[x, y + 1] == Tile.FLOOR) ////Spawn door below the current node if there's another node there.
            {
                GameObject door = Instantiate(doorPrefab, new Vector3(x * dungeonScale, 0.05f * dungeonScale, (y + 0.5f) * dungeonScale), Quaternion.identity);
                door.transform.localScale *= dungeonScale;
                door.name = "door";
            }
        }
    }


    

}
