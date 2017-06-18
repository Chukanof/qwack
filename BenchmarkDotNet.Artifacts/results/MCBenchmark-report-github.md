``` ini

BenchmarkDotNet=v0.10.8, OS=Windows 10.0.16215
Processor=Intel Xeon CPU E3-1505M v5 2.80GHz, ProcessorCount=8
Frequency=2742186 Hz, Resolution=364.6726 ns, Timer=TSC
dotnet cli version=1.0.4
  [Host]     : .NET Core 4.6.25211.01, 64bit RyuJIT
  Job-WRHAKV : .NET Core 4.6.25211.01, 64bit RyuJIT

Platform=X64  Runtime=Core  InvocationCount=10  
LaunchCount=2  TargetCount=3  UnrollFactor=5  
WarmupCount=1  

```
 |      Method |     Mean |    Error |    StdDev | Scaled | ScaledSD |      Gen 0 |    Gen 1 |    Gen 2 | Allocated |
 |------------ |---------:|---------:|----------:|-------:|---------:|-----------:|---------:|---------:|----------:|
 |    FirstCut | 67.17 ms | 2.629 ms | 0.9377 ms |   1.00 |     0.00 | 15900.0000 | 500.0000 | 300.0000 |  75.27 MB |
 | ImprovedCut | 68.96 ms | 6.712 ms | 2.3938 ms |   1.03 |     0.03 | 15900.0000 | 500.0000 | 300.0000 |  75.27 MB |
