using UnityEngine;

public class SaltAndLightScenario2 : NPCInteraction
{
    protected override void Start()
    {
        // Set up the Salt and Light scenario about the streetlamp
        npcName = "Community Member";
        dialogueText = "As you walk past a quiet street corner, you notice an old streetlamp flickering. The light barely shines, making the area feel unsafe at night. You also see a nearby community notice board with a phone number for reporting maintenance issues. People walk by without paying attention, and no one seems to care. You stop for a moment, considering what to do.";
        
        // Set up the choices
        choiceTexts = new string[4];
        responseTexts = new string[4];
        pointsAwarded = new int[4];
        
        // Choice 1: Indifferent
        choiceTexts[0] = "(Indifferent Response) - \"It's not my problem. I don't even live around here.\"";
        responseTexts[0] = "You walk away, leaving the area dark and potentially unsafe for others.";
        pointsAwarded[0] = -10;
        
        // Choice 2: Excuse-Making
        choiceTexts[1] = "(Excuse-Making Response) - \"Someone else probably already reported it.\"";
        responseTexts[1] = "You assume someone else will take care of it, but the light remains broken for weeks.";
        pointsAwarded[1] = -15;
        
        // Choice 3: Doubtful
        choiceTexts[2] = "(Doubtful Response) - \"One broken light isn't a big deal. It's not like it changes anything.\"";
        responseTexts[2] = "You minimize the importance of the issue and move on, not realizing how a dark street affects safety.";
        pointsAwarded[2] = -5;
        
        // Choice 4: Salt and Light (Correct)
        choiceTexts[3] = "(Salt and Light Response - Correct Choice) - \"A well-lit street keeps people safe. I should report this.\"";
        responseTexts[3] = "You take out your phone and call the number on the notice board. The area feels safer and more welcoming. You realize that even small actions—ones that no one else might notice—can make a difference in making the world a little brighter.";
        pointsAwarded[3] = 20;
        
        // Set the correct choice
        correctChoiceIndex = 3;
        
        // Call the parent Start method to finish setup
        base.Start();
    }
} 