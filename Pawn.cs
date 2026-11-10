// =====================================================================
// Pawn.cs — เบี้ย
// [if-else] เช็กเงื่อนไขการเดิน: หน้า 1 ช่อง / หน้า 2 ช่องจากจุดเริ่ม / กินทแยง
// =====================================================================
using System;

namespace SimpleChess
{
    public class Pawn : ChessPiece
    {
        public Pawn(bool isWhite, int row, int col) : base(isWhite, row, col) { }

        // [Function/Method] กฎการเดินของเบี้ย
        public override bool IsValidMove(int toRow, int toCol, ChessPiece[,] board)
        {
            // ปลายทางต้องอยู่ในกระดาน
            if (toRow < 0 || toRow >= 8 || toCol < 0 || toCol >= 8)
                return false;

            int direction = IsWhite ? -1 : 1;   // ขาวเดินขึ้น (-1), ดำเดินลง (+1)
            int startRow = IsWhite ? 6 : 1;     // แถวเริ่มต้นของเบี้ย

            // เดินหน้า 1 ช่อง (ช่องหน้าต้องว่าง)
            if (toCol == Col && toRow == Row + direction)
            {
                if (board[toRow, toCol] == null)
                    return true;
            }

            // เดินหน้า 2 ช่อง จากแถวเริ่มต้นเท่านั้น (ทั้ง 2 ช่องต้องว่าง)
            if (toCol == Col && toRow == Row + (2 * direction) && Row == startRow)
            {
                int middleRow = Row + direction;
                if (board[toRow, toCol] == null && board[middleRow, Col] == null)
                    return true;
            }

            // กินทแยง 1 ช่อง (ต้องมีหมากฝ่ายตรงข้ามอยู่)
            if (Math.Abs(toCol - Col) == 1 && toRow == Row + direction)
            {
                if (board[toRow, toCol] != null && board[toRow, toCol].IsWhite != IsWhite)
                    return true;
            }

            return false;
        }

        public override string GetSymbol()
        {
            return IsWhite ? "P" : "p";
        }
    }
}
