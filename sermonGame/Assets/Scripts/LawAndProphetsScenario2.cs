using UnityEngine;

public class LawAndProphetsScenario2 : NPCInteraction
{
    protected override void Start()
    {
        // Set up the Law and Prophets scenario
        npcName = "Bible Scholar";
        dialogueText = "Multiple choice: According to Jesus, who will be considered great in the kingdom of heaven?";
        
        // Set up the choices
        choiceTexts = new string[4];
        responseTexts = new string[4];
        pointsAwarded = new int[4];
        
        // Choice 1
        choiceTexts[0] = "A) Those who memorize the commandments but do not follow them.";
        responseTexts[0] = "That's not correct. Jesus criticized those who knew the Law but didn't practice it.";
        pointsAwarded[0] = -10;
        
        // Choice 2 (Correct)
        choiceTexts[1] = "B) Those who follow and teach the commandments.";
        responseTexts[1] = "Correct! Jesus said whoever practices and teaches the commandments will be called great in the kingdom of heaven.";
        pointsAwarded[1] = 20;
        
        // Choice 3
        choiceTexts[2] = "C) Those who find loopholes in the Law to benefit themselves.";
        responseTexts[2] = "That's incorrect. Jesus criticized the Pharisees for finding loopholes in the Law.";
        pointsAwarded[2] = -15;
        
        // Choice 4
        choiceTexts[3] = "D) Those who claim to be religious but do not practice righteousness.";
        responseTexts[3] = "That's not right. Jesus warned against hypocrisy and emphasized the importance of practicing what you preach.";
        pointsAwarded[3] = -5;
        
        // Set the correct choice
        correctChoiceIndex = 1;
        
        // Call the parent Start method to finish setup
        base.Start();
    }
} 