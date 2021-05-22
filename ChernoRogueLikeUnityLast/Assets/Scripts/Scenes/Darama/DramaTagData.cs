public class DramaTagData
{
    /// <summary>
    /// タグの種類
    /// </summary>
    public enum TagType
    {
        Speed = 0,
        All,
    }

    /// <summary>
    /// タグの文字タイプ
    /// Normal= speed  Short = s  All = s|speed
    /// </summary>
    public enum TagTextType
    {
        Normal,
        Short,
        All
    }

    public TagType tagType = TagType.All;

    public string normalTag = string.Empty;
    public string shortTag = string.Empty;
    public string allTag = string.Empty;

    public DramaTagData(TagType type, string normalTag, string shortTag, string allTag)
    {
        tagType = type;
        this.normalTag = normalTag;
        this.shortTag = shortTag;
        this.allTag = allTag;
    }
}