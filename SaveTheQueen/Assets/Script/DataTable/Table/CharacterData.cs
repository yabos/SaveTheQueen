using System;
using System.Collections.Generic;
using table.db;

namespace Aniz.Data
{
    public class CharacterData : BinaryScriptData
    {
        private DB_CharacterList m_dbNpcList = new DB_CharacterList();

        public DB_Character GetCharacter(string name)
        {
            return m_dbNpcList.items.Find(x => (name.Equals(name, StringComparison.InvariantCultureIgnoreCase)));
        }

        public DB_Character GetCharacter(int id)
        {
            return m_dbNpcList.items.Find(x => (id == id));
        }

        public string GetNpcSprite(string name)
        {
            //DB_Npc dbNpc = GetNpc(name);
            //if (dbNpc.spriteName.Equals("all", StringComparison.InvariantCultureIgnoreCase))
            //{
            //    ResData resData = DataManager.Instance.GetScriptData<ResData>(E_GameScriptData.Resource);
            //    return resData.GetRandomMonster();
            //}

            return null;
        }

        //public PlayerInfo MakeNpcInfo(int maplevel)
        //{
        //    HeroData heroData = DataManager.Instance.GetScriptData<HeroData>(E_GameScriptData.Hero);
        //    PlayerInfo monsterData = new PlayerInfo();
        //    DB_Status_Stat dbStatusStat = heroData.GetDefaultStatus_Stat(PC_Class.warrior);
        //    DB_Status_HpMp dbStatusHpMp = heroData.GetLevelHpMp(maplevel);

        //    monsterData.StatusInfo = CalStatus.MakeStatus(maplevel, dbStatusStat);

        //    return monsterData;
        //}

        //public MonsterInfo MakeMonsterInfo(int maplevel)
        //{
        //    HeroData heroData = DataManager.Instance.GetScriptData<HeroData>(E_GameScriptData.Hero);
        //    MonsterInfo monsterInfo = new MonsterInfo();
        //    DB_Status_Stat dbStatusStat = heroData.GetDefaultStatus_Stat(PC_Class.warrior);
        //    DB_Status_HpMp dbStatusHpMp = heroData.GetLevelHpMp(maplevel);

        //    monsterInfo.StatusInfo = CalStatus.MakeStatus(maplevel, dbStatusStat);

        //    PlayerInfo monsterData = new PlayerInfo();
        //    monsterData.Name = "monster1";
        //    monsterData.StatusInfo = CalStatus.MakeStatus(maplevel, dbStatusStat);
        //    monsterInfo.LstPlayerInfo.Add(monsterData);
        //    monsterData = new PlayerInfo();
        //    monsterData.Name = "monster2";
        //    monsterData.StatusInfo = CalStatus.MakeStatus(maplevel, dbStatusStat);
        //    monsterInfo.LstPlayerInfo.Add(monsterData);

        //    return monsterInfo;
        //}

        //public PlayerInfo RandomMonsterInfo(int maplevel)
        //{
        //    DB_Npc npc = m_dbNpcList.items[UnityEngine.Random.Range(0, m_dbNpcList.items.Count)];

        //    PlayerInfo monsterInfo = new PlayerInfo();

        //    monsterInfo.StatusInfo = CalStatus.MakeStatus(maplevel, npc); ;

        //    monsterInfo.Name = npc.npcName;
        //    monsterInfo.DisplayName = npc.displayName;
        //    //monsterInfo.StatusInfo = CalStatus.MakeStatus(maplevel, dbStatusStat, dbStatusHpMp);


        //    return monsterInfo;
        //}



        public override void Release()
        {
        }

        protected override bool ReadData(BinaryDecoder decoder)
        {
            if (m_dbNpcList.Decode(decoder) == false)
                return false;

            return true;
        }
    }
}