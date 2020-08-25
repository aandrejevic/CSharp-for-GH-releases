  private void RunScript(bool enable, List<Mesh> meshList, double reducePercentage, ref object reducedMesh)
  {
    if(!enable)return;
    meshList = meshList.Where(x => x != null).ToList();
    if(!meshList.Any())return;


    List<Mesh> meshListReduced = new List<Mesh> ();

    for(int i = 0; i < meshList.Count;i++){
      Mesh mesh = meshList[i];
      Guid meshObj = doc.Objects.AddMesh(mesh);
      doc.Objects.UnselectAll();
      doc.Objects.Select(meshObj);
      Rhino.RhinoApp.RunScript(string.Format("_-ReduceMesh _ReductionPercentage {0} _Enter", reducePercentage.ToString()), false);
      Rhino.DocObjects.RhinoObject MObj = doc.Objects.GetSelectedObjects(false, false).First();
      doc.Objects.Delete(meshObj, true);
      meshListReduced.Add((Mesh) MObj.Geometry);
    }

    reducedMesh = meshListReduced;

  }