using UnityEngine;

public class AngerScenario1 : NPCInteraction
{
    protected override void Start()
    {
        // Set up the Anger scenario
        npcName = "Passerby";
        dialogueText = "You're walking down a busy street when someone rushes past and bumps into you hard, nearly knocking you over. They don't stop or apologize—they just keep going as if nothing happened.";
        
        // Set up the choices
        choiceTexts = new string[4];
        responseTexts = new string[4];
        pointsAwarded = new int[4];
        
        // Choice 1: Retaliation
        choiceTexts[0] = "(Retaliation Response) - \"They think they can just bump into me? I should shove them back!\"";
        responseTexts[0] = "You chase after them and push them, escalating the situation into a potential conflict.";
        pointsAwarded[0] = -15;
        
        // Choice 2: Angry Outburst
        choiceTexts[1] = "(Angry Outburst Response) - \"Hey! Watch where you're going!\"";
        responseTexts[1] = "You yell at them, drawing attention from others around you and increasing your own stress.";
        pointsAwarded[1] = -10;
        
        // Choice 3: Passive Resentment
        choiceTexts[2] = "(Passive Resentment Response) - \"Unbelievable. People are so rude. I'll just stay mad about this all day.\"";
        responseTexts[2] = "You let the incident fester in your mind, allowing it to ruin your mood for hours.";
        pointsAwarded[2] = -5;
        
        // Choice 4: Righteous (Correct)
        choiceTexts[3] = "(Righteous Response - Correct Choice) - \"Maybe they didn't even realize. I'll let it go and move on.\"";
        responseTexts[3] = "You take a deep breath and continue walking, not letting it ruin your day. Letting go of your anger, you feel a sense of peace, knowing that staying calm was the better choice.";
        pointsAwarded[3] = 20;
        
        // Set the correct choice
        correctChoiceIndex = 3;
        
        // Call the parent Start method to finish setup
        base.Start();
    }
} 