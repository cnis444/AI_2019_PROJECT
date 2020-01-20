using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using System.Text;


public class Solver
{
    public List<ConstraintObject> objects { get; set; }

    public Solver(List<ConstraintObject> objects)
    {
        this.objects = objects;
    }
       
    private string Prefix(string file)
    {
        string path = Application.dataPath;
        return Path.Combine(path, "Scripts", "Constraints", file);
    }

    private void SolveConstraints(string constraints)
    {
        
        System.IO.File.WriteAllText(Prefix("Scenario.xml"), constraints);
        System.Diagnostics.Process process = new System.Diagnostics.Process();
        System.Diagnostics.ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo();
        startInfo.FileName = "cmd.exe";
        startInfo.Arguments = "java -cp " + Prefix("AbsCon19-06.jar") + " AbsCon " + Prefix("Scenario.xml") + " -cm | findstr /b [v] > " + Prefix("Solution.xml");
        startInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
        process.StartInfo = startInfo;
        process.Start();
    }

    private string BuildEntityConstraints(List<Constraint> guide)
    {
        StringBuilder constraints = new StringBuilder();
        constraints.Append("< instance format = \"XCSP3\" type = \"CSP\" >< variables >");
        objects.ForEach(e => { constraints.Append("<var id=\"" + e.GetType().ToString() + "\" type = \"integer\"> 0.." + e.maxEntity.ToString() + " </var>"); });
        constraints.Append("</variables>< constraints >");

        guide.ForEach(e => { if (e is EntityConstraint) constraints.Append((e as EntityConstraint).ConstraintToString(objects)); });
        
        for (int i = 0; i< objects.Count; i++)
        {
            for (int y = 0; y < objects[i].Constraints.Count; y++)
            {
                if (objects[i].Constraints[y] is EntityConstraint)
                    constraints.Append((objects[i].Constraints[y] as EntityConstraint).ConstraintToString(objects));
            }        
        }
    
        constraints.Append("</constraints></ instance >");
        return constraints.ToString();
    }

    private string BuildPositionConstraints(List<Constraint> guide, Dictionary<string, int> arraySize)
    {
        List<ConstraintObject> concernedObjects = new List<ConstraintObject>();
        objects.ForEach(e => { if (arraySize.ContainsKey(e.GetType().ToString())) concernedObjects.Add(e); });

        StringBuilder constraints = new StringBuilder();
        constraints.Append("< instance format = \"XCSP3\" type = \"CSP\" >< variables >");
        foreach (KeyValuePair<string, int> entry in arraySize)
        {
            constraints.Append("<array id=\"" + entry.Key + "\" size = \"[" + entry.Value.ToString() + "][3]\"> 0..1000 </array>");
        }
        constraints.Append("</variables>< constraints >");
        guide.ForEach(e => { if (e is PositionConstraint) constraints.Append((e as PositionConstraint).ConstraintToString(concernedObjects, arraySize)); });
        constraints.Append("</constraints></ instance >");
        return constraints.ToString();
    }

    private List<String[]> ParseSolution()
    {
        try
        {
            String text = File.ReadAllText(Prefix("Solution.xml"));
            String[] separator = { "v <instantiation id='sol1' type='solution'>  <list> ", "</list>  <values> ", "</values>  </instantiation>" };
            String[] sol = text.Split(separator, 2, StringSplitOptions.RemoveEmptyEntries);
            separator = new String[] {" ", "[]"};
            String[] ids = sol[0].Split(separator, StringSplitOptions.RemoveEmptyEntries);
            String[] vals = sol[1].Split(separator, StringSplitOptions.RemoveEmptyEntries);
            return (new List<String[]>() { ids, vals });
        }
        catch (System.IndexOutOfRangeException)
        {
            throw (new NoSolutionException());
        }
    }


    public List<Solutions> SolvePosition(List<Constraint> guide, Dictionary<string, int> arraySize)
    {
        List<Solutions> res = new List<Solutions>();
        string entityConstraints = BuildPositionConstraints(guide, arraySize);
        SolveConstraints(entityConstraints);
        List<String[]> sol = ParseSolution();
        int pos = 0;
        ConstraintObject current = objects[0];
        foreach (string entry in sol[0])
        {
            foreach (ConstraintObject e in objects){
                if (e.GetType().ToString() == entry)
                {
                    current = e;
                    break;
                }
            }
            for (int i = 0; i < arraySize[entry]; i++)
            {
                res.Add(new Solutions(current, float.Parse(sol[1][pos]) / 10, float.Parse(sol[1][pos+1]) / 10, float.Parse(sol[1][pos+2]) / 10 ));
                pos += 3;
            }
        }
        return res;
    }


    public List<Solutions> SolveEntity(List<Constraint> guide)
    {
        string entityConstraints = BuildEntityConstraints(guide);
        SolveConstraints(entityConstraints);
        List<String[]> solString = ParseSolution();
        Dictionary<string, int> sol = new Dictionary<string, int>();
        
        for (int i = 0; i < solString[0].Length; i++)
            sol.Add(solString[0][i], int.Parse(solString[1][i]));

        try
        {
            return SolvePosition(guide, sol);
        }
        catch (NoSolutionException)
        {
            guide.Add(new ForbiddingEntitySolutionConstraint(solString[0], solString[1]));
            return SolveEntity(guide);
        }
    }

    public List<Solutions> Solve(List<Constraint> guide)
    {
            return SolveEntity(guide);
    }
    
}
