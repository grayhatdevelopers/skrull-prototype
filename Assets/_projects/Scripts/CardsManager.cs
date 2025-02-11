using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CardsManager : MonoBehaviour
{
    public static CardsManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public List<GameObject> selectedCardTypes = new();

    public void SelectedCards(CardInput cardInput, bool state, GameObject selectionIndicator)
    {
        // Card Selection and indicator
        var cardNumber = selectionIndicator.GetComponentInChildren<TMP_Text>();
        if (state)
        {
            selectionIndicator.SetActive(true);
            selectedCardTypes.Add(cardInput.gameObject);
            cardNumber.text = selectedCardTypes.Count.ToString();
        }
        else
        {
            selectionIndicator.SetActive(false);
            selectedCardTypes.Remove(cardInput.gameObject);

            foreach (GameObject cards in selectedCardTypes)
            {
                cards.GetComponent<CardInput>().cardVisual.selectionIndicator.GetComponentInChildren<TMP_Text>().text =
                    $"{selectedCardTypes.IndexOf(cards) + 1}";
            }
        }
    }
}