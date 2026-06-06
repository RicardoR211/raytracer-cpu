using System;
using System.Collections.Generic;
using System.Text;

namespace Projeto01_Vetores
{
    internal struct Sphere
    {
        Vec3 Center;
        float Radius;
        float RadiusSquared;

        public Sphere(Vec3 Center, float Radius)
        {
            this.Center = Center;
            this.Radius = Radius;
            this.RadiusSquared = Radius * Radius;
        }

        public bool Intersect(Ray ray, out float t)
        {
            Vec3 oc = ray.Origin - Center;

            float a = 1;
            float b = 2 * Vec3.DotProduct(oc, ray.Direction);
            float c = Vec3.DotProduct(oc, oc) - RadiusSquared;

            float discriminante = (b * b) - (4f * a * c);

            if(discriminante < 0)
            {
                t = 0f;
                return false;
            }

            //Encontrando o menor t positivo
            t = (-b - MathF.Sqrt(discriminante)) / (2f * a);

            return true;
        }

        //Versão otimizada se a direção vier otimizada
        public bool IntersectOptimized(Ray ray, out float t)
        {
            Vec3 oc = ray.Origin - Center;

            float h = Vec3.DotProduct(oc, ray.Direction);
            float c = Vec3.DotProduct(oc, oc) - RadiusSquared;

            // Discriminante simplificado
            float discriminante = (h * h) - c;

            if (discriminante < 0f)
            {
                t = 0f;
                return false;
            }

            // Cálculo direto sem multiplicações ou divisões extras
            t = -h - MathF.Sqrt(discriminante);

            if (t < 0f)
            {
                // Câmera pode estar dentro da esfera, testa a face de saída
                t = -h + MathF.Sqrt(discriminante);

                // Se ainda for negativo, a esfera inteira está atrás do raio
                if (t < 0f)
                {
                    t = 0f;
                    return false;
                }
            }

            return true;
        }
    }
}
