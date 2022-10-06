using System;
using System.Collections.Generic;
using System.Linq;
using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.Optimization;

namespace CustomizableAnalysisLibrary.Nodes
{
    [Node("フィッティング/任意関数の和/レーベンバーグ・マーカート法")]
    public class FittingLevenbergMarquardtMulti : ICalculationNode, IOptionNode
    {
        private const string CodeTemplate = @"
using System;
using System.Collections.Generic;
using MathNet.Numerics.LinearAlgebra;

public static class Code
{
    public static double Evaluate(Vector<double> a, double x) => $Code;
}
";

        private const string DefaultCode = "Math.Exp(-a[0]*x)";

        public int IndexX { get; set; } = 0;
        public int IndexY { get; set; } = 1;
        public bool OutputCurve { get; set; } = true;

        public int ParameterCount { get; set; } = 1;
        public double[] Parameters { get; set; } = { 1.0 };

        public int FunctionCount { get; set; } = 1;
        public string[] FunctionCodes { get; set; } = new string[] { DefaultCode };

        public int MaximumIterations { get; set; } = -1;

        private Dictionary<string, Func<Vector<double>, double, double>> evaluateFunctions = new Dictionary<string, Func<Vector<double>, double, double>>();

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

            yield return ("関数の項数", new Value(FunctionCount));
            var oldCodes = FunctionCodes;
            FunctionCodes = new string[FunctionCount];
            if (oldCodes != null) Array.Copy(oldCodes, FunctionCodes, oldCodes.Length);
            for (int i = 0; i < FunctionCodes.Length; ++i)
            {
                yield return ($"項{i + 1}", new Value(FunctionCodes[i]));
            }

            yield return ("最大反復数", new Value(MaximumIterations));
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

            FunctionCount = Math.Max(0, options[parameterCount + 4].ToInt());
            FunctionCodes = new string[FunctionCount];
            int functionCount = 0;
            for (int i = 0; i < FunctionCodes.Length; ++i)
            {
                var option = options[parameterCount + functionCount + 5];
                var isString = option.ValueType == ValueType.String;
                FunctionCodes[i] = (isString) ? option.StringValue : DefaultCode;

                if (i == functionCount && isString)
                {
                    ++functionCount;
                }
            }

            MaximumIterations = options[options.Length - 1].ToInt();
        }

        private double EvaluateFunc(Vector<double> p, double x)
        {
            var result = 0.0;
            for (int i = 0; i < FunctionCodes.Length; ++i)
            {
                result += EvaluateSingleFunc(FunctionCodes[i], p, x);
            }
            return result;
        }

        private double EvaluateSingleFunc(string code, Vector<double> p, double x)
        {
            var key = GenerateKey(code);
            if (evaluateFunctions.ContainsKey(key)) return evaluateFunctions[key].Invoke(p, x);

            evaluateFunctions[key] = null;

            var source = CodeTemplate.Replace("$Code", code);
            var assembly = Utility.LoadFromSource(key, source);

            var codeType = assembly.GetType("Code");
            var evaluateInfo = codeType.GetMethod("Evaluate");
            evaluateFunctions[key] = (Func<Vector<double>, double, double>)Delegate.CreateDelegate(typeof(Func<Vector<double>, double, double>), evaluateInfo);

            return evaluateFunctions[key].Invoke(p, x);
        }

        private string GenerateKey(string code) => $"{nameof(CustomizableAnalysisLibrary.Nodes)}{nameof(FittingLevenbergMarquardtMulti)}:{code}";

        public Table Run(Table data)
        {
            var dataX = Vector<double>.Build.DenseOfArray(data.GetColumn(IndexX).ToDoubleArray());
            var dataY = Vector<double>.Build.DenseOfArray(data.GetColumn(IndexY).ToDoubleArray());
            var model = ObjectiveFunction.NonlinearModel(EvaluateFunc, dataX, dataY);

            var initialParameters = CreateVector.DenseOfArray(Parameters);
            var solver = new LevenbergMarquardtMinimizer(maximumIterations: MaximumIterations);

            var result = solver.FindMinimum(model, initialParameters);
            Parameters = result.MinimizingPoint.ToArray();

            if (OutputCurve)
            {
                var columns = new List<IEnumerable<Value>>();
                columns.Add(data.GetColumn(IndexX));
                columns.Add(data.GetColumn(IndexY));
                columns.Add(result.MinimizedValues.Select(v => new Value(v)));

                if (FunctionCodes.Length > 1)
                {
                    for (int i = 0; i < FunctionCodes.Length; ++i)
                    {
                        var code = FunctionCodes[i];
                        columns.Add(dataX.Select(x => new Value(EvaluateSingleFunc(code, result.MinimizingPoint, x))));
                    }
                }

                return Table.CreateFromColumns(columns);
            }
            else
            {
                return Table.CreateFromRows(Parameters.Select(x => new Value(x)));
            }
        }
    }
}
