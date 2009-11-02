//The Game!
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
    public partial class MainPage : UserControl
    {
        private Rectangle block = null;
        private string turn = null;//"p1" or "p2"
        private const int BLOCK_SIZE = 15;
        private static int squares = 0;
        private static Dictionary<string, object> blocksHash = new Dictionary<string, object>(); // contains all blocks
        private List<string> adjs = new List<string>(); // contains current players adjacent squares
        private List<Rectangle> usedSquares = new List<Rectangle>();//Cannot move onto these squares
        private bool flag = false;//wtf is this for? Cow Pie Bingo 
        //Colors
        static SolidColorBrush p1color = new SolidColorBrush(Colors.Red); //Player 1 Square Color
        static SolidColorBrush p2color = new SolidColorBrush(Colors.Blue); //Player 2 Square Color
        static SolidColorBrush movecolor = new SolidColorBrush(Colors.Yellow); //Highlights where a player can move
        static SolidColorBrush portalcolor = new SolidColorBrush(Colors.Magenta); //Color of portal
        static SolidColorBrush gridcolor = new SolidColorBrush(Colors.White); //grid color
        static SolidColorBrush linecolor = new SolidColorBrush(Colors.Brown);
        //----------------------------------CREATES PLAYING GRID----
        public void build()
        {
            GameCanvas.Height = (squares * BLOCK_SIZE) + (BLOCK_SIZE * 2);
            GameCanvas.Width = (squares * BLOCK_SIZE) + (BLOCK_SIZE * 2);
            GameScroll.Width = (squares * BLOCK_SIZE) + (BLOCK_SIZE * 5);
            if (squares < 20)
                GameScroll.Height = GameScroll.Width;
            else
                GameScroll.Height = 400;
            P1WeaponStack.Opacity = 1;
            P2WeaponStack.Opacity = 1;
            PlayingStack.SetValue(Canvas.ZIndexProperty, 2);
            for (int x = 1; x <= squares; x++)//Row #
            {
                double left = x * (BLOCK_SIZE);
                for (int y = 1; y <= squares; y++)//Col #
                {
                    double top = y * (BLOCK_SIZE);
                    string tag = "rect" + x.ToString() + "x" + y.ToString();

                    Rectangle block = new Rectangle();
                    block.Fill = gridcolor;
                    block.Stroke = linecolor;
                    block.Width = BLOCK_SIZE;
                    block.Height = BLOCK_SIZE;
                    block.SetValue(Canvas.TopProperty, top);
                    block.SetValue(Canvas.LeftProperty, left);
                    block.MouseLeftButtonDown += block_MouseLeftButtonDown;
                    block.Tag = tag;
                    this.GameCanvas.Children.Add(block);
                    blocksHash.Add(tag, block);
                }
            }
            adjs.Clear();
            Random wgf = new Random();
            int whoGoesFirst = wgf.Next(1, 3);
            if (whoGoesFirst == 1)
            {
                turn="p1";
                adjs.Add("rect1x2");
                ((Rectangle)blocksHash[adjs[0]]).Fill = movecolor;
                adjs.Add("rect2x1");
                ((Rectangle)blocksHash[adjs[1]]).Fill = movecolor;
                adjs.Add("rect2x2");
                ((Rectangle)blocksHash[adjs[2]]).Fill = movecolor;
            }
            else
            {
                turn = "p2";
                adjs.Add("rect"+squares.ToString()+"x"+(squares-1).ToString());
                ((Rectangle)blocksHash[adjs[0]]).Fill = movecolor;
                adjs.Add("rect" + (squares-1).ToString() + "x" + (squares - 1).ToString());
                ((Rectangle)blocksHash[adjs[1]]).Fill = movecolor;
                adjs.Add("rect" + (squares-1).ToString() + "x" + squares.ToString());
                ((Rectangle)blocksHash[adjs[2]]).Fill = movecolor;
            }
            ((Rectangle)blocksHash["rect1x1"]).Fill = p1color;
            ((Rectangle)blocksHash["rect" + squares + "x" + squares]).Fill = p2color;
        }


        //--------HIGHLIGHTS SQUARES w/ GREEN--------
        public void turngreen(string adj)
        {
            if (turn == "p1")
            {
                adjs.Add(adj);
                for (int v = adjs.Count - 1; v > 0; v--)
                {
                    if (((Rectangle)blocksHash[adjs[v]]).Fill.GetValue(SolidColorBrush.ColorProperty).Equals(Colors.Green))
                    {
                        ((Rectangle)blocksHash[adjs[v]]).Fill = gridcolor;
                    }
                    
                }
                adjs.Clear();
            }
            else
            {
                adjs.Add(adj);
                for (int v = adjs.Count - 1; v > 0; v--)
                {
                    if (((Rectangle)blocksHash[adjs[v]]).Fill.GetValue(SolidColorBrush.ColorProperty).Equals(Colors.Green))
                    {
                        ((Rectangle)blocksHash[adjs[v]]).Fill = gridcolor;
                    }     
                }
                adjs.Clear();
            }
        }


        //------------------------MOVE-----------------------------
        public void move(Rectangle block)
        {
            new Error(msgboard4, " ");
            usedSquares.Add(block);

            if (turn == "p1")
                block.Fill = p1color;

            else
                block.Fill = p2color;

            String name = block.Tag.ToString(); // "rect1x1"
            name = name.Substring(4); // chop off "rect"
            int x = name.IndexOf("x"); // find the "x"
            int col = int.Parse(name.Substring(0, x)); // grab the column number
            int row = int.Parse(name.Substring(x + 1)); // grab the row number

            bool northExists, southExists, eastExists, westExists = false;

            northExists = row - 1 > 0 ? true : false;
            southExists = row + 1 <= squares ? true : false;
            eastExists = col + 1 <= squares ? true : false;
            westExists = col - 1 > 0 ? true : false;

            
                String adj = null;

                if (northExists && southExists && eastExists && westExists)
                {
                    for (int i = (col - 1); i <= (col + 1); i++)
                    {
                        for (int j = (row - 1); j <= (row + 1); j++)
                        {
                            if (i == col && j == row) continue;

                            adj = "rect" + i.ToString() + "x" + j.ToString();

                            turngreen(adj);

                            if (((Rectangle)blocksHash[adj]).Fill.GetValue(SolidColorBrush.ColorProperty).Equals(Colors.White))
                            {
                                ((Rectangle)blocksHash[adj]).Fill = movecolor;
                            }
                        }
                    }
                    flag = true;
                }
                if (turn == "p1")
                    turn = "p2";
                else
                    turn = "p1";
        }

        //---------------------------??COWPIE COWPOE CPWOE
        public MainPage()
        {
            InitializeComponent();
        }


        //-----------------------------PLAYER MOVE / LEFT MOUSE CLICK----
        private void block_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            block = (Rectangle)sender;
            if (adjs.Contains(block.Tag.ToString()))
                move(block);
            else
                new Error(msgboard4, "You Cannot Move there!");   
        }


        //----------------------------------SUBMIT BUTTON CLICK----
        private void Submit_Click(object sender, RoutedEventArgs e)
        {
            Regex regex = new Regex(@"^\d+$"); // Numbers only
            if (regex.IsMatch(NumSquares.Text.ToString()))
            {
                squares = int.Parse(NumSquares.Text);
                if ((squares < 42) && (squares > 4))
                {
                    build();
                }
                else
                {
                    new Error(msgboard1, "# between 4-42");
                }
            }
            else
            {
                new Error(msgboard1, "Numbers only.");
            }
        }


        // -------------------------------------WEAPON CLICK --------
        private void gernade1_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Image P1weapon = (Image)sender;
            P1weapon.Opacity = 0;
        }
    }

    //------------------------ERRORS THE GAME INTO CPB--------------
    public partial class Error
    {
        public Error(TextBlock textblock, String message)
        {
            textblock.Text = message;
        }
    }
}