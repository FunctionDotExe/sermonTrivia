using UnityEngine;

public class AngerScenario2 : NPCInteraction
{
    protected override void Start()
    {
        // Set up the Anger scenario
        npcName = "Water Fountain User";
        dialogueText = "You're walking through a public square when someone suddenly splashes a bit of water on you while at the water fountain. They don't seem to notice and keep going.";
        
        // Set up the choices
        choiceTexts = new string[4];
        responseTexts = new string[4];
        pointsAwarded = new int[4];
        
        // Choice 1: Angry
        choiceTexts[0] = "(Angry Response) - \"Seriously? How rude! I should yell at them.\"";
        responseTexts[0] = "You raise your voice and create a scene over a small splash of water.";
        pointsAwarded[0] = -15;
        
        // Choice 2: Passive-Aggressive
        choiceTexts[1] = "(Passive-Aggressive Response) - \"Wow, guess people don't have any manners these days.\"";
        responseTexts[1] = "You make a loud comment that others can hear, creating an uncomfortable atmosphere.";
        pointsAwarded[1] = -10;
        
        // Choice 3: Resentful
        choiceTexts[2] = "(Resentful Response) - \"That was annoying. I'll stay mad about this for a while.\"";
        responseTexts[2] = "You let a minor incident affect your mood negatively.";
        pointsAwarded[2] = -5;
        
        // Choice 4: Righteous (Correct)
        choiceTexts[3] = "(Righteous Response - Correct Choice) - \"It's just a little water. Not worth getting upset over.\"";
        responseTexts[3] = "You brush it off and continue with your day. A moment later, a breeze dries off the water, and you realize how small the moment was. By letting it go, you kept your peace and didn't let a minor inconvenience ruin your mood.";
        pointsAwarded[3] = 20;
        
        // Set the correct choice
        correctChoiceIndex = 3;
        
        // Call the parent Start method to finish setup
        base.Start();
    }
} 