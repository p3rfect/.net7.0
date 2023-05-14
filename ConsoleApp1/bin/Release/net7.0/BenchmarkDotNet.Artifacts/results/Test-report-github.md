``` ini

BenchmarkDotNet=v0.13.5, OS=Windows 10 (10.0.19044.2965/21H2/November2021Update)
Intel Core i5-8265U CPU 1.60GHz (Whiskey Lake), 1 CPU, 8 logical and 4 physical cores
.NET SDK=7.0.102
  [Host]     : .NET 7.0.2 (7.0.222.60605), X64 RyuJIT AVX2
  Job-MNDDMO : .NET 7.0.2 (7.0.222.60605), X64 RyuJIT AVX2

IterationCount=10  

```
|                Method |       Mean |     Error |   StdDev | Ratio | RatioSD |
|---------------------- |-----------:|----------:|---------:|------:|--------:|
|      GetUserToken2000 |   432.4 ms |   5.58 ms |  3.69 ms |  1.00 |    0.00 |
| GetAllSpecialties2000 | 6,235.8 ms | 110.81 ms | 57.96 ms | 14.39 |    0.14 |
