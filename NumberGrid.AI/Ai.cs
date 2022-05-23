using System.Text;

namespace NumberGrid.AI;

public class Ai
{
	public int[] Initial { get; init; }
	public int[] Goal { get; init; }
	public int Depth { get; init; }

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

	public List<int[]> astar()
	{
		

		return new();
	}

	public IEnumerable<int[]> Compute()
	{
		Visited.Add(HashCode(Initial));
		yield return Initial;
		if (Initial.SequenceEqual(Goal))
		{
			yield break;
		}

		int[] currMove = Initial;
		do
		{
			List<int[]> possibleMoves = ValidMoves(currMove);
			currMove = possibleMoves.AsParallel().Select(move => new
			{
				Move = move,
				Score = ComputeScore(move)
			}).AsSequential().MaxBy(x => x.Score)!.Move;

			Visited.Add(HashCode(currMove));
			yield return currMove;
		} while (!currMove.SequenceEqual(Goal));

		yield break;
	}

	private int HashCode(int[] board)
	{
		StringBuilder sb = new();
		foreach (int cell in board)
			sb.Append(cell);
		return sb.ToString().GetHashCode();
	}

	private int ComputeScore(int[] move)
	{
		List<int[]> currBranch = ValidMoves(move);
		for (int i = 1; i < Depth; ++i)
		{
			List<int[]> nextBranch = new();
			foreach (var node in currBranch)
			{
				nextBranch.AddRange(ValidMoves(node));
			}
			currBranch = nextBranch;
		}
		if (currBranch.Count == 0)
			return 9;
		return currBranch
				   .Select(node => StaticComputation(node))
				   .Min(score => score);
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

	private readonly List<int> Visited = new();

	private List<int[]> ValidMoves(int[] board)
	{
		var emptySpace = Array.FindIndex(board, x => x == 0);
		var swapMoves = MoveSet(emptySpace);

		List<int[]> result = new List<int[]>();
		foreach (var move in swapMoves)
		{
			var newMove = board.Swap(emptySpace + move, emptySpace);
			if (!Visited.Contains(HashCode(newMove)))
				result.Add(newMove);
		}

		return result;
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

