using System.Collections.Generic;

namespace Aniz.Resource
{

    public enum ePath
    {
        Data,
        Game,
        UI,
        UISprite,
        Sound,
        Entity,
        Effect,
        Actor,
        Atlas,

        MapPrefabs,
        MapAutoTile,
        MapAutoAsset,
        MapAsset,
        MapActorAsset,

        VFX,

        SFX,
        SFXAsset,
        SFXMixer,

        Max,
    }

    public class PathManager
    {
        private Dictionary<ePath, string> m_dicPath = new Dictionary<ePath, string>();

        public PathManager()
        {
            m_dicPath.Add(ePath.Game, "Game");
            m_dicPath.Add(ePath.Actor, "Actor");
            m_dicPath.Add(ePath.Data, "DataTable");
            m_dicPath.Add(ePath.UI, "Prefabs/Widget");
            m_dicPath.Add(ePath.UISprite, "Prefabs/Widget/Sprite");
            m_dicPath.Add(ePath.Effect, "Effect");
            m_dicPath.Add(ePath.Sound, "Sound");
            m_dicPath.Add(ePath.Atlas, "Atlas");
            m_dicPath.Add(ePath.MapPrefabs, "Map/Prefabs");
            m_dicPath.Add(ePath.MapAutoTile, "Map/Prefabs/AutoTile");
            m_dicPath.Add(ePath.MapAutoAsset, "Map/Asset/Auto");
            m_dicPath.Add(ePath.MapActorAsset, "Map/Asset/Character");
            m_dicPath.Add(ePath.MapAsset, "Map/Asset");

            m_dicPath.Add(ePath.SFX, "Sounds");
            m_dicPath.Add(ePath.SFXAsset, "Sounds/Asset");
            m_dicPath.Add(ePath.SFXMixer, "Sounds/Asset/AudioSource");
            m_dicPath.Add(ePath.VFX, "VFX");
        }

        public string GetPath(ePath eType)
        {
            return m_dicPath[eType];
        }
    }
}