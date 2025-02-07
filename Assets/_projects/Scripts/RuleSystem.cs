using System.Collections.Generic;
using UnityEngine;
using TMPro; 

public class RuleSystem : MonoBehaviour
{
    public TextMeshProUGUI discardText;
    public TextMeshProUGUI ruleText; 
    private int discardCount = 0;

    public class Card
    {
        public string Name { get; set; }  
        public string Description { get; set; }
        
        
        public Card(string name, string description)
        {
            Name = name;
            Description = description;
        }

        
        // public void ShowCard()
        // {
        //     Debug.Log("Card: " + Name + "\nDescription: " + Description);
        // }
    }


    private List< Card > ruleSet = new List<Card>
    {
        new Card("Bottoms up", "Players can reveal cards from the bottom of the pile."),
        new Card("Golden Gun", "Once during a reveal phase, a player may predict that the next card is a skull. If correct, it counts as a rose reveal. If incorrect, it counts as a skull and immediately ends the reveal phase."),
        new Card("Mystery Mode", "Players don’t have to reveal all their cards first during a reveal."),
        new Card("Spin to Win","The first bid of the round starts with a dice roll."),
        new Card("Market Surplus:","In a pile of 3 or more cards, players can reveal the bottom card without consequences (no points are awarded and skulls have no effect) ."),
        new Card("The Good Gone Bad","All Duds are now Skulls."),
        new Card("Card Inflation","Each player must play at least 2 cards per turn if they have more than 1 card in hand."),
        new Card("Go Green","When this card is revealed, every player with 3 or fewer cards receives a random card from the discard pile"),
    };

   

    void Start()
    {
        Debug.Log("Rule System Initialized.");
        UpdateDiscardUI();
    }

    
    public void DiscardCard()
    {
        discardCount++;
        UpdateDiscardUI();

        if (discardCount >= 4)
        {
            AddNewRule();
            discardCount = 0; // Reset counter
            UpdateDiscardUI();
        }
    }

    private void AddNewRule()
    {
        if (ruleSet.Count == 0)
        {
            Debug.Log("No more rules available.");
            return;
        }

        Card newRule = ruleSet[UnityEngine.Random.Range(0, ruleSet.Count)];
        Debug.Log("New Rule Added: " +newRule.Name+ " : " + newRule.Description);
        DisplayNewRule( newRule );
    }
    private void DisplayNewRule(Card newRule)
    {
        if (ruleText != null)
        {
            ruleText.text = "New Rule Added: " + newRule.Name + " : " + newRule.Description;
        }
    }

    private void UpdateDiscardUI()
    {
        if (discardText != null)
        {
            discardText.text = "Discards: " + discardCount + "/4";
        }
    }
}