using UnityEngine;

public class AlmsgivingScenario1 : NPCInteraction
{
    protected override void Start()
    {
        // Set up the Almsgiving scenario
        npcName = "Charity Worker";
        dialogueText = "You go to a charity station and think about how you will give some money. (in mind) \"I wonder how I can give to this charity, and help these people?\"";
        
        // Set up the choices
        choiceTexts = new string[2];
        responseTexts = new string[2];
        pointsAwarded = new int[2];
        
        // Choice 1: Positive (Correct)
        choiceTexts[0] = "(Positive) - Give the money silently and in an envelope.";
        responseTexts[0] = "You quietly donate without drawing attention to yourself. The charity worker smiles and thanks you sincerely.";
        pointsAwarded[0] = 20;
        
        // Choice 2: Negative
        choiceTexts[1] = "(Negative) - Loudly proclaim that \"I am donating money to this charity, and I'm a great person!\"";
        responseTexts[1] = "People around you look uncomfortable as you draw attention to your donation. The charity worker thanks you, but seems embarrassed by the display.";
        pointsAwarded[1] = -15;
        
        // Set the correct choice
        correctChoiceIndex = 0;
        
        // Call the parent Start method to finish setup
        base.Start();
    }
} 