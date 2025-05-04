using System;

[Serializable]
public struct Point
{
	public int x;

	public int y;

	public Point(int px, int py)
	{
		x = px;
		y = py;
	}

	public override bool Equals(object obj)
	{
		if (obj is Point)
		{
			return this == (Point)obj;
		}
		return false;
	}

	public override int GetHashCode()
	{
		return (x * y).GetHashCode();
	}

	public static Point operator +(Point pointA, Point pointB)
	{
		return new Point(pointA.x + pointB.x, pointA.y + pointB.y);
	}

	public static Point operator -(Point pointA, Point pointB)
	{
		return new Point(pointA.x - pointB.x, pointA.y - pointB.y);
	}

	public static Point operator *(Point pointA, int value)
	{
		return new Point(pointA.x * value, pointA.y * value);
	}

	public static Point operator /(Point pointA, int value)
	{
		return new Point(pointA.x / value, pointA.y / value);
	}

	public static Point operator *(Point pointA, Point pointB)
	{
		return new Point(pointA.x * pointB.x, pointA.y * pointB.y);
	}

	public static Point operator /(Point pointA, Point pointB)
	{
		return new Point(pointA.x / pointB.x, pointA.y / pointB.y);
	}

	public static bool operator ==(Point pointA, Point pointB)
	{
		return pointA.x == pointB.x && pointA.y == pointB.y;
	}

	public static bool operator !=(Point pointA, Point pointB)
	{
		return !(pointA == pointB);
	}
}
