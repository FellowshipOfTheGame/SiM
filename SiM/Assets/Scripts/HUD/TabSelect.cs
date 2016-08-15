using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;
using System.Collections;

public class TabSelect : MonoBehaviour
{
    private EventSystem eventSys;

    void Start()
    {
        this.eventSys = EventSystem.current;
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Tab))
        {
            Selectable next = null;
            Selectable current = null;

            if(eventSys.currentSelectedGameObject != null)
            {
                if (eventSys.currentSelectedGameObject.activeInHierarchy)
                    current = eventSys.currentSelectedGameObject.GetComponent<Selectable>();
            }

            if(current != null)
            {
                if(Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
                {
                    next = current.FindSelectableOnLeft();
                    if(next == null)
                        next = current.FindSelectableOnUp();
                }
                else
                {
                    next = current.FindSelectableOnRight();
                    if (next == null)
                        next = current.FindSelectableOnDown();
                }
            }
            else
            {
                if (Selectable.allSelectables.Count > 0)
                    next = Selectable.allSelectables[0];
            }

            if (next != null)
                next.Select();
        }
    }
}
