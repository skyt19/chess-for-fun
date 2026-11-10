// =====================================================================
// Queen.cs — ควีน: เดินได้ทั้งแบบเรือ (ตรง) และบิชอป (ทแยง)
// [Loop] while เดินไล่ตรวจทีละช่องตามเส้นทาง
// [if-else] เช็กว่าเป็นแนวตรงหรือแนวทแยง + ช่องปลายทาง
// =====================================================================
using System;

namespace SimpleChess
{
    public class Queen : ChessPiece
    {
        public Queen(bool isWhite, int row, int col) : base(isWhite, row, col) { }

        // [Function/Method] กฎการเดินของควีน
        public override bool IsValidMove(int toRow, int toCol, ChessPiece[,] board)
        {
            if (toRow < 0 || toRow >= 8 || toCol < 0 || toCol >= 8)
                return false;

            int rowDiff = Math.Abs(toRow - Row);
            int colDiff = Math.Abs(toCol - Col);

            bool isDiagonal = rowDiff == colDiff && rowDiff > 0;
            bool isStraight = (Row == toRow && Col != toCol) || (Row != toRow && Col == toCol);

            if (!isDiagonal && !isStraight)
                return false;

            int rowStep = Row == toRow ? 0 : (toRow > Row ? 1 : -1);
            int colStep = Col == toCol ? 0 : (toCol > Col ? 1 : -1);
            int currentRow = Row + rowStep;
            int currentCol = Col + colStep;

            // ตรวจทุกช่องระหว่างทาง ห้ามมีหมาก
            while (currentRow != toRow || currentCol != toCol)
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
            return IsWhite ? "Q" : "q";
        }
    }
}
