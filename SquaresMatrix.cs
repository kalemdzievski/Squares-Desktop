using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace Igrica
{
    public class SquaresMatrix
    {

        private List<List<Square>> matrix;
        private int rows, columns, offset, squareWidth;
        private Random random;
        private RandomImage randomImage;
        private Square selectedSquare;
        private Square selectedSquareDest;
        public Sounds gameSound { get; set; }
        public int score { get; set; }
        public int combo { get; set; }
        public int getComboScore { get; set; }
        public int numberOfSquares { get; set; }
        public int numberOfSquaresPainted { get; set; }
        public bool gameOver { get; set; }

        public SquaresMatrix(int r, int c, int w, int o)
        {
            rows = r;
            columns = c;
            offset = o;
            squareWidth = w;
            score = 0;
            combo = 0;
            numberOfSquares = 5;
            numberOfSquaresPainted = 0;
            gameOver = false;
            random = new Random();
            randomImage = new RandomImage();
            gameSound = new Sounds();
            selectedSquare = null;
            selectedSquareDest = null;

            initializeMatrix();
            paintRandomSquares(numberOfSquares);
        }

        private void initializeMatrix()
        {
            matrix = new List<List<Square>>();
            for (int i = 0; i < rows; i++)
            {
                List<Square> list = new List<Square>();
                for (int j = 0; j < columns; j++)
                {
                    Square s = new Square(offset + squareWidth * i, offset + squareWidth * j, squareWidth);
                    list.Add(s);
                }
                matrix.Add(list);
            }
        }

        public void drawMatrix(Graphics g)
        {
            for (int i = 0; i < rows; i++)
                for (int j = 0; j < columns; j++)
                    matrix.ElementAt(i).ElementAt(j).drawSquare(g);
            if (selectedSquare != null)
                selectedSquare.drawSquare(g);
        }

        public void paintRandomSquares(int n)
        {
            int i = 0, x, y;
            while (i != n)
            {
                x = random.Next(rows);
                y = random.Next(columns);
                if (!matrix.ElementAt(x).ElementAt(y).isPainted)
                {
                    i++;
                    numberOfSquaresPainted++;
                    Image i1, i2;
                    if(numberOfSquares <= 8)
                        randomImage.getImages(n, out i1, out i2);
                    else randomImage.getImages(8, out i1, out i2);
                    matrix.ElementAt(x).ElementAt(y).image = i1;
                    matrix.ElementAt(x).ElementAt(y).imageSelected = i2;
                    matrix.ElementAt(x).ElementAt(y).isPainted = true;
                    if (hasConnected(x, y))
                    {
                        gameSound.playScore();
                        score += 100;
                        numberOfSquaresPainted -= 3;
                    }
                }
                if (numberOfSquaresPainted == rows * columns)
                {
                    gameOver = true;
                    break;
                }
            }
        }

        public void selectAndMove(int X, int Y)
        {
            int x = 10, y = 10;
            bool change = false;
            int i = (Y - offset) / squareWidth;
            int j = (X - offset) / squareWidth;
            if (matrix.ElementAt(i).ElementAt(j).isHit(X, Y) && matrix.ElementAt(i).ElementAt(j).isPainted)
            {
                if (selectedSquare != null)
                    selectedSquare.isSelected = false;
                matrix.ElementAt(i).ElementAt(j).isSelected = true;
                selectedSquare = matrix.ElementAt(i).ElementAt(j);
                change = true;
            }
            else if (matrix.ElementAt(i).ElementAt(j).isHit(X, Y) && selectedSquare != null)
            {
                selectedSquareDest = matrix.ElementAt(i).ElementAt(j);
                x = i;
                y = j;
            }
            if (!change && selectedSquare != null && selectedSquareDest != null)
            {
                if (path())
                {
                    selectedSquareDest.image = selectedSquare.image;
                    selectedSquareDest.imageSelected = selectedSquare.imageSelected;
                    selectedSquare.image = selectedSquare.imageDefault;
                    selectedSquare.imageSelected = null;
                    selectedSquare.isSelected = false;
                    selectedSquare.isPainted = false;
                    selectedSquareDest.isSelected = false;
                    selectedSquareDest.isPainted = true;
                    selectedSquare = null;
                    selectedSquareDest = null;
                    if (!hasConnected(x, y))
                    {
                        gameSound.playPlaceSquare();
                        paintRandomSquares(numberOfSquares);
                        if (combo >= 3)
                        {
                            score += combo * 50;
                            getComboScore = combo * 50;
                        }
                        combo = 0;
                    }
                    else
                    {
                        gameSound.playScore();
                        score += 100;
                        numberOfSquaresPainted -= 3;
                        combo++;
                    }
                }
                else gameSound.playNoPath();
            }
            if (numberOfSquaresPainted == 0)
                paintRandomSquares(numberOfSquares);
        }

        public bool hasConnected(int i, int j)
        {
            Point p2, p3;
            if (hasLine(i, j, out p2, out p3))
            {
                matrix.ElementAt(i).ElementAt(j).image = matrix.ElementAt(i).ElementAt(j).imageDefault;
                matrix.ElementAt(p2.X).ElementAt(p2.Y).image = matrix.ElementAt(i).ElementAt(j).imageDefault;
                matrix.ElementAt(p3.X).ElementAt(p3.Y).image = matrix.ElementAt(i).ElementAt(j).imageDefault;
                matrix.ElementAt(i).ElementAt(j).isPainted = false;
                matrix.ElementAt(p2.X).ElementAt(p2.Y).isPainted = false;
                matrix.ElementAt(p3.X).ElementAt(p3.Y).isPainted = false;
                return true;
            }
            else return false;
        }

        public bool hasLine(int i, int j, out Point p2, out Point p3)
        {
            Image image = matrix.ElementAt(i).ElementAt(j).image;
            p2 = new Point(-1, -1);
            p3 = new Point(-1, -1);
            if (i == 0 && j == 0) // GOREN - LEV KOSH
            {
                if (image != matrix.ElementAt(i).ElementAt(j + 1).image && image != matrix.ElementAt(i + 1).ElementAt(j).image)
                    return false;
                else
                {
                    if (image == matrix.ElementAt(i).ElementAt(j + 1).image && image == matrix.ElementAt(i).ElementAt(j + 2).image)
                    {
                        p2 = new Point(i, j + 1);
                        p3 = new Point(i, j + 2);
                        return true;
                    }
                    else if (image == matrix.ElementAt(i + 1).ElementAt(j).image && image == matrix.ElementAt(i + 2).ElementAt(j).image)
                    {
                        p2 = new Point(i + 1, j);
                        p3 = new Point(i + 2, j);
                        return true;
                    }
                    else return false;
                }
            }
            else if (i == 0 && j == (columns - 1)) // GOREN - DESEN KOSH
            {
                if (image != matrix.ElementAt(i).ElementAt(j - 1).image && image != matrix.ElementAt(i + 1).ElementAt(j).image)
                    return false;
                else
                {
                    if (image == matrix.ElementAt(i).ElementAt(j - 1).image && image == matrix.ElementAt(i).ElementAt(j - 2).image)
                    {
                        p2 = new Point(i, j - 1);
                        p3 = new Point(i, j - 2);
                        return true;
                    }
                    else if (image == matrix.ElementAt(i + 1).ElementAt(j).image && image == matrix.ElementAt(i + 2).ElementAt(j).image)
                    {
                        p2 = new Point(i + 1, j);
                        p3 = new Point(i + 2, j);
                        return true;
                    }
                    else return false;
                }
            }
            else if (i == (rows - 1) && j == 0) // DOLEN - LEV KOSH
            {
                if (image != matrix.ElementAt(i).ElementAt(j + 1).image && image != matrix.ElementAt(i - 1).ElementAt(j).image)
                    return false;
                else
                {
                    if (image == matrix.ElementAt(i).ElementAt(j + 1).image && image == matrix.ElementAt(i).ElementAt(j + 2).image)
                    {
                        p2 = new Point(i, j + 1);
                        p3 = new Point(i, j + 2);
                        return true;
                    }
                    else if (image == matrix.ElementAt(i - 1).ElementAt(j).image && image == matrix.ElementAt(i - 2).ElementAt(j).image)
                    {
                        p2 = new Point(i - 1, j);
                        p3 = new Point(i - 2, j);
                        return true;
                    }
                    else return false;
                }
            }
            else if (i == (rows - 1) && j == (columns - 1)) // DOLEN - DESEN KOSH
            {
                if (image != matrix.ElementAt(i).ElementAt(j - 1).image && image != matrix.ElementAt(i - 1).ElementAt(j).image)
                    return false;
                else
                {
                    if (image == matrix.ElementAt(i).ElementAt(j - 1).image && image == matrix.ElementAt(i).ElementAt(j - 2).image)
                    {
                        p2 = new Point(i, j - 1);
                        p3 = new Point(i, j - 2);
                        return true;
                    }
                    else if (image == matrix.ElementAt(i - 1).ElementAt(j).image && image == matrix.ElementAt(i - 2).ElementAt(j).image)
                    {
                        p2 = new Point(i - 1, j);
                        p3 = new Point(i - 2, j);
                        return true;
                    }
                    else return false;
                }
            }
            else if (i == 0) // PRVA REDICA
            {
                if (image != matrix.ElementAt(i).ElementAt(j - 1).image && image != matrix.ElementAt(i).ElementAt(j + 1).image && image != matrix.ElementAt(i + 1).ElementAt(j).image)
                    return false;
                else if (image == matrix.ElementAt(i).ElementAt(j - 1).image && image == matrix.ElementAt(i).ElementAt(j + 1).image)
                {
                    p2 = new Point(i, j - 1);
                    p3 = new Point(i, j + 1);
                    return true;
                }
                else
                {
                    if ((j - 2) >= 0 && image == matrix.ElementAt(i).ElementAt(j - 1).image && image == matrix.ElementAt(i).ElementAt(j - 2).image)
                    {
                        p2 = new Point(i, j - 1);
                        p3 = new Point(i, j - 2);
                        return true;
                    }
                    else if ((j + 2) <= (columns - 1) && image == matrix.ElementAt(i).ElementAt(j + 1).image && image == matrix.ElementAt(i).ElementAt(j + 2).image)
                    {
                        p2 = new Point(i, j + 1);
                        p3 = new Point(i, j + 2);
                        return true;
                    }
                    else if (image == matrix.ElementAt(i + 1).ElementAt(j).image && image == matrix.ElementAt(i + 2).ElementAt(j).image)
                    {
                        p2 = new Point(i + 1, j);
                        p3 = new Point(i + 2, j);
                        return true;
                    }
                    else return false;
                }
            }
            else if (i == (rows - 1)) // POSLEDNA REDICA
            {
                if (image != matrix.ElementAt(i).ElementAt(j - 1).image && image != matrix.ElementAt(i).ElementAt(j + 1).image && image != matrix.ElementAt(i - 1).ElementAt(j).image)
                    return false;
                else if (image == matrix.ElementAt(i).ElementAt(j - 1).image && image == matrix.ElementAt(i).ElementAt(j + 1).image)
                {
                    p2 = new Point(i, j - 1);
                    p3 = new Point(i, j + 1);
                    return true;
                }
                else
                {
                    if ((j - 2) >= 0 && image == matrix.ElementAt(i).ElementAt(j - 1).image && image == matrix.ElementAt(i).ElementAt(j - 2).image)
                    {
                        p2 = new Point(i, j - 1);
                        p3 = new Point(i, j - 2);
                        return true;
                    }
                    else if ((j + 2) <= (columns - 1) && image == matrix.ElementAt(i).ElementAt(j + 1).image && image == matrix.ElementAt(i).ElementAt(j + 2).image)
                    {
                        p2 = new Point(i, j + 1);
                        p3 = new Point(i, j + 2);
                        return true;
                    }
                    else if (image == matrix.ElementAt(i - 1).ElementAt(j).image && image == matrix.ElementAt(i - 2).ElementAt(j).image)
                    {
                        p2 = new Point(i - 1, j);
                        p3 = new Point(i - 2, j);
                        return true;
                    }
                    else return false;
                }
            }
            else if (j == 0) // PRVA KOLONA
            {
                if (image != matrix.ElementAt(i - 1).ElementAt(j).image && image != matrix.ElementAt(i + 1).ElementAt(j).image && image != matrix.ElementAt(i).ElementAt(j + 1).image)
                    return false;
                else if (image == matrix.ElementAt(i - 1).ElementAt(j).image && image == matrix.ElementAt(i + 1).ElementAt(j).image)
                {
                    p2 = new Point(i - 1, j);
                    p3 = new Point(i + 1, j);
                    return true;
                }
                else
                {
                    if (image == matrix.ElementAt(i).ElementAt(j + 1).image && image == matrix.ElementAt(i).ElementAt(j + 2).image)
                    {
                        p2 = new Point(i, j + 1);
                        p3 = new Point(i, j + 2);
                        return true;
                    }
                    else if ((i - 2) >= 0 && image == matrix.ElementAt(i - 1).ElementAt(j).image && image == matrix.ElementAt(i - 2).ElementAt(j).image)
                    {
                        p2 = new Point(i - 1, j);
                        p3 = new Point(i - 2, j);
                        return true;
                    }
                    else if ((i + 2) <= (rows - 1) && image == matrix.ElementAt(i + 1).ElementAt(j).image && image == matrix.ElementAt(i + 2).ElementAt(j).image)
                    {
                        p2 = new Point(i + 1, j);
                        p3 = new Point(i + 2, j);
                        return true;
                    }
                    else return false;
                }
            }
            else if (j == (columns - 1)) // POSLEDNA KOLONA
            {
                if (image != matrix.ElementAt(i - 1).ElementAt(j).image && image != matrix.ElementAt(i + 1).ElementAt(j).image && image != matrix.ElementAt(i).ElementAt(j - 1).image)
                    return false;
                else if (image == matrix.ElementAt(i - 1).ElementAt(j).image && image == matrix.ElementAt(i + 1).ElementAt(j).image)
                {
                    p2 = new Point(i - 1, j);
                    p3 = new Point(i + 1, j);
                    return true;
                }
                else
                {
                    if (image == matrix.ElementAt(i).ElementAt(j - 1).image && image == matrix.ElementAt(i).ElementAt(j - 2).image)
                    {
                        p2 = new Point(i, j - 1);
                        p3 = new Point(i, j - 2);
                        return true;
                    }
                    else if ((i - 2) >= 0 && image == matrix.ElementAt(i - 1).ElementAt(j).image && image == matrix.ElementAt(i - 2).ElementAt(j).image)
                    {
                        p2 = new Point(i - 1, j);
                        p3 = new Point(i - 2, j);
                        return true;
                    }
                    else if ((i + 2) <= (rows - 1) && image == matrix.ElementAt(i + 1).ElementAt(j).image && image == matrix.ElementAt(i + 2).ElementAt(j).image)
                    {
                        p2 = new Point(i + 1, j);
                        p3 = new Point(i + 2, j);
                        return true;
                    }
                    else return false;
                }
            }
            else // SITE IZMEGJU
            {
                if (image != matrix.ElementAt(i - 1).ElementAt(j).image && image != matrix.ElementAt(i + 1).ElementAt(j).image && image != matrix.ElementAt(i).ElementAt(j + 1).image && image != matrix.ElementAt(i).ElementAt(j - 1).image)
                    return false;
                else if (image == matrix.ElementAt(i - 1).ElementAt(j).image && image == matrix.ElementAt(i + 1).ElementAt(j).image)
                {
                    p2 = new Point(i - 1, j);
                    p3 = new Point(i + 1, j);
                    return true;
                }
                else if (image == matrix.ElementAt(i).ElementAt(j - 1).image && image == matrix.ElementAt(i).ElementAt(j + 1).image)
                {
                    p2 = new Point(i, j - 1);
                    p3 = new Point(i, j + 1);
                    return true;
                }
                else
                {
                    if ((j - 2) >= 0 && image == matrix.ElementAt(i).ElementAt(j - 1).image && image == matrix.ElementAt(i).ElementAt(j - 2).image)
                    {
                        p2 = new Point(i, j - 1);
                        p3 = new Point(i, j - 2);
                        return true;
                    }
                    else if ((j + 2) <= (columns - 1) && image == matrix.ElementAt(i).ElementAt(j + 1).image && image == matrix.ElementAt(i).ElementAt(j + 2).image)
                    {
                        p2 = new Point(i, j + 1);
                        p3 = new Point(i, j + 2);

                        return true;
                    }
                    else if ((i - 2) >= 0 && image == matrix.ElementAt(i - 1).ElementAt(j).image && image == matrix.ElementAt(i - 2).ElementAt(j).image)
                    {
                        p2 = new Point(i - 1, j);
                        p3 = new Point(i - 2, j);
                        return true;
                    }
                    else if ((i + 2) <= (rows - 1) && image == matrix.ElementAt(i + 1).ElementAt(j).image && image == matrix.ElementAt(i + 2).ElementAt(j).image)
                    {
                        p2 = new Point(i + 1, j);
                        p3 = new Point(i + 2, j);
                        return true;
                    }
                    else return false;
                }
            }
        }

        public bool path()
        {
            String[] input = new String[rows + 2];
            Maze maze = new Maze();
            char[,] mazematrix = new char[rows + 2, columns + 2];
            for (int i = 0; i < rows + 2; i++)
            {
                mazematrix[0, i] = '#';
                mazematrix[rows + 1, i] = '#';
                mazematrix[i, 0] = '#';
                mazematrix[i, columns + 1] = '#';
            }

            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < columns; j++)
                {
                    if (matrix.ElementAt(i).ElementAt(j).isPainted)
                    {
                        mazematrix[i + 1, j + 1] = '#';
                    }
                    else mazematrix[i + 1, j + 1] = ' ';

                    if (selectedSquare == matrix.ElementAt(i).ElementAt(j))
                    {
                        mazematrix[i + 1, j + 1] = 'S';
                    }
                    if (selectedSquareDest == matrix.ElementAt(i).ElementAt(j))
                    {
                        mazematrix[i + 1, j + 1] = 'E';
                    }
                }
            }
            for (int i = 0; i < rows + 2; i++)
            {
                for (int j = 0; j < columns + 2; j++)
                {
                    input[i] += mazematrix[i, j];
                }
            }
            maze.generateGraph(rows + 2, columns + 2, input);
            return maze.findPath();
        }

        public void bombThem(int X, int Y)
        {
            int i = (Y - offset) / squareWidth;
            int j = (X - offset) / squareWidth;
            int points = 0;

            if (i - 1 < 0)
            {
                if (j - 1 < 0)
                {
                    for (int k = 0; k < i + 2 && k < rows; k++)
                    {
                        for (int p = 0; p < j + 2 && p < columns; p++)
                        {
                            if (matrix.ElementAt(k).ElementAt(p).isPainted)
                            {
                                matrix.ElementAt(k).ElementAt(p).image = matrix.ElementAt(k).ElementAt(p).imageDefault;
                                matrix.ElementAt(k).ElementAt(p).isPainted = false;
                                points++;
                            }
                        }
                    }
                }
                else
                {
                    for (int k = 0; k < i + 2 && k < rows; k++)
                    {
                        for (int p = j - 1; p < j + 2 && p < columns; p++)
                        {
                            if (matrix.ElementAt(k).ElementAt(p).isPainted)
                            {
                                matrix.ElementAt(k).ElementAt(p).image = matrix.ElementAt(k).ElementAt(p).imageDefault;
                                matrix.ElementAt(k).ElementAt(p).isPainted = false;
                                points++;
                            }
                        }
                    }
                }
            }
            else
            {
                if (j - 1 < 0)
                {
                    for (int k = i - 1; k < i + 2 && k < rows; k++)
                    {
                        for (int p = 0; p < j + 2 && p < columns; p++)
                        {
                            if (matrix.ElementAt(k).ElementAt(p).isPainted)
                            {
                                matrix.ElementAt(k).ElementAt(p).image = matrix.ElementAt(k).ElementAt(p).imageDefault;
                                matrix.ElementAt(k).ElementAt(p).isPainted = false;
                                points++;
                            }
                        }
                    }
                }
                else
                {
                    for (int k = i - 1; k < i + 2 && k < rows; k++)
                    {
                        for (int p = j - 1; p < j + 2 && p < columns; p++)
                        {
                            if (matrix.ElementAt(k).ElementAt(p).isPainted)
                            {
                                matrix.ElementAt(k).ElementAt(p).image = matrix.ElementAt(k).ElementAt(p).imageDefault;
                                matrix.ElementAt(k).ElementAt(p).isPainted = false;
                                points++;
                            }
                        }
                    }
                }

            }
            numberOfSquaresPainted -= points;
            getComboScore = points * 100;
            score += getComboScore;
            if (numberOfSquaresPainted == 0)
                paintRandomSquares(numberOfSquares);
            gameSound.playBomb();
        }

        public void cutThem(int X, int Y)
        {
            int i = (Y - offset) / squareWidth;
            int j = (X - offset) / squareWidth;
            int points = 0;

            for (int k = 0; k < columns; k++)
            {
                if (matrix.ElementAt(i).ElementAt(k).isPainted)
                {
                    matrix.ElementAt(i).ElementAt(k).image = matrix.ElementAt(i).ElementAt(k).imageDefault;
                    matrix.ElementAt(i).ElementAt(k).isPainted = false;
                    points++;
                }
            }
            for (int k = 0; k < rows; k++)
            {
                if (matrix.ElementAt(k).ElementAt(j).isPainted)
                {
                    matrix.ElementAt(k).ElementAt(j).image = matrix.ElementAt(k).ElementAt(j).imageDefault;
                    matrix.ElementAt(k).ElementAt(j).isPainted = false;
                    points++;
                }
            }
            numberOfSquaresPainted -= points;
            getComboScore = points * 100;
            score += getComboScore;
            if (numberOfSquaresPainted == 0)
                paintRandomSquares(numberOfSquares);
            gameSound.playSword();
        }

    }
}