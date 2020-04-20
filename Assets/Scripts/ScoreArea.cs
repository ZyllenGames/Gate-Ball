using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreArea : MonoBehaviour
{
    public GameObject SparkingEffect;
    public GameObject PointLightEffect;
    public GameObject ScroreEffectPrefab;
    public float RewardSeconds;

    Color m_Color;
    public Color {get{return m_Color;}; set {m_Color = value; }};

    bool m_HasPassed = false;

    public static System.Action OnPassed;

    private void OnTriggerExit(Collider other)
    {
        if(other.tag == "Ball" && !m_HasPassed && other.gameObject.GetComponent<Ball>().Color == m_Color)
        {
            m_HasPassed = true;
            TimeManager.Instance.AddTime(RewardSeconds);

            GameObject newScoreEffect = Instantiate(ScroreEffectPrefab, transform.position, transform.rotation);
            Destroy(newScoreEffect, 2);

            SparkingEffect.SetActive(false);
            PointLightEffect.SetActive(false);
            GetComponent<MeshRenderer>().enabled = false;

            OnPassed();

            AudioManager.Instance.Play2DSound("GatePassed");
        }
    }
}
