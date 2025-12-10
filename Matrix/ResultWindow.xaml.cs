using System.Windows;
using System.Windows.Controls;

namespace MatrixCalculator
{
    public partial class ResultWindow : Window
    {
        public ResultWindow(Matrix resultMatrix)
        {
            InitializeComponent();
            DisplayMatrix(resultMatrix);
        }

        private void DisplayMatrix(Matrix matrix)
        {
            ResultGrid.Rows = matrix.Rows;
            ResultGrid.Columns = matrix.Columns;
            ResultGrid.Children.Clear();

            for (int i = 0; i < matrix.Rows; i++)
            {
                for (int j = 0; j < matrix.Columns; j++)
                {
                    TextBox cell = new TextBox
                    {
                        Text = matrix[i, j].ToString(),
                        Margin = new Thickness(2),
                        IsReadOnly = true,
                        HorizontalContentAlignment = HorizontalAlignment.Center,
                        VerticalContentAlignment = VerticalAlignment.Center
                    };
                    ResultGrid.Children.Add(cell);
                }
            }
        }
    }
}
