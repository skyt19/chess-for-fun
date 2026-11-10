// =====================================================================
// Rook.cs — เรือ: เดินแนวตั้ง/แนวนอน กี่ช่องก็ได้ ห้ามมีหมากขวาง
// [Loop] while เดินไล่ตรวจทีละช่องตามเส้นทาง
// [if-else] เช็กทิศทาง + ช่องปลายทาง
// =====================================================================
namespace SimpleChess
{
    public class Rook : ChessPiece
    {
        public Rook(bool isWhite, int row, int col) : base(isWhite, row, col) { }

        // [Function/Method] กฎการเดินของเรือ
        public override bool IsValidMove(int toRow, int toCol, ChessPiece[,] board)
        {
            if (toRow < 0 || toRow >= 8 || toCol < 0 || toCol >= 8)
                return false;

            // ต้องอยู่แถวเดียวกันหรือคอลัมน์เดียวกัน
            if (Row != toRow && Col != toCol)
                return false;

            // อยู่กับที่ไม่นับเป็นการเดิน
            if (Row == toRow && Col == toCol)
                return false;

            // ก้าวทีละ 1 ช่องในทิศที่จะไป (-1, 0, +1)
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
            return IsWhite ? "R" : "r";
        }
    }
}
