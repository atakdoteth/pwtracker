using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace PasswordTracker
{
    class Program
    {
        static void Main(string[] args)
        {
            //Defining text path and array that contains them

            string entrancePasswordLoc = @"pw.txt";
            string textPathsLocation = @"location.txt";
            string[] textPathsArray;
            string[] pw;
            string[] keyRTextFile;
            string[] passwordTextFile;
            bool filePwFileExist = File.Exists(entrancePasswordLoc);
            bool textPathArrayExist = File.Exists(textPathsLocation);
            //////////////////////////////////////////////////////////////////////////////////////////////////////////
            //LOCATION.TXT

            if (textPathArrayExist == true)//checking if location.txt exists
            {
                File.SetAttributes(textPathsLocation, FileAttributes.Normal);//visible
                textPathsArray = File.ReadAllLines(textPathsLocation);
                File.SetAttributes(textPathsLocation, FileAttributes.Hidden);//hidden
                Console.WriteLine("Your path file is found, checking password file...");
            }
            else
            {
                Console.WriteLine("\nFirst time using the program or changed the location of the program, need to reassign the password!\n");
                using (FileStream fs = File.Create("location.txt")) { };//creating location.txt
                Console.WriteLine("You need to set pathway for you text files directly!");
                Console.Write("Pathway to your password.txt!:");
                string passwordPath = Console.ReadLine();
                Console.Write("Pathway to your retrieval key.txt!:");
                string retrieveKeyPath = Console.ReadLine();

                using (StreamWriter w = new StreamWriter(textPathsLocation))
                {
                    w.WriteLine(passwordPath);//writing pw path
                    w.WriteLine(retrieveKeyPath);//writing key path
                    File.SetAttributes(textPathsLocation, FileAttributes.Hidden);//hidden
                }
            }

            //////////////////////////////////////////////////////////////////////////////////////////////////////////
            ///PW.TXT

            if (filePwFileExist == true)//checks if the file exist (if true)
            {
                textPathsArray = File.ReadAllLines(textPathsLocation);
                File.SetAttributes(entrancePasswordLoc, FileAttributes.Normal);//making the file visible to get info
                pw = File.ReadAllLines(entrancePasswordLoc);//gets the pw
                File.SetAttributes(entrancePasswordLoc, FileAttributes.Hidden);//hiding it again
                Console.WriteLine("\nYour password file is found, moving to password checking process...\n");
                textPathsArray = File.ReadAllLines(textPathsLocation);//pw and retrieval key paths
                passwordTextFile = File.ReadAllLines(textPathsArray[0]);//getting text info from pw.txt file
                keyRTextFile = File.ReadAllLines(textPathsArray[1]);//getting text info from retrievalkey.txt file
            }
            else//if false
            {
                Console.WriteLine("\nFirst time using the program or changed the location of the program, need to reassign the password!\n");
                using (FileStream fs = File.Create("pw.txt")) { };//creating pw.txt
            pwchanger:
                Console.Write("New Password:");
                string pw1 = Console.ReadLine();
                Console.Write("New Password Again:");
                string pw2 = Console.ReadLine();
                if (pw1 == pw2)//checking if the new password is written correctly
                {
                    using (StreamWriter w = new StreamWriter(entrancePasswordLoc))
                    {
                        File.SetAttributes(entrancePasswordLoc, FileAttributes.Normal);//visible
                        w.WriteLine(pw1);//writing the password into pw.txt
                        File.SetAttributes(entrancePasswordLoc, FileAttributes.Hidden);
                    }
                    File.SetAttributes(entrancePasswordLoc, FileAttributes.Normal);//making the file visible to get the info
                    pw = File.ReadAllLines(entrancePasswordLoc);//getting the new pw
                    File.SetAttributes(entrancePasswordLoc, FileAttributes.Hidden);//hiding it again
                    Console.WriteLine("Password is changed to:" + pw1);//showing that pw is changed to new pw
                    textPathsArray = File.ReadAllLines(textPathsLocation);//pw and retrieval key paths
                    passwordTextFile = File.ReadAllLines(textPathsArray[0]);//getting text info from pw.txt file
                    keyRTextFile = File.ReadAllLines(textPathsArray[1]);//getting text info from retrievalkey.txt file

                }
                else//if the passwords dont match, try again
                {
                    Console.WriteLine("Your password didn't match try again!");
                    goto pwchanger;
                }
            }
            //////////////////////////////////////////////////////////////////////////////////////////////////////////
            ///START OF THE ACTUAL CODE AFTER CONFIRMING YOU ARE THE OWNER
            //DO NOT REMOVE THIS

            if (passwordCheck(pw[0]) == true)//password checker to enter the program
            {
                Console.WriteLine("PW is correct, moving to the program!");
                Console.WriteLine();

            mainmenu://as the title says, main menu for chosing what to do

                //Showing the menu
                Console.WriteLine("\nWhat Do You Want To Do?");
                Console.WriteLine("1-Changing Path Of The Text File");
                Console.WriteLine("2-Checking How Many Passwords Are Stored");
                Console.WriteLine("3-Password Storing / Retrieving");
                Console.WriteLine("4-Password Retrieval Key Storing / Retrieving");
                Console.WriteLine("5-Change App Password");
                Console.Write("Enter the number:");
                int choice = int.Parse(Console.ReadLine());

                switch (choice)
                {
                    case 1://Changing path (need to write the new path inside a .txt to work with that later too)
                    PR://to start over
                        Console.Write("(P)assword Path or (R)etrieval Key Path(P/R):");
                        string PRChoice = Console.ReadLine().ToLower();
                        textPathsArray = File.ReadAllLines(textPathsLocation);
                        if (PRChoice == "p")
                        {
                            Console.Write("Write the whole path exactly otherwise it wont work!:");
                            string input = Console.ReadLine();
                            string keyText = textPathsArray[1];
                            textPathsArray[0] = @input;//new .txt file path
                            using(StreamWriter w = new StreamWriter(textPathsLocation))
                            {
                                w.WriteLine(input);//new password path
                                w.WriteLine(textPathsArray[1]);//writing the same retrieval key
                            }
                            Console.WriteLine("New path is:" + textPathsArray[0]);//Showing new path

                        }
                        else if (PRChoice == "r")
                        {
                            Console.Write("Write the whole path exactly otherwise it wont work!:");
                            string input = Console.ReadLine();
                            string pwText = textPathsArray[0];
                            textPathsArray[1] = @input;//new .txt file path
                            using(StreamWriter w = new StreamWriter(textPathsLocation))
                            {
                                w.WriteLine(textPathsArray[0]);//writing the same password path
                                w.WriteLine(input);//writing the same retrieval key
                            }
                            Console.WriteLine("New path is:" + textPathsArray[1]);//showing new path
                        }
                        else
                        {
                            goto PR;
                        }
                        break;

                    case 2://Number of passwords inside the .txt file
                        numberOfPw(textPathsArray[0], passwordTextFile);
                        break;

                    case 3://password storage and retrieval
                    PWDecision:
                        Console.Write("Do you want to store or retrieve password(S/R):");
                        string decision = Console.ReadLine().ToLower();//turns your input to lower for easier coding

                        //if statements for telling the program if it should execute storing or retrieving
                        if (decision == "s")//store function
                        {
                            Console.Write("How many password are you going to register now?:");
                            int regAmount = int.Parse(Console.ReadLine());
                            for (int i = 1; i <= regAmount; i++)//for loop for counting how many registers
                            {
                                Console.WriteLine("Password registery no:{0}", i);
                                Store(textPathsArray[0], passwordTextFile);
                            }

                        }
                        else if (decision == "r")//retrieve function
                        {
                            Retrieve(textPathsArray[0], passwordTextFile);//using retrieve function
                        }
                        else
                        {
                            Console.WriteLine("Wrong input try again!");
                            goto PWDecision;//wrong input so go back to decision phase to do again
                        }
                        break;

                    case 4://Retrieval key store and retrieve
                    KeyRDecision:
                        Console.Write("Do you want to store or retrieve key(S/R):");
                        string keyD = Console.ReadLine().ToLower();//turns your input to lower for easier coding

                        if (keyD == "s")//storing new data
                        {
                            StoreKey(textPathsArray[1], keyRTextFile);
                        }
                        else if (keyD == "r")//getting data
                        {
                            Retrieve(textPathsArray[1], keyRTextFile);
                        }
                        else//wrong input
                        {
                            Console.WriteLine("Wrong input try again!");
                            goto KeyRDecision;
                        }

                        break;
                    case 5://App pw change
                        File.SetAttributes(entrancePasswordLoc, FileAttributes.Normal);
                    pwchanger:
                        Console.Write("New Password:");
                        string pw1 = Console.ReadLine();
                        Console.Write("New Password Again:");
                        string pw2 = Console.ReadLine();
                        if (pw1 == pw2)
                        {
                            using (StreamWriter w = new StreamWriter(entrancePasswordLoc))
                            {
                                w.WriteLine();
                                File.SetAttributes(entrancePasswordLoc, FileAttributes.Hidden);//hiding
                            }
                            Console.WriteLine("Password is changed to:" + pw1);

                        }
                        else
                        {
                            Console.WriteLine("Your password didn't match try again!");
                            goto pwchanger;
                        }

                        break;

                    default://If non of the above
                        Console.WriteLine("Wrong input try again!");
                        goto mainmenu;
                }

                //****************************************************************
                //PROGRAM ENDED HERE, ASKING FOR RETURNING MAIN MENU


                bool gotoMainMenu = mainMenuBool();//returning if true by using the created function below

                if (gotoMainMenu)//if true
                {
                    goto mainmenu;
                }
                else//if false
                {
                    Console.WriteLine("Your decision, have a great day!");
                    Console.ReadLine();
                }
            }
            else
            {
                Console.WriteLine("Closing program!");
                Console.ReadLine();

            }




        }
        //FUNCTIONS
        static void Retrieve(string textPath, string[] textFile)
        {
            //User input
            Console.Write("What are you looking for?:");
            string searchInput = Console.ReadLine().ToLower();

            int num = Array.IndexOf(textFile, searchInput);
            string[] output = Array.FindAll(textFile, e => e.Contains(searchInput));
            for (int i = 0; i < output.Length; i++)
            {
                Console.WriteLine(output[i]);
            }

        }

        static void Store(string textPath, string[] textFile)
        {
            //defining and writing variables to write to file
            Console.Write("ID please:");
            string id = Console.ReadLine();
            Console.Write("Password please(careful about capitals and lowers):");
            string pw = Console.ReadLine();
            Console.Write("Site/App name please:");
            string siteName = Console.ReadLine().ToLower();

            using (StreamWriter writer = new StreamWriter(textPath, true))//writing the lines, (defining it true makes it not overwrite)
            {
                writer.WriteLine("{0}, {1}, {2}", id, pw, siteName);//writing
                writer.Close();//not overwriting
            }
        }

        static void StoreKey(string textPath, string[] textFile)
        {
            //defining vars
            Console.Write("ID Please:");
            string id = Console.ReadLine();
            Console.Write("Password Retrieval key:");
            string key = Console.ReadLine();
            Console.Write("Site/App name please:");
            string name = Console.ReadLine().ToLower();

            using (StreamWriter w = new StreamWriter(textPath, true))
            {
                w.WriteLine("{0}, {1}, {2}", id, key, name);
                w.Close();
            }
        }

        static void numberOfPw(string textPath, string[] textFile)
        {
            textFile = File.ReadAllLines(textPath);
            Console.WriteLine("There are {0} password stored right now in the database.", textFile.Length);
        }

        static bool mainMenuBool()
        {

        startagain:
            bool isTrue = false;
            //Asking for doing again
            Console.Write("Do you want to return to main menu?(Y/N):");
            string again = Console.ReadLine().ToLower();

            //Checking if they want to do it again

            if (again == "yes" || again == "y")//Answer = yes to returning back to main menu
            {
                isTrue = true;
            }
            else if (again == "no" || again == "n")
            {
                isTrue = false;
            }
            else// wrong answer try again
            {
                Console.WriteLine("Wrong input try again!");
                goto startagain;
            }


            return isTrue;
        }

        static bool passwordCheck(string realPassword)//PW check function
        {
            Console.Write("Write your password in order to use the program:");
            string pwInput = Console.ReadLine();
            bool isTrue = false;
            if (pwInput == realPassword)
            {
                isTrue = true;
            }
            return isTrue;
        }
    }
    }

