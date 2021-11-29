using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class HexMapController
{
    // Hex map
    public HexMap hexMap;

    // Character pathfinding
    private Hex startHex;
    public Character character;
    private List<Hex> possibleHexes;
    private List<Hex> placementHexes;

    // Player
    public Player player;


    // Constructor
    public HexMapController(HexMap hexMap, Player player)
    {
        this.hexMap = hexMap;
        this.player = player;
    }


    // Clear placement tiles
    public void ClearPlacementTiles()
    {
        hexMap.ClearMovementTilemap();
        player.SetPlayerState(PlayerState.None);
    }


    // Paint placement tiles
    public void SetPlacementHexes()
    {
        placementHexes = hexMap.GetPlacementHexes(player);
        player.SetPlayerState(PlayerState.Placement);
    }


    // Show placement state
    public void ShowPlacementState()
    {
        hexMap.ClearMovementTilemap();
        hexMap.PaintPlacementHexes(placementHexes);
    }


    // Clear selected hex
    public void Clear()
    {
        startHex = null;
        character = null;
        possibleHexes.Clear();
        player.ClearSelectedCharacter();
        hexMap.ClearMovementTilemap();
    }


    // Set selected hex
    public void SetSelectedHex(Hex hex)
    {
        // Clear if no character or character can't act
        if (!hex.hasCharacter || !hex.character.hasActions)
        {
            Clear();
            Debug.Log("in clear");
            return;
        }

        startHex = hex;
        character = hex.character;

        // Get possible targets
        possibleHexes = hexMap.GetMaxPath(startHex);
        player.SetSelectedCharacter(character);
        player.SetPlayerState(PlayerState.Action);
    }


    // Show action state
    public void ShowActionState()
    {
        hexMap.ClearMovementTilemap();
        hexMap.PaintPath(possibleHexes);
    }


    // Attack character
    private void AttackHex(Hex targetHex)
    {
        character.DealDamage(targetHex.character);
        hexMap.ClearMovementTilemap();
        player.playerState = PlayerState.None;
    }


    // Move character
    private void MoveCharacter(List<Hex> path)
    {
        character.MoveCharacter(path);
        player.playerUi.UpdateCharacter(character);
        hexMap.ClearMovementTilemap();
    }


    // Add character to hex map
    private void AddCharacterToHexMap(Hex hex)
    {
        // Create new character
        Character newCharacter = Character.CreateCharacter(player.characterCard);
        player.PlayCharacterCard();
        player.AddCharacter(newCharacter);

        // Add character to map
        hexMap.AddCharacterToHex(newCharacter, hex);
        hexMap.ClearMovementTilemap();
        player.playerState = PlayerState.None;
    }


    // Left click
    public void LeftClick(Vector3 worldPosition)
    {
        Hex hex = hexMap.GetHexAtWorldPosition(worldPosition);

        // Return if null
        if (hex == null || hex.character.playerId != player.id)
            return;

        // Reset if click on start twice
        Debug.Log("Left click at hex: " + hex.hexCoords);
        if (hex == startHex)
        {
            Clear();
        }

        // Set start hex
        else 
        {
            SetSelectedHex(hex);
        }
    }


    // Right click
    public void RightClick(Vector3 worldPosition)
    {
        Hex hex = hexMap.GetHexAtWorldPosition(worldPosition);

        if (hex == null)
            return;

        if (player.playerState == PlayerState.Action)
        {
            // Return if null or not possible to move to or attack
            if (startHex == null || !possibleHexes.Contains(hex))
                return;

            // Attack character
            Debug.Log("Right click at hex: " + hex.hexCoords);
            if (hex.hasCharacter && Hex.GetDistanceHexes(startHex, hex) <= character.range)
            {
                AttackHex(hex);
                // INDICATE TARGET IS NOT IN RANGE
                // HEX MAP SHOW TILES IN RANGE
                return;
            }

            // Move character
            List<Hex> path = hexMap.GetPath(startHex, hex);
            if (path != null)
            {
                MoveCharacter(path);
                return;
            }
        }
        else if (player.playerState == PlayerState.Placement)
        { 
            // Return if null or can't player character
            Debug.Log("can play: " + !player.CanPlayCharacterCard(player.characterCard));
            if (hex.hasCharacter || !player.CanPlayCharacterCard(player.characterCard) || !placementHexes.Contains(hex))
                return;

            // Add character to hex map
            AddCharacterToHexMap(hex);
        }
    }
}
