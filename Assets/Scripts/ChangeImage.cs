using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ChangeImage : MonoBehaviour
{
    public Image image;
    public List<Sprite> sprites = new List<Sprite>();
    public GameObject Canvas;
    public TMP_Text textmesh;
    public List<string> text;
    
    public void Change(int index)
    {
        Canvas.SetActive(true);
        image.sprite = sprites[index];
        textmesh.text = text[index];
    }
}
