public enum States
{
    CardPlacement,
    Bid,
    Reveal,
    // Add more states here
    GetPoint,
    WinByPoints,
    Discard,
    EliminatePlayer,
    Rule,
    GameEnd
    
    
    
}

public enum TriggerEvents
{
    BiddingStarted,
    // Add more triggers here
    PlacingCards,
    CardSRevealing,
    GainPoint,
    TwoPointsAwarded , 
    RemoveEliminatedPlayer,
    ApplyNewRule, 
    EndsGame
    
}