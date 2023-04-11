using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class TweenSequenceWorld : MonoBehaviour
{
    private Tween moveTween;
    private Tween scaleTween;

    [SerializeField] private MoveSteps[] stages = new MoveSteps[0];

    //[SerializeField] private float duration;
    [SerializeField] private int step = 0;
    //[SerializeField] private float speed;
    //[SerializeField] private Vector3[] locations = new Vector3[0];
    [SerializeField] private bool[] stepsDone = new bool[0];


    private void Start()
    {
        //moveTween = transform.DOMove(locations[0], duration);
        //moveTween.Play();

        for (int i = 0; i < stages.Length; i++)
        {
            stages[i].mTween = transform.DOMove(stages[i].destination, stages[i].duration);
            stages[i].mTween.SetEase(stages[i].easeType);
            stages[i].mTween.Pause();

            stages[i].rTween = transform.DORotate(stages[i].finalRotation, stages[i].duration, RotateMode.FastBeyond360);
            stages[i].rTween.SetEase(stages[i].easeType);

            stages[i].sTween = transform.DOScale(stages[i].targetScale, stages[i].duration);
            stages[i].sTween.SetEase(stages[i].easeType);
        }

        //StartCoroutine(runPath());

        StartCoroutine(pathCoroutine());
    }

    private void MoveToNextPoint()
    {
        //transform.position = Vector3.MoveTowards(transform.position, locations[step], speed * Time.deltaTime);

        if (!stages[step].mTween.IsPlaying())
        {
            stages[step].mTween.Play();
        }

        if (stages[step].mTween.IsComplete())
        {
            stepsDone[step] = true;
            //stages[step].mTween.Kill();
            step += 1;
            Debug.Log("tween done");
        }
    }

    /*private IEnumerator runPath()
    {
        while (stepsDone[stepsDone.Length - 1] == false)
        {
            //while (moveTween.IsComplete() == false)
            while (stepsDone[step] == false)
            {
                MoveToNextPoint();
                if (transform.position == locations[step])
                {
                    stepsDone[step] = true;
                    step += 1;
                }
                yield return null;
            }

            
            yield return null;
        }
        yield return null;
    }
    */

    private IEnumerator pathCoroutine()
    {
        while (stepsDone[stepsDone.Length - 1] == false)
        {
            //while (moveTween.IsComplete() == false)
            while (stepsDone[step] == false)
            {
                MoveToNextPoint();

                yield return null;
            }


            yield return null;
        }
        yield return null;
    }
}

[System.Serializable]
public class MoveSteps
{
    public Tween mTween;
    public Tween rTween;
    public Tween sTween;
    public float duration;
    public Vector3 destination;
    public Vector3 targetScale;
    public Vector3 finalRotation;
    public Ease easeType;
}