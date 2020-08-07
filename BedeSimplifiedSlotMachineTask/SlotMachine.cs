using BedeSimplifiedSlotMachine.Models.Contracts;
using BedeSimplifiedSlotMachine.Models;
using System;
using System.Configuration;
using System.Collections.Specialized;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Linq;
using BedeSimplifiedSlotMachineTask.Models.Models;
using BedeSimplifiedSlotMachineTask.Providers;
using System.Drawing;
using System.Resources;
using System.Globalization;
using System.Collections;
using BedeSimplifiedSlotMachineTask.Providers.Constants;
using BedeSimplifiedSlotMachine.Helpers;

namespace BedeSimplifiedSlotMachineTask
{
    public partial class SlotMachine : Form
    {
        private decimal Bet;
        private decimal Credits;
        private readonly Random randomProvider;

        public SlotMachine()
        {
            InitializeComponent();

            this.randomProvider = new Random();

            var dialogResult = Prompt.ShowEnterCreditsDialog("Deposit amount", "Enter Deposit");
            SetCreditsAmount(dialogResult);

            SetInitialSlotMachineImages();

        }

        private void SetInitialSlotMachineImages()
        {
            var picBoxes = new Stack<PictureBox>(Controls.OfType<PictureBox>());
            foreach (var box in picBoxes)
            {
                box.Image = Properties.Resources.q;
            }
        }

        private void SetCreditsAmount(decimal dialogResult)
        {
            Credits = dialogResult;

            CreditsVal.Text = dialogResult.ToString();
        }

        private void SetSlotMachineImages(ISlotMachineItem[,] matrix, PictureBox[,] pictureBoxMatrix)
        {
            ResourceSet resourceSet = Properties.Resources.ResourceManager.GetResourceSet(CultureInfo.CurrentCulture, false, true);
            var imageDictionary = new Dictionary<string, Bitmap>();

            foreach (DictionaryEntry entry in resourceSet)
            {
                String name = entry.Key.ToString();
                var resource = (Bitmap)entry.Value;
                imageDictionary.Add(name, resource);
            }

            for (int row = 0; row < MatrixSizeConstants.Rows; row++)
            {
                for (int col = 0; col < MatrixSizeConstants.Cols; col++)
                {
                    pictureBoxMatrix[row, col].Image = imageDictionary[(matrix[row, col].Symbol)];
                }
            }

        }

        public PictureBox[,] GetPictureBoxMatrix(int rows, int cols)
        {
            var picBoxes = new Stack<PictureBox>(Controls.OfType<PictureBox>());
            var picBoxMatrix = new PictureBox[4, 3];

            for (int row = 0; row < 4; row++)
            {
                for (int col = 0; col < 3; col++)
                {
                    picBoxMatrix[row, col] = picBoxes.Pop();
                }
            }
            return picBoxMatrix;
        }

        //public static string ShowDialog(string text, string caption)
        //{
        //    Form prompt = new Form()
        //    {
        //        Width = 500,
        //        Height = 150,
        //        FormBorderStyle = FormBorderStyle.FixedDialog,
        //        Text = caption,
        //        StartPosition = FormStartPosition.CenterScreen
        //    };

        //    Label textLabel = new Label() { Left = 50, Top = 20, Text = text };

        //    TextBox textBox = new TextBox() { Left = 50, Top = 50, Width = 200 };
        //    textBox.KeyPress += TextBox_KeyPress;

        //    Button confirmation = new Button() { Text = "Play", Left = 250, Width = 100, Top = 49, DialogResult = DialogResult.OK };
        //    confirmation.Click += (sender, e) => { prompt.Close(); };

        //    prompt.Controls.Add(textBox);
        //    prompt.Controls.Add(confirmation);
        //    prompt.Controls.Add(textLabel);
        //    prompt.AcceptButton = confirmation;

        //    prompt.ShowDialog();

        //    if (prompt.DialogResult != DialogResult.OK)
        //    {
        //        Environment.Exit(1);
        //    }

        //    return textBox.Text;
        //}

        private static void TextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void SpinBtn_Click(object sender, EventArgs e)
        {
            this.Bet = betNumericUpDown.Value;

            this.Credits = Convert.ToDecimal(CreditsVal.Text);

            if (Bet <= this.Credits && Bet > 0)
            {
                RemoveBetFromCredits(this.Credits, this.Bet);


                var SlotMachineItemsArray = Shuffle(GetSlotMachineItems());
                //var SlotMachineItemsArray = SlotMachineHelper.Shuffle(GetSlotMachineItems());

                var matrix = GenerateMatrix(SlotMachineItemsArray);
                //set images
                var picBoxMatrix = GetPictureBoxMatrix(4, 3);
                SetSlotMachineImages(matrix, picBoxMatrix);
                // calc win
                var winCoef = GetTotalWinningRowsCoefficient(matrix);
                if (winCoef > 0)
                {
                    //add credits
                    var winAmount = AddCreditsToCreditAmount(winCoef);
                    //write win message
                    SetSpinResultText(SpinResult.Win, winAmount);
                }
                else
                {
                    //RemoveBetFromCredits(this.Credits, this.Bet);
                    //write loss message
                    SetSpinResultText(SpinResult.Loss, Bet);
                }
            }
            else if (this.Credits <= 0)
            {
                decimal dialogResult = Prompt.ShowEnterCreditsDialog("You don't have enough credits. Please add more to continue playing", "Not enough credits");

                SetCreditsAmount(dialogResult);
            }
            else if (this.Bet > this.Credits)
            {
                Prompt.ShowInformationDialog("You're bet is bigger than your credits. Please lower your bet to continue playing", "Bet bigger than credits");

            }
        }

        private void SetSpinResultText(SpinResult result, decimal credits)
        {
            WinLabel.Text = result.ToString();
            WinVal.Text = credits.ToString();
        }

        private void RemoveBetFromCredits(decimal creditsAmount, decimal betAmount)
        {
            this.Credits = creditsAmount - betAmount;
            CreditsVal.Text = Credits.ToString();
        }

        private decimal AddCreditsToCreditAmount(decimal winCoef)
        {
            decimal winAmount = Math.Round((this.Bet * winCoef), 2);
            this.Credits += winAmount;

            //WinLabel.Text = "Win";
            CreditsVal.Text = Math.Round(this.Credits, 2).ToString();
            return winAmount;
        }

        private decimal GetTotalWinningRowsCoefficient(ISlotMachineItem[,] matrix)
        {
            decimal totalCoefficient = 0;

            for (int row = 0; row < matrix.GetLength(0); row++)
            {
                decimal rowCoefficient = 0;
                bool isWinningRow = true;
                ISlotMachineItem prevItem = null;

                for (int col = 0; col < matrix.GetLength(1); col++)
                {
                    var currentItem = matrix[row, col];

                    if (currentItem.IsWildCard)
                    {
                        continue;
                    }

                    if (prevItem != null && prevItem != currentItem)
                    {
                        isWinningRow = false;

                        break;
                    }

                    rowCoefficient += currentItem.Coefficient;
                    prevItem = currentItem;

                }
                if (isWinningRow)
                {
                    totalCoefficient += rowCoefficient;
                }

            }
            return totalCoefficient;
        }

        private void betNumericUpDown_ValueChanged(object sender, EventArgs e)
        {
            this.Bet = betNumericUpDown.Value;
        }

        public void SetMatrixItemsImages()
        {

        }

        public ISlotMachineItem[,] GenerateMatrix(IList<ISlotMachineItem> items)
        {
            var slotMachiteItems = items;

            int rows = MatrixSizeConstants.Rows;
            int cols = MatrixSizeConstants.Cols;

            var matrix = new ISlotMachineItem[rows, cols];
            for (int row = 0; row < rows; row++)
            {
                for (int col = 0; col < cols; col++)
                {
                    var randomNumber = GetRandomNumber(0, 100);

                    matrix[row, col] = slotMachiteItems[randomNumber];
                }
            }

            return matrix;
        }

        int GetRandomNumber(int minRange, int maxRange)
        {
            return this.randomProvider.Next(minRange, maxRange);
        }

        List<ISlotMachineItem> GetSlotMachineItems()
        {
            var itemsArray = new List<ISlotMachineItem>();

            var apple = new SlotMachineItem("Apple", 0.4m, 45, null, false, "a");
            var banana = new SlotMachineItem("Banana", 0.6m, 35, null, false, "b");
            var pineapple = new SlotMachineItem("Pineapple", 0.8m, 15, null, false, "p");
            var wildcard = new SlotMachineItem("Wildcard", 0, 5, null, true, "w");

            var slotMachineItems = new List<ISlotMachineItem>
            {
                apple,
                banana,
                pineapple,
                wildcard
            };


            foreach (var item in slotMachineItems)
            {
                var imageProvider = new ImageProvider();

                item.Image = imageProvider.GetImageLocation(item.Symbol);
                for (int i = 0; i < item.ProbabilityPercent; i++)
                {
                    itemsArray.Add(item);
                }

            }

            return itemsArray;
        }

        private static Random random = new Random();

        public List<ISlotMachineItem> Shuffle(List<ISlotMachineItem> list)
        {
            int n = list.Count;
            while (n > 1)
            {
                n--;
                int k = random.Next(n + 1);
                ISlotMachineItem value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
            return list;
        }

        private enum SpinResult
        {
            Win,
            Loss
        }
    }
}
