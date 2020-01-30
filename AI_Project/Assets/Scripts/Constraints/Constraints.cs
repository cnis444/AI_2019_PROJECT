using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

#region ABSTRACT

public abstract class Constraint{ }

public abstract class EntityConstraint : Constraint 
{
    public abstract string ConstraintToString(List<ConstraintObject> constraints);
}
public abstract class PositionConstraint : Constraint 
{
    public abstract string ConstraintToString(List<ConstraintObject> constraints, Dictionary<string, int> arraySize);
}

public abstract class NumberEntityComparisonConstraint : EntityConstraint
{
    private Type id;
    private string val;
    private string op; 

    public NumberEntityComparisonConstraint(Type id, int val, string op) 
    {
        this.op = op;
        this.id = id;
        this.val = val.ToString();
        
    }
    public NumberEntityComparisonConstraint(string id, int val, string op)
    {
        this.op = op;
        this.id = Type.GetType(id);
        this.val = val.ToString();
    }
    private string BuildSum(List<ConstraintObject> constraints)
    {
        StringBuilder sum = new StringBuilder();
        int parenthesis = 0;
        constraints.ForEach(e => {
            if ((e.GetType().IsSubclassOf(id)) || (e.GetType() == id))
            {
                sum.Append("add(");
                sum.Append(e.GetType().ToString());
                sum.Append(",");
                parenthesis ++;
            }
        });
        sum.Append("0");
        for (; parenthesis != 0; parenthesis --)
            sum.Append(")");

        return sum.ToString();
    }
    public override string ConstraintToString(List<ConstraintObject> constraints)
    {
        return "<intension>" + op + BuildSum(constraints) + ", " + val +") </intension>";
    }
}
public class ForbiddingEntitySolutionConstraint : EntityConstraint
{
    private string[] id;
    private string[] val;

    public ForbiddingEntitySolutionConstraint(string[] id, string[] val)
    {
        this.id = id;
        this.val = val;
    }

    public ForbiddingEntitySolutionConstraint(string[] id, int[] val)
    {
        string[] value = new string[val.Length];
        for (int i = 0; i < val.Length; i++)
            value[i] = val[i].ToString();
        this.id = id;
        this.val = value;
    }

    public override string ConstraintToString(List<ConstraintObject> constraints)
    {
        StringBuilder BannedSolution = new StringBuilder();

        for (int i = 0; i < id.Length; i++)
        {
            BannedSolution.Append("<intension> ne(");
            BannedSolution.Append(id[i]);
            BannedSolution.Append(",");
            BannedSolution.Append(val[i]);
            BannedSolution.Append(") </intension>");
        }

        return BannedSolution.ToString();
    }
}
public abstract class NumberEntityPerEntityComparisonConstraint : EntityConstraint
{
    private Type id1;
    private Type id2;
    private string val;
    private string op;

    public NumberEntityPerEntityComparisonConstraint(Type id1, Type id2, int val, string op)
    {
        this.id2 = id2;
        this.op = op;
        this.id1 = id1;
        this.val = val.ToString();

    }
    public NumberEntityPerEntityComparisonConstraint(string id1, string id2, int val, string op)
    {
        this.op = op;
        this.id1 = Type.GetType(id1);
        this.id2 = Type.GetType(id2);
        this.val = val.ToString();
    }
    public NumberEntityPerEntityComparisonConstraint(Type id1, string id2, int val, string op)
    {
        this.id2 = Type.GetType(id2);
        this.op = op;
        this.id1 = id1;
        this.val = val.ToString();

    }
    public NumberEntityPerEntityComparisonConstraint(string id1, Type id2, int val, string op)
    {
        this.op = op;
        this.id1 = Type.GetType(id1);
        this.id2 = id2;
        this.val = val.ToString();
    }

    private string BuildSum(List<ConstraintObject> constraints, Type id)
    {
        StringBuilder sum = new StringBuilder();
        int parenthesis = 0;
        constraints.ForEach(e => {
            if ((e.GetType().IsSubclassOf(id)) || (e.GetType() == id))
            {
                sum.Append("add(");
                sum.Append(e.GetType().ToString());
                sum.Append(",");
                parenthesis++;
            }
        });
        sum.Append("0");
        for (; parenthesis != 0; parenthesis--)
            sum.Append(")");

        return sum.ToString();
    }

    public override string ConstraintToString(List<ConstraintObject> constraints)
    {
        return "<intension>" + op + "mul(" + val + "," + BuildSum(constraints, id1) + "), " + BuildSum(constraints, id2) + ") </intension>";
    }
}

public abstract class EntityRelativeDistanceComparisonConstraint : PositionConstraint 
{
    private Type id1;
    private Type id2;
    private string distanceSquared;
    private string op;

    public EntityRelativeDistanceComparisonConstraint(Type id1, Type id2, int distance, string op)
    {
        this.id2 = id2;
        this.op = op;
        this.id1 = id1;
        this.distanceSquared = (10 * distance *distance).ToString();
    }
    public EntityRelativeDistanceComparisonConstraint(string id1, string id2, int distance, string op)
    {
        this.op = op;
        this.id1 = Type.GetType(id1);
        this.id2 = Type.GetType(id2);
        this.distanceSquared = (10 * distance * distance).ToString();
    }
    public EntityRelativeDistanceComparisonConstraint(Type id1, string id2, int distance, string op)
    {
        this.id2 = Type.GetType(id2);
        this.op = op;
        this.id1 = id1;
        this.distanceSquared = (10 * distance * distance).ToString();
    }
    public EntityRelativeDistanceComparisonConstraint(string id1, Type id2, int distance, string op)
    {
        this.op = op;
        this.id1 = Type.GetType(id1);
        this.id2 = id2;
        this.distanceSquared = (10 * distance * distance).ToString();
    }
    public EntityRelativeDistanceComparisonConstraint(Type id1, Type id2, float distance, string op)
    {
        this.id2 = id2;
        this.op = op;
        this.id1 = id1;
        this.distanceSquared = (10 * distance * distance).ToString().Replace(",", ".");
    }
    public EntityRelativeDistanceComparisonConstraint(string id1, string id2, float distance, string op)
    {
        this.op = op;
        this.id1 = Type.GetType(id1);
        this.id2 = Type.GetType(id2);
        this.distanceSquared = (10 * distance * distance).ToString().Replace(",", ".");
    }
    public EntityRelativeDistanceComparisonConstraint(Type id1, string id2, float distance, string op)
    {
        this.id2 = Type.GetType(id2);
        this.op = op;
        this.id1 = id1;
        this.distanceSquared = (10 * distance * distance).ToString().Replace(",", ".");
    }
    public EntityRelativeDistanceComparisonConstraint(string id1, Type id2, float distance, string op)
    {
        this.op = op;
        this.id1 = Type.GetType(id1);
        this.id2 = id2;
        this.distanceSquared = (10 * distance * distance).ToString().Replace(",", ".");
    }

    private void BuildArg(StringBuilder args, int x, int y, string tag1, string tag2)
    {
        args.Append("<args> ");
        args.Append(tag1);
        args.Append("[");
        args.Append(x.ToString());
        args.Append("][0] ");
        args.Append(tag1);
        args.Append("[");
        args.Append(x.ToString());
        args.Append("][1] ");
        args.Append(tag1);
        args.Append("[");
        args.Append(x.ToString());
        args.Append("][2] ");
        args.Append(tag2);
        args.Append("[");
        args.Append(y.ToString());
        args.Append("][0] ");
        args.Append(tag2);
        args.Append("[");
        args.Append(y.ToString());
        args.Append("][1] ");
        args.Append(tag2);
        args.Append("[");
        args.Append(y.ToString());
        args.Append("][2] ");
        args.Append("</args>");
    }

    private string argsPart(List<ConstraintObject> objects, Dictionary<string, int> arraySize)
    {
        StringBuilder args= new StringBuilder();
        objects.ForEach(e => 
        {
            if ((e.GetType().IsSubclassOf(id1)) || (e.GetType() == id1))
            {
                objects.ForEach(i => 
                { 
                    if ((i.GetType().IsSubclassOf(id2)) || (i.GetType() == id2))
                    {
                        string tag1 = e.GetType().ToString();
                        string tag2 = i.GetType().ToString();
                        if (tag1 != tag2)
                        {
                            for (int x = 0; x < arraySize[tag1]; x++)
                            {
                                for (int y = 0; y < arraySize[tag2]; y++)
                                {
                                    BuildArg(args, x, y, tag1, tag2);
                                }
                            }
                        }
                        else
                        {
                            for (int x = 0; x < arraySize[tag1]; x++)
                            {
                                for (int y = 0; y < arraySize[tag2]; y++)
                                {
                                    if (x != y)
                                        BuildArg(args, x, y, tag1, tag2);
                                }
                            }
                        }
                    }
                });
            }
        });

        return args.ToString();
    }

    public override string ConstraintToString(List<ConstraintObject> objects, Dictionary<string, int> arraySize)
    {
        return "<group> < intension > " + op + "add(sqr(sub(%0, %3)), sqr(sub(%1, %4)), sqr(sub(%2, %5)))," + distanceSquared + ")</intension>" + argsPart(objects, arraySize) + "</ group >";
    }
}
public abstract class EntityToPositionDistanceComparisonConstraint : PositionConstraint
{
    private Type id1;
    private string x;
    private string y;
    private string z;
    private string distanceSquared;
    private string op;

    public EntityToPositionDistanceComparisonConstraint(Type id1, int distance, string op, Vector3Int pos)
    {
        this.op = op;
        this.id1 = id1;
        this.distanceSquared = (10 * distance * distance).ToString();
        x = pos.x.ToString();
        y = pos.y.ToString();
        z = pos.z.ToString();
}
    public EntityToPositionDistanceComparisonConstraint(string id1, int distance, string op, Vector3Int pos)
    {
        this.op = op;
        this.id1 = Type.GetType(id1);
        this.distanceSquared = (10 * distance * distance).ToString();
        x = pos.x.ToString();
        y = pos.y.ToString();
        z = pos.z.ToString();
    }
    public EntityToPositionDistanceComparisonConstraint(string id1, float distance, string op, Vector3Int pos)
    {
        this.op = op;
        this.id1 = Type.GetType(id1);
        this.distanceSquared = (10 * distance * distance).ToString().Replace(",", ".");
        x = pos.x.ToString();
        y = pos.y.ToString();
        z = pos.z.ToString();
    }
    public EntityToPositionDistanceComparisonConstraint(Type id1, float distance, string op, Vector3Int pos)
    {
        this.op = op;
        this.id1 = id1;
        this.distanceSquared = (10 * distance * distance).ToString().Replace(",", ".");
        x = pos.x.ToString();
        y = pos.y.ToString();
        z = pos.z.ToString();
    }
    public EntityToPositionDistanceComparisonConstraint(Type id1, int distance, string op, Vector3 pos)
    {
        this.op = op;
        this.id1 = id1;
        this.distanceSquared = (10 * distance * distance).ToString();
        x = pos.x.ToString().Replace(",", ".");
        y = pos.y.ToString().Replace(",", ".");
        z = pos.z.ToString().Replace(",", ".");
    }
    public EntityToPositionDistanceComparisonConstraint(string id1, int distance, string op, Vector3 pos)
    {
        this.op = op;
        this.id1 = Type.GetType(id1);
        this.distanceSquared = (10 * distance * distance).ToString();
        x = pos.x.ToString().Replace(",", ".");
        y = pos.y.ToString().Replace(",", ".");
        z = pos.z.ToString().Replace(",", ".");
    }
    public EntityToPositionDistanceComparisonConstraint(string id1, float distance, string op, Vector3 pos)
    {
        this.op = op;
        this.id1 = Type.GetType(id1);
        this.distanceSquared = (10 * distance * distance).ToString().Replace(",", ".");
        x = pos.x.ToString().Replace(",", ".");
        y = pos.y.ToString().Replace(",", ".");
        z = pos.z.ToString().Replace(",", ".");
    }
    public EntityToPositionDistanceComparisonConstraint(Type id1, float distance, string op, Vector3 pos)
    {
        this.op = op;
        this.id1 = id1;
        this.distanceSquared = (10 * distance * distance).ToString().Replace(",", ".");
        x = pos.x.ToString().Replace(",", ".");
        y = pos.y.ToString().Replace(",", ".");
        z = pos.z.ToString().Replace(",", ".");
    }


    private void BuildArg(StringBuilder args, string x, string tag1)
    {
        args.Append("<args> ");
        args.Append(tag1);
        args.Append("[");
        args.Append(x);
        args.Append("][0] ");
        args.Append(tag1);
        args.Append("[");
        args.Append(x);
        args.Append("][1] ");
        args.Append(tag1);
        args.Append("[");
        args.Append(x);
        args.Append("][2] ");
    }

    private string argsPart(List<ConstraintObject> objects, Dictionary<string, int> arraySize)
    {
        StringBuilder args = new StringBuilder();
        objects.ForEach(e =>
        {
            if ((e.GetType().IsSubclassOf(id1)) || (e.GetType() == id1))
            {
                string tag1 = e.GetType().ToString();
                for (int x = 0; x < arraySize[tag1]; x++)
                    BuildArg(args, x.ToString(), tag1);
            }
        });

        return args.ToString();
    }

    public override string ConstraintToString(List<ConstraintObject> objects, Dictionary<string, int> arraySize)
    {
        return "<group> < intension > " + op + "add(sqr(sub(%0, "+ x +")), sqr(sub(%1, "+ y +")), sqr(sub(%2, "+ z +")))," + distanceSquared + ")</intension>" + argsPart(objects, arraySize) + "</ group >";
    }
}

#endregion

#region ENTITY

#region ENTITY GLOBAL

public class NumberEntityDifferentConstraint : NumberEntityComparisonConstraint
{
    public NumberEntityDifferentConstraint(Type id, int val) : base(id, val, "ne(") { }
    public NumberEntityDifferentConstraint(string id, int val) : base(id, val, "ne(") { }
}
public class NumberEntityEqualConstraint : NumberEntityComparisonConstraint
{
    public NumberEntityEqualConstraint(Type id, int val) : base(id, val, "eq(") { }
    public NumberEntityEqualConstraint(string id, int val) : base(id, val, "eq(") { }
}
public class NumberEntityGreaterConstraint : NumberEntityComparisonConstraint
{
    public NumberEntityGreaterConstraint(Type id, int val) : base(id, val, "ge(") { }
    public NumberEntityGreaterConstraint(string id, int val) : base(id, val, "ge(") { }
}
public class NumberEntityGreaterStrictConstraint : NumberEntityComparisonConstraint
{
    public NumberEntityGreaterStrictConstraint(Type id, int val) : base(id, val, "gt(") { }
    public NumberEntityGreaterStrictConstraint(string id, int val) : base(id, val, "gt(") { }
}
public class NumberEntityLessConstraint : NumberEntityComparisonConstraint
{
    public NumberEntityLessConstraint(Type id, int val) : base(id, val, "le(") { }
    public NumberEntityLessConstraint(string id, int val) : base(id, val, "le(") { }
}
public class NumberEntityLessStrictConstraint : NumberEntityComparisonConstraint
{
    public NumberEntityLessStrictConstraint(Type id, int val) : base(id, val, "lt(") { }
    public NumberEntityLessStrictConstraint(string id, int val) : base(id, val, "lt(") { }
}

#endregion

#region PER ENTITY

public class NumberEntityPerEntityDifferentConstraint : NumberEntityPerEntityComparisonConstraint
{
    public NumberEntityPerEntityDifferentConstraint(Type id1, Type id2, int val, string op) : base(id1, id2, val, "ne(") { }
    public NumberEntityPerEntityDifferentConstraint(string id1, string id2, int val, string op) : base(id1, id2, val, "ne(") { }
    public NumberEntityPerEntityDifferentConstraint(Type id1, string id2, int val, string op) : base(id1, id2, val, "ne(") { }
    public NumberEntityPerEntityDifferentConstraint(string id1, Type id2, int val, string op) : base(id1, id2, val, "ne(") { }
}
public class NumberEntityPerEntityEqualConstraint : NumberEntityPerEntityComparisonConstraint
{
    public NumberEntityPerEntityEqualConstraint(Type id1, Type id2, int val, string op) : base(id1, id2, val, "eq(") { }
    public NumberEntityPerEntityEqualConstraint(string id1, string id2, int val, string op) : base(id1, id2, val, "eq(") { }
    public NumberEntityPerEntityEqualConstraint(Type id1, string id2, int val, string op) : base(id1, id2, val, "eq(") { }
    public NumberEntityPerEntityEqualConstraint(string id1, Type id2, int val, string op) : base(id1, id2, val, "eq(") { }
}
public class NumberEntityPerEntityGreaterConstraint : NumberEntityPerEntityComparisonConstraint
{
    public NumberEntityPerEntityGreaterConstraint(Type id1, Type id2, int val, string op) : base(id1, id2, val, "ge(") { }
    public NumberEntityPerEntityGreaterConstraint(string id1, string id2, int val, string op) : base(id1, id2, val, "ge(") { }
    public NumberEntityPerEntityGreaterConstraint(Type id1, string id2, int val, string op) : base(id1, id2, val, "ge(") { }
    public NumberEntityPerEntityGreaterConstraint(string id1, Type id2, int val, string op) : base(id1, id2, val, "ge(") { }
}
public class NumberEntityPerEntityGreaterStrictConstraint : NumberEntityPerEntityComparisonConstraint
{
    public NumberEntityPerEntityGreaterStrictConstraint(Type id1, Type id2, int val, string op) : base(id1, id2, val, "gt(") { }
    public NumberEntityPerEntityGreaterStrictConstraint(string id1, string id2, int val, string op) : base(id1, id2, val, "gt(") { }
    public NumberEntityPerEntityGreaterStrictConstraint(Type id1, string id2, int val, string op) : base(id1, id2, val, "gt(") { }
    public NumberEntityPerEntityGreaterStrictConstraint(string id1, Type id2, int val, string op) : base(id1, id2, val, "gt(") { }
}
public class NumberEntityPerEntityLessConstraint : NumberEntityPerEntityComparisonConstraint
{
    public NumberEntityPerEntityLessConstraint(Type id1, Type id2, int val, string op) : base(id1, id2, val, "le(") { }
    public NumberEntityPerEntityLessConstraint(string id1, string id2, int val, string op) : base(id1, id2, val, "le(") { }
    public NumberEntityPerEntityLessConstraint(Type id1, string id2, int val, string op) : base(id1, id2, val, "le(") { }
    public NumberEntityPerEntityLessConstraint(string id1, Type id2, int val, string op) : base(id1, id2, val, "le(") { }
}
public class NumberEntityPerEntityLessStrictConstraint : NumberEntityPerEntityComparisonConstraint
{
    public NumberEntityPerEntityLessStrictConstraint(Type id1, Type id2, int val, string op) : base(id1, id2, val, "lt(") { }
    public NumberEntityPerEntityLessStrictConstraint(string id1, string id2, int val, string op) : base(id1, id2, val, "lt(") { }
    public NumberEntityPerEntityLessStrictConstraint(Type id1, string id2, int val, string op) : base(id1, id2, val, "lt(") { }
    public NumberEntityPerEntityLessStrictConstraint(string id1, Type id2, int val, string op) : base(id1, id2, val, "lt(") { }
}

#endregion

#endregion

#region DISTANCE

#region RELATIVE DISTANCE
public class EntityRelativeDistanceDifferentConstraint : EntityRelativeDistanceComparisonConstraint
{
    public EntityRelativeDistanceDifferentConstraint(Type id1, Type id2, int distance) : base(id1, id2, distance, "ne(") { }
    public EntityRelativeDistanceDifferentConstraint(string id1, string id2, int distance) : base(id1, id2, distance, "ne(") { }
    public EntityRelativeDistanceDifferentConstraint(Type id1, string id2, int distance) : base(id1, id2, distance, "ne(") { }
    public EntityRelativeDistanceDifferentConstraint(string id1, Type id2, int distance) : base(id1, id2, distance, "ne(") { }
    public EntityRelativeDistanceDifferentConstraint(Type id1, Type id2, float distance) : base(id1, id2, distance, "ne(") { }
    public EntityRelativeDistanceDifferentConstraint(string id1, string id2, float distance) : base(id1, id2, distance, "ne(") { }
    public EntityRelativeDistanceDifferentConstraint(Type id1, string id2, float distance) : base(id1, id2, distance, "ne(") { }
    public EntityRelativeDistanceDifferentConstraint(string id1, Type id2, float distance) : base(id1, id2, distance, "ne(") { }
}
public class EntityRelativeDistanceEqualConstraint : EntityRelativeDistanceComparisonConstraint
{
    public EntityRelativeDistanceEqualConstraint(Type id1, Type id2, int distance) : base(id1, id2, distance, "eq(") { }
    public EntityRelativeDistanceEqualConstraint(string id1, string id2, int distance) : base(id1, id2, distance, "eq(") { }
    public EntityRelativeDistanceEqualConstraint(Type id1, string id2, int distance) : base(id1, id2, distance, "eq(") { }
    public EntityRelativeDistanceEqualConstraint(string id1, Type id2, int distance) : base(id1, id2, distance, "eq(") { }
    public EntityRelativeDistanceEqualConstraint(Type id1, Type id2, float distance) : base(id1, id2, distance, "eq(") { }
    public EntityRelativeDistanceEqualConstraint(string id1, string id2, float distance) : base(id1, id2, distance, "eq(") { }
    public EntityRelativeDistanceEqualConstraint(Type id1, string id2, float distance) : base(id1, id2, distance, "eq(") { }
    public EntityRelativeDistanceEqualConstraint(string id1, Type id2, float distance) : base(id1, id2, distance, "eq(") { }
}
public class EntityRelativeDistanceGreaterConstraint : EntityRelativeDistanceComparisonConstraint
{
    public EntityRelativeDistanceGreaterConstraint(Type id1, Type id2, int distance) : base(id1, id2, distance, "ge(") { }
    public EntityRelativeDistanceGreaterConstraint(string id1, string id2, int distance) : base(id1, id2, distance, "ge(") { }
    public EntityRelativeDistanceGreaterConstraint(Type id1, string id2, int distance) : base(id1, id2, distance, "ge(") { }
    public EntityRelativeDistanceGreaterConstraint(string id1, Type id2, int distance) : base(id1, id2, distance, "ge(") { }
    public EntityRelativeDistanceGreaterConstraint(Type id1, Type id2, float distance) : base(id1, id2, distance, "ge(") { }
    public EntityRelativeDistanceGreaterConstraint(string id1, string id2, float distance) : base(id1, id2, distance, "ge(") { }
    public EntityRelativeDistanceGreaterConstraint(Type id1, string id2, float distance) : base(id1, id2, distance, "ge(") { }
    public EntityRelativeDistanceGreaterConstraint(string id1, Type id2, float distance) : base(id1, id2, distance, "ge(") { }
}
public class EntityRelativeDistanceGreaterStrictConstraint : EntityRelativeDistanceComparisonConstraint
{
    public EntityRelativeDistanceGreaterStrictConstraint(Type id1, Type id2, int distance) : base(id1, id2, distance, "gt(") { }
    public EntityRelativeDistanceGreaterStrictConstraint(string id1, string id2, int distance) : base(id1, id2, distance, "gt(") { }
    public EntityRelativeDistanceGreaterStrictConstraint(Type id1, string id2, int distance) : base(id1, id2, distance, "gt(") { }
    public EntityRelativeDistanceGreaterStrictConstraint(string id1, Type id2, int distance) : base(id1, id2, distance, "gt(") { } 
    public EntityRelativeDistanceGreaterStrictConstraint(Type id1, Type id2, float distance) : base(id1, id2, distance, "gt(") { }
    public EntityRelativeDistanceGreaterStrictConstraint(string id1, string id2, float distance) : base(id1, id2, distance, "gt(") { }
    public EntityRelativeDistanceGreaterStrictConstraint(Type id1, string id2, float distance) : base(id1, id2, distance, "gt(") { }
    public EntityRelativeDistanceGreaterStrictConstraint(string id1, Type id2, float distance) : base(id1, id2, distance, "gt(") { }

}
public class EntityRelativeDistanceLessConstraint : EntityRelativeDistanceComparisonConstraint
{
    public EntityRelativeDistanceLessConstraint(Type id1, Type id2, int distance) : base(id1, id2, distance, "le(") { }
    public EntityRelativeDistanceLessConstraint(string id1, string id2, int distance) : base(id1, id2, distance, "le(") { }
    public EntityRelativeDistanceLessConstraint(Type id1, string id2, int distance) : base(id1, id2, distance, "le(") { }
    public EntityRelativeDistanceLessConstraint(string id1, Type id2, int distance) : base(id1, id2, distance, "le(") { }
    public EntityRelativeDistanceLessConstraint(Type id1, Type id2, float distance) : base(id1, id2, distance, "le(") { }
    public EntityRelativeDistanceLessConstraint(string id1, string id2, float distance) : base(id1, id2, distance, "le(") { }
    public EntityRelativeDistanceLessConstraint(Type id1, string id2, float distance) : base(id1, id2, distance, "le(") { }
    public EntityRelativeDistanceLessConstraint(string id1, Type id2, float distance) : base(id1, id2, distance, "le(") { }
}
public class EntityRelativeDistanceLessStrictConstraint : EntityRelativeDistanceComparisonConstraint
{
    public EntityRelativeDistanceLessStrictConstraint(Type id1, Type id2, int distance) : base(id1, id2, distance, "lt(") { }
    public EntityRelativeDistanceLessStrictConstraint(string id1, string id2, int distance) : base(id1, id2, distance, "lt(") { }
    public EntityRelativeDistanceLessStrictConstraint(Type id1, string id2, int distance) : base(id1, id2, distance, "lt(") { }
    public EntityRelativeDistanceLessStrictConstraint(string id1, Type id2, int distance) : base(id1, id2, distance, "lt(") { }
    public EntityRelativeDistanceLessStrictConstraint(Type id1, Type id2, float distance) : base(id1, id2, distance, "lt(") { }
    public EntityRelativeDistanceLessStrictConstraint(string id1, string id2, float distance) : base(id1, id2, distance, "lt(") { }
    public EntityRelativeDistanceLessStrictConstraint(Type id1, string id2, float distance) : base(id1, id2, distance, "lt(") { }
    public EntityRelativeDistanceLessStrictConstraint(string id1, Type id2, float distance) : base(id1, id2, distance, "lt(") { }
}

#endregion

#region TO POSITION DISTANCE

public class EntityToPositionDistanceDifferentConstraint : EntityToPositionDistanceComparisonConstraint
{
    public EntityToPositionDistanceDifferentConstraint(Type id1, int distance, Vector3Int pos) : base(id1, distance, "ne(", pos) { }
    public EntityToPositionDistanceDifferentConstraint(string id1, int distance, Vector3Int pos) : base(id1, distance, "ne(", pos) { }
    public EntityToPositionDistanceDifferentConstraint(string id1, float distance, Vector3Int pos) : base(id1, distance, "ne(", pos) { }
    public EntityToPositionDistanceDifferentConstraint(Type id1, float distance, Vector3Int pos) : base(id1, distance, "ne(", pos) { }
    public EntityToPositionDistanceDifferentConstraint(Type id1, int distance, Vector3 pos) : base(id1, distance, "ne(", pos) { }
    public EntityToPositionDistanceDifferentConstraint(string id1, int distance, Vector3 pos) : base(id1, distance, "ne(", pos) { }
    public EntityToPositionDistanceDifferentConstraint(string id1, float distance, Vector3 pos) : base(id1, distance, "ne(", pos) { }
    public EntityToPositionDistanceDifferentConstraint(Type id1, float distance, Vector3 pos) : base(id1, distance, "ne(", pos) { }
}
public class EntityToPositionDistanceEqualConstraint : EntityToPositionDistanceComparisonConstraint
{
    public EntityToPositionDistanceEqualConstraint(Type id1, int distance, Vector3Int pos) : base(id1, distance, "eq(", pos) { }
    public EntityToPositionDistanceEqualConstraint(string id1, int distance, Vector3Int pos) : base(id1, distance, "eq(", pos) { }
    public EntityToPositionDistanceEqualConstraint(string id1, float distance, Vector3Int pos) : base(id1, distance, "eq(", pos) { }
    public EntityToPositionDistanceEqualConstraint(Type id1, float distance, Vector3Int pos) : base(id1, distance, "eq(", pos) { }
    public EntityToPositionDistanceEqualConstraint(Type id1, int distance, Vector3 pos) : base(id1, distance, "eq(", pos) { }
    public EntityToPositionDistanceEqualConstraint(string id1, int distance, Vector3 pos) : base(id1, distance, "eq(", pos) { }
    public EntityToPositionDistanceEqualConstraint(string id1, float distance, Vector3 pos) : base(id1, distance, "eq(", pos) { }
    public EntityToPositionDistanceEqualConstraint(Type id1, float distance, Vector3 pos) : base(id1, distance, "eq(", pos) { }
}
public class EntityToPositionDistanceLessConstraint : EntityToPositionDistanceComparisonConstraint
{
    public EntityToPositionDistanceLessConstraint(Type id1, int distance, Vector3Int pos) : base(id1, distance, "le(", pos) { }
    public EntityToPositionDistanceLessConstraint(string id1, int distance, Vector3Int pos) : base(id1, distance, "le(", pos) { }
    public EntityToPositionDistanceLessConstraint(string id1, float distance, Vector3Int pos) : base(id1, distance, "le(", pos) { }
    public EntityToPositionDistanceLessConstraint(Type id1, float distance, Vector3Int pos) : base(id1, distance, "le(", pos) { }
    public EntityToPositionDistanceLessConstraint(Type id1, int distance, Vector3 pos) : base(id1, distance, "le(", pos) { }
    public EntityToPositionDistanceLessConstraint(string id1, int distance, Vector3 pos) : base(id1, distance, "le(", pos) { }
    public EntityToPositionDistanceLessConstraint(string id1, float distance, Vector3 pos) : base(id1, distance, "le(", pos) { }
    public EntityToPositionDistanceLessConstraint(Type id1, float distance, Vector3 pos) : base(id1, distance, "le(", pos) { }
}
public class EntityToPositionDistanceLessStrictConstraint : EntityToPositionDistanceComparisonConstraint
{
    public EntityToPositionDistanceLessStrictConstraint(Type id1, int distance, Vector3Int pos) : base(id1, distance, "lt(", pos) { }
    public EntityToPositionDistanceLessStrictConstraint(string id1, int distance, Vector3Int pos) : base(id1, distance, "lt(", pos) { }
    public EntityToPositionDistanceLessStrictConstraint(string id1, float distance, Vector3Int pos) : base(id1, distance, "lt(", pos) { }
    public EntityToPositionDistanceLessStrictConstraint(Type id1, float distance, Vector3Int pos) : base(id1, distance, "lt(", pos) { }
    public EntityToPositionDistanceLessStrictConstraint(Type id1, int distance, Vector3 pos) : base(id1, distance, "lt(", pos) { }
    public EntityToPositionDistanceLessStrictConstraint(string id1, int distance, Vector3 pos) : base(id1, distance, "lt(", pos) { }
    public EntityToPositionDistanceLessStrictConstraint(string id1, float distance, Vector3 pos) : base(id1, distance, "lt(", pos) { }
    public EntityToPositionDistanceLessStrictConstraint(Type id1, float distance, Vector3 pos) : base(id1, distance, "lt(", pos) { }
}
public class EntityToPositionDistanceGreaterConstraint : EntityToPositionDistanceComparisonConstraint
{
    public EntityToPositionDistanceGreaterConstraint(Type id1, int distance, Vector3Int pos) : base(id1, distance, "ge(", pos) { }
    public EntityToPositionDistanceGreaterConstraint(string id1, int distance, Vector3Int pos) : base(id1, distance, "ge(", pos) { }
    public EntityToPositionDistanceGreaterConstraint(string id1, float distance, Vector3Int pos) : base(id1, distance, "ge(", pos) { }
    public EntityToPositionDistanceGreaterConstraint(Type id1, float distance, Vector3Int pos) : base(id1, distance, "ge(", pos) { }
    public EntityToPositionDistanceGreaterConstraint(Type id1, int distance, Vector3 pos) : base(id1, distance, "ge(", pos) { }
    public EntityToPositionDistanceGreaterConstraint(string id1, int distance, Vector3 pos) : base(id1, distance, "ge(", pos) { }
    public EntityToPositionDistanceGreaterConstraint(string id1, float distance, Vector3 pos) : base(id1, distance, "ge(", pos) { }
    public EntityToPositionDistanceGreaterConstraint(Type id1, float distance, Vector3 pos) : base(id1, distance, "ge(", pos) { }
}
public class EntityToPositionDistanceGreaterStrictConstraint : EntityToPositionDistanceComparisonConstraint
{
    public EntityToPositionDistanceGreaterStrictConstraint(Type id1, int distance, Vector3Int pos) : base(id1, distance, "gt(", pos) { }
    public EntityToPositionDistanceGreaterStrictConstraint(string id1, int distance, Vector3Int pos) : base(id1, distance, "gt(", pos) { }
    public EntityToPositionDistanceGreaterStrictConstraint(string id1, float distance, Vector3Int pos) : base(id1, distance, "gt(", pos) { }
    public EntityToPositionDistanceGreaterStrictConstraint(Type id1, float distance, Vector3Int pos) : base(id1, distance, "gt(", pos) { }
    public EntityToPositionDistanceGreaterStrictConstraint(Type id1, int distance, Vector3 pos) : base(id1, distance, "gt(", pos) { }
    public EntityToPositionDistanceGreaterStrictConstraint(string id1, int distance, Vector3 pos) : base(id1, distance, "gt(", pos) { }
    public EntityToPositionDistanceGreaterStrictConstraint(string id1, float distance, Vector3 pos) : base(id1, distance, "gt(", pos) { }
    public EntityToPositionDistanceGreaterStrictConstraint(Type id1, float distance, Vector3 pos) : base(id1, distance, "gt(", pos) { }
}

#endregion

#endregion