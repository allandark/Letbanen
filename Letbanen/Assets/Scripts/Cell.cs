using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cell : MonoBehaviour
{
    public int posX, posY;

    public Texture texture;

    private MeshRenderer meshRenderer;

    private void Awake()
    {
        meshRenderer = GetComponent<MeshRenderer>();

    }

    public void SetTexture(Texture tex)
    {
        meshRenderer.material.SetTexture("_MainTex", tex);
    }
}


public enum CellType
{
    None,
    EndStop,
    Road

}