using UnityEngine;

public class GoldenRuleScenario2 : NPCInteraction
{
    protected override void Start()
    {
        // Set up the Golden Rule scenario
        npcName = "Basketball Player";
        dialogueText = "You're playing basketball with your friends, and some other guy comes and asks if he could play. All of your friends say \"No we don't need you go find someone else\"";
        
        // Set up the choices
        choiceTexts = new string[2];
        responseTexts = new string[2];
        pointsAwarded = new int[2];
        
        // Choice 1: Positive (Correct)
        choiceTexts[0] = "(Positive) - \"You know what guys nothing bad would happen if he plays so why not just include him\"";
        responseTexts[0] = "Your friends reluctantly agree, and the new player turns out to be friendly and fun to play with. Everyone ends up having a good time.";
        pointsAwarded[0] = 20;
        
        // Choice 2: Negative
        choiceTexts[1] = "(Negative) - \"Yeah you see we are playing and having fun get lost\"";
        responseTexts[1] = "The person walks away looking dejected. Your game continues, but there's an uncomfortable feeling in the group.";
        pointsAwarded[1] = -15;
        
        // Set the correct choice
        correctChoiceIndex = 0;
        
        // Call the parent Start method to finish setup
        base.Start();
    }
} 