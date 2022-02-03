using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MinigamePickCorrect
{
    [Serializable]
    public struct AnimationData
    {
        [Tooltip("Множитель скейла до которого объект вырастет во время Bounce эффекта.")]
        public float scale;
        [Tooltip("Длительность роста скейла до значения.")]
        public float firstDuration;
        [Tooltip("Длительность возврата скейла до нормального значения.")]
        public float secondDuration;
    }
    public class PickItemScript : MonoBehaviour
    {
        [Tooltip("Свойства анимации Bounce.")]
        [SerializeField] AnimationData animationData;
        [SerializeField] GameObject particlesPrefab;

        private PickItemBase behaviourHandler;
        public void Init(bool isCorrect, System.Action onClickCallback = null)
        {
            if (isCorrect)
            {
                behaviourHandler = new CorrectItem(transform, animationData, onClickCallback, particlesPrefab);
            }
            else
            {
                behaviourHandler = new WrongItem(transform, animationData);
            }
            behaviourHandler.DoBounce();
        }
        private void OnMouseDown()
        {
            behaviourHandler.OnClick();
        }
        private void OnDestroy()
        {
            behaviourHandler?.KillCurrentTween();
        }
    }
}
