namespace HALUtilities
{
  internal class TypeStat
  {
    internal int Count { get; private set; }

    internal StuckType Type { get; private set; }

    internal void Increment() => ++this.Count;

    internal TypeStat(StuckType t) => this.Type = t;
  }
}
