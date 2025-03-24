using UnityEngine;

public class LoveForEnemiesScenario : NPCInteraction
{
    protected override void Start()
    {
        // Set up the Love for Enemies scenario
        npcName = "Angry Stranger";
        dialogueText = "You find a guy on the street and when you talk to him he gets mad and yells at you: \"Get out of my way you idiot!\"";
        
        // Set up the choices
        choiceTexts = new string[4];
        responseTexts = new string[4];
        pointsAwarded = new int[4];
        
        // Choice 1: Aggressive
        choiceTexts[0] = "(Aggressive Response) - \"Who are you calling an idiot? Maybe watch where you're going next time!\"";
        responseTexts[0] = "The situation escalates as you both exchange angry words. You walk away feeling upset and tense.";
        pointsAwarded[0] = -15;
        
        // Choice 2: Sarcastic
        choiceTexts[1] = "(Sarcastic Response) - \"Wow, someone's having a bad day. Hope you don't treat everyone like this.\"";
        responseTexts[1] = "Your sarcasm only makes the person more defensive. They walk away muttering under their breath.";
        pointsAwarded[1] = -10;
        
        // Choice 3: Indifferent
        choiceTexts[2] = "(Indifferent Response) - \"Whatever, not my problem.\"";
        responseTexts[2] = "You dismiss the interaction, but miss an opportunity to show compassion to someone who might be struggling.";
        pointsAwarded[2] = -5;
        
        // Choice 4: Compassionate (Correct)
        choiceTexts[3] = "(Compassionate Response - Correct Choice) - \"Hey, I didn't mean to bother you. Is everything okay?\"";
        responseTexts[3] = "The person's expression softens. \"Sorry... just having a rough day. Didn't mean to snap at you.\" Your kindness in the face of hostility made a difference.";
        pointsAwarded[3] = 20;
        
        // Set the correct choice
        correctChoiceIndex = 3;
        
        // Call the parent Start method to finish setup
        base.Start();
    }
} 