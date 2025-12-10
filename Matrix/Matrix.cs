using System;

namespace MatrixCalculator
{
    public class Matrix
    {
        public double[,] Values { get; private set; }
        public int Rows => Values.GetLength(0);
        public int Columns => Values.GetLength(1);

        // Конструктор
        public Matrix(int rows, int cols)
        {
            if (rows < 1 || cols < 1 || rows > 5 || cols > 5)
                throw new ArgumentException("Размеры матрицы должны быть от 1 до 5.");
            Values = new double[rows, cols];
        }

        public Matrix(double[,] values)
        {
            Values = (double[,])values.Clone();
        }

        // Индексатор для удобного доступа
        public double this[int i, int j]
        {
            get => Values[i, j];
            set => Values[i, j] = value;
        }

        // Сложение
        public Matrix Add(Matrix other)
        {
            if (Rows != other.Rows || Columns != other.Columns)
                throw new Exception("Размеры матриц должны совпадать для сложения.");

            Matrix result = new Matrix(Rows, Columns);
            for (int i = 0; i < Rows; i++)
                for (int j = 0; j < Columns; j++)
                    result[i, j] = Values[i, j] + other[i, j];
            return result;
        }

        // Вычитание
        public Matrix Subtract(Matrix other)
        {
            if (Rows != other.Rows || Columns != other.Columns)
                throw new Exception("Размеры матриц должны совпадать для вычитания.");

            Matrix result = new Matrix(Rows, Columns);
            for (int i = 0; i < Rows; i++)
                for (int j = 0; j < Columns; j++)
                    result[i, j] = Values[i, j] - other[i, j];
            return result;
        }

        // Умножение матриц
        public Matrix Multiply(Matrix other)
        {
            if (Columns != other.Rows)
                throw new Exception("Количество столбцов A должно совпадать с количеством строк B.");

            Matrix result = new Matrix(Rows, other.Columns);
            for (int i = 0; i < Rows; i++)
                for (int j = 0; j < other.Columns; j++)
                    for (int k = 0; k < Columns; k++)
                        result[i, j] += Values[i, k] * other[k, j];
            return result;
        }

        // Умножение на число
        public Matrix MultiplyByScalar(double scalar)
        {
            Matrix result = new Matrix(Rows, Columns);
            for (int i = 0; i < Rows; i++)
                for (int j = 0; j < Columns; j++)
                    result[i, j] = Values[i, j] * scalar;
            return result;
        }

        // Транспонирование
        public Matrix Transpose()
        {
            Matrix result = new Matrix(Columns, Rows);
            for (int i = 0; i < Rows; i++)
                for (int j = 0; j < Columns; j++)
                    result[j, i] = Values[i, j];
            return result;
        }

        // След
        public double Trace()
        {
            if (Rows != Columns)
                throw new Exception("След вычисляется только для квадратных матриц.");

            double trace = 0;
            for (int i = 0; i < Rows; i++)
                trace += Values[i, i];
            return trace;
        }

        // Определитель (реализован для 2×2 и 3×3)
        public double Determinant()
        {
            if (Rows != Columns)
                throw new Exception("Определитель вычисляется только для квадратных матриц.");

            int n = Rows;
            if (n == 2)
                return Values[0, 0] * Values[1, 1] - Values[0, 1] * Values[1, 0];

            if (n == 3)
                return Values[0, 0] * (Values[1, 1] * Values[2, 2] - Values[1, 2] * Values[2, 1])
                     - Values[0, 1] * (Values[1, 0] * Values[2, 2] - Values[1, 2] * Values[2, 0])
                     + Values[0, 2] * (Values[1, 0] * Values[2, 1] - Values[1, 1] * Values[2, 0]);

            throw new Exception("Определитель реализован только для 2×2 и 3×3.");
        }

        // Ранг (метод Гаусса)
        public int Rank()
        {
            double[,] temp = (double[,])Values.Clone();
            int rank = Columns;
            for (int row = 0; row < rank; row++)
            {
                if (temp[row, row] != 0)
                {
                    for (int col = 0; col < Rows; col++)
                    {
                        if (col != row)
                        {
                            double mult = temp[col, row] / temp[row, row];
                            for (int i = 0; i < rank; i++)
                                temp[col, i] -= mult * temp[row, i];
                        }
                    }
                }
                else
                {
                    bool reduce = true;
                    for (int i = row + 1; i < Rows; i++)
                    {
                        if (temp[i, row] != 0)
                        {
                            for (int j = 0; j < rank; j++)
                            {
                                double tmp = temp[row, j];
                                temp[row, j] = temp[i, j];
                                temp[i, j] = tmp;
                            }
                            reduce = false;
                            break;
                        }
                    }
                    if (reduce)
                    {
                        rank--;
                        for (int i = 0; i < Rows; i++)
                            temp[i, row] = temp[i, rank];
                    }
                    row--;
                }
            }
            return rank;
        }

        // Обратная матрица (метод Гаусса-Жордана)
        public Matrix Inverse()
        {
            if (Rows != Columns)
                throw new Exception("Обратная матрица существует только для квадратных матриц.");

            int n = Rows;
            double[,] aug = new double[n, 2 * n];

            // Формируем расширенную матрицу [A|I]
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                    aug[i, j] = Values[i, j];
                aug[i, n + i] = 1;
            }

            // Прямой ход
            for (int i = 0; i < n; i++)
            {
                if (aug[i, i] == 0)
                    throw new Exception("Матрица вырождена, обратной не существует.");

                double diag = aug[i, i];
                for (int j = 0; j < 2 * n; j++)
                    aug[i, j] /= diag;

                for (int k = 0; k < n; k++)
                {
                    if (k != i)
                    {
                        double factor = aug[k, i];
                        for (int j = 0; j < 2 * n; j++)
                            aug[k, j] -= factor * aug[i, j];
                    }
                }
            }

            // Извлекаем обратную матрицу
            double[,] inv = new double[n, n];
            for (int i = 0; i < n; i++)
                for (int j = 0; j < n; j++)
                    inv[i, j] = aug[i, j + n];

            return new Matrix(inv);
        }
    }
}
