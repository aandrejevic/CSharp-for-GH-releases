private void RunScript(object VL, List<string> Keys, List<string> Values)
  {

    if(VL == null)
      this.Component.AddRuntimeMessage(GH_RuntimeMessageLevel.Warning, "VL == null");
    if(Keys.Count != Values.Count)
      this.Component.AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "Keys.Count != Values.Count");

    foreach(IGH_Param source in this.Component.Params.Input[0].Sources){
      if(source is Grasshopper.Kernel.Special.GH_ValueList ){
        Grasshopper.Kernel.Special.GH_ValueList vl = source as Grasshopper.Kernel.Special.GH_ValueList;
        vl.ListItems.Clear();
        for(int i = 0; i < Keys.Count; i++)
          vl.ListItems.Add(new Grasshopper.Kernel.Special.GH_ValueListItem(Keys[i], Values[i]));
        GrasshopperDocument.SolutionEnd += ExpireIt;
        return;
      }
    }

  }

  // <Custom additional code> 

  public void ExpireIt(object sender, EventArgs e){
    GrasshopperDocument.SolutionEnd -= ExpireIt;
    IGH_Param vl = this.Component.Params.Input[0].Sources[0];
    this.Component.Params.Input[0].RemoveAllSources();
    this.Component.ExpireSolution(true);
    vl.ExpireSolution(true);
  }