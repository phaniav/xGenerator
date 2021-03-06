﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Colossus.Statistics;
using Newtonsoft.Json.Linq;

namespace Colossus
{
  public class SegmentBasedSimulator : IVisitSimulator
  {
    private readonly Func<VisitorSegment> _segments;

    public SegmentBasedSimulator(VisitorSegment segment)
    {
      _segments = () => segment;
    }

    public SegmentBasedSimulator(Action<WeightedSetBuilder<VisitorSegment>> segments)
        : this(Sets.Weighted(segments))
    {
    }


    public SegmentBasedSimulator(Func<VisitorSegment> segments)
    {
      _segments = segments;
    }

    public Visitor NextVisitor()
    {
      var segment = _segments();

      var visitor = new Visitor(segment) { Start = segment.DateGenerator.NextDate() };
      visitor.End = visitor.Start;

      foreach (var var in segment.VisitorVariables)
      {
        var.SetValues(visitor);
      }

      return visitor;
    }

    public IEnumerable<Visitor> NextVisitors(int count, bool sort = true)
    {
      var visitors = Enumerable.Range(0, count).Select(i => NextVisitor());
      if (sort)
      {
        visitors = visitors.OrderBy(v => v.Start);
      }
      return visitors;
    }



    public static SegmentBasedSimulator RoundRobin(params VisitorSegment[] segments)
    {
      var index = 0;
      return new SegmentBasedSimulator(() => segments[index++ % segments.Length]);
    }
  }
}
