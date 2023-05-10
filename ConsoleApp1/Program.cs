using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;
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
    [IterationCount(2)]
    public void Test1()
    {
        IUserService userService = new UserService();
        IEmailService emailService = new EmailService();
        AccountController accountController=new(userService, emailService);
        Parallel.For(0, 2000, (o1, o2) =>
        {
            ThreadPool.QueueUserWorkItem((worker) =>
            {
                accountController.Token("adokuchaeva11@gmail.com", "aaaaaaaaaa");
                
            });
        });
        
    }
}