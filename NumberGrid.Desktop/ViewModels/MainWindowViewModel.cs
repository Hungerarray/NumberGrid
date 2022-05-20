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

	public void StartComputation()
	{
		int[] initial, goal;
		try
		{
			initial = input.Split(',').Select(x => int.Parse(x)).ToArray();
			goal = input.Split(',').Select(x => int.Parse(x)).ToArray();
		}
		catch(Exception)
		{
			Status = "Invalid data entry";
			return;
		}

		Status = "Running";
		foreach(var move in Ai.Compute(initial, goal))
		{
			Moves.Add(move);
		}
		Status = "Finished";
	}

	[ICommand]
	public Task StartComputationAsync()
	{
		return Task.Run(StartComputation);
	}
}
