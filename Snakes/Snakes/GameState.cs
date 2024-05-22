using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snakes
{
    public class GameState
    {
        public int Rows { get; }
        public int Columns { get; }
        public GridValue[,] Grid { get; }
        public Direction Direction { get; private set; }
        public int Score { get; private set; }
        public  bool GameOver { get; private set; }

        public readonly LinkedList<Position> snakesPositions = new LinkedList<Position>();
        public readonly Random random = new Random(); //will be used to figure out where the food should go

        public GameState(int rows, int cols)
        {
            Rows = rows;
            Columns = cols;
            Grid = new GridValue[rows, cols];
            Direction = Direction.Right;

            AddSnake();
            AddFood();
        }

        private void AddSnake()
        {
            //must appearin the middle row
            int r = Rows / 2;
            for (int c = 1; c <= 3; c++)
            {
                Grid[r, c] = GridValue.Snake;
                snakesPositions.AddFirst(new Position(r, c));
            }
        }//Adds the snake to the grid

        private IEnumerable<Position> EmptyPositions()
        {
            for (int r = 0; r < Rows; r++)
            {
                for (int c = 0; c < Columns; c++)
                {
                    if (Grid[r,c] == GridValue.Empty)
                    {
                        yield return new Position(r, c);
                    }
                }
            }
        } //Ensuring theres empty spaces to add food

        private void AddFood()
        {
            List<Position> empty = new List<Position>(EmptyPositions());
            if(empty.Count == 0)
            {
                return;
            }

            Position pos = empty[random.Next(empty.Count)];
            Grid[pos.Row, pos.Column] = GridValue.Food;
        }//Adding food to random empty positions

        public Position HeadPosition()
        {
            return snakesPositions.First.Value;
        }

        public Position TailPosition()
        {
            return snakesPositions.Last.Value;
        }

        public IEnumerable<Position> SnakePositions()
        {
            return snakesPositions;
        }//Returns all the positions of the snake

        private void AddHead(Position pos)
        {
            snakesPositions.AddFirst(pos);
            Grid[pos.Row, pos.Column] = GridValue.Snake;
        }//Adds the given position to the front of the snake

        private void RemoveHead()
        {
            Position head = snakesPositions.First.Value;
            Grid[head.Row, head.Column] = GridValue.Empty;
            snakesPositions.RemoveFirst();
        }

        private void RemoveTail()
        {
            Position tail = snakesPositions.Last.Value;
            Grid[tail.Row, tail.Column] = GridValue.Empty;
            snakesPositions.RemoveLast();
        }

        public void ChangeDirection(Direction direction)
        {
            Direction = direction;

        }

        private bool OutsideGrid(Position pos)
        {
            return pos.Row < 0 || pos.Row >= Rows || pos.Column < 0 || pos.Column >= Columns;
        }

        private GridValue WillHit(Position newHeadPos)
        {
            if (OutsideGrid(newHeadPos))
            {
                return GridValue.Outside;
            }

            if(newHeadPos == TailPosition())
            {
                return GridValue.Empty;
            }
            return Grid[newHeadPos.Row, newHeadPos.Column];
        }

        public void Move()
        {
            Position newHeadPosition = HeadPosition().Translate(Direction);
            GridValue hit = WillHit(newHeadPosition);

            if(hit == GridValue.Outside || hit == GridValue.Snake)
            {
                GameOver = true;
            }
            else if (hit == GridValue.Empty)
            {
                RemoveTail();
                AddHead(newHeadPosition);
            }
            else if (hit == GridValue.Food)
            {
                AddHead(newHeadPosition);
                Score++;
                AddFood();
            }
        }
    }
}
