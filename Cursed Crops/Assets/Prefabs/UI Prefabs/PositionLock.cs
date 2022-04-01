using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// for the Ammo Counter Prefab
public class PositionLock : MonoBehaviour
{
    // Start is called before the first frame update
    public Transform Image;
    public Transform FillArea;

    // LateUpdates updates after most other update processes have executed;
    // needed in this case b/c this is actively working against the natural
    // setting of childs position to parents position
    private void LateUpdate()
    {
        Image.position = FillArea.position;
    }
}
