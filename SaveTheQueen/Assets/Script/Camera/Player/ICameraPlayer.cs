using Aniz.Cam.Info;

namespace Aniz.Cam.Player
{
    public enum E_CameraPlayer
    {
        AtQuakeEffect = 0,
        EyeQuakeEffect,
        EyeArcRound,
        Max
    };
    public interface ICameraPlayer
    {
        E_CameraPlayer CameraPlayer { get; }
        void Update(ref CameraUpdateInfo cameraUpdateInfo, float deltaTime);
    }
}