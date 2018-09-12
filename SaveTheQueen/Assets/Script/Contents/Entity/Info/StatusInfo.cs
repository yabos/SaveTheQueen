namespace Aniz.NodeGraph.Level.Group.Info
{
    public class StatusInfo
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

        private const int INT_UNIT = 1000;
        private const float INV_INT_UNIT = 1.0f / INT_UNIT;

        private readonly Type m_type;

        public int Amount
        {
            get
            {
                if (m_isfloat)
                {
                    return -1;
                }
                return m_value;
            }
        }

        public float fAmount
        {
            get
            {
                if (!m_isfloat)
                {
                    return float.MaxValue;
                }
                return m_value * INV_INT_UNIT;
            }
        }

        private int m_value = 0;
        private bool m_isfloat = false;

        public Type SType
        {
            get { return m_type; }
        }

        public bool Isfloat
        {
            get { return m_isfloat; }
        }

        public StatusInfo(Type type)
        {
            m_type = type;
            m_value = 0;
        }

        public void SetValue(int value)
        {
            m_isfloat = false;
            m_value = value;
        }
        public void SetValue(float value)
        {
            m_isfloat = true;
            m_value = (int)(INT_UNIT * value);
        }
    }
}