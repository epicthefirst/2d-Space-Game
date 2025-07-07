using System;

public class RandomExample
{
    public static void Main(string[] args)
    {
        int seed = 12345; // Step 1: Choose a seed
        Random random = new Random(seed); // Step 2: Initialize the RNG with the seed

        // Step 3: Generate and print random numbers
        for (int i = 0; i < 10; i++)
        {
            Console.WriteLine(random.Next(1, 101)); // Random number between 1 and 100
        }

        // Resetting with the same seed will produce the same sequence
        Random resetRandom = new Random(seed);
        Console.WriteLine("Reset with the same seed:");
        for (int i = 0; i < 10; i++)
        {
            Console.WriteLine(resetRandom.Next(1, 101)); // Same output as above
        }
    }
}
