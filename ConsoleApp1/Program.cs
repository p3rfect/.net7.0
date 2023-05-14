using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;
using Microsoft.AspNetCore.Mvc;
using WebApplication4.Controllers;
using WebApplication4.Models.Interfaces;
using WebApplication4.Models.Services;

internal class Program
{
    private static void Main(string[] args)
    {
        BenchmarkRunner.Run<Test>();
    }
}

public class Test
{
    
    [Benchmark(Baseline=true)]
    [IterationCount(10)]
    public void Test1()
    {
        IUserService userService = new UserService();
        IEmailService emailService = new EmailService();
        AccountController accountController=new(userService, emailService);
        for(int i = 0; i < 2000; i++)
        {
            //_ = ThreadPool.QueueUserWorkItem((worker) =>
            //{
                Task.Run(() => accountController.Token("adokuchaeva11@gmail.com", "aaaaaaaaaa"));
            Console.WriteLine(i);
            //});
        }
        
    }
}