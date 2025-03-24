using UnityEngine;

public class BullyingScenario : NPCInteraction
{
    // Override the Start method from the parent class
    protected override void Start()
    {
        // Set up the bullying scenario
        npcName = "Bullied Child";
        dialogueText = "You're walking near a public park when you see a group of kids picking on a younger child. They shove him to the ground and laugh as he tries to get up. Some bystanders glance over but do nothing.";
        
        // Set up the choices
        choiceTexts = new string[4];
        responseTexts = new string[4];
        pointsAwarded = new int[4];
        
        // Choice 1: Passive
        choiceTexts[0] = "(Passive Response) - \"I don't want to get involved. Someone else will step in.\"";
        responseTexts[0] = "You walk away. The bullying continues, and the child looks even more dejected.";
        pointsAwarded[0] = -10;
        
        // Choice 2: Dismissive
        choiceTexts[1] = "(Dismissive Response) - \"Kids will be kids. It's not that serious.\"";
        responseTexts[1] = "You ignore the situation. Later, you learn the child has been bullied for weeks.";
        pointsAwarded[1] = -15;
        
        // Choice 3: Cowardly
        choiceTexts[2] = "(Cowardly Response) - \"I'd help, but what if they come after me next?\"";
        responseTexts[2] = "Your fear keeps you from acting. The child notices you hesitating and looks away in disappointment.";
        pointsAwarded[2] = -5;
        
        // Choice 4: Righteous (Correct)
        choiceTexts[3] = "(Righteous Response) - \"Hey! Leave him alone. Pick on someone your own size.\"";
        responseTexts[3] = "The bullies run away. The child looks up at you gratefully. \"Thanks... I thought no one was going to help.\"";
        pointsAwarded[3] = 20;
        
        // Set the correct choice
        correctChoiceIndex = 3;
        
        // Call the parent Start method to finish setup
        base.Start();
    }
} 