using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    #region 싱글톤
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

    public Canvas Canvas { get; set; }
    public ResultPanel ResultPanel;
    public OptionPanel OptionPanel {  get; set; }
}