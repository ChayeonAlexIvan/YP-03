﻿using System;
using System.Drawing;
using System.Windows.Forms;
using System.IO;

namespace Calc.v2._0
{
    public partial class Calc : Form
    {
        public Calc()
        {
            InitializeComponent();
            textResult.ReadOnly = true;
            textHistory.ReadOnly = true;
        }
        private bool isPoint = false; // запятая
        private bool isNum2 = false; // второе число
        private bool isOperationTwo = false; // вторая операция
        private bool isSaveResultTwo = false; // сохранение большого вычисления

        private bool isNumberZero = false; // второй нуль

        string currentOperation = "";

        private string num1 = null;
        private string num2 = null;

        private void AddNum(string txt)
        {
            if (isNum2)
            {
                num2 += txt;
                isOperationTwo = true;
                textResult.Text = num2;
            }
            else
            {
                num1 += txt;
                textResult.Text = num1;
            }
        }

        private void SetNum(string txt)
        {
            if (isNum2)
            {
                num2 = txt;
                textHistory.Text = num2;
            }
            else
            {
                num1 = txt;
                textHistory.Text = num1;
            }
        }
        private void SetNumPlusMinus(string txt)
        {
            if (isNum2)
            {
                num2 = txt;
            }
            else
            {
                num1 = txt;
            }
        }
        private void buttonNumberClick(object obj, EventArgs e)
        {
            var txt = ((Button)obj).Text;
            {
                if (isPoint && txt == ",") { return; }
                if (txt == ",") { isPoint = true; }
            }

            if(txt == "+/-")
            {
                if(textResult.Text.Length > 0)
                    if(textResult.Text[0] == '-')
                    {
                        textResult.Text = textResult.Text.Substring(1, textResult.Text.Length - 1);
                    }
                    else
                    {
                        textResult.Text = "-" + textResult.Text;
                    }
                SetNumPlusMinus(textResult.Text);
                return;
            }
            AddNum(txt);
        }

        private void buttonOperationClick(object obj, EventArgs e)
        {
            if (num1 == null)
            {
                if (textResult.Text.Length > 0)
                    num1 = textResult.Text;
                else
                    return;
            }
            isNum2 = true;
            if (isOperationTwo)
            {
                SetResultTwo(currentOperation);
                currentOperation = ((Button)obj).Text;
                Two(currentOperation);
            }
            else
            {
                currentOperation = ((Button)obj).Text;
                SetResult(currentOperation);
            }
        }
        private void SetResult(string operation)
        {
            string result = null;

            switch (operation)
            {
                case "+": { result = MathOperation.Addition(num1, num2); break; }
                case "-": { result = MathOperation.Subtraction(num1, num2); break; }
                case "*": { result = MathOperation.Multiplication(num1, num2); break; }
                case "/": { result = MathOperation.Division(num1, num2); break; }
                default:break;
            }
            if (isOperationTwo)
            {
                OutputResultTwo(result, operation);
            }
            else
                OutputResult(result, operation);
            if (isNum2) 
            {
                if (result != null)
                    num1 = result;
                isPoint = false;
                isNumberZero = false;
            }
        }
        string result1 = null;
        private void SetResultTwo(string operation)
        {
            string result = null;

            switch (operation)
            {
                case "+": { result = MathOperation.Addition(num1, num2); break; }
                case "-": { result = MathOperation.Subtraction(num1, num2); break; }
                case "*": { result = MathOperation.Multiplication(num1, num2); break; }
                case "/": { result = MathOperation.Division(num1, num2); break; }
                default: break;
            }
            result1 = result;
        }
        private void Two(string operation)
        {
            if (isOperationTwo)
            {
                OutputResultTwo(result1, operation);
            }
            else
                OutputResult(result1, operation);
            if (isNum2)
            {
                if (result1 != null)
                    num1 = result1;
                isPoint = false;
                isNumberZero = false;
            }
        }
        private void OutputResult(string result, string operation)
        {
            switch (operation)
            {
                default:
                    {
                        if (num2 != null)
                        {
                            textHistory.Text = num1 + " " + operation + " " + num2 + " " + "=";
                        }
                        else
                        {
                            if (num1 != null)
                            {
                                textHistory.Text = num1 + " " + operation + " ";
                                break; 
                            }
                        }
                    }
                    break;
            }
            num2 = null;
            if(result != null)
            {
                textResult.Text = result;
            }
        }
        private void OutputResultTwo(string result, string operation)
        {
            if (isSaveResultTwo)
                textHistory.Text += num2 + " = ";
            else
                textHistory.Text += num2 + " " + operation + " ";          
            num2 = null;
            if (result != null)
            {
                textResult.Text = result;
            }
        }
        private void buttonClear(object obj, EventArgs e)
        {
            textHistory.Text = "";
            textResult.Text = "";
            isNum2 = false;
            currentOperation = null;
            num1 = null;
            num2 = null;
            isPoint = false;
            isOperationTwo = false;
            isSaveResultTwo = false;
            isNumberZero = false;
        }
        private void buttonClearOne(object obj, EventArgs e)
        {
            textResult.Text = "0";
            isNum2 = true;
            num2 = null;
        }
        private void buttonResultClick(object obj, EventArgs e)
        {
            isSaveResultTwo = true;
            SetResult(currentOperation);
            isNum2 = false;
            num1 = null;
            num2 = null;
            isOperationTwo = false;
            isSaveResultTwo = false;
            isNumberZero = false;
        }
        private void buttonResetNumber(object obj, EventArgs e)
        {
            if(textResult.Text.Length <= 0)
            {
                return;
            }
            textResult.Text = textResult.Text.Substring(0, textResult.Text.Length - 1);
            SetNum(textResult.Text);
        }

        private void button19_Click(object sender, EventArgs e)
        {
            if(folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                string text = textHistory.Text;
                text += textResult.Text;
                string fileText = folderBrowserDialog1.SelectedPath;
                StreamWriter sw = new StreamWriter(new FileStream(fileText + "\\Test.txt", FileMode.Append, FileAccess.Write));
                sw.WriteLine("Было выполнено: " + DateTime.Now);
                sw.WriteLine("Выражение: " + text);
                sw.Close();
            }
        }

        private void button21_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private bool isClickMouse = false;
        private Point currentPosition = new Point(0, 0);

        private void panel1_MouseUp(object sender, MouseEventArgs e)
        {
            isClickMouse = false;
        }

        private void panel1_MouseDown(object sender, MouseEventArgs e)
        {
            isClickMouse = true;
            currentPosition = new Point(e.X, e.Y);
        }

        private void panel1_MouseMove(object sender, MouseEventArgs e)
        {
            if (!isClickMouse) { return; }
            Point buf = new Point(this.Location.X, this.Location.Y);
            buf.X += e.X - currentPosition.X;
            buf.Y += e.Y - currentPosition.Y;

            this.Location = buf;
        }
    }
}
