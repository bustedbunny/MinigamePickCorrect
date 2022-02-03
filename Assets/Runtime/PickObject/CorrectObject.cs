using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MinigamePickCorrect
{

    public abstract class PickItemBase
    {
        public PickItemBase(Transform parent, AnimationData animationData)
        {
            this.transform = parent;
            this.animationData = animationData;
        }

        protected Transform transform;
        protected Tweener currentTween;

        private readonly AnimationData animationData;

        public virtual void OnClick()
        {
            CompleteCurrentTween();
        }
        public void CompleteCurrentTween()
        {
            currentTween?.Complete();
        }
        public void KillCurrentTween()
        {
            currentTween?.Kill();
        }
        public void DoBounce()
        {
            CompleteCurrentTween();
            Vector3 originalScale = transform.localScale;
            Vector3 targetScale = new Vector3(originalScale.x * animationData.scale, originalScale.y * animationData.scale, originalScale.z);
            currentTween = transform.DOScale(targetScale, animationData.firstDuration)
                .ChangeStartValue(new Vector3(0.01f, 0.01f, originalScale.z))
                .OnComplete(() =>
            {
                currentTween = transform.DOScale(originalScale, animationData.secondDuration);
            });
        }
    }
    public class CorrectItem : PickItemBase
    {
        private GameObject particlesPrefab;
        private Action onClickCallback;
        public CorrectItem(Transform parent, AnimationData animationData, System.Action onClickCallback, GameObject particlesPrefab = null) : base(parent, animationData)
        {
            this.particlesPrefab = particlesPrefab;
            this.onClickCallback = onClickCallback;
        }

        public void SetParticlesPrefab(GameObject prefab)
        {
            particlesPrefab = prefab;
        }
        public override void OnClick()
        {
            base.OnClick();
            DoBounce();
            GameObject.Instantiate(particlesPrefab, transform);
            onClickCallback?.Invoke();
        }
    }
    public class WrongItem : PickItemBase
    {
        public WrongItem(Transform parent, AnimationData animationData) : base(parent, animationData)
        {
        }

        public override void OnClick()
        {
            base.OnClick();
            currentTween = transform.DOShakePosition(1f, new Vector3(2f, 0.25f, 0f));
        }
    }
}
