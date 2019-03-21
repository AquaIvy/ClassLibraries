using System;
using System.Collections.Generic;
using UnityEngine;

namespace Aquaivy.Unity
{

    /// <summary>
    /// ��������
    /// </summary>
    /// <param name="timeElapsed">��ǰʱ��ļ��</param>
    /// <param name="start">��ʼֵ</param>
    /// <param name="change">��ʼֵ���յ�ֵ�Ĳ�ֵ</param>
    /// <param name="duration">����ʱ��</param>
    /// <returns></returns>
    /// <remarks>
    /// �ð汾Ϊ�޸����İ汾�����һ��ض�Ϊ�趨ֵ
    /// </remarks>
    public delegate float TweeningFunction(float timeElapsed, float start, float change, float duration);

    /// <summary>
    /// ����������
    /// </summary>
    public class TweenLite
    {
        #region Static Members

        private static List<TweenLite> m_tweeners = new List<TweenLite>();

        public static void Update(int elapsedTime)
        {
            if (m_tweeners.Count > 0)
            {
                // ����ÿһ��tween
                for (var i = 0; i < m_tweeners.Count; i++)
                {
                    var tween = m_tweeners[i];
                    if (!tween.waitRelease && tween.InternalUpdate(elapsedTime))
                    {
                        m_tweeners.RemoveAt(i);
                        i--;
                    }
                }

                // �Ƴ������ΪwaitRelease��
                for (var i = 0; i < m_tweeners.Count; i++)
                {
                    var tween = m_tweeners[i];
                    if (tween.waitRelease)
                    {
                        m_tweeners.RemoveAt(i);
                        i--;
                    }
                }

            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="from">��ʼֵ</param>
        /// <param name="to">����ֵ</param>
        /// <param name="duration">����ʱ�䣨��λ�����룩</param>
        /// <param name="tweeningFunction">������ʽ</param>
        /// <param name="onupdate">
        /// ÿ֡Update�Ļص�
        /// ���� float ��ʾ��ǰ�Ĳ�ֵ
        /// </param>
        /// <param name="onend">��������ʱ�Ļص�</param>
        /// <returns></returns>
        public static TweenLite To(float from, float to, float duration, TweeningFunction tweeningFunction, Action<float> onupdate, Action onend = null)
        {
            var tween = new TweenLite(from, to, duration, tweeningFunction, onupdate);
            if (onend != null)
                tween.Ended += () => { onend(); };
            m_tweeners.Add(tween);
            return tween;
        }

        public static void ReleaseAll()
        {
            m_tweeners.ForEach(o => o.Release());
        }

        #endregion

        private TweenLite(float from, float to, float duration, TweeningFunction tweeningFunction, Action<float> onupdate)
        {
            _from = from;
            _position = from;
            _change = to - from;
            _tweeningFunction = tweeningFunction;
            _duration = duration;
            _onupdate = onupdate;
        }

        private TweenLite(float from, float to, TimeSpan duration, TweeningFunction tweeningFunction, Action<float> onupdate)
            : this(from, to, (float)duration.TotalSeconds, tweeningFunction, onupdate)
        {
        }

        #region Properties
        private float _position;

        /// <summary>
        /// ��ǰֵ
        /// from->to�Ĳ�ֵ
        /// </summary>
        public float Position
        {
            get
            {
                return _position;
            }
            protected set
            {
                _position = value;
            }
        }

        private float _from;

        /// <summary>
        /// ��ʼֵ
        /// </summary>
        protected float from
        {
            get
            {
                return _from;
            }
            set
            {
                _from = value;
            }
        }

        private float _change;

        /// <summary>
        /// �仯��ֵ
        /// </summary>
        protected float change
        {
            get
            {
                return _change;
            }
            set
            {
                _change = value;
            }
        }

        private float _duration;

        /// <summary>
        /// ʱ��
        /// </summary>
        protected float duration
        {
            get
            {
                return _duration;
            }
        }

        private float _elapsed = 0.0f;
        protected float elapsed
        {
            get
            {
                return _elapsed;
            }
            set
            {
                _elapsed = value;
            }
        }

        private bool _running = true;

        /// <summary>
        /// �Ƿ�����ִ��
        /// </summary>
        public bool Running
        {
            get { return _running; }
            protected set { _running = value; }
        }

        private TweeningFunction _tweeningFunction;
        protected TweeningFunction tweeningFunction
        {
            get
            {
                return _tweeningFunction;
            }
        }

        private Action<float> _onupdate;
        protected Action<float> onUpdate
        {
            get { return _onupdate; }
        }

        public delegate void EndHandler();
        public event EndHandler Ended;
        #endregion

        #region Methods

        private bool InternalUpdate(int elapsedTime)
        {
            if (!Running || (elapsed == duration))
            {
                return false;
            }

            if (elapsed + elapsedTime < duration)
            {
                Position = tweeningFunction(elapsed, from, change, duration);
                if (onUpdate != null)
                    onUpdate(Position);
            }

            elapsed += elapsedTime;
            if (elapsed >= duration)
            {
                elapsed = duration;

                //����һ�����μ������������������ڵ���onUpdate
                //Position = from + change;

                //������������onUpdate������һ��ִ�����1֡
                Position = tweeningFunction(elapsed, from, change, duration);
                if (onUpdate != null)
                    onUpdate(Position);

                OnEnd();
                return true;
            }

            return false;
        }

        protected void OnEnd()
        {
            if (Ended != null)
            {
                Ended();
            }
        }

        public void Start()
        {
            Running = true;
        }

        public void Stop()
        {
            Running = false;
        }

        public void Reset()
        {
            elapsed = 0.0f;
            from = Position;
        }

        public void Reset(float to)
        {
            change = to - Position;
            Reset();
        }

        public void Reverse()
        {
            elapsed = 0.0f;
            change = -change + (from + change - Position);
            from = Position;
        }


        private bool waitRelease = false;
        public void Release()
        {
            Stop();
            waitRelease = true;
        }

        //public override string ToString()
        //{
        //    return String.Format("{0}.{1}. Tween {2} -> {3} in {4}s. Elapsed {5:##0.##}s",
        //        tweeningFunction.Method.DeclaringType.Name,
        //        tweeningFunction.Method.Name,
        //        from,
        //        from + change,
        //        duration,
        //        elapsed);
        //}

        #endregion
    }
}
