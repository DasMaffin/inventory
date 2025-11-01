using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(GridLayoutGroup))]
public class ResponsiveGridLayoutGroup : MonoBehaviour
{
    public int columns = 10;
    public int rows = 4;
    public bool usePercentages = true;
    public RectOffset paddingPercent;
    public Vector2 spacingPercent;

    private GridLayoutGroup gridLayout;

    void Start()
    {
        gridLayout = GetComponent<GridLayoutGroup>();
        if(gridLayout.constraint == GridLayoutGroup.Constraint.FixedColumnCount)
        {
            columns = gridLayout.constraintCount;
        }
        else if(gridLayout.constraint == GridLayoutGroup.Constraint.FixedRowCount)
        {
            rows = gridLayout.constraintCount;
        }
        AdjustCellSize();
        //Settings.Instance.onChangeResolution += AdjustCellSize;
    }

    void AdjustCellSize()
    {
        RectTransform rt = GetComponent<RectTransform>();
        float width = rt.rect.width;
        float height = rt.rect.height;

        if(usePercentages)
        {
            gridLayout.padding.left = Mathf.RoundToInt((paddingPercent.left/100f) * width);
            gridLayout.padding.right = Mathf.RoundToInt((paddingPercent.right/100f) * width);
            gridLayout.padding.top = Mathf.RoundToInt((paddingPercent.top/100f) * height);
            gridLayout.padding.bottom = Mathf.RoundToInt((paddingPercent.bottom/100f) * height);
            gridLayout.spacing = new Vector2((spacingPercent.x/100f) * width, (spacingPercent.y/100f) * height);
        }

        float cellWidth, cellHeight;
        // Calculate cell width and height by subtracting padding and spacing.
        if (gridLayout.constraint == GridLayoutGroup.Constraint.FixedColumnCount)
        {
            cellWidth = (width - gridLayout.padding.left - gridLayout.padding.right - (columns - 1) * gridLayout.spacing.x) / columns;
            cellHeight = cellWidth;
        }
        else
        {
            cellHeight = (height - gridLayout.padding.top - gridLayout.padding.bottom - (rows - 1) * gridLayout.spacing.y) / rows;
            cellWidth = cellHeight;
        }

        gridLayout.cellSize = new Vector2(cellWidth, cellHeight);
    }
}
