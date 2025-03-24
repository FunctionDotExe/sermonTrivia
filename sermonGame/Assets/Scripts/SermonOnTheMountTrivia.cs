using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SermonOnTheMountTrivia : MonoBehaviour
{
    [SerializeField] private GameObject triviaPanel;
    [SerializeField] private Text questionText;
    [SerializeField] private Button[] answerButtons;
    [SerializeField] private Text[] answerTexts;
    [SerializeField] private Text feedbackText;
    
    private List<TriviaQuestion> triviaQuestions = new List<TriviaQuestion>();
    private TriviaQuestion currentQuestion;
    private int currentQuestionIndex = 0;
    
    [System.Serializable]
    public class TriviaQuestion
    {
        public string question;
        public string[] answers;
        public int correctAnswerIndex;
        public int pointsForCorrectAnswer = 10;
    }
    
    void Start()
    {
        Debug.Log("SermonOnTheMountTrivia Start called");
        SetupTriviaQuestions();
    }
    
    void SetupTriviaQuestions()
    {
        Debug.Log("Setting up trivia questions");
        
        // Add your trivia questions here
        TriviaQuestion q1 = new TriviaQuestion();
        q1.question = "What is the first Beatitude?";
        q1.answers = new string[] {
            "Blessed are the poor in spirit, for theirs is the kingdom of heaven.",
            "Blessed are those who mourn, for they shall be comforted.",
            "Blessed are the meek, for they shall inherit the earth.",
            "Blessed are the peacemakers, for they shall be called sons of God."
        };
        q1.correctAnswerIndex = 0;
        triviaQuestions.Add(q1);
        
        // Add more questions as needed
        
        Debug.Log("Successfully initialized " + triviaQuestions.Count + " trivia questions");
    }
    
    public void ShowQuestion(int questionIndex)
    {
        if (questionIndex < 0 || questionIndex >= triviaQuestions.Count)
        {
            Debug.LogError("Invalid question index: " + questionIndex);
            return;
        }
        
        currentQuestionIndex = questionIndex;
        currentQuestion = triviaQuestions[questionIndex];
        
        // Display the question
        questionText.text = currentQuestion.question;
        
        // Set up answer buttons
        for (int i = 0; i < answerButtons.Length; i++)
        {
            if (i < currentQuestion.answers.Length)
            {
                answerButtons[i].gameObject.SetActive(true);
                answerTexts[i].text = currentQuestion.answers[i];
            }
            else
            {
                answerButtons[i].gameObject.SetActive(false);
            }
        }
        
        // Show the trivia panel
        triviaPanel.SetActive(true);
        
        // Clear feedback text
        feedbackText.text = "";
    }
    
    public void OnAnswerSelected(int answerIndex)
    {
        if (currentQuestion == null) return;
        
        bool isCorrect = (answerIndex == currentQuestion.correctAnswerIndex);
        
        if (isCorrect)
        {
            // Award points for correct answer
            ScoreHelper.AwardPoints(currentQuestion.pointsForCorrectAnswer);
            
            // Show feedback
            feedbackText.text = "Correct! +" + currentQuestion.pointsForCorrectAnswer + " points";
        }
        else
        {
            // Show feedback for incorrect answer
            feedbackText.text = "Incorrect. The correct answer is: " + 
                currentQuestion.answers[currentQuestion.correctAnswerIndex];
        }
        
        // Disable answer buttons
        for (int i = 0; i < answerButtons.Length; i++)
        {
            answerButtons[i].interactable = false;
        }
        
        // Hide the panel after a delay
        StartCoroutine(HidePanelAfterDelay(3f));
    }
    
    private IEnumerator HidePanelAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        triviaPanel.SetActive(false);
        
        // Re-enable answer buttons for next question
        for (int i = 0; i < answerButtons.Length; i++)
        {
            answerButtons[i].interactable = true;
        }
    }
} 