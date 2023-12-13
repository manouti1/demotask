// See https://aka.ms/new-console-template for more information
using System.Text.RegularExpressions;

string binaryString1 = "110100";
string binaryString2 = "101011";

bool isGood1 = IsGoodBinaryString(binaryString1);
bool isGood2 = IsGoodBinaryString(binaryString2);

Console.WriteLine($"{binaryString1} is good: {isGood1}");
Console.WriteLine($"{binaryString2} is good: {isGood2}");


static bool IsGoodBinaryString(string binaryString)
{
    // Validate input
    if (string.IsNullOrEmpty(binaryString))
    {
        throw new ArgumentException("Input binary string cannot be null or empty.");
    }

    if (!Regex.IsMatch(binaryString, @"^[01]+$"))
    {
        throw new ArgumentException("Input binary string must only contain '0' and '1' characters.");
    }

    // Check for equal number of 0s and 1s
    int countOfZeros = 0;
    int countOfOnes = 0;
    foreach (char c in binaryString)
    {
        if (c == '0')
        {
            countOfZeros++;
        }
        else if (c == '1')
        {
            countOfOnes++;
        }
    }

    if (countOfZeros != countOfOnes)
    {
        return false;
    }

    // Check for prefix condition
    //It iterates through the string and keeps track of the difference between the number of '1's and '0's seen so far.
    //It returns false if the difference becomes negative at any point, indicating a violation of the prefix condition.
    int balance = 0;
    foreach (char c in binaryString)
    {
        if (c == '1')
        {
            balance++;
        }
        else if (c == '0')
        {
            balance--;
        }

        if (balance < 0)
        {
            return false;
        }
    }

    return true;
}
