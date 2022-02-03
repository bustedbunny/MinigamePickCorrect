using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

namespace MinigamePickCorrect
{
    [CreateAssetMenu(fileName = "New Level Bundle", menuName = "Minigame Pick Correct", order = 10)]
    public class LevelBundle : ScriptableObject
    {
        public LevelElement[] Elements => elements;
        [SerializeField] private LevelElement[] elements;

        public List<LevelElement> ElementsList
        {
            get
            {
                List<LevelElement> list = new List<LevelElement>();
                foreach (var element in elements)
                {
                    list.Add(element);
                }
                return list;
            }
        }
    }

    [Serializable]
    public class LevelElement
    {
        public string Name => name;
        [SerializeField] private string name;
        public Sprite Sprite => sprite;
        [SerializeField] private Sprite sprite;
    }
}

