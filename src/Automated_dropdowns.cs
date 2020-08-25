private void RunScript(List<string> x, int y, ref object params_, ref object results_)
  {

    var models = new List<Model>();

    //parse input text
    for (int i = 0; i < x.Count; i += (valuecount + 2))
    {
      var model = new Model();
      model.Params = FormatParamString(x[i]); //convert CSV to list of doubles
      for (int j = i + 1; j < i + 4; j++)
      {
        var item = new DictItem();
        FormatModelLine(x[j], out item.Name, out item.Value);
        model.Results.Add(item);
      }
      models.Add(model);
    }
    Component.Message = models.Count.ToString() + " models";

    AnalysisTypes = CalcUniqueAnalyses(models);

    //make dropdown box
    if(Component.Params.Input[1].SourceCount == 0 && Component.Params.Input[0].SourceCount > 0)
    {
      var vallist = new Grasshopper.Kernel.Special.GH_ValueList();
      vallist.CreateAttributes();
      vallist.Name = "Analysis types";
      vallist.NickName = "Analysis:";
      vallist.ListMode = Grasshopper.Kernel.Special.GH_ValueListMode.DropDown;

      int inputcount = this.Component.Params.Input[1].SourceCount;
      vallist.Attributes.Pivot = new PointF((float) this.Component.Attributes.DocObject.Attributes.Bounds.Left - vallist.Attributes.Bounds.Width - 30, (float) this.Component.Params.Input[1].Attributes.Bounds.Y + inputcount * 30);

      vallist.ListItems.Clear();

      for(int i = 0; i < AnalysisTypes.Count; i++)
      {
        vallist.ListItems.Add(new Grasshopper.Kernel.Special.GH_ValueListItem(AnalysisTypes[i], i.ToString()));
      }
      vallist.Description = AnalysisTypes.Count.ToString() + " analyses were found in the SBA file.";

      GrasshopperDocument.AddObject(vallist, false);

      this.Component.Params.Input[1].AddSource(vallist);
      vallist.ExpireSolution(true);
    }

    //we now have our results in a nice classy format. let's convert them to a datatree
    var resultsvals = new DataTree<double>();
    var paramvals = new DataTree<double>();
    string astr = AnalysisTypes[y];

    Component.Params.Output[1].VolatileData.Clear(); //bug fix for when new dropdown is made
    Component.Params.Output[2].VolatileData.Clear(); //bug fix for when new dropdown is made

    for (int i = 0; i < models.Count; i++)
    {
      var pth = new GH_Path(i);
      foreach (var result in models[i].Results)
      {
        if(astr == result.Name)
        {
          resultsvals.Add(result.Value, pth);
          foreach(var param in models[i].Params)
          {
            paramvals.Add(param, pth);
          }
        }
      }
    }
    results_ = resultsvals;
    params_ = paramvals;


  }

  // <Custom additional code> 

  int valuecount = 3;
  List<string> AnalysisTypes = new List<string>();

  public class DictItem
  {
    public DictItem()
    {
    }

    public string Name;
    public double Value;
  }

  public class Model
  {
    public Model()
    {
    }

    public List<double> Params = new List<double>();
    public List<DictItem> Results = new List<DictItem>();

    public List<string> ToListString()
    {
      var rtnlist = new List<string>();
      foreach (var result in Results) rtnlist.Add(result.Name + ", " + result.Value.ToString());
      return rtnlist;
    }

  }

  List<double> FormatParamString(string input)
  {
    var rtnlist = new List<double>();


    input = input.Replace("<", "");
    input = input.Replace(">", "");
    input = input.Replace('m', '-');
    input = input.Replace('_', '.');
    string[] splitstring = input.Split('c');
    foreach(string str in splitstring)
    {
      try
      {
        rtnlist.Add(Convert.ToDouble(str));
      }
      catch
      {
        rtnlist.Add(0);
      }
    }
    return rtnlist;
  }

  /// <summary>
  /// Get name and value of analysis from an SBA string
  /// </summary>
  /// <param name="input"></param>
  /// <param name="name"></param>
  /// <param name="val"></param>
  /// <returns></returns>
  void FormatModelLine(string input, out string name, out double val)
  {
    int firstopen = input.IndexOf('<');
    int firstclose = input.IndexOf('>');
    int lastopen = input.LastIndexOf('<');

    name = input.Substring(firstopen + 1, firstclose - firstopen - 1);
    val = Convert.ToDouble(input.Substring(firstclose + 1, lastopen - firstclose - 1));
  }


  /// <summary>
  /// Get analysis types from list of models
  /// </summary>
  /// <param name="models"></param>
  /// <returns></returns>
  List<string> CalcUniqueAnalyses(List < Model > models)
  {
    List<string> rtnlist = new List<string>();
    foreach (var model in models)
    {
      foreach (var result in model.Results)
      {
        bool found = false;
        foreach (var calc in rtnlist)
        {
          if(calc == result.Name) found = true;
        }
        if(!found) rtnlist.Add(result.Name);
      }
    }
    return rtnlist;
  }