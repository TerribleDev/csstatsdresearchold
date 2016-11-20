﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace StatsdClient
{
  /// <summary>
  /// A class that is used to measure a latency wrapped in a using block.
  /// </summary>
  [DebuggerDisplay("{_name} - IsActive = {_stopwatch.IsRunning}")]
  public sealed class TimingToken : IDisposable
  {
    private IStatsd _client;
    private string _name;
    private Stopwatch _stopwatch;

    internal TimingToken(IStatsd client, string name)
    {
      _stopwatch = Stopwatch.StartNew();
      _client = client;
      _name = name;
    }

    /// <summary>
    /// Stops the internal timer and logs a latency metric.
    /// </summary>
    public void Dispose()
    {
      _stopwatch.Stop();
      _client.LogTimingAsync(_name, (int)_stopwatch.ElapsedMilliseconds);
    }
  }
}
