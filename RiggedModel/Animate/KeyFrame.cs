﻿using System.Windows.Media.Animation;

namespace LSystem.Animate
{
    public class KeyFrame
    {
        private ArmaturePose _pose;
        private float _timeStamp;

        public bool ContainsKey(string boneName) => _pose.ContainsKey(boneName);

        public KeyFrame(float timeStamp)
        {
            _timeStamp = timeStamp;
            _pose = new ArmaturePose();
        }

        public ArmaturePose Pose
        {
            get => _pose;
            set => _pose = value;
        }

        public float TimeStamp
        {
            get => _timeStamp;
            set => _timeStamp = value;
        }

        public BonePose this[string boneName]
        {
            get => _pose[boneName];
            set => _pose[boneName] = value;
        }

        public void AddBoneTransform(string boneName, BonePose jointTransform)
        {
            _pose[boneName] = jointTransform;
        }

        public KeyFrame Clone()
        {
            KeyFrame res = new KeyFrame(_timeStamp);
            ArmaturePose armaturePose = new ArmaturePose();
            armaturePose = _pose.Clone();
            res.Pose = armaturePose;
            return res;
        }

    }
}
