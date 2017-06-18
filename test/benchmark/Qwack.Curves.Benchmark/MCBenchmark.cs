using System;
using System.Collections.Generic;
using System.Text;
using BenchmarkDotNet.Attributes;
using Qwack.Paths;
using Qwack.Paths.Payoffs;
using Qwack.Paths.Processes;

namespace Qwack.Curves.Benchmark
{
    [Config(typeof(SolveConfig))]
    public class MCBenchmark
    {

        [Benchmark(Baseline =true)]
        public double FirstCut()
        {
            var engine = new PathEngine(2 << 13);
            engine.AddPathProcess(new Random.MersenneTwister.MersenneTwister64()
            {
                UseNormalInverse = true
            });
            var asset = new ConstantVolSingleAsset
                (
                    startDate: DateTime.Now.Date,
                    expiry: DateTime.Now.Date.AddYears(1),
                    vol: 0.30,
                    spot: 1000,
                    drift: 0.00,
                    numberOfSteps: 100,
                    name: "TestAsset"
                );
            engine.AddPathProcess(asset);
            var payoff = new Put("TestAsset2", 500, DateTime.Now.Date.AddYears(1));
            engine.AddPathProcess(payoff);
            engine.SetupFeatures();
            engine.RunProcess();
            engine.Dispose();
            return payoff.AverageValue;
        }

        [Benchmark()]
        public double ImprovedCut()
        {
            var engine = new FasterPathEngine(2 << 13);
            engine.AddPathProcess(new Random.MersenneTwister.MersenneTwister64()
            {
                UseNormalInverse = true
            });
            var asset = new FasterConstantVolSingleAsset
                (
                    startDate: DateTime.Now.Date,
                    expiry: DateTime.Now.Date.AddYears(1),
                    vol: 0.30,
                    spot: 1000,
                    drift: 0.00,
                    numberOfSteps: 100,
                    name: "TestAsset"
                );
            engine.AddPathProcess(asset);
            var payoff = new Put("TestAsset2", 500, DateTime.Now.Date.AddYears(1));
            engine.AddPathProcess(payoff);
            engine.SetupFeatures();
            engine.RunProcess();
            engine.Dispose();
            return payoff.AverageValue;
        }
    }
}
