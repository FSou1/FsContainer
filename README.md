# FsContainer [![NuGet Version](http://img.shields.io/nuget/v/Fs.Container.svg?style=flat)](https://www.nuget.org/packages/Fs.Container/) 
Yet another IoC container

``` ini

BenchmarkDotNet=v0.10.8, OS=Windows 8.1 (6.3.9600)
Processor=Intel Core i5-3550 CPU 3.30GHz (Ivy Bridge), ProcessorCount=4
Frequency=3319944 Hz, Resolution=301.2099 ns, Timer=TSC
dotnet cli version=1.0.3
  [Host]    : .NET Core 4.6.25009.03, 64bit RyuJIT
  RyuJitX64 : .NET Core 4.6.25009.03, 64bit RyuJIT

Job=RyuJitX64  Jit=RyuJit  Platform=X64  

```
## Transient
 |         Method |         Mean |       Error |      StdDev |   Scaled | ScaledSD |  Gen 0 |  Gen 1 |  Gen 2 | Allocated |
 |--------------- |-------------:|------------:|------------:|---------:|---------:|-------:|-------:|-------:|----------:|
 |         Direct |     3.767 ns |   0.0110 ns |   0.0086 ns |     1.00 |     0.00 | 0.0076 |      - |      - |      24 B |
 |    LightInject |    26.467 ns |   0.1348 ns |   0.1195 ns |     7.03 |     0.03 | 0.0076 |      - |      - |      24 B |
 | SimpleInjector |    37.053 ns |   0.3118 ns |   0.2917 ns |     9.84 |     0.08 | 0.0076 |      - |      - |      24 B |
 |     AspNetCore |    50.210 ns |   0.1033 ns |   0.0966 ns |    13.33 |     0.04 | 0.0076 |      - |      - |      24 B |
 |    FsContainer |   394.685 ns |   3.8606 ns |   3.6112 ns |   104.77 |     0.95 | 0.1040 |      - |      - |     327 B |
 |        Autofac |   620.254 ns |   2.4735 ns |   2.3137 ns |   164.64 |     0.69 | 0.2384 |      - |      - |     750 B |
 |   StructureMap |   730.726 ns |   9.8796 ns |   9.2414 ns |   193.97 |     2.41 | 0.3271 |      - |      - |    1029 B |
 |        Ninject | 5,112.882 ns | 102.2952 ns | 271.2723 ns | 1,357.19 |    71.63 | 0.6410 | 0.1529 | 0.0002 |    2029 B |
## Singleton
 |         Method |         Mean |      Error |     StdDev | Scaled | ScaledSD |  Gen 0 | Allocated |
 |--------------- |-------------:|-----------:|-----------:|-------:|---------:|-------:|----------:|
 |         Direct |     3.493 ns |  0.0185 ns |  0.0164 ns |   1.00 |     0.00 |      - |       0 B |
 |    LightInject |    25.506 ns |  0.0708 ns |  0.0662 ns |   7.30 |     0.04 |      - |       0 B |
 | SimpleInjector |    34.997 ns |  0.1396 ns |  0.1238 ns |  10.02 |     0.06 |      - |       0 B |
 |     AspNetCore |    46.608 ns |  0.1178 ns |  0.0983 ns |  13.34 |     0.07 |      - |       0 B |
 |    FsContainer |   188.486 ns |  1.7664 ns |  1.6523 ns |  53.97 |     0.52 | 0.0634 |     199 B |
 |        Autofac |   433.188 ns |  1.3005 ns |  1.1528 ns | 124.03 |     0.64 | 0.2031 |     638 B |
 |   StructureMap |   503.597 ns |  5.4962 ns |  5.1412 ns | 144.19 |     1.56 | 0.3557 |    1117 B |
 |        Ninject | 1,545.695 ns | 11.8883 ns | 11.1203 ns | 442.56 |     3.67 | 0.3376 |    1061 B |
## Combined
 |         Method |         Mean |       Error |      StdDev | Scaled | ScaledSD |  Gen 0 |  Gen 1 |  Gen 2 | Allocated |
 |--------------- |-------------:|------------:|------------:|-------:|---------:|-------:|-------:|-------:|----------:|
 |         Direct |     13.24 ns |   0.0836 ns |   0.0698 ns |   1.00 |     0.00 | 0.0178 |      - |      - |      56 B |
 |    LightInject |     37.39 ns |   0.0570 ns |   0.0533 ns |   2.82 |     0.01 | 0.0178 |      - |      - |      56 B |
 | SimpleInjector |     46.22 ns |   0.2327 ns |   0.2063 ns |   3.49 |     0.02 | 0.0178 |      - |      - |      56 B |
 |     AspNetCore |     70.53 ns |   0.2885 ns |   0.2698 ns |   5.33 |     0.03 | 0.0178 |      - |      - |      56 B |
 |    FsContainer |  1,038.13 ns |  17.1037 ns |  15.9988 ns |  78.41 |     1.23 | 0.2327 |      - |      - |     734 B |
 |        Autofac |  1,551.33 ns |   3.6293 ns |   3.2173 ns | 117.17 |     0.64 | 0.5741 |      - |      - |    1803 B |
 |   StructureMap |  1,944.35 ns |   1.8665 ns |   1.7459 ns | 146.85 |     0.76 | 0.6294 |      - |      - |    1978 B |
 |        Ninject | 13,139.70 ns | 260.8754 ns | 508.8174 ns | 992.43 |    38.35 | 1.7857 | 0.4425 | 0.0004 |    5682 B |
