using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class InterrogationSystem : MonoBehaviour
{
    [System.Serializable]
    public class Question
    {
        public string questionText;
        public string[] possibleAnswers;
        public int correctAnswerIndex;
        public string correctResponse;
        public string wrongResponse;
    }

    public List<Question> questions;
    public float timePerQuestion = 15f;
    public GameObject interrogationUI;
    public Text questionText;
    public Button[] answerButtons;
    public Text timerText;
    public Text resultText;

    private Question currentQuestion;
    private float currentTime;
    private bool isQuestionActive;
    private Coroutine timerCoroutine;

    private void Start()
    {
        InitializeQuestions();
        
        for (int i = 0; i < answerButtons.Length; i++)
        {
            int index = i; // Capture the index for the closure
            answerButtons[i].onClick.AddListener(() => AnswerQuestion(index));
        }
    }

    private void InitializeQuestions()
    {
        questions = new List<Question>
        {
            new Question
            {
                questionText = "What is the purpose of your visit?",
                possibleAnswers = new string[] { "Tourism", "Business meeting", "Just passing through", "Visiting family" },
                correctAnswerIndex = 0,
                correctResponse = "Passport shows tourist visa. Answer matches.",
                wrongResponse = "Inconsistent with visa type."
            },
            new Question
            {
                questionText = "How long do you plan to stay?",
                possibleAnswers = new string[] { "1 week", "2 days", "1 month", "Not sure yet" },
                correctAnswerIndex = 2,
                correctResponse = "Matches declared duration.",
                wrongResponse = "Doesn't match declared duration."
            },
            new Question
            {
                questionText = "Where will you be staying?",
                possibleAnswers = new string[] { "Hotel", "With friends", "Airbnb", "Haven't decided" },
                correctAnswerIndex = 0,
                correctResponse = "Hotel booking confirmed.",
                wrongResponse = "No accommodation records found."
            }
        };
    }

    public void StartInterrogation()
    {
        if (questions.Count == 0) return;

        currentQuestion = questions[Random.Range(0, questions.Count)];
        currentTime = timePerQuestion;
        isQuestionActive = true;

        interrogationUI.SetActive(true);
        questionText.text = currentQuestion.questionText;

        for (int i = 0; i < answerButtons.Length; i++)
        {
            if (i < currentQuestion.possibleAnswers.Length)
            {
                answerButtons[i].gameObject.SetActive(true);
                answerButtons[i].GetComponentInChildren<Text>().text = currentQuestion.possibleAnswers[i];
            }
            else
            {
                answerButtons[i].gameObject.SetActive(false);
            }
        }

        timerCoroutine = StartCoroutine(QuestionTimer());
    }

    private IEnumerator QuestionTimer()
    {
        while (currentTime > 0 && isQuestionActive)
        {
            currentTime -= Time.deltaTime;
            timerText.text = $"Time: {Mathf.CeilToInt(currentTime)}s";
            yield return null;
        }

        if (isQuestionActive)
        {
            TimeOut();
        }
    }

    public void AnswerQuestion(int answerIndex)
    {
        if (!isQuestionActive) return;

        isQuestionActive = false;
        StopCoroutine(timerCoroutine);

        bool isCorrect = (answerIndex == currentQuestion.correctAnswerIndex);
        resultText.text = isCorrect ? currentQuestion.correctResponse : currentQuestion.wrongResponse;
        resultText.gameObject.SetActive(true);

        if (isCorrect)
        {
            GameManager.Instance.score += 50;
        }
        else
        {
            GameManager.Instance.score -= 30;
        }

        Invoke("EndInterrogation", 2f);
    }

    private void TimeOut()
    {
        isQuestionActive = false;
        resultText.text = "Time's up! No answer given.";
        resultText.gameObject.SetActive(true);
        GameManager.Instance.score -= 20;

        Invoke("EndInterrogation", 2f);
    }

    private void EndInterrogation()
    {
        interrogationUI.SetActive(false);
        resultText.gameObject.SetActive(false);
    }
}