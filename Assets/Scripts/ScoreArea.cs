using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreArea : MonoBehaviour
{
    public GameObject SparkingEffect;
    public GameObject PointLightEffect;
    public GameObject ScoreEffectPrefab;
    public float RewardSeconds;

    Color m_Color;

    bool m_HasPassed = false;

    public static System.Action OnPassed;

    public Color Color { get => m_Color; set => m_Color = value; }

    public void InitializeColor(Color color)
    {
        m_Color = color;
        GetComponent<MeshRenderer>().material.color = color;
        SparkingEffect.GetComponent<ParticleSystem>().startColor = color;
        PointLightEffect.GetComponent<Light>().color = color;
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.tag == "Ball")
        {
            if(!m_HasPassed)
            {
                if(other.GetComponent<Ball>().Color == m_Color)
                {
                    m_HasPassed = true;
                    TimeManager.Instance.AddTime(RewardSeconds);

                    GameObject newScoreEffect = Instantiate(ScoreEffectPrefab, transform.position, transform.rotation);
                    Destroy(newScoreEffect, 2);

                    SparkingEffect.SetActive(false);
                    PointLightEffect.SetActive(false);
                    GetComponent<MeshRenderer>().enabled = false;

                    OnPassed();

                    AudioManager.Instance.Play2DSound("GatePassed");
                }
            }
        }
    }
}
