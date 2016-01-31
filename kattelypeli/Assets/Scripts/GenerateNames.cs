using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System;

public class GenerateNames : MonoBehaviour
{

    public List<string> nameList = new List<string>();
    public List<string> surnameList = new List<string>();
    public List<string> firstTitleList = new List<string>();
    public List<string> secondTitleList = new List<string>();
    public List<string> quotes = new List<string>();


    // Use this for initialization
    void Awake()
    {
        Names();
        Surnames();
        FirstTitle();
        SecondTitle();
        Quotes();
    }


    void Names()
    {
        try
        {
            using (StreamReader sr = new StreamReader(Path.Combine(Application.dataPath, "Names/names.txt"), Encoding.GetEncoding("ISO-8859-1")))
            {
                string line;
                while ((line = sr.ReadLine()) != null)
                {
                    nameList.Add(line);
                }
            }
        }
        catch (Exception e)
        {
            // Let the user know what went wrong.
            Console.WriteLine("The file could not be read:");
            Console.WriteLine(e.Message);
        }
    }


    void Surnames()
    {
        try
        {
            using (StreamReader sr = new StreamReader(Path.Combine(Application.dataPath, "Names/Surnames.txt"), Encoding.GetEncoding("ISO-8859-1")))
            {
                string line;
                while ((line = sr.ReadLine()) != null)
                {
                    surnameList.Add(line);
                }
            }
        }
        catch (Exception e)
        {
            // Let the user know what went wrong.
            Console.WriteLine("The file could not be read:");
            Console.WriteLine(e.Message);
        }
    }


    void FirstTitle()
    {
        try
        {
            using (StreamReader sr = new StreamReader(Path.Combine(Application.dataPath, "Names/FirstTitle.txt"), Encoding.GetEncoding("ISO-8859-1")))
            {
                string line;
                while ((line = sr.ReadLine()) != null)
                {
                    firstTitleList.Add(line);
                }
            }
        }
        catch (Exception e)
        {
            // Let the user know what went wrong.
            Console.WriteLine("The file could not be read:");
            Console.WriteLine(e.Message);
        }
    }


    void SecondTitle()
    {
        try
        {
            using (StreamReader sr = new StreamReader(Path.Combine(Application.dataPath, "Names/SecondTitle.txt"), Encoding.GetEncoding("ISO-8859-1")))
            {
                string line;
                while ((line = sr.ReadLine()) != null)
                {
                    secondTitleList.Add(line);
                }
            }
        }
        catch (Exception e)
        {
            // Let the user know what went wrong.
            Console.WriteLine("The file could not be read:");
            Console.WriteLine(e.Message);
        }
    }

    void Quotes()
    {
        try
        {
            using (StreamReader sr = new StreamReader(Path.Combine(Application.dataPath, "Names/Quotes.txt"), Encoding.GetEncoding("ISO-8859-1")))
            {
                string line;
                while ((line = sr.ReadLine()) != null)
                {
                    quotes.Add(line);
                }
            }
        }
        catch (Exception e)
        {
            // Let the user know what went wrong.
            Console.WriteLine("The file could not be read:");
            Console.WriteLine(e.Message);
        }
    }
}


