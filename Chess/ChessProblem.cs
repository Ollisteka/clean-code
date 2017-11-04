using System.Linq;

namespace Chess
{
	public class ChessProblem
	{
		private static Board board;
		public static ChessStatus ChessStatus;

		public static void LoadFrom(string[] lines)
		{
			board = new BoardParser().ParseBoard(lines);
		}

		private static bool CheckForSafeMoves(Location whiteLocation, Location locTo)
		{
			using (var tempBoard = new TemporaryPieceMove(board, whiteLocation, locTo, board.GetPiece(whiteLocation)))
			{
				tempBoard.Move();
				if (!IsCheckForWhite(tempBoard.board))
					return true;
			}
			return false;
		}

		// Определяет мат, шах или пат белым.
		public static void CalculateChessStatus()
		{
			var isCheck = IsCheckForWhite(board);

			var hasMoves = board.GetPieces(PieceColor.White)
				.Any(whiteLocation =>
						board.GetPiece(whiteLocation)
							.GetMoves(whiteLocation, board)
							.Any(x => CheckForSafeMoves(whiteLocation, x)));

//			if (isCheck)
//				if (hasMoves)
//					ChessStatus = ChessStatus.Check;
//				else ChessStatus = ChessStatus.Mate;
//			else if (hasMoves) ChessStatus = ChessStatus.Ok;
//			else ChessStatus = ChessStatus.Stalemate;
			if (isCheck && hasMoves)
				ChessStatus = ChessStatus.Check;

		}

		// check — это шах
		private static bool IsCheckForWhite(Board aBoard)
		{
			return (from blackPiecesLocation in aBoard.GetPieces(PieceColor.Black)
					let blackPiece = aBoard.GetPiece(blackPiecesLocation)
					select blackPiece.GetMoves(blackPiecesLocation, aBoard))
				.Any(moves => moves.Any(destination => aBoard.GetPiece(destination)
					.Is(PieceColor.White, PieceType.King)));
		}
	}
}