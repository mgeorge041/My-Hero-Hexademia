using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(fileName = "New Hex Stats", menuName = "Hex Stats")]
public class HexStats : ScriptableObject
{
    public Sprite sprite;
    public int moveCost = 1;
    public Tile tile;
    public bool moveable;

    public static HexStats LoadHexStats(string tileName)
    {
        switch (tileName)
        {
            case "mud":
            case "Mud Tile":
                return Resources.Load<HexStats>("Map/Tiles/Mud Tile Stats");
            case "wall":
            case "Wall Tile":
                return Resources.Load<HexStats>("Map/Tiles/Wall Tile Stats");
            case "grass":
            case "Grass Tile":
                return Resources.Load<HexStats>("Map/Tiles/Grass Tile Stats");
            default:
                return Resources.Load<HexStats>("Map/Tiles/White Tile Stats");
        }
    }
}
