using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace MinigamePickCorrect
{
    public class GridSpawner : ControllerBase
    {
        public GridSpawner(GameObject pickObjectPrefab, GameObject framePrefab, MinigamePickCorrect parent) : base(parent)
        {
            pickObjects = new List<GameObject>();
            frameObjects = new List<GameObject>();
            usedElements = new List<LevelElement>();

            this.pickObjectPrefab = pickObjectPrefab;
            this.framePrefab = framePrefab;
        }
        public override void Init()
        {
            this.parentTransform = parent.GetComponent<RectTransform>();
            this.gridLayoutGroup = parent.GetComponent<GridLayoutGroup>();
            originalFrameSize = framePrefab.GetComponent<SpriteRenderer>().size;
        }

        // Листы
        private readonly List<GameObject> pickObjects;
        private readonly List<GameObject> frameObjects;
        private readonly List<LevelElement> usedElements;

        // Префабы
        private readonly GameObject pickObjectPrefab;
        private readonly GameObject framePrefab;

        // Рантайм поля
        private float frameSize;
        private LevelElement currentCorrectElement;

        // Референс компонентов
        private RectTransform parentTransform;
        private GridLayoutGroup gridLayoutGroup;

        // Изначальный размер рамки
        private Vector2 originalFrameSize;

        public void Deploy(int size, LevelBundle bundle)
        {
            RegenerateFrameGrid(size);
            DestroyPickObjects();
            SpawnPickObjects(size, bundle);
        }
        private void DestroyPickObjects()
        {
            foreach (GameObject pickObject in pickObjects)
            {
                GameObject.Destroy(pickObject);
            }
            pickObjects.Clear();
        }
        private void DisableOnClick()
        {
            foreach (GameObject pickObject in pickObjects)
            {
                pickObject.GetComponent<Collider2D>().enabled = false;
            }
        }
        public override void NotifyLevelIsFinished()
        {
            DisableOnClick();
        }
        public override void NotifyGameIsOver()
        {
            DisableOnClick();
            usedElements.Clear();
        }
        private void SpawnPickObjects(int size, LevelBundle bundle)
        {
            List<LevelElement> bundleList = bundle.ElementsList;

            // Выбираем и запоминаем правильный случайный элемент
            currentCorrectElement = null;
            {
                int crashSafetyCounter = 0;
                while (currentCorrectElement == null)
                {
                    LevelElement tempElement = ListUtility.RandomElementFromList(bundleList, out int ind);
                    if (!usedElements.Contains(tempElement))
                    {
                        bundleList.RemoveAt(ind);
                        usedElements.Add(tempElement);
                        currentCorrectElement = tempElement;
                        break;
                    }
                    crashSafetyCounter++;
                    if (crashSafetyCounter >= size)
                    {
                        Debug.LogError($"Ran out of level elements. Bundle {bundle.name} is way too small.");
                        break;
                    }
                }
            }
            // Элементов меньше размера сетки, придётся создать повторы
            if (size > bundleList.Count)
            {
                while (bundleList.Count < size)
                {
                    bundleList.Add(bundleList[size - bundleList.Count]);
                }
            }

            // Выбираем случайную рамку для правильного ответа
            GameObject correctFrame = ListUtility.RandomElementFromList(frameObjects);

            foreach (GameObject frame in frameObjects)
            {
                GameObject newPickObject = GameObject.Instantiate(pickObjectPrefab, frame.transform);
                PickItemComponent pickItemScript = newPickObject.GetComponent<PickItemComponent>();

                // Магическое число соотношения размера рамки и её скейла
                float newScale = frameSize * 0.25f * bundle.Scale;
                newPickObject.transform.localScale = new Vector3(newScale, newScale, 1f);

                // Инициализируем наш объект либо как правильный, либо как неправильный
                bool isCorrect = frame == correctFrame;
                LevelElement elementToUse = null;
                if (isCorrect)
                {
                    elementToUse = currentCorrectElement;
                    pickItemScript.Init(true, parent.NotifyLevelIsFinished);
                }
                else
                {
                    elementToUse = ListUtility.RandomElementFromList(bundleList, out int ind);
                    bundleList.RemoveAt(ind);
                    pickItemScript.Init(false);
                }
                pickObjects.Add(newPickObject);

#if UNITY_EDITOR
                newPickObject.name = "Item " + elementToUse.Name;
                frame.name = "Frame " + elementToUse.Name;
#endif
                newPickObject.GetComponent<SpriteRenderer>().sprite = elementToUse.Sprite;
                newPickObject.transform.localRotation = Quaternion.Euler(0, 0, elementToUse.RotationOffset);
            }
        }
        public virtual string GetTaskString()
        {
            return $"Find '{currentCorrectElement.Name}'";
        }
        private void RegenerateFrameGrid(int size)
        {
            int count = frameObjects.Count;
            if (count == size)
            {
                return;
            }
            while (count < size)
            {
                GameObject newFrame = GameObject.Instantiate(framePrefab, parentTransform);
                frameObjects.Add(newFrame);
                count++;
            }
            while (count > size)
            {
                GameObject frameToDestroy = frameObjects.ElementAt(0);
                frameObjects.RemoveAt(0);
                GameObject.Destroy(frameToDestroy);
                count--;
            }

            // Динамическое вычисление размера рамки
            // 4 игнорируется, чтобы 4 рамки уложились в 2 ряда, вместо одного
            if (size % 4 == 0 && size != 4)
            {
                frameSize = parentTransform.rect.width / 4;
            }
            else if (size % 3 == 0)
            {
                frameSize = parentTransform.rect.width / 3;
            }
            else if (size % 2 == 0 && size % 5 != 0)
            {
                frameSize = parentTransform.rect.width / 2;
            }
            // Вычисление для нестандартных исключений (крайне желательно избегать, т.к. сетка не красивая)
            else
            {
                frameSize = Mathf.Sqrt(parentTransform.rect.width * parentTransform.rect.height / size);
                // Магические числа для коррекции размера рамки по размеру игрового окна
                frameSize = Mathf.Clamp(frameSize, originalFrameSize.x * 0.5f, originalFrameSize.x * 2f) / 1.2f;
            }
            //frameSize *= 0.7f;

            // Установка размера для сетки и каждой рамки
            gridLayoutGroup.cellSize = new Vector2(frameSize, frameSize);
            foreach (GameObject newFrame in frameObjects)
            {
                var spriteRenderer = newFrame.GetComponent<SpriteRenderer>();
                spriteRenderer.size = new Vector2(frameSize, frameSize);

                // Магические числа для получения сатурированного, не очень темного цвета
                spriteRenderer.color = UnityEngine.Random.ColorHSV(MIN_HUE, MAX_HUE, MIN_SATURATION, MAX_SATURATION, MIN_VALUE, MAX_VALUE, FIXED_ALPHA, FIXED_ALPHA);
            }
        }
        private const float MIN_HUE = 0f;
        private const float MAX_HUE = 1f;
        private const float MIN_SATURATION = 1f;
        private const float MAX_SATURATION = 1f;
        private const float MIN_VALUE = 0.5f;
        private const float MAX_VALUE = 1f;
        private const float FIXED_ALPHA = 0.25f;
    }
}

