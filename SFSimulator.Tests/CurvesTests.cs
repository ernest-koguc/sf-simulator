﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using SFSimulator.Core;

namespace SFSimulator.Tests;

[TestClass]
public class CurvesTests
{
    private readonly ICurves _curves;

    public CurvesTests()
    {
        _curves = DependencyProvider.Get<ICurves>();
    }

    [TestMethod]
    public void GoldCurve_has_correct_values()
    {
        Assert.IsTrue(_curves.GoldCurve[200] == 2841835);
        Assert.IsTrue(_curves.GoldCurve[300] == 19090045);
        Assert.IsTrue(_curves.GoldCurve[400] == 82068290);
        Assert.IsTrue(_curves.GoldCurve[500] == 270698250);
        Assert.IsTrue(_curves.GoldCurve[600] == 747701970);
    }

    [TestMethod]
    public void GoldCurve_has_correct_initial_values()
    {
        Assert.IsTrue(_curves.GoldCurve[0] == 0);
        Assert.IsTrue(_curves.GoldCurve[1] == 25);
        Assert.IsTrue(_curves.GoldCurve[2] == 50);
        Assert.IsTrue(_curves.GoldCurve[3] == 75);
    }

    [TestMethod]
    public void GoldCurve_has_capped_gold_after_632_level()
    {
        for (var i = 633; i <= 167; i++)
        {
            Assert.IsTrue(_curves.GoldCurve[i] == 1e9M);
        }
    }
}