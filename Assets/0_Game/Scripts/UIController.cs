using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIController : MonoBehaviour
{
    public static UIController ins;

    private void Awake()
    {
        ins = this;
    }

    public TMP_Text overHeatedMessage;
}
