using System;
using UnityEngine;

namespace MinigamePickCorrect
{
    [Serializable]
    public class Level
    {
        [Tooltip("Количество объектов в уровне. Предпочтительно использовать значения кратные 3 или 4.")]
        [SerializeField] public int size;

        [Tooltip("Бандл визуализации используемый в уровне. Чтобы объекты не повторялись, количеству элементов в бандле должно быть больше или равно количества объектов в уровне.")]
        [SerializeField] public LevelBundle bundle;
    }
}

