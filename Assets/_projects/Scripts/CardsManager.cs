using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

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

    public List<GameObject> selectedCards = new();

    public void SelectedCards(CardInput cardInput, bool state, GameObject selectionIndicator)
    
    public List<CardVisual.CardType> GetSelectedCardTypes()
    {
        List<CardVisual.CardType> selectedCardTypes = new();
        foreach (CardVisual.CardType type in selectedCardTypes)
        {
            selectedCardTypes.Add(type);
        }

        return selectedCardTypes;
    }
}