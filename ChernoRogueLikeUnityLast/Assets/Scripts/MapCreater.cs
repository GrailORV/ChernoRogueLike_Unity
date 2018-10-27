using UnityEngine;
using UnityEngine.UI;


public class MapCreater : MonoBehaviour
{
    [SerializeField]
    private ScrollRect MapView;
    private RectTransform content;

    [SerializeField]
    private InputField WidthInputField;
    private int width = 10;

    [SerializeField]
    private InputField HeightInputField;
    private int height = 10;

    public ScrollRect MapView1
    {
        get
        {
            return MapView;
        }

        set
        {
            MapView = value;
        }
    }

    public InputField WidthInputField1
    {
        get
        {
            return WidthInputField;
        }

        set
        {
            WidthInputField = value;
        }
    }

    public InputField HeightInputField1
    {
        get
        {
            return HeightInputField;
        }

        set
        {
            HeightInputField = value;
        }
    }

    // Use this for initialization
    void Start()
    {
        content = MapView1.content;
        width = height = 10;
        SetWidth();
        SetHeight();
        WidthInputField1.text = width.ToString();
        HeightInputField1.text = height.ToString();
        SetMapPanel();
    }

    // Update is called once per frame
    void Update()
    {

    }

    void SetWidth()
    {
        Vector2 buf = content.sizeDelta;
        GridLayoutGroup glg = content.GetComponent<GridLayoutGroup>();
        buf.x = glg.padding.left + glg.padding.right + (glg.cellSize.x + glg.spacing.x) * width;
        content.sizeDelta = buf;
    }

    void SetHeight()
    {
        Vector2 buf = content.sizeDelta;
        GridLayoutGroup glg = content.GetComponent<GridLayoutGroup>();
        buf.y = glg.padding.top + glg.padding.bottom + (glg.cellSize.y + glg.spacing.y) * height;
        content.sizeDelta = buf;
    }

    public void OnChangeWidth(string value)
    {
        width = int.Parse(value);
        SetWidth();
        SetMapPanel();
    }

    public void OnChangeHeight(string value)
    {
        height = int.Parse(value);
        SetHeight();
        SetMapPanel();
    }

    public void OnClickEnlarge()
    {
        GridLayoutGroup glg = content.GetComponent<GridLayoutGroup>();
        if(glg.cellSize.x>=128)
        {
            return;
        }

        glg.cellSize *= 2.0f;
        SetWidth();
        SetHeight();
    }

    public void OnClickShrink()
    {
        GridLayoutGroup glg = content.GetComponent<GridLayoutGroup>();
        if (glg.cellSize.x <= 8)
        {
            return;
        }

        glg.cellSize /= 2.0f;
        SetWidth();
        SetHeight();
    }

    private void SetMapPanel()
    {
        foreach(RectTransform child in content)
        {
            Destroy(child.gameObject);
        }

        GameObject panelPrefab = Resources.Load("Prefabs/MapPanel") as GameObject;
        for (int i = 0; i < width * height; i++)
        {
            GameObject MapPanel = Instantiate(panelPrefab, content);
        }
    }

}
