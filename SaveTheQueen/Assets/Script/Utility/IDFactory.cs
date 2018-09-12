
using System;
using Lib.Pattern;

namespace Aniz
{
    public class IDFactory : Singleton<IDFactory>, SingletonDispose
    {
        public enum eIDScope
        {
            Tile = 0,
            Trigger = 1,

            Actor = 2,
            Fx = 3,
            //ITEM = 3,
            UI = 4,

            //MONSTER = 5,
            MAX,
        }

        private IdGenerator[] m_ids = new IdGenerator[(int)eIDScope.MAX];
        private int[] m_initZero = new int[(int)eIDScope.MAX];

        public IDFactory()
        {
            for (int i = 0; i < (int)eIDScope.MAX; i++)
            {
                m_ids[i] = new IdGenerator(10000);
            }
        }
        public void Init()
        {
            //기본 아이디는 무조건 0이상으로 시작해야되기때문에...
            //하나씩 뽑아 놓는다
            for (int i = 0; i < (int)eIDScope.MAX; i++)
            {
                m_initZero[i] = GetId((eIDScope)i);
            }
        }

        public void Release()
        {
            //0을 반납하고 GetIdCount 찍을때 0이 아니면 반납안됨 놈이 있는것임!!!!
            for (int i = 0; i < (int)eIDScope.MAX; i++)
            {
                RemoveId((eIDScope)i, m_initZero[i]);
                int count = GetIdCount((eIDScope)i);
                if (count > 0)
                {
                    UnityEngine.Debug.LogWarning(string.Format("{0} count : {1}", (eIDScope)i, count));
                }
            }
        }

        public void OnUpdate(float deltaTime)
        {
        }

        public int GetId(eIDScope eIdScope)
        {
            return m_ids[(int)eIdScope].GetId();
        }

        public void RemoveId(eIDScope eIdScope, int id)
        {
            m_ids[(int)eIdScope].RemoveId(id);
        }

        public void SetId(eIDScope eIdScope, int id)
        {
            m_ids[(int)eIdScope].SetId(id);
        }

        public int GetIdCount(eIDScope eIdScope)
        {
            return m_ids[(int)eIdScope].GetIdCount();
        }

        public void Clear(eIDScope eIdScope)
        {
            m_ids[(int)eIdScope].Clear();
        }


        public static int GenerateActorID()
        {
            return Instance.GetId(eIDScope.Actor);
        }

        public static int GenerateTriggerID()
        {
            return Instance.GetId(eIDScope.Trigger);
        }

        public static int GenerateTileID()
        {
            return Instance.GetId(eIDScope.Tile);
        }

        public static int GenerateUIID()
        {
            return Instance.GetId(eIDScope.UI);
        }

        //public static int GenerateMonsterID()
        //{
        //	return Instance.GetId(E_IDScope.MONSTER);
        //}



        public static void DeleteActorID(int id)
        {
            Instance.RemoveId(eIDScope.Actor, id);
        }

        public static void DeleteTriggerID(int id)
        {
            Instance.RemoveId(eIDScope.Trigger, id);
        }

        public static void DeleteUIID(int id)
        {
            Instance.RemoveId(eIDScope.UI, id);
        }

        public static void DeleteTileID(int id)
        {
            Instance.RemoveId(eIDScope.Tile, id);
        }

    }
}