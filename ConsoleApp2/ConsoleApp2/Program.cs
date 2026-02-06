
public class Class1
{

    static  void DoExercise1()
    {
        string myName = "Narda";
        Console.WriteLine("Enter Name: ");
        string? name = Console.ReadLine();
        if (name != null && name.ToLower() == myName.ToLower())
        {
            Console.WriteLine($"Welcome {name}, have a nice day");
        }
        else
        {
            Console.WriteLine($"Hello {name}");
        }
    }

    static void DoExercise2()
    {
        int total = 0;
        bool isDone = false;
        while(!isDone)
        {
            Console.Clear();
            Console.WriteLine("Enter A Number: ");
            string? input = Console.ReadLine();
            isDone = input == null || input.ToLower() == "done";
            if (!isDone)
            {
                int value = int.Parse(input);
                total += value;
            }
        }
        Console.WriteLine($"Total: {total}");
    }
    static private void DoExercise3(string[] args)
    {
        // check if the first armunt is -i 
        // if argument is a -i , check 
        if (args.Length < 2)
        {
            Console.WriteLine("No Arguments Given! ");
            return;
        }
        if (args[0] == "-i")
        {
            string dir = args[1];
            string filePath = args[1]; 
            if(File.Exists(filePath))
            {
                Console.WriteLine("Reading file: ");
                StreamReader fileStream = new StreamReader(filePath);
                while (fileStream.Peek() > 0)
                { 
                    Console.WriteLine(fileStream.ReadLine());
                }
                fileStream.Close();
            }
            else
            {
                Console.WriteLine($"FilePath does not exist: {filePath} ");
            }
        }
        
    }
    
    public static void Main(string[] args)
    {
        // DoExercise1();
        // DoExercise2();
        DoExercise3(args);

    }
}