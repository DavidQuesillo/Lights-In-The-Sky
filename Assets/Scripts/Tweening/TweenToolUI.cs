using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class TweenToolUI : MonoBehaviour
{
    public bool translate, scale, rotate;
    [SerializeField] private RectTransform rect;
    [SerializeField] private Vector2 finalPos, finalScale, finalRotate;
    [SerializeField] private float duration;
    [SerializeField] private Ease easeTypeT, easeTypeS, easeTypeR;

    // Start is called before the first frame update
    void Start()
    {
        if (translate)
        {
            rect.DOAnchorPos(finalPos, 1f).SetEase(easeTypeT);
        }
        if (rotate)
        {
            rect.DORotate(finalRotate, 1f, RotateMode.FastBeyond360).SetEase(easeTypeR);
        }
        if (scale)
        {
            rect.DOScale(finalScale, duration).SetEase(easeTypeS);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
