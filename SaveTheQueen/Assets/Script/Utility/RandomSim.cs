using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public sealed class RandomSim
{
	private static int seed = 781001;
	private static System.Random rand = new Random(seed); // given the same seed, rand will generate the same series of random numbers
	private static uint _count = 0;

	public static int Seed
	{
		set // recreate Random with new seed
		{
			seed = value;
			rand = new Random(seed);

			EventLogger.Log(EventLogType.RandomSim, string.Format("Seed : {0}", seed));
		}
	}

	public static uint count
	{
		get { return _count; }
	}

	private static int Rand() // Returns a non-negative random integer.
	{
		++_count;
		var ret = rand.Next();
		EventLogger.Log(EventLogType.RandomSim, string.Format("Rand : {0}({1}) [int type]", ret, _count));

		return ret;
	}

	private static double RandDouble()
	{
		++_count;
		var ret = rand.NextDouble();
		EventLogger.Log(EventLogType.RandomSim, string.Format("Rand : {0}({1}) [double type]", ret, _count));

		return ret;
	}

	public static int Range(int a, int b) // min(a, b) <= return < max(a, b)
	{
		if (a == b)
		{
			return a;
		}
		else if (a > b)
		{
			int n = a - b;
			return (Rand() % n) + b;
		}
		else  // if (a < b)
		{
			int n = b - a;
			return (Rand() % n) + a;
		}
	}

	//public static FixedPoint Range(FixedPoint a, FixedPoint b) // min(a, b) <= return < max(a, b)
	//{
	//	if (a == b)
	//	{
	//		return a;
	//	}
	//	else if (a > b)
	//	{
	//		FixedPoint n = a - b;
	//		return FixedPoint.FromRawValue(Rand() % n.RawValue) + b;
	//	}
	//	else // if (a < b)
	//	{
	//		FixedPoint n = b - a;
	//		return FixedPoint.FromRawValue(Rand() % n.RawValue) + a;
	//	}
	//}

	public static float Range(float a, float b) // min(a, b) <= return < max(a, b)
	{
		if (a == b)
		{
			return a;
		}
		else if (a > b)
		{
			float diff = a - b;
			var ret = (float)RandDouble() * diff + b;
			return ret;
		}
		else // if (a < b)
		{
			float diff = b - a;
			var ret = (float)RandDouble() * diff + a;
			return ret;
		}
	}

	public static int RandIndex(int[] probs) // 각 원소의 크기가 클 수록 해당 원소의 인덱스가 선택될 확률이 높아진다
	{
		int total = 0;

		for (int i = 0; i < probs.Length; i++) // 매번 계산하는것은 낭비가 될 수 있다.
			total += probs[i];

		int r = Range(0, total);

		int cum = 0;
		for (int i = 0; i < probs.Length; i++)
		{
			if (cum <= r && r < (probs[i] + cum)) return i;
			cum += probs[i];
		}

		return -1; // failed to select any item
	}
}
