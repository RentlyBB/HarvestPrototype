using UnityEngine;
using UnityEngine.UI;

namespace _Scripts.UI {
    public class FlexibleGridLayout : LayoutGroup {
        public enum FitType {
            Uniform,
            Width,
            Height,
            FixedRows,
            FixedColumns,
        }

        public FitType fitType;
        public int rows;
        public int columns;
        public Vector2 cellSize;
        public Vector2 spacing;

        public bool fitX;
        public bool fitY;

        public override void CalculateLayoutInputHorizontal() {
            base.CalculateLayoutInputHorizontal();

            // Determine rows and columns based on FitType
            if (fitType == FitType.Width || fitType == FitType.Height || fitType == FitType.Uniform) {
                fitX = true;
                fitY = true;
                float sqrRt = Mathf.Sqrt(transform.childCount);
                rows = Mathf.CeilToInt(sqrRt);
                columns = Mathf.CeilToInt(sqrRt);
            }

            if (fitType == FitType.Width || fitType == FitType.FixedColumns) {
                rows = Mathf.CeilToInt(transform.childCount / (float)columns);
            }

            if (fitType == FitType.Height || fitType == FitType.FixedRows) {
                columns = Mathf.CeilToInt(transform.childCount / (float)rows);
            }

            if (columns == 0 || rows == 0)return;

        // Calculate cell size
            float parentWidth = rectTransform.rect.width;
            float parentHeight = rectTransform.rect.height;

            float cellWidth = (parentWidth / columns) - ((spacing.x / columns) * 2) - (padding.left / columns) - (padding.right / columns);
            float cellHeight = (parentHeight / rows) - ((spacing.y / rows) * 2) - (padding.top / rows) - (padding.bottom / rows);

            cellSize.x = fitX ? cellWidth : cellSize.x;
            cellSize.y = fitY ? cellHeight : cellSize.y;

            // Set positions
            int columnCount = 0;
            int rowCount = 0;

            for (int i = 0; i < rectChildren.Count; i++) {
                rowCount = i / columns;
                columnCount = i % columns;

                var item = rectChildren[i];

                // Calculate position based on alignment
                var xPos = (cellSize.x * columnCount) + (spacing.x * columnCount) + padding.left;
                var yPos = (cellSize.y * rowCount) + (spacing.y * rowCount) + padding.top;

                // Adjust position based on alignment
                if (childAlignment == TextAnchor.MiddleCenter || 
                    childAlignment == TextAnchor.UpperCenter || 
                    childAlignment == TextAnchor.LowerCenter) {
                    xPos += (parentWidth - (cellSize.x * columns + spacing.x * (columns - 1) + padding.left + padding.right)) / 2;
                }
                if (childAlignment == TextAnchor.MiddleRight || 
                    childAlignment == TextAnchor.UpperRight || 
                    childAlignment == TextAnchor.LowerRight) {
                    xPos += parentWidth - (cellSize.x * columns + spacing.x * (columns - 1) + padding.left + padding.right);
                }
                if (childAlignment == TextAnchor.MiddleCenter || 
                    childAlignment == TextAnchor.MiddleLeft || 
                    childAlignment == TextAnchor.MiddleRight) {
                    yPos += (parentHeight - (cellSize.y * rows + spacing.y * (rows - 1) + padding.top + padding.bottom)) / 2;
                }
                if (childAlignment == TextAnchor.LowerCenter || 
                    childAlignment == TextAnchor.LowerLeft || 
                    childAlignment == TextAnchor.LowerRight) {
                    yPos += parentHeight - (cellSize.y * rows + spacing.y * (rows - 1) + padding.top + padding.bottom);
                }

                SetChildAlongAxis(item, 0, xPos, cellSize.x);
                SetChildAlongAxis(item, 1, yPos, cellSize.y);
            }

        }

        public override void CalculateLayoutInputVertical() { }


        public override void SetLayoutHorizontal() { }


        public override void SetLayoutVertical() { }
        
    }
}