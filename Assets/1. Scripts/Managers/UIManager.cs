using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    #region ΩÃ±€≈Ê
    static UIManager instance;
    public static UIManager UI { get { return instance; } set { instance = value; } }
    private void Awake()
    {
        if (instance == null)
            instance = this;
        
        else
            Destroy(gameObject);
    }
    #endregion

    public ResultPanel ResultPanel {  get; set; }
    public OptionPanel OptionPanel {  get; set; }
}