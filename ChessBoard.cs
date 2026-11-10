// =====================================================================
// ChessBoard.cs — กระดาน + ตาเดิน + กฎ check / checkmate / stalemate + Save/Load
// ---------------------------------------------------------------------
// แท็กหัวข้อ: [Array] [Loop] [if-else] [switch case] [Function/Method]
//            [Complexity] [Completeness]
// =====================================================================
using System;
using System.IO;

namespace SimpleChess
{
    public class ChessBoard
    {
        // [Array] array 2 มิติ 8x8 = กระดาน
        // แต่ละช่องเก็บหมาก 1 ตัว หรือ null (ช่องว่าง)
        private ChessPiece[,] board;
        private bool isWhiteTurn;   // true = ตาขาว, false = ตาดำ

        public ChessBoard()
        {
            board = new ChessPiece[8, 8];
            isWhiteTurn = true;     // ขาวเดินก่อน
            InitializeBoard();
        }

        // [Function/Method] วางหมากตำแหน่งเริ่มต้นลง array
        // [Loop] for วางเบี้ยแถวละ 8 ตัว
        private void InitializeBoard()
        {
            // หมากดำ (แถว 0-1 = ด้านบน)
            board[0, 0] = new Rook(false, 0, 0);
            board[0, 1] = new Knight(false, 0, 1);
            board[0, 2] = new Bishop(false, 0, 2);
            board[0, 3] = new Queen(false, 0, 3);
            board[0, 4] = new King(false, 0, 4);
            board[0, 5] = new Bishop(false, 0, 5);
            board[0, 6] = new Knight(false, 0, 6);
            board[0, 7] = new Rook(false, 0, 7);

            for (int col = 0; col < 8; col++)
            {
                board[1, col] = new Pawn(false, 1, col);
            }

            // หมากขาว (แถว 6-7 = ด้านล่าง)
            for (int col = 0; col < 8; col++)
            {
                board[6, col] = new Pawn(true, 6, col);
            }

            board[7, 0] = new Rook(true, 7, 0);
            board[7, 1] = new Knight(true, 7, 1);
            board[7, 2] = new Bishop(true, 7, 2);
            board[7, 3] = new Queen(true, 7, 3);
            board[7, 4] = new King(true, 7, 4);
            board[7, 5] = new Bishop(true, 7, 5);
            board[7, 6] = new Knight(true, 7, 6);
            board[7, 7] = new Rook(true, 7, 7);
        }

        // [Function/Method] คืนหมากที่ช่อง (row, col)
        // [if-else] นอกกระดาน -> คืน null กัน IndexOutOfRange
        public ChessPiece GetPiece(int row, int col)
        {
            if (row < 0 || row >= 8 || col < 0 || col >= 8)
                return null;
            return board[row, col];
        }

        // [Function/Method] ตอนนี้ถึงตาขาวหรือไม่
        public bool IsWhiteTurn()
        {
            return isWhiteTurn;
        }

        // [Function/Method] [Complexity] เช็กว่าเดินได้ "ตามกฎจริง"
        // = เดินได้ตามรูปแบบของหมาก + เดินแล้วคิงตัวเองไม่โดนรุก
        public bool IsLegalMove(int fromRow, int fromCol, int toRow, int toCol)
        {
            ChessPiece piece = GetPiece(fromRow, fromCol);

            if (piece == null)
                return false;

            if (!piece.IsValidMove(toRow, toCol, board))
                return false;

            return !WouldBeInCheck(fromRow, fromCol, toRow, toCol, piece.IsWhite);
        }

        // [Function/Method] เดินหมากจริง แล้วสลับตา
        // [if-else] ตรวจ: มีหมากไหม / ใช่ของฝ่ายที่ถึงตาไหม / ถูกกฎไหม
        public bool MovePiece(int fromRow, int fromCol, int toRow, int toCol)
        {
            ChessPiece piece = GetPiece(fromRow, fromCol);

            if (piece == null || piece.IsWhite != isWhiteTurn)
                return false;

            if (!IsLegalMove(fromRow, fromCol, toRow, toCol))
                return false;

            // ย้ายหมากใน array (ถ้ามีหมากฝ่ายตรงข้ามอยู่ = ถูกกินโดยการเขียนทับ)
            board[toRow, toCol] = piece;
            board[fromRow, fromCol] = null;
            piece.Row = toRow;
            piece.Col = toCol;

            // [Complexity] Pawn promotion: เบี้ยเดินถึงแถวสุดท้าย -> เปลี่ยนเป็นควีนอัตโนมัติ
            // [if-else]
            if (piece is Pawn && (toRow == 0 || toRow == 7))
            {
                board[toRow, toCol] = new Queen(piece.IsWhite, toRow, toCol);
            }

            isWhiteTurn = !isWhiteTurn;   // สลับตา
            return true;
        }

        // [Function/Method] [Complexity] คิงของฝ่ายนี้โดนรุกอยู่หรือไม่
        // [Loop] for ซ้อน: รอบแรกหาคิง, รอบสองเช็กว่าหมากฝ่ายตรงข้ามตัวไหนกินคิงได้
        public bool IsInCheck(bool whiteKing)
        {
            int kingRow = -1, kingCol = -1;

            for (int row = 0; row < 8 && kingRow == -1; row++)
            {
                for (int col = 0; col < 8; col++)
                {
                    ChessPiece piece = board[row, col];
                    if (piece != null && piece is King && piece.IsWhite == whiteKing)
                    {
                        kingRow = row;
                        kingCol = col;
                        break;
                    }
                }
            }

            // [if-else] ไม่เจอคิง (เช่น ไฟล์เซฟแปลก) -> ถือว่าไม่โดนรุก
            if (kingRow == -1)
                return false;

            for (int row = 0; row < 8; row++)
            {
                for (int col = 0; col < 8; col++)
                {
                    ChessPiece piece = board[row, col];
                    if (piece != null && piece.IsWhite != whiteKing &&
                        piece.IsValidMove(kingRow, kingCol, board))
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        // [Function/Method] [Complexity] ทดลองเดินชั่วคราว -> เช็กว่าคิงโดนรุกไหม -> ย้อนคืน
        private bool WouldBeInCheck(int fromRow, int fromCol, int toRow, int toCol, bool isWhite)
        {
            ChessPiece piece = board[fromRow, fromCol];
            ChessPiece capturedPiece = board[toRow, toCol];
            int oldRow = piece.Row;
            int oldCol = piece.Col;

            // ทดลองเดิน
            board[toRow, toCol] = piece;
            board[fromRow, fromCol] = null;
            piece.Row = toRow;
            piece.Col = toCol;

            bool inCheck = IsInCheck(isWhite);

            // ย้อนคืนสภาพเดิม
            board[fromRow, fromCol] = piece;
            board[toRow, toCol] = capturedPiece;
            piece.Row = oldRow;
            piece.Col = oldCol;

            return inCheck;
        }

        // [Function/Method] [Complexity] ฝ่ายนี้ยังมีตาเดินที่ถูกกฎเหลืออยู่ไหม
        // [Loop] for 4 ชั้น: ทุกช่องต้นทาง x ทุกช่องปลายทาง
        private bool HasAnyLegalMove(bool whitePlayer)
        {
            for (int fromRow = 0; fromRow < 8; fromRow++)
            {
                for (int fromCol = 0; fromCol < 8; fromCol++)
                {
                    ChessPiece piece = board[fromRow, fromCol];
                    if (piece == null || piece.IsWhite != whitePlayer)
                        continue;

                    for (int toRow = 0; toRow < 8; toRow++)
                    {
                        for (int toCol = 0; toCol < 8; toCol++)
                        {
                            if (IsLegalMove(fromRow, fromCol, toRow, toCol))
                                return true;
                        }
                    }
                }
            }
            return false;
        }

        // [Function/Method] [Complexity] รุกจน = โดนรุก + ไม่มีตาเดินหนี
        public bool IsCheckmate(bool whitePlayer)
        {
            if (IsInCheck(whitePlayer))
            {
                return !HasAnyLegalMove(whitePlayer);
            }
            else
            {
                return false;
            }
        }

        // [Function/Method] [Complexity] อับ (เสมอ) = ไม่โดนรุก + ไม่มีตาเดินเลย
        // [Completeness] ของเดิมไม่มี -> เกมค้าง ผู้เล่นเดินไม่ได้แต่เกมไม่จบ
        public bool IsStalemate(bool whitePlayer)
        {
            if (IsInCheck(whitePlayer))
            {
                return false;
            }
            else
            {
                return !HasAnyLegalMove(whitePlayer);
            }
        }

        // [Function/Method] [Complexity] บันทึกเกมลงไฟล์ข้อความ (File I/O)
        // รูปแบบไฟล์: บรรทัดแรก WHITE/BLACK, ตามด้วยกระดาน 8 บรรทัด
        // [Loop] for ซ้อน วนทุกช่อง  [if-else] ช่องว่างเขียน ".."
        public void SaveGame(string filename)
        {
            try
            {
                using (StreamWriter writer = new StreamWriter(filename))
                {
                    writer.WriteLine(isWhiteTurn ? "WHITE" : "BLACK");

                    for (int row = 0; row < 8; row++)
                    {
                        for (int col = 0; col < 8; col++)
                        {
                            if (board[row, col] == null)
                            {
                                writer.Write(".. ");
                            }
                            else
                            {
                                writer.Write(board[row, col].GetSymbol().PadRight(3));
                            }
                        }
                        writer.WriteLine();
                    }
                }
                Console.WriteLine($"Game saved to {filename}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving game: {ex.Message}");
            }
        }

        // [Function/Method] [Complexity] โหลดเกมจากไฟล์ (File I/O)
        // [Loop] for อ่านทีละแถว/ทีละช่อง
        // [if-else] ตรวจไฟล์ทุกขั้น ถ้าเสียจะไม่แตะเกมปัจจุบัน
        // [Completeness] แก้บั๊ก: Split(' ') เดิมได้ "" ปนมา -> crash กลางทาง + กระดานหาย
        public bool LoadGame(string filename)
        {
            try
            {
                if (!File.Exists(filename))
                {
                    Console.WriteLine("Save file not found.");
                    return false;
                }

                // โหลดลง array ชั่วคราวก่อน สำเร็จทั้งหมดค่อยแทนกระดานจริง
                ChessPiece[,] newBoard = new ChessPiece[8, 8];
                bool newTurn = true;

                using (StreamReader reader = new StreamReader(filename))
                {
                    string turn = reader.ReadLine();
                    if (turn == "WHITE")
                    {
                        newTurn = true;
                    }
                    else if (turn == "BLACK")
                    {
                        newTurn = false;
                    }
                    else
                    {
                        Console.WriteLine("Invalid save file: first line must be WHITE or BLACK.");
                        return false;
                    }

                    for (int row = 0; row < 8; row++)
                    {
                        string line = reader.ReadLine();
                        if (line == null)
                        {
                            Console.WriteLine("Invalid save file: missing board rows.");
                            return false;
                        }

                        // RemoveEmptyEntries = ตัดช่องว่างซ้ำที่เกิดจาก PadRight(3) ตอน save
                        string[] pieces = line.Split(' ', StringSplitOptions.RemoveEmptyEntries);
                        if (pieces.Length != 8)
                        {
                            Console.WriteLine($"Invalid save file: row {row + 1} must have 8 squares.");
                            return false;
                        }

                        for (int col = 0; col < 8; col++)
                        {
                            string symbol = pieces[col];
                            if (symbol == "..")
                            {
                                newBoard[row, col] = null;
                            }
                            else
                            {
                                ChessPiece piece = CreatePieceFromSymbol(symbol, row, col);
                                if (piece == null)
                                {
                                    Console.WriteLine($"Invalid save file: unknown piece '{symbol}'.");
                                    return false;
                                }
                                newBoard[row, col] = piece;
                            }
                        }
                    }
                }

                board = newBoard;
                isWhiteTurn = newTurn;
                Console.WriteLine($"Game loaded from {filename}");
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading game: {ex.Message}");
                return false;
            }
        }

        // [Function/Method] แปลงสัญลักษณ์ในไฟล์ -> object หมาก
        // [switch case] แยกชนิดหมากตามตัวอักษร (ตัวใหญ่ = ขาว, ตัวเล็ก = ดำ)
        private ChessPiece CreatePieceFromSymbol(string symbol, int row, int col)
        {
            bool isWhite = char.IsUpper(symbol[0]);
            string pieceType = symbol.ToLower();

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
                    return null;   // สัญลักษณ์ไม่รู้จัก
            }
        }
    }
}
