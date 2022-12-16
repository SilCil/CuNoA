using System;
using System.Collections.Generic;
using System.Linq;
using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.Optimization;

namespace CustomizableAnalysisLibrary.Nodes
{
    [Obsolete]
    // [Node("フィッティング/任意関数/ネルダー・ミード法")]
    public class FittingNelderMeadSimplex : ICalculationNode, IOptionNode
    {
        private const string CodeTemplate = @"
using System;
using System.Collections.Generic;
using CustomizableAnalysisLibrary;

public static class Code
{
    public static double Evaluate(double x, IReadOnlyList<double> a) => $Code;
}
";

        public int IndexX { get; set; } = 0;
        public int IndexY { get; set; } = 1;
        public bool OutputCurve { get; set; } = true;

        public int ParameterCount { get; set; } = 1;
        public double[] Parameters { get; set; } = { 1.0 };

        public string FunctionCode { get; set; } = "Math.Exp(-a[0]*x)";
        public int MaximumIterations { get; set; } = 1000;
        public bool AllowNotConverged { get; set; } = false;

        private string codeKey = default;
        private Func<double, IReadOnlyList<double>, double> evaluate = default;

        public IEnumerable<(string label, Value)> GetOptions()
        {
            yield return ("xの列番号", new Value(IndexX));
            yield return ("yの列番号", new Value(IndexY));
            yield return ("x, y, f(x)を出力（Noの場合、パラメータを出力）", new Value(OutputCurve));

            yield return ("パラメータの数", new Value(ParameterCount));
            var oldParameters = Parameters;
            Parameters = new double[ParameterCount];
            if (oldParameters != null) Array.Copy(oldParameters, Parameters, oldParameters.Length);
            for (int i = 0; i < Parameters.Length; ++i)
            {
                yield return ($"パラメータa[{i}]", new Value(Parameters[i]));
            }

            yield return ("フィッティング関数", new Value(FunctionCode));
            yield return ("最大反復数", new Value(MaximumIterations));
            yield return ("収束しない場合、初期値を用いて出力", new Value(AllowNotConverged));
        }

        public void SetOptions(params Value[] options)
        {
            IndexX = options[0].ToInt();
            IndexY = options[1].ToInt();
            OutputCurve = options[2].ToBool();

            ParameterCount = Math.Max(0, options[3].ToInt());
            Parameters = new double[ParameterCount];
            int parameterCount = 0;
            for (int i = 0; i < Parameters.Length; ++i)
            {
                var option = options[parameterCount + 4];
                var isDouble = option.ValueType == ValueType.Double;
                Parameters[i] = (isDouble) ? option.DoubleValue : 0.0;

                if (i == parameterCount && isDouble)
                {
                    ++parameterCount;
                }
            }

            FunctionCode = options[options.Length - 3].ToString();
            MaximumIterations = options[options.Length - 2].ToInt();
            AllowNotConverged = options[options.Length - 1].ToBool();
        }

        private string GenerateKey() => $"{nameof(Nodes)}{nameof(FittingNelderMeadSimplex)}:{FunctionCode}";

        private double EvaluateFunc(double x, IReadOnlyList<double> parameters)
        {
            var key = GenerateKey();
            if (key == codeKey && evaluate != null) return evaluate.Invoke(x, parameters);

            codeKey = key;
            evaluate = null;

            var source = CodeTemplate.Replace("$Code", FunctionCode);
            var assembly = Utility.LoadFromSource(codeKey, source);

            var codeType = assembly.GetType("Code");
            var evaluateInfo = codeType.GetMethod("Evaluate");
            evaluate = (Func<double, IReadOnlyList<double>, double>)Delegate.CreateDelegate(typeof(Func<double, IReadOnlyList<double>, double>), evaluateInfo);

            return evaluate.Invoke(x, parameters);
        }

        private double SumSquared(Vector<double> args, double[] dataX, double[] dataY)
        {
            var argsArray = args.ToArray();
            var sum = 0.0;
            for (int i = 0; i < dataX.Length && i < dataY.Length; ++i)
            {
                var result = EvaluateFunc(dataX[i], argsArray);
                sum += (dataY[i] - result) * (dataY[i] - result);
            }
            return sum;
        }

        public Table Run(Table data)
        {
            var dataX = data.GetColumn(IndexX).ToDoubleArray();
            var dataY = data.GetColumn(IndexY).ToDoubleArray();

            var func = (Func<Vector<double>, double>)(x => SumSquared(x, dataX, dataY));
            var objFunc = ObjectiveFunction.Value(func);
            var initial = CreateVector.DenseOfEnumerable(Parameters);

            try
            {
                var result = NelderMeadSimplex.Minimum(objFunc, initial, maximumIterations: MaximumIterations);
                Parameters = result.MinimizingPoint.ToArray();
            }
            catch (MaximumIterationsException mie)
            {
                if (AllowNotConverged == false) throw mie;
                Parameters = initial.ToArray();
            }
            
            if (OutputCurve)
            {
                return Table.CreateFromColumns(data.GetColumn(IndexX), data.GetColumn(IndexY), dataX.Select(x => new Value(EvaluateFunc(x, Parameters))));
            }
            else
            {
                return Table.CreateFromRows(Parameters.Select(x => new Value(x)));
            }
        }
    }
}
