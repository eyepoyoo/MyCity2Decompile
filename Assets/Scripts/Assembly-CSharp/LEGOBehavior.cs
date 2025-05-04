using UnityEngine;

public class LEGOBehavior : MonoBehaviour
{
	public T GetInterfaceComponent<T>()
	{
		Component[] components = GetComponents<Component>();
		Component[] array = components;
		foreach (Component component in array)
		{
			if (component is T)
			{
				return Convert<T>(component);
			}
		}
		return default(T);
	}

	private T Convert<T>(object obj)
	{
		return (T)obj;
	}
}
