using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonHoverEffect : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public RectTransform target;
    public float scaleAmount = 0.98f;  
    public float duration = 0.1f;
    private float prevScaleAmount;
    private void Awake()
    {
        if (target == null)
            target = GetComponent<RectTransform>();

    }


    private void Update()
    {
        if (scaleAmount != prevScaleAmount)
        {
            prevScaleAmount = scaleAmount;
            LeanTween.cancel(target);
            target.localScale = Vector3.one;
        }
    }




    public void OnPointerEnter(PointerEventData eventData)
    {
        LeanTween.cancel(target);
        LeanTween.scale(target, Vector3.one * scaleAmount, duration).setEaseOutQuad();
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        LeanTween.cancel(target);
        LeanTween.scale(target, Vector3.one, duration).setEaseOutQuad();
    }
}
