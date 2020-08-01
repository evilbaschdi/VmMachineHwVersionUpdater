using System;
using System.IO;
using System.Linq;
using JetBrains.Annotations;

namespace VmMachineHwVersionUpdater.Core.PerMachine
{
    /// <inheritdoc />
    public abstract class UpsertVmxLine<T> : IUpsertVmxLine<T>
    {
        private string _falseValue;
        private string _lineKey;
        private string _trueValue;

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

            if (string.IsNullOrWhiteSpace(_lineKey))
            {
                return;
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

            if (valueString.Contains('"'))
            {
                return;
            }

            var newLine = $"{_lineKey} = \"{valueString}\"";

            foreach (var line in readAllLines.Where(line => line.Trim().StartsWith(_lineKey, StringComparison.InvariantCultureIgnoreCase)))
            {
                text = text.Replace(line, newLine, StringComparison.InvariantCultureIgnoreCase);
                lineContained = true;
            }

            if (!lineContained)
            {
                text = $"{text}{Environment.NewLine}{newLine}";
            }

            var outputStreamWriter = File.CreateText(vmxPath);
            outputStreamWriter.Write(text);
            outputStreamWriter.Close();
        }

        /// <inheritdoc />
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// </summary>
        /// <param name="disposing"></param>
        private void Dispose(bool disposing)
        {
            if (!disposing)
            {
                return;
            }

            _lineKey = string.Empty;
            _trueValue = string.Empty;
            _falseValue = string.Empty;
        }
    }
}