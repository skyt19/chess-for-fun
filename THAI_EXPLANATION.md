# คำอธิบายโปรแกรมหมากรุกภาษา C# (Thai Explanation)

> ⚠️ **หมายเหตุ:** ส่วนที่พูดถึงการพิมพ์คำสั่ง `move e2 e4` เป็นของเวอร์ชันเก่า เวอร์ชันปัจจุบันควบคุมด้วยปุ่มลูกศร ดู `README.md` และ `HOW_TO_RUN.txt`


## ภาพรวมโปรแกรม

โปรแกรมนี้เป็นเกมหมากรุกแบบ Command Line ที่เขียนด้วยภาษา C# โดยมีฟีเจอร์ครบถ้วนตามที่กำหนด ผู้เล่นสามารถเล่นหมากรุกได้ 2 คน (ขาว vs ดำ) ผ่านหน้าจอคำสั่ง

---

## 1. Problem Solving & Algorithm (การแก้ปัญหาและอัลกอริทึม)

### อัลกอริทึมการตรวจสอบ Check (รุกฆาต)
```csharp
public bool IsInCheck(bool whiteKing)
{
    // 1. หาตำแหน่งของกษัตริย์ที่ต้องการตรวจสอบ
    // 2. วนลูปตรวจสอบทุกหมากของฝ่ายตรงข้าม
    // 3. ถ้าหมากใดๆ สามารถกินกษัตริย์ได้ = อยู่ในสถานะ Check
}
```

**วิธีการทำงาน:**
- วนตรวจสอบทั้งกระดาน (8x8) เพื่อหาตำแหน่งของ King
- ตรวจสอบว่ามีหมากฝ่ายตรงข้าม สามารถเดินมากินกษัตริย์ได้หรือไม่

### อัลกอริทึมการตรวจสอบ Checkmate (จนตรอก)
```csharp
public bool IsCheckmate(bool whitePlayer)
{
    // 1. ตรวจสอบว่าอยู่ในสถานะ Check หรือไม่ (ถ้าไม่ใช่ = ไม่ใช่ checkmate)
    // 2. ลองเดินทุกหมากที่เป็นไปได้
    // 3. ถ้าไม่มีหมากใดที่เดินแล้วหลุดจาก Check = Checkmate
}
```

### อัลกอริทึมการเดินหมาก
แต่ละชนิดของหมากมีกฎการเดินที่แตกต่างกัน:

**Pawn (เบี้ย):**
- เดินหน้าได้ 1 ช่อง
- เดินหน้าได้ 2 ช่องในตาแรก
- กินได้เฉพาะทแยงมุม

**Rook (เรือ):**
- เดินแนวนอนหรือแนวตั้งได้ไม่จำกัด
- ต้องไม่มีหมากกีดขวาง

**Knight (ม้า):**
- เดินแบบตัว L (2 ช่องทิศหนึ่ง + 1 ช่องทิศตั้งฉาก)
- ข้ามหมากอื่นได้

**Bishop (โคน):**
- เดินแนวทแยงได้ไม่จำกัด
- ต้องไม่มีหมากกีดขวาง

**Queen (ควีน):**
- เดินได้ทุกทิศทาง (รวม Rook + Bishop)
- เดินได้ไกลไม่จำกัด

**King (กษัตริย์):**
- เดินได้ทุกทิศทาง แต่ได้แค่ 1 ช่อง

---

## 2. If-Else (การใช้เงื่อนไข)

### ตัวอย่างในการตรวจสอบการเดิน Pawn
```csharp
// ตรวจสอบว่าเดินหน้า 1 ช่อง
if (toCol == Col && toRow == Row + direction)
{
    if (board[toRow, toCol] == null)
        return true;
}

// ตรวจสอบว่าเดินหน้า 2 ช่องจากตำแหน่งเริ่มต้น
if (toCol == Col && toRow == Row + (2 * direction) && Row == startRow)
{
    if (board[toRow, toCol] == null && board[Row + direction, Col] == null)
        return true;
}
```

### ตัวอย่างในการตรวจสอบเทิร์น
```csharp
if (piece == null || piece.IsWhite != isWhiteTurn)
    return false; // ไม่ใช่หมากของผู้เล่นที่ต้องเดิน
```

---

## 3. Switch-Case (การเลือกกรณี)

### ใช้ในการประมวลผลคำสั่ง
```csharp
switch (command)
{
    case "move":
        ProcessMove(board, parts[1], parts[2]);
        break;
    
    case "save":
        board.SaveGame(parts[1]);
        break;
    
    case "load":
        board.LoadGame(parts[1]);
        break;
    
    case "quit":
        gameRunning = false;
        break;
    
    default:
        Console.WriteLine("Unknown command");
        break;
}
```

### ใช้ในการสร้างหมากจากสัญลักษณ์
```csharp
switch (pieceType)
{
    case "p":
        return new Pawn(isWhite, row, col);
    case "r":
        return new Rook(isWhite, row, col);
    case "kn":
        return new Knight(isWhite, row, col);
    case "b":
        return new Bishop(isWhite, row, col);
    case "q":
        return new Queen(isWhite, row, col);
    case "k":
        return new King(isWhite, row, col);
    default:
        return null;
}
```

---

## 4. Functions/Methods (ฟังก์ชัน/เมธอด)

### ฟังก์ชันหลักที่สำคัญ:

**1. MovePiece()** - ย้ายหมาก
```csharp
public bool MovePiece(int fromRow, int fromCol, int toRow, int toCol)
```
- ตรวจสอบว่าหมากเป็นของผู้เล่นที่ต้องเดินหรือไม่
- ตรวจสอบว่าเดินได้ตามกฎหรือไม่
- ตรวจสอบว่าเดินแล้วกษัตริย์จะโดน Check หรือไม่
- ทำการย้ายหมาก
- สลับเทิร์น

**2. IsInCheck()** - ตรวจสอบว่าอยู่ในสถานะรุกฆาต
```csharp
public bool IsInCheck(bool whiteKing)
```

**3. IsCheckmate()** - ตรวจสอบว่าจนตรอกหรือไม่
```csharp
public bool IsCheckmate(bool whitePlayer)
```

**4. DisplayBoard()** - แสดงกระดาน
```csharp
public void DisplayBoard()
```

**5. IsValidMove()** - ตรวจสอบการเดินของหมากแต่ละชนิด (Abstract method)
```csharp
public abstract bool IsValidMove(int toRow, int toCol, ChessPiece[,] board);
```

---

## 5. Looping (การวนซ้ำ)

### For Loop - ใช้ในการตั้งกระดาน
```csharp
for (int col = 0; col < 8; col++)
{
    board[1, col] = new Pawn(false, 1, col); // ตั้งเบี้ยดำ
}
```

### For Loop - ใช้ในการแสดงกระดาน
```csharp
for (int row = 0; row < 8; row++)
{
    for (int col = 0; col < 8; col++)
    {
        // แสดงหมากในแต่ละช่อง
    }
}
```

### While Loop - ตรวจสอบทางเดินของหมาก
```csharp
while (currentRow != toRow || currentCol != toCol)
{
    if (board[currentRow, currentCol] != null)
        return false; // มีหมากขวาง
    
    currentRow += rowStep;
    currentCol += colStep;
}
```

### While Loop - Loop หลักของเกม
```csharp
while (gameRunning)
{
    board.DisplayBoard();
    // รับคำสั่งจากผู้เล่น
    // ประมวลผลคำสั่ง
}
```

---

## 6. Array (อาร์เรย์)

### Array 2D (8x8) สำหรับกระดานหมากรุก
```csharp
private ChessPiece[,] board; // อาร์เรย์ 2 มิติ ขนาด 8x8

board = new ChessPiece[8, 8];
```

**การเข้าถึง:**
```csharp
board[row, col] = new Pawn(true, row, col); // กำหนดค่า
ChessPiece piece = board[row, col]; // อ่านค่า
```

### Array 1D - แยกคำสั่ง
```csharp
string[] parts = input.Split(' '); // ["move", "e2", "e4"]
string command = parts[0]; // "move"
string from = parts[1];    // "e2"
string to = parts[2];      // "e4"
```

---

## 7. File I/O (การจัดการไฟล์)

### Save Game (บันทึกเกม)
```csharp
public void SaveGame(string filename)
{
    using (StreamWriter writer = new StreamWriter(filename))
    {
        // เขียนข้อมูลว่าเทิร์นใคร
        writer.WriteLine(isWhiteTurn ? "WHITE" : "BLACK");
        
        // เขียนสถานะกระดานทั้งหมด
        for (int row = 0; row < 8; row++)
        {
            for (int col = 0; col < 8; col++)
            {
                // เขียนสัญลักษณ์หมาก
            }
        }
    }
}
```

**รูปแบบไฟล์ที่บันทึก:**
```
WHITE
r  kn b  q  k  b  kn r  
p  p  p  p  p  p  p  p  
.. .. .. .. .. .. .. .. 
.. .. .. .. .. .. .. .. 
.. .. .. .. .. .. .. .. 
.. .. .. .. .. .. .. .. 
P  P  P  P  P  P  P  P  
R  Kn B  Q  K  B  Kn R  
```

### Load Game (โหลดเกม)
```csharp
public bool LoadGame(string filename)
{
    using (StreamReader reader = new StreamReader(filename))
    {
        // อ่านว่าเทิร์นใคร
        string turn = reader.ReadLine();
        
        // อ่านสถานะกระดาน
        for (int row = 0; row < 8; row++)
        {
            string line = reader.ReadLine();
            // แปลงสัญลักษณ์กลับเป็นหมาก
        }
    }
}
```

---

## 8. Modular Programming (การเขียนโปรแกรมแบบแยกส่วน)

### โครงสร้างโมดูล:

**1. ChessPiece.cs** - Base Class (คลาสแม่)
```csharp
public abstract class ChessPiece
{
    public bool IsWhite;
    public int Row, Col;
    public abstract bool IsValidMove(...);
    public abstract string GetSymbol();
}
```

**2. Pawn.cs, Rook.cs, Knight.cs, Bishop.cs, Queen.cs, King.cs**
- คลาสย่อยสำหรับหมากแต่ละชนิด
- สืบทอดมาจาก ChessPiece
- Implement การเดินเฉพาะของหมากแต่ละชนิด

**3. ChessBoard.cs** - จัดการกระดานและเกม
- เก็บสถานะกระดาน (2D Array)
- ตรวจสอบ Check/Checkmate
- จัดการการเดินหมาก
- Save/Load เกม

**4. Program.cs** - User Interface และ Main Loop
- รับคำสั่งจากผู้เล่น
- แสดงกระดาน
- จัดการ game loop

### ความสัมพันธ์ของโมดูล:
```
Program.cs
    ↓ (ใช้)
ChessBoard.cs
    ↓ (ใช้)
ChessPiece.cs (abstract)
    ↓ (สืบทอด)
├── Pawn.cs
├── Rook.cs
├── Knight.cs
├── Bishop.cs
├── Queen.cs
└── King.cs
```

---

## 9. Integration (การรวมระบบ)

### การทำงานร่วมกันของทุกโมดูล:

**1. ผู้เล่นป้อนคำสั่ง (Program.cs)**
```
"move e2 e4"
```

**2. แปลงเป็นพิกัดในอาร์เรย์ (Program.cs)**
```csharp
int fromCol = ChessBoard.ColToIndex('e'); // 4
int fromRow = ChessBoard.RowToIndex(2);   // 6
int toCol = ChessBoard.ColToIndex('e');   // 4
int toRow = ChessBoard.RowToIndex(4);     // 4
```

**3. เรียก MovePiece (ChessBoard.cs)**
```csharp
board.MovePiece(fromRow, fromCol, toRow, toCol)
```

**4. ตรวจสอบหมาก (ChessBoard.cs + ChessPiece classes)**
- ตรวจสอบว่าเป็นหมากของผู้เล่นที่ถูกต้อง (if-else)
- เรียก piece.IsValidMove() (Polymorphism)
- ตรวจสอบว่าเดินแล้วจะโดน Check หรือไม่ (Algorithm)

**5. อัพเดทกระดาน**
```csharp
board[toRow, toCol] = piece;
board[fromRow, fromCol] = null;
```

**6. ตรวจสอบ Check/Checkmate**
```csharp
if (board.IsCheckmate(true))
    Console.WriteLine("Black wins!");
```

**7. บันทึก/โหลดเกม (File I/O)**
```csharp
board.SaveGame("mygame.txt");
board.LoadGame("mygame.txt");
```

---

## สัญลักษณ์หมาก (Piece Notation)

### ขาว (Uppercase):
- **P** = Pawn (เบี้ย)
- **R** = Rook (เรือ)
- **Kn** = Knight (ม้า)
- **B** = Bishop (โคน)
- **Q** = Queen (ควีน)
- **K** = King (กษัตริย์)

### ดำ (Lowercase):
- **p** = pawn (เบี้ย)
- **r** = rook (เรือ)
- **kn** = knight (ม้า)
- **b** = bishop (โคน)
- **q** = queen (ควีน)
- **k** = king (กษัตริย์)

---

## วิธีเล่น

### 1. Compile และ Run
```bash
dotnet build
dotnet run
```

### 2. คำสั่งที่ใช้ได้:
- **move <จาก> <ไป>** - เดินหมาก เช่น "move e2 e4"
- **save <ชื่อไฟล์>** - บันทึกเกม
- **load <ชื่อไฟล์>** - โหลดเกม
- **quit** - ออกจากเกม

### 3. รูปแบบการเดิน:
- ใช้ตัวอักษร a-h สำหรับคอลัมน์
- ใช้เลข 1-8 สำหรับแถว
- ตัวอย่าง: e2, e4, a7, h1

### 4. กฎการเล่น:
- ขาวเดินก่อน
- สลับกันเดินทีละคน
- ห้ามเดินให้กษัตริย์ตัวเองโดน Check
- เกมจบเมื่อเกิด Checkmate

---

## ตัวอย่างการเล่น

```
   a  b  c  d  e  f  g  h
  -------------------------
8 | r  kn b  q  k  b  kn r  | 8
7 | p  p  p  p  p  p  p  p  | 7
6 | .  .  .  .  .  .  .  .  | 6
5 | .  .  .  .  .  .  .  .  | 5
4 | .  .  .  .  .  .  .  .  | 4
3 | .  .  .  .  .  .  .  .  | 3
2 | P  P  P  P  P  P  P  P  | 2
1 | R  Kn B  Q  K  B  Kn R  | 1
  -------------------------
   a  b  c  d  e  f  g  h

White's turn

Enter command: move e2 e4
Moved from e2 to e4

Enter command: move e7 e5
Moved from e7 to e5

Enter command: save game1.txt
Game saved to game1.txt
```

---

## สรุป

โปรแกรมนี้แสดงให้เห็นถึง:

1. **Problem Solving & Algorithm** - อัลกอริทึมตรวจสอบ Check/Checkmate และการเดินหมาก
2. **If-Else** - ใช้ในการตรวจสอบเงื่อนไขต่างๆ มากมาย
3. **Switch-Case** - ใช้ในการจัดการคำสั่งและสร้างหมาก
4. **Functions/Methods** - แยกฟังก์ชันชัดเจน มีหน้าที่เฉพาะ
5. **Looping** - ใช้ทั้ง for และ while ในหลายจุด
6. **Array (1D & 2D)** - ใช้ 2D array เป็นกระดาน และ 1D array แยกคำสั่ง
7. **File I/O** - บันทึกและโหลดเกมได้
8. **Modular Programming** - แยกโค้ดเป็นคลาสต่างๆ อย่างชัดเจน
9. **Integration** - ทุกส่วนทำงานร่วมกันได้อย่างลงตัว

โปรแกรมนี้เหมาะสำหรับการเรียนรู้และเข้าใจแนวคิดพื้นฐานของการเขียนโปรแกรม C# ผ่านโปรเจกต์จริงที่สามารถเล่นได้!
