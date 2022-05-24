using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using NumberGrid.AI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace NumberGrid.Desktop.ViewModels;
// initial: 1,2,3,4,5,6,7,8,0 | 3,5,1,4,0,8,6,2,7
// goal: 5,7,2,3,1,6,4,0,8 | 8,1,2,7,0,3,6,5,4

[INotifyPropertyChanged]
public partial class MainWindowViewModel
{
	[ObservableProperty]
	private List<int[]>? moves;

	[ObservableProperty]
	private string? status;

	[ObservableProperty]
	private string input = string.Empty;
	[ObservableProperty]
	private string output = string.Empty;

	[ObservableProperty]
	private string? movesTaken;
	[ObservableProperty]
	private string? length;

	[ObservableProperty]
	private string? loading;

	public void StartComputation()
	{
		Moves = null;
		MovesTaken = null;
		Length = null;
		Loading = "Computing...";

		int[] initial, goal;
		try
		{
			initial = Input.Split(',').Select(x => int.Parse(x)).ToArray();
			goal = Output.Split(',').Select(x => int.Parse(x)).ToArray();
			if (!ValidateInput(initial) || !ValidateInput(goal))
				throw new Exception();
		}
		catch(Exception)
		{
			Status = "Invalid data entry";
			return;
		}
		Ai ai = new(initial, goal, 3);
		if (!ai.IsValid)
		{
			Status = "Unreachable";
			return;
		}

		Status = "Running";
		int stepsTaken = 0;
		foreach(var moves in ai.Compute())
		{
			stepsTaken++;
			MovesTaken = $"Moves Count: {stepsTaken}";
		}

		Loading = null;
		Moves = ai.Compute().Last();	
		Status = "Finished";
		Length = $"Solution Length: {Moves.Count}";

	}

	[ICommand]
	public Task StartComputationAsync()
	{
		return Task.Run(StartComputation);
	}

	private bool ValidateInput(int[] board)
	{
		if (board.Length != 9)
			return false;
		foreach(int x in board)
		{
			if (x < 0 || x > 8)
				return false;
		}
		return true;
	}
}
