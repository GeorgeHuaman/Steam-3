using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PanelNumerico : MonoBehaviour
{
    public GameObject panel;
    public TextMeshProUGUI textmesh;
    public GameObject error;
    public static PanelNumerico instance;
    [HideInInspector] public GameObject door;
    [HideInInspector] public int textCorrect;
    // Start is called before the first frame update
    private void Awake()
    {
        instance = this;
    }

    public void InputNumber(string number)
    {
        textmesh.text = $"{textmesh.text}{ number}";
    }

    public void Verify()
    {
        if (textCorrect.ToString() == textmesh.text) // si es verdadero
        {
            door.GetComponent<Door>().Completed();
            panel.SetActive(false);
        }
        else
        {
            error.SetActive(false);
            error.SetActive(true);
            textmesh.text = "";
        }
    }

    public void Closed()
    {
        panel.SetActive(false);
    }
    public void Open(GameObject door,int text)
    {
        this.textCorrect = text;
        textmesh.text = "";
        this.door = door;
        error.SetActive(false);
        panel.SetActive(true);
    }
}
