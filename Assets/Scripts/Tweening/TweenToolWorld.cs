using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class TweenToolWorld : MonoBehaviour
{
    public bool translate, scale, rotate;
    [SerializeField] private Transform tr;
    [SerializeField] private bool looping;
    [SerializeField] private LoopType mLoop, rLoop, sLoop;
    [SerializeField] private float duration;
    [SerializeField] private Vector3 finalPos, finalScale, finalRotate;
    [SerializeField] private Ease easeTypeT, easeTypeS, easeTypeR;
    private Tween mTween;
    private Tween rTween;
    private Tween sTween;

    // Start is called before the first frame update
    void Start()
    {

        
    }

    private void OnEnable()
    {
        if (translate)
        {
            if (looping)
            {
                mTween = tr.DOMove(finalPos, duration);
                mTween.SetLoops(-1, mLoop);
                mTween.Play();
            }
            else
            {
                //rect.DOAnchorPos(finalPos, 1f).SetEase(easeTypeT);
                tr.DOMove(finalPos, duration).SetEase(easeTypeT);
            }
        }
        if (rotate)
        {
            if (looping)
            {
                rTween = tr.DOLocalRotate(finalRotate, duration, RotateMode.FastBeyond360).SetEase(easeTypeR); //tr.DORotate(finalRotate, duration, RotateMode.FastBeyond360);

                rTween.SetLoops(-1, rLoop);
                rTween.Play();
            }
            else
            {
                //rect.DORotate(finalRotate, 1f, RotateMode.FastBeyond360).SetEase(easeTypeR);
                tr.DORotate(finalRotate, duration).SetEase(easeTypeR);
            }
        }
        if (scale)
        {
            if (looping)
            {

            }
            else
            {
                //rect.DOScale(finalScale, duration).SetEase(easeTypeS);

                tr.DOScale(finalScale, duration).SetEase(easeTypeS);
            }
        }
    }

    private void OnDisable()
    {
        mTween.Kill();
        rTween.Kill();
        sTween.Kill();
        tr.localRotation = Quaternion.identity;
        //Debug.Log("disabled");
    }

    // Update is called once per frame
    void Update()
    {

    }
}
