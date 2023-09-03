using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using ZB.Sort;

namespace ZB.UI
{
    public class UIScroll : MonoBehaviour
    {
        public bool IsPlaying { get => isPlaying; }
        public Vector3 scrollDir;
        public float scrollSpeed;

        [Space(20)]
        [Header("Start ���� ��ũ�� ��ý���")]
        [SerializeField] private bool playOnStart;
        [Header("�̵� ������Ʈ ���")]
        [SerializeField] private RectTransform bodyTarget;
        [Header("��ũ�� ���� ������Ʈ ���")]
        [SerializeField] private RectTransform copyTarget;
        [Header("��ũ�� ���� ������Ʈ ���� �Ÿ�")]
        [SerializeField] private float dist;
        [Header("��ũ�� ���� ������Ʈ ����")]
        [SerializeField] private int num;
        [Header("��ũ�� ���� ������Ʈ �̵� �ΰ���")]
        [SerializeField] private float moveMinValue;

        private bool isPlaying;
        private RectTransform[] rtf_copied;

        public void Play()
        {
            Play(this.scrollDir, this.scrollSpeed);
        }
        public void Play(Vector2 scrollDir, float scrollSpeed)
        {
            isPlaying = true;

            this.scrollDir = scrollDir;
            this.scrollSpeed = scrollSpeed;

            if (isPlaying)
                Stop();
            ScrollCycle_C = ScrollCycle();
            StartCoroutine(ScrollCycle_C);
        }

        public void Stop()
        {
            isPlaying = false;

            if (ScrollCycle_C != null)
                StopCoroutine(ScrollCycle_C);
        }

        private void Start()
        {
            rtf_copied = new RectTransform[num];
            RectTransform temp;
            for (int i = 0; i < num; i++)
            {
                temp = Instantiate(copyTarget);
                temp.transform.SetParent(bodyTarget.transform);
                rtf_copied[i] = temp;
            }
            copyTarget.gameObject.SetActive(false);

            if (playOnStart)
            {
                Play();
            }
        }
        IEnumerator ScrollCycle_C;
        IEnumerator ScrollCycle()
        {
            //���� ��ġ ����
            bodyTarget.position = Vector3.zero;

            //��ũ�� ���� �ʱ�ȭ
            float movedDist = 0;

            //Vector2[] targetPositions = new Vector2[rtf_copied.Length];
            //for (int i = 0; i < targetPositions.Length; i++)
            //    targetPositions[i] = rtf_copied[i].position;
            //Vector2[] sortedPositions = SortUtility.SortByIntervals(targetPositions, Vector2.zero, scrollDir, dist);
            //for (int i = 0; i < targetPositions.Length; i++)
            //{
            //    rtf_copied[i].position = sortedPositions[i];
            //    Debug.LogError($"{i} / {sortedPositions[i]}");
            //}
            ////Ų ���ҵ� �ε��� ����
            //Comparison<RectTransform> rtfComparision = (x, y) => x.anchoredPosition.x.CompareTo(y.anchoredPosition.x);

            //��ũ�� �ݺ���
            while (true)
            {
                movedDist += scrollSpeed * Time.deltaTime;

                bodyTarget.position += scrollDir.normalized * movedDist;

                yield return null;
            }
        }
    }
}