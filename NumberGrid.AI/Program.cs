
// Validate function

using NumberGrid.AI;

int[] initial = { 5, 7, 2, 3, 1, 6, 4, 0, 8 };
int[] goal = { 1, 2, 3, 4, 5, 0, 7, 8, 6 };

int[] initial2 = { 3, 5, 1, 4, 0, 8, 6, 2, 7 };
int[] goal2 = { 8, 1, 2, 7, 0, 3, 6, 5, 4 };
Ai.Validate(initial2, goal2);

// Compute function

int count = 0;
foreach(var move in Ai.Compute(initial2, goal2))
{
	 DisplayBoard(move);
	Console.WriteLine();
	count++;
}
Console.WriteLine(count);

void DisplayBoard(int[] board)
{
	int count = 0;
	foreach (int cell in board)
	{
		count++;
		Console.Write($"{cell}\t");
		if(count == 3)
		{
			count = 0;
			Console.WriteLine();
		}
	}
}
