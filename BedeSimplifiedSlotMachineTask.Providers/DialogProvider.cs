using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BedeSimplifiedSlotMachineTask.Providers
{
    public static class Prompt
    {
        public static void ShowInformationDialog(string text, string caption)
        {
            Form dialog = new Form();
            dialog.Width = 300;
            dialog.Height = 150;
            dialog.Text = caption;
            dialog.StartPosition = FormStartPosition.CenterParent;
            Label textLabel = new Label() { Left = 50, Top = 20, Width = 200 , Text = text };

            Button confirmation = new Button() { Text = "Ok", Left = 70, Width = 100, Top = 70 };
            confirmation.Click += (sender, e) => { dialog.Close(); };
            dialog.Controls.Add(confirmation);
            dialog.Controls.Add(textLabel);

            dialog.ShowDialog();
        }

        public static decimal ShowEnterCreditsDialog(string text, string caption)
        {
            Form prompt = new Form()
            {
                Width = 500,
                Height = 150,
                FormBorderStyle = FormBorderStyle.FixedDialog,
                Text = caption,
                StartPosition = FormStartPosition.CenterScreen
            };

            Label textLabel = new Label() { Left = 50, Top = 20,Width = 400, Text = text };

            NumericUpDown inputBox = new NumericUpDown() { Left = 50, Top = 50, Width = 100 };

            Button confirmation = new Button() { Text = "Play", Left = 250, Width = 100, Top = 49, DialogResult = DialogResult.OK };
            confirmation.Click += (sender, e) => { prompt.Close(); };

            prompt.Controls.Add(inputBox);
            prompt.Controls.Add(confirmation);
            prompt.Controls.Add(textLabel);
            prompt.AcceptButton = confirmation;

            prompt.ShowDialog();

            if (prompt.DialogResult != DialogResult.OK)
            {
                Environment.Exit(1);
            }

            return inputBox.Value;
        }
    }
}
