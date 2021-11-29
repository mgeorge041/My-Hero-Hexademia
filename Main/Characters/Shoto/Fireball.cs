using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fireball : Ability
{
    /*
     * Shoto shoots a fireball that hits 2 hexes in a line
     */

    private int range = 2;
    private int damage = 5;
    private Hex startHex;
    private HexMap hexMap;

    // Constructor
    public Fireball()
    {
        abilityName = "Fireball";
    }

    // Get targets
    public override List<Hex> GetTargets(HexMap hexMap)
    {
        // Get hexes
        startHex = character.hex;
        this.hexMap = hexMap;
        List<Hex> filteredTargets = new List<Hex>();
        List<Hex> possibleTargets = hexMap.GetHexTargetsWithinRange(startHex, range);
        Debug.Log("possible fireball targets: " + possibleTargets.Count);
        foreach (Hex hex in possibleTargets)
        {
            if (Hex.HexesInLine(startHex, hex))
            {
                filteredTargets.Add(hex);
            }
        }
        return filteredTargets;
    }


    // Animate
    public override IEnumerator Animate()
    {
        float t = 0;
        while (t < 1)
        {
            t += Time.deltaTime;
            yield return null;
        }
        animating = false;
    }


    // Perform ability
    public override void PerformAbility(Hex targetHex)
    {
        List<Hex> hitHexes = hexMap.GetHexesInLine(startHex, targetHex, false);

        // Get hex behind target
        if (hitHexes.Count == 1)
        {
            hitHexes.Add(hexMap.GetNextHexInLine(startHex, targetHex));
        }

        // Animate fireball
        animating = true;
        character.StartCoroutine(Animate());
        // Wait animation to finish

        // Deal damage
        foreach (Hex hex in hitHexes)
        {
            if (hex.hasCharacter)
            {
                hex.character.TakeDamage(damage);
            }
        }
    }

    // Perform ability
    public override void PerformAbility(List<Hex> targetHexes)
    {

    }
}
