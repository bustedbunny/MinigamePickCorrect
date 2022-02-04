using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace MinigamePickCorrect
{
    [CreateAssetMenu(fileName = "New Level Bundle", menuName = "Minigame Pick Correct", order = 10)]
    public class LevelBundle : ScriptableObject
    {
        public float Scale => scale;
        [Tooltip("Множитель скейла для всех спрайтов бандла.")]
        [SerializeField] private float scale = 1f;
        public LevelElement[] Elements => elements;
        [SerializeField] private LevelElement[] elements;

        public List<LevelElement> ElementsList => elements.ToList();
    }

    [Serializable]
    public class LevelElement
    {
        public string Name => name;
        [Tooltip("Название элемента, которое будет отображаться при постановке задания.")]
        [SerializeField] private string name;
        public Sprite Sprite => sprite;
        [SerializeField] private Sprite sprite;
        public float RotationOffset => rotationOffset;
        [Tooltip("Оффсет для ротации по Z оси (в градусах).")]
        [SerializeField] private float rotationOffset;
    }
}

