// =====================================================================
// Bishop.cs — บิชอป (โคน): เดินทแยง กี่ช่องก็ได้ ห้ามมีหมากขวาง
// [Loop] while เดินไล่ตรวจทีละช่องตามแนวทแยง
// [if-else] เช็กว่าเป็นแนวทแยงจริง + ช่องปลายทาง
// =====================================================================
using System;

namespace SimpleChess
{
    public class Bishop : ChessPiece
    {
        public Bishop(bool isWhite, int row, int col) : base(isWhite, row, col) { }

        // [Function/Method] กฎการเดินของบิชอป
        public override bool IsValidMove(int toRow, int toCol, ChessPiece[,] board)
        {
            if (toRow < 0 || toRow >= 8 || toCol < 0 || toCol >= 8)
                return false;

            // แนวทแยง = ระยะแถวเท่ากับระยะคอลัมน์ และต้องขยับจริง
            int rowDiff = Math.Abs(toRow - Row);
            int colDiff = Math.Abs(toCol - Col);
            if (rowDiff != colDiff || rowDiff == 0)
                return false;

            int rowStep = toRow > Row ? 1 : -1;
            int colStep = toCol > Col ? 1 : -1;
            int currentRow = Row + rowStep;
            int currentCol = Col + colStep;

            // ตรวจทุกช่องระหว่างทาง ห้ามมีหมาก
            while (currentRow != toRow)
            {
                if (board[currentRow, currentCol] != null)
                    return false;
                currentRow += rowStep;
                currentCol += colStep;
            }

            // ช่องปลายทางห้ามเป็นหมากฝ่ายเดียวกัน
            if (board[toRow, toCol] != null && board[toRow, toCol].IsWhite == IsWhite)
                return false;

            return true;
        }

        public override string GetSymbol()
        {
            return IsWhite ? "B" : "b";
        }
    }
}
