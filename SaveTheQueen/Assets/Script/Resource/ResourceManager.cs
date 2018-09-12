using System.Collections;
using System.Collections.Generic;
using Aniz.Resource.Unit;
using Lib.Event;
using Lib.Pattern;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Aniz.Resource
{


    public enum eResourceType
    {
        Prefab,
        Script,
        Sprite,
        SpriteAsset,
        SpritePacker,
        Sound,
        Texture,
        Scriptable,
        UI,
        Max,
    }

    public class ResourceManager : GlobalManagerBase<ManagerSettingBase>
    {
        private Dictionary<int, IResource>[] m_dicResource = new Dictionary<int, IResource>[(int)eResourceType.Max];
        private PathManager m_pathManager = new PathManager();

        public ResourceManager()
        {
            for (int i = 0; i < (int)eResourceType.Max; i++)
            {
                m_dicResource[i] = new Dictionary<int, IResource>();
            }
        }


        #region Events
        public override void OnAppStart(ManagerSettingBase managerSetting)
        {
            m_name = typeof(ResourceManager).ToString();

            if (string.IsNullOrEmpty(m_name))
            {
                throw new System.Exception("manager name is empty");
            }

            m_setting = managerSetting as ManagerSettingBase;

            if (null == m_setting)
            {
                throw new System.Exception("manager setting is null");
            }

            // Set backgroundLoadingPriority as High.
            Application.backgroundLoadingPriority = ThreadPriority.High;

            // Set Caching as Decompressed State.
            Caching.compressionEnabled = false;

            // Set Maximum Available Disk Space as "1 GB(1024 MB)".
            Caching.maximumAvailableDiskSpace = 1024 * 1024 * 1024;

            Init();
        }

        public override void OnAppEnd()
        {
            Release();

            DestroyRootObject();

            if (m_setting != null)
            {
                GameObjectFactory.DestroyComponent(m_setting);
                m_setting = null;
            }
        }

        public override void OnAppFocus(bool focused)
        {

        }

        public override void OnAppPause(bool paused)
        {

        }

        public override void OnPageEnter(string pageName)
        {
        }

        public override IEnumerator OnPageExit()
        {
            yield return new WaitForEndOfFrame();
        }

        #endregion Events


        #region IGraphUpdatable

        public override void BhvOnEnter()
        {

        }

        public override void BhvOnLeave()
        {

        }

        public override void BhvFixedUpdate(float dt)
        {

        }

        public override void BhvLateFixedUpdate(float dt)
        {

        }

        public override void BhvUpdate(float dt)
        {
        }

        public override void BhvLateUpdate(float dt)
        {

        }

        public override bool OnMessage(IMessage message)
        {
            return false;
        }

        #endregion IGraphUpdatable

        public void Init()
        {
            //spriteload
            //             string path = m_pathManager.GetPath(E_Path.Sprite);
            //             Sprite[] textures = Resources.LoadAll<Sprite>(path);
            //
            //             foreach (Sprite sprite in textures)
            //             {
            //                 string resourcename = sprite.name.ToLower();
            //                 if (m_dicSprite.ContainsKey(resourcename) == false)
            //                 {
            //                     m_dicSprite.Add(resourcename, sprite);
            //                 }
            //                 else
            //                 {
            //                     string msg = string.Format("ResourceManager SpriteResource Add Error : {0}", resourcename);
            //                     LogManager.Instance.LogError(msg);
            //                 }
            //             }
        }

        public void Release()
        {
            for (int i = 0; i < (int)eResourceType.Max; i++)
            {
                foreach (IResource resource in m_dicResource[i].Values)
                {
                    resource.Release();
                }
            }
        }

        public void UnLoadResource(eResourceType eType, string name)
        {
            IResource resource = FindResource(eType, name);
            GetDicResource(eType).Remove(resource.GetHashCode());
            resource.UnLoad(true);
        }


        public void UnLoadResource(IResource resource)
        {
            GetDicResource(resource.Type).Remove(resource.GetHashCode());
            resource.UnLoad(true);
        }

        public void UnLoadResource(eResourceType resourceType, bool isUnloadUnusedAssets)
        {
            for (int i = 0; i < (int)eResourceType.Max; i++)
            {
                if (resourceType == (eResourceType)i)
                {
                    foreach (IResource resource in m_dicResource[i].Values)
                    {
                        resource.Release();
                    }
                    m_dicResource[i].Clear();
                }
            }

            if (isUnloadUnusedAssets)
            {
                Resources.UnloadUnusedAssets();
            }
        }

        public void UnLoad()
        {
            for (int i = 0; i < (int)eResourceType.Max; i++)
            {
                foreach (IResource resource in m_dicResource[i].Values)
                {
                    resource.Release();
                }
                m_dicResource[i].Clear();
            }

            Resources.UnloadUnusedAssets();
        }

        public void DestoryResource(eResourceType eType, string name)
        {
            IResource resource = FindResource(eType, name);
            GetDicResource(eType).Remove(resource.GetHashCode());
            resource.Release();
        }

        public void DestoryResource(IResource resource)
        {
            GetDicResource(resource.Type).Remove(resource.GetHashCode());
            resource.Release();
        }

        private Dictionary<int, IResource> GetDicResource(eResourceType eType)
        {
            return m_dicResource[(int)eType];
        }


        public SpriteResource CreateSpriteResource(string name, ePath ePath, bool assetbundle)
        {
            IResource res = FindResource(eResourceType.Sprite, name.ToLower());
            if (res != null)
            {
                return res as SpriteResource;
            }
            res = MakeSpriteResource(eResourceType.Sprite, ePath, name.ToLower());

            return (SpriteResource)res;
        }

        public SpriteAssetResource CreateSpriteAssetResource(ePath ePath, string name, bool assetbundle)
        {
            IResource res = null;
            res = FindResource(eResourceType.SpriteAsset, name.ToLower());
            if (res != null)
            {
                return (SpriteAssetResource)res;
            }
            return (SpriteAssetResource)MakeSpriteResource(eResourceType.SpriteAsset, ePath, name.ToLower());
        }


        public PrefabResource CreateGameResource(string name, ePath ePath)
        {
            IResource res = null;
            //             if (ePath == E_Path.Sprite)
            //             {
            //                 return CreateSpriteResource(name, ePath);
            //             }

            res = FindResource(eResourceType.Prefab, name);
            if (res != null)
            {
                return (PrefabResource)res;
            }

            return (PrefabResource)CreateResource(eResourceType.Prefab, name, ePath);
        }

        public SoundResource CreateSFXResource(string name, eResourceType resourceType, ePath ePath)
        {
            IResource res = null;
            res = FindResource(resourceType, name);
            if (res != null)
            {
                return (SoundResource)res;
            }

            return (SoundResource)CreateResource(resourceType, name, ePath);
        }

        public ScriptResource CreateScriptResource(string name, ePath ePath)
        {
            IResource res = null;
            res = FindResource(eResourceType.Script, name);
            if (res != null)
            {
                return (ScriptResource)res;
            }

            return (ScriptResource)CreateResource(eResourceType.Script, name, ePath);
        }


        public SoundResource CreateSoundResource(string name, ePath ePath)
        {
            IResource res = null;
            res = FindResource(eResourceType.Sound, name);
            if (res != null)
            {
                return (SoundResource)res;
            }

            return (SoundResource)CreateResource(eResourceType.Sound, name, ePath);
        }

        public PrefabResource CreateUIResource(string prefabName, ePath pathType, bool dontDestroyOnLoad)
        {
            IResource res = FindResource(eResourceType.UI, prefabName);
            if (res != null)
            {
                return (PrefabResource)res;
            }

            return (PrefabResource)CreateResource(eResourceType.UI, prefabName, pathType);
        }

        public PrefabResource CreatePrefabResource(string prefabName, ePath pathType)
        {
            IResource res = FindResource(eResourceType.Prefab, prefabName);
            if (res != null)
            {
                return (PrefabResource)res;
            }

            return (PrefabResource)CreateResource(eResourceType.Prefab, prefabName, pathType);
        }

        public IResource FindResource(string name)
        {
            for (int i = 0; i < (int)eResourceType.Max; i++)
            {
                IResource res = FindResource((eResourceType)i, name);
                if (res != null)
                    return res;
            }
            return null;
        }

        public IResource FindResource(eResourceType eType, string name)
        {
            Dictionary<int, IResource> dicresource = GetDicResource(eType);
            int hashcode = name.GetHashCode();
            if (dicresource.ContainsKey(hashcode))
            {
                return dicresource[hashcode];
            }
            return null;
        }

        private IResource MakeSpriteResource(eResourceType eType, ePath ePath, string name)
        {
            string path = StringUtil.Format("{0}/{1}", m_pathManager.GetPath(ePath), name);

            Sprite[] allresource = null;
            Object obj = null;
            Object autotile = null;
            SpritePackerAsset spritePackerAsset = null;
            if (ePath == Resource.ePath.MapAutoTile)
            {
                obj = Resources.Load(path);
                string autotilepath = m_pathManager.GetPath(ePath) + "AutoTile";
                autotile = Resources.Load(autotilepath);
                if (obj is SpritePackerAsset)
                {
                    spritePackerAsset = (SpritePackerAsset)obj;
                    allresource = spritePackerAsset.AllSprites;
                }
            }
            if (ePath == Resource.ePath.UISprite)
            {
                obj = Resources.Load(path);
            }
            else if (ePath == Resource.ePath.MapAsset || ePath == Resource.ePath.MapAutoAsset || ePath == Resource.ePath.MapActorAsset)
            {
                obj = Resources.Load(path);
                if (obj is SpritePackerAsset)
                {
                    spritePackerAsset = (SpritePackerAsset)obj;
                    allresource = spritePackerAsset.AllSprites;
                }
            }
            else
            {
                allresource = Resources.LoadAll<Sprite>(path);
            }

            IResource resource = null;
            if (eType == eResourceType.SpriteAsset)
            {

                if (obj == null)
                {
                    Debug.LogError("TileSprite MakeSpriteResource name error" + name);
                    return null;
                }

                resource = new SpritePackerResource(spritePackerAsset, spritePackerAsset.AllSprites, false);
            }
            else if (eType == eResourceType.Sprite)
            {
                if (obj == null)
                {
                    Debug.LogError("UISprite MakeSpriteResource name error" + name);
                    return null;
                }

                resource = new SpriteResource(obj, false);
            }
            else
            {

                if (allresource == null)
                {
                    Debug.LogError("SpriteResource name error" + name);
                    return null;
                }

                resource = new SpritePackerResource(spritePackerAsset, allresource, false);
            }
            string[] split = path.Split('/');
            string resourcename = split[split.Length - 1];
            resource.InitLoad(resourcename, path);
            Dictionary<int, IResource> dicRes = GetDicResource(eType);
            if (dicRes.ContainsKey(resource.GetHashCode()))
            {
                Debug.LogError("MakeSpriteResource name error" + name);
            }
            else
            {
                dicRes.Add(resource.GetHashCode(), resource);
            }
            return resource;

        }


        public PrefabResource CreatePrefabResource(GameObject prefab)
        {
            IResource res = null;
            res = FindResource(eResourceType.Prefab, prefab.name);
            if (res != null)
            {
                return (PrefabResource)res;
            }

            return (PrefabResource)CreateResource(eResourceType.Prefab, prefab.name, prefab.name, prefab, false);
        }

        private IResource CreateResource(eResourceType eType, string name, ePath ePath)
        {
            // 사운드는 넘어오는 name이 실제 파일 이름이 아니라 내부 이름이므로 변경 프로세스가 필요
            //             if (eType == E_ResourceType.Sound)
            //             {
            //                 SoundInfo si;
            //                 if (DataManager.Instance.GetScriptData<SoundData>(E_GameScriptData.Sound).GetSoundInfo(name, out si))
            //                 {
            //                     name = si.Filename;
            //                 }
            //             }

            bool isAssetBundle = false;
            string path = StringUtil.Format("{0}/{1}", m_pathManager.GetPath(ePath), name);
            //string path = m_pathManager.GetPath(ePath) + name;
            Object objresource = Resources.Load(path);

            if (objresource == null)
            {
                return null;
            }

            return CreateResource(eType, name, path, objresource, isAssetBundle);
        }


        private IResource CreateResource(eResourceType eType, string name, string assetpath, Object objresource, bool isAssetBundle)
        {
            IResource resource = null;
            switch (eType)
            {
                //case E_ResourceType.Actor:
                case eResourceType.UI:
                case eResourceType.Prefab:
                    resource = new PrefabResource(objresource, eType, isAssetBundle);
                    break;
                //                 case E_ResourceType.UnitySprite:
                //                     resource = new SpriteResource(objresource);
                //                     break;
                case eResourceType.Script:
                    resource = new ScriptResource(objresource, isAssetBundle);
                    break;
                case eResourceType.SpriteAsset:
                    SpritePackerAsset spritePackerAsset = (SpritePackerAsset)objresource;
                    resource = new SpriteAssetResource(spritePackerAsset.AllSprites, isAssetBundle);
                    break;
                case eResourceType.Scriptable:
                    resource = new ScriptableResource(objresource, isAssetBundle);
                    break;

                case eResourceType.Texture:
                    resource = new TextureResouce(objresource, isAssetBundle);
                    break;

                case eResourceType.Sound:
                    resource = new SoundResource(objresource, isAssetBundle);
                    break;

            }
            resource.InitLoad(name, assetpath);

            Dictionary<int, IResource> dicRes = GetDicResource(eType);
            if (dicRes.ContainsKey(resource.GetHashCode()))
            {
                //           LogManager.GetInstance().LogDebug("CreateResource name error" + name);
            }
            else
            {
                dicRes.Add(resource.GetHashCode(), resource);
            }

            return resource;
        }



        #region Asysc Methods


        public IEnumerator LoadSceneAsync(string sceneName, System.Action<float> OnLoadingProgressAction, System.Action OnSceneLoadingCompleteAction)
        {

            AsyncOperation asyncOperation = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(sceneName, UnityEngine.SceneManagement.LoadSceneMode.Single);
            if (asyncOperation == null)
            {
                throw new System.Exception(StringUtil.Format("Failed to LoadSceneAsync({0}.unity, asyncOperation == null )", sceneName));
            }

            asyncOperation.allowSceneActivation = false;

            while (asyncOperation.isDone == false)
            {
                if (asyncOperation.progress < 0.9f)
                {
                    if (OnLoadingProgressAction != null)
                    {
                        OnLoadingProgressAction(asyncOperation.progress);
                    }
                }
                else if (false == asyncOperation.allowSceneActivation)
                {
                    asyncOperation.allowSceneActivation = true;
                }
                else
                {
                    yield return asyncOperation;
                }
            }

            yield return new WaitForEndOfFrame();

            if (OnLoadingProgressAction != null)
            {
                OnLoadingProgressAction(1.0f);
            }

            if (null != OnSceneLoadingCompleteAction)
            {
                OnSceneLoadingCompleteAction();
            }

            yield return true;
        }


        public IEnumerator CreateResourceAsync(eResourceType resourceType, string resourceName, ePath pathType, System.Action<IResource> action)
        {
            if (string.IsNullOrEmpty(resourceName) == true)
            {
                action(null);
                yield break;
            }

            bool isAssetBundle = false;
            string resourcePath = StringUtil.Format("{0}/{1}", m_pathManager.GetPath(pathType), resourceName);
            UnityEngine.Object resourceData = null;

            ResourceRequest request = Resources.LoadAsync(resourcePath);
            yield return request;
            resourceData = request.asset;

            if (resourceData == null)
            {
                LogError("resource is null: " + resourceType.ToString() + ", " + pathType.ToString() + ", " + resourceName);
                action(null);
                yield break;
            }

            action(CreateResource(resourceType, resourceName, resourcePath, resourceData, isAssetBundle));
        }


        public IEnumerator CreatePrefabResourceAsync(ePath path, string prefabName, System.Action<PrefabResource> action)
        {
            IResource res = FindResource(eResourceType.Prefab, prefabName);
            if (res != null)
            {
                action(res as PrefabResource);
            }
            else
            {
                yield return CreateResourceAsync(eResourceType.Prefab, prefabName, path, result =>
                {
                    action(result as PrefabResource);
                });
            }
        }


        public IEnumerator CreateUIResourceAsync(string prefabName, ePath pathType, bool dontDestroyOnLoad, System.Action<PrefabResource> action)
        {
            IResource res = FindResource(eResourceType.UI, prefabName);
            if (res != null)
            {
                action(res as PrefabResource);
            }
            else
            {
                yield return CreateResourceAsync(eResourceType.UI, prefabName, pathType, result =>
                {
                    action(result as PrefabResource);
                });
            }
        }
        #endregion Asysc Methods
    }
}
