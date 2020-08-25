private void RunScript(List<int> x, bool reset)
  {
    GH_DocumentIO parentIO = new GH_DocumentIO(GrasshopperDocument);
    GH_DocumentIO childIO = new GH_DocumentIO();


    // clear all lists
    if (reset)
    {
      copiedNums.Clear();
      origItems.Clear();
      copiedClusters.Clear();
      expandedGuids.Clear();
    }


    // Just a precaution to always only have the 3 original items. May not be necessary
    //if (origItems.Count > 3)
    //origItems.Clear();

    // populate original items list
    if (origItems.Count == 0)
    {
      foreach (IGH_ActiveObject thisObject in GrasshopperDocument.ActiveObjects())
      {
        if(thisObject.GetType().ToString() == "Grasshopper.Kernel.Special.GH_Cluster")
        {
          origItems.Add(thisObject.InstanceGuid);
        }
        else if (thisObject.GetType().ToString() == "Human.GH_ValueList")
        {
          origItems.Add(thisObject.InstanceGuid);
        }
      }
    }

    // If field selector number has not already been selected then copy & paste
    foreach (int nr in x)
    {
      if (!copiedNums.Contains(nr) && nr != 0)
      {
        List<Guid> newGuid = CopyPaste(nr, origItems, parentIO, childIO);
        copiedNums.Add(nr);
        copiedClusters.Add(newGuid);
        foreach (Guid ID in newGuid)
          expandedGuids.Add(ID);
      }
    }

    // delete clusters and item selector
    for (int i = 0; i < copiedNums.Count; i++)
    {
      if (!x.Contains(copiedNums[i]))
      {
        foreach (Guid ID in copiedClusters[i])
        {
          expandedGuids.Remove(ID);
        }
        copiedClusters.RemoveAt(i);
        copiedNums.RemoveAt(i);

        foreach (IGH_ActiveObject thisObject in GrasshopperDocument.ActiveObjects())
        {
          if(thisObject.GetType().ToString() == "Grasshopper.Kernel.Special.GH_Cluster")
          {
            if (!origItems.Contains(thisObject.InstanceGuid) && !expandedGuids.Contains(thisObject.InstanceGuid))
              GrasshopperDocument.RemoveObject(thisObject, true);
          }
          else if (thisObject.GetType().ToString() == "Human.GH_ValueList")
          {
            if (!origItems.Contains(thisObject.InstanceGuid) && !expandedGuids.Contains(thisObject.InstanceGuid))
              GrasshopperDocument.RemoveObject(thisObject, true);
          }
        }
      }
    }

    //A = origItems;
    //B = copiedNums;
    //C = copiedClusters;
  }

  // <Custom additional code> 
  public List<Guid> CopyPaste(int num, List<Guid> originalClusters, GH_DocumentIO parent, GH_DocumentIO child)
  {
    List<Guid> copiedGuid = new List<Guid>();
    GrasshopperDocument.ScheduleSolution(5);
    parent.Copy(GH_ClipboardType.Global, originalClusters);
    child.Paste(GH_ClipboardType.Global);
    parent.ClearClipboard(GH_ClipboardType.Global);

    child.Document.SelectAll();
    child.Document.TranslateObjects(new Size(0, num * 200), true);
    child.Document.ExpireSolution();
    child.Document.MutateAllIds();
    child.Document.DeselectAll();

    foreach (IGH_ActiveObject thisObject in child.Document.ActiveObjects())
    {
      if(thisObject.GetType().ToString() == "Grasshopper.Kernel.Special.GH_Cluster")
      {
        copiedGuid.Add(thisObject.InstanceGuid);
      }
      else if (thisObject.GetType().ToString() == "Human.GH_ValueList")
      {
        copiedGuid.Add(thisObject.InstanceGuid);
      }
    }

    GrasshopperDocument.DeselectAll();
    GrasshopperDocument.MergeDocument(child.Document);

    return copiedGuid;
  }

  List<Guid> origItems = new List<Guid>();
  List<List<Guid>> copiedClusters = new List<List<Guid>>();
  List<int> copiedNums = new List<int>();
  List<Guid> expandedGuids = new List<Guid>();