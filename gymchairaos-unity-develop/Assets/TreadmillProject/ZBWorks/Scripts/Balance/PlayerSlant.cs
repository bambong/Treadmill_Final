using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using DG.Tweening;

namespace ZB.Balance
{
    public class PlayerSlant : MonoBehaviour
    {
        UnityEvent uEvent;

        [SerializeField] Transform m_pivot_left;
        [SerializeField] Transform m_pivot_right;
        [SerializeField] Transform m_body;

        [SerializeField] float m_slantAngle;
        [SerializeField] bool m_rotating;

        int m_nowSlantDir = 0;

        public void SlantReset()
        {
            m_body.parent.DORotate(new Vector3(0, 0, 0), 1).SetEase(Ease.OutQuart).SetUpdate(true);
        }
        public void Slant(int dir)
        {
            if (m_rotating)
            {
                uEvent.RemoveAllListeners();
                uEvent.AddListener(() => Slant(dir));
            }
            else if (dir != m_nowSlantDir)
            {
                m_rotating = true;

                //중간에 있다가 다른방향으로
                if (m_nowSlantDir == 0 && dir != 0)
                {
                    m_body.parent = dir == 1 ? m_pivot_right : m_pivot_left;
                    m_body.parent.DORotate(new Vector3(0, 0, -m_slantAngle * dir), 1).SetEase(Ease.OutQuart).OnComplete(() =>
                    {
                        m_rotating = false;
                        uEvent.Invoke();
                    });
                }

                //다른방향에 있다가 중간으로
                else if (m_nowSlantDir != 0 && dir == 0)
                {
                    m_body.parent.DORotate(new Vector3(0, 0, 0), 1).SetEase(Ease.OutQuart).OnComplete(() =>
                    {
                        m_rotating = false;
                        uEvent.Invoke();
                    });
                }

                //완전히 다른방향으로
                else if (m_nowSlantDir * -1 == dir)
                {
                    m_body.parent.DORotate(new Vector3(0, 0, 0), 0.5f).SetEase(Ease.Unset).OnComplete(() =>
                    {
                        m_body.parent = dir == 1 ? m_pivot_right : m_pivot_left;
                        m_body.parent.DORotate(new Vector3(0, 0, -m_slantAngle * dir), 0.5f).SetEase(Ease.OutQuart).OnComplete(() =>
                        {
                            m_rotating = false;
                            uEvent.Invoke();
                        });
                    });
                }

                m_nowSlantDir = dir;
            }
        }

        [ContextMenu("mid")]
        void Slant_mid()
        {
            Slant(0);
        }
        [ContextMenu("left")]
        void Slant_left()
        {
            Slant(-1);
        }
        [ContextMenu("right")]
        void Slant_right()
        {
            Slant(1);
        }

        private void Awake()
        {
            uEvent = new UnityEvent();
        }
    }
}