
public class IdGenerator
{
    private BitTrain m_ids;

    public IdGenerator(int size)
    {
        m_ids = new BitTrain(size);
    }

    public int GetId()
    {
        int id = m_ids.FirstUnUsedIndex();
        m_ids.Set(id, true);

        return id;
    }

    public void RemoveId(int id)
    {
        m_ids.Set(id, false);
    }

    public void SetId(int id)
    {
        m_ids.Set(id, true);
    }

    public int GetIdCount()
    {
        return m_ids.UsedCount;
    }

    public void Clear()
    {
        int size = m_ids.TotalSeatCount;
        m_ids = new BitTrain(size);
    }
}
