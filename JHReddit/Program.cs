// See https://aka.ms/new-console-template for more information
using JHReddit;
using JHReddit.DTO;
using Microsoft.Extensions.Configuration;
using System.Runtime.CompilerServices;

public class Program
{
    public static async Task Main(string[] args)
    {
        Console.WriteLine("Hello, Reddit!");
        RedditManager redditManager = new RedditManager();
        redditManager.Start();

        Console.WriteLine("Press any button to stop the service");
        Console.ReadKey();
    }
}