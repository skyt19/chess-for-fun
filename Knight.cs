// =====================================================================
// Knight.cs — ม้า: เดินรูปตัว L (2 ช่อง + 1 ช่องตั้งฉาก) กระโดดข้ามหมากได้
// [if-else] เช็กรูปแบบตัว L + ช่องปลายทาง
// =====================================================================
using System;

namespace SimpleChess
{
    public class Knight : ChessPiece
    {
        public Knight(bool isWhite, int row, int col) : base(isWhite, row, col) { }

        // [Function/Method] กฎการเดินของม้า
        public override bool IsValidMove(int toRow, int toCol, ChessPiece[,] board)
        {
            if (toRow < 0 || toRow >= 8 || toCol < 0 || toCol >= 8)
                return false;

            int rowDiff = Math.Abs(toRow - Row);
            int colDiff = Math.Abs(toCol - Col);

            // ตัว L: (2,1) หรือ (1,2)
            if (!((rowDiff == 2 && colDiff == 1) || (rowDiff == 1 && colDiff == 2)))
                return false;

            // ช่องปลายทางห้ามเป็นหมากฝ่ายเดียวกัน
            if (board[toRow, toCol] != null && board[toRow, toCol].IsWhite == IsWhite)
                return false;

            return true;
        }

        public override string GetSymbol()
        {
            return IsWhite ? "Kn" : "kn";
        }
    }
}
