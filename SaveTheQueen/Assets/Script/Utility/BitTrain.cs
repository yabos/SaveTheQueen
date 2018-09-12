using System;
using System.Diagnostics;

public class BitTrain
{
    private readonly uint[] m_train;
    private readonly int m_totalSeatCount;

    public BitTrain(int seatCount)
    {
        if (seatCount < 1) throw new ArgumentOutOfRangeException();

        m_totalSeatCount = seatCount;

        int carriageCount = (seatCount - 1) / 32 + 1;
        m_train = new uint[carriageCount];
    }

    public BitTrain(int seatCount, bool init)
        : this(seatCount)
    {
        Initialize(init);
    }

    public BitTrain(int seatCount, byte[] bytes)
        : this(seatCount)
    {
        if (m_train.Length != (bytes.Length - 1) / 4 + 1) throw new ArgumentException();

        Buffer.BlockCopy(bytes, 0, m_train, 0, bytes.Length);
    }

    private BitTrain(int seatCount, uint[] train)
    {
        m_train = new uint[train.Length];
        Array.Copy(train, m_train, train.Length);
        m_totalSeatCount = seatCount;
    }

    public int TotalSeatCount
    {
        get { return m_totalSeatCount; }
    }

    public int UsedCount
    {
        get { return GetUsedCount(); }
    }

    public byte[] ToBytes
    {
        get
        {
            byte[] bytes = new byte[(m_totalSeatCount - 1) / 8 + 1];
            Buffer.BlockCopy(m_train, 0, bytes, 0, bytes.Length);
            return bytes;
        }
    }

    public void Initialize(bool init)
    {
        if (init)
        {
            int lastCarriage = GetCarriageNumber(m_totalSeatCount - 1);
            for (int i = 0; i < lastCarriage; ++i)
            {
                m_train[i] = 0xffffffff;
            }

            int seat = GetSeatNumber(m_totalSeatCount - 1);

            m_train[lastCarriage] = 0xffffffff >> (31 - seat);
        }
        else
        {
            for (int i = 0; i < m_train.Length; ++i)
            {
                m_train[i] = 0;
            }
        }
    }

    public bool IsUsed(int index)
    {
        int trainNumber = GetCarriageNumber(index);
        int seatNumber = GetSeatNumber(index);

        return ((m_train[trainNumber] >> seatNumber) & 0x01) == 0x01;
    }

    public void Toggle(int index)
    {
        VerifyRange(index);

        int carriageNumber = GetCarriageNumber(index);
        int seatNumber = GetSeatNumber(index);

        m_train[carriageNumber] ^= (uint)(0x01 << seatNumber);
    }

    private int GetUsedCount()
    {
        int totalUsedCount = 0;
        for (int carriageIndex = 0; carriageIndex < m_train.Length; ++carriageIndex)
        {
            uint carriage = m_train[carriageIndex];
            carriage = (carriage - ((carriage >> 1) & 0x55555555));
            carriage = (carriage & 0x33333333) + ((carriage >> 2) & 0x33333333);
            carriage = (carriage + (carriage >> 4)) & 0x0f0f0f0f;
            carriage = carriage + (carriage >> 8);
            carriage = carriage + (carriage >> 16);
            totalUsedCount += (int)carriage & 0x0000003f;
        }

        return totalUsedCount;
    }

    private static int GetCarriageNumber(int index)
    {
        return index / 32;
    }

    private static int GetSeatNumber(int index)
    {
        return index % 32;
    }

    private void VerifyRange(int index)
    {
        if (index < 0 ||
            index >= m_totalSeatCount)
        {
            throw new ArgumentOutOfRangeException();
        }
    }

    public void Set(int index, bool use)
    {
        VerifyRange(index);

        int carriageNumber = GetCarriageNumber(index);
        int seatNumber = GetSeatNumber(index);

        if (use)
            m_train[carriageNumber] |= (uint)(0x01 << seatNumber);
        else
            m_train[carriageNumber] &= (uint)~(0x01 << seatNumber);
    }

    public int FirstUnUsedIndex()
    {
        for (int carriageIndex = 0; carriageIndex < m_train.Length; ++carriageIndex)
        {
            uint compareBit = 0xffffffff;

            uint carriage = m_train[carriageIndex];
            if ((carriage & compareBit) == compareBit) continue;

            int findSeat = 0;

            for (int recursive = 0; recursive < 5; ++recursive)
            {
                int shiftBit = 32 >> (recursive + 1);
                compareBit = compareBit >> shiftBit;
                if ((carriage & compareBit) != compareBit)
                {
                    carriage = carriage & compareBit;
                }
                else
                {
                    findSeat += shiftBit;
                    carriage = carriage >> shiftBit;

                }
            }

            int find = carriageIndex * 32 + findSeat;
            if (find >= TotalSeatCount) return -1;
            else return find;
        }

        return -1;
    }

    public object Clone()
    {
        return new BitTrain(m_totalSeatCount, m_train);
    }

    public void And(BitTrain bitTrain)
    {
        Trace.Assert(bitTrain.m_totalSeatCount == m_totalSeatCount);
        for (int i = 0; i < m_train.Length; ++i)
        {
            m_train[i] &= bitTrain.m_train[i];
        }
    }

    public void Or(BitTrain bitTrain)
    {
        Trace.Assert(bitTrain.m_totalSeatCount == m_totalSeatCount);
        for (int i = 0; i < m_train.Length; ++i)
        {
            m_train[i] |= bitTrain.m_train[i];
        }
    }
}