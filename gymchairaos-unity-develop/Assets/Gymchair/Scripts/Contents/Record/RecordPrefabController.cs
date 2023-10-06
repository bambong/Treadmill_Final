using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Gymchair.Contents.Record
{
    public class RecordPrefabController : MonoBehaviour
    {
        public Text _textDate;

        public Text _textTime;
        public Text _textMeter;
        public Text _textHighSpeed;
        public Text _textBPM;
        public Text _textkcal;

        [SerializeField] private Image bg;
        [SerializeField] private Sprite green;
        [SerializeField] private Sprite blue;

        public int _keyNumber;

        public Action<int> _actionClick;

        public void SetBG_Green(bool active)
        {
            bg.sprite = active ? green : blue;
        }

        [SerializeField] Button _buttonClick;

        private void Awake()
        {
            _buttonClick.onClick.AddListener(() => {
                _actionClick?.Invoke(_keyNumber);
            });
        }
    }
}