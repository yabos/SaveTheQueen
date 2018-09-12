using System.Collections.Generic;
using Aniz.Cam;
using Aniz.Cam.Quake;
using Aniz.Cam.Info;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    private CameraImpl m_cameraImpl;
    public CameraImpl Impl
    {
        get { return m_cameraImpl; }
        set { m_cameraImpl = value; }
    }

    public List<AtQuakeInfo> AtCameraDatas = new List<AtQuakeInfo>();
    public List<EyeQuakeInfo> EyeCameraDatas = new List<EyeQuakeInfo>();

    public string EffectName;
    public int EffectID;
    public bool UserUsed = false;
    public float LifeTime = 1.0f;
    public int LoopCount = 0;
    public float FadeTime = 0;
    public float Power = 0.1f;
    public float AtMaxRange = 1;
    public Vector3 Offset = Vector3.zero;
    public Vector3 Direction = Vector3.zero;

    public EyeQuakeInfo.Type eType = EyeQuakeInfo.Type.Increase;
    public int LoadCount = 30;
    public uint BlendWidth = 3;
    public int StepCount = 2;
    public float EyeMaxRange = 1;
    public float TimeLength = 0.05f;
    public bool RandState = false;
    public float RandLength = 30;
    public float EyeLifeTime = 1.0f;

    private AtQuakeUnit m_atQuakeUnit = new AtQuakeUnit();

    public void EyeShake(EyeQuakeInfo data)
    {
        Global.CameraMgr.Impl.EyeQuake.RemoveQuakeUnit();

        EyeQuakeUnit.Stock stock = new EyeQuakeUnit.Stock(data);
        Global.CameraMgr.Impl.EyeQuake.SetQuakeUnit(stock);
    }

    public void AtShake(AtQuakeInfo data)
    {
        m_atQuakeUnit.Initialize(data);
        m_atQuakeUnit.Play((Offset));
    }

    public void EyeShake()
    {
        Global.CameraMgr.Impl.EyeQuake.RemoveQuakeUnit();

        EyeQuakeInfo data = new EyeQuakeInfo();
        data.eType = eType;
        data.LoadCount = LoadCount;
        data.BlendWidth = BlendWidth;
        data.StepCount = StepCount;
        data.TimeLength = TimeLength;
        data.RandState = RandState;
        data.MaxRange = EyeMaxRange;
        data.RandLength = RandLength;

        EyeQuakeUnit.Stock stock = new EyeQuakeUnit.Stock(data);
        Global.CameraMgr.Impl.EyeQuake.SetQuakeUnit(stock);
    }

    public void AtShake()
    {
        AtQuakeInfo data = new AtQuakeInfo();
        data.UserUsed = UserUsed;
        data.FadeTime = FadeTime;
        data.Power = Power;
        data.MaxRange = AtMaxRange;
        data.LifeTime = LifeTime;
        data.Direction = Direction;
        data.LoopCount = LoopCount;

        m_atQuakeUnit.Initialize(data);
        m_atQuakeUnit.Play((Offset));

    }

    void Update()
    {
        if (m_atQuakeUnit != null)
        {
            if (m_atQuakeUnit.IsFinish == false)
                m_atQuakeUnit.Process(TimeManager.deltaTime);
        }
    }

    public void StopShaking()
    {

    }
}