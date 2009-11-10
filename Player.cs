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
        List<String> adjsqrs = new List<String>();

        public List<String> move(Rectangle activerect, int gridsize)
        {
            string name = activerect.Tag.ToString();//rect1x1
            int col = int.Parse(name.Substring(4, 1)); //grab the column number (x)
            int row = int.Parse(name.Substring(4, 1)); //grab the row number (y) WTF:Why doesnt 6 work instead of 4!!

            bool northExists, southExists, eastExists, westExists = false;

            northExists = row - 1 > 0 ? true : false;
            southExists = row + 1 <= gridsize ? true : false;
            eastExists = col + 1 <= gridsize ? true : false;
            westExists = col - 1 > 0 ? true : false;
            //test for openings using above variables here
                for (int x = col - 1; x <= col + 1; x++)
                {
                    for (int y = row - 1; y <= row + 1; y++)
                    {
                        adjsqrs.Add("rect" + x.ToString() + "x" + y.ToString());
                        
                    }
                }
            return adjsqrs;
         }
            
            
        }
 }
