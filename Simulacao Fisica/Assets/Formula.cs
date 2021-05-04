using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Formula : MonoBehaviour
{
    float vI; //velocidade inicial
    float vF; //velocidade final do intervalo de tempo
    float pI; //posição inicial
    float pF; //posição final do intervalo de tempo
    float g = 9.8f; //valor da aceleração da gravidade

    void Start()
    {
        QuedaLivreGalileu();
        QuedaLivreFormulaDada();
        QuedaLivreMinhaFormula();

        QuedaLivreComIteracoesFormulaDada();
        QuedaLivreComIteracoesMinhaFormula();
    }

    private void QuedaLivreComIteracoesMinhaFormula()
    {
        vI = 0; //velocidade inicial começando no 0
        pI = 0; //distancia inicial começando no 0
        float dP = 0; //um float para controlar a distância percorrida naquele intervalo de tempo

        for (int i = 0; i < 3; i++)
        {
            vF = vI + g * 1;
            dP = ((vI + vF) * 1) / 2;
            pF = pI + dP;

            vI = vF;
            pI = pF;
        }

        print("Na formula proposta por mim, com 3 iterações de 1s, o objeto cai " + pF + " metros em 3s");
    }

    private void QuedaLivreComIteracoesFormulaDada()
    {
        vI = 0; //velocidade inicial começando no 0
        pI = 0; //distancia inicial começando no 0

        for (int i = 0; i < 3; i++)
        {
            vF = vI + g * 1;
            pF = pI + vF * 1;

            vI = vF;
            pI = pF;
        }

        print("Na formula dada pelo exercício, com 3 iterações de 1s, o objeto cai " + pF + " metros em 3s");
    }

    private void QuedaLivreMinhaFormula()
    {
        vI = 0; //velocidade inicial começando no 0
        pI = 0; //distancia inicial começando no 0

        vF = vI + g * 3;
        pF = ((vI + vF) * 3) / 2;

        print("Na formula proposta por mim, com uma única iteração de 3s, o objeto cai " + pF + " metros em 3s");
    }

    private void QuedaLivreFormulaDada()
    {
        vI = 0; //velocidade inicial começando no 0
        pI = 0; //distancia inicial começando no 0

        vF = vI + g * 3;
        pF = pI + vF * 3;

        print("Na formula dada pelo exercício, com uma única iteração de 3s, o objeto cai " + pF + " metros em 3s");

    }

    void QuedaLivreGalileu()
    {
        pF = (g * (3 * 3)) / 2; //Distancia = gravidade vezes o tempo ao quadrado, sobre 2

        print("Na formula de Galileu, o objeto cai " + pF + " metros em 3s");
    }
}
