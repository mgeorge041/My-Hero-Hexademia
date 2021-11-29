using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterController
{
    private bool targeting = false;
    public List<Hex> targets;
    public HexMap hexMap;
    public Player player;
    private Character character;


    // Constructor
    public CharacterController(HexMap hexMap, Player player)
    {
        this.hexMap = hexMap;
        this.player = player;
    }


    // Clear
    public void Clear()
    {
        targeting = false;
        targets.Clear();
        character = null;
    }


    // Show ability state
    public void ShowAbilityState()
    {
        hexMap.ClearMovementTilemap();
        hexMap.PaintPath(targets);
    }


    // Hit A button
    public void HitAButton()
    {
        if (player.character == null)
            return;

        // Revert to action state
        if (player.playerState == PlayerState.Ability)
        {
            player.SetPlayerState(PlayerState.Action);
            targets.Clear();
            return;
        }

        character = player.character;
        targeting = true;
        targets = character.ability1.GetTargets(hexMap);
        foreach (Hex hex in targets)
            Debug.Log(hex.hexCoords);
        player.SetPlayerState(PlayerState.Ability);
    }


    // Left click
    public void LeftClick(Vector3 worldPosition)
    {

    }


    // Right click
    public void RightClick(Vector3 worldPosition)
    {
        Hex hex = hexMap.GetHexAtWorldPosition(worldPosition);

        // Return if null
        if (hex == null || !targets.Contains(hex))
            return;

        character.ability1.PerformAbility(hex);
        player.SetPlayerState(PlayerState.None);
    }


    // Use 1st ability
    public void UseAbility1()
    {

    }

    // Use 2nd ability
    public void UseAbility2()
    {

    }
}
