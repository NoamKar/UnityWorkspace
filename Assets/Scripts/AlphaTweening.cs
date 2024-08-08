using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class AlphaTweening : MonoBehaviour
{
    public float duration;
    Material targetMaterial;

    private void Start()
    {
        MeshRenderer meshRenderer = GetComponent<MeshRenderer>();
        targetMaterial = meshRenderer.material;
    }

    public void SwitchOn()
    {
        targetMaterial.DOKill();
        Color targetColor = targetMaterial.color;
        targetColor.a = 1;
        targetMaterial.DOColor(targetColor, duration);
    }

    public void SwitchOff()
    {
        targetMaterial.DOKill();
        Color targetColor = targetMaterial.color;
        targetColor.a = 0;
        targetMaterial.DOColor(targetColor, duration);
    }
}
