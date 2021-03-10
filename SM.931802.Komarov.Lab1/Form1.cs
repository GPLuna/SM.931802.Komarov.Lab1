using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApplication4
{
    public partial class Form1 : Form
    {
        Dictionary<CheckBox, Cell> field = new Dictionary<CheckBox, Cell>();
        Dictionary<string, int> Price = new Dictionary<string, int>();
        private int day = 0;
        private int money = 100;
        private int Tick = 10;

        public Form1()
        {
            InitializeComponent();
            foreach (CheckBox cb in panel1.Controls)
                field.Add(cb, new Cell());
            InitializePrice();
        }

        private void InitializePrice()
        {
            Price.Add("plant", -2);
            Price.Add("yellow", 3);
            Price.Add("red", 5);
            Price.Add("brown", -1);
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox cb = (sender as CheckBox);
            if (cb.Checked) Plant(cb);
            else Harvest(cb);
        }

        private void Plant(CheckBox cb)
        {
            if (checkMoney(Price["plant"]))
            {
                field[cb].Plant();
                UpdateBox(cb);
            }
        }

        private bool checkMoney(int sum)
        {
            if (money <- sum) return false;
            money += sum;
            return true;
        }

        private bool Pay(Cell veg)
        {
            int earn = 0;
            switch (veg.state)
            {
                case CellState.Immature:
                    earn = Price["yellow"];
                    break;
                case CellState.Mature:
                    earn = Price["red"];
                    break;
                case CellState.Overgrown:
                    if (!checkMoney(Price["brown"]))
                        return false;
                    break;
                default:
                    break;
            }
            money += earn;
            return true;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            foreach (CheckBox cb in panel1.Controls)
                NextStep(cb);
            day++;
            LDay.Text = "Day: " + day;
            LMoney.Text = "Money: " + money;
        }


        private void Harvest(CheckBox cb)
        {
            if (Pay(field[cb]))
            {
                field[cb].Harvest();
                UpdateBox(cb);
            }
        }

        private void NextStep(CheckBox cb)
        {
            field[cb].NextStep();
            UpdateBox(cb);
        }

        private void UpdateBox(CheckBox cb)
        {
            Color c = Color.White;
            switch (field[cb].state)
            {
                case CellState.Planted:
                    c = Color.Black;
                    break;
                case CellState.Green:
                    c = Color.Green;
                    break;
                case CellState.Immature:
                    c = Color.Yellow;
                    break;
                case CellState.Mature:
                    c = Color.Red;
                    break;
                case CellState.Overgrown:
                    c = Color.Brown;
                    break;
            }
            cb.BackColor = c;
        }

        enum CellState
        {
            Empty,
            Planted,
            Green,
            Immature,
            Mature,
            Overgrown
        }

        class Cell
        {
            public CellState state = CellState.Empty;
            public int progress = 0;

            private const int prPlanted = 20;
            private const int prGreen = 100;
            private const int prImmature = 120;
            private const int prMature = 140;

            public void Plant()
            {
                state = CellState.Planted;
                progress = 1;
            }

            public void Harvest()
            {
                state = CellState.Empty;
                progress = 0;
            }

            public void NextStep()
            {
                if ((state != CellState.Empty) && (state != CellState.Overgrown))
                {
                    progress++;
                    if (progress < prPlanted) state = CellState.Planted;
                    else if (progress < prGreen) state = CellState.Green;
                    else if (progress < prImmature) state = CellState.Immature;
                    else if (progress < prMature) state = CellState.Mature;
                    else state = CellState.Overgrown;
                }
            }
        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            timer1.Stop();
            timer1.Interval = Convert.ToInt32(Tick*(10-trackBar1.Value));
            timer1.Start();
        }
    }
}