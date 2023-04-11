using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class TweenObjWorld : MonoBehaviour
{
    [SerializeField] private TweenStep[] sq = new TweenStep[0];
    [SerializeField] private bool playOnAwake = true;
    private Sequence stageSequence;

    // Start is called before the first frame update
    void Start()
    {
        DOTween.Init();
        stageSequence = DOTween.Sequence();

        for (int i = 0; i < sq.Length; i++)
        {
            if (sq[i].moves)
            {
                sq[i].mStep = transform.DOMove(sq[i].finalLocation, sq[i].duration);
                sq[i].mStep.SetEase(sq[i].mEase);
                sq[i].mStep.Pause();
                stageSequence.Append(sq[i].mStep);
                if (sq[i].rotates)
                {
                    sq[i].rStep = transform.DORotate(sq[i].finalRotation, sq[i].duration);
                    sq[i].rStep.SetEase(sq[i].rEase);
                    sq[i].rStep.Pause();
                    stageSequence.Join(sq[i].rStep);
                }
                if (sq[i].scales)
                {
                    sq[i].sStep = transform.DOScale(sq[i].finalScale, sq[i].duration);
                    sq[i].sStep.SetEase(sq[i].sEase);
                    sq[i].rStep.Pause();
                    stageSequence.Join(sq[i].sStep);
                }
            }
            if (playOnAwake)
            {
                stageSequence.Play();
            }
        }
    }

    public void PlaySq()
    {
        stageSequence.Play();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

[System.Serializable]
public class TweenStep
{
    public Tween mStep, rStep, sStep;
    public bool moves, rotates, scales;
    [Header("Ease")]
    public Ease mEase, rEase, sEase;
    public float duration;
    public Vector3 finalLocation, finalRotation, finalScale = Vector3.one;
}
