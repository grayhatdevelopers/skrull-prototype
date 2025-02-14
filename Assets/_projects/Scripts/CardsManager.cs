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
    {
        // Card Selection and indicator
        var cardNumber = selectionIndicator.GetComponentInChildren<TMP_Text>();
        if (state)
        {
            selectionIndicator.SetActive(true);
            selectedCards.Add(cardInput.gameObject);
            cardNumber.text = selectedCards.Count.ToString();
        }
        else
        {
            selectionIndicator.SetActive(false);
            selectedCards.Remove(cardInput.gameObject);

            foreach (GameObject cards in selectedCards)
            {
                cards.GetComponent<CardInput>().cardVisual.selectionIndicator.GetComponentInChildren<TMP_Text>().text =
                    $"{selectedCards.IndexOf(cards) + 1}";
            }
        }
    }


    public List<CardVisual.CardType> GetSelectedCardTypes()
    {
        List<CardVisual.CardType> selectedCardTypes = new();
        foreach (GameObject card in selectedCards)
        {
            CardVisual.CardType type = card.GetComponent<CardInput>().cardVisual.cardType;

            selectedCardTypes.Add(type);
        }

        return selectedCardTypes;
    }
    
#region BiddingRegion
    
    public int currentBidAmount = 0;

    public bool ValidateBidding()
    {
        int selectedCount = selectedCards.Count;
        
        if (selectedCount < 2)
        {
            Debug.LogWarning("You must place at least 2 selected cards to bid");
            return false;
        }

        int maxBid = NetworkManager.Instance.GetMaxBidAmount();
        if (selectedCount > maxBid)
        {
            Debug.LogWarning($"You cannot bid more than {maxBid} selected cards.");
            return false;
        }
        
        return true;
    }

    public void PlaceBid()
    {
        if (!ValidateBidding()) return;

        currentBidAmount = selectedCards.Count;
        NetworkManager.Instance.PlayerBid(currentBidAmount);
    }

    public void PassBid()
    {
        NetworkManager.Instance.PlayerPass();
    }

    public void ClearSelectedCards()
    {
        selectedCards.Clear();
    }

    public List<CardVisual.CardType> GetSelectedCardsTypes()
    {
        List<CardVisual.CardType> selectedCardTypes = new();
        foreach (GameObject card in selectedCards)
        {
            CardVisual.CardType type = card.GetComponent<CardInput>().cardVisual.cardType;
            selectedCardTypes.Add(type);
        }
        return selectedCardTypes;
    }

    public bool CanPlaceBid()
    {
        int selectedCount = selectedCards.Count;
        int maxAllowed = NetworkManager.Instance.GetMaxBidAmount();
        
        return selectedCount >= 2 && selectedCount <= maxAllowed;
        
    }
    

#endregion    
   
    
}