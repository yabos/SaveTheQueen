using System.Collections.Generic;
using Aniz.Data.Map;
using Aniz.Graph;
using Lib.Event;
using Lib.Pattern;

namespace Aniz.Data
{

    public class DataManager : Singleton<DataManager>
    {
        private List<IScriptData> m_scriptDataList = new List<IScriptData>();
        private bool m_initLoad = false;


        public void LoadData()
        {
            if (m_initLoad)
                return;
            IScriptData scriptData = null;
            
            scriptData = new CharacterData();
            scriptData.Load("Npc_Database");
            m_scriptDataList.Add(scriptData);

            scriptData = new ResData();
            scriptData.Load("Resource_Database");
            m_scriptDataList.Add(scriptData);

            scriptData = new StoryData();
            scriptData.Load("StoreBoards_Database");
            m_scriptDataList.Add(scriptData);

            scriptData = new TextData();
            scriptData.Load("UIText_Database");
            m_scriptDataList.Add(scriptData);
            m_initLoad = true;
        }


        public T GetScriptData<T>() where T : class, IScriptData
        {
            IScriptData scriptdata = null;

            for (int i = 0; i < m_scriptDataList.Count; i++)
            {
                if (m_scriptDataList[i] is T)
                {
                    return (T)m_scriptDataList[i];
                }
            }

            return null;
        }
    }
}