using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterCard : MonoBehaviour
{
    public int cost 
    { 
        get 
        { 
            return stats.pointCost; 
        } 
    }
    public Team team
    {
        get
        {
            return stats.team;
        }
    }
    public Player player;
    public CharacterStats stats;
    public Animator animator;
    public RuntimeAnimatorController animatorController;
    public Image sprite;
    public Text costText;
    private bool walkAnimation = false;


    // Create character card
    public static CharacterCard CreateCharacterCard(CharacterStats stats)
    {
        CharacterCard newCharacterCard = Instantiate(GetCharacterCardPrefab());
        newCharacterCard.SetCharacterStats(stats);
        return newCharacterCard;
    }


    // Create character card
    public static CharacterCard CreateCharacterCard(string characterName)
    {
        CharacterCard newCharacterCard = Instantiate(GetCharacterCardPrefab());
        newCharacterCard.SetCharacterStats(CharacterStats.LoadCharacterStats(characterName));
        return newCharacterCard;
    }


    // Get character card prefab
    public static CharacterCard GetCharacterCardPrefab()
    {
        CharacterCard newCharacterCard = Resources.Load<CharacterCard>(ENV.CHARACTER_CARD_PREFAB_RESOURCE_PATH).GetComponent<CharacterCard>();
        return newCharacterCard;
    }


    // Set character placement
    public void SetCharacterCard()
    {
        if (player.SetSelectedCharacterCard(this))
        {
            walkAnimation = !walkAnimation;
            animator.SetBool("walk", walkAnimation);
        }
        else
        {
            StopWalkAnimation();
        }
    }


    // Stop walk animation
    public void StopWalkAnimation()
    {
        walkAnimation = false;
        animator.SetBool("walk", walkAnimation);
    }


    // Set character stats
    public void SetCharacterStats(CharacterStats stats)
    {
        this.stats = stats;
        //animator = stats.animator;
        sprite.sprite = stats.sprite;
        sprite.SetNativeSize();
        costText.text = stats.pointCost.ToString();
        animator.runtimeAnimatorController = stats.cardAnimatorController;
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
