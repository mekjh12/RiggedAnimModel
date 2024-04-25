using OpenGL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LSystem.Animate
{
    public class BoneAngle
    {
        Vertex4f _angle;
        Vertex2f _twist;

        public Vertex4f ConstraintAngle
        {
            get => _angle; set => _angle = value;
        }

        public Vertex2f TwistAngle
        {
            get => _twist; set => _twist = value;
        }

        public BoneAngle(float theta1 = 180.0f, float theta2 = 180.0f, float theta3 = 180.0f, float theta4 = 180.0f, float twistThetaMin = -90.0f, float twistThetaMax = 90.0f)
        {
            _angle = new Vertex4f(theta1, theta2, theta3, theta4);
            _twist = new Vertex2f(twistThetaMin, twistThetaMax);
        }

        public bool IsInboundTwist(float theta) => (_twist.x <= theta && theta <= _twist.y);

    }
}
