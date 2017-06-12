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
## Transient
 |         Method |         Mean |       Error |      StdDev |   Scaled | ScaledSD |  Gen 0 |  Gen 1 |  Gen 2 | Allocated |
 |--------------- |-------------:|------------:|------------:|---------:|---------:|-------:|-------:|-------:|----------:|
 |         Direct |     3.835 ns |   0.0532 ns |   0.0498 ns |     1.00 |     0.00 | 0.0076 |      - |      - |      24 B |
 |    LightInject |    27.398 ns |   0.1956 ns |   0.1829 ns |     7.15 |     0.10 | 0.0076 |      - |      - |      24 B |
 | SimpleInjector |    37.023 ns |   0.2075 ns |   0.1941 ns |     9.66 |     0.13 | 0.0076 |      - |      - |      24 B |
 |     AspNetCore |    50.435 ns |   0.1997 ns |   0.1770 ns |    13.15 |     0.17 | 0.0076 |      - |      - |      24 B |
 |        Autofac |   637.336 ns |   2.6765 ns |   2.5036 ns |   166.22 |     2.18 | 0.2384 |      - |      - |     750 B |
 |   StructureMap |   729.164 ns |   7.3893 ns |   6.9120 ns |   190.17 |     2.96 | 0.3271 |      - |      - |    1029 B |
 |    FsContainer | 1,177.012 ns |   5.0281 ns |   4.7033 ns |   306.97 |     4.03 | 0.2041 |      - |      - |     646 B |
 |        Ninject | 5,251.293 ns | 104.3953 ns | 252.1264 ns | 1,369.54 |    67.51 | 0.6411 | 0.1528 | 0.0002 |    2037 B |
## Singleton
 |         Method |         Mean |      Error |     StdDev | Scaled | ScaledSD |  Gen 0 | Allocated |
 |--------------- |-------------:|-----------:|-----------:|-------:|---------:|-------:|----------:|
 |         Direct |     3.499 ns |  0.0343 ns |  0.0321 ns |   1.00 |     0.00 |      - |       0 B |
 |    LightInject |    25.647 ns |  0.1364 ns |  0.1276 ns |   7.33 |     0.07 |      - |       0 B |
 | SimpleInjector |    35.134 ns |  0.1211 ns |  0.1133 ns |  10.04 |     0.09 |      - |       0 B |
 |     AspNetCore |    46.717 ns |  0.3129 ns |  0.2927 ns |  13.35 |     0.14 |      - |       0 B |
 |    FsContainer |   294.476 ns |  1.1687 ns |  1.0360 ns |  84.17 |     0.80 | 0.0634 |     199 B |
 |        Autofac |   440.683 ns |  4.5419 ns |  4.2485 ns | 125.97 |     1.62 | 0.2031 |     638 B |
 |   StructureMap |   503.629 ns |  3.7314 ns |  3.3078 ns | 143.96 |     1.57 | 0.3557 |    1117 B |
 |        Ninject | 1,484.866 ns | 19.3405 ns | 18.0911 ns | 424.44 |     6.25 | 0.3376 |    1061 B |
## Combined
 |         Method |         Mean |       Error |      StdDev | Scaled | ScaledSD |  Gen 0 |  Gen 1 |  Gen 2 | Allocated |
 |--------------- |-------------:|------------:|------------:|-------:|---------:|-------:|-------:|-------:|----------:|
 |         Direct |     13.36 ns |   0.1028 ns |   0.0962 ns |   1.00 |     0.00 | 0.0178 |      - |      - |      56 B |
 |    LightInject |     37.54 ns |   0.4746 ns |   0.4207 ns |   2.81 |     0.04 | 0.0178 |      - |      - |      56 B |
 | SimpleInjector |     46.51 ns |   0.3345 ns |   0.2793 ns |   3.48 |     0.03 | 0.0178 |      - |      - |      56 B |
 |     AspNetCore |     70.84 ns |   0.4610 ns |   0.4312 ns |   5.30 |     0.05 | 0.0178 |      - |      - |      56 B |
 |        Autofac |  1,732.72 ns |  17.8146 ns |  16.6638 ns | 129.71 |     1.51 | 0.5741 |      - |      - |    1803 B |
 |   StructureMap |  1,852.80 ns |  11.8812 ns |  11.1137 ns | 138.70 |     1.26 | 0.6294 |      - |      - |    1978 B |
 |    FsContainer |  2,673.78 ns |  21.5895 ns |  20.1948 ns | 200.15 |     2.02 | 0.4845 |      - |      - |    1524 B |
 |        Ninject | 13,163.34 ns | 260.6241 ns | 572.0762 ns | 985.37 |    43.01 | 1.7865 | 0.4425 | 0.0012 |    5661 B |