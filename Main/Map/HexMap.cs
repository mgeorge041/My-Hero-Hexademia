using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using System;

public class HexMap : MonoBehaviour
{
    public Grid grid;
    public Tilemap terrainMap;
    public Tilemap actionMap;
    public List<Vector3Int> actionTileCoords = new List<Vector3Int>();
    public HexStats defaultHex;
    public Tile moveTile;
    public HexStats wallHex;
    public Tile startTile;
    public Tile attackTile;
    public HexStats mudHex;
    public Dictionary<Vector3Int, Hex> hexCoordsDict = new Dictionary<Vector3Int, Hex>();
    public Dictionary<Vector3Int, Vector3Int> tileHexCoordsDict = new Dictionary<Vector3Int, Vector3Int>();
    public int mapRadius = 5;
    public int mapSize;
    public Camera mapCamera;
    public HexMapState hexMapState = HexMapState.None;
    public GameObject characterPrefab;
    private List<Character> characters = new List<Character>();

    /*
     * X runs horizontal 
     * Y runs down left
     * Z runs up left
     */

    // Initialize
    public void Initialize()
    {
        CreateMap();
    }


    // Initialize
    public void Initialize(int mapRadius)
    {
        this.mapRadius = mapRadius;
        CreateMap();
    }


    // Get hex at mouse position
    public Hex GetHexAtMousePosition(Camera camera)
    {
        return GetHexAtWorldPosition(camera.ScreenToWorldPoint(Input.mousePosition));
    }


    // Get hex at world position
    public Hex GetHexAtWorldPosition(Vector3 worldPosition)
    {
        Vector3Int tileCoords = grid.WorldToCell(worldPosition);
        tileCoords.z = 0;
        return GetHexAtTileCoords(tileCoords);
    }


    // Get hex from tile coords
    public Hex GetHexAtTileCoords(Vector3Int tileCoords)
    {
        Vector3Int hexCoords;
        if (tileHexCoordsDict.TryGetValue(tileCoords, out hexCoords))
            return GetHexAtHexCoords(hexCoords);
        return null;
    }


    // Get hex from hex coords
    public Hex GetHexAtHexCoords(Vector3Int hexCoords)
    {
        Hex hex;
        hexCoordsDict.TryGetValue(hexCoords, out hex);
        return hex;
    }


    // Get hex coords from world coords
    public Vector3Int WorldToHexCoords(Vector3 worldPosition)
    {
        return Hex.TileToHexCoords(grid.WorldToCell(worldPosition));
    }


    // Get tile coords from world coords
    public Vector3Int WorldToTileCoords(Vector3 worldPosition)
    {
        return grid.WorldToCell(worldPosition);
    }


    // Get world coords from hex coords
    public Vector3 HexToWorldCoords(Vector3Int hexCoords)
    {
        return grid.CellToWorld(Hex.HexToTileCoords(hexCoords));
    }


    // Get world coords from tile coords
    public Vector3 TileToWorldCoords(Vector3Int tileCoords)
    {
        return grid.CellToWorld(tileCoords);
    }


    // Get hex neighbors
    public List<Hex> GetHexNeighbors(Vector3Int[] neighborCoords)
    {
        Hex hex;
        List<Hex> neighborHexes = new List<Hex>();
        foreach (Vector3Int neighborCoord in neighborCoords)
        {
            hexCoordsDict.TryGetValue(neighborCoord, out hex);
            if (hex != null)
                neighborHexes.Add(hex);
        }
        return neighborHexes;
    }


    // Get whether same player
    public bool NotSamePlayer(Hex hex1, Hex hex2)
    {
        if (hex1.hasCharacter && hex2.hasCharacter)
            return hex1.character.playerId != hex2.character.playerId;
        return true;
    }


    // Get path back
    public List<Hex> GetPathBack(Hex startHex, Hex targetHex)
    {
        List<Hex> hexes = new List<Hex>();
        while (targetHex != startHex)
        {
            hexes.Add(targetHex);
            targetHex = targetHex.pathParent;
        }
        hexes.Add(startHex);
        hexes.Reverse();
        return hexes;
    }


    // Paint start tile
    public void PaintStartTile(Vector3Int tileCoords)
    {
        actionMap.SetTile(tileCoords, startTile);
        actionMap.RefreshTile(tileCoords);
    }


    // Paint tile
    public void PaintMovementTile(Vector3Int tileCoords, Tile tile)
    {
        actionMap.SetTile(tileCoords, tile);
        actionMap.RefreshTile(tileCoords);
    }


    // Clear tilemap
    public void ClearMovementTilemap()
    {
        foreach (Vector3Int tileCoords in actionTileCoords)
        {
            actionMap.SetTile(tileCoords, null);
        }
        actionMap.RefreshAllTiles();
        actionTileCoords.Clear();
    }


    // Paint path
    public void PaintPath(List<Hex> hexPath)
    {
        if (hexPath == null)
            return;

        foreach (Hex hex in hexPath)
        {
            if (hex.hasCharacter)
                actionMap.SetTile(hex.tileCoords, attackTile);
            else
                actionMap.SetTile(hex.tileCoords, moveTile);
            actionTileCoords.Add(hex.tileCoords);
        }
        actionMap.RefreshAllTiles();
    }


    // Pathfinding
    public List<Hex> GetPath(Hex startHex, Hex targetHex)
    {
        Debug.Log("Getting path");
        // Exit if not possible to pathfind
        if (targetHex == null || startHex == null || !targetHex.moveable)
        {
            return null;
        }
        Debug.Log("Start hex: " + startHex.hexCoords);
        Debug.Log("Target hex: " + targetHex.hexCoords);

        // Create lists for tracking hexes
        ResetHexPathfinding();
        MinHeap<Hex> openHexes = new MinHeap<Hex>(mapSize);
        List<Hex> closedHexes = new List<Hex>();
        openHexes.Add(startHex);

        // Loop through hexes
        while (openHexes.Count > 0)
        {
            // Get shortest path hex
            Hex currentHex = openHexes.Pop();
            closedHexes.Add(currentHex);

            // Return when found target
            if (currentHex == targetHex)
            {
                return GetPathBack(startHex, targetHex);
            }

            // Process neighbor move costs
            List<Hex> neighbors = GetHexNeighbors(currentHex.GetAllNeighborCoords());
            foreach (Hex neighbor in neighbors)
            {
                // Do nothing for unmoveable or already processed hexes
                if (!neighbor.moveable || closedHexes.Contains(neighbor))
                {
                    continue;
                }

                // Update neighbor costs
                int newDist = currentHex.gCost + (currentHex.moveCost - 1) + Hex.GetDistanceHexes(currentHex, neighbor);
                if (newDist < neighbor.gCost || !openHexes.Contains(neighbor))
                {
                    neighbor.gCost = newDist;
                    neighbor.hCost = Hex.GetDistanceHexes(neighbor, targetHex);
                    neighbor.pathParent = currentHex;

                    // Add new neighbor to process
                    if (!openHexes.Contains(neighbor))
                    {
                        openHexes.Add(neighbor);
                    }
                }
            }
        }

        // Clear if no path found
        return null;
    }


    // Get max path
    public List<Hex> GetMaxPath(Hex startHex)
    {
        Debug.Log("Getting max path");
        // Exit if not possible to pathfind
        if (startHex == null)
        {
            return null;
        }
        Debug.Log("Start hex: " + startHex.hexCoords);

        // Create lists for tracking hexes
        ResetHexPathfinding();
        MinHeap<Hex> openHexes = new MinHeap<Hex>(mapSize);
        List<Hex> closedHexes = new List<Hex>();
        List<Hex> possibleHexes = new List<Hex>();
        int maxDistance = startHex.character.remainingSpeed;
        openHexes.Add(startHex);

        // Loop through hexes
        while (openHexes.Count > 0)
        {
            // Get shortest path hex
            Hex currentHex = openHexes.Pop();
            currentHex.visited = true;
            //closedHexes.Add(currentHex);

            // Skip if outside max distance
            if (Hex.GetDistanceHexes(currentHex, startHex) > maxDistance) 
            {
                continue;
            }

            // Process neighbors
            List<Hex> neighbors = GetHexNeighbors(currentHex.GetAllNeighborCoords());
            foreach (Hex neighbor in neighbors)
            {
                // Add if neighbor has character
                if (neighbor.hasCharacter && neighbor != startHex && NotSamePlayer(neighbor, startHex) && !possibleHexes.Contains(neighbor))
                {
                    possibleHexes.Add(neighbor);
                }

                // Do nothing for unmoveable or already processed hexes
                //if (!neighbor.moveable || closedHexes.Contains(neighbor) || neighbor.gCost > maxDistance || !NotSamePlayer(neighbor, startHex))
                if (!neighbor.moveable || neighbor.visited || neighbor.gCost > maxDistance || !NotSamePlayer(neighbor, startHex))
                {
                    continue;
                }

                // Update neighbor costs
                int newDist = currentHex.gCost + (neighbor.moveCost - 1) + Hex.GetDistanceHexes(currentHex, neighbor);
                if (newDist < neighbor.gCost || !openHexes.Contains(neighbor))
                {
                    neighbor.gCost = newDist;
                    neighbor.pathParent = currentHex;

                    // Add new neighbor to process
                    if (!openHexes.Contains(neighbor))
                    {
                        openHexes.Add(neighbor);
                    }

                    // Add potential hex
                    if (!possibleHexes.Contains(neighbor) && neighbor.gCost <= maxDistance && NotSamePlayer(neighbor, startHex))
                    {
                        possibleHexes.Add(neighbor);
                    }

                    // Get potential targets
                    if (neighbor.gCost == maxDistance)
                    {
                        List<Hex> potentialTargets = GetHexTargetsWithinRange(neighbor, startHex.character);
                        foreach (Hex hex in potentialTargets)
                        {
                            if (!possibleHexes.Contains(hex) && NotSamePlayer(hex, startHex))
                            {
                                possibleHexes.Add(hex);
                            }
                        }
                    }
                }
            }
        }

        // Return possible hexes
        return possibleHexes;
    }


    // Get hexes within range
    public List<Hex> GetHexesWithinRange(Hex startHex, int range)
    {
        Debug.Log("Getting hexes in range " + range + " of: " + startHex.hexCoords);
        List<Hex> hexes = new List<Hex>();

        for (int i = -range; i <= range; i++)
        {

            // Get upper and lower bounds for map columns
            int lowerBound = Math.Max(-i - range, -range);
            int upperBound = Math.Min(range, -i + range);

            for (int j = lowerBound; j <= upperBound; j++)
            {
                int z = -i - j;
                Vector3Int hexCoords = new Vector3Int(i, j, z) + startHex.hexCoords;
                Hex hex;
                if (hexCoordsDict.TryGetValue(hexCoords, out hex))
                {
                    hexes.Add(hexCoordsDict[hexCoords]);
                }
            }
        }
        return hexes;
    }


    // Get hexes within range
    public List<Hex> GetHexTargetsWithinRange(Hex startHex, Character targetingCharacter)
    {
        int range = targetingCharacter.range;
        Debug.Log("Getting target hexes in range " + range + " of: " + startHex.hexCoords);
        List<Hex> hexes = new List<Hex>();

        for (int i = -range; i <= range; i++)
        {

            // Get upper and lower bounds for map columns
            int lowerBound = Math.Max(-i - range, -range);
            int upperBound = Math.Min(range, -i + range);

            for (int j = lowerBound; j <= upperBound; j++)
            {
                int z = -i - j;
                Vector3Int hexCoords = new Vector3Int(i, j, z) + startHex.hexCoords;
                Hex hex;
                if (hexCoordsDict.TryGetValue(hexCoords, out hex) && hexCoords != startHex.hexCoords 
                    && hex.hasCharacter && hex.character != targetingCharacter && NotSamePlayer(hex, startHex))
                {
                    hexes.Add(hexCoordsDict[hexCoords]);
                }
            }
        }
        return hexes;
    }


    // Get hexes within range
    public List<Hex> GetHexTargetsWithinRange(Hex startHex, int range)
    {
        Debug.Log("Getting target hexes in range " + range + " of: " + startHex.hexCoords);
        List<Hex> hexes = new List<Hex>();

        for (int i = -range; i <= range; i++)
        {

            // Get upper and lower bounds for map columns
            int lowerBound = Math.Max(-i - range, -range);
            int upperBound = Math.Min(range, -i + range);

            for (int j = lowerBound; j <= upperBound; j++)
            {
                int z = -i - j;
                Vector3Int hexCoords = new Vector3Int(i, j, z) + startHex.hexCoords;
                Hex hex;
                if (hexCoordsDict.TryGetValue(hexCoords, out hex) && hexCoords != startHex.hexCoords
                    && hex.hasCharacter && NotSamePlayer(hex, startHex))
                {
                    hexes.Add(hexCoordsDict[hexCoords]);
                }
            }
        }
        return hexes;
    }


    // Get hexes in line
    public List<Hex> GetHexesInLine(Hex startHex, Hex targetHex, bool includeCenter = true)
    {
        if (!Hex.HexesInLine(startHex, targetHex) || startHex == targetHex)
            return null;

        List<Hex> hexes = new List<Hex>();
        Vector3Int currentCoords = startHex.hexCoords;

        // In line along X axis
        if (startHex.hexCoords.x == targetHex.hexCoords.x)
        {
            // Move up
            while (currentCoords.y >= targetHex.hexCoords.y)
            {
                if (currentCoords != startHex.hexCoords || includeCenter)
                    hexes.Add(GetHexAtHexCoords(currentCoords));
                currentCoords += Hex.neighborCoords[0];
            }
            currentCoords = startHex.hexCoords;

            // Move down
            while (currentCoords.y <= targetHex.hexCoords.y)
            {
                if (currentCoords != startHex.hexCoords || includeCenter)
                    hexes.Add(GetHexAtHexCoords(currentCoords));
                currentCoords += Hex.neighborCoords[3];
            }
        }

        // In line along Y axis
        else if (startHex.hexCoords.y == targetHex.hexCoords.y)
        {
            // Move UL
            while (currentCoords.x >= targetHex.hexCoords.x)
            {
                if (currentCoords != startHex.hexCoords || includeCenter)
                    hexes.Add(GetHexAtHexCoords(currentCoords));
                currentCoords += Hex.neighborCoords[5];
            }
            currentCoords = startHex.hexCoords;

            // Move DR
            while (currentCoords.x <= targetHex.hexCoords.x)
            {
                if (currentCoords != startHex.hexCoords || includeCenter)
                    hexes.Add(GetHexAtHexCoords(currentCoords));
                currentCoords += Hex.neighborCoords[2];
            }
        }

        // In line along Z axis
        else 
        {
            // Move DL
            while (currentCoords.x >= targetHex.hexCoords.x)
            {
                if (currentCoords != startHex.hexCoords || includeCenter)
                    hexes.Add(GetHexAtHexCoords(currentCoords));
                currentCoords += Hex.neighborCoords[4];
            }
            currentCoords = startHex.hexCoords;

            // Move UR
            while (currentCoords.x <= targetHex.hexCoords.x)
            {
                if (currentCoords != startHex.hexCoords || includeCenter)
                    hexes.Add(GetHexAtHexCoords(currentCoords));
                currentCoords += Hex.neighborCoords[1];
            }
        }
        return hexes;
    }


    // Get next hex in line
    public Hex GetNextHexInLine(Hex startHex, Hex endHex)
    {
        Vector3Int diff = endHex.hexCoords - startHex.hexCoords;
        return GetHexAtHexCoords(diff + endHex.hexCoords);
    }


    // Reset hex pathfinding
    public void ResetHexPathfinding()
    {
        foreach (Hex hex in hexCoordsDict.Values)
        {
            hex.gCost = 0;
            hex.hCost = 0;
            hex.visited = false;
        }
    }


    // Get placement hexes for a player
    public List<Hex> GetPlacementHexes(Player player)
    {
        if (player.id == 1)
        {
            List<Hex> placementHexes = GetHexesWithinRange(GetHexAtHexCoords(new Vector3Int(0, mapRadius, -mapRadius)), 2);
            return placementHexes;
        }
        return null;
    }


    // Create placement hexes
    public void PaintPlacementHexes(List<Hex> placementHexes)
    {
        PaintPath(placementHexes);
    }


    // Create map
    public void CreateMap()
    {
        // Draw the tilemap
        for (int i = -mapRadius; i <= mapRadius; i++)
        {
            // Get upper and lower bounds for map columns
            int lowerBound = Math.Max(-i - mapRadius, -mapRadius);
            int upperBound = Math.Min(mapRadius, -i + mapRadius);

            for (int j = lowerBound; j <= upperBound; j++)
            {
                // Get z coordinate
                int z = -i - j;
                Vector3Int newHexCoords = new Vector3Int(i, j, z);
                Vector3Int newTileCoords = Hex.HexToTileCoords(newHexCoords);
                int distance = Hex.GetDistanceHexCoords(newHexCoords, Vector3Int.zero);

                // Only add tiles in size range
                if (distance <= mapRadius && !hexCoordsDict.ContainsKey(newHexCoords))
                {
                    // Add coords to dicts and create hex
                    Hex newHex = new Hex(newHexCoords);
                    hexCoordsDict.Add(newHexCoords, newHex);
                    tileHexCoordsDict.Add(newTileCoords, newHexCoords);
                    newHex.worldPosition = grid.CellToWorld(newTileCoords);
                    newHex.worldPosition.z = 0;
                    newHex.hexStats = defaultHex;
                    terrainMap.SetTile(newTileCoords, newHex.defaultTile);
                }
            }
        }
        terrainMap.RefreshAllTiles();
        mapSize = hexCoordsDict.Count;
    }


    // Reset map
    public void Reset()
    {
        foreach (Hex hex in hexCoordsDict.Values)
        {
            hex.Reset();
            terrainMap.SetTile(hex.tileCoords, hex.defaultTile);
        }
        ClearMovementTilemap();
        terrainMap.RefreshAllTiles();

        foreach (Character character in characters)
        {
            C.Destroy(character);
        }
        characters.Clear();
    }


    // Add character from character card
    public void AddCharacterFromCard(Player player, Hex hex)
    {
        // Return if can't play character to hex
        if (hex.hasCharacter)
            return;

        // Create new character
        Character newCharacter = Character.CreateCharacter(player.characterCard.stats);
        player.AddCharacter(newCharacter);

        // Add character to map
        AddCharacterToHex(newCharacter, hex);
    }


    // Add character
    public void AddCharacterToHex(Character character, Hex hex)
    {
        // Return if can't play character to hex
        if (hex == null || hex.hasCharacter || character == null)
            return;

        // Add character
        characters.Add(character);
        hex.AddCharacter(character);
    }


    // Start is called before the first frame update
    void Start()
    {
        
    }


    // Update is called once per frame
    void Update()
    {
        
    }
}
