using UnityEngine;

public class JudgingOthersScenario1 : NPCInteraction
{
    protected override void Start()
    {
        // Set up the Judging Others scenario
        npcName = "Complaining Friend";
        dialogueText = "A guy is complaining about how messy his friend is. \"Yeah he's so messy around the entire apartment, he can't even put his clothes in the hamper properly.\"";
        
        // Set up the choices
        choiceTexts = new string[2];
        responseTexts = new string[2];
        pointsAwarded = new int[2];
        
        // Choice 1: Positive (Correct)
        choiceTexts[0] = "(Positive) - \"Hey we all get messy sometimes, the best thing to do would be to help each other out.\"";
        responseTexts[0] = "Your friend considers your perspective. \"I guess you're right. Maybe I should talk to him instead of just complaining.\"";
        pointsAwarded[0] = 20;
        
        // Choice 2: Negative
        choiceTexts[1] = "(Negative) - \"That's so real right, I hate it when my friends are being so lazy from no good reason\"";
        responseTexts[1] = "You join in the judgment, and the conversation spirals into more negativity about others.";
        pointsAwarded[1] = -15;
        
        // Set the correct choice
        correctChoiceIndex = 0;
        
        // Call the parent Start method to finish setup
        base.Start();
    }
} 