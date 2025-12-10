using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;

namespace MatrixCalculator
{
    public partial class MainWindow : Window
    {
        private int rows = 3;
        private int cols = 3;

        public MainWindow()
        {
            InitializeComponent();
            GenerateMatrix(MatrixAGrid, rows, cols);
            GenerateMatrix(MatrixBGrid, rows, cols);
            GenerateMatrix(ResultGrid, rows, cols);
        }

        // Генерация каркаса матрицы
        private void GenerateMatrix(UniformGrid grid, int rows, int cols)
        {
            grid.Children.Clear();
            grid.Rows = rows;
            grid.Columns = cols;

            for (int i = 0; i < rows * cols; i++)
            {
                TextBox cell = new TextBox
                {
                    Text = "0",
                    Margin = new Thickness(2),
                    HorizontalContentAlignment = HorizontalAlignment.Center,
                    VerticalContentAlignment = VerticalAlignment.Center
                };
                grid.Children.Add(cell);
            }
        }

        // Применение размеров
        private void ApplySize_Click(object sender, RoutedEventArgs e)
        {
            if (int.TryParse(RowInput.Text, out int newRows) &&
                int.TryParse(ColumnInput.Text, out int newCols) &&
                newRows >= 1 && newRows <= 5 &&
                newCols >= 1 && newCols <= 5)
            {
                rows = newRows;
                cols = newCols;

                GenerateMatrix(MatrixAGrid, rows, cols);
                GenerateMatrix(MatrixBGrid, rows, cols);
                GenerateMatrix(ResultGrid, rows, cols);
            }
            else
            {
                MessageBox.Show("Введите размеры от 1 до 5.");
            }
        }

        // Переключение матриц
        private void MatrixToggle_Checked(object sender, RoutedEventArgs e)
        {
            if (MatrixAEnabled != null)
                MatrixAPanel.Visibility = MatrixAEnabled.IsChecked == true ? Visibility.Visible : Visibility.Collapsed;

            if (MatrixBEnabled != null)
                MatrixBPanel.Visibility = MatrixBEnabled.IsChecked == true ? Visibility.Visible : Visibility.Collapsed;
        }

        // Получение матрицы из UniformGrid
        private double[,] GetMatrix(UniformGrid grid)
        {
            double[,] matrix = new double[grid.Rows, grid.Columns];
            int index = 0;

            for (int i = 0; i < grid.Rows; i++)
            {
                for (int j = 0; j < grid.Columns; j++)
                {
                    TextBox cell = (TextBox)grid.Children[index++];
                    if (double.TryParse(cell.Text, out double value))
                        matrix[i, j] = value;
                    else
                        matrix[i, j] = 0;
                }
            }
            return matrix;
        }

        // Заполнение результата
        private void SetResult(double[,] result)
        {
            ResultGrid.Children.Clear();
            ResultGrid.Rows = result.GetLength(0);
            ResultGrid.Columns = result.GetLength(1);

            for (int i = 0; i < result.GetLength(0); i++)
            {
                for (int j = 0; j < result.GetLength(1); j++)
                {
                    TextBox cell = new TextBox
                    {
                        Text = result[i, j].ToString(),
                        Margin = new Thickness(2),
                        IsReadOnly = true,
                        HorizontalContentAlignment = HorizontalAlignment.Center,
                        VerticalContentAlignment = VerticalAlignment.Center
                    };
                    ResultGrid.Children.Add(cell);
                }
            }
        }

        // Кнопка "Посчитать"
        private void CalculateButton_Click(object sender, RoutedEventArgs e)
        {
            string operation = (OperationComboBox.SelectedItem as ComboBoxItem)?.Content.ToString();

            Matrix A = new Matrix(GetMatrix(MatrixAGrid));
            Matrix B = new Matrix(GetMatrix(MatrixBGrid));

            try
            {
                Matrix result = null;

                switch (operation)
                {
                    case "Сложение (A + B)":
                        result = A.Add(B);
                        break;
                    case "Вычитание (A - B)":
                        result = A.Subtract(B);
                        break;
                    case "Умножение (A × B)":
                        result = A.Multiply(B);
                        break;
                    case "Транспонировать":
                        result = A.Transpose();
                        break;
                    case "Определитель":
                        MessageBox.Show("Определитель: " + A.Determinant());
                        break;
                    case "След":
                        MessageBox.Show("След: " + A.Trace());
                        break;
                    case "Умножить на число":
                        if (double.TryParse(ScalarInput.Text, out double scalar))
                            result = A.MultiplyByScalar(scalar);
                        else
                            MessageBox.Show("Введите число!");
                        break;
                    case "Обратная":
                        result = A.Inverse();
                        break;
                    case "Ранг":
                        MessageBox.Show("Ранг: " + A.Rank());
                        break;
                }

                if (result != null)
                {
                    SetResult(result.Values);

                    // открываем второе окно с результатом
                    ResultWindow resultWindow = new ResultWindow(result);
                    resultWindow.Show();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка: " + ex.Message);
            }
        }
    }
}
