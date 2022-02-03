using System.Linq;
using UnityEngine;


namespace MinigamePickCorrect
{
    public class MinigamePickCorrect : MonoBehaviour
    {
        [Tooltip("Префаб рамки. Должен иметь компоненты 'SpriteRenderer' и 'LayoutElement'.")]
        [SerializeField] private GameObject framePrefab;

        [Tooltip("Префаб объекта для клика TODO")]
        [SerializeField] private GameObject pickObjectPrefab;

        [Tooltip("Уровни, которые будут использоваться в игре по порядку.")]
        [SerializeField] private Level[] levels;

        [SerializeField] private UIGroup uiGroup;

        [Tooltip("Если истина, то будут использоваться случайные бандлы визуализации (а бандлы установленные в уровне, игнорироваться).")]
        [SerializeField] private bool randomBundle;

        [Tooltip("Бандлы для случайного выбора визуализации.")]
        [SerializeField] private LevelBundle[] bundles;

        private int currentLevelIndex;
        private GridSpawner spawner;
        private UIController uiController;


        private void Awake()
        {
            currentLevelIndex = -1;
            spawner = new GridSpawner(pickObjectPrefab, framePrefab, this);
            uiController = new UIController(uiGroup, this);
        }
        void Start()
        {
            uiController.Init();
            spawner.Init();

            StartNextLevel();
        }

        public void NotifyLevelIsFinished()
        {
            if (currentLevelIndex + 1 >= levels.Length)
            {
                currentLevelIndex = -1;
                uiController.NotifyGameIsOver();
                spawner.NotifyGameIsOver();
            }
            else
            {
                uiController.NotifyLevelIsFinished();
                spawner.NotifyLevelIsFinished();
            }

        }
        public void StartNextLevel()
        {
            currentLevelIndex++;
            LoadLevel(currentLevelIndex);
        }
        public void LoadLevel(int ind)
        {
            Level level = levels[ind];
            LevelBundle bundleToUse = randomBundle ? ListUtility.RandomElementFromList(bundles.ToList()) : levels[ind].bundle;
            spawner.Deploy(level.size, bundleToUse);
            uiController.UpdateTaskText(spawner.GetTaskString());
        }
    }
}

