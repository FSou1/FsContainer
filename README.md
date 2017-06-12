# FsContainer [![NuGet Version](http://img.shields.io/nuget/v/Fs.Container.svg?style=flat)](https://www.nuget.org/packages/Fs.Container/) 
Yet another IoC container

``` ini

BenchmarkDotNet=v0.10.8, OS=Windows 8.1 (6.3.9600)
Processor=Intel Core i5-3550 CPU 3.30GHz (Ivy Bridge), ProcessorCount=4
Frequency=3319940 Hz, Resolution=301.2103 ns, Timer=TSC
dotnet cli version=1.0.3
  [Host]    : .NET Core 4.6.25009.03, 64bit RyuJIT
  RyuJitX64 : .NET Core 4.6.25009.03, 64bit RyuJIT

Job=RyuJitX64  Jit=RyuJit  Platform=X64  

```
 |           Method |       Mean |     Error |   StdDev |  Gen 0 | Allocated |
 |----------------- |-----------:|----------:|---------:|-------:|----------:|
 | ResolveSingleton |   185.7 ns |  1.425 ns | 1.263 ns | 0.0634 |     199 B |
 | ResolveTransient |   929.3 ns |  5.074 ns | 4.498 ns | 0.2050 |     646 B |
 |  ResolveCombined | 2,072.0 ns | 10.914 ns | 8.521 ns | 0.4845 |    1524 B |