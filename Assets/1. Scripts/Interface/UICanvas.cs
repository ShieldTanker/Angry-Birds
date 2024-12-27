using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UICanvas : MonoBehaviour
{
    void Start()
    {
        UIManager.UI.Canvas = this.GetComponent<Canvas>();
    }
}
