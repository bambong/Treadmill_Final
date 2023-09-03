using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ZB;

namespace ZB
{
    public class ObjectsScrolling : MonoBehaviour
    {
        public float ScrollSpeed { get { return scrollSpeed; } }

        [Header("라이프사이클 Start에서 바로스크롤시작")]
        [SerializeField] bool scrollOnStart;
        [Header("스크롤 속도")]
        [SerializeField] float scrollSpeed;
        [Header("스크롤 방향")]
        [SerializeField] Vector3 scrollDir;
        [Space]
        [Header("스크롤 대상 정보")]
        [SerializeField] RepeatSet[] repeatSets;
        [Space]
        [Header("Flexible 오브젝트")]
        [SerializeField] List<SingleObstacle> flexibleObjs;
        [SerializeField] Transform flexiblesOriginalParent;

        //스크롤 시작
        [ContextMenu("스크롤시작")]
        public void ScrollStart()
        {
            if(ScrollUpdate_C != null) 
            {
                StopCoroutine(ScrollUpdate_C);
            }
            ScrollUpdate_C = ScrollUpdate();
            StartCoroutine(ScrollUpdate_C);
        }

        //스크롤 멈춤
        [ContextMenu("스크롤멈춤")]
        public void ScrollStop()
        {
            if (ScrollUpdate_C != null)
            {
                StopCoroutine(ScrollUpdate_C);
            }
            if (ScrollSpeedChangeLinearCycle_C != null)
                StopCoroutine(ScrollSpeedChangeLinearCycle_C);
        }

        //스크롤 속도 변경
        public void ScrollSpeedChange(float value)
        {
            scrollSpeed = value;
        }
        public void ScrollSpeedChangeLinear(float value, float duration)
        {
            if (ScrollSpeedChangeLinearCycle_C != null)
                StopCoroutine(ScrollSpeedChangeLinearCycle_C);
            ScrollSpeedChangeLinearCycle_C = ScrollSpeedChangeLinearCycle(value, duration);
            StartCoroutine(ScrollSpeedChangeLinearCycle_C);
        }

        public void InsertObj(Transform target)
        {
            SingleObstacle obstacle;
            target.TryGetComponent(out obstacle);

            flexibleObjs.Add(obstacle);
            target.parent = transform;
        }
        public void RemoveObj(Transform target)
        {
            SingleObstacle obstacle;
            target.TryGetComponent(out obstacle);

            int targetIndex = flexibleObjs.IndexOf(obstacle);
            target.parent = flexiblesOriginalParent;
            flexibleObjs.RemoveAt(targetIndex);
        }

        public void ResetFlexible()
        {
            for (int i = 0; i < flexibleObjs.Count; i++)
            {
                flexibleObjs[i].transform.parent = flexiblesOriginalParent;
                flexibleObjs[i].StopAllCoroutines();
                flexibleObjs[i].gameObject.SetActive(false);
            }
            flexibleObjs.Clear();
        }

        void Awake()
        {
            for (int i = 0; i < repeatSets.Length; i++)
            {
                repeatSets[i].Init(scrollDir);
            }
            flexibleObjs = new List<SingleObstacle>();
        }
        void Start()
        {
            if (scrollOnStart)
                ScrollStart();
        }

        IEnumerator ScrollUpdate_C;
        IEnumerator ScrollUpdate()
        {
            float repeat = 0;
            while (true)
            {
                repeat += scrollSpeed * Time.deltaTime;
                transform.position += scrollDir.normalized * scrollSpeed * Time.deltaTime;

                for (int i = 0; i < repeatSets.Length; i++)
                {
                    repeatSets[i].OnScrollUpdate(scrollDir);
                }

                yield return null;
            }
        }
        IEnumerator ScrollSpeedChangeLinearCycle_C;
        IEnumerator ScrollSpeedChangeLinearCycle(float goalSpeed, float duration)
        {
            float gap = goalSpeed - scrollSpeed;
            gap *= 1 / duration;
            while (duration > 0) 
            {
                scrollSpeed += gap * Time.deltaTime;
                duration -= Time.deltaTime;
                yield return null;
            }
            scrollSpeed = goalSpeed;
        }

        [System.Serializable]
        public class RepeatSet
        {
            public float RepeatRange { get { return repeatRange; } }
            public float CopyCount { get { return copyCount; } }

            [Header("복사 타겟")]
            [SerializeField] Transform Target;
            [Header("복사본 개수")]
            [SerializeField] int copyCount;
            [Header("복사본 사이 간격")]
            [SerializeField] float repeatRange;
            [Header("이동 최소거리")]
            [SerializeField] float swapDist;
            Transform[] repeatObjs;

            Vector3 focusingMoveGoal;
            int focusingIndex;
            int focusingMoveTargetIndex { get { return focusingIndex + 1 >= copyCount ? 0 : focusingIndex + 1; } }

            public void Init(Vector3 scrollDir)
            {
                Vector3 copyStartPos = Target.position - ((copyCount - 1) * repeatRange * scrollDir / 2);
                repeatObjs = new Transform[copyCount];
                repeatObjs[0] = Target;
                Target.position = copyStartPos;
                for (int i = 1; i < copyCount; i++)
                {
                    repeatObjs[i] = Instantiate(Target);
                    repeatObjs[i].parent = Target.parent;
                    repeatObjs[i].position = copyStartPos + scrollDir * repeatRange * i;
                }

                focusingIndex = copyCount - 1;
                focusingMoveGoal = repeatObjs[repeatObjs.Length - 1].position;
            }
            public void OnScrollUpdate(Vector3 scrollDir)
            {
                if (Vector3.Distance(repeatObjs[focusingIndex].position, focusingMoveGoal) <= swapDist)
                {
                    repeatObjs[focusingIndex].position = repeatObjs[focusingMoveTargetIndex].position - scrollDir * repeatRange;
                    focusingIndex = focusingIndex - 1 < 0 ? copyCount - 1 : focusingIndex - 1;
                }
            }
        }
    }
}