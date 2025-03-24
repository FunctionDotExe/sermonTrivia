using UnityEngine;

public class JudgingOthersScenario2 : NPCInteraction
{
    protected override void Start()
    {
        // Set up the Judging Others scenario
        npcName = "Judgmental Mother";
        dialogueText = "A woman judges the shoes her daughter picked from the store. (Talking to a friend) \"I don't get why she bought such ugly shoes from the store, modern fashion is terrible.\"";
        
        // Set up the choices
        choiceTexts = new string[2];
        responseTexts = new string[2];
        pointsAwarded = new int[2];
        
        // Choice 1: Positive (Correct)
        choiceTexts[0] = "(Positive) - \"Fashion and our opinion change over time, maybe what your daughter sees in those shoes is something important to her.\"";
        responseTexts[0] = "The mother pauses and considers your words. \"I hadn't thought of it that way. Maybe I should ask her what she likes about them instead of just criticizing.\"";
        pointsAwarded[0] = 20;
        
        // Choice 2: Negative
        choiceTexts[1] = "(Negative) - \"Modern fashion is the worst, I hate it so much when my kids waste their money.\"";
        responseTexts[1] = "You reinforce her judgment, and she continues criticizing her daughter's choices.";
        pointsAwarded[1] = -15;
        
        // Set the correct choice
        correctChoiceIndex = 0;
        
        // Call the parent Start method to finish setup
        base.Start();
    }
} 