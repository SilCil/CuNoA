using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.Optimization;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CustomizableAnalysisLibrary.Nodes
{
    [Obsolete]
    // [Node("フィッティング/任意関数の和/ネルダー・ミード法")]
    public class FittingNelderMeasSimplexMulti : ICalculationNode, IOptionNode
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

        private const string DefaultCode = "Math.Exp(-a[0]*x)";

        public int IndexX { get; set; } = 0;
        public int IndexY { get; set; } = 1;
        public bool OutputCurve { get; set; } = true;

        public int ParameterCount { get; set; } = 1;
        public double[] Parameters { get; set; } = { 1.0 };

        public int FunctionCount { get; set; } = 1;
        public string[] FunctionCodes { get; set; } = new string[] { DefaultCode };

        public int MaximumIterations { get; set; } = 5000;
        public bool AllowNotConverged { get; set; } = false;

        private Dictionary<string, Func<double, IReadOnlyList<double>, double>> evaluateFunctions = new Dictionary<string, Func<double, IReadOnlyList<double>, double>>();

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

            MaximumIterations = options[options.Length - 2].ToInt();
            AllowNotConverged = options[options.Length - 1].ToBool();
        }

        private double EvaluateFunc(double x, IReadOnlyList<double> parameters)
        {
            var result = 0.0;
            for (int i = 0; i < FunctionCodes.Length; ++i)
            {
                result += EvaluateSingleFunc(FunctionCodes[i], x, parameters);
            }
            return result;
        }

        private string GenerateKey(string code) => $"{nameof(Nodes)}{nameof(FittingNelderMeasSimplexMulti)}:{code}";

        private double EvaluateSingleFunc(string code, double x, IReadOnlyList<double> parameters)
        {
            var key = GenerateKey(code);
            if (evaluateFunctions.ContainsKey(key)) return evaluateFunctions[key].Invoke(x, parameters);

            evaluateFunctions[key] = null;

            var source = CodeTemplate.Replace("$Code", code);
            var assembly = Utility.LoadFromSource(key, source);

            var codeType = assembly.GetType("Code");
            var evaluateInfo = codeType.GetMethod("Evaluate");
            evaluateFunctions[key] = (Func<double, IReadOnlyList<double>, double>)Delegate.CreateDelegate(typeof(Func<double, IReadOnlyList<double>, double>), evaluateInfo);

            return evaluateFunctions[key].Invoke(x, parameters);
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
            return sum / Math.Min(dataX.Length, dataY.Length);
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
            catch(MaximumIterationsException mie)
            {
                if (AllowNotConverged == false) throw mie;
                Parameters = initial.ToArray();
            }

            if (OutputCurve)
            {
                List<IEnumerable<Value>> columns = new List<IEnumerable<Value>>();
                columns.Add(data.GetColumn(IndexX));
                columns.Add(data.GetColumn(IndexY));
                columns.Add(dataX.Select(x => new Value(EvaluateFunc(x, Parameters))));
                for(int i = 0; i < FunctionCodes.Length && FunctionCodes.Length > 1; ++i)
                {
                    var code = FunctionCodes[i];
                    columns.Add(dataX.Select(x => new Value(EvaluateSingleFunc(code, x, Parameters))));
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
