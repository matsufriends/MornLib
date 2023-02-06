namespace MornLib.Beats
{
    /// <summary>
    ///     1小節内における拍の構造体
    /// </summary>
    public readonly struct BeatTimingInfo
    {
        /// <summary>
        ///     何チック目か
        /// </summary>
        private readonly int _currentTick;

        /// <summary>
        ///     1小節に何チックあるか
        /// </summary>
        public readonly int TickCountPerMeasure;

        /// <summary>
        ///     コンストラクタ
        /// </summary>
        /// <param name="currentTick">何チック目か</param>
        /// <param name="tickCountPerMeasure">1小節に何チックあるか</param>
        public BeatTimingInfo(int currentTick, int tickCountPerMeasure)
        {
            _currentTick = currentTick;
            TickCountPerMeasure = tickCountPerMeasure;
        }

        /// <summary>
        ///     現在のチックに<paramref name="offsetTick" />を加算したインスタンスを作成する。
        /// </summary>
        /// <param name="offsetTick">オフセットチック</param>
        /// <returns>作成したインスタンス</returns>
        public BeatTimingInfo CloneWithOffset(int offsetTick)
        {
            return new BeatTimingInfo(_currentTick + offsetTick, TickCountPerMeasure);
        }

        /// <summary>
        ///     チックを上書きしたインスタンスを作成する
        /// </summary>
        /// <param name="tick">チック</param>
        /// <returns>作成したインスタンス</returns>
        public BeatTimingInfo CloneWithTickOverride(int tick)
        {
            return new BeatTimingInfo(tick, TickCountPerMeasure);
        }

        /// <summary>
        ///     <paramref name="beat" />ビートのいずれか拍に合うかどうか
        /// </summary>
        /// <param name="beat">1小節に何拍あるか</param>
        /// <param name="offsetTick">オフセットチック</param>
        /// <returns>拍に合うかどうか</returns>
        public bool IsJustForAnyBeat(int beat, int offsetTick = 0)
        {
            return (_currentTick + offsetTick) % (TickCountPerMeasure / beat) == 0;
        }

        /// <summary>
        ///     <paramref name="beat" />ビートの<paramref name="numerator" />拍目に合うかどうか
        /// </summary>
        /// <param name="numerator">特定の拍目</param>
        /// <param name="beat">1小節に何拍あるか</param>
        /// <param name="offsetTick">オフセットチック</param>
        /// <returns>拍に合うかどうか</returns>
        public bool IsJustForSpecificBeat(int numerator, int beat, int offsetTick = 0)
        {
            return (_currentTick + offsetTick) % TickCountPerMeasure == numerator * TickCountPerMeasure / beat;
        }
    }
}
