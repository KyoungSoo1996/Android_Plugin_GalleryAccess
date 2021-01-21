using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections;
public class DragImage : EventTrigger
{
    private bool startDragging;
    float[] posGroup = { 0, 0 };

    private float startX, startY;
    public override void OnDrag(PointerEventData eventData)
    {
        GetComponent<Button>().interactable = false;
        posGroup = gameObject.GetComponentInParent<GetProfileImg>().MoveImage(Input.mousePosition.x - startX, Input.mousePosition.y - startY);
    }

    public override void OnPointerDown(PointerEventData eventData)
    {
        startDragging = true;
        startX = Input.mousePosition.x;
        startY = Input.mousePosition.y;
    }

    public override void OnPointerUp(PointerEventData eventData)
    {
        startDragging = false;
        StartCoroutine(CoStartButton());
    }

    private IEnumerator CoStartButton()
    {
        yield return new WaitForSeconds(0.3f);
        GetComponent<Button>().interactable = true;
        yield break;
    }

    public void OnClickSave()
    {
        StartCoroutine(gameObject.GetComponentInParent<GetProfileImg>().CoSaveImg(GetComponentInParent<GetProfileImg>().profileImg.texture, posGroup[0] , posGroup[1]));
    }
}