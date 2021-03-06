using System.Diagnostics.CodeAnalysis;

namespace NumberGrid.AI;

public static class AiExtensions
{
	static public int[] Swap(this int[] array, int pos1, int pos2)
	{
		int[] result = new int[array.Length];
		Array.Copy(array, result, array.Length);

		int temp = result[pos1];
		result[pos1] = result[pos2];
		result[pos2] = temp;

		return result;
	}
}

public record Path
{
	public List<int> Moves { get; init; } = new();
	public int PathCost { get; init; }
}

