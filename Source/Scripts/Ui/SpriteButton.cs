using DG.Tweening;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SpriteButton : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private UnityEvent onClick;
    [SerializeField] private Sprite defaultSprite;
    [SerializeField] private Sprite clickedSprite;
    [SerializeField] private float textOffset;
    [SerializeField] private RectTransform buttonTextRect;

    public void OnPointerClick(PointerEventData eventData)
    {
        ButtonAnimation();
        onClick?.Invoke();
    }

    private void ButtonAnimation()
    {
        GetComponent<Image>().sprite = clickedSprite;
        float startTextY = buttonTextRect.localPosition.y;
        buttonTextRect.DOLocalMoveY(buttonTextRect.localPosition.y - textOffset, 0.05f);
        IEnumerator SetButtonSpriteBack() { yield return new WaitForSeconds(0.1f); GetComponent<Image>().sprite = defaultSprite; buttonTextRect.DOLocalMoveY(startTextY, 0.05f); }
        StartCoroutine(SetButtonSpriteBack());
    }
}
