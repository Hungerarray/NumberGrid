using Avalonia.Controls;
using Avalonia.Controls.Templates;

namespace NumberGrid.Desktop.Views;

public class NumberGrid : IDataTemplate
{
	public IControl Build(object param)
	{
		int[] board = (int[]) param;

		Grid grid = new Grid()
		{
			RowDefinitions = new RowDefinitions("Auto,Auto,Auto"),
			ColumnDefinitions = new ColumnDefinitions("Auto,Auto,Auto"),
			HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Center,
			VerticalAlignment = Avalonia.Layout.VerticalAlignment.Center,
			ShowGridLines = true,
		};

		for (int i = 0, row = 0, col = 0; i < board.Length; i++, col = (i % 3), row = i / 3)
		{
			int cell = board[i];
			Button button = new Button()
			{
				Content = cell == 0 ? string.Empty : cell,
				Height = 75,
				Width = 75,
				HorizontalContentAlignment = Avalonia.Layout.HorizontalAlignment.Center,
				VerticalContentAlignment = Avalonia.Layout.VerticalAlignment.Center,
				FontWeight = Avalonia.Media.FontWeight.Bold,
				FontSize = 20.0,
				IsEnabled = false,
			};
			Grid.SetRow(button, row);
			Grid.SetColumn(button, col++);
			grid.Children.Add(button);
		}

		return grid;
	}

	public bool Match(object data)
	{
		return data is int[];
	}

	public  Grid RenderBoard(int[] board)
	{
		Grid grid = new Grid()
		{
			RowDefinitions = new RowDefinitions("Auto,Auto,Auto"),
			ColumnDefinitions = new ColumnDefinitions("Auto,Auto,Auto"),
			HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Center,
			VerticalAlignment = Avalonia.Layout.VerticalAlignment.Center,
			ShowGridLines = true,
		};

		for (int i = 0, row = 0, col = 0; i < board.Length; i++, col = (i % 3), row = i / 3)
		{
			int cell = board[i];
			Button button = new Button()
			{
				Content = cell == 0 ? string.Empty : cell,
				Height = 75,
				Width = 75,
				HorizontalContentAlignment = Avalonia.Layout.HorizontalAlignment.Center,
				VerticalContentAlignment = Avalonia.Layout.VerticalAlignment.Center,
				FontWeight = Avalonia.Media.FontWeight.Bold,
				FontSize = 20.0,
				IsEnabled = false,
			};
			Grid.SetRow(button, row);
			Grid.SetColumn(button, col++);
			grid.Children.Add(button);
		}

		return grid;
	}
}
