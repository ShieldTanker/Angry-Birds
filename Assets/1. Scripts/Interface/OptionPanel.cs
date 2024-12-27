using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OptionPanel : MonoBehaviour
{
    public GameObject panel;

    private void Start()
    {
        UIManager.UI.OptionPanel = this;
    }
}
