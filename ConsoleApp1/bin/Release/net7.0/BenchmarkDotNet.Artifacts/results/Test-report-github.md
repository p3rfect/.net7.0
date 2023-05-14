``` ini

BenchmarkDotNet=v0.13.5, OS=Windows 10 (10.0.19044.2965/21H2/November2021Update)
Intel Core i5-8265U CPU 1.60GHz (Whiskey Lake), 1 CPU, 8 logical and 4 physical cores
.NET SDK=7.0.102
  [Host]     : .NET 7.0.2 (7.0.222.60605), X64 RyuJIT AVX2 [AttachedDebugger]
  Job-TMTKTI : .NET 7.0.2 (7.0.222.60605), X64 RyuJIT AVX2

IterationCount=10  

```
| Method |    Mean |   Error |   StdDev | Ratio |
|------- |--------:|--------:|---------:|------:|
|  Test1 | 4.086 s | 1.293 s | 0.8551 s |  1.00 |
