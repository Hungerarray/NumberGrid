using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using NumberGrid.AI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace NumberGrid.Desktop.ViewModels;

[INotifyPropertyChanged]
public partial class MainWindowViewModel
{
	public ObservableCollection<int[]> Moves { get; } = new();

	[ObservableProperty]
	private string status = string.Empty;

	[ObservableProperty]
	private string input = string.Empty;
	[ObservableProperty]
	private string output = string.Empty;

	[ObservableProperty]
	private string movesTaken = string.Empty;

	public void StartComputation()
	{
		Moves.Clear();
		Ai.Reset();
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
		if (!Ai.Validate(initial, goal))
			Status = "No legal moves to reach goal state from initial state";

		Status = "Running";
		int count = 0;
		foreach(var move in Ai.Compute(initial, goal))
		{
			count++;
			Moves.Add(move);
		}
		Status = "Finished";
		MovesTaken = count.ToString();
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
