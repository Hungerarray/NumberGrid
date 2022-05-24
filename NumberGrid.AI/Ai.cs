using System.Text;

namespace NumberGrid.AI;

public class Ai
{
	public int[] Initial { get; init; }
	public int[] Goal { get; init; }
	public int Depth { get; init; }

	private readonly List<int> Visited = new();
	private readonly Dictionary<int, int[]> movesTable = new();
	private readonly Dictionary<int, int> heuristicTable = new();
	private List<int[]>? solution;

	public Ai(int[] initial, int[] goal, int depth = 2)
	{
		Initial = initial;
		Goal = goal;
		Depth = depth;
	}

	public bool IsValid
	{
		get
		{
			int initInversion = countInversions(Initial);
			int goalInversion = countInversions(Goal);
			return (initInversion & 0x1) == (goalInversion & 0x1);
		}
	}

	public IEnumerable<List<int[]>> Compute()
	{
		if (!IsValid)
			yield break;
		if (solution is not null)
		{
			yield return solution;
			yield break;
		}	

		int initialHashCode = HashCode(Initial);
		int goalHashCode = HashCode(Goal);
		if (initialHashCode == goalHashCode)
		{
			yield return new() { Initial };
			yield break;
		}

		Visited.Add(initialHashCode);
		List<Path> possiblePaths = ValidMoves(Initial)
			.Select(move => {
				var path = new Path()
				{
					PathCost = Heuristic(move)
				};
				path.Moves.Add(initialHashCode);
				path.Moves.Add(move);
				return path;
			}).ToList();

		Path currPath;
		int visitedMove;
		do
		{
			currPath = possiblePaths.MinBy(path => path.PathCost)!;
			possiblePaths.Remove(currPath);
			visitedMove = currPath.Moves.Last();
			Visited.Add(visitedMove);

			var moves = ValidMoves(visitedMove);
			foreach (var move in moves)
			{
				Path newPath = new()
				{
					PathCost = currPath.PathCost + Heuristic(move) - Heuristic(visitedMove) + 1
				};
				newPath.Moves.AddRange(currPath.Moves);
				newPath.Moves.Add(move);
				possiblePaths.Add(newPath);
			}

			solution = currPath.Moves.Select(move => movesTable[move]).ToList();
			yield return solution; 
		} while (goalHashCode != visitedMove);

		yield break;
	}

	private int HashCode(int[] board)
	{
		StringBuilder sb = new();
		foreach (int cell in board)
			sb.Append(cell);
		return sb.ToString().GetHashCode();
	}

	private int Heuristic(int move)
	{
		if (heuristicTable.ContainsKey(move))
			return heuristicTable[move];

		List<int> currBranch = ValidMoves(move);
		for (int i = 1; i < Depth; ++i)
		{
			List<int> nextBranch = new();
			foreach (var node in currBranch)
			{
				nextBranch.AddRange(ValidMoves(node));
			}
			currBranch = nextBranch;
		}
		if (currBranch.Count == 0)
			return 9;
		var value = currBranch
				   .Select(node => StaticComputation(node))
				   .Min(score => score);
		heuristicTable[move] = value;
		return value;
	}

	private int StaticComputation(int node)
	{
		return StaticComputation(movesTable[node]);
	}

	private int StaticComputation(int[] node)
	{
		int score = 9;
		for (int i = 0; i < node.Length; ++i)
		{
			if (node[i] == Goal[i])
				--score;
		}
		return score;
	}


	private List<int> ValidMoves(int[] board)
	{
		var emptySpace = Array.FindIndex(board, x => x == 0);
		var swapMoves = MoveSet(emptySpace);

		List<int> result = new();
		foreach (var move in swapMoves)
		{
			var newMove = board.Swap(emptySpace + move, emptySpace);
			int hashCode = HashCode(newMove);
			if (!movesTable.ContainsKey(hashCode))
				movesTable.Add(hashCode, newMove);

			if (!Visited.Contains(hashCode))
				result.Add(hashCode);
		}

		return result;
	}

	private List<int> ValidMoves(int boardHash)
	{
		return ValidMoves(movesTable[boardHash]);
	}

	private List<int> MoveSet(int emptySpace)
	{
		List<int> swapMoves = new List<int>();
		// Row moves
		switch (emptySpace)
		{
			case 0:
			case 1:
			case 2:
				swapMoves.Add(3);
				break;
			case 3:
			case 4:
			case 5:
				swapMoves.Add(3);
				swapMoves.Add(-3);
				break;
			case 6:
			case 7:
			case 8:
				swapMoves.Add(-3);
				break;
			default:
				break;
		}
		// Column moves
		switch (emptySpace)
		{
			case 0:
			case 3:
			case 6:
				swapMoves.Add(1);
				break;
			case 1:
			case 4:
			case 7:
				swapMoves.Add(1);
				swapMoves.Add(-1);
				break;
			case 2:
			case 5:
			case 8:
				swapMoves.Add(-1);
				break;
			default:
				break;
		}
		return swapMoves;
	}

	private int countInversions(int[] board)
	{
		int inversion = 0;
		for (int i = 0; i < board.Length; ++i)
		{
			for (int j = i + 1; j < board.Length; ++j)
			{
				if (board[j] != 0 && board[i] > board[j])
					inversion++;
			}
		}
		return inversion;
	}
}

