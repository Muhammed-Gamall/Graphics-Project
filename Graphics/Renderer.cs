using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Tao.OpenGl;

//include GLM library
using GlmNet;


using System.IO;
using System.Diagnostics;

namespace Graphics
{
    class Renderer
    {
        Shader sh;
        
        uint triangleBufferID;
        uint xyzAxesBufferID;

        //3D Drawing
        mat4 ModelMatrix;
        mat4 ViewMatrix;
        mat4 ProjectionMatrix;
        
        int ShaderModelMatrixID;
        int ShaderViewMatrixID;
        int ShaderProjectionMatrixID;

        const float rotationSpeed = 1f;
        float rotationAngle = 0;

        public float translationX=0, 
                     translationY=0, 
                     translationZ=0;

        Stopwatch timer = Stopwatch.StartNew();

        vec3 triangleCenter;

        public void Initialize()
        {
            string projectPath = Directory.GetParent(Environment.CurrentDirectory).Parent.FullName;
            sh = new Shader(projectPath + "\\Shaders\\SimpleVertexShader.vertexshader", projectPath + "\\Shaders\\SimpleFragmentShader.fragmentshader");

            Gl.glClearColor(0, 0, 0.4f, 1);
            
            float[] rabitVertices= { 
                           //left ear
		     -0.592f*30,0.958f*30,0.0f*30,   //0
              241,166,75,
              -0.700f*30,0.600f*30,0.0f*30,  //1 
              241,166,75,
              -0.625f*30,0.245f*30,0.0f*30,  //2
              241,166,75,
              -0.456f*30,0.216f*30,0.0f*30,  //3
              241,166,75,
              -0.400f*30,0.654f*30,0.0f*30,  //4 
              241,166,75,
              -0.592f*30,0.958f*30,0.0f*30,  //5
              241,166,75,
                         // right ear
             0.160f*30,0.874f*30,0.0f*30, //6
              241,166,75,
            -0.103f*30,0.617f*30,0.0f*30, //7
              241,166,75,
            -0.151f*30,0.180f*30,0.0f*30, //8
              241,166,75,
             0.044f*30,0.164f*30,0.0f*30, //8
              241,166,75,
             0.202f*30,0.533f*30,0.0f*30, //10
              241,166,75,
             0.199f*30,0.874f*30,0.0f*30,//11
             0,0,1,
                           // head
             -0.625f*30,  0.245f*30,  0.0f*30,  //12
              0,1,0,
             -0.780f*30,-0.111f*30,0.0f*30, //13
              0,1,0,
              -0.370f*30,-0.500f*30,0.0f*30, //14
              0,1,0,
              0.11f*30,-0.172f*30,0.0f*30, //15
              0,1,0,
              0.044f*30,0.164f*30,0.0f*30, //16
              0,1,0,
              -0.625f*30,  0.245f*30,  0.0f*30,    //17
              0,1,0,
                          //body
             0.11f*30,-0.172f*30,0.0f*30,//24
             1,0,1,
            -0.370f*30,-0.500f*30,0.0f*30,//25
            1,0,1,
            -0.529f*30,-0.350f*30,0.0f*30,//26
            1,0,1,
            -0.300f*30,-0.850f*30,0.0f*30,//27
            1,0,1,
                       //mouse
           -0.780f*30,-0.111f*30,0.0f*30, //21
           0,1,1,
           -0.370f*30,-0.500f*30,0.0f*30, //22
           0,1,1,
           0.11f*30,-0.172f*30,0.0f*30, //23
           0,1,1,
                       // nose
           -0.625f*30,  0.245f*30,  0.0f*30, //18
           1,1,0,
           -0.370f*30,-0.500f*30,0.0f*30, //19
           1,1,0,
           0.044f*30,0.164f*30,0.0f*30, //20
           1,1,0,
                     //parallel
           0.11f*30,-0.172f*30,0.0f*30,//28
           1,1,1,
           -0.380f*30,-1.00f*30,0.0f*30,//29
           1,1,1,
           0.051f*30,-0.990f*30,0.0f*30, //30
           1,1,1,
           0.40f*30,-0.340f*30,0.0f*30, //31
           1,1,1,
                    //tringle1
            -0.350f*30,-0.740f*30,0.0f*30,//32
           0,1,0,
           -0.500f*30,-0.99f*30,0.0f*30,//33
          0,1,0,
           -0.380f*30,-1.00f*30,0.0f*30,//34
           0,1,0,
           -0.300f*30,-0.850f*30,0.0f*30,//35
           0,1,0,
                     //tringle2
            0.11f*30,-0.172f*30,0.0f*30, //36
            -7,7,7,
            0.400f*30,-0.990f*30,0.0f*30, //23
            -7,7,7,
            0.450f*30,-0.770f*30,0.0f*30, //23
            -7,7,7,
            0.40f*30,-0.340f*30,0.0f*30, //23
            -7,7,7,
                       //tringle 3
            0.260f*30,-0.600f*30,0.0f*30,//40
            0,0,1,
            0.051f*30,-0.990f*30,0.0f*30,//41
            0,0,1,
            0.400f*30,-0.990f*30,0.0f*30,//42
            0,0,1,
                 //tringle 4
            0.11f*30,-0.172f*30,0.0f*30,//43
            1,0,0,
            0.260f*30,-0.600f*30,0.0f*30,//44
            1,0,0,
            0.40f*30,-0.340f*30,0.0f*30,//45
            1,0,0 
            }; 
            
            //triangleCenter = new vec3(10, 7, -5);

            float[] xyzAxesVertices = {
		        //x
		        0.0f, 0.0f, 0.0f,
                1.0f, 0.0f, 0.0f, 
		        100.0f, 0.0f, 0.0f,
                1.0f, 0.0f, 0.0f, 
		        //y
	            0.0f, 0.0f, 0.0f,
                0.0f,1.0f, 0.0f, 
		        0.0f, 100.0f, 0.0f,
                0.0f, 1.0f, 0.0f, 
		        //z
	            0.0f, 0.0f, 0.0f,
                0.0f, 0.0f, 1.0f,  
		        0.0f, 0.0f, -100.0f,
                0.0f, 0.0f, 1.0f,  
            };


            triangleBufferID = GPU.GenerateBuffer(rabitVertices);
            xyzAxesBufferID = GPU.GenerateBuffer(xyzAxesVertices);

            // View matrix 
            ViewMatrix = glm.lookAt(
                        new vec3(50, 50, 50), // Camera is at (0,5,5), in World Space
                        new vec3(0, 0, 0), // and looks at the origin
                        new vec3(0, 1, 0)  // Head is up (set to 0,-1,0 to look upside-down)
                );
            // Model Matrix Initialization
            ModelMatrix = new mat4(1);

            //ProjectionMatrix = glm.perspective(FOV, Width / Height, Near, Far);
            ProjectionMatrix = glm.perspective(45.0f, 4.0f / 3.0f, 0.1f, 100.0f);
            
            // Our MVP matrix which is a multiplication of our 3 matrices 
            sh.UseShader();


            //Get a handle for our "MVP" uniform (the holder we created in the vertex shader)
            ShaderModelMatrixID = Gl.glGetUniformLocation(sh.ID, "modelMatrix");
            ShaderViewMatrixID = Gl.glGetUniformLocation(sh.ID, "viewMatrix");
            ShaderProjectionMatrixID = Gl.glGetUniformLocation(sh.ID, "projectionMatrix");

            Gl.glUniformMatrix4fv(ShaderViewMatrixID, 1, Gl.GL_FALSE, ViewMatrix.to_array());
            Gl.glUniformMatrix4fv(ShaderProjectionMatrixID, 1, Gl.GL_FALSE, ProjectionMatrix.to_array());

            timer.Start();
        }

        public void Draw()
        {
            sh.UseShader();
            Gl.glClear(Gl.GL_COLOR_BUFFER_BIT);

            #region XYZ axis

            Gl.glBindBuffer(Gl.GL_ARRAY_BUFFER, xyzAxesBufferID);
            Gl.glUniformMatrix4fv(ShaderModelMatrixID, 1, Gl.GL_FALSE, new mat4(1).to_array()); // Identity

            Gl.glEnableVertexAttribArray(0);
            Gl.glEnableVertexAttribArray(1);

            Gl.glVertexAttribPointer(0, 3, Gl.GL_FLOAT, Gl.GL_FALSE, 6 * sizeof(float), (IntPtr)0);
            Gl.glVertexAttribPointer(1, 3, Gl.GL_FLOAT, Gl.GL_FALSE, 6 * sizeof(float), (IntPtr)(3 * sizeof(float)));
             
            Gl.glDrawArrays(Gl.GL_LINES, 0, 6);

            Gl.glDisableVertexAttribArray(0);
            Gl.glDisableVertexAttribArray(1);

            #endregion

            #region Animated rabit
            Gl.glBindBuffer(Gl.GL_ARRAY_BUFFER, triangleBufferID);
            Gl.glUniformMatrix4fv(ShaderModelMatrixID, 1, Gl.GL_FALSE, ModelMatrix.to_array());

            Gl.glEnableVertexAttribArray(0);
            Gl.glEnableVertexAttribArray(1);

            Gl.glVertexAttribPointer(0, 3, Gl.GL_FLOAT, Gl.GL_FALSE, 6 * sizeof(float), (IntPtr)0);
            Gl.glVertexAttribPointer(1, 3, Gl.GL_FLOAT, Gl.GL_FALSE, 6 * sizeof(float), (IntPtr)(3 * sizeof(float)));

            Gl.glDrawArrays(Gl.GL_POLYGON, 0, 6);
            Gl.glDrawArrays(Gl.GL_POLYGON, 6, 6);
            Gl.glDrawArrays(Gl.GL_POLYGON, 12, 6);
            Gl.glDrawArrays(Gl.GL_POLYGON, 18, 4);
            Gl.glDrawArrays(Gl.GL_POLYGON, 22, 3);
            Gl.glDrawArrays(Gl.GL_POLYGON, 25, 3);
            Gl.glDrawArrays(Gl.GL_POLYGON, 28, 4);
            Gl.glDrawArrays(Gl.GL_POLYGON, 32, 4);
            Gl.glDrawArrays(Gl.GL_POLYGON, 36, 4);
            Gl.glDrawArrays(Gl.GL_POLYGON, 40, 3);
            Gl.glDrawArrays(Gl.GL_POLYGON, 43, 3);

            Gl.glDisableVertexAttribArray(0);
            Gl.glDisableVertexAttribArray(1);
            #endregion
        }
        

        public void Update()
        {

            timer.Stop();
            var deltaTime = timer.ElapsedMilliseconds/1000.0f;

            rotationAngle += deltaTime * rotationSpeed;

            List<mat4> transformations = new List<mat4>();
            transformations.Add(glm.translate(new mat4(1), -1 * triangleCenter));
            transformations.Add(glm.rotate(rotationAngle, new vec3(0, 0, 1)));
            transformations.Add(glm.translate(new mat4(1),  triangleCenter));
            transformations.Add(glm.translate(new mat4(1), new vec3(translationX, translationY, translationZ)));

            ModelMatrix =  MathHelper.MultiplyMatrices(transformations);
            
            timer.Reset();
            timer.Start();
        }
        
        public void CleanUp()
        {
            sh.DestroyShader();
        }
    }
}
