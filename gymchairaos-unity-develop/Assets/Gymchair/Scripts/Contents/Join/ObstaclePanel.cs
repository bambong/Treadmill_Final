using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ObstaclePanel : MonoBehaviour
{
    [SerializeField] Button _buttonPanel;
    [SerializeField] Text _textInfo;

    bool _check = false;
    Action<ObstaclePanel> _action = null;

    private void Awake()
    {
        _buttonPanel.onClick.AddListener(() =>
        {
            Gymchair.Core.Mgr.SoundMgr.Instance.PlayEffect("touch");
            _action?.Invoke(this);
        });
    }

    public void SetInfo(string info)
    {
        _textInfo.text = info;
    }

    public string GetInfo()
    {
        return _textInfo.text;
    }

    public void SetCheck(bool check)
    {
        _check = check;
    }

    public bool GetCheck()
    {
        return _check;
    }

    public void SetButtonClick(Action<ObstaclePanel> action)
    {
        _action = action;
    }
}
