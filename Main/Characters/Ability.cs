using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Ability
{
    public Character character;
    public string abilityName;
    public bool animating = false;

    public abstract List<Hex> GetTargets(HexMap hexMap);
    public abstract IEnumerator Animate();
    public abstract void PerformAbility(Hex targetHex);
    public abstract void PerformAbility(List<Hex> targetHexes);
}
