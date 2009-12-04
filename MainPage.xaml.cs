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
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Text.RegularExpressions;


namespace thegame
{
    public partial class MainPage : UserControl
    {
        private Player p = new Player(); //Player Class
        private Rectangle block = null;
        private string turn = null;//"p1" or "p2"
        private const int BLOCK_SIZE = 15;
        private static int squares = 0;
        private static Dictionary<string, object> blocksHash = new Dictionary<string, object>(); // contains all blocks
        private List<string> adjs = new List<string>(); // contains current players adjacent squares
        private List<string> usedSquares = new List<string>();//Cannot move onto these squares
        private bool flag = false;//wtf is this for? Cow Pie Bingo
        private Rectangle p1pos, p2pos, p3pos, p4pos = null; //current positions
        //Colors
        static SolidColorBrush p1color = new SolidColorBrush(Colors.Red); //Player 1 Square
        static SolidColorBrush p2color = new SolidColorBrush(Colors.Blue); //Player 2 Square
        static SolidColorBrush movecolor = new SolidColorBrush(Colors.Yellow); //Highlights where a player can move
        static SolidColorBrush portalcolor = new SolidColorBrush(Colors.Magenta); //portal
        static SolidColorBrush gridcolor = new SolidColorBrush(Colors.Transparent); //grid
        static SolidColorBrush linecolor = new SolidColorBrush(Colors.Brown);//grid lines

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
                turn = "p1";
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
                adjs.Add("rect" + squares.ToString() + "x" + (squares - 1).ToString());
                ((Rectangle)blocksHash[adjs[0]]).Fill = movecolor;
                adjs.Add("rect" + (squares - 1).ToString() + "x" + (squares - 1).ToString());
                ((Rectangle)blocksHash[adjs[1]]).Fill = movecolor;
                adjs.Add("rect" + (squares - 1).ToString() + "x" + squares.ToString());
                ((Rectangle)blocksHash[adjs[2]]).Fill = movecolor;
            }
            p1pos=((Rectangle)blocksHash["rect1x1"]);
            usedSquares.Add("rect1x1");
            p1pos.Fill = p1color;
            p2pos = ((Rectangle)blocksHash["rect" + squares + "x" + squares]);
            usedSquares.Add("rect" + squares + "x" + squares);
            p2pos.Fill = p2color;
        }

        //-----------------------------??COWPIE COWPOE CPWOE----
        public MainPage()
        {
            InitializeComponent();

        }

        //-----------------------------LEFT MOUSE CLICK on GRID----
        private void block_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (adjs.Count>0)//If no adjacent squares are open, Player loses
            {
                block=(Rectangle)sender;
                if (adjs.Contains(block.Tag.ToString()))//If player does not select an adj sqr, they cannot move
                {
                    for (int x = adjs.Count() - 1; x >= 0; x--)//clears squares
                        ((Rectangle)blocksHash[adjs[x]]).Fill = gridcolor;
                    adjs.Clear();
                    usedSquares.Add(block.Tag.ToString());
                    if (turn == "p1") //Player1's turn
                    {
                        p1pos = block;
                        adjs = p.move(p2pos, squares,usedSquares);
                        p1pos.Fill= p1color;
                        turn = "p2";    
                    }
                    else //Player2's turn
                    {
                        p2pos = block;
                        adjs = p.move(p1pos, squares,usedSquares);
                        p2pos.Fill = p2color;
                        turn = "p1"; 
                    }
                }                  
                else
                    new Error(msgboard4, "You Cannot Move there!");
                for (int x = adjs.Count() - 1; x >= 0; x--)//Cycle through the adjs list and fill those rects
                    ((Rectangle)blocksHash[adjs[x]]).Fill = movecolor;
            }
            else
                msgboard4.Text = "Game Over! " + turn + ", You lose!";

            flag = true; 

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
            Point pt = e.GetPosition(null);
            Cursor = Cursors.None;
            Canvas.SetTop(gernade1, pt.Y);
            Canvas.SetLeft(gernade1, pt.X);
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

    //Custom Curors
    public partial class ccursor : UserControl
    {
        public void setcursorimage(string source)
        {
            Uri uri = new Uri(source, UriKind.Relative);
            elCursor.source = new System.Windows.Media.Imaging.BitmapImage(uri);
        }
    }
}