using System;
using System.Collections.Generic;
using System.Text;

namespace Projeto01_Vetores
{
    internal struct Camera
    {
        public Vec3 Position;
        public Vec3 Target;
        public Vec3 Up = new Vec3(0f, 1f, 0f);
        public float Fov;
        public float FovRad;

        //Construtor
        public Camera (Vec3 Position, Vec3 Target, float Fov)
        {
            this.Position = Position;
            this.Target = Target;
            this.Fov = Fov;
            this.FovRad = Fov * MathF.PI / 180f;
        }

        //Transforma uma pixel (x, y) em um raio.
        public Ray GetRay(int x, int y,  int screenWidth, int screenHeight)
        {
            float xConvert = ((float)x / screenWidth) * 2.0f - 1.0f;
            float yConvert = ((float)y / screenHeight) * 2.0f - 1.0f;

            //Invertindo o eixo Y para corrigir renderização de ponta-cabeça
            //Se comentar essa parte de baixo fica de ponta-cabeça nossa render
            yConvert = yConvert * -1f;

            //Pegando as dimensões físicas no viewport
            float aspectRatio = (float)screenWidth / screenHeight;
            float viewportHeight = 2 * MathF.Tan(FovRad / 2);
            float viewportWidth = viewportHeight * aspectRatio;

            //Definindo os eixos locais da câmera
            Vec3 zAxis = (Position - Target).Normalize();
            Vec3 xAxis = Vec3.Cross(Up, zAxis).Normalize();
            Vec3 yAxis = Vec3.Cross(zAxis, xAxis);

            //Fazendo forawrd que aponta pra frente
            //Em resumo, é o oposto do zAxis
            Vec3 forward = zAxis * -1f;

            //Encontrando o ponto no plano do Viewport
            Vec3 centroDoViewport = Position + forward;
            Vec3 deslocamentoHorizontal = xAxis * (xConvert * (viewportWidth / 2f));
            Vec3 deslocamentoVertical = yAxis * (yConvert * (viewportHeight / 2f));

            Vec3 pointInViewport = centroDoViewport + deslocamentoHorizontal + deslocamentoVertical;

            //Calcula a direção normalizada do raio
            Vec3 rayDirection = (pointInViewport - Position).Normalize();


            return new Ray(Position, rayDirection);


        }
    }
}
