// =====================================================================
// ChessPiece.cs — class แม่ของหมากทุกตัว
// [Complexity] ใช้ abstract class + inheritance: หมากแต่ละชนิดเขียนกฎการเดินของตัวเอง
// =====================================================================
namespace SimpleChess
{
    public abstract class ChessPiece
    {
        public bool IsWhite { get; set; }   // true = ขาว, false = ดำ
        public int Row { get; set; }        // แถวปัจจุบันใน array (0 = บนสุด)
        public int Col { get; set; }        // คอลัมน์ปัจจุบัน (0 = a)

        public ChessPiece(bool isWhite, int row, int col)
        {
            IsWhite = isWhite;
            Row = row;
            Col = col;
        }

        // [Function/Method] กฎการเดินเฉพาะของหมากแต่ละชนิด (class ลูก override)
        // รับ board = array 2 มิติ เพื่อตรวจว่ามีหมากขวางทาง/ช่องปลายทางมีอะไร
        public abstract bool IsValidMove(int toRow, int toCol, ChessPiece[,] board);

        // [Function/Method] สัญลักษณ์ที่แสดงบนกระดาน
        public abstract string GetSymbol();
    }
}
