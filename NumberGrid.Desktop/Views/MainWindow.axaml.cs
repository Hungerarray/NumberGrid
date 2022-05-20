using Avalonia.Controls;
using Avalonia.Controls.Templates;
using NumberGrid.AI;

namespace NumberGrid.Desktop.Views
{
	public partial class MainWindow : Window
	{
		public MainWindow()
		{
			InitializeComponent();
			var viewbox = this.Find<ListBox>("viewbox");
			viewbox.ItemTemplate = new NumberGrid();
		}

	}
}
