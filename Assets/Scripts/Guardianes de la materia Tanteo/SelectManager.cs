using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectManager : MonoBehaviour
{
    public enum Molecula
    {
        Azul,
        Roja
    }
    public static SelectManager instance;
    public Molecula currentmol;

    private void Awake()
    {
        instance = this;
    }
    public void SelectBlue()
    {
        currentmol = Molecula.Azul;
    }

    public void SelectRed()
    {
        currentmol = Molecula.Roja;
    }
}
