using Gymchair.Contents.Popup;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GymInfoPopup : MonoBehaviour
{
    event Action _actionNone;
    event Action<string> _actionOK;
    event Action _actionCancel;

    List<string> _gymName = new List<string>();

    public static GymInfoPopup Create(string gymName = "", Action actionNone = null, Action<string> actionOK = null, Action actionCancel = null)
    {
        Canvas obj = Resources.Load<Canvas>("Prefabs/GymInfoPopup");
        Canvas canvas = Instantiate(obj);
        GymInfoPopup script = canvas.GetComponent<GymInfoPopup>();
        script._actionNone = actionNone;
        script._actionOK = actionOK;
        script._actionCancel = actionCancel;

        if (gymName.Length != 0)
        {
            string[] gymNames = gymName.Split("/");
            script._gymName = new List<string>(gymNames);
        }

        return script;
    }

    [SerializeField] Sprite _spriteNormal;
    [SerializeField] Sprite _spriteSelect;

    [SerializeField] GameObject[] _objButtons;

    private void Start()
    {
        initButtonList();
    }

    void initButtonList()
    {
        foreach (var gymName in _gymName)
        {
            foreach (var obj in _objButtons)
            {
                var child = obj.transform.GetChild(0);
                var tx = child.GetComponent<Text>();

                if (tx.text == gymName)
                {
                    obj.GetComponent<Image>().sprite = _spriteSelect;
                    break;
                }
            }
        }
    }

    public void OnSelectButton(GameObject obj)
    {
        Managers.Sound.PlayTouchEffect();
        if (obj.GetComponent<Image>().sprite == _spriteNormal)
        {
            obj.GetComponent<Image>().sprite = _spriteSelect;

            var child = obj.transform.GetChild(0);
            var tx = child.GetComponent<Text>();

            _gymName.Add(tx.text);
        }
        else
        {
            obj.GetComponent<Image>().sprite = _spriteNormal;

            var child = obj.transform.GetChild(0);
            var tx = child.GetComponent<Text>();

            _gymName.Remove(tx.text);
        }
    }

    public void OnNoneButton()
    {
        Managers.Sound.PlayTouchEffect();
        _actionNone?.Invoke();
        Destroy(this.gameObject);
    }
    
    public void OnOkButton()
    {
        Managers.Sound.PlayTouchEffect();
        string gymName = "";
        
        if (_gymName.Count != 0)
            gymName = string.Join("/", _gymName.ToArray());

        _actionOK?.Invoke(gymName);
        Destroy(this.gameObject);
    }

    public void OnCancelButton()
    {
        Managers.Sound.PlayTouchEffect();
        _actionCancel?.Invoke();
        Destroy(this.gameObject);
    }
}
