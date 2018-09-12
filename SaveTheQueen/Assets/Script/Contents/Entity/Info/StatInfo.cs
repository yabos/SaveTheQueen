namespace Aniz.Contents.Entity.Info
{

    //-------------------------------------------------------------------
    // MonsterInfo
    //-------------------------------------------------------------------

    public class StatInfo
    {
        [System.Flags]
        public enum Type
        {
            //int
            CLASS_LEVEL,

            //물리 공격력
            P_ATTACK_POWER,

            MAX_HP, //총합
            CUR_HP, //현재 HP

            MOVE_SPEED,

            Invalid,
        }

        private readonly Type m_type;

        public int Amount
        {
            get { return m_value; }
        }

        public float fAmount
        {
            get { return m_fvalue; }
        }

        private int m_value = 0;
        private float m_fvalue = 0;

        public Type SType
        {
            get { return m_type; }
        }


        public StatInfo(Type type)
        {
            m_type = type;
            m_value = 0;
        }

        public void SetValue(int value)
        {
            m_value = value;
        }
        public void SetValue(float value)
        {
            m_fvalue = value;
        }
    }
}