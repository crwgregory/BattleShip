﻿using System;
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
        public bool TargetJustAttacked { get; set; }
        public Random rng { get; set; }

        public Grid()
        {
            //Size of ocean
            this.rng = new Random();
            this.TargetJustAttacked = false;
            this.SizeOcean = 13;
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

            //place ships randomly within the ocean.
            foreach (Ship item in ListOfShips)
            {
                


                
            }
            
        }

        public void PlaceShip(Ship shipToPlace)
        {
            int placementX = 0; int placementY = 0;

            int Horizontal = (int)PlaceShipDirection.Horizontal;
            int Vertical = (int)PlaceShipDirection.Vertical;

            //random numbers to be used for {0}horizontal or {1} vertica; placement
            int Random = rng.Next(0, 2);
            
            

            //bool set to false if there is already a ship in the position
            bool currentShipPlace = false;

            //if this currentShipPlace gets set to true at anytime during the check it will loop again and get new starting numbers
            //loops again if it can't place ship
            for (int x = 0; x < 100; x++)
			{

                //random numbers to be used for startX and startY
                int randomX = rng.Next(0, SizeOcean);
                int randomY = rng.Next(0, SizeOcean);
			
                for (int i = 0; i < shipToPlace.Length; i++)
                {
                    //gets all of the occupied points of the ships within the list of ships
                    //used for cross checking new ship placement


                    foreach (Ship points in ListOfShips)
                    {
                        foreach (Point inside in points.OccupiedPoints)
                        {
                            if (inside.XAxis == placementX && inside.YAxis == placementY)
                            {
                                //if this is true that means there is already a ship here and new start points must be entered.
                                currentShipPlace = true;
                                break;
                            }
                        }
                        if (currentShipPlace)
                        {
                            break;
                        }
                       
                    }

                    //if current ship place has been set to true, we need to get new starting point numbers
                    if (currentShipPlace)
                    {
                        break;
                    }

                    if (Random == 0)    //horizontal placement
                    {
                        placementX++;
                    }
                    else if (Random == 1)       //vertical
                    {
                        placementY++;
                    }

                } 
                //if we made it this far with currentShipPlace still being false then we have a positive place to put the ship
                //and we can break the loop getting new numbers
                if (!currentShipPlace)
                {
                    break;
                }

            }




            //if there is no current ship place corisponding to future place of the chip to be placed... place the ship
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

        public void DisplayOcean()
        {
            
            Console.Clear();
            Console.Write("   ");
            for (int i = 0; i < SizeOcean; i++)
            {
                if (i > 9)
	            {
                    if (i == 10)
                    {
                        Console.Write("  ");
                    }
		            Console.Write(i + "  ");
	            }
                else
                {
                    Console.Write("  " + i + " "); 
                }
            }
            Console.WriteLine();
            for (int y = 0; y < SizeOcean; y++)
            {
                if (y < 10)
                {
                    Console.Write(" ");
                }
                Console.Write(y);
                Console.Write("|");
                for (int x = 0; x < SizeOcean; x++)
                {
                    {
                        Console.Write(" ");
                    }
                    if (this.Target.XAxis == x && this.Target.YAxis == y && !TargetJustAttacked)
                    {
                        Console.Write(" X ");
                    }
                    else if (Ocean[x, y].PointStatus == Point.Status.Empty)
                    {
                        Console.Write("/~/");
                    }
                    else if (Ocean[x, y].PointStatus == Point.Status.Ship)
                    {
                        Console.Write("/~/");
                    }
                    else if (Ocean[x, y].PointStatus == Point.Status.Hit)
                    {
                        Console.Write("/!/");
                    }
                    else if (Ocean[x, y].PointStatus == Point.Status.Miss)
                    {
                        Console.Write("   ");
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
            TargetJustAttacked = false;
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
                else if (input == ConsoleKey.Spacebar)
                {
                    return 10;
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
            if (input != 10)
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
        }

        public bool WasHit(int userMove)
        {
            if (userMove == 10)
            {
                TargetJustAttacked = true;
                if (Ocean[this.Target.XAxis, this.Target.YAxis].PointStatus == Point.Status.Ship)
                {
                    Ocean[this.Target.XAxis, this.Target.YAxis].PointStatus = Point.Status.Hit;
                    Console.WriteLine("You hit!");
                    return true;
                }
                else if (Ocean[this.Target.XAxis, this.Target.YAxis].PointStatus == Point.Status.Empty)
                {
                    Ocean[this.Target.XAxis, this.Target.YAxis].PointStatus = Point.Status.Miss;
                    Console.WriteLine("You missed");
                } 
            }
            
            return false;
        }

        public bool ValidUserAttack()
        {
            ConsoleKey input = Console.ReadKey().Key;
            if (input == ConsoleKey.Spacebar)
            {
                return true;
            }
            else
            {
                Console.WriteLine("Not a valid entry");
            }
            return false;
        }

        public void PlayGame()
        {
            int userMove = 0;
            while (!AllShipsDestroyed)
            {
                this.DisplayOcean();
                userMove = this.GetUserMove();
                this.MoveTarget(userMove);

                if (WasHit(userMove))
                {

                }
                
            }
            
        }


    }

}
