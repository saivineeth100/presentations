﻿// Copyright Information
// ==================================
// DesignPatterns - CreationPatterns - SausagePizza.cs
// All samples copyright Philip Japikse
// http://www.skimedic.com 2022/01/20
// ==================================

namespace CreationPatterns.Factory.Entities.Base;

public class SausagePizza : IPizza
{
    public SausagePizza()
    {
        Toppings = new List<string> { "Sausage" };
    }

    public IList<string> Toppings { get; init; }
    public DoughTypeEnum Dough { get; init; }
    public virtual void Prepare()
    {
        //Do some prep
    }

    public virtual void Bake()
    {
        //Bake it
    }

    public virtual void Cut()
    {
        //Slice it
    }

    public virtual void Box()
    {
        //Box it
    }
}