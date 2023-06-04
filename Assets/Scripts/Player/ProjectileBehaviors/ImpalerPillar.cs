using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.VFX;

public class ImpalerPillar : PlayerBullet
{
    [SerializeField] private GameObject pillarMesh;
    [SerializeField] private Vector3 pillarStartPos; //the position of the pillar mesh obj before it emerges
    [SerializeField] private float pillarFinalZ; //the local Z axis of the mesh after its fully emerged
    //[SerializeField] private float lifetime = 0.4f;
    //private float lifeTimer;
    [SerializeField] private float emergeDuration = 0.01f;
    [SerializeField] private VisualEffect crumblingFX;

    private void OnEnable()
    {
        lifeTimer = lifetime;
        StopCoroutine(SelfCollapse());
        StartCoroutine(SelfCollapse());
        //pillarMesh.transform.localposition = pillarStartPos;
        //pillarMesh.transform.rotation = Quaternion.Euler(Random.Range(-180f, 180f), 0f, 0f); //may try to randomly rotate the pillars for presentation
        pillarMesh.SetActive(true);
        pillarMesh.transform.DOLocalMoveZ(pillarFinalZ, emergeDuration);
    }

    private IEnumerator SelfCollapse()
    {
        crumblingFX.Play();
        lifeTimer = lifetime;
        while (lifeTimer > 0f)
        {
            lifeTimer -= Time.deltaTime;
            //print("collapse looped once");
            //print(lifeTimer.ToString());
            yield return null;
        }
        //Debug.Log("exited pillar while");
        pillarMesh.SetActive(false);
        pillarMesh.transform.localPosition = pillarStartPos;
        crumblingFX.Stop();
        gameObject.SetActive(false);
    }

    protected override void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            if (other.GetComponent<EnemyBase>().GetHP() <= damage)
            {
                //GameObject particle = IceDagParticlesPool.Instance.RequestPoolObject();

                //GameObject killParticle = IceDagKillParticlesPool.Instance.RequestPoolObject();
                //killParticle.transform.SetPositionAndRotation(transform.position, Quaternion.LookRotation(-transform.forward));
                //killParticle.GetComponent<VisualEffect>()?.Play();
                //killParticle.GetComponent<VisualEffect>().Stop()
                //killParticle.GetComponent<AudioSource>()?.Play();
                //Debug.Log(other.name);
                //gameObject.SetActive(false);
                //return;
            }
        }

        //GameObject particle = IceDagParticlesPool.Instance.RequestPoolObject();
        //particle.transform.SetPositionAndRotation(transform.position, Quaternion.LookRotation(-transform.forward));
        //particle.GetComponent<AudioSource>()?.Play();
        //Debug.Log(other.name);
        //gameObject.SetActive(false);
    }
}
