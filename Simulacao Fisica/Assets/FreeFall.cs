using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreeFall : MonoBehaviour
{

    public struct CubeFall
    {
       public float velocidadeInicial;
       public float deltaDistance;
    }

    [SerializeField] ComputeShader freeFallCompute;
    CubeFall[] data;
    int count;
    [SerializeField] GameObject ground;
    [SerializeField]GameObject[] cubes;

    public float[] posY;
    float[] velInicial;
    float g = 9.8f;

    public bool fallCPU = false;
    public bool fallGPU = false;
    public bool colorChanged = false;

    float tempoI;
    float tempoF;

    void Start()
    {
        count = gameObject.GetComponent<RandomColor>().counts;
        data = new CubeFall[count * count];
        posY = new float[count * count];
        velInicial = new float[count * count];

        for (int i = 0; i < data.Length; i++)
        {
            posY[i] = gameObject.transform.position.y;
            velInicial[i] = 0;
        }

        ground = GameObject.FindGameObjectWithTag("Ground");
    }

    void Update()
    { 

        if (fallCPU)
        {
            FallCPU();
        }

        if (fallGPU)
        {
            FallGPU();
        }

        CheckGround();
    }

    private void CheckGround()
    {
        for (int i = 0; i < cubes.Length; i++)
        {
            if (cubes[i].transform.position.y <= ground.transform.position.y + 0.25f)
            {
                if (fallCPU)
                {
                    fallCPU = false;
                    if (!colorChanged)
                        ChangeColor();
                }

                if (fallGPU)
                {
                    fallGPU = false;
                    if (!colorChanged)
                    {
                        colorChanged = true;
                        gameObject.GetComponent<RandomColor>().ChangeColorGPU();
                    }
                }

                cubes[i].transform.position = new Vector3(cubes[i].transform.position.x, ground.transform.position.y + 0.25f, cubes[i].transform.position.z);

                velInicial[i] = 0;
            }
        }
    }

    private void FallCPU()
    {
        for (int i = 0; i < cubes.Length; i++)
        {
            float[] vF = new float[cubes.Length];
            float[] dP = new float[cubes.Length];
            float[] pF = new float[cubes.Length];

            vF[i] = velInicial[i] + g * Time.deltaTime;
            dP[i] = ((velInicial[i] + vF[i]) * Time.deltaTime) / 2;
            pF[i] = posY[i] - dP[i];

            cubes[i].transform.position = new Vector3(cubes[i].transform.position.x, pF[i], cubes[i].transform.position.z);

            velInicial[i] = vF[i];
            posY[i] = cubes[i].transform.position.y;
        }
    }

    private void FallGPU()
    {
        int totalSize = sizeof(float) * 2;

        ComputeBuffer computeBuffer = new ComputeBuffer(data.Length, totalSize);
        computeBuffer.SetData(data);

        for (int i = 0; i < cubes.Length; i++)
        {
            freeFallCompute.SetBuffer(0, "cubeFall", computeBuffer);
            freeFallCompute.SetFloat("posI", posY[i]);
            freeFallCompute.SetFloat("velI", velInicial[i]);
            freeFallCompute.SetFloat("deltaTime", Time.deltaTime);
            freeFallCompute.SetInt("nCubes", data.Length);
            freeFallCompute.Dispatch(0, Mathf.CeilToInt(data.Length / 10), 1, 1);
        }
        computeBuffer.GetData(data);

        for (int i = 0; i < cubes.Length; i++)
        {
            posY[i] = data[i].deltaDistance;
            velInicial[i] = data[i].velocidadeInicial;

            cubes[i].transform.position = new Vector3(cubes[i].transform.position.x, posY[i], cubes[i].transform.position.z);
        }

        computeBuffer.Dispose();
    }

    private void ChangeColor()
    {
        for (int i = 0; i < cubes.Length; i++)
        {
            cubes[i].GetComponent<MeshRenderer>().material.SetColor("_Color", UnityEngine.Random.ColorHSV());
            colorChanged = true;
        }
        EndTime();
    }

    public void SetCubes()
    {
        cubes = gameObject.GetComponent<RandomColor>().gameObjects;
    }

    public void StartTime()
    {
        tempoI = Time.realtimeSinceStartup;
        print(tempoI);
    }

    public void EndTime()
    {
        tempoF = Time.realtimeSinceStartup;
        print("Tempo final = " + tempoF + ". Tempo total percorrido = " + (tempoF - tempoI));
    }
}
