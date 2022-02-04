using DG.Tweening;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace MinigamePickCorrect
{
    [Serializable]
    public struct UIGroup
    {
        [Tooltip("Текстовый объект для генерации текста задания.")]
        [SerializeField] public Text taskText;

        [Tooltip("Кнопка для перехода на следующий уровень.")]
        [SerializeField] public Button startNextLevelButton;

        [Tooltip("Кнопка перезапуска, которая появится после прохождения всех уровней.")]
        [SerializeField] public Button restartButton;

        [Tooltip("Объект который обеспечивает эффект затемнения.")]
        [SerializeField] public CanvasGroup fadeObject;

        [Tooltip("Экран загрузки.")]
        [SerializeField] public CanvasGroup loadingScreen;
    }
    public class UIController : ControllerBase
    {
        private Text taskText;
        private Button startNextLevelButton;
        private Button restartButton;
        private CanvasGroup fadeObject;
        private CanvasGroup loadingScreen;

        public UIController(UIGroup uIGroup, MinigamePickCorrect parent) : base(parent)
        {
            this.taskText = uIGroup.taskText;
            this.startNextLevelButton = uIGroup.startNextLevelButton;
            this.restartButton = uIGroup.restartButton;
            this.fadeObject = uIGroup.fadeObject;
            this.loadingScreen = uIGroup.loadingScreen;
        }
        public override void Init()
        {
            startNextLevelButton.gameObject.SetActive(false);
            startNextLevelButton.GetComponent<Button>().onClick.AddListener(() =>
            {
                startNextLevelButton.gameObject.SetActive(false);
                parent.StartNextLevel();
            });

            restartButton.gameObject.SetActive(false);
            restartButton.GetComponent<Button>().onClick.AddListener(() =>
            {
                restartButton.gameObject.SetActive(false);
                fadeObject.DOFade(0f, 2f);
                loadingScreen.DOFade(1f, 2f).OnComplete(() =>
                {
                    parent.StartNextLevel();
                    loadingScreen.DOFade(0f, 2f);
                });

            });
        }
        public override void NotifyLevelIsFinished()
        {
            startNextLevelButton.gameObject.SetActive(true);
        }
        public override void NotifyGameIsOver()
        {
            fadeObject.DOFade(0.5f, 1f);
            restartButton.gameObject.SetActive(true);
        }
        public void UpdateTaskText(string newText)
        {
            taskText.text = newText;
        }
    }
}

