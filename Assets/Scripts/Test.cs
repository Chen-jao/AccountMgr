using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class Test : MonoBehaviour
{

    public GameObject m_Sphere;
    public Transform m_Root;

    public LineRenderer lineRenderer;

    private List<Vector3> points = new List<Vector3>();

    [Range(0, 1F), Header("衰减因子")]
    public float FactorDecay = 0.2f;
    [Range(0, 1F), Header("调和因子")]
    public float FactorHarmonic = 0.1f;
    [Header("基础数值ATK")]
    public float BaseAtk = 100;
    [Range(0, 1000F), Header("基础数值POWER")]
    public float BasePower = 1000;
    [Range(0, 1000F), Header("基础数值STRENGTH")]
    public float BaseStrength = 100;
    [Range(0, 1F), Header("PATK基准系数")]
    public float FactorBenchmarkPhysicATKFixed = 0.5f;
    [Range(-1F, 1F), Header("PATK可变系数")]
    public float FactorHarmonicPhysicATKVari = 0.1f;


    public float BaseDef = 100;
    [Range(0, 1000F)]
    public float BaseMentality = 1000;
    [Range(0, 1000F)]
    public float BasePerception = 100;
    public float FactorBenchmarkPhysicDEFFixed = 0.23f;
    public float FactorHarmonicPhysicDEFVari = 0.1F;

    private void Simulate()
    {
        for(var i = 0; i < 1000; i++)
        {
            BaseMentality = i;
            points.Add(new Vector3(i, Result_MagicATK, 0));
            //var go = Instantiate(m_Sphere, m_Root).transform;
            //go.position = new Vector3(i, Result_MagicATK, go.position.z);
        }

        lineRenderer.positionCount = points.Count;//设置构成线条的点的数量
        lineRenderer.SetPositions(points.ToArray());
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Simulate();
            //Debug.Log(Result_MagicATK);
        }
    }


    /// <summary>
    /// Physic Result Caculate
    /// </summary>
    private float Result_PhysicATK
    {
        get
        {
            // 计算调和率
            var harFactor = 1 - Mathf.Sqrt(Mathf.Abs(BasePower - BaseStrength)) / Mathf.Sqrt(Mathf.Abs(BasePower + BaseStrength)) *
                BasePower > BaseStrength ? (BasePower - BaseStrength) / BasePower : 1;
            // 计算调和值
            var harValue = harFactor * (1 - (FactorDecay - FactorHarmonic));
            // 计算调和结果
            var harResult = (Mathf.Sqrt(Mathf.Abs(BasePower)) / 10 + Mathf.Sqrt(BaseStrength) / 100) * harValue * (FactorBenchmarkPhysicATKFixed + FactorHarmonicPhysicATKVari);
            // 结果
            return BaseAtk * harResult;
        }
    }

    private float Result_PhysicDEF
    {
        get
        {
            var harFactor = 1 - Mathf.Sqrt(Mathf.Abs(BaseStrength - BasePower)) / Mathf.Sqrt(Mathf.Abs(BaseStrength + BasePower)) *
                BaseStrength > BasePower ? (BaseStrength - BasePower) / BaseStrength : 1;
            // 计算调和值
            var harValue = harFactor * (1 - (FactorDecay - FactorHarmonic));
            // 计算调和结果
            var harResult = (Mathf.Sqrt(Mathf.Abs(BaseStrength)) / 10 + Mathf.Sqrt(BasePower) / 100) * harValue * (FactorBenchmarkPhysicDEFFixed + FactorHarmonicPhysicDEFVari);
            // 结果
            return BaseDef * harResult;
        }
    }

    /// <summary>
    /// Magic Result Caculate
    /// </summary>
    private float Result_MagicATK
    {
        get
        {
            // 计算调和率
            var harFactor = 1 - Mathf.Sqrt(Mathf.Abs(BaseMentality - BasePerception)) / Mathf.Sqrt(Mathf.Abs(BaseMentality + BasePerception)) * 
                BaseMentality > BasePerception ? (BaseMentality - BasePerception) / BaseMentality : 1;
            // 计算调和值
            var harValue = harFactor * (1 - (FactorDecay - FactorHarmonic));
            // 计算调和结果
            var harResult = (Mathf.Sqrt(Mathf.Abs(BaseMentality)) / 10 + Mathf.Sqrt(BasePerception) / 100) * harValue * (FactorBenchmarkPhysicDEFFixed + FactorHarmonicPhysicDEFVari);
            // 结果
            return BaseAtk * harResult;
        }
    }
}







