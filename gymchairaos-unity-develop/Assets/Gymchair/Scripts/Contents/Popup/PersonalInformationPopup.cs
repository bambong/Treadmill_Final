using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using static UnityEngine.GraphicsBuffer;

namespace Gymchair.Contents.Popup
{
    public class PersonalInformationPopup : MonoBehaviour
    {
        event Action<float, float, float> _actionButton;
        event Action _actionCancel;

        [SerializeField] ScrollRect _scrollRectCM;
        [SerializeField] GameObject _scrollContentCM;
        [SerializeField] ScrollRect _scrollRectKG;
        [SerializeField] GameObject _scrollContentKG;

        [SerializeField] Text _textBmi;

        [SerializeField] int _startCM = 498;
        [SerializeField] int _endCM = 3002;

        [SerializeField] int _startKG = 8;
        [SerializeField] int _endKG = 2002;

        int _cm = -1;
        float _cm_time = -1.0f;

        int _kg = -1;
        float _kg_time = -1.0f;

        List<GameObject> _objCMs = new List<GameObject>();
        List<GameObject> _objKGs = new List<GameObject>();

        public static PersonalInformationPopup Create(int cm, int kg, Action<float, float, float> actionOkButton = null, Action actionCancel = null)
        {
            Canvas obj = Resources.Load<Canvas>("Prefabs/PersonalInformationPopup");
            Canvas canvas = Instantiate(obj);
            PersonalInformationPopup script = canvas.GetComponent<PersonalInformationPopup>();
            script._actionButton = actionOkButton;
            script._actionCancel = actionCancel;

            script.initCmAndKg(cm, kg);
            return script;
        }

        public void onOkButtonHide()
        {
            Managers.Sound.PlayTouchEffect();
            int cm = getCMNumber();
            int kg = getKGNumber();
            _actionButton?.Invoke(cm, kg, float.Parse(_textBmi.text));
            Destroy(this.gameObject);
        }

        public void OnCancelButtonHide()
        {
            Managers.Sound.PlayTouchEffect();
            _actionCancel?.Invoke();
            Destroy(this.gameObject);
        }

        private void Awake()
        {
            initCM();
            initKG();
        }

        private void initCmAndKg(int cm, int kg)
        {
            StartCoroutine(setCMTargetName(cm));
            StartCoroutine(setKGTargetName(kg));
        }
        
        void initCM()
        {
            int iCmCount = _endCM - _startCM + 1;
            float fHeight = 114.912f * iCmCount;

            for (int iNum = 0; iNum < 9; iNum++)
            {
                GameObject obj = new GameObject($"CM");
                obj.AddComponent<CanvasRenderer>();
                Text tx = obj.AddComponent<Text>();
                tx.font = Resources.Load<Font>("Font/NanumBarunGothicBold");
                tx.fontSize = 60;

                tx.alignment = TextAnchor.MiddleCenter;
                tx.color = Color.black;

                var rectObj = obj.GetComponent<RectTransform>();
                rectObj.sizeDelta = new Vector2(305.0f, 114.912f);
                obj.transform.parent = _scrollContentCM.transform;
                _objCMs.Add(obj);
            }

            var rectTransform = _scrollContentCM.GetComponent<RectTransform>();
            rectTransform.sizeDelta = new Vector2(0.0f, fHeight);
        }

        void initKG()
        {
            int iCmCount = _endKG - _startKG + 1;
            float fHeight = 114.912f * iCmCount;

            for (int iNum = 0; iNum < 9; iNum++)
            {
                GameObject obj = new GameObject($"KG");
                obj.AddComponent<CanvasRenderer>();
                Text tx = obj.AddComponent<Text>();
                tx.font = Resources.Load<Font>("Font/NanumBarunGothicBold");
                tx.fontSize = 60;

                tx.alignment = TextAnchor.MiddleCenter;
                tx.color = Color.black;

                var rectObj = obj.GetComponent<RectTransform>();
                rectObj.sizeDelta = new Vector2(305.0f, 114.912f);
                obj.transform.parent = _scrollContentKG.transform;
                _objKGs.Add(obj);
            }

            var rectTransform = _scrollContentKG.GetComponent<RectTransform>();
            rectTransform.sizeDelta = new Vector2(0.0f, fHeight);
        }

        IEnumerator setCMTargetName(int cm)
        {
            yield return new WaitForEndOfFrame();
            setCMNumber(cm);
        }
        
        public void setCMNumber(int cm)
        {
            var pos = 1f - (((cm - (_startCM + 2)) * 114.912f) / (114.912f * (_endCM - _startCM - 4)));
            _scrollRectCM.verticalNormalizedPosition = pos;
        }

        public int getCMNumber()
        {
            var pos = ((1f - _scrollRectCM.verticalNormalizedPosition) * (114.912f * (_endCM - _startCM - 4)));
            int posY = (int)Math.Round(pos / 114.912f);
            return posY + 2 + _startCM;
        }

        IEnumerator setKGTargetName(int kg)
        {
            yield return new WaitForEndOfFrame();
            setKGNumber(kg);
        }

        public void setKGNumber(int kg)
        {
            var pos = 1f - (((kg - (_startKG + 2)) * 114.912f) / (114.912f * (_endKG - _startKG - 4)));
            _scrollRectKG.verticalNormalizedPosition = pos;
        }

        public int getKGNumber()
        {
            var pos = ((1f - _scrollRectKG.verticalNormalizedPosition) * (114.912f * (_endKG - _startKG - 4)));
            int posY = (int)Math.Round(pos / 114.912f);
            return posY + 2 + _startKG;
        }

        public void OnChangeCMValue()
        {
            int cm = getCMNumber();

            if (_cm != cm)
            {
                _cm_time = Time.time;
                _cm = cm;

                OnSetBmi();
            }

            int iCmCount = _endCM - _startCM;
            float fHeight = 114.912f * iCmCount + 1;

            float fStartHeight = ((114.912f * (cm - 2)) - (114.912f * _startCM) - (fHeight * 0.5f)) * -1.0f;

            int iCount = _objCMs.Count;

            for (int iNum = 0; iNum < iCount; iNum++)
            {
                var objCm = _objCMs[iNum];
                objCm.GetComponent<RectTransform>().anchoredPosition = new Vector3(0.0f, fStartHeight - (114.912f * iNum), 0.0f);
                objCm.GetComponent<Text>().text = string.Format("{0:0.0}", (float)(cm  - 2 + iNum) / 10.0f);
            }
        }

        public void OnChangeKGValue()
        {
            int kg = getKGNumber();

            if (_kg != kg)
            {
                _kg_time = Time.time;
                _kg = kg;

                OnSetBmi();
            }

            int iCmCount = _endKG - _startKG;
            float fHeight = 114.912f * iCmCount + 1;

            float fStartHeight = ((114.912f * (kg - 2)) - (114.912f * _startKG) - (fHeight * 0.5f)) * -1.0f;

            int iCount = _objKGs.Count;

            for (int iNum = 0; iNum < iCount; iNum++)
            {
                var objKg = _objKGs[iNum];
                objKg.GetComponent<RectTransform>().anchoredPosition = new Vector3(0.0f, fStartHeight - (114.912f * iNum), 0.0f);
                objKg.GetComponent<Text>().text = string.Format("{0:0.0}", (float)(kg - 2 + iNum) / 10.0f);
            }
        }

        public void OnSetBmi()
        {
            if (_textBmi.text.Length != 0)
                Managers.Sound.PlayTouchEffect();

            int cm = getCMNumber();
            int kg = getKGNumber();

            float fCm = cm / 10.0f;
            float fKg = kg / 10.0f;

            float m = fCm / 100.0f;
            m *= m;

            float bmi = fKg / m;

            _textBmi.text = string.Format("{0:0.00}", bmi);
        }

        private void Update()
        {
            if (_cm_time != -1.0f)
            {
                if (Time.time - _cm_time > 0.5f)
                {
                    setCMNumber(getCMNumber());
                    _cm_time = -1.0f;
                    _cm = -1;
                }
            }
            
            if (_kg_time != -1.0f)
            {
                if (Time.time - _kg_time > 0.5f)
                {
                    setKGNumber(getKGNumber());
                    _kg_time = -1.0f;
                    _kg = -1;
                }
            }
        }
    }
}

