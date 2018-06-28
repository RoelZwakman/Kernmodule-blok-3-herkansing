using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DungeonRoom {

    public int xPos;
    public int yPos;
    public Tile[,] roomTiles;
    public RoomType roomType;


    public DungeonRoom(int w, int h)
    {
        roomTiles = new Tile[w, h];
    }

    

}
