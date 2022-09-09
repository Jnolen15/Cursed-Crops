using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class GoBack : MonoBehaviour, ICancelHandler
{
    public Menu_Manager mm;

    public void OnCancel(BaseEventData eventData)
    {
        mm.GoBack();
    }
}
