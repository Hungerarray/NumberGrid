using System.Text;

namespace NumberGrid.AI;

public static class Ai
{
	static public bool Validate(int[] initial, int[] goal)
	{
		int initInversion = countInversions(initial);
		int goalInversion = countInversions(goal);

		return (initInversion & 0x1) == (goalInversion & 0x1);
	}

	static public IEnumerable<int[]> Compute(int[] initial, int[] goal)
	{
		Visited.Add(HashCode(initial));
		yield return initial;
		if (initial.SequenceEqual(goal))
		{
			yield break;
		}

		int[] currMove = initial;
		do
		{
			List<int[]> possibleMoves = ValidMoves(currMove);
			int depth = 2;
			currMove = possibleMoves.AsParallel().Select(move => new
			{
				Move = move,
				Score = ComputeScore(move, goal, depth)
			}).AsSequential().MaxBy(x => x.Score)!.Move;

			Visited.Add(HashCode(currMove));
			yield return currMove;
		} while (!currMove.SequenceEqual(goal));

		yield break;
	}

	static public int HashCode(int[] board)
	{
		StringBuilder sb = new();
		foreach (int cell in board)
			sb.Append(cell);
		return sb.ToString().GetHashCode();
	}

	private static int ComputeScore(int[] move, int[] goal, int depth)
	{
		List<int[]> currBranch = ValidMoves(move);
		for(int i = 1; i < depth; ++i)
		{
			List<int[]> nextBranch = new();
			foreach (var node in currBranch)
			{
				nextBranch.AddRange(ValidMoves(node));
			}
			currBranch = nextBranch;
		}
		if (currBranch.Count == 0)
			return 0;
		return currBranch
				   .Select(node => StaticComputation(node, goal))
				   .Max(score => score);
	}
	
	private static int StaticComputation(int[] node, int[] goal)
	{
		int score = 0;
		for (int i = 0; i < node.Length; ++i)
		{
			if (node[i] == goal[i])
				score++;
		}
		return score;
	}

	static private readonly List<int> Visited = new();

	static private List<int[]> ValidMoves(int[] board)
	{
		var emptySpace = Array.FindIndex(board, x => x == 0);
		var swapMoves = MoveSet(emptySpace);

		List<int[]> result = new List<int[]>();
		foreach (var move in swapMoves)
		{
			var newMove = board.Swap(emptySpace + move, emptySpace);
			if(!Visited.Contains(HashCode(newMove)))
				result.Add(newMove);
		}

		return result;
	}

	private static List<int> MoveSet(int emptySpace)
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

	static private int countInversions(int[] board)
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

public static class AiExtensions
{
	public static int[] Swap(this int[] array, int pos1, int pos2)
	{
		int[] result = new int[array.Length];
		Array.Copy(array, result, array.Length);

		int temp = result[pos1];
		result[pos1] = result[pos2];
		result[pos2] = temp;

		return result;
	}
}
