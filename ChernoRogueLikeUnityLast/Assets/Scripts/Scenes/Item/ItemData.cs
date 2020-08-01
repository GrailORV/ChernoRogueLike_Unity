using System.Collections;
using System.Collections.Generic;

public class ItemData
{
    public enum ItemType
    {
        None = 0,
        Wepon,
        armor,
        Food,
        Pot,
    }

    /// <summary> ID </summary>
    public int Id { get; set; }

    /// <summary> 種類 </summary>
    public ItemType Type { get; set; }

    /// <summary> 名前 </summary>
    public string Name { get; set; }

    /// <summary> ツボのアイテム情報 </summary>
    public List<ItemData> potItemDataList { get; set; }

    public ItemData(ItemTableData.Data data)
    {
        if (data == null)
        {
            Id = -1;
            return;
        }

        Id = data.id;
        Type = (ItemType)data.type;
        Name = data.name;

        switch (Type)
        {
            case ItemType.None:
                break;
            case ItemType.Wepon:
                break;
            case ItemType.armor:
                break;
            case ItemType.Food:
                break;
            case ItemType.Pot:
                potItemDataList = new List<ItemData>();
                break;
            default:
                break;
        }
    }
}
