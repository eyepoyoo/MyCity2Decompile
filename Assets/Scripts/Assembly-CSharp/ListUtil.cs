using System.Collections.Generic;

public class ListUtil
{
	public static bool Contains<T>(List<T> list, T needle) where T : class
	{
		int count = list.Count;
		for (int i = 0; i < count; i++)
		{
			if (list[i] == needle)
			{
				return true;
			}
		}
		return false;
	}
}
