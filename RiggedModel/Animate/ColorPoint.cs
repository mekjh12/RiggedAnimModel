using OpenGL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LSystem.Animate
{
    public struct ColorPoint
    {
        Vertex3f _pos;
        Vertex3f _color;
        float _size;

        public float Size
        {
            get => _size; 
            set => _size = value;
        }


        public Vertex3f Color3
        {
            get => _color;
            set => _color = value;
        }

        public Vertex4f Color4 => new Vertex4f(_color.x, _color.y, _color.z, 1.0f);

        public Vertex3f Position
        {
            get => _pos;
            set => _pos = value;
        }

        public ColorPoint(float x, float y, float z, float r, float g, float b, float size)
        {
            _pos = new Vertex3f(x, y, z);
            _color = new Vertex3f(r, g, b);
            _size = size;
        }

        public ColorPoint(float x, float y, float z, float r, float g, float b)
        {
            _pos = new Vertex3f(x, y, z);
            _color = new Vertex3f(r, g, b);
            _size = 0.02f;
        }

        public ColorPoint(float x, float y, float z)
        {
            _pos = new Vertex3f(x, y, z);
            _color = new Vertex3f(1, 0, 0);
            _size = 0.02f;
        }

        public ColorPoint(Vertex3f pos, Vertex3f color)
        {
            _pos = pos;
            _color = color;
            _size = 0.02f;
        }

        public ColorPoint(Vertex3f pos, float r, float g, float b)
        {
            _pos = pos;
            _color = new Vertex3f(r, g, b);
            _size = 0.02f;
        }

        public ColorPoint(Vertex3f pos, float r, float g, float b, float size)
        {
            _pos = pos;
            _color = new Vertex3f(r, g, b);
            _size = size;
        }
    }
}
