using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace bpdev
{
    public class FlexibleGridLayout : LayoutGroup
    {
        public enum FitType
        {
            Uniform,
            Width,
            Height,
            FixedRows,
            FixedColumns,
        }

        public FitType fitType;


        [Min(1)] public int Rows;
        [Min(1)] public int Columns;
        public Vector2 CellSize;
        [Min(0)] public Vector2 Spacing;

        public bool FitX;
        public bool FitY;

        public override void CalculateLayoutInputHorizontal()
        {
            base.CalculateLayoutInputHorizontal();

            if (fitType == FitType.Width || fitType == FitType.Height || fitType == FitType.Uniform)
            {
                FitX = true;
                FitY = true;

                float sqrRt = Mathf.Sqrt(rectChildren.Count);
                Rows = Mathf.CeilToInt(sqrRt);
                Columns = Mathf.CeilToInt(sqrRt);
            }

            if (fitType == FitType.Width || fitType == FitType.FixedColumns)
            {
                Rows = Mathf.CeilToInt(rectChildren.Count / (float) Columns);
            }

            if (fitType == FitType.Height || fitType == FitType.FixedRows)
            {
                Columns = Mathf.CeilToInt(rectChildren.Count / (float) Rows);
            }

            float parentWidth = rectTransform.rect.width;
            float parentHeight = rectTransform.rect.height;

            float cellWidth = (parentWidth / (float) Columns) - ((Spacing.x / (float) Columns) * (Columns - 1)) - (padding.left / (float) Columns) - (padding.right / (float) Columns);
            float cellHeight = (parentHeight / (float) Rows) - ((Spacing.y / (float) Rows) * (Rows - 1)) - (padding.top / (float) Rows) - (padding.bottom / (float) Rows);

            // think this is the problem
            // for some reason, cellsize is defaulting to negative values, and this function is not being called on refresh
            CellSize.x = FitX ? cellWidth : CellSize.x;
            CellSize.y = FitY ? cellHeight : CellSize.y;

            int columnCount = 0;
            int rowCount = 0;

            for (int i = 0; i < rectChildren.Count; i++)
            {
                rowCount = i / Columns;
                columnCount = i % Columns;

                var cell = rectChildren[i];

                var xPos = (CellSize.x * columnCount) + (Spacing.x * columnCount) + padding.left;
                var yPos = (CellSize.y * rowCount) + (Spacing.y * rowCount) + padding.top;

                SetChildAlongAxis(cell, 0, xPos, CellSize.x);
                SetChildAlongAxis(cell, 1, yPos, CellSize.y);
            }
        }
        
        public override void CalculateLayoutInputVertical()
        {
        }

        public override void SetLayoutHorizontal()
        {
        }

        public override void SetLayoutVertical()
        {
        }
    }
}