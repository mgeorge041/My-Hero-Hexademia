using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class PlayerUI : MonoBehaviour
{
    // Character UI
    public Image characterPortrait;
    public Image villainBar;
    public Image heroBar;
    public Text heroPointsLabel;
    public Text villainPointsLabel;

    // Heroes/villains UI
    private bool heroesExpanded = false;
    public GameObject heroesPanel;
    public GameObject villainsPanel;
    private List<CharacterCard> characterCards = new List<CharacterCard>();
    public Button showHeroesButton;
    public Button showVillainsButton;
    public Image showHeroesButtonImage;
    public Image showVillainsButtonImage;

    // Character UI
    public CharacterStatsPanel characterStatsPanel;
    public Image characterPortrait2;
    public Image characterHealthbar;
    public Text characterHealthbarText;
    public Image attackIcon;
    public Image speedIcon;
    public Text attackText;
    public Text defenseText;
    public Text rangeText;
    public Text speedText;
    public Image ability1Image;
    public Image ability2Image;


    // Player
    public Player player;


    // Reset
    public void Reset()
    {
        foreach (CharacterCard characterCard in characterCards)
        {
            C.Destroy(characterCard);
        }
        characterCards.Clear();
        UpdateBarsAndPointLabels();
    }


    // Animate opening menu
    public IEnumerator AnimateMenu(RectTransform rect, int size)
    {
        float t = 0;
        Vector2 newSizeDelta = new Vector2(size, 1);
        while (t < 1)
        {
            Vector2 sizeDelta = rect.sizeDelta;
            heroesPanel.GetComponent<RectTransform>().sizeDelta = Vector2.Lerp(sizeDelta, newSizeDelta, t);
            t += Time.deltaTime;
            yield return null;
        }
        rect.sizeDelta = new Vector2(size, 1);
    }


    // Toggle heroes panel
    public void ToggleHeroesPanel()
    {
        heroesPanel.gameObject.SetActive(!heroesPanel.gameObject.activeSelf);
        /*
        if (!heroesExpanded)
            StartCoroutine(AnimateMenu(heroesPanel.GetComponent<RectTransform>(), 0));
        else
            StartCoroutine(AnimateMenu(heroesPanel.GetComponent<RectTransform>(), 20));
        */
        showHeroesButtonImage.transform.localScale = new Vector3(-showHeroesButtonImage.transform.localScale.x, 1, 1);
    }


    // Toggle villains panel
    public void ToggleVillainsPanel()
    {
        villainsPanel.gameObject.SetActive(!villainsPanel.gameObject.activeSelf);
        showVillainsButtonImage.transform.localScale = new Vector3(-showVillainsButtonImage.transform.localScale.x, 1, 1);
    }


    // Show heroes panel
    public void ShowHeroesPanel()
    {
        heroesPanel.gameObject.SetActive(true);
        showHeroesButtonImage.transform.rotation = new Quaternion(0, 0, 180, 0);
    }


    // Hide heroes panel
    public void HideHeroesPanel()
    {
        heroesPanel.gameObject.SetActive(false);
        showHeroesButtonImage.transform.rotation = new Quaternion(0, 0, 0, 0);
    }


    // Show villains panel
    public void ShowVillainsPanel()
    {
        villainsPanel.gameObject.SetActive(true);
        showVillainsButtonImage.transform.rotation = new Quaternion(0, 0, 180, 0);
    }


    // Hide villains panel
    public void HideVillainsPanel()
    {
        villainsPanel.gameObject.SetActive(false);
        showVillainsButtonImage.transform.rotation = new Quaternion(0, 0, 0, 0);
    }


    // Update bar and point labels
    public void UpdateBarsAndPointLabels()
    {
        UpdateHeroBarAndPointsLabel();
        UpdateVillainBarAndPointsLabel();
    }


    // Update hero bar and points label
    public void UpdateHeroBarAndPointsLabel()
    {
        heroPointsLabel.text = player.heroPoints.ToString();
        heroBar.fillAmount = Math.Min((float)player.heroPoints / player.maxPoints, 1);
    }


    // Update villain bar and points label
    public void UpdateVillainBarAndPointsLabel()
    {
        villainPointsLabel.text = player.villainPoints.ToString();
        villainBar.fillAmount = Math.Min((float)player.villainPoints / player.maxPoints, 1);
    }


    // Set player selected character card
    public void SetPlayerSelectedCharacterCard(CharacterCard characterCard)
    {
        player.SetSelectedCharacterCard(characterCard);
    }


    // Play character card
    public void PlayCharacterCard(CharacterCard characterCard)
    {
        UpdateBarsAndPointLabels();
        characterCards.Remove(characterCard);
        C.Destroy(characterCard);
    }


    // Add character card
    public void AddCharacterCard(CharacterCard characterCard)
    {
        if (characterCard.team == Team.Hero)
        {
            characterCard.transform.SetParent(heroesPanel.transform);
        }
        else
        {
            characterCard.transform.SetParent(villainsPanel.transform);
        }
        characterCards.Add(characterCard);
    }


    // Update character
    public void UpdateCharacter(Character character)
    {
        SetSelectedCharacter(character);
    }


    // Set selected character
    public void SetSelectedCharacter(Character character)
    {
        characterStatsPanel.SetSelectedCharacter(character);
    }


    // Start
    public void Start()
    {
        
    }
}
