public static class ParseUtil
{

	public static bool ParseBool(string value)
	{
		if (value == "True")
			return true;
		return false;
	}

	public static float ParseFloat(string value)
	{
		return float.Parse(value);
	}

	public static int ParseInt(string value)
	{
		return int.Parse(value);
	}

}