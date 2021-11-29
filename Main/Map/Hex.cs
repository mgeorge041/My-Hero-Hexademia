using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Tilemaps;

public class Hex : IHeapItem<Hex>
{
    // Hex info
    public Vector3Int hexCoords { get; set; }
    public Vector3Int tileCoords;
    public Vector3 worldPosition;
    public HexStats hexStats;
    public Tile defaultTile {
        get {
            return hexStats.tile; 
        }
    }
    public HexInfoPanel hexInfoPanel;
    public Character character;
    public bool hasCharacter
    {
        get
        {
            if (character != null)
                return true;
            return false;
        }
    }

    // Pathfinding variables
    public bool moveable
    {
        get
        {
            if (character != null)
                return false;
            return hexStats.moveable;
        }
        set 
        {
            hexStats.moveable = value;
        }
    }
    public Hex pathParent;
    public int moveCost
    {
        get
        {
            return hexStats.moveCost;
        }
    }
    public int gCost;
    public int hCost;
    public int fCost
    {
        get
        {
            return gCost + hCost;
        }
    }
    public bool visited = false;

    // Heap variables
    public int heapValue 
    { 
        get { 
            return fCost; 
        } 
    }
    public int heapIndex { get; set; }

    // Neighbor coordinates
    public static Vector3Int[] neighborCoords = new Vector3Int[]
    {
        new Vector3Int(0, -1, 1),
        new Vector3Int(1, -1, 0),
        new Vector3Int(1, 0, -1),
        new Vector3Int(0, 1, -1),
        new Vector3Int(-1, 1, 0),
        new Vector3Int(-1, 0, 1)
    };


    // Constructor
    public Hex(Vector3Int hexCoords)
    {
        this.hexCoords = hexCoords;
        tileCoords = HexToTileCoords(hexCoords);
    }


    // Reset
    public void Reset()
    {
        character = null;
    }

    
    // Get neighbor at index
    public Vector3Int GetNeighborCoords(int index)
    {
        return neighborCoords[index] + hexCoords;
    }


    // Get hex neighbors
    public Vector3Int[] GetAllNeighborCoords()
    {
        Vector3Int[] neighbors = new Vector3Int[]
        {
            hexCoords + neighborCoords[0],
            hexCoords + neighborCoords[1],
            hexCoords + neighborCoords[2],
            hexCoords + neighborCoords[3],
            hexCoords + neighborCoords[4],
            hexCoords + neighborCoords[5]
        };
        return neighbors;
    }


    // Get hex neighbors
    public static Vector3Int[] GetAllNeighborCoords(Vector3Int hexCoords)
    {
        Vector3Int[] neighbors = new Vector3Int[]
        {
            hexCoords + neighborCoords[0],
            hexCoords + neighborCoords[1],
            hexCoords + neighborCoords[2],
            hexCoords + neighborCoords[3],
            hexCoords + neighborCoords[4],
            hexCoords + neighborCoords[5]
        };
        return neighbors;
    }


    // Converts hex coordinates to tilemap coordinates
    public static Vector3Int HexToTileCoords(Vector3Int hexCoords)
    {
        int x = hexCoords.z + (hexCoords.x - (hexCoords.x & 1)) / 2;
        int y = hexCoords.x;

        return new Vector3Int(x, y, 0);
    }


    // Converts tilemap coordinates to hex coordinates
    public static Vector3Int TileToHexCoords(Vector3Int tileCoords)
    {
        int x = tileCoords.y;
        int y = -tileCoords.x - (tileCoords.y + (tileCoords.y & 1)) / 2;
        int z = -x - y;

        return new Vector3Int(x, y, z);
    }


    // Get distance between two hex coordinates
    public static int GetDistanceHexCoords(Vector3Int hexCoords1, Vector3Int hexCoords2)
    {
        int x = Math.Abs(hexCoords1.x - hexCoords2.x);
        int y = Math.Abs(hexCoords1.y - hexCoords2.y);
        int z = Math.Abs(hexCoords1.z - hexCoords2.z);

        int distance = (x + y + z) / 2;

        return distance;
    }


    // Get distance between two hexes
    public static int GetDistanceHexes(Hex hex1, Hex hex2)
    {
        return GetDistanceHexCoords(hex1.hexCoords, hex2.hexCoords);
    }


    // Get whether hexes in line
    public static bool HexesInLine(Hex hex1, Hex hex2)
    {
        if (hex1.hexCoords.x == hex2.hexCoords.x ||
            hex1.hexCoords.y == hex2.hexCoords.y ||
            hex1.hexCoords.z == hex2.hexCoords.z)
            return true;
        return false;
    }


    // Add character
    public void AddCharacter(Character character)
    {
        this.character = character;

        if (character == null)
            return;

        character.hex = this;
        character.transform.position = worldPosition;
    }
}
