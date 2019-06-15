public class ItemData
{
    /// <summary> ID </summary>
    int _id = -1;
    public int Id
    {
        get { return _id; }
        set { _id = value; }
    }

    /// <summary> 種類 </summary>
    string _type = "";
    public string Type
    {
        get { return _type; }
        set { _type = value; }
    }

    /// <summary> 名前 </summary>
    string _name = "";
    public string Name
    {
        get { return _name; }
        set { _name = value; }
    }
}
