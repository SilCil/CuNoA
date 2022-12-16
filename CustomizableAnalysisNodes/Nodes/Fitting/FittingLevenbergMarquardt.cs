using System;
using System.Collections.Generic;
using System.Linq;
using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.Optimization;

namespace CustomizableAnalysisLibrary.Nodes
{
    [Node("フィッティング/任意関数/レーベンバーグ・マーカート法")]
    public class FittingLevenbergMarquardt : ICalculationNode, IOptionNode
    {
        private const string CodeTemplate = @"
using System;
using System.Collections.Generic;
using MathNet.Numerics.LinearAlgebra;
using CustomizableAnalysisLibrary;

public static class Code
{
    public static double Evaluate(Vector<double> a, double x) => $Code;
}
";

        public int IndexX { get; set; } = 0;
        public int IndexY { get; set; } = 1;
        public bool OutputCurve { get; set; } = true;

        public int ParameterCount { get; set; } = 1;
        public double[] Parameters { get; set; } = { 1.0 };

        public double[] ResultingParameters { get; set; } = default;

        public string FunctionCode { get; set; } = "Math.Exp(-a[0]*x)";
        public int MaximumIterations { get; set; } = -1;

        private string codeKey = default;
        private Func<Vector<double>, double, double> evaluate = default;

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

            FunctionCode = options[options.Length - 2].ToString();
            MaximumIterations = options[options.Length - 1].ToInt();
        }

        private string GenerateKey() => $"{nameof(CustomizableAnalysisLibrary.Nodes)}{nameof(FittingLevenbergMarquardt)}:{FunctionCode}";

        private void SetEvaluateFunction()
        {
            var key = GenerateKey();
            if (key == codeKey && evaluate != null) return;

            codeKey = key;
            evaluate = null;

            var source = CodeTemplate.Replace("$Code", FunctionCode);
            var assembly = Utility.LoadFromSource(codeKey, source);

            var codeType = assembly.GetType("Code");
            var evaluateInfo = codeType.GetMethod("Evaluate");
            evaluate = (Func<Vector<double>, double, double>)Delegate.CreateDelegate(typeof(Func<Vector<double>, double, double>), evaluateInfo);
        }

        public Table Run(Table data)
        {
            SetEvaluateFunction();
            var dataX = Vector<double>.Build.DenseOfArray(data.GetColumn(IndexX).ToDoubleArray());
            var dataY = Vector<double>.Build.DenseOfArray(data.GetColumn(IndexY).ToDoubleArray());
            var model = ObjectiveFunction.NonlinearModel(evaluate, dataX, dataY);

            var initialParameters = CreateVector.DenseOfArray(Parameters);
            var solver = new LevenbergMarquardtMinimizer(maximumIterations: MaximumIterations);

            var result = solver.FindMinimum(model, initialParameters);
            ResultingParameters = result.MinimizingPoint.ToArray();
            
            if (OutputCurve)
            {
                var curve = result.MinimizedValues.Select(v => new Value(v));
                return Table.CreateFromColumns(data.GetColumn(IndexX), data.GetColumn(IndexY), curve);
            }
            else
            {
                return Table.CreateFromRows(ResultingParameters.Select(x => new Value(x)));
            }
        }
    }
}
