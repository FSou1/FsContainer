# FsContainer [![NuGet Version](http://img.shields.io/nuget/v/Fs.Container.svg?style=flat)](https://www.nuget.org/packages/Fs.Container/) 
Yet another IoC container

``` ini

BenchmarkDotNet=v0.10.8, OS=Windows 10 Redstone 1 (10.0.14393)
Processor=Intel Core i5-4460 CPU 3.20GHz (Haswell), ProcessorCount=4
Frequency=3117781 Hz, Resolution=320.7409 ns, Timer=TSC
dotnet cli version=1.0.3
  [Host]    : .NET Core 4.6.25009.03, 64bit RyuJIT
  RyuJitX64 : .NET Core 4.6.25009.03, 64bit RyuJIT

Job=RyuJitX64  Jit=RyuJit  Platform=X64  

```
## Transient
 |         Method |         Mean |       Error |      StdDev | Scaled | ScaledSD |  Gen 0 |  Gen 1 |  Gen 2 | Allocated |
 |--------------- |-------------:|------------:|------------:|-------:|---------:|-------:|-------:|-------:|----------:|
 |         Direct |     5.121 ns |   0.1924 ns |   0.2880 ns |   1.00 |     0.00 | 0.0076 |      - |      - |      24 B |
 |    LightInject |    27.015 ns |   0.7784 ns |   0.6500 ns |   5.29 |     0.31 | 0.0076 |      - |      - |      24 B |
 | SimpleInjector |    41.523 ns |   0.9198 ns |   0.8604 ns |   8.13 |     0.47 | 0.0076 |      - |      - |      24 B |
 |     AspNetCore |    54.809 ns |   0.9170 ns |   0.8577 ns |  10.73 |     0.60 | 0.0075 |      - |      - |      24 B |
 |    FsContainer |   570.093 ns |  10.7539 ns |  11.0434 ns | 111.65 |     6.39 | 0.1040 |      - |      - |     327 B |
 |        Autofac |   587.759 ns |  11.7262 ns |  22.0247 ns | 115.11 |     7.55 | 0.2384 |      - |      - |     750 B |
 |   StructureMap |   785.454 ns |   6.2994 ns |   5.5843 ns | 153.83 |     8.38 | 0.3271 |      - |      - |    1029 B |
 |        Ninject | 4,270.979 ns | 151.3714 ns | 434.3130 ns | 836.46 |    96.04 | 0.6412 | 0.1567 | 0.0003 |    2023 B |
## Singleton
 |         Method |         Mean |      Error |     StdDev | Scaled | ScaledSD |  Gen 0 | Allocated |
 |--------------- |-------------:|-----------:|-----------:|-------:|---------:|-------:|----------:|
 |         Direct |     3.791 ns |  0.0497 ns |  0.0465 ns |   1.00 |     0.00 |      - |       0 B |
 |    LightInject |    24.347 ns |  0.1775 ns |  0.1660 ns |   6.42 |     0.09 |      - |       0 B |
 | SimpleInjector |    38.942 ns |  0.2195 ns |  0.2054 ns |  10.27 |     0.13 |      - |       0 B |
 |     AspNetCore |    48.072 ns |  0.2724 ns |  0.2548 ns |  12.68 |     0.16 |      - |       0 B |
 |    FsContainer |   216.559 ns |  4.3910 ns |  6.5722 ns |  57.13 |     1.84 | 0.0634 |     199 B |
 |        Autofac |   460.172 ns | 11.3192 ns | 10.5880 ns | 121.40 |     3.06 | 0.2031 |     638 B |
 |   StructureMap |   660.678 ns | 13.2897 ns | 13.6475 ns | 174.30 |     4.06 | 0.3557 |    1117 B |
 |        Ninject | 1,489.907 ns | 11.6174 ns |  9.0701 ns | 393.07 |     5.20 | 0.3376 |    1061 B |
## Combined
 |         Method |         Mean |       Error |      StdDev | Scaled | ScaledSD |  Gen 0 |  Gen 1 |  Gen 2 | Allocated |
 |--------------- |-------------:|------------:|------------:|-------:|---------:|-------:|-------:|-------:|----------:|
 |         Direct |     15.53 ns |   0.3919 ns |   0.3474 ns |   1.00 |     0.00 | 0.0178 |      - |      - |      56 B |
 |    LightInject |     37.13 ns |   0.8536 ns |   0.7984 ns |   2.39 |     0.07 | 0.0178 |      - |      - |      56 B |
 | SimpleInjector |     52.82 ns |   0.6061 ns |   0.5669 ns |   3.40 |     0.08 | 0.0178 |      - |      - |      56 B |
 |     AspNetCore |     76.91 ns |   1.2817 ns |   1.1989 ns |   4.96 |     0.13 | 0.0178 |      - |      - |      56 B |
 |    FsContainer |  1,383.39 ns |   6.3612 ns |   5.6390 ns |  89.15 |     1.93 | 0.2460 |      - |      - |     774 B |
 |        Autofac |  1,650.67 ns |   8.2541 ns |   7.7209 ns | 106.37 |     2.32 | 0.5741 |      - |      - |    1803 B |
 |   StructureMap |  1,961.42 ns |  38.8872 ns |  44.7825 ns | 126.39 |     3.90 | 0.6294 |      - |      - |    1978 B |
 |        Ninject | 10,659.21 ns | 261.9340 ns | 755.7393 ns | 686.88 |    50.62 | 1.7859 | 0.4425 | 0.0006 |    5650 B |
