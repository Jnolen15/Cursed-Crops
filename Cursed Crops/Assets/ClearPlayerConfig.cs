using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClearPlayerConfig : MonoBehaviour
{
    private void Awake()
    {
        // Set Player config to null so its player input isn't counted.
        // (It will be deleted upon entering the Player select)
        GameObject[] objs = GetDontDestroyOnLoadObjects();
        if (objs.Length > 0)
        {
            foreach (GameObject obj in objs)
            {
                obj.SetActive(false);
            }
        }
    }

    public static GameObject[] GetDontDestroyOnLoadObjects()
    {
        GameObject temp = null;
        try
        {
            temp = new GameObject();
            Object.DontDestroyOnLoad(temp);
            UnityEngine.SceneManagement.Scene dontDestroyOnLoad = temp.scene;
            Object.DestroyImmediate(temp);
            temp = null;

            return dontDestroyOnLoad.GetRootGameObjects();
        }
        finally
        {
            if (temp != null)
                Object.DestroyImmediate(temp);
        }
    }

}
