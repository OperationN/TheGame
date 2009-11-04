//The Game! -- Player Class
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Text.RegularExpressions;


namespace thegame
{
    public class Player
    {
        string test;
        List<String> adjsqrs = new List<String>();
        Rectangle myrect=new Rectangle();
        bool canmove;

        public List<String> adjsquares(Rectangle activerect, int gridsize)
        {
            string name = activerect.Tag.ToString();
            name = name.Substring(4);
            int x = name.IndexOf("x"); // find the "x"
            int col = int.Parse(name.Substring(0, x)); // grab the column number
            int row = int.Parse(name.Substring(x + 1)); // grab the row number

            bool northExists, southExists, eastExists, westExists = false;

            northExists = row - 1 > 0 ? true : false;
            southExists = row + 1 <= gridsize ? true : false;
            eastExists = col + 1 <= gridsize ? true : false;
            westExists = col - 1 > 0 ? true : false;

           // if ((col > 1) && (row > 1) && (col < gridsize) && (row < gridsize))
            //{
                for (int n = col - 1; n == col + 1; n++)
                {
                    for (int y = row - 1; y == row + 1; y++)
                    {
                        myrect.Name = "rect" + n + "x" + y;
                        adjsqrs.Add(myrect.Name);
                    }
                }
            //}
            return adjsqrs;
        }
        public bool move(Rectangle click)
        {


            return canmove;
        }

    }
}