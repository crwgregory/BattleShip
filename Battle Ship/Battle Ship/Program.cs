using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Battle_Ship
{
    class Program
    {
        static void Main(string[] args)
        {
            Grid Game = new Grid();
            Game.PlayGame();
            Console.ReadKey();
        }
    }

    class Point
    {
        public enum Status { Empty, Ship, Hit, Miss, Target}

        public int XAxis { get; set; }
        public int YAxis { get; set; }
        public Status PointStatus { get; set; }

        public Point(int X, int Y)
        {
            this.XAxis = X;
            this.YAxis = Y;
            this.PointStatus = Status.Empty;
        }
    }

    class Ship
    {
        public enum Type { Carrier, Battleship, Cruiser, Submarine, Minesweeper}

        public Type ShipType { get; set; }
        public List<Point> OccupiedPoints { get; set; }
        public int Length { get; set; }
        
        public bool IsDestroyed
        {
            get
            {
                bool hit = false;
                foreach (Point item in OccupiedPoints)
                {
                    if (item.PointStatus == Point.Status.Hit)
                    {
                        hit = true;
                    }
                    else
                    {
                        hit = false;
                        break;
                    }
                }
                return hit;
            }
        }

        public Ship(Type typeOfShip)
        {
            this.OccupiedPoints = new List<Point>();
            this.ShipType = typeOfShip;
            switch (typeOfShip)
            {
                case Type.Carrier: this.Length = 5;
                    break;
                case Type.Battleship: this.Length = 4;
                    break;
                case Type.Cruiser: this.Length = 3;
                    break;
                case Type.Submarine: this.Length = 3;
                    break;
                case Type.Minesweeper: this.Length = 2;
                    break;
                default:
                    break;
            }
        }
    }

    class Grid
    {
        public enum PlaceShipDirection { Horizontal, Vertical}

        public Point[,] Ocean { get; set; }
        public List<Ship> ListOfShips { get; set; }
        public bool AllShipsDestroyed
        {
            get
            {
                bool AllDestroyed = false;
                foreach (Ship item in ListOfShips)
                {
                    if (item.IsDestroyed)
                    {
                        AllDestroyed = true;
                    }
                    else
                    {
                        AllDestroyed = false;
                        break;
                    }
                }
                return AllDestroyed;
            }
        }
        public int CombatRound { get; set; }
        public int SizeOcean { get; set; }
        public Point Target { get; set; }

        public Grid()
        {
            //Size of ocean
            this.SizeOcean = 10;
            this.Ocean = new Point[SizeOcean, SizeOcean];

            //create the target
            this.Target = new Point((SizeOcean / 2), (SizeOcean / 2));

            //create ocean and populate with empty points
            for (int y = 0; y < SizeOcean; y++)
            {
                for (int x = 0; x < SizeOcean; x++)
                {
                    Ocean[x, y] = new Point(x, y);
                }
            }

            this.ListOfShips = new List<Ship>();

            //Carrier, Battleship, Cruiser, Submarine, Minesweeper

            Ship BattleShip = new Ship(Ship.Type.Battleship);
            this.ListOfShips.Add(BattleShip);
            this.PlaceShip(BattleShip, PlaceShipDirection.Vertical, 0, 0);

            Ship Carrier = new Ship(Ship.Type.Carrier);
            this.ListOfShips.Add(Carrier);
            this.PlaceShip(Carrier, PlaceShipDirection.Vertical, 1, 0);

            Ship Cruiser = new Ship(Ship.Type.Cruiser);
            this.ListOfShips.Add(Cruiser);
            this.PlaceShip(Cruiser, PlaceShipDirection.Vertical, 2, 0);

            Ship Submarine = new Ship(Ship.Type.Submarine);
            this.ListOfShips.Add(Submarine);
            this.PlaceShip(Submarine, PlaceShipDirection.Vertical, 3, 0);

            Ship Minesweeper = new Ship(Ship.Type.Minesweeper);
            this.ListOfShips.Add(Minesweeper);
            this.PlaceShip(Minesweeper, PlaceShipDirection.Vertical, 4, 0);

            //call on all of ships above
            
        }

        public void PlaceShip(Ship shipToPlace, PlaceShipDirection direction, int startX, int startY)
        {
            for (int i = 0; i < shipToPlace.Length; i++)
            {
                //set starting point to ship
                Ocean[startX, startY].PointStatus = Point.Status.Ship;

                //Add point to list within the ship
                shipToPlace.OccupiedPoints.Add(Ocean[startX, startY]);

                if (direction == PlaceShipDirection.Horizontal)
                {
                    startX++;
                }
                else if (direction == PlaceShipDirection.Vertical)
                {
                    startY++;
                }
            }
        }

        public void DisplayOcean()
        {
            
            Console.Clear();
            Console.Write("  ");
            for (int i = 0; i < SizeOcean; i++)
            {
                Console.Write(" " + i + " ");
            }
            Console.WriteLine();
            for (int y = 0; y < SizeOcean; y++)
            {
                Console.Write(y);
                Console.Write("|");
                for (int x = 0; x < SizeOcean; x++)
                {
                    if (this.Target.XAxis == x && this.Target.YAxis == y)
                    {
                        Console.Write("[X]");
                    }
                    else if (Ocean[x, y].PointStatus == Point.Status.Empty)
                    {
                        Console.Write("[ ]");
                    }
                    else if (Ocean[x, y].PointStatus == Point.Status.Ship)
                    {
                        Console.Write("[S]");
                    }
                    else if (Ocean[x, y].PointStatus == Point.Status.Hit)
                    {
                        Console.Write("[H]");
                    }
                    else if (Ocean[x, y].PointStatus == Point.Status.Miss)
                    {
                        Console.Write("[O]");
                    }
                    else
                    {
                        Console.Write("[!]");
                    }
                    //if (Grid[x, y].PointValue > 1000)
                    //{
                    //    Console.Write("[ ]");
                    //}
                    //else
                    //{
                    //    Console.Write("[" + Grid[x, y].PointValue + "]");
                    //}
                }
                Console.WriteLine();
            }
        }

        public bool TargetInput(int x, int y)
        {
            int numberOfShipsDestroyed = this.ListOfShips.Where(j => j.IsDestroyed).Count();

            Point theTarget = Ocean[x, y];
            if (theTarget.PointStatus == Point.Status.Ship)
            {
                Ocean[x, y].PointStatus = Point.Status.Hit;
            }
            else if (theTarget.PointStatus == Point.Status.Empty)
            {
                Ocean[x, y].PointStatus = Point.Status.Miss;
            }

            int newNumberOfShipsDestryoed = this.ListOfShips.Where(j => j.IsDestroyed).Count();
            if (numberOfShipsDestroyed < newNumberOfShipsDestryoed)
            {
                return true;
            }
            return false;
        }

        public int GetUserMove()
        {
            //returning an integer for move, if up then 1, if 
            //down then 3, if right then 2, if right then 4.

            //[ ][ ][1][ ][ ]
            //[ ][4][0][2][ ]  
            //[ ][ ][3][ ][ ]

            ConsoleKey input = new ConsoleKey();
            bool gettingInput = true;
            while (gettingInput)
            {
                input = Console.ReadKey().Key;

                if (input == ConsoleKey.UpArrow)
                {

                    gettingInput = false;
                    return 1;
                }
                else if (input == ConsoleKey.DownArrow)
                {

                    gettingInput = false;
                    return 3;
                }
                else if (input == ConsoleKey.RightArrow)
                {

                    gettingInput = false;
                    return 2;
                }
                else if (input == ConsoleKey.LeftArrow)
                {

                    gettingInput = false;
                    return 4;
                }
                else
                {
                    Console.WriteLine("Press up arrow, down arrow, left arrow, right arrow.");

                }
            }
            return 0;
        }

        public void MoveTarget(int input)
        {
            int move = input;

            int targetX = this.Target.XAxis; int targetY = this.Target.YAxis;

            //Down
            if (input == 3 && (targetY + 1) < SizeOcean)
            {
                targetY++;
            }
            //Up
            else if (input == 1 && (targetY - 1) >= 0)
            {
                targetY--;
            }
            //Right
            else if (input == 2 && (targetX + 1) < SizeOcean)
            {
                targetX++;
            }
            //Left
            else if (input == 4 && (targetX - 1) >= 0)
            {
                targetX--;
            }

            this.Target.XAxis = targetX;
            this.Target.YAxis = targetY;
        }

        public void PlayGame()
        {
            while (!AllShipsDestroyed)
            {
                this.DisplayOcean();
                this.MoveTarget(this.GetUserMove());
                
            }
        }


    }

}
