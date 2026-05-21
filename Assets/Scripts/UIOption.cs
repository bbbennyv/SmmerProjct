using System;
using UnityEngine;

public class UIOption : MonoBehaviour
{
    public Action OnConfirm;
    public Action OnCancel;

    [SerializeField] private GameObject highlightIndicator;

    public void SetHighlighted(bool highlighted)
    {
        highlightIndicator.SetActive(highlighted);
    }
}