using OpenGL;

namespace LSystem
{
    public class DirectionLight
    {
        private Vertex3f _direction;
        protected Vertex4f _ambient;

        /// <summary>
        /// 빛이 나아가는 방향의 벡터
        /// </summary>
        public Vertex3f Direction
        {
            get => _direction;
            set => _direction = value;
        }

        public DirectionLight(Vertex3f direction, Vertex3f color)
        {
            _ambient = color;
            _direction = direction.Normalized;
        }

        public DirectionLight(Vertex3f direction, Vertex3f ambient, Vertex3f diffuse, Vertex3f specular)
        {
            _ambient = ambient;
            _direction = direction.Normalized;
        }

    }
}
