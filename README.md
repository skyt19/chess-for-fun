# Simple Chess Game (C# Console)

เกมหมากรุก 2 ผู้เล่นบน console ควบคุมด้วยปุ่มลูกศร มี Save/Load, ตรวจ check / checkmate / stalemate

วิธีรัน: ดู `HOW_TO_RUN.txt` (`dotnet run`)

## แผนที่ Evaluation Criteria → โค้ด

ทุกจุดในโค้ดมีคอมเมนต์แท็ก เช่น `// [Loop]` ค้นหาด้วยชื่อแท็กได้เลย

| หัวข้อ | อยู่ที่ไหน |
|---|---|
| **if-else** | `DisplayBoardWithCursor` (สีช่อง), `InputPlayerNames`, `MovePiece`, `IsCheckmate`, `IsStalemate`, `LoadGame`, กฎการเดินในไฟล์หมากทุกตัว |
| **switch case** | `Main` (ผลเกม), `PlayInteractive` (ปุ่มที่กด), `CreatePieceFromSymbol` (ชนิดหมาก) |
| **Function/Method** | `Main` เรียก `ShowWelcomeScreen`, `InputPlayerNames`, `PlayInteractive`, `ShowGameOver` → เรียกต่อ `DisplayBoardWithCursor`, `GetValidMoves`, `ChessBoard.*` |
| **Loop** | `while` game loop, `for` ซ้อนวนกระดาน 8x8, `foreach` ใน `IsValidMoveInList`, `while` ตรวจเส้นทางใน Rook/Bishop/Queen |
| **Array** | `ChessPiece[8,8] board` (2 มิติ), `string[] playerNames`, `string[] pieces` ตอน Load |
| **Add input data to array** | `InputPlayerNames()` รับชื่อจาก `Console.ReadLine()` ใส่ `playerNames` ครบทุกช่อง |
| **Complexity** | abstract class + inheritance, check/checkmate/stalemate, pawn promotion, File I/O, UI ลูกศร+สี |

## โครงสร้างไฟล์

- `Program.cs` — Main, หน้าจอ, ควบคุมลูกศร, รับชื่อผู้เล่น
- `ChessBoard.cs` — กระดาน, ตาเดิน, กฎ check/checkmate/stalemate, Save/Load
- `ChessPiece.cs` — abstract class แม่ของหมาก
- `Pawn.cs` `Rook.cs` `Knight.cs` `Bishop.cs` `Queen.cs` `King.cs` — กฎการเดินของหมากแต่ละชนิด

## ข้อจำกัด (ยังไม่ทำ)

- Castling, en passant
- Promotion เลือกได้แค่ควีน (อัตโนมัติ)
