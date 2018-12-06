using System;

public struct MapPosition
{
    public byte x;
    public byte y;

    public static MapPosition Zero = new MapPosition(0, 0);

    public MapPosition(byte X, byte Y)
    {
        x = X;
        y = Y;
    }

    // override object.Equals
    public override bool Equals(object obj)
    {
        if (obj == null || GetType() != obj.GetType())
        {
            return false;
        }

        MapPosition p = (MapPosition)obj;
        return (x == p.x && y == p.y);
    }

    // override object.GetHashCode
    public override int GetHashCode()
    {
        return x + y;
    }

    public static MapPosition operator +(MapPosition a, MapPosition b)
    {
        byte newX = (byte)Math.Min(255, a.x + b.x);
        byte newY = (byte)Math.Min(255, a.y + b.y);
        return new MapPosition(newX, newY);
    }

    public static MapPosition operator -(MapPosition a, MapPosition b)
    {
        byte newX = (byte)Math.Max(0, a.x - b.x);
        byte newY = (byte)Math.Max(0, a.y - b.y);
        return new MapPosition(newX, newY);
    }

    public static bool operator ==(MapPosition a,MapPosition b)
    {
        return (a.x == b.x && a.y == b.y);
    }

    public static bool operator !=(MapPosition a, MapPosition b)
    {
        return (a.x != b.x || a.y != b.y);
    }
}