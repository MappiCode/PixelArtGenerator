using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ExportButton : MonoBehaviour, IPointerDownHandler
{
    [SerializeField]
    private Image spriteImage;

    public void OnPointerDown(PointerEventData eventData)
    {
        ImageExporter.ExortSprite(spriteImage.sprite);
    }
}
