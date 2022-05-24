//using NumberGrid.AI;

//int[] initial = { 1, 2, 3, 4, 5, 6, 7, 8, 0 };
//int[] goal = { 1, 2, 3, 4, 5, 6, 7, 0, 8 };
//int[] initial2 = { 3,5,1,4,0,8,6,2,7};
//int[] goal2 = { 8,1,2,7,0,3,6,5,4 };

//Ai ai = new(initial2, goal2);

//int count = 0;
//foreach (var move in ai.Compute())
//	count++;
//Console.WriteLine($"First: {count}");
//count = 0;
//foreach (var move in ai.Compute())
//	count++;
//Console.WriteLine($"Second: {count}");

//var solution = ai.Compute().Last();
//foreach(var move in solution)
//{
//	DisplayMove(move);
//	Console.WriteLine();
//}
//Console.WriteLine($"Steps taken: {solution.Count}");
////foreach (var path in ai.Compute())
////{
////	Console.WriteLine("-------------------------");
////	foreach (var move in path)
////	{
////		 DisplayMove(move);
////		Console.WriteLine();
////	}
////	Console.WriteLine("-------------------------");
////}

//void DisplayMove(int[] move)
//{
//	for (int i = 0; i < move.Length; i++)
//	{
//		if (i % 3 == 0)
//			Console.WriteLine();
//		Console.Write($"{move[i]}\t");
//	}
//}
