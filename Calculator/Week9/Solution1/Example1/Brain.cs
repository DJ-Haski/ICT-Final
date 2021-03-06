using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Example1
{
    delegate void DisplayMessage(string text);

    class Brain
    {
        DisplayMessage displayMessage;
        public Brain(DisplayMessage displayMessageDelegate)
        {
            displayMessage = displayMessageDelegate;
        }

        string[] nonZeroDigit = { "1", "2", "3", "4", "5", "6", "7", "8", "9" };
        string[] digit = { "0", "1", "2", "3", "4", "5", "6", "7", "8", "9" };
        string[] zero = { "0" };
        string[] operation = { "+" , "-", "X", "/","1/x"};
        string[] equal = { "=" };

        enum State
        {
            Zero,
            AccumulateDigits,
            ComputePending,
            Compute,
        }

        State currentState = State.Zero;
        string previousNumber = "";
        string currentNumber = "";
        string currentOperation = "";
        public void ProcessSignal(string message)
        {
            switch (currentState)
            {
                case State.Zero:
                    ProcessZeroState(message, false);
                    break;
                case State.AccumulateDigits:
                    ProcessAccumulateDigits(message, false);
                    break;
                case State.ComputePending:
                    ProcessComputePending(message, false);
                    break;
                case State.Compute:
                    break;
                default:
                    break;
            }
        }

        void ProcessZeroState(string msg, bool income)
        {
            if (income)
            {
                currentState = State.Zero;
            }
            else
            {
                if (nonZeroDigit.Contains(msg))
                {
                    ProcessAccumulateDigits(msg, true);
                }
            }
        }


        void ProcessAccumulateDigits(string msg, bool income)
        {
            if (income)
            {
                currentState = State.AccumulateDigits;
                if (zero.Contains(currentNumber))
                {
                    currentNumber = msg;
                }
                else
                {
                    currentNumber = currentNumber + msg;
                }
                displayMessage(currentNumber);
            }
            else
            {
                if (digit.Contains(msg))
                {
                    ProcessAccumulateDigits(msg, true);
                }
                else if (operation.Contains(msg))
                {
                    ProcessComputePending(msg, true);
                }
                else if (equal.Contains(msg))
                {
                    ProcessCompute(msg, true);
                }
            }

        }

        void ProcessComputePending(string msg, bool income)
        {
            if (income)
            {
               
                currentState = State.ComputePending;
                
                    previousNumber = currentNumber;
                    currentNumber = "";
                
                
                
                currentOperation = msg;
                Console.WriteLine(currentOperation);
            }
            else
            {
                if (digit.Contains(msg))
                {
                    ProcessAccumulateDigits(msg, true);
                }
            }
        }

        void ProcessCompute(string msg, bool income)
        {
            if (income)
            {
                currentState = State.Compute;
                
                Console.WriteLine(previousNumber+"here");
                double a = double.Parse(previousNumber);
                double b = double.Parse(currentNumber);

                /*if (currentOperation == "+")
                {
                    currentNumber = (a + b).ToString();
                }*/

                switch (currentOperation)
                {
                    case "+":
                        currentNumber = (a + b).ToString();
                        break;
                    case "-":
                        currentNumber = (a - b).ToString();
                        break;
                    case "X":
                        currentNumber = (a * b).ToString();
                        break;
                    case "/":
                        currentNumber = (a / b).ToString();
                        break;
                    case "1/x":
                        currentNumber = (1 / a).ToString();
                        break;

                    default:
                        break;

                }
                

                previousNumber = currentNumber;

                displayMessage(currentNumber);

                currentNumber = "";



                currentState = State.Zero;
                

            }
            else
            {
               // ProcessComputePending(msg, true);
            }
        }
    }
}
