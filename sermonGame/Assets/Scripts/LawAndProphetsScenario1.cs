using UnityEngine;

public class LawAndProphetsScenario1 : NPCInteraction
{
    protected override void Start()
    {
        // Set up the Law and Prophets scenario
        npcName = "Bible Scholar";
        dialogueText = "Multiple Choice: What did Jesus mean when He said He came to fulfill the Law and the Prophets?";
        
        // Set up the choices
        choiceTexts = new string[4];
        responseTexts = new string[4];
        pointsAwarded = new int[4];
        
        // Choice 1
        choiceTexts[0] = "A) He intended to remove the old laws and replace them with new ones.";
        responseTexts[0] = "That's not quite right. Jesus didn't come to abolish the Law but to fulfill it.";
        pointsAwarded[0] = -5;
        
        // Choice 2 (Correct)
        choiceTexts[1] = "B) He came to complete and bring out the true meaning of God's law.";
        responseTexts[1] = "Correct! Jesus came not to abolish the Law but to fulfill it by revealing its deeper meaning and living it out perfectly.";
        pointsAwarded[1] = 20;
        
        // Choice 3
        choiceTexts[2] = "C) He wanted people to focus only on love and ignore the commandments.";
        responseTexts[2] = "Not quite. Jesus emphasized that love fulfills the Law, but He didn't tell us to ignore the commandments.";
        pointsAwarded[2] = -5;
        
        // Choice 4
        choiceTexts[3] = "D) He was saying that the laws no longer mattered after His coming.";
        responseTexts[3] = "That's incorrect. Jesus specifically said He did not come to abolish the Law.";
        pointsAwarded[3] = -10;
        
        // Set the correct choice
        correctChoiceIndex = 1;
        
        // Call the parent Start method to finish setup
        base.Start();
    }
} 