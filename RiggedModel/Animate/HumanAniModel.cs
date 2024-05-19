using Assimp;
using OpenGL;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LSystem.Animate
{
    class HumanAniModel : Mammal
    {
        public HumanAniModel(string name, Entity model, AniDae xmlDae) : base(name, model, xmlDae)
        {
           
        }

    }
}
