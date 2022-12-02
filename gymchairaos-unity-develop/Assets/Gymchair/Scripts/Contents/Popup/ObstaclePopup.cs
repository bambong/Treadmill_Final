using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ObstaclePopup : MonoBehaviour
{
    [Header("비장애")]
    [SerializeField] Sprite _able_bodied_button_normal;
    [SerializeField] Sprite _able_bodied_button_touch;

    [Header("장애")]
    [SerializeField] Sprite _disabled_button_normal;
    [SerializeField] Sprite _disabled_button_touch;

    [Header("지체 장애")]
    [SerializeField] Sprite _physical_disability_button_normal;
    [SerializeField] Sprite _physical_disability_button_touch;

    [Header("뇌변병 장애")]
    [SerializeField] Sprite _brain_disease_disorder_button_normal;
    [SerializeField] Sprite _brain_disease_disorder_button_touch;

    [Header("기타")]
    [SerializeField] Sprite _ect_button_normal;
    [SerializeField] Sprite _ect_button_touch;

    [Header("절단 장애")]
    [SerializeField] Sprite _spinal_cord_disorder_button_normal;
    [SerializeField] Sprite _spinal_cord_disorder_button_touch;

    [Header("척수 장애")]
    [SerializeField] Sprite _amputation_button_normal;
    [SerializeField] Sprite _amputation_button_touch;

    [Header("지체기능장애")]
    [SerializeField] Sprite _physical_dysfunction_button_normal;
    [SerializeField] Sprite _physical_dysfunction_button_touch;

    [Header("판넬")]
    [SerializeField] GameObject _panelFirst;
    [SerializeField] GameObject _panelNone;
    [SerializeField] GameObject _panelSecond;
    [SerializeField] GameObject _panelThird;
    [SerializeField] GameObject _panelFourth;

    [Header("First")]
    [SerializeField] GameObject _obj_able_bodied_button;
    [SerializeField] GameObject _obj_disabled_button;

    [Header("None")]
    [SerializeField] InputField _inputWhy;

    [Header("Second")]
    [SerializeField] GameObject _obj_physical_disability_button;
    [SerializeField] GameObject _obj_brain_disease_disorder_button;
    [SerializeField] GameObject _obj_ect_button;

    [Header("Third")]
    [SerializeField] GameObject _obj_spinal_cord_disorder_button;
    [SerializeField] GameObject _obj_amputation_button;
    [SerializeField] GameObject _obj_physical_dysfunction_button;
    [SerializeField] GameObject _obj_third_ect_button;

    [Header("Fourth")]
    [SerializeField] TMP_Dropdown _dropDetail;
    [SerializeField] TMP_Dropdown _dropNumber;
    [SerializeField] TMP_Dropdown _dropInfo;


    Action<int, string> _actionOK;
    Action _actionCancel;

    public static ObstaclePopup Create(Action<int, string> actionOK = null, Action actionCancel = null)
    {
        Canvas obj = Resources.Load<Canvas>("Prefabs/ObstaclePopup");
        Canvas canvas = Instantiate(obj);
        ObstaclePopup script = canvas.GetComponent<ObstaclePopup>();
        script._actionOK = actionOK;
        script._actionCancel = actionCancel;
        return script;
    }

    private void Start()
    {
        _panelFirst.SetActive(true);
        _panelNone.SetActive(false);
        _panelSecond.SetActive(false);
        _panelThird.SetActive(false);
        _panelFourth.SetActive(false);
    }

    public void OnFirstButton(GameObject obj)
    {
        Gymchair.Core.Mgr.SoundMgr.Instance.PlayEffect("touch");
        if (obj == _obj_able_bodied_button)
        {
            _obj_able_bodied_button.GetComponent<Image>().sprite = _able_bodied_button_touch;
            _obj_disabled_button.GetComponent<Image>().sprite = _disabled_button_normal;
        }
        else
        {
            _obj_able_bodied_button.GetComponent<Image>().sprite = _able_bodied_button_normal;
            _obj_disabled_button.GetComponent<Image>().sprite = _disabled_button_touch;
        }
    }

    public void OnFirstOkButton()
    {
        Gymchair.Core.Mgr.SoundMgr.Instance.PlayEffect("touch");
        if (_obj_able_bodied_button.GetComponent<Image>().sprite == _able_bodied_button_touch)
        {
            _panelNone.SetActive(true);
            _panelFirst.SetActive(false);
        }
        else
        {
            _panelSecond.SetActive(true);
            _panelFirst.SetActive(false);
        }
    }

    public void OnNoneOkButton()
    {
        Gymchair.Core.Mgr.SoundMgr.Instance.PlayEffect("touch");
        _actionOK?.Invoke(0, _inputWhy.text);
        Destroy(gameObject);
    }
    public void OnCancelButton()
    {
        Gymchair.Core.Mgr.SoundMgr.Instance.PlayEffect("touch");
        _actionCancel?.Invoke();
        Destroy(gameObject);
    }

    public void OnSecondButton(GameObject obj)
    {
        Gymchair.Core.Mgr.SoundMgr.Instance.PlayEffect("touch");
        if (obj == _obj_physical_disability_button)
        {
            _obj_physical_disability_button.GetComponent<Image>().sprite = _physical_disability_button_touch;
            _obj_brain_disease_disorder_button.GetComponent<Image>().sprite = _brain_disease_disorder_button_normal;
            _obj_ect_button.GetComponent<Image>().sprite = _ect_button_normal;
        }
        else if (obj == _obj_brain_disease_disorder_button)
        {
            _obj_physical_disability_button.GetComponent<Image>().sprite = _physical_disability_button_normal;
            _obj_brain_disease_disorder_button.GetComponent<Image>().sprite = _brain_disease_disorder_button_touch;
            _obj_ect_button.GetComponent<Image>().sprite = _ect_button_normal;
        }
        else
        {
            _obj_physical_disability_button.GetComponent<Image>().sprite = _physical_disability_button_normal;
            _obj_brain_disease_disorder_button.GetComponent<Image>().sprite = _brain_disease_disorder_button_normal;
            _obj_ect_button.GetComponent<Image>().sprite = _ect_button_touch;
        }

    }

    public void OnSecondOkButton()
    {
        Gymchair.Core.Mgr.SoundMgr.Instance.PlayEffect("touch");
        if (_obj_physical_disability_button.GetComponent<Image>().sprite == _physical_disability_button_touch)
        {
            _panelThird.SetActive(true);
            _panelSecond.SetActive(false);
        }
        else if (_obj_brain_disease_disorder_button.GetComponent<Image>().sprite == _brain_disease_disorder_button_touch)
        {
            _actionOK?.Invoke(1, "뇌병변 장애");
            Destroy(gameObject);
        }
        else if (_obj_ect_button.GetComponent<Image>().sprite == _ect_button_touch)
        {
            _actionOK?.Invoke(1, "기타");
            Destroy(gameObject);
        }
    }

    public void OnThirdButton(GameObject obj)
    {
        Gymchair.Core.Mgr.SoundMgr.Instance.PlayEffect("touch");
        if (obj == _obj_spinal_cord_disorder_button)
        {
            _obj_spinal_cord_disorder_button.GetComponent<Image>().sprite = _spinal_cord_disorder_button_touch;
            _obj_amputation_button.GetComponent<Image>().sprite = _amputation_button_normal;
            _obj_physical_dysfunction_button.GetComponent<Image>().sprite = _physical_dysfunction_button_normal;
            _obj_third_ect_button.GetComponent<Image>().sprite = _ect_button_normal;
        }
        else if (obj == _obj_amputation_button)
        {
            _obj_spinal_cord_disorder_button.GetComponent<Image>().sprite = _spinal_cord_disorder_button_normal;
            _obj_amputation_button.GetComponent<Image>().sprite = _amputation_button_touch;
            _obj_physical_dysfunction_button.GetComponent<Image>().sprite = _physical_dysfunction_button_normal;
            _obj_third_ect_button.GetComponent<Image>().sprite = _ect_button_normal;
        }
        else if (obj == _obj_physical_dysfunction_button)
        {
            _obj_spinal_cord_disorder_button.GetComponent<Image>().sprite = _spinal_cord_disorder_button_normal;
            _obj_amputation_button.GetComponent<Image>().sprite = _amputation_button_normal;
            _obj_physical_dysfunction_button.GetComponent<Image>().sprite = _physical_dysfunction_button_touch;
            _obj_third_ect_button.GetComponent<Image>().sprite = _ect_button_normal;
        }
        else
        {
            _obj_spinal_cord_disorder_button.GetComponent<Image>().sprite = _spinal_cord_disorder_button_normal;
            _obj_amputation_button.GetComponent<Image>().sprite = _amputation_button_normal;
            _obj_physical_dysfunction_button.GetComponent<Image>().sprite = _physical_dysfunction_button_normal;
            _obj_third_ect_button.GetComponent<Image>().sprite = _ect_button_touch;
        }
    }

    public void OnThirdOkButton()
    {
        Gymchair.Core.Mgr.SoundMgr.Instance.PlayEffect("touch");
        if (_obj_spinal_cord_disorder_button.GetComponent<Image>().sprite == _spinal_cord_disorder_button_touch)
        {
            _actionOK?.Invoke(1, "지체 장애/절단 장애");
            Destroy(gameObject);
        }
        else if (_obj_amputation_button.GetComponent<Image>().sprite == _amputation_button_touch)
        {
            _panelFourth.SetActive(true);
            _panelThird.SetActive(false);
        }
        else if (_obj_physical_dysfunction_button.GetComponent<Image>().sprite == _physical_dysfunction_button_touch)
        {
            _actionOK?.Invoke(1, "지체 장애/지체기능장애");
            Destroy(gameObject);
        }
        else if (_obj_third_ect_button.GetComponent<Image>().sprite == _ect_button_touch)
        {
            _actionOK?.Invoke(1, "지체 장애/기타");
            Destroy(gameObject);
        }
    }

    public void OnFourthOkButton()
    {
        Gymchair.Core.Mgr.SoundMgr.Instance.PlayEffect("touch");
        List<string> texts = new List<string>();
        texts.Add("지체 장애");
        texts.Add("척수 장애");
        texts.Add(_dropDetail.options[_dropDetail.value].text);
        texts.Add(_dropNumber.options[_dropNumber.value].text);
        texts.Add(_dropInfo.options[_dropInfo.value].text);

        var text = string.Join("/", texts);

        _actionOK?.Invoke(1, text);
        Destroy(gameObject);
    }

    public void OnChangeFourthFirstDropdown()
    {
        Gymchair.Core.Mgr.SoundMgr.Instance.PlayEffect("touch");
        _dropNumber.ClearOptions();

        List<string> options = new List<string>();


        int value = _dropDetail.value;


        if (value == 0)
        {
            for (var iNum = 1; iNum < 8; iNum++)
            {
                options.Add(iNum.ToString());
            }
        }
        else if (value == 1)
        {
            for (var iNum = 1; iNum < 16; iNum++)
            {
                options.Add(iNum.ToString());
            }
        }
        else if (value == 2)
        {
            for (var iNum = 1; iNum < 6; iNum++)
            {
                options.Add(iNum.ToString());
            }
        }

        _dropNumber.AddOptions(options);
        _dropNumber.value = 0;
    }
}
