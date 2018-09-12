using System.Collections.Generic;
using table.db;
using UnityEngine;

namespace Aniz.Data
{
    public class StoryData : BinaryScriptData
    {
        private DB_EventBoardList m_eventBoardList = new DB_EventBoardList();
        private DB_StoreBoardList m_storeBoardList = new DB_StoreBoardList();
        private Dictionary<int, List<DB_StoreBoard>>  m_dicStory = new Dictionary<int, List<DB_StoreBoard>>();

        public override void Release()
        {
        }

        public List<DB_StoreBoard> GetScene(int scene)
        {
            if (m_dicStory.ContainsKey(scene))
            {
                return m_dicStory[scene];
            }
            return null;
        }

        protected override bool ReadData(BinaryDecoder decoder)
        {

            if (!m_eventBoardList.Decode(decoder))
            {
                Debug.LogError("[StoryData] m_eventBoardList decoding failed");
                return false;
            }
            if (!m_storeBoardList.Decode(decoder))
            {
                Debug.LogError("[StoryData] m_storeBoardList decoding failed");
                return false;
            }

            MakeStoryTable(m_storeBoardList.chapter1);

            return true;
        }

        private void MakeStoryTable(List< DB_StoreBoard > lsStoreBoards)
        {
            for (int i = 0; i < lsStoreBoards.Count; i++)
            {
                if (m_dicStory.ContainsKey(lsStoreBoards[i].scene))
                {
                    m_dicStory[lsStoreBoards[i].scene].Add(lsStoreBoards[i]);
                }
                else
                {
                    List<DB_StoreBoard> lstList = new List<DB_StoreBoard>();
                    lstList.Add(lsStoreBoards[i]);
                    m_dicStory.Add(lsStoreBoards[i].scene, lstList);
                }
            }
            
        }
    }
}