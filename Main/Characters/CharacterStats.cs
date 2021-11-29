using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "New Character", menuName = "Character")]
public class CharacterStats : ScriptableObject
{
    public string characterName;
    public int health;
    public int strength;
    public int speed;
    public int range;
    public int pointCost;
    public Team team;
    public Animator animator;
    public RuntimeAnimatorController cardAnimatorController;
    public RuntimeAnimatorController characterAnimatorController;
    public Sprite sprite;
    public Material material;


    // Load stats for character
    public static CharacterStats LoadCharacterStats()
    {
        CharacterStats stats = Resources.Load<CharacterStats>(ENV.CHARACTER_RESOURCE_PATH + "Test/Stats");
        return stats;
    }


    // Load stats for character
    public static CharacterStats LoadCharacterStats(string characterName)
    {
        CharacterStats stats = Resources.Load<CharacterStats>(ENV.CHARACTER_RESOURCE_PATH + characterName + "/Stats");
        return stats;
    }
}
