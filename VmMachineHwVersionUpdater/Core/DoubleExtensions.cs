namespace VmMachineHwVersionUpdater.Core
{
    /// <summary>
    /// </summary>
    public static class DoubleExtensions
    {
        /// <summary>
        ///     input multiplied with 1024d * 1024d * 1024d
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static double GibibytesToKibibytes(this double? input)
        {
            return input * 1073741824d ?? 0d;
        }

        /// <summary>
        ///     input multiplied with 1024d * 1024d * 1024d
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static double GibibytesToKibibytes(this double input)
        {
            return input * 1073741824d;
        }

        /// <summary>
        ///     input divided by 1024d * 1024d * 1024d
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static double KibibytesToGibibytes(this double? input)
        {
            return input / 1073741824d ?? 0d;
        }

        /// <summary>
        ///     input divided by 1024d * 1024d * 1024d
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static double KibibytesToGibibytes(this double input)
        {
            return input / 1073741824d;
        }
    }
}