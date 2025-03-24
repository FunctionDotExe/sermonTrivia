using UnityEngine;

public class PrayerScenario : NPCInteraction
{
    protected override void Start()
    {
        // Set up the Prayer scenario
        npcName = "Troubled Person";
        dialogueText = "You walk by a person who seems to be having a bad day. You want to make a small prayer to help them, how do you do so? \"I want to make a prayer for him, since it looks like he's having a bad day.\"";
        
        // Set up the choices
        choiceTexts = new string[3];
        responseTexts = new string[3];
        pointsAwarded = new int[3];
        
        // Choice 1: Positive (Correct)
        choiceTexts[0] = "(Positive) - *Quietly* \"Please bless this man with a better day.\"";
        responseTexts[0] = "You offer a sincere, humble prayer. Though the person doesn't hear it, you feel a sense of peace knowing you've asked for help on their behalf.";
        pointsAwarded[0] = 20;
        
        // Choice 2: Neutral
        choiceTexts[1] = "(Neutral) - \"Never mind, I think he'll be fine\" *Walks away*";
        responseTexts[1] = "You decide not to pray and continue on your way. The opportunity to offer spiritual support passes.";
        pointsAwarded[1] = 0;
        
        // Choice 3: Negative
        choiceTexts[2] = "(Negative) - *loudly speaking* \"PLEASE LORD GOD THE FATHER HELP THIS POOR SOUL.\"";
        responseTexts[2] = "Your loud prayer draws unwanted attention and embarrasses the person. They quickly walk away, looking more uncomfortable than before.";
        pointsAwarded[2] = -15;
        
        // Set the correct choice
        correctChoiceIndex = 0;
        
        // Call the parent Start method to finish setup
        base.Start();
    }
} 