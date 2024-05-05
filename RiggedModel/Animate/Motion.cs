using System;
using System.Collections.Generic;
using System.Linq;

namespace LSystem.Animate
{
    public class Motion
    {
        private string _animationName;
        private float _length;
        private Dictionary<float, KeyFrame> _keyframes;

        public KeyFrame FirstKeyFrame => (_keyframes.Values.Count > 0) ? _keyframes.Values.ElementAt(0) : null;

        public KeyFrame LastKeyFrame => (_keyframes.Values.Count > 0) ? _keyframes.Values.ElementAt(_keyframes.Count - 1) : null;

        public KeyFrame MiddleKeyFrame
        {
            get
            {
                int numKeyFrame = _keyframes.Count;
                int idx = (int)(numKeyFrame / 2.0f);
                return _keyframes.Values.ElementAt(idx);
            }
        }

        public Dictionary<float, KeyFrame> Keyframes => _keyframes;

        public float Length => _length;

        public string Name => _animationName;

        public int KeyFrameCount => _keyframes.Count;

        public Motion(string name, float lengthInSeconds)
        {
            _animationName = name;
            _length = lengthInSeconds;
            _keyframes = new Dictionary<float, KeyFrame>();
        }

        public KeyFrame CloneKeyFrame(float time)
        {
            float currentKeyFrameTime = 0.0f;
            foreach (KeyValuePair<float, KeyFrame> item in _keyframes)
            {
                float keytime = item.Key;
                currentKeyFrameTime = keytime;
                if (time < currentKeyFrameTime) break;
            }

            KeyFrame keyFrame = new KeyFrame(currentKeyFrameTime);
            keyFrame = _keyframes[currentKeyFrameTime].Clone();
            return keyFrame;
        }

        public KeyFrame this[float time]
        {
            get
            {
                if (_keyframes.ContainsKey(time))
                {
                    return (_keyframes.Values.Count > 0) ? _keyframes[time] : null;
                }
                else
                {
                    return FirstKeyFrame;
                }
            }

            set
            {
                if (_keyframes.ContainsKey(time))
                {
                    _keyframes[time] = value;
                }
                else
                {
                    _keyframes.Add(time, value);
                }
            }
        }

        public KeyFrame Frame(int index)
        {
            return _keyframes.Values.ElementAt(index);
        }

        public void AddKeyFrame(float time)
        {
            if (!_keyframes.ContainsKey(time))
            {
                _keyframes[time] = new KeyFrame(time);
            }
        }

        public void AddKeyFrame(KeyFrame keyFrame)
        {
            _keyframes[keyFrame.TimeStamp] = keyFrame;
        }

        /// <summary>
        /// 두 모션을 블렌딩한 모션을 반환한다.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="prevMotion"></param>
        /// <param name="prevTime"></param>
        /// <param name="nextMotion"></param>
        /// <param name="nextTime"></param>
        /// <param name="blendingInterval"></param>
        /// <returns></returns>
        public static Motion BlendMotion(string name, Motion prevMotion, float prevTime, Motion nextMotion, float nextTime, float blendingInterval)
        {
            KeyFrame k0 = prevMotion.CloneKeyFrame(prevTime);
            k0.TimeStamp = 0;
            KeyFrame k1 = nextMotion.CloneKeyFrame(nextTime);
            k1.TimeStamp = blendingInterval;
            Motion blendMotion = new Motion(name, blendingInterval);
            if (k0 != null) blendMotion.AddKeyFrame(k0);
            if (k1 != null) blendMotion.AddKeyFrame(k1);
            return blendMotion;
        }
    }
}
