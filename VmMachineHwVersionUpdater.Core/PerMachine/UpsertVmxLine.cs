using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using JetBrains.Annotations;

namespace VmMachineHwVersionUpdater.Core.PerMachine
{
    /// <inheritdoc />
    public abstract class UpsertVmxLine<T> : IUpsertVmxLine<T>
    {
        private readonly string _falseValue;
        private readonly string _lineKey;
        private readonly string _trueValue;

        /// <summary>
        ///     Constructor
        /// </summary>
        /// <param name="lineKey"></param>
        protected UpsertVmxLine([NotNull] string lineKey)
        {
            _lineKey = lineKey ?? throw new ArgumentNullException(nameof(lineKey));
        }

        /// <summary>
        ///     Constructor
        /// </summary>
        /// <param name="lineKey"></param>
        /// <param name="trueValue"></param>
        /// <param name="falseValue"></param>
        protected UpsertVmxLine([NotNull] string lineKey, [NotNull] string trueValue, [NotNull] string falseValue)
        {
            _lineKey = lineKey ?? throw new ArgumentNullException(nameof(lineKey));
            _trueValue = trueValue ?? throw new ArgumentNullException(nameof(trueValue));
            _falseValue = falseValue ?? throw new ArgumentNullException(nameof(falseValue));
        }

        /// <inheritdoc />
        public void RunFor(string vmxPath, T value)
        {
            if (string.IsNullOrWhiteSpace(vmxPath))
            {
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(vmxPath));
            }

            var readAllLines = File.ReadAllLines(vmxPath).Where(l => !string.IsNullOrWhiteSpace(l)).ToList();

            var text = string.Join(Environment.NewLine, readAllLines);
            var lineContained = false;

            var valueString = value switch
            {
                bool b => b ? _trueValue : _falseValue,
                int i => i.ToString(),
                string s => s,
                _ => throw new InvalidOperationException($"Type {typeof(T)} is not handled.")
            };

            var newLine = $"{_lineKey} = \"{valueString}\"";

            foreach (var line in from line in readAllLines where line.Contains(_lineKey, StringComparison.InvariantCultureIgnoreCase) select line)
            {
                lineContained = true;
                text = Regex.Replace(text, line, newLine, RegexOptions.IgnoreCase).Trim();
            }

            if (!lineContained)
            {
                text = $"{text}{Environment.NewLine}{newLine}";
            }

            var outputStreamWriter = File.CreateText(vmxPath);
            outputStreamWriter.Write(text);
            outputStreamWriter.Close();
        }
    }
}