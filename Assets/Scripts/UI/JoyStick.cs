using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class JoyStick : ScrollRect {
  public float mRadius = 0f;
  public float xJoy = 0f;
  public float yJoy = 0f;

  protected override void Start() {
    base.Start();
    mRadius = (transform as RectTransform).sizeDelta.x * 0.45f;
  }

  public override void OnDrag(PointerEventData eventData) {
    base.OnDrag(eventData);
    content.anchoredPosition *= 2.5f;
    var contentPosition = this.content.anchoredPosition;
    if (contentPosition.magnitude > mRadius) {
      contentPosition = contentPosition.normalized * mRadius;
      SetContentAnchoredPosition(contentPosition);
    }
    xJoy = contentPosition.x / mRadius;
    yJoy = contentPosition.y / mRadius;
  }
  
  public override void OnEndDrag(PointerEventData eventData) {
    base.OnEndDrag(eventData);
    xJoy = 0;
    yJoy = 0;
  }
}
