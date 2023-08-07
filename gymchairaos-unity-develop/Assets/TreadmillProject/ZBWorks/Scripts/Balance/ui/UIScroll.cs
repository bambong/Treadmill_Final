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
        [Header("Start 에서 스크롤 즉시시작")]
        [SerializeField] private bool playOnStart;
        [Header("이동 오브젝트 대상")]
        [SerializeField] private RectTransform bodyTarget;
        [Header("스크롤 복제 오브젝트 대상")]
        [SerializeField] private RectTransform copyTarget;
        [Header("스크롤 복제 오브젝트 사이 거리")]
        [SerializeField] private float dist;
        [Header("스크롤 복제 오브젝트 개수")]
        [SerializeField] private int num;
        [Header("스크롤 복제 오브젝트 이동 민감도")]
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
            //최초 위치 지정
            bodyTarget.position = Vector3.zero;

            //스크롤 정보 초기화
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
            ////킨 원소들 인덱스 정렬
            //Comparison<RectTransform> rtfComparision = (x, y) => x.anchoredPosition.x.CompareTo(y.anchoredPosition.x);

            //스크롤 반복문
            while (true)
            {
                movedDist += scrollSpeed * Time.deltaTime;

                bodyTarget.position += scrollDir.normalized * movedDist;

                yield return null;
            }
        }
    }
}