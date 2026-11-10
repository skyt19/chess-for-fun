// =====================================================================
// King.cs — คิง: เดินได้ทุกทิศ ทีละ 1 ช่อง
// [if-else] เช็กระยะ + ช่องปลายทาง
// (การห้ามเดินเข้าไปโดนรุก เช็กใน ChessBoard.IsLegalMove)
// =====================================================================
using System;

namespace SimpleChess
{
    public class King : ChessPiece
    {
        public King(bool isWhite, int row, int col) : base(isWhite, row, col) { }

        // [Function/Method] กฎการเดินของคิง
        public override bool IsValidMove(int toRow, int toCol, ChessPiece[,] board)
        {
            if (toRow < 0 || toRow >= 8 || toCol < 0 || toCol >= 8)
                return false;

            int rowDiff = Math.Abs(toRow - Row);
            int colDiff = Math.Abs(toCol - Col);

            // ไกลเกิน 1 ช่อง -> เดินไม่ได้
            if (rowDiff > 1 || colDiff > 1)
                return false;

            // อยู่กับที่ -> ไม่นับ
            if (rowDiff == 0 && colDiff == 0)
                return false;

            // ช่องปลายทางห้ามเป็นหมากฝ่ายเดียวกัน
            if (board[toRow, toCol] != null && board[toRow, toCol].IsWhite == IsWhite)
                return false;

            return true;
        }

        public override string GetSymbol()
        {
            return IsWhite ? "K" : "k";
        }
    }
}
