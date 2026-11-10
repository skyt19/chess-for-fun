// =====================================================================
// Program.cs — จุดเริ่มโปรแกรม + หน้าจอ + ควบคุมด้วยลูกศร
// ---------------------------------------------------------------------
// แท็กหัวข้อตาม Evaluation Criteria:
//   [if-else] [switch case] [Function/Method] [Loop] [Array]
//   [Add input data to array] [Complexity] [Completeness]
// =====================================================================
using System;
using System.Collections.Generic;

namespace SimpleChess
{
    // ผลลัพธ์ของเกม (ใช้กับ switch ใน Main)
    // [switch case] ทุก case มีคำสั่งอยู่ข้างใน
    enum GameResult
    {
        WhiteWins,
        BlackWins,
        Draw,
        Quit
    }

    class Program
    {
        // [Array] array 1 มิติ เก็บชื่อผู้เล่น: [0] = White, [1] = Black
        static string[] playerNames = new string[2];

        // [Function/Method] Main = จุดเริ่มโปรแกรม เรียกทุกฟังก์ชันหลักจากที่นี่
        static void Main(string[] args)
        {
            ChessBoard board = new ChessBoard();
            bool gameRunning = true;

            ShowWelcomeScreen();   // [Function/Method] เรียกจาก Main
            InputPlayerNames();    // [Add input data to array] เรียกจาก Main

            // [Loop] game loop หลัก วนจนกว่าเกมจบหรือผู้เล่นกด Quit
            while (gameRunning)
            {
                GameResult result = PlayInteractive(board);   // [Function/Method] เรียกจาก Main

                // [switch case] แยกการทำงานตามผลของเกม
                switch (result)
                {
                    case GameResult.WhiteWins:
                        ShowGameOver(board, $"CHECKMATE! {playerNames[0]} (White) wins!");
                        gameRunning = false;
                        break;

                    case GameResult.BlackWins:
                        ShowGameOver(board, $"CHECKMATE! {playerNames[1]} (Black) wins!");
                        gameRunning = false;
                        break;

                    case GameResult.Draw:
                        ShowGameOver(board, "STALEMATE! Draw - no legal moves left.");
                        gameRunning = false;
                        break;

                    case GameResult.Quit:
                        Console.WriteLine("\nThanks for playing!");
                        gameRunning = false;
                        break;
                }
            }

            Console.WriteLine("\nPress any key to exit...");
            Console.ReadKey(true);
        }

        // [Function/Method] แสดงหน้าต้อนรับ + วิธีเล่น
        static void ShowWelcomeScreen()
        {
            Console.Clear();
            Console.WriteLine("╔═══════════════════════════════════════════════╗");
            Console.WriteLine("║       Simple Chess Game - Arrow Controls      ║");
            Console.WriteLine("╠═══════════════════════════════════════════════╣");
            Console.WriteLine("║  ↑↓←→  : Move cursor                          ║");
            Console.WriteLine("║  Enter : Select piece / Confirm move          ║");
            Console.WriteLine("║  Space : Select piece / Confirm move          ║");
            Console.WriteLine("║  Esc   : Cancel selection                     ║");
            Console.WriteLine("║  S     : Save game                            ║");
            Console.WriteLine("║  L     : Load game                            ║");
            Console.WriteLine("║  Q     : Quit                                 ║");
            Console.WriteLine("╠═══════════════════════════════════════════════╣");
            Console.WriteLine("║  White: P R Kn B Q K  (Uppercase)             ║");
            Console.WriteLine("║  Black: p r kn b q k  (Lowercase)             ║");
            Console.WriteLine("╚═══════════════════════════════════════════════╝");
            Console.WriteLine();
        }

        // [Add input data to array] รับชื่อจากผู้ใช้ใส่ playerNames ครบทุกช่อง
        // [Loop] for วนตามจำนวนช่องของ array
        // [if-else] ไม่พิมพ์ชื่อ -> ใช้ชื่อสีแทน, พิมพ์ -> เก็บชื่อที่พิมพ์
        static void InputPlayerNames()
        {
            string[] colors = { "White", "Black" };

            for (int i = 0; i < playerNames.Length; i++)
            {
                Console.Write($"Enter {colors[i]} player name: ");
                string name = Console.ReadLine();

                if (string.IsNullOrWhiteSpace(name))
                {
                    playerNames[i] = colors[i];
                }
                else
                {
                    playerNames[i] = name.Trim();
                }
            }
        }

        // [Function/Method] แสดงกระดานสุดท้าย + ข้อความจบเกม
        static void ShowGameOver(ChessBoard board, string message)
        {
            // [Completeness] แก้บั๊ก: ของเดิมส่ง null แล้ว crash ตอนรุกจน
            // ตอนนี้ IsValidMoveInList รองรับ null แล้ว และใช้ -1 = ไม่มี cursor
            DisplayBoardWithCursor(board, -1, -1, -1, -1, null);
            Console.WriteLine("\n════════════════════════════════════════════");
            Console.WriteLine("  " + message);
            Console.WriteLine("════════════════════════════════════════════");
        }

        // [Function/Method] เล่นเกม 1 รอบจนจบ คืนค่าผลลัพธ์ให้ Main
        // [Loop] while(true) วนรับปุ่มทีละครั้ง จนกว่าจะ return
        static GameResult PlayInteractive(ChessBoard board)
        {
            int cursorRow = 6;     // เริ่มที่แถวเบี้ยขาว
            int cursorCol = 4;     // คอลัมน์ e
            int selectedRow = -1;  // -1 = ยังไม่ได้เลือกหมาก
            int selectedCol = -1;
            List<Position> validMoves = new List<Position>();

            while (true)
            {
                bool whiteTurn = board.IsWhiteTurn();

                // [Complexity] เช็กจบเกมก่อนทุกตา: รุกจน (checkmate) / อับ (stalemate)
                // [if-else]
                if (board.IsCheckmate(whiteTurn))
                {
                    return whiteTurn ? GameResult.BlackWins : GameResult.WhiteWins;
                }
                else if (board.IsStalemate(whiteTurn))
                {
                    return GameResult.Draw;
                }

                DisplayBoardWithCursor(board, cursorRow, cursorCol, selectedRow, selectedCol, validMoves);

                // [if-else] แจ้งเตือนเมื่อคิงฝ่ายที่กำลังเดินโดนรุก
                if (board.IsInCheck(whiteTurn))
                {
                    Console.WriteLine(whiteTurn ? "⚠️  WHITE KING IS IN CHECK!" : "⚠️  BLACK KING IS IN CHECK!");
                }

                // [if-else] แสดงคำแนะนำตามสถานะ (ยังไม่เลือก / เลือกแล้ว)
                if (selectedRow == -1)
                {
                    Console.WriteLine("\n[↑↓←→] Move  [Enter/Space] Select  [S] Save  [L] Load  [Q] Quit");
                }
                else
                {
                    Console.WriteLine($"\nSelected: {GetPositionName(selectedRow, selectedCol)}");
                    Console.WriteLine("[↑↓←→] Choose destination  [Enter/Space] Confirm  [Esc] Cancel");
                }

                ConsoleKeyInfo key = Console.ReadKey(true);

                // [switch case] แยกการทำงานตามปุ่มที่กด
                switch (key.Key)
                {
                    case ConsoleKey.UpArrow:
                        if (cursorRow > 0) cursorRow--;
                        break;

                    case ConsoleKey.DownArrow:
                        if (cursorRow < 7) cursorRow++;
                        break;

                    case ConsoleKey.LeftArrow:
                        if (cursorCol > 0) cursorCol--;
                        break;

                    case ConsoleKey.RightArrow:
                        if (cursorCol < 7) cursorCol++;
                        break;

                    case ConsoleKey.Enter:
                    case ConsoleKey.Spacebar:
                        if (selectedRow == -1)
                        {
                            // ขั้นที่ 1: เลือกหมากของฝ่ายที่ถึงตา
                            ChessPiece piece = board.GetPiece(cursorRow, cursorCol);
                            if (piece != null && piece.IsWhite == whiteTurn)
                            {
                                validMoves = GetValidMoves(board, cursorRow, cursorCol);

                                if (validMoves.Count == 0)
                                {
                                    Console.WriteLine("\nNo valid moves for this piece!");
                                    System.Threading.Thread.Sleep(1000);
                                }
                                else
                                {
                                    selectedRow = cursorRow;
                                    selectedCol = cursorCol;
                                }
                            }
                        }
                        else
                        {
                            // ขั้นที่ 2: เลือกช่องปลายทาง แล้วเดินจริง
                            if (IsValidMoveInList(cursorRow, cursorCol, validMoves) &&
                                board.MovePiece(selectedRow, selectedCol, cursorRow, cursorCol))
                            {
                                selectedRow = -1;
                                selectedCol = -1;
                                validMoves.Clear();
                            }
                            else
                            {
                                Console.WriteLine("\nInvalid move!");
                                System.Threading.Thread.Sleep(800);
                            }
                        }
                        break;

                    case ConsoleKey.Escape:
                        selectedRow = -1;
                        selectedCol = -1;
                        validMoves.Clear();
                        break;

                    case ConsoleKey.S:
                        Console.Write("\nEnter filename to save: ");
                        string saveFile = Console.ReadLine();
                        if (!string.IsNullOrWhiteSpace(saveFile))
                        {
                            board.SaveGame(saveFile.Trim());
                            System.Threading.Thread.Sleep(1500);
                        }
                        break;

                    case ConsoleKey.L:
                        Console.Write("\nEnter filename to load: ");
                        string loadFile = Console.ReadLine();
                        if (!string.IsNullOrWhiteSpace(loadFile))
                        {
                            // [if-else] ล้างการเลือกเฉพาะเมื่อโหลดสำเร็จ
                            if (board.LoadGame(loadFile.Trim()))
                            {
                                selectedRow = -1;
                                selectedCol = -1;
                                validMoves.Clear();
                            }
                            System.Threading.Thread.Sleep(1500);
                        }
                        break;

                    case ConsoleKey.Q:
                        Console.Write("\nAre you sure you want to quit? (Y/N): ");
                        if (Console.ReadKey(true).Key == ConsoleKey.Y)
                            return GameResult.Quit;
                        break;
                }
            }
        }

        // [Function/Method] วาดกระดาน 8x8 พร้อมสี cursor / หมากที่เลือก / ช่องที่เดินได้
        // [Loop] for ซ้อน 2 ชั้น วนทุกช่องของ array 2 มิติ
        // [if-else] เลือกสีของแต่ละช่องตามสถานะ
        static void DisplayBoardWithCursor(ChessBoard board, int cursorRow, int cursorCol,
            int selectedRow, int selectedCol, List<Position> validMoves)
        {
            Console.Clear();
            Console.WriteLine("\n╔═══════════════════════════════════════════════════╗");
            Console.WriteLine("║           SIMPLE CHESS - ARROW CONTROLS           ║");
            Console.WriteLine("╚═══════════════════════════════════════════════════╝\n");

            Console.WriteLine("    a    b    c    d    e    f    g    h");
            Console.WriteLine("  ┌────┬────┬────┬────┬────┬────┬────┬────┐");

            for (int row = 0; row < 8; row++)
            {
                Console.Write((8 - row) + " │");

                for (int col = 0; col < 8; col++)
                {
                    bool isCursor = (row == cursorRow && col == cursorCol);
                    bool isSelected = (row == selectedRow && col == selectedCol);
                    bool isValidMove = IsValidMoveInList(row, col, validMoves);

                    if (isCursor && isSelected)
                    {
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.BackgroundColor = ConsoleColor.DarkBlue;
                    }
                    else if (isCursor)
                    {
                        Console.ForegroundColor = ConsoleColor.Black;
                        Console.BackgroundColor = ConsoleColor.Cyan;
                    }
                    else if (isSelected)
                    {
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.BackgroundColor = ConsoleColor.DarkGreen;
                    }
                    else if (isValidMove)
                    {
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.BackgroundColor = ConsoleColor.DarkGray;
                    }
                    else
                    {
                        // ลายกระดานสลับสี
                        if ((row + col) % 2 == 0)
                        {
                            Console.ForegroundColor = ConsoleColor.White;
                            Console.BackgroundColor = ConsoleColor.DarkGray;
                        }
                        else
                        {
                            Console.ForegroundColor = ConsoleColor.White;
                            Console.BackgroundColor = ConsoleColor.Black;
                        }
                    }

                    ChessPiece piece = board.GetPiece(row, col);
                    string symbol = piece == null ? "  " : piece.GetSymbol();
                    Console.Write(" " + symbol.PadRight(2) + " ");

                    Console.ResetColor();
                    Console.Write("│");
                }

                Console.Write(" " + (8 - row));

                if (row < 7)
                {
                    Console.WriteLine("\n  ├────┼────┼────┼────┼────┼────┼────┼────┤");
                }
            }

            Console.WriteLine("\n  └────┴────┴────┴────┴────┴────┴────┴────┘");
            Console.WriteLine("    a    b    c    d    e    f    g    h\n");

            // [Array] ดึงชื่อผู้เล่นจาก playerNames มาแสดงตาเดิน
            Console.WriteLine(board.IsWhiteTurn()
                ? $"  Turn: WHITE - {playerNames[0]} (Uppercase)"
                : $"  Turn: BLACK - {playerNames[1]} (Lowercase)");
        }

        // [Function/Method] หาช่องที่หมากตัวนี้เดินได้ "ตามกฎจริง"
        // [Loop] for ซ้อน ลองทุกช่องปลายทาง 8x8
        // [Completeness] แก้: ของเดิมไม่เช็กว่าเดินแล้วคิงตัวเองโดนรุก -> โชว์ช่องเขียวที่กดแล้วเดินไม่ได้
        static List<Position> GetValidMoves(ChessBoard board, int fromRow, int fromCol)
        {
            List<Position> moves = new List<Position>();

            for (int toRow = 0; toRow < 8; toRow++)
            {
                for (int toCol = 0; toCol < 8; toCol++)
                {
                    if (board.IsLegalMove(fromRow, fromCol, toRow, toCol))
                    {
                        moves.Add(new Position(toRow, toCol));
                    }
                }
            }

            return moves;
        }

        // [Function/Method] เช็กว่าช่อง (row, col) อยู่ในรายการช่องที่เดินได้หรือไม่
        // [Loop] foreach วนทุกตำแหน่งใน list
        // [Completeness] แก้บั๊ก crash: รองรับ validMoves = null
        static bool IsValidMoveInList(int row, int col, List<Position> validMoves)
        {
            if (validMoves == null)
                return false;

            foreach (Position pos in validMoves)
            {
                if (pos.Row == row && pos.Col == col)
                    return true;
            }
            return false;
        }

        // [Function/Method] แปลงตำแหน่ง array เป็นชื่อช่องหมากรุก เช่น (6,4) -> "e2"
        static string GetPositionName(int row, int col)
        {
            char colName = (char)('a' + col);
            int rowName = 8 - row;
            return $"{colName}{rowName}";
        }
    }

    // [Complexity] class เก็บพิกัดแถว/คอลัมน์ 1 ช่อง
    class Position
    {
        public int Row { get; set; }
        public int Col { get; set; }

        public Position(int row, int col)
        {
            Row = row;
            Col = col;
        }
    }
}
