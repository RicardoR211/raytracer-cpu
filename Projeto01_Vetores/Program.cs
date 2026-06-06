using Projeto01_Vetores;
using Raylib_cs;
using System;
using System.Numerics;
using System.Runtime.ConstrainedExecution;

namespace Projeto01_Vetores
{
    internal class Program
    {
        public static void Main(String[] args)
        {
            int width = 800;
            int height = 600;


            Raylib.InitWindow(width, height, "Projeto 04 - Raycast 2D");
            Raylib.SetTargetFPS(60);

            //Criando a esfera
            Vec3 esferaCentro = new Vec3(0, 0, 0);
            float esferaRaio = 0.5f;

            Sphere esfera = new Sphere(esferaCentro, esferaRaio);

            //Criando a câmera
            Vec3 cameraPosition = new Vec3(0, 0, 3f);
            Vec3 cameraTarget = new Vec3(0, 0, 0);
            float cameraFov = 50f;

            Camera camera = new Camera(cameraPosition, cameraTarget, cameraFov);

            //Para rotacionar a câmera
            float theta = 0f;
            float phi = MathF.PI / 2f;
            float radius = 3f;
            float sensibilidade = 0.025f;

            //Tentantiva de otimzar usando Image
            Image image = Raylib.GenImageColor(width, height, Color.Black);
            Texture2D texture = Raylib.LoadTextureFromImage(image);

            Color[] pixels = new Color[width * height];

            //Definindo a posição da luz
            Vec3 luzPosicao = new Vec3(2f, 3f, 2f);
            while (!Raylib.WindowShouldClose())
            {
                if (Input.SegurandoClique(MouseButton.Left))
                {
                    Vector2 delta = Raylib.GetMouseDelta();

                    // Passo 3: Atualiza os ângulos com o delta e a sensibilidade
                    theta += delta.X * sensibilidade;
                    phi += delta.Y * sensibilidade;

                    // Trava o eixo vertical (phi) para a câmera não dar cambalhotas invertendo o "cima"
                    phi = Math.Clamp(phi, 0.01f, MathF.PI - 0.01f);


                    float camX = radius * MathF.Sin(phi) * MathF.Cos(theta);
                    float camY = radius * MathF.Cos(phi);
                    float camZ = radius * MathF.Sin(phi) * MathF.Sin(theta);

                    cameraPosition = new Vec3(camX, camY, camZ);
                    cameraTarget = new Vec3(0, 0, 0);

                    // Passo 5: Recria a câmera com a nova posição orbital
                    camera = new Camera(cameraPosition, cameraTarget, 50f);
                }

                if (Input.SegurandoClique(MouseButton.Right))
                {
                    Vector2 delta = Raylib.GetMouseDelta();

                    // Passo 3: Atualiza os ângulos com o delta e a sensibilidade
                    theta += delta.X * sensibilidade;
                    phi += delta.Y * sensibilidade;

                    // Trava o eixo vertical (phi) para a câmera não dar cambalhotas invertendo o "cima"
                    phi = Math.Clamp(phi, 0.01f, MathF.PI - 0.01f);


                    float lightX = radius * MathF.Sin(phi) * MathF.Cos(theta);
                    float lightY = radius * MathF.Cos(phi);
                    float lightZ = radius * MathF.Sin(phi) * MathF.Sin(theta);

                    luzPosicao = new Vec3(lightX, lightY, lightZ);
                }


                Parallel.For(0, height, i =>
                {
                    for (int j = 0; j < width; j++)
                    {
                        Ray ray = camera.GetRay(j, i, width, height);

                        if (esfera.Intersect(ray, out float t))
                        {
                            //Encontra o ponto onde o raio bateu
                            Vec3 hitPoint = ray.At(t);

                            Vec3 normal = (hitPoint - esferaCentro).Normalize();

                            //Com luz
                            Vec3 lightDir = (luzPosicao - hitPoint).Normalize();
                            float intensidade = Vec3.DotProduct(normal, lightDir);

                            if (intensidade < 0) intensidade = 0;

                            //Definindo a cor base da esfera
                            float baseR = 255f;
                            float baseG = 0f;
                            float baseB = 0f;

                            //Multiplicando o base pela luz
                            /*
                            byte r = (byte)(baseR * intensidade);
                            byte g = (byte)(baseG * intensidade);
                            byte b = (byte)(baseB * intensidade);
                            */
                            //Mapeia a normal para rgb (aq usa sem a luz)
                           
                            byte r = (byte)(((normal.X + 1.0f) * 0.5f) * 255.0f * intensidade);
                            byte g = (byte)(((normal.Y + 1.0f) * 0.5f) * 255.0f * intensidade);
                            byte b = (byte)(((normal.Z + 1.0f) * 0.5f) * 255.0f * intensidade);
                           

                            Color cor = new Color(r, g, b, (byte)255);

                            //Raylib.ImageDrawPixel(ref image, j, i, cor);
                            pixels[i * width + j] = cor;
                        }
                        else
                        {
                            // O raio passou direto. Define a cor do céu/fundo para o pixel (x, y)
                            pixels[i * width + j] = Color.Black;
                        }
                    }

                });
                
                unsafe
                {
                    //Raylib.UpdateTexture(texture, image.Data);
                    fixed (Color* ptr = pixels)
                    {
                        Raylib.UpdateTexture(texture, ptr);
                    }
                }

                Raylib.BeginDrawing();
                Raylib.ClearBackground(Color.Black);

                // Desenha o buffer completo renderizado
                Raylib.DrawTexture(texture, 0, 0, Color.White);

                Raylib.DrawFPS(10, 10);
                Raylib.EndDrawing();
            }

            Raylib.UnloadImage(image);
            Raylib.UnloadTexture(texture);
            Raylib.CloseWindow();
        }
        
    }
}
