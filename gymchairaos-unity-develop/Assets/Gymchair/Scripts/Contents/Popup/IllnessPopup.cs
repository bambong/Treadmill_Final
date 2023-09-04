using Gymchair.Contents.Popup;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class IllnessPopup : MonoBehaviour
{
    [SerializeField] TMP_Dropdown _dropDownFirst;
    [SerializeField] TMP_Dropdown _dropDownSecond;

    Action _actionNone;
    Action<string> _actionOK;
    Action _actionCancel;

    string _text = "";

    public static IllnessPopup Create(string text, Action actionNone = null, Action<string> actionOK = null, Action actionCancel = null)
    {
        Canvas obj = Resources.Load<Canvas>("Prefabs/IllnessPopup");
        Canvas canvas = Instantiate(obj);
        IllnessPopup script = canvas.GetComponent<IllnessPopup>();
        script._actionNone = actionNone;
        script._actionOK = actionOK;
        script._actionCancel = actionCancel;
        script._text = text;
        return script;
    }

    private void Start()
    {
        initDropDown();
    }

    public void initDropDown()
    {
        int first = 0;
        int second = 0;

        if (_text.Length != 0)
        {
            string[] split = _text.Split('/');

            foreach (var option in _dropDownFirst.options)
            {
                if (option.text == split[0])
                {
                    first = _dropDownFirst.options.IndexOf(option);
                    break;
                }
            }

            initSecondDropDown(first);

            foreach (var option in _dropDownSecond.options)
            {
                if (option.text == split[1])
                {
                    second = _dropDownSecond.options.IndexOf(option);
                    break;
                }
            }

            _dropDownFirst.value = first;
            _dropDownSecond.value = second;
        }
    }

    public void initSecondDropDown(int firstValue)
    {
        _dropDownSecond.ClearOptions();

        List<string> options = new List<string>();

        if (firstValue == 0)
            options.Add("없음");
        else if (firstValue == 1)
        {
            options.Add("심근경색증");
            options.Add("동맥경화증");
            options.Add("심장병");
            options.Add("관상동맥혈전증");
            options.Add("류마티스성 심장질환");
            options.Add("심장발작");
            options.Add("심차단");
            options.Add("동맥류");
            options.Add("협심증");
            options.Add("심장판막증");
            options.Add("관상동맥폐쇄증");
            options.Add("심부전");
            options.Add("심잡음");
            options.Add("심계항진");
            options.Add("빈맥");
            options.Add("서맥");
        }
        else if (firstValue == 2)
        {
            options.Add("방광염");
            options.Add("비뇨장애");
            options.Add("요로결석");
            options.Add("요로감염");
            options.Add("요실금");
            options.Add("신경인성방광");
            options.Add("방광 (요관 역류)");
            options.Add("신 수종");
            options.Add("소변 내 비정상 단백질 수준");
            options.Add("배뇨 횟수, 소변색, 소변 형태 변화, 배뇨 어려움 등");
            options.Add("낮은 수준 사구체 여과율");
        }
        else if (firstValue == 3)
        {
            options.Add("압박궤양");
            options.Add("욕창");
            options.Add("근위축증");
            options.Add("관절 종창");
            options.Add("요통");
            options.Add("만성 가려움");
            options.Add("발목부종");
            options.Add("절단술");
            options.Add("인공관절");
            options.Add("구강 내 금속 맛");
            options.Add("관절염");
            options.Add("골절");
            options.Add("골관절 통증");
        }
        else if (firstValue == 4)
        {
            options.Add("고혈압");
            options.Add("고콜레스테롤혈증");
            options.Add("가슴통증");
            options.Add("허혈");
            options.Add("향정신성 처방약 복용");
            options.Add("천식");
            options.Add("기관지염");
            options.Add("폐기종");
            options.Add("야간호흡곤란");
            options.Add("각혈");
            options.Add("뇌졸중");
            options.Add("간경화");
            options.Add("운동 유발성 천식");
            options.Add("당뇨병");
            options.Add("비만");
            options.Add("포도당 불내성");
            options.Add("갑상선질환");
            options.Add("투석");
            options.Add("McArble's 증후군");
            options.Add("저혈당증");
            options.Add("빈혈(피곤, 오한, 호흡곤란, 현기증)");
        }
        _dropDownSecond.AddOptions(options);
        _dropDownSecond.value = 0;
    }

    public void OnChangeFirstValue(TMP_Dropdown dropdown)
    {
        _dropDownSecond.ClearOptions();

        List<string> options = new List<string>();


        if (dropdown.value == 0)
            options.Add("없음");
        else if (dropdown.value == 1)
        {
            options.Add("심근경색증");
            options.Add("동맥경화증");
            options.Add("심장병");
            options.Add("관상동맥혈전증");
            options.Add("류마티스성 심장질환");
            options.Add("심장발작");
            options.Add("심차단");
            options.Add("동맥류");
            options.Add("협심증");
            options.Add("심장판막증");
            options.Add("관상동맥폐쇄증");
            options.Add("심부전");
            options.Add("심잡음");
            options.Add("심계항진");
            options.Add("빈맥");
            options.Add("서맥");
        }
        else if (dropdown.value == 2)
        {
            options.Add("방광염");
            options.Add("비뇨장애");
            options.Add("요로결석");
            options.Add("요로감염");
            options.Add("요실금");
            options.Add("신경인성방광");
            options.Add("방광 (요관 역류)");
            options.Add("신 수종");
            options.Add("소변 내 비정상 단백질 수준");
            options.Add("배뇨 횟수, 소변색, 소변 형태 변화, 배뇨 어려움 등");
            options.Add("낮은 수준 사구체 여과율");
        }
        else if (dropdown.value == 3)
        {
            options.Add("압박궤양");
            options.Add("욕창");
            options.Add("근위축증");
            options.Add("관절 종창");
            options.Add("요통");
            options.Add("만성 가려움");
            options.Add("발목부종");
            options.Add("절단술");
            options.Add("인공관절");
            options.Add("구강 내 금속 맛");
            options.Add("관절염");
            options.Add("골절");
            options.Add("골관절 통증");
        }
        else if (dropdown.value == 4)
        {
            options.Add("고혈압");
            options.Add("고콜레스테롤혈증");
            options.Add("가슴통증");
            options.Add("허혈");
            options.Add("향정신성 처방약 복용");
            options.Add("천식");
            options.Add("기관지염");
            options.Add("폐기종");
            options.Add("야간호흡곤란");
            options.Add("각혈");
            options.Add("뇌졸중");
            options.Add("간경화");
            options.Add("운동 유발성 천식");
            options.Add("당뇨병");
            options.Add("비만");
            options.Add("포도당 불내성");
            options.Add("갑상선질환");
            options.Add("투석");
            options.Add("McArble's 증후군");
            options.Add("저혈당증");
            options.Add("빈혈(피곤, 오한, 호흡곤란, 현기증)");
        }
        _dropDownSecond.AddOptions(options);
        _dropDownSecond.value = 0;
    }

    public void OnNoneButton()
    {
        Managers.Sound.PlayTouchEffect();

        this._actionNone?.Invoke();
        Destroy(gameObject);
    }

    public void OnOkButton()
    {
        Managers.Sound.PlayTouchEffect();

        string info = "없음";

        if (_dropDownFirst.value != 0)
        {
            info = _dropDownFirst.options[_dropDownFirst.value].text + "/" + _dropDownSecond.options[_dropDownSecond.value].text;
        }

        this._actionOK?.Invoke(info);
        Destroy(gameObject);
    }

    public void OnCancelButton()
    {
        Managers.Sound.PlayTouchEffect();

        this._actionCancel?.Invoke();
        Destroy(gameObject);
    }
}
