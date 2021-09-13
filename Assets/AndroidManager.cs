using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AndroidManager : MonoBehaviour
{

    [SerializeField] private GameObject _buttons;
    private SelectMode _selectMode = SelectMode.SELECT;
    public SelectMode SelectMode => _selectMode;

    private void Start()
    {
        if (!GameManager.Instance.IsAndroid)
        {
            _buttons.SetActive(false);
            Destroy(this);
        }
    }

    private void ChangeSelectMode(SelectMode selectMode)
    {
        _selectMode = selectMode;
    }

    public void SetModeSelect()
    {
        ChangeSelectMode(SelectMode.SELECT);
    }
    
    public void SetModeAttack()
    {
        ChangeSelectMode(SelectMode.ATTACK);
    }
}

public enum SelectMode
{
    SELECT,
    ATTACK
}