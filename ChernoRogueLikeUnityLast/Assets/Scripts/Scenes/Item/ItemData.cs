public class ItemData
{
    /// <summary> ID </summary>
    public int Id { get; set; }

    /// <summary> 種類 </summary>
    public string Type { get; set; }

    /// <summary> 名前 </summary>
    public string Name { get; set; }

    public ItemData(ItemTableData.Data data)
    {
        Id = data.id;
        Type = data.type;
        Name = data.name;
    }
}
