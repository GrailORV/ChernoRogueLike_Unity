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

    /// <summary> 説明 </summary>
    public string Description { get; set; }

    private List<ItemData> _potItemDataList;
    /// <summary> ツボのアイテム情報 </summary>
    public List<ItemData> PotItemDataList
    {
        get
        {
            if (Type != ItemType.Pot)
            {
                return null;
            }

            if (_potItemDataList == null)
            {
                _potItemDataList = new List<ItemData>();
            }

            return _potItemDataList;
        }
        set
        {
            if (Type != ItemType.Pot)
            {
                return;
            }

            _potItemDataList = value;
        }
    }

    public ItemData(ItemTableData.Data data = null)
    {
        if (data == null)
        {
            Id = -1;
            return;
        }

        Id = data.id;
        Type = (ItemType)data.type;
        Name = data.name;
        Description = data.Description;
    }

    /// <summary>
    /// データの初期化
    /// </summary>
    public void Init()
    {
        Id = -1;
        Type = ItemType.None;
        Name = string.Empty;
        Description = string.Empty;

        if (PotItemDataList != null)
        {
            PotItemDataList.Clear();
        }
    }
}
