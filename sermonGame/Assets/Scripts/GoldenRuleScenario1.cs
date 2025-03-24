using UnityEngine;

public class GoldenRuleScenario1 : NPCInteraction
{
    protected override void Start()
    {
        // Set up the Golden Rule scenario
        npcName = "Food Shop Worker";
        dialogueText = "It's a busy day at an outdoor food shop when you finally get to order after a long awaited lineup in the heat, you notice that the worker has mixed your order up, but also take into account that it's a very busy day.";
        
        // Set up the choices
        choiceTexts = new string[2];
        responseTexts = new string[2];
        pointsAwarded = new int[2];
        
        // Choice 1: Positive (Correct)
        choiceTexts[0] = "(Positive) - \"Oh no problem at all I totally understand I see it's a very busy day for you guys today\"";
        responseTexts[0] = "The worker looks relieved and grateful. \"Thank you so much for understanding. Let me fix that for you right away.\" They quickly correct your order and add a small extra item as thanks.";
        pointsAwarded[0] = 20;
        
        // Choice 2: Negative
        choiceTexts[1] = "(Negative) - \"Oh my goodness I have waited almost 15 minutes in the heat and you cant even give me the food I ordered!\"";
        responseTexts[1] = "The worker looks stressed and apologizes repeatedly while fixing your order. Other customers look uncomfortable with your outburst.";
        pointsAwarded[1] = -15;
        
        // Set the correct choice
        correctChoiceIndex = 0;
        
        // Call the parent Start method to finish setup
        base.Start();
    }
} 