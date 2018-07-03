using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class QuizGameDataManager : MonoBehaviour
{
    private QuestionList loadedGameData;

    [SerializeField]
    protected string gameDataFileName;
    
    [SerializeField]
    private GameObject question;
    [SerializeField]
    private GameObject[] answers;
    [SerializeField]
    private Text scoreText;
    [SerializeField]
    private GameObject nextQuestion;
    [SerializeField]
    private Image image;

    private int selectedAnswer = 0;
    private int score = 0;
    private string[] letters = new string[] {"A) ", "B) ", "C) ", "D) "};

    private int questionIndex;

    private void Start()
    {
        LoadGameData();
        questionIndex = 0;
        scoreText.text = 0.ToString();
        nextQuestion.SetActive(false);
    }

    private void LoadGameData()
    {
        // Path.Combine monta o caminho do arquivo 
        // Application.StreamingAssets aponta para Assets/StreamingAssets no Editor, e StreamingAssets ao criar uma build
        string filePath = Path.Combine(Application.streamingAssetsPath, gameDataFileName);

        if (File.Exists(filePath))
        {
            // Lê o json do arquivo para uma string
            string dataAsJson = File.ReadAllText(filePath);
            //Através da API JsonUtility Serializa o conteúdo String em um objeto do tipo GameData. 
            //Aqui, claramente, as estrutura precisam ser compatíveis
            loadedGameData = JsonUtility.FromJson<QuestionList>(dataAsJson);
            // for (int i = 0; i < loadedGameData.questions.Length; i++)
            // {
            //     Debug.Log("Description " + loadedGameData.questions[i].description);
            //     for (int j = 0; j < loadedGameData.questions[i].answers.Length; j++)
            //         Debug.Log("Answers " + loadedGameData.questions[i].answers[j]);

            // }
        }else
            Debug.LogError("Cannot load game data!");
    }

    private void LoadQuestion(int index) {
        question.GetComponent<Text>().text = loadedGameData.questions[index].description;
        Debug.Log(Resources.Load<Sprite>(loadedGameData.questions[questionIndex].image));
        image.sprite = Resources.Load<Sprite>(loadedGameData.questions[questionIndex].image);
        for (int j = 0; j < loadedGameData.questions[index].answers.Length; j++) {
            answers[j].GetComponentInChildren<Text>().text = letters[j] + loadedGameData.questions[index].answers[j];
        } 
    }

    [System.Serializable]
    public class QuestionList
    {
        public QuizGameData[] questions;
    }

    [System.Serializable]
    public class QuizGameData
    {
        public string description;
        public string[] answers;
        public int correct;
        public string image;
    }

    public QuestionList GetGameData () {
        return loadedGameData;
    }

    void Update () {
        LoadQuestion(questionIndex);
        if (selectedAnswer != 0) {
            
        }
    }

    public void setSelectedAnswer (int num) {
        selectedAnswer = num;
        for (int i = 0; i < 4; i++) {
            if (i+1 == loadedGameData.questions[questionIndex].correct) {
                answers[i].GetComponent<Image>().color = new Color32(54, 244, 88, 255);
            } else {
                answers[i].GetComponent<Image>().color = new Color32(244, 67, 54, 255);
            }
            answers[i].GetComponent<Button>().interactable = false;
        }
        if (selectedAnswer == loadedGameData.questions[questionIndex].correct) {
            increaseScore();
        }
        toggleNextQuestion();

    }

    private void increaseScore() {
        score += 10;
        scoreText.text = score.ToString();
    }

    private void toggleNextQuestion () {
        nextQuestion.SetActive(!nextQuestion.activeSelf);
    }

    public void GoToNextQuestion () {
        for (int i = 0; i < 4; i++) {
            answers[i].GetComponent<Image>().color = Color.white;
            answers[i].GetComponent<Button>().interactable = true;
        }
        selectedAnswer = 0;
        LoadQuestion(++questionIndex);
        toggleNextQuestion();
    }
}
