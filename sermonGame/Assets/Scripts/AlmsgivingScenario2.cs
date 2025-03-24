using UnityEngine;

public class AlmsgivingScenario2 : NPCInteraction
{
    protected override void Start()
    {
        // Set up the Almsgiving scenario
        npcName = "Homeless Person";
        dialogueText = "A homeless person approaches you and asks you if there is any change you could give. \"Excuse me could you spare some change for me to buy some food?\"";
        
        // Set up the choices
        choiceTexts = new string[2];
        responseTexts = new string[2];
        pointsAwarded = new int[2];
        
        // Choice 1: Positive (Correct)
        choiceTexts[0] = "(Positive) - \"Yes of course I could give you my change, have a great day\".";
        responseTexts[0] = "The person's face lights up with gratitude. \"Thank you so much! God bless you.\" Your small act of kindness might make a big difference in their day.";
        pointsAwarded[0] = 20;
        
        // Choice 2: Negative
        choiceTexts[1] = "(Negative) - You jingle the change in your hand and just walk away while laughing.";
        responseTexts[1] = "The person looks down, dejected. Your mockery has added to their suffering rather than easing it.";
        pointsAwarded[1] = -20;
        
        // Set the correct choice
        correctChoiceIndex = 0;
        
        // Call the parent Start method to finish setup
        base.Start();
    }
} 