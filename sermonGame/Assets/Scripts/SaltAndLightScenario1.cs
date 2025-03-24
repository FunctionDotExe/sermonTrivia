using UnityEngine;

public class SaltAndLightScenario1 : NPCInteraction
{
    protected override void Start()
    {
        // Set up the Salt and Light scenario about litter
        npcName = "Park Visitor";
        dialogueText = "You're walking through a park in the evening when you notice that the public benches and pathways are covered in litter—food wrappers, plastic bottles, and old newspapers. People walk past, ignoring the mess. Some even add to it, tossing their trash carelessly onto the ground. You pause, looking around. No one else seems to care.";
        
        // Set up the choices
        choiceTexts = new string[4];
        responseTexts = new string[4];
        pointsAwarded = new int[4];
        
        // Choice 1: Lazy
        choiceTexts[0] = "(Lazy Response) - \"This isn't my responsibility. Someone else will clean it up.\"";
        responseTexts[0] = "You walk away, leaving the litter behind. The park remains dirty, and others continue to add to the mess.";
        pointsAwarded[0] = -10;
        
        // Choice 2: Indifferent
        choiceTexts[1] = "(Indifferent Response) - \"It's just a little trash. It's not a big deal.\"";
        responseTexts[1] = "You ignore the litter and continue on your way. The park's beauty continues to be marred by the growing pile of trash.";
        pointsAwarded[1] = -15;
        
        // Choice 3: Excuse-Making
        choiceTexts[2] = "(Excuse-Making Response) - \"I'd help, but I don't have time for this right now.\"";
        responseTexts[2] = "You convince yourself you're too busy to help. The litter remains, and the problem gets worse.";
        pointsAwarded[2] = -5;
        
        // Choice 4: Salt and Light (Correct)
        choiceTexts[3] = "(Salt and Light Response - Correct Choice) - \"If no one else will do something, then I will.\"";
        responseTexts[3] = "You start picking up the trash, setting an example. A passerby nods to you and says, \"Hey, that's a good thing you're doing.\"";
        pointsAwarded[3] = 20;
        
        // Set the correct choice
        correctChoiceIndex = 3;
        
        // Call the parent Start method to finish setup
        base.Start();
    }
} 