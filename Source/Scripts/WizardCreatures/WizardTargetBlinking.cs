using DG.Tweening;
using UnityEngine;

public class WizardTargetBlinking : MonoBehaviour
{
    [SerializeField] private float animTime;
    [SerializeField] private SpriteRenderer img;

    private Tween blinkTween;

    private void OnEnable()
    {
        HighlightOn();
    }

    private void OnDisable()
    {
        if (blinkTween != null)
            blinkTween.Kill();
    }

    private void HighlightOn()
    {
        blinkTween = img.DOFade(0.6f, animTime);
        blinkTween.onComplete += HighlightOff;
    }
    private void HighlightOff()
    {
        blinkTween = img.DOFade(0.3f, animTime);
        blinkTween.onComplete += HighlightOn;
    }
}