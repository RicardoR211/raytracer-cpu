using System;
using System.Collections.Generic;
using System.Text;

namespace Projeto01_Vetores
{
    internal struct Ray
    {
        public Vec3 Origin;
        public Vec3 Direction;

        //Construtor
        public Ray(Vec3 Origin, Vec3 Direction)
        {
            this.Origin = Origin;
            this.Direction = Direction;
        }

        //Um método At(float t) que retorna o ponto ao longo do raio naquele t
        public Vec3 At(float t)
        {
            return Origin + (Direction * t);
        }
    }
}
