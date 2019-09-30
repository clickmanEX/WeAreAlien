using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

public class VirtualJoystick : MonoBehaviour, IDragHandler, IPointerUpHandler, IPointerDownHandler
{
    public Image VirtualJoystick_BG_Image;
    public Image VirtualJoystick_Image;
    public Vector3 InputDirection { set; get; }
    private float x;
    private float y;

    private void Start()
    {
        //VirtualJoystick_BG_Image = GetComponentsInChildren<Image> ();
        //VirtualJoystick_Image = GetComponentsInChildren<Image> ();
        InputDirection = Vector3.zero;
    }

    public virtual void OnDrag(PointerEventData ped)
    {
        //Debug.Log ("OnDrag");
        Vector2 pos = Vector2.zero;
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(VirtualJoystick_BG_Image.rectTransform, ped.position, ped.pressEventCamera, out pos))
        {
            pos.x = (pos.x / VirtualJoystick_BG_Image.rectTransform.sizeDelta.x);
            pos.y = (pos.y / VirtualJoystick_BG_Image.rectTransform.sizeDelta.y);

            x = (VirtualJoystick_BG_Image.rectTransform.pivot.x == 1) ? x = pos.x * 2 + 1 : x = pos.x * 2 - 1;
            y = (VirtualJoystick_BG_Image.rectTransform.pivot.y == 1) ? y = pos.y * 2 + 1 : y = pos.y * 2 - 1;

            InputDirection = new Vector3(x, 0, y);
            if (InputDirection.magnitude > 1)
            {
                InputDirection = InputDirection.normalized;
            }
            VirtualJoystick_Image.rectTransform.anchoredPosition = new Vector3(InputDirection.x * (VirtualJoystick_BG_Image.rectTransform.sizeDelta.x / 4)
                                                                                , InputDirection.z * (VirtualJoystick_BG_Image.rectTransform.sizeDelta.y / 4));
        }
    }

    public virtual void OnPointerDown(PointerEventData ped)
    {
        OnDrag(ped);
    }

    public virtual void OnPointerUp(PointerEventData ped)
    {
        InputDirection = Vector3.zero;
        VirtualJoystick_Image.rectTransform.anchoredPosition = Vector3.zero;
    }
}
